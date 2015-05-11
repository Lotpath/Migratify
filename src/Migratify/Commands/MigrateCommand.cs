using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Migratify.Database;
using Migratify.Dialects;
using Migratify.Migrations;
using Migratify.Options;

namespace Migratify.Commands
{
    public class MigrateCommand : ICanHandleOption
    {
        private readonly ILogger _logger;
        private readonly ISqlDialect _dialect;
        private readonly IConnectionFactory _connectionFactory;
        private readonly MigrationConfiguration _config;
        private readonly IMigrationFactory _migrationFactory;

        public MigrateCommand(ILogger logger, ISqlDialect dialect,
            IConnectionFactory connectionFactory,
            MigrationConfiguration config,
            IMigrationFactory migrationFactory)
        {
            _logger = logger;
            _dialect = dialect;
            _connectionFactory = connectionFactory;
            _config = config;
            _migrationFactory = migrationFactory;
        }

        public bool CanHandle(IOption option)
        {
            return option is MigrateOption;
        }

        public void Execute()
        {
            var currentVersion = CurrentVersion;
            var migrations = _migrationFactory.BuildMigrations()
                .Where(x => x.Version > currentVersion)
                .ToList();

            if (currentVersion < 0 || !migrations.Any())
            {
                return;
            }

            var dependencies = new Dictionary<Type, object>();

            using (var connection = _connectionFactory.OpenTarget())
            using (var transaction = connection.BeginTransaction())
            {
                dependencies.Add(typeof(IDbConnection), connection);
                dependencies.Add(typeof(IDbTransaction), transaction);

                foreach (var migration in migrations)
                {
                    var type = migration.Type;

                    var constructor = type.GetConstructors()
                        .GroupBy(x => x)
                        .Select(x => new { Constructor = x.Key, ParameterCount = x.Sum(y => y.GetParameters().Count()) })
                        .OrderByDescending(x => x.ParameterCount)
                        .First().Constructor;

                    var parameters = new List<object>();
                    var parameterInfos = constructor.GetParameters();

                    foreach (var parameterInfo in parameterInfos)
                    {
                        parameters.Add(dependencies[parameterInfo.ParameterType]);
                    }

                    dynamic source = constructor.Invoke(parameters.ToArray());

                    var scripts = ((IEnumerable<string>)(source).Up()).ToList();

                    _logger.Log("Executing migration {0} - {1}", migration.Version, migration.Name);

                    foreach (var script in scripts)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = script;
                            command.ExecuteNonQuery();
                            _logger.Log("Executed script:\r\n" + script);
                        }
                    }

                    var versionCommand = connection.CreateCommand(_dialect.InsertSchemaVersion, migration.Version);
                    versionCommand.Transaction = transaction;
                    versionCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }

            _dialect.ClearAllPools();
        }

        private bool CannotRunMigrations()
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                if (!DatabaseExists(masterConnection, _config.TargetDatabaseName))
                {
                    _logger.Log("Database {0} does not exist.  You must initialize the database.", _config.TargetDatabaseName);
                    return true;
                }
            }
            return false;
        }

        private long CurrentVersion
        {
            get
            {
                if (CannotRunMigrations()) return -1;

                try
                {
                    using (var targetConnection = _connectionFactory.OpenTarget())
                    {
                        return GetCurrentVersion(targetConnection);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(ex.Message);
                    return -1;
                }
            }
        }

        private long GetCurrentVersion(IDbConnection connection, IDbTransaction transaction = null)
        {
            using (var command = connection.CreateCommand(_dialect.CurrentVersion))
            {
                if (transaction != null) command.Transaction = transaction;
                return (long)command.ExecuteScalar();
            }
        }

        private bool DatabaseExists(IDbConnection connection, string databaseName)
        {
            using (var command = connection.CreateCommand(_dialect.DatabaseExists, databaseName))
            {
                return (bool)command.ExecuteScalar();
            }
        }
    }
}
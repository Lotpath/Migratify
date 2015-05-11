using System.Data;
using Migratify.Database;
using Migratify.Dialects;
using Migratify.Options;

namespace Migratify.Commands
{
    public class CreateCommand : ICanHandleOption
    {
        private readonly ILogger _logger;
        private readonly ISqlDialect _dialect;
        private readonly IConnectionFactory _connectionFactory;
        private readonly MigrationConfiguration _config;

        public CreateCommand(ILogger logger, ISqlDialect dialect, 
            IConnectionFactory connectionFactory,
            MigrationConfiguration config)
        {
            _logger = logger;
            _dialect = dialect;
            _connectionFactory = connectionFactory;
            _config = config;
        }

        public bool CanHandle(IOption option)
        {
            return option is CreateOption;
        }

        public void Execute()
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                EnsureTargetDatabase(masterConnection);
            }

            _dialect.ClearAllPools();
        }

        private void EnsureTargetDatabase(IDbConnection connection)
        {
            var databaseName = _config.TargetDatabaseName;

            if (DatabaseExists(connection, databaseName))
            {
                _logger.Log("Database {0} already exists.", databaseName);
                return;
            }

            using (var createCommand = connection.CreateCommand(string.Format(_dialect.CreateDatabase, databaseName)))
            {
                createCommand.ExecuteNonQuery();
                _logger.Log("Database {0} created.", databaseName);
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
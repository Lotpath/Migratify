using System.Data;
using Migratify.Database;
using Migratify.Dialects;
using Migratify.Options;

namespace Migratify.Commands
{
    public class DropCommand : ICanHandleOption
    {
        private readonly ILogger _logger;
        private readonly ISqlDialect _dialect;
        private readonly IConnectionFactory _connectionFactory;
        private readonly MigrationConfiguration _config;

        public DropCommand(ILogger logger, ISqlDialect dialect,
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
            return option is DropOption;
        }

        public void Execute()
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                DropTargetDatabase(masterConnection);
            }

            _dialect.ClearAllPools();
        }


        private void DropTargetDatabase(IDbConnection connection)
        {
            var databaseName = _config.TargetDatabaseName;

            if (!DatabaseExists(connection, databaseName))
            {
                _logger.Log("Database {0} already dropped.", databaseName);
                return;
            }

            using (var disconnectCommand = connection.CreateCommand(string.Format(_dialect.DropAllConnections, databaseName)))
            {
                disconnectCommand.ExecuteNonQuery();
                _logger.Log("Database connections dropped for {0}.", databaseName);
            }

            using (var dropCommand = connection.CreateCommand(string.Format(_dialect.DropDatabase, databaseName)))
            {
                dropCommand.ExecuteNonQuery();
                _logger.Log("Database {0} dropped.", databaseName);
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
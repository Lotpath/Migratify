using System.Data;
using Migratify.Database;
using Migratify.Dialects;
using Migratify.Options;

namespace Migratify.Commands
{
    public class InitCommand : ICanHandleOption
    {
        private readonly ILogger _logger;
        private readonly ISqlDialect _dialect;
        private readonly IConnectionFactory _connectionFactory;

        public InitCommand(ILogger logger, ISqlDialect dialect, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _dialect = dialect;
            _connectionFactory = connectionFactory;
        }

        public bool CanHandle(IOption option)
        {
            return option is InitOption;
        }

        public void Execute()
        {
            using (var targetConnection = _connectionFactory.OpenTarget())
            {
                EnsureSchemaVersionTable(targetConnection);
            }
        }

        private void EnsureSchemaVersionTable(IDbConnection targetConnection)
        {
            if (SchemaVersionTableExists(targetConnection))
            {
                _logger.Log("Schema Version Table already exists.");
                return;
            }

            using (var createCommand = targetConnection.CreateCommand(_dialect.CreateSchemaVersionTable))
            {
                createCommand.ExecuteNonQuery();
                _logger.Log("Schema Version Table created.");
            }
        }

        private bool SchemaVersionTableExists(IDbConnection connection)
        {
            using (var command = connection.CreateCommand(_dialect.SchemaVersionTableExists))
            {
                return (bool)command.ExecuteScalar();
            }
        }

    }
}
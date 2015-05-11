using System.Data;
using Migratify.Database;
using Migratify.Dialects;
using Migratify.Options;

namespace Migratify.Commands
{
    public class TearDownCommand : ICanHandleOption
    {
        private readonly ILogger _logger;
        private readonly ISqlDialect _dialect;
        private readonly IConnectionFactory _connectionFactory;
        private readonly MigrationConfiguration _config;

        public TearDownCommand(ILogger logger, ISqlDialect dialect,
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
            return option is TearDownOption;
        }

        public void Execute()
        {
            var databaseName = _config.TargetDatabaseName;

            using (var targetConnection = _connectionFactory.OpenTarget())
            using (var transaction = targetConnection.BeginTransaction())
            {
                using (var command = targetConnection.CreateCommand(_dialect.TearDown))
                {
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    _logger.Log("Database {0} torn down.", databaseName);
                }

                transaction.Commit();
            }

            _dialect.ClearAllPools();
        }
    }
}
using Migratify.Database;
using Migratify.Dialects;
using Migratify.Options;

namespace Migratify.Commands
{
    public class CurrentVersionCommand : ICanHandleOption
    {
        private readonly ILogger _logger;
        private readonly ISqlDialect _dialect;
        private readonly IConnectionFactory _connectionFactory;
        private readonly MigrationConfiguration _config;

        public CurrentVersionCommand(ILogger logger, ISqlDialect dialect, 
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
            return option is CurrentVersionOption;
        }

        public void Execute()
        {
            using (var connection = _connectionFactory.OpenTarget())
            using (var command = connection.CreateCommand(_dialect.CurrentVersion))
            {
                _logger.Log("Current version is {0}", (long)command.ExecuteScalar());
            }
        }
    }
}
using System.Data;
using System.Data.Common;

namespace Migratify.Database
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _masterConnectionString;
        private readonly string _targetConnectionString;

        public ConnectionFactory(MigrationConfiguration configuration)
        {
            _dbProviderFactory = configuration.DbProviderFactory;
            _masterConnectionString = configuration.MasterConnectionString;
            _targetConnectionString = configuration.TargetConnectionString;
        }

        public IDbConnection OpenMaster()
        {
            if (string.IsNullOrEmpty(_masterConnectionString))
            {
                throw new MigratifyException("Master connection string required for this operation");
            }
            var connection = _dbProviderFactory.CreateConnection();
            if (connection == null)
            {
                throw new MigratifyException("Failed to create a Master connection using the specified DbProviderFactory: " + _dbProviderFactory.GetType().Name);
            }
            connection.ConnectionString = _masterConnectionString;
            connection.Open();
            return connection;
        }

        public IDbConnection OpenTarget()
        {
            if (string.IsNullOrEmpty(_targetConnectionString))
            {
                throw new MigratifyException("Target connection string required for this operation");
            }
            var connection = _dbProviderFactory.CreateConnection();
            if (connection == null)
            {
                throw new MigratifyException("Failed to create a Target connection using the specified DbProviderFactory: " + _dbProviderFactory.GetType().Name);
            }
            connection.ConnectionString = _targetConnectionString;
            connection.Open();
            return connection;
        }
    }
}
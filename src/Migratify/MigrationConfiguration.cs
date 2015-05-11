using System.Configuration;
using System.Data.Common;
using Migratify.Dialects;

namespace Migratify
{
    public class MigrationConfiguration : IMigrationConfiguration
    {
        public MigrationConfiguration()
        {
            var provider = ConfigurationManager.AppSettings["ProviderInvariantName"];
            var master = ConfigurationManager.ConnectionStrings["Master"];
            var target = ConfigurationManager.ConnectionStrings["Target"];

            ProviderName = provider
                           ?? (target == null ? null : target.ProviderName)
                           ?? "System.Data.SqlClient";
            MasterConnectionString = (master == null ? null : master.ConnectionString);
            TargetConnectionString = (target == null ? null : target.ConnectionString);

            DbProviderFactory = DbProviderFactories.GetFactory(ProviderName);
            TargetDatabaseName = SqlDialectResolver.GetDialect(ProviderName).ExtractDatabaseName(TargetConnectionString);
        }

        public string ProviderName { get; private set; }
        public string MasterConnectionString { get; private set; }
        public string TargetConnectionString { get; private set; }
        public string TargetDatabaseName { get; private set; }
        public DbProviderFactory DbProviderFactory { get; private set; }
    }
}
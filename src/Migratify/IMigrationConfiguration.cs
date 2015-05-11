using System.Data.Common;

namespace Migratify
{
    public interface IMigrationConfiguration
    {
        string ProviderName { get; }
        string MasterConnectionString { get; }
        string TargetConnectionString { get; }
        string TargetDatabaseName { get; }
        DbProviderFactory DbProviderFactory { get; }
    }
}
using System.Configuration;
using Xunit;

namespace Migratify.Tests
{
    public class MigrationConfigurationTests
    {
        [Fact]
        public void can_load_default_configuration()
        {
            var configuration = new MigrationConfiguration();

            var provider = ConfigurationManager.AppSettings["ProviderInvariantName"];
            var master = ConfigurationManager.ConnectionStrings["Master"];
            var target = ConfigurationManager.ConnectionStrings["Target"];

            Assert.NotNull(provider);
            Assert.NotNull(master);
            Assert.NotNull(target);
            Assert.Equal(provider, configuration.ProviderName);
            Assert.Equal(master.ConnectionString, configuration.MasterConnectionString);
            Assert.Equal(target.ConnectionString, configuration.TargetConnectionString);
        }
    }
}
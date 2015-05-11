using System.Linq;
using Migratify.Migrations;
using Migratify.Tests.Migrations;
using Xunit;

namespace Migratify.Tests
{
    public class MigrationFactoryTests
    {
        private MigrationFactory _factory;

        public MigrationFactoryTests()
        {
            _factory = new MigrationFactory();
        }

        [Fact]
        public void can_build_migrations()
        {
            var migrations = _factory.BuildMigrations().ToList();
            
            Assert.Equal(typeof(FirstMigration), migrations[0].Type);
            Assert.Equal(typeof(SecondMigration), migrations[1].Type);
            Assert.Equal(typeof(FourthMigrationSkippingThird), migrations[2].Type);
            Assert.Equal(typeof(FifthMigrationRequiringConnectionInCtor), migrations[3].Type);
            Assert.Equal(typeof(SixthMigrationRequiringConnectionAndTransactionInCtor), migrations[4].Type);
        }
    }
}
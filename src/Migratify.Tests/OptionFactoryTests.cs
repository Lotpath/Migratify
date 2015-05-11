using System;
using System.Linq;
using Migratify.Options;
using Xunit;
using Xunit.Extensions;

namespace Migratify.Tests
{
    public class OptionFactoryTests
    {
        [Theory]
        [InlineData("--help", typeof(HelpOption))]
        [InlineData("--create", typeof(CreateOption))]
        [InlineData("--init", typeof(InitOption))]
        [InlineData("--drop", typeof(DropOption))]
        [InlineData("--tear-down", typeof(TearDownOption))]
        [InlineData("--migrate", typeof(MigrateOption))]
        [InlineData("--current-version", typeof(CurrentVersionOption))]
        public void can_parse_option(string arg, Type optionType)
        {
            // arrange
            var argSource = new FakeArgumentSource(new[] { arg });

            var factory = new OptionFactory(new OptionSource(), argSource);

            // act
            var options = factory.BuildOptions();

            // assert
            Assert.True(options.Any(x => x.GetType() == optionType));
        }
    }
}

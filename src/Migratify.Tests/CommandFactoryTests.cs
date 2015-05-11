using System.Linq;
using Migratify.Commands;
using Xunit;

namespace Migratify.Tests
{
    public class CommandFactoryTests
    {
        private readonly CommandFactory _factory;

        public CommandFactoryTests()
        {
            var container = new Bootstrapper().Bootstrap(cfg =>
                {
                    cfg.Register<IArgumentSource>(new FakeArgumentSource(new[]{ "--help" }));
                });
            _factory = container.GetInstance<CommandFactory>();
        }

        [Fact]
        public void can_handle_help_option()
        {
            var commands = _factory.BuildCommands();
            Assert.True(commands.Any(x => x is HelpCommand));
        }
    }
}
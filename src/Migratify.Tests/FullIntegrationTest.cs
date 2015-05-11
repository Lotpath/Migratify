using Migratify.Commands;
using Xunit;

namespace Migratify.Tests
{
    public class FullIntegrationTest
    {
        private ICommandRunner _runner;

        public FullIntegrationTest()
        {
            var container = new Bootstrapper().Bootstrap(cfg =>
            {
                cfg.Register<IArgumentSource>(
                    new FakeArgumentSource(new[]
                        {
                            "--create",
                            "--init",
                            "--current-version",
                            "--migrate",
                            "--current-version",
                            "--tear-down",
                            "--drop",
                        }));
            });
            _runner = container.GetInstance<CommandRunner>();
        }

        [Fact]
        public void can_run_full_integration_test()
        {
            _runner.Run();
        }
    }
}
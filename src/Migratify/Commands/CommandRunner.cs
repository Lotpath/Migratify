
namespace Migratify.Commands
{
    public class CommandRunner : ICommandRunner
    {
        private readonly ICommandFactory _commandFactory;

        public CommandRunner(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public void Run()
        {
            foreach (var command in _commandFactory.BuildCommands())
            {
                command.Execute();
            }
        }
    }
}
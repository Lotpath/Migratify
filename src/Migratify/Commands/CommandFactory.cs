using System.Collections.Generic;
using System.Linq;
using Migratify.Options;

namespace Migratify.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IOptionFactory _optionFactory;
        private readonly IContainer _container;

        public CommandFactory(IOptionFactory optionFactory, IContainer container)
        {
            _optionFactory = optionFactory;
            _container = container;
        }

        public IEnumerable<ICommand> BuildCommands()
        {
            var commands = typeof (ICanHandleOption)
                .Assembly
                .GetTypes()
                .Where(x => !x.IsInterface && (typeof (ICanHandleOption)).IsAssignableFrom(x))
                .Select(x => (ICanHandleOption) _container.GetInstance(x));

            return _optionFactory
                .BuildOptions()
                .SelectMany(option =>
                            commands
                                .Where(command =>
                                       command.CanHandle(option)));
        }
    }
}
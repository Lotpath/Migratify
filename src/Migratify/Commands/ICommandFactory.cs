using System.Collections.Generic;

namespace Migratify.Commands
{
    public interface ICommandFactory
    {
        IEnumerable<ICommand> BuildCommands();
    }
}
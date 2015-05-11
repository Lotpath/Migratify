using Migratify.Options;

namespace Migratify.Commands
{
    public interface ICanHandleOption : ICommand
    {
        bool CanHandle(IOption option);
    }
}
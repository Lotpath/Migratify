using System.Linq;
using System.Text;
using Migratify.Options;

namespace Migratify.Commands
{
    public class HelpCommand : ICanHandleOption
    {
        private readonly IOptionSource _optionSource;
        private readonly ILogger _logger;

        public HelpCommand(IOptionSource optionSource, ILogger logger)
        {
            _optionSource = optionSource;
            _logger = logger;
        }

        public bool CanHandle(IOption option)
        {
            return option is HelpOption;
        }

        public void Execute()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Migratify Help");

            foreach (var option in _optionSource
                .GetOptions()
                .Select(x => x.ToString())
                .OrderBy(x => x))
            {
                sb.AppendLine(option);
            }

            _logger.Log(sb.ToString());
        }
    }
}
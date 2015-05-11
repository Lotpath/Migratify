using System.Collections.Generic;
using System.Linq;
using Migratify.Commands;

namespace Migratify.Options
{
    public class OptionFactory : IOptionFactory
    {
        private readonly IOptionSource _optionSource;
        private readonly IArgumentSource _argumentSource;

        public OptionFactory(IOptionSource optionSource, IArgumentSource argumentSource)
        {
            _optionSource = optionSource;
            _argumentSource = argumentSource;
        }

        public IEnumerable<IOption> BuildOptions()
        {
            var arguments = _argumentSource.GetArguments();

            var options = new List<IOption>();

            foreach (var argument in arguments)
            {
                options.AddRange(_optionSource
                    .GetOptions()
                    .Where(o => o.IsMatch(argument)));
            }

            return options;
        }
    }
}
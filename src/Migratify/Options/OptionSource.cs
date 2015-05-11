using System;
using System.Collections.Generic;
using System.Linq;

namespace Migratify.Options
{
    public class OptionSource : IOptionSource
    {
        public IEnumerable<IOption> GetOptions()
        {
            return typeof (IOption)
                .Assembly
                .GetTypes()
                .Where(x => !x.IsInterface && (typeof (IOption)).IsAssignableFrom(x))
                .Select(x => (IOption) Activator.CreateInstance(x));
        }
    }
}
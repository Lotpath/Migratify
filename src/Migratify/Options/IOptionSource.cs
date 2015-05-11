using System.Collections.Generic;

namespace Migratify.Options
{
    public interface IOptionSource
    {
        IEnumerable<IOption> GetOptions();
    }
}
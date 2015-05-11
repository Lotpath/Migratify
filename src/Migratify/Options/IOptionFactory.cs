using System.Collections.Generic;

namespace Migratify.Options
{
    public interface IOptionFactory
    {
        IEnumerable<IOption> BuildOptions();
    }
}
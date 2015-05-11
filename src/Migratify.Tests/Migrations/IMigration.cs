using System.Collections.Generic;

namespace Migratify.Tests.Migrations
{
    public interface IMigration
    {
        IEnumerable<string> Up();
    }
}

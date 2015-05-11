using System.Collections.Generic;

namespace Migratify.Tests.Migrations
{
    [Migration(1)]
    public class FirstMigration : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield return "select 1 as one";
        }
    }
}
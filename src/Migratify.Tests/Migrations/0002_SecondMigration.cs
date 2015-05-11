using System.Collections.Generic;

namespace Migratify.Tests.Migrations
{
    [Migration(2)]
    public class SecondMigration : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield return "select 2 as two";
        }
    }
}
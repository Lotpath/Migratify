using System.Collections.Generic;

namespace Migratify.Tests.Migrations
{
    [Migration(4)]
    public class FourthMigrationSkippingThird : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield return "select 4 as four";
        }
    }
}
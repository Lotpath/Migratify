using System.Collections.Generic;
using System.Data;

namespace Migratify.Tests.Migrations
{
    [Migration(5)]
    public class FifthMigrationRequiringConnectionInCtor : IMigration
    {
        public IDbConnection Connection { get; private set; }

        public FifthMigrationRequiringConnectionInCtor(IDbConnection connection)
        {
            Connection = connection;
        }

        public IEnumerable<string> Up()
        {
            yield return "select 5 as five";
        }
    }
}
using System.Collections.Generic;
using System.Data;

namespace Migratify.Tests.Migrations
{
    [Migration(6)]
    public class SixthMigrationRequiringConnectionAndTransactionInCtor : IMigration
    {
        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }

        public SixthMigrationRequiringConnectionAndTransactionInCtor(IDbConnection connection, IDbTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public IEnumerable<string> Up()
        {
            yield return "select 6 as six";
        }
    }
}
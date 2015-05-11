using System.Collections.Generic;

namespace Migratify.Migrations
{
    public interface IMigrationFactory
    {
        IEnumerable<Migration> BuildMigrations();
    }
}
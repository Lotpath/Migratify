using System;

namespace Migratify.Migrations
{
    public class Migration
    {
        public Migration(long version, Type type)
        {
            Version = version;
            Name = type.Name;
            Type = type;
        }

        public long Version { get; private set; }
        public string Name { get; private set; }
        public Type Type { get; private set; }
    }

    public class MigrationContext
    {
        
    }
}

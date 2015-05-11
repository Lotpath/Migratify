using System;

namespace Migratify.Tests.Migrations
{
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(long version)
        {
            Version = version;
        }

        public long Version { get; private set; }
    }
}
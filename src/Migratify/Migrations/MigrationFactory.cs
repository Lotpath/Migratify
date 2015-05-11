using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Migratify.Migrations
{
    public class MigrationFactory : IMigrationFactory
    {
        public IEnumerable<Migration> BuildMigrations()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.exe").Select(x => new FileInfo(x))
                .Union(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(x => new FileInfo(x)))
                .Select(x => Assembly.LoadFrom(x.FullName))
                .ToList();

            var scannedTypes = assemblies.SelectMany(x => x.GetExportedTypes()).ToList();

            var preparedTypes = PrepareMigrationTypes(scannedTypes);

            return preparedTypes.Select(type => new Migration(type.Key, type.Value));
        }

        private IDictionary<long, Type> PrepareMigrationTypes(IEnumerable<Type> types)
        {
            var migrations = new Dictionary<long, Type>();

            var migrationAttributeType = types.SingleOrDefault(x => x.Name == "MigrationAttribute");
            var migrationInterfaceType = types.SingleOrDefault(x => x.Name == "IMigration");

            var migrationTypes = types.Where(x => x.GetInterfaces().Contains(migrationInterfaceType));

            foreach (var type in migrationTypes)
            {
                dynamic attribute = type.GetCustomAttributes(migrationAttributeType, true).SingleOrDefault();
                long version = attribute.Version;

                migrations.Add(version, type);
            }

            return migrations.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
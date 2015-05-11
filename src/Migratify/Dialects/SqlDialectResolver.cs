namespace Migratify.Dialects
{
    public class SqlDialectResolver
    {
        public static ISqlDialect GetDialect(string providerName)
        {
            providerName = providerName.ToUpperInvariant();

            if (providerName.Contains("POSTGRES") || providerName.Contains("NPGSQL"))
            {
                return new PostgreSqlDialect();
            }

            return new MsSqlDialect();
        }
    }
}
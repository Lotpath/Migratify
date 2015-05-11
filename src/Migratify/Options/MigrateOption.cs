namespace Migratify.Options
{
    public class MigrateOption : IOption
    {
        public bool IsMatch(string argument)
        {
            return argument.Contains("--migrate");
        }

        public override string ToString()
        {
            return string.Format("\t{0,-20}{1}", "--migrate", "executes migrations to bring the target database up to the latest version");
        }
    }
}
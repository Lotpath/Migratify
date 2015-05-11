namespace Migratify.Options
{
    public class TearDownOption : IOption
    {
        public bool IsMatch(string argument)
        {
            return argument.Contains("--tear-down");
        }

        public override string ToString()
        {
            return string.Format("\t{0,-20}{1}", "--tear-down", "tears down the target database, dropping all database objects and resetting the schema version to zero");
        }
    }
}
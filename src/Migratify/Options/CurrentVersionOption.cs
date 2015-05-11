namespace Migratify.Options
{
    public class CurrentVersionOption : IOption
    {
        public bool IsMatch(string argument)
        {
            return argument.Contains("--current-version");
        }

        public override string ToString()
        {
            return string.Format("\t{0,-20}{1}", "--current-version", "displays the current schema version of the target database");
        }
    }
}
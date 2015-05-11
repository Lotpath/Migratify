namespace Migratify.Options
{
    public class InitOption : IOption
    {
        public bool IsMatch(string argument)
        {
            return argument.Contains("--init");
        }

        public override string ToString()
        {
            return string.Format("\t{0,-20}{1}", "--init", "initializes the target database");
        }
    }
}
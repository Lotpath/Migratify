namespace Migratify.Options
{
    public class HelpOption : IOption
    {
        public bool IsMatch(string argument)
        {
            return argument.Contains("--help");
        }

        public override string ToString()
        {
            return string.Format("\t{0,-20}{1}", "--help", "displays this help message");
        }
    }
}
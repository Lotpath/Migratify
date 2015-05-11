namespace Migratify.Options
{
    public class DropOption : IOption
    {
        public bool IsMatch(string argument)
        {
            return argument.Contains("--drop");
        }

        public override string ToString()
        {
            return string.Format("\t{0,-20}{1}", "--drop", "drops the target database");
        }
    }
}
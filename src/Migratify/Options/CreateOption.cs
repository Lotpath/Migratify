namespace Migratify.Options
{
    public class CreateOption : IOption
    {
        public bool IsMatch(string argument)
        {
            return argument.Contains("--create");
        }

        public override string ToString()
        {
            return string.Format("\t{0,-20}{1}", "--create", "creates the target database if it does not exist");
        }
    }
}
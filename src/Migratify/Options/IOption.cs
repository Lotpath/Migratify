namespace Migratify.Options
{
    public interface IOption
    {
        bool IsMatch(string arg);
    }
}
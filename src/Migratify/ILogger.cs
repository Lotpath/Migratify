namespace Migratify
{
    public interface ILogger
    {
        void Log(string message, params object[] parameters);
    }
}
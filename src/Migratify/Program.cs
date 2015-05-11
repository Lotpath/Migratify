using Migratify.Commands;

namespace Migratify
{
    public class Program
    {
        public static void Main()
        {
            var container = new Bootstrapper().Bootstrap();
            var runner = container.GetInstance<ICommandRunner>();
            runner.Run();
        }
    }
}

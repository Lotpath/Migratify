using System;
using System.Linq;

namespace Migratify.Commands
{
    public class ArgumentSource : IArgumentSource
    {
        public string[] GetArguments()
        {
            return Environment.GetCommandLineArgs().Skip(1).ToArray();
        }
    }
}
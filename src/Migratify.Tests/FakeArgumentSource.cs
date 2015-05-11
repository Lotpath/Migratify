using Migratify.Commands;

namespace Migratify.Tests
{
    public class FakeArgumentSource : IArgumentSource
    {
        private readonly string[] _args;

        public FakeArgumentSource(string[] args)
        {
            _args = args;
        }

        public string[] GetArguments()
        {
            return _args;
        }
    }
}
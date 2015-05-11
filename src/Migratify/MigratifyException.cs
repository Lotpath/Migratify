using System;

namespace Migratify
{
    public class MigratifyException : Exception
    {
        public MigratifyException(string message)
            : this(message, null)
        {
        }

        public MigratifyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
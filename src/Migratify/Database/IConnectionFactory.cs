using System.Data;

namespace Migratify.Database
{
    public interface IConnectionFactory
    {
        IDbConnection OpenMaster();
        IDbConnection OpenTarget();
    }
}
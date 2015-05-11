using System;

namespace Migratify
{
    public interface IContainer
    {
        T GetInstance<T>();
        object GetInstance(Type type);
    }
}
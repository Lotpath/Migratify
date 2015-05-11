using System;
using Migratify.Database;

namespace Migratify
{
    public class Bootstrapper
    {
        public IContainer Bootstrap(Action<Container.ContainerConfigurer> configure = null)
        {
            var container = new Container(
                cfg =>
                    {
                        cfg.Register(new MigrationConfiguration());
                        cfg.RegisterFactory(c =>
                            {
                                var factory = c.GetInstance<IConnectionFactory>();
                                return factory.OpenTarget();
                            });
                        if (configure != null)
                        {
                            configure(cfg);
                        }
                    });
            return container;
        }
    }
}
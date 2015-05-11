using System;
using System.Collections.Generic;
using System.Linq;

namespace Migratify
{
    public class Container : IContainer
    {
        private readonly IDictionary<Type, Type> _typeRegistrations;
        private readonly IDictionary<Type, Func<IContainer, object>> _services;

        public Container(Action<ContainerConfigurer> configure = null)
        {
            _typeRegistrations = new Dictionary<Type, Type>();
            _services = new Dictionary<Type, Func<IContainer, object>>();

            _services.Add(typeof(IContainer), _ => this);

            var configurer = new ContainerConfigurer(this);
            if (configure != null)
            {
                configure(configurer);
            }
        }

        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        public object GetInstance(Type type)
        {
            if (_services.ContainsKey(type))
            {
                return _services[type];
            }

            var service = Build(type);

            return service;
        }

        private object Build(Type serviceType)
        {
            if (serviceType.IsInterface)
            {
                if (_typeRegistrations.ContainsKey(serviceType))
                {
                    return Build(_typeRegistrations[serviceType]);
                }
                else
                {
                    var concrete = serviceType
                        .Assembly
                        .GetTypes()
                        .FirstOrDefault(x => serviceType.IsAssignableFrom(x)
                            && !x.IsInterface);

                    return Build(concrete);
                }
            }

            var constructor = serviceType.GetConstructors()
                    .GroupBy(x => x)
                    .Select(x => new { Constructor = x.Key, ParameterCount = x.Sum(y => y.GetParameters().Count()) })
                    .OrderByDescending(x => x.ParameterCount)
                    .First().Constructor;

            var dependencies = new List<object>();

            foreach (var parameter in constructor.GetParameters())
            {
                if (_services.ContainsKey(parameter.ParameterType))
                {
                    dependencies.Add(_services[parameter.ParameterType](this));
                }
                else
                {
                    var dependency = Build(parameter.ParameterType);
                    dependencies.Add(dependency);
                    _services.Add(parameter.ParameterType, _ => dependency);
                }
            }

            var service = constructor.Invoke(dependencies.ToArray());

            return service;
        }

        public class ContainerConfigurer
        {
            private readonly Container _container;

            protected internal ContainerConfigurer(Container container)
            {
                _container = container;
            }

            public void Register<T>(T service)
            {
                _container._services.Add(typeof(T), _ => service);
            }

            public void Register<TAbstract, TConcrete>(TConcrete service)
                where TConcrete : TAbstract
            {
                _container._services.Add(typeof(TAbstract), _ => service);
            }

            public void Register<TAbstract, TConcrete>()
                where TConcrete : TAbstract
            {
                _container._typeRegistrations.Add(typeof(TAbstract), typeof(TConcrete));
            }

            public void RegisterFactory<T>(Func<IContainer, T> factory)
            {
                _container._services.Add(typeof(T), _ => factory(_));
            }
        }
    }
}
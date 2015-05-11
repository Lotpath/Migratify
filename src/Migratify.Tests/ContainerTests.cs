using Xunit;

namespace Migratify.Tests
{
    public class ContainerTests
    {        
        [Fact]
        public void can_auto_resolve_service_with_parameterless_ctor()
        {
            // arrange
            var container = new Container();

            // act
            var service = container.GetInstance<ServiceWithParameterlessConstructor>();

            // assert
            Assert.NotNull(service);
        }

        [Fact]
        public void can_auto_resolve_by_interface_with_default_naming_convention()
        {
            // arrange
            var container = new Container();

            // act
            var service = container.GetInstance<IServiceWithParameterlessConstructor>();

            // assert
            Assert.NotNull(service);
            Assert.IsType<ServiceWithParameterlessConstructor>(service);
        }

        [Fact]
        public void can_auto_resolve_service_with_single_dependency()
        {
            // arrange
            var container = new Container();

            // act
            var service = container.GetInstance<IServiceWithSingleDependency>();

            // assert
            Assert.NotNull(service);
            Assert.IsType<ServiceWithSingleDependency>(service);
            Assert.NotNull(service.FirstDependency);
            Assert.IsType<FirstDependency>(service.FirstDependency);
        }

        [Fact]
        public void can_register_and_resolve_with_alternative_dependency_registered_as_instantiated()
        {
            // arrange
            var container = new Container(cfg =>
                {
                    cfg.Register<IFirstDependency>(new OtherFirstDependency());
                });

            // act
            var service = container.GetInstance<IServiceWithSingleDependency>();

            // assert
            Assert.NotNull(service);
            Assert.IsType<ServiceWithSingleDependency>(service);
            Assert.NotNull(service.FirstDependency);
            Assert.IsType<OtherFirstDependency>(service.FirstDependency);
        }

        [Fact]
        public void can_register_and_resolve_with_alternative_dependency_registered_by_type()
        {
            // arrange
            var container = new Container(cfg =>
            {
                cfg.Register<IFirstDependency, OtherFirstDependency>();
            });

            // act
            var service = container.GetInstance<IServiceWithSingleDependency>();

            // assert
            Assert.NotNull(service);
            Assert.IsType<ServiceWithSingleDependency>(service);
            Assert.NotNull(service.FirstDependency);
            Assert.IsType<OtherFirstDependency>(service.FirstDependency);
        }

        [Fact]
        public void can_auto_resolve_type_with_single_implementation()
        {
            // arrange
            var container = new Container();

            // act
            var service = container.GetInstance<IServiceWithOneImplementation>();

            // assert
            Assert.NotNull(service);
            Assert.IsType<ServiceWithSingleImplementationWithDifferentNameThanInterface>(service);
        }

        public interface IServiceWithParameterlessConstructor
        {            
        }

        public class ServiceWithParameterlessConstructor : IServiceWithParameterlessConstructor
        {
        }

        public interface IFirstDependency
        {            
        }

        public class FirstDependency : IFirstDependency
        {            
        }

        public class OtherFirstDependency : IFirstDependency
        {
        }

        public interface IServiceWithSingleDependency
        {
            IFirstDependency FirstDependency { get; }
        }

        public class ServiceWithSingleDependency : IServiceWithSingleDependency
        {
            private readonly IFirstDependency _firstDependency;

            public ServiceWithSingleDependency(IFirstDependency firstDependency)
            {
                _firstDependency = firstDependency;
            }

            public IFirstDependency FirstDependency { get { return _firstDependency; } }
        }

        public interface IServiceWithOneImplementation
        {            
        }

        public class ServiceWithSingleImplementationWithDifferentNameThanInterface : IServiceWithOneImplementation
        {            
        }
    }
}
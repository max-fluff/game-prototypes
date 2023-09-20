using System;
using NUnit.Framework;
using Omega.IoC;

namespace Omega.IoC.Tests.Editor
{
    public class CoreTests
    {
        [Test]
        public void ContainerShouldBeConfigured()
        {
            var container = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
            });

            Assert.True(container.TryResolve<IPureService1>(out var pureService));
            Assert.NotNull(pureService);
        }
        
        [Test]
        public void ContainerWithDependencyShouldBeConfigured()
        {
            var container = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
                c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyImpl<IPureService1>>();
            });

            Assert.True(container.TryResolve<IPureService1>( out var pureService));
            Assert.NotNull(pureService);
            
            Assert.True(container.TryResolve<IServiceWithDependency<IPureService1>>(out var serviceWithDependency));
            Assert.NotNull(serviceWithDependency);
            Assert.NotNull(serviceWithDependency.Dependency);
        }

        [Test]
        public void ContainerWithNotContainsDependencyShouldNotBeConfigured()
        {
            Assert.Throws<DependencyNotFoundException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    //c.Add<IPureService, PureServiceImpl>();
                    c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyImpl<IPureService1>>();
                });
            });
        }
        
        [Test]
        public void ContainerWithServiceWithPrivateConstructorShouldNotBeConfigured()
        {
            Assert.Throws<InvalidImplementationTypeException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, PureService1WithPrivateConstructorImpl>();
                });
            });
        }
        
        [Test]
        public void ContainerWithServiceWithInternalConstructorShouldNotBeConfigured()
        {
            Assert.Throws<InvalidImplementationTypeException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, PureService1WithInternalConstructorImpl>();
                });
            });
        }
        
        [Test]
        public void ContainerWithServiceWithProtectedConstructorShouldNotBeConfigured()
        {
            Assert.Throws<InvalidImplementationTypeException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, PureService1WithProtectedConstructorImpl>();
                });
            });
        }
        
        [Test]
        public void ContainerWithAbstractServiceShouldNotBeConfigured()
        {
            Assert.Throws<InvalidImplementationTypeException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, AbstractPureService1Impl>();
                });
            });
        }
        
        [Test]
        public void ContainerWithServiceWithManyPublicConstructorsShouldNotBeConfigured()
        {
            Assert.Throws<InvalidImplementationTypeException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, PureService1Impl>();
                    c.Add<IServiceWithDependency<IPureService1>, ServiceWithManyPublicConstructors>();
                });
            });
        }
        
        [Test]
        public void ContainerShouldNotBeConfiguredWithMultipleImplementations()
        {
            Assert.Throws<UnableMultipleImplementationsException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, PureService1Impl>();
                    c.Add<IPureService1, PureService1Impl>();
                });
            });
        }
        
        [Test]
        public void ContainerShouldBeConfiguredWithSingletonInstance()
        {
            var instance = new PureService1Impl();
            
            var container = IoContainer.Configure(c =>
            {
                c.AddSingleton<IPureService1, PureService1Impl>(instance);
                c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyImpl<IPureService1>>();
            });

            var pureService = container.Resolve<IPureService1>();
            Assert.AreEqual(pureService, instance);

            var serviceWithDependency = container.Resolve<IServiceWithDependency<IPureService1>>();
            Assert.AreEqual(serviceWithDependency.Dependency, instance);
        }
        
        [Test]
        public void SingletonInstanceShouldBeEqualBetweenResolveCalls()
        {
            var container = IoContainer.Configure(c =>
            {
                c.AddSingleton<IPureService1, PureService1Impl>();
            });
            
            Assert.True(container.TryResolve<IPureService1>(out var pureServiceFromFirstResolve));
            Assert.True(container.TryResolve<IPureService1>(out var pureServiceFromSecondResolve));
            Assert.NotNull(pureServiceFromFirstResolve);
            Assert.AreEqual(pureServiceFromFirstResolve, pureServiceFromSecondResolve);
        }
        
        [Test]
        public void NotSingletonInstanceShouldNotBeEqualBetweenResolveCalls()
        {
            var container = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
            });
            
            Assert.True(container.TryResolve<IPureService1>( out var pureServiceFromFirstResolve));
            Assert.True(container.TryResolve<IPureService1>(out var pureServiceFromSecondResolve));
            Assert.NotNull(pureServiceFromFirstResolve);
            Assert.NotNull(pureServiceFromSecondResolve);
            Assert.AreNotEqual(pureServiceFromFirstResolve, pureServiceFromSecondResolve);
        }
    }
    
    
}
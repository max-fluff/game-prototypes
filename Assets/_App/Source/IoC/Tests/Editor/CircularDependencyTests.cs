using NUnit.Framework;
using UnityEngine;

namespace Omega.IoC.Tests.Editor
{
    public class CircularDependencyTests
    {
        [Test]
        public void SelfDependenceShouldNotBeConfigured()
        {
            Assert.Throws<SelfDependenceDetectedException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, SelfDependencePureService1Impl>();
                });
            });
        }
        
        [Test]
        public void CircularDependenciesShouldNotBeConfigured()
        {
            Assert.Throws<CircularDependencyDetectedException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IAService, AServiceDependsOfBService>();
                    c.Add<IBService, BServiceDependsOfCService>();
                    c.Add<ICService, CServiceDependsOfAService>();
                });
            });
        }
        
        [Test]
        public void ContainerShouldBeConfiguredWithSingletonInParentScopeInstance()
        {
            var instance = new PureService1Impl();
            
            var root = IoContainer.Configure(c =>
            {
                c.AddSingleton<IPureService1, PureService1Impl>(instance);
            });

            var child = root.ConfigureScoped(c =>
            {
                c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyImpl<IPureService1>>();
            });

            var pureServiceFromRoot = root.Resolve<IPureService1>();
            Assert.AreEqual(pureServiceFromRoot, instance);
            
            var pureServiceFromChild = child.Resolve<IPureService1>();
            Assert.AreEqual(pureServiceFromChild, instance);

            var serviceWithDependency = child.Resolve<IServiceWithDependency<IPureService1>>();
            Assert.AreEqual(serviceWithDependency.Dependency, instance);
        }

        public class SelfDependencePureService1Impl : IPureService1
        {
            public SelfDependencePureService1Impl(IPureService1 dependency)
            {
            }
        }
        
        public class AServiceDependsOfBService : IAService
        {
            public AServiceDependsOfBService(IBService dependency)
            {
            }
        }
        
        public class BServiceDependsOfCService : IBService
        {
            public BServiceDependsOfCService(ICService dependency)
            {
            }
        }
        
        public class CServiceDependsOfAService : ICService
        {
            public CServiceDependsOfAService(IAService dependency)
            {
            }
        }
        
        public interface IAService
        {
        }
        public interface IBService
        {
        }
        public interface ICService
        {
        }
    }
}
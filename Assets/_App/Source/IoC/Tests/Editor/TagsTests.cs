using System;
using NUnit.Framework;

namespace Omega.IoC.Tests.Editor
{
    public class TagsTests
    {
        [Test]
        public void ContainerWithTagsShouldBeConfigured1()
        {
            var container = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
                c.Add<IPureService1, PureService1ImplA>("A");
                c.Add<IPureService1, PureService1ImplB>("B");
            });

            var pureService = container.Resolve<IPureService1>();
            var pureServiceA = container.Resolve<IPureService1>("A");
            var pureServiceB = container.Resolve<IPureService1>("B");
            
            Assert.NotNull(pureService);
            Assert.NotNull(pureServiceA);
            Assert.NotNull(pureServiceB);
            
            // check implementation types
            Assert.AreNotEqual(pureService.GetType(), pureServiceA.GetType());
            Assert.AreNotEqual(pureService.GetType(), pureServiceB.GetType());
            Assert.AreNotEqual(pureServiceA.GetType(), pureServiceB.GetType());
        }

        [Test]
        public void ContainerWithMatchedTagsShouldNotBeConfigured()
        {
            Assert.Catch<UnableMultipleImplementationsException>(() =>
            {
                var container = IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, PureService1Impl>("A");
                    c.Add<IPureService1, PureService1ImplA>("A");
                });
            });
        }
        
        [Test]
        public void ResolveWithoutTagsShouldBeThrownException()
        {
            var container = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1ImplA>("A");
                c.Add<IPureService1, PureService1ImplB>("B");
            });
            
            Assert.Catch<InvalidOperationException>(() =>
            {
                container.Resolve<IPureService1>();
            });
        }
        
        [Test]
        public void InjectionWithTagDependencyShouldBeSuccess1()
        {
            var container = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>("A");
                c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyTagAsA<IPureService1>>(); // < IPureService1 with tag 'A'
            });
            
            var service = container.Resolve<IServiceWithDependency<IPureService1>>();
            Assert.NotNull(service.Dependency);
        }
        
        [Test]
        public void InjectionWithTagDependencyShouldBeSuccess2()
        {
            var container = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
                c.Add<IPureService1, PureService1ImplA>("A");
                c.Add<IPureService1, PureService1Impl>("B");
                c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyTagAsA<IPureService1>>(); // < IPureService1 with tag 'A'
            });
            
            var service = container.Resolve<IServiceWithDependency<IPureService1>>();
            Assert.NotNull(service.Dependency);
            Assert.IsInstanceOf<PureService1ImplA>(service.Dependency);
        }

        [Test]
        public void InjectionWithTagScopedDependencyShouldBeSuccess()
        {
            var root = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1ImplA>("A");
            });

            var child = root.ConfigureScoped(c =>
            {
                c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyTagAsA<IPureService1>>(); // < IPureService1 with tag 'A'
            });
            
            var service = child.Resolve<IServiceWithDependency<IPureService1>>();
            Assert.NotNull(service.Dependency);
        }        
        
        class PureService1ImplA : IPureService1
        {
        }

        class PureService1ImplB : IPureService1
        {
        }

        class ServiceWithDependencyTagAsA<T> : IServiceWithDependency<T>
        {
            public T Dependency { get; }
            
            public ServiceWithDependencyTagAsA([Tag("A")] T dependency)
            {
                Dependency = dependency;
            }
        }
    }
}
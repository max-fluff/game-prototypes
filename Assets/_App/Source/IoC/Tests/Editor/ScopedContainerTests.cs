using NUnit.Framework;

namespace Omega.IoC.Tests.Editor
{
    public class ScopedContainerTests
    {
        [Test]
        public void ScopedContainerShouldBeConfiguredAndDependenciesShouldBeInherited1()
        {
            var rootContainer = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
            });

            var scopedContainer = rootContainer.ConfigureScoped(c =>
            {
            });
            
            Assert.True(scopedContainer.TryResolve<IPureService1>(out var pureService));
            Assert.NotNull(pureService);
        }
        
        [Test]
        public void ScopedContainerShouldBeConfiguredAndDependenciesShouldBeInherited2()
        {
            var rootContainer = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
            });

            var scopedContainer = rootContainer.ConfigureScoped(c =>
            {
                c.Add<IServiceWithDependency<IPureService1>, ServiceWithDependencyImpl<IPureService1>>();
            });
            
            Assert.True(scopedContainer.TryResolve<IServiceWithDependency<IPureService1>>(out var service));
            Assert.NotNull(service);
        }
        
        [Test]
        public void ScopedContainerShouldNotBeConfiguredWithOverlap()
        {
            var rootContainer = IoContainer.Configure(c =>
            {
                c.Add<IPureService1, PureService1Impl>();
            });

            Assert.Throws<UnableOverlapImplementationException>(() =>
            {
                var scopedContainer = rootContainer.ConfigureScoped(c =>
                {
                    c.Add<IPureService1, PureService1Impl>();
                });
            });
        }
    }
}
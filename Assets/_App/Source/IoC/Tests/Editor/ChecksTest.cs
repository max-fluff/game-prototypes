using NUnit.Framework;

namespace Omega.IoC.Tests.Editor
{
    public class ChecksTests
    {
        [Test]
        public void AccessTypeIsArrayShouldNotBeConfigured()
        {
            Assert.Throws<InvalidImplementationTypeException>(() =>
            {
                IoContainer.Configure(c =>
                {
                    c.Add<int[], int[]>();
                });
            });
        }
        
        [Test]
        public void ImplementationTypeIsInterfaceShouldNotBeConfigured()
        {
            Assert.Throws<InvalidImplementationTypeException>(() =>
            {
                IoContainer.Configure(c =>
                {
                    c.Add<IPureService1, IPureService1>();
                });
            });
        }
    }
}
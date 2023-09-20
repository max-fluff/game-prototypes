namespace Omega.IoC.Tests.Editor
{
 public interface IPureService1
        {
        }
        
        public interface IPureService2
        {
        }

        public class PureService1Impl : IPureService1
        {
        }
        
        public interface IServiceWithDependency<TDependency>
        {
            public TDependency Dependency { get; }
        }
        
        public class ServiceWithDependencyImpl<TDependency> : IServiceWithDependency<TDependency>
        {
            private readonly TDependency pureService;

            public TDependency Dependency => pureService;

            public ServiceWithDependencyImpl(TDependency pureService)
            {
                this.pureService = pureService;
            }
        }
        
        public class PureService1WithPrivateConstructorImpl : IPureService1
        {
            private PureService1WithPrivateConstructorImpl()
            {
            }
        }
        
        public class PureService1WithInternalConstructorImpl : IPureService1
        {
            internal PureService1WithInternalConstructorImpl()
            {
            }
        }
        
        public class PureService1WithProtectedConstructorImpl : IPureService1
        {
            protected PureService1WithProtectedConstructorImpl()
            {
            }
        }

        public abstract class AbstractPureService1Impl : IPureService1
        {
        }
        
        public class ServiceWithManyPublicConstructors : IServiceWithDependency<IPureService1>
        {
            public IPureService1 Dependency { get; }

            public ServiceWithManyPublicConstructors()
            {
            }

            public ServiceWithManyPublicConstructors(IPureService1 dependency)
            {
                Dependency = dependency;
            }
        }
}
using System;

namespace Omega.IoC
{
    public sealed class SelfDependenceDetectedException : Exception
    {
        internal readonly Type ImplementationType;
        internal readonly TypeWithTag AccessType;

        internal SelfDependenceDetectedException(Type implementationType, TypeWithTag accessType)
            :base(Init(implementationType, accessType))
        {
            ImplementationType = implementationType;
            AccessType = accessType;
        }

        private static string Init(Type implementationType, TypeWithTag accessType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));
            if (accessType.IsDefault)
                throw new ArgumentNullException(nameof(accessType));

            return $"Self dependence was detected:\n" +
                   $"`{implementationType}` implement `{accessType}` interface\n" +
                   $"\tbut `{implementationType}` depends of `{accessType}`";
        }
    }
}
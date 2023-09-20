using System;

namespace Omega.IoC
{
    public sealed class CircularDependencyDetectedException : Exception
    {
        internal readonly Type TopImplementationType;
        internal readonly TypeWithTag TopAccessType;
        internal readonly TypeWithTag DependencyAccessType;

        internal CircularDependencyDetectedException(Type topImplementationType, TypeWithTag topAccessType,
            TypeWithTag dependencyAccessType)
            : base(Init(topImplementationType, topAccessType, dependencyAccessType))
        {
            TopImplementationType = topImplementationType;
            TopAccessType = topAccessType;
            DependencyAccessType = dependencyAccessType;
        }

        private static string Init(Type topImplementationType, TypeWithTag topAccessType, TypeWithTag dependencyAccessType)
        {
            if (topImplementationType is null)
                throw new ArgumentNullException(nameof(topImplementationType));
            if (topAccessType.IsDefault)
                throw new ArgumentNullException(nameof(topAccessType));
            if (dependencyAccessType.IsDefault)
                throw new ArgumentNullException(nameof(dependencyAccessType));

            return $"Circular dependency was detected:\n" +
                   $"`{topImplementationType}` implement `{topAccessType}` interface\n" +
                   $"\t`{topImplementationType}` depends of `{dependencyAccessType}` (through several other dependencies)\n" +
                   $"\tbut `{dependencyAccessType}` depends of `{topAccessType}`";
        }
    }
}
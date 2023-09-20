using System;

namespace Omega.IoC
{
    public sealed class DependencyNotFoundException : Exception
    {
        internal readonly Type ImplementationType;
        internal readonly TypeWithTag DependencyType;

        internal DependencyNotFoundException(Type implementationType, TypeWithTag dependencyType) 
            : base(Init(implementationType, dependencyType))
        {
            ImplementationType = implementationType;
            DependencyType = dependencyType;
        }
        
        private static string Init(Type implementationType, TypeWithTag dependencyType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));
            if (dependencyType.IsDefault)
                throw new ArgumentNullException(nameof(dependencyType));

            return $"Dependency not found:\n" +
                   $"`{implementationType}` depends of `{dependencyType}`\n" +
                   $"\tbut `{dependencyType}` not found in container";
        }
    }
}
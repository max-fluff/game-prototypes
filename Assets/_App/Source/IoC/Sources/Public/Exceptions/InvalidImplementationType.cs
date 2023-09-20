using System;

namespace Omega.IoC
{
    public sealed class InvalidImplementationTypeException : Exception
    {
        internal readonly Type ImplementationType;

        private InvalidImplementationTypeException(string message, Type implementationType) 
            : base(message)
        {
            ImplementationType = implementationType;
        }

        public static InvalidImplementationTypeException New(Type implementationType,
            RejectConstructorInjectionReason reason)
        {
            return reason switch
            {
                RejectConstructorInjectionReason.ConstructorNotFound => ConstructorNotFound(implementationType),
                RejectConstructorInjectionReason.ConstructorsConflict => ConstructorConflict(implementationType),
                RejectConstructorInjectionReason.CantInjectToAbstractClass => CantInjectToAbstractClass(implementationType),
                RejectConstructorInjectionReason.CantInjectToInterface => CantInjectToInterface(implementationType),
                RejectConstructorInjectionReason.CantInjectToStaticClass => CantInjectToStaticClass(implementationType),
                RejectConstructorInjectionReason.CantInjectToValueType => CantInjectToValueType(implementationType),
                RejectConstructorInjectionReason.CantInjectToArray => CantInjectToArray(implementationType),
                _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
            };
        }
        
        public static InvalidImplementationTypeException ConstructorNotFound(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            var message = $"No public constructor was found for the implementation type.\n" +
                          $"Implementation type: `{implementationType}`";

            return new InvalidImplementationTypeException(message, implementationType);
        }
        public static InvalidImplementationTypeException ConstructorConflict(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            var message = $"Several public constructors were found and only one was expected.\n" +
                          $"Implementation type: `{implementationType}`";

            return new InvalidImplementationTypeException(message, implementationType);
        }

        public static InvalidImplementationTypeException CantInjectToAbstractClass(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            var message = $"Implementation type cant be an abstract class" +
                          $"Implementation type: `{implementationType}`";

            return new InvalidImplementationTypeException(message, implementationType);
        }
        public static InvalidImplementationTypeException CantInjectToInterface(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            var message = $"Implementation type cant be an interface" +
                          $"Implementation type: `{implementationType}`";

            return new InvalidImplementationTypeException(message, implementationType);
        }
        public static InvalidImplementationTypeException CantInjectToStaticClass(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            var message = $"Implementation type cant be an static class" +
                          $"Implementation type: `{implementationType}`";

            return new InvalidImplementationTypeException(message, implementationType);
        }
        public static InvalidImplementationTypeException CantInjectToValueType(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            var message = $"Implementation type cant be an value-type" +
                          $"Implementation type: `{implementationType}`";

            return new InvalidImplementationTypeException(message, implementationType);
        }
        
        public static InvalidImplementationTypeException CantInjectToArray(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));

            var message = $"Implementation type cant be an array" +
                          $"Implementation type: `{implementationType}`";

            return new InvalidImplementationTypeException(message, implementationType);
        }
    }
}
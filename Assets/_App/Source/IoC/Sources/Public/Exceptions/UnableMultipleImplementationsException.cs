using System;

namespace Omega.IoC
{
    // TODO: improve message by recommendation to using tags 
    public sealed class UnableMultipleImplementationsException : Exception
    {
        internal readonly TypeWithTag AccessType;
        internal readonly Type FirstImplementationType;
        internal readonly Type SecondImplementationType;
        
        internal UnableMultipleImplementationsException(TypeWithTag accessType, Type firstImplementationType,
            Type secondImplementationType)
            : base(Init(accessType, firstImplementationType, secondImplementationType))
        {
            AccessType = accessType;
            FirstImplementationType = firstImplementationType;
            SecondImplementationType = secondImplementationType;
        }

        private static string Init(TypeWithTag accessType, Type firstImplementationType,
            Type secondImplementationType)
        {
            if (accessType.IsDefault)
                throw new ArgumentNullException(nameof(accessType));
            if (firstImplementationType is null)
                throw new ArgumentNullException(nameof(firstImplementationType));
            if (secondImplementationType is null)
                throw new ArgumentNullException(nameof(secondImplementationType));

            return $"Unable to define multiple implementations for the same Access type." +
                   $"Implementation for `{accessType}` already defined as `{firstImplementationType}`\n" +
                   $"\tbut there was also an attempt to define it as `{secondImplementationType}`";
        }
    }
}
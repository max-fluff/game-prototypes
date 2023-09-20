using System;

namespace Omega.IoC
{
    public sealed class UnableOverlapImplementationException : Exception
    {
        internal readonly TypeWithTag AccessType;

        internal UnableOverlapImplementationException(TypeWithTag accessType)
            : base(Init(accessType))
        {
            AccessType = accessType;
        }

        private static string Init(TypeWithTag accessType)
        {
            if (accessType.IsDefault)
                throw new ArgumentNullException(nameof(accessType));

            return $"Unable to overlap implementation defined in the parent container." +
                   $"Implementation for `{accessType}` already defined in parent container";
        }
    }
}
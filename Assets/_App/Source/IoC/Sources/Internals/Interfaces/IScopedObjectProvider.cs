using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Omega.IoC
{
    internal interface IScopedObjectProvider
    {
        bool TryGetScopedInstance(TypeWithTag accessType, out object result);
        bool CanGetScopedInstance(TypeWithTag accessType);
    }
}
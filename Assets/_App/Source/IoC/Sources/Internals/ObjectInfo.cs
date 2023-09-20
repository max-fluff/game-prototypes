using System;

namespace Omega.IoC
{
    internal struct ObjectInfo
    {
        public TypeWithTag AccessType;
        
        public Type ImplementationType;
        
        public bool IsSingleton;
        public object Instance;
    }
}
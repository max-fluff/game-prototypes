using System;

namespace Omega.IoC
{
    public static class Exts
    {
        public static IoContainer ConfigureScoped(this IoContainer container, ConfigureDelegate action)
        {
            if (container is null)
                throw new NullReferenceException();
            
            return IoContainer.Configure(container, action);
        }
    }
}
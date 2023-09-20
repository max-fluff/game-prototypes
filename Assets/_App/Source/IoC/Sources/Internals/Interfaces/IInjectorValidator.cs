using System;

namespace Omega.IoC
{
    internal interface IInjectorValidator
    {
        bool CanBeInjected(Type type, out RejectConstructorInjectionReason rejectReason);
    }
}
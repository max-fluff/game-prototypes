using System;
using System.Reflection;

namespace Omega.IoC
{
    internal interface IConstructorInfoProvider
    {
        ParameterInfo[] GetConstructorParameters(Type targetType);
    }
}
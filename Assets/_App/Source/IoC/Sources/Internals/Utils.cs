using System;
using System.Collections.Generic;
using System.Reflection;

namespace Omega.IoC
{
    internal static class Utils
    {
        internal static string GetTagOrNull(this ParameterInfo parameterInfo)
        {
            var tagAttribute = parameterInfo.GetCustomAttribute<TagAttribute>();
            return tagAttribute?.Tag;
        }
        
        internal static bool TryFind<T>(this List<T> list, Predicate<T> predicate, out T result)
        {
            var index = list.FindIndex(predicate);
            if (index < 0)
            {
                result = default;
                return false;
            }

            result = list[index];
            return true;
        }
    }
}
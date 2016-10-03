using System;
using System.Linq;
using System.Reflection;

namespace Combo
{
    static class Utils
    {
        public static bool IsType(this object obj, Type type)
        {
            return obj != null && type.GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo());
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttributes<T>().Any();
        }
    }
}

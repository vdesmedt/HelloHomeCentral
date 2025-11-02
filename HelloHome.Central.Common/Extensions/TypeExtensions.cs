using System;
using System.Linq;

namespace HelloHome.Central.Common.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasAttribute<T>(this Type type, bool includeInherited = true) where T : Attribute
        {
            return type.IsDefined(typeof(T), includeInherited);
        }

        public static T? GetAttribute<T>(this Type provider, bool includeInherited = true) where T : Attribute
        {
            var attrs = provider.GetCustomAttributes(typeof(T), includeInherited);
            return attrs.FirstOrDefault() as T;
        }        
        
        public static bool IsSubclassOfRawGeneric (this Type toCheck, Type generic )
        {
            while (toCheck != null && toCheck != typeof (object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition () : toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public static string GetShortName(this Type type){
            var parts = type.FullName.Split('.').Skip(2);
            var shortParts =
                parts
                    .Take(parts.Count() - 1)
                    .Select(x => new string(x.Where(y => y >= 65 && y <= 91).ToArray())).ToList();
            shortParts.Add(parts.Last());
            return string.Join('.', shortParts);
        }
    }
}
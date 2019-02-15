using System;
using System.Linq;

namespace HelloHome.Central.Common.Extensions
{
    public static class TypeExtensions
    {
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
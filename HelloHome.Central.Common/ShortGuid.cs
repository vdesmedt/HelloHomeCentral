using System;

namespace HelloHome.Central.Common
{
    public class ShortGuid
    {
        public static string Create()
        {
            string modifiedBase64 = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace('+', '-').Replace('/', '_')
                .Substring(0, 22);
            return modifiedBase64;            
        }
        public static byte[] CreateAsByteArray()
        {
            return Guid.NewGuid().ToByteArray();
        }
    }
}
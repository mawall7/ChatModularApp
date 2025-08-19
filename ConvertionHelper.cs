using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularApp
{
    public static class ConvertionHelper
    {
        public static byte[] ToBytes(this string source) //don't use static is not testable
        {
            if (source == null)
            {
                return null;
            }
            byte[] messageBytes = Encoding.UTF8.GetBytes(source);
            return messageBytes;
        }
    }
}

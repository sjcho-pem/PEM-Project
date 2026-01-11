using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
    public class StructParsing
    {
        public static byte[] MethodObjectToByteSerializer(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] dst = new byte[len];

            try
            {
                IntPtr src = Marshal.AllocHGlobal(len);
                Marshal.StructureToPtr(obj, src, false);
                Marshal.Copy(src, dst, 0, len);

                Marshal.FreeHGlobal(src);
            }
            catch
            {
                return null;
            }

            return dst;
        }

        public static object MethodByteToObjectDeserialize(byte[] src, Type type)
        {
            object obj = null;
            int len = src.Length;
            Type typeTemp = type;

            try
            {
                IntPtr dst = Marshal.AllocHGlobal(len);
                Marshal.Copy(src, 0, dst, len);

                obj = Marshal.PtrToStructure(dst, typeTemp);
                Marshal.FreeHGlobal(dst);
            }
            catch
            {
                return obj;
            }

            return obj;
        }
    }
}

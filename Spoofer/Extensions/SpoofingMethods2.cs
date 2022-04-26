using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.EXMethods
{
    public static class SpoofingMethods2
    {
        private const string LIBRARY_FILENAME2 = "Core2.dll";
        [DllImport(LIBRARY_FILENAME2, EntryPoint = "main", CallingConvention = CallingConvention.Cdecl)]
        public static extern int main(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);
    }
}

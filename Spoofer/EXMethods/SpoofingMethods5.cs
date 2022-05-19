using System;
using System.Runtime.InteropServices;

namespace Spoofer.EXMethods
{
    internal class SpoofingMethods5
    {
        private const string LIBRARY_FILENAME2 = "Core5.dll";
        [DllImport(LIBRARY_FILENAME2, EntryPoint = "main", CallingConvention = CallingConvention.Cdecl)]
        public static extern int main(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);
    }
}
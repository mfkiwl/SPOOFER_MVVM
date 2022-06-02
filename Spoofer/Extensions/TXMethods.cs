using System.Runtime.InteropServices;

namespace Spoofer.EXMethods
{
    public static class TXMethods
    {
        private const string LIBRARY_FILENAME = "uhd.dll";
        [DllImport(LIBRARY_FILENAME, EntryPoint = "main", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UHD_SAFE_MAIN(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);
    }
}

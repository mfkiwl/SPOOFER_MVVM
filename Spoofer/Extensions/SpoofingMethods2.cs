using System.Runtime.InteropServices;

namespace Spoofer.EXMethods
{
    public static class SpoofingMethods2
    {
        private const string LIBRARY_FILENAME2 = "Core2.dll";
        [DllImport(LIBRARY_FILENAME2, EntryPoint = "main", CallingConvention = CallingConvention.Cdecl)]
        public static extern int main(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);
    }
}

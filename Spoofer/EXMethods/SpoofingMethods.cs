using System;
using System.Runtime.InteropServices;

namespace Spoofer.EXMethods
{
    public static class SpoofingMethods
    {
        private const string LIBRARY_FILENAME = "Core.dll";

        [DllImport(LIBRARY_FILENAME, SetLastError = true)]
        public static extern double normVect(IntPtr x);

        [DllImport(LIBRARY_FILENAME)]
        public static extern int MyMethod();
    }
}
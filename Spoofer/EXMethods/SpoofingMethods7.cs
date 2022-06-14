﻿using System.Runtime.InteropServices;

namespace Spoofer.EXMethods
{
    internal class SpoofingMethods7
    {
        private const string LIBRARY_FILENAME2 = @"C:\Program Files (x86)\Phantom Technologies LTD\Spoofer\Core7.dll";
        [DllImport(LIBRARY_FILENAME2, EntryPoint = "main", CallingConvention = CallingConvention.Cdecl)]
        public static extern int main(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);
    }
}
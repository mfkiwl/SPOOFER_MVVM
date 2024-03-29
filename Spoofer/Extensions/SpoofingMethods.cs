﻿using System.Runtime.InteropServices;

namespace Spoofer.EXMethods
{
    public static class SpoofingMethods
    {
        private const string LIBRARY_FILENAME = @"C:\Program Files (x86)\Phantom Technologies LTD\Spoofer\Core.dll";


        
        [DllImport(LIBRARY_FILENAME, EntryPoint = "normVect", CallingConvention = CallingConvention.Cdecl)]
        public static extern double normVect(ref double x);
        [DllImport(LIBRARY_FILENAME)]
        public static extern int MyMethod();

        [DllImport(LIBRARY_FILENAME, EntryPoint = "main", CallingConvention = CallingConvention.Cdecl)]
        public static extern int main(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);


    }
}
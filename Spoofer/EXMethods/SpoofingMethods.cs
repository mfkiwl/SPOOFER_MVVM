using Spoofer.Structs;
using System;
using System.Runtime.InteropServices;

namespace Spoofer.EXMethods
{
    public static class SpoofingMethods
    {
        private const string LIBRARY_FILENAME = "Core.dll";

        [DllImport(LIBRARY_FILENAME, EntryPoint = "normVect", CallingConvention = CallingConvention.Cdecl)]
        public static  extern double normVect(ref double x);

        [DllImport(LIBRARY_FILENAME)]
        public static extern int MyMethod();
        [DllImport(LIBRARY_FILENAME, EntryPoint = "readRinexNavAll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int readRinexNavAll(Ephem_T[][] eph, ref IonOutC_T ionOutC, ref char fName);
    }
}
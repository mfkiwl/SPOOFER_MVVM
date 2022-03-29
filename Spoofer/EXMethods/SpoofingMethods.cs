using Spoofer.Structs;
using System;
using System.IO;
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
        [DllImport(LIBRARY_FILENAME, EntryPoint = "main", CallingConvention = CallingConvention.StdCall)]
        public static extern int main(double [] llh, Ephem_T [,] eph, GPSTime_T g0, Channel_T[] chan, GPSTime_T grx, DateTime_T t0, DateTime_T tmin, DateTime_T tmax
            , GPSTime_T gmin, GPSTime_T gmax, IonOutC_T ionutc, GPSTime_T gtmap, DateTime_T ttmp, Range_T rho);
    }
}
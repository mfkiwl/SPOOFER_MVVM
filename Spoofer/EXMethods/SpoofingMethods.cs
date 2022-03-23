using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.EXMethods
{
    public static class SpoofingMethods
    {
        private const string LIBRARY_FILENAME = "Core.dll";
        [DllImport(LIBRARY_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double normVect(double x);
    }
}

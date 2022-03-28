using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Range_T
    {
        public GPSTime_T g;
        public double range; // pseudorange
        public double rate;
        public double d; // geometric distance
        public double[] azel;
        public double iono_delay;
    }
}

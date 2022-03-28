using System.Runtime.InteropServices;

namespace Spoofer.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct GPSTime_T
    {
       public int week;   /*!< GPS week number (since January 1980) */
       public double sec; 	/*!< second inside the GPS \a week */
    }
}
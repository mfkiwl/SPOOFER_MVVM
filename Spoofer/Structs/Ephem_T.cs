using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Ephem_T
    {
        public DateTime_T t;
        public GPSTime_T toc;
        public GPSTime_T toe;
        int iodc;   /*!< Issue of Data, Clock */
        int iode;   /*!< Isuse of Data, Ephemeris */
        double deltan;  /*!< Delta-N (radians/sec) */
        double cuc; /*!< Cuc (radians) */
        double cus; /*!< Cus (radians) */
        double cic; /*!< Correction to inclination cos (radians) */
        double cis; /*!< Correction to inclination sin (radians) */
        double crc; /*!< Correction to radius cos (meters) */
        double crs; /*!< Correction to radius sin (meters) */
        double ecc; /*!< e Eccentricity */
        double sqrta;   /*!< sqrt(A) (sqrt(m)) */
        double m0;  /*!< Mean anamoly (radians) */
        double omg0;    /*!< Longitude of the ascending node (radians) */
        double inc0;    /*!< Inclination (radians) */
        double aop;
        double omgdot;  /*!< Omega dot (radians/s) */
        double idot;    /*!< IDOT (radians/s) */
        double af0; /*!< Clock offset (seconds) */
        double af1; /*!< rate (sec/sec) */
        double af2; /*!< acceleration (sec/sec^2) */
        double tgd; /*!< Group delay L2 bias */
        int svhlth;
        int codeL2;
        // Working variables follow
        double n;   /*!< Mean motion (Average angular velocity) */
        double sq1e2;   /*!< sqrt(1-e^2) */
        double A;   /*!< Semi-major axis */
        double omgkdot; /*!< OmegaDot-OmegaEdot */
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Channel_T
    {
        int prn;    /*< PRN Number */
        int[] ca; /*< C/A Sequence */
        double f_carr;  /*< Carrier frequency */
        double f_code;  /*< Code frequency */
        double carr_phase;
        int carr_phasestep; /*< Carrier phasestep */
        double code_phase; /*< Code phase */
        GPSTime_T g0;   /*!< GPS time at start */
        int iword;  /*!< initial word */
        int ibit;   /*!< initial bit */
        int icode;  /*!< initial code */
        int dataBit;    /*!< current data bit */
        int codeCA; /*!< current C/A code */
        double [] azel;
        Range_T rho0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct IonOutC_T
    {
        public int enable;
        public int vflg;
        public double alpha0, alpha1, alpha2, alpha3;
        public double beta0, beta1, beta2, beta3;
        public double A0, A1;
        public int dtls, tot, wnt;
        public int dtlsf, dn, wnlsf;
    }
}

#ifndef GPSSIM_H
#define GPSSIM_H

#define FLOAT_CARR_PHASE // For RKT simulation. Higher computational load, but smoother carrier phase.

#define TRUE	(1)
#define FALSE	(0)

/*! \brief Maximum length of a line in a text file (RINEX, motion) */
#define MAX_CHAR (100)

/*! \brief Maximum number of satellites in RINEX file */
#define MAX_SAT (32)

/*! \brief Maximum number of channels we simulate */
#define MAX_CHAN (16)

/*! \brief Maximum number of user motion points */
#ifndef USER_MOTION_SIZE
#define USER_MOTION_SIZE (3000) // max duration at 10Hz
#endif

/*! \brief Maximum duration for static mode*/
#define STATIC_MAX_DURATION (86400) // second

/*! \brief Number of subframes */
#define N_SBF (5) // 5 subframes per frame

/*! \brief Number of words per subframe */
#define N_DWRD_SBF (10) // 10 word per subframe

/*! \brief Number of words */
#define N_DWRD ((N_SBF+1)*N_DWRD_SBF) // Subframe word buffer size

/*! \brief C/A code sequence length */
#define CA_SEQ_LEN (1023)

#define SECONDS_IN_WEEK 604800.0
#define SECONDS_IN_HALF_WEEK 302400.0
#define SECONDS_IN_DAY 86400.0
#define SECONDS_IN_HOUR 3600.0
#define SECONDS_IN_MINUTE 60.0

#define POW2_M5  0.03125
#define POW2_M19 1.907348632812500e-6
#define POW2_M29 1.862645149230957e-9
#define POW2_M31 4.656612873077393e-10
#define POW2_M33 1.164153218269348e-10
#define POW2_M43 1.136868377216160e-13
#define POW2_M55 2.775557561562891e-17

#define POW2_M50 8.881784197001252e-016
#define POW2_M30 9.313225746154785e-010
#define POW2_M27 7.450580596923828e-009
#define POW2_M24 5.960464477539063e-008

// Conventional values employed in GPS ephemeris model (ICD-GPS-200)
#define GM_EARTH 3.986005e14
#define OMEGA_EARTH 7.2921151467e-5
#define PI 3.1415926535898

#define WGS84_RADIUS	6378137.0
#define WGS84_ECCENTRICITY 0.0818191908426

#define R2D 57.2957795131

#define SPEED_OF_LIGHT 2.99792458e8
#define LAMBDA_L1 0.190293672798365

/*! \brief GPS L1 Carrier frequency */
#define CARR_FREQ (1575.42e6)
/*! \brief C/A code frequency */
#define CODE_FREQ (1.023e6)
#define CARR_TO_CODE (1.0/1540.0)

// Sampling data format
#define SC01 (1)
#define SC08 (8)
#define SC16 (16)

#define EPHEM_ARRAY_SIZE (13) // for daily GPS broadcast ephemers file (brdc)

/*! \brief Structure representing GPS time */
typedef struct
{
	int week;	/*!< GPS week number (since January 1980) */
	double sec; 	/*!< second inside the GPS \a week */
} gpstime_t;

/*! \brief Structure repreenting UTC time */
typedef struct
{
	int y; 		/*!< Calendar year */
	int m;		/*!< Calendar month */
	int d;		/*!< Calendar day */
	int hh;		/*!< Calendar hour */
	int mm;		/*!< Calendar minutes */
	double sec;	/*!< Calendar seconds */
} datetime_t;

/*! \brief Structure representing ephemeris of a single satellite */
typedef struct
{
	int vflg;	/*!< Valid Flag */
	datetime_t t;
	gpstime_t toc;	/*!< Time of Clock */
	gpstime_t toe;	/*!< Time of Ephemeris */
	int iodc;	/*!< Issue of Data, Clock */
	int iode;	/*!< Isuse of Data, Ephemeris */
	double deltan;	/*!< Delta-N (radians/sec) */
	double cuc;	/*!< Cuc (radians) */
	double cus;	/*!< Cus (radians) */
	double cic;	/*!< Correction to inclination cos (radians) */
	double cis;	/*!< Correction to inclination sin (radians) */
	double crc;	/*!< Correction to radius cos (meters) */
	double crs;	/*!< Correction to radius sin (meters) */
	double ecc;	/*!< e Eccentricity */
	double sqrta;	/*!< sqrt(A) (sqrt(m)) */
	double m0;	/*!< Mean anamoly (radians) */
	double omg0;	/*!< Longitude of the ascending node (radians) */
	double inc0;	/*!< Inclination (radians) */
	double aop;
	double omgdot;	/*!< Omega dot (radians/s) */
	double idot;	/*!< IDOT (radians/s) */
	double af0;	/*!< Clock offset (seconds) */
	double af1;	/*!< rate (sec/sec) */
	double af2;	/*!< acceleration (sec/sec^2) */
	double tgd;	/*!< Group delay L2 bias */
	int svhlth;
	int codeL2;
	// Working variables follow
	double n; 	/*!< Mean motion (Average angular velocity) */
	double sq1e2;	/*!< sqrt(1-e^2) */
	double A;	/*!< Semi-major axis */
	double omgkdot; /*!< OmegaDot-OmegaEdot */
} ephem_t;

typedef struct
{
	int enable;
	int vflg;
	double alpha0, alpha1, alpha2, alpha3;
	double beta0, beta1, beta2, beta3;
	double A0, A1;
	int dtls, tot, wnt;
	int dtlsf, dn, wnlsf;
} ionoutc_t;

typedef struct
{
	gpstime_t g;
	double range; // pseudorange
	double rate;
	double d; // geometric distance
	double azel[2];
	double iono_delay;
} range_t;

/*! \brief Structure representing a Channel */
typedef struct
{
	int prn;	/*< PRN Number */
	int ca[CA_SEQ_LEN]; /*< C/A Sequence */
	double f_carr;	/*< Carrier frequency */
	double f_code;	/*< Code frequency */
#ifdef FLOAT_CARR_PHASE
	double carr_phase;
#else
	unsigned int carr_phase; /*< Carrier phase */
	int carr_phasestep;	/*< Carrier phasestep */
#endif
	double code_phase; /*< Code phase */
	gpstime_t g0;	/*!< GPS time at start */
	unsigned long sbf[5][N_DWRD_SBF]; /*!< current subframe */
	unsigned long dwrd[N_DWRD]; /*!< Data words of sub-frame */
	int iword;	/*!< initial word */
	int ibit;	/*!< initial bit */
	int icode;	/*!< initial code */
	int dataBit;	/*!< current data bit */
	int codeCA;	/*!< current C/A code */
	double azel[2];
	range_t rho0;
} channel_t;

#endif

#ifdef GPSSIM_DLL_EXPORTS
#define GPSSIM_DLL __declspec(dllexport)
#else
#define GPSSIM_DLL __declspec(dllimport)
#endif
extern GPSSIM_DLL int MyMethod();
extern  GPSSIM_DLL void subVect(double* y, const double* x1, const double* x2);
extern  GPSSIM_DLL double normVect(const double* x);
extern  GPSSIM_DLL double dotProd(const double* x1, const double* x2);
extern  GPSSIM_DLL void codegen(int* ca, int prn);
extern  GPSSIM_DLL void date2gps(const datetime_t* t, gpstime_t* g);
extern  GPSSIM_DLL void gps2date(const gpstime_t* g, datetime_t* t);
extern  GPSSIM_DLL void xyz2llh(const double* xyz, double* llh);
extern  GPSSIM_DLL void llh2xyz(const double* llh, double* xyz);
extern  GPSSIM_DLL void ltcmat(const double* llh, double t[3][3]);
extern  GPSSIM_DLL void ecef2neu(const double* xyz, double t[3][3], double* neu);
extern  GPSSIM_DLL void neu2azel(double* azel, const double* neu);
extern  GPSSIM_DLL void satpos(ephem_t eph, gpstime_t g, double* pos, double* vel, double* clk);
extern  GPSSIM_DLL void eph2sbf(const ephem_t eph, const ionoutc_t ionoutc, unsigned long sbf[5][N_DWRD_SBF]);
extern  GPSSIM_DLL unsigned long countBits(unsigned long v);
extern  GPSSIM_DLL unsigned long computeChecksum(unsigned long source, int nib);
extern  GPSSIM_DLL int replaceExpDesignator(char* str, int len);
extern  GPSSIM_DLL double subGpsTime(gpstime_t g1, gpstime_t g0);
extern  GPSSIM_DLL gpstime_t incGpsTime(gpstime_t g0, double dt);
extern  GPSSIM_DLL int readRinexNavAll(ephem_t eph[][MAX_SAT], ionoutc_t* ionoutc, const char* fname);
extern  GPSSIM_DLL double ionosphericDelay(const ionoutc_t* ionoutc, gpstime_t g, double* llh, double* azel);
extern  GPSSIM_DLL void computeRange(range_t* rho, ephem_t eph, ionoutc_t* ionoutc, gpstime_t g, double xyz[]);
extern  GPSSIM_DLL void computeCodePhase(channel_t* chan, range_t rho1, double dt);
extern  GPSSIM_DLL int readUserMotion(double xyz[USER_MOTION_SIZE][3], const char* filename);
extern  GPSSIM_DLL int readNmeaGGA(double xyz[USER_MOTION_SIZE][3], const char* filename);
extern  GPSSIM_DLL int generateNavMsg(gpstime_t g, channel_t* chan, int init);
extern  GPSSIM_DLL int checkSatVisibility(ephem_t eph, gpstime_t g, double* xyz, double elvMask, double* azel);
extern  GPSSIM_DLL int allocateChannel(channel_t* chan, ephem_t* eph, ionoutc_t ionoutc, gpstime_t grx, double* xyz, double elvMask);
extern  GPSSIM_DLL void usage(void);
extern  GPSSIM_DLL int main(int argc, char* argv[]);

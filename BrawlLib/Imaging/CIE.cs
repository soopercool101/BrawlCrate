using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LabPixel
    {
        public double L;
        public double A;
        public double B;

        public LabPixel(double l, double a, double b)
        {
            L = l;
            A = a;
            B = b;
        }

        public static explicit operator LabPixel(ARGBPixel p)
        {
            double r = CIE.powtable[p.R];
            double g = CIE.powtable[p.G];
            double b = CIE.powtable[p.B];

            double x = CIE.mRGB_XYZ[0, 0] * r + CIE.mRGB_XYZ[0, 1] * g + CIE.mRGB_XYZ[0, 2] * b / CIE.xnn;
            double y = CIE.mRGB_XYZ[1, 0] * r + CIE.mRGB_XYZ[1, 1] * g + CIE.mRGB_XYZ[1, 2] * b / CIE.ynn;
            double z = CIE.mRGB_XYZ[2, 0] * r + CIE.mRGB_XYZ[2, 1] * g + CIE.mRGB_XYZ[2, 2] * b / CIE.znn;

            double l;
            if (y > 0.008856)
            {
                y = Math.Pow(y, 1.0 / 3.0);
                l = 116.0 * y - 16.0;
            }
            else
            {
                l = y > 0.0 ? y * 903.3 : 0.0;
                y = 7.787 * y + 16.0 / 116.0;
            }

            x = x > 0.00856 ? Math.Pow(x, 1.0 / 3.0) : 7.787 * x + 16.0 / 116.0;
            z = z > 0.00856 ? Math.Pow(z, 1.0 / 3.0) : 7.787 * z + 16.0 / 116.0;

            return new LabPixel(l, 500.0 * (x - y), 200.0 * (y - z));
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XYZPixel
    {
        public double X;
        public double Y;
        public double Z;

        public XYZPixel(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static explicit operator XYZPixel(ARGBPixel p)
        {
            double r = CIE.powtable[p.R];
            double g = CIE.powtable[p.G];
            double b = CIE.powtable[p.B];

            double x = CIE.mRGB_XYZ[0, 0] * r + CIE.mRGB_XYZ[0, 1] * g + CIE.mRGB_XYZ[0, 2] * b;
            double y = CIE.mRGB_XYZ[1, 0] * r + CIE.mRGB_XYZ[1, 1] * g + CIE.mRGB_XYZ[1, 2] * b;
            double z = CIE.mRGB_XYZ[2, 0] * r + CIE.mRGB_XYZ[2, 1] * g + CIE.mRGB_XYZ[2, 2] * b;

            return new XYZPixel(x, y, z);
        }
    }

    public static class CIE
    {
        internal static double[] powtable = new double[256];

        internal static double[,] mRGB_XYZ = new double[3, 3]
        {
            {0.412410846488539, 0.357584567852952, 0.18045380393360835},
            {0.21264934272065292, 0.715169135705904, 0.072181521573443333},
            {0.019331758429150282, 0.11919485595098411, 0.9503900340503374}
        };

        internal static double[,] mXYZ_RGB = new double[3, 3]
        {
            {3.2408123988952813, -1.5373084456298127, -0.49858652290696637},
            {-0.96924301700864013, 1.8759663029085734, 0.04155503085668566},
            {0.055638398436112846, -0.20400746093241376, 1.057129570286143}
        };

        internal const double pxr = 0.64;
        internal const double pyr = 0.33;
        internal const double pxg = 0.30;
        internal const double pyg = 0.60;
        internal const double pxb = 0.15;
        internal const double pyb = 0.06;
        internal const double lxn = 0.312713;
        internal const double lyn = 0.329016;
        internal const double xnn = lxn / lyn;
        internal const double ynn = 1.0;
        internal const double znn = (ynn - (lxn + lyn)) / lyn;
        internal const double gamma = 2.2;

        internal const double lowA = -86.181;
        internal const double lowB = -107.858;
        internal const double highA = 98.237;
        internal const double highB = 94.480;

        internal const double lrat = 2.55;
        internal const double arat = 255.0 / (highA - lowA);
        internal const double brat = 255.0 / (highB - lowB);

        static CIE()
        {
            init_powtable(gamma);
        }

        private static void init_powtable(double gamma)
        {
            int i;
            for (i = 0; i < 11; i++)
            {
                powtable[i] = i / 255.0 * 12.92;
            }

            for (; i < 256; i++)
            {
                powtable[i] = Math.Pow((i / 255.0 + 0.055) / 1.055, 2.4);
            }
        }

        //static void rgbxyzrgb_init()
        //{
        //    init_powtable(gamma);

        //    {
        //        double[,] MRC = new double[3, 3];
        //        double[,] MRCi = new double[3, 3];

        //        double C1, C2, C3;

        //        MRC[0, 0] = pxr;
        //        MRC[0, 1] = pxg;
        //        MRC[0, 2] = pxb;
        //        MRC[1, 0] = pyr;
        //        MRC[1, 1] = pyg;
        //        MRC[1, 2] = pyb;
        //        MRC[2, 0] = 1.0 - (pxr + pyr);
        //        MRC[2, 1] = 1.0 - (pxg + pyg);
        //        MRC[2, 2] = 1.0 - (pxb + pyb);

        //        Minvert(MRC, MRCi);

        //        C1 = (MRCi[0, 0] * xnn) + (MRCi[0, 1] * ynn) + (MRCi[0, 2] * znn);
        //        C2 = (MRCi[1, 0] * xnn) + (MRCi[1, 1] * ynn) + (MRCi[1, 2] * znn);
        //        C3 = (MRCi[2, 0] * xnn) + (MRCi[2, 1] * ynn) + (MRCi[2, 2] * znn);

        //        mRGB_XYZ[0, 0] = MRC[0, 0] * C1;
        //        mRGB_XYZ[0, 1] = MRC[0, 1] * C2;
        //        mRGB_XYZ[0, 2] = MRC[0, 2] * C3;
        //        mRGB_XYZ[1, 0] = MRC[1, 0] * C1;
        //        mRGB_XYZ[1, 1] = MRC[1, 1] * C2;
        //        mRGB_XYZ[1, 2] = MRC[1, 2] * C3;
        //        mRGB_XYZ[2, 0] = MRC[2, 0] * C1;
        //        mRGB_XYZ[2, 1] = MRC[2, 1] * C2;
        //        mRGB_XYZ[2, 2] = MRC[2, 2] * C3;

        //        Minvert(mRGB_XYZ, mXYZ_RGB);
        //    }
        //}

        //static int Minvert(double[,] src, double[,] dest)
        //{
        //    double det;

        //    dest[0, 0] = src[1, 1] * src[2, 2] - src[1, 2] * src[2, 1];
        //    dest[0, 1] = src[0, 2] * src[2, 1] - src[0, 1] * src[2, 2];
        //    dest[0, 2] = src[0, 1] * src[1, 2] - src[0, 2] * src[1, 1];
        //    dest[1, 0] = src[1, 2] * src[2, 0] - src[1, 0] * src[2, 2];
        //    dest[1, 1] = src[0, 0] * src[2, 2] - src[0, 2] * src[2, 0];
        //    dest[1, 2] = src[0, 2] * src[1, 0] - src[0, 0] * src[1, 2];
        //    dest[2, 0] = src[1, 0] * src[2, 1] - src[1, 1] * src[2, 0];
        //    dest[2, 1] = src[0, 1] * src[2, 0] - src[0, 0] * src[2, 1];
        //    dest[2, 2] = src[0, 0] * src[1, 1] - src[0, 1] * src[1, 0];

        //    det =
        //      src[0, 0] * dest[0, 0] +
        //      src[0, 1] * dest[1, 0] +
        //      src[0, 2] * dest[2, 0];

        //    if (det <= 0.0)
        //    {
        //        return 0;
        //    }

        //    dest[0, 0] /= det;
        //    dest[0, 1] /= det;
        //    dest[0, 2] /= det;
        //    dest[1, 0] /= det;
        //    dest[1, 1] /= det;
        //    dest[1, 2] /= det;
        //    dest[2, 0] /= det;
        //    dest[2, 1] /= det;
        //    dest[2, 2] /= det;

        //    return 1;
        //}
    }
}
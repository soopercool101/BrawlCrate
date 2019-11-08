using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DVector3
    {
        public double X;
        public double Y;
        public double Z;

        public DVector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public unsafe double this[int index]
        {
            get
            {
                fixed (DVector3* p = &this)
                {
                    return ((double*) p)[index];
                }
            }
            set
            {
                fixed (DVector3* p = &this)
                {
                    ((double*) p)[index] = value;
                }
            }
        }
    }
}
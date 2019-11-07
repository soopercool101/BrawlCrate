using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Matrix4d
    {
        public fixed double _data[16];

        public static Vector3 Multiply(double[] matrix, Vector3 vector)
        {
            Vector3 nv = new Vector3
            {
                _x = (float) (matrix[0] * vector._x + matrix[4] * vector._y + matrix[8] * vector._z + matrix[12]),
                _y = (float) (matrix[1] * vector._x + matrix[5] * vector._y + matrix[9] * vector._z + matrix[13]),
                _z = (float) (matrix[2] * vector._x + matrix[6] * vector._y + matrix[10] * vector._z + matrix[14])
            };

            return nv;
        }

        public static void Multiply(Matrix4d* mLeft, Matrix4d* mRight, Matrix4d* mOut)
        {
            double* s1 = mLeft->_data, s2 = mRight->_data;
            double* dPtr = mOut->_data;
            int index = 0;
            double val;

            for (int b = 0; b < 16; b += 4)
            {
                for (int a = 0; a < 4; a++)
                {
                    val = 0.0;
                    for (int x = b, y = a; y < 16; y += 4)
                    {
                        val += s1[x++] * s2[y];
                    }

                    dPtr[index++] = val;
                }
            }
        }
    }
}
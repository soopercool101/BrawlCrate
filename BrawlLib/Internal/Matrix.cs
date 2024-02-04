using BrawlLib.Modeling;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace BrawlLib.Internal
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Matrix : ISerializable
    {
        public static readonly Matrix Identity = ScaleMatrix(1.0f, 1.0f, 1.0f);

        [NonSerialized] private fixed float _values[16];

        public Matrix(SerializationInfo info, StreamingContext ctxt)
        {
            for (int i = 0; i < 16; i++)
            {
                Data[i] = info.GetSingle($"_values[{i}]");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < 16; i++)
            {
                info.AddValue($"_values[{i}]", Data[i]);
            }
        }

        #region Value Management

        public Vector4 Row0
        {
            get => *(Vector4*) &Data[0];
            set => *(Vector4*) &Data[0] = value;
        }

        public Vector4 Row1
        {
            get => *(Vector4*) &Data[4];
            set => *(Vector4*) &Data[4] = value;
        }

        public Vector4 Row2
        {
            get => *(Vector4*) &Data[8];
            set => *(Vector4*) &Data[8] = value;
        }

        public Vector4 Row3
        {
            get => *(Vector4*) &Data[12];
            set => *(Vector4*) &Data[12] = value;
        }

        public Vector4 Col0
        {
            get => new Vector4(Data[0], Data[4], Data[8], Data[12]);
            set
            {
                Data[0] = value._x;
                Data[4] = value._y;
                Data[8] = value._z;
                Data[12] = value._w;
            }
        }

        public Vector4 Col1
        {
            get => new Vector4(Data[1], Data[5], Data[9], Data[13]);
            set
            {
                Data[1] = value._x;
                Data[5] = value._y;
                Data[9] = value._z;
                Data[13] = value._w;
            }
        }

        public Vector4 Col2
        {
            get => new Vector4(Data[2], Data[6], Data[10], Data[14]);
            set
            {
                Data[2] = value._x;
                Data[6] = value._y;
                Data[10] = value._z;
                Data[14] = value._w;
            }
        }

        public Vector4 Col3
        {
            get => new Vector4(Data[3], Data[7], Data[11], Data[15]);
            set
            {
                Data[3] = value._x;
                Data[7] = value._y;
                Data[11] = value._z;
                Data[15] = value._w;
            }
        }

        /// <summary>
        /// Row 1, Column 1
        /// </summary>
        public float M11
        {
            get => Data[0];
            set => Data[0] = value;
        }

        /// <summary>
        /// Row 1, Column 2
        /// </summary>
        public float M12
        {
            get => Data[1];
            set => Data[1] = value;
        }

        /// <summary>
        /// Row 1, Column 3
        /// </summary>
        public float M13
        {
            get => Data[2];
            set => Data[2] = value;
        }

        /// <summary>
        /// Row 1, Column 4
        /// </summary>
        public float M14
        {
            get => Data[3];
            set => Data[3] = value;
        }

        /// <summary>
        /// Row 2, Column 1
        /// </summary>
        public float M21
        {
            get => Data[4];
            set => Data[4] = value;
        }

        /// <summary>
        /// Row 2, Column 2
        /// </summary>
        public float M22
        {
            get => Data[5];
            set => Data[5] = value;
        }

        /// <summary>
        /// Row 2, Column 3
        /// </summary>
        public float M23
        {
            get => Data[6];
            set => Data[6] = value;
        }

        /// <summary>
        /// Row 2, Column 4
        /// </summary>
        public float M24
        {
            get => Data[7];
            set => Data[7] = value;
        }

        /// <summary>
        /// Row 3, Column 1
        /// </summary>
        public float M31
        {
            get => Data[8];
            set => Data[8] = value;
        }

        /// <summary>
        /// Row 3, Column 2
        /// </summary>
        public float M32
        {
            get => Data[9];
            set => Data[9] = value;
        }

        /// <summary>
        /// Row 3, Column 3
        /// </summary>
        public float M33
        {
            get => Data[10];
            set => Data[10] = value;
        }

        /// <summary>
        /// Row 3, Column 4
        /// </summary>
        public float M34
        {
            get => Data[11];
            set => Data[11] = value;
        }

        /// <summary>
        /// Row 4, Column 1
        /// </summary>
        public float M41
        {
            get => Data[12];
            set => Data[12] = value;
        }

        /// <summary>
        /// Row 4, Column 2
        /// </summary>
        public float M42
        {
            get => Data[13];
            set => Data[13] = value;
        }

        /// <summary>
        /// Row 4, Column 3
        /// </summary>
        public float M43
        {
            get => Data[14];
            set => Data[14] = value;
        }

        /// <summary>
        /// Row 4, Column 4
        /// </summary>
        public float M44
        {
            get => Data[15];
            set => Data[15] = value;
        }

        public float* Data
        {
            get
            {
                fixed (float* ptr = _values)
                {
                    return ptr;
                }
            }
        }

        public float this[int x, int y]
        {
            get => Data[(y << 2) + x];
            set => Data[(y << 2) + x] = value;
        }

        public float this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        #endregion

        public Matrix Copy()
        {
            return new Matrix(Data);
        }

        public Matrix(float* values)
        {
            Matrix m = this;
            float* p = (float*) &m;
            for (int i = 0; i < 16; i++)
            {
                p[i] = values[i];
            }
        }

        public Matrix(params float[] values)
        {
            Matrix m = this;
            float* p = (float*) &m;
            for (int i = 0; i < 16; i++)
            {
                p[i] = values[i];
            }
        }

        public Matrix Reverse()
        {
            Matrix m;
            float* pOut = (float*) &m;
            fixed (float* p = _values)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        *pOut++ = p[(x << 2) + y];
                    }
                }
            }

            return m;
        }

        public Vector3 GetPoint()
        {
            fixed (float* p = _values)
            {
                return new Vector3(p[12], p[13], p[14]);
            }
        }

        public Vector3 GetScale()
        {
            fixed (float* p = _values)
            {
                return new Vector3(p[0], p[5], p[10]);
            }
        }

        public static Matrix ScaleMatrix(Vector3 scale)
        {
            return ScaleMatrix(scale._x, scale._y, scale._z);
        }

        public static Matrix ScaleMatrix(float x, float y, float z)
        {
            Matrix m = new Matrix();
            float* p = (float*) &m;
            p[0] = x;
            p[5] = y;
            p[10] = z;
            p[15] = 1.0f;
            return m;
        }

        public static Matrix TranslationMatrix(Vector3 v)
        {
            return TranslationMatrix(v._x, v._y, v._z);
        }

        public static Matrix TranslationMatrix(float x, float y, float z)
        {
            Matrix m = Identity;
            float* p = (float*) &m;
            p[12] = x;
            p[13] = y;
            p[14] = z;
            return m;
        }

        public static Matrix ReverseTranslationMatrix(Vector3 v)
        {
            return ReverseTranslationMatrix(v._x, v._y, v._z);
        }

        public static Matrix ReverseTranslationMatrix(float x, float y, float z)
        {
            Matrix m = Identity;
            float* p = (float*) &m;
            p[12] = -x - y - z;
            p[13] = -x - y - z;
            p[14] = -x - y - z;
            return m;
        }

        public static Matrix InfluenceMatrix(List<BoneWeight> weights)
        {
            Matrix m = new Matrix();
            foreach (BoneWeight w in weights)
            {
                if (w.Bone != null)
                {
                    m += w.Bone.Matrix * w.Bone.InverseBindMatrix * w.Weight;
                }
            }

            return m;
        }

        public static Matrix ReverseInfluenceMatrix(List<BoneWeight> weights)
        {
            Matrix m = new Matrix();
            foreach (BoneWeight w in weights)
            {
                if (w.Bone != null)
                {
                    m += w.Bone.InverseMatrix * w.Bone.BindMatrix * w.Weight;
                }
            }

            return m;
        }

        public static Matrix RotationAboutX(float angle)
        {
            angle *= Maths._deg2radf;

            Matrix m = Identity;
            float* p = (float*) &m;

            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);

            m[5] = cos;
            m[6] = -sin;
            m[9] = sin;
            m[10] = cos;

            return m;
        }

        public static Matrix RotationAboutY(float angle)
        {
            angle *= Maths._deg2radf;

            Matrix m = Identity;
            float* p = (float*) &m;

            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);

            m[0] = cos;
            m[2] = sin;
            m[8] = -sin;
            m[10] = cos;

            return m;
        }

        public static Matrix RotationAboutZ(float angle)
        {
            angle *= Maths._deg2radf;

            Matrix m = Identity;
            float* p = (float*) &m;

            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);

            m[0] = cos;
            m[1] = -sin;
            m[4] = sin;
            m[5] = cos;

            return m;
        }

        public static Matrix RotationMatrix(Vector3 angles)
        {
            return RotationMatrix(angles._x, angles._y, angles._z);
        }

        public static Matrix RotationMatrix(float x, float y, float z)
        {
            float cosx = (float) Math.Cos(x * Maths._deg2radf);
            float sinx = (float) Math.Sin(x * Maths._deg2radf);
            float cosy = (float) Math.Cos(y * Maths._deg2radf);
            float siny = (float) Math.Sin(y * Maths._deg2radf);
            float cosz = (float) Math.Cos(z * Maths._deg2radf);
            float sinz = (float) Math.Sin(z * Maths._deg2radf);

            Matrix m = Identity;
            float* p = (float*) &m;

            m[0] = cosy * cosz;
            m[1] = sinz * cosy;
            m[2] = -siny;
            m[4] = sinx * cosz * siny - cosx * sinz;
            m[5] = sinx * sinz * siny + cosz * cosx;
            m[6] = sinx * cosy;
            m[8] = sinx * sinz + cosx * cosz * siny;
            m[9] = cosx * sinz * siny - sinx * cosz;
            m[10] = cosx * cosy;

            return m;
        }

        public static Matrix ReverseRotationMatrix(Vector3 angles)
        {
            return ReverseRotationMatrix(angles._x, angles._y, angles._z);
        }

        public static Matrix ReverseRotationMatrix(float x, float y, float z)
        {
            float cosx = (float) Math.Cos(x * Maths._deg2radf);
            float sinx = (float) Math.Sin(x * Maths._deg2radf);
            float cosy = (float) Math.Cos(y * Maths._deg2radf);
            float siny = (float) Math.Sin(y * Maths._deg2radf);
            float cosz = (float) Math.Cos(z * Maths._deg2radf);
            float sinz = (float) Math.Sin(z * Maths._deg2radf);

            Matrix m = Identity;
            float* p = (float*) &m;

            p[0] = cosy * cosz;
            p[1] = sinx * siny * cosz - cosx * sinz;
            p[2] = cosx * siny * cosz + sinx * sinz;
            p[4] = cosy * sinz;
            p[5] = sinx * siny * sinz + cosx * cosz;
            p[6] = cosx * siny * sinz - sinx * cosz;
            p[8] = -siny;
            p[9] = sinx * cosy;
            p[10] = cosx * cosy;

            return m;
        }

        public void Translate(Vector3 v)
        {
            Translate(v._x, v._y, v._z);
        }

        public void Translate(float x, float y, float z)
        {
            fixed (float* p = _values)
            {
                p[12] += p[0] * x + p[4] * y + p[8] * z;
                p[13] += p[1] * x + p[5] * y + p[9] * z;
                p[14] += p[2] * x + p[6] * y + p[10] * z;
                p[15] += p[3] * x + p[7] * y + p[11] * z;
            }
        }

        public Vector3 Multiply(Vector3 v)
        {
            Vector3 nv = new Vector3();
            fixed (float* p = _values)
            {
                nv._x = p[0] * v._x + p[4] * v._y + p[8] * v._z + p[12];
                nv._y = p[1] * v._x + p[5] * v._y + p[9] * v._z + p[13];
                nv._z = p[2] * v._x + p[6] * v._y + p[10] * v._z + p[14];
            }

            return nv;
        }

        public Vector3 MultiplyInverse(Vector3 v)
        {
            Vector3 nv = new Vector3();
            fixed (float* p = _values)
            {
                nv._x = p[0] * v._x + p[1] * v._y + p[2] * v._z;
                nv._y = p[4] * v._x + p[5] * v._y + p[6] * v._z;
                nv._z = p[8] * v._x + p[9] * v._y + p[10] * v._z;
            }

            return nv;
        }

        public static Vector2 operator *(Matrix m, Vector2 v)
        {
            Vector2 nv;
            float* p = (float*) &m;
            nv._x = p[0] * v._x + p[4] * v._y + p[8] + p[12];
            nv._y = p[1] * v._x + p[5] * v._y + p[9] + p[13];
            //nv._x = (p[0] * v._x) + (p[4] * v._y) + (p[8] * v._z) + p[12];
            //nv._y = (p[1] * v._x) + (p[5] * v._y) + (p[9] * v._z) + p[13];
            //nv._z = (p[2] * v._x) + (p[6] * v._y) + (p[10] * v._z) + p[14];
            return nv;
        }

        private static Vector3 Transform(Vector3 v, Matrix m)
        {
            Vector3 nv;
            float* p = (float*) &m;
            nv._x = p[0] * v._x + p[4] * v._y + p[8] * v._z + p[12];
            nv._y = p[1] * v._x + p[5] * v._y + p[9] * v._z + p[13];
            nv._z = p[2] * v._x + p[6] * v._y + p[10] * v._z + p[14];
            return nv;
        }

        public static Vector3 operator *(Vector3 v, Matrix m)
        {
            return Transform(v, m);
        }

        public static Vector3 operator *(Matrix m, Vector3 v)
        {
            return Transform(v, m);
        }

        public static Vector4 operator *(Matrix m, Vector4 v)
        {
            Vector4 nv;
            float* dPtr = (float*) &nv;
            float* p0 = (float*) &m, p1 = p0 + 4, p2 = p0 + 8, p3 = p0 + 12;
            for (int i = 0; i < 4; i++)
            {
                dPtr[i] = p0[i] * v._x + p1[i] * v._y + p2[i] * v._z + p3[i] * v._w;
            }

            return nv;
        }

        private void Multiply(float p)
        {
            fixed (float* dPtr = _values)
            {
                for (int i = 0; i < 16; i++)
                {
                    dPtr[i] *= p;
                }
            }
        }

        private void Multiply(Matrix m)
        {
            this *= m;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix dm;
            float* s1 = (float*) &m2, s2 = (float*) &m1, d = (float*) &dm;

            int index = 0;
            float val;
            for (int b = 0; b < 16; b += 4)
            {
                for (int a = 0; a < 4; a++)
                {
                    val = 0.0f;
                    for (int x = b, y = a; y < 16; y += 4)
                    {
                        val += s1[x++] * s2[y];
                    }

                    d[index++] = val;
                }
            }

            return dm;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            float* dPtr = (float*) &m1;
            float* sPtr = (float*) &m2;
            for (int i = 0; i < 16; i++)
            {
                *dPtr++ += *sPtr++;
            }

            return m1;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            float* dPtr = (float*) &m1;
            float* sPtr = (float*) &m2;
            for (int i = 0; i < 16; i++)
            {
                *dPtr++ -= *sPtr++;
            }

            return m1;
        }

        //public static Matrix operator /(Matrix m1, Matrix m2)
        //{
        //    float* dPtr = (float*)&m1;
        //    float* sPtr = (float*)&m2;
        //    for (int i = 0; i < 16; i++)
        //        *dPtr++ /= *sPtr++;
        //    return m1;
        //}
        public static Matrix operator *(Matrix m, float f)
        {
            float* p = (float*) &m;
            for (int i = 0; i < 16; i++)
            {
                *p++ *= f;
            }

            return m;
        }

        public static Matrix operator /(Matrix m, float f)
        {
            float* p = (float*) &m;
            for (int i = 0; i < 16; i++)
            {
                *p++ /= f;
            }

            return m;
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            float* p1 = (float*) &m1;
            float* p2 = (float*) &m2;

            for (int i = 0; i < 16; i++)
            {
                if (*p1++ != *p2++)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            float* p1 = (float*) &m1;
            float* p2 = (float*) &m2;

            for (int i = 0; i < 16; i++)
            {
                if (*p1++ != *p2++)
                {
                    return true;
                }
            }

            return false;
        }

        public static Matrix operator -(Matrix m)
        {
            float* p = (float*) &m;
            int i = 0;
            while (i++ < 16)
            {
                *p = -*p++;
            }

            return m;
        }

        public void RotateX(float x)
        {
            float var1, var2;
            float cosx = (float) Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float) Math.Sin(x / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[4];
                var2 = p[8];
                p[4] = var1 * cosx + var2 * sinx;
                p[8] = var1 * -sinx + var2 * cosx;

                var1 = p[5];
                var2 = p[9];
                p[5] = var1 * cosx + var2 * sinx;
                p[9] = var1 * -sinx + var2 * cosx;

                var1 = p[6];
                var2 = p[10];
                p[6] = var1 * cosx + var2 * sinx;
                p[10] = var1 * -sinx + var2 * cosx;
            }
        }

        public void RotateY(float y)
        {
            float var1, var2;
            float cosy = (float) Math.Cos(y / 180.0f * Math.PI);
            float siny = (float) Math.Sin(y / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[0];
                var2 = p[8];
                p[0] = var1 * cosy + var2 * -siny;
                p[8] = var1 * siny + var2 * cosy;

                var1 = p[1];
                var2 = p[9];
                p[1] = var1 * cosy + var2 * -siny;
                p[9] = var1 * siny + var2 * cosy;

                var1 = p[2];
                var2 = p[10];
                p[2] = var1 * cosy + var2 * -siny;
                p[10] = var1 * siny + var2 * cosy;
            }
        }

        public void RotateZ(float z)
        {
            float var1, var2;
            float cosz = (float) Math.Cos(z / 180.0f * Math.PI);
            float sinz = (float) Math.Sin(z / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[0];
                var2 = p[4];
                p[0] = var1 * cosz + var2 * sinz;
                p[4] = var1 * -sinz + var2 * cosz;

                var1 = p[1];
                var2 = p[5];
                p[1] = var1 * cosz + var2 * sinz;
                p[5] = var1 * -sinz + var2 * cosz;

                var1 = p[2];
                var2 = p[6];
                p[2] = var1 * cosz + var2 * sinz;
                p[6] = var1 * -sinz + var2 * cosz;
            }
        }

        public Matrix GetRotationMatrix()
        {
            Matrix m = Identity;
            float* p = (float*) &m;
            fixed (float* src = _values)
            {
                p[0] = src[0];
                p[1] = src[1];
                p[2] = src[2];

                p[4] = src[4];
                p[5] = src[5];
                p[6] = src[6];

                p[8] = src[8];
                p[9] = src[9];
                p[10] = src[10];
            }

            return m;
        }

        public Vector4 GetQuaternion1()
        {
            Matrix m = this;
            float* p = (float*) &m;

            Vector4 result = new Vector4();

            float scale = p[0] + p[5] + p[10] + 1;
            float sqrt;

            if (scale > 0.0f)
            {
                sqrt = 0.5f / (float) Math.Sqrt(scale);

                result._x = (p[9] - p[6]) * sqrt;
                result._y = (p[2] - p[8]) * sqrt;
                result._z = (p[4] - p[1]) * sqrt;
                result._w = 0.25f / sqrt;

                return result;
            }

            if (p[0] >= p[5] && p[0] >= p[10])
            {
                sqrt = (float) Math.Sqrt(1.0 + p[0] - p[5] - p[10]) * 2;

                result._x = 0.5f / sqrt;
                result._y = (p[1] + p[4]) / sqrt;
                result._z = (p[2] + p[8]) / sqrt;
                result._w = (p[6] + p[9]) / sqrt;

                return result;
            }

            if (p[5] > p[10])
            {
                sqrt = (float) Math.Sqrt(1.0 + p[5] - p[0] - p[10]) * 2;

                result._x = (p[1] + p[4]) / sqrt;
                result._y = 0.5f / sqrt;
                result._z = (p[6] + p[9]) / sqrt;
                result._w = (p[2] + p[8]) / sqrt;

                return result;
            }

            sqrt = (float) Math.Sqrt(1.0 + p[10] - p[0] - p[5]) * 2;

            result._x = (p[2] + p[8]) / sqrt;
            result._y = (p[6] + p[9]) / sqrt;
            result._z = 0.5f / sqrt;
            result._w = (p[1] + p[4]) / sqrt;

            return result;
        }

        public Vector4 GetQuaternion2()
        {
            Vector4 q = new Vector4();

            Matrix m = this;
            float* p = (float*) &m;

            double trace = p[0] + p[5] + p[10] + 1.0;

            if (trace > 1e-7)
            {
                double s = 0.5 / Math.Sqrt(trace);
                q[0] = (float) ((p[9] - p[6]) * s);
                q[1] = (float) ((p[2] - p[8]) * s);
                q[2] = (float) ((p[4] - p[1]) * s);
                q[3] = (float) (0.25 / s);
            }
            else
            {
                if (p[0] > p[5] && p[0] > p[10])
                {
                    double s = 2.0 * Math.Sqrt(1.0 + p[0] - p[5] - p[10]);
                    q[0] = (float) (0.25 * s);
                    q[1] = (float) ((p[1] + p[4]) / s);
                    q[2] = (float) ((p[2] + p[8]) / s);
                    q[3] = (float) ((p[6] - p[9]) / s);
                }
                else if (p[5] > p[10])
                {
                    double s = 2.0 * Math.Sqrt(1.0 + p[5] - p[0] - p[10]);
                    q[0] = (float) ((p[1] + p[4]) / s);
                    q[1] = (float) (0.25 * s);
                    q[2] = (float) ((p[6] + p[9]) / s);
                    q[3] = (float) ((p[2] - p[8]) / s);
                }
                else
                {
                    double s = 2.0 * Math.Sqrt(1.0 + p[10] - p[0] - p[5]);
                    q[0] = (float) ((p[2] + p[8]) / s);
                    q[1] = (float) ((p[6] + p[9]) / s);
                    q[2] = (float) (0.25 * s);
                    q[3] = (float) ((p[1] - p[4]) / s);
                }
            }

            q.Normalize();

            return q * Maths._rad2degf;
        }

        public Vector4 GetQuaternion3()
        {
            Matrix m = this;
            Vector4 q = new Vector4
            {
                _w = (float) Math.Sqrt(Math.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2,
                _x = (float) Math.Sqrt(Math.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2,
                _y = (float) Math.Sqrt(Math.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2,
                _z = (float) Math.Sqrt(Math.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2
            };
            q._x *= Math.Sign(q._x * (m[2, 1] - m[1, 2]));
            q._y *= Math.Sign(q._y * (m[0, 2] - m[2, 0]));
            q._z *= Math.Sign(q._z * (m[1, 0] - m[0, 1]));
            return q;
        }

        //Derive Euler angles from matrix, simply by reversing the transformation process.
        //Vulnerable to gimbal lock, quaternions may be a better solution
        public Vector3 GetAngles()
        {
            float x, y, z, c;
            fixed (float* p = _values)
            {
                y = (float) Math.Asin(-p[2]);
                if (Maths._halfPif - Math.Abs(y) < 0.0001f)
                {
                    //Gimbal lock, occurs when the y rotation falls on pi/2 or -pi/2
                    z = 0.0f;
                    if (y > 0)
                    {
                        x = (float) Math.Atan2(p[4], p[8]);
                    }
                    else
                    {
                        x = (float) Math.Atan2(-p[4], -p[8]);
                    }
                }
                else
                {
                    c = (float) Math.Cos(y);
                    x = (float) Math.Atan2(p[6] / c, p[10] / c);
                    z = (float) Math.Atan2(p[1] / c, p[0] / c);

                    //180 z/x inverts y, use second option
                    if (Maths._pif - Math.Abs(z) < 0.05f)
                    {
                        y = Maths._pif - y;
                        c = (float) Math.Cos(y);
                        x = (float) Math.Atan2(p[6] / c, p[10] / c);
                        z = (float) Math.Atan2(p[1] / c, p[0] / c);
                    }
                }
            }

            return new Vector3(x, y, z) * Maths._rad2degf;
        }

        public FrameState Derive()
        {
            FrameState state = new FrameState();

            fixed (float* p = _values)
            {
                //Translation is easy!
                state._translate = *(Vector3*) &p[12];

                //Scale, use sqrt of rotation columns
                state._scale._x = (float) Math.Round(Math.Sqrt(p[0] * p[0] + p[1] * p[1] + p[2] * p[2]), 4);
                state._scale._y = (float) Math.Round(Math.Sqrt(p[4] * p[4] + p[5] * p[5] + p[6] * p[6]), 4);
                state._scale._z = (float) Math.Round(Math.Sqrt(p[8] * p[8] + p[9] * p[9] + p[10] * p[10]), 4);

                state._rotate = GetAngles();
            }

            state.CalcTransforms();
            return state;
        }

        internal void Scale(Vector3 v)
        {
            Scale(v._x, v._y, v._z);
        }

        internal void Scale(float x, float y, float z)
        {
            Matrix m = ScaleMatrix(x, y, z);
            Multiply(m);
        }

        internal void Rotate(Vector3 v)
        {
            Rotate(v._x, v._y, v._z);
        }

        internal void Rotate(float x, float y, float z)
        {
            Matrix m = RotationMatrix(x, y, z);
            Multiply(m);
        }

        public static explicit operator Matrix(Matrix34 m)
        {
            Matrix m1;
            float* sPtr = (float*) &m;
            float* dPtr = (float*) &m1;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = 0.0f;
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = 0.0f;
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = 0.0f;
            dPtr[12] = sPtr[3];
            dPtr[13] = sPtr[7];
            dPtr[14] = sPtr[11];
            dPtr[15] = 1.0f;

            return m1;
        }

        public static explicit operator Matrix34(Matrix m)
        {
            Matrix34 m1;
            float* sPtr = (float*) &m;
            float* dPtr = (float*) &m1;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = sPtr[12];
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = sPtr[13];
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = sPtr[14];

            return m1;
        }

        public static Matrix OrthographicMatrix(Vector4 dimensions, Vector2 depth)
        {
            return OrthographicMatrix(dimensions, depth._x, depth._y);
        }

        public static Matrix OrthographicMatrix(Vector4 dimensions, float nearZ, float farZ)
        {
            return OrthographicMatrix(dimensions._x, dimensions._y, dimensions._z, dimensions._w, nearZ, farZ);
        }

        public static Matrix OrthographicMatrix(float w, float h, float nearZ, float farZ)
        {
            return OrthographicMatrix(-w / 2, w / 2, h / 2, -h / 2, nearZ, farZ);
        }

        public static Matrix OrthographicMatrix(float left, float right, float top, float bottom, float nearZ,
                                                float farZ)
        {
            Matrix m = Identity;

            float* p = (float*) &m;

            float rl = right - left;
            float tb = top - bottom;
            float fn = farZ - nearZ;

            p[0] = 2.0f / rl;
            p[5] = 2.0f / tb;
            p[10] = -2.0f / fn;

            p[12] = -(right + left) / rl;
            p[13] = -(top + bottom) / tb;
            p[14] = -(farZ + nearZ) / fn;

            return m;
        }

        public static Matrix ReverseOrthographicMatrix(Vector4 dimensions, Vector2 depth)
        {
            return ReverseOrthographicMatrix(dimensions, depth._x, depth._y);
        }

        public static Matrix ReverseOrthographicMatrix(Vector4 dimensions, float nearZ, float farZ)
        {
            return ReverseOrthographicMatrix(dimensions._x, dimensions._y, dimensions._z, dimensions._w, nearZ, farZ);
        }

        public static Matrix ReverseOrthographicMatrix(float w, float h, float nearZ, float farZ)
        {
            return ReverseOrthographicMatrix(-w / 2, w / 2, h / 2, -h / 2, nearZ, farZ);
        }

        public static Matrix ReverseOrthographicMatrix(float left, float right, float top, float bottom, float nearZ,
                                                       float farZ)
        {
            Matrix m = Identity;

            float* p = (float*) &m;

            float rl = right - left;
            float tb = top - bottom;
            float fn = farZ - nearZ;

            p[0] = rl / 2.0f;
            p[5] = tb / 2.0f;
            p[10] = fn / -2.0f;

            p[12] = (right + left) / 2.0f;
            p[13] = (top + bottom) / 2.0f;
            p[14] = (farZ + nearZ) / 2.0f;

            return m;
        }

        public static Matrix PerspectiveMatrix(float fovY, float aspect, float nearZ, float farZ)
        {
            Matrix m;

            float* p = (float*) &m;

            float cotan = (float) (1.0 / Math.Tan(fovY / 2 * Math.PI / 180.0));

            p[0] = cotan / aspect;
            p[5] = cotan;
            p[10] = (farZ + nearZ) / (nearZ - farZ);
            p[11] = -1.0f;
            p[14] = 2.0f * farZ * nearZ / (nearZ - farZ);

            p[1] = p[2] = p[3] = p[4] = p[6] = p[7] = p[8] = p[9] = p[12] = p[13] = p[15] = 0.0f;

            return m;
        }

        public static Matrix ReversePerspectiveMatrix(float fovY, float aspect, float nearZ, float farZ)
        {
            Matrix m;

            float* p = (float*) &m;

            float cotan = (float) (1.0 / Math.Tan(fovY / 2 * Math.PI / 180.0));
            float val = 2.0f * farZ * nearZ / (nearZ - farZ);

            p[0] = aspect / cotan;
            p[5] = 1.0f / cotan;
            p[11] = 1.0f / val;
            p[14] = -1.0f;
            p[15] = (farZ + nearZ) / (nearZ - farZ) / val;

            p[1] = p[2] = p[3] = p[4] = p[6] = p[7] = p[8] = p[9] = p[10] = p[12] = p[13] = 0.0f;

            return m;
        }

        public static Matrix TransformMatrix(Vector3 scale, Vector3 rotate, Vector3 translate)
        {
            Matrix m;
            float* d = (float*) &m;

            float cosx = (float) Math.Cos(rotate._x * Maths._deg2radf);
            float sinx = (float) Math.Sin(rotate._x * Maths._deg2radf);
            float cosy = (float) Math.Cos(rotate._y * Maths._deg2radf);
            float siny = (float) Math.Sin(rotate._y * Maths._deg2radf);
            float cosz = (float) Math.Cos(rotate._z * Maths._deg2radf);
            float sinz = (float) Math.Sin(rotate._z * Maths._deg2radf);

            d[0] = scale._x * cosy * cosz;
            d[1] = scale._x * sinz * cosy;
            d[2] = -scale._x * siny;
            d[3] = 0.0f;
            d[4] = scale._y * (sinx * cosz * siny - cosx * sinz);
            d[5] = scale._y * (sinx * sinz * siny + cosz * cosx);
            d[6] = scale._y * sinx * cosy;
            d[7] = 0.0f;
            d[8] = scale._z * (sinx * sinz + cosx * cosz * siny);
            d[9] = scale._z * (cosx * sinz * siny - sinx * cosz);
            d[10] = scale._z * cosx * cosy;
            d[11] = 0.0f;
            d[12] = translate._x;
            d[13] = translate._y;
            d[14] = translate._z;
            d[15] = 1.0f;

            return m;
        }

        public static Matrix ReverseTransformMatrix(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            float cosx = (float) Math.Cos(rotation._x * Maths._deg2radf);
            float sinx = (float) Math.Sin(rotation._x * Maths._deg2radf);
            float cosy = (float) Math.Cos(rotation._y * Maths._deg2radf);
            float siny = (float) Math.Sin(rotation._y * Maths._deg2radf);
            float cosz = (float) Math.Cos(rotation._z * Maths._deg2radf);
            float sinz = (float) Math.Sin(rotation._z * Maths._deg2radf);

            scale._x = 1 / scale._x;
            scale._y = 1 / scale._y;
            scale._z = 1 / scale._z;
            translation._x = -translation._x;
            translation._y = -translation._y;
            translation._z = -translation._z;

            Matrix m;
            float* p = (float*) &m;

            p[0] = scale._x * cosy * cosz;
            p[1] = scale._y * (sinx * siny * cosz - cosx * sinz);
            p[2] = scale._z * (cosx * siny * cosz + sinx * sinz);
            p[3] = 0.0f;

            p[4] = scale._x * cosy * sinz;
            p[5] = scale._y * (sinx * siny * sinz + cosx * cosz);
            p[6] = scale._z * (cosx * siny * sinz - sinx * cosz);
            p[7] = 0.0f;

            p[8] = -scale._x * siny;
            p[9] = scale._y * sinx * cosy;
            p[10] = scale._z * cosx * cosy;
            p[11] = 0.0f;

            p[12] = translation._x * p[0] + translation._y * p[4] + translation._z * p[8];
            p[13] = translation._x * p[1] + translation._y * p[5] + translation._z * p[9];
            p[14] = translation._x * p[2] + translation._y * p[6] + translation._z * p[10];
            p[15] = 1.0f;

            return m;
        }

        public static Matrix QuaternionTransformMatrix(Vector3 scale, Vector4 rotate, Vector3 translate)
        {
            Matrix m;
            float* p = (float*) &m;

            float xx = rotate._x * rotate._x;
            float yy = rotate._y * rotate._y;
            float zz = rotate._z * rotate._z;
            float xy = rotate._x * rotate._y;
            float zw = rotate._z * rotate._w;
            float zx = rotate._z * rotate._x;
            float yw = rotate._y * rotate._w;
            float yz = rotate._y * rotate._z;
            float xw = rotate._x * rotate._w;

            p[0] = scale._x * (1.0f - 2.0f * (yy + zz));
            p[1] = scale._x * (2.0f * (xy + zw));
            p[2] = scale._x * (2.0f * (zx - yw));
            p[3] = 0.0f;

            p[4] = scale._y * (2.0f * (xy - zw));
            p[5] = scale._y * (1.0f - 2.0f * (zz + xx));
            p[6] = scale._y * (2.0f * (yz + xw));
            p[7] = 0.0f;

            p[8] = scale._z * (2.0f * (zx + yw));
            p[9] = scale._z * (2.0f * (yz - xw));
            p[10] = scale._z * (1.0f - 2.0f * (yy + xx));
            p[11] = 0.0f;

            p[12] = translate._x;
            p[13] = translate._y;
            p[14] = translate._z;
            p[15] = 1.0f;

            return m;
        }

        public static Matrix ReverseQuaternionTransformMatrix(Vector3 scale, Vector4 rotate, Vector3 translate)
        {
            Matrix m;
            float* p = (float*) &m;

            float xx = rotate._x * rotate._x;
            float yy = rotate._y * rotate._y;
            float zz = rotate._z * rotate._z;
            float xy = rotate._x * rotate._y;
            float zw = rotate._z * rotate._w;
            float zx = rotate._z * rotate._x;
            float yw = rotate._y * rotate._w;
            float yz = rotate._y * rotate._z;
            float xw = rotate._x * rotate._w;

            scale._x = 1 / scale._x;
            scale._y = 1 / scale._y;
            scale._z = 1 / scale._z;
            translate._x = -translate._x;
            translate._y = -translate._y;
            translate._z = -translate._z;

            p[0] = scale._x * (1.0f - 2.0f * (yy + zz));
            p[1] = scale._x * (2.0f * (xy + zw));
            p[2] = scale._x * (2.0f * (zx - yw));
            p[3] = 0.0f;

            p[4] = scale._y * (2.0f * (xy - zw));
            p[5] = scale._y * (1.0f - 2.0f * (zz + xx));
            p[6] = scale._y * (2.0f * (yz + xw));
            p[7] = 0.0f;

            p[8] = scale._z * (2.0f * (zx + yw));
            p[9] = scale._z * (2.0f * (yz - xw));
            p[10] = scale._z * (1.0f - 2.0f * (yy + xx));
            p[11] = 0.0f;

            p[12] = translate._x * p[0] + translate._y * p[4] + translate._z * p[8];
            p[13] = translate._x * p[1] + translate._y * p[5] + translate._z * p[9];
            p[14] = translate._x * p[2] + translate._y * p[6] + translate._z * p[10];
            p[15] = 1.0f;

            return m;
        }

        public static Matrix AxisAngleMatrix(Vector3 point1, Vector3 point2)
        {
            Matrix m = Identity;
            //Equal points will cause a corrupt matrix
            if (point1 != point2)
            {
                float* pOut = (float*) &m;

                point1 = point1.Normalize();
                point2 = point2.Normalize();

                Vector3 vSin = point1.Cross(point2);
                Vector3 axis = vSin.Normalize();

                float cos = point1.Dot(point2);
                Vector3 vt = axis * (1.0f - cos);

                pOut[0] = vt._x * axis._x + cos;
                pOut[5] = vt._y * axis._y + cos;
                pOut[10] = vt._z * axis._z + cos;

                vt._x *= axis._y;
                vt._z *= axis._x;
                vt._y *= axis._z;

                pOut[1] = vt._x + vSin._z;
                pOut[2] = vt._z - vSin._y;
                pOut[4] = vt._x - vSin._z;
                pOut[6] = vt._y + vSin._x;
                pOut[8] = vt._z + vSin._y;
                pOut[9] = vt._y - vSin._x;
            }

            return m;
        }

        public static Matrix Lookat(Vector3 eye, Vector3 target, float roll)
        {
            Vector3 up = new Vector3(-(float) Math.Sin(roll * Maths._deg2radf),
                (float) Math.Cos(roll * Maths._deg2radf), 0);

            Vector3 zaxis = (eye - target).Normalize();
            Vector3 xaxis = up.Cross(zaxis).Normalize();
            Vector3 yaxis = zaxis.Cross(xaxis).Normalize();

            Matrix m = Identity;
            float* pOut = (float*) &m;

            pOut[0] = xaxis._x;
            pOut[4] = yaxis._x;
            pOut[8] = zaxis._x;

            pOut[1] = xaxis._y;
            pOut[5] = yaxis._y;
            pOut[9] = zaxis._y;

            pOut[2] = xaxis._z;
            pOut[6] = yaxis._z;
            pOut[10] = zaxis._z;

            pOut[12] = eye._x;
            pOut[13] = eye._y;
            pOut[14] = eye._z;

            return m;
        }

        public static Matrix ReverseLookat(Vector3 eye, Vector3 target, float roll)
        {
            Vector3 up = new Vector3(-(float) Math.Sin(roll * Maths._deg2radf),
                (float) Math.Cos(roll * Maths._deg2radf), 0);

            Vector3 zaxis = (target - eye).Normalize();
            Vector3 xaxis = up.Cross(zaxis).Normalize();
            Vector3 yaxis = xaxis.Cross(zaxis).Normalize();

            Matrix m = Identity;
            float* pOut = (float*) &m;

            pOut[0] = xaxis._x;
            pOut[1] = yaxis._x;
            pOut[2] = zaxis._x;

            pOut[4] = xaxis._y;
            pOut[5] = yaxis._y;
            pOut[6] = zaxis._y;

            pOut[8] = xaxis._z;
            pOut[9] = yaxis._z;
            pOut[10] = zaxis._z;

            eye._x = -eye._x;
            eye._y = -eye._y;
            eye._z = -eye._z;

            pOut[12] = eye._x * pOut[0] + eye._y * pOut[4] + eye._z * pOut[8];
            pOut[13] = eye._x * pOut[1] + eye._y * pOut[5] + eye._z * pOut[9];
            pOut[14] = eye._x * pOut[2] + eye._y * pOut[6] + eye._z * pOut[10];

            return m;
        }

        //From OpenTK
        public Matrix Invert()
        {
            return Invert(this);
        }

        public static Matrix Invert(Matrix mat)
        {
            int[] colIdx = {0, 0, 0, 0};
            int[] rowIdx = {0, 0, 0, 0};
            int[] pivotIdx = {-1, -1, -1, -1};

            // convert the matrix to an array for easy looping
            float[,] inverse =
            {
                {mat.Row0._x, mat.Row0._y, mat.Row0._z, mat.Row0._w},
                {mat.Row1._x, mat.Row1._y, mat.Row1._z, mat.Row1._w},
                {mat.Row2._x, mat.Row2._y, mat.Row2._z, mat.Row2._w},
                {mat.Row3._x, mat.Row3._y, mat.Row3._z, mat.Row3._w}
            };
            int icol = 0;
            int irow = 0;
            for (int i = 0; i < 4; i++)
            {
                // Find the largest pivot value
                float maxPivot = 0.0f;
                for (int j = 0; j < 4; j++)
                {
                    if (pivotIdx[j] != 0)
                    {
                        for (int k = 0; k < 4; ++k)
                        {
                            if (pivotIdx[k] == -1)
                            {
                                float absVal = Math.Abs(inverse[j, k]);
                                if (absVal > maxPivot)
                                {
                                    maxPivot = absVal;
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (pivotIdx[k] > 0)
                            {
                                return mat;
                            }
                        }
                    }
                }

                ++pivotIdx[icol];

                // Swap rows over so pivot is on diagonal
                if (irow != icol)
                {
                    for (int k = 0; k < 4; ++k)
                    {
                        float f = inverse[irow, k];
                        inverse[irow, k] = inverse[icol, k];
                        inverse[icol, k] = f;
                    }
                }

                rowIdx[i] = irow;
                colIdx[i] = icol;

                float pivot = inverse[icol, icol];
                // check for singular matrix
                if (pivot == 0.0f)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                    //return mat;
                }

                // Scale row so it has a unit diagonal
                float oneOverPivot = 1.0f / pivot;
                inverse[icol, icol] = 1.0f;
                for (int k = 0; k < 4; ++k)
                {
                    inverse[icol, k] *= oneOverPivot;
                }

                // Do elimination of non-diagonal elements
                for (int j = 0; j < 4; ++j)
                {
                    // check this isn't on the diagonal
                    if (icol != j)
                    {
                        float f = inverse[j, icol];
                        inverse[j, icol] = 0.0f;
                        for (int k = 0; k < 4; ++k)
                        {
                            inverse[j, k] -= inverse[icol, k] * f;
                        }
                    }
                }
            }

            for (int j = 3; j >= 0; --j)
            {
                int ir = rowIdx[j];
                int ic = colIdx[j];
                for (int k = 0; k < 4; ++k)
                {
                    float f = inverse[k, ir];
                    inverse[k, ir] = inverse[k, ic];
                    inverse[k, ic] = f;
                }
            }

            mat.Row0 = new Vector4(inverse[0, 0], inverse[0, 1], inverse[0, 2], inverse[0, 3]);
            mat.Row1 = new Vector4(inverse[1, 0], inverse[1, 1], inverse[1, 2], inverse[1, 3]);
            mat.Row2 = new Vector4(inverse[2, 0], inverse[2, 1], inverse[2, 2], inverse[2, 3]);
            mat.Row3 = new Vector4(inverse[3, 0], inverse[3, 1], inverse[3, 2], inverse[3, 3]);

            return mat;
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix matrix)
            {
                return matrix == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? $"({this[0]} {this[1]} {this[2]} {this[3]})({this[4]} {this[5]} {this[6]} {this[7]})({this[8]} {this[9]} {this[10]} {this[11]})({this[12]} {this[13]} {this[14]} {this[15]})"
                : $"({this[0]},{this[1]},{this[2]},{this[3]})({this[4]},{this[5]},{this[6]},{this[7]})({this[8]},{this[9]},{this[10]},{this[11]})({this[12]},{this[13]},{this[14]},{this[15]})";
        }
    }
}
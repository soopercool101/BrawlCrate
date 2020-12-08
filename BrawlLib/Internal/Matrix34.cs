using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Animations;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Matrix34
    {
        public static readonly Matrix34 Identity = ScaleMatrix(1.0f, 1.0f, 1.0f);

        internal fixed float _data[12];

        public Matrix34(params float[] values)
        {
            fixed (float* b = _data)
            {
                for (int i = 0; i < Math.Min(12, values.Length); i++)
                {
                    b[i] = values[i];
                }
            }
        }

        public float this[int index]
        {
            get
            {
                fixed (float* p = _data)
                {
                    return p[index];
                }
            }
            set
            {
                fixed (float* p = _data)
                {
                    p[index] = value;
                }
            }
        }

        public void Translate(float x, float y, float z)
        {
            Matrix34 m = TranslationMatrix(x, y, z);
            Multiply(&m);
        }

        internal void Rotate(float x, float y, float z)
        {
            Matrix34 m = RotationMatrix(x, y, z);
            Multiply(&m);
        }

        internal void Scale(float x, float y, float z)
        {
            Matrix34 m = ScaleMatrix(x, y, z);
            Multiply(&m);
        }

        public Matrix34 GetTranslation()
        {
            Matrix34 m = Identity;
            float* p = (float*) &m;
            fixed (float* s = _data)
            {
                p[3] = s[3];
                p[7] = s[7];
                p[11] = s[11];
            }

            return m;
        }

        public static Matrix34 ScaleMatrix(float x, float y, float z)
        {
            Matrix34 m = new Matrix34();
            float* p = (float*) &m;
            p[0] = x;
            p[5] = y;
            p[10] = z;
            return m;
        }

        public static Matrix34 TranslationMatrix(float x, float y, float z)
        {
            Matrix34 m = new Matrix34();
            float* p = (float*) &m;
            p[3] = x;
            p[7] = y;
            p[11] = z;
            p[0] = p[5] = p[10] = 1.0f;
            return m;
        }

        public static Matrix34 RotationMatrixX(float x)
        {
            Matrix34 m = new Matrix34();
            float* p = (float*) &m;

            float cosx = (float) Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float) Math.Sin(x / 180.0f * Math.PI);

            p[0] = 1.0f;
            p[5] = cosx;
            p[6] = -sinx;
            p[9] = sinx;
            p[10] = cosx;

            return m;
        }

        public static Matrix34 RotationMatrixRX(float x)
        {
            Matrix34 m = new Matrix34();
            float* p = (float*) &m;

            float cosx = (float) Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float) Math.Sin(x / 180.0f * Math.PI);

            p[0] = 1.0f;
            p[5] = cosx;
            p[6] = sinx;
            p[9] = -sinx;
            p[10] = cosx;

            return m;
        }

        public static Matrix34 RotationMatrixY(float y)
        {
            Matrix34 m = new Matrix34();
            float* p = (float*) &m;

            float cosy = (float) Math.Cos(y / 180.0f * Math.PI);
            float siny = (float) Math.Sin(y / 180.0f * Math.PI);

            p[5] = 1.0f;

            p[0] = cosy;
            p[2] = siny;
            p[8] = -siny;
            p[10] = cosy;

            return m;
        }

        public static Matrix34 RotationMatrixRY(float y)
        {
            Matrix34 m = new Matrix34();
            float* p = (float*) &m;

            float cosy = (float) Math.Cos(y / 180.0f * Math.PI);
            float siny = (float) Math.Sin(y / 180.0f * Math.PI);

            p[5] = 1.0f;

            p[0] = cosy;
            p[2] = -siny;
            p[8] = siny;
            p[10] = cosy;

            return m;
        }

        public void RotateX(float x)
        {
            float var1, var2;
            float cosx = (float) Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float) Math.Sin(x / 180.0f * Math.PI);

            fixed (float* p = _data)
            {
                var1 = p[1];
                var2 = p[2];
                p[1] = var1 * cosx + var2 * sinx;
                p[2] = var1 * -sinx + var2 * cosx;

                var1 = p[5];
                var2 = p[6];
                p[5] = var1 * cosx + var2 * sinx;
                p[6] = var1 * -sinx + var2 * cosx;

                var1 = p[9];
                var2 = p[10];
                p[9] = var1 * cosx + var2 * sinx;
                p[10] = var1 * -sinx + var2 * cosx;
            }
        }

        public void RotateY(float y)
        {
            float var1, var2;
            float cosy = (float) Math.Cos(y / 180.0f * Math.PI);
            float siny = (float) Math.Sin(y / 180.0f * Math.PI);

            fixed (float* p = _data)
            {
                var1 = p[0];
                var2 = p[2];
                p[0] = var1 * cosy + var2 * -siny;
                p[2] = var1 * siny + var2 * cosy;

                var1 = p[4];
                var2 = p[6];
                p[4] = var1 * cosy + var2 * -siny;
                p[6] = var1 * siny + var2 * cosy;

                var1 = p[8];
                var2 = p[10];
                p[8] = var1 * cosy + var2 * -siny;
                p[10] = var1 * siny + var2 * cosy;
            }
        }

        public void RotateZ(float z)
        {
            float var1, var2;
            float cosz = (float) Math.Cos(z / 180.0f * Math.PI);
            float sinz = (float) Math.Sin(z / 180.0f * Math.PI);

            fixed (float* p = _data)
            {
                var1 = p[0];
                var2 = p[1];
                p[0] = var1 * cosz + var2 * sinz;
                p[1] = var1 * -sinz + var2 * cosz;

                var1 = p[4];
                var2 = p[5];
                p[4] = var1 * cosz + var2 * sinz;
                p[5] = var1 * -sinz + var2 * cosz;

                var1 = p[8];
                var2 = p[9];
                p[8] = var1 * cosz + var2 * sinz;
                p[9] = var1 * -sinz + var2 * cosz;
            }
        }

        public static Matrix34 RotationMatrix(float x, float y, float z)
        {
            float cosx = (float) Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float) Math.Sin(x / 180.0f * Math.PI);
            float cosy = (float) Math.Cos(y / 180.0f * Math.PI);
            float siny = (float) Math.Sin(y / 180.0f * Math.PI);
            float cosz = (float) Math.Cos(z / 180.0f * Math.PI);
            float sinz = (float) Math.Sin(z / 180.0f * Math.PI);

            Matrix34 m = Identity;
            float* p = (float*) &m;

            p[5] = cosx;
            p[6] = -sinx;
            p[9] = sinx;
            p[10] = cosx;

            Matrix34 m2 = Identity;
            float* p2 = (float*) &m2;
            p2[0] = cosy;
            p2[2] = siny;
            p2[8] = -siny;
            p2[10] = cosy;

            Matrix34 m3 = Identity;
            float* p3 = (float*) &m3;
            p3[0] = cosz;
            p3[1] = -sinz;
            p3[4] = sinz;
            p3[5] = cosz;

            m.Multiply(&m2);
            m.Multiply(&m3);

            //p[0] = cosy * cosz;
            //p[1] = cosy * sinz;
            //p[2] = -siny;
            //p[4] = (sinx * siny * cosz - cosx * sinz);
            //p[5] = (sinx * siny * sinz + cosx * cosz);
            //p[6] = sinx * cosy;
            //p[8] = (cosx * siny * cosz + sinx * sinz);
            //p[9] = (cosx * siny * sinz - sinx * cosz);
            //p[10] = cosx * cosy;

            return m;
        }

        public static Matrix34 TransformationMatrix(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            float cosx = (float) Math.Cos(rotation._x / 180.0 * Math.PI);
            float sinx = (float) Math.Sin(rotation._x / 180.0 * Math.PI);
            float cosy = (float) Math.Cos(rotation._y / 180.0 * Math.PI);
            float siny = (float) Math.Sin(rotation._y / 180.0 * Math.PI);
            float cosz = (float) Math.Cos(rotation._z / 180.0 * Math.PI);
            float sinz = (float) Math.Sin(rotation._z / 180.0 * Math.PI);

            Matrix34 m;
            float* p = (float*) &m;

            p[0] = scale._x * cosy * cosz;
            p[1] = scale._y * (sinx * siny * cosz - cosx * sinz);
            p[2] = scale._z * (cosx * siny * cosz + sinx * sinz);
            p[3] = translation._x;
            p[4] = scale._x * cosy * sinz;
            p[5] = scale._y * (sinx * siny * sinz + cosx * cosz);
            p[6] = scale._z * (cosx * siny * sinz - sinx * cosz);
            p[7] = translation._y;
            p[8] = -scale._x * siny;
            p[9] = scale._y * sinx * cosy;
            p[10] = scale._z * cosx * cosy;
            p[11] = translation._z;

            return m;
        }

        public static Matrix34 ReverseTransformMatrix(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            float cosx = (float) Math.Cos(rotation._x / 180.0 * Math.PI);
            float sinx = (float) Math.Sin(rotation._x / 180.0 * Math.PI);
            float cosy = (float) Math.Cos(rotation._y / 180.0 * Math.PI);
            float siny = (float) Math.Sin(rotation._y / 180.0 * Math.PI);
            float cosz = (float) Math.Cos(rotation._z / 180.0 * Math.PI);
            float sinz = (float) Math.Sin(rotation._z / 180.0 * Math.PI);

            scale._x = 1 / scale._x;
            scale._y = 1 / scale._y;
            scale._z = 1 / scale._z;
            translation._x = -translation._x;
            translation._y = -translation._y;
            translation._z = -translation._z;

            Matrix34 m;
            float* p = (float*) &m;

            p[0] = scale._x * cosy * cosz;
            p[1] = scale._x * cosy * sinz;
            p[2] = -scale._x * siny;
            p[3] = translation._x * p[0] + translation._y * p[1] + translation._z * p[2];
            p[4] = scale._y * (sinx * siny * cosz - cosx * sinz);
            p[5] = scale._y * (sinx * siny * sinz + cosx * cosz);
            p[6] = scale._y * sinx * cosy;
            p[7] = translation._x * p[4] + translation._y * p[5] + translation._z * p[6];
            p[8] = scale._z * (cosx * siny * cosz + sinx * sinz);
            p[9] = scale._z * (cosx * siny * sinz - sinx * cosz);
            p[10] = scale._z * cosx * cosy;
            p[11] = translation._x * p[8] + translation._y * p[9] + translation._z * p[10];

            return m;
        }

        private delegate void MtxFunc(float* d, TextureFrameState state);

        private static readonly MtxFunc[] MtxArray =
        {
            BasicMtx,
            MayaMtxSRT,
            MayaMtxRT,
            MayaMtxST,
            MayaMtxT,
            MayaMtxSR,
            MayaMtxR,
            MayaMtxS,
            XSIMtxSRT,
            XSIMtxRT,
            XSIMtxST,
            XSIMtxT,
            XSIMtxSR,
            XSIMtxR,
            XSIMtxS,
            MaxMtxSRT,
            MaxMtxRT,
            MaxMtxST,
            MaxMtxT,
            MaxMtxSR,
            MaxMtxR,
            MaxMtxS
        };

        public static Matrix34 TextureMatrix(TextureFrameState state)
        {
            Matrix34 m = Identity;
            if (state.Flags != 7)
            {
                MtxArray[state.Indirect ? 0 : 1 + ((int) state.MatrixMode).Clamp(0, 2) * 7 + state.Flags]((float*) &m,
                    state);
            }

            return m;
        }

        private static void BasicMtx(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            d[0] = state.Scale._x * cosR;
            d[1] = -state.Scale._y * sinR;
            d[3] = state.Translate._x;
            d[4] = state.Scale._x * sinR;
            d[5] = state.Scale._y * cosR;
            d[7] = state.Translate._y;
        }

        #region Maya Matrices

        private static void MayaMtxS(float* d, TextureFrameState state)
        {
            d[0] = state.Scale._x;
            d[5] = state.Scale._y;
            d[7] = 1.0f - state.Scale._y;
        }

        private static void MayaMtxR(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sin = 0.5f * sinR;
            float cos = 0.5f - 0.5f * cosR;

            d[0] = cosR;
            d[1] = sinR;
            d[3] = cos - sin;
            d[4] = -sinR;
            d[5] = cosR;
            d[6] = cos + sin;
        }

        private static void MayaMtxT(float* d, TextureFrameState state)
        {
            d[3] = -state.Translate._x;
            d[7] = state.Translate._y;
        }

        private static void MayaMtxSR(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sxcr = state.Scale._x * cosR;
            float sxsr = state.Scale._x * sinR;
            float sycr = state.Scale._y * cosR;
            float sysr = state.Scale._y * sinR;

            d[0] = sxcr;
            d[1] = sxsr;
            d[3] = -0.5f * (sxsr + sxcr - state.Scale._x);
            d[4] = -sysr;
            d[5] = sycr;
            d[7] = 0.5f * (sysr - sycr - state.Scale._y) + 1.0f;
        }

        private static void MayaMtxRT(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sin = 0.5f * sinR;
            float cos = 0.5f - 0.5f * cosR;

            d[0] = cosR;
            d[1] = sinR;
            d[3] = cos - sin - state.Translate._x;
            d[4] = -sinR;
            d[5] = cosR;
            d[7] = cos + sin + state.Translate._y;
        }

        private static void MayaMtxST(float* d, TextureFrameState state)
        {
            d[0] = state.Scale._x;
            d[3] = -state.Scale._x * state.Translate._x;
            d[5] = state.Scale._y;
            d[7] = state.Scale._y * (state.Translate._y - 1.0f) + 1.0f;
        }

        private static void MayaMtxSRT(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sin = 0.5f * sinR - 0.5f;
            float cos = -0.5f * cosR;

            d[0] = state.Scale._x * cosR;
            d[1] = state.Scale._x * sinR;
            d[3] = state.Scale._x * (cos - sin - state.Translate._x);
            d[4] = -state.Scale._y * sinR;
            d[5] = state.Scale._y * cosR;
            d[7] = state.Scale._y * (cos + sin + state.Translate._y) + 1.0f;
        }

        #endregion

        #region XSI Matrices

        private static void XSIMtxS(float* d, TextureFrameState state)
        {
            d[0] = state.Scale._x;
            d[5] = state.Scale._y;
            d[7] = 1.0f - state.Scale._y;
        }

        private static void XSIMtxR(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            d[0] = cosR;
            d[1] = -sinR;
            d[3] = sinR;
            d[4] = sinR;
            d[5] = cosR;
            d[7] = 1.0f - cosR;
        }

        private static void XSIMtxT(float* d, TextureFrameState state)
        {
            d[3] = -state.Translate._x;
            d[7] = state.Translate._y;
        }

        private static void XSIMtxSR(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sxcr = state.Scale._x * cosR;
            float sxsr = state.Scale._x * sinR;
            float sycr = state.Scale._y * cosR;
            float sysr = state.Scale._y * sinR;

            d[0] = sxcr;
            d[1] = -sxsr;
            d[3] = sxsr;
            d[4] = sysr;
            d[5] = sycr;
            d[7] = -sycr + 1.0f;
        }

        private static void XSIMtxRT(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            d[0] = cosR;
            d[1] = -sinR;
            d[3] = sinR - cosR * state.Translate._x - sinR * state.Translate._y;
            d[4] = sinR;
            d[5] = cosR;
            d[7] = -cosR - sinR * state.Translate._x + cosR * state.Translate._y + 1.0f;
        }

        private static void XSIMtxST(float* d, TextureFrameState state)
        {
            d[0] = state.Scale._x;
            d[3] = -state.Scale._x * state.Translate._x;
            d[5] = state.Scale._y;
            d[7] = state.Scale._y * (state.Translate._y - 1.0f) + 1.0f;
        }

        private static void XSIMtxSRT(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sxcr = state.Scale._x * cosR;
            float sxsr = state.Scale._x * sinR;
            float sycr = state.Scale._y * cosR;
            float sysr = state.Scale._y * sinR;

            d[0] = sxcr;
            d[1] = -sxsr;
            d[3] = sxsr - sxcr * state.Translate._x - sxsr * state.Translate._y;
            d[4] = sysr;
            d[5] = sycr;
            d[7] = -sycr - sysr * state.Translate._x + sycr * state.Translate._y + 1.0f;
        }

        #endregion

        #region Max Matrices

        private static void MaxMtxS(float* d, TextureFrameState state)
        {
            d[0] = state.Scale._x;
            d[3] = 0.5f * (1.0f - state.Scale._x);
            d[5] = state.Scale._y;
            d[7] = 0.5f * (1.0f - state.Scale._y);
        }

        private static void MaxMtxR(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            d[0] = cosR;
            d[1] = sinR;
            d[3] = -0.5f * (cosR + sinR - 1.0f);
            d[4] = -sinR;
            d[5] = cosR;
            d[7] = -0.5f * (-sinR + cosR - 1.0f);
        }

        private static void MaxMtxT(float* d, TextureFrameState state)
        {
            d[3] = -state.Translate._x;
            d[7] = state.Translate._y;
        }

        private static void MaxMtxSR(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sxcr = state.Scale._x * cosR;
            float sxsr = state.Scale._x * sinR;
            float sycr = state.Scale._y * cosR;
            float sysr = state.Scale._y * sinR;

            d[0] = sxcr;
            d[1] = sxsr;
            d[3] = -0.5f * (sxcr + sxsr - 1.0f);
            d[4] = -sysr;
            d[5] = sycr;
            d[7] = -0.5f * (-sysr + sycr - 1.0f);
        }

        private static void MaxMtxRT(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            d[0] = cosR;
            d[1] = sinR;
            d[3] = -cosR * (state.Translate._x + 0.5f) + sinR * (state.Translate._y - 0.5f) + 0.5f;
            d[4] = -sinR;
            d[5] = cosR;
            d[7] = sinR * (state.Translate._x + 0.5f) + cosR * (state.Translate._y - 0.5f) + 0.5f;
        }

        private static void MaxMtxST(float* d, TextureFrameState state)
        {
            d[0] = state.Scale._x;
            d[3] = -state.Scale._x * (state.Translate._x + 0.5f) + 0.5f;
            d[5] = state.Scale._y;
            d[7] = state.Scale._y * (state.Translate._y - 0.5f) + 0.5f;
        }

        private static void MaxMtxSRT(float* d, TextureFrameState state)
        {
            float sinR = (float) Math.Sin(state.Rotate * Maths._deg2radf);
            float cosR = (float) Math.Cos(state.Rotate * Maths._deg2radf);

            float sxcr = state.Scale._x * cosR;
            float sxsr = state.Scale._x * sinR;
            float sycr = state.Scale._y * cosR;
            float sysr = state.Scale._y * sinR;

            d[0] = sxcr;
            d[1] = sxsr;
            d[2] = 0.0f;
            d[3] = -sxcr * (state.Translate._x + 0.5f) + sxsr * (state.Translate._y - 0.5f) + 0.5f;

            d[4] = -sysr;
            d[5] = sycr;
            d[6] = 0.0f;
            d[7] = sysr * (state.Translate._x + 0.5f) + sycr * (state.Translate._y - 0.5f) + 0.5f;
        }

        #endregion

        public static Matrix34 EnvironmentTexMtx()
        {
            Matrix34 m = Identity;
            m[0] = 0.5f;
            m[3] = 0.5f;
            m[5] = -0.5f;
            m[7] = 0.5f;
            m[10] = 0.0f;
            m[11] = 1.0f;
            return m;
        }

        private static Matrix34 ProjectionTexMtx(GLCamera c)
        {
            if (c.Orthographic)
            {
                return LightMtxOrtho(
                    c._orthoDimensions[2],
                    c._orthoDimensions[3],
                    c._orthoDimensions[0],
                    c._orthoDimensions[1],
                    0.5f, -0.5f, 0.5f, 0.5f);
            }

            return LightMtxPersp(
                c._fovY, 1.0f,
                0.5f, -0.5f, 0.5f, 0.5f);
        }

        private static Matrix34 ProjectionTexMtx(SCN0CameraNode c, float frame)
        {
            CameraAnimationFrame f = c.GetAnimFrame(frame);
            if (c.ProjectionType == Wii.Graphics.ProjectionType.Orthographic)
            {
                return LightMtxOrtho(
                    f.Height / 2.0f,
                    -f.Height / 2.0f,
                    -f.Height * f.Aspect / 2.0f,
                    f.Height * f.Aspect / 2.0f,
                    0.5f, -0.5f, 0.5f, 0.5f);
            }

            return LightMtxPersp(
                f.FovY, 1.0f,
                0.5f, -0.5f, 0.5f, 0.5f);
        }

        private static Matrix34 LightMtxPersp(float fovY, float aspect, float scaleS, float scaleT, float transS,
                                              float transT)
        {
            // find the cotangent of half the (YZ) field of view
            float cot = 1.0f / (float) Math.Tan(Maths._deg2rad * (fovY * 0.5f));
            return new Matrix34(
                cot / aspect * scaleS, 0.0f, -transS, 0.0f,
                0.0f, cot * scaleT, -transT, 0.0f,
                0.0f, 0.0f, -1.0f, 0.0f);
        }

        private static Matrix34 LightMtxOrtho(float t, float b, float l, float r, float scaleS, float scaleT,
                                              float transS, float transT)
        {
            float tmp1 = 1.0f / (r - l), tmp2 = 1.0f / (t - b);
            return new Matrix34(
                2.0f * tmp1 * scaleS, 0.0f, 0.0f, -(r + l) * tmp1 * scaleS + transS,
                0.0f, 2.0f * tmp2 * scaleT, 0.0f, -(t + b) * tmp2 * scaleT + transT,
                0.0f, 0.0f, 0.0f, 1.0f);
        }

        public static Matrix EnvCamMap(int refCam, SCN0Node node, ModelPanelViewport v, float frame)
        {
            GLCamera cam = v.Camera;
            if (refCam >= 0 && node?.CameraGroup != null && refCam < node.CameraGroup.Children.Count)
            {
                SCN0CameraNode camNode = (SCN0CameraNode) node.CameraGroup.Children[refCam];
                camNode.GetModelViewMatrix(frame, out Matrix cm, out Matrix cmInv);
                return (Matrix) EnvironmentTexMtx() * cm.GetRotationMatrix();
            }

            return (Matrix) EnvironmentTexMtx() * v.Camera._matrix.GetRotationMatrix();
        }

        public static Matrix EnvLightMap(int refLight, SCN0Node node, ModelPanelViewport v, float frame)
        {
            Matrix m = Matrix.Identity;
            GLCamera cam = v.Camera;
            Matrix camMtx = cam._matrix;
            Matrix invCamMtx = cam._matrixInverse;

            Matrix34 envMtx = new Matrix34(
                0.5f, 0.0f, 0.0f, 0.5f,
                0.0f, -0.5f, 0.0f, 0.5f,
                0.0f, 0.0f, 0.0f, 1.0f);

            //If no light is referenced, use the BrawlCrate built-in light
            if (refLight < 0 || node?.LightGroup != null && refLight >= node.LightGroup.Children.Count)
            {
                refLight = 0;
                node = null;
            }

            // The light position and direction needs to be transformed with the camera's inverse matrix.
            Vector3 vLook, camUp, vRight, vUp;

            vLook = GetLightLook(node, refLight, invCamMtx, v, frame, out bool specEnabled).Normalize();

            // Calculate without using a target because the margin of error for calculations must be taken into account when the light is far away.
            // Take the absolute value as a measure against transformation margin.
            if (Math.Abs(vLook._x) < 0.000001f &&
                Math.Abs(vLook._z) < 0.000001f)
            {
                camUp._x = camUp._y = 0.0f;
                if (vLook._y <= 0.0f)
                {
                    // Look straight down
                    camUp._z = -1.0f;
                }
                else
                {
                    // Look straight up
                    camUp._z = 1.0f;
                }
            }
            else
            {
                camUp._x = camUp._z = 0.0f;
                camUp._y = 1.0f;
            }

            vUp = (vRight = vLook.Cross(camUp).Normalize()).Cross(vLook);

            Matrix34 m34 = new Matrix34(
                vRight._x, vRight._y, vRight._z, 0.0f,
                vUp._x, vUp._y, vUp._z, 0.0f,
                -vLook._x, -vLook._y, -vLook._z, 0.0f);

            m34 = (Matrix34) ((Matrix) m34 * invCamMtx);

            m34[3] = 0.0f;
            m34[7] = 0.0f;
            m34[11] = 0.0f;

            return (Matrix) (envMtx * m34);

            //return (Matrix)envMtx * Matrix.RotationMatrix(new Vector3().LookatAngles((Vector3)v._posLight) * Maths._rad2degf);
        }

        public static Matrix ProjectionMapping(
            int ref_camera,
            SCN0Node node,
            ModelPanelViewport v,
            float frame)
        {
            Matrix projMtx = Matrix.Identity;
            Matrix camMtx = Matrix.Identity;
            GLCamera cam = v.Camera;
            if (ref_camera >= 0 && node?.CameraGroup != null && ref_camera < node.CameraGroup.Children.Count)
            {
                // Set so that the image is projected from the specified camera.
                // Transform to the viewing coordinate system of the specified camera
                SCN0CameraNode camNode = (SCN0CameraNode) node.CameraGroup.Children[ref_camera];
                camNode.GetModelViewMatrix(frame, out Matrix cm, out Matrix cmInv);
                camMtx = cm * cam._matrix;
                projMtx = (Matrix) ProjectionTexMtx(camNode, frame);
            }
            else
            {
                camMtx = cam._matrix;
                projMtx = (Matrix) ProjectionTexMtx(cam);
            }

            return projMtx * camMtx;
        }

        private static Vector3 GetHalfAngle(Vector3 a, Vector3 b)
        {
            Vector3 aTmp, bTmp, hTmp;

            aTmp._x = -a._x;
            aTmp._y = -a._y;
            aTmp._z = -a._z;

            bTmp._x = -b._x;
            bTmp._y = -b._y;
            bTmp._z = -b._z;

            aTmp.Normalize();
            bTmp.Normalize();

            hTmp = aTmp + bTmp;

            if (hTmp.Dot() > 0.0f)
            {
                return hTmp.Normalize();
            }

            return hTmp;
        }

        /// <summary>
        /// This function returns a texture matrix
        /// that will aim the texture to the midpoint between the active camera
        /// and the given reference camera or light.
        /// </summary>
        public static Matrix EnvSpecMap(
            int refCam,
            int refLight,
            SCN0Node node,
            ModelPanelViewport v,
            float frame)
        {
            // Normal environmental map when neither the light nor the camera is specified.
            Matrix34 finalMtx = EnvironmentTexMtx();

            GLCamera cam = v.Camera;
            Vector3 vLook, camUp, camLook;
            Matrix camMtx = cam._matrixInverse;
            Matrix invCamMtx = cam._matrix;

            Matrix34 m34 = (Matrix34) camMtx;
            camLook._x = -m34[8];
            camLook._y = -m34[9];
            camLook._z = -m34[10];

            if (refLight >= 0)
            {
                Vector3 lgtLook = GetLightLook(node, refLight, invCamMtx, v, frame, out bool specEnabled);

                // Specular light is already set as a vector taking the center position.
                if (!specEnabled)
                {
                    vLook = GetHalfAngle(camLook, lgtLook);
                }
                else
                {
                    vLook = -lgtLook;
                }

                if (Math.Abs(vLook._x) < 0.000001f &&
                    Math.Abs(vLook._z) < 0.000001f)
                {
                    camUp._x = camUp._y = 0.0f;
                    if (vLook._y <= 0.0f)
                    {
                        // Look straight down
                        camUp._z = -1.0f;
                    }
                    else
                    {
                        // Look straight up
                        camUp._z = 1.0f;
                    }
                }
                else
                {
                    camUp._x = camUp._z = 0.0f;
                    camUp._y = 1.0f;
                }
            }
            else if (refCam >= 0)
            {
                SCN0CameraNode camNode = null;

                if (node?.CameraGroup != null && refCam < node.CameraGroup.Children.Count)
                {
                    camNode = (SCN0CameraNode) node.CameraGroup.Children[refCam];
                }
                else
                {
                    camNode = new SCN0CameraNode();
                }

                camNode.GetModelViewMatrix(frame, out Matrix cM, out Matrix cMInv);

                // Map from the midpoint of the view camera and the specified camera.
                Matrix34 lgtCam = (Matrix34) cM;
                camUp._x = lgtCam[4];
                camUp._y = lgtCam[5];
                camUp._z = lgtCam[6];
                Vector3 lgtLook = new Vector3(-lgtCam[8], -lgtCam[9], -lgtCam[10]);

                vLook = GetHalfAngle(camLook, lgtLook);
            }
            else
            {
                return (Matrix) finalMtx;
            }

            vLook.Normalize();
            Vector3 vRight, vUp;

            vUp = (vRight = vLook.Cross(camUp).Normalize()).Cross(vLook);
            m34 = new Matrix34(
                vRight._x, vRight._y, vRight._z, 0.0f,
                vUp._x, vUp._y, vUp._z, 0.0f,
                vLook._x, vLook._y, vLook._z, 0.0f);

            m34 = (Matrix34) ((Matrix) m34 * invCamMtx);
            m34[3] = 0.0f;
            m34[7] = 0.0f;
            m34[11] = 0.0f;

            return (Matrix) (finalMtx * m34);
        }

        private static Vector3 GetLightLook(
            SCN0Node node, int refLight, Matrix invCamMtx, ModelPanelViewport v, float frame, out bool specEnabled)
        {
            Vector3 start, end;
            LightType lightType;

            if (node?.LightGroup != null && refLight < node.LightGroup.Children.Count && refLight >= 0)
            {
                //SCN0 light exists
                SCN0LightNode lightNode = (SCN0LightNode) node.LightGroup.Children[refLight];
                start = lightNode.GetStart(frame);
                end = lightNode.GetEnd(frame);
                lightType = lightNode.LightType;
                specEnabled = lightNode.SpecularEnabled;
            }
            else //Use the model viewer light settings by default
            {
                start = (Vector3) v._posLight;
                end = new Vector3();
                lightType = LightType.Directional;
                specEnabled = true;
            }

            //Don't use if not enabled?
            //bool enabled = lightNode.GetEnabled(frame);

            Vector3 lgtLook = (end - start).Normalize();

            bool temp = lgtLook._x == 0.0f && lgtLook._y == 0.0f && lgtLook._z == 0.0f;
            if (lightType != LightType.Spotlight && !specEnabled || temp)
            {
                // Use light position if they are diffuse light or if light has no direction.
                if (temp)
                {
                    lgtLook = start;
                }

                lgtLook = -(invCamMtx.GetRotationMatrix() * lgtLook);
                if (lgtLook._x == 0.0f &&
                    lgtLook._y == 0.0f &&
                    lgtLook._z == 0.0f)
                {
                    // If the light position is the origin, treat as if light is coming from the top of y-axis.
                    lgtLook._y = -1.0f;
                }
            }
            else
            {
                lgtLook = invCamMtx.GetRotationMatrix() * lgtLook;
            }

            return lgtLook;
        }

        public void Multiply(Matrix34* m)
        {
            Matrix34 m2 = this;

            float* s1 = (float*) &m2, s2 = (float*) m;

            fixed (float* p = _data)
            {
                int index = 0;
                float val;
                for (int b = 0; b < 12; b += 4)
                {
                    for (int a = 0; a < 4; a++)
                    {
                        val = 0.0f;
                        for (int x = b, y = a; y < 12; y += 4)
                        {
                            val += s1[x++] * s2[y];
                        }

                        p[index++] = val;
                    }
                }

                p[3] += s1[3];
                p[7] += s1[7];
                p[11] += s1[11];
            }
        }

        public Vector3 Multiply(Vector3 v)
        {
            Vector3 nv = new Vector3();
            fixed (float* p = _data)
            {
                nv._x = p[0] * v._x + p[1] * v._y + p[2] * v._z + p[3];
                nv._y = p[4] * v._x + p[5] * v._y + p[6] * v._z + p[7];
                nv._z = p[8] * v._x + p[9] * v._y + p[10] * v._z + p[11];
            }

            return nv;
        }

        public void Add(Matrix34* m)
        {
            float* s = (float*) m;
            fixed (float* d = _data)
            {
                for (int i = 0; i < 12; i++)
                {
                    d[i] += s[i];
                }
            }
        }

        public void Subtract(Matrix34* m)
        {
            float* s = (float*) m;
            fixed (float* d = _data)
            {
                for (int i = 0; i < 12; i++)
                {
                    d[i] -= s[i];
                }
            }
        }

        internal void Multiply(float v)
        {
            fixed (float* p = _data)
            {
                for (int i = 0; i < 12; i++)
                {
                    p[i] *= v;
                }
            }
        }

        public static Matrix34 operator *(Matrix34 m1, Matrix34 m2)
        {
            Matrix34 m;

            float* s1 = (float*) &m1, s2 = (float*) &m2, p = (float*) &m;

            int index = 0;
            float val;
            for (int b = 0; b < 12; b += 4)
            {
                for (int a = 0; a < 4; a++)
                {
                    val = 0.0f;
                    for (int x = b, y = a; y < 12; y += 4)
                    {
                        val += s1[x++] * s2[y];
                    }

                    p[index++] = val;
                }
            }

            p[3] += s1[3];
            p[7] += s1[7];
            p[11] += s1[11];

            return m;
        }

        public static bool operator ==(Matrix34 m1, Matrix34 m2)
        {
            float* p1 = (float*) &m1, p2 = (float*) &m2;
            for (int i = 0; i < 12; i++)
            {
                if (*p1++ != *p2++)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix34 m1, Matrix34 m2)
        {
            return !(m1 == m2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix34 matrix34)
            {
                return matrix34 == this;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            fixed (float* p = _data)
            {
                return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                    ? $"({p[0]} {p[1]} {p[2]} {p[3]})({p[4]} {p[5]} {p[6]} {p[7]})({p[8]} {p[9]} {p[10]} {p[11]})"
                    : $"({p[0]},{p[1]},{p[2]},{p[3]})({p[4]},{p[5]},{p[6]},{p[7]})({p[8]},{p[9]},{p[10]},{p[11]})";
            }
        }
    }
}
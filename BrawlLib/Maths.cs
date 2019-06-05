namespace System
{
    public static unsafe class Maths
    {
        private const double _double2fixmagic = 68719476736.0f * 1.5f;
        public const double _rad2deg = 180.0 / Math.PI;

        public const double _deg2rad = Math.PI / 180.0;

        //public const double _rad2deg = 180.0 / _pif;
        //public const double _deg2rad = _pif / 180.0;
        public const float _rad2degf = (float) _rad2deg;
        public const float _deg2radf = (float) _deg2rad;
        public const float _halfPif = (float) (Math.PI / 2.0);

        public const float _pif = (float) Math.PI;
        //public const float _halfPif = (float)(_pif / 2.0);
        //public const float _pif = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930382f;

        public static float CosLawGetSide(float angle, float a, float b)
        {
            return (float) Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(angle * _deg2radf));
        }

        public static float CosLawGetAngle(float a, float b, float c)
        {
            return (float) Math.Acos((a * a + b * b - c * c) / (2 * a * b));
        }

        public static Vector3 RotateAboutPoint(Vector3 point, Vector3 center, Vector3 angles)
        {
            return point * Matrix.TranslationMatrix(-center) * Matrix.RotationMatrix(angles) *
                   Matrix.TranslationMatrix(center);
        }

        public static Vector2 RotateAboutPoint(Vector2 point, Vector2 center, float angle)
        {
            return (Vector2) ((Vector3) point * Matrix.TranslationMatrix((Vector3) (-center)) *
                              Matrix.RotationAboutZ(angle) * Matrix.TranslationMatrix((Vector3) center));
        }

        public static Vector3 ScaleAboutPoint(Vector3 point, Vector3 center, Vector3 scale)
        {
            return point * Matrix.TranslationMatrix(-center) * Matrix.ScaleMatrix(scale) *
                   Matrix.TranslationMatrix(center);
        }

        public static Vector2 ScaleAboutPoint(Vector2 point, Vector2 center, Vector2 scale)
        {
            return (Vector2) ((Vector3) point * Matrix.TranslationMatrix((Vector3) (-center)) *
                              Matrix.ScaleMatrix(scale._x, scale._y, 1.0f) *
                              Matrix.TranslationMatrix((Vector3) center));
        }

        public static Vector3 TransformAboutPoint(Vector3 point, Vector3 center, Matrix transform)
        {
            return point * Matrix.TranslationMatrix(-center) * transform * Matrix.TranslationMatrix(center);
        }

        public static bool LineSphereIntersect(Vector3 start, Vector3 end, Vector3 center, float radius,
            out Vector3 result)
        {
            var diff = end - start;
            var a = diff.Dot();

            if (a > 0.0f)
            {
                var b = 2 * diff.Dot(start - center);
                var c = center.Dot() + start.Dot() - 2 * center.Dot(start) - radius * radius;

                var magnitude = b * b - 4 * a * c;

                if (magnitude >= 0.0f)
                {
                    magnitude = (float) Math.Sqrt(magnitude);
                    a *= 2;

                    var scale = (-b + magnitude) / a;
                    var dist2 = (-b - magnitude) / a;

                    if (dist2 < scale) scale = dist2;

                    result = start + diff * scale;
                    return true;
                }
            }

            result = new Vector3();
            return false;
        }

        public static bool LinePlaneIntersect(Vector3 lineStart, Vector3 lineEnd, Vector3 planePoint,
            Vector3 planeNormal, out Vector3 result)
        {
            var diff = lineEnd - lineStart;
            var scale = -planeNormal.Dot(lineStart - planePoint) / planeNormal.Dot(diff);

            if (float.IsNaN(scale) || scale < 0.0f || scale > 1.0f)
            {
                result = new Vector3();
                return false;
            }

            result = lineStart + diff * scale;
            return true;
        }

        public static Vector3 PointAtLineDistance(Vector3 start, Vector3 end, float distance)
        {
            var diff = end - start;
            return start + diff * (distance / diff.TrueDistance());
        }

        public static Vector3 PointLineIntersect(Vector3 start, Vector3 end, Vector3 point)
        {
            var diff = end - start;
            return start + diff * (diff.Dot(point - start) / diff.Dot());
        }

        public static void FFloor3(float* v)
        {
            double d;
            var p = (int*) &d;
            var i = 3;
            while (i-- > 0)
            {
                d = v[i] + _double2fixmagic;
                v[i] = *p >> 16;
            }
        }

        public static void FMult3(float* l, float* r)
        {
            *l++ *= *r++;
            *l++ *= *r++;
            *l++ *= *r++;
            //for (int i = 3; i-- > 0; )
            //    l[i] *= r[i];
            //return l;
        }

        public static void FMult3(float* l, float r)
        {
            *l++ *= r;
            *l++ *= r;
            *l++ *= r;
            //for (int i = 3; i-- > 0; )
            //    l[i] *= r;
            //return l;
        }

        public static void FAdd3(float* l, float* r)
        {
            *l++ += *r++;
            *l++ += *r++;
            *l++ += *r++;
            //for (int i = 3; i-- > 0; )
            //    l[i] += r[i];
            //return l;
        }

        public static void FAdd3(float* l, float r)
        {
            *l++ += r;
            *l++ += r;
            *l++ += r;
            //for (int i = 3; i-- > 0; )
            //    l[i] += r;
            //return l;
        }

        public static void FSub3(float* l, float* r)
        {
            *l++ -= *r++;
            *l++ -= *r++;
            *l++ -= *r++;
            //for (int i = 3; i-- > 0; )
            //    l[i] -= r[i];
            //return l;
        }

        public static float Power(float value, int amount)
        {
            var i = 0;
            float result = 1;
            for (i = 0; i < amount; i++) result *= value;

            return value;
        }

        public static float Bezier(float p0, float p1, float p2, float p3, float t)
        {
            return
                Power(1 - t, 3) * p0 +
                3 * Power(1 - t, 2) * t * p1 +
                3 * (1 - t) * Power(t, 2) * p2 +
                Power(t, 3) * p3;
        }

        public static float Max(params float[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Max(v, values[i]);

            return v;
        }

        public static int Max(params int[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Max(v, values[i]);

            return v;
        }

        public static uint Max(params uint[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Max(v, values[i]);

            return v;
        }

        public static short Max(params short[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Max(v, values[i]);

            return v;
        }

        public static ushort Max(params ushort[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Max(v, values[i]);

            return v;
        }

        public static byte Max(params byte[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Max(v, values[i]);

            return v;
        }

        public static sbyte Max(params sbyte[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Max(v, values[i]);

            return v;
        }

        public static float Min(params float[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Min(v, values[i]);

            return v;
        }

        public static int Min(params int[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Min(v, values[i]);

            return v;
        }

        public static uint Min(params uint[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Min(v, values[i]);

            return v;
        }

        public static short Min(params short[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Min(v, values[i]);

            return v;
        }

        public static ushort Min(params ushort[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Min(v, values[i]);

            return v;
        }

        public static byte Min(params byte[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Min(v, values[i]);

            return v;
        }

        public static sbyte Min(params sbyte[] values)
        {
            var v = values[0];
            if (values.Length > 1)
                for (var i = 1; i < values.Length; i++)
                    v = Math.Min(v, values[i]);

            return v;
        }
    }
}
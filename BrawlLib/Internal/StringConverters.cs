using BrawlLib.Imaging;
using System;
using System.ComponentModel;
using System.Globalization;

namespace BrawlLib.Internal
{
    public static class LanguageCheck
    {
        public static char[] DecimalDelimiters =>
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",")
                ? new[] {'(', ')', ' '}
                : new[] {',', '(', ')', ' '};
    }

    public class Vector4StringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Vector4);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Vector4 v = new Vector4();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(LanguageCheck.DecimalDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 4)
            {
                float.TryParse(arr[0], out v._x);
                float.TryParse(arr[1], out v._y);
                float.TryParse(arr[2], out v._z);
                float.TryParse(arr[3], out v._w);
            }
            else if (arr.Length == 1 && float.TryParse(arr[0], out float i))
            {
                v._x = i;
                v._y = i;
                v._z = i;
                v._w = i;
            }

            return v;
        }
    }

    public class Vector3StringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Vector3);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Vector3 v = new Vector3();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(LanguageCheck.DecimalDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 3)
            {
                float.TryParse(arr[0], out v._x);
                float.TryParse(arr[1], out v._y);
                float.TryParse(arr[2], out v._z);
            }
            else if (arr.Length == 1 && float.TryParse(arr[0], out float i))
            {
                v._x = i;
                v._y = i;
                v._z = i;
            }

            return v;
        }
    }

    public class Vector2StringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Vector2);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Vector2 v = new Vector2();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(LanguageCheck.DecimalDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 2)
            {
                float.TryParse(arr[0], out v._x);
                float.TryParse(arr[1], out v._y);
            }
            else if (arr.Length == 1 && float.TryParse(arr[0], out float i))
            {
                v._x = i;
                v._y = i;
            }

            return v;
        }
    }

    public class RGBAStringConverter : TypeConverter
    {
        private static readonly char[] Delims = {',', 'R', 'G', 'B', 'A', ':', ' '};

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(RGBAPixel);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            RGBAPixel p = new RGBAPixel();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(Delims, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 4)
            {
                byte.TryParse(arr[0], out p.R);
                byte.TryParse(arr[1], out p.G);
                byte.TryParse(arr[2], out p.B);
                byte.TryParse(arr[3], out p.A);
            }
            else if (arr.Length == 1 && byte.TryParse(arr[0], out byte px))
            {
                p.R = px;
                p.G = px;
                p.B = px;
                p.A = px;
            }

            return p;
        }
    }

    public class GXColorS10StringConverter : TypeConverter
    {
        private static readonly char[] Delims = {',', 'R', 'G', 'B', 'A', ':', ' '};

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(GXColorS10);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            GXColorS10 p = new GXColorS10();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(Delims, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 4)
            {
                short.TryParse(arr[0], out p.R);
                short.TryParse(arr[1], out p.G);
                short.TryParse(arr[2], out p.B);
                short.TryParse(arr[3], out p.A);
            }
            else if (arr.Length == 1 && short.TryParse(arr[0], out short newValue))
            {
                p.R = newValue;
                p.G = newValue;
                p.B = newValue;
                p.A = newValue;
            }

            return p;
        }
    }

    public unsafe class Matrix43StringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Matrix34);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Matrix34 m = new Matrix34();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(LanguageCheck.DecimalDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 12)
            {
                float.TryParse(arr[0], out m._data[0]);
                float.TryParse(arr[1], out m._data[1]);
                float.TryParse(arr[2], out m._data[2]);
                float.TryParse(arr[3], out m._data[3]);
                float.TryParse(arr[4], out m._data[4]);
                float.TryParse(arr[5], out m._data[5]);
                float.TryParse(arr[6], out m._data[6]);
                float.TryParse(arr[7], out m._data[7]);
                float.TryParse(arr[8], out m._data[8]);
                float.TryParse(arr[9], out m._data[9]);
                float.TryParse(arr[10], out m._data[10]);
                float.TryParse(arr[11], out m._data[11]);
            }
            else if (arr.Length == 1 && float.TryParse(arr[0], out float f))
            {
                m._data[0] = f;
                m._data[1] = f;
                m._data[2] = f;
                m._data[3] = f;
                m._data[4] = f;
                m._data[5] = f;
                m._data[6] = f;
                m._data[7] = f;
                m._data[8] = f;
                m._data[9] = f;
                m._data[10] = f;
                m._data[11] = f;
            }

            return m;
        }
    }

    public unsafe class MatrixStringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Matrix);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Matrix m = new Matrix();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(LanguageCheck.DecimalDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 16)
            {
                float.TryParse(arr[0], out m.Data[0]);
                float.TryParse(arr[1], out m.Data[1]);
                float.TryParse(arr[2], out m.Data[2]);
                float.TryParse(arr[3], out m.Data[3]);
                float.TryParse(arr[4], out m.Data[4]);
                float.TryParse(arr[5], out m.Data[5]);
                float.TryParse(arr[6], out m.Data[6]);
                float.TryParse(arr[7], out m.Data[7]);
                float.TryParse(arr[8], out m.Data[8]);
                float.TryParse(arr[9], out m.Data[9]);
                float.TryParse(arr[10], out m.Data[10]);
                float.TryParse(arr[11], out m.Data[11]);
                float.TryParse(arr[12], out m.Data[12]);
                float.TryParse(arr[13], out m.Data[13]);
                float.TryParse(arr[14], out m.Data[14]);
                float.TryParse(arr[15], out m.Data[15]);
            }
            else if (arr.Length == 1 && float.TryParse(arr[0], out float f))
            {
                m.Data[0] = f;
                m.Data[1] = f;
                m.Data[2] = f;
                m.Data[3] = f;
                m.Data[4] = f;
                m.Data[5] = f;
                m.Data[6] = f;
                m.Data[7] = f;
                m.Data[8] = f;
                m.Data[9] = f;
                m.Data[10] = f;
                m.Data[11] = f;
                m.Data[12] = f;
                m.Data[13] = f;
                m.Data[14] = f;
                m.Data[15] = f;
            }

            return m;
        }
    }

    public class QuaternionStringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Vector4);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Vector4 q = new Vector4();

            string s = value?.ToString() ?? "";
            string[] arr = s.Split(LanguageCheck.DecimalDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 4)
            {
                float.TryParse(arr[0], out q._x);
                float.TryParse(arr[1], out q._y);
                float.TryParse(arr[2], out q._z);
                float.TryParse(arr[3], out q._w);
            }
            else if (arr.Length == 1 && float.TryParse(arr[0], out float f))
            {
                q._x = f;
                q._y = f;
                q._z = f;
                q._w = f;
            }

            return q;
        }
    }

    public class Bin8StringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Bin8);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return Bin8.FromString(value?.ToString() ?? "");
        }
    }

    public class Bin16StringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Bin16);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return Bin16.FromString(value?.ToString() ?? "");
        }
    }

    public class Bin32StringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Bin32);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            return value?.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return Bin32.FromString(value?.ToString() ?? "");
        }
    }
}
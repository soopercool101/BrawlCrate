using BrawlLib.SSBB;
using System;
using System.ComponentModel;
using System.Globalization;

namespace BrawlLib.Internal
{
    internal class NullableByteConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string)) {
                switch (value) {
                    case byte _:
                    case sbyte _:
                        return (byte)value == 0xFF ? "" : value.ToString();
                    default:
                        return base.ConvertTo(context, culture, value, destinationType);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return (byte) 255;
                }
                return Convert.ToByte(input);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }


    internal class HexConverterBase : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                switch (value)
                {
                    case uint _:
                    case int _:
                        return $"0x{value:X8}";
                    case ushort _:
                    case short _:
                        return $"0x{value:X4}";
                    case byte _:
                    case sbyte _:
                        return $"0x{value:X2}";
                    default:
                        return base.ConvertTo(context, culture, value, destinationType);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    internal class HexUIntConverter : HexConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                int inputBase = 10;
                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                    inputBase = 16;
                }

                input = input.Trim();

                return Convert.ToUInt32(input, inputBase);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class HexIntConverter : HexConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                int inputBase = 10;
                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                    inputBase = 16;
                }

                input = input.Trim();

                return Convert.ToInt32(input, inputBase);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class HexUShortConverter : HexConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                int inputBase = 10;
                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                    inputBase = 16;
                }

                input = input.Trim();

                return Convert.ToUInt16(input, inputBase);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class HexShortConverter : HexConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                int inputBase = 10;
                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                    inputBase = 16;
                }

                input = input.Trim();

                return Convert.ToInt16(input, inputBase);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class HexByteConverter : HexConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                int inputBase = 10;
                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                    inputBase = 16;
                }

                input = input.Trim();

                return Convert.ToByte(input, inputBase);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class HexOnlyConverterBase : HexConverterBase
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return $"{value:X}";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class HexOnlyByteConverter : HexOnlyConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                return Convert.ToByte(input, 16);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    internal class UserDataConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destType)
        {
            if (destType == typeof(string) && value is UserDataClass u)
            {
                return u.ToString();
            }

            return base.ConvertTo(context, culture, value, destType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
            {
                try
                {
                    string[] s2 = s.Split(':');
                    string[] s3 = s2[1].Split(',');

                    UserDataClass d = new UserDataClass
                    {
                        Name = s2[0]
                    };
                    foreach (string i in s3)
                    {
                        d._entries.Add(i);
                    }

                    return d;
                }
                catch
                {
                    // ignored
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(UserDataClass) || base.CanConvertTo(context, destType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
    }

    internal class ExpandableObjectCustomConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destType)
        {
            string s = (string) base.ConvertTo(context, culture, value, destType);
            return s.Substring(s.LastIndexOf('.') + 1);
        }
    }
}
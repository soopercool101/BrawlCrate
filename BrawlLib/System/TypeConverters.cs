using System.ComponentModel;
using System.Globalization;
using BrawlLib.SSBB.ResourceNodes;

namespace System
{
    internal class UserDataConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destType)
        {
            if (destType == typeof(string) && value is UserDataClass) return ((UserDataClass) value).ToString();

            return base.ConvertTo(context, culture, value, destType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                try
                {
                    var s = (string) value;
                    var s2 = s.Split(':');
                    var s3 = s2[1].Split(',');

                    var d = new UserDataClass
                    {
                        Name = s2[0]
                    };
                    foreach (var i in s3) d._entries.Add(i);

                    return d;
                }
                catch
                {
                }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(UserDataClass) ? true : base.CanConvertTo(context, destType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ? true : base.CanConvertFrom(context, sourceType);
        }
    }

    internal class ExpandableObjectCustomConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destType)
        {
            var s = (string) base.ConvertTo(context, culture, value, destType);
            return s.Substring(s.LastIndexOf('.') + 1);
        }
    }
}
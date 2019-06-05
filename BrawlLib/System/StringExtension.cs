using System.Linq;
using Enumerable = System.Linq.Enumerable;

namespace System
{
    public static class StringExtension
    {
        public static bool Contains(this string source, char value)
        {
            return source.IndexOf(value) >= 0;
        }

        public static bool Contains(this string source, char value, StringComparison comp)
        {
            return source.IndexOf(value.ToString(), comp) >= 0;
        }

        public static bool Contains(this string source, string value, StringComparison comp)
        {
            return source.IndexOf(value, comp) >= 0;
        }

        public static unsafe string TruncateAndFill(this string s, int length, char fillChar)
        {
            var buffer = stackalloc char[length];

            int i;
            var min = Math.Min(s.Length, length);
            for (i = 0; i < min; i++) buffer[i] = s[i];

            while (i < length) buffer[i++] = fillChar;

            return new string(buffer, 0, length);
        }

        public static unsafe int IndexOfOccurance(this string s, char c, int index)
        {
            var len = s.Length;
            fixed (char* cPtr = s)
            {
                for (int i = 0, count = 0; i < len; i++)
                    if (cPtr[i] == c && count++ == index)
                        return i;
            }

            return -1;
        }

        //internal static Encoding encoder = Encoding.GetEncoding(932);
        public static unsafe void Write(this string s, sbyte* ptr)
        {
            //var b = encoder.GetBytes(s);
            for (var i = 0; i < s.Length; i++) ptr[i] = (sbyte) s[i];
        }

        public static unsafe void Write(this string s, ref sbyte* ptr)
        {
            //var b = encoder.GetBytes(s);
            for (var i = 0; i < s.Length; i++) *ptr++ = (sbyte) s[i];

            *ptr++ = 0; //Null terminator
        }

        public static unsafe string Read(this string s, byte* ptr)
        {
            //List<byte> vals = new List<byte>();
            //byte val;
            //while ((val = *ptr++) != 0)
            //    vals.Add(val);
            //return encoder.GetString(vals.ToArray());
            return new string((sbyte*) ptr);
        }

        public static string ToBinaryArray(this string s)
        {
            //string value = "";
            //for (int i = 0; i < s.Length; i++)
            //{
            //    byte c = (byte)s[i];
            //    for (int x = 0; x < 8; x++)
            //        value += ((c >> (7 - x)) & 1);
            //}
            //return value;
            var result = string.Empty;
            foreach (var ch in s) result += Convert.ToString(ch, 2);

            return result.PadLeft(result.Length.Align(8), '0');
        }

        public static int CompareBits(this string t1, string t2)
        {
            var bit = 0;
            var found = false;
            var min = Math.Min(t1.Length, t2.Length);
            for (var i = 0; i < min; i++)
            {
                var c1 = (byte) t1[i];
                var c2 = (byte) t2[i];

                for (var x = 0; x < 8; x++)
                    if (c1 >> (7 - x) != c2 >> (7 - x))
                    {
                        bit = i * 8 + x;
                        found = true;
                        break;
                    }

                if (bit != 0) break;
            }

            if (!found) bit = min * 8 + 1;

            return bit;
        }

        public static bool AtBit(this string s, int bitIndex)
        {
            var bit = bitIndex % 8;
            var byteIndex = (bitIndex - bit) / 8;
            return ((s[byteIndex] >> (7 - bit)) & 1) != 0;
        }

        public static string RemoveInvalidCharacters(this string s, string valid)
        {
            var m = "";
            var t = Enumerable.Where(s.ToCharArray(), x => valid.Contains(x)).ToArray();
            foreach (var c in t) m += c;

            return m;
        }
    }
}
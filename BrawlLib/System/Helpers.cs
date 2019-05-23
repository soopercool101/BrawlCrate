using System.Collections.Generic;

namespace System
{
    public static class Helpers
    {
        public static string TruncateLeft(string strIn, int totalWidth)
        {
            if (totalWidth <= 0) return "";
            if (totalWidth > strIn.Length) return strIn;
            return strIn.Substring(strIn.Length - totalWidth);
        }

        public static string Hex(int val) { return TruncateLeft(val.ToString("X"), 8); }
        public static string Hex(long val) { return TruncateLeft(val.ToString("X"), 8); }
        public static string Hex8(int val) { return TruncateLeft(val.ToString("X"), 8).PadLeft(8, '0'); }
        public static string Hex8(long val) { return TruncateLeft(val.ToString("X"), 8).PadLeft(8, '0'); }
        public static int UnHex(string val) { return int.Parse(val, System.Globalization.NumberStyles.HexNumber); }

        public static string WordH(string val, int wordNum)
        {
            if (wordNum >= 2) return "";
            return TruncateLeft(val, 8).PadLeft(8, '0').Substring(wordNum * 4, 4);
        }

        public static string WordB(string val, int byteNum)
        {
            if (byteNum >= 4) return "";
            return TruncateLeft(val, 8).PadLeft(8, '0').Substring(byteNum * 2, 2);
        }

        public static long Float(float val)
        {
            if (val == 0) return 0;
            long sign = (val >= 0 ? 0 : 8);
            long exponent = 0x7F;
            float mantissa = Math.Abs(val);

            if (mantissa > 1)
                while (mantissa > 2)
                { mantissa /= 2; exponent++; }
            else
                while (mantissa < 1)
                { mantissa *= 2; exponent--; }
            mantissa -= 1;
            mantissa *= (float)Math.Pow(2, 23);

            return (
                  sign * 0x10000000
                + exponent * 0x800000
                + (long)mantissa);
        }
        public static float UnFloat(long val)
        {
            if (val == 0) return 0;
            float sign = ((val & 0x80000000) == 0 ? 1 : -1);
            int exponent = ((int)(val & 0x7F800000) / 0x800000) - 0x7F;
            float mantissa = (val & 0x7FFFFF);
            long mantissaBits = 23;

            if (mantissa != 0)
                while (((long)mantissa & 0x1) != 1)
                { mantissa /= 2; mantissaBits--; }
            mantissa /= (float)Math.Pow(2, mantissaBits);
            mantissa += 1;

            mantissa *= (float)Math.Pow(2, exponent);
            return mantissa *= sign;
        }

        public static long RoundUp(long val, long factor) { return val + (factor - 1) - (val + (factor - 1)) % factor; }
        public static long RoundDown(long val, long factor) { return val - val % factor; }

        //Find the first occuring instance of the passed character.
        public static int FindFirst(string str, int begin, char chr)
        {
            for (int i = begin; i < str.Length; i++)
                if (str[i] == chr) return i;
            return -1;
        }

        //Find the last occuring instance of the passed character.
        public static int FindLast(string str, int begin, char chr)
        {
            for (int i = str.Length - 1; i >= begin; i--)
                if (str[i] == chr) return i;
            return -1;
        }

        //Find the number of instances of the passed character.
        public static int FindCount(string str, int begin, char chr)
        {
            int x = 0;
            for (int i = begin; i < str.Length; i++)
                if (str[i] == chr) x++;
            return x;
        }

        //Find the indices of instances of the passed character.
        public static int[] IndiciesOf(string str, int begin, char chr)
        {
            List<int> indices = new List<int>();
            for (int i = begin; i < str.Length; i++)
                if (str[i] == chr) indices.Add(i);
            return indices.ToArray();
        }

        //Find the first occuring instance of any of the passed characters.
        public static int FindFirstOf(string str, int begin, char[] chr, ref char chrFound)
        {
            int result = -1;
            chrFound = '\0';
            for (int i = 0; i < chr.Length; i++)
            {
                int temp = FindFirst(str, begin, chr[i]);

                if (temp != -1 && (temp < result || result == -1))
                {
                    result = temp;
                    chrFound = chr[i];
                }
            }
            return result;
        }

        //FindFirst ignoring any pairs of () and anything contained inside.
        public static int FindFirstIgnoreNest(string str, int begin, char chr)
        {
            if (chr == '(') return FindFirst(str, begin, chr);
            char[] searchCharacters = { chr, '(', ')' };
            char chrResult = '\0';
            int searchResult = begin;
            int nested = 0;

            do
            {
                if (chrResult == ')' && nested > 0) nested--;
                searchResult = FindFirstOf(str, searchResult, searchCharacters, ref chrResult);
                if (chrResult == '(') nested++;
                searchResult++;
            } while ((nested > 0 || chrResult != chr) && chrResult != '\0');
            searchResult--;

            return searchResult;
        }

        //FindFirstOf ignoring any paris of () and anything contained inside.
        public static int FindFirstOfIgnoreNest(string str, int begin, char[] chr, ref char chrFound)
        {
            int result = -1;
            chrFound = '\0';
            for (int i = 0; i < chr.Length; i++)
            {
                int temp = FindFirstIgnoreNest(str, begin, chr[i]);

                if (temp != -1 && (temp < result || result == -1))
                {
                    result = temp;
                    chrFound = chr[i];
                }
            }
            return result;
        }

        //Find the first instance that is not the character passed.
        public static int FindFirstNot(string str, int begin, char chr)
        {
            for (int i = begin; i < str.Length; i++)
                if (str[i] != chr) return i;

            return -1;
        }

        //Find the first instance that is not the character passed.
        public static int FindFirstNotDual(string str, int begin, char chr1, char chr2)
        {
            for (int i = begin; i < str.Length; i++)
                if (str[i] != chr1 && str[i] != chr2) return i;

            return -1;
        }

        //Return the string passed with the new string insterted into the specified position.
        public static string InsString(string str, string insString, int begin)
        {
            return str.Substring(0, begin) + insString + str.Substring(begin);
        }

        //Return the string passed minus the substring specified.
        public static string DelSubstring(string str, int begin, int len)
        {
            return str.Substring(0, begin) + str.Substring(begin + len);
        }

        //Delete any whitespace before and after the string.
        public static string ClearWhiteSpace(string str)
        {
            int whiteSpace = FindFirstNot(str, 0, ' ');
            if (whiteSpace > 0)
                str = DelSubstring(str, 0, whiteSpace);

            whiteSpace = -1;
            for (int i = str.Length - 1; i >= 0; i--)
                if (str[i] == ' ') whiteSpace = i;
                else break;
            if (whiteSpace != -1)
                str = DelSubstring(str, whiteSpace, str.Length - whiteSpace);

            return str;
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<Object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }

        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }
    public class SectionParamInfo
    {
        public string _newName;
        public List<AttributeInfo> _attributes;
    }
}

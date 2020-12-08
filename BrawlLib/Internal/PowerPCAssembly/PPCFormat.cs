using System;

namespace BrawlLib.Internal.PowerPCAssembly
{
    public enum DisplayType
    {
        Decimal,
        UnsignedHex,
        Hex
    }

    public class PPCFormat
    {
        public static DisplayType disassemblerDisplay = DisplayType.Decimal;

        private PPCFormat()
        {
        }

        public static string Hex(int val)
        {
            return val.ToString("X");
        }

        public static string Hex(uint val)
        {
            return Hex((int) val);
        }

        public static string Half(int val)
        {
            return Half(Hex(val));
        }

        public static string Half(uint val)
        {
            return Half(Hex(val));
        }

        public static string Half(string val)
        {
            return val.Substring(val.Length > 4 ? val.Length - 4 : 0);
        }

        public static string SHalf(int val)
        {
            return (val < 0 ? "-" : "") + Half(Math.Abs(val));
        }

        public static string Word(int val)
        {
            return Word(Hex(val));
        }

        public static string Word(uint val)
        {
            return Word(Hex(val));
        }

        public static string Word(string val)
        {
            return val.Substring(val.Length > 8 ? val.Length - 8 : 0);
        }

        public static string SWord(int val)
        {
            return (val < 0 ? "-" : "") + Word(Math.Abs(val));
        }

        public static string Offset(int val)
        {
            return Offset(Hex(val));
        }

        public static string Offset(uint val)
        {
            return Offset(Hex(val));
        }

        public static string Offset(string val)
        {
            return "0x" + Word(val);
        }

        public static string SOffset(int val)
        {
            return (val < 0 ? "-" : "") + "0x" + Word(Math.Abs(val));
        }
    }
}
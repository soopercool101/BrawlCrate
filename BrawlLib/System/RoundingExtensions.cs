namespace System
{
    public static class RoundingExtensions
    {
        public static byte RoundUp(this byte value, int factor)
        {
            if (factor <= 0) return value;
            return (byte)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }
        public static byte RoundDown(this byte value, int factor)
        {
            if (factor <= 0) return value;
            return (byte)(value - value % factor);
        }

        public static UInt16 RoundUp(this UInt16 value, int factor)
        {
            if (factor <= 0) return value;
            return (ushort)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }

        public static UInt16 RoundDown(this UInt16 value, int factor)
        {
            if (factor <= 0) return value;
            return (ushort)(value - value % factor);
        }

        public static UInt32 RoundUp(this UInt32 value, int factor)
        {
            if (factor <= 0) return value;
            return (uint)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }

        public static UInt32 RoundDown(this UInt32 value, int factor)
        {
            if (factor <= 0) return value;
            return (uint)(value - value % factor);
        }

        public static Int16 RoundUp(this Int16 value, int factor)
        {
            if (factor <= 0) return value;
            return (short)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }
        public static Int16 RoundDown(this Int16 value, int factor)
        {
            if (factor <= 0) return value;
            return (short)(value - value % factor);
        }

        public static Int32 RoundUp(this Int32 value, int factor)
        {
            if (factor <= 0) return value;
            return (int)((value + (factor - 1)) - ((value + (factor - 1)) % factor));
        }

        public static Int32 RoundDown(this Int32 value, int factor)
        {
            if (factor <= 0) return value;
            return (int)(value - value % factor);
        }

        public static Int64 RoundUp(this Int64 value, int factor)
        {
            if (factor <= 0) return value;
            return (long)((value + (factor - 1)) - (value + (factor - 1)) % factor);
        }

        public static Int64 RoundDown(this Int64 value, int factor)
        {
            if (factor <= 0) return value;
            return (long)(value - value % factor);
        }
    }
}
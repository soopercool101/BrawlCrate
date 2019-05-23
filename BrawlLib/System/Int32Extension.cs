namespace System
{
    public static class Int32Extension
    {
        public static unsafe Int32 Reverse(this Int32 value)
        {
            return ((value >> 24) & 0xFF) | (value << 24) | ((value >> 8) & 0xFF00) | ((value & 0xFF00) << 8);
        }
        public static Int32 Align(this Int32 value, int align)
        {
            if (align == 0) return value;
            return (value + align - 1) / align * align;
        }
        public static Int32 Clamp(this Int32 value, int min, int max)
        {
            return value <= min ? min : value >= max ? max : value;
        }
        public static Int32 ClampMin(this Int32 value, int min)
        {
            return value <= min ? min : value;
        }
        public static Int32 RoundDownToEven(this Int32 value)
        {
            return value - (value % 2);
        }
        public static Int32 RoundUpToEven(this Int32 value)
        {
            return value + (value % 2);
        }
    }
}

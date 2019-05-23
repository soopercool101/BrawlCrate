namespace System
{
    public static class UInt32Extension
    {
        public static UInt32 Reverse(this UInt32 value)
        {
            return ((value >> 24) & 0xFF) | (value << 24) | ((value >> 8) & 0xFF00) | ((value & 0xFF00) << 8);
        }
        public static UInt32 Align(this UInt32 value, uint align)
        {
            if (align <= 1) return value;
            uint temp = value % align;
            if (temp != 0) value += align - temp;
            return value;
        }
        public static UInt32 Clamp(this UInt32 value, uint min, uint max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}

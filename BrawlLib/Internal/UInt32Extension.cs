namespace BrawlLib.Internal
{
    public static class UInt32Extension
    {
        public static uint Reverse(this uint value)
        {
            return ((value >> 24) & 0xFF) | (value << 24) | ((value >> 8) & 0xFF00) | ((value & 0xFF00) << 8);
        }

        public static uint Align(this uint value, uint align)
        {
            if (align <= 1)
            {
                return value;
            }

            uint temp = value % align;
            if (temp != 0)
            {
                value += align - temp;
            }

            return value;
        }

        public static uint Clamp(this uint value, uint min, uint max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}
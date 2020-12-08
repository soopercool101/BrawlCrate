namespace BrawlLib.Internal
{
    public static class Int64Extension
    {
        public static long Reverse(this long value)
        {
            return
                ((value >> 56) & 0xFF) | ((value & 0xFF) << 56) |
                ((value >> 40) & 0xFF00) | ((value & 0xFF00) << 40) |
                ((value >> 24) & 0xFF0000) | ((value & 0xFF0000) << 24) |
                ((value >> 8) & 0xFF000000) | ((value & 0xFF000000) << 8);
        }

        public static long Align(this long value, int align)
        {
            if (value < 0)
            {
                return 0;
            }

            if (align <= 1)
            {
                return value;
            }

            long temp = value % align;
            if (temp != 0)
            {
                value += align - temp;
            }

            return value;
        }

        public static long Clamp(this long value, long min, long max)
        {
            if (value <= min)
            {
                return min;
            }

            if (value >= max)
            {
                return max;
            }

            return value;
        }
    }
}
namespace BrawlLib.Internal
{
    public static class RoundingExtensions
    {
        public static byte RoundUp(this byte value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (byte) (value + (factor - 1) - (value + (factor - 1)) % factor);
        }

        public static byte RoundDown(this byte value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (byte) (value - value % factor);
        }

        public static ushort RoundUp(this ushort value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (ushort) (value + (factor - 1) - (value + (factor - 1)) % factor);
        }

        public static ushort RoundDown(this ushort value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (ushort) (value - value % factor);
        }

        public static uint RoundUp(this uint value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (uint) (value + (factor - 1) - (value + (factor - 1)) % factor);
        }

        public static uint RoundDown(this uint value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (uint) (value - value % factor);
        }

        public static short RoundUp(this short value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (short) (value + (factor - 1) - (value + (factor - 1)) % factor);
        }

        public static short RoundDown(this short value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return (short) (value - value % factor);
        }

        public static int RoundUp(this int value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return value + (factor - 1) - (value + (factor - 1)) % factor;
        }

        public static int RoundDown(this int value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return value - value % factor;
        }

        public static long RoundUp(this long value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return value + (factor - 1) - (value + (factor - 1)) % factor;
        }

        public static long RoundDown(this long value, int factor)
        {
            if (factor <= 0)
            {
                return value;
            }

            return value - value % factor;
        }
    }
}
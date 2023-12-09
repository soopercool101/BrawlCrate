namespace BrawlLib.Internal
{
    public static class ByteExtension
    {
        public static byte GetUpper(this byte b)
        {
            return (byte) ((b & 0xF0) / 0x10);
        }

        public static void SetUpper(ref this byte b, byte value)
        {
            b = (byte)(value * 0x10 + b.GetLower());
        }

        public static byte GetLower(this byte b)
        {
            return (byte) (b & 0x0F);
        }

        public static void SetLower(ref this byte b, byte value)
        {
            b = (byte)(value + b.GetUpper() * 0x10);
        }

        public static int CompareBits(this byte b1, byte b2)
        {
            for (int i = 8, b = 0x80; i-- != 0; b >>= 1)
            {
                if ((b1 & b) != (b2 & b))
                {
                    return i;
                }
            }

            return 0;
        }

        public static int CountBits(this byte b)
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                if (((b >> i) & 1) != 0)
                {
                    count++;
                }
            }

            return count;
        }

        public static byte Clamp(this byte value, byte min, byte max)
        {
            return value <= min ? min : value >= max ? max : value;
        }

        public static sbyte Clamp(this sbyte value, sbyte min, sbyte max)
        {
            return value <= min ? min : value >= max ? max : value;
        }
    }
}
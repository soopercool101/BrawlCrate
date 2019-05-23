namespace System
{
    public static class ByteExtension
    {
        public static int CompareBits(this Byte b1, byte b2)
        {
            for (int i = 8, b = 0x80; i-- != 0; b >>= 1)
                if ((b1 & b) != (b2 & b))
                    return i;
            return 0;
        }
        public static int CountBits(this Byte b)
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
                if (((b >> i) & 1) != 0)
                    count++;
            return count;
        }
        public static byte Clamp(this byte value, byte min, byte max)
        {
            return value <= min ? min : value >= max ? max : value;
        }
    }
}

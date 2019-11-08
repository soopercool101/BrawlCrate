namespace BrawlLib.Internal
{
    public static class Int16Extension
    {
        public static short Reverse(this short value)
        {
            return (short) (((value >> 8) & 0xFF) | (value << 8));
        }
    }
}
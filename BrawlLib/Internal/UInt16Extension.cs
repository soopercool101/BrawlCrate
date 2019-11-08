namespace BrawlLib.Internal
{
    public static class UInt16Extension
    {
        public static ushort Reverse(this ushort value)
        {
            return (ushort) ((value >> 8) | (value << 8));
        }
    }
}
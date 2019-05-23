namespace System
{
    public static class UInt16Extension
    {
        public static UInt16 Reverse(this UInt16 value)
        {
            return (ushort)((value >> 8) | (value << 8));
        }
    }
}

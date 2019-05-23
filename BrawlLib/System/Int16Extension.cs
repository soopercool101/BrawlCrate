namespace System
{
    public static class Int16Extension
    {
        public static Int16 Reverse(this Int16 value)
        {
            return (short)(((value >> 8) & 0xFF) | (value << 8));
        }
    }
}

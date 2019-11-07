namespace BrawlLib.Internal
{
    public static unsafe class DoubleExtension
    {
        private static readonly float[] _quantTable = new float[]
        {
            1f, 2f, 4f, 8f, 16f, 32f, 64f, 128f, 256f, 512f, 1024f, 2048f, 4096f, 8192f, 16384f, 32768f,
            65536f, 131072f, 262144f, 524288f, 1048576f, 2097152f, 4194304f, 8388608f, 16777216f, 33554432f, 67108864f,
            134217728f, 268435456f, 536870912f, 1073741824f, 2147483648f, 4294967296f
        };

        public static float Unquantize(byte* data, bool pair, int scale)
        {
            float value;

            float scaleVal = scale < 32 ? 1.0f / _quantTable[scale] : _quantTable[64 - scale];

            value = data[0] * scaleVal;

            return value;
        }

        public static double Clamp(this double value, double min, double max)
        {
            return value <= min ? min : value >= max ? max : value;
        }
    }
}
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBGCEntry // TBGC
    {
        public const int Size = 0x8;

        public byte _cameo1;
        public byte _cameo2;
        public byte _cameo3;
        public byte _cameo4;
        public byte _cameo5;
        public byte _cameo6;
        public byte _cameo7;
        public byte _cameo8;
    }
}
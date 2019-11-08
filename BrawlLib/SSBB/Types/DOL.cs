using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct DOLHeader
    {
        public const int Size = 0x100;

        public buint Text0Offset;
        public buint Text1Offset;
        public buint Text2Offset;
        public buint Text3Offset;
        public buint Text4Offset;
        public buint Text5Offset;
        public buint Text6Offset;

        public buint Data0Offset;
        public buint Data1Offset;
        public buint Data2Offset;
        public buint Data3Offset;
        public buint Data4Offset;
        public buint Data5Offset;
        public buint Data6Offset;
        public buint Data7Offset;
        public buint Data8Offset;
        public buint Data9Offset;
        public buint Data10Offset;

        public buint Text0LoadAddr;
        public buint Text1LoadAddr;
        public buint Text2LoadAddr;
        public buint Text3LoadAddr;
        public buint Text4LoadAddr;
        public buint Text5LoadAddr;
        public buint Text6LoadAddr;

        public buint Data0LoadAddr;
        public buint Data1LoadAddr;
        public buint Data2LoadAddr;
        public buint Data3LoadAddr;
        public buint Data4LoadAddr;
        public buint Data5LoadAddr;
        public buint Data6LoadAddr;
        public buint Data7LoadAddr;
        public buint Data8LoadAddr;
        public buint Data9LoadAddr;
        public buint Data10LoadAddr;

        public buint Text0Size;
        public buint Text1Size;
        public buint Text2Size;
        public buint Text3Size;
        public buint Text4Size;
        public buint Text5Size;
        public buint Text6Size;

        public buint Data0Size;
        public buint Data1Size;
        public buint Data2Size;
        public buint Data3Size;
        public buint Data4Size;
        public buint Data5Size;
        public buint Data6Size;
        public buint Data7Size;
        public buint Data8Size;
        public buint Data9Size;
        public buint Data10Size;

        public buint bssOffset;
        public buint bssSize;
        public buint entryPoint;
        public fixed byte padding[28];

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public buint* TextOffset => (buint*) Address;
        public buint* DataOffset => (buint*) Address + 7;
        public buint* TextLoadAddr => (buint*) Address + 18;
        public buint* DataLoadAddr => (buint*) Address + 25;
        public buint* TextSize => (buint*) Address + 36;
        public buint* DataSize => (buint*) Address + 43;

        public uint GetSize()
        {
            buint* offsets = TextOffset;
            buint* sizes = TextSize;
            uint maxLen = 0;
            for (int i = 0; i < 18; ++i)
            {
                maxLen = Math.Max(offsets[i] + sizes[i], maxLen);
            }

            return maxLen + 0x100;
        }
    }
}
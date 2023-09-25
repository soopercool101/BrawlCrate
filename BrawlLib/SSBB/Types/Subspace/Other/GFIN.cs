using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GFINEntry
    {
        public const int Size = 0x04;

        public byte _backgroundModelData;
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }
}
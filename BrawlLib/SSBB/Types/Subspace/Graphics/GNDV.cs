using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GNDVEntry
    {
        public const int Size = 0x30;

        public byte _modelDataFileIndex;        // 0x00
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;
        private fixed sbyte _boneName[0x20];    // 0x04
        public bint _sfx;                       // 0x24
        public buint _gfx;                      // 0x28
        public TriggerData _trigger;            // 0x2C

        public string BoneName
        {
            get => Address.GetUTF8String(0x4, 0x20);
            set => Address.WriteUTF8String(value, false, 0x4, 0x20);
        }

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
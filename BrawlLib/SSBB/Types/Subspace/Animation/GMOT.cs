using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Animation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GMOTEntry
    {
        public const int Size = 0x1E4;

        public bfloat _unknown0x000;
        public bfloat _unknown0x004;
        public bfloat _unknown0x008;
        public bfloat _unknown0x00C;
        public bfloat _unknown0x010;
        public bfloat _unknown0x014;
        public bfloat _unknown0x018;
        public bfloat _unknown0x01C;
        public bfloat _unknown0x020;
        public bfloat _unknown0x024;
        public bfloat _unknown0x028;
        public bfloat _unknown0x02C;
        public bfloat _unknown0x030;
        public bfloat _unknown0x034;
        public bfloat _unknown0x038;
        public byte _modelDataIndex;        // 0x3C
        public byte _collisionDataIndex;    // 0x3D
        public byte _unknown0x03E;
        public byte _unknown0x03F;
        public bint _unknown0x040;
        public bint _unknown0x044;
        public bint _unknown0x048;
        public bint _unknown0x04C;
        public bint _unknown0x050;
        public bint _unknown0x054;
        public bint _unknown0x058;
        public bint _unknown0x05C;
        public bint _unknown0x060;
        public bint _unknown0x064;
        public bint _unknown0x068;
        public bint _unknown0x06C;
        public bint _unknown0x070;
        public bint _unknown0x074;
        public bint _unknown0x078;
        public bint _unknown0x07C;
        public bint _unknown0x080;
        public bint _unknown0x084;
        public bint _unknown0x088;
        public bint _unknown0x08C;
        public bint _unknown0x090;
        public bint _unknown0x094;
        public bint _unknown0x098;
        public bint _unknown0x09C;
        public bint _unknown0x0A0;
        public bint _unknown0x0A4;
        public bint _unknown0x0A8;
        public bint _unknown0x0AC;
        public bint _unknown0x0B0;
        public bint _unknown0x0B4;
        public bint _unknown0x0B8;
        public bint _unknown0x0BC;
        public bint _unknown0x0C0;
        public bint _unknown0x0C4;
        public bint _unknown0x0C8;
        public bint _unknown0x0CC;
        public bint _unknown0x0D0;
        public bint _unknown0x0D4;
        public bint _unknown0x0D8;
        public bint _unknown0x0DC;
        public bint _unknown0x0E0;
        public bint _unknown0x0E4;
        public bint _unknown0x0E8;
        public bint _unknown0x0EC;
        public bint _unknown0x0F0;
        public bint _unknown0x0F4;
        public bint _unknown0x0F8;
        public bint _unknown0x0FC;
        public bint _unknown0x100;
        public bint _unknown0x104;
        public bint _unknown0x108;
        public bint _unknown0x10C;
        public bint _unknown0x110;
        public bint _unknown0x114;
        public bint _soundInfoIndex;        // 0x118
        public bint _unknown0x11C;
        public bint _unknown0x120;
        public bint _unknown0x124;
        public bint _unknown0x128;
        public bint _unknown0x12C;
        public bint _unknown0x130;
        public bint _unknown0x134;
        public bint _unknown0x138;
        public bint _unknown0x13C;
        public bint _unknown0x140;
        public bint _unknown0x144;
        public bint _unknown0x148;
        public bint _unknown0x14C;
        public bint _unknown0x150;
        public bint _unknown0x154;
        public bint _unknown0x158;
        public bint _unknown0x15C;
        public bint _unknown0x160;
        public bint _unknown0x164;
        public bint _unknown0x168;
        public bint _unknown0x16C;
        public bint _unknown0x170;
        public bint _unknown0x174;
        public bint _unknown0x178;
        public bint _unknown0x17C;
        public bint _unknown0x180;
        public bint _unknown0x184;
        public bint _unknown0x188;
        public bint _unknown0x18C;
        public bint _unknown0x190;
        public bint _unknown0x194;
        public bint _unknown0x198;
        public bint _unknown0x19C;
        public bint _unknown0x1A0;
        public bint _unknown0x1A4;
        public bint _unknown0x1A8;
        public bint _unknown0x1AC;
        public bint _unknown0x1B0;
        public bint _unknown0x1B4;
        public bint _unknown0x1B8;
        public bint _unknown0x1BC;
        public bint _unknown0x1C0;
        public bint _unknown0x1C4;
        public bint _unknown0x1C8;
        public bint _unknown0x1CC;
        public bint _unknown0x1D0;
        public bint _unknown0x1D4;
        public TriggerData _trigger1;
        public TriggerData _trigger2;
        public TriggerData _trigger3;
        
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
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Animation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GMVAEntry
    {
        public const int Size = 0x248;

        public MotionPathData _motionPath; // 0x000
        public DifficultyRatios _difficultyMotionRatio; // 0x008
        public byte _modelDataIndex; // 0x044
        public byte _collisionDataIndex; // 0x045
        public byte _unknown0x046;
        public byte _unknown0x047;
        public bint _unknown0x048;
        public buint _unknown0x04C;
        public buint _unknown0x050;
        public bint _unknown0x054;
        public buint _unknown0x058;
        public buint _unknown0x05C;
        public bint _unknown0x060;
        public buint _unknown0x064;
        public buint _unknown0x068;
        public bshort _unknown0x06C;
        public bshort _unknown0x06E;
        public buint _unknown0x070;
        public buint _unknown0x074;
        public buint _unknown0x078;
        public buint _unknown0x07C;
        public buint _unknown0x080;
        public buint _unknown0x084;
        public buint _unknown0x088;
        public buint _unknown0x08C;
        public bint _unknown0x090;
        public buint _unknown0x094;
        public buint _unknown0x098;
        public bint _unknown0x09C;
        public buint _unknown0x0A0;
        public buint _unknown0x0A4;
        public bint _unknown0x0A8;
        public buint _unknown0x0AC;
        public buint _unknown0x0B0;
        public buint _unknown0x0B4;
        public buint _unknown0x0B8;
        public buint _unknown0x0BC;
        public buint _unknown0x0C0;
        public buint _unknown0x0C4;
        public buint _unknown0x0C8;
        public buint _unknown0x0CC;
        public buint _unknown0x0D0;
        public buint _unknown0x0D4;
        public bint _unknown0x0D8;
        public buint _unknown0x0DC;
        public buint _unknown0x0E0;
        public bint _unknown0x0E4;
        public buint _unknown0x0E8;
        public buint _unknown0x0EC;
        public bint _unknown0x0F0;
        public buint _unknown0x0F4;
        public buint _unknown0x0F8;
        public buint _unknown0x0FC;
        public buint _unknown0x100;
        public buint _unknown0x104;
        public buint _unknown0x108;
        public buint _unknown0x10C;
        public buint _unknown0x110;
        public buint _unknown0x114;
        public buint _unknown0x118;
        public buint _unknown0x11C;
        public buint _unknown0x120;
        public bshort _unknown0x124;
        public bshort _unknown0x126;
        public bshort _unknown0x128;
        public bshort _unknown0x12A;
        public bint _unknown0x12C;
        public bshort _unknown0x130;
        public bshort _unknown0x132;
        public buint _unknown0x134;
        public bint _unknown0x138;
        public bshort _unknown0x13C;
        public bshort _unknown0x13E;
        public buint _unknown0x140;
        public bint _unknown0x144;
        public buint _unknown0x148;
        public buint _unknown0x14C;
        public bint _unknown0x150;
        public buint _unknown0x154;
        public buint _unknown0x158;
        public bshort _unknown0x15C;
        public bshort _unknown0x15E;
        public bshort _unknown0x160;
        public bshort _unknown0x162;
        public bshort _unknown0x164;
        public bshort _unknown0x166;
        public bshort _unknown0x168;
        public bshort _unknown0x16A;
        public bshort _unknown0x16C;
        public bshort _unknown0x16E;
        public bshort _unknown0x170;
        public bshort _unknown0x172;
        public bshort _unknown0x174;
        public bshort _unknown0x176;
        public bshort _unknown0x178;
        public bshort _unknown0x17A;
        public bshort _unknown0x17C;
        public bshort _unknown0x17E;
        public buint _unknown0x180;
        public buint _unknown0x184;
        public buint _unknown0x188;
        public buint _unknown0x18C;
        public buint _unknown0x190;
        public buint _unknown0x194;
        public bint _unknown0x198;
        public buint _unknown0x19C;
        public buint _unknown0x1A0;
        public bint _unknown0x1A4;
        public buint _unknown0x1A8;
        public buint _unknown0x1AC;
        public bint _unknown0x1B0;
        public buint _unknown0x1B4;
        public buint _unknown0x1B8;
        public buint _unknown0x1BC;
        public buint _unknown0x1C0;
        public buint _unknown0x1C4;
        public buint _unknown0x1C8;
        public buint _unknown0x1CC;
        public buint _unknown0x1D0;
        public buint _unknown0x1D4;
        public buint _unknown0x1D8;
        public buint _unknown0x1DC;
        public TriggerData _unknown0x1E0;
        public TriggerData _unknown0x1E4;
        public buint _unknown0x1E8;
        public bfloat _unknown0x1EC;
        public bfloat _unknown0x1F0;
        public bfloat _unknown0x1F4;
        public buint _unknown0x1F8;
        public bfloat _unknown0x1FC;
        public buint _unknown0x200;
        public buint _unknown0x204;
        public buint _unknown0x208;
        public buint _unknown0x20C;
        public buint _unknown0x210;
        public bshort _unknown0x214;
        public bshort _unknown0x216;
        public byte _unknown0x218;
        public byte _unknown0x219;
        public byte _unknown0x21A;
        public byte _unknown0x21B;
        public buint _unknown0x21C;
        public buint _unknown0x220;
        public buint _unknown0x224;
        public buint _unknown0x228;
        public byte _unknown0x22C;
        public byte _unknown0x22D;
        public byte _unknown0x22E;
        public byte _unknown0x22F;
        public buint _unknown0x230;
        public byte _unknown0x234;
        public bool8 _unknown0x235;
        public byte _unknown0x236;
        public byte _unknown0x237;
        public buint _unknown0x238;
        public buint _unknown0x23C;
        public byte _unknown0x240;
        public byte _unknown0x241;
        public byte _unknown0x242;
        public byte _unknown0x243;
        public buint _unknown0x244;
    }
}
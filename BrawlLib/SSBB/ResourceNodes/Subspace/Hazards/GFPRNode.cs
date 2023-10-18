using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFPRNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFPREntryNode);
        protected override string baseName => "Fire Pillars";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFPR" ? new GFPRNode() : null;
        }
    }

    public unsafe class GFPREntryNode : ResourceNode
    {
        protected internal GFPREntry Data;

        [DisplayName("Unknown0x000 (float)")]
        [Category("Unknown")]
        public float Unknown0x000
        {
            get => Data._unknown0x000;
            set
            {
                Data._unknown0x000 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x004 (float)")]
        [Category("Unknown")]
        public float Unknown0x004
        {
            get => Data._unknown0x004;
            set
            {
                Data._unknown0x004 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x008 (float)")]
        [Category("Unknown")]
        public float Unknown0x008
        {
            get => Data._unknown0x008;
            set
            {
                Data._unknown0x008 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x00C (float)")]
        [Category("Unknown")]
        public float Unknown0x00C
        {
            get => Data._unknown0x00C;
            set
            {
                Data._unknown0x00C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x010 (float)")]
        [Category("Unknown")]
        public float Unknown0x010
        {
            get => Data._unknown0x010;
            set
            {
                Data._unknown0x010 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x014 (float)")]
        [Category("Unknown")]
        public float Unknown0x014
        {
            get => Data._unknown0x014;
            set
            {
                Data._unknown0x014 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x018 (float)")]
        [Category("Unknown")]
        public float Unknown0x018
        {
            get => Data._unknown0x018;
            set
            {
                Data._unknown0x018 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x01C (float)")]
        [Category("Unknown")]
        public float Unknown0x01C
        {
            get => Data._unknown0x01C;
            set
            {
                Data._unknown0x01C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x020 (float)")]
        [Category("Unknown")]
        public float Unknown0x020
        {
            get => Data._unknown0x020;
            set
            {
                Data._unknown0x020 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x024 (float)")]
        [Category("Unknown")]
        public float Unknown0x024
        {
            get => Data._unknown0x024;
            set
            {
                Data._unknown0x024 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x028 (float)")]
        [Category("Unknown")]
        public float Unknown0x028
        {
            get => Data._unknown0x028;
            set
            {
                Data._unknown0x028 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x02C (float)")]
        [Category("Unknown")]
        public float Unknown0x02C
        {
            get => Data._unknown0x02C;
            set
            {
                Data._unknown0x02C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x030 (float)")]
        [Category("Unknown")]
        public float Unknown0x030
        {
            get => Data._unknown0x030;
            set
            {
                Data._unknown0x030 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x034 (float)")]
        [Category("Unknown")]
        public float Unknown0x034
        {
            get => Data._unknown0x034;
            set
            {
                Data._unknown0x034 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x038 (float)")]
        [Category("Unknown")]
        public float Unknown0x038
        {
            get => Data._unknown0x038;
            set
            {
                Data._unknown0x038 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x03C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x03C
        {
            get => Data._unknown0x03C;
            set
            {
                Data._unknown0x03C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x03D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x03D
        {
            get => Data._unknown0x03D;
            set
            {
                Data._unknown0x03D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x03E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x03E
        {
            get => Data._unknown0x03E;
            set
            {
                Data._unknown0x03E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x03F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x03F
        {
            get => Data._unknown0x03F;
            set
            {
                Data._unknown0x03F = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x040 (int)")]
        [Category("Unknown")]
        public int Unknown0x040
        {
            get => Data._unknown0x040;
            set
            {
                Data._unknown0x040 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x044 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x044
        {
            get => Data._unknown0x044;
            set
            {
                Data._unknown0x044 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x048 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x048
        {
            get => Data._unknown0x048;
            set
            {
                Data._unknown0x048 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x04C (int)")]
        [Category("Unknown")]
        public int Unknown0x04C
        {
            get => Data._unknown0x04C;
            set
            {
                Data._unknown0x04C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x050 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x050
        {
            get => Data._unknown0x050;
            set
            {
                Data._unknown0x050 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x054 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x054
        {
            get => Data._unknown0x054;
            set
            {
                Data._unknown0x054 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x058 (int)")]
        [Category("Unknown")]
        public int Unknown0x058
        {
            get => Data._unknown0x058;
            set
            {
                Data._unknown0x058 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x05C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x05C
        {
            get => Data._unknown0x05C;
            set
            {
                Data._unknown0x05C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x060 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x060
        {
            get => Data._unknown0x060;
            set
            {
                Data._unknown0x060 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x064 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x064
        {
            get => Data._unknown0x064;
            set
            {
                Data._unknown0x064 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x068 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x068
        {
            get => Data._unknown0x068;
            set
            {
                Data._unknown0x068 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x06C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x06C
        {
            get => Data._unknown0x06C;
            set
            {
                Data._unknown0x06C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x070 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x070
        {
            get => Data._unknown0x070;
            set
            {
                Data._unknown0x070 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x074 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x074
        {
            get => Data._unknown0x074;
            set
            {
                Data._unknown0x074 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x078 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x078
        {
            get => Data._unknown0x078;
            set
            {
                Data._unknown0x078 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x07C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x07C
        {
            get => Data._unknown0x07C;
            set
            {
                Data._unknown0x07C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x080 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x080
        {
            get => Data._unknown0x080;
            set
            {
                Data._unknown0x080 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x084 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x084
        {
            get => Data._unknown0x084;
            set
            {
                Data._unknown0x084 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x088 (int)")]
        [Category("Unknown")]
        public int Unknown0x088
        {
            get => Data._unknown0x088;
            set
            {
                Data._unknown0x088 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x08C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x08C
        {
            get => Data._unknown0x08C;
            set
            {
                Data._unknown0x08C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x090 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x090
        {
            get => Data._unknown0x090;
            set
            {
                Data._unknown0x090 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x094 (int)")]
        [Category("Unknown")]
        public int Unknown0x094
        {
            get => Data._unknown0x094;
            set
            {
                Data._unknown0x094 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x098 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x098
        {
            get => Data._unknown0x098;
            set
            {
                Data._unknown0x098 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x09C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x09C
        {
            get => Data._unknown0x09C;
            set
            {
                Data._unknown0x09C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0A0 (int)")]
        [Category("Unknown")]
        public int Unknown0x0A0
        {
            get => Data._unknown0x0A0;
            set
            {
                Data._unknown0x0A0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0A4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0A4
        {
            get => Data._unknown0x0A4;
            set
            {
                Data._unknown0x0A4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0A8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0A8
        {
            get => Data._unknown0x0A8;
            set
            {
                Data._unknown0x0A8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0AC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0AC
        {
            get => Data._unknown0x0AC;
            set
            {
                Data._unknown0x0AC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0B0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0B0
        {
            get => Data._unknown0x0B0;
            set
            {
                Data._unknown0x0B0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0B4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0B4
        {
            get => Data._unknown0x0B4;
            set
            {
                Data._unknown0x0B4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0B8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0B8
        {
            get => Data._unknown0x0B8;
            set
            {
                Data._unknown0x0B8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0BC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0BC
        {
            get => Data._unknown0x0BC;
            set
            {
                Data._unknown0x0BC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0C0
        {
            get => Data._unknown0x0C0;
            set
            {
                Data._unknown0x0C0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0C4
        {
            get => Data._unknown0x0C4;
            set
            {
                Data._unknown0x0C4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0C8
        {
            get => Data._unknown0x0C8;
            set
            {
                Data._unknown0x0C8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0CC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0CC
        {
            get => Data._unknown0x0CC;
            set
            {
                Data._unknown0x0CC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D0 (int)")]
        [Category("Unknown")]
        public int Unknown0x0D0
        {
            get => Data._unknown0x0D0;
            set
            {
                Data._unknown0x0D0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0D4
        {
            get => Data._unknown0x0D4;
            set
            {
                Data._unknown0x0D4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0D8
        {
            get => Data._unknown0x0D8;
            set
            {
                Data._unknown0x0D8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0DC (int)")]
        [Category("Unknown")]
        public int Unknown0x0DC
        {
            get => Data._unknown0x0DC;
            set
            {
                Data._unknown0x0DC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0E0
        {
            get => Data._unknown0x0E0;
            set
            {
                Data._unknown0x0E0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0E4
        {
            get => Data._unknown0x0E4;
            set
            {
                Data._unknown0x0E4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E8 (int)")]
        [Category("Unknown")]
        public int Unknown0x0E8
        {
            get => Data._unknown0x0E8;
            set
            {
                Data._unknown0x0E8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0EC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0EC
        {
            get => Data._unknown0x0EC;
            set
            {
                Data._unknown0x0EC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0F0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0F0
        {
            get => Data._unknown0x0F0;
            set
            {
                Data._unknown0x0F0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0F4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0F4
        {
            get => Data._unknown0x0F4;
            set
            {
                Data._unknown0x0F4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0F8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0F8
        {
            get => Data._unknown0x0F8;
            set
            {
                Data._unknown0x0F8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0FC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0FC
        {
            get => Data._unknown0x0FC;
            set
            {
                Data._unknown0x0FC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x100 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x100
        {
            get => Data._unknown0x100;
            set
            {
                Data._unknown0x100 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x104 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x104
        {
            get => Data._unknown0x104;
            set
            {
                Data._unknown0x104 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x108 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x108
        {
            get => Data._unknown0x108;
            set
            {
                Data._unknown0x108 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x10C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x10C
        {
            get => Data._unknown0x10C;
            set
            {
                Data._unknown0x10C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x110 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x110
        {
            get => Data._unknown0x110;
            set
            {
                Data._unknown0x110 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x114 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x114
        {
            get => Data._unknown0x114;
            set
            {
                Data._unknown0x114 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x118 (int)")]
        [Category("Unknown")]
        public int Unknown0x118
        {
            get => Data._unknown0x118;
            set
            {
                Data._unknown0x118 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x11C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x11C
        {
            get => Data._unknown0x11C;
            set
            {
                Data._unknown0x11C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x120 (short)")]
        [Category("Unknown")]
        public short Unknown0x120
        {
            get => Data._unknown0x120;
            set
            {
                Data._unknown0x120 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x122 (short)")]
        [Category("Unknown")]
        public short Unknown0x122
        {
            get => Data._unknown0x122;
            set
            {
                Data._unknown0x122 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x124 (int)")]
        [Category("Unknown")]
        public int Unknown0x124
        {
            get => Data._unknown0x124;
            set
            {
                Data._unknown0x124 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x128 (short)")]
        [Category("Unknown")]
        public short Unknown0x128
        {
            get => Data._unknown0x128;
            set
            {
                Data._unknown0x128 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x12A (short)")]
        [Category("Unknown")]
        public short Unknown0x12A
        {
            get => Data._unknown0x12A;
            set
            {
                Data._unknown0x12A = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x12C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x12C
        {
            get => Data._unknown0x12C;
            set
            {
                Data._unknown0x12C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x130 (int)")]
        [Category("Unknown")]
        public int Unknown0x130
        {
            get => Data._unknown0x130;
            set
            {
                Data._unknown0x130 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x134 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x134
        {
            get => Data._unknown0x134;
            set
            {
                Data._unknown0x134 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x138 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x138
        {
            get => Data._unknown0x138;
            set
            {
                Data._unknown0x138 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x13C (int)")]
        [Category("Unknown")]
        public int Unknown0x13C
        {
            get => Data._unknown0x13C;
            set
            {
                Data._unknown0x13C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x140 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x140
        {
            get => Data._unknown0x140;
            set
            {
                Data._unknown0x140 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x144 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x144
        {
            get => Data._unknown0x144;
            set
            {
                Data._unknown0x144 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x148 (int)")]
        [Category("Unknown")]
        public int Unknown0x148
        {
            get => Data._unknown0x148;
            set
            {
                Data._unknown0x148 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x14C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x14C
        {
            get => Data._unknown0x14C;
            set
            {
                Data._unknown0x14C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x150 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x150
        {
            get => Data._unknown0x150;
            set
            {
                Data._unknown0x150 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x154 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x154
        {
            get => Data._unknown0x154;
            set
            {
                Data._unknown0x154 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x158 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x158
        {
            get => Data._unknown0x158;
            set
            {
                Data._unknown0x158 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x15C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x15C
        {
            get => Data._unknown0x15C;
            set
            {
                Data._unknown0x15C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x160 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x160
        {
            get => Data._unknown0x160;
            set
            {
                Data._unknown0x160 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x164 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x164
        {
            get => Data._unknown0x164;
            set
            {
                Data._unknown0x164 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x168 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x168
        {
            get => Data._unknown0x168;
            set
            {
                Data._unknown0x168 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x16C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x16C
        {
            get => Data._unknown0x16C;
            set
            {
                Data._unknown0x16C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x170 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x170
        {
            get => Data._unknown0x170;
            set
            {
                Data._unknown0x170 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x174 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x174
        {
            get => Data._unknown0x174;
            set
            {
                Data._unknown0x174 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x178 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x178
        {
            get => Data._unknown0x178;
            set
            {
                Data._unknown0x178 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x17C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x17C
        {
            get => Data._unknown0x17C;
            set
            {
                Data._unknown0x17C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x180 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x180
        {
            get => Data._unknown0x180;
            set
            {
                Data._unknown0x180 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x184 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x184
        {
            get => Data._unknown0x184;
            set
            {
                Data._unknown0x184 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x188 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x188
        {
            get => Data._unknown0x188;
            set
            {
                Data._unknown0x188 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x18C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x18C
        {
            get => Data._unknown0x18C;
            set
            {
                Data._unknown0x18C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x190 (int)")]
        [Category("Unknown")]
        public int Unknown0x190
        {
            get => Data._unknown0x190;
            set
            {
                Data._unknown0x190 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x194 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x194
        {
            get => Data._unknown0x194;
            set
            {
                Data._unknown0x194 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x198 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x198
        {
            get => Data._unknown0x198;
            set
            {
                Data._unknown0x198 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x19C (int)")]
        [Category("Unknown")]
        public int Unknown0x19C
        {
            get => Data._unknown0x19C;
            set
            {
                Data._unknown0x19C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1A0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1A0
        {
            get => Data._unknown0x1A0;
            set
            {
                Data._unknown0x1A0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1A4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1A4
        {
            get => Data._unknown0x1A4;
            set
            {
                Data._unknown0x1A4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1A8 (int)")]
        [Category("Unknown")]
        public int Unknown0x1A8
        {
            get => Data._unknown0x1A8;
            set
            {
                Data._unknown0x1A8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1AC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1AC
        {
            get => Data._unknown0x1AC;
            set
            {
                Data._unknown0x1AC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1B0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1B0
        {
            get => Data._unknown0x1B0;
            set
            {
                Data._unknown0x1B0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1B4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1B4
        {
            get => Data._unknown0x1B4;
            set
            {
                Data._unknown0x1B4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1B8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1B8
        {
            get => Data._unknown0x1B8;
            set
            {
                Data._unknown0x1B8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1BC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1BC
        {
            get => Data._unknown0x1BC;
            set
            {
                Data._unknown0x1BC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1C0
        {
            get => Data._unknown0x1C0;
            set
            {
                Data._unknown0x1C0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1C4
        {
            get => Data._unknown0x1C4;
            set
            {
                Data._unknown0x1C4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1C8
        {
            get => Data._unknown0x1C8;
            set
            {
                Data._unknown0x1C8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1CC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1CC
        {
            get => Data._unknown0x1CC;
            set
            {
                Data._unknown0x1CC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1D0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1D0
        {
            get => Data._unknown0x1D0;
            set
            {
                Data._unknown0x1D0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1D4 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1D4
        {
            get => Data._unknown0x1D4;
            set
            {
                Data._unknown0x1D4 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x1D8;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x1D8 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x1D8
        {
            get => _unknown0x1D8 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x1D8 = value;
                Data._unknown0x1D8 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x1DC;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x1DC (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x1DC
        {
            get => _unknown0x1DC ?? new TriggerDataClass(this);
            set
            {
                _unknown0x1DC = value;
                Data._unknown0x1DC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1E0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1E0
        {
            get => Data._unknown0x1E0;
            set
            {
                Data._unknown0x1E0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1E4 (float)")]
        [Category("Unknown")]
        public float Unknown0x1E4
        {
            get => Data._unknown0x1E4;
            set
            {
                Data._unknown0x1E4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1E8 (float)")]
        [Category("Unknown")]
        public float Unknown0x1E8
        {
            get => Data._unknown0x1E8;
            set
            {
                Data._unknown0x1E8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1EC (float)")]
        [Category("Unknown")]
        public float Unknown0x1EC
        {
            get => Data._unknown0x1EC;
            set
            {
                Data._unknown0x1EC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1F0 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1F0
        {
            get => Data._unknown0x1F0;
            set
            {
                Data._unknown0x1F0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1F4 (float)")]
        [Category("Unknown")]
        public float Unknown0x1F4
        {
            get => Data._unknown0x1F4;
            set
            {
                Data._unknown0x1F4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1F8 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1F8
        {
            get => Data._unknown0x1F8;
            set
            {
                Data._unknown0x1F8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1FC (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1FC
        {
            get => Data._unknown0x1FC;
            set
            {
                Data._unknown0x1FC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x200 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x200
        {
            get => Data._unknown0x200;
            set
            {
                Data._unknown0x200 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x204 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x204
        {
            get => Data._unknown0x204;
            set
            {
                Data._unknown0x204 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x208 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x208
        {
            get => Data._unknown0x208;
            set
            {
                Data._unknown0x208 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x20C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x20C
        {
            get => Data._unknown0x20C;
            set
            {
                Data._unknown0x20C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x210 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x210
        {
            get => Data._unknown0x210;
            set
            {
                Data._unknown0x210 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x211 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x211
        {
            get => Data._unknown0x211;
            set
            {
                Data._unknown0x211 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x212 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x212
        {
            get => Data._unknown0x212;
            set
            {
                Data._unknown0x212 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x213 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x213
        {
            get => Data._unknown0x213;
            set
            {
                Data._unknown0x213 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x214 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x214
        {
            get => Data._unknown0x214;
            set
            {
                Data._unknown0x214 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x218 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x218
        {
            get => Data._unknown0x218;
            set
            {
                Data._unknown0x218 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x21C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x21C
        {
            get => Data._unknown0x21C;
            set
            {
                Data._unknown0x21C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x220 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x220
        {
            get => Data._unknown0x220;
            set
            {
                Data._unknown0x220 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x224 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x224
        {
            get => Data._unknown0x224;
            set
            {
                Data._unknown0x224 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x225 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x225
        {
            get => Data._unknown0x225;
            set
            {
                Data._unknown0x225 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x226 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x226
        {
            get => Data._unknown0x226;
            set
            {
                Data._unknown0x226 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x227 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x227
        {
            get => Data._unknown0x227;
            set
            {
                Data._unknown0x227 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x228 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x228
        {
            get => Data._unknown0x228;
            set
            {
                Data._unknown0x228 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x22C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x22C
        {
            get => Data._unknown0x22C;
            set
            {
                Data._unknown0x22C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x22D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x22D
        {
            get => Data._unknown0x22D;
            set
            {
                Data._unknown0x22D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x22E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x22E
        {
            get => Data._unknown0x22E;
            set
            {
                Data._unknown0x22E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x22F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x22F
        {
            get => Data._unknown0x22F;
            set
            {
                Data._unknown0x22F = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x230 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x230
        {
            get => Data._unknown0x230;
            set
            {
                Data._unknown0x230 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x234 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x234
        {
            get => Data._unknown0x234;
            set
            {
                Data._unknown0x234 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x238 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x238
        {
            get => Data._unknown0x238;
            set
            {
                Data._unknown0x238 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x23C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x23C
        {
            get => Data._unknown0x23C;
            set
            {
                Data._unknown0x23C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x240 (float)")]
        [Category("Unknown")]
        public float Unknown0x240
        {
            get => Data._unknown0x240;
            set
            {
                Data._unknown0x240 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x244 (float)")]
        [Category("Unknown")]
        public float Unknown0x244
        {
            get => Data._unknown0x244;
            set
            {
                Data._unknown0x244 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x248 (float)")]
        [Category("Unknown")]
        public float Unknown0x248
        {
            get => Data._unknown0x248;
            set
            {
                Data._unknown0x248 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x24C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x24C
        {
            get => Data._unknown0x24C;
            set
            {
                Data._unknown0x24C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x250 (float)")]
        [Category("Unknown")]
        public float Unknown0x250
        {
            get => Data._unknown0x250;
            set
            {
                Data._unknown0x250 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x254 (float)")]
        [Category("Unknown")]
        public float Unknown0x254
        {
            get => Data._unknown0x254;
            set
            {
                Data._unknown0x254 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x258;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x258 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x258
        {
            get => _unknown0x258 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x258 = value;
                Data._unknown0x258 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x25C (float)")]
        [Category("Unknown")]
        public float Unknown0x25C
        {
            get => Data._unknown0x25C;
            set
            {
                Data._unknown0x25C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x260 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x260
        {
            get => Data._unknown0x260;
            set
            {
                Data._unknown0x260 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x261 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x261
        {
            get => Data._unknown0x261;
            set
            {
                Data._unknown0x261 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x262 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x262
        {
            get => Data._unknown0x262;
            set
            {
                Data._unknown0x262 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x263 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x263
        {
            get => Data._unknown0x263;
            set
            {
                Data._unknown0x263 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x264 (float)")]
        [Category("Unknown")]
        public float Unknown0x264
        {
            get => Data._unknown0x264;
            set
            {
                Data._unknown0x264 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x268 (float)")]
        [Category("Unknown")]
        public float Unknown0x268
        {
            get => Data._unknown0x268;
            set
            {
                Data._unknown0x268 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x26C (float)")]
        [Category("Unknown")]
        public float Unknown0x26C
        {
            get => Data._unknown0x26C;
            set
            {
                Data._unknown0x26C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x270 (float)")]
        [Category("Unknown")]
        public float Unknown0x270
        {
            get => Data._unknown0x270;
            set
            {
                Data._unknown0x270 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x274 (float)")]
        [Category("Unknown")]
        public float Unknown0x274
        {
            get => Data._unknown0x274;
            set
            {
                Data._unknown0x274 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x278 (float)")]
        [Category("Unknown")]
        public float Unknown0x278
        {
            get => Data._unknown0x278;
            set
            {
                Data._unknown0x278 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x27C (float)")]
        [Category("Unknown")]
        public float Unknown0x27C
        {
            get => Data._unknown0x27C;
            set
            {
                Data._unknown0x27C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x280 (float)")]
        [Category("Unknown")]
        public float Unknown0x280
        {
            get => Data._unknown0x280;
            set
            {
                Data._unknown0x280 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x284 (float)")]
        [Category("Unknown")]
        public float Unknown0x284
        {
            get => Data._unknown0x284;
            set
            {
                Data._unknown0x284 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x288 (float)")]
        [Category("Unknown")]
        public float Unknown0x288
        {
            get => Data._unknown0x288;
            set
            {
                Data._unknown0x288 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x28C (float)")]
        [Category("Unknown")]
        public float Unknown0x28C
        {
            get => Data._unknown0x28C;
            set
            {
                Data._unknown0x28C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x290 (float)")]
        [Category("Unknown")]
        public float Unknown0x290
        {
            get => Data._unknown0x290;
            set
            {
                Data._unknown0x290 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x294 (float)")]
        [Category("Unknown")]
        public float Unknown0x294
        {
            get => Data._unknown0x294;
            set
            {
                Data._unknown0x294 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x298 (float)")]
        [Category("Unknown")]
        public float Unknown0x298
        {
            get => Data._unknown0x298;
            set
            {
                Data._unknown0x298 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x29C (float)")]
        [Category("Unknown")]
        public float Unknown0x29C
        {
            get => Data._unknown0x29C;
            set
            {
                Data._unknown0x29C = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GFPREntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GFPREntry*) WorkingUncompressed.Address;
            _unknown0x1D8 = new TriggerDataClass(this, Data._unknown0x1D8);
            _unknown0x1DC = new TriggerDataClass(this, Data._unknown0x1DC);
            _unknown0x258 = new TriggerDataClass(this, Data._unknown0x258);

            if (_name == null)
            {
                _name = $"Fire Pillar [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFPREntry* hdr = (GFPREntry*)address;
            Data._unknown0x1D8 = _unknown0x1D8;
            Data._unknown0x1DC = _unknown0x1DC;
            Data._unknown0x258 = _unknown0x258;
            *hdr = Data;
        }
    }
}
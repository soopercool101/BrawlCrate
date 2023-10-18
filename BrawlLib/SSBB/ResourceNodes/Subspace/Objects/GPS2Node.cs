using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GPS2Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GPS2EntryNode);
        protected override string baseName => "Punch Sliders 2";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GPS2" ? new GPS2Node() : null;
        }
    }

    public unsafe class GPS2EntryNode : ResourceNode
    {
        protected internal GPS2Entry Data;

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

        [DisplayName("Unknown0x004 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x004
        {
            get => Data._unknown0x004;
            set
            {
                Data._unknown0x004 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x005 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x005
        {
            get => Data._unknown0x005;
            set
            {
                Data._unknown0x005 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x006 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x006
        {
            get => Data._unknown0x006;
            set
            {
                Data._unknown0x006 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x007 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x007
        {
            get => Data._unknown0x007;
            set
            {
                Data._unknown0x007 = value;
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

        [DisplayName("Unknown0x00C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x00C
        {
            get => Data._unknown0x00C;
            set
            {
                Data._unknown0x00C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x00D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x00D
        {
            get => Data._unknown0x00D;
            set
            {
                Data._unknown0x00D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x00E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x00E
        {
            get => Data._unknown0x00E;
            set
            {
                Data._unknown0x00E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x00F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x00F
        {
            get => Data._unknown0x00F;
            set
            {
                Data._unknown0x00F = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x010 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x010
        {
            get => Data._unknown0x010;
            set
            {
                Data._unknown0x010 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x014 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x014
        {
            get => Data._unknown0x014;
            set
            {
                Data._unknown0x014 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x018 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x018
        {
            get => Data._unknown0x018;
            set
            {
                Data._unknown0x018 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x01C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x01C
        {
            get => Data._unknown0x01C;
            set
            {
                Data._unknown0x01C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x020 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x020
        {
            get => Data._unknown0x020;
            set
            {
                Data._unknown0x020 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x024 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x024
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

        [DisplayName("Unknown0x02C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x02C
        {
            get => Data._unknown0x02C;
            set
            {
                Data._unknown0x02C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x030 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x030
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

        [DisplayName("Unknown0x038 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x038
        {
            get => Data._unknown0x038;
            set
            {
                Data._unknown0x038 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x03C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x03C
        {
            get => Data._unknown0x03C;
            set
            {
                Data._unknown0x03C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x040 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x040
        {
            get => Data._unknown0x040;
            set
            {
                Data._unknown0x040 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x044 (float)")]
        [Category("Unknown")]
        public float Unknown0x044
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

        [DisplayName("Unknown0x04C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x04C
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

        [DisplayName("Unknown0x058 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x058
        {
            get => Data._unknown0x058;
            set
            {
                Data._unknown0x058 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x05C (short)")]
        [Category("Unknown")]
        public short Unknown0x05C
        {
            get => Data._unknown0x05C;
            set
            {
                Data._unknown0x05C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x05E (short)")]
        [Category("Unknown")]
        public short Unknown0x05E
        {
            get => Data._unknown0x05E;
            set
            {
                Data._unknown0x05E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x060 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x060
        {
            get => Data._unknown0x060;
            set
            {
                Data._unknown0x060 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x061 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x061
        {
            get => Data._unknown0x061;
            set
            {
                Data._unknown0x061 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x062 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x062
        {
            get => Data._unknown0x062;
            set
            {
                Data._unknown0x062 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x063 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x063
        {
            get => Data._unknown0x063;
            set
            {
                Data._unknown0x063 = value;
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

        [DisplayName("Unknown0x074 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x074
        {
            get => Data._unknown0x074;
            set
            {
                Data._unknown0x074 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x075 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x075
        {
            get => Data._unknown0x075;
            set
            {
                Data._unknown0x075 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x076 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x076
        {
            get => Data._unknown0x076;
            set
            {
                Data._unknown0x076 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x077 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x077
        {
            get => Data._unknown0x077;
            set
            {
                Data._unknown0x077 = value;
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

        [DisplayName("Unknown0x088 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x088
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

        [DisplayName("Unknown0x094 (float)")]
        [Category("Unknown")]
        public float Unknown0x094
        {
            get => Data._unknown0x094;
            set
            {
                Data._unknown0x094 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x098 (float)")]
        [Category("Unknown")]
        public float Unknown0x098
        {
            get => Data._unknown0x098;
            set
            {
                Data._unknown0x098 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x09C (float)")]
        [Category("Unknown")]
        public float Unknown0x09C
        {
            get => Data._unknown0x09C;
            set
            {
                Data._unknown0x09C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0A0 (float)")]
        [Category("Unknown")]
        public float Unknown0x0A0
        {
            get => Data._unknown0x0A0;
            set
            {
                Data._unknown0x0A0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0A4 (float)")]
        [Category("Unknown")]
        public float Unknown0x0A4
        {
            get => Data._unknown0x0A4;
            set
            {
                Data._unknown0x0A4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0A8 (float)")]
        [Category("Unknown")]
        public float Unknown0x0A8
        {
            get => Data._unknown0x0A8;
            set
            {
                Data._unknown0x0A8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0AC (float)")]
        [Category("Unknown")]
        public float Unknown0x0AC
        {
            get => Data._unknown0x0AC;
            set
            {
                Data._unknown0x0AC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0B0 (float)")]
        [Category("Unknown")]
        public float Unknown0x0B0
        {
            get => Data._unknown0x0B0;
            set
            {
                Data._unknown0x0B0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0B4 (float)")]
        [Category("Unknown")]
        public float Unknown0x0B4
        {
            get => Data._unknown0x0B4;
            set
            {
                Data._unknown0x0B4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0B8 (float)")]
        [Category("Unknown")]
        public float Unknown0x0B8
        {
            get => Data._unknown0x0B8;
            set
            {
                Data._unknown0x0B8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0BC (float)")]
        [Category("Unknown")]
        public float Unknown0x0BC
        {
            get => Data._unknown0x0BC;
            set
            {
                Data._unknown0x0BC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C0 (float)")]
        [Category("Unknown")]
        public float Unknown0x0C0
        {
            get => Data._unknown0x0C0;
            set
            {
                Data._unknown0x0C0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C4 (float)")]
        [Category("Unknown")]
        public float Unknown0x0C4
        {
            get => Data._unknown0x0C4;
            set
            {
                Data._unknown0x0C4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C8 (float)")]
        [Category("Unknown")]
        public float Unknown0x0C8
        {
            get => Data._unknown0x0C8;
            set
            {
                Data._unknown0x0C8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0CC (float)")]
        [Category("Unknown")]
        public float Unknown0x0CC
        {
            get => Data._unknown0x0CC;
            set
            {
                Data._unknown0x0CC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D0 (float)")]
        [Category("Unknown")]
        public float Unknown0x0D0
        {
            get => Data._unknown0x0D0;
            set
            {
                Data._unknown0x0D0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D4 (float)")]
        [Category("Unknown")]
        public float Unknown0x0D4
        {
            get => Data._unknown0x0D4;
            set
            {
                Data._unknown0x0D4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D8 (float)")]
        [Category("Unknown")]
        public float Unknown0x0D8
        {
            get => Data._unknown0x0D8;
            set
            {
                Data._unknown0x0D8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0DC (float)")]
        [Category("Unknown")]
        public float Unknown0x0DC
        {
            get => Data._unknown0x0DC;
            set
            {
                Data._unknown0x0DC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E0 (float)")]
        [Category("Unknown")]
        public float Unknown0x0E0
        {
            get => Data._unknown0x0E0;
            set
            {
                Data._unknown0x0E0 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E4 (float)")]
        [Category("Unknown")]
        public float Unknown0x0E4
        {
            get => Data._unknown0x0E4;
            set
            {
                Data._unknown0x0E4 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x0E8;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x0E8 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x0E8
        {
            get => _unknown0x0E8 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x0E8 = value;
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

        [DisplayName("Unknown0x0F8 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0F8
        {
            get => Data._unknown0x0F8;
            set
            {
                Data._unknown0x0F8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0F9 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0F9
        {
            get => Data._unknown0x0F9;
            set
            {
                Data._unknown0x0F9 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0FA (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0FA
        {
            get => Data._unknown0x0FA;
            set
            {
                Data._unknown0x0FA = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0FB (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0FB
        {
            get => Data._unknown0x0FB;
            set
            {
                Data._unknown0x0FB = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0FC (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0FC
        {
            get => Data._unknown0x0FC;
            set
            {
                Data._unknown0x0FC = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0FD (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0FD
        {
            get => Data._unknown0x0FD;
            set
            {
                Data._unknown0x0FD = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0FE (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0FE
        {
            get => Data._unknown0x0FE;
            set
            {
                Data._unknown0x0FE = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0FF (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0FF
        {
            get => Data._unknown0x0FF;
            set
            {
                Data._unknown0x0FF = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x100 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x100
        {
            get => Data._unknown0x100;
            set
            {
                Data._unknown0x100 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x101 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x101
        {
            get => Data._unknown0x101;
            set
            {
                Data._unknown0x101 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x102 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x102
        {
            get => Data._unknown0x102;
            set
            {
                Data._unknown0x102 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x103 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x103
        {
            get => Data._unknown0x103;
            set
            {
                Data._unknown0x103 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x104 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x104
        {
            get => Data._unknown0x104;
            set
            {
                Data._unknown0x104 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x105 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x105
        {
            get => Data._unknown0x105;
            set
            {
                Data._unknown0x105 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x106 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x106
        {
            get => Data._unknown0x106;
            set
            {
                Data._unknown0x106 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x107 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x107
        {
            get => Data._unknown0x107;
            set
            {
                Data._unknown0x107 = value;
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

        [DisplayName("Unknown0x118 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x118
        {
            get => Data._unknown0x118;
            set
            {
                Data._unknown0x118 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x11C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x11C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x11C
        {
            get => _unknown0x11C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x11C = value;
                Data._unknown0x11C = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GPS2Entry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GPS2Entry*) WorkingUncompressed.Address;
            _unknown0x0E8 = new TriggerDataClass(this, Data._unknown0x0E8);
            _unknown0x11C = new TriggerDataClass(this, Data._unknown0x11C);

            if (_name == null)
            {
                _name = $"Punch Slider [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GPS2Entry* hdr = (GPS2Entry*)address;
            Data._unknown0x0E8 = _unknown0x0E8;
            Data._unknown0x11C = _unknown0x11C;
            *hdr = Data;
        }
    }
}
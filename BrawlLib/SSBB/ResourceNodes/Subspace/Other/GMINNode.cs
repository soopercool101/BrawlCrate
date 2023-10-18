using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Other;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GMINNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GMINEntryNode);
        protected override string baseName => "Mine";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMIN" ? new GMINNode() : null;
        }
    }

    public unsafe class GMINEntryNode : ResourceNode
    {
        protected internal GMINEntry Data;

        [DisplayName("Unknown0x00 (float)")]
        [Category("Unknown")]
        public float Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x04 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x08 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x10 (float)")]
        [Category("Unknown")]
        public float Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x14 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x18 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x20 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x20
        {
            get => Data._unknown0x20;
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x24 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x28 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x2C
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x2D
        {
            get => Data._unknown0x2D;
            set
            {
                Data._unknown0x2D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x2E
        {
            get => Data._unknown0x2E;
            set
            {
                Data._unknown0x2E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x2F
        {
            get => Data._unknown0x2F;
            set
            {
                Data._unknown0x2F = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x30 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x34 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x38 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x3C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x3C
        {
            get => Data._unknown0x3C;
            set
            {
                Data._unknown0x3C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x40 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x40
        {
            get => Data._unknown0x40;
            set
            {
                Data._unknown0x40 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x41 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x41
        {
            get => Data._unknown0x41;
            set
            {
                Data._unknown0x41 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x42 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x42
        {
            get => Data._unknown0x42;
            set
            {
                Data._unknown0x42 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x43 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x43
        {
            get => Data._unknown0x43;
            set
            {
                Data._unknown0x43 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x44 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x44
        {
            get => Data._unknown0x44;
            set
            {
                Data._unknown0x44 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x48 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x48
        {
            get => Data._unknown0x48;
            set
            {
                Data._unknown0x48 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x4C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x4C
        {
            get => Data._unknown0x4C;
            set
            {
                Data._unknown0x4C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x50 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x50
        {
            get => Data._unknown0x50;
            set
            {
                Data._unknown0x50 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x54 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x54
        {
            get => Data._unknown0x54;
            set
            {
                Data._unknown0x54 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x55 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x55
        {
            get => Data._unknown0x55;
            set
            {
                Data._unknown0x55 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x56 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x56
        {
            get => Data._unknown0x56;
            set
            {
                Data._unknown0x56 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x57 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x57
        {
            get => Data._unknown0x57;
            set
            {
                Data._unknown0x57 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x58 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x58
        {
            get => Data._unknown0x58;
            set
            {
                Data._unknown0x58 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x5C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x5C
        {
            get => Data._unknown0x5C;
            set
            {
                Data._unknown0x5C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x60 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x60
        {
            get => Data._unknown0x60;
            set
            {
                Data._unknown0x60 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x64 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x64
        {
            get => Data._unknown0x64;
            set
            {
                Data._unknown0x64 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x68 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x68
        {
            get => Data._unknown0x68;
            set
            {
                Data._unknown0x68 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x6C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x6C
        {
            get => Data._unknown0x6C;
            set
            {
                Data._unknown0x6C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x70 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x70
        {
            get => Data._unknown0x70;
            set
            {
                Data._unknown0x70 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x74 (float)")]
        [Category("Unknown")]
        public float Unknown0x74
        {
            get => Data._unknown0x74;
            set
            {
                Data._unknown0x74 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x78 (short)")]
        [Category("Unknown")]
        public short Unknown0x78
        {
            get => Data._unknown0x78;
            set
            {
                Data._unknown0x78 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x7A (short)")]
        [Category("Unknown")]
        public short Unknown0x7A
        {
            get => Data._unknown0x7A;
            set
            {
                Data._unknown0x7A = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x7C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x7C
        {
            get => Data._unknown0x7C;
            set
            {
                Data._unknown0x7C = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x80;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x80 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x80
        {
            get => _unknown0x80 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x80 = value;
                Data._unknown0x80 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x84 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x84
        {
            get => Data._unknown0x84;
            set
            {
                Data._unknown0x84 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x85 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x85
        {
            get => Data._unknown0x85;
            set
            {
                Data._unknown0x85 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x86 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x86
        {
            get => Data._unknown0x86;
            set
            {
                Data._unknown0x86 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x87 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x87
        {
            get => Data._unknown0x87;
            set
            {
                Data._unknown0x87 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x88 (float)")]
        [Category("Unknown")]
        public float Unknown0x88
        {
            get => Data._unknown0x88;
            set
            {
                Data._unknown0x88 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x8C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x8C
        {
            get => Data._unknown0x8C;
            set
            {
                Data._unknown0x8C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x8D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x8D
        {
            get => Data._unknown0x8D;
            set
            {
                Data._unknown0x8D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x8E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x8E
        {
            get => Data._unknown0x8E;
            set
            {
                Data._unknown0x8E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x8F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x8F
        {
            get => Data._unknown0x8F;
            set
            {
                Data._unknown0x8F = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GMINEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GMINEntry*) WorkingUncompressed.Address;
            _unknown0x80 = new TriggerDataClass(this, Data._unknown0x80);

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GMINEntry* hdr = (GMINEntry*)address;
            Data._unknown0x80 = _unknown0x80;
            *hdr = Data;
        }
    }
}
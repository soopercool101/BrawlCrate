using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GMWONode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GMWOEntryNode);
        protected override string baseName => "AttackSwitchOnce";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMWO" ? new GMWONode() : null;
        }
    }

    public unsafe class GMWOEntryNode : ResourceNode
    {
        protected internal GMWOEntry Data;

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

        [DisplayName("Unknown0x04 (float)")]
        [Category("Unknown")]
        public float Unknown0x04
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

        [DisplayName("Unknown0x10 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x11 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x11
        {
            get => Data._unknown0x11;
            set
            {
                Data._unknown0x11 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x12 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x12
        {
            get => Data._unknown0x12;
            set
            {
                Data._unknown0x12 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x13 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x13
        {
            get => Data._unknown0x13;
            set
            {
                Data._unknown0x13 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x14 (float)")]
        [Category("Unknown")]
        public float Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x18 (bool)")]
        [Category("Unknown")]
        public bool Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x19 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x19
        {
            get => Data._unknown0x19;
            set
            {
                Data._unknown0x19 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1A (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1A
        {
            get => Data._unknown0x1A;
            set
            {
                Data._unknown0x1A = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1B (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1B
        {
            get => Data._unknown0x1B;
            set
            {
                Data._unknown0x1B = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C (float)")]
        [Category("Unknown")]
        public float Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x20 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x20
        {
            get => Data._unknown0x20;
            set
            {
                Data._unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x21 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x21
        {
            get => Data._unknown0x21;
            set
            {
                Data._unknown0x21 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x22 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x22
        {
            get => Data._unknown0x22;
            set
            {
                Data._unknown0x22 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x23 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x23
        {
            get => Data._unknown0x23;
            set
            {
                Data._unknown0x23 = value;
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

        public TriggerDataClass _unknown0x2C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x2C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x2C
        {
            get => _unknown0x2C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x2C = value;
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x30;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x30 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x30
        {
            get => _unknown0x30 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x30 = value;
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

        [DisplayName("Unknown0x40 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x40
        {
            get => Data._unknown0x40;
            set
            {
                Data._unknown0x40 = value;
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

        [DisplayName("Unknown0x54 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x54
        {
            get => Data._unknown0x54;
            set
            {
                Data._unknown0x54 = value;
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

        [DisplayName("Unknown0x78 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x78
        {
            get => Data._unknown0x78;
            set
            {
                Data._unknown0x78 = value;
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

        [DisplayName("Unknown0x80 (float)")]
        [Category("Unknown")]
        public float Unknown0x80
        {
            get => Data._unknown0x80;
            set
            {
                Data._unknown0x80 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x84 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x84
        {
            get => Data._unknown0x84;
            set
            {
                Data._unknown0x84 = value;
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

        [DisplayName("Unknown0x90 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x90
        {
            get => Data._unknown0x90;
            set
            {
                Data._unknown0x90 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x94 (short)")]
        [Category("Unknown")]
        public short Unknown0x94
        {
            get => Data._unknown0x94;
            set
            {
                Data._unknown0x94 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x96 (short)")]
        [Category("Unknown")]
        public short Unknown0x96
        {
            get => Data._unknown0x96;
            set
            {
                Data._unknown0x96 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GMWOEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GMWOEntry*) WorkingUncompressed.Address;
            _unknown0x2C = new TriggerDataClass(this, Data._unknown0x2C);
            _unknown0x30 = new TriggerDataClass(this, Data._unknown0x30);

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GMWOEntry* hdr = (GMWOEntry*)address;
            Data._unknown0x2C = _unknown0x2C;
            Data._unknown0x30 = _unknown0x30;
            *hdr = Data;
        }
    }
}
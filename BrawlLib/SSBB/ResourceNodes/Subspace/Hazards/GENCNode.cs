using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GENCNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GENCEntryNode);
        protected override string baseName => "Enemy Generators";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GENC" ? new GENCNode() : null;
        }
    }

    public unsafe class GENCEntryNode : ResourceNode
    {
        protected internal GENCEntry Data;

        [DisplayName("Unknown0x00 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x00
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

        [DisplayName("Unknown0x10 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x10
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

        [DisplayName("Unknown0x18 (float)")]
        [Category("Unknown")]
        public float Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C (short)")]
        [Category("Unknown")]
        public short Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1E (short)")]
        [Category("Unknown")]
        public short Unknown0x1E
        {
            get => Data._unknown0x1E;
            set
            {
                Data._unknown0x1E = value;
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

        [DisplayName("Unknown0x24 (float)")]
        [Category("Unknown")]
        public float Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x28 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x29 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x29
        {
            get => Data._unknown0x29;
            set
            {
                Data._unknown0x29 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2A (byte)")]
        [Category("Unknown")]
        public byte Unknown0x2A
        {
            get => Data._unknown0x2A;
            set
            {
                Data._unknown0x2A = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2B (byte)")]
        [Category("Unknown")]
        public byte Unknown0x2B
        {
            get => Data._unknown0x2B;
            set
            {
                Data._unknown0x2B = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2C (float)")]
        [Category("Unknown")]
        public float Unknown0x2C
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x30 (float)")]
        [Category("Unknown")]
        public float Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x34 (float)")]
        [Category("Unknown")]
        public float Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x38 (float)")]
        [Category("Unknown")]
        public float Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x3C (float)")]
        [Category("Unknown")]
        public float Unknown0x3C
        {
            get => Data._unknown0x3C;
            set
            {
                Data._unknown0x3C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x40 (float)")]
        [Category("Unknown")]
        public float Unknown0x40
        {
            get => Data._unknown0x40;
            set
            {
                Data._unknown0x40 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x44 (float)")]
        [Category("Unknown")]
        public float Unknown0x44
        {
            get => Data._unknown0x44;
            set
            {
                Data._unknown0x44 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x48 (float)")]
        [Category("Unknown")]
        public float Unknown0x48
        {
            get => Data._unknown0x48;
            set
            {
                Data._unknown0x48 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x4C (float)")]
        [Category("Unknown")]
        public float Unknown0x4C
        {
            get => Data._unknown0x4C;
            set
            {
                Data._unknown0x4C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x50 (float)")]
        [Category("Unknown")]
        public float Unknown0x50
        {
            get => Data._unknown0x50;
            set
            {
                Data._unknown0x50 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x54 (float)")]
        [Category("Unknown")]
        public float Unknown0x54
        {
            get => Data._unknown0x54;
            set
            {
                Data._unknown0x54 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x58 (float)")]
        [Category("Unknown")]
        public float Unknown0x58
        {
            get => Data._unknown0x58;
            set
            {
                Data._unknown0x58 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x5C (float)")]
        [Category("Unknown")]
        public float Unknown0x5C
        {
            get => Data._unknown0x5C;
            set
            {
                Data._unknown0x5C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x60 (float)")]
        [Category("Unknown")]
        public float Unknown0x60
        {
            get => Data._unknown0x60;
            set
            {
                Data._unknown0x60 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x64 (float)")]
        [Category("Unknown")]
        public float Unknown0x64
        {
            get => Data._unknown0x64;
            set
            {
                Data._unknown0x64 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x68 (float)")]
        [Category("Unknown")]
        public float Unknown0x68
        {
            get => Data._unknown0x68;
            set
            {
                Data._unknown0x68 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x6C (float)")]
        [Category("Unknown")]
        public float Unknown0x6C
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

        [DisplayName("Unknown0x74 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x74
        {
            get => Data._unknown0x74;
            set
            {
                Data._unknown0x74 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x78 (float)")]
        [Category("Unknown")]
        public float Unknown0x78
        {
            get => Data._unknown0x78;
            set
            {
                Data._unknown0x78 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x7C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x7C
        {
            get => Data._unknown0x7C;
            set
            {
                Data._unknown0x7C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x7D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x7D
        {
            get => Data._unknown0x7D;
            set
            {
                Data._unknown0x7D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x7E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x7E
        {
            get => Data._unknown0x7E;
            set
            {
                Data._unknown0x7E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x7F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x7F
        {
            get => Data._unknown0x7F;
            set
            {
                Data._unknown0x7F = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x80 (int)")]
        [Category("Unknown")]
        public int Unknown0x80
        {
            get => Data._unknown0x80;
            set
            {
                Data._unknown0x80 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x84 (int)")]
        [Category("Unknown")]
        public int Unknown0x84
        {
            get => Data._unknown0x84;
            set
            {
                Data._unknown0x84 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x88 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x88
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

        [DisplayName("Unknown0x94 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x94
        {
            get => Data._unknown0x94;
            set
            {
                Data._unknown0x94 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x98 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x98
        {
            get => Data._unknown0x98;
            set
            {
                Data._unknown0x98 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x9C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x9C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x9C
        {
            get => _unknown0x9C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x9C = value;
                Data._unknown0x9C = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0xA0;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0xA0 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0xA0
        {
            get => _unknown0xA0 ?? new TriggerDataClass(this);
            set
            {
                _unknown0xA0 = value;
                Data._unknown0xA0 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0xA4;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0xA4 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0xA4
        {
            get => _unknown0xA4 ?? new TriggerDataClass(this);
            set
            {
                _unknown0xA4 = value;
                Data._unknown0xA4 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0xA8;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0xA8 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0xA8
        {
            get => _unknown0xA8 ?? new TriggerDataClass(this);
            set
            {
                _unknown0xA8 = value;
                Data._unknown0xA8 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GENCEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GENCEntry*) WorkingUncompressed.Address;
            _unknown0x9C = new TriggerDataClass(this, Data._unknown0x9C);
            _unknown0xA0 = new TriggerDataClass(this, Data._unknown0xA0);
            _unknown0xA4 = new TriggerDataClass(this, Data._unknown0xA4);
            _unknown0xA8 = new TriggerDataClass(this, Data._unknown0xA8);

            if (_name == null)
            {
                _name = $"Generator [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GENCEntry* hdr = (GENCEntry*)address;
            Data._unknown0x9C = _unknown0x9C;
            Data._unknown0xA0 = _unknown0xA0;
            Data._unknown0xA4 = _unknown0xA4;
            Data._unknown0xA8 = _unknown0xA8;
            *hdr = Data;
        }
    }
}
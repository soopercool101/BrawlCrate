using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GSBKNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GSBKEntryNode);
        protected override string baseName => "Block Sympathetics";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GSBK" ? new GSBKNode() : null;
        }
    }

    public unsafe class GSBKEntryNode : ResourceNode
    {
        protected internal GSBKEntry Data;

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

        [DisplayName("Unknown0x24 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x25 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x25
        {
            get => Data._unknown0x25;
            set
            {
                Data._unknown0x25 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x26 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x26
        {
            get => Data._unknown0x26;
            set
            {
                Data._unknown0x26 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x27 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x27
        {
            get => Data._unknown0x27;
            set
            {
                Data._unknown0x27 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x28 (short)")]
        [Category("Unknown")]
        public short Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x2A (short)")]
        [Category("Unknown")]
        public short Unknown0x2A
        {
            get => Data._unknown0x2A;
            set
            {
                Data._unknown0x2A = value;
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

        [DisplayName("Unknown0x30 (int)")]
        [Category("Unknown")]
        public int Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x34 (int)")]
        [Category("Unknown")]
        public int Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x38 (bool)")]
        [Category("Unknown")]
        public bool Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x39 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x39
        {
            get => Data._unknown0x39;
            set
            {
                Data._unknown0x39 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x3A (byte)")]
        [Category("Unknown")]
        public byte Unknown0x3A
        {
            get => Data._unknown0x3A;
            set
            {
                Data._unknown0x3A = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x3B (byte)")]
        [Category("Unknown")]
        public byte Unknown0x3B
        {
            get => Data._unknown0x3B;
            set
            {
                Data._unknown0x3B = value;
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

        [DisplayName("Unknown0x40 (short)")]
        [Category("Unknown")]
        public short Unknown0x40
        {
            get => Data._unknown0x40;
            set
            {
                Data._unknown0x40 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x42 (short)")]
        [Category("Unknown")]
        public short Unknown0x42
        {
            get => Data._unknown0x42;
            set
            {
                Data._unknown0x42 = value;
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

        public TriggerDataClass _unknown0x48;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x48 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x48
        {
            get => _unknown0x48 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x48 = value;
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

        public override int OnCalculateSize(bool force)
        {
            return GSBKEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GSBKEntry*) WorkingUncompressed.Address;
            _unknown0x48 = new TriggerDataClass(this, Data._unknown0x48);

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GSBKEntry* hdr = (GSBKEntry*)address;
            Data._unknown0x48 = _unknown0x48;
            *hdr = Data;
        }
    }
}
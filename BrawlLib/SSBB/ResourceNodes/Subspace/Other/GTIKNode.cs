using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Other;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GTIKNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GTIKEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GTIK" ? new GTIKNode() : null;
        }
    }

    public unsafe class GTIKEntryNode : ResourceNode
    {
        protected internal GTIKEntry Data;

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

        [DisplayName("Unknown0x14 (short)")]
        [Category("Unknown")]
        public short Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x16 (short)")]
        [Category("Unknown")]
        public short Unknown0x16
        {
            get => Data._unknown0x16;
            set
            {
                Data._unknown0x16 = value;
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

        [DisplayName("Unknown0x1C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1D
        {
            get => Data._unknown0x1D;
            set
            {
                Data._unknown0x1D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1E
        {
            get => Data._unknown0x1E;
            set
            {
                Data._unknown0x1E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x1F
        {
            get => Data._unknown0x1F;
            set
            {
                Data._unknown0x1F = value;
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

        public override int OnCalculateSize(bool force)
        {
            return GTIKEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GTIKEntry*) WorkingUncompressed.Address;
            _unknown0x2C = new TriggerDataClass(this, Data._unknown0x2C);

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GTIKEntry* hdr = (GTIKEntry*)address;
            Data._unknown0x2C = _unknown0x2C;
            *hdr = Data;
        }
    }
}
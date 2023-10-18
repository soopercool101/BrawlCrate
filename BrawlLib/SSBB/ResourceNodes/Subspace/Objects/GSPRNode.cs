using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GSPRNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GSPREntryNode);
        protected override string baseName => "Springs";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GSPR" ? new GSPRNode() : null;
        }
    }

    public unsafe class GSPREntryNode : ResourceNode
    {
        protected internal GSPREntry Data;

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

        [DisplayName("Unknown0x04 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x05 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x05
        {
            get => Data._unknown0x05;
            set
            {
                Data._unknown0x05 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x06 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x06
        {
            get => Data._unknown0x06;
            set
            {
                Data._unknown0x06 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x07 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x07
        {
            get => Data._unknown0x07;
            set
            {
                Data._unknown0x07 = value;
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

        [DisplayName("Unknown0x28 (float)")]
        [Category("Unknown")]
        public float Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
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

        [DisplayName("Unknown0x48 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x48
        {
            get => Data._unknown0x48;
            set
            {
                Data._unknown0x48 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x49 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x49
        {
            get => Data._unknown0x49;
            set
            {
                Data._unknown0x49 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x4A (byte)")]
        [Category("Unknown")]
        public byte Unknown0x4A
        {
            get => Data._unknown0x4A;
            set
            {
                Data._unknown0x4A = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x4B (byte)")]
        [Category("Unknown")]
        public byte Unknown0x4B
        {
            get => Data._unknown0x4B;
            set
            {
                Data._unknown0x4B = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x4C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x4C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x4C
        {
            get => _unknown0x4C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x4C = value;
                Data._unknown0x4C = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GSPREntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GSPREntry*) WorkingUncompressed.Address;
            _unknown0x4C = new TriggerDataClass(this, Data._unknown0x4C);

            if (_name == null)
            {
                _name = $"Spring [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GSPREntry* hdr = (GSPREntry*)address;
            Data._unknown0x4C = _unknown0x4C;
            *hdr = Data;
        }
    }
}
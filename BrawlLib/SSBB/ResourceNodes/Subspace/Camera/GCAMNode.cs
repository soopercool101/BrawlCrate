using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Camera;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GCAMNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GCAMEntryNode);
        protected override string baseName => "Camera Demos";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GCAM" ? new GCAMNode() : null;
        }
    }

    public unsafe class GCAMEntryNode : ResourceNode
    {
        protected internal GCAMEntry Data;

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

        [DisplayName("Unknown0x08 (float)")]
        [Category("Unknown")]
        public float Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0D
        {
            get => Data._unknown0x0D;
            set
            {
                Data._unknown0x0D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0E
        {
            get => Data._unknown0x0E;
            set
            {
                Data._unknown0x0E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0F
        {
            get => Data._unknown0x0F;
            set
            {
                Data._unknown0x0F = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x10;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x10 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x10
        {
            get => _unknown0x10 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x10 = value;
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x14;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x14 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x14
        {
            get => _unknown0x14 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x14 = value;
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

        public override int OnCalculateSize(bool force)
        {
            return GCAMEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GCAMEntry*) WorkingUncompressed.Address;
            _unknown0x10 = new TriggerDataClass(this, Data._unknown0x10);
            _unknown0x14 = new TriggerDataClass(this, Data._unknown0x14);

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GCAMEntry* hdr = (GCAMEntry*)address;
            Data._unknown0x10 = _unknown0x10;
            Data._unknown0x14 = _unknown0x14;
            *hdr = Data;
        }
    }
}
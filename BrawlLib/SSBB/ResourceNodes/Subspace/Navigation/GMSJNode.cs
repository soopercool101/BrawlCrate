using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Navigation;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GMSJNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GMSJEntryNode);
        protected override string baseName => "Step Jumps";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMSJ" ? new GMSJNode() : null;
        }
    }

    public unsafe class GMSJEntryNode : ResourceNode
    {
        protected internal GMSJEntry Data;

        [DisplayName("Unknown0x00 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x00
        {
            get => Data._unknown0x00;
            set
            {
                Data._unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x01 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x01
        {
            get => Data._unknown0x01;
            set
            {
                Data._unknown0x01 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x02 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x02
        {
            get => Data._unknown0x02;
            set
            {
                Data._unknown0x02 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x03 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._unknown0x03;
            set
            {
                Data._unknown0x03 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x04 (short)")]
        [Category("Unknown")]
        public short Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x06 (short)")]
        [Category("Unknown")]
        public short Unknown0x06
        {
            get => Data._unknown0x06;
            set
            {
                Data._unknown0x06 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x08;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x08 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x08
        {
            get => _unknown0x08 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x08 = value;
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GMSJEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GMSJEntry*) WorkingUncompressed.Address;
            _unknown0x08 = new TriggerDataClass(this, Data._unknown0x08);

            if (_name == null)
            {
                _name = $"Step Jump [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GMSJEntry* hdr = (GMSJEntry*)address;
            Data._unknown0x08 = _unknown0x08;
            *hdr = Data;
        }
    }
}
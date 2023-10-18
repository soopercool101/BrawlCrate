using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFRTNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFRTEntryNode);
        protected override string baseName => "Frame Triggers";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFRT" ? new GFRTNode() : null;
        }
    }

    public unsafe class GFRTEntryNode : ResourceNode
    {
        protected internal GFRTEntry Data;

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

        public TriggerDataClass _unknown0x0C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x0C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x0C
        {
            get => _unknown0x0C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x0C = value;
                Data._unknown0x0C = value;
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

        public override int OnCalculateSize(bool force)
        {
            return GFRTEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GFRTEntry*) WorkingUncompressed.Address;
            _unknown0x0C = new TriggerDataClass(this, Data._unknown0x0C);
            _unknown0x10 = new TriggerDataClass(this, Data._unknown0x10);

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFRTEntry* hdr = (GFRTEntry*)address;
            Data._unknown0x0C = _unknown0x0C;
            Data._unknown0x10 = _unknown0x10;
            *hdr = Data;
        }
    }
}
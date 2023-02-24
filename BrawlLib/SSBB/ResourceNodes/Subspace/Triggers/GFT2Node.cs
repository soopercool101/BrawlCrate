using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFT2Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFT2EntryNode);
        protected override string baseName => "Frame Trigger 2";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFT2" ? new GFT2Node() : null;
        }
    }

    public unsafe class GFT2EntryNode : ResourceNode
    {
        internal GFT2Entry* Header => (GFT2Entry*)WorkingUncompressed.Address;
        public override bool supportsCompression => false;

        public float _unknown0x00;
        [Category("Unknown")]
        public float Unknown0x00
        {
            get => _unknown0x00;
            set
            {
                _unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger1;

        [Category("GFT2")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger1
        {
            get => _trigger1;
            set
            {
                _trigger1 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger2;

        [Category("GFT2")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger2
        {
            get => _trigger2;
            set
            {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        public GFT2EntryNode()
        {
            _trigger1 = new TriggerDataClass(this);
            _trigger2 = new TriggerDataClass(this);
        }

        public override int OnCalculateSize(bool force)
        {
            return GFT2Entry.Size;
        }

        public override bool OnInitialize()
        {
            _unknown0x00 = Header->_unknown0x00;
            _trigger1 = new TriggerDataClass(this, Header->_trigger1);
            _trigger2 = new TriggerDataClass(this, Header->_trigger2);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFT2Entry* hdr = (GFT2Entry*)address;
            hdr->_unknown0x00 = _unknown0x00;
            hdr->_trigger1 = _trigger1;
            hdr->_trigger2 = _trigger2;
        }
    }
}

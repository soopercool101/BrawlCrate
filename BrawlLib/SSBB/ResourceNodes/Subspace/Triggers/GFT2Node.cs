using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFT2Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFT2EntryNode);

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
        [Category("GFT2 Entry")]
        public float Unknown0x00
        {
            get => _unknown0x00;
            set
            {
                _unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        public uint _trigger1; // 0x04

        [Category("GFT2 Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger1
        {
            get => _trigger1;
            set
            {
                _trigger1 = value;
                SignalPropertyChange();
            }
        }

        public uint _trigger2; // 0x08

        [Category("GFT2 Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint Trigger2
        {
            get => _trigger2;
            set
            {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GFT2Entry.Size;
        }

        public override bool OnInitialize()
        {
            _unknown0x00 = Header->_unknown0x00;
            _trigger1 = Header->_trigger1;
            _trigger2 = Header->_trigger2;

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

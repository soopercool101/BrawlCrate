using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGCNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBGC;
        public override Type SubEntryType => typeof(TBGCEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGC" ? new TBGCNode() : null;
        }
    }

    public unsafe class TBGCEntryNode : ResourceNode
    {
        public byte _unk0x00;

        [Category("TBGCEntry")]
        public byte Unknown0x00
        {
            get => _unk0x00;
            set
            {
                _unk0x00 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x01;

        [Category("TBGCEntry")]
        public byte Unknown0x01
        {
            get => _unk0x01;
            set
            {
                _unk0x01 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x02;

        [Category("TBGCEntry")]
        public byte Unknown0x02
        {
            get => _unk0x02;
            set
            {
                _unk0x02 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x03;

        [Category("TBGCEntry")]
        public byte Unknown0x03
        {
            get => _unk0x03;
            set
            {
                _unk0x03 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x04;

        [Category("TBGCEntry")]
        public byte Unknown0x04
        {
            get => _unk0x04;
            set
            {
                _unk0x04 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x05;

        [Category("TBGCEntry")]
        public byte Unknown0x05
        {
            get => _unk0x05;
            set
            {
                _unk0x05 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x06;

        [Category("TBGCEntry")]
        public byte Unknown0x06
        {
            get => _unk0x06;
            set
            {
                _unk0x06 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x07;

        [Category("TBGCEntry")]
        public byte Unknown0x07
        {
            get => _unk0x07;
            set
            {
                _unk0x07 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TBGCEntry.Size;
        }

        public override bool OnInitialize()
        {
            TBGCEntry* header = (TBGCEntry*)WorkingUncompressed.Address;

            _unk0x00 = header->_unk0x00;
            _unk0x01 = header->_unk0x01;
            _unk0x02 = header->_unk0x02;
            _unk0x03 = header->_unk0x03;
            _unk0x04 = header->_unk0x04;
            _unk0x05 = header->_unk0x05;
            _unk0x06 = header->_unk0x06;
            _unk0x07 = header->_unk0x07;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBGCEntry* hdr = (TBGCEntry*)address;
            hdr->_unk0x00 = _unk0x00;
            hdr->_unk0x01 = _unk0x01;
            hdr->_unk0x02 = _unk0x02;
            hdr->_unk0x03 = _unk0x03;
            hdr->_unk0x04 = _unk0x04;
            hdr->_unk0x05 = _unk0x05;
            hdr->_unk0x06 = _unk0x06;
            hdr->_unk0x07 = _unk0x07;
        }
    }
}
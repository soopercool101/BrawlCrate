using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGMNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBGM;
        public override Type SubEntryType => typeof(TBGMEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGM" ? new TBGMNode() : null;
        }
    }

    public unsafe class TBGMEntryNode : ResourceNode
    {
        public float _unk0x00;

        [Category("TBGM Entry")]
        public float Unknown0x00
        {
            get => _unk0x00;
            set
            {
                _unk0x00 = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x04;

        [Category("TBGM Entry")]
        public float Unknown0x04
        {
            get => _unk0x04;
            set
            {
                _unk0x04 = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x08;

        [Category("TBGM Entry")]
        public float Unknown0x08
        {
            get => _unk0x08;
            set
            {
                _unk0x08 = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x0C;

        [Category("TBGM Entry")]
        public float Unknown0x0C
        {
            get => _unk0x0C;
            set
            {
                _unk0x0C = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x10;

        [Category("TBGM Entry")]
        public float Unknown0x10
        {
            get => _unk0x10;
            set
            {
                _unk0x10 = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x14;

        [Category("TBGM Entry")]
        public float Unknown0x14
        {
            get => _unk0x14;
            set
            {
                _unk0x14 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TBGMEntry.Size;
        }

        public override bool OnInitialize()
        {
            TBGMEntry* header = (TBGMEntry*)WorkingUncompressed.Address;

            _unk0x00 = header->_unk0x00;
            _unk0x04 = header->_unk0x04;
            _unk0x08 = header->_unk0x08;
            _unk0x0C = header->_unk0x0C;
            _unk0x10 = header->_unk0x10;
            _unk0x14 = header->_unk0x14;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBGMEntry* hdr = (TBGMEntry*)address;

            hdr->_unk0x00 = _unk0x00;
            hdr->_unk0x04 = _unk0x04;
            hdr->_unk0x08 = _unk0x08;
            hdr->_unk0x0C = _unk0x0C;
            hdr->_unk0x10 = _unk0x10;
            hdr->_unk0x14 = _unk0x14;
        }
    }
}
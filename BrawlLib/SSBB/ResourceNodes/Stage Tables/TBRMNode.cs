using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBRMNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBRM;
        public override Type SubEntryType => typeof(TBRMEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBRM" ? new TBRMNode() : null;
        }
    }

    public unsafe class TBRMEntryNode : ResourceNode
    {
        public int _unk0x00;

        [Category("TBRMEntry")]
        public int Unknown0x00
        {
            get => _unk0x00;
            set
            {
                _unk0x00 = value;
                SignalPropertyChange();
            }
        }

        public short _unk0x04;

        [Category("TBRMEntry")]
        public short Unknown0x04
        {
            get => _unk0x04;
            set
            {
                _unk0x04 = value;
                SignalPropertyChange();
            }
        }

        public short _unk0x06;

        [Category("TBRMEntry")]
        public short Unknown0x06
        {
            get => _unk0x06;
            set
            {
                _unk0x06 = value;
                SignalPropertyChange();
            }
        }

        public short _unk0x08;

        [Category("TBRMEntry")]
        public short Unknown0x08
        {
            get => _unk0x08;
            set
            {
                _unk0x08 = value;
                SignalPropertyChange();
            }
        }

        public short _unk0x0A;

        [Category("TBRMEntry")]
        public short Unknown0x0A
        {
            get => _unk0x0A;
            set
            {
                _unk0x0A = value;
                SignalPropertyChange();
            }
        }

        public short _unk0x0C;

        [Category("TBRMEntry")]
        public short Unknown0x0C
        {
            get => _unk0x0C;
            set
            {
                _unk0x0C = value;
                SignalPropertyChange();
            }
        }

        public short _unk0x0E;

        [Category("TBRMEntry")]
        public short Unknown0x0E
        {
            get => _unk0x0E;
            set
            {
                _unk0x0E = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x10;

        [Category("TBRMEntry")]
        public float Unknown0x10
        {
            get => _unk0x10;
            set
            {
                _unk0x10 = value;
                SignalPropertyChange();
            }
        }


        public override int OnCalculateSize(bool force)
        {
            return TBRMEntry.Size;
        }

        public override bool OnInitialize()
        {
            TBRMEntry* header = (TBRMEntry*)WorkingUncompressed.Address;

            _unk0x00 = header->_unk0x00;
            _unk0x04 = header->_unk0x04;
            _unk0x06 = header->_unk0x06;
            _unk0x08 = header->_unk0x08;
            _unk0x0A = header->_unk0x0A;
            _unk0x0C = header->_unk0x0C;
            _unk0x0E = header->_unk0x0E;
            _unk0x10 = header->_unk0x10;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBRMEntry* hdr = (TBRMEntry*)address;
            hdr->_unk0x00 = _unk0x00;
            hdr->_unk0x04 = _unk0x04;
            hdr->_unk0x06 = _unk0x06;
            hdr->_unk0x08 = _unk0x08;
            hdr->_unk0x0A = _unk0x0A;
            hdr->_unk0x0C = _unk0x0C;
            hdr->_unk0x0E = _unk0x0E;
            hdr->_unk0x10 = _unk0x10;
        }
    }
}
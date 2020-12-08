using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGDNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBGD;
        public override Type SubEntryType => typeof(TBGDEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGD" ? new TBGDNode() : null;
        }
    }

    public unsafe class TBGDEntryNode : ResourceNode
    {
        public byte _unk0x00;

        [Category("TBGDEntry")]
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

        [Category("TBGDEntry")]
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

        [Category("TBGDEntry")]
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

        [Category("TBGDEntry")]
        public byte Unknown0x03
        {
            get => _unk0x03;
            set
            {
                _unk0x03 = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x04;

        [Category("TBGDEntry")]
        public float Unknown0x04
        {
            get => _unk0x04;
            set
            {
                _unk0x04 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x08;

        [Category("TBGDEntry")]
        public byte Unknown0x08
        {
            get => _unk0x08;
            set
            {
                _unk0x08 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x09;

        [Category("TBGDEntry")]
        public byte Unknown0x09
        {
            get => _unk0x09;
            set
            {
                _unk0x09 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x0A;

        [Category("TBGDEntry")]
        public byte Unknown0x0A
        {
            get => _unk0x0A;
            set
            {
                _unk0x0A = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x0B;

        [Category("TBGDEntry")]
        public byte Unknown0x0B
        {
            get => _unk0x0B;
            set
            {
                _unk0x0B = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x0C;

        [Category("TBGDEntry")]
        public byte Unknown0x0C
        {
            get => _unk0x0C;
            set
            {
                _unk0x0C = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x0D;

        [Category("TBGDEntry")]
        public byte Unknown0x0D
        {
            get => _unk0x0D;
            set
            {
                _unk0x0D = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x0E;

        [Category("TBGDEntry")]
        public byte Unknown0x0E
        {
            get => _unk0x0E;
            set
            {
                _unk0x0E = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x0F;

        [Category("TBGDEntry")]
        public byte Unknown0x0F
        {
            get => _unk0x0F;
            set
            {
                _unk0x0F = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x10;

        [Category("TBGDEntry")]
        public byte Unknown0x10
        {
            get => _unk0x10;
            set
            {
                _unk0x10 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x11;

        [Category("TBGDEntry")]
        public byte Unknown0x11
        {
            get => _unk0x11;
            set
            {
                _unk0x11 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x12;

        [Category("TBGDEntry")]
        public byte Unknown0x12
        {
            get => _unk0x12;
            set
            {
                _unk0x12 = value;
                SignalPropertyChange();
            }
        }

        public byte _unk0x13;

        [Category("TBGDEntry")]
        public byte Unknown0x13
        {
            get => _unk0x13;
            set
            {
                _unk0x13 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TBGDEntry.Size;
        }

        public override bool OnInitialize()
        {
            TBGDEntry* header = (TBGDEntry*)WorkingUncompressed.Address;

            _unk0x00 = header->_unk0x00;
            _unk0x01 = header->_unk0x01;
            _unk0x02 = header->_unk0x02;
            _unk0x03 = header->_unk0x03;
            _unk0x04 = header->_unk0x04;
            _unk0x08 = header->_unk0x08;
            _unk0x09 = header->_unk0x09;
            _unk0x0A = header->_unk0x0A;
            _unk0x0B = header->_unk0x0B;
            _unk0x0C = header->_unk0x0C;
            _unk0x0D = header->_unk0x0D;
            _unk0x0E = header->_unk0x0E;
            _unk0x0F = header->_unk0x0F;
            _unk0x10 = header->_unk0x10;
            _unk0x11 = header->_unk0x11;
            _unk0x12 = header->_unk0x12;
            _unk0x13 = header->_unk0x13;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBGDEntry* hdr = (TBGDEntry*)address;
            hdr->_unk0x00 = _unk0x00;
            hdr->_unk0x01 = _unk0x01;
            hdr->_unk0x02 = _unk0x02;
            hdr->_unk0x03 = _unk0x03;
            hdr->_unk0x04 = _unk0x04;
            hdr->_unk0x08 = _unk0x08;
            hdr->_unk0x09 = _unk0x09;
            hdr->_unk0x0A = _unk0x0A;
            hdr->_unk0x0B = _unk0x0B;
            hdr->_unk0x0C = _unk0x0C;
            hdr->_unk0x0D = _unk0x0D;
            hdr->_unk0x0E = _unk0x0E;
            hdr->_unk0x0F = _unk0x0F;
            hdr->_unk0x10 = _unk0x10;
            hdr->_unk0x11 = _unk0x11;
            hdr->_unk0x12 = _unk0x12;
            hdr->_unk0x13 = _unk0x13;
        }
    }
}
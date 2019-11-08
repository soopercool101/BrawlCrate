using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Animation;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GCAMNode : ResourceNode
    {
        internal GCAM* Header => (GCAM*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.GCAM;

        [Category("GCAM")]
        [DisplayName("Entry Count")]
        public int EntryCount => Children?.Count ?? 0;

        private const int _entrySize = 0x23; // The constant size of a child entry

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new GCAMEntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x08 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GCAM* header = (GCAM*) address;
            *header = new GCAM(Children.Count);
            uint offset = (uint) (0x08 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*) (address + 0x08 + i * 4) = offset;
                r.Rebuild(address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Animated Camera";
            }

            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GCAM*) source.Address)->_tag == GCAM.Tag ? new GCAMNode() : null;
        }
    }

    public unsafe class GCAMEntryNode : ResourceNode
    {
        internal GCAMEntry* Header => (GCAMEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public byte _unknown0x00;
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;
        public byte _unknown0x04;
        public byte _unknown0x05;
        public byte _modeldataid1;
        public byte _unknown0x07;
        public byte _unknown0x08;
        public byte _unknown0x09;
        public byte _unknown0x0A;
        public byte _unknown0x0B;
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _modeldataid2; // 0x0E
        public byte _unknown0x0F;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public byte _unknown0x14;
        public byte _flag0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public byte _unknown0x18;
        public byte _unknown0x19;
        public byte _unknown0x1A;
        public byte _unknown0x1B;
        public byte _unknown0x1C;
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public byte _unknown0x20;
        public byte _unknown0x21;
        public byte _unknown0x22;
        public byte _unknown0x23;
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;
        public byte _unknown0x27;
        public byte _unknown0x28;
        public byte _unknown0x29;
        public byte _unknown0x2A;
        public byte _unknown0x2B;
        public byte _unknown0x2C;
        public byte _unknown0x2D;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public byte _unknown0x30;
        public byte _unknown0x31;
        public byte _unknown0x32;
        public byte _unknown0x33;
        public byte _unknown0x34;
        public byte _unknown0x35;
        public byte _unknown0x36;
        public byte _unknown0x37;

        [Category("Camera")]
        [DisplayName("1st Model Data ID")]
        public byte ModelDataID1
        {
            get => _modeldataid1;
            set
            {
                _modeldataid1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Camera")]
        [DisplayName("2nd Model Data ID")]
        public byte ModelDataID2
        {
            get => _modeldataid2;
            set
            {
                _modeldataid2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Camera")]
        [DisplayName("Unknown Flag")]
        public byte Flag15
        {
            get => _flag0x15;
            set
            {
                _flag0x15 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _unknown0x00 = Header->_unknown0x00;
            _unknown0x01 = Header->_unknown0x01;
            _unknown0x02 = Header->_unknown0x02;
            _unknown0x03 = Header->_unknown0x03;
            _unknown0x04 = Header->_unknown0x04;
            _unknown0x05 = Header->_unknown0x05;
            _modeldataid1 = Header->_modeldataid1;
            _unknown0x07 = Header->_unknown0x07;
            _unknown0x08 = Header->_unknown0x08;
            _unknown0x09 = Header->_unknown0x09;
            _unknown0x0A = Header->_unknown0x0A;
            _unknown0x0B = Header->_unknown0x0B;
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x0D = Header->_unknown0x0D;
            _modeldataid2 = Header->_modeldataid2;
            _unknown0x0F = Header->_unknown0x0F;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x11 = Header->_unknown0x11;
            _unknown0x12 = Header->_unknown0x12;
            _unknown0x13 = Header->_unknown0x13;
            _unknown0x14 = Header->_unknown0x14;
            _flag0x15 = Header->_flag0x15;
            _unknown0x16 = Header->_unknown0x16;
            _unknown0x17 = Header->_unknown0x17;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x19 = Header->_unknown0x19;
            _unknown0x1A = Header->_unknown0x1A;
            _unknown0x1B = Header->_unknown0x1B;
            _unknown0x1C = Header->_unknown0x1C;
            _unknown0x1D = Header->_unknown0x1D;
            _unknown0x1E = Header->_unknown0x1E;
            _unknown0x1F = Header->_unknown0x1F;
            _unknown0x20 = Header->_unknown0x20;
            _unknown0x21 = Header->_unknown0x21;
            _unknown0x22 = Header->_unknown0x22;
            _unknown0x23 = Header->_unknown0x23;
            _unknown0x24 = Header->_unknown0x24;
            _unknown0x25 = Header->_unknown0x25;
            _unknown0x26 = Header->_unknown0x26;
            _unknown0x27 = Header->_unknown0x27;
            _unknown0x28 = Header->_unknown0x28;
            _unknown0x29 = Header->_unknown0x29;
            _unknown0x2A = Header->_unknown0x2A;
            _unknown0x2B = Header->_unknown0x2B;
            _unknown0x2C = Header->_unknown0x2C;
            _unknown0x2D = Header->_unknown0x2D;
            _unknown0x2E = Header->_unknown0x2E;
            _unknown0x2F = Header->_unknown0x2F;
            _unknown0x30 = Header->_unknown0x30;
            _unknown0x31 = Header->_unknown0x31;
            _unknown0x32 = Header->_unknown0x32;
            _unknown0x33 = Header->_unknown0x33;
            _unknown0x34 = Header->_unknown0x34;
            _unknown0x35 = Header->_unknown0x35;
            _unknown0x36 = Header->_unknown0x36;
            _unknown0x37 = Header->_unknown0x37;
            if (_name == null)
            {
                _name = "Camera [" + Index + "]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x23;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GCAMEntry* hdr = (GCAMEntry*) address;
            hdr->_unknown0x00 = _unknown0x00;
            hdr->_unknown0x01 = _unknown0x01;
            hdr->_unknown0x02 = _unknown0x02;
            hdr->_unknown0x03 = _unknown0x03;
            hdr->_unknown0x04 = _unknown0x04;
            hdr->_unknown0x05 = _unknown0x05;
            hdr->_modeldataid1 = _modeldataid1;
            hdr->_unknown0x07 = _unknown0x07;
            hdr->_unknown0x08 = _unknown0x08;
            hdr->_unknown0x09 = _unknown0x09;
            hdr->_unknown0x0A = _unknown0x0A;
            hdr->_unknown0x0B = _unknown0x0B;
            hdr->_unknown0x0C = _unknown0x0C;
            hdr->_unknown0x0D = _unknown0x0D;
            hdr->_modeldataid2 = _modeldataid2;
            hdr->_unknown0x0F = _unknown0x0F;
            hdr->_unknown0x10 = _unknown0x10;
            hdr->_unknown0x11 = _unknown0x11;
            hdr->_unknown0x12 = _unknown0x12;
            hdr->_unknown0x13 = _unknown0x13;
            hdr->_unknown0x14 = _unknown0x14;
            hdr->_flag0x15 = _flag0x15;
            hdr->_unknown0x16 = _unknown0x16;
            hdr->_unknown0x17 = _unknown0x17;
            hdr->_unknown0x18 = _unknown0x18;
            hdr->_unknown0x19 = _unknown0x19;
            hdr->_unknown0x1A = _unknown0x1A;
            hdr->_unknown0x1B = _unknown0x1B;
            hdr->_unknown0x1C = _unknown0x1C;
            hdr->_unknown0x1D = _unknown0x1D;
            hdr->_unknown0x1E = _unknown0x1E;
            hdr->_unknown0x1F = _unknown0x1F;
            hdr->_unknown0x20 = _unknown0x20;
            hdr->_unknown0x21 = _unknown0x21;
            hdr->_unknown0x22 = _unknown0x22;
            hdr->_unknown0x23 = _unknown0x23;
            hdr->_unknown0x24 = _unknown0x24;
            hdr->_unknown0x25 = _unknown0x25;
            hdr->_unknown0x26 = _unknown0x26;
            hdr->_unknown0x27 = _unknown0x27;
            hdr->_unknown0x28 = _unknown0x28;
            hdr->_unknown0x29 = _unknown0x29;
            hdr->_unknown0x2A = _unknown0x2A;
            hdr->_unknown0x2B = _unknown0x2B;
            hdr->_unknown0x2C = _unknown0x2C;
            hdr->_unknown0x2C = _unknown0x2C;
            hdr->_unknown0x2D = _unknown0x2D;
            hdr->_unknown0x2E = _unknown0x2E;
            hdr->_unknown0x2F = _unknown0x2F;
            hdr->_unknown0x30 = _unknown0x30;
            hdr->_unknown0x31 = _unknown0x31;
            hdr->_unknown0x32 = _unknown0x32;
            hdr->_unknown0x33 = _unknown0x33;
            hdr->_unknown0x34 = _unknown0x34;
            hdr->_unknown0x35 = _unknown0x35;
            hdr->_unknown0x36 = _unknown0x36;
            hdr->_unknown0x37 = _unknown0x37;
        }
    }
}
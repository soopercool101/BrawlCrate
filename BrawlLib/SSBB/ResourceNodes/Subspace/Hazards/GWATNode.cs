using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GWATNode : ResourceNode
    {
        internal GWAT* Header => (GWAT*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.GWAT;

        [Category("GWAT")]
        [DisplayName("Entry Count")]
        public int Count => Children?.Count ?? 0;

        private const int _entrySize = 0x38; // The constant size of a child entry

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

                new GWATEntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x08 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GWAT* header = (GWAT*) address;
            *header = new GWAT(Children.Count);
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
                _name = "Swimmable Water";
            }

            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GWAT*) source.Address)->_tag == GWAT.Tag ? new GWATNode() : null;
        }
    }

    public unsafe class GWATEntryNode : ResourceNode
    {
        internal GWATEntry* Header => (GWATEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public byte _unknown0x00;
        public byte _unknown0x01;
        public byte _unknown0x02;
        public byte _unknown0x03;
        public byte _unknown0x04;
        public byte _unknown0x05;
        public byte _unknown0x06;
        public byte _unknown0x07;
        public byte _unknown0x08;
        public byte _unknown0x09;
        public byte _unknown0x0A;
        public byte _unknown0x0B;
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _unknown0x0E;
        public byte _unknown0x0F;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public byte _unknown0x14;
        public byte _unknown0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public float _posX;      // 0x18
        public float _float0x1C; // 0x1C
        public float _width;     // 0x20
        public float _float0x24; // 0x24
        public float _posY;      // 0x28
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

        public Vector2 _waterPos;

        [Category("Swimmable Water")]
        [DisplayName("Position")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 WaterPosition
        {
            get => _waterPos;
            set
            {
                _waterPos = value;
                SignalPropertyChange();
            }
        }

        [Category("Swimmable Water")]
        [DisplayName("Width")]
        public float WaterWidth
        {
            get => _width;
            set
            {
                _width = value;
                SignalPropertyChange();
            }
        }

        [Category("Swimmable Water")]
        [DisplayName("Depth")]
        public float WaterDepth
        {
            get => _float0x1C;
            set
            {
                _float0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("Swimmable Water")]
        [DisplayName("Float 0x24")]
        public float Float0x24
        {
            get => _float0x24;
            set
            {
                _float0x24 = value;
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
            _unknown0x06 = Header->_unknown0x06;
            _unknown0x07 = Header->_unknown0x07;
            _unknown0x08 = Header->_unknown0x08;
            _unknown0x09 = Header->_unknown0x09;
            _unknown0x0A = Header->_unknown0x0A;
            _unknown0x0B = Header->_unknown0x0B;
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x0D = Header->_unknown0x0D;
            _unknown0x0E = Header->_unknown0x0E;
            _unknown0x0F = Header->_unknown0x0F;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x11 = Header->_unknown0x11;
            _unknown0x12 = Header->_unknown0x12;
            _unknown0x13 = Header->_unknown0x13;
            _unknown0x14 = Header->_unknown0x14;
            _unknown0x15 = Header->_unknown0x15;
            _unknown0x16 = Header->_unknown0x16;
            _unknown0x17 = Header->_unknown0x17;
            _posX = Header->_posX;
            _float0x1C = Header->_float0x1C;
            _width = Header->_width;
            _float0x24 = Header->_float0x24;
            _posY = Header->_posY;
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
            _waterPos._x = _posX;
            _waterPos._y = _posY;
            if (_name == null)
            {
                _name = "Water [" + Index + "]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x38;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GWATEntry* hdr = (GWATEntry*) address;
            hdr->_unknown0x00 = _unknown0x00;
            hdr->_unknown0x01 = _unknown0x01;
            hdr->_unknown0x02 = _unknown0x02;
            hdr->_unknown0x03 = _unknown0x03;
            hdr->_unknown0x04 = _unknown0x04;
            hdr->_unknown0x05 = _unknown0x05;
            hdr->_unknown0x06 = _unknown0x06;
            hdr->_unknown0x07 = _unknown0x07;
            hdr->_unknown0x08 = _unknown0x08;
            hdr->_unknown0x09 = _unknown0x09;
            hdr->_unknown0x0A = _unknown0x0A;
            hdr->_unknown0x0B = _unknown0x0B;
            hdr->_unknown0x0C = _unknown0x0C;
            hdr->_unknown0x0D = _unknown0x0D;
            hdr->_unknown0x0E = _unknown0x0E;
            hdr->_unknown0x0F = _unknown0x0F;
            hdr->_unknown0x10 = _unknown0x10;
            hdr->_unknown0x11 = _unknown0x11;
            hdr->_unknown0x12 = _unknown0x12;
            hdr->_unknown0x13 = _unknown0x13;
            hdr->_unknown0x14 = _unknown0x14;
            hdr->_unknown0x15 = _unknown0x15;
            hdr->_unknown0x16 = _unknown0x16;
            hdr->_unknown0x17 = _unknown0x17;
            hdr->_posX = _waterPos._x;
            hdr->_float0x1C = _float0x1C;
            hdr->_width = _width;
            hdr->_float0x24 = _float0x24;
            hdr->_posY = _waterPos._y;
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
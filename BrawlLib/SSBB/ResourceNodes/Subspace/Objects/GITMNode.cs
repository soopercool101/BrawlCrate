using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GITMNode : ResourceNode
    {
        internal GITM* Header => (GITM*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.GITM;

        [Category("GITM")]
        [DisplayName("Entry Count")]
        public int Count => Children?.Count ?? 0;

        private const int _entrySize = 0x17; // The constant size of a child entry

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

                new GITMEntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x08 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GITM* header = (GITM*) address;
            *header = new GITM(Children.Count);
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
                _name = "Fighter Trophies (unlock)";
            }

            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GITM*) source.Address)->_tag == GITM.Tag ? new GITMNode() : null;
        }
    }

    public unsafe class GITMEntryNode : ResourceNode
    {
        internal GITMEntry* Header => (GITMEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public bfloat _xpos;
        public bfloat _ypos;
        public byte _unknown0;
        public byte _unknown1;
        public byte _unknown2;
        public byte _unknown3;
        public byte _unknown4;
        public byte _unknown5;
        public byte _unknown6;
        public byte _unknown7;
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

        [Category("Trophy")]
        [DisplayName("Position X")]
        public float PosX
        {
            get => _xpos;
            set
            {
                _xpos = value;
                SignalPropertyChange();
            }
        }

        [Category("Trophy")]
        [DisplayName("Position Y")]
        public float PosY
        {
            get => _ypos;
            set
            {
                _ypos = value;
                SignalPropertyChange();
            }
        }

        [Category("Trophy")]
        [DisplayName("Unknown3")]
        public byte Unk3
        {
            get => _unknown3;
            set
            {
                _unknown3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Trophy")]
        [DisplayName("Unknown4")]
        public byte Unk4
        {
            get => _unknown4;
            set
            {
                _unknown4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Trophy")]
        [DisplayName("Unknown5")]
        public byte Unk5
        {
            get => _unknown5;
            set
            {
                _unknown5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Trophy")]
        [DisplayName("Unknown6")]
        public byte Unk6
        {
            get => _unknown6;
            set
            {
                _unknown6 = value;
                SignalPropertyChange();
            }
        }

        [Category("Trophy")]
        [DisplayName("Unknown7")]
        public byte Unk7
        {
            get => _unknown7;
            set
            {
                _unknown7 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _xpos = Header->_xpos;
            _ypos = Header->_ypos;
            _unknown0 = Header->_unknown0;
            _unknown1 = Header->_unknown1;
            _unknown2 = Header->_unknown2;
            _unknown3 = Header->_unknown3;
            _unknown4 = Header->_unknown4;
            _unknown5 = Header->_unknown5;
            _unknown6 = Header->_unknown6;
            _unknown7 = Header->_unknown7;
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
                _name = "Trophy [" + Index + "]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x17;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GITMEntry* hdr = (GITMEntry*) address;
            hdr->_xpos = _xpos;
            hdr->_ypos = _ypos;
            hdr->_unknown0 = _unknown0;
            hdr->_unknown1 = _unknown1;
            hdr->_unknown2 = _unknown2;
            hdr->_unknown3 = _unknown3;
            hdr->_unknown4 = _unknown4;
            hdr->_unknown5 = _unknown5;
            hdr->_unknown6 = _unknown6;
            hdr->_unknown7 = _unknown7;
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
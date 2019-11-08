using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GIB2Node : ResourceNode
    {
        internal GIB2* Header => (GIB2*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.GIB2;

        [Category("GIB2")]
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

                new GIB2EntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x08 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GIB2* header = (GIB2*) address;
            *header = new GIB2(Children.Count);
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
                _name = "Item Boxes";
            }

            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GIB2*) source.Address)->_tag == GIB2.Tag ? new GIB2Node() : null;
        }
    }

    public unsafe class GIB2EntryNode : ResourceNode
    {
        internal GIB2Entry* Header => (GIB2Entry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public bfloat _header;
        public byte _unknown0x04;
        public byte _unkflag0;
        public byte _unkflag1;
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
        public byte _unknown0x18;
        public byte _unknown0x19;
        public float _unkflag2;
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
        public byte _unkflag3;
        public byte _modeldataid;
        public byte _unkflag5;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public float _unkflag6;
        public float _unkflag7;
        public byte _unknown0x38;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unkflag8;
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public byte _unknown0x44;
        public byte _unkflag9;
        public byte _unkflag10;
        public byte _unkflag11;
        public byte _unkflag12;
        public byte _unknown0x48;
        public byte _unknown0x49;
        public byte _unkflag13;
        public byte _unknown0x4B;
        public byte _unknown0x4C;
        public byte _unknown0x4D;
        public byte _unkflag14;
        public byte _unknown0x4F;
        public byte _unknown0x50;
        public byte _unknown0x51;
        public byte _unkflag15;
        public byte _unknown0x53;

        [Category("Item Box")]
        [DisplayName("Unk0")]
        public byte Unk0
        {
            get => _unkflag0;
            set
            {
                _unkflag0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk1")]
        public byte Unk1
        {
            get => _unkflag1;
            set
            {
                _unkflag1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk2")]
        public float Unk2
        {
            get => _unkflag2;
            set
            {
                _unkflag2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk3")]
        public byte Unk3
        {
            get => _unkflag3;
            set
            {
                _unkflag3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Model Data ID")]
        public byte ModelDataID
        {
            get => _modeldataid;
            set
            {
                _modeldataid = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk5")]
        public byte Unk5
        {
            get => _unkflag5;
            set
            {
                _unkflag5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk6")]
        public float Unk6
        {
            get => _unkflag6;
            set
            {
                _unkflag6 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk7")]
        public float Unk7
        {
            get => _unkflag7;
            set
            {
                _unkflag7 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk8")]
        public byte Unk8
        {
            get => _unkflag8;
            set
            {
                _unkflag8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk9")]
        public byte Unk9
        {
            get => _unkflag9;
            set
            {
                _unkflag9 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk10")]
        public byte Unk10
        {
            get => _unkflag10;
            set
            {
                _unkflag10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk11")]
        public byte Unk11
        {
            get => _unkflag11;
            set
            {
                _unkflag11 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk12")]
        public byte Unk12
        {
            get => _unkflag12;
            set
            {
                _unkflag12 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk13")]
        public byte Unk13
        {
            get => _unkflag13;
            set
            {
                _unkflag13 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk14")]
        public byte Unk14
        {
            get => _unkflag14;
            set
            {
                _unkflag14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Box")]
        [DisplayName("Unk15")]
        public byte Unk15
        {
            get => _unkflag15;
            set
            {
                _unkflag15 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _header = Header->_header;
            _unknown0x04 = Header->_unknown0x04;
            _unkflag0 = Header->_unkflag0;
            _unkflag1 = Header->_unkflag1;
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
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x19 = Header->_unknown0x19;
            _unkflag2 = Header->_unkflag2;
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
            _unkflag3 = Header->_unkflag3;
            _modeldataid = Header->_modeldataid;
            _unkflag5 = Header->_unkflag5;
            _unknown0x2E = Header->_unknown0x2E;
            _unknown0x2F = Header->_unknown0x2F;
            _unkflag6 = Header->_unkflag6;
            _unkflag7 = Header->_unkflag7;
            _unknown0x38 = Header->_unknown0x38;
            _unknown0x39 = Header->_unknown0x39;
            _unknown0x3A = Header->_unknown0x3A;
            _unkflag8 = Header->_unkflag8;
            _unknown0x3C = Header->_unknown0x3C;
            _unknown0x3D = Header->_unknown0x3D;
            _unknown0x3E = Header->_unknown0x3E;
            _unknown0x3F = Header->_unknown0x3F;
            _unknown0x40 = Header->_unknown0x40;
            _unknown0x41 = Header->_unknown0x41;
            _unknown0x42 = Header->_unknown0x42;
            _unknown0x43 = Header->_unknown0x43;
            _unknown0x44 = Header->_unknown0x44;
            _unkflag9 = Header->_unkflag9;
            _unkflag10 = Header->_unkflag10;
            _unkflag11 = Header->_unkflag11;
            _unkflag12 = Header->_unkflag12;
            _unknown0x48 = Header->_unknown0x48;
            _unknown0x49 = Header->_unknown0x49;
            _unkflag13 = Header->_unkflag13;
            _unknown0x4B = Header->_unknown0x4B;
            _unknown0x4C = Header->_unknown0x4C;
            _unknown0x4D = Header->_unknown0x4D;
            _unkflag14 = Header->_unkflag14;
            _unknown0x4F = Header->_unknown0x4F;
            _unkflag15 = Header->_unkflag15;
            _unknown0x53 = Header->_unknown0x53;
            if (_name == null)
            {
                _name = "Item Box [" + Index + "]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x17;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GIB2Entry* hdr = (GIB2Entry*) address;
            hdr->_header = _header;
            hdr->_unkflag0 = _unkflag0;
            hdr->_unkflag1 = _unkflag1;
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
            hdr->_unknown0x18 = _unknown0x18;
            hdr->_unknown0x19 = _unknown0x19;
            hdr->_unkflag2 = _unkflag2;
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
            hdr->_unkflag3 = _unkflag3;
            hdr->_modeldataid = _modeldataid;
            hdr->_unkflag5 = _unkflag5;
            hdr->_unknown0x2E = _unknown0x2E;
            hdr->_unknown0x2F = _unknown0x2F;
            hdr->_unkflag6 = _unkflag6;
            hdr->_unkflag7 = _unkflag7;
            hdr->_unknown0x38 = _unknown0x38;
            hdr->_unknown0x39 = _unknown0x39;
            hdr->_unknown0x3A = _unknown0x3A;
            hdr->_unkflag8 = _unkflag8;
            hdr->_unknown0x3C = _unknown0x3C;
            hdr->_unknown0x3D = _unknown0x3D;
            hdr->_unknown0x3E = _unknown0x3E;
            hdr->_unknown0x3F = _unknown0x3F;
            hdr->_unknown0x40 = _unknown0x40;
            hdr->_unknown0x41 = _unknown0x41;
            hdr->_unknown0x42 = _unknown0x42;
            hdr->_unknown0x43 = _unknown0x43;
            hdr->_unknown0x44 = _unknown0x44;
            hdr->_unkflag9 = _unkflag9;
            hdr->_unkflag10 = _unkflag10;
            hdr->_unkflag11 = _unkflag11;
            hdr->_unkflag12 = _unkflag12;
            hdr->_unknown0x48 = _unknown0x48;
            hdr->_unknown0x49 = _unknown0x49;
            hdr->_unkflag13 = _unkflag13;
            hdr->_unknown0x4B = _unknown0x4B;
            hdr->_unknown0x4C = _unknown0x4C;
            hdr->_unknown0x4D = _unknown0x4D;
            hdr->_unkflag14 = _unkflag14;
            hdr->_unknown0x4F = _unknown0x4F;
            hdr->_unknown0x50 = _unknown0x50;
            hdr->_unknown0x51 = _unknown0x51;
            hdr->_unkflag15 = _unkflag15;
            hdr->_unknown0x53 = _unknown0x53;
        }
    }
}
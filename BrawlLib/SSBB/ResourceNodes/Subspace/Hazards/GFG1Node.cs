using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GFG1Node : ResourceNode
    {
        internal GFG1* Header => (GFG1*) WorkingUncompressed.Address;

        //public override ResourceType ResourceType { get { return ResourceType.GFG1; } }

        private const int _entrySize = 0x54; // The constant size of a child entry

        [Category("GFG1")]
        [DisplayName("Entries")]
        public int count => Header->_count;

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

                new GFG1EntryNode().Initialize(this, source);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "GFG1";
            }

            return Header->_count > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x08 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFG1* header = (GFG1*) address;
            *header = new GFG1(Children.Count);
            uint offset = (uint) (0x08 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*) (address + 0x08 + i * 4) = offset;
                r.Rebuild(address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GFG1*) source.Address)->_tag == GFG1.Tag ? new GFG1Node() : null;
        }
    }

    public unsafe class GFG1EntryNode : ResourceNode
    {
        internal GFG1Entry* Header => (GFG1Entry*) WorkingUncompressed.Address;
        //public override ResourceType ResourceType { get { return ResourceType.GFG1ENTRY; } }

        public uint _header1; // 0x00
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
        public byte _costumeID; // 0x12
        public byte _flag0x13;
        public byte _unknown0x14;
        public byte _unknown0x15;
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
        public float _offenseKBMult; // 0x24
        public float _defenseKBMult; // 0x28
        public float _scale;         // 0x2C
        public byte _unknown0x30;
        public byte _unknown0x31;
        public byte _unknown0x32;
        public byte _unknown0x33;
        public byte _unknown0x34;
        public byte _unknown0x35;
        public byte _unknown0x36;
        public byte _unknown0x37;
        public byte _unknown0x38;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unknown0x3B;
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public byte _unknown0x44;
        public byte _unknown0x45;
        public byte _unknown0x46;
        public byte _unknown0x47;
        public byte _unknown0x48;
        public byte _unknown0x49;
        public byte _unknown0x4A;
        public byte _unknown0x4B;
        public byte _unknown0x4C;
        public byte _unknown0x4D;
        public byte _unknown0x4E;
        public byte _unknown0x4F;
        public byte _unknown0x50;
        public byte _unknown0x51;
        public byte _unknown0x52;
        public byte _unknown0x53;

        [Category("Fighter Info")]
        [DisplayName("Costume ID")]
        public byte EnemyID
        {
            get => _costumeID;
            set
            {
                _costumeID = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        [DisplayName("Scale")]
        public float Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        [DisplayName("Offensive Knockback Multiplier")]
        public float OffensiveKBMult
        {
            get => _offenseKBMult;
            set
            {
                _offenseKBMult = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        [DisplayName("Defensive Knockback Multiplier")]
        public float DefensiveKBMult
        {
            get => _defenseKBMult;
            set
            {
                _defenseKBMult = value;
                SignalPropertyChange();
            }
        }

        [Category("Special Flags")]
        [DisplayName("Flag 0x13")]
        public byte Flag0x13
        {
            get => _flag0x13;
            set
            {
                _flag0x13 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _header1 = Header->_header1;
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
            _costumeID = Header->_costumeID;
            _flag0x13 = Header->_flag0x13;
            _unknown0x14 = Header->_unknown0x14;
            _unknown0x15 = Header->_unknown0x15;
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
            _offenseKBMult = Header->_offenseKBMult;
            _defenseKBMult = Header->_defenseKBMult;
            _scale = Header->_scale;
            _unknown0x30 = Header->_unknown0x30;
            _unknown0x31 = Header->_unknown0x31;
            _unknown0x32 = Header->_unknown0x32;
            _unknown0x33 = Header->_unknown0x33;
            _unknown0x34 = Header->_unknown0x34;
            _unknown0x35 = Header->_unknown0x35;
            _unknown0x36 = Header->_unknown0x36;
            _unknown0x37 = Header->_unknown0x37;
            _unknown0x38 = Header->_unknown0x38;
            _unknown0x39 = Header->_unknown0x39;
            _unknown0x3A = Header->_unknown0x3A;
            _unknown0x3B = Header->_unknown0x3B;
            _unknown0x3C = Header->_unknown0x3C;
            _unknown0x3D = Header->_unknown0x3D;
            _unknown0x3E = Header->_unknown0x3E;
            _unknown0x3F = Header->_unknown0x3F;
            _unknown0x40 = Header->_unknown0x40;
            _unknown0x41 = Header->_unknown0x41;
            _unknown0x42 = Header->_unknown0x42;
            _unknown0x43 = Header->_unknown0x43;
            _unknown0x44 = Header->_unknown0x44;
            _unknown0x45 = Header->_unknown0x45;
            _unknown0x46 = Header->_unknown0x46;
            _unknown0x47 = Header->_unknown0x47;
            _unknown0x48 = Header->_unknown0x48;
            _unknown0x49 = Header->_unknown0x49;
            _unknown0x4A = Header->_unknown0x4A;
            _unknown0x4B = Header->_unknown0x4B;
            _unknown0x4C = Header->_unknown0x4C;
            _unknown0x4D = Header->_unknown0x4D;
            _unknown0x4E = Header->_unknown0x4E;
            _unknown0x4F = Header->_unknown0x4F;
            _unknown0x50 = Header->_unknown0x50;
            _unknown0x51 = Header->_unknown0x51;
            _unknown0x52 = Header->_unknown0x52;
            _unknown0x53 = Header->_unknown0x53;
            if (_name == null)
            {
                _name = "Entry [" + Index + ']';
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x54;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFG1Entry* hdr = (GFG1Entry*) address;
            hdr->_header1 = _header1;
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
            hdr->_costumeID = _costumeID;
            hdr->_flag0x13 = _flag0x13;
            hdr->_unknown0x14 = _unknown0x14;
            hdr->_unknown0x15 = _unknown0x15;
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
            hdr->_offenseKBMult = _offenseKBMult;
            hdr->_defenseKBMult = _defenseKBMult;
            hdr->_scale = _scale;
            hdr->_unknown0x30 = _unknown0x30;
            hdr->_unknown0x31 = _unknown0x31;
            hdr->_unknown0x32 = _unknown0x32;
            hdr->_unknown0x33 = _unknown0x33;
            hdr->_unknown0x34 = _unknown0x34;
            hdr->_unknown0x35 = _unknown0x35;
            hdr->_unknown0x36 = _unknown0x36;
            hdr->_unknown0x37 = _unknown0x37;
            hdr->_unknown0x38 = _unknown0x38;
            hdr->_unknown0x39 = _unknown0x39;
            hdr->_unknown0x3A = _unknown0x3A;
            hdr->_unknown0x3B = _unknown0x3B;
            hdr->_unknown0x3C = _unknown0x3C;
            hdr->_unknown0x3D = _unknown0x3D;
            hdr->_unknown0x3E = _unknown0x3E;
            hdr->_unknown0x3F = _unknown0x3F;
            hdr->_unknown0x40 = _unknown0x40;
            hdr->_unknown0x41 = _unknown0x41;
            hdr->_unknown0x42 = _unknown0x42;
            hdr->_unknown0x43 = _unknown0x43;
            hdr->_unknown0x44 = _unknown0x44;
            hdr->_unknown0x45 = _unknown0x45;
            hdr->_unknown0x46 = _unknown0x46;
            hdr->_unknown0x47 = _unknown0x47;
            hdr->_unknown0x48 = _unknown0x48;
            hdr->_unknown0x49 = _unknown0x49;
            hdr->_unknown0x4A = _unknown0x4A;
            hdr->_unknown0x4B = _unknown0x4B;
            hdr->_unknown0x4C = _unknown0x4C;
            hdr->_unknown0x4D = _unknown0x4D;
            hdr->_unknown0x4E = _unknown0x4E;
            hdr->_unknown0x4F = _unknown0x4F;
            hdr->_unknown0x50 = _unknown0x50;
            hdr->_unknown0x51 = _unknown0x51;
            hdr->_unknown0x52 = _unknown0x52;
            hdr->_unknown0x53 = _unknown0x53;
        }
    }
}
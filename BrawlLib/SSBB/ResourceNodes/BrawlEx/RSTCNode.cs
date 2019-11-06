using BrawlLib.SSBBTypes;
using System;
using System.BrawlEx;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSTCNode : ResourceNode
    {
        internal RSTC* Header => (RSTC*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RSTC;

        public uint _tag;         // 0x00 - Uneditable; RSTC
        public uint _size;        // 0x04 - Uneditable; Should be "E0"
        public uint _version;     // 0x08 - Version; Only parses "1" currently
        public byte _unknown0x0C; // 0x0C - Unused?

        public byte
            _charNum; // 0x0D - Number of characters in the char list; should be generated automatically. Max is 100? (Maybe 104, or may have padding).

        public byte _unknown0x0E; // 0x0E - Unused?

        public byte
            _randNum; // 0x0F - Number of characters in the random list; should be generated automatically. Max is 100? (Maybe 104, or may have padding).

        public byte[] _charList = new byte[104];
        public byte[] _randList = new byte[104];

        public RSTCGroupNode cssList = new RSTCGroupNode();
        public RSTCGroupNode randList = new RSTCGroupNode();

        public int NumChars
        {
            get
            {
                if (FindChildrenByName(cssList.Name).Length == 0)
                {
                    return 0;
                }

                return cssList.Children.Count;
            }
        }

        public int NumRands
        {
            get
            {
                if (FindChildrenByName(randList.Name).Length == 0)
                {
                    return 0;
                }

                return randList.Children.Count;
            }
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < 104; i++)
            {
                if (i >= _charNum)
                {
                    break;
                }

                DataSource source;
                source = new DataSource((*Header)[i], 1);
                new RSTCEntryNode().Initialize(cssList, source);
            }

            for (int i = 0; i < 104; i++)
            {
                if (i >= _randNum)
                {
                    break;
                }

                DataSource source;
                source = new DataSource((*Header)[i + 104], 1);
                new RSTCEntryNode().Initialize(randList, source);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RSTC* hdr = (RSTC*) address;
            *hdr = new RSTC();
            hdr->_tag = _tag;
            hdr->_size = _size;
            hdr->_version = _version;
            hdr->_unknown0x0C = _unknown0x0C;
            hdr->_charNum = (byte) cssList.Children.Count;
            hdr->_unknown0x0E = _unknown0x0E;
            hdr->_randNum = (byte) randList.Children.Count;
            uint offset = 0x10;
            // Ensures no junk data got saved to the character list
            for (int i = 0; i < _charList.Length; i++)
            {
                hdr->_charList[i] = 0x00;
            }

            for (int i = 0; i < cssList.Children.Count; i++)
            {
                ResourceNode r = cssList.Children[i];
                *(buint*) ((byte*) address + 0x10 + i) = offset;
                r.Rebuild(address + offset, 0x01, true);
                offset += 0x01;
            }

            offset = 0x10 + 104;
            // Ensures no junk data got saved to the random list
            for (int i = 0; i < _randList.Length; i++)
            {
                hdr->_randList[i] = 0x00;
            }

            for (int i = 0; i < randList.Children.Count; i++)
            {
                ResourceNode r = randList.Children[i];
                *(buint*) ((byte*) address + 0x10 + (i + 104)) = offset;
                r.Rebuild(address + offset, 0x01, true);
                offset += 0x01;
            }

            // Clear all junk data from the end of the lists
            for (int i = cssList.Children.Count; i < _charList.Length; i++)
            {
                hdr->_charList[i] = 0x00;
            }

            for (int i = randList.Children.Count; i < _randList.Length; i++)
            {
                hdr->_randList[i] = 0x00;
            }
        }

        public override bool OnInitialize()
        {
            _tag = Header->_tag;
            _size = Header->_size;
            _version = Header->_version;
            _unknown0x0C = Header->_unknown0x0C;
            _charNum = Header->_charNum;
            _unknown0x0E = Header->_unknown0x0E;
            _randNum = Header->_randNum;
            for (int i = 0; i < 104; i++)
            {
                _charList[i] = Header->_charList[i];
            }

            for (int j = 0; j < 104; j++)
            {
                _randList[j] = Header->_randList[j];
            }

            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            cssList._name = "Character Select";
            cssList._type = "Character Select";
            randList._name = "Random Character List";
            randList._type = "Random Character List";
            AddChild(cssList);
            AddChild(randList);
            _changed = false;
            return true;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((RSTC*) source.Address)->_tag == RSTC.Tag ? new RSTCNode() : null;
        }
    }

    public unsafe class RSTCGroupNode : ResourceNode
    {
        internal ResourceGroup* Group => (ResourceGroup*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RSTCGroup;
        public string _type;

        [Category("RSTCGroup")]
        [DisplayName("Group Type")]
        public string GroupType => _type;

        [Category("RSTCGroup")]
        [DisplayName("Entries")]
        public int entries => Children.Count;

        public override bool OnInitialize()
        {
            return true;
        }
    }

    public unsafe class RSTCEntryNode : ResourceNode
    {
        internal RSTCEntry* Header => (RSTCEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public byte _fighterID;

        [Category("CSS Slot")]
        [TypeConverter(typeof(DropDownListBrawlExCSSIDs))]
        [DisplayName("CSS Slot ID")]
        public byte FighterID
        {
            get => _fighterID;
            set
            {
                _fighterID = value;
                Name = BrawlCrate.FighterNameGenerators.FromID(_fighterID,
                    BrawlCrate.FighterNameGenerators.cssSlotIDIndex, "+S");
                if (Name == null)
                {
                    Name = "ID 0x" + _fighterID.ToString("X2");
                }

                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return RSTCEntry.Size;
        }

        public override bool OnInitialize()
        {
            _fighterID = Header->_fighterID;
            if (_name == null)
            {
                _name = BrawlCrate.FighterNameGenerators.FromID(_fighterID,
                    BrawlCrate.FighterNameGenerators.cssSlotIDIndex, "+S");
            }

            if (_name == null)
            {
                _name = "ID 0x" + _fighterID.ToString("X2");
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RSTCEntry* hdr = (RSTCEntry*) address;
            hdr->_fighterID = _fighterID;
        }

        public override string ToString()
        {
            return $"Slot {Index + 1} - {Name}";
        }
    }
}
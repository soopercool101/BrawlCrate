using BrawlLib.CustomLists;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.BrawlEx;
using System;
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

        [Description("Editing this may result in the file no longer working, this needs code changes to function properly")]
        public int MaxEntries
        {
            get
            {
                return (int)((_size - RSTC.Size) / 2);
            }
            set
            {
                _size = (uint)((value * 2) + RSTC.Size);
                SignalPropertyChange();
            }
        }


        public RSTCGroupNode cssList = new RSTCGroupNode {_type = "Character Select"};
        public RSTCGroupNode randList = new RSTCGroupNode {_type = "Random Character List"};

        public int NumChars => cssList?.Children?.Count ?? 0;

        public int NumRands => randList?.Children?.Count ?? 0;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RSTC* hdr = (RSTC*) address;
            *hdr = new RSTC();
            hdr->_tag = _tag;
            hdr->_size = (uint) length;
            hdr->_version = _version;
            hdr->_unknown0x0C = _unknown0x0C;
            hdr->_charNum = (byte) Math.Min(cssList.Children.Count, (length - RSTC.Size) / 2);
            hdr->_unknown0x0E = _unknown0x0E;
            hdr->_randNum = (byte)Math.Min(randList.Children.Count, (length - RSTC.Size) / 2);
            int entrySize = (length - 0x10) / 2;
            cssList.Rebuild(address + 0x10, entrySize, force);
            randList.Rebuild(address + 0x10 + entrySize, entrySize, force);
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
            cssList.Initialize(this, new DataSource((*Header)[0], MaxEntries));
            randList.Initialize(this, new DataSource((*Header)[MaxEntries], MaxEntries));

            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            _changed = false;
            return true;
        }

        public override int OnCalculateSize(bool force)
        {
            return (int) _size;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((RSTC*) source.Address)->_tag == RSTC.Tag ? new RSTCNode() : null;
        }
    }

    public class RSTCGroupNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.RSTCGroup;
        public string _type;

        [Category("RSTCGroup")]
        [DisplayName("Group Type")]
        public string GroupType => _type;

        [Category("RSTCGroup")]
        [DisplayName("Entries")]
        public int entries => Children?.Count ?? 0;

        public override bool OnInitialize()
        {
            _name = _type;
            if (Parent != null && Parent is RSTCNode r)
            {
                for (int i = 0;
                    i < (_type.Contains("Random", StringComparison.OrdinalIgnoreCase) ? r._randNum : r._charNum);
                    i++)
                {
                    new RSTCEntryNode().Initialize(this, WorkingUncompressed.Address[i, 1], 1);
                }

                return HasChildren;
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            for (int i = 0; i < entries; i++)
            {
                Children[i].Rebuild(address + i, 1, force);
            }
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
                Name = FighterNameGenerators.FromID(_fighterID,
                    FighterNameGenerators.cssSlotIDIndex, "+S");
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
                _name = FighterNameGenerators.FromID(_fighterID,
                    FighterNameGenerators.cssSlotIDIndex, "+S");
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
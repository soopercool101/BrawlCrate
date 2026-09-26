using BrawlLib.Internal;
using BrawlLib.SSBB.Types.BrawlEx;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class COSCNode : ARCEntryNode
    {
        internal COSC* Header => (COSC*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.COSC;

        public uint _tag;       // 0x00 - Uneditable; COSC
        public uint _size;      // 0x04 - Uneditable; Should be "40"
        public uint _version;   // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1; // 0x0C - Unused?
        public byte _editFlag2; // 0x0D - Unused?
        public byte _editFlag3; // 0X0E - Unused?

        public byte
            _hasSecondary; // 0X0F - 00 is not transforming, 01 is transforming. Possibly could be set automatically based on if a secondary character slot is FF or not?

        public byte _cosmeticID;        // 0x10 - Cosmetic slot; Parse as hex?
        public byte _unknown0x11;       // 0x11
        public byte _primaryCharSlot;   // 0x12 - Primary Character Slot (Parse as hex?)
        public byte _secondaryCharSlot; // 0x13 - Secondary Character Slot (Parse as hex?)
        public byte _franchise;         // 0x14 - Make this a list
        public byte _unknown0x15;       // 0x15
        public byte _unknown0x16;       // 0x16
        public byte _unknown0x17;       // 0x17
        public uint _announcerSFX;      // 0x18 - Announcer Call
        public uint _unknown0x1C;       // 0x1C
        public string _victoryName;     // 0x20 - 32 characters

        [Category("Character")]
        [DisplayName("Character Name")]
        public string CharacterName
        {
            get => _victoryName;
            set
            {
                _victoryName = value;
                SignalPropertyChange();
            }
        }

        [Category("Character")]
        [DisplayName("Set Primary/Secondary")]
        public bool HasSecondary
        {
            get
            {
                if (_hasSecondary == 0)
                {
                    return false;
                }

                return true;
            }
            set
            {
                if (value)
                {
                    _hasSecondary = 1;
                }
                else
                {
                    _hasSecondary = 0;
                }

                SignalPropertyChange();
            }
        }

        [Category("Character")]
        [TypeConverter(typeof(DropDownListBrawlExSlotIDs))]
        [DisplayName("Primary Character Slot")]
        public byte CharSlot1
        {
            get => _primaryCharSlot;
            set
            {
                _primaryCharSlot = value;
                SignalPropertyChange();
            }
        }

        [Category("Character")]
        [TypeConverter(typeof(DropDownListBrawlExSlotIDs))]
        [DisplayName("Secondary Character Slot")]
        public byte CharSlot2
        {
            get => _secondaryCharSlot;
            set
            {
                _secondaryCharSlot = value;
                SignalPropertyChange();
            }
        }

        [Category("Cosmetics")]
        [DisplayName("Cosmetic ID")]
        [TypeConverter(typeof(HexByteConverter))]
        public byte CosmeticID
        {
            get => _cosmeticID;
            set
            {
                _cosmeticID = value;
                SignalPropertyChange();
            }
        }

        [Category("Cosmetics")]
        [TypeConverter(typeof(DropDownListBrawlExIconIDs))]
        [DisplayName("Franchise Icon")]
        public byte FranchiseIconID
        {
            get => _franchise;
            set
            {
                _franchise = value;
                SignalPropertyChange();
            }
        }

        [Category("Cosmetics")]
        [DisplayName("Announcer Call")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint AnnouncerID
        {
            get => _announcerSFX;
            set
            {
                _announcerSFX = value;
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            // This node has no children
        }

        public override int OnCalculateSize(bool force)
        {
            return COSC.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            COSC* hdr = (COSC*) address;
            hdr->_tag = _tag;
            hdr->_size = _size;
            hdr->_version = _version;
            hdr->_editFlag1 = _editFlag1;
            hdr->_editFlag2 = _editFlag2;
            hdr->_editFlag3 = _editFlag3;
            hdr->_hasSecondary = _hasSecondary;
            hdr->_cosmeticID = _cosmeticID;
            hdr->_unknown0x11 = _unknown0x11;
            hdr->_primaryCharSlot = _primaryCharSlot;
            hdr->_secondaryCharSlot = _secondaryCharSlot;
            hdr->_franchise = _franchise;
            hdr->_unknown0x15 = _unknown0x15;
            hdr->_unknown0x16 = _unknown0x16;
            hdr->_unknown0x17 = _unknown0x17;
            hdr->_announcerSFX = _announcerSFX;
            hdr->_unknown0x1C = _unknown0x1C;
            address.WriteUTF8String(_victoryName, false, 0x20, 32);
        }

        public override bool OnInitialize()
        {
            _tag = Header->_tag;
            _size = Header->_size;
            _version = Header->_version;
            _editFlag1 = Header->_editFlag1;
            _editFlag2 = Header->_editFlag2;
            _editFlag3 = Header->_editFlag3;
            _hasSecondary = Header->_hasSecondary;
            _cosmeticID = Header->_cosmeticID;
            _unknown0x11 = Header->_unknown0x11;
            _primaryCharSlot = Header->_primaryCharSlot;
            _secondaryCharSlot = Header->_secondaryCharSlot;
            _franchise = Header->_franchise;
            _unknown0x15 = Header->_unknown0x15;
            _unknown0x16 = Header->_unknown0x16;
            _unknown0x17 = Header->_unknown0x17;
            _announcerSFX = Header->_announcerSFX;
            _unknown0x1C = Header->_unknown0x1C;
            _victoryName = WorkingUncompressed.Address.GetUTF8String(0x20, 32);
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            return false;
        }
        public override string FileTypeString => "Cosmetic Data";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((COSC*) source.Address)->_tag == COSC.Tag ? new COSCNode() : null;
        }
    }
}
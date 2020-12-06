using BrawlLib.Internal;
using BrawlLib.SSBB.Types.BrawlEx;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class COSCNode : ResourceNode
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

        public byte[] _victoryNameArray = new byte[32];

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
            _victoryNameArray = System.Text.Encoding.UTF8.GetBytes(_victoryName);
            if (_victoryNameArray.Length >= 1)
            {
                hdr->_victoryNameArray[0] = _victoryNameArray[0];
            }
            else
            {
                hdr->_victoryNameArray[0] = 0;
            }

            if (_victoryNameArray.Length >= 2)
            {
                hdr->_victoryNameArray[1] = _victoryNameArray[1];
            }
            else
            {
                hdr->_victoryNameArray[1] = 0;
            }

            if (_victoryNameArray.Length >= 3)
            {
                hdr->_victoryNameArray[2] = _victoryNameArray[2];
            }
            else
            {
                hdr->_victoryNameArray[2] = 0;
            }

            if (_victoryNameArray.Length >= 4)
            {
                hdr->_victoryNameArray[3] = _victoryNameArray[3];
            }
            else
            {
                hdr->_victoryNameArray[3] = 0;
            }

            if (_victoryNameArray.Length >= 5)
            {
                hdr->_victoryNameArray[4] = _victoryNameArray[4];
            }
            else
            {
                hdr->_victoryNameArray[4] = 0;
            }

            if (_victoryNameArray.Length >= 6)
            {
                hdr->_victoryNameArray[5] = _victoryNameArray[5];
            }
            else
            {
                hdr->_victoryNameArray[5] = 0;
            }

            if (_victoryNameArray.Length >= 7)
            {
                hdr->_victoryNameArray[6] = _victoryNameArray[6];
            }
            else
            {
                hdr->_victoryNameArray[6] = 0;
            }

            if (_victoryNameArray.Length >= 8)
            {
                hdr->_victoryNameArray[7] = _victoryNameArray[7];
            }
            else
            {
                hdr->_victoryNameArray[7] = 0;
            }

            if (_victoryNameArray.Length >= 9)
            {
                hdr->_victoryNameArray[8] = _victoryNameArray[8];
            }
            else
            {
                hdr->_victoryNameArray[8] = 0;
            }

            if (_victoryNameArray.Length >= 10)
            {
                hdr->_victoryNameArray[9] = _victoryNameArray[9];
            }
            else
            {
                hdr->_victoryNameArray[9] = 0;
            }

            if (_victoryNameArray.Length >= 11)
            {
                hdr->_victoryNameArray[10] = _victoryNameArray[10];
            }
            else
            {
                hdr->_victoryNameArray[10] = 0;
            }

            if (_victoryNameArray.Length >= 12)
            {
                hdr->_victoryNameArray[11] = _victoryNameArray[11];
            }
            else
            {
                hdr->_victoryNameArray[11] = 0;
            }

            if (_victoryNameArray.Length >= 13)
            {
                hdr->_victoryNameArray[12] = _victoryNameArray[12];
            }
            else
            {
                hdr->_victoryNameArray[12] = 0;
            }

            if (_victoryNameArray.Length >= 14)
            {
                hdr->_victoryNameArray[13] = _victoryNameArray[13];
            }
            else
            {
                hdr->_victoryNameArray[13] = 0;
            }

            if (_victoryNameArray.Length >= 15)
            {
                hdr->_victoryNameArray[14] = _victoryNameArray[14];
            }
            else
            {
                hdr->_victoryNameArray[14] = 0;
            }

            if (_victoryNameArray.Length >= 16)
            {
                hdr->_victoryNameArray[15] = _victoryNameArray[15];
            }
            else
            {
                hdr->_victoryNameArray[15] = 0;
            }

            if (_victoryNameArray.Length >= 17)
            {
                hdr->_victoryNameArray[16] = _victoryNameArray[16];
            }
            else
            {
                hdr->_victoryNameArray[16] = 0;
            }

            if (_victoryNameArray.Length >= 18)
            {
                hdr->_victoryNameArray[17] = _victoryNameArray[17];
            }
            else
            {
                hdr->_victoryNameArray[17] = 0;
            }

            if (_victoryNameArray.Length >= 19)
            {
                hdr->_victoryNameArray[18] = _victoryNameArray[18];
            }
            else
            {
                hdr->_victoryNameArray[18] = 0;
            }

            if (_victoryNameArray.Length >= 20)
            {
                hdr->_victoryNameArray[19] = _victoryNameArray[19];
            }
            else
            {
                hdr->_victoryNameArray[19] = 0;
            }

            if (_victoryNameArray.Length >= 21)
            {
                hdr->_victoryNameArray[20] = _victoryNameArray[20];
            }
            else
            {
                hdr->_victoryNameArray[20] = 0;
            }

            if (_victoryNameArray.Length >= 22)
            {
                hdr->_victoryNameArray[21] = _victoryNameArray[21];
            }
            else
            {
                hdr->_victoryNameArray[21] = 0;
            }

            if (_victoryNameArray.Length >= 23)
            {
                hdr->_victoryNameArray[22] = _victoryNameArray[22];
            }
            else
            {
                hdr->_victoryNameArray[22] = 0;
            }

            if (_victoryNameArray.Length >= 24)
            {
                hdr->_victoryNameArray[23] = _victoryNameArray[23];
            }
            else
            {
                hdr->_victoryNameArray[23] = 0;
            }

            if (_victoryNameArray.Length >= 25)
            {
                hdr->_victoryNameArray[24] = _victoryNameArray[24];
            }
            else
            {
                hdr->_victoryNameArray[24] = 0;
            }

            if (_victoryNameArray.Length >= 26)
            {
                hdr->_victoryNameArray[25] = _victoryNameArray[25];
            }
            else
            {
                hdr->_victoryNameArray[25] = 0;
            }

            if (_victoryNameArray.Length >= 27)
            {
                hdr->_victoryNameArray[26] = _victoryNameArray[26];
            }
            else
            {
                hdr->_victoryNameArray[26] = 0;
            }

            if (_victoryNameArray.Length >= 28)
            {
                hdr->_victoryNameArray[27] = _victoryNameArray[27];
            }
            else
            {
                hdr->_victoryNameArray[27] = 0;
            }

            if (_victoryNameArray.Length >= 29)
            {
                hdr->_victoryNameArray[28] = _victoryNameArray[28];
            }
            else
            {
                hdr->_victoryNameArray[28] = 0;
            }

            if (_victoryNameArray.Length >= 30)
            {
                hdr->_victoryNameArray[29] = _victoryNameArray[29];
            }
            else
            {
                hdr->_victoryNameArray[29] = 0;
            }

            if (_victoryNameArray.Length >= 31)
            {
                hdr->_victoryNameArray[30] = _victoryNameArray[30];
            }
            else
            {
                hdr->_victoryNameArray[30] = 0;
            }

            if (_victoryNameArray.Length >= 32)
            {
                hdr->_victoryNameArray[31] = _victoryNameArray[31];
            }
            else
            {
                hdr->_victoryNameArray[31] = 0;
            }
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
            _victoryNameArray[0] = Header->_victoryNameArray[0];
            _victoryNameArray[1] = Header->_victoryNameArray[1];
            _victoryNameArray[2] = Header->_victoryNameArray[2];
            _victoryNameArray[3] = Header->_victoryNameArray[3];
            _victoryNameArray[4] = Header->_victoryNameArray[4];
            _victoryNameArray[5] = Header->_victoryNameArray[5];
            _victoryNameArray[6] = Header->_victoryNameArray[6];
            _victoryNameArray[7] = Header->_victoryNameArray[7];
            _victoryNameArray[8] = Header->_victoryNameArray[8];
            _victoryNameArray[9] = Header->_victoryNameArray[9];
            _victoryNameArray[10] = Header->_victoryNameArray[10];
            _victoryNameArray[11] = Header->_victoryNameArray[11];
            _victoryNameArray[12] = Header->_victoryNameArray[12];
            _victoryNameArray[13] = Header->_victoryNameArray[13];
            _victoryNameArray[14] = Header->_victoryNameArray[14];
            _victoryNameArray[15] = Header->_victoryNameArray[15];
            _victoryNameArray[16] = Header->_victoryNameArray[16];
            _victoryNameArray[17] = Header->_victoryNameArray[17];
            _victoryNameArray[18] = Header->_victoryNameArray[18];
            _victoryNameArray[19] = Header->_victoryNameArray[19];
            _victoryNameArray[20] = Header->_victoryNameArray[20];
            _victoryNameArray[21] = Header->_victoryNameArray[21];
            _victoryNameArray[22] = Header->_victoryNameArray[22];
            _victoryNameArray[23] = Header->_victoryNameArray[23];
            _victoryNameArray[24] = Header->_victoryNameArray[24];
            _victoryNameArray[25] = Header->_victoryNameArray[25];
            _victoryNameArray[26] = Header->_victoryNameArray[26];
            _victoryNameArray[27] = Header->_victoryNameArray[27];
            _victoryNameArray[28] = Header->_victoryNameArray[28];
            _victoryNameArray[29] = Header->_victoryNameArray[29];
            _victoryNameArray[30] = Header->_victoryNameArray[30];
            _victoryNameArray[31] = Header->_victoryNameArray[31];
            _victoryName = System.Text.Encoding.UTF8.GetString(_victoryNameArray).TrimEnd(new char[] {'\0'});
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }
            //_name = "Cosmetic" + _cosmeticID.ToString("X2") + " (" + _victoryName + ")";

            return false;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((COSC*) source.Address)->_tag == COSC.Tag ? new COSCNode() : null;
        }
    }
}
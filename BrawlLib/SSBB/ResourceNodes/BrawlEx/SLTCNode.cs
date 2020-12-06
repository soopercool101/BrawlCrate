using BrawlLib.Internal;
using BrawlLib.SSBB.Types.BrawlEx;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SLTCNode : ResourceNode
    {
        internal SLTC* Header => (SLTC*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SLTC;

        public uint _tag;               // 0x00 - Uneditable; SLTC
        public uint _size;              // 0x04 - Uneditable; Should be "40"
        public uint _version;           // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1;         // 0x0C - Unused?
        public byte _editFlag2;         // 0x0D - Unused?
        public byte _editFlag3;         // 0X0E - Unused?
        public byte _setSlotCharacters; // 0X0F - 00 is not set, 01 is set
        public uint _slot1;             // 0x10
        public uint _slot2;             // 0x14
        public uint _slot3;             // 0x18
        public uint _slot4;             // 0x1C
        public uint _victoryTheme;      // 0x20 - Victory Theme
        public byte _recordSlot;        // 0x24 - Record Bank
        public byte _unknown0x25;       // 0x25 - Seems to always be 0xCC
        public byte _unknown0x26;       // 0x26 - Seems to always be 0xCC
        public byte _unknown0x27;       // 0x27 - Seems to always be 0xCC
        public uint _announcerSFX;      // 0x28 - Announcer voiceline for victory screen
        public uint _unknown0x2C;       // 0x2C - Appears to always be 0xCCCCCCCC
        public float _victoryCamera1;   // 0x30 - Camera Distance
        public float _victoryCamera2;   // 0x34 - Camera Distance
        public float _victoryCamera3;   // 0x38 - Camera Distance
        public float _victoryCamera4;   // 0x3C - Camera Distance

        public byte[] _victoryNameArray = new byte[32];

        [Category("Characters")]
        [DisplayName("Set Slot Characters")]
        public bool SetSlot
        {
            get
            {
                if (_setSlotCharacters == 0)
                {
                    return false;
                }

                return true;
            }
            set
            {
                if (value)
                {
                    _setSlotCharacters = 1;
                }
                else
                {
                    _setSlotCharacters = 0;
                }

                SignalPropertyChange();
            }
        }

        [Category("Characters")]
        [TypeConverter(typeof(DropDownListBrawlExFighterIDsLong))]
        [DisplayName("Character Slot 1")]
        public uint CharSlot1
        {
            get => _slot1;
            set
            {
                _slot1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Characters")]
        [TypeConverter(typeof(DropDownListBrawlExFighterIDsLong))]
        [DisplayName("Character Slot 2")]
        public uint CharSlot2
        {
            get => _slot2;
            set
            {
                _slot2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Characters")]
        [TypeConverter(typeof(DropDownListBrawlExFighterIDsLong))]
        [DisplayName("Character Slot 3")]
        public uint CharSlot3
        {
            get => _slot3;
            set
            {
                _slot3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Characters")]
        [TypeConverter(typeof(DropDownListBrawlExFighterIDsLong))]
        [DisplayName("Character Slot 4")]
        public uint CharSlot4
        {
            get => _slot4;
            set
            {
                _slot4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Characters")]
        [TypeConverter(typeof(DropDownListBrawlExRecordIDs))]
        [DisplayName("Record Bank")]
        public byte Records
        {
            get => _recordSlot;
            set
            {
                _recordSlot = value;
                SignalPropertyChange();
            }
        }

        [Category("Victory")]
        [DisplayName("Victory Theme")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint VictoryTheme
        {
            get => _victoryTheme;
            set
            {
                _victoryTheme = value;
                SignalPropertyChange();
            }
        }

        [Category("Victory")]
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

        [Category("Victory")]
        [DisplayName("Camera Distance 1")]
        public float CameraDistance1
        {
            get => _victoryCamera1;
            set
            {
                _victoryCamera1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Victory")]
        [DisplayName("Camera Distance 2")]
        public float CameraDistance2
        {
            get => _victoryCamera2;
            set
            {
                _victoryCamera2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Victory")]
        [DisplayName("Camera Distance 3")]
        public float CameraDistance3
        {
            get => _victoryCamera3;
            set
            {
                _victoryCamera3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Victory")]
        [DisplayName("Camera Distance 4")]
        public float CameraDistance4
        {
            get => _victoryCamera4;
            set
            {
                _victoryCamera4 = value;
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            // This node has no children
        }

        public override int OnCalculateSize(bool force)
        {
            return SLTC.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SLTC* hdr = (SLTC*) address;
            hdr->_tag = _tag;
            hdr->_size = _size;
            hdr->_version = _version;
            hdr->_editFlag1 = _editFlag1;
            hdr->_editFlag2 = _editFlag2;
            hdr->_editFlag3 = _editFlag3;
            hdr->_setSlotCharacters = _setSlotCharacters;
            hdr->_slot1 = _slot1;
            hdr->_slot2 = _slot2;
            hdr->_slot3 = _slot3;
            hdr->_slot4 = _slot4;
            hdr->_victoryTheme = _victoryTheme;
            hdr->_recordSlot = _recordSlot;
            hdr->_unknown0x25 = _unknown0x25;
            hdr->_unknown0x26 = _unknown0x26;
            hdr->_unknown0x27 = _unknown0x27;
            hdr->_announcerSFX = _announcerSFX;
            hdr->_unknown0x2C = _unknown0x2C;
            hdr->_victoryCamera1 = _victoryCamera1;
            hdr->_victoryCamera2 = _victoryCamera2;
            hdr->_victoryCamera3 = _victoryCamera3;
            hdr->_victoryCamera4 = _victoryCamera4;
        }

        public override bool OnInitialize()
        {
            _tag = Header->_tag;
            _size = Header->_size;
            _version = Header->_version;
            _editFlag1 = Header->_editFlag1;
            _editFlag2 = Header->_editFlag2;
            _editFlag3 = Header->_editFlag3;
            _setSlotCharacters = Header->_setSlotCharacters;
            _slot1 = Header->_slot1;
            _slot2 = Header->_slot2;
            _slot3 = Header->_slot3;
            _slot4 = Header->_slot4;
            _victoryTheme = Header->_victoryTheme;
            _recordSlot = Header->_recordSlot;
            _unknown0x25 = Header->_unknown0x25;
            _unknown0x26 = Header->_unknown0x26;
            _unknown0x27 = Header->_unknown0x27;
            _announcerSFX = Header->_announcerSFX;
            _unknown0x2C = Header->_unknown0x2C;
            _victoryCamera1 = Header->_victoryCamera1;
            _victoryCamera2 = Header->_victoryCamera2;
            _victoryCamera3 = Header->_victoryCamera3;
            _victoryCamera4 = Header->_victoryCamera4;
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }
            //_name = "Cosmetic" + _cosmeticID.ToString("X2") + " (" + _victoryName + ")";

            return false;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((SLTC*) source.Address)->_tag == SLTC.Tag ? new SLTCNode() : null;
        }
    }
}
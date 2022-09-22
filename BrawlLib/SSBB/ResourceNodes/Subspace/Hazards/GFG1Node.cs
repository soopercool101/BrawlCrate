using BrawlLib.CustomLists;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System.ComponentModel;
using System;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFG1Node : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GFG1EntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFG1" ? new GFG1Node() : null;
        }
    }

    public unsafe class GFG1EntryNode : ResourceNode
    {
        internal GFG1Entry* Header => (GFG1Entry*) WorkingUncompressed.Address;
        //public override ResourceType ResourceType { get { return ResourceType.GFG1ENTRY; } }

        public byte _fighterID;
        public byte _stockCount;
        public byte _costumeID;
        public byte _unknown0x03;
        public byte _unknown0x04;
        public byte _isTeamMember;
        public byte _unknown0x06;
        public byte _isMetal;
        public byte _unknown0x08;
        public byte _isSpyCloak;
        public byte _isLowGravity;
        public byte _isNoVoice;
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _unknown0x0E;
        public byte _unknown0x0F;
        public byte _displayStaminaHP;
        public byte _unknown0x11;
        public byte _cpuType;
        public byte _cpuRank;
        public byte _playerNumber;
        public byte _costumeType;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public short _startDamage;
        public short _hitPointMax;
        public byte _unknown0x1C;
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public short _glowAttack;
        public short _glowDefense;
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
        public uint _triggerData;

        [Category("Fighter Flags")]
        public bool IsTeamMember
        {
            get => _isTeamMember == 1;
            set
            {
                _isTeamMember = (byte)(value ? 1 : 0);
                SignalPropertyChange();
            }
        }

        [Category("Fighter Flags")]
        public bool IsMetal
        {
            get => _isMetal == 1;
            set
            {
                _isMetal = (byte)(value ? 1 : 0);
                SignalPropertyChange();
            }
        }

        [Category("Fighter Flags")]
        public bool IsSpyCloak
        {
            get => _isSpyCloak == 1;
            set
            {
                _isSpyCloak = (byte)(value ? 1 : 0);
                SignalPropertyChange();
            }
        }

        [Category("Fighter Flags")]
        public bool IsLowGravity
        {
            get => _isLowGravity == 1;
            set
            {
                _isLowGravity = (byte)(value ? 1 : 0);
                SignalPropertyChange();
            }
        }

        [Category("Fighter Flags")]
        public bool IsNoVoice
        {
            get => _isNoVoice == 1;
            set
            {
                _isNoVoice = (byte)(value ? 1 : 0);
                SignalPropertyChange();
            }
        }

        [Category("Fighter Flags")]
        public bool DisplayStaminaHP
        {
            get => _displayStaminaHP == 1;
            set
            {
                _displayStaminaHP = (byte)(value ? 1 : 0);
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        [TypeConverter(typeof(DropDownListBrawlExSlotIDs))]
        [DisplayName("Fighter ID")]
        public byte FighterID
        {
            get => _fighterID;
            set
            {
                _fighterID = value;
                Name = FighterNameGenerators.FromID(_fighterID,
                    FighterNameGenerators.slotIDIndex, "+S") + $" [{Index}]";

                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        public byte StockCount
        {
            get => _stockCount;
            set
            {
                _stockCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        public byte PlayerNumber
        {
            get => _playerNumber;
            set
            {
                _playerNumber = value;
                SignalPropertyChange();
            }
        }

        public enum CostumeTypes : byte
        {
            Normal = 0,
            Dark = 1,
            Fake = 2
        }
        [Category("Fighter Info")]
        public CostumeTypes CostumeType
        {
            get => (CostumeTypes)_playerNumber;
            set
            {
                _playerNumber = (byte)value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        public short StartDamage
        {
            get => _startDamage;
            set
            {
                _startDamage = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        public short HitPointMax
        {
            get => _hitPointMax;
            set
            {
                _hitPointMax = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        public short GlowAttack
        {
            get => _glowAttack;
            set
            {
                _glowAttack = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        public short GlowDefense
        {
            get => _glowDefense;
            set
            {
                _glowDefense = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        [DisplayName("Costume ID")]
        public byte CostumeID
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

        [Category("Fighter Info")]
        [DisplayName("CPU Type")]
        public byte CPUType
        {
            get => _cpuType;
            set
            {
                _cpuType = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter Info")]
        [DisplayName("CPU Rank")]
        public byte CPURank
        {
            get => _cpuRank;
            set
            {
                _cpuRank = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _fighterID = Header->_fighterID;
            _stockCount = Header->_stockCount;
            _costumeID = Header->_costumeID;
            _unknown0x03 = Header->_unknown0x03;
            _unknown0x04 = Header->_unknown0x04;
            _isTeamMember = Header->_isTeamMember;
            _unknown0x06 = Header->_unknown0x06;
            _isMetal = Header->_isMetal;
            _unknown0x08 = Header->_unknown0x08;
            _isSpyCloak = Header->_isSpyCloak;
            _isLowGravity = Header->_isLowGravity;
            _isNoVoice = Header->_isNoVoice;
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x0D = Header->_unknown0x0D;
            _unknown0x0E = Header->_unknown0x0E;
            _unknown0x0F = Header->_unknown0x0F;
            _displayStaminaHP = Header->_displayStaminaHP;
            _unknown0x11 = Header->_unknown0x11;
            _cpuType = Header->_cpuType;
            _cpuRank = Header->_cpuRank;
            _playerNumber = Header->_playerNumber;
            _costumeType = Header->_costumeType;
            _unknown0x16 = Header->_unknown0x16;
            _unknown0x17 = Header->_unknown0x17;
            _startDamage = Header->_startDamage;
            _hitPointMax = Header->_hitPointMax;
            _unknown0x1C = Header->_unknown0x1C;
            _unknown0x1D = Header->_unknown0x1D;
            _unknown0x1E = Header->_unknown0x1E;
            _unknown0x1F = Header->_unknown0x1F;
            _glowAttack = Header->_glowAttack;
            _glowDefense = Header->_glowDefense;
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
            _triggerData = Header->_triggerData;
            if (_name == null)
            {
                _name = FighterNameGenerators.FromID(_fighterID,
                    FighterNameGenerators.slotIDIndex, "+S") + $" [{Index}]";
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
            hdr->_fighterID = _fighterID;
            hdr->_stockCount = _stockCount;
            hdr->_costumeID = _costumeID;
            hdr->_unknown0x03 = _unknown0x03;
            hdr->_unknown0x04 = _unknown0x04;
            hdr->_isTeamMember = _isTeamMember;
            hdr->_unknown0x06 = _unknown0x06;
            hdr->_isMetal = _isMetal;
            hdr->_unknown0x08 = _unknown0x08;
            hdr->_isSpyCloak = _isSpyCloak;
            hdr->_isLowGravity = _isLowGravity;
            hdr->_isNoVoice = _isNoVoice;
            hdr->_unknown0x0C = _unknown0x0C;
            hdr->_unknown0x0D = _unknown0x0D;
            hdr->_unknown0x0E = _unknown0x0E;
            hdr->_unknown0x0F = _unknown0x0F;
            hdr->_displayStaminaHP = _displayStaminaHP;
            hdr->_unknown0x11 = _unknown0x11;
            hdr->_cpuType = _cpuType;
            hdr->_cpuRank = _cpuRank;
            hdr->_playerNumber = _playerNumber;
            hdr->_costumeType = _costumeType;
            hdr->_unknown0x16 = _unknown0x16;
            hdr->_unknown0x17 = _unknown0x17;
            hdr->_startDamage = _startDamage;
            hdr->_hitPointMax = _hitPointMax;
            hdr->_unknown0x1C = _unknown0x1C;
            hdr->_unknown0x1D = _unknown0x1D;
            hdr->_unknown0x1E = _unknown0x1E;
            hdr->_unknown0x1F = _unknown0x1F;
            hdr->_glowAttack = _glowAttack;
            hdr->_glowDefense = _glowDefense;
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
            hdr->_triggerData = _triggerData;
        }
    }
}
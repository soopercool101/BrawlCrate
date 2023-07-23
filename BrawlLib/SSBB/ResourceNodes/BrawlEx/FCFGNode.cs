using BrawlLib.Internal;
using BrawlLib.SSBB.Types.BrawlEx;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class FCFGNode : ResourceNode
    {
        internal FCFG* Header => (FCFG*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.FCFG;

        public uint _tag;                              // 0x00 - Uneditable; FCFG (Or FITC)
        public uint _size;                             // 0x04 - Uneditable; Should be "100"
        public uint _version;                          // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1;                        // 0x0C - Unused?
        public byte _editFlag2;                        // 0x0D - Unused?
        public byte _editFlag3;                        // 0X0E - Unused?
        public byte _editFlag4;                        // 0X0F - Unused?
        public byte _kirbyCopyColorFlags;              // 0x10 - 0 = No file; 1 = Per Color files; 2: One Kirby file
        public byte _entryColorFlags;                  // 0x11
        public byte _resultColorFlags;                 // 0x12
        public CharacterLoadFlags _characterLoadFlags; // 0x13 - http://opensa.dantarion.com/wiki/Load_Flags
        public byte _finalSmashColorFlags;             // 0x14 - 0x00 = no file; 0x01 = per color files; 
        public byte _unknown0x15;                      // 0x15
        public CostumeLoadFlags _colorFlags;           // 0x16
        public uint _entryArticleFlag;                 // 0x18
        public uint _soundbank;                        // 0x1C - Parse as list
        public uint _kirbySoundbank;                   // 0x20
        public uint _unknown0x24;                      // 0x24
        public uint _unknown0x28;                      // 0x28
        public uint _unknown0x2C;                      // 0x2C

        // Name stuff. Name should be no more than 16 characters (technical max for filenames is 20 but internal name lowers the limit)
        public byte[]
            _pacNameArray = new byte[48]; // 0x30 - 48 characters; Auto generate from name: (name-lower)/Fit(name).pac

        public byte[]
            _kirbyPacNameArray =
                new byte[48]; // 0x60 - 48 characters; Auto generate from name: kirby/FitKirby(name).pac

        public byte[]
            _moduleNameArray = new byte[32]; // 0x90 - 32 characters; Auto generate from name: ft_(name-lower).rel

        public byte[] _internalNameArray = new byte[16]; // 0xB0 - 16 characters; Auto generate from name: (name-upper)

        // Strings
        public string _fighterName;
        public string _pacName;
        public string _kirbyPacName;
        public string _moduleName;
        public string _internalName;

        // IC Constants
        public uint _jabFlag;              // 0xC0 - Usage unknown
        public uint _jabCount;             // 0xC4 - Number of jabs in combo
        public uint _hasRapidJab;          // 0xC8 - Whether the fighter has a rapid jab
        public uint _canTilt;              // 0xCC - Whether the fighter can angle their forward tilt attack
        public uint _fSmashCount;          // 0xD0 - Number of attacks in Fsmash chain
        public uint _airJumpCount;         // 0xD4 - Number of mid-air jumps the fighter has
        public uint _canGlide;             // 0xD8 - Whether the fighter can glide
        public uint _canCrawl;             // 0xDC - Whether the fighter can crawl
        public uint _dashAttackIntoCrouch; // 0xE0 - Whether the fighter's dash attack puts them in a crouching position
        public uint _canWallJump;          // 0xE4 - Whether the fighter can jump off walls
        public uint _canCling;             // 0xE8 - Whether the fighter can cling to walls
        public uint _canZAir;              // 0xEC - Whether the fighter can use an aerial tether
        public uint _u12Flag;              // 0xF0 - Usage unknown
        public uint _u13Flag;              // 0xF4 - Usage unknown

        public uint _textureLoad;  // 0xF8 - 0/1/2/3/4/5
        public uint _aiController; // 0xFC

        [Browsable(false)]
        [Flags]
        public enum CharacterLoadFlags : byte
        {
            None = 0x00,                   // 0000 0000
            WorkManageFlag = 0x01,         // 0000 0001
            FinalSmashFilesFlag = 0x02,    // 0000 0010
            FinalSmashMusicOffFlag = 0x04, // 0000 0100
            IkPhysicsFlag = 0x08,          // 0000 1000
            MergeMotionEtcFlag = 0x10,     // 0001 0000
            PerCostumeEtc = 0x20,          // 0010 0000
            UnknownFlag_A = 0x40,          // 0100 0000
            UnknownFlag_B = 0x80           // 1000 0000
        }

        [Browsable(false)]
        [Flags]
        public enum CostumeLoadFlags : ushort
        {
            None = 0x0000,
            CostumeFlag00 = 0x0001,
            CostumeFlag01 = 0x0002,
            CostumeFlag02 = 0x0004,
            CostumeFlag03 = 0x0008,
            CostumeFlag04 = 0x0010,
            CostumeFlag05 = 0x0020,
            CostumeFlag06 = 0x0040,
            CostumeFlag07 = 0x0080,
            CostumeFlag08 = 0x0100,
            CostumeFlag09 = 0x0200,
            CostumeFlag10 = 0x0400,
            CostumeFlag11 = 0x0800,
            UnknownFlag_A = 0x1000,
            UnknownFlag_B = 0x2000,

            // Theoretical Flags
            UnknownFlag_C = 0x4000,
            UnknownFlag_D = 0x8000
        }

        [Browsable(false)]
        public enum EntryResultLoadFlags : byte
        {
            None = 0x00,
            Single = 0x01,
            PerColor = 0x02
        }

        // The kirby flags swap the single and per-color for whatever reason
        [Browsable(false)]
        public enum KirbyLoadFlags : byte
        {
            None = 0x00,
            Single = 0x02,
            PerColor = 0x01
        }

        [Browsable(false)]
        public enum FinalLoadFlagsV1 : byte // Used in BrawlEx 1.0
        {
            None = 0x01,
            Single = 0x00,
            PerColor = 0x02,
            UseFitFoxFinal = 0x03
        }

        [Browsable(false)]
        public enum FinalLoadFlags : byte // Used in BrawlEx 2.0 and onward
        {
            None = 0x00,
            Single = 0x01,
            PerColor = 0x02,
            UseFitFoxFinal = 0x03
        }

        [Browsable(false)]
        public enum AirJumpFlags : uint
        {
            None = 0x00000000,
            NormalAirJump = 0x00000001,
            MultiAirJump = 0x00000002
        }

        [Category("\t\tFighter")]
        [Description(
            "The fighter's file name. Editing this will automatically change the other properties in the fighter group.")]
        [DisplayName("Fighter File Name")]
        public string FighterName
        {
            get => _fighterName;
            set
            {
                if (value.Length > 0)
                {
                    if (value.Length <= 16)
                    {
                        _fighterName = value;
                    }
                    else
                    {
                        _fighterName = value.Substring(0, 16);
                    }

                    _pacName = AutoPacName;
                    _kirbyPacName = AutoKirbyPacName;
                    _moduleName = AutoModuleName;
                    _internalName = AutoInternalFighterName;
                    SignalPropertyChange();
                }
            }
        }

        [Category("\t\tFighter")]
        [Description("This is changed automatically when the fighter name is changed (So change that first)")]
        [DisplayName("Pac File Name")]
        public string PacName
        {
            get => _pacName;
            set
            {
                if (value.Length > 0)
                {
                    if (value.Length <= 46)
                    {
                        _pacName = value;
                    }
                    else
                    {
                        _pacName = value.Substring(0, 46);
                    }

                    SignalPropertyChange();
                }
            }
        }

        [Category("\t\tFighter")]
        [Description("This is changed automatically when the fighter name is changed (So change that first)")]
        [DisplayName("Kirby Pac File Name")]
        public string KirbyPacName
        {
            get => _kirbyPacName;
            set
            {
                if (value.Length > 0)
                {
                    if (value.Length <= 46)
                    {
                        _kirbyPacName = value;
                    }
                    else
                    {
                        _kirbyPacName = value.Substring(0, 46);
                    }

                    SignalPropertyChange();
                }
            }
        }

        [Category("\t\tFighter")]
        [Description("This is changed automatically when the fighter name is changed (So change that first)")]
        [DisplayName("Module File Name")]
        public string ModuleName
        {
            get => _moduleName;
            set
            {
                if (value.Length > 0)
                {
                    if (value.Length <= 30)
                    {
                        _moduleName = value;
                    }
                    else
                    {
                        _moduleName = value.Substring(0, 30);
                    }

                    SignalPropertyChange();
                }
            }
        }

        public bool _hasInternalName = true;

        [Browsable(false)]
        [Category("\t\tFighter")]
        [Description("Whether or not the fighter has an internal name. Never appears to be set to false.")]
        [DisplayName("Has Internal Name")]
        public bool HasInternalName
        {
            get => _hasInternalName;
            set
            {
                _hasInternalName = value;
                SignalPropertyChange();
            }
        }

        [Category("\t\tFighter")]
        [Description("This is changed automatically when the fighter name is changed (So change that first)")]
        [DisplayName("Internal Fighter Name")]
        public string InternalFighterName
        {
            get => _internalName;
            set
            {
                if (value.Length > 0)
                {
                    if (value.Length <= 16)
                    {
                        _internalName = value;
                    }
                    else
                    {
                        _internalName = value.Substring(0, 16);
                    }

                    SignalPropertyChange();
                }
            }
        }

        [Browsable(false)]
        [Category("\t\tFighter")]
        [DisplayName("Automatic Pac File Name")]
        public string AutoPacName => _fighterName.ToLower() + "/Fit" + _fighterName + ".pac";

        [Browsable(false)]
        [Category("\t\tFighter")]
        [DisplayName("Automatic Kirby Pac File Name")]
        public string AutoKirbyPacName => "kirby/FitKirby" + _fighterName + ".pac";

        [Browsable(false)]
        [Category("\t\tFighter")]
        [DisplayName("Automatic Module File Name")]
        public string AutoModuleName => "ft_" + _fighterName.ToLower() + ".rel";

        [Browsable(false)]
        [Category("\t\tFighter")]
        [DisplayName("Automatic Internal Fighter Name")]
        public string AutoInternalFighterName => _fighterName.ToUpper();

        [Category("\tAbilities")]
        [Description("Specifies whether the fighter can crawl.")]
        [DisplayName("Can Crawl")]
        public bool CanCrawl
        {
            get => Convert.ToBoolean(_canCrawl);
            set
            {
                _canCrawl = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }

        [Category("\tAbilities")]
        [Description("Specifies how many forward tilts the fighter has.")]
        [DisplayName("Forward Tilt Count")]
        public byte CanFTilt
        {
            get => Convert.ToByte(_canTilt);
            set
            {
                _canTilt = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }

        [Category("\tAbilities")]
        [Description("Specifies whether the fighter can glide.")]
        [DisplayName("Can Glide")]
        public bool CanGlide
        {
            get => Convert.ToBoolean(_canGlide);
            set
            {
                _canGlide = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }

        [Category("\tAbilities")]
        [Description("Specifies whether the fighter can cling to walls.")]
        [DisplayName("Can Wall Cling")]
        public bool CanWallCling
        {
            get => Convert.ToBoolean(_canCling);
            set
            {
                _canCling = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }

        [Category("\tAbilities")]
        [Description("Specifies whether the fighter can wall jump.")]
        [DisplayName("Can Wall Jump")]
        public bool CanWallJump
        {
            get => Convert.ToBoolean(_canWallJump);
            set
            {
                _canWallJump = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }

        [Category("\tAbilities")]
        [Description("Specifies whether the fighter can execute an aerial tether using Z.")]
        [DisplayName("Can Z-Air")]
        public bool CanZAir
        {
            get => Convert.ToBoolean(_canZAir);
            set
            {
                _canZAir = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("\tCharacteristics")]
        [Description("Specifies the number of mid-air jumps for the fighter.")]
        [DisplayName("Air Jump Count")]
        public AirJumpFlags AirJumpCount
        {
            get => (AirJumpFlags) _airJumpCount;
            set
            {
                _airJumpCount = (uint) value;
                SignalPropertyChange();
            }
        }

        [Category("\tCharacteristics")]
        [Description("Specifies whether the fighter will enter the crouch position after a dash attack.")]
        [DisplayName("Dash Attack Into Crouch")]
        public bool DAIntoCrouch
        {
            get => Convert.ToBoolean(_dashAttackIntoCrouch);
            set
            {
                _dashAttackIntoCrouch = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }

        [Category("\tCharacteristics")]
        [Description("Specifies how many times the fighter can chain their forward smash.")]
        [DisplayName("Forward Smash Count")]
        public uint FSmashCount
        {
            get => _fSmashCount;
            set
            {
                _fSmashCount = value;
                SignalPropertyChange();
            }
        }

        [Category("\tCharacteristics")]
        [Description("Specifies whether the fighter will use a rapid jab at the end of their jab combo.")]
        [DisplayName("Has Rapid Jab")]
        public bool HasRapidJab
        {
            get => Convert.ToBoolean(_hasRapidJab);
            set
            {
                _hasRapidJab = Convert.ToUInt32(value);
                SignalPropertyChange();
            }
        }


        [Category("\tCharacteristics")]
        [Description("Specifies the number of hits for the fighter's jab combo.")]
        [DisplayName("Jab Count")]
        public uint JabCount
        {
            get => _jabCount;
            set
            {
                _jabCount = value;
                SignalPropertyChange();
            }
        }

        [Category("\tCharacteristics")]
        [Description("Usage Unknown.")]
        [DisplayName("Jab Flag")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint JabFlag
        {
            get => _jabFlag;
            set
            {
                _jabFlag = value;
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume00")]
        public bool HasCostume00
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag00) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag00) |
                              (value ? CostumeLoadFlags.CostumeFlag00 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume01")]
        public bool HasCostume01
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag01) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag01) |
                              (value ? CostumeLoadFlags.CostumeFlag01 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume02")]
        public bool HasCostume02
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag02) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag02) |
                              (value ? CostumeLoadFlags.CostumeFlag02 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume03")]
        public bool HasCostume03
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag03) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag03) |
                              (value ? CostumeLoadFlags.CostumeFlag03 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume04")]
        public bool HasCostume04
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag04) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag04) |
                              (value ? CostumeLoadFlags.CostumeFlag04 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume05")]
        public bool HasCostume05
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag05) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag05) |
                              (value ? CostumeLoadFlags.CostumeFlag05 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume06")]
        public bool HasCostume06
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag06) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag06) |
                              (value ? CostumeLoadFlags.CostumeFlag06 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume07")]
        public bool HasCostume07
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag07) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag07) |
                              (value ? CostumeLoadFlags.CostumeFlag07 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume08")]
        public bool HasCostume08
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag08) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag08) |
                              (value ? CostumeLoadFlags.CostumeFlag08 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume09")]
        public bool HasCostume09
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag09) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag09) |
                              (value ? CostumeLoadFlags.CostumeFlag09 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume10")]
        public bool HasCostume10
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag10) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag10) |
                              (value ? CostumeLoadFlags.CostumeFlag10 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume11")]
        public bool HasCostume11
        {
            get => (_colorFlags & CostumeLoadFlags.CostumeFlag11) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag11) |
                              (value ? CostumeLoadFlags.CostumeFlag11 : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [Description("Normally set to True for every character except Mr. Game & Watch.")]
        [DisplayName("Unknown Flag A")]
        public bool UnknownFlagA
        {
            get => (_colorFlags & CostumeLoadFlags.UnknownFlag_A) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_A) |
                              (value ? CostumeLoadFlags.UnknownFlag_A : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [Description("Usage unknown.")]
        [DisplayName("Unknown Flag B")]
        public bool UnknownFlagB
        {
            get => (_colorFlags & CostumeLoadFlags.UnknownFlag_B) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_B) |
                              (value ? CostumeLoadFlags.UnknownFlag_B : 0);
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        [Category("\tCostumes")]
        [Description("Usage unknown. Theoretical.")]
        [DisplayName("Unknown Flag C")]
        public bool UnknownFlagC
        {
            get => (_colorFlags & CostumeLoadFlags.UnknownFlag_C) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_C) |
                              (value ? CostumeLoadFlags.UnknownFlag_C : 0);
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        [Category("\tCostumes")]
        [Description("Usage unknown. Theoretical.")]
        [DisplayName("Unknown Flag D")]
        public bool UnknownFlagD
        {
            get => (_colorFlags & CostumeLoadFlags.UnknownFlag_D) != 0;
            set
            {
                _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_D) |
                              (value ? CostumeLoadFlags.UnknownFlag_D : 0);
                SignalPropertyChange();
            }
        }

        public bool _hasPac = true;

        [Category("\tResources")]
        [Description("Whether or not the fighter has a .pac file. Normally only set to false for unused characters.")]
        [DisplayName("Has Pac")]
        public bool HasPac
        {
            get => _hasPac;
            set
            {
                _hasPac = value;
                SignalPropertyChange();
            }
        }

        public bool _hasModule = true;

        [Category("\tResources")]
        [Description("Whether or not the fighter has a .rel file. Normally only set to false for unused characters.")]
        [DisplayName("Has Module")]
        public bool HasModule
        {
            get => _hasModule;
            set
            {
                _hasModule = value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        public enum MotionEtcTypes
        {
            SingleSeparate,
            SingleMerged,
            PerCostumeSeparate,
            PerCostumeSet
        }

        [Browsable(true)]
        [Category("\tResources")]
        [Description(@"SingleSeparate: Use a single Motion and a single Etc file for all costumes
SingleMerged: Use a single MotionEtc file for all costumes
PerCostumeSeparate: Use a single Motion for all costumes and give each costume its own Etc file")]
        [DisplayName("MotionEtc Type")]
        public MotionEtcTypes MotionEtcType
        {
            get => PerCostumeEtc ? 
                (MergeMotionEtc ? MotionEtcTypes.PerCostumeSet : MotionEtcTypes.PerCostumeSeparate) :
                (MergeMotionEtc ? MotionEtcTypes.SingleMerged : MotionEtcTypes.SingleSeparate);
            set
            {
                switch (value)
                {
                    case MotionEtcTypes.SingleSeparate:
                        MergeMotionEtc = false;
                        PerCostumeEtc = false;
                        break;
                    case MotionEtcTypes.SingleMerged:
                        MergeMotionEtc = true;
                        PerCostumeEtc = false;
                        break;
                    case MotionEtcTypes.PerCostumeSeparate:
                        MergeMotionEtc = false;
                        PerCostumeEtc = true;
                        break;
                    case MotionEtcTypes.PerCostumeSet:
                        MergeMotionEtc = true;
                        PerCostumeEtc = true;
                        break;
                }
            }
        }

#if !DEBUG
        [Browsable(false)]
#endif
        [Category("\tResources")]
        [Description("Use one MotionEtc file instead of splitting them.")]
        [DisplayName("Merge Motion/Etc.")]
        public bool MergeMotionEtc
        {
            get => (_characterLoadFlags & CharacterLoadFlags.MergeMotionEtcFlag) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.MergeMotionEtcFlag) |
                                      (value ? CharacterLoadFlags.MergeMotionEtcFlag : 0);
                SignalPropertyChange();
            }
        }

#if !DEBUG
        [Browsable(false)]
#endif
        [Category("\tResources")]
        [Description("Determines whether or not per costume Etc. files are used")]
        [DisplayName("Per Costume Etc.")]
        public bool PerCostumeEtc
        {
            get => (_characterLoadFlags & CharacterLoadFlags.PerCostumeEtc) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.PerCostumeEtc) |
                                      (value ? CharacterLoadFlags.PerCostumeEtc : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tResources")]
        [Description("Unknown and unused in vanilla Brawl")]
        [DisplayName("Unknown Load Flag A")]
        public bool UnknownLoadFlagA
        {
            get => (_characterLoadFlags & CharacterLoadFlags.UnknownFlag_A) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.UnknownFlag_A) |
                                      (value ? CharacterLoadFlags.UnknownFlag_A : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tResources")]
        [Description("Unknown and unused in vanilla Brawl")]
        [DisplayName("Unknown Load Flag B")]
        public bool UnknownLoadFlagB
        {
            get => (_characterLoadFlags & CharacterLoadFlags.UnknownFlag_B) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.UnknownFlag_B) |
                                      (value ? CharacterLoadFlags.UnknownFlag_B : 0);
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("\tResources")]
        [Description("What (if any) FitEntry files will be loaded.")]
        [TypeConverter(typeof(EnumConverter))]
        [DisplayName("Entry Load Type")]
        public EntryResultLoadFlags EntryLoadType
        {
            get => (EntryResultLoadFlags) _entryColorFlags;
            set
            {
                _entryColorFlags = (byte) value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("\tResources")]
        [Description("What (if any) FitResult files will be loaded.")]
        [TypeConverter(typeof(EnumConverter))]
        [DisplayName("Results Load Type")]
        public EntryResultLoadFlags ResultLoadType
        {
            get => (EntryResultLoadFlags) _resultColorFlags;
            set
            {
                _resultColorFlags = (byte) value;
                SignalPropertyChange();
            }
        }

        // Set automatically by the Kirby Load Type
        public bool _hasKirbyHat = true;

        [Browsable(false)]
        [Category("\tResources")]
        [Description(
            "Whether or not the fighter has a Kirby hat. Normally set to false for characters like Giga Bowser, WarioMan, or the Alloys.")]
        [DisplayName("Has Kirby Hat")]
        public bool HasKirbyHat
        {
            get => _hasKirbyHat;
            set
            {
                _hasKirbyHat = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("\tResources")]
        [Description("What (if any) Kirby Hat files will be loaded.")]
        [TypeConverter(typeof(EnumConverter))]
        [DisplayName("Kirby Hat Load Type")]
        public KirbyLoadFlags KirbyLoadType
        {
            get => (KirbyLoadFlags) _kirbyCopyColorFlags;
            set
            {
                _kirbyCopyColorFlags = (byte) value;
                if (_kirbyCopyColorFlags == 0x00)
                {
                    HasKirbyHat = false;
                }
                else
                {
                    HasKirbyHat = true;
                }

                SignalPropertyChange();
            }
        }

        [Category("Fighter Config")]
        public uint Version
        {
            get => _version;
            set
            {
                _version = value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
        [Category("\tResources")]
        [Description("Whether to load a FitFinal")]
        [DisplayName("Load Final Smash")]
        public bool LoadFitFinal
        {
            get => (_characterLoadFlags & CharacterLoadFlags.FinalSmashFilesFlag) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.FinalSmashFilesFlag) |
                                      (value ? CharacterLoadFlags.FinalSmashFilesFlag : 0);
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("\tResources")]
        [Description("What (if any) FitFinal files will be loaded.")]
        [TypeConverter(typeof(EnumConverter))]
        [DisplayName("Final Load Type")]
        public FinalLoadFlags FinalLoadType
        {
            get => (FinalLoadFlags) _finalSmashColorFlags;
            set
            {
                _finalSmashColorFlags = (byte) value;
                if (_finalSmashColorFlags == 0x00)
                {
                    LoadFitFinal = false;
                }
                else
                {
                    LoadFitFinal = true;
                }

                SignalPropertyChange();
            }
        }

        [Category("\tSound")]
        [Description("Normally set to false for characters whose Final Smash is accompanied by music.")]
        [DisplayName("Final Smash Music Flag")]
        public bool FinalSmashMusic
        {
            get => (_characterLoadFlags & CharacterLoadFlags.FinalSmashMusicOffFlag) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.FinalSmashMusicOffFlag) |
                                      (value ? CharacterLoadFlags.FinalSmashMusicOffFlag : 0);
                SignalPropertyChange();
            }
        }

        [Category("\tSound")]
        [Description("Specifies the sound bank that is loaded for the fighter.")]
        [DisplayName("Sound Bank")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint SoundBank
        {
            get => _soundbank;
            set
            {
                _soundbank = value;
                SignalPropertyChange();
            }
        }

        [Category("\tSound")]
        [Description("Specifies the sound bank that is loaded for the fighter.")]
        [DisplayName("Kirby Sound Bank")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint KirbySoundBank
        {
            get => _kirbySoundbank;
            set
            {
                _kirbySoundbank = value;
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [TypeConverter(typeof(DropDownListBrawlExAIControllers))]
        [DisplayName("AI Controller")]
        public uint AIController
        {
            get => _aiController;
            set
            {
                _aiController = value;
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [Description("Usage Unknown.")]
        [DisplayName("Entry Flag")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint EntryFlag
        {
            get => _entryArticleFlag;
            set
            {
                _entryArticleFlag = value;
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [Description("Normally set to True for Ice Climbers and Olimar")]
        [DisplayName("Ik Physics Flag")]
        public bool IkPhysics
        {
            get => (_characterLoadFlags & CharacterLoadFlags.IkPhysicsFlag) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.IkPhysicsFlag) |
                                      (value ? CharacterLoadFlags.IkPhysicsFlag : 0);
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [Description("Specifies what Texture Loader to use when loading the fighter's entry articles.")]
        [DisplayName("Texture Loader")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint TextureLoader
        {
            get => _textureLoad;
            set
            {
                _textureLoad = value;
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [Description("Usage Unknown.")]
        [DisplayName("U12 Flag")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint U12Flag
        {
            get => _u12Flag;
            set
            {
                _u12Flag = value;
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [Description("Usage Unknown.")]
        [DisplayName("U13 Flag")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint U13Flag
        {
            get => _u13Flag;
            set
            {
                _u13Flag = value;
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [Description("Normally set to False for Mario, Luigi, Popo, Nana, and Pit")]
        [DisplayName("Work Manage Flag")]
        public bool WorkManage
        {
            get => (_characterLoadFlags & CharacterLoadFlags.WorkManageFlag) != 0;
            set
            {
                _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.WorkManageFlag) |
                                      (value ? CharacterLoadFlags.WorkManageFlag : 0);
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return FCFG.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            FCFG* hdr = (FCFG*) address;
            hdr->_tag = _tag;
            hdr->_size = _size;
            hdr->_version = _version;
            hdr->_editFlag1 = _editFlag1;
            hdr->_editFlag2 = _editFlag2;
            hdr->_editFlag3 = _editFlag3;
            hdr->_editFlag4 = _editFlag4;
            hdr->_kirbyCopyColorFlags = _kirbyCopyColorFlags;
            hdr->_entryColorFlags = _entryColorFlags;
            hdr->_resultColorFlags = _resultColorFlags;
            hdr->_characterLoadFlags = (byte) _characterLoadFlags;
            hdr->_finalSmashColorFlags = _finalSmashColorFlags;
            if (_version == 1) // None and Single Final Smash Load Flags were switched in v1, correct here
            {
                switch ((FinalLoadFlags)_finalSmashColorFlags)
                {
                    case FinalLoadFlags.Single:
                        hdr->_finalSmashColorFlags = (byte)FinalLoadFlagsV1.Single;
                        break;
                    case FinalLoadFlags.None:
                        hdr->_finalSmashColorFlags = (byte)FinalLoadFlagsV1.None;
                        break;
                }
            }
            hdr->_unknown0x15 = _unknown0x15;
            hdr->_colorFlags = (ushort) _colorFlags;
            hdr->_entryArticleFlag = _entryArticleFlag;
            hdr->_soundbank = _soundbank;
            hdr->_kirbySoundbank = _kirbySoundbank;
            hdr->_unknown0x24 = _unknown0x24;
            hdr->_unknown0x28 = _unknown0x28;
            hdr->_unknown0x2C = _unknown0x2C;

            for (int i = 0; i < 48; i++)
            {
                hdr->_pacNameArray[i] = 0x00;
                hdr->_kirbyPacNameArray[i] = 0x00;
                if (i < 32)
                {
                    hdr->_moduleNameArray[i] = 0x00;
                }

                if (i < 16)
                {
                    hdr->_internalNameArray[i] = 0x00;
                }
            }

            _pacNameArray = Encoding.UTF8.GetBytes(_pacName);
            _kirbyPacNameArray = Encoding.UTF8.GetBytes(_kirbyPacName);
            _moduleNameArray = Encoding.UTF8.GetBytes(_moduleName);
            _internalNameArray = Encoding.UTF8.GetBytes(_internalName);

            for (int i = 0; i < _pacNameArray.Length; i++)
            {
                hdr->_pacNameArray[i] = _pacNameArray[i];
            }

            for (int i = 0; i < _kirbyPacNameArray.Length; i++)
            {
                hdr->_kirbyPacNameArray[i] = _kirbyPacNameArray[i];
            }

            for (int i = 0; i < _moduleNameArray.Length; i++)
            {
                hdr->_moduleNameArray[i] = _moduleNameArray[i];
            }

            for (int i = 0; i < _internalNameArray.Length; i++)
            {
                hdr->_internalNameArray[i] = _internalNameArray[i];
            }

            if (!HasPac && _pacName.Length < 48)
            {
                hdr->_pacNameArray[_pacName.Length + 1] = 0x78;
            }

            if (!HasKirbyHat && _kirbyPacName.Length < 48)
            {
                hdr->_kirbyPacNameArray[_kirbyPacName.Length + 1] = 0x78;
            }

            if (!HasModule && _moduleName.Length < 32)
            {
                hdr->_moduleNameArray[_moduleName.Length + 1] = 0x78;
            }
            // Seemingly unused
            /*if (!HasInternalName)
                hdr->_internalNameArray[_internalName.Length + 1] = 0x78;*/

            hdr->_jabFlag = _jabFlag;
            hdr->_jabCount = _jabCount;
            hdr->_hasRapidJab = _hasRapidJab;
            hdr->_canTilt = _canTilt;
            hdr->_fSmashCount = _fSmashCount;
            hdr->_airJumpCount = _airJumpCount;
            hdr->_canGlide = _canGlide;
            hdr->_canCrawl = _canCrawl;
            hdr->_dashAttackIntoCrouch = _dashAttackIntoCrouch;
            hdr->_canWallJump = _canWallJump;
            hdr->_canCling = _canCling;
            hdr->_canZAir = _canZAir;
            hdr->_u12Flag = _u12Flag;
            hdr->_u13Flag = _u13Flag;
            hdr->_textureLoad = _textureLoad;
            hdr->_aiController = _aiController;
        }

        public override bool OnInitialize()
        {
            _tag = Header->_tag;
            _size = Header->_size;
            _version = Header->_version;
            _editFlag1 = Header->_editFlag1;
            _editFlag2 = Header->_editFlag2;
            _editFlag3 = Header->_editFlag3;
            _editFlag4 = Header->_editFlag4;
            _kirbyCopyColorFlags = Header->_kirbyCopyColorFlags;
            _entryColorFlags = Header->_entryColorFlags;
            _resultColorFlags = Header->_resultColorFlags;
            _characterLoadFlags = (CharacterLoadFlags) Header->_characterLoadFlags;
            _finalSmashColorFlags = Header->_finalSmashColorFlags;
            if (_version == 1) // None and Single Final Smash Load Flags were switched in v1, correct here
            {
                if ((FinalLoadFlagsV1)_finalSmashColorFlags == FinalLoadFlagsV1.Single)
                {
                    _finalSmashColorFlags = (byte)FinalLoadFlags.Single;
                }
                else if ((FinalLoadFlagsV1)_finalSmashColorFlags == FinalLoadFlagsV1.None)
                {
                    _finalSmashColorFlags = (byte)FinalLoadFlags.None;
                }
            }
            _unknown0x15 = Header->_unknown0x15;
            _colorFlags = (CostumeLoadFlags) (ushort) Header->_colorFlags;
            _entryArticleFlag = Header->_entryArticleFlag;
            _soundbank = Header->_soundbank;
            _kirbySoundbank = Header->_kirbySoundbank;
            _unknown0x24 = Header->_unknown0x24;
            _unknown0x28 = Header->_unknown0x28;
            _unknown0x2C = Header->_unknown0x2C;

            for (int i = 0; i < _pacNameArray.Length; i++)
            {
                _pacNameArray[i] = Header->_pacNameArray[i];
            }

            for (int i = 0; i < _kirbyPacNameArray.Length; i++)
            {
                _kirbyPacNameArray[i] = Header->_kirbyPacNameArray[i];
            }

            for (int i = 0; i < _moduleNameArray.Length; i++)
            {
                _moduleNameArray[i] = Header->_moduleNameArray[i];
            }

            for (int i = 0; i < _internalNameArray.Length; i++)
            {
                _internalNameArray[i] = Header->_internalNameArray[i];
            }

            _pacName = Encoding.UTF8.GetString(_pacNameArray)
                .Substring(0, Encoding.UTF8.GetString(_pacNameArray).IndexOf('\0'))
                .TrimEnd('\0');
            _kirbyPacName = Encoding.UTF8.GetString(_kirbyPacNameArray)
                .Substring(0, Encoding.UTF8.GetString(_kirbyPacNameArray).IndexOf('\0'))
                .TrimEnd('\0');
            _moduleName = Encoding.UTF8.GetString(_moduleNameArray)
                .Substring(0, Encoding.UTF8.GetString(_moduleNameArray).IndexOf('\0'))
                .TrimEnd('\0');
            _internalName = Encoding.UTF8.GetString(_internalNameArray)
                .Substring(0, Encoding.UTF8.GetString(_internalNameArray).IndexOf('\0'))
                .TrimEnd('\0');
            try
            {
                if (_pacName.ToUpper().LastIndexOf(".PAC") > 0 && _pacName.ToUpper().Contains("/FIT"))
                {
                    _fighterName = _pacName.Substring(_pacName.ToUpper().LastIndexOf("/FIT") + 4,
                        _pacName.ToUpper().LastIndexOf(".PAC") - (_pacName.ToUpper().LastIndexOf("/FIT") + 4));
                }
                else
                {
                    _fighterName = _internalName;
                }
            }
            catch
            {
                _fighterName = _internalName;
            }

            if (Encoding.UTF8.GetString(_pacNameArray).ToUpper().TrimEnd('\0').EndsWith("\0X"))
            {
                _hasPac = false;
            }

            if (Encoding.UTF8.GetString(_kirbyPacNameArray).ToUpper().TrimEnd('\0').EndsWith("\0X"))
            {
                _hasKirbyHat = false;
            }

            if (Encoding.UTF8.GetString(_moduleNameArray).ToUpper().TrimEnd('\0').EndsWith("\0X"))
            {
                _hasModule = false;
            }

            if (Encoding.UTF8.GetString(_internalNameArray).ToUpper().TrimEnd('\0').EndsWith("\0X"))
            {
                _hasInternalName = false;
            }

            _jabFlag = Header->_jabFlag;
            _jabCount = Header->_jabCount;
            _hasRapidJab = Header->_hasRapidJab;
            _canTilt = Header->_canTilt;
            _fSmashCount = Header->_fSmashCount;
            _airJumpCount = Header->_airJumpCount;
            _canGlide = Header->_canGlide;
            _canCrawl = Header->_canCrawl;
            _dashAttackIntoCrouch = Header->_dashAttackIntoCrouch;
            _canWallJump = Header->_canWallJump;
            _canCling = Header->_canCling;
            _canZAir = Header->_canZAir;
            _u12Flag = Header->_u12Flag;
            _u13Flag = Header->_u13Flag;

            _textureLoad = Header->_textureLoad;
            _aiController = Header->_aiController;

            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            return false;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((FCFG*) source.Address)->_tag == FCFG.Tag1 || ((FCFG*) source.Address)->_tag == FCFG.Tag2
                ? new FCFGNode()
                : null;
        }
    }
}
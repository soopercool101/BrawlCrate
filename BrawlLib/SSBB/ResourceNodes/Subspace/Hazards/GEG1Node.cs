using BrawlLib.Internal;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GEG1Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GEG1EntryNode);
        public override ResourceType ResourceFileType => ResourceType.GEG1;
        protected override string baseName => "Subspace Enemies";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GEG1" ? new GEG1Node() : null;
        }
    }

    public enum EnemyList : ushort
    {
        Goomba = 0,
        Poppant = 1,
        Feyesh = 2,
        Jyk = 3,
        Auroros = 4,
        Cymul = 5,
        Roturret = 6,
        Borboras = 7,
        GiantGoomba = 8,
        Buckot = 9,
        Bucculus = 10,
        Greap = 11,
        Armight = 12,
        BulletBill = 13,
        Roader = 14,
        Spaak = 15,
        Mite = 16,
        Ticken = 17,
        Towtow = 18,
        HammerBro = 19,
        Bytan = 20,
        Floow = 21,
        Puppit = 22,
        Primid = 23,
        Shellpod = 24,
        Koopa = 25,
        Shaydas = 26,
        Bombed = 27,
        MetalPrimid = 28,
        Nagagog = 29,
        Trowlon = 30,
        BigPrimid = 31,
        BoomerangPrimid = 32,
        FirePrimid = 33,
        ScopePrimid = 34,
        SwordPrimid = 35,
        Gamyga = 36,
        ROBBlaster = 37,
        ROBDistance = 38,
        ROBLauncher = 39,
        ROBSentry = 40,
        Autolance = 41,
        Armank = 42,
        Glire = 43,
        Glice = 44,
        Glunder = 45,
        PeteyPiranha = 46,
        GamygaBase01_Top = 47,
        GamygaBase02 = 48,
        GamygaBase03 = 49,
        GamygaBase04_Bottom = 50,
        Galleom = 51,
        Ridley = 52,
        Rayquaza = 53,
        Duon = 54,
        Porky = 55,
        MetaRidley = 56,
        FalconFlyer = 57,
        Tabuu = 58,
        MasterHand = 59,
        CrazyHand = 60
    }

    public unsafe class GEG1EntryNode : ResourceNode, IRenderedLink
    {
        internal GEG1Entry Data;
        public List<ResourceNode> RenderTargets
        {
            get
            {
                List<ResourceNode> _targets = new List<ResourceNode>();
                if (Parent?.Parent?.Parent?.Parent?.Children.FirstOrDefault(o => o is ARCEntryNode ae && ae.FileType == ARCFileType.MiscData && ae.FileIndex == 0) is ARCNode a)
                {
                    ResourceNode model = a.Children.FirstOrDefault(c => c is ARCEntryNode ae && ae.FileType == ARCFileType.MiscData && ae.FileIndex == EnemyBrresID);
                    if (model != null)
                        _targets.Add(model);
                }
                return _targets;
            }
        }
        public override ResourceType ResourceFileType => ResourceType.ENEMY;

        public void RegenName()
        {
            Name = EnemyNameList();
        }

        internal MotionPathDataClass _motionPathData;
        [Category("GEG1")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public MotionPathDataClass MotionPathData
        {
            get => _motionPathData;
            set
            {
                _motionPathData = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public short EpbmIndex
        {
            get => Data._epbmIndex;
            set
            {
                Data._epbmIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public short EpspIndex
        {
            get => Data._epspIndex;
            set
            {
                Data._epspIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public ushort ConnectedEnemyID
        {
            get => Data._connectedEnemyID;
            set
            {
                Data._connectedEnemyID = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public short Unknown0x0E
        {
            get => Data._unknown0x0E;
            set
            {
                Data._unknown0x0E = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public ushort EnemyID
        {
            get => Data._enemyID;
            set
            {
                Data._enemyID = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        [DisplayName("Enemy ARC ID")]
        public int EnemyArcID => EnemyID * 2;

        [Category("GEG1")]
        [DisplayName("Enemy BRRES ARC ID")]
        public int EnemyBrresID => EnemyID * 2 + 1;

        [Category("Unknown")]
        public short Unknown0x1E
        {
            get => Data._unknown0x1E;
            set
            {
                Data._unknown0x1E = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public uint StartingAction
        {
            get => Data._startingAction;
            set
            {
                Data._startingAction = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 SpawnPos
        {
            get => new Vector3(Data._spawnPosX, Data._spawnPosY, Data._spawnPosZ);
            set
            {
                Data._spawnPosX = value._x;
                Data._spawnPosY = value._y;
                Data._spawnPosZ = value._z;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public float PosX1
        {
            get => Data._posX1;
            set
            {
                Data._posX1 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public float PosX2
        {
            get => Data._posX2;
            set
            {
                Data._posX2 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public float PosY1
        {
            get => Data._posY1;
            set
            {
                Data._posY1 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public float PosY2
        {
            get => Data._posY2;
            set
            {
                Data._posY2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x48
        {
            get => Data._unknown0x48;
            set
            {
                Data._unknown0x48 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x4C
        {
            get => Data._unknown0x4C;
            set
            {
                Data._unknown0x4C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x50
        {
            get => Data._unknown0x50;
            set
            {
                Data._unknown0x50 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x54
        {
            get => Data._unknown0x54;
            set
            {
                Data._unknown0x54 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x58
        {
            get => Data._unknown0x58;
            set
            {
                Data._unknown0x58 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x5C
        {
            get => Data._unknown0x5C;
            set
            {
                Data._unknown0x5C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x60
        {
            get => Data._unknown0x60;
            set
            {
                Data._unknown0x60 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x64
        {
            get => Data._unknown0x64;
            set
            {
                Data._unknown0x64 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x65
        {
            get => Data._unknown0x65;
            set
            {
                Data._unknown0x65 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x66
        {
            get => Data._unknown0x66;
            set
            {
                Data._unknown0x66 = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public bool IsFacingRight
        {
            get => Data._isFacingRight;
            set
            {
                Data._isFacingRight = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x68
        {
            get => Data._unknown0x68;
            set
            {
                Data._unknown0x68 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x69
        {
            get => Data._unknown0x69;
            set
            {
                Data._unknown0x69 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x6A
        {
            get => Data._unknown0x6A;
            set
            {
                Data._unknown0x6A = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x6B
        {
            get => Data._unknown0x6B;
            set
            {
                Data._unknown0x6B = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public int ItemDropGenParamId
        {
            get => Data._itemDropGenParamId;
            set
            {
                Data._itemDropGenParamId = value;
                SignalPropertyChange();
            }
        }

        [Category("GEG1")]
        public int RareItemDropGenParamId
        {
            get => Data._rareItemDropGenParamId;
            set
            {
                Data._rareItemDropGenParamId = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x74
        {
            get => Data._unknown0x74;
            set
            {
                Data._unknown0x74 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x75
        {
            get => Data._unknown0x75;
            set
            {
                Data._unknown0x75 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x76
        {
            get => Data._unknown0x76;
            set
            {
                Data._unknown0x76 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x77
        {
            get => Data._unknown0x77;
            set
            {
                Data._unknown0x77 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x78
        {
            get => Data._unknown0x78;
            set
            {
                Data._unknown0x78 = value;
                SignalPropertyChange();
            }
        }

        internal TriggerDataClass _spawnTrigger;
        [Category("GEG1")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass SpawnTrigger
        {
            get => _spawnTrigger;
            set
            {
                _spawnTrigger = value;
                SignalPropertyChange();
            }
        }

        internal TriggerDataClass _defeatTrigger;
        [Category("GEG1")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass DefeatTrigger
        {
            get => _defeatTrigger;
            set
            {
                _defeatTrigger = value;
                SignalPropertyChange();
            }
        }

        public GEG1EntryNode()
        {
            _motionPathData = new MotionPathDataClass(this);
            _spawnTrigger = new TriggerDataClass(this);
            _defeatTrigger = new TriggerDataClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *(GEG1Entry*)WorkingUncompressed.Address;
            if (_name == null)
            {
                _name = EnemyNameList();
            }

            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _spawnTrigger = new TriggerDataClass(this, Data._spawnTrigger);
            _defeatTrigger = new TriggerDataClass(this, Data._defeatTrigger);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GEG1Entry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Data._motionPathData = _motionPathData;
            Data._spawnTrigger = _spawnTrigger;
            Data._defeatTrigger = _defeatTrigger;
            GEG1Entry* hdr = (GEG1Entry*)address;
            *hdr = Data;
        }

        private string EnemyNameList()
        {
            if (Enum.IsDefined(typeof(EnemyList), EnemyID))
            {
                switch (EnemyID)
                {
                    case 0:
                        return "Goomba [" + Index + "]";
                    case 1:
                        return "Poppant [" + Index + "]";
                    case 2:
                        return "Feyesh [" + Index + "]";
                    case 3:
                        return "Jyk [" + Index + "]";
                    case 4:
                        return "Auroros [" + Index + "]";
                    case 5:
                        return "Cymul [" + Index + "]";
                    case 6:
                        return "Roturret [" + Index + "]";
                    case 7:
                        return "Borboras [" + Index + "]";
                    case 8:
                        return "Giant Goomba [" + Index + "]";
                    case 9:
                        return "Buckot [" + Index + "]";
                    case 10:
                        return "Bucculus [" + Index + "]";
                    case 11:
                        return "Greap [" + Index + "]";
                    case 12:
                        return "Armight [" + Index + "]";
                    case 13:
                        return "Bullet Bill [" + Index + "]";
                    case 14:
                        return "Roader [" + Index + "]";
                    case 15:
                        return "Spaak [" + Index + "]";
                    case 16:
                        return "Mite [" + Index + "]";
                    case 17:
                        return "Ticken [" + Index + "]";
                    case 18:
                        return "Towtow [" + Index + "]";
                    case 19:
                        return "Hammer Bro [" + Index + "]";
                    case 20:
                        return "Bytan [" + Index + "]";
                    case 21:
                        return "Floow [" + Index + "]";
                    case 22:
                        return "Puppit [" + Index + "]";
                    case 23:
                        return "Primid [" + Index + "]";
                    case 24:
                        return "Shellpod [" + Index + "]";
                    case 25:
                        return "Koopa [" + Index + "]";
                    case 26:
                        return "Shaydas [" + Index + "]";
                    case 27:
                        return "Bombed [" + Index + "]";
                    case 28:
                        return "Metal Primid [" + Index + "]";
                    case 29:
                        return "Nagagog [" + Index + "]";
                    case 30:
                        return "Trowlon [" + Index + "]";
                    case 31:
                        return "Big Primid [" + Index + "]";
                    case 32:
                        return "Boomerang Primid [" + Index + "]";
                    case 33:
                        return "Fire Primid [" + Index + "]";
                    case 34:
                        return "Scope Primid [" + Index + "]";
                    case 35:
                        return "Sword Primid [" + Index + "]";
                    case 36:
                        return "Gamyga [" + Index + "]";
                    case 37:
                        return "R.O.B. Blaster [" + Index + "]";
                    case 38:
                        return "R.O.B. Distance (?) [" + Index + "]";
                    case 39:
                        return "R.O.B. Launcher [" + Index + "]";
                    case 40:
                        return "R.O.B. Sentry [" + Index + "]";
                    case 41:
                        return "Autolance [" + Index + "]";
                    case 42:
                        return "Armank [" + Index + "]";
                    case 43:
                        return "Glire [" + Index + "]";
                    case 44:
                        return "Glice [" + Index + "]";
                    case 45:
                        return "Glunder [" + Index + "]";
                    case 46:
                        return "Petey Piranha [" + Index + "]";
                    case 47:
                        return "Gamyga Base 01 (Top) [" + Index + "]";
                    case 48:
                        return "Gamyga Base 02 [" + Index + "]";
                    case 49:
                        return "Gamyga Base 03 [" + Index + "]";
                    case 50:
                        return "Gamyga Base 04 (Bottom) [" + Index + "]";
                    case 51:
                        return "Galleom [" + Index + "]";
                    case 52:
                        return "Ridley [" + Index + "]";
                    case 53:
                        return "Rayquaza [" + Index + "]";
                    case 54:
                        return "Duon [" + Index + "]";
                    case 55:
                        return "Porky [" + Index + "]";
                    case 56:
                        return "Meta Ridley [" + Index + "]";
                    case 57:
                        return "Falcon Flyer [" + Index + "]";
                    case 58:
                        return "Tabuu [" + Index + "]";
                    case 59:
                        return "Master Hand [" + Index + "]";
                    case 60:
                        return "Crazy Hand [" + Index + "]";
                    default:
                        return "Unknown Enemy [" + Index + "]";
                }
            }

            return "Unknown Enemy [" + Index + "]";
        }
    }
}
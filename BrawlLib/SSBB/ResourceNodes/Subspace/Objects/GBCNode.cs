using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Objects
{
    public unsafe class GBC1Node : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GBC1EntryNode);
        protected override string baseName => "Pathed Barrel Cannon";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GBC1" ? new GBC1Node() : null;
        }
    }

    public unsafe class GBC2Node : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GBC2EntryNode);
        protected override string baseName => "Static Barrel Cannon";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GBC2" ? new GBC2Node() : null;
        }
    }

    public unsafe abstract class GBCEntryNode : ResourceNode
    {
        internal GBCHeader Data;

        private MotionPathDataClass _motionPathData;
        [Category("GBC")]
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

        [Category("Unknown")]
        public uint Unknown0x008
        {
            get => Data._unknown0x008;
            set
            {
                Data._unknown0x008 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x00C
        {
            get => Data._unknown0x00C;
            set
            {
                Data._unknown0x00C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x010
        {
            get => Data._unknown0x010;
            set
            {
                Data._unknown0x010 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x014
        {
            get => Data._unknown0x014;
            set
            {
                Data._unknown0x014 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x018
        {
            get => Data._unknown0x018;
            set
            {
                Data._unknown0x018 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x01C
        {
            get => Data._unknown0x01C;
            set
            {
                Data._unknown0x01C = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 AreaOffsetPosition
        {
            get => new Vector2(Data._areaOffsetPosX, Data._areaOffsetPosY);
            set
            {
                Data._areaOffsetPosX = value.X;
                Data._areaOffsetPosY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 AreaRange
        {
            get => new Vector2(Data._areaRangeX, Data._areaRangeY);
            set
            {
                Data._areaRangeX = value.X;
                Data._areaRangeY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Position
        {
            get => new Vector2(Data._posX, Data._posY);
            set
            {
                Data._posX = value.X;
                Data._posY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public float Rotation
        {
            get => Data._rot;
            set
            {
                Data._rot = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public float MaxRotation
        {
            get => Data._maxRot;
            set
            {
                Data._maxRot = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 1")]
        public float DifficultyRotateSpeed1
        {
            get => Data._difficultyRotateSpeed1;
            set
            {
                Data._difficultyRotateSpeed1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 2")]
        public float DifficultyRotateSpeed2
        {
            get => Data._difficultyRotateSpeed2;
            set
            {
                Data._difficultyRotateSpeed2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 3")]
        public float DifficultyRotateSpeed3
        {
            get => Data._difficultyRotateSpeed3;
            set
            {
                Data._difficultyRotateSpeed3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 4")]
        public float DifficultyRotateSpeed4
        {
            get => Data._difficultyRotateSpeed4;
            set
            {
                Data._difficultyRotateSpeed4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 5")]
        public float DifficultyRotateSpeed5
        {
            get => Data._difficultyRotateSpeed5;
            set
            {
                Data._difficultyRotateSpeed5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 6")]
        public float DifficultyRotateSpeed6
        {
            get => Data._difficultyRotateSpeed6;
            set
            {
                Data._difficultyRotateSpeed6 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 7")]
        public float DifficultyRotateSpeed7
        {
            get => Data._difficultyRotateSpeed7;
            set
            {
                Data._difficultyRotateSpeed7 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 8")]
        public float DifficultyRotateSpeed8
        {
            get => Data._difficultyRotateSpeed8;
            set
            {
                Data._difficultyRotateSpeed8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 9")]
        public float DifficultyRotateSpeed9
        {
            get => Data._difficultyRotateSpeed9;
            set
            {
                Data._difficultyRotateSpeed9 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 10")]
        public float DifficultyRotateSpeed10
        {
            get => Data._difficultyRotateSpeed10;
            set
            {
                Data._difficultyRotateSpeed10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 11")]
        public float DifficultyRotateSpeed11
        {
            get => Data._difficultyRotateSpeed11;
            set
            {
                Data._difficultyRotateSpeed11 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 12")]
        public float DifficultyRotateSpeed12
        {
            get => Data._difficultyRotateSpeed12;
            set
            {
                Data._difficultyRotateSpeed12 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 13")]
        public float DifficultyRotateSpeed13
        {
            get => Data._difficultyRotateSpeed13;
            set
            {
                Data._difficultyRotateSpeed13 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 14")]
        public float DifficultyRotateSpeed14
        {
            get => Data._difficultyRotateSpeed14;
            set
            {
                Data._difficultyRotateSpeed14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Rotate Speed")]
        [DisplayName("Difficulty 15")]
        public float DifficultyRotateSpeed15
        {
            get => Data._difficultyRotateSpeed15;
            set
            {
                Data._difficultyRotateSpeed15 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 1")]
        public float DifficultyMotionRatio1
        {
            get => Data._difficultyMotionRatio1;
            set
            {
                Data._difficultyMotionRatio1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 2")]
        public float DifficultyMotionRatio2
        {
            get => Data._difficultyMotionRatio2;
            set
            {
                Data._difficultyMotionRatio2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 3")]
        public float DifficultyMotionRatio3
        {
            get => Data._difficultyMotionRatio3;
            set
            {
                Data._difficultyMotionRatio3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 4")]
        public float DifficultyMotionRatio4
        {
            get => Data._difficultyMotionRatio4;
            set
            {
                Data._difficultyMotionRatio4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 5")]
        public float DifficultyMotionRatio5
        {
            get => Data._difficultyMotionRatio5;
            set
            {
                Data._difficultyMotionRatio5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 6")]
        public float DifficultyMotionRatio6
        {
            get => Data._difficultyMotionRatio6;
            set
            {
                Data._difficultyMotionRatio6 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 7")]
        public float DifficultyMotionRatio7
        {
            get => Data._difficultyMotionRatio7;
            set
            {
                Data._difficultyMotionRatio7 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 8")]
        public float DifficultyMotionRatio8
        {
            get => Data._difficultyMotionRatio8;
            set
            {
                Data._difficultyMotionRatio8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 9")]
        public float DifficultyMotionRatio9
        {
            get => Data._difficultyMotionRatio9;
            set
            {
                Data._difficultyMotionRatio9 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 10")]
        public float DifficultyMotionRatio10
        {
            get => Data._difficultyMotionRatio10;
            set
            {
                Data._difficultyMotionRatio10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 11")]
        public float DifficultyMotionRatio11
        {
            get => Data._difficultyMotionRatio11;
            set
            {
                Data._difficultyMotionRatio11 = value;
                SignalPropertyChange();
            }
        }


        [Category("Motion Ratio")]
        [DisplayName("Difficulty 12")]
        public float DifficultyMotionRatio12
        {
            get => Data._difficultyMotionRatio12;
            set
            {
                Data._difficultyMotionRatio12 = value;
                SignalPropertyChange();
            }
        }


        [Category("Motion Ratio")]
        [DisplayName("Difficulty 13")]
        public float DifficultyMotionRatio13
        {
            get => Data._difficultyMotionRatio13;
            set
            {
                Data._difficultyMotionRatio13 = value;
                SignalPropertyChange();
            }
        }


        [Category("Motion Ratio")]
        [DisplayName("Difficulty 14")]
        public float DifficultyMotionRatio14
        {
            get => Data._difficultyMotionRatio14;
            set
            {
                Data._difficultyMotionRatio14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Motion Ratio")]
        [DisplayName("Difficulty 15")]
        public float DifficultyMotionRatio15
        {
            get => Data._difficultyMotionRatio15;
            set
            {
                Data._difficultyMotionRatio15 = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public uint MaxFrames
        {
            get => Data._maxFrames;
            set
            {
                Data._maxFrames = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public float MaxFireRotation
        {
            get => Data._maxFireRot;
            set
            {
                Data._maxFireRot = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 CameraOffset
        {
            get => new Vector2(Data._cameraOffsetX, Data._cameraOffsetY);
            set
            {
                Data._cameraOffsetX = value.X;
                Data._cameraOffsetY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public bool IsAutoFire
        {
            get => Data._isAutoFire == 1;
            set
            {
                Data._isAutoFire = value ? (byte)1 : (byte)0;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x0C9
        {
            get => Data._unknown0x0C9;
            set
            {

                Data._unknown0x0C9 = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public bool FullRotate
        {
            get => Data._fullRotate == 1;
            set
            {
                Data._fullRotate = value ? (byte)1 : (byte)0;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public bool AlwaysRotate
        {
            get => Data._alwaysRotate == 1;
            set
            {
                Data._alwaysRotate = value ? (byte)1 : (byte)0;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        public byte ModelDataIndex
        {
            get => Data._mdlIndex;
            set
            {
                Data._mdlIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x0CD
        {
            get => Data._unknown0x0CD;
            set
            {
                Data._unknown0x0CD = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public ushort Unknown0x0CE
        {
            get => Data._unknown0x0CE;
            set
            {
                Data._unknown0x0CE = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x0D0
        {
            get => Data._unknown0x0D0;
            set
            {
                Data._unknown0x0D0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x0D4
        {
            get => Data._unknown0x0D4;
            set
            {
                Data._unknown0x0D4 = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint EnterCannonTrigger
        {
            get => Data._enterCannonTrigger;
            set
            {
                Data._enterCannonTrigger = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint MotionPathTrigger
        {
            get => Data._motionPathTrigger;
            set
            {
                Data._motionPathTrigger = value;
                SignalPropertyChange();
            }
        }

        [Category("GBC")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint IsValidTrigger
        {
            get => Data._isValidTrigger;
            set
            {
                Data._isValidTrigger = value;
                SignalPropertyChange();
            }
        }

        private AttackDataClass _attackData;
        [Category("GBC")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public AttackDataClass AttackData
        {
            get => _attackData;
            set
            {
                _attackData = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x13C
        {
            get => Data._unknown0x13C;
            set
            {
                Data._unknown0x13C = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *((GBCHeader*)WorkingUncompressed.Address);

            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _attackData = new AttackDataClass(this, Data._attackData);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Data._motionPathData = MotionPathData;
            Data._attackData = AttackData;
        }
    }

    public unsafe class GBC1EntryNode : GBCEntryNode
    {
        private MotionPathDataClass _shootMotionPathData;
        [Category("GBC1")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public MotionPathDataClass ShootMotionPathData
        {
            get => _shootMotionPathData;
            set
            {
                _shootMotionPathData = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GBC1Entry.Size;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _shootMotionPathData = new MotionPathDataClass(this, ((GBC1Entry*)WorkingUncompressed.Address)->_shootMotionPathData);
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            GBC1Entry* hdr = (GBC1Entry*)address;
            hdr->_header = Data;
            hdr->_shootMotionPathData = ShootMotionPathData;
        }
    }

    public unsafe class GBC2EntryNode : GBCEntryNode
    {
        private float _shootSpeed;
        [Category("GBC2")]
        public float ShootSpeed
        {
            get => _shootSpeed;
            set
            {
                _shootSpeed = value;
                SignalPropertyChange();
            }
        }

        private float _shootTimerSpeed;
        [Category("GBC2")]
        public float ShootTimerSpeed
        {
            get => _shootTimerSpeed;
            set
            {
                _shootTimerSpeed = value;
                SignalPropertyChange();
            }
        }

        private float _shootAngleOffset;
        [Category("GBC2")]
        public float ShootAngleOffset
        {
            get => _shootAngleOffset;
            set
            {
                _shootAngleOffset = value;
                SignalPropertyChange();
            }
        }

        private float _shootStunTimerSpeed;
        [Category("GBC2")]
        public float ShootStunTimerSpeed
        {
            get => _shootStunTimerSpeed;
            set
            {
                _shootStunTimerSpeed = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GBC2Entry.Size;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            GBC2Entry* header = (GBC2Entry*)WorkingUncompressed.Address;
            _shootSpeed = header->_shootSpeed;
            _shootAngleOffset = header->_shootAngleOffset;
            _shootStunTimerSpeed = header->_shootStunTimerSpeed;
            _shootTimerSpeed = header->_shootTimerSpeed;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            GBC2Entry* hdr = (GBC2Entry*)address;
            hdr->_header = Data;
            hdr->_shootSpeed = _shootSpeed;
            hdr->_shootAngleOffset = _shootAngleOffset;
            hdr->_shootTimerSpeed = _shootTimerSpeed;
            hdr->_shootStunTimerSpeed = _shootStunTimerSpeed;
        }
    }
}
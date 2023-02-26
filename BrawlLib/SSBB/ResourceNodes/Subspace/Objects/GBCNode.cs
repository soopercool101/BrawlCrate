using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Objects
{
    public class GBC1Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GBC1EntryNode);
        protected override string baseName => "Pathed Barrel Cannon";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GBC1" ? new GBC1Node() : null;
        }
    }

    public class GBC2Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GBC2EntryNode);
        protected override string baseName => "Static Barrel Cannon";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GBC2" ? new GBC2Node() : null;
        }
    }

    public abstract unsafe class GBCEntryNode : ResourceNode
    {
        internal GBCHeader Data;
        public List<ResourceNode> RenderTargets
        {
            get
            {
                List<ResourceNode> _targets = new List<ResourceNode>();
                if (Parent?.Parent?.Parent is ARCNode a)
                {
                    if (ModelDataIndex != byte.MaxValue)
                    {
                        ResourceNode model = a.Children.FirstOrDefault(c => c is ARCEntryNode ae && ae.FileType == ARCFileType.ModelData && ae.FileIndex == ModelDataIndex);
                        if (model != null)
                            _targets.Add(model);
                    }
                }
                return _targets;
            }
        }

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

        public DifficultyRatiosClass _difficultyRotateSpeeds;
        [Category("GBC")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public DifficultyRatiosClass DifficultyRotateSpeeds
        {
            get => _difficultyRotateSpeeds;
            set
            {
                _difficultyRotateSpeeds = value;
                SignalPropertyChange();
            }
        }

        public DifficultyRatiosClass _difficultyMotionRatios;
        [Category("GBC")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public DifficultyRatiosClass DifficultyMotionRatios
        {
            get => _difficultyMotionRatios;
            set
            {
                _difficultyMotionRatios = value;
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

        public TriggerDataClass _enterCannonTrigger;
        [Category("GBC")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass EnterCannonTrigger
        {
            get => _enterCannonTrigger;
            set
            {
                _enterCannonTrigger = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _motionPathTrigger;
        [Category("GBC")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass MotionPathTrigger
        {
            get => _motionPathTrigger;
            set
            {
                _motionPathTrigger = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _isValidTrigger;
        [Category("GBC")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass IsValidTrigger
        {
            get => _isValidTrigger;
            set
            {
                _isValidTrigger = value;
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

        public GBCEntryNode()
        {
            Data = new GBCHeader();
            Data._unknown0x00C = 0x12;
            Data._unknown0x0CE = 0x08;
            _attackData = new AttackDataClass(this);
            _motionPathData = new MotionPathDataClass(this);
            _enterCannonTrigger = new TriggerDataClass(this);
            _motionPathTrigger = new TriggerDataClass(this);
            _isValidTrigger = new TriggerDataClass(this);
            _difficultyMotionRatios = new DifficultyRatiosClass(this);
            _difficultyRotateSpeeds = new DifficultyRatiosClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *((GBCHeader*)WorkingUncompressed.Address);

            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _attackData = new AttackDataClass(this, Data._attackData);
            _enterCannonTrigger = new TriggerDataClass(this, Data._enterCannonTrigger);
            _motionPathTrigger = new TriggerDataClass(this, Data._motionPathTrigger);
            _isValidTrigger = new TriggerDataClass(this, Data._isValidTrigger);
            _difficultyRotateSpeeds = new DifficultyRatiosClass(this, Data._difficultyRotateSpeeds);
            _difficultyMotionRatios = new DifficultyRatiosClass(this, Data._difficultyMotionRatios);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Data._motionPathData = MotionPathData;
            Data._attackData = AttackData;
            Data._enterCannonTrigger = EnterCannonTrigger;
            Data._motionPathTrigger = MotionPathTrigger;
            Data._isValidTrigger = IsValidTrigger;
            Data._difficultyRotateSpeeds = _difficultyRotateSpeeds;
            Data._difficultyMotionRatios = _difficultyMotionRatios;
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

        public GBC1EntryNode() : base()
        {
            _shootMotionPathData = new MotionPathDataClass(this, new MotionPathData());
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
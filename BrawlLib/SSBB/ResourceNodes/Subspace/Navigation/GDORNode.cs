using BrawlLib.Internal;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Navigation;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GDORNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GDOREntryNode);
        public override ResourceType ResourceFileType => ResourceType.GDOR;
        protected override string baseName => "Subspace Doors";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GDOR" ? new GDORNode() : null;
        }
    }

    public class GDBFNode : GDORNode
    {
        public override ResourceType ResourceFileType => ResourceType.GDBF;
        protected override string baseName => "Factory Doors";

        internal new static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GDBF" ? new GDBFNode() : null;
        }
    }

    public class GDTPNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GDTPEntryNode);
        protected override string baseName => "Three-Pin Doors";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GDTP" ? new GDTPNode() : null;
        }
    }

    public class GDBSNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GDBSEntryNode);
        protected override string baseName => "Boss Door";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GDBS" ? new GDBSNode() : null;
        }
    }

    public unsafe class GDOREntryNode : ResourceNode, IRenderedLink
    {
        public GDOREntry Data;
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

        public MotionPathDataClass _motionPathData;
        [Category("Door")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
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
        public uint Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public uint Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
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
        
        [Category("Door")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 AreaOffsetPos
        {
            get => new Vector2(Data._areaOffsetPosX, Data._areaOffsetPosY);
            set
            {
                Data._areaOffsetPosX = value.X;
                Data._areaOffsetPosY = value.Y;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
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
        
        [Category("Door")]
        [TypeConverter(typeof(HexOnlyByteConverter))]
        public byte LevelID
        {
            get => Data._levelId;
            set
            {
                Data._levelId = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        [TypeConverter(typeof(HexOnlyByteConverter))]
        public byte LevelSequenceID
        {
            get => Data._levelSequenceId;
            set
            {
                Data._levelSequenceId = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        [TypeConverter(typeof(HexOnlyByteConverter))]
        public byte LevelSegmentID
        {
            get => Data._levelSegmentId;
            set
            {
                Data._levelSegmentId = value;
                SignalPropertyChange();
            }
        }

        [Category("Door")]
        [TypeConverter(typeof(HexOnlyByteConverter))]
        public byte DoorIndex
        {
            get => Data._doorIndex;
            set
            {
                Data._doorIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Door")]
        public string JumpDoorID => $"{LevelID:X2}{LevelSequenceID:X2}{LevelSegmentID:X2}{DoorIndex:X2}";

        [Category("Door")]
        public DoorGimmickKind DoorGimmick
        {
            get => Data._doorGimmick;
            set
            {
                Data._doorGimmick = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x35
        {
            get => Data._unknown0x35;
            set
            {
                Data._unknown0x35 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        public byte ModelDataIndex
        {
            get => Data._modelIndex;
            set
            {
                Data._modelIndex = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        public bool PlayDoorTypeEffect
        {
            get => Data._playDoorTypeEffect;
            set
            {
                Data._playDoorTypeEffect = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Position
        {
            get => new Vector2(Data._positionX, Data._positionY);
            set
            {
                Data._positionX = value.X;
                Data._positionY = value.Y;
                SignalPropertyChange();
            }
        }
        
        public TriggerDataClass _openDoorTrigger;
        [Category("Door")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass OpenDoorTrigger
        {
            get => _openDoorTrigger;
            set
            {
                _openDoorTrigger = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        public DoorType DoorType
        {
            get => Data._doorType;
            set
            {
                Data._doorType = value;
                SignalPropertyChange();
            }
        }


        [Category("Unknown")]
        public byte Unknown0x45
        {
            get => Data._unknown0x45;
            set
            {
                Data._unknown0x45 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x46
        {
            get => Data._unknown0x46;
            set
            {
                Data._unknown0x46 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x47
        {
            get => Data._unknown0x47;
            set
            {
                Data._unknown0x47 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        [TypeConverter(typeof(HexIntConverter))]
        public int SoundID
        {
            get => Data._soundId;
            set
            {
                Data._soundId = value;
                SignalPropertyChange();
            }
        }
        
        public TriggerDataClass _motionPathTrigger;
        [Category("Door")]
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
        [Category("Door")]
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

        public DifficultyRatiosClass _difficultyMotionRatios;
        [Category("Door")]
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

        public GDOREntryNode()
        {
            _motionPathData = new MotionPathDataClass(this);
            _openDoorTrigger = new TriggerDataClass(this);
            _motionPathTrigger = new TriggerDataClass(this);
            _isValidTrigger = new TriggerDataClass(this);
            _difficultyMotionRatios = new DifficultyRatiosClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *(GDOREntry*) WorkingUncompressed.Address;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _openDoorTrigger = new TriggerDataClass(this, Data._openDoorTrigger);
            _motionPathTrigger = new TriggerDataClass(this, Data._motionPathTrigger);
            _isValidTrigger = new TriggerDataClass(this, Data._isValidTrigger);
            _difficultyMotionRatios = new DifficultyRatiosClass(this, Data._difficultyMotionRatios);

            if (_name == null)
            {
                _name = $"Door [{Index}]";
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GDOREntry* header = (GDOREntry*) address;
            Data._motionPathData = _motionPathData;
            Data._openDoorTrigger = _openDoorTrigger;
            Data._motionPathTrigger = _motionPathTrigger;
            Data._isValidTrigger = _isValidTrigger;
            Data._difficultyMotionRatios = _difficultyMotionRatios;
            *header = Data;
        }

        public override int OnCalculateSize(bool force)
        {
            return GDOREntry.Size;
        }
    }

    public unsafe class GDTPEntryNode : GDOREntryNode
    {
        public TriggerDataClass _unlockTrigger;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger
        {
            get => _unlockTrigger;
            set
            {
                _unlockTrigger = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _pinTrigger1;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass PinTrigger1
        {
            get => _pinTrigger1;
            set
            {
                _pinTrigger1 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _pinTrigger2;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass PinTrigger2
        {
            get => _pinTrigger2;
            set
            {
                _pinTrigger2 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _pinTrigger3;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass PinTrigger3
        {
            get => _pinTrigger3;
            set
            {
                _pinTrigger3 = value;
                SignalPropertyChange();
            }
        }

        public GDTPEntryNode()
        {
            _unlockTrigger = new TriggerDataClass(this);
            _pinTrigger1 = new TriggerDataClass(this);
            _pinTrigger2 = new TriggerDataClass(this);
            _pinTrigger3 = new TriggerDataClass(this);
        }
        
        public override bool OnInitialize()
        {
            GDTPEntry data = *(GDTPEntry*) WorkingUncompressed.Address;
            _unlockTrigger = new TriggerDataClass(this, data._unlockTrigger);
            _pinTrigger1 = new TriggerDataClass(this, data._pinTrigger1);
            _pinTrigger2 = new TriggerDataClass(this, data._pinTrigger2);
            _pinTrigger3 = new TriggerDataClass(this, data._pinTrigger3);

            base.OnInitialize();

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GDTPEntry* header = (GDTPEntry*)address;
            Data._motionPathData = _motionPathData;
            Data._openDoorTrigger = _openDoorTrigger;
            Data._motionPathTrigger = _motionPathTrigger;
            Data._isValidTrigger = _isValidTrigger;
            GDTPEntry gdtp = new GDTPEntry { _doorHeader = Data };
            gdtp._unlockTrigger = _unlockTrigger;
            gdtp._pinTrigger1 = _pinTrigger1;
            gdtp._pinTrigger2 = _pinTrigger2;
            gdtp._pinTrigger3 = _pinTrigger3;
            *header = gdtp;
        }

        public override int OnCalculateSize(bool force)
        {
            return GDTPEntry.Size;
        }
    }

    public unsafe class GDBSEntryNode : GDOREntryNode
    {
        public TriggerDataClass _unlockTrigger1;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger1
        {
            get => _unlockTrigger1;
            set
            {
                _unlockTrigger1 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger2;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger2
        {
            get => _unlockTrigger2;
            set
            {
                _unlockTrigger2 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger3;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger3
        {
            get => _unlockTrigger3;
            set
            {
                _unlockTrigger3 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger4;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger4
        {
            get => _unlockTrigger4;
            set
            {
                _unlockTrigger4 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger5;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger5
        {
            get => _unlockTrigger5;
            set
            {
                _unlockTrigger5 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger6;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger6
        {
            get => _unlockTrigger6;
            set
            {
                _unlockTrigger6 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger7;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger7
        {
            get => _unlockTrigger7;
            set
            {
                _unlockTrigger7 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger8;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger8
        {
            get => _unlockTrigger8;
            set
            {
                _unlockTrigger8 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger9;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger9
        {
            get => _unlockTrigger9;
            set
            {
                _unlockTrigger9 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger10;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger10
        {
            get => _unlockTrigger10;
            set
            {
                _unlockTrigger10 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger11;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger11
        {
            get => _unlockTrigger11;
            set
            {
                _unlockTrigger11 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger12;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger12
        {
            get => _unlockTrigger12;
            set
            {
                _unlockTrigger12 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger13;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger13
        {
            get => _unlockTrigger13;
            set
            {
                _unlockTrigger13 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger14;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger14
        {
            get => _unlockTrigger14;
            set
            {
                _unlockTrigger14 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger15;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger15
        {
            get => _unlockTrigger15;
            set
            {
                _unlockTrigger15 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger16;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger16
        {
            get => _unlockTrigger16;
            set
            {
                _unlockTrigger16 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger17;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger17
        {
            get => _unlockTrigger17;
            set
            {
                _unlockTrigger17 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger18;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger18
        {
            get => _unlockTrigger18;
            set
            {
                _unlockTrigger18 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger19;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger19
        {
            get => _unlockTrigger19;
            set
            {
                _unlockTrigger19 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger20;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger20
        {
            get => _unlockTrigger20;
            set
            {
                _unlockTrigger20 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger21;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger21
        {
            get => _unlockTrigger21;
            set
            {
                _unlockTrigger21 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger22;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger22
        {
            get => _unlockTrigger22;
            set
            {
                _unlockTrigger22 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger23;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger23
        {
            get => _unlockTrigger23;
            set
            {
                _unlockTrigger23 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger24;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger24
        {
            get => _unlockTrigger24;
            set
            {
                _unlockTrigger24 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger25;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger25
        {
            get => _unlockTrigger25;
            set
            {
                _unlockTrigger25 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger26;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger26
        {
            get => _unlockTrigger26;
            set
            {
                _unlockTrigger26 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger27;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger27
        {
            get => _unlockTrigger27;
            set
            {
                _unlockTrigger27 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger28;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger28
        {
            get => _unlockTrigger28;
            set
            {
                _unlockTrigger28 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger29;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger29
        {
            get => _unlockTrigger29;
            set
            {
                _unlockTrigger29 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger30;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger30
        {
            get => _unlockTrigger30;
            set
            {
                _unlockTrigger30 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger31;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger31
        {
            get => _unlockTrigger31;
            set
            {
                _unlockTrigger31 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger32;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger32
        {
            get => _unlockTrigger32;
            set
            {
                _unlockTrigger32 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger33;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger33
        {
            get => _unlockTrigger33;
            set
            {
                _unlockTrigger33 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger34;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger34
        {
            get => _unlockTrigger34;
            set
            {
                _unlockTrigger34 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger35;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger35
        {
            get => _unlockTrigger35;
            set
            {
                _unlockTrigger35 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger36;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger36
        {
            get => _unlockTrigger36;
            set
            {
                _unlockTrigger36 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger37;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger37
        {
            get => _unlockTrigger37;
            set
            {
                _unlockTrigger37 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger38;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger38
        {
            get => _unlockTrigger38;
            set
            {
                _unlockTrigger38 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger39;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger39
        {
            get => _unlockTrigger39;
            set
            {
                _unlockTrigger39 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unlockTrigger40;

        [Category("GDBS")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass UnlockTrigger40
        {
            get => _unlockTrigger40;
            set
            {
                _unlockTrigger40 = value;
                SignalPropertyChange();
            }
        }


        public GDBSEntryNode()
        {
            _unlockTrigger1 = new TriggerDataClass(this);
            _unlockTrigger2 = new TriggerDataClass(this);
            _unlockTrigger3 = new TriggerDataClass(this);
            _unlockTrigger4 = new TriggerDataClass(this);
            _unlockTrigger5 = new TriggerDataClass(this);
            _unlockTrigger6 = new TriggerDataClass(this);
            _unlockTrigger7 = new TriggerDataClass(this);
            _unlockTrigger8 = new TriggerDataClass(this);
            _unlockTrigger9 = new TriggerDataClass(this);
            _unlockTrigger10 = new TriggerDataClass(this);
            _unlockTrigger11 = new TriggerDataClass(this);
            _unlockTrigger12 = new TriggerDataClass(this);
            _unlockTrigger13 = new TriggerDataClass(this);
            _unlockTrigger14 = new TriggerDataClass(this);
            _unlockTrigger15 = new TriggerDataClass(this);
            _unlockTrigger16 = new TriggerDataClass(this);
            _unlockTrigger17 = new TriggerDataClass(this);
            _unlockTrigger18 = new TriggerDataClass(this);
            _unlockTrigger19 = new TriggerDataClass(this);
            _unlockTrigger20 = new TriggerDataClass(this);
            _unlockTrigger21 = new TriggerDataClass(this);
            _unlockTrigger22 = new TriggerDataClass(this);
            _unlockTrigger23 = new TriggerDataClass(this);
            _unlockTrigger24 = new TriggerDataClass(this);
            _unlockTrigger25 = new TriggerDataClass(this);
            _unlockTrigger26 = new TriggerDataClass(this);
            _unlockTrigger27 = new TriggerDataClass(this);
            _unlockTrigger28 = new TriggerDataClass(this);
            _unlockTrigger29 = new TriggerDataClass(this);
            _unlockTrigger30 = new TriggerDataClass(this);
            _unlockTrigger31 = new TriggerDataClass(this);
            _unlockTrigger32 = new TriggerDataClass(this);
            _unlockTrigger33 = new TriggerDataClass(this);
            _unlockTrigger34 = new TriggerDataClass(this);
            _unlockTrigger35 = new TriggerDataClass(this);
            _unlockTrigger36 = new TriggerDataClass(this);
            _unlockTrigger37 = new TriggerDataClass(this);
            _unlockTrigger38 = new TriggerDataClass(this);
            _unlockTrigger39 = new TriggerDataClass(this);
            _unlockTrigger40 = new TriggerDataClass(this);
        }

        public override bool OnInitialize()
        {
            GDBSEntry data = *(GDBSEntry*)WorkingUncompressed.Address;
            _unlockTrigger1 = new TriggerDataClass(this, data._unlockTrigger1);
            _unlockTrigger2 = new TriggerDataClass(this, data._unlockTrigger2);
            _unlockTrigger3 = new TriggerDataClass(this, data._unlockTrigger3);
            _unlockTrigger4 = new TriggerDataClass(this, data._unlockTrigger4);
            _unlockTrigger5 = new TriggerDataClass(this, data._unlockTrigger5);
            _unlockTrigger6 = new TriggerDataClass(this, data._unlockTrigger6);
            _unlockTrigger7 = new TriggerDataClass(this, data._unlockTrigger7);
            _unlockTrigger8 = new TriggerDataClass(this, data._unlockTrigger8);
            _unlockTrigger9 = new TriggerDataClass(this, data._unlockTrigger9);
            _unlockTrigger10 = new TriggerDataClass(this, data._unlockTrigger10);
            _unlockTrigger11 = new TriggerDataClass(this, data._unlockTrigger11);
            _unlockTrigger12 = new TriggerDataClass(this, data._unlockTrigger12);
            _unlockTrigger13 = new TriggerDataClass(this, data._unlockTrigger13);
            _unlockTrigger14 = new TriggerDataClass(this, data._unlockTrigger14);
            _unlockTrigger15 = new TriggerDataClass(this, data._unlockTrigger15);
            _unlockTrigger16 = new TriggerDataClass(this, data._unlockTrigger16);
            _unlockTrigger17 = new TriggerDataClass(this, data._unlockTrigger17);
            _unlockTrigger18 = new TriggerDataClass(this, data._unlockTrigger18);
            _unlockTrigger19 = new TriggerDataClass(this, data._unlockTrigger19);
            _unlockTrigger20 = new TriggerDataClass(this, data._unlockTrigger20);
            _unlockTrigger21 = new TriggerDataClass(this, data._unlockTrigger21);
            _unlockTrigger22 = new TriggerDataClass(this, data._unlockTrigger22);
            _unlockTrigger23 = new TriggerDataClass(this, data._unlockTrigger23);
            _unlockTrigger24 = new TriggerDataClass(this, data._unlockTrigger24);
            _unlockTrigger25 = new TriggerDataClass(this, data._unlockTrigger25);
            _unlockTrigger26 = new TriggerDataClass(this, data._unlockTrigger26);
            _unlockTrigger27 = new TriggerDataClass(this, data._unlockTrigger27);
            _unlockTrigger28 = new TriggerDataClass(this, data._unlockTrigger28);
            _unlockTrigger29 = new TriggerDataClass(this, data._unlockTrigger29);
            _unlockTrigger30 = new TriggerDataClass(this, data._unlockTrigger30);
            _unlockTrigger31 = new TriggerDataClass(this, data._unlockTrigger31);
            _unlockTrigger32 = new TriggerDataClass(this, data._unlockTrigger32);
            _unlockTrigger33 = new TriggerDataClass(this, data._unlockTrigger33);
            _unlockTrigger34 = new TriggerDataClass(this, data._unlockTrigger34);
            _unlockTrigger35 = new TriggerDataClass(this, data._unlockTrigger35);
            _unlockTrigger36 = new TriggerDataClass(this, data._unlockTrigger36);
            _unlockTrigger37 = new TriggerDataClass(this, data._unlockTrigger37);
            _unlockTrigger38 = new TriggerDataClass(this, data._unlockTrigger38);
            _unlockTrigger39 = new TriggerDataClass(this, data._unlockTrigger39);
            _unlockTrigger40 = new TriggerDataClass(this, data._unlockTrigger40);

            base.OnInitialize();

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GDBSEntry* header = (GDBSEntry*)address;
            Data._motionPathData = _motionPathData;
            Data._openDoorTrigger = _openDoorTrigger;
            Data._motionPathTrigger = _motionPathTrigger;
            Data._isValidTrigger = _isValidTrigger;
            GDBSEntry gdbs = new GDBSEntry { _doorHeader = Data };
            gdbs._unlockTrigger1 = _unlockTrigger1;
            gdbs._unlockTrigger2 = _unlockTrigger2;
            gdbs._unlockTrigger3 = _unlockTrigger3;
            gdbs._unlockTrigger4 = _unlockTrigger4;
            gdbs._unlockTrigger5 = _unlockTrigger5;
            gdbs._unlockTrigger6 = _unlockTrigger6;
            gdbs._unlockTrigger7 = _unlockTrigger7;
            gdbs._unlockTrigger8 = _unlockTrigger8;
            gdbs._unlockTrigger9 = _unlockTrigger9;
            gdbs._unlockTrigger10 = _unlockTrigger10;
            gdbs._unlockTrigger11 = _unlockTrigger11;
            gdbs._unlockTrigger12 = _unlockTrigger12;
            gdbs._unlockTrigger13 = _unlockTrigger13;
            gdbs._unlockTrigger14 = _unlockTrigger14;
            gdbs._unlockTrigger15 = _unlockTrigger15;
            gdbs._unlockTrigger16 = _unlockTrigger16;
            gdbs._unlockTrigger17 = _unlockTrigger17;
            gdbs._unlockTrigger18 = _unlockTrigger18;
            gdbs._unlockTrigger19 = _unlockTrigger19;
            gdbs._unlockTrigger20 = _unlockTrigger20;
            gdbs._unlockTrigger21 = _unlockTrigger21;
            gdbs._unlockTrigger22 = _unlockTrigger22;
            gdbs._unlockTrigger23 = _unlockTrigger23;
            gdbs._unlockTrigger24 = _unlockTrigger24;
            gdbs._unlockTrigger25 = _unlockTrigger25;
            gdbs._unlockTrigger26 = _unlockTrigger26;
            gdbs._unlockTrigger27 = _unlockTrigger27;
            gdbs._unlockTrigger28 = _unlockTrigger28;
            gdbs._unlockTrigger29 = _unlockTrigger29;
            gdbs._unlockTrigger30 = _unlockTrigger30;
            gdbs._unlockTrigger31 = _unlockTrigger31;
            gdbs._unlockTrigger32 = _unlockTrigger32;
            gdbs._unlockTrigger33 = _unlockTrigger33;
            gdbs._unlockTrigger34 = _unlockTrigger34;
            gdbs._unlockTrigger35 = _unlockTrigger35;
            gdbs._unlockTrigger36 = _unlockTrigger36;
            gdbs._unlockTrigger37 = _unlockTrigger37;
            gdbs._unlockTrigger38 = _unlockTrigger38;
            gdbs._unlockTrigger39 = _unlockTrigger39;
            gdbs._unlockTrigger40 = _unlockTrigger40;
            *header = gdbs;
        }

        public override int OnCalculateSize(bool force)
        {
            return GDBSEntry.Size;
        }
    }
}
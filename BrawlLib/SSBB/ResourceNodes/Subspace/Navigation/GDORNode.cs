using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Navigation;
using System.ComponentModel;
using System;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using BrawlLib.SSBB.Types;
using System.Diagnostics;
using System.Windows.Markup;

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

    public unsafe class GDOREntryNode : ResourceNode
    {
        public GDOREntry Data;
        
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
        public byte DoorIndex
        {
            get => Data._doorIndex;
            set
            {
                Data._doorIndex = value;
                Name = $"Door [{value}]";
                SignalPropertyChange();
            }
        }
        
        [Category("Door")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint JumpData
        {
            get => Data._jumpData;
            set
            {
                Data._jumpData = value;
                SignalPropertyChange();
            }
        }

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
        public byte ModelIndex
        {
            get => Data._modelIndex;
            set
            {
                Data._modelIndex = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public byte Unknown0x37
        {
            get => Data._unknown0x37;
            set
            {
                Data._unknown0x37 = value;
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

#if DEBUG
        [Category("Door")]
        [TypeConverter(typeof(HexByteConverter))]
        public byte DoorTypeByte
        {
            get => (byte)Data._doorType;
            set
            {
                Data._doorType = (DoorType)value;
                SignalPropertyChange();
            }
        }
#endif
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

        public GDOREntryNode()
        {
            _motionPathData = new MotionPathDataClass(this);
            _openDoorTrigger = new TriggerDataClass(this);
            _motionPathTrigger = new TriggerDataClass(this);
            _isValidTrigger = new TriggerDataClass(this);

            _name = "Door [0]";
        }

        public override bool OnInitialize()
        {
            Data = *(GDOREntry*) WorkingUncompressed.Address;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _openDoorTrigger = new TriggerDataClass(this, Data._openDoorTrigger);
            _motionPathTrigger = new TriggerDataClass(this, Data._motionPathTrigger);
            _isValidTrigger = new TriggerDataClass(this, Data._isValidTrigger);

            if (_name == null)
            {
                _name = $"Door [{Data._doorIndex}]";
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
            *header = Data;
        }

        public override int OnCalculateSize(bool force)
        {
            return GDOREntry.Size;
        }
    }

    public unsafe class GDTPEntryNode : GDOREntryNode
    {

        public TriggerDataClass _trigger1;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger1
        {
            get => _trigger1;
            set
            {
                _trigger1 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger2;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger2
        {
            get => _trigger2;
            set
            {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger3;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger3
        {
            get => _trigger3;
            set
            {
                _trigger3 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _trigger4;

        [Category("GDTP")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger4
        {
            get => _trigger4;
            set
            {
                _trigger4 = value;
                SignalPropertyChange();
            }
        }

        public GDTPEntryNode()
        {
            _trigger1 = new TriggerDataClass(this);
            _trigger2 = new TriggerDataClass(this);
            _trigger3 = new TriggerDataClass(this);
            _trigger4 = new TriggerDataClass(this);
        }
        
        public override bool OnInitialize()
        {
            GDTPEntry data = *(GDTPEntry*) WorkingUncompressed.Address;
            _trigger1 = new TriggerDataClass(this, data._trigger1);
            _trigger2 = new TriggerDataClass(this, data._trigger2);
            _trigger3 = new TriggerDataClass(this, data._trigger3);
            _trigger4 = new TriggerDataClass(this, data._trigger4);

            if (_name == null)
            {
                _name = $"Three-Pin Door [{data._doorHeader._doorIndex}]";
            }

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
            gdtp._trigger1 = _trigger1;
            gdtp._trigger2 = _trigger2;
            gdtp._trigger3 = _trigger3;
            gdtp._trigger4 = _trigger4;
            *header = gdtp;
        }

        public override int OnCalculateSize(bool force)
        {
            return GDTPEntry.Size;
        }
    }
}
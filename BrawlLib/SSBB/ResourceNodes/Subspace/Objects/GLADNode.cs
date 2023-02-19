using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Objects
{
    public class GLADNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GLADEntryNode);
        protected override string baseName => "Ladders";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GLAD" ? new GLADNode() : null;
        }
    }

    public unsafe class GLADEntryNode : ResourceNode
    {
        internal GLADEntry Data;
        
        public MotionPathDataClass _motionPathData;
        [Category("Ladder")]
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
        
        [Category("Unknown")]
        public uint Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Ladder")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 AreaOffset
        {
            get => new Vector2(Data._areaOffsetPosX, Data._areaOffsetPosY);
            set
            {
                Data._areaOffsetPosX = value.X;
                Data._areaOffsetPosY = value.Y;
                SignalPropertyChange();
            }
        }
        
        [Category("Ladder")]
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
        
        [Category("Ladder")]
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
        public byte Unknown0x31
        {
            get => Data._unknown0x31;
            set
            {
                Data._unknown0x31 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Ladder")]
        public bool RestrictUpExit
        {
            get => Data._restrictUpExit;
            set
            {
                Data._restrictUpExit = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public byte Unknown0x33
        {
            get => Data._unknown0x33;
            set
            {
                Data._unknown0x33 = value;
                SignalPropertyChange();
            }
        }
        
        public string BoneName
        {
            get => Data.BoneName;
            set
            {
                Data.BoneName = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _isValidTrigger;
        [Category("Ladder")]
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

        public TriggerDataClass _motionPathTrigger;
        [Category("Ladder")]
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

        public GLADEntryNode()
        {
            _motionPathData = new MotionPathDataClass(this);
            _isValidTrigger = new TriggerDataClass(this);
            _motionPathTrigger = new TriggerDataClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *(GLADEntry*)WorkingUncompressed.Address;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _isValidTrigger = new TriggerDataClass(this, Data._isValidTrigger);
            _motionPathTrigger = new TriggerDataClass(this, Data._motionPathTrigger);

            if (_name == null)
            {
                _name = $"Ladder [{Index}]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GLADEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GLADEntry* hdr = (GLADEntry*)address;
            Data._motionPathData = _motionPathData;
            Data._isValidTrigger = _isValidTrigger;
            Data._motionPathTrigger = _motionPathTrigger;
            *hdr = Data;
        }
    }
}

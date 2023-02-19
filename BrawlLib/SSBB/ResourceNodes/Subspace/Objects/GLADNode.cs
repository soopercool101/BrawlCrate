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
        public float Unknown0x64
        {
            get => Data._unknown0x64;
            set
            {
                Data._unknown0x64 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x68
        {
            get => Data._unknown0x68;
            set
            {
                Data._unknown0x68 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x6C
        {
            get => Data._unknown0x6C;
            set
            {
                Data._unknown0x6C = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x70
        {
            get => Data._unknown0x70;
            set
            {
                Data._unknown0x70 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x74
        {
            get => Data._unknown0x74;
            set
            {
                Data._unknown0x74 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x78
        {
            get => Data._unknown0x78;
            set
            {
                Data._unknown0x78 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x7C
        {
            get => Data._unknown0x7C;
            set
            {
                Data._unknown0x7C = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x80
        {
            get => Data._unknown0x80;
            set
            {
                Data._unknown0x80 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x84
        {
            get => Data._unknown0x84;
            set
            {
                Data._unknown0x84 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x88
        {
            get => Data._unknown0x88;
            set
            {
                Data._unknown0x88 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x8C
        {
            get => Data._unknown0x8C;
            set
            {
                Data._unknown0x8C = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x90
        {
            get => Data._unknown0x90;
            set
            {
                Data._unknown0x90 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x94
        {
            get => Data._unknown0x94;
            set
            {
                Data._unknown0x94 = value;
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

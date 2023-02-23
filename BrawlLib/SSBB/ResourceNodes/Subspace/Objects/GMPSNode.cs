using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GMPSNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GMPSEntryNode);
        public override ResourceType ResourceFileType => ResourceType.GMPS;
        protected override string baseName => "Trackballs";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMPS" ? new GMPSNode() : null;
        }
    }

    public class GPS2Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GMPSEntryNode);
        protected override string baseName => "Moving Trackballs";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GPS2" ? new GPS2Node() : null;
        }
    }

    public unsafe class GMPSEntryNode : ResourceNode
    {
        internal GMPSEntry Data;

        public override ResourceType ResourceFileType => ResourceType.Unknown;
        
        public MotionPathDataClass _motionPathData;
        [Category("GMPS")]
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
        
        public MotionPathDataClass _sliderPathData;
        [Category("GMPS")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MotionPathDataClass SliderPathData
        {
            get => _sliderPathData;
            set
            {
                _sliderPathData = value;
                SignalPropertyChange();
            }
        }
        
        public HitDataClass _hitData;
        [Category("GMPS")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public HitDataClass HitData
        {
            get => _hitData;
            set
            {
                _hitData = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public uint Unknown0x030
        {
            get => Data._unknown0x030;
            set
            {
                Data._unknown0x030 = value;
                SignalPropertyChange();
            }
        }
        
        public AttackDataClass _attackData;
        [Category("GMPS")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
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
        public uint Unknown0x08C
        {
            get => Data._unknown0x08C;
            set
            {
                Data._unknown0x08C = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public uint Unknown0x090
        {
            get => Data._unknown0x090;
            set
            {
                Data._unknown0x090 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x094
        {
            get => Data._unknown0x094;
            set
            {
                Data._unknown0x094 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x098
        {
            get => Data._unknown0x098;
            set
            {
                Data._unknown0x098 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x09C
        {
            get => Data._unknown0x09C;
            set
            {
                Data._unknown0x09C = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x0A0
        {
            get => Data._unknown0x0A0;
            set
            {
                Data._unknown0x0A0 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x0A4
        {
            get => Data._unknown0x0A4;
            set
            {
                Data._unknown0x0A4 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public float Unknown0x0A8
        {
            get => Data._unknown0x0A8;
            set
            {
                Data._unknown0x0A8 = value;
                SignalPropertyChange();
            }
        }

        public DifficultyRatiosClass _difficultyMotionRatios;
        [Category("GMPS")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DifficultyRatiosClass DifficultyMotionRatios
        {
            get => _difficultyMotionRatios;
            set
            {
                _difficultyMotionRatios = value;
                SignalPropertyChange();
            }
        }
        
        public TriggerDataClass _triggerData;
        [Category("GMPS")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TriggerDataClass TriggerData
        {
            get => _triggerData;
            set
            {
                _triggerData = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public uint Unknown0x0EC
        {
            get => Data._unknown0x0EC;
            set
            {
                Data._unknown0x0EC = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public uint Unknown0x0F0
        {
            get => Data._unknown0x0F0;
            set
            {
                Data._unknown0x0F0 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public uint Unknown0x0F4
        {
            get => Data._unknown0x0F4;
            set
            {
                Data._unknown0x0F4 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("GMPS")]
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
        public byte Unknown0x0F9
        {
            get => Data._unknown0x0F9;
            set
            {
                Data._unknown0x0F9 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public byte Unknown0x0FA
        {
            get => Data._unknown0x0FA;
            set
            {
                Data._unknown0x0FA = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public byte Unknown0x0FB
        {
            get => Data._unknown0x0FB;
            set
            {
                Data._unknown0x0FB = value;
                SignalPropertyChange();
            }
        }
        
        [Category("GMPS")]
        public string BoneName
        {
            get => Data.BoneName;
            set
            {
                Data.BoneName = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Unknown")]
        public uint Unknown0x11C
        {
            get => Data._unknown0x11C;
            set
            {
                Data._unknown0x11C = value;
                SignalPropertyChange();
            }
        }

        public GMPSEntryNode()
        {
            _motionPathData = new MotionPathDataClass(this);
            _sliderPathData = new MotionPathDataClass(this);
            _hitData = new HitDataClass(this);
            _attackData = new AttackDataClass(this);
            _triggerData = new TriggerDataClass(this);
            _difficultyMotionRatios = new DifficultyRatiosClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *(GMPSEntry*)WorkingUncompressed.Address;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _sliderPathData = new MotionPathDataClass(this, Data._sliderPathData);
            _hitData = new HitDataClass(this, Data._hitData);
            _attackData = new AttackDataClass(this, Data._attackData);
            _triggerData = new TriggerDataClass(this, Data._triggerData);
            _difficultyMotionRatios = new DifficultyRatiosClass(this, Data._difficultyMotionRatios);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GMPSEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GMPSEntry* hdr = (GMPSEntry*)address;
            Data._motionPathData = _motionPathData;
            Data._sliderPathData = _sliderPathData;
            Data._hitData = _hitData;
            Data._attackData = _attackData;
            Data._triggerData = _triggerData;
            Data._difficultyMotionRatios = _difficultyMotionRatios;
            *hdr = Data;
        }
    }
}
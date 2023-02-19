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
        }

        public override bool OnInitialize()
        {
            Data = *(GMPSEntry*)WorkingUncompressed.Address;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _sliderPathData = new MotionPathDataClass(this, Data._sliderPathData);
            _hitData = new HitDataClass(this, Data._hitData);
            _attackData = new AttackDataClass(this, Data._attackData);
            _triggerData = new TriggerDataClass(this, Data._triggerData);

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
            *hdr = Data;
        }
    }
}
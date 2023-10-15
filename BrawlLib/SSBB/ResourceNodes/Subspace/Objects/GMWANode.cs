using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Objects;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Objects
{
    public class GMWANode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GMWAEntryNode);
        protected override string baseName => "Switches";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GMWA" ? new GMWANode() : null;
        }
    }

    public unsafe class GMWAEntryNode : ResourceNode
    {
        internal GMWAEntry Data;

        [Category("Switch")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Location
        {
            get => new Vector2(Data._locationX, Data._locationY);
            set
            {
                Data._locationX = value._x;
                Data._locationY = value._y;
                SignalPropertyChange();
            }
        }

        [Category("Switch")]
        [Description("Rotation on the Z-axis")]
        public float Rotation
        {
            get => Data._rotationZ;
            set
            {
                Data._rotationZ = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Switch")]
        [TypeConverter(typeof(NullableByteConverter))]
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
        public byte Unknown0x12
        {
            get => Data._unknown0x12;
            set
            {
                Data._unknown0x12 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x13
        {
            get => Data._unknown0x13;
            set
            {
                Data._unknown0x13 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        public MotionPathDataClass _motionPathData;
        [Category("Switch")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public MotionPathDataClass MotionPathData
        {
            get => _motionPathData ?? new MotionPathDataClass(this);
            set
            {
                _motionPathData = value;
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

        private TriggerDataClass _trigger0x28;
        [Category("Switch")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger0x28
        {
            get => _trigger0x28 ?? new TriggerDataClass(this);
            set
            {
                Data._trigger0x28 = _trigger0x28 = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger0x2C;
        [Category("Switch")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger0x2C
        {
            get => _trigger0x2C ?? new TriggerDataClass(this);
            set
            {
                Data._trigger0x2C = _trigger0x2C = value;
                SignalPropertyChange();
            }
        }

        private TriggerDataClass _trigger0x30;
        [Category("Switch")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass Trigger0x30
        {
            get => _trigger0x30 ?? new TriggerDataClass(this);
            set
            {
                Data._trigger0x30 = _trigger0x30 = value;
                SignalPropertyChange();
            }
        }

        private DifficultyRatiosClass _difficultyRatios;
        [Category("Switch")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public DifficultyRatiosClass DifficultyMotionRatios
        {
            get => _difficultyRatios ?? new DifficultyRatiosClass(this);
            set
            {
                Data._difficultyRatios = _difficultyRatios = value;
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

        [Category("Switch")]
        public float HurtboxSize
        {
            get => Data._hurtboxSize;
            set
            {
                Data._hurtboxSize = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x8C
        {
            get => Data._unknown0x8C;
            set
            {
                Data._unknown0x8C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x8D
        {
            get => Data._unknown0x8D;
            set
            {
                Data._unknown0x8D = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x8E
        {
            get => Data._unknown0x8E;
            set
            {
                Data._unknown0x8E = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x8F
        {
            get => Data._unknown0x8F;
            set
            {
                Data._unknown0x8F = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x90
        {
            get => Data._unknown0x90;
            set
            {
                Data._unknown0x90 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x91
        {
            get => Data._unknown0x91;
            set
            {
                Data._unknown0x91 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x92
        {
            get => Data._unknown0x92;
            set
            {
                Data._unknown0x92 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x93
        {
            get => Data._unknown0x93;
            set
            {
                Data._unknown0x93 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x94
        {
            get => Data._unknown0x94;
            set
            {
                Data._unknown0x94 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x95
        {
            get => Data._unknown0x95;
            set
            {
                Data._unknown0x95 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x96
        {
            get => Data._unknown0x96;
            set
            {
                Data._unknown0x96 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x97
        {
            get => Data._unknown0x97;
            set
            {
                Data._unknown0x97 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *(GMWAEntry*)WorkingUncompressed.Address;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _trigger0x28 = new TriggerDataClass(this, Data._trigger0x28);
            _trigger0x2C = new TriggerDataClass(this, Data._trigger0x2C);
            _trigger0x30 = new TriggerDataClass(this, Data._trigger0x30);
            _difficultyRatios = new DifficultyRatiosClass(this, Data._difficultyRatios);

            if (_name == null)
            {
                _name = $"Switch [{Index}]";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GMWAEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GMWAEntry* hdr = (GMWAEntry*)address;
            Data._motionPathData = _motionPathData;
            Data._trigger0x28 = _trigger0x28;
            Data._trigger0x2C = _trigger0x2C;
            Data._trigger0x30 = _trigger0x30;
            Data._difficultyRatios = _difficultyRatios;
            *hdr = Data;
        }
    }
}

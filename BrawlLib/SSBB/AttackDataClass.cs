using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB
{
    public class AttackDataClass
    {
        private ResourceNode _parent;
        private AttackData Data;

        [Category("Attack Data")]
        public float Damage
        {
            get => Data._damage;
            set
            {
                Data._damage = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 AreaOffsetPosition
        {
            get => new Vector3(Data._offsetPosX, Data._offsetPosY, Data._offsetPosZ);
            set
            {
                Data._offsetPosX = value._x;
                Data._offsetPosY = value._y;
                Data._offsetPosZ = value._z;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public float Size
        {
            get => Data._size;
            set
            {
                Data._size = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public int Vector
        {
            get => Data._vector;
            set
            {
                Data._vector = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public int ReactionEffect
        {
            get => Data._reactionEffect;
            set
            {
                Data._reactionEffect = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public int ReactionFix
        {
            get => Data._reactionFix;
            set
            {
                Data._reactionFix = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public int ReactionAdd
        {
            get => Data._reactionAdd;
            set
            {
                Data._reactionAdd = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public AttackElementType ElementType
        {
            get => (AttackElementType)(uint)Data._elementType;
            set
            {
                Data._elementType = (uint)value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public bool IsClankable
        {
            get => Data._isClankable == 1;
            set
            {
                Data._isClankable = value ? (byte)1 : (byte)0;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public uint DetectionRate
        {
            get => Data._detectionRate;
            set
            {
                Data._detectionRate = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public uint HitSoundLevel
        {
            get => Data._hitSoundLevel;
            set
            {
                Data._hitSoundLevel = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public HitSoundType HitSoundType
        {
            get => (HitSoundType)(uint)Data._hitSoundType;
            set
            {
                Data._hitSoundType = (uint)value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public bool IsShapeCapsule
        {
            get => Data._isShapeCapsule == 1;
            set
            {
                Data._isShapeCapsule = value ? (byte)1 : (byte)0;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public uint NodeIndex
        {
            get => Data._nodeIndex;
            set
            {
                Data._nodeIndex = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Attack Data")]
        public int Power
        {
            get => Data._power;
            set
            {
                Data._power = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2D
        {
            get => Data._unknown0x2D;
            set
            {
                Data._unknown0x2D = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2E
        {
            get => Data._unknown0x2E;
            set
            {
                Data._unknown0x2E = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x2F
        {
            get => Data._unknown0x2F;
            set
            {
                Data._unknown0x2F = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x48
        {
            get => Data._unknown0x48;
            set
            {
                Data._unknown0x48 = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x4A
        {
            get => Data._unknown0x4A;
            set
            {
                Data._unknown0x4A = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x4B
        {
            get => Data._unknown0x4B;
            set
            {
                Data._unknown0x4B = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x4C
        {
            get => Data._unknown0x4C;
            set
            {
                Data._unknown0x4C = value;
                _parent.SignalPropertyChange();
            }
        }

        public AttackDataClass(ResourceNode parent)
        {
            _parent = parent;
            Data = new AttackData();
        }

        public AttackDataClass(ResourceNode parent, AttackData data)
        {
            _parent = parent;
            Data = data;
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public static implicit operator AttackData(AttackDataClass val)
        {
            return val.Data;
        }
    }
}

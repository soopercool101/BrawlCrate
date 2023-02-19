using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB
{
    public class HitDataClass
    {
        private ResourceNode _parent;
        private HitData Data;

        [Category("Hit Data")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 StartOffset
        {
            get => new Vector3(Data._startOffsetX, Data._startOffsetY, Data._startOffsetZ);
            set
            {
                Data._startOffsetX = value._x;
                Data._startOffsetY = value._y;
                Data._startOffsetZ = value._z;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Hit Data")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 EndOffset
        {
            get => new Vector3(Data._endOffsetX, Data._endOffsetY, Data._endOffsetZ);
            set
            {
                Data._endOffsetX = value._x;
                Data._endOffsetY = value._y;
                Data._endOffsetZ = value._z;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Hit Data")]
        public float Size
        {
            get => Data._size;
            set
            {
                Data._size = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Hit Data")]
        public byte NodeIndex
        {
            get => Data._nodeIndex;
            set
            {
                Data._nodeIndex = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1D
        {
            get => Data._unknown0x1D;
            set
            {
                Data._unknown0x1D = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1E
        {
            get => Data._unknown0x1E;
            set
            {
                Data._unknown0x1E = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1F
        {
            get => Data._unknown0x1F;
            set
            {
                Data._unknown0x1F = value;
                _parent.SignalPropertyChange();
            }
        }

        public HitDataClass(ResourceNode parent)
        {
            _parent = parent;
            Data = new HitData();
        }

        public HitDataClass(ResourceNode parent, HitData data)
        {
            _parent = parent;
            Data = data;
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public static implicit operator HitData(HitDataClass val)
        {
            return val.Data;
        }
    }
}

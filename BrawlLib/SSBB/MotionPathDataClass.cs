using BrawlLib.Internal;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;

namespace BrawlLib.SSBB
{
    public class MotionPathDataClass
    {
        private ResourceNode _parent;
        private MotionPathData Data;

        [Category("Motion Path Data")]
        public float MotionRatio
        {
            get => Data._motionRatio;
            set
            {
                Data._motionRatio = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Motion Path Data")]
        public byte Index
        {
            get => Data._index;
            set
            {
                Data._index = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Motion Path Data")]
        public MotionPathMode PathMode
        {
            get => Data._pathMode;
            set
            {
                Data._pathMode = value;
                _parent.SignalPropertyChange();
            } 
        }

        [Category("Motion Path Data")]
        [TypeConverter(typeof(NullableByteConverter))]
        public byte ModelDataIndex
        {
            get => Data._modelIndex;
            set
            {
                Data._modelIndex = value;
                _parent.SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x7
        {
            get => Data._unknown0x7;
            set
            {
                Data._unknown0x7 = value;
                _parent.SignalPropertyChange();
            }
        }

        public MotionPathDataClass(ResourceNode parent)
        {
            _parent = parent;
            Data = new MotionPathData();
        }

        public MotionPathDataClass(ResourceNode parent, MotionPathData data)
        {
            _parent = parent;
            Data = data;
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public static implicit operator MotionPathData(MotionPathDataClass val)
        {
            return val.Data;
        }
    }
}

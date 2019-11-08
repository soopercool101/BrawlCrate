using System.Runtime.InteropServices;

namespace BrawlLib.Internal
{
    public struct Box
    {
        public static readonly Box ExpandableVolume = new Box(new Vector3(float.MaxValue), new Vector3(float.MinValue));
        public static readonly Box ZeroBox = new Box();
        private Vector3 _min, _max;

        public Vector3 Min
        {
            get => _min;
            set => _min = value;
        }

        public Vector3 Max
        {
            get => _max;
            set => _max = value;
        }

        public Box(Vector3 min, Vector3 max)
        {
            _min = min;
            _max = max;
        }

        public void ExpandVolume(Vector3 value)
        {
            _min.Min(value);
            _max.Max(value);
        }

        public void ExpandVolume(Box value)
        {
            ExpandVolume(value.Min);
            ExpandVolume(value.Max);
        }

        public static Box GetVolume(Vector3[] points)
        {
            if (points == null || points.Length == 0)
            {
                return new Box();
            }

            Box box = ExpandableVolume;
            foreach (Vector3 point in points)
            {
                box.ExpandVolume(point);
            }

            return box;
        }

        public bool IsValid => _min < _max;

        public static implicit operator Box(BBox val)
        {
            return new Box(val.Min, val.Max);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BBox
    {
        private BVec3 _min, _max;

        public Vector3 Min
        {
            get => _min;
            set => _min = value;
        }

        public Vector3 Max
        {
            get => _max;
            set => _max = value;
        }

        public BBox(Vector3 min, Vector3 max)
        {
            _min = min;
            _max = max;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public static implicit operator BBox(Box val)
        {
            return new BBox(val.Min, val.Max);
        }
    }
}
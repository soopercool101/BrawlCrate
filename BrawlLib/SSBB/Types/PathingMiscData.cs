using BrawlLib.Internal;
using System.Runtime.InteropServices;
using System.Text;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PathingMiscData
    {
        public static readonly uint HeaderSize = 0x20;

        public buint _count;
        public buint _headerSize; // 0x20
        public bfloat _minX;
        public bfloat _minY;
        public bfloat _minZ;
        public bfloat _maxX;
        public bfloat _maxY;
        public bfloat _maxZ;

        public VoidPtr this[int index] => (byte*) Address + _headerSize + index * PathingMiscDataEntry.Size;

        public VoidPtr GetDataBlock(int index)
        {
            if (index > _count)
            {
                return null;
            }

            return Address + ((PathingMiscDataEntry*) this[index])->_dataOffset;
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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PathingMiscDataEntry
    {
        public static readonly uint Size = 0x40;
        public bushort _count;
        public bushort _id;
        public buint _dataOffset;
        public bfloat _minX;
        public bfloat _minY;
        public bfloat _minZ;
        public bfloat _maxX;
        public bfloat _maxY;
        public bfloat _maxZ;
        private fixed byte _name[0x20];

        public string Name
        {
            get
            {
                fixed (byte* name = _name)
                {
                    return Encoding.UTF8.GetString(name, 0x20);
                }
            }
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
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PathingMiscDataSubEntry
    {
        public static readonly uint Size = 0x0C;
        public bfloat _x;
        public bfloat _y;
        public bfloat _z;
    }
}
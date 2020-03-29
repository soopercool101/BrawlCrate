using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCLA
    {
        public const string Tag = "SCLA";
        public const int Size = 0x10;

        public BinTag _tag;
        public bint _count;
        public int pad0;
        public int pad1;

        public SCLA(int count)
        {
            _tag = Tag;
            _count = count;
            pad0 = pad1 = 0;
        }

        public VoidPtr this[int index] => (byte*) Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *((buint*) Address + 4 + index);
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
    public unsafe struct SCLAEntry
    {
        public buint _index;
        public bfloat _unk1;
        public buint _unk2;
        public SCLASubEntry _entry1;
        public SCLASubEntry _entry2;
        public SCLASubEntry _entry3;

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
    public unsafe struct SCLASubEntry
    {
        public byte _unk1;
        public byte _unk2;
        public bushort _unk3;
        public buint _unk4;
        public bint _index1;
        public bint _index2;
        public bint _index3;
        public bint _index4;

        public SCLASubEntry(ResourceNodes.SCLAEntryNode.SCLASubEntryClass e)
        {
            _unk1 = e._unk1;
            _unk2 = e._unk2;
            _unk3 = e._unk3;
            _unk4 = e._unk4;
            _index1 = e._index1;
            _index2 = e._index2;
            _index3 = e._index3;
            _index4 = e._index4;
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
}
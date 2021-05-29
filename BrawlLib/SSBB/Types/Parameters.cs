using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Parameter
    {
        public const string TagSTPM = "STPM";
        public const string TagADPM = "ADPM";
        public const string TagMVPM = "MVPM";
        public const int Size = 0x10;

        public BinTag _tag;
        public bint _count;
        public int pad0;
        public int pad1;

        public Parameter(string tag, int count)
        {
            _tag = tag;
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
    public unsafe struct StageParameterEntry
    {
        public bushort _id;
        public byte _id2;
        public byte _echo;

        public fixed int _values[64];

        public StageParameterEntry(ushort id, byte echo, byte id2)
        {
            _id = id;
            _echo = echo;
            _id2 = id2;
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
    public unsafe struct MovParameterEntry
    {
        public fixed int _values[22];

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
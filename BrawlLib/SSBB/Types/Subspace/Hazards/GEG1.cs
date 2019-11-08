using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Hazards
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GEG1
    {
        public const uint Tag = 0x31474547;
        public const int Size = 132;
        public uint _tag;
        public bint _count;
        public bint _DataOffset;

        public GEG1(int count)
        {
            _tag = Tag;
            _count = count;
            _DataOffset = count * 4;
        }

        //private GDOR* Address { get { fixed (GDOR* ptr = &this)return ptr; } }
        //public byte* Data { get { return (byte*)(Address + _DataOffset); } }

        public VoidPtr this[int index] => (byte*) Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *(buint*) ((byte*) Address + 0x08 + index * 4);
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
    public unsafe struct GEG1Entry
    {
        // I believe these are constant values for the Header
        public const uint Header1 = 0x0000803F; // 0x00

        public const uint Header2 = 0x00FF0100; // 0x04

        // Unknown values. I just assumed byte for all of them for now// Headers are known
        public uint _header1; // 0x00
        public uint _header2; // 0x04
        public byte _extrahealth;
        public byte _flag0x09;
        public byte _flag0x0A;
        public byte _flag0x0B;
        public byte _flag0x0C;
        public byte _connectedenemyid;
        public byte _flag0x0E;
        public byte _flag0x0F;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public byte _unknown0x14;
        public byte _unknown0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public byte _unknown0x18;
        public byte _unknown0x19;
        public byte _unknown0x1A;
        public byte _unknown0x1B;

        public byte _unknown0x1C;

        // EnemyID is known
        public byte _enemyID; // 0x1D
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public byte _unknown0x20;
        public byte _unknown0x21;
        public byte _unknown0x22;
        public byte _startingaction;
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;

        public byte _unknown0x27;

        // Spawn Position is known
        public bfloat _spawnX; // 0x28
        public bfloat _spawnY; // 0x2C
        public byte _unknown0x30;
        public byte _unknown0x31;
        public byte _unknown0x32;
        public byte _unknown0x33;
        public byte _unknown0x34;
        public byte _unknown0x35;
        public byte _unknown0x36;
        public byte _unknown0x37;
        public byte _unknown0x38;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unknown0x3B;
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public byte _unknown0x44;
        public byte _unknown0x45;
        public byte _unknown0x46;
        public byte _unknown0x47;
        public byte _unknown0x48;
        public byte _unknown0x49;
        public byte _unknown0x4A;
        public byte _unknown0x4B;
        public byte _unknown0x4C;
        public byte _unknown0x4D;
        public byte _unknown0x4E;
        public byte _unknown0x4F;
        public byte _unknown0x50;
        public byte _unknown0x51;
        public byte _unknown0x52;
        public byte _unknown0x53;
        public byte _unknown0x54;
        public byte _unknown0x55;
        public byte _unknown0x56;
        public byte _unknown0x57;
        public byte _unknown0x58;
        public byte _unknown0x59;
        public byte _unknown0x5A;
        public byte _unknown0x5B;
        public byte _unknown0x5C;
        public byte _unknown0x5D;
        public byte _unknown0x5E;
        public byte _unknown0x5F;
        public byte _unknown0x60;
        public byte _unknown0x61;
        public byte _unknown0x62;
        public byte _unknown0x63;
        public byte _flag0x64;
        public byte _unknown0x65;
        public byte _unknown0x66;
        public byte _flag0x67;
        public byte _unknown0x68;
        public byte _unknown0x69;
        public byte _unknown0x6A;
        public byte _unknown0x6B;
        public byte _unknown0x6C;
        public byte _unknown0x6D;
        public byte _unknown0x6E;
        public byte _unknown0x6F;
        public byte _unknown0x70;
        public byte _unknown0x71;
        public byte _unknown0x72;
        public byte _unknown0x73;
        public byte _unknown0x74;
        public byte _unknown0x75;
        public byte _unknown0x76;
        public byte _unknown0x77;
        public byte _unknown0x78;
        public byte _unknown0x79;
        public byte _unknown0x7A;
        public byte _unknown0x7B;
        public byte _unknown0x7C;
        public byte _spawnid;
        public byte _flag0x7E;
        public byte _flag0x7F;
        public byte _flag0x80;
        public byte _flag0x81;
        public byte _flag0x82;
        public byte _flag0x83;

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
using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ARCHeader
    {
        public const int Size = 0x40;
        public const uint Tag = 0x00435241;

        public uint _tag;       //ARC
        public ushort _version; //0x0101
        public bushort _numFiles;
        private readonly uint _unk1;
        private readonly uint _unk2;
        private fixed sbyte _name[48];

        internal VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public string Name
        {
            get => new string((sbyte*) Address + 0x10);
            set
            {
                if (value == null)
                {
                    value = "";
                }

                fixed (sbyte* ptr = _name)
                {
                    int i = 0;
                    while (i < 47 && i < value.Length)
                    {
                        ptr[i] = (sbyte) value[i++];
                    }

                    while (i < 48)
                    {
                        ptr[i++] = 0;
                    }
                }
            }
        }

        public ARCFileHeader* First => (ARCFileHeader*) (Address + Size);

        public ARCHeader(ushort numFiles, string name)
        {
            _tag = Tag;
            _version = 0x0101;
            _numFiles = numFiles;
            _unk1 = 0;
            _unk2 = 0;
            Name = name;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ARCFileHeader
    {
        public const int Size = 0x20;

        internal bshort _type;
        internal bshort _index;
        internal bint _size;
        internal byte _groupIndex;
        internal byte _padding;
        internal bshort _redirectIndex; //The index of a different file to read.
        internal bint _pad1;
        internal bint _pad2;
        internal bint _pad3;
        internal bint _pad4;
        internal bint _pad5;

        public ARCFileHeader(ARCFileType type, int index, int size, byte groupIndex, short id)
        {
            _type = (short) type;
            _index = (short) index;
            _size = size;
            _groupIndex = groupIndex;
            _padding = 0;
            _redirectIndex = id;
            _pad1 = _pad2 = _pad3 = _pad4 = _pad5 = 0;
        }

        private ARCFileHeader* Address
        {
            get
            {
                fixed (ARCFileHeader* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public VoidPtr Data => (VoidPtr) Address + Size;
        public ARCFileHeader* Next => (ARCFileHeader*) ((uint) (Data + _size)).Align(Size);

        public ARCFileType FileType
        {
            get => (ARCFileType) (short) _type;
            set => _type = (short) value;
        }

        public short Index
        {
            get => _index;
            set => _index = value;
        }

        public int Length
        {
            get => _size;
            set => _size = value;
        }

        public byte GroupIndex
        {
            get => _groupIndex;
            set => _groupIndex = value;
        }

        //public byte Unknown { get { return _padding; } set { _padding = value; } }
        public short ID
        {
            get => _redirectIndex;
            set => _redirectIndex = value;
        }
    }

    public enum ARCFileType : short
    {
        None = 0x0,
        MiscData = 0x1,
        ModelData = 0x2,
        TextureData = 0x3,
        AnimationData = 0x4,
        SceneData = 0x5,
        Type6 = 0x6,
        GroupedArchive = 0x7,
        EffectData = 0x8
    }
}
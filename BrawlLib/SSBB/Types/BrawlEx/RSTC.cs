using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.BrawlEx
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RSTC
    {
        public const uint Tag = 0x43545352;
        public const int Size = 0xE0;
        public uint _tag;         // 0x00 - Uneditable; RSTC
        public uint _size;        // 0x04 - Uneditable; Should be "E0"
        public uint _version;     // 0x08 - Version; Only parses "1" currently
        public byte _unknown0x0C; // 0x0C - Unused?

        public byte
            _charNum; // 0x0D - Number of characters in the char list; should be generated automatically. Max is 100? (Maybe 104, or may have padding).

        public byte _unknown0x0E; // 0x0E - Unused?

        public byte
            _randNum; // 0x0F - Number of characters in the random list; should be generated automatically. Max is 100? (Maybe 104, or may have padding).

        public fixed byte _charList[104]; // 0x20 - 104 bytes
        public fixed byte _randList[104]; // 0x20 - 104 bytes

        public VoidPtr this[int index] => (byte*) Address + 0x10 + index;

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
    public unsafe struct RSTCEntry
    {
        public const int Size = 0x01;
        public byte _fighterID;
    }
}
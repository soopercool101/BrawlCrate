using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.BrawlEx
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CSSC
    {
        public const uint Tag = 0x43535343;
        public const int Size = 0x40;
        public uint _tag;       // 0x00 - Uneditable; CSSC
        public buint _size;     // 0x04 - Uneditable; Should be "40"
        public buint _version;  // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1; // 0x0C - Unused?
        public byte _editFlag2; // 0x0D - Unused?
        public byte _editFlag3; // 0X0E - Unused?

        public byte _editFlag4; // 0x0F - 0x01 = Set Primary Character/Secondary Character;

        //        0x02 = Set Cosmetic Slot; 0x03 = Set both
        //        Use Booleans for both?
        public byte _primaryCharSlot;   // 0x10 - Primary Character Slot: Only used when set in _editFlag4
        public byte _secondaryCharSlot; // 0x11 - Secondary Character Slot: Only used when set in _editFlag4
        public byte _recordSlot;        // 0x12 - Record Bank
        public byte _cosmeticSlot;      // 0x13 - Only used when set in _editFlag4
        public buint _wiimoteSFX;       // 0x14
        public buint _unknown0x18;      // 0x18 - Seemingly padding

        public buint _status; // 0x1C - I have no idea what this is
        //public fixed byte _cosmetics[32];   // 0x20 - 32 bytes

        public VoidPtr this[int index] => (byte*) Address + 0x20 + index * 2;

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
    public unsafe struct CSSCEntry
    {
        public const int Size = 0x02;

        // 2 bytes repeatedly
        public byte _colorID;
        public byte _costumeID;

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
using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.BrawlEx
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct COSC
    {
        public const uint Tag = 0x43534F43;
        public const int Size = 0x40;
        public uint _tag;       // 0x00 - Uneditable; COSC
        public uint _size;      // 0x04 - Uneditable; Should be "40"
        public uint _version;   // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1; // 0x0C - Unused?
        public byte _editFlag2; // 0x0D - Unused?
        public byte _editFlag3; // 0X0E - Unused?

        public byte
            _hasSecondary; // 0X0F - 00 is not transforming, 01 is transforming. Possibly could be set automatically based on if a secondary character slot is FF or not?

        public byte _cosmeticID;                 // 0x10 - Cosmetic slot; Parse as hex?
        public byte _unknown0x11;                // 0x11
        public byte _primaryCharSlot;            // 0x12 - Primary Character Slot (Parse as hex?)
        public byte _secondaryCharSlot;          // 0x13 - Secondary Character Slot (Parse as hex?)
        public byte _franchise;                  // 0x14 - Make this a list
        public byte _unknown0x15;                // 0x15
        public byte _unknown0x16;                // 0x16
        public byte _unknown0x17;                // 0x17
        public buint _announcerSFX;              // 0x18 - Announcer Call
        public buint _unknown0x1C;               // 0x1C
        public fixed byte _victoryNameArray[32]; // 0x20 - 32 characters

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
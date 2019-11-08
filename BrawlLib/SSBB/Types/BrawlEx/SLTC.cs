using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.BrawlEx
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SLTC
    {
        public const uint Tag = 0x43544C53;
        public const int Size = 0x40;
        public uint _tag;               // 0x00 - Uneditable; SLTC
        public uint _size;              // 0x04 - Uneditable; Should be "40"
        public uint _version;           // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1;         // 0x0C - Unused?
        public byte _editFlag2;         // 0x0D - Unused?
        public byte _editFlag3;         // 0X0E - Unused?
        public byte _setSlotCharacters; // 0X0F - 00 is not set, 01 is set
        public buint _slot1;            // 0x10
        public buint _slot2;            // 0x14
        public buint _slot3;            // 0x18
        public buint _slot4;            // 0x1C
        public buint _victoryTheme;     // 0x20 - Victory Theme
        public byte _recordSlot;        // 0x24 - Record Bank
        public byte _unknown0x25;       // 0x25 - Seems to always be 0xCC
        public byte _unknown0x26;       // 0x26 - Seems to always be 0xCC
        public byte _unknown0x27;       // 0x27 - Seems to always be 0xCC
        public buint _announcerSFX;     // 0x28 - Announcer voiceline for victory screen
        public buint _unknown0x2C;      // 0x2C - Appears to always be 0xCCCCCCCC
        public bfloat _victoryCamera1;  // 0x30 - Camera Distance
        public bfloat _victoryCamera2;  // 0x34 - Camera Distance
        public bfloat _victoryCamera3;  // 0x38 - Camera Distance
        public bfloat _victoryCamera4;  // 0x3C - Camera Distance

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
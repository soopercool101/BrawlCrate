using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.BrawlEx
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FCFG
    {
        public const uint Tag1 = 0x47464346; // FGFC
        public const uint Tag2 = 0x43544946; // FITC
        public const int Size = 0x100;
        public uint _tag;                  // 0x00 - Uneditable, FCFG or FITC both work here
        public buint _size;                // 0x04 - Uneditable; Should be "100"
        public buint _version;             // 0x08 - Version; Currently allows 1 or 2
        public byte _editFlag1;            // 0x0C - Unused?
        public byte _editFlag2;            // 0x0D - Unused?
        public byte _editFlag3;            // 0X0E - Unused?
        public byte _editFlag4;            // 0X0F - Unused?
        public byte _kirbyCopyColorFlags;  // 0x10 - 0 = No hat; 1 = Per Color hats; 2: One Kirby Hat
        public byte _entryColorFlags;      // 0x11
        public byte _resultColorFlags;     // 0x12
        public byte _characterLoadFlags;   // 0x13 - http://opensa.dantarion.com/wiki/Load_Flags
        public byte _finalSmashColorFlags; // 0x14
        public byte _unknown0x15;          // 0x15
        public bushort _colorFlags;        // 0x16
        public buint _entryArticleFlag;    // 0x18
        public buint _soundbank;           // 0x1C - Parse as list
        public buint _kirbySoundbank;      // 0x20
        public uint _unknown0x24;          // 0x24
        public uint _unknown0x28;          // 0x28
        public uint _unknown0x2C;          // 0x2C

        // Name stuff. Name should be no more than 16 characters (technical max for filenames is 20 but internal name lowers the limit)
        public fixed byte
            _pacNameArray[48]; // 0x30 - 48 characters; Auto generate from name: (name-lower)/Fit(name).pac

        public fixed byte
            _kirbyPacNameArray[48]; // 0x60 - 48 characters; Auto generate from name: kirby/FitKirby(name).pac

        public fixed byte _moduleNameArray[32];   // 0x90 - 32 characters; Auto generate from name: ft_(name-lower).rel
        public fixed byte _internalNameArray[16]; // 0xB0 - 16 characters; Auto generate from name: (name-upper)

        // IC Constants
        public buint _jabFlag;      // 0xC0 - Usage unknown
        public buint _jabCount;     // 0xC4 - Number of jabs in combo
        public buint _hasRapidJab;  // 0xC8 - Whether the fighter has a rapid jab
        public buint _canTilt;      // 0xCC - Whether the fighter can angle their forward tilt attack
        public buint _fSmashCount;  // 0xD0 - Number of attacks in Fsmash chain
        public buint _airJumpCount; // 0xD4 - Number of mid-air jumps the fighter has
        public buint _canGlide;     // 0xD8 - Whether the fighter can glide
        public buint _canCrawl;     // 0xDC - Whether the fighter can crawl

        public buint
            _dashAttackIntoCrouch; // 0xE0 - Whether the fighter's dash attack puts them in a crouching position

        public buint _canWallJump; // 0xE4 - Whether the fighter can jump off walls
        public buint _canCling;    // 0xE8 - Whether the fighter can cling to walls
        public buint _canZAir;     // 0xEC - Whether the fighter can use an aerial tether
        public buint _thrownType;  // 0xF0 - Animation used when thrown
        public buint _grabSize;    // 0xF4 - Animation used when grabbed

        public buint _textureLoad;  // 0xF8 - 0/1/2/3/4/5
        public buint _aiController; // 0xFC

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
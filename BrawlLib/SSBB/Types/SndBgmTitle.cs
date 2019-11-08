using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SndBgmTitleEntry
    {
        public bint _ID;
        public bint _unknown04;
        public bint _unknown08;
        public bint _unknown0c;
        public bint _SongTitleIndex;
        public bint _unknown14;
        public bint _unknown18;
        public bint _unknown1c;
        public bint _unknown20;
        public bint _unknown24;
        public bint _unknown28;
        public bint _unknown2c;

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
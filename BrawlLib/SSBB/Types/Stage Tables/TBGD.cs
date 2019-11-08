using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TBGD // TBGD
    {
        public const uint Tag = 0x44474254;

        public uint _tag;
        public bint _unk0;
        public bint _unk1;
        public bint _unk2;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public bfloat* Entries => (bfloat*) (Address + 0x10);
    }
}
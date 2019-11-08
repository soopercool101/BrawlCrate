using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TBCL // TBCL
    {
        public const uint Tag = 0x4C434254;

        public uint _tag;
        public bint _version;
        public bint _unk1;
        public bint _unk2;
        public bint _entryOffset;

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

        public bfloat* Entries => (bfloat*) (Address + _entryOffset);
    }
}
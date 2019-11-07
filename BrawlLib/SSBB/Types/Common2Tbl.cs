using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Common2TblHeader
    {
        public const int Size = 0x20;

        public bint _Length;
        public bint _DataLength;
        public bint _OffCount;
        public bint _DataTable;
        public int _pad0;
        public int _pad1;
        public int _pad2;
        public int _pad3;


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

        public string Str => new string((sbyte*) Address + _DataLength + _OffCount * 4 + 0x20 + _DataTable * 8);
    }
}
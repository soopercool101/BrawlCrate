using BrawlLib.Internal;
using System;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Windows.Markup;

namespace BrawlLib.SSBB.Types.ProjectPlus
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ASLS
    {
        public static readonly uint HeaderSize = 0x08;
        public static readonly uint Tag = 0x41534C53;
        public buint _tag;
        public bshort _count;
        public bshort _nameOffset;

        public VoidPtr this[int index] => (byte*) Address + HeaderSize + (index * 0x04);

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
    public unsafe struct ASLSEntry
    {
        public static readonly uint Size = 0x04;
        public bushort _buttonFlags;
        public bushort _nameOffset;
    }
}

using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.SSEEX
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SELB
    {
        public const int Size = 0x04;

        public byte _characterCount;
        public sbyte _stockCount;
        public byte _sublevelChanger;
        public byte _pad;

        public VoidPtr this[int index] => (byte*)Address + Size + index * SELBEntry.Size;

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
    public struct SELBEntry
    {
        public const int Size = 0x14;

        public byte _cssID;
        public BInt24 _pad;
        public bfloat _cursorX;
        public bfloat _cursorY;
        public bfloat _nameX;
        public bfloat _nameY;
    }
}
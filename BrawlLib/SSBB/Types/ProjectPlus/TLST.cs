using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.ProjectPlus
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TLST
    {
        public static readonly uint HeaderSize = 0x0C;
        public static readonly uint Tag = 0x544C5354;
        public buint _tag;
        public buint _count;
        public bushort _fileSize;
        public bushort _nameOffset;

        public VoidPtr this[int index] => (byte*)Address + HeaderSize + index * TLSTEntry.Size;

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
    public struct TLSTEntry
    {
        public static readonly uint Size = 0x10;
        public buint _songID;
        public bshort _songDelay;
        public byte _songVolume;
        public byte _songFrequency;
        public bushort _fileName;
        public bushort _title;
        public bushort _songSwitch;
        public byte _disableStockPinch;
        public byte _disableTracklistInclusion;
    }
}

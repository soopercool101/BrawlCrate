using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.ProjectPlus
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct STEX
    {
        public static readonly uint HeaderSize = 0x2C;
        public static readonly uint Tag = 0x53544558;
        public buint _tag;
        public buint _stringOffset;
        public buint _size;
        public buint _rgbaOverlay;
        public bushort _soundBank;
        public bushort _effectBank;
        public bushort _flags;
        public byte _stageType;
        public byte _subStageRange;
        public buint _trackListOffset;
        public buint _stageNameOffset;
        public buint _moduleNameOffset;
        public buint _memoryAllocation;
        public bfloat _wildSpeed;

        public VoidPtr this[int index] => (byte*)Address + HeaderSize + index * 0x04;

        public uint subStageCount => (_stringOffset - 0x2C) / 4;

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

        public string subStageName(int index)
        {
            if (index >= subStageCount)
            {
                return null;
            }

            return Address.GetUTF8String(_stringOffset + this[index].UInt);
        }

        public string trackListName => _trackListOffset == 0xFFFFFFFF ? null : new string((sbyte*)(Address + _stringOffset + _trackListOffset));
        public string stageName => _stageNameOffset == 0xFFFFFFFF ? null : new string((sbyte*)(Address + _stringOffset + _stageNameOffset));
        public string moduleName => _moduleNameOffset == 0xFFFFFFFF ? null : new string((sbyte*)(Address + _stringOffset + _moduleNameOffset));
    }
}

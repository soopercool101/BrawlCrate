using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GEG1
    {
        public const uint Tag = 0x31474547;
        public const int Size = 132;
        public uint _tag;
        public bint _count;
        public bint _DataOffset;

        //private GDOR* Address { get { fixed (GDOR* ptr = &this)return ptr; } }
        //public byte* Data { get { return (byte*)(Address + _DataOffset); } }

        public VoidPtr this[int index] { get { return (VoidPtr)((byte*)Address + Offsets(index)); } }
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x08 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GEG1Entry
    {
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        
        public enum EnemyType : short
        {
            Spaak = 0x0F,
            Prim = 0x17,
            BoxerPrim = 0x14,
            BoomPrim = 0x20,
            SwordPrim = 0x23,
        }
        internal static EnemyType[] _KnownEnemies = 
        {
            EnemyType.Spaak,
            EnemyType.Prim,
            EnemyType.BoxerPrim,
            EnemyType.BoomPrim,
            EnemyType.SwordPrim
        };
    }

}

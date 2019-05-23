using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWAR
    {
        public const string Tag = "RWAR";
        public const int Size = 0x20;

        public NW4RCommonHeader _header;

        public bint _tableOffset;
        public bint _tableLength;
        public bint _dataOffset;
        public bint _dataLength;
        
        public RWARTableBlock* Table { get { return (RWARTableBlock*)(Address + _tableOffset); } }
        public RWARDataBlock* Data { get { return (RWARDataBlock*)(Address + _dataOffset); } }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWARDataBlock
    {
        public const string Tag = "DATA";
        public const int Size = 0x20;

        public SSBBEntryHeader _header;
        public fixed uint _padding[6];

        public RWAV* GetEntry(uint offset) { return (RWAV*)(Address + offset); }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct TableItem
    {
        public ruint waveFileRef; //Offset to RWAV, base is DataBlock
        public buint waveFileSize;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWARTableBlock
    {
        public const string Tag = "TABL";

        public SSBBEntryHeader _header;
        public buint _entryCount;

        public TableItem* Entries { get { return (TableItem*)(Address + 12); } }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
}
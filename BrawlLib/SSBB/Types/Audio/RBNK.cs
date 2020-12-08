using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RBNKHeader
    {
        public const uint Tag = 0x4B4E4252;
        public const int Size = 0x20;

        public NW4RCommonHeader _header;

        public bint _dataOffset;
        public bint _dataLength;
        public bint _waveOffset;
        public bint _waveLength;

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

        public RBNK_DATAHeader* Data => (RBNK_DATAHeader*) (Address + _dataOffset);
        public RBNK_WAVEHeader* Wave => (RBNK_WAVEHeader*) (Address + _waveOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RangeTable
    {
        public byte _tableCount;

        public byte GetKey(int index)
        {
            return *(byte*) (Address + 1 + index);
        }

        //Align to 4 bytes after byte table
        public RuintCollection* Collection => (RuintCollection*) (Address + (1 + _tableCount).Align(4));

        //Invalid, InstParam, RangeTable, IndexTable, Null
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
    internal struct IndexTable
    {
        public byte _min;
        public byte _max;
        public bushort _reserved;
        public RuintCollection _collection; //void,InstParam,RangeTable,IndexTable, any else == void
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RBNK_WAVEHeader
    {
        public const uint Tag = 0x45564157;

        public uint _tag;
        public buint _length;
        public RuintList _list;

        public WaveInfo* this[int index] => (WaveInfo*) (_list.Address + _list.Entries[index]);

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
    internal unsafe struct RBNK_DATAHeader
    {
        public const uint Tag = 0x41544144;

        public uint _tag;
        public bint _length;
        public RuintList _list;

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

    public enum WaveDataLocation
    {
        Index = 0,
        Address = 1,
        Callback = 2
    }

    public enum NoteOffType
    {
        Release = 0,
        Ignore = 1
    }

    //These entries are embedded in a list, using RuintList
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RBNKInstParam
    {
        public const int Size = 0x30;

        public bint _waveIndex;
        public byte _attack;
        public byte _decay;
        public byte _sustain;
        public byte _release;
        public byte _hold;
        public byte _waveDataLocationType;
        public byte _noteOffType;
        public byte _alternateAssign;
        public byte _originalKey;
        public byte _volume;
        public byte _pan;
        public byte _surroundPan;
        public bfloat _pitch;             //1.0
        public ruint _lfoTableRef;        //control = 0, data = 0
        public ruint _graphEnvTablevRef;  //control = 0, data = 0
        public ruint _randomizerTableRef; //control = 0, data = 0
        public bint _reserved;            //0
    }
}
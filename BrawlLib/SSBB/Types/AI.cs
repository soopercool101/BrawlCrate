using BrawlLib.Internal;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct ATKD
    {
        public const uint Tag = 0x444B5441;

        public uint _tag;
        public bint _numEntries;
        public bint _unk1; //0x1CE
        public bint _unk2;

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

        public ATKDEntry* entries => (ATKDEntry*) (Address + 0x10);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct ATKDEntry
    {
        public const uint Size = 0x24;

        public bshort _SubActID; //ID of Sub Action
        public bshort _unk1;
        public bshort _StartFrame;
        public bshort _EndFrame;
        public bfloat _xMinRange;
        public bfloat _xMaxRange;
        public bfloat _yMinRange;
        public bfloat _yMaxRange;

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

        public BVec2 MinimumRange => new BVec2(_xMinRange, _yMinRange);
        public BVec2 MaximumRange => new BVec2(_xMaxRange, _yMaxRange);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct CEHeader //stands for Condition Evaluation
    {
        public bint _unk1;
        public bint _numEntries; //number of offsets
        public bint _unk2;       //0x1
        public bint _unk3;

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

        public bint* entryOffsets =>
            (bint*) (Address + 0x10); //contains entry offset. each offset is distance from Address

        public bint* stringOffsets =>
            (bint*) Address +
            0x10 * _numEntries; //contains string offset but there seems to be other entries on that address
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct CEEntry
    {
        public bint _ID;
        public bint _EventsOffset;
        public bint _part2Offset;
        public bint _unknown;

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

        public VoidPtr Event => Address + _EventsOffset;
        public bfloat* part2 => (bfloat*) (Address + _part2Offset);
    }

    public unsafe struct CEEvent
    {
        public sbyte _type;
        public sbyte _numEntries; //sometimes, it is 0
        public bshort _entrySize;

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

        public bint* Entries => (bint*) (Address + 0x4);
    }

    public enum CEEventType
    {
        SetReaction = 0x1,
        If = 0x6,
        EndIf = 0x9
    }

    public unsafe struct CEString
    {
        public bint _numEntries;
        public bint _unk1;
        public bint _unk2;
        public bint _unk3;

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

        public bint* Entries => (bint*) (Address + 0x10);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPD
    {
        public const uint Tag = 0x44504941;
        public const int Size = 0xF;

        public uint _tag;
        public bint DataOffset; //0000000C
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

        public VoidPtr DefBlock1 => Address + 0x10;
        public VoidPtr DefBlock2 => Address + 0x70;
        public VoidPtr SubBlock1 => Address + 0xD0;
        public VoidPtr SubBlock2 => Address + 0x100;
        public VoidPtr UnkBlock => Address + 0x130;
        public VoidPtr Type1Offsets => Address + 0x170; //from 0x170 to 0x1CF
        public VoidPtr Type2Offsets => Address + 0x1E0; //from0x1E0 to 0x263
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDDefBlock
    {
        public bfloat _float1;
        public bfloat _float2;
        public bshort _short1;
        public bshort _short2;
        public bshort _short3;
        public bshort _short4;
        public bfloat _float3;
        public bfloat _float4;
        public bshort _short5;
        public bshort _short6;
        public bshort _short7;
        public bshort _short8;
        public bfloat _float5;
        public bshort _short9;
        public bshort _short10;
        public bfloat _float6;
        public bshort _short11;
        public bshort _short12;
        public bfloat _float7;
        public bfloat _float8;
        public bfloat _float9;
        public bfloat _float10;
        public bint _int1;
        public bint _int2;
        public bint _int3;
        public bint _int4;
        public bint _int5;
        public bint _int6;
        public bint _int7;
        public byte _byte1;
        public byte _byte2;
        public byte _byte3;
        public byte _byte4;

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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDSubBlock
    {
        public bshort _short1;
        public bshort _short2;
        public bshort _short3;
        public bshort _short4;
        public bfloat _float1;
        public bfloat _float2;
        public bshort _short5;
        public bshort _short6;
        public bshort _short7;
        public bshort _short8;
        public bshort _short9;
        public bshort _short10;
        public byte _byte1;
        public byte _byte2;
        public byte _byte3;
        public byte _byte4;
        public bshort _short11;
        public bshort _short12;
        public bint _int1;
        public bint _int2;
        public bint _int3;

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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDUnkBlock
    {
        public const int numEntries = 64;

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

        public byte[] Padding
        {
            get
            {
                byte[] entries = new byte[numEntries];
                for (int i = 0; i < numEntries; i++)
                {
                    entries[i] = ((byte*) Address)[i];
                }

                return entries;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDType1Offsets
    {
        public const int _numEntries = 28;

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

        public bint* Offsets => (bint*) Address;

        public List<VoidPtr> Entries
        {
            get
            {
                VoidPtr entry = null;
                List<VoidPtr> ptrs = new List<VoidPtr>();
                for (int i = 0; i < _numEntries - 4; i++)
                {
                    entry = Address - 0x170 + Offsets[i];
                    if (Offsets[i] > 0)
                    {
                        ptrs.Add(entry);
                    }
                    else
                    {
                        ptrs.Add(0x0);
                    }
                }

                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDType1
    {
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

        public List<VoidPtr> Entries
        {
            get
            {
                List<VoidPtr> ptrs = new List<VoidPtr>();
                AIPDType1Entry* entry = (AIPDType1Entry*) Address;
                while (entry->_command != 0)
                {
                    ptrs.Add(entry);
                    entry++;
                }

                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDType1Entry
    {
        public byte _command;
        public byte _control1;
        public byte _control2;

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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDType2Offsets
    {
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

        public bint* Offsets => (bint*) Address;

        public List<VoidPtr> Entries
        {
            get
            {
                AIPDType2* entry = null;
                List<VoidPtr> ptrs = new List<VoidPtr>();
                for (int i = 0; Offsets[i] < 0xFFFF; i++)
                {
                    entry = (AIPDType2*) (Address - 0x1E0 + Offsets[i]);
                    ptrs.Add(entry);
                }

                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDType2
    {
        public bshort _id;
        public byte _flag;
        public byte _numEntries;

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

        public List<VoidPtr> Entries
        {
            get
            {
                AIPDType2Entry* entry = (AIPDType2Entry*) (Address + 0x4);
                List<VoidPtr> ptrs = new List<VoidPtr>();
                for (int i = 0; i < _numEntries; i++)
                {
                    ptrs.Add(entry);
                    entry++;
                }

                return ptrs;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIPDType2Entry
    {
        public byte _unk1;
        public byte _unk2;
        public byte _unk3;
        public byte _unk4;
        public bshort _unk5;

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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIUnkDef1
    {
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

        public AIUnkDef1Entry* Entries => (AIUnkDef1Entry*) Address;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct AIUnkDef1Entry
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;
        public bfloat _unk4;
        public byte _unk5; //seems to be always 0F
        public byte _unk6; //usually 14 but 1E,0A sometimes
        public bint padding;
        public bfloat _unk7;
        public bfloat _unk8;

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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct AIUnkDef2
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;
        public bint _pad1;
        public bint _pad2;
        public bint _pad3;

        public bint _pad4;
        //I haven't finished this yet.
    }
}
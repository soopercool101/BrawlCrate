using BrawlLib.Internal;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RSEQHeader
    {
        public const uint Tag = 0x51455352;

        public NW4RCommonHeader _header;

        public bint _dataOffset;
        public bint _dataLength;
        public bint _lablOffset;
        public bint _lablLength;

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

        public RSEQ_DATAHeader* Data => (RSEQ_DATAHeader*) (Address + _dataOffset);
        public LABLHeader* Labl => (LABLHeader*) (Address + _lablOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RSEQ_DATAHeader
    {
        public const uint Tag = 0x41544144;

        public uint _tag;
        public bint _size;
        public bint _baseOffset;

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

        public VoidPtr MMLCommands => Address + _baseOffset;
    }

    public class MMLCommand
    {
        public Mml _cmd;
        public uint _value;
        public MMLCommand[] _ex;

        public MMLCommand()
        {
        }

        public MMLCommand(Mml cmd)
        {
            _cmd = cmd;
        }

        public MMLCommand(Mml cmd, uint value)
        {
            _cmd = cmd;
            _value = value;
        }

        public override string ToString()
        {
            return _cmd.ToString();
        }
    }

    public unsafe class MMLParser
    {
        public static MMLCommand[] Parse(VoidPtr address)
        {
            List<MMLCommand> commands = new List<MMLCommand>();

            Mml cmd;
            byte* addr = (byte*) address;
            while ((cmd = (Mml) (*addr++)) != Mml.MML_FIN)
            {
                MMLCommand mml = new MMLCommand(cmd);
                if (cmd == Mml.MML_WAIT || cmd == Mml.MML_PRG)
                {
                    switch ((SeqArgType) (*addr++))
                    {
                        case SeqArgType.SEQ_ARG_NONE:
                            addr++;
                            break;
                        case SeqArgType.SEQ_ARG_RANDOM:
                            break;
                        case SeqArgType.SEQ_ARG_S16:
                            break;
                        case SeqArgType.SEQ_ARG_U8:
                            break;
                        case SeqArgType.SEQ_ARG_VARIABLE:
                            break;
                        case SeqArgType.SEQ_ARG_VMIDI:
                            break;
                    }
                }
                else if (cmd == Mml.MML_EX_COMMAND)
                {
                    switch ((MmlEx) (*addr++))
                    {
                        case MmlEx.MML_SETVAR:   break;
                        case MmlEx.MML_ADDVAR:   break;
                        case MmlEx.MML_SUBVAR:   break;
                        case MmlEx.MML_MULVAR:   break;
                        case MmlEx.MML_DIVVAR:   break;
                        case MmlEx.MML_SHIFTVAR: break;
                        case MmlEx.MML_RANDVAR:  break;
                        case MmlEx.MML_ANDVAR:   break;
                        case MmlEx.MML_ORVAR:    break;
                        case MmlEx.MML_XORVAR:   break;
                        case MmlEx.MML_NOTVAR:   break;
                        case MmlEx.MML_MODVAR:   break;
                        case MmlEx.MML_CMP_EQ:   break;
                        case MmlEx.MML_CMP_GE:   break;
                        case MmlEx.MML_CMP_GT:   break;
                        case MmlEx.MML_CMP_LE:   break;
                        case MmlEx.MML_CMP_LT:   break;
                        case MmlEx.MML_CMP_NE:   break;
                        case MmlEx.MML_USERPROC: break;
                    }

                    addr += 3;
                }
            }

            commands.Add(new MMLCommand(Mml.MML_FIN, 0));
            return commands.ToArray();
        }
    }

    public enum Mml
    {
        // Variable length parameter commands.
        MML_WAIT = 0x80,
        MML_PRG = 0x81,

        // Control commands.
        MML_OPEN_TRACK = 0x88,
        MML_JUMP = 0x89,
        MML_CALL = 0x8a,

        // prefix commands
        MML_RANDOM = 0xa0,
        MML_VARIABLE = 0xa1,
        MML_IF = 0xa2,
        MML_TIME = 0xa3,
        MML_TIME_RANDOM = 0xa4,
        MML_TIME_VARIABLE = 0xa5,

        // u8 parameter commands.
        MML_TIMEBASE = 0xb0,
        MML_ENV_HOLD = 0xb1,
        MML_MONOPHONIC = 0xb2,
        MML_VELOCITY_RANGE = 0xb3,
        MML_BIQUAD_TYPE = 0xb4,
        MML_BIQUAD_VALUE = 0xb5,
        MML_PAN = 0xc0,
        MML_VOLUME = 0xc1,
        MML_MAIN_VOLUME = 0xc2,
        MML_TRANSPOSE = 0xc3,
        MML_PITCH_BEND = 0xc4,
        MML_BEND_RANGE = 0xc5,
        MML_PRIO = 0xc6,
        MML_NOTE_WAIT = 0xc7,
        MML_TIE = 0xc8,
        MML_PORTA = 0xc9,
        MML_MOD_DEPTH = 0xca,
        MML_MOD_SPEED = 0xcb,
        MML_MOD_TYPE = 0xcc,
        MML_MOD_RANGE = 0xcd,
        MML_PORTA_SW = 0xce,
        MML_PORTA_TIME = 0xcf,
        MML_ATTACK = 0xd0,
        MML_DECAY = 0xd1,
        MML_SUSTAIN = 0xd2,
        MML_RELEASE = 0xd3,
        MML_LOOP_START = 0xd4,
        MML_VOLUME2 = 0xd5,
        MML_PRINTVAR = 0xd6,
        MML_SURROUND_PAN = 0xd7,
        MML_LPF_CUTOFF = 0xd8,
        MML_FXSEND_A = 0xd9,
        MML_FXSEND_B = 0xda,
        MML_MAINSEND = 0xdb,
        MML_INIT_PAN = 0xdc,
        MML_MUTE = 0xdd,
        MML_FXSEND_C = 0xde,
        MML_DAMPER = 0xdf,

        // s16 parameter commands.
        MML_MOD_DELAY = 0xe0,
        MML_TEMPO = 0xe1,
        MML_SWEEP_PITCH = 0xe3,

        // Extended commands.
        MML_EX_COMMAND = 0xf0,

        // Other
        MML_ENV_RESET = 0xfb,
        MML_LOOP_END = 0xfc,
        MML_RET = 0xfd,
        MML_ALLOC_TRACK = 0xfe,
        MML_FIN = 0xff
    }

    public enum MmlEx
    {
        MML_SETVAR = 0x80,
        MML_ADDVAR = 0x81,
        MML_SUBVAR = 0x82,
        MML_MULVAR = 0x83,
        MML_DIVVAR = 0x84,
        MML_SHIFTVAR = 0x85,
        MML_RANDVAR = 0x86,
        MML_ANDVAR = 0x87,
        MML_ORVAR = 0x88,
        MML_XORVAR = 0x89,
        MML_NOTVAR = 0x8a,
        MML_MODVAR = 0x8b,

        MML_CMP_EQ = 0x90,
        MML_CMP_GE = 0x91,
        MML_CMP_GT = 0x92,
        MML_CMP_LE = 0x93,
        MML_CMP_LT = 0x94,
        MML_CMP_NE = 0x95,

        MML_USERPROC = 0xe0
    }

    public enum SeqArgType
    {
        SEQ_ARG_NONE,
        SEQ_ARG_U8,
        SEQ_ARG_S16,
        SEQ_ARG_VMIDI,
        SEQ_ARG_RANDOM,
        SEQ_ARG_VARIABLE
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct LABLHeader
    {
        public const uint Tag = 0x4C42414C;

        public uint _tag;
        public bint _size;
        public bint _numEntries;

        public void Set(int size, int count)
        {
            _tag = Tag;
            _size = size;
            _numEntries = count;
        }

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

        public bint* EntryOffset => (bint*) (Address + 12);

        public LABLEntry* Get(int index)
        {
            bint* offset = (bint*) (Address + 8);
            return (LABLEntry*) ((int) offset + offset[index + 1]);
        }

        public string GetString(int index)
        {
            bint* offset = (bint*) (Address + 8);
            return ((LABLEntry*) ((int) offset + offset[index + 1]))->Name;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct LABLEntry
    {
        public buint _id;
        public buint _stringLength;

        public void Set(uint id, string str)
        {
            uint len = (uint) str.Length;
            int i = 0;
            sbyte* dPtr = (sbyte*) (Address + 8);
            char* sPtr;

            _id = id;
            _stringLength = len;

            fixed (char* s = str)
            {
                sPtr = s;
                while (i++ < len)
                {
                    *dPtr++ = (sbyte) *sPtr++;
                }
            }

            //Trailing zero
            *dPtr++ = 0;

            //Padding
            while ((i++ & 3) != 0)
            {
                *dPtr++ = 0;
            }
        }

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

        public string Name => new string((sbyte*) Address + 8);
    }
}
using System;

namespace BrawlLib.Internal.PowerPCAssembly
{
    public abstract partial class PPCOpCode
    {
        public class PPCOperand
        {
            private readonly PPCOpCode _owner;

            private readonly OperandType _opType = OperandType.VAL;
            private readonly int _bitShift;
            private readonly uint _bitMask;
            private readonly uint _negBit;
            private readonly string _name;

            public int Value
            {
                get => Get();
                set => Set(value);
            }

            public string Name => _name;

            public PPCOperand(PPCOpCode owner, OperandType type, int shift, uint mask) : this(owner, type, shift, mask,
                0x0)
            {
            }

            public PPCOperand(PPCOpCode owner, OperandType type, int shift, uint mask, uint negBit) : this(owner, type,
                shift, mask, negBit, "")
            {
            }

            public PPCOperand(PPCOpCode owner, OperandType type, int shift, uint mask, uint negBit, string name)
            {
                _owner = owner;
                _opType = type;
                _bitShift = shift;
                _bitMask = mask & ~negBit;
                _negBit = negBit;
                _name = name;
            }

            public int Get()
            {
                uint baseVal = _owner._data;

                int result = (int) ((baseVal >> _bitShift) & _bitMask);

                if ((baseVal & _negBit) != 0)
                {
                    result -= (int) _negBit;
                }

                return result;
            }

            public string GetFormatted()
            {
                int val = Get();

                switch (_opType)
                {
                    case OperandType.VAL:       return (val < 0 ? "-" : "") + $"0x{Math.Abs(val):X}";
                    case OperandType.UVAL:      return $"0x{val:X}";
                    case OperandType.OFFSET:    return (val < 0 ? "-" : "") + $"0x{Math.Abs(val):X}";
                    case OperandType.UOFFSET:   return $"0x{val:X}";
                    case OperandType.REGISTER:  return $"r{val}";
                    case OperandType.FREGISTER: return $"f{val}";
                    case OperandType.CREGISTER: return $"cr{val}";
                    case OperandType.VREGISTER: return $"v{val}";
                    case OperandType.SREGISTER:
                        switch (val)
                        {
                            case 0:  return "mq";
                            case 1:  return "xer";
                            case 4:  return "rtcu";
                            case 5:  return "rtcl";
                            case 8:  return "lr";
                            case 9:  return "ctr";
                            case 18: return "dsisr";
                            case 19: return "dar";
                            case 22: return "dec";
                            case 25: return "sdr1";
                            case 26: return "srr0";
                            case 27: return "srr1";
                            default: return "sp" + val;
                        }

                    default:
                        return "";
                }
            }

            public void Set(int value)
            {
                _owner._data &= ~(_bitMask << _bitShift);

                if (value > 0)
                {
                    _owner._data |= (uint) ((value & _bitMask) << _bitShift);
                    _owner._data &= ~_negBit;
                }
                else
                {
                    _owner._data |= (uint) (((value + _negBit) & _bitMask) << _bitShift);
                    _owner._data |= _negBit;
                }
            }
        }
    }
}
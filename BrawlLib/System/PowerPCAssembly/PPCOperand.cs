namespace System.PowerPcAssembly
{
    public abstract unsafe partial class PPCOpCode
    {
        public class PPCOperand
        {
            private PPCOpCode _owner = null;

            private OperandType _opType = OperandType.VAL;
            private int _bitShift = 0;
            private uint _bitMask = 0;
            private uint _negBit = 0;
            private string _name;

            public int Value { get { return Get(); } set { Set(value); } }
            public string Name { get { return _name; } }
            
            public PPCOperand(PPCOpCode owner, OperandType type, int shift, uint mask) : this(owner, type, shift, mask, 0x0) { }
            public PPCOperand(PPCOpCode owner, OperandType type, int shift, uint mask, uint negBit) : this(owner, type, shift, mask, negBit, "") { }
            public PPCOperand(PPCOpCode owner, OperandType type, int shift, uint mask, uint negBit, string name)
            {
                _owner = owner;
                _opType = type;
                _bitShift = shift;
                _bitMask = mask & ~(uint)negBit;
                _negBit = negBit;
                _name = name;
            }

            public int Get()
            {
                uint baseVal = _owner._data;

                int result = (int)((baseVal >> _bitShift) & _bitMask);

                if ((baseVal & _negBit) != 0)
                    result -= (int)_negBit;

                return result;
            }

            public string GetFormatted()
            {
                int val = Get();

                switch (_opType)
                {
                    case OperandType.VAL: return (val < 0 ? "-" : "") + String.Format("0x{0:X}", Math.Abs(val));
                    case OperandType.UVAL: return String.Format("0x{0:X}", val);
                    case OperandType.OFFSET: return (val < 0 ? "-" : "") + String.Format("0x{0:X}", Math.Abs(val));
                    case OperandType.UOFFSET: return String.Format("0x{0:X}", val);
                    case OperandType.REGISTER: return String.Format("r{0}", val);
                    case OperandType.FREGISTER: return String.Format("f{0}", val);
                    case OperandType.CREGISTER: return String.Format("cr{0}", val);
                    case OperandType.VREGISTER: return String.Format("v{0}", val);
                    case OperandType.SREGISTER:
                        switch (val)
                        {
                            case 0: return "mq";
                            case 1: return "xer";
                            case 4: return "rtcu";
                            case 5: return "rtcl";
                            case 8: return "lr";
                            case 9: return "ctr";
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
                    _owner._data |= (uint)((value & _bitMask) << _bitShift);
                    _owner._data &= ~_negBit;
                }
                else
                {
                    _owner._data |= (uint)(((value + _negBit) & _bitMask) << _bitShift);
                    _owner._data |= (uint)_negBit;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.Internal.PowerPCAssembly
{
    public abstract partial class PPCOpCode : ICloneable
    {
        protected Bin32 _data = 0;

        protected List<string> _names;
        protected List<PPCOperand> _operands;

        [Browsable(false)]
        public PPCMnemonic Operation
        {
            get => (PPCMnemonic) (this & 0xFC000000);
            set
            {
                _data &= ~0xFC000000;
                _data |= (uint) value & 0xFC000000;
            }
        }

        [Browsable(false)] public string Name => GetName();
        [Browsable(false)] public PPCOperand[] Operands => _operands.ToArray();

        internal PPCOpCode(uint value)
        {
            _data = value;
            _names = new List<string>();
            _operands = new List<PPCOperand>();
        }

        public static implicit operator Bin32(PPCOpCode o)
        {
            return o._data;
        }

        public static implicit operator PPCOpCode(Bin32 u)
        {
            return PowerPC.Disassemble(u);
        }

        public static implicit operator uint(PPCOpCode o)
        {
            return o._data;
        }

        public static implicit operator PPCOpCode(uint u)
        {
            return PowerPC.Disassemble(u);
        }

        public virtual string GetName()
        {
            if (_names.Count > 0)
            {
                return _names[0];
            }

            return ".word";
        }

        public virtual string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            return string.Join(", ", formatted);
        }

        public override string ToString()
        {
            return GetName() + " " + GetFormattedOperands();
        }

        #region ICloneable Members

        public PPCOpCode Clone()
        {
            return (PPCOpCode) MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }

    //  .word
    public class PPCWord : PPCOpCode
    {
        internal PPCWord(uint value) : base(value)
        {
            _names.Add(".word");
        }

        public override string GetFormattedOperands()
        {
            return $"0x{(uint) _data._data:X8}";
        }
    }

    //  vaddubm
    public class PPCVaddubm : PPCOpCode
    {
        internal PPCVaddubm(uint value) : base(value)
        {
            _names.Add("vaddubm");
        }

        public override string GetFormattedOperands()
        {
            return $"0x{_data._data:X8}";
        }
    }

    //  mulli
    public class PPCMulli : PPCOpCode
    {
        public int LeftRegister
        {
            get => _operands[0].Value;
            set => _operands[0].Value = value;
        }

        public int RightRegister
        {
            get => _operands[1].Value;
            set => _operands[1].Value = value;
        }

        public int Value
        {
            get => _operands[2].Value;
            set => _operands[2].Value = value;
        }

        internal PPCMulli(uint value) : base(value)
        {
            _names.Add("mulli");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value
        }
    }

    //  mullw
    public class PPCMullw : PPCOpCode
    {
        public int LeftRegister
        {
            get => _operands[0].Value;
            set => _operands[0].Value = value;
        }

        public int RightRegister
        {
            get => _operands[1].Value;
            set => _operands[1].Value = value;
        }

        public int Value
        {
            get => _operands[2].Value;
            set => _operands[2].Value = value;
        }

        internal PPCMullw(uint value) : base(value)
        {
            _names.Add("mullw");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Immediate Value
        }
    }

    //  subfic
    public class PPCSubfic : PPCOpCode
    {
        public int LeftRegister
        {
            get => _operands[0].Value;
            set => _operands[0].Value = value;
        }

        public int RightRegister
        {
            get => _operands[1].Value;
            set => _operands[1].Value = value;
        }

        public int Value
        {
            get => _operands[2].Value;
            set => _operands[2].Value = value;
        }

        internal PPCSubfic(uint value) : base(value)
        {
            _names.Add("subfic");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value            
        }
    }

    //  cmplwi, cmpldi
    public class PPCCmpli : PPCOpCode
    {
        public int ConditionRegister
        {
            get => _operands[0].Value;
            set => _operands[0].Value = value;
        }

        public bool Unknown
        {
            get => _operands[1].Value != 0;
            set => _operands[1].Value = value ? 1 : 0;
        }

        public bool IsDouble
        {
            get => _operands[2].Value != 0;
            set => _operands[2].Value = value ? 1 : 0;
        }

        public int LeftRegister
        {
            get => _operands[3].Value;
            set => _operands[3].Value = value;
        }

        public int Value
        {
            get => _operands[4].Value;
            set => _operands[4].Value = value;
        }

        internal PPCCmpli(uint value) : base(value)
        {
            _names.Add("cmplwi");
            _names.Add("cmpldi");
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 23, 0x7));     //  [0] Condition Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 22, 0x1));           //  [1] Unknown
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1));           //  [2] Is Double
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [3] Left Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [4] Immediate Value
        }

        public override string GetName()
        {
            if (IsDouble)
            {
                return _names[1];
            }

            return _names[0];
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (_operands[0].Value == 0)
            {
                return string.Format("{3}, {4}", formatted);
            }

            return string.Format("{0}, {3}, {4}", formatted);
        }
    }

    //  cmpwi, cmpdi
    public class PPCCmpi : PPCOpCode
    {
        public int ConditionRegister
        {
            get => _operands[0].Value;
            set => _operands[0].Value = value;
        }

        public bool Unknown
        {
            get => _operands[1].Value != 0;
            set => _operands[1].Value = value ? 1 : 0;
        }

        public bool IsDouble
        {
            get => _operands[2].Value != 0;
            set => _operands[2].Value = value ? 1 : 0;
        }

        public int LeftRegister
        {
            get => _operands[3].Value;
            set => _operands[3].Value = value;
        }

        public int Value
        {
            get => _operands[4].Value;
            set => _operands[4].Value = value;
        }

        internal PPCCmpi(uint value) : base(value)
        {
            _names.Add("cmpwi");
            _names.Add("cmpdi");
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 23, 0x7));     //  [0] Condition Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 22, 0x1));           //  [1] Unknown
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1));           //  [2] Is Double
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [3] Left Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [4] Immediate Value
        }

        public override string GetName()
        {
            if (IsDouble)
            {
                return _names[1];
            }

            return _names[0];
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (_operands[0].Value == 0)
            {
                return string.Format("{3}, {4}", formatted);
            }

            return string.Format("{0}, {3}, {4}", formatted);
        }
    }

    //  addic, subic, addic., subic.
    public class PPCAddic : PPCOpCode
    {
        public bool SetCr => Operation == PPCMnemonic.addic_D;

        public int LeftRegister
        {
            get => _operands[0].Value;
            set => _operands[0].Value = value;
        }

        public int RightRegister
        {
            get => _operands[1].Value;
            set => _operands[1].Value = value;
        }

        public int ImmediateValue
        {
            get => _operands[2].Value;
            set => _operands[2].Value = value;
        }

        internal PPCAddic(uint value) : base(value)
        {
            _names.Add("addic");
            _names.Add("subic");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value      
        }

        public override string GetName()
        {
            string name = "";

            if (_operands[2].Value >= 0)
            {
                name = _names[0];
            }
            else
            {
                name = _names[1];
            }

            if (SetCr)
            {
                name += ".";
            }

            return name;
        }
    }

    //  addi, addis, subi, subis, li, lis
    public class PPCAddi : PPCOpCode
    {
        public bool Shifted => Operation == PPCMnemonic.addis;

        internal PPCAddi(uint value) : base(value)
        {
            _names.Add("addi");
            _names.Add("subi");
            _names.Add("li");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        public override string GetName()
        {
            string name = "";

            if (_operands[1].Value == 0)
            {
                name = _names[2];
            }
            else if (_operands[2].Value < 0)
            {
                name = _names[1];
            }
            else
            {
                name = _names[0];
            }

            if (Shifted)
            {
                name += "s";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (_operands[1].Value == 0)
            {
                return string.Format("{0}, {2}", formatted);
            }

            return string.Format("{0}, {1}, {2}", formatted);
        }
    }

    public class PPCBranch : PPCOpCode
    {
        internal PPCBranch(uint value) : base(value)
        {
        }

        public int _offsetID;

        [Browsable(false)]
        public int DataOffset
        {
            get => _operands[_offsetID].Value;
            set => _operands[_offsetID].Value = value;
        }

        public string Offset
        {
            get => (DataOffset < 0 ? "-" : "") + "0x" + Math.Abs(DataOffset).ToString("X");
            set
            {
                string s = value;
                bool neg = s.StartsWith("-");
                if (neg)
                {
                    s = s.Substring(1);
                }

                s = s.StartsWith("0x")
                    ? s.Substring(2, Math.Min(s.Length - 2, 8))
                    : s.Substring(0, Math.Min(s.Length, 8));
                if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out int offset))
                {
                    DataOffset = offset * (neg ? -1 : 1);
                }
            }
        }

        public bool Absolute
        {
            get => _operands[_offsetID + 1].Value != 0;
            set => _operands[_offsetID + 1].Value = value ? 1 : 0;
        }

        public bool Link
        {
            get => _operands[_offsetID + 2].Value != 0;
            set => _operands[_offsetID + 2].Value = value ? 1 : 0;
        }
    }

    //  bc, bdnz, bdnzf, bdnzt, bdz, bdzf, bdzt, beq, bne, bgt, blt, bge, ble
    public class PPCBc : PPCBranch
    {
        public bool IgnoreCr
        {
            get => _operands[0].Value != 0;
            set => _operands[0].Value = value ? 1 : 0;
        }

        public bool CrState
        {
            get => _operands[1].Value != 0;
            set => _operands[1].Value = value ? 1 : 0;
        }

        public bool IgnoreCtr
        {
            get => _operands[2].Value != 0;
            set => _operands[2].Value = value ? 1 : 0;
        }

        public bool CtrState
        {
            get => _operands[3].Value != 0;
            set => _operands[3].Value = value ? 1 : 0;
        }

        public bool Hint
        {
            get => _operands[4].Value != 0;
            set => _operands[4].Value = value ? 1 : 0;
        }

        public int ConditionRegister
        {
            get => _operands[5].Value;
            set => _operands[5].Value = value;
        }

        public int CompareType
        {
            get => _operands[6].Value;
            set => _operands[6].Value = value;
        }

        public int BranchInputs
        {
            get => _operands[7].Value;
            set => _operands[7].Value = value;
        }

        public int BranchOptions
        {
            get => _operands[8].Value;
            set => _operands[8].Value = value;
        }
        //public string Offset
        //{
        //    get { return (_operands[9].Value < 0 ? "-" : "") + "0x" + Math.Abs(_operands[9].Value).ToString("X"); }
        //    set
        //    {
        //        string s = value;
        //        bool neg = s.StartsWith("-");
        //        if (neg) s = s.Substring(1);
        //        s = (s.StartsWith("0x") ? s.Substring(2, Math.Min(s.Length - 2, 8)) : s.Substring(0, Math.Min(s.Length, 8)));
        //        int offset;
        //        if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out offset))
        //            _operands[9].Value = (offset * (neg ? -1 : 1));
        //    }
        //}
        //public bool Absolute { get { return _operands[10].Value != 0; } set { _operands[10].Value = value ? 1 : 0; } }
        //public bool Link { get { return _operands[11].Value != 0; } set { _operands[11].Value = value ? 1 : 0; } }

        internal PPCBc(uint value) : base(value)
        {
            _offsetID = 9;

            _names.Add("bc");
            _names.Add("bdnz");
            _names.Add("bdz");
            _names.Add("bge");
            _names.Add("blt");
            _names.Add("ble");
            _names.Add("bgt");
            _names.Add("bne");
            _names.Add("beq");
            _names.Add("bns");
            _names.Add("bso");
            _operands.Add(new PPCOperand(this, OperandType.VAL, 25,
                0x1)); //  [0] Ignore Cr                       (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 24,
                0x1)); //  [1] Cr State 0=false, 1=true        (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 23,
                0x1)); //  [2] Ignore Ctr                      (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 22,
                0x1)); //  [3] Ctr State 0=not zero, 1=zero    (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21,
                0x1));                                                                  //  [4] Hint 0=unlikely, 1=likely (BI)
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 18, 0x7));        //  [5] Condition Register  (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 16, 0x3));              //  [6] Compare Type        (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1F));             //  [7] Branch Inputs (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 16, 0x1F));             //  [8] Branch Options (BI)
            _operands.Add(new PPCOperand(this, OperandType.OFFSET, 0, 0x7FFC, 0x8000)); //  [9] Offset
            _operands.Add(new PPCOperand(this, OperandType.VAL, 1, 0x1));               //  [10] Absolute
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));               //  [11] Link
        }

        public override string GetName()
        {
            if (IgnoreCr && IgnoreCtr)
            {
                return _names[0];
            }

            string name = "";
            if (!IgnoreCr && IgnoreCtr)
            {
                name = _names[3 + CompareType * 2 + (CrState ? 1 : 0)];
            }
            else if (IgnoreCr && !IgnoreCtr)
            {
                name = _names[1 + (CtrState ? 1 : 0)];
            }

            if (!IgnoreCr && !IgnoreCtr)
            {
                name = _names[1 + (CtrState ? 1 : 0)] + (CrState ? "t" : "f");
            }

            if (Link)
            {
                name += "l";
            }

            if (Absolute)
            {
                name += "a";
            }

            if (Hint ^ (_operands[5].Value < 0))
            {
                name += "+";
            }
            else
            {
                name += "-";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (!IgnoreCr && IgnoreCtr)
            {
                if (_operands[0].Value == 0)
                {
                    return string.Format("{9}", formatted);
                }

                return string.Format("{5}, {9}", formatted);
            }

            if (IgnoreCr && !IgnoreCtr)
            {
                return string.Format("{9}", formatted);
            }

            if (!IgnoreCr && !IgnoreCtr)
            {
                return string.Format("{8}, {9}", formatted);
            }

            return string.Format("{7}, {8}, {9}", formatted);
        }
    }

    //  b, bl, ba, bla
    public class PPCbx : PPCBranch
    {
        //public string Offset
        //{
        //    get { return (_operands[0].Value < 0 ? "-" : "") + "0x" + Math.Abs(_operands[0].Value).ToString("X"); }
        //    set
        //    {
        //        string s = value;
        //        bool neg = s.StartsWith("-");
        //        if (neg) s = s.Substring(1);
        //        s = (s.StartsWith("0x") ? s.Substring(2, Math.Min(s.Length - 2, 8)) : s.Substring(0, Math.Min(s.Length, 8)));
        //        int offset;
        //        if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out offset))
        //            _operands[0].Value = (offset * (neg ? -1 : 1));
        //    }
        //}
        //public bool Absolute { get { return _operands[1].Value != 0; } set { _operands[1].Value = value ? 1 : 0; } }
        //public bool Link { get { return _operands[2].Value != 0; } set { _operands[2].Value = value ? 1 : 0; } }

        internal PPCbx(uint value) : base(value)
        {
            _offsetID = 0;

            _names.Add("b");
            _operands.Add(new PPCOperand(this, OperandType.OFFSET, 0, 0x3FFFFFC, 0x2000000)); //  [0] Offset
            _operands.Add(new PPCOperand(this, OperandType.VAL, 1, 0x1));                     //  [1] Absolute
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));                     //  [2] Link
        }

        public override string GetName()
        {
            string name = _names[0];

            if (Link)
            {
                name += "l";
            }

            if (Absolute)
            {
                name += "a";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            return string.Format("{0}", formatted);
        }
    }

    //  blr
    public class PPCblr : PPCBranch
    {
        public bool IgnoreCr
        {
            get => _operands[0].Value != 0;
            set => _operands[0].Value = value ? 1 : 0;
        }

        public bool CrState
        {
            get => _operands[1].Value != 0;
            set => _operands[1].Value = value ? 1 : 0;
        }

        public bool IgnoreCtr
        {
            get => _operands[2].Value != 0;
            set => _operands[2].Value = value ? 1 : 0;
        }

        public bool CtrState
        {
            get => _operands[3].Value != 0;
            set => _operands[3].Value = value ? 1 : 0;
        }

        public bool Hint
        {
            get => _operands[4].Value != 0;
            set => _operands[4].Value = value ? 1 : 0;
        }

        public int ConditionRegister
        {
            get => _operands[5].Value;
            set => _operands[5].Value = value;
        }

        public int CompareType
        {
            get => _operands[6].Value;
            set => _operands[6].Value = value;
        }

        public int BranchInputs
        {
            get => _operands[7].Value;
            set => _operands[7].Value = value;
        }

        public int BranchOptions
        {
            get => _operands[8].Value;
            set => _operands[8].Value = value;
        }
        //public string Offset
        //{
        //    get { return (_operands[9].Value < 0 ? "-" : "") + "0x" + Math.Abs(_operands[9].Value).ToString("X"); }
        //    set
        //    {
        //        string s = value;
        //        bool neg = s.StartsWith("-");
        //        if (neg) s = s.Substring(1);
        //        s = (s.StartsWith("0x") ? s.Substring(2, Math.Min(s.Length - 2, 8)) : s.Substring(0, Math.Min(s.Length, 8)));
        //        int offset;
        //        if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out offset))
        //            _operands[9].Value = (offset * (neg ? -1 : 1));
        //    }
        //}
        //public bool Absolute { get { return _operands[10].Value != 0; } set { _operands[10].Value = value ? 1 : 0; } }
        //public bool Link { get { return _operands[11].Value != 0; } set { _operands[11].Value = value ? 1 : 0; } }

        internal PPCblr(uint value) : base(value)
        {
            _offsetID = 9;

            _names.Add("blr");
            _names.Add("bdnz");
            _names.Add("bdz");
            _names.Add("bge");
            _names.Add("blt");
            _names.Add("ble");
            _names.Add("bgt");
            _names.Add("bne");
            _names.Add("beq");
            _names.Add("bns");
            _names.Add("bso");
            _operands.Add(new PPCOperand(this, OperandType.VAL, 25,
                0x1)); //  [0] Ignore Cr                       (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 24,
                0x1)); //  [1] Cr State 0=false, 1=true        (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 23,
                0x1)); //  [2] Ignore Ctr                      (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 22,
                0x1)); //  [3] Ctr State 0=not zero, 1=zero    (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21,
                0x1));                                                                  //  [4] Hint 0=unlikely, 1=likely (BI)
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 18, 0x7));        //  [5] Condition Register  (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 16, 0x3));              //  [6] Compare Type        (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1F));             //  [7] Branch Inputs (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 16, 0x1F));             //  [8] Branch Options (BI)
            _operands.Add(new PPCOperand(this, OperandType.OFFSET, 0, 0x7FFC, 0x8000)); //  [9] Offset
            _operands.Add(new PPCOperand(this, OperandType.VAL, 1, 0x1));               //  [10] Absolute
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));               //  [11] Link
        }

        public override string GetName()
        {
            if (IgnoreCr && IgnoreCtr)
            {
                return _names[0];
            }

            string name = "";
            if (!IgnoreCr && IgnoreCtr)
            {
                name = _names[3 + CompareType * 2 + (CrState ? 1 : 0)];
            }
            else if (IgnoreCr && !IgnoreCtr)
            {
                name = _names[1 + (CtrState ? 1 : 0)];
            }

            if (!IgnoreCr && !IgnoreCtr)
            {
                name = _names[1 + (CtrState ? 1 : 0)] + (CrState ? "t" : "f");
            }

            name += "lr";

            if (Link)
            {
                name += "l";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (!IgnoreCr && IgnoreCtr)
            {
                if (_operands[0].Value == 0)
                {
                    return string.Format("{9}", formatted);
                }

                return string.Format("{5}, {9}", formatted);
            }

            if (IgnoreCr && !IgnoreCtr)
            {
                return string.Format("{9}", formatted);
            }

            if (!IgnoreCr && !IgnoreCtr)
            {
                return string.Format("{8}, {9}", formatted);
            }

            return "";
            //return String.Format("{7}, {8}, {9}", formatted);
        }
    }

    //  bctr
    public class PPCbctr : PPCBranch
    {
        public bool IgnoreCr
        {
            get => _operands[0].Value != 0;
            set => _operands[0].Value = value ? 1 : 0;
        }

        public bool CrState
        {
            get => _operands[1].Value != 0;
            set => _operands[1].Value = value ? 1 : 0;
        }

        public bool IgnoreCtr
        {
            get => _operands[2].Value != 0;
            set => _operands[2].Value = value ? 1 : 0;
        }

        public bool CtrState
        {
            get => _operands[3].Value != 0;
            set => _operands[3].Value = value ? 1 : 0;
        }

        public bool Hint
        {
            get => _operands[4].Value != 0;
            set => _operands[4].Value = value ? 1 : 0;
        }

        public int ConditionRegister
        {
            get => _operands[5].Value;
            set => _operands[5].Value = value;
        }

        public int CompareType
        {
            get => _operands[6].Value;
            set => _operands[6].Value = value;
        }

        public int BranchInputs
        {
            get => _operands[7].Value;
            set => _operands[7].Value = value;
        }

        public int BranchOptions
        {
            get => _operands[8].Value;
            set => _operands[8].Value = value;
        }
        //public string Offset
        //{
        //    get { return (_operands[9].Value < 0 ? "-" : "") + "0x" + Math.Abs(_operands[9].Value).ToString("X"); }
        //    set
        //    {
        //        string s = value;
        //        bool neg = s.StartsWith("-");
        //        if (neg) s = s.Substring(1);
        //        s = (s.StartsWith("0x") ? s.Substring(2, Math.Min(s.Length - 2, 8)) : s.Substring(0, Math.Min(s.Length, 8)));
        //        int offset;
        //        if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out offset))
        //            _operands[9].Value = (offset * (neg ? -1 : 1));
        //    }
        //}
        //public bool Absolute { get { return _operands[10].Value != 0; } set { _operands[10].Value = value ? 1 : 0; } }
        //public bool Link { get { return _operands[11].Value != 0; } set { _operands[11].Value = value ? 1 : 0; } }

        internal PPCbctr(uint value) : base(value)
        {
            _offsetID = 9;

            _names.Add("bctr");
            _names.Add("bdnz");
            _names.Add("bdz");
            _names.Add("bge");
            _names.Add("blt");
            _names.Add("ble");
            _names.Add("bgt");
            _names.Add("bne");
            _names.Add("beq");
            _names.Add("bns");
            _names.Add("bso");
            _operands.Add(new PPCOperand(this, OperandType.VAL, 25,
                0x1)); //  [0] Ignore Cr                       (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 24,
                0x1)); //  [1] Cr State 0=false, 1=true        (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 23,
                0x1)); //  [2] Ignore Ctr                      (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 22,
                0x1));                                                           //  [3] Ctr State 0=not zero, 1=zero    (BI)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1));       //  [-] Hint 0=unlikely, 1=likely (BI)
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 18, 0x7)); //  [4] Condition Register  (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 16, 0x3));       //  [5] Compare Type        (BO)

            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1F));             //  [6] Branch Inputs (BO)
            _operands.Add(new PPCOperand(this, OperandType.VAL, 16, 0x1F));             //  [7] Branch Options (BI)
            _operands.Add(new PPCOperand(this, OperandType.OFFSET, 0, 0x7FFC, 0x8000)); //  [-] Offset
            _operands.Add(new PPCOperand(this, OperandType.VAL, 1, 0x1));               //  [-] Absolute
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));               //  [8] Link
        }

        public override string GetName()
        {
            string name = "";
            if (IgnoreCr && IgnoreCtr)
            {
                name = _names[0];
                if (Link)
                {
                    name += "l";
                }

                return name;
            }

            if (!IgnoreCr && IgnoreCtr)
            {
                name = _names[3 + CompareType * 2 + (CrState ? 1 : 0)];
            }
            else if (IgnoreCr && !IgnoreCtr)
            {
                name = _names[1 + (CtrState ? 1 : 0)];
            }

            if (!IgnoreCr && !IgnoreCtr)
            {
                name = _names[1 + (CtrState ? 1 : 0)] + (CrState ? "t" : "f");
            }

            name += "ctr";

            if (Link)
            {
                name += "l";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (!IgnoreCr && IgnoreCtr)
            {
                if (_operands[0].Value == 0)
                {
                    return string.Format("{9}", formatted);
                }

                return string.Format("{5}, {9}", formatted);
            }

            if (IgnoreCr && !IgnoreCtr)
            {
                return string.Format("{9}", formatted);
            }

            if (!IgnoreCr && !IgnoreCtr)
            {
                return string.Format("{8}, {9}", formatted);
            }

            return "";
        }
    }

    //  rlwimi
    public class PPCRlwimi : PPCOpCode
    {
        public bool SetCr
        {
            get => _data[0];
            set => _data[0] = value;
        }

        internal PPCRlwimi(uint value) : base(value)
        {
            _names.Add("rlwimi");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); // [1] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); // [0] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 11, 0x1F));      // [2] Roll Left
            _operands.Add(new PPCOperand(this, OperandType.VAL, 6, 0x1F));       // [3] NAND Mask
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x3E));       // [4] AND Mask
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            // [-] Set Cr
        }

        public override string GetName()
        {
            return base.GetName() + (SetCr ? "." : "");
        }
    }

    //  rlwinm
    public class PPCRlwinm : PPCOpCode
    {
        public bool SetCr => (this & 0x1) != 0;

        public PPCRlwinm(uint value) : base(value)
        {
            _names.Add("rlwinm");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); // [1] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); // [0] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 11, 0x1F));      // [2] Roll Left
            _operands.Add(new PPCOperand(this, OperandType.VAL, 6, 0x1F));       // [3] NAND Mask
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x3E));       // [4] AND Mask
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            // [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  rlwmn
    public class OpRlwmn : PPCOpCode
    {
        public bool SetCr => (this & 0x1) != 0;

        internal OpRlwmn(uint value) : base(value)
        {
            _names.Add("rlwmn");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); // [1] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); // [0] Right Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); // [2] Right Register 2
            _operands.Add(new PPCOperand(this, OperandType.VAL, 6, 0x1F));       // [3] NAND Mask
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x3E));       // [4] AND Mask
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            // [-] Set Cr      
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  xor
    public class PPCXor : PPCOpCode
    {
        internal PPCXor(uint value)
            : base(value)
        {
            _names.Add("xor");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register 1
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Right Register 2
        }
    }

    //  or, mr
    public class PPCOr : PPCOpCode
    {
        internal PPCOr(uint value)
            : base(value)
        {
            _names.Add("or");
            _names.Add("mr");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register 1
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Right Register 2
        }

        public override string GetName()
        {
            if (_operands[1].Value == _operands[2].Value)
            {
                return _names[1];
            }

            return base.GetName();
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (_operands[1].Value == _operands[2].Value)
            {
                return string.Format("{0}, {1}", formatted);
            }

            return string.Format("{0}, {1}, {2}", formatted);
        }
    }

    //  ori, oris, xori, xoris, nop
    public class PPCOri : PPCOpCode
    {
        private bool Shifted => Operation == PPCMnemonic.oris || Operation == PPCMnemonic.xoris;

        private bool Exclusive => Operation == PPCMnemonic.xori || Operation == PPCMnemonic.xoris;

        internal PPCOri(uint value) : base(value)
        {
            _names.Add("ori");
            _names.Add("xori");
            _names.Add("nop");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value
        }

        public override string GetName()
        {
            if (!Exclusive && _operands[0].Value == 0 && _operands[1].Value == 0 && _operands[2].Value == 0)
            {
                return _names[2];
            }

            string name = "";

            if (!Exclusive)
            {
                name = _names[0];
            }
            else
            {
                name = _names[1];
            }

            if (Shifted)
            {
                name += "s";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            if (!Exclusive && _operands[0].Value == 0 && _operands[1].Value == 0 && _operands[2].Value == 0)
            {
                return "";
            }

            return base.GetFormattedOperands();
        }
    }

    //  andi., andis.
    public class PPCAndi : PPCOpCode
    {
        public bool Shifted => Operation == PPCMnemonic.andis_D;

        internal PPCAndi(uint value) : base(value)
        {
            _names.Add("andi.");
            _names.Add("andis.");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value
        }

        public override string GetName()
        {
            return _names[0] + (Shifted ? "s" : "");
        }
    }

    //  rldicl
    public class PPCRldicl : PPCOpCode
    {
        public PPCRldicl(uint value) : base(value)
        {
            _names.Add("rldicl");
        }

        public override string GetFormattedOperands()
        {
            return $"0x{this:X8}";
        }
    }

    // cmpw, cmpd
    public class PPCCmp : PPCOpCode
    {
        private bool IsDouble => _operands[1].Value != 0;

        internal PPCCmp(uint value) : base(value)
        {
            _names.Add("cmpw");
            _names.Add("cmpd");
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 23, 0x7)); //  [0] Condition Register
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 22, 0x1));           //  [-] Undefined
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1));       //  [1] Is Double
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [2] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [3] Right 
        }

        public override string GetName()
        {
            if (IsDouble)
            {
                return _names[1];
            }

            return _names[0];
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (_operands[0].Value == 0)
            {
                return string.Format("{2}, {3}", formatted);
            }

            return string.Format("{0}, {2}, {3}", formatted);
        }
    }

    //  subc
    public class PPCSubc : PPCOpCode
    {
        public bool SetCr => (this & 0x00000001) != 0;

        internal PPCSubc(uint value)
            : base(value)
        {
            _names.Add("subc");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register 1
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Right Register 2
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));             //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  slw
    public class PPCSlw : PPCOpCode
    {
        public bool SetCr => (this & 0x00000001) != 0;

        internal PPCSlw(uint value)
            : base(value)
        {
            _names.Add("slw");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register 1
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Right Register 2
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));             //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  and
    public class PPCAnd : PPCOpCode
    {
        public bool SetCr => (this & 0x00000001) != 0;

        internal PPCAnd(uint value) : base(value)
        {
            _names.Add("and");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register 1
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Right Register 2
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));             //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  cmplw, cmpld
    public class PPCCmpl : PPCOpCode
    {
        private bool IsDouble => (this & 0x00200000) != 0;

        internal PPCCmpl(uint value)
            : base(value)
        {
            _names.Add("cmplw");
            _names.Add("cmpld");
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 23, 0x7)); //  [0] Condition Register
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 22, 0x1));           //  [-] Undefined
            _operands.Add(new PPCOperand(this, OperandType.VAL, 21, 0x1));       //  [1] Is Double
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [2] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [3] Right 
        }

        public override string GetName()
        {
            if (IsDouble)
            {
                return _names[1];
            }

            return _names[0];
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            if (_operands[0].Value == 0)
            {
                return string.Format("{2}, {3}", formatted);
            }

            return string.Format("{0}, {2}, {3}", formatted);
        }
    }

    //  sub
    public class PPCSub : PPCOpCode
    {
        public bool SetCr => (this & 0x00000001) != 0;

        internal PPCSub(uint value)
            : base(value)
        {
            _names.Add("sub");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register 1
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Right Register 2
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }


    //  cntlzw
    public class PPCCntlzw : PPCOpCode
    {
        public bool SetCr => (this & 0x00000001) != 0;

        internal PPCCntlzw(uint value)
            : base(value)
        {
            _names.Add("cntlzw");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right 
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  cntlzd
    public class PPCCntlzd : PPCOpCode
    {
        public bool SetCr => (this & 0x00000001) != 0;

        internal PPCCntlzd(uint value)
            : base(value)
        {
            _names.Add("cntlzd");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right 
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  add
    public class PPCAdd : PPCOpCode
    {
        public bool SetCr => (this & 0x00000001) != 0;

        internal PPCAdd(uint value)
            : base(value)
        {
            _names.Add("add");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right Register 1
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 11, 0x1F)); //  [2] Right Register 2
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  mfspr
    public class PPCMfspr : PPCOpCode
    {
        internal PPCMfspr(uint value) : base(value)
        {
            _names.Add("mfspr");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));  //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.SREGISTER, 16, 0x1F)); //  [1] Right Register
        }
    }

    //  mtspr
    public class PPCMtspr : PPCOpCode
    {
        public PPCMtspr(uint value) : base(value)
        {
            _names.Add("mtspr");
            _operands.Add(new PPCOperand(this, OperandType.SREGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));  //  [1] Right Register
        }

        public override string GetName()
        {
            return "mt" + _operands[0].GetFormatted();
        }

        public override string GetFormattedOperands()
        {
            return _operands[1].GetFormatted();
        }
    }

    //  extsh
    public class PPCExtsh : PPCOpCode
    {
        public bool SetCr
        {
            get => _data[0];
            set => _data[0] = value;
        }

        internal PPCExtsh(uint value)
            : base(value)
        {
            _names.Add("extsh");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right 
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  extsb
    public class PPCExtsb : PPCOpCode
    {
        public bool SetCr
        {
            get => _data[0];
            set => _data[0] = value;
        }

        internal PPCExtsb(uint value)
            : base(value)
        {
            _names.Add("extsb");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F)); //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F)); //  [1] Right 
            //_Operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string GetName()
        {
            return _names[0] + (SetCr ? "." : "");
        }
    }

    //  lwz, lwzu, lbz, lbzu, lhz, lhzu, lha, lhau, ld
    public class PPCLwz : PPCOpCode
    {
        internal PPCLwz(uint value) : base(value)
        {
            _names.Add("lwz");
            _names.Add("lbz");
            _names.Add("lha");
            _names.Add("lhz");
            _names.Add("lmw");
            _names.Add("ld");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        public bool Update => Operation == PPCMnemonic.lwzu || Operation == PPCMnemonic.lbzu ||
                              Operation == PPCMnemonic.lhzu || Operation == PPCMnemonic.lhau;

        public bool LoadWord => Operation == PPCMnemonic.lwz || Operation == PPCMnemonic.lwzu;
        public bool LoadHalf => Operation == PPCMnemonic.lhz || Operation == PPCMnemonic.lhzu;
        public bool LoadHalfAlgebraic => Operation == PPCMnemonic.lha || Operation == PPCMnemonic.lhau;
        public bool LoadByte => Operation == PPCMnemonic.lbz || Operation == PPCMnemonic.lbzu;

        public bool LoadMultiple => Operation == PPCMnemonic.lmw;
        public bool LoadDouble => Operation == PPCMnemonic.ld;

        public override string GetName()
        {
            if (LoadDouble)
            {
                return _names[5];
            }

            if (LoadMultiple)
            {
                return _names[4];
            }

            string name = "";
            if (LoadWord)
            {
                name = _names[0];
            }
            else if (LoadByte)
            {
                name = _names[1];
            }
            else if (LoadHalfAlgebraic)
            {
                name = _names[2];
            }
            else
            {
                name = _names[3];
            }

            if (Update)
            {
                name += "u";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            return string.Format("{0}, {2}({1})", formatted);
        }
    }

    //  stw, stwu, stb, stbu, sth, sthu, std
    public class PPCStw : PPCOpCode
    {
        internal PPCStw(uint value) : base(value)
        {
            _names.Add("stw");
            _names.Add("stb");
            _names.Add("sth");
            _names.Add("stmw");
            _names.Add("std");
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 21, 0x1F));     //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        public bool Update => Operation == PPCMnemonic.stwu || Operation == PPCMnemonic.sthu ||
                              Operation == PPCMnemonic.stbu;

        public bool StoreWord => Operation == PPCMnemonic.stw || Operation == PPCMnemonic.stwu;
        public bool StoreHalf => Operation == PPCMnemonic.sth || Operation == PPCMnemonic.sthu;
        public bool StoreByte => Operation == PPCMnemonic.stb || Operation == PPCMnemonic.stbu;

        public bool StoreMultiple => Operation == PPCMnemonic.stmw;
        public bool StoreDouble => Operation == PPCMnemonic.std;

        public override string GetName()
        {
            if (StoreDouble)
            {
                return _names[4];
            }

            if (StoreMultiple)
            {
                return _names[3];
            }

            string name = "";
            if (StoreWord)
            {
                name = _names[0];
            }
            else if (StoreByte)
            {
                name = _names[1];
            }
            else if (StoreHalf)
            {
                name = _names[2];
            }

            if (Update)
            {
                name += "u";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            return string.Format("{0}, {2}({1})", formatted);
        }
    }

    //  lfs, lfsu, lfd, lfdu
    public class PPCLfs : PPCOpCode
    {
        internal PPCLfs(uint value) : base(value)
        {
            _names.Add("lfs");
            _names.Add("lfd");
            _operands.Add(new PPCOperand(this, OperandType.FREGISTER, 21, 0x1F));    //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        private bool Update => Operation == PPCMnemonic.lfsu || Operation == PPCMnemonic.lfdu;

        private bool IsDouble => Operation == PPCMnemonic.lfd || Operation == PPCMnemonic.lfdu;

        public override string GetName()
        {
            string name = _names[0];

            if (IsDouble)
            {
                name = _names[1];
            }

            if (Update)
            {
                name += "u";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            return string.Format("{0}, {2}({1})", formatted);
        }
    }

    //  stfs, stfsu, stfd, stfdu
    public class PPCStfs : PPCOpCode
    {
        internal PPCStfs(uint value) : base(value)
        {
            _names.Add("stfs");
            _names.Add("stfd");
            _operands.Add(new PPCOperand(this, OperandType.FREGISTER, 21, 0x1F));    //  [0] Left Register
            _operands.Add(new PPCOperand(this, OperandType.REGISTER, 16, 0x1F));     //  [1] Right Register
            _operands.Add(new PPCOperand(this, OperandType.VAL, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        private bool Update => Operation == PPCMnemonic.stfsu || Operation == PPCMnemonic.stfdu;

        private bool IsDouble => Operation == PPCMnemonic.stfd || Operation == PPCMnemonic.stfdu;

        public override string GetName()
        {
            string name = _names[0];

            if (IsDouble)
            {
                name = _names[1];
            }

            if (Update)
            {
                name += "u";
            }

            return name;
        }

        public override string GetFormattedOperands()
        {
            string[] formatted = _operands.Select(x => x.GetFormatted()).ToArray();

            return string.Format("{0}, {2}({1})", formatted);
        }
    }

    //  fcmpu
    public class OpFcmpu : PPCOpCode
    {
        internal OpFcmpu(uint value) : base(value)
        {
            _names.Add("fcmpu");
            _operands.Add(new PPCOperand(this, OperandType.CREGISTER, 23, 0x7)); //  [0] Condition Register
            //_Operands.Add(new PPCOperand(OperandType.VAL, 21, 0x3));                 //  [-] Unknown
            _operands.Add(new PPCOperand(this, OperandType.FREGISTER, 16, 0x1F)); //  [1] Left Register
            _operands.Add(new PPCOperand(this, OperandType.FREGISTER, 11, 0x1F)); //  [2] Right Register
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.PowerPcAssembly
{
    //  .word
    public unsafe class OpWord : PPCOpCode
    {
        public OpWord(uint value) : base(value) { }
        public override string FormOps() { return "0x" + PPCFormat.Word(this); }
    }

    //  vaddubm
    public unsafe class OpVaddubm : PPCOpCode
    {
        public OpVaddubm(uint value) : base(value) { }
        public override string FormOps() { return "0x" + PPCFormat.Word(this); }
    }

    //  mulli
    public unsafe class OpMulli : PPCOpCode
    {
        public OpMulli(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value
        }
    }

    //  subfic
    public unsafe class OpSubfic : PPCOpCode
    {
        public OpSubfic(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value            
        }
    }

    //  cmplwi, cmpldi
    public unsafe class OpCmpli : PPCOpCode
    {
        public OpCmpli(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.ConditionRegister, 23, 0x7));       //  [0] Condition Register
            //data.Add(new OperandLocator(OperandType.VAL, 22, 0x1));                    //  [-] Unknown
            //data.Add(new OperandLocator(OperandType.VAL, 21, 0x1));                    //  [-] Is Double
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));               //  [1] Left Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000));         //  [2] Immediate Value
        }

        public override string FormName()
        {
            if (IsDouble)
                return _names[1];

            return _names[0];
        }

        bool IsDouble { get { return (this & 0x00200000) != 0; } }
    }

    //  cmpwi, cmpdi
    public unsafe class OpCmpi : PPCOpCode
    {
        public OpCmpi(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.ConditionRegister, 23, 0x7));       //  [0] Condition Register
            //data.Add(new OperandLocator(OperandType.VAL, 22, 0x1));                    //  [-] Unknown
            //data.Add(new OperandLocator(OperandType.VAL, 21, 0x1));                    //  [-] Is Double
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));               //  [1] Left Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000));         //  [2] Immediate Value
        }

        public override string FormName()
        {
            if (IsDouble)
                return _names[1];

            return _names[0];
        }

        bool IsDouble { get { return (this & 0x00200000) != 0; } }
    }

    //  addic, subic, addic., subic.
    public unsafe class OpAddic : PPCOpCode
    {
        public OpAddic(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value      
        }

        bool SetCr { get { return (this & 0xFC000000) == 0x34000000; } }

        public override string FormName()
        {
            if (_data[2] < 0)
                return _names[1];

            return _names[0];
        }
    }

    //  addi, addis, subi, subis, li, lis
    public unsafe class OpAddi : PPCOpCode
    {
        public OpAddi(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        bool shifted { get { return (this & 0xFC000000) == 0x3C000000; } }

        public override string FormName()
        {
            if (_data[1] != 0)
                if (_data[2] < 0)
                    return _names[1];
                else
                    return _names[0];

            return _names[Math.Min(2, _names.Count - 1)];
        }

        public override string FormOps()
        {
            if (_data[1] == 0)
                return _data.Formal(0) + "," + _data.Formal(2);
                //return _data.Formal(0) + " = " + _data.Formal(2);

            //if (_data.Formal(0) == _data.Formal(1))
            //{
            //    string s = _data.Formal(2);
            //    if (s.StartsWith("-"))
            //        return _data.Formal(0) + " -= " + s;
            //    else
            //        return _data.Formal(0) + " += " + s;
            //}
            //else
            //{
            //    string s = _data.Formal(2);
            //    if (s.StartsWith("-"))
            //        return _data.Formal(0) + " = " + _data.Formal(1) + " - " + s.Substring(1);
            //    else
            //        return _data.Formal(0) + " = " + _data.Formal(1) + " + " + s;
            //}

            return base.FormOps();
        }
    }

    #region Branches

    public abstract unsafe class BranchOpcode : PPCOpCode
    {
        protected BranchOpcode(uint value) : base(value) { }

        protected VoidPtr AbsoluteAddr = 0x7FFFCF00;
        protected uint MaxDistance = 0;

        protected bool _badDest = false;
        public bool BadDestination { get { return _badDest; } }

        public int Offset { get { return _data[0]; } set { _data[0] = value; } }

        protected VoidPtr _dest = 0;
        public VoidPtr Destination
        {
            get
            {
                uint baseAddr = Address;
                if (Absolute) baseAddr = AbsoluteAddr;

                if (BadDestination)
                    return _dest;
                else
                    return (uint)(baseAddr + _data[0]);
            }
            set
            {
                uint baseAddr = Address;
                if (Absolute) baseAddr = AbsoluteAddr;

                _dest = (VoidPtr)(((uint)value).RoundDown(4));
                _badDest = false;

                if (_dest < baseAddr + MaxDistance && _dest >= baseAddr - MaxDistance)
                    _data[0] = (int)_dest - (int)baseAddr;
                else
                {
                    _badDest = true;
                    _data[0] = 0;
                }
            }
        }

        public bool Link { get { return (this & 0x1) != 0; } }
        public bool Absolute { get { return (this & 0x2) != 0; } }
    }

    //  bc, bdnz, bdnzf, bdnzt, bdz, bdzf, bdzt, beq, bne, bgt, blt, bge, ble
    public unsafe class OpBc : BranchOpcode
    {
        public OpBc(uint value) : base(value)
        {
            //data.Add(new OperandLocator(OperandType.VAL, 25, 0x1));               //  [-] Unknown
            //data.Add(new OperandLocator(OperandType.VAL, 24, 0x1));               //  [-] Not
            //data.Add(new OperandLocator(OperandType.VAL, 23, 0x1));               //  [-] Value Compare
            //data.Add(new OperandLocator(OperandType.VAL, 22, 0x1));               //  [-] cr Conditional Not
            //data.Add(new OperandLocator(OperandType.VAL, 21, 0x1));               //  [-] Reverse
            _data.Add(new OperandLocator(OperandType.Offset, 0, 0x7FFC, 0x8000));   //  [0] Offset
            _data.Add(new OperandLocator(OperandType.ConditionRegister, 18, 0x7));  //  [1] Condition Register
            //data.Add(new OperandLocator(OperandType.VAL, 16, 0x3));               //  [-] Value Compare Type
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x1));                //  [-] Link
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x2));                //  [-] Absolute

            MaxDistance = 0x8000;
        }

        bool Unknown { get { return (this & 0x2000000) != 0; } }
        bool Not { get { return (this & 0x1000000) != 0; } }
        bool ValCompare { get { return (this & 0x800000) != 0; } }
        bool CRCNot { get { return (this & 0x400000) != 0; } }
        bool Reverse { get { return (this & 0x200000) != 0; } }
        int ValCmpType { get { return ((int)(uint)this >> 16) & 0x3; } }

        public override string FormName()
        {
            if (Unknown)
                return base.FormName();

            if (!ValCompare)
                return _names[1 + (CRCNot ? 1 : 0)] + (Not ? "t" : "f") + (_data[0] < 0 ^ Reverse ? "+" : "-");
            else
                return _names[3 + (ValCmpType * 2) + (Not ? 1 : 0)] + (_data[0] < 0 ^ Reverse ? "+" : "-");
        }

        public override string FormOps()
        {
            if (Unknown)
                return "0x" + PPCFormat.Word(this);

            if (/*Address == 0 && */!Absolute)
                return (_data.Formal(1) != "" ? _data.Formal(1) + "," : "") + PPCFormat.SOffset((int)Destination);

            return (_data.Formal(1) != "" ? _data.Formal(1) + "," : "") + PPCFormat.Offset(Destination);
        }
    }

    //  b, bl, ba, bla
    public unsafe class OpB : BranchOpcode
    {
        public OpB(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Offset, 0, 0x3FFFFFC, 0x2000000));    //  [0] Offset
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x1));                       //  [-] Link
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x2));                       //  [-] Absolute

            MaxDistance = 0x2000000;
        }

        public override string FormName()
        {
            return base.FormName() + (Link ? "l" : "") + (Absolute ? "a" : "");
        }

        public override string FormOps()
        {
            if (/*Address == 0 && */!Absolute)
                return PPCFormat.SOffset((int)Destination);

            return PPCFormat.Offset(Destination);
        }
    }

    #endregion

    //  blr
    public unsafe class OpBlr : PPCOpCode
    {
        public OpBlr(uint value) : base(value) { }
    }

    //  bctr
    public unsafe class OpBctr : PPCOpCode
    {
        public OpBctr(uint value) : base(value) { }
    }

    //  rlwimi
    public unsafe class OpRlwimi : PPCOpCode
    {
        public OpRlwimi(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       // [1] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       // [0] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 11, 0x1F));          // [2] Roll Left
            _data.Add(new OperandLocator(OperandType.Value, 6, 0x1F));           // [3] NAND Mask
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x3E));           // [4] AND Mask
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x1));             // [-] Set Cr
        }
    }

    //  rlwinm
    public unsafe class OpRlwinm : PPCOpCode
    {
        public OpRlwinm(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       // [1] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       // [0] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 11, 0x1F));          // [2] Roll Left
            _data.Add(new OperandLocator(OperandType.Value, 6, 0x1F));           // [3] NAND Mask
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x3E));           // [4] AND Mask
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x1));             // [-] Set Cr
        }
    }

    //  rlwmn
    public unsafe class OpRlwmn : PPCOpCode
    {
        public OpRlwmn(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       // [1] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       // [0] Right Register
            _data.Add(new OperandLocator(OperandType.Register, 11, 0x1F));       // [2] Right Register 2
            _data.Add(new OperandLocator(OperandType.Value, 6, 0x1F));           // [3] NAND Mask
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x3E));           // [4] AND Mask
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x1));             // [-] Set Cr      
        }
    }

    //  or, xor, mr
    public unsafe class OpOr : PPCOpCode
    {
        public OpOr(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));        //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));        //  [1] Right Register 1
            _data.Add(new OperandLocator(OperandType.Register, 11, 0x1F));        //  [2] Right Register 2
        }

        bool Exclusive { get { return (this & 0x7FE) == 0xA78; } }

        public override string FormName()
        {
            if (!Exclusive)
                if (_data[1] == _data[2])
                    return _names[1];

            return base.FormName();
        }

        public override string FormOps()
        {
            if (!Exclusive)
                if (_data[1] == _data[2])
                    return _data.Formal(0) + "," + _data.Formal(1);

            return base.FormOps();
        }
    }

    //  ori, oris, xori, xoris, nop
    public unsafe class OpOri : PPCOpCode
    {
        public OpOri(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value
        }

        bool Shifted { get { return (this & 0xFC000000) == PPCInfo.oris || (this & 0xFC000000) == PPCInfo.xoris; } }
        bool Exclusive { get { return (this & 0xFC000000) == PPCInfo.xori || (this & 0xFC000000) == PPCInfo.xoris; } }

        public override string FormName()
        {
            if (!Exclusive)
                if (_data[0] == 0 && _data[1] == 0 && _data[2] == 0)
                    return _names[1];

            return base.FormName();
        }

        public override string FormOps()
        {
            if (!Exclusive)
                if (_data[0] == 0 && _data[1] == 0 && _data[2] == 0)
                    return "";

            return base.FormOps();
        }
    }

    //  andi., andis.
    public unsafe class OpAndiSCR : PPCOpCode
    {
        public OpAndiSCR(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value
        }

        bool Shifted { get { return (this & 0xFC000000) == PPCInfo.andis_D; } }
    }

    //  rldicl
    public unsafe class OpRldicl : PPCOpCode
    {
        public OpRldicl(uint value) : base(value) { }
        public override string FormOps() { return "0x" + PPCFormat.Word(this); }
    }

    //  mfspr
    public unsafe class OpMfspr : PPCOpCode
    {
        public OpMfspr(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));             //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.SpecialRegister, 16, 0x1F));      //  [1] Right Register
        }

        //public override string FormOps()
        //{
        //    return _data.Formal(0) + " = " + _data.Formal(1);
        //}
    }

    //  mtspr
    public unsafe class OpMtspr : PPCOpCode
    {
        public OpMtspr(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.SpecialRegister, 16, 0x1F));    //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));           //  [1] Right Register
        }

        //public override string FormOps()
        //{
        //    return _data.Formal(0) + " = " + _data.Formal(1);
        //}
    }

    //  extsh
    public unsafe class OpExtsh : PPCOpCode
    {
        public bool SetCr { get { return (this & 0x00000001) != 0; } }

        public OpExtsh(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [1] Right 
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string FormName()
        {
            if (!SetCr)
                return _names[0];
            else
                return _names[1];
        }
    }

    //  extsb
    public unsafe class OpExtsb : PPCOpCode
    {
        public bool SetCr { get { return (this & 0x00000001) != 0; } }

        public OpExtsb(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [1] Right 
            //data.Add(new OperandLocator(OperandType.VAL, 0, 0x1));            //  [-] Set Cr
        }

        public override string FormName()
        {
            if (!SetCr)
                return _names[0];
            else
                return _names[1];
        }
    }

    //  lwz, lwzu, lbz, lbzu, lhz, lhzu, lha, lhau, ld
    public unsafe class OpLwz : PPCOpCode
    {
        public OpLwz(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        bool Update { get { return (this & 0xFC000000) == PPCInfo.lwzu || (this & 0xFC000000) == PPCInfo.lbzu || (this & 0xFC000000) == PPCInfo.lhzu || (this & 0xFC000000) == PPCInfo.lhau; } }
        bool Multiple { get { return (this & 0xFC000000) == PPCInfo.lmw; } }
        bool Algebraic { get { return (this & 0xFC000000) == PPCInfo.lha || (this & 0xFC000000) == PPCInfo.lhau; } }
        
        DataSize Size
        {
            get
            {
                switch (this & 0xFC000000)
                {
                    case PPCInfo.lwz:
                    case PPCInfo.lwzu:
                    case PPCInfo.lmw:
                        return DataSize.Word;
                    case PPCInfo.lbz:
                    case PPCInfo.lbzu:
                        return DataSize.Byte;
                    case PPCInfo.lhz:
                    case PPCInfo.lhzu:
                    case PPCInfo.lha:
                    case PPCInfo.lhau:
                        return DataSize.Half;
                    case PPCInfo.ld:
                        return DataSize.Double;
                    default:
                        return DataSize.Undefined;
                }
            }
        }

        public override string FormOps()
        {
            return _data.Formal(0) + "," + _data.Formal(2) + "(" + _data.Formal(1) + ")";
            //return _data.Formal(0) + " = *(" + _data.Formal(1) + " +" + (Update ? "= " : " ") + _data.Formal(2) + ")";
        }
    }

    //  stw, stwu, stb, stbu, sth, sthu, std
    public unsafe class OpStw : PPCOpCode
    {
        public OpStw(uint value)
            : base(value)
        {
            _data.Add(new OperandLocator(OperandType.Register, 21, 0x1F));       //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        bool Update { get { return (this & 0xFC000000) == PPCInfo.stwu || (this & 0xFC000000) == PPCInfo.stbu || (this & 0xFC000000) == PPCInfo.sthu; } }
        bool Multiple { get { return (this & 0xFC000000) == PPCInfo.stmw; } }
        DataSize Size
        {
            get
            {
                switch (this & 0xFC000000)
                {
                    case PPCInfo.stw:
                    case PPCInfo.stwu:
                    case PPCInfo.stmw:
                        return DataSize.Word;
                    case PPCInfo.stb:
                    case PPCInfo.stbu:
                        return DataSize.Byte;
                    case PPCInfo.sth:
                    case PPCInfo.sthu:
                        return DataSize.Half;
                    case PPCInfo.std:
                        return DataSize.Double;
                    default:
                        return DataSize.Undefined;
                }
            }
        }

        public override string FormOps()
        {
            return _data.Formal(0) + "," + _data.Formal(2) + "(" + _data.Formal(1) + ")";
            //if (_data.Formal(2) == "0" || _data.Formal(2) == "-0")
            //    return _data.Formal(1) + " = " + _data.Formal(0);
            //else
            //{
            //    string s = _data.Formal(2);
            //    if (s.StartsWith("-"))
            //        return "*(" + _data.Formal(1) + " - " + s.Substring(1) + ") = " + _data.Formal(0);
            //    else
            //        return "*(" + _data.Formal(1) + " + " + s + ") = " + _data.Formal(0);
            //}
        }
    }

    //  lfs, lfsu, lfd, lfdu
    public unsafe class OpLfs : PPCOpCode
    {
        public OpLfs(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.FloatRegister, 21, 0x1F));      //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));           //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000));     //  [2] Immediate Value 
        }

        bool Update { get { return (this & 0xFC0000000) == PPCInfo.lfsu || (this & 0xFC000000) == PPCInfo.lfdu; } }
        bool IsDouble { get { return (this & 0xFC000000) == PPCInfo.lfd || (this & 0xFC000000) == PPCInfo.lfdu; } }

        public override string FormOps()
        {
            return _data.Formal(0) + "," + _data.Formal(2) + "(" + _data.Formal(1) + ")";
        }
    }

    //  stfs, stfsu, stfd, stfdu
    public unsafe class OpStfs : PPCOpCode
    {
        public OpStfs(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.FloatRegister, 21, 0x1F));  //  [0] Left Register
            _data.Add(new OperandLocator(OperandType.Register, 16, 0x1F));       //  [1] Right Register
            _data.Add(new OperandLocator(OperandType.Value, 0, 0x7FFF, 0x8000)); //  [2] Immediate Value 
        }

        bool Update { get { return (this & 0xFC000000) == PPCInfo.stfsu || (this & 0xFC000000) == PPCInfo.stfdu; } }
        bool IsDouble { get { return (this & 0xFC000000) == PPCInfo.stfd || (this & 0xFC000000) == PPCInfo.stfdu; } }

        public override string FormOps()
        {
            return _data.Formal(0) + "," + _data.Formal(2) + "(" + _data.Formal(1) + ")";
        }
    }

    //  fcmpu
    public unsafe class OpFcmpu : PPCOpCode
    {
        public OpFcmpu(uint value) : base(value)
        {
            _data.Add(new OperandLocator(OperandType.ConditionRegister, 23, 0x7));       //  [0] Condition Register
            //data.Add(new OperandLocator(OperandType.VAL, 21, 0x3));                    //  [-] Unknown
            _data.Add(new OperandLocator(OperandType.FloatRegister, 16, 0x1F));          //  [1] Left Register
            _data.Add(new OperandLocator(OperandType.FloatRegister, 11, 0x1F));          //  [2] Right Register
        }
    }

    public enum DataSize { Byte, Half, Word, Double, Undefined }
    public enum OperandType { Value, UnsignedValue, Offset, UnsignedOffset, Register, FloatRegister, ConditionRegister, VRegister, SpecialRegister }
    public struct OperandLocator
    {
        public OperandLocator(OperandType type, int shift, uint bits) : this(type, shift, bits, 0) { }
        public OperandLocator(OperandType type, int shift, uint bits, uint neg_bit)
        {
            _type = type;
            _shift = shift;
            _bits = bits & ~(uint)neg_bit;
            _negBit = neg_bit;
        }

        public OperandType _type;
        public int _shift;

        public uint _bits;
        public uint _negBit;
    }
}
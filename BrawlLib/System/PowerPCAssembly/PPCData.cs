using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.PowerPcAssembly
{
    public class PPCData
    {
        protected List<OperandLocator> operands = new List<OperandLocator>();

        public PPCData(uint value) { _value = value; }
        public Bin32 _value = 0;

        public void Add(OperandLocator locator) { operands.Add(locator); }
        public int Count { get { return operands.Count; } }
        public int this[int i]
        {
            get
            {
                if (i >= operands.Count())
                    throw new IndexOutOfRangeException();

                if ((_value & operands[i]._negBit) == 0)
                    return (int)((_value >> operands[i]._shift) & operands[i]._bits);
                else
                    return (int)((_value >> operands[i]._shift) & operands[i]._bits) - (int)operands[i]._negBit;
            }
            set
            {
                if (i >= operands.Count())
                    throw new IndexOutOfRangeException();

                _value &= ~(operands[i]._bits << operands[i]._shift);
                if (value > 0)
                {
                    _value |= (uint)((value & operands[i]._bits) << operands[i]._shift);
                    _value &= ~(operands[i]._negBit);
                }
                else
                {
                    _value |= (uint)(((value + operands[i]._negBit) & operands[i]._bits) << operands[i]._shift);
                    _value |= (uint)operands[i]._negBit;
                }
            }
        }

        public string Formal(int i)
        {
            switch (operands[i]._type)
            {
                case OperandType.Value:
                    switch (PPCFormat.disassemblerDisplay)
                    {
                        case DisplayType.UnsignedHex: return PPCFormat.Word(this[i]);
                        case DisplayType.Hex: return PPCFormat.SWord(this[i]);
                        default: return this[i].ToString();
                    }
                case OperandType.UnsignedValue: return ((uint)this[i]).ToString();
                case OperandType.Offset: return PPCFormat.SOffset(this[i]);
                case OperandType.UnsignedOffset: return PPCFormat.Offset(this[i]);
                case OperandType.Register: return "r" + this[i].ToString();
                case OperandType.FloatRegister: return "f" + this[i].ToString();
                case OperandType.ConditionRegister: return "cr" + this[i].ToString(); //return (this[i] != 0 ? "cr" + this[i].ToString() : "");
                case OperandType.VRegister: return "v" + this[i].ToString();
                case OperandType.SpecialRegister:
                    switch (this[i])
                    {
                        case 1: return "xer"; //Exception Register
                        case 8: return "lr"; //Link Register
                        case 9: return "ctr"; //Counter Register
                        case 18: return "dsisr";
                        case 19: return "dar";
                        case 22: return "dec";
                        case 25: return "sdr1";
                        case 26: return "srr0";
                        case 27: return "srr1";
                        case 272: return "sprg0";
                        case 273: return "sprg1";
                        case 274: return "sprg2";
                        case 275: return "sprg3";
                        case 282: return "ear";
                        case 284: return "tbl";
                        case 285: return "tbu";
                        case 528: return "ibat0u";
                        case 529: return "ibat0l";
                        case 530: return "ibat1u";
                        case 531: return "ibat1l";
                        case 532: return "ibat2u";
                        case 533: return "ibat2l";
                        case 534: return "ibat3u";
                        case 535: return "ibat3l";
                        case 536: return "dbat0u";
                        case 537: return "dbat0l";
                        case 538: return "dbat1u";
                        case 539: return "dbat1l";
                        case 540: return "dbat2u";
                        case 541: return "dbat2l";
                        case 542: return "dbat3u";
                        case 543: return "dbat3l";
                        default: return "sp" + this[i];
                    }
                default:
                    return "";
            }
        }
    }
}
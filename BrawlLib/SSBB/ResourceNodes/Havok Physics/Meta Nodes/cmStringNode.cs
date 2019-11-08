using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmStringNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 4;
        }

        public string _value;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _value = new string((sbyte*) Data + *(bint*) Data);
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int offset = *(bint*) Data;
            sbyte* strAddr = (sbyte*) address + offset;
            string prev = new string((sbyte*) Data + offset);
            _value.Substring(0, ((prev.Length + 1).Align(0x10) - 1).Clamp(0, _value.Length)).Write(ref strAddr);
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value);
        }
    }
}
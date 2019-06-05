using System;
using System.Collections.Generic;
using System.Xml;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmStringNode : ClassMemberInstanceNode
    {
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

        public override int GetSize()
        {
            return 4;
        }

        public override bool OnInitialize()
        {
            _value = new string((sbyte*) Data + *(bint*) Data);
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int offset = *(bint*) Data;
            var strAddr = (sbyte*) address + offset;
            var prev = new string((sbyte*) Data + offset);
            _value.Substring(0, ((prev.Length + 1).Align(0x10) - 1).Clamp(0, _value.Length)).Write(ref strAddr);
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value);
        }
    }
}
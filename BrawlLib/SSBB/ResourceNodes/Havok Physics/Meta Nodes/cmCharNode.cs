using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmCharNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 1;
        }

        public char _value;

        public char Value
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
            _value = *(char*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(char*) address = _value;
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString());
        }
    }
}
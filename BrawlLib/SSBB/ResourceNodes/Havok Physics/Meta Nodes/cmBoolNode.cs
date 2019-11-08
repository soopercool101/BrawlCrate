using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmBoolNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 1;
        }

        public bool _value;

        public bool Value
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
            _value = *(byte*) Data != 0;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(byte*) address = (byte) (_value ? 1 : 0);
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString().ToLower());
        }
    }
}
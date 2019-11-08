using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmLongNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 8;
        }

        public long _value;

        public long Value
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
            _value = *(blong*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(blong*) address = _value;
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
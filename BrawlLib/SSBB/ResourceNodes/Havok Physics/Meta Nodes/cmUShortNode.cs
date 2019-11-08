using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmUShortNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 2;
        }

        public ushort _value;

        public ushort Value
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
            _value = *(bushort*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(bushort*) address = _value;
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
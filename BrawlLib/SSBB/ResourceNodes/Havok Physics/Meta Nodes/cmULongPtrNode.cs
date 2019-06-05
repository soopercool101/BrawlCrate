using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmULongPtrNode : ClassMemberInstanceNode
    {
        public uint _value;

        public uint Value
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
            _value = *(buint*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(buint*) address = _value;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
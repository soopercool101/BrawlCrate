using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmLongNode : ClassMemberInstanceNode
    {
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

        public override int GetSize()
        {
            return 8;
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

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmSByteNode : ClassMemberInstanceNode
    {
        public sbyte _value;

        public sbyte Value
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
            return 1;
        }

        public override bool OnInitialize()
        {
            _value = *(sbyte*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(sbyte*) address = _value;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
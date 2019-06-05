using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmFloatNode : ClassMemberInstanceNode
    {
        public float _value;

        public float Value
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
            _value = *(bfloat*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(bfloat*) address = _value;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(_value.ToString("0.000000", CultureInfo.InvariantCulture));
        }
    }
}
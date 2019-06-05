using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmMat3Node : ClassMemberInstanceNode
    {
        public bool _isRotationMatrix;

        public Matrix34 _value;
        public bool IsRotationMatrix => _isRotationMatrix;

        [TypeConverter(typeof(Matrix43StringConverter))]
        public Matrix34 Value
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
            return 48;
        }

        public override bool OnInitialize()
        {
            _isRotationMatrix = _memberType == hkClassMember.Type.TYPE_ROTATION;
            _value = *(bMatrix43*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(bMatrix43*) address = _value;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(string.Format("({0} {1} {2})({3} {4} {5})({6} {7} {8})",
                _value._data[0].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[1].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[2].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[4].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[5].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[6].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[8].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[9].ToString("0.000000", CultureInfo.InvariantCulture),
                _value._data[10].ToString("0.000000", CultureInfo.InvariantCulture)));
        }
    }
}
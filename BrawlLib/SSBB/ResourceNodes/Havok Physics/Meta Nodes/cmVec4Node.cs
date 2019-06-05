using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmVec4Node : ClassMemberInstanceNode
    {
        public bool _isQuaternion;

        public Vector4 _value;
        public bool IsQuaternion => _isQuaternion;

        [TypeConverter(typeof(Vector4StringConverter))]
        public Vector4 Value
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
            return 16;
        }

        public override bool OnInitialize()
        {
            _isQuaternion = _memberType == hkClassMember.Type.TYPE_QUATERNION;
            _value = *(BVec4*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(Vector4*) address = _value;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(string.Format("({0} {1} {2} {3})",
                _value._x.ToString("0.000000", CultureInfo.InvariantCulture),
                _value._y.ToString("0.000000", CultureInfo.InvariantCulture),
                _value._z.ToString("0.000000", CultureInfo.InvariantCulture),
                _value._w.ToString("0.000000", CultureInfo.InvariantCulture)));
        }
    }
}
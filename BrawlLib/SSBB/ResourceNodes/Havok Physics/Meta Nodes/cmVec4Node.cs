using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmVec4Node : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 16;
        }

        public bool _isQuaternion;
        public bool IsQuaternion => _isQuaternion;

        public Vector4 _value;

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

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(
                $"({_value._x.ToString("0.000000", CultureInfo.InvariantCulture)} {_value._y.ToString("0.000000", CultureInfo.InvariantCulture)} {_value._z.ToString("0.000000", CultureInfo.InvariantCulture)} {_value._w.ToString("0.000000", CultureInfo.InvariantCulture)})");
        }
    }
}
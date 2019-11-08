using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmMat3Node : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 48;
        }

        public bool _isRotationMatrix;
        public bool IsRotationMatrix => _isRotationMatrix;

        public Matrix34 _value;

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

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            fixed (float* p = _value._data)
            {
                writer.WriteString(
                    $"({p[0].ToString("0.000000", CultureInfo.InvariantCulture)} {p[1].ToString("0.000000", CultureInfo.InvariantCulture)} {p[2].ToString("0.000000", CultureInfo.InvariantCulture)})({p[4].ToString("0.000000", CultureInfo.InvariantCulture)} {p[5].ToString("0.000000", CultureInfo.InvariantCulture)} {p[6].ToString("0.000000", CultureInfo.InvariantCulture)})({p[8].ToString("0.000000", CultureInfo.InvariantCulture)} {p[9].ToString("0.000000", CultureInfo.InvariantCulture)} {p[10].ToString("0.000000", CultureInfo.InvariantCulture)})");
            }
        }
    }
}
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmMat4Node : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 64;
        }

        public bool _isTransform;
        public bool IsTransformMatrix => _isTransform;

        public Matrix _value;

        [TypeConverter(typeof(MatrixStringConverter))]
        public Matrix Value
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
            _isTransform = _memberType == hkClassMember.Type.TYPE_TRANSFORM;
            _value = *(bMatrix*) Data;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(bMatrix*) address = _value;
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            float* p = _value.Data;
            writer.WriteString(
                $"({p[0].ToString("0.000000", CultureInfo.InvariantCulture)} {p[1].ToString("0.000000", CultureInfo.InvariantCulture)} {p[2].ToString("0.000000", CultureInfo.InvariantCulture)} {p[3].ToString("0.000000", CultureInfo.InvariantCulture)})({p[4].ToString("0.000000", CultureInfo.InvariantCulture)} {p[5].ToString("0.000000", CultureInfo.InvariantCulture)} {p[6].ToString("0.000000", CultureInfo.InvariantCulture)} {p[7].ToString("0.000000", CultureInfo.InvariantCulture)})({p[8].ToString("0.000000", CultureInfo.InvariantCulture)} {p[9].ToString("0.000000", CultureInfo.InvariantCulture)} {p[10].ToString("0.000000", CultureInfo.InvariantCulture)} {p[11].ToString("0.000000", CultureInfo.InvariantCulture)})({p[12].ToString("0.000000", CultureInfo.InvariantCulture)} {p[13].ToString("0.000000", CultureInfo.InvariantCulture)} {p[14].ToString("0.000000", CultureInfo.InvariantCulture)} {p[15].ToString("0.000000", CultureInfo.InvariantCulture)})");
        }
    }
}
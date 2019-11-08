using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmQSTransformNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 48;
        }

        public Vector3 _translation;
        public Vector4 _quaternion;
        public Vector3 _scale;

        [Category("Transform")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                SignalPropertyChange();
            }
        }

        [Category("Transform")]
        [TypeConverter(typeof(Vector4StringConverter))]
        public Vector4 Quaternion
        {
            get => _quaternion;
            set
            {
                _quaternion = value;
                SignalPropertyChange();
            }
        }

        [Category("Transform")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            HavokQSTransform* addr = (HavokQSTransform*) Data;
            _translation = addr->_translate;
            _quaternion = addr->_quaternion;
            _scale = addr->_scale;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            HavokQSTransform* addr = (HavokQSTransform*) address;
            addr->_translate = _translation;
            addr->_quaternion = _quaternion;
            addr->_scale = _scale;
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(
                $"({_translation._x.ToString("0.000000", CultureInfo.InvariantCulture)} {_translation._y.ToString("0.000000", CultureInfo.InvariantCulture)} {_translation._z.ToString("0.000000", CultureInfo.InvariantCulture)})({_quaternion._x.ToString("0.000000", CultureInfo.InvariantCulture)} {_quaternion._y.ToString("0.000000", CultureInfo.InvariantCulture)} {_quaternion._z.ToString("0.000000", CultureInfo.InvariantCulture)} {_quaternion._w.ToString("0.000000", CultureInfo.InvariantCulture)})({_scale._x.ToString("0.000000", CultureInfo.InvariantCulture)} {_scale._y.ToString("0.000000", CultureInfo.InvariantCulture)} {_scale._z.ToString("0.000000", CultureInfo.InvariantCulture)})");
        }
    }
}
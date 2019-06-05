using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using BrawlLib.SSBBTypes;
using Enumerable = System.Linq.Enumerable;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmEnumNode : ClassMemberInstanceNode
    {
        private const string Acceptable = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,";

        public bool _bitFlags;

        public Dictionary<string, int> _enums;

        public int _value;
        public bool BitFlags => _bitFlags;

        [TypeConverter(typeof(DropDownListHavokEnum))]
        public string Value
        {
            get
            {
                if (_bitFlags)
                {
                    var display = "";
                    var first = true;
                    foreach (var p in _enums)
                        if ((p.Value & _value) != 0)
                        {
                            if (first)
                                first = false;
                            else
                                display += ", ";

                            display += p.Key;
                        }

                    return display;
                }

                foreach (var p in _enums)
                    if (p.Value == _value)
                        return p.Key;

                return "";
            }
            set
            {
                var entries = new string(Enumerable.Where(value, x => Acceptable.Contains(x)).ToArray()).Split(',');
                if (_bitFlags)
                {
                    var enumValue = 0;
                    foreach (var e in entries)
                        if (_enums.ContainsKey(e))
                            enumValue |= _enums[e];

                    _value = enumValue;
                    SignalPropertyChange();
                }
                else
                {
                    if (entries.Length > 0 && _enums.ContainsKey(entries[0]))
                    {
                        _value = _enums[entries[0]];
                        SignalPropertyChange();
                    }
                }
            }
        }

        public override int GetSize()
        {
            return 0;
        }

        public override bool OnInitialize()
        {
            _bitFlags = _memberType == hkClassMember.Type.TYPE_FLAGS;

            _enums = new Dictionary<string, int>();
            foreach (hkClassEnumEntryNode e in _enumNode.Children) _enums.Add(e._name, e.Value);

            if (((int) _memberFlags & 0x8) != 0)
                _value = *(sbyte*) Data;
            else if (((int) _memberFlags & 0x10) != 0)
                _value = *(bshort*) Data;
            else if (((int) _memberFlags & 0x20) != 0) _value = *(bint*) Data;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (((int) _memberFlags & 0x8) != 0)
                *(sbyte*) address = (sbyte) _value;
            else if (((int) _memberFlags & 0x10) != 0)
                *(bshort*) address = (short) _value;
            else if (((int) _memberFlags & 0x20) != 0) *(bint*) address = _value;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteString(Value.Replace(", ", " "));
        }
    }

    public class DropDownListHavokEnum : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var enumNode = context.Instance as cmEnumNode;
            return new StandardValuesCollection(enumNode._enums.Keys);
        }
    }
}
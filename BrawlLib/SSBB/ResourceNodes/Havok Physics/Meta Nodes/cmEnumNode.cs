using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmEnumNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 0;
        }

        public Dictionary<string, int> _enums;

        public bool _bitFlags;
        public bool BitFlags => _bitFlags;

        public int _value;
        private const string Acceptable = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,";

        [TypeConverter(typeof(DropDownListHavokEnum))]
        public string Value
        {
            get
            {
                if (_bitFlags)
                {
                    string display = "";
                    bool first = true;
                    foreach (KeyValuePair<string, int> p in _enums)
                    {
                        if ((p.Value & _value) != 0)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                display += ", ";
                            }

                            display += p.Key;
                        }
                    }

                    return display;
                }

                foreach (KeyValuePair<string, int> p in _enums)
                {
                    if (p.Value == _value)
                    {
                        return p.Key;
                    }
                }

                return "";
            }
            set
            {
                string[] entries = new string(value.Where(x => Acceptable.Contains(x)).ToArray()).Split(',');
                if (_bitFlags)
                {
                    int enumValue = 0;
                    foreach (string e in entries)
                    {
                        if (_enums.ContainsKey(e))
                        {
                            enumValue |= _enums[e];
                        }
                    }

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

        public override bool OnInitialize()
        {
            _bitFlags = _memberType == hkClassMember.Type.TYPE_FLAGS;

            _enums = new Dictionary<string, int>();
            if (_enumNode != null)
            {
                foreach (hkClassEnumEntryNode e in _enumNode.Children)
                {
                    _enums.Add(e._name, e.Value);
                }
            }

            if (((int) _memberFlags & 0x8) != 0)
            {
                _value = *(sbyte*) Data;
            }
            else if (((int) _memberFlags & 0x10) != 0)
            {
                _value = *(bshort*) Data;
            }
            else if (((int) _memberFlags & 0x20) != 0)
            {
                _value = *(bint*) Data;
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (((int) _memberFlags & 0x8) != 0)
            {
                *(sbyte*) address = (sbyte) _value;
            }
            else if (((int) _memberFlags & 0x10) != 0)
            {
                *(bshort*) address = (short) _value;
            }
            else if (((int) _memberFlags & 0x20) != 0)
            {
                *(bint*) address = _value;
            }
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
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
            cmEnumNode enumNode = context.Instance as cmEnumNode;
            return new StandardValuesCollection(enumNode._enums.Keys);
        }
    }
}
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class hkClassNode : HavokClassNode
    {
        internal hkClass* Header => (hkClass*) WorkingUncompressed.Address;

        public List<hkClassNode> _inheritance;

        private int _interfaceCount, _enumCount, _memberCount, _version, _size, _flags, _defaultsPtr;
        private string _parentName;

        [Category("hkClass Node")]
        public string Inheritance =>
            _inheritance == null ? ClassName : string.Join(", ", _inheritance.Select(x => x.Name));

        [Category("hkClass Node")] public string ParentClass => _parentName;
        [Category("hkClass Node")] public int InterfaceCount => _interfaceCount;
        [Category("hkClass Node")] public int EnumCount => _enumCount;
        [Category("hkClass Node")] public int MemberCount => _memberCount;
        [Category("hkClass Node")] public int Version => _version;
        [Category("hkClass Node")] public int Size => _size;
        [Category("hkClass Node")] public int Flags => _flags;
        [Category("hkClass Node")] public int DefaultsPtr => _defaultsPtr;

        public override bool OnInitialize()
        {
            if (Header->_namePtr > 0)
            {
                _name = new string((sbyte*) Header->_namePtr.OffsetAddress);
            }

            _interfaceCount = Header->_interfaceCount;
            _enumCount = Header->_enumCount;
            _memberCount = Header->_propertyCount;
            _version = Header->_version;
            _size = Header->_size;
            _flags = Header->_flags;
            _defaultsPtr = Header->_defaultsPtr;

            if (Header->_parentPtr != 0)
            {
                hkClass* parent = (hkClass*) Header->_parentPtr.OffsetAddress;
                _parentName = new string((sbyte*) parent->_namePtr.OffsetAddress);
            }

            int attribCount = 0;
            if (Header->_attribPtr != 0)
            {
                hkCustomAttributes* attribHeader = (hkCustomAttributes*) Header->_attribPtr.OffsetAddress;
                attribCount = attribHeader->_count;
            }

            if (Header->_defaultsPtr != 0)
            {
                Console.WriteLine(Name + " has defaults");
            }

            SetSizeInternal(48);

            return Header->_propertyCount > 0 || Header->_enumCount > 0 || attribCount > 0;
        }

        private void RecursiveGetInheritance(ref List<hkClassNode> i, hkClassNode node)
        {
            if (node != null)
            {
                foreach (HavokSectionNode section in HavokNode.Children)
                {
                    if (section._classCache != null && section != HavokNode._dataSection)
                    {
                        foreach (HavokClassNode c in section._classCache)
                        {
                            hkClassNode x = c as hkClassNode;
                            if (x != null && x != this && x.ParentClass == node.Name)
                            {
                                _inheritance.Insert(0, x);
                                RecursiveGetInheritance(ref i, x);
                            }
                        }
                    }
                }
            }
        }

        public void GetInheritance()
        {
            _inheritance = new List<hkClassNode>();
            ResourceNode current = this;

            //First, get classes inheriting this one
            TOP:
            bool found = false;
            if (current != null)
            {
                foreach (HavokSectionNode section in HavokNode.Children)
                {
                    if (section._classCache != null && section != HavokNode._dataSection)
                    {
                        foreach (HavokClassNode c in section._classCache)
                        {
                            hkClassNode x = c as hkClassNode;
                            if (x != null && x != this && x.ParentClass == current.Name)
                            {
                                current = x;
                                _inheritance.Insert(0, x);
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }
            }

            if (found)
            {
                goto TOP;
            }

            current = this;

            //Now add this class and the classes it inherits
            while (current is hkClassNode)
            {
                hkClassNode cNode = (hkClassNode) current;

                _inheritance.Add(cNode);

                if (HavokNode.AssignClassParents)
                {
                    current = current.Parent;
                }

                //else if (!string.IsNullOrEmpty(cNode.ParentClass))
                //{
                //    current = null;
                //    HavokClassNode parent = HavokNode.GetClassNode(cNode.ParentClass);
                //    if (parent is hkClassNode)
                //    {
                //        current = parent;
                //    }
                //}
            }

            //Start with the eldest class, added last
            _inheritance.Reverse();
        }

        public override void OnPopulate()
        {
            if (Header->_propertyPtr != 0 && Header->_propertyCount > 0)
            {
                HavokGroupNode memberGroup = new HavokGroupNode {_name = "Members"};
                memberGroup.Parent = this;
                hkClassMember* member = (hkClassMember*) Header->_propertyPtr.OffsetAddress;
                for (int i = 0; i < Header->_propertyCount; i++, member++)
                {
                    new hkClassMemberNode().Initialize(memberGroup, member, 20);
                }
            }

            if (Header->_enumPtr != 0 && Header->_enumCount > 0)
            {
                HavokGroupNode enumGroup = new HavokGroupNode {_name = "Enums"};
                enumGroup.Parent = this;
                hkClassEnum* Enum = (hkClassEnum*) Header->_enumPtr.OffsetAddress;
                for (int i = 0; i < Header->_enumCount; i++, Enum++)
                {
                    new hkClassEnumNode().Initialize(enumGroup, Enum, 16);
                }
            }

            if (Header->_attribPtr != 0)
            {
                hkCustomAttributes* attribHeader = (hkCustomAttributes*) Header->_attribPtr.OffsetAddress;
                if (attribHeader->_count > 0)
                {
                    HavokGroupNode attribGroup = new HavokGroupNode {_name = "Attributes"};
                    attribGroup.Parent = this;
                    HavokAttribute* attrib = (HavokAttribute*) attribHeader->_attribPtr.OffsetAddress;
                    for (int i = 0; i < attribHeader->_count; i++, attrib++)
                    {
                        new HavokAttributeNode().Initialize(attribGroup, attrib, 12);
                    }
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "name");
            writer.WriteString(Name);
            writer.WriteEndElement();

            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "parent");
            writer.WriteString(_parent is HavokClassNode
                ? HavokXML.GetObjectName(classNodes, _parent as HavokClassNode)
                : "null");
            writer.WriteEndElement();

            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "objectSize");
            writer.WriteString(Size.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "numImplementedInterfaces");
            writer.WriteString(InterfaceCount.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            ResourceNode enums = FindChild("Enums", false);
            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "declaredEnums");
            writer.WriteAttributeString("numelements", enums == null ? "0" : enums.Children.Count.ToString());
            {
                if (enums != null)
                {
                    foreach (hkClassEnumNode e in enums.Children)
                    {
                        e.WriteParams(writer, classNodes);
                    }
                }
            }
            writer.WriteEndElement();

            ResourceNode members = FindChild("Members", false);
            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "declaredMembers");
            writer.WriteAttributeString("numelements", members == null ? "0" : members.Children.Count.ToString());
            {
                if (members != null)
                {
                    foreach (hkClassMemberNode e in members.Children)
                    {
                        e.WriteParams(writer, classNodes);
                    }
                }
            }
            writer.WriteEndElement();

            writer.WriteComment(" defaults SERIALIZE_IGNORED ");
            writer.WriteComment(" attributes SERIALIZE_IGNORED ");

            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "flags");
            writer.WriteString(Flags.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "describedVersion");
            writer.WriteString(Version.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }
    }

    public unsafe class HavokAttributeNode : HavokEntryNode
    {
        internal HavokAttribute* Header => (HavokAttribute*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            _name = new string((sbyte*) Header->_namePtr.OffsetAddress);

            return base.OnInitialize();
        }
    }

    public unsafe class hkClassMemberNode : HavokEntryNode
    {
        internal hkClassMember* Header => (hkClassMember*) WorkingUncompressed.Address;

        [Category("Class Member")] public hkClassMember.Type Type => _type;
        [Category("Class Member")] public int OffsetInStruct => _structOffset;
        [Category("Class Member")] public int ArraySize => _arraySize;
        [Category("Class Member")] public hkClassMember.Type SubType => _subType;
        [Category("Class Member")] public string EnumName => _enum;
        [Category("Class Member")] public string ClassName => _class;
        [Category("Class Member")] public hkClassMember.Flags Flags => _flags;

        public hkClassMember.Type _type;
        public hkClassMember.Type _subType;
        public int _structOffset;
        public hkClassMember.Flags _flags;
        public int _arraySize;
        public string _enum, _class;

        public override bool OnInitialize()
        {
            _name = new string((sbyte*) Header->_namePtr.OffsetAddress);

            _type = (hkClassMember.Type) Header->_type;
            _subType = (hkClassMember.Type) Header->_pointedType;
            _structOffset = Header->_structOffset;
            _flags = (hkClassMember.Flags) (ushort) Header->_flags;
            _arraySize = Header->_arraySize;

            if (Header->_enumPtr != 0)
            {
                hkClassEnum* e = (hkClassEnum*) Header->_enumPtr.OffsetAddress;
                _enum = new string((sbyte*) e->_namePtr.OffsetAddress);
            }

            if (Header->_classPtr != 0)
            {
                hkClass* c = (hkClass*) Header->_classPtr.OffsetAddress;
                _class = new string((sbyte*) c->_namePtr.OffsetAddress);
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        public void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteStartElement("hkobject");
            {
                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "name");
                writer.WriteString(Name);
                writer.WriteEndElement();

                HavokClassNode c = string.IsNullOrEmpty(_class) ? null : HavokNode.GetClassNode(_class);
                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "class");
                writer.WriteString(c != null ? HavokXML.GetObjectName(classNodes, c) : "null");
                writer.WriteEndElement();

                HavokClassNode e = string.IsNullOrEmpty(_enum) ? null : HavokNode.GetClassNode(_enum);
                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "enum");
                writer.WriteString(e != null ? HavokXML.GetObjectName(classNodes, e) : "null");
                writer.WriteEndElement();

                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "type");
                string type = Type.ToString();
                if (type == "TYPE_CSTRING")
                {
                    type = "TYPE_STRINGPTR";
                }

                writer.WriteString(type);
                writer.WriteEndElement();

                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "subtype");
                writer.WriteString(SubType.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "cArraySize");
                writer.WriteString(ArraySize.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();

                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "flags");
                writer.WriteString(((int) Flags).ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();

                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "offset");
                writer.WriteString(OffsetInStruct.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();

                writer.WriteComment(" attributes SERIALIZE_IGNORED ");
            }
            writer.WriteEndElement();
        }
    }
}
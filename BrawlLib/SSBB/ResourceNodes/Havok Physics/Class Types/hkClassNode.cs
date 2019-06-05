using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class hkClassNode : HavokClassNode
    {
        public List<hkClassNode> _inheritance;

        internal hkClass* Header => (hkClass*) WorkingUncompressed.Address;

        [Category("hkClass Node")]
        public string Inheritance =>
            _inheritance == null ? ClassName : string.Join(", ", _inheritance.Select(x => x.Name));

        [Category("hkClass Node")] public string ParentClass { get; private set; }

        [Category("hkClass Node")] public int InterfaceCount { get; private set; }

        [Category("hkClass Node")] public int EnumCount { get; private set; }

        [Category("hkClass Node")] public int MemberCount { get; private set; }

        [Category("hkClass Node")] public int Version { get; private set; }

        [Category("hkClass Node")] public int Size { get; private set; }

        [Category("hkClass Node")] public int Flags { get; private set; }

        [Category("hkClass Node")] public int DefaultsPtr { get; private set; }

        public override bool OnInitialize()
        {
            if (Header->_namePtr > 0) _name = new string((sbyte*) Header->_namePtr.OffsetAddress);

            InterfaceCount = Header->_interfaceCount;
            EnumCount = Header->_enumCount;
            MemberCount = Header->_propertyCount;
            Version = Header->_version;
            Size = Header->_size;
            Flags = Header->_flags;
            DefaultsPtr = Header->_defaultsPtr;

            if (Header->_parentPtr != 0)
            {
                var parent = (hkClass*) Header->_parentPtr.OffsetAddress;
                ParentClass = new string((sbyte*) parent->_namePtr.OffsetAddress);
            }

            var attribCount = 0;
            if (Header->_attribPtr != 0)
            {
                var attribHeader = (hkCustomAttributes*) Header->_attribPtr.OffsetAddress;
                attribCount = attribHeader->_count;
            }

            if (Header->_defaultsPtr != 0) Console.WriteLine(Name + " has defaults");

            SetSizeInternal(48);

            return Header->_propertyCount > 0 || Header->_enumCount > 0 || attribCount > 0;
        }

        private void RecursiveGetInheritance(ref List<hkClassNode> i, hkClassNode node)
        {
            if (node != null)
                foreach (HavokSectionNode section in HavokNode.Children)
                    if (section._classCache != null && section != HavokNode._dataSection)
                        foreach (var c in section._classCache)
                        {
                            var x = c as hkClassNode;
                            if (x != null && x != this && x.ParentClass == node.Name)
                            {
                                _inheritance.Insert(0, x);
                                RecursiveGetInheritance(ref i, x);
                            }
                        }
        }

        public void GetInheritance()
        {
            _inheritance = new List<hkClassNode>();
            ResourceNode current = this;

            //First, get classes inheriting this one
            TOP:
            var found = false;
            if (current != null)
                foreach (HavokSectionNode section in HavokNode.Children)
                {
                    if (section._classCache != null && section != HavokNode._dataSection)
                        foreach (var c in section._classCache)
                        {
                            var x = c as hkClassNode;
                            if (x != null && x != this && x.ParentClass == current.Name)
                            {
                                current = x;
                                _inheritance.Insert(0, x);
                                found = true;
                                break;
                            }
                        }

                    if (found) break;
                }

            if (found) goto TOP;

            current = this;

            //Now add this class and the classes it inherits
            while (current != null && current is hkClassNode)
            {
                var cNode = (hkClassNode) current;

                _inheritance.Add(cNode);

                if (HavokNode.AssignClassParents) current = current.Parent;
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
                var memberGroup = new HavokGroupNode {_name = "Members"};
                memberGroup.Parent = this;
                var member = (hkClassMember*) Header->_propertyPtr.OffsetAddress;
                for (var i = 0; i < Header->_propertyCount; i++, member++)
                    new hkClassMemberNode().Initialize(memberGroup, member, 20);
            }

            if (Header->_enumPtr != 0 && Header->_enumCount > 0)
            {
                var enumGroup = new HavokGroupNode {_name = "Enums"};
                enumGroup.Parent = this;
                var Enum = (hkClassEnum*) Header->_enumPtr.OffsetAddress;
                for (var i = 0; i < Header->_enumCount; i++, Enum++)
                    new hkClassEnumNode().Initialize(enumGroup, Enum, 16);
            }

            if (Header->_attribPtr != 0)
            {
                var attribHeader = (hkCustomAttributes*) Header->_attribPtr.OffsetAddress;
                if (attribHeader->_count > 0)
                {
                    var attribGroup = new HavokGroupNode {_name = "Attributes"};
                    attribGroup.Parent = this;
                    var attrib = (HavokAttribute*) attribHeader->_attribPtr.OffsetAddress;
                    for (var i = 0; i < attribHeader->_count; i++, attrib++)
                        new HavokAttributeNode().Initialize(attribGroup, attrib, 12);
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

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
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

            var enums = FindChild("Enums", false);
            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "declaredEnums");
            writer.WriteAttributeString("numelements", enums == null ? "0" : enums.Children.Count.ToString());
            {
                if (enums != null)
                    foreach (hkClassEnumNode e in enums.Children)
                        e.WriteParams(writer, classNodes);
            }
            writer.WriteEndElement();

            var members = FindChild("Members", false);
            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "declaredMembers");
            writer.WriteAttributeString("numelements", members == null ? "0" : members.Children.Count.ToString());
            {
                if (members != null)
                    foreach (hkClassMemberNode e in members.Children)
                        e.WriteParams(writer, classNodes);
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
        public int _arraySize;
        public string _enum, _class;
        public hkClassMember.Flags _flags;
        public int _structOffset;
        public hkClassMember.Type _subType;

        public hkClassMember.Type _type;
        internal hkClassMember* Header => (hkClassMember*) WorkingUncompressed.Address;

        [Category("Class Member")] public hkClassMember.Type Type => _type;

        [Category("Class Member")] public int OffsetInStruct => _structOffset;

        [Category("Class Member")] public int ArraySize => _arraySize;

        [Category("Class Member")] public hkClassMember.Type SubType => _subType;

        [Category("Class Member")] public string EnumName => _enum;

        [Category("Class Member")] public string ClassName => _class;

        [Category("Class Member")] public hkClassMember.Flags Flags => _flags;

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
                var e = (hkClassEnum*) Header->_enumPtr.OffsetAddress;
                _enum = new string((sbyte*) e->_namePtr.OffsetAddress);
            }

            if (Header->_classPtr != 0)
            {
                var c = (hkClass*) Header->_classPtr.OffsetAddress;
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

        public void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteStartElement("hkobject");
            {
                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "name");
                writer.WriteString(Name);
                writer.WriteEndElement();

                var c = string.IsNullOrEmpty(_class) ? null : HavokNode.GetClassNode(_class);
                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "class");
                writer.WriteString(c != null ? HavokXML.GetObjectName(classNodes, c) : "null");
                writer.WriteEndElement();

                var e = string.IsNullOrEmpty(_enum) ? null : HavokNode.GetClassNode(_enum);
                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "enum");
                writer.WriteString(e != null ? HavokXML.GetObjectName(classNodes, e) : "null");
                writer.WriteEndElement();

                writer.WriteStartElement("hkparam");
                writer.WriteAttributeString("name", "type");
                var type = Type.ToString();
                if (type == "TYPE_CSTRING") type = "TYPE_STRINGPTR";

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
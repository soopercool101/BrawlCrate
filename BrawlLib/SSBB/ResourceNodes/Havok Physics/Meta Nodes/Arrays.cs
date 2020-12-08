using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class HavokCommonArrayNode : ClassMemberInstanceNode
    {
        protected VoidPtr dataAddr = null;
        protected int count;

        public override bool OnInitialize()
        {
            return count > 0 && dataAddr != null;
        }

        public override void OnPopulate()
        {
            VoidPtr dataPtr = dataAddr;
            int size = 0;
            for (int i = 0; i < count; i++, dataPtr += size)
            {
                switch (_memberType)
                {
                    case hkClassMember.Type.TYPE_STRUCT:
                        new HavokMetaObjectNode(_classNode)
                            .Initialize(this, dataPtr, size = _classNode == null ? 0 : _classNode.Size);
                        break;
                    case hkClassMember.Type.TYPE_ENUM:
                    case hkClassMember.Type.TYPE_FLAGS:
                        new cmEnumNode
                            {
                                _enumNode = _enumNode
                            }
                            .Initialize(this, dataPtr, size = (int) _memberFlags & 0x3F);
                        break;
                    case hkClassMember.Type.TYPE_ARRAY:
                    case hkClassMember.Type.TYPE_HOMOGENEOUSARRAY:
                    case hkClassMember.Type.TYPE_INPLACEARRAY:
                    case hkClassMember.Type.TYPE_SIMPLEARRAY:
                    case hkClassMember.Type.TYPE_FUNCTIONPOINTER:
                    case hkClassMember.Type.TYPE_ZERO:
                        Console.WriteLine("This shouldn't happen");
                        break;
                    default:
                        ClassMemberInstanceNode instance = HavokMetaObjectNode.TryGetMember(_memberType);
                        if (instance != null)
                        {
                            hkClassMember.Type tempType = _memberType;
                            if (tempType == hkClassMember.Type.TYPE_POINTER)
                            {
                                tempType = hkClassMember.Type.TYPE_STRUCT; //Pointer to class object
                            }

                            instance._isZero = false;
                            instance._name = "Entry" + i;
                            instance._memberType = tempType;
                            instance._memberFlags = _memberFlags;
                            instance._classNode = _classNode;
                            instance._enumNode = _enumNode;

                            instance.Initialize(this, dataPtr, size = instance.GetSize());
                        }

                        break;
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = GetSize();
            foreach (HavokEntryNode e in Children)
            {
                size += e.CalculateSize(force);
            }

            return size;
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteAttributeString("numelements",
                Children.Count.ToString(System.Globalization.CultureInfo.InvariantCulture));

            writer.WriteString(" ");
            foreach (HavokClassNode c in Children)
            {
                //Arrays embed parameters instead of pointing to an object containing them elsewhere

                HavokClassNode c2 = c;
                if (c is cmPointerNode && c.Children.Count > 0)
                {
                    c2 = c.Children[0] as HavokClassNode;
                }

                if (c2 is HavokMetaObjectNode || c2 is hkClassNode)
                {
                    ResourceNode[] nodes = c2.FindChildrenByClassType(null, typeof(HavokMetaObjectNode));
                    if (nodes == null && nodes.Length == 0)
                    {
                        writer.WriteStartElement("hkobject");
                        c2.WriteParams(writer, classNodes);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteString(HavokXML.GetObjectName(classNodes, c2));
                    }
                }
                else if (c2 is cmStringNode)
                {
                    writer.WriteStartElement("hkstring");
                    c2.WriteParams(writer, classNodes);
                    writer.WriteEndElement();
                }
                else
                {
                    c2.WriteParams(writer, classNodes);
                }

                writer.WriteString(" ");
            }
        }
    }

    public unsafe class cmSimpleArrayNode : HavokCommonArrayNode
    {
        public override int GetSize()
        {
            return 8;
        }

        public override bool OnInitialize()
        {
            bint* addr = (bint*) Data;
            dataAddr = addr[0].OffsetAddress;
            count = addr[1];
            return base.OnInitialize();
        }
    }

    public unsafe class cmArrayNode : HavokCommonArrayNode
    {
        public override int GetSize()
        {
            return 12;
        }

        private hkArray.FlagsEnum _flags;
        private int _capacity;

        [Category("Array")] public hkArray.FlagsEnum Flags => _flags;
        [Category("Array")] public int Capacity => _capacity;

        public override bool OnInitialize()
        {
            hkArray* addr = (hkArray*) Data;
            dataAddr = addr->_dataPtr.OffsetAddress;
            count = addr->_count;
            _flags = addr->Flags;
            _capacity = addr->Capacity;
            return base.OnInitialize();
        }
    }
}
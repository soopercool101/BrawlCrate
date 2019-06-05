using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class HavokCommonArrayNode : ClassMemberInstanceNode
    {
        protected int count;
        protected VoidPtr dataAddr = null;

        public override bool OnInitialize()
        {
            return count > 0 && dataAddr != null;
        }

        public override void OnPopulate()
        {
            var dataPtr = dataAddr;
            var size = 0;
            for (var i = 0; i < count; i++, dataPtr += size)
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
                        var instance = HavokMetaObjectNode.TryGetMember(_memberType);
                        if (instance != null)
                        {
                            var tempType = _memberType;
                            if (tempType == hkClassMember.Type.TYPE_POINTER)
                                tempType = hkClassMember.Type.TYPE_STRUCT; //Pointer to class object

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

        public override int OnCalculateSize(bool force)
        {
            var size = GetSize();
            foreach (HavokEntryNode e in Children) size += e.CalculateSize(force);

            return size;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteAttributeString("numelements", Children.Count.ToString(CultureInfo.InvariantCulture));

            writer.WriteString(" ");
            foreach (HavokClassNode c in Children)
            {
                //Arrays embed parameters instead of pointing to an object containing them elsewhere

                var c2 = c;
                if (c is cmPointerNode && c.Children.Count > 0) c2 = c.Children[0] as HavokClassNode;

                if (c2 is HavokMetaObjectNode || c2 is hkClassNode)
                {
                    var nodes = c2.FindChildrenByClassType(null, typeof(HavokMetaObjectNode));
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
            var addr = (bint*) Data;
            dataAddr = addr[0].OffsetAddress;
            count = addr[1];
            return base.OnInitialize();
        }
    }

    public unsafe class cmArrayNode : HavokCommonArrayNode
    {
        [Category("Array")] public hkArray.FlagsEnum Flags { get; private set; }

        [Category("Array")] public int Capacity { get; private set; }

        public override int GetSize()
        {
            return 12;
        }

        public override bool OnInitialize()
        {
            var addr = (hkArray*) Data;
            dataAddr = addr->_dataPtr.OffsetAddress;
            count = addr->_count;
            Flags = addr->Flags;
            Capacity = addr->Capacity;
            return base.OnInitialize();
        }
    }
}
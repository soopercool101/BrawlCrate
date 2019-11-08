using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmPointerNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 4;
        }

        public override bool OnInitialize()
        {
            if (_name == null || _name == "<null>")
            {
                _name = "Pointer" + Index;
            }

            return *(bint*) Data != 0;
        }

        public override void OnPopulate()
        {
            VoidPtr dataPtr = ((bint*) Data)->OffsetAddress;
            switch (_memberType)
            {
                case hkClassMember.Type.TYPE_STRUCT:
                    new HavokMetaObjectNode(_classNode)
                        .Initialize(this, dataPtr, _classNode.Size);
                    break;
                case hkClassMember.Type.TYPE_ENUM:
                case hkClassMember.Type.TYPE_FLAGS:
                    new cmEnumNode
                        {
                            _enumNode = _enumNode
                        }
                        .Initialize(this, dataPtr, (int) _memberFlags & 0x3F);
                    break;
                case hkClassMember.Type.TYPE_ARRAY:
                case hkClassMember.Type.TYPE_HOMOGENEOUSARRAY:
                case hkClassMember.Type.TYPE_INPLACEARRAY:
                case hkClassMember.Type.TYPE_SIMPLEARRAY:
                case hkClassMember.Type.TYPE_ZERO:
                    Console.WriteLine("This shouldn't happen");
                    break;
                default:
                    ClassMemberInstanceNode instance = HavokMetaObjectNode.TryGetMember(_memberType);
                    if (instance != null)
                    {
                        instance._isZero = false;
                        instance._name = "";
                        instance._memberType = _memberType;
                        instance._memberFlags = _memberFlags;
                        instance._classNode = _classNode;
                        instance._enumNode = _enumNode;

                        instance.Initialize(this, dataPtr, instance.GetSize());
                    }

                    break;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GetSize() + (Children.Count > 0 ? Children[0].CalculateSize(force) : 0);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            if (Children.Count > 0)
            {
                HavokClassNode n = Children[0] as HavokClassNode;
                if (n is HavokMetaObjectNode || n is hkClassNode)
                {
                    writer.WriteString(HavokXML.GetObjectName(classNodes, n));
                }
                else
                {
                    n.WriteParams(writer, classNodes);
                }
            }
        }
    }
}
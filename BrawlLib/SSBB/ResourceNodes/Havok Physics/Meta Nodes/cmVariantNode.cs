using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class cmVariantNode : ClassMemberInstanceNode
    {
        public override int GetSize()
        {
            return 8;
        }

        public override bool OnInitialize()
        {
            return ((hkVariant*) Data)->_objectPtr != 0;
        }

        public override void OnPopulate()
        {
            hkVariant* v = (hkVariant*) Data;
            hkClass* classData = (hkClass*) v->_classPtr.OffsetAddress;
            HavokClassNode entry = HavokNode.GetClassNode(new string((sbyte*) classData->_namePtr.OffsetAddress), true);
            if (entry != null)
            {
                new HavokMetaObjectNode(entry as hkClassNode)
                    .Initialize(this, v->_objectPtr.OffsetAddress, classData->_size);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            hkVariant* v = (hkVariant*) address;
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            if (Children.Count > 0)
            {
                writer.WriteString(HavokXML.GetObjectName(classNodes, Children[0] as HavokClassNode));
            }
        }
    }
}
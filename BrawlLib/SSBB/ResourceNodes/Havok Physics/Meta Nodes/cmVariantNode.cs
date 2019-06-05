using System;
using System.Collections.Generic;
using System.Xml;
using BrawlLib.SSBBTypes;

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
            var v = (hkVariant*) Data;
            var classData = (hkClass*) v->_classPtr.OffsetAddress;
            var entry = HavokNode.GetClassNode(new string((sbyte*) classData->_namePtr.OffsetAddress));
            if (entry != null)
                new HavokMetaObjectNode(entry as hkClassNode)
                    .Initialize(this, v->_objectPtr.OffsetAddress, classData->_size);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var v = (hkVariant*) address;
        }

        public override void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            if (Children.Count > 0)
                writer.WriteString(HavokXML.GetObjectName(classNodes, Children[0] as HavokClassNode));
        }
    }
}
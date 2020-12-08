using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    //Parses instance data with class meta
    public class HavokMetaObjectNode : ClassMemberInstanceNode
    {
        public override ResourceType ResourceFileType => ResourceType.NoEditFolder;

        private List<hkClassMemberNode> _memberArray;

        public HavokMetaObjectNode()
        {
        }

        public HavokMetaObjectNode(hkClassNode classNode)
        {
            _classNode = classNode;
        }

        public override bool OnInitialize()
        {
            if (_classNode == null)
            {
                return false;
            }

            _className = _classNode.Name;

            HavokNode node = HavokNode;

            if (node._allSignatures.ContainsKey(_className))
            {
                _signature = HavokNode._allSignatures[_className];
            }

            node._dataSection._classCache.Add(this);

            if (_name == null)
            {
                _name = _classNode.Name;
            }

            _memberArray = new List<hkClassMemberNode>();

            //Youngest class has size for all inherited classes included
            if (_classNode._inheritance.Count > 0)
            {
                SetSizeInternal(_classNode._inheritance[0].Size);
            }

            foreach (hkClassNode c in _classNode._inheritance)
            {
                ResourceNode members = c.FindChild("Members", false);
                if (members != null)
                {
                    _memberArray.AddRange(members.Children.Select(x => x as hkClassMemberNode));
                }
            }

            return _memberArray != null && _memberArray.Count > 0;
        }

        public override void OnPopulate()
        {
            VoidPtr addr = WorkingUncompressed.Address;
            foreach (hkClassMemberNode member in _memberArray)
            {
                hkClassMember.Type type = member.Type;
                bool zero = type == hkClassMember.Type.TYPE_ZERO;
                if (zero)
                {
                    type = member.SubType;
                }

                hkClassNode memberClass = null;
                hkClassEnumNode memberEnum = null;

                if (!string.IsNullOrEmpty(member._class))
                {
                    HavokClassNode c = HavokNode.GetClassNode(member._class);
                    if (c is hkClassNode)
                    {
                        memberClass = c as hkClassNode;
                    }

                    if (memberClass == null)
                    {
                        Console.WriteLine("Could not find " + member._class + " class");
                    }
                }

                if (!string.IsNullOrEmpty(member._enum))
                {
                    //Loop through inheritance starting with the eldest class, added last
                    foreach (hkClassNode c in _classNode._inheritance)
                    {
                        ResourceNode enums = c.FindChild("Enums", false);
                        if (enums != null)
                        {
                            foreach (hkClassEnumNode e in enums.Children)
                            {
                                if (e.Name == member._enum)
                                {
                                    memberEnum = e;
                                    break;
                                }
                            }
                        }
                    }

                    if (memberEnum == null)
                    {
                        Console.WriteLine("Could not find " + member._enum + " enum in " + _classNode.Name + " class");
                    }
                }

                ClassMemberInstanceNode instance = TryGetMember(type);
                if (instance != null)
                {
                    switch (type)
                    {
                        case hkClassMember.Type.TYPE_SIMPLEARRAY:
                        case hkClassMember.Type.TYPE_ARRAY:
                        case hkClassMember.Type.TYPE_INPLACEARRAY:
                        case hkClassMember.Type.TYPE_HOMOGENEOUSARRAY:
                        case hkClassMember.Type.TYPE_POINTER:
                        case hkClassMember.Type.TYPE_FUNCTIONPOINTER:
                            type = member.SubType;
                            break;
                    }

                    instance._isZero = zero;
                    instance._name = member._name;
                    instance._memberType = type;
                    instance._memberFlags = member._flags;
                    instance._classNode = memberClass;
                    instance._enumNode = memberEnum;

                    instance.Initialize(this, addr + member._structOffset, instance.GetSize());
                }
            }
        }

        public static ClassMemberInstanceNode TryGetMember(hkClassMember.Type type)
        {
            ClassMemberInstanceNode m = null;
            int index = (int) type;
            if (index >= 0 && index < MemberTypes.Length)
            {
                Type t = MemberTypes[index];
                if (t != null)
                {
                    m = Activator.CreateInstance(t) as ClassMemberInstanceNode;
                }
            }

            if (m == null)
            {
                Console.WriteLine("Problem creating class member instance of " + type);
            }

            return m;
        }

        private static readonly Type[] MemberTypes = new Type[]
        {
            null, //void
            typeof(cmBoolNode),
            typeof(cmCharNode),
            typeof(cmSByteNode),
            typeof(cmByteNode),
            typeof(cmShortNode),
            typeof(cmUShortNode),
            typeof(cmIntNode),
            typeof(cmUIntNode),
            typeof(cmLongNode),
            typeof(cmULongNode),
            typeof(cmFloatNode),
            typeof(cmVec4Node),
            typeof(cmVec4Node), //Quaternion is a Vector4
            typeof(cmMat3Node), //Stored as a 4x3
            typeof(cmMat3Node), //Rotation is just a Mat3x3, stored as 4x3 though
            typeof(cmQSTransformNode),
            typeof(cmMat4Node),
            typeof(cmMat4Node), //Transform is hkRotation(Mat3) + Trans(Vec4), so just a Mat4
            null,               //Zero
            typeof(cmPointerNode),
            typeof(cmPointerNode), //Function Pointer
            typeof(cmArrayNode),   //Array
            null,                  //In Place Array
            typeof(cmEnumNode),
            typeof(HavokMetaObjectNode),
            typeof(cmSimpleArrayNode),
            null, //Homogenous Array
            typeof(cmVariantNode),
            typeof(cmStringNode),
            typeof(cmULongPtrNode),
            typeof(cmEnumNode),
            null //max
        };

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            foreach (ClassMemberInstanceNode i in Children)
            {
                if (!i.SerializedAsZero)
                {
                    writer.WriteStartElement("hkparam");
                    writer.WriteAttributeString("name", i.Name);
                    i.WriteParams(writer, classNodes);
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteComment($" {i.Name} SERIALIZE_IGNORED ");
                }
            }
        }
    }
}
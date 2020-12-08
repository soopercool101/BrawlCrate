using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class HavokXML
    {
        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings
            {Indent = true, IndentChars = "\t", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace};

        public static void Serialize(HavokNode node, string outFile)
        {
            using (FileStream stream = new FileStream(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None,
                0x1000, FileOptions.SequentialScan))
            {
                using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
                {
                    writer.Flush();
                    stream.Position = 0;

                    node.Populate();

                    Dictionary<HavokClassNode, int> classIDs = new Dictionary<HavokClassNode, int>();
                    int i = 1;
                    foreach (HavokSectionNode s in node.Children)
                    {
                        foreach (HavokClassNode c in s._classCache)
                        {
                            if (!classIDs.ContainsKey(c))
                            {
                                classIDs.Add(c, i++);
                            }
                        }
                    }

                    //RecursiveGetClassIDsData(ref classIDs, node, node._dataSection);

                    HavokClassNode rootClass = node._dataSection.Children[0] as HavokClassNode;
                    List<HavokSectionNode> sections =
                        node.Children.Select(x => x as HavokSectionNode)
                            .Where(x => x != node._dataSection).ToList();
                    sections.Add(node._dataSection);

                    writer.WriteStartDocument();
                    {
                        writer.WriteStartElement("hkpackfile");
                        writer.WriteAttributeString("classversion", "11");
                        writer.WriteAttributeString("contentsversion", "hk_2014.1.0-r1");
                        writer.WriteAttributeString("toplevelobject", GetObjectName(classIDs, rootClass));
                        writer.WriteAttributeString("maxpredicate", "21");
                        writer.WriteAttributeString("predicates", "");
                        {
                            foreach (HavokSectionNode s in sections)
                            {
                                writer.WriteStartElement("hksection");
                                writer.WriteAttributeString("name", s.Name == "Classes" ? "__types__" : "__data__");
                                {
                                    for (int x = s._classCache.Count - 1; x >= 0; x--)
                                    {
                                        HavokClassNode c = s._classCache[x];

                                        //HavokCommonArrayNode array = null;
                                        //if (c.Parent is cmPointerNode && c.Parent.Parent is HavokCommonArrayNode)
                                        //    array = c.Parent.Parent as HavokCommonArrayNode;
                                        //else if  (c.Parent is HavokCommonArrayNode)
                                        //    array = c.Parent as HavokCommonArrayNode;
                                        //if (array != null)
                                        //{
                                        //    //ResourceNode[] nodes = array.FindChildrenByClassType(null, typeof(HavokMetaObjectNode));
                                        //    //if (nodes != null && nodes.Length > 0)
                                        //        continue;
                                        //}

                                        writer.WriteStartElement("hkobject");
                                        writer.WriteAttributeString("name", GetObjectName(classIDs, c));
                                        writer.WriteAttributeString("class", SwapName(c._className));
                                        writer.WriteAttributeString("signature", GetSignature(c._className));
                                        {
                                            c.WriteParams(writer, classIDs);
                                        }
                                        writer.WriteEndElement();
                                    }
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndDocument();
                }
            }
        }

        public static string GetObjectName(Dictionary<HavokClassNode, int> classNodes, HavokClassNode classNode)
        {
            return "#" + classNodes[classNode].ToString().PadLeft(4, '0');
        }

        private static void RecursiveGetClassIDsData(ref Dictionary<HavokClassNode, int> classIDs, HavokNode node,
                                                     ResourceNode e)
        {
            if (e is HavokMetaObjectNode || e is hkClassNode)
            {
                HavokClassNode data = e as HavokClassNode;
                HavokClassNode type = node.GetClassNode(data._className);
                RecursiveGetClassIDsClasses(ref classIDs, node, type);
                classIDs.Add(data, classIDs.Count + 1);
            }

            for (int i = e.Children.Count - 1; i >= 0; i--)
            {
                RecursiveGetClassIDsData(ref classIDs, node, e.Children[i]);
            }
        }

        private static void RecursiveGetClassIDsClasses(ref Dictionary<HavokClassNode, int> classIDs, HavokNode node,
                                                        ResourceNode e)
        {
            if (e is hkClassNode)
            {
                hkClassNode type = e as hkClassNode;
                if (!string.IsNullOrEmpty(type.ParentClass))
                {
                    HavokClassNode c = node.GetClassNode(type.ParentClass);
                    classIDs.Add(c, classIDs.Count + 1);
                }

                ResourceNode members = type.FindChild("Members", false);
                foreach (hkClassMemberNode member in members.Children)
                {
                    if (!string.IsNullOrEmpty(member._class))
                    {
                        HavokClassNode c = node.GetClassNode(member._class);
                        classIDs.Add(c, classIDs.Count + 1);
                    }

                    if (!string.IsNullOrEmpty(member._enum))
                    {
                        HavokClassNode c = node.GetClassNode(member._enum);
                        classIDs.Add(c, classIDs.Count + 1);
                    }
                }

                if (!classIDs.ContainsKey(type))
                {
                    classIDs.Add(type, classIDs.Count + 1);
                }
            }
        }

        public static string SwapName(string className)
        {
            if (HardcodedName.ContainsKey(className))
            {
                return HardcodedName[className].Key;
            }

            return className;
        }

        public static string GetSignature(string className)
        {
            if (HardcodedName.ContainsKey(className))
            {
                return HardcodedName[className].Value;
            }

            return "0x0";
        }

        public static readonly Dictionary<string, KeyValuePair<string, string>> HardcodedName =
            new Dictionary<string, KeyValuePair<string, string>>
            {
                {"hkClass", new KeyValuePair<string, string>("hkClass", "0x33d42383")},
                {"hkClassEnum", new KeyValuePair<string, string>("hkClassEnum", "0x8a3609cf")},

                {"hkxScene", new KeyValuePair<string, string>("hkxScene", "0xc093dd28")},
                {"hkRootLevelContainer", new KeyValuePair<string, string>("hkRootLevelContainer", "0x2772c11e")},

                {"hkSkeleton", new KeyValuePair<string, string>("hkaSkeleton", "0xfec1cedb")},
                {"hkAnimationContainer", new KeyValuePair<string, string>("hkaAnimationContainer", "0x26859f4c")},
                {"hkBone", new KeyValuePair<string, string>("hkaBone", "0")}
            };
    }
}
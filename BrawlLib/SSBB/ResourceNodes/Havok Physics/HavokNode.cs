using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class HavokNode : ARCEntryNode
    {
        public const bool AssignClassParents = true;

        private static readonly Dictionary<string, Type> _classNodeTypes = new Dictionary<string, Type>
        {
            {"hkClass", typeof(hkClassNode)},
            {"hkClassEnum", typeof(hkClassEnumNode)}

            //Class types can be explicitly supported
            //Otherwise they are interpreted with a meta object node
            //as long as class meta is available
            //{ "hkxScene", typeof(hkxSceneNode) },
            //{ "hkPhysicsData", typeof(hkPhysicsDataNode) },
        };

        public Dictionary<string, uint> _allSignatures;

        public UnsafeBuffer _buffer;
        public HavokSectionNode _dataSection;
        public Dictionary<string, uint> _mainDataSignatures;

        public Dictionary<string, uint> _mainTypeSignatures;
        public string _rootClass;
        private int _userTag;
        private int _version;
        public string _versionString;

        internal HKXHeader* Header => (HKXHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Havok;

        [Category("Havok Physics")] public int UserTag => Header->_userTag;

        [Category("Havok Physics")] public int Version => Header->_classVersion;

        [Category("Havok Physics")] public string VersionString => _versionString;

        [Category("Havok Physics")] public string RootClass => _rootClass;

        public Dictionary<string, uint> MainTypeSignatures => _mainTypeSignatures;
        public Dictionary<string, uint> MainDataSignatures => _mainDataSignatures;
        public Dictionary<string, uint> AllSignatures => _allSignatures;

        protected override string GetName()
        {
            return base.GetName("HavokData");
        }

        public override bool OnInitialize()
        {
            _userTag = Header->_userTag;
            _version = Header->_classVersion;
            _versionString = Header->Name;

            _mainTypeSignatures = new Dictionary<string, uint>();
            _mainDataSignatures = new Dictionary<string, uint>();
            _allSignatures = new Dictionary<string, uint>();

            PatchPointers();

            var section = Header->OffsetSections;
            var classNames = (sbyte*) (_buffer.Address + section[Header->_classNameSectionIndex]._dataOffset);
            _rootClass = new string(classNames + Header->_rootClassNameOffset);

            return true;
        }

        private void PatchPointers()
        {
            if (_buffer != null) _buffer.Dispose();

            //Make a copy of the file's data that we can patch with offsets
            _buffer = new UnsafeBuffer(WorkingUncompressed.Length);
            Memory.Move(_buffer.Address, WorkingUncompressed.Address, (uint) WorkingUncompressed.Length);

            var header = (HKXHeader*) _buffer.Address;
            var section = header->OffsetSections;
            for (var i = 0; i < header->_sectionCount; i++, section++)
            {
                int dataOffset = section->_dataOffset;
                var data = _buffer.Address + dataOffset;
                int local = section->LocalPatchesLength, global = section->GlobalPatchesLength;

                if (section->ExportsLength > 0) Console.WriteLine("Has exports");

                if (section->ImportsLength > 0) Console.WriteLine("Has imports");

                //Global patches have to be made before local ones
                if (global > 0)
                {
                    //Global patches set offsets from this section to data in another section (or this one)
                    var start = data + section->_globalPatchesOffset;
                    var patch = (GlobalPatch*) start;
                    while ((int) patch - (int) start < global && patch->_dataOffset >= 0 && patch->_pointerOffset >= 0)
                    {
                        //Make the pointer offset relative to itself so it's self-contained
                        int ptrOffset = patch->_pointerOffset;
                        var ptr = (bint*) (data + ptrOffset);
                        var otherSection = &header->OffsetSections[patch->_sectionIndex];
                        var dOffset = patch->_dataOffset + otherSection->_dataOffset - dataOffset;
                        var offset = dOffset - ptrOffset;
                        *ptr = offset;
                        patch++;
                    }
                }

                if (local > 0)
                {
                    //Local patches set offsets to data located elsewhere in this section
                    var start = data + section->_localPatchesOffset;
                    var patch = (LocalPatch*) start;
                    while ((int) patch - (int) start < local && patch->_dataOffset >= 0)
                    {
                        //Make the pointer offset relative to itself so it's self-contained
                        int ptrOffset = patch->_pointerOffset;
                        var ptr = (bint*) (data + ptrOffset);
                        *ptr = patch->_dataOffset - ptrOffset;
                        patch++;
                    }
                }
            }
        }

        public override void OnPopulate()
        {
            var header = (HKXHeader*) _buffer.Address;
            var section = header->OffsetSections;

            var classes = &section[header->_classNameSectionIndex];
            var classNames = (sbyte*) (_buffer.Address + classes->_dataOffset);

            VoidPtr dataAddr = classNames;
            var baseAddr = dataAddr;
            while (dataAddr - baseAddr < classes->DataLength && *(byte*) (dataAddr + 4) == 9)
            {
                uint signature = *(buint*) dataAddr;
                var c = new string((sbyte*) dataAddr + 5);
                if (!_allSignatures.ContainsKey(c)) _allSignatures.Add(c, signature);

                dataAddr += 5 + c.Length + 1;
            }

            //Types have to be parsed first so that they can be used to parse the data last
            for (var i = 0; i < header->_sectionCount; i++, section++)
            {
                if (i == header->_classNameSectionIndex || i == header->_dataSectionIndex) continue;

                int dataOffset = section->_dataOffset;
                var data = _buffer.Address + dataOffset;

                var classNamePatchLength = section->ClassNamePatchesLength;
                if (classNamePatchLength > 0)
                {
                    var sectionNode = new HavokSectionNode
                    {
                        //sectionNode._name = section->Name;
                        _name = "Classes"
                    };
                    sectionNode.Initialize(this, data, section->DataLength);

                    //HavokGroupNode classGroup = new HavokGroupNode() { _parent = sectionNode, _name = "Classes" };
                    //sectionNode.Children.Add(classGroup);

                    //HavokGroupNode enumGroup = new HavokGroupNode() { _parent = sectionNode, _name = "Enums" };
                    //sectionNode.Children.Add(enumGroup);

                    var start = data + section->_classNamePatchesOffset;
                    var patch = (ClassNamePatch*) start;

                    var x = 0;
                    while ((int) patch - (int) start < classNamePatchLength && patch->_dataOffset >= 0)
                    {
                        var className = new string(classNames + patch->_classNameOffset);
                        uint signature = *(buint*) (classNames + (patch->_classNameOffset - 5));
                        if (!_mainTypeSignatures.ContainsKey(className)) _mainTypeSignatures.Add(className, signature);

                        var entry = GetClassNode(className, false);
                        if (entry != null)
                        {
                            entry._signature = signature;
                            entry._className = className;
                            entry.Initialize(sectionNode, data + patch->_dataOffset, 0);
                        }

                        patch++;
                        x++;
                    }

                    sectionNode._classCache = sectionNode._children.Select(b => b as HavokClassNode).ToList();
                    if (AssignClassParents)
                    {
                        sectionNode._children.Clear();
                        for (var r = 0; r < sectionNode._classCache.Count; r++)
                        {
                            var n = sectionNode._classCache[r];
                            if (n == null) continue;

                            n.Populate(0);
                            n._parent = sectionNode;
                            if (n is hkClassNode)
                            {
                                var c = n as hkClassNode;
                                if (!string.IsNullOrEmpty(c.ParentClass))
                                    for (var w = 0; w < sectionNode._classCache.Count; w++)
                                    {
                                        var n2 = sectionNode._classCache[w];
                                        if (w != r && n2 is hkClassNode && n2.Name == c.ParentClass) n._parent = n2;
                                    }
                            }
                        }

                        foreach (var n in sectionNode._classCache)
                        {
                            if (n == null) continue;

                            if (n._parent._children == null) n._parent._children = new List<ResourceNode>();

                            n._parent._children.Add(n);
                        }
                    }

                    foreach (var classNode in sectionNode._classCache)
                        if (classNode is hkClassNode)
                        {
                            var c = classNode as hkClassNode;
                            c.GetInheritance();
                        }
                }
            }

            //Parse data using class types, unless the data is explicitly supported
            if (header->_dataSectionIndex >= 0)
            {
                section = &header->OffsetSections[header->_dataSectionIndex];
                int dataOffset = section->_dataOffset;
                var data = _buffer.Address + dataOffset;

                var classNamePatchLength = section->ClassNamePatchesLength;
                if (classNamePatchLength > 0)
                {
                    var sectionNode = new HavokSectionNode
                    {
                        //sectionNode._name = section->Name;
                        _name = "Instances"
                    };
                    sectionNode.Initialize(this, data, section->DataLength);
                    sectionNode._classCache = new List<HavokClassNode>();
                    _dataSection = sectionNode;

                    uint rootOffset = header->_rootClassNameOffset;

                    var start = data + section->_classNamePatchesOffset;
                    var patch = (ClassNamePatch*) start;

                    var x = 0;
                    while ((int) patch - (int) start < classNamePatchLength && patch->_dataOffset >= 0)
                    {
                        var className = new string(classNames + patch->_classNameOffset);
                        uint signature = *(buint*) (classNames + (patch->_classNameOffset - 5));

                        if (!_mainDataSignatures.ContainsKey(className)) _mainDataSignatures.Add(className, signature);

                        var entry = GetClassNode(className);
                        if (entry != null && patch->_classNameOffset == rootOffset)
                            new HavokMetaObjectNode(entry as hkClassNode) {_signature = signature}
                                .Initialize(sectionNode, data + patch->_dataOffset, 0);

                        patch++;
                        x++;
                    }
                }
            }
        }

        public HavokClassNode GetClassNode(string className, bool searchClasses = true)
        {
            HavokClassNode e = null;
            if (_classNodeTypes.ContainsKey(className))
                e = Activator.CreateInstance(_classNodeTypes[className]) as HavokClassNode;

            if (e == null && searchClasses)
                foreach (HavokSectionNode section in Children)
                {
                    if (section._classCache != null)
                        foreach (var c in section._classCache)
                            if (c.Name == className)
                            {
                                e = c;
                                break;
                            }

                    if (e != null) break;
                }

            if (e == null) Console.WriteLine("Unsupported class type: " + className);

            return e;
        }

        public override void Export(string outPath)
        {
            if (outPath.ToUpper().EndsWith(".XML"))
                HavokXML.Serialize(this, outPath);
            //else if (outPath.ToUpper().EndsWith(".PMD"))
            //    PMDModel.Export(this, outPath);
            //else if (outPath.ToUpper().EndsWith(".RMDL"))
            //    XMLExporter.ExportRMDL(this, outPath);
            else
                base.Export(outPath);
        }

        public override int OnCalculateSize(bool force)
        {
            return _buffer.Length;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Memory.Move(address, WorkingUncompressed.Address, (uint) length);
            foreach (HavokEntryNode r in Children) RecursiveRebuild(r, address);
        }

        private void RecursiveRebuild(HavokEntryNode node, VoidPtr baseAddr)
        {
            if (node is ClassMemberInstanceNode && node.HasChanged) node.Rebuild(baseAddr + node.DataOffset, 0, true);

            if (node._children != null && node._children.Count > 0)
                foreach (HavokEntryNode r in node._children)
                    RecursiveRebuild(r, baseAddr);
        }

        public override void Replace(string fileName, FileMapProtect prot, FileOptions options)
        {
            base.Replace(fileName, prot, options);
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((HKXHeader*) source.Address)->_tag1 == HKXHeader.Tag1 ? new HavokNode() : null;
        }

        public bool IsValid()
        {
            return _buffer != null && _buffer.Length > 0 && _buffer.Address != null;
        }

        public VoidPtr GetAddress()
        {
            return _buffer.Address;
        }

        public int GetLength()
        {
            return _buffer.Length;
        }
    }

    public abstract class HavokEntryNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.NoEditEntry;

        [Browsable(false)]
        public HavokNode HavokNode
        {
            get
            {
                var n = _parent;
                while (!(n is HavokNode) && n != null) n = n._parent;

                return n as HavokNode;
            }
        }

        [Browsable(false)]
        public int DataOffset
        {
            get
            {
                var p = HavokNode;
                if (p != null)
                    return (int) WorkingUncompressed.Address - (int) p._buffer.Address;
                return -1;
            }
        }

        public string Offset
        {
            get
            {
                var offset = DataOffset;
                if (offset > 0) return "0x" + offset.ToString("X");

                return "null";
            }
        }

        public string DataSize => "0x" + WorkingUncompressed.Length.ToString("X");
    }

    public class HavokGroupNode : HavokEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.NoEditFolder;
    }

    public abstract class HavokClassNode : HavokEntryNode
    {
        public string _className;
        public uint _signature;

        [Category("Havok Class")] public string ClassName => _className;

        public virtual void WriteParams(XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
        }
    }

    public class HavokSectionNode : HavokEntryNode
    {
        public List<HavokClassNode> _classCache;
        public override ResourceType ResourceFileType => ResourceType.HavokGroup;
        public HavokClassNode[] ClassCache => _classCache.ToArray();
    }
}
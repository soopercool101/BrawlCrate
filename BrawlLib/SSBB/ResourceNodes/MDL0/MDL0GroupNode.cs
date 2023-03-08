using BrawlLib.Internal;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.Linq;
#if !DEBUG
using System.ComponentModel;
#endif

namespace BrawlLib.SSBB.ResourceNodes
{
    public abstract class MDL0EntryNode : ResourceNode
    {
        internal virtual void GetStrings(StringTable table)
        {
            table.Add(_name);
        }

        public int _entryIndex;

#if !DEBUG
        [Browsable(false)]
#endif
        public MDL0Node Model
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is MDL0Node) && n != null)
                {
                    n = n._parent;
                }

                return n as MDL0Node;
            }
        }

#if !DEBUG
        [Browsable(false)]
#endif
        public BRRESNode BRESNode
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is BRRESNode) && n != null)
                {
                    n = n._parent;
                }

                return n as BRRESNode;
            }
        }

        internal virtual void Bind()
        {
        }

        internal virtual void Unbind()
        {
        }

        public override void SignalPropertyChange()
        {
            base.SignalPropertyChange();

            TKContext.InvalidateModelPanels(Model);
        }

        protected internal virtual void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
        }
    }

    public unsafe class MDL0GroupNode : ResourceNode
    {
        internal ResourceGroup* Header => (ResourceGroup*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.MDL0Group;

        public MDLResourceType _type;

        internal int _index;
        //internal List<ResourceNode> _nodeCache;

        public MDL0GroupNode(MDLResourceType type)
        {
            _type = type;
            _name = _type.ToString("g");
        }

        internal void GetStrings(StringTable table)
        {
            foreach (MDL0EntryNode n in Children)
            {
                n.GetStrings(table);
            }
        }

        internal void Initialize(ResourceNode parent, DataSource source, int index)
        {
            _index = index;
            Initialize(parent, source);
        }

        public override void RemoveChild(ResourceNode child)
        {
            base.RemoveChild(child);
            if (_children == null || _children.Count == 0)
            {
                _parent.RemoveChild(this);
            }
        }

        internal void Parse(MDL0Node model)
        {
            Influence inf;
            ModelLinker linker = model._linker;

            int typeIndex = (int) _type;
            fixed (ResourceGroup** gList = &linker.Defs)
            {
                if (gList[typeIndex] != null)
                {
                    ExtractGroup(gList[typeIndex], ModelLinker.TypeBank[typeIndex]);
                }
                else
                {
                    return; //Nothing to read
                }
            }

            //Special handling for bones, objects, and materials
            if (_type == MDLResourceType.Bones)
            {
                //Bones have been parsed from raw data as a flat list.
                //Bones re-assign parents in their Initialize block, so parents are true.
                //Parents must be assigned now as bones will be moved in memory when assigned as children.

                //Cache flat list
                linker.BoneCache = _children.Select(x => x as MDL0BoneNode).ToArray();

                //Reset children so we can rebuild
                _children.Clear();

                //Assign children using each bones' parent offset in case NodeTree is corrupted.
                //Bone parents are assigned when they are initialized in a flat array.
                foreach (MDL0BoneNode b in linker.BoneCache)
                {
                    MDL0Bone* header = b.Header;

                    //Assign true parent using parent header offset
                    int offset = header->_parentOffset;
                    if (offset != 0)
                    {
                        //Get address of parent header
                        MDL0Bone* pHeader = (MDL0Bone*) ((byte*) header + offset);
                        //Search bone list for matching header
                        foreach (MDL0BoneNode b2 in linker.BoneCache)
                        {
                            if (pHeader == b2.Header)
                            {
                                b._parent = b2;
                                break;
                            }
                        }
                    }

                    if (b._boneFlags.HasFlag(BoneFlags.HasBillboardParent))
                    {
                        b._bbRefNode = model._linker.BoneCache[header->_bbIndex];
                    }
                }

                //Make sure the node cache is the correct size
                int highest = 0;

                //Add bones to their parent's child lists and find highest node id
                foreach (MDL0BoneNode b in linker.BoneCache)
                {
                    b._parent._children.Add(b);

                    if (b._nodeIndex >= linker.NodeCache.Length && b._nodeIndex > highest)
                    {
                        highest = b._nodeIndex;
                    }
                }

                if (highest >= linker.NodeCache.Length)
                {
                    linker.NodeCache = new IMatrixNode[highest + 1];
                }

                //Populate node cache
                MDL0BoneNode bone = null;
                int index;
                int count = linker.BoneCache.Length;
                for (int i = 0; i < count; i++)
                {
                    linker.NodeCache[(bone = linker.BoneCache[i])._nodeIndex] = bone;
                }

                int nullCount = 0;

                bool nodeTreeError = false;

                //Now that bones and primary influences have been cached, we can create weighted influences.
                foreach (ResourcePair p in *linker.Defs)
                {
                    if (p.Name == "NodeTree")
                    {
                        //Double check bone tree using the NodeTree definition.
                        //If the NodeTree is corrupt, the user will be informed that it needs to be rebuilt.
                        byte* pData = (byte*) p.Data;
                        bool fixCS0159 = false;

                        List<MDL0BoneNode> bones = linker.BoneCache.ToList();

                        STop:
                        if (*pData == 2)
                        {
                            bone = linker.BoneCache[*(bushort*) (pData + 1)];
                            index = *(bushort*) (pData + 3); //Parent bone node index

                            if (bone.Header->_parentOffset == 0)
                            {
                                if (!_children.Contains(bone))
                                {
                                    nodeTreeError = true;
                                    continue;
                                }

                                bones.Remove(bone);
                            }
                            else
                            {
                                MDL0BoneNode parent = linker.NodeCache[index] as MDL0BoneNode;
                                if (parent == null || bone._parent != parent || !parent._children.Contains(bone))
                                {
                                    nodeTreeError = true;
                                    continue;
                                }

                                bones.Remove(bone);
                            }

                            pData += 5;
                            fixCS0159 = true;
                        }

                        if (fixCS0159)
                        {
                            fixCS0159 = false;
                            goto STop;
                        }

                        if (bones.Count > 0)
                        {
                            nodeTreeError = true;
                        }
                    }
                    else if (p.Name == "NodeMix")
                    {
                        //Use node mix to create weight groups
                        byte* pData = (byte*) p.Data;
                        bool fixCS0159 = false;
                        TTop:
                        switch (*pData)
                        {
                            //Type 3 is for weighted influences
                            case 3:
                                //Get index/count fields
                                index = *(bushort*) (pData + 1);
                                count = pData[3];
                                //Get data pointer (offset of 4)
                                MDL0NodeType3Entry* nEntry = (MDL0NodeType3Entry*) (pData + 4);
                                //Create influence with specified count
                                inf = new Influence();
                                //Iterate through weights, adding each to the influence
                                //Here, we are referring back to the NodeCache to grab the bone.
                                //Note that the weights do not reference other influences, only bones. There is a good reason for this.
                                MDL0BoneNode b = null;
                                List<int> nullIndices = new List<int>();
                                for (int i = 0; i < count; i++, nEntry++)
                                {
                                    if (nEntry->_id < linker.NodeCache.Length &&
                                        (b = linker.NodeCache[nEntry->_id] as MDL0BoneNode) != null)
                                    {
                                        inf.AddWeight(new BoneWeight(b, nEntry->_value));
                                    }
                                    else
                                    {
                                        nullIndices.Add(i);
                                    }
                                }

                                bool noWeights = false;
                                if ((nullCount = nullIndices.Count) > 0)
                                {
                                    List<BoneWeight> newWeights = new List<BoneWeight>();
                                    for (int i = 0; i < inf.Weights.Count; i++)
                                    {
                                        if (!nullIndices.Contains(i))
                                        {
                                            newWeights.Add(inf.Weights[i]);
                                        }
                                    }

                                    if (newWeights.Count == 0)
                                    {
                                        noWeights = true;
                                    }
                                    else
                                    {
                                        inf.SetWeights(newWeights);
                                    }
                                }

                                //Add influence to model object, while adding it to the cache.
                                //Don't add user references here, they will be added during each object's initialization
                                if (!noWeights)
                                {
                                    ((Influence) (linker.NodeCache[index] = model._influences.FindOrCreate(inf)))
                                        ._index = index;
                                }

                                //Move data pointer to next entry
                                pData = (byte*) nEntry;
                                fixCS0159 = true;
                                break;
                            //Type 5 is for primary influences
                            case 5:
                                pData += 5;
                                fixCS0159 = true;
                                break;
                        }

                        if (fixCS0159)
                        {
                            fixCS0159 = false;
                            goto TTop;
                        }
                    }
                }

                if (nullCount > 0)
                {
                    model._errors.Add("There were " + nullCount + " null weights in NodeMix.");
                }

                if (nodeTreeError)
                {
                    model._errors.Add("The NodeTree definition did not match the bone tree.");
                }
            }
            else if (_type == MDLResourceType.Objects)
            {
                //Attach materials to polygons.
                //This assumes that materials have already been parsed.

                List<ResourceNode> matList = ((MDL0Node) _parent)._matList;
                MDL0ObjectNode obj;
                MDL0MaterialNode mat;

                //Find DrawOpa or DrawXlu entry in Definition list
                foreach (ResourcePair p in *linker.Defs)
                {
                    if (p.Name == "DrawOpa" || p.Name == "DrawXlu")
                    {
                        bool isXLU = p.Name == "DrawXlu";

                        ushort objectIndex = 0;
                        byte* pData = (byte*) p.Data;
                        while (*pData++ == 4)
                        {
                            //Get object with index
                            objectIndex = *(bushort*) (pData + 2);
                            if (objectIndex >= _children.Count || objectIndex < 0)
                            {
                                model._errors.Add("Object index was greater than the actual object count.");
                                objectIndex = 0;
                            }

                            obj = _children[objectIndex] as MDL0ObjectNode;

                            //Get material with index
                            mat = matList[*(bushort*) pData] as MDL0MaterialNode;

                            //Get bone with index
                            int boneIndex = *(bushort*) (pData + 4);
                            MDL0BoneNode visBone = null;
                            if (linker.BoneCache != null && boneIndex >= 0 && boneIndex < linker.BoneCache.Length)
                            {
                                visBone = linker.BoneCache[boneIndex];
                            }

                            obj._drawCalls.Add(new DrawCall(obj)
                            {
                                _drawOrder = pData[6],
                                _isXLU = isXLU,
                                MaterialNode = mat,
                                VisibilityBoneNode = visBone
                            });

                            //Increment pointer
                            pData += 7;
                        }
                    }
                }

                foreach (MDL0ObjectNode m in _children)
                {
                    int max = 0;
                    foreach (DrawCall c in m._drawCalls)
                    {
                        max = Maths.Max(max, c.MaterialNode.Children.Count);
                        if (c.MaterialNode.MetalMaterial != null)
                        {
                            max = Maths.Max(max, c.MaterialNode.MetalMaterial.Children.Count);
                        }
                    }

                    bool hasUnused = false;
                    if (m._manager != null)
                    {
                        for (int i = max; i < 8; i++)
                        {
                            if (m._manager.HasTextureMatrix[i])
                            {
                                m._manager.HasTextureMatrix[i] = false;
                                m._forceRebuild = true;
                                hasUnused = true;
                            }
                        }
                    }

                    if (hasUnused)
                    {
                        ((MDL0Node) Parent)._errors.Add("Object " + m.Index + " has unused texture matrices.");
                    }

                    //This error doesn't seem to always be true for factory models...
                    //if (m.HasTexMtx && m.HasNonFloatVertices)
                    //{
                    //    ((MDL0Node)Parent)._errors.Add("Object " + m.Index + " has texture matrices and non-float vertices, meaning it will explode in-game.");
                    //    m.SignalPropertyChange();
                    //}
                }
            }
            else if (_type == MDLResourceType.Materials)
            {
                foreach (var mat in Children)
                {
                    foreach (var matEntry in mat.Children)
                    {
                        if (matEntry is MDL0MaterialRefNode mRef)
                        {
                            bool foundError = false;

                            if (mRef.Scale.Y == 0)
                            {
                                mRef.Scale = new Vector2(mRef.Scale.X, 1);
                                foundError = true;
                            }

                            if (foundError)
                            {
                                model._errors.Add($"Material Reference {mRef.Name} had incorrect texture scale");
                            }
                        }
                    }
                }
            }
        }

        //Extracts resources from a group, using the specified type
        private void ExtractGroup(ResourceGroup* pGroup, Type t)
        {
            //If using shaders, cache results instead of unique entries
            //This is because shaders can appear multiple times, but with different names
            bool useCache = t == typeof(MDL0ShaderNode);

            MDL0CommonHeader* pHeader;
            ResourceNode node;
            VoidPtr* offsetCache = stackalloc VoidPtr[128];
            VoidPtr offsetCount = 0, offset, x;

            foreach (ResourcePair p in *pGroup)
            {
                //Get data offset
                offset = p.Data;
                if (useCache)
                {
                    //search for entry within offset cache
                    for (x = 0; x < offsetCount && offsetCache[x] != offset; x++)
                    {
                        ;
                    }

                    //If found, skip to next entry
                    if (x < offsetCount)
                    {
                        continue;
                    }

                    //Otherwise, store offset
                    offsetCache[offsetCount++] = offset;
                }

                //Create resource instance
                pHeader = (MDL0CommonHeader*) p.Data;
                node = Activator.CreateInstance(t) as ResourceNode;

                //Initialize
                node.Initialize(this, pHeader, pHeader->_size);

                //Set the name of the node. This is necessary for defs.
                //Make sure we're not naming the shaders,
                //or it will name it the name of the first material it's linked to.
                if (t != typeof(MDL0ShaderNode))
                {
                    node._name = (string) p.Name;
                }
            }
        }

        protected internal virtual void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            if (dataAddress <= mdlAddress)
            {
                return;
            }

            ResourceGroup* pGroup = (ResourceGroup*) dataAddress;
            ResourceEntry* rEntry = &pGroup->_first;
            int index = 1;
            (*rEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            if (_name == "Definitions")
            {
                return;
            }

            List<ResourceNode> entries = _name == "Bones"
                ? ((MDL0Node) Parent)._linker.BoneCache.Select(x => x as ResourceNode).ToList()
                : Children;

            foreach (MDL0EntryNode n in entries)
            {
                dataAddress = (VoidPtr) pGroup + (rEntry++)->_dataOffset;
                ResourceEntry.Build(pGroup, index++, dataAddress, (BRESString*) stringTable[n.Name]);

                if (dataAddress > mdlAddress)
                {
                    n.PostProcess(mdlAddress, dataAddress, stringTable);
                }
            }
        }

        internal void Bind()
        {
            foreach (MDL0EntryNode e in Children)
            {
                e.Bind();
            }
        }

        internal void Unbind()
        {
            foreach (MDL0EntryNode e in Children)
            {
                e.Unbind();
            }
        }

        public override void SortChildren()
        {
            if (Children == null || Children.Count <= 0 || !(Children[0] is MDL0MaterialNode))
            {
                base.SortChildren();
                return;
            }

            _children = _children.OrderBy(o => ((MDL0MaterialNode) o).IsMetal).ThenBy(o => o.Name).ToList();
            SignalPropertyChange();
        }
    }
}
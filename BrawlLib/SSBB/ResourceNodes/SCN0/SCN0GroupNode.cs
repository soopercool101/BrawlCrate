using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0GroupNode : ResourceNode
    {
        internal ResourceGroup* Group => (ResourceGroup*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.MDL0Group;

        public GroupType _type;

        public enum GroupType
        {
            LightSet,
            AmbientLight,
            Light,
            Fog,
            Camera
        }

        public static readonly string[] _names =
        {
            "LightSet(NW4R)",
            "AmbLights(NW4R)",
            "Lights(NW4R)",
            "Fogs(NW4R)",
            "Cameras(NW4R)"
        };

        public static readonly Type[] _types =
        {
            typeof(SCN0LightSetNode),
            typeof(SCN0AmbientLightNode),
            typeof(SCN0LightNode),
            typeof(SCN0FogNode),
            typeof(SCN0CameraNode)
        };


        public override string Name
        {
            get => _names[(int) _type];
            set
            {
                int i = _names.IndexOf(value);
                if (i >= 0 && i < 5)
                {
                    _type = (GroupType) i;
                }

                base.Name = value;
            }
        }

        public SCN0GroupNode(GroupType t)
        {
            _type = t;
        }

        public SCN0GroupNode(string name)
        {
            int i = _names.IndexOf(name);
            if (i >= 0 && i < 5)
            {
                _type = (GroupType) i;
            }
        }

        internal void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SCN0EntryNode n in Children)
            {
                n.GetStrings(table);
            }
        }

        public override void RemoveChild(ResourceNode child)
        {
            if (_children != null && _children.Count == 1 && _children.Contains(child))
            {
                _parent.RemoveChild(this);
            }
            else
            {
                base.RemoveChild(child);
            }
        }

        public override bool OnInitialize()
        {
            return Group->_numEntries > 0;
        }

        //groups, entries, keys, colors, vis
        public int[] _dataLengths = {0, 0, 0, 0, 0};

        public override int OnCalculateSize(bool force)
        {
            //Calculate resource group length.
            //Null nodes are not included in the resource group.
            _dataLengths[0] = ResourceGroup.Size + UsedChildren.Count * ResourceEntry.Size;

            //Reset entry length
            _dataLengths[1] = 0;

            //Loop through each node and increase entry and data lengths
            foreach (SCN0EntryNode n in Children)
            {
                //Add length of header and calculate data lengths
                _dataLengths[1] += n.CalculateSize(true);

                for (int i = 0; i < 3; i++)
                {
                    _dataLengths[i + 2] += n._dataLengths[i];
                }
            }

            return _dataLengths[0] + _dataLengths[1] + _dataLengths[2] + _dataLengths[3] + _dataLengths[4];
        }

        //Entries, Keys, Colors, Vis
        public VoidPtr[] _addrs = new VoidPtr[4];

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            //Create resource group
            ResourceGroup* group = (ResourceGroup*) address;
            *group = new ResourceGroup(UsedChildren.Count);
            ResourceEntry* entry = group->First;

            //Loop through entries, 
            //set data offset in group,
            //set data addresses to each entry
            //rebuild entry, and increment data addresses
            foreach (SCN0EntryNode n in Children)
            {
                //Null entries are not written to the resource group
                if (n.Name != "<null>")
                {
                    (entry++)->_dataOffset = (int) _addrs[0] - (int) group;
                }

                //Set data addresses to entry
                for (int i = 0; i < 3; i++)
                {
                    n._dataAddrs[i] = _addrs[i + 1];
                }

                //Rebuild entry
                n.Rebuild(_addrs[0], n._calcSize, true);

                //Increase data addresses
                _addrs[0] += n._calcSize;
                for (int i = 0; i < 3; i++)
                {
                    _addrs[i + 1] += n._dataLengths[i];
                }
            }
        }

        protected internal virtual void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            ResourceGroup* group = (ResourceGroup*) dataAddress;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SCN0EntryNode n in UsedChildren)
            {
                dataAddress = (VoidPtr) group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*) stringTable[n.Name]);
                //n.PostProcess(scn0Address, dataAddress, stringTable);
            }

            int len = 0;
            switch (_type)
            {
                case GroupType.LightSet:
                    len = SCN0LightSet.Size;
                    break;
                case GroupType.AmbientLight:
                    len = SCN0AmbientLight.Size;
                    break;
                case GroupType.Light:
                    len = SCN0Light.Size;
                    break;
                case GroupType.Fog:
                    len = SCN0Fog.Size;
                    break;
                case GroupType.Camera:
                    len = SCN0Camera.Size;
                    break;
            }

            bint* hdr = (bint*) scn0Address + 5;
            VoidPtr entries = scn0Address + hdr[(int) _type];
            foreach (SCN0EntryNode n in Children)
            {
                n.PostProcess(scn0Address, entries, stringTable);
                entries += len;
            }
        }

        /// <summary>
        /// Returns a list of all non-null children.
        /// </summary>
        [Browsable(false)]
        public List<ResourceNode> UsedChildren
        {
            get
            {
                List<ResourceNode> l = new List<ResourceNode>();
                foreach (SCN0EntryNode n in Children)
                {
                    if (n.Name != "<null>")
                    {
                        l.Add(n);
                    }
                }

                return l;
            }
        }
    }

    public unsafe class SCN0EntryNode : ResourceNode
    {
        internal SCN0CommonHeader* Header => (SCN0CommonHeader*) WorkingUncompressed.Address;
        public override bool AllowNullNames => true;

        [Browsable(false)]
        public SCN0Node Scene
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is SCN0Node) && n != null)
                {
                    n = n._parent;
                }

                return n as SCN0Node;
            }
        }

        //Key, Color, Vis
        public int[] _dataLengths = {0, 0, 0};
        public VoidPtr[] _dataAddrs = new VoidPtr[3];

        [Category("SCN0 Entry")]
        public int NodeIndex => Name != "<null>" ? ((SCN0GroupNode) Parent).UsedChildren.IndexOf(this) : -1;

        [Category("SCN0 Entry")] public int RealIndex => Name != "<null>" ? Index : -1;

        internal virtual void GetStrings(StringTable table)
        {
            if (Name != "<null>")
            {
                table.Add(Name);
            }
        }

        public override bool OnInitialize()
        {
            if (!_replaced && _name == null)
            {
                if (Header->_stringOffset != 0)
                {
                    _name = Header->ResourceString;
                }
                else
                {
                    _name = "<null>";
                }
            }

            SetSizeInternal(Header->_length);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SCN0CommonHeader* header = (SCN0CommonHeader*) address;
            header->_length = length;
        }

        protected internal virtual void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            SCN0CommonHeader* header = (SCN0CommonHeader*) dataAddress;
            header->_scn0Offset = (int) scn0Address - (int) dataAddress;
            header->_nodeIndex = NodeIndex;
            header->_realIndex = RealIndex;

            if (Name != "<null>")
            {
                header->ResourceStringAddress = stringTable[Name] + 4;
            }
            else
            {
                header->_stringOffset = 0;
            }
        }

        public IColorSource FindColorMatch(bool constant, int frameCount, int id)
        {
            IColorSource match = null;
            IColorSource s = this as IColorSource;
            if (!constant && s != null)
            {
                foreach (IColorSource n in ((ResourceNode) s).Parent.Children)
                {
                    if (n == s)
                    {
                        break;
                    }

                    if (!n.GetColorConstant(id))
                    {
                        for (int i = 0; i <= frameCount; i++)
                        {
                            if (n.GetColor(i, id) != s.GetColor(i, id))
                            {
                                break;
                            }

                            if (i == frameCount)
                            {
                                match = n;
                            }
                        }
                    }

                    if (match != null)
                    {
                        break;
                    }
                }
            }

            return match;
        }

        public int WriteColors(ref int flags,
                               int bit,
                               RGBAPixel solidColor,
                               List<RGBAPixel> colors,
                               bool constant,
                               int frameCount,
                               VoidPtr valueAddr,
                               ref VoidPtr thisMatchAddr,
                               VoidPtr thatMatchAddr,
                               RGBAPixel* dataAddr)
        {
            if (!constant)
            {
                flags &= ~bit;
                thisMatchAddr = dataAddr;
                if (thatMatchAddr == null)
                {
                    VoidPtr start = dataAddr;
                    *(bint*) valueAddr = (int) dataAddr - (int) valueAddr;
                    for (int x = 0; x <= frameCount; x++)
                    {
                        if (x < colors.Count)
                        {
                            *dataAddr++ = colors[x];
                        }
                        else
                        {
                            *dataAddr++ = new RGBAPixel();
                        }
                    }

                    return dataAddr - start;
                }

                *(bint*) valueAddr = (int) thatMatchAddr - (int) valueAddr;
                return 0;
            }

            flags |= bit;
            *(RGBAPixel*) valueAddr = solidColor;
            return 0;
        }

        protected void ReadColors(
            uint flags,
            uint bit,
            ref RGBAPixel solidColor,
            ref List<RGBAPixel> colors,
            int frameCount,
            VoidPtr address,
            ref bool constant,
            ref int numEntries)
        {
            colors = new List<RGBAPixel>();
            if (constant = (flags & bit) != 0)
            {
                solidColor = *(RGBAPixel*) address;
                numEntries = 0;
            }
            else
            {
                numEntries = frameCount + 1;
                RGBAPixel* addr = (RGBAPixel*) (address + *(bint*) address);
                for (int x = 0; x < numEntries; x++)
                {
                    colors.Add(*addr++);
                }
            }
        }

        protected void CalcKeyLen(KeyframeArray keyframes)
        {
            if (keyframes._keyCount > 1)
            {
                _dataLengths[0] += SCN0KeyframesHeader.Size + keyframes._keyCount * SCN0KeyframeStruct.Size + 4;
            }
        }

        /// <summary>
        /// Reads keyframes from an address and sets them in the keyframe array provided.
        /// </summary>
        /// <param name="kf">The array to set decoded frames to.</param>
        /// <param name="dataAddr">The address of the OFFSET to the data.</param>
        /// <param name="flags">The flags used to determine if the value is constant.</param>
        /// <param name="fixedBit">The bit located in the flags that determines if the value is constant.</param>
        public void DecodeKeyframes(KeyframeArray kf, VoidPtr dataAddr, int flags, int fixedBit)
        {
            if ((flags & fixedBit) != 0)
            {
                kf[0] = *(bfloat*) dataAddr;
            }
            else
            {
                DecodeKeyframes(kf, dataAddr + *(bint*) dataAddr);
            }
        }

        /// <summary>
        /// Reads keyframes at an address, starting with the keyframe array header.
        /// </summary>
        /// <param name="kf">The array to set decoded frames to.</param>
        /// <param name="dataAddr">The address of the keyframe array header.</param>
        public static void DecodeKeyframes(KeyframeArray kf, VoidPtr dataAddr)
        {
            SCN0KeyframesHeader* header = (SCN0KeyframesHeader*) dataAddr;
            SCN0KeyframeStruct* entry = header->Data;
            for (int i = 0; i < header->_numFrames; i++, entry++)
            {
                kf.SetFrameValue((int) entry->_index, entry->_value, true)._tangent = entry->_tangent;
            }
        }

        public static int EncodeKeyframes(KeyframeArray kf, VoidPtr dataAddr, VoidPtr offset, ref int flags,
                                          int fixedBit)
        {
            if (kf._keyCount > 1)
            {
                flags &= ~fixedBit;
                return EncodeKeyframes(kf, dataAddr, offset);
            }

            flags |= fixedBit;
            *(bfloat*) offset = kf._keyRoot._next._value;
            return 0;
        }

        public static int EncodeKeyframes(KeyframeArray kf, VoidPtr dataAddr, VoidPtr offset)
        {
            *(bint*) offset = (int) dataAddr - (int) offset;
            return EncodeKeyframes(kf, dataAddr);
        }

        public static int EncodeKeyframes(KeyframeArray kf, VoidPtr dataAddr)
        {
            VoidPtr start = dataAddr;
            SCN0KeyframesHeader* header = (SCN0KeyframesHeader*) dataAddr;
            *header = new SCN0KeyframesHeader(kf._keyCount);
            KeyframeEntry frame, root = kf._keyRoot;

            SCN0KeyframeStruct* entry = header->Data;
            for (frame = root._next; frame._index != -1; frame = frame._next)
            {
                *entry++ = new SCN0KeyframeStruct(frame._tangent, frame._index, frame._value);
            }

            *(bint*) entry = 0;
            dataAddr = (VoidPtr) entry + 4;
            return dataAddr - start;
        }
    }
}
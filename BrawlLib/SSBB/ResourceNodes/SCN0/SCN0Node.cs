using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0Node : NW4RAnimationNode
    {
        internal SCN0v4* Header4 => (SCN0v4*) WorkingUncompressed.Address;
        internal SCN0v5* Header5 => (SCN0v5*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SCN0;
        public override int[] SupportedVersions => new int[] {4, 5};
        public override string Tag => "SCN0";

        public SCN0Node()
        {
            _version = 4;
        }

        public void ReplaceCamera(SCN0Node camNode)
        {
            CameraGroup.Children.Clear();
            foreach (ResourceNode n in camNode.CameraGroup.Children)
            {
                CameraGroup.AddChild(n);
            }
        }

        private const string _category = "Scene Definition";
        public int _specLights, _lightsets, _amblights, _lights, _fogs, _cameras;

        public SCN0GroupNode[] _groups = new SCN0GroupNode[5];

        [Browsable(false)]
        public SCN0GroupNode LightSetGroup
        {
            get => _groups[0];
            set => _groups[0] = value;
        }

        [Browsable(false)]
        public SCN0GroupNode AmbientGroup
        {
            get => _groups[1];
            set => _groups[1] = value;
        }

        [Browsable(false)]
        public SCN0GroupNode LightGroup
        {
            get => _groups[2];
            set => _groups[2] = value;
        }

        [Browsable(false)]
        public SCN0GroupNode FogGroup
        {
            get => _groups[3];
            set => _groups[3] = value;
        }

        [Browsable(false)]
        public SCN0GroupNode CameraGroup
        {
            get => _groups[4];
            set => _groups[4] = value;
        }

        [Category(_category)]
        public override int FrameCount
        {
            get => base.FrameCount;
            set => base.FrameCount = value;
        }

        [Category(_category)]
        public override bool Loop
        {
            get => base.Loop;
            set
            {
                base.Loop = value;
                UpdateChildFrameLimits();
            }
        }

        protected override void UpdateChildFrameLimits()
        {
            if (LightGroup != null)
            {
                foreach (SCN0LightNode l in LightGroup.Children)
                {
                    l.SetSize(_numFrames, Loop);
                }
            }

            if (AmbientGroup != null)
            {
                foreach (SCN0AmbientLightNode l in AmbientGroup.Children)
                {
                    l.SetSize(_numFrames);
                }
            }

            if (FogGroup != null)
            {
                foreach (SCN0FogNode l in FogGroup.Children)
                {
                    l.SetSize(_numFrames, Loop);
                }
            }

            if (CameraGroup != null)
            {
                foreach (SCN0CameraNode l in CameraGroup.Children)
                {
                    l.SetSize(_numFrames, Loop);
                }
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_version == 5)
            {
                SCN0v5* header = Header5;
                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
                }

                _numFrames = header->_frameCount;
                _specLights = header->_specLightCount;
                _loop = header->_loop != 0;
                _lightsets = header->_lightSetCount;
                _amblights = header->_ambientCount;
                _lights = header->_lightCount;
                _fogs = header->_fogCount;
                _cameras = header->_cameraCount;

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                (_userEntries = new UserDataCollection()).Read(header->UserData, WorkingUncompressed);

                return header->Group->_numEntries > 0;
            }
            else
            {
                SCN0v4* header = Header4;
                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
                }

                _numFrames = header->_frameCount;
                _specLights = header->_specLightCount;
                _loop = header->_loop != 0;
                _lightsets = header->_lightSetCount;
                _amblights = header->_ambientCount;
                _lights = header->_lightCount;
                _fogs = header->_fogCount;
                _cameras = header->_cameraCount;

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                return header->Group->_numEntries > 0;
            }
        }

        public override void OnPopulate()
        {
            ResourceGroup* group;
            bint* offsets;
            bshort* counts;
            if (Header->_version == 5)
            {
                group = Header5->Group;
                offsets = (bint*) Header5->_lightSetOffset.Address;
                counts = (bshort*) Header5->_lightSetCount.Address;
            }
            else
            {
                group = Header4->Group;
                offsets = (bint*) Header4->_lightSetOffset.Address;
                counts = (bshort*) Header4->_lightSetCount.Address;
            }

            SCN0GroupNode g;
            for (int i = 0; i < group->_numEntries; i++)
            {
                string name = group->First[i].GetName();
                (g = new SCN0GroupNode(name)).Initialize(this, new DataSource(group->First[i].DataAddress, 0));

                int typeID = (int) g._type;
                _groups[typeID] = g;

                Type t = SCN0GroupNode._types[typeID];

                ResourceNode r;
                VoidPtr entryAddr = (VoidPtr) Header + offsets[typeID];
                for (int x = 0, offset = 0, size = 0; x < counts[typeID]; x++, offset += size)
                {
                    r = Activator.CreateInstance(t) as ResourceNode;
                    size = *(bint*) (entryAddr + offset);
                    r.Initialize(g, new DataSource(entryAddr + offset, size));
                }
            }

            if (LightSetGroup != null)
            {
                foreach (SCN0LightSetNode t in LightSetGroup.Children)
                {
                    t.AttachNodes();
                }
            }
        }

        public SCN0GroupNode GetOrCreateFolder<T>() where T : SCN0EntryNode
        {
            return GetOrCreateFolder((SCN0GroupNode.GroupType) SCN0GroupNode._types.IndexOf(typeof(T)));
        }

        public SCN0GroupNode GetOrCreateFolder(SCN0GroupNode.GroupType type)
        {
            int typeID = (int) type;
            if (typeID < 0 || typeID >= 5)
            {
                return null;
            }

            if (_groups[typeID] == null)
            {
                AddChild(_groups[typeID] = new SCN0GroupNode(type));
            }

            return _groups[typeID];
        }

        public SCN0GroupNode GetFolder<T>() where T : SCN0EntryNode
        {
            return GetFolder((SCN0GroupNode.GroupType) SCN0GroupNode._types.IndexOf(typeof(T)));
        }

        public SCN0GroupNode GetFolder(SCN0GroupNode.GroupType type)
        {
            Populate(0);

            int typeID = (int) type;
            if (typeID < 0 || typeID >= 5)
            {
                return null;
            }

            return _groups[typeID];
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SCN0GroupNode n in Children)
            {
                n.GetStrings(table);
            }

            foreach (UserDataClass s in _userEntries)
            {
                table.Add(s._name);
                if (s._type == UserValueType.String && s._entries.Count > 0)
                {
                    table.Add(s._entries[0]);
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            //Reset specular light count so it can be recounted
            _specLights = 0;

            //Add header and resource group size
            int size = SCN0v4.Size + ResourceGroup.Size + Children.Count * ResourceEntry.Size;

            //Add the size of each resource group, and headers and all data
            foreach (SCN0GroupNode n in Children)
            {
                size += n.CalculateSize(true);
            }

            //Add size of user entries
            if (_version == 5)
            {
                size += _userEntries.GetSize();
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            //Create data lengths for each group
            //group, lightset, ambient, light, fog, camera
            int[] lengths = new int[6];

            //Create entry counts for each group
            //lightset, ambient, light, fog, camera
            short[] counts = new short[5];

            //Create addresses for all data sections
            //group, entry, key, color, vis
            VoidPtr[] dataAddrs = new VoidPtr[5];

            //Write header and retrieve main resource group
            ResourceGroup* group;
            if (_version == 5)
            {
                SCN0v5* header = (SCN0v5*) address;

                header->_origPathOffset = 0;
                header->_frameCount = (ushort) _numFrames;
                header->_specLightCount = (ushort) _specLights;
                header->_loop = _loop ? 1 : 0;
                header->_pad = 0;
                header->_dataOffset = SCN0v5.Size;

                group = header->Group;
            }
            else
            {
                SCN0v4* header = (SCN0v4*) address;

                header->_origPathOffset = 0;
                header->_frameCount = (ushort) _numFrames;
                header->_specLightCount = (ushort) _specLights;
                header->_loop = _loop ? 1 : 0;
                header->_pad = 0;
                header->_dataOffset = SCN0v4.Size;

                group = header->Group;
            }

            //Create resource group
            *group = new ResourceGroup(Children.Count);
            lengths[0] = group->_totalSize;

            //Get pointer to main resource entry buffer
            ResourceEntry* entry = group->First;

            //Get pointer to resource groups for each child group
            dataAddrs[0] = group->EndAddress;

            //Get pointer to the address for headers
            for (int i = 0; i < 4; i++)
            {
                dataAddrs[i + 1] = dataAddrs[i];
                foreach (SCN0GroupNode g in Children)
                {
                    if (i == 0)
                    {
                        dataAddrs[i + 1] += g._dataLengths[i];
                    }
                    else
                    {
                        foreach (SCN0EntryNode e in g.Children)
                        {
                            dataAddrs[i + 1] += i == 1 ? e._calcSize : e._dataLengths[i - 2];
                        }
                    }
                }
            }

            //Use an index array to remap and write groups in proper order.
            int[] indices = new int[] {-1, -1, -1, -1, -1};

            //Loop through groups and set index, length and count
            foreach (SCN0GroupNode g in Children)
            {
                int i = (int) g._type;
                indices[i] = g.Index;
                lengths[i + 1] = g._dataLengths[1];
                counts[i] = (short) g.Children.Count;
            }

            //Now loop through indices to get each group and write it
            for (int i = 0; i < 5; i++)
            {
                //Make sure the group exists
                SCN0GroupNode g = indices[i] >= 0 ? Children[indices[i]] as SCN0GroupNode : null;
                if (g != null)
                {
                    //Set offset to group in main resource group
                    (entry++)->_dataOffset = (int) dataAddrs[0] - (int) group;

                    //Set addresses for rebuild
                    for (int x = 0; x < 4; x++)
                    {
                        g._addrs[x] = dataAddrs[x + 1];
                    }

                    //Rebuild focusing on the resource group
                    g.Rebuild(dataAddrs[0], g._dataLengths[0], true);

                    //Increment addresses
                    lengths[0] += g._dataLengths[0];
                    for (int x = 0; x < 5; x++)
                    {
                        dataAddrs[x] += g._dataLengths[x];
                    }
                }
            }

            //Set header values
            if (_version == 5)
            {
                SCN0v5* header = (SCN0v5*) address;
                header->_lightSetCount = counts[0];
                header->_ambientCount = counts[1];
                header->_lightCount = counts[2];
                header->_fogCount = counts[3];
                header->_cameraCount = counts[4];
                header->Set(lengths[0], lengths[1], lengths[2], lengths[3], lengths[4], lengths[5]);

                if (_userEntries.Count > 0)
                {
                    _userEntries.Write(header->UserData = dataAddrs[4]);
                }
            }
            else
            {
                SCN0v4* header = (SCN0v4*) address;
                header->_lightSetCount = counts[0];
                header->_ambientCount = counts[1];
                header->_lightCount = counts[2];
                header->_fogCount = counts[3];
                header->_cameraCount = counts[4];
                header->Set(lengths[0], lengths[1], lengths[2], lengths[3], lengths[4], lengths[5]);
            }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            ResourceGroup* group;
            if (_version == 5)
            {
                SCN0v5* header = (SCN0v5*) dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }

                group = header->Group;
            }
            else
            {
                SCN0v4* header = (SCN0v4*) dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }

                group = header->Group;
            }

            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            int[] indices = new int[] {-1, -1, -1, -1, -1};
            foreach (SCN0GroupNode g in Children)
            {
                indices[(int) g._type] = g.Index;
            }

            VoidPtr addr = dataAddress;
            for (int i = 0; i < 5; i++)
            {
                SCN0GroupNode n = indices[i] >= 0 ? Children[indices[i]] as SCN0GroupNode : null;
                if (n != null)
                {
                    addr = (VoidPtr) group + (rEntry++)->_dataOffset;
                    ResourceEntry.Build(group, index++, addr, (BRESString*) stringTable[n.Name]);
                    n.PostProcess(dataAddress, addr, stringTable);
                }
            }

            if (_version == 5)
            {
                _userEntries.PostProcess(((SCN0v5*) addr)->UserData, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((SCN0v4*) source.Address)->_header._tag == SCN0v4.Tag ? new SCN0Node() : null;
        }

        public T CreateResource<T>(string name) where T : SCN0EntryNode
        {
            SCN0GroupNode group = GetOrCreateFolder<T>();
            if (group == null)
            {
                return null;
            }

            T n = Activator.CreateInstance<T>();
            n.Name = group.FindName(name);
            group.AddChild(n);

            return n;
        }

        public SCN0EntryNode CreateResource(SCN0GroupNode.GroupType type, string name)
        {
            SCN0GroupNode group = GetOrCreateFolder(type);
            if (group == null)
            {
                return null;
            }

            Type t = SCN0GroupNode._types[(int) type];
            SCN0EntryNode n = Activator.CreateInstance(t) as SCN0EntryNode;
            n.Name = group.FindName(name);
            group.AddChild(n);

            return n;
        }
    }
}
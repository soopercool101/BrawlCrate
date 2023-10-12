using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using BrawlLib.Wii.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SHP0Node : NW4RAnimationNode
    {
        internal SHP0v3* Header3 => (SHP0v3*) WorkingUncompressed.Address;
        internal SHP0v4* Header4 => (SHP0v4*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SHP0;
        public override Type[] AllowedChildTypes => new Type[] {typeof(SHP0EntryNode)};
        public override int[] SupportedVersions => new int[] {3, 4};
        public override string Tag => "SHP0";

        public SHP0Node()
        {
            _version = 3;
        }

        private const string _category = "Vertex Morph Animation";

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
            foreach (SHP0EntryNode n in Children)
            {
                foreach (SHP0VertexSetNode s in n.Children)
                {
                    s.SetSize(_numFrames, Loop);
                }
            }
        }

        public void InsertKeyframe(int index)
        {
            FrameCount++;
            foreach (SHP0EntryNode e in Children)
            {
                foreach (SHP0VertexSetNode c in e.Children)
                {
                    c.Keyframes.Insert(index);
                }
            }
        }

        public void DeleteKeyframe(int index)
        {
            foreach (SHP0EntryNode e in Children)
            {
                foreach (SHP0VertexSetNode c in e.Children)
                {
                    c.Keyframes.Delete(index);
                }
            }

            FrameCount--;
        }

        //public string[] StringEntries { get { return _strings.ToArray(); } }
        internal List<string> _strings = new List<string>();

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (string s in _strings)
            {
                table.Add(s);
            }

            if (_version == 4)
            {
                _userEntries.GetStrings(table);
            }

            if (!string.IsNullOrEmpty(_originalPath))
            {
                table.Add(_originalPath);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _strings.Clear();
            if (_version == 4)
            {
                SHP0v4* header = Header4;

                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
                }

                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;

                bint* stringOffset = header->StringEntries;
                for (int i = 0; i < header->_numEntries; i++)
                {
                    _strings.Add(new string((sbyte*) stringOffset + stringOffset[i]));
                }

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                (_userEntries = new UserDataCollection()).Read(header->UserData, WorkingUncompressed);

                return header->Group->_numEntries > 0;
            }
            else
            {
                SHP0v3* header = Header3;

                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
                }

                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;

                bint* stringOffset = header->StringEntries;
                for (int i = 0; i < header->_numEntries; i++)
                {
                    _strings.Add(new string((sbyte*) stringOffset + stringOffset[i]));
                }

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                return header->Group->_numEntries > 0;
            }
        }

        public override void OnPopulate()
        {
            ResourceGroup* group = Header4->Group;
            for (int i = 0; i < group->_numEntries; i++)
            {
                new SHP0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ResourceGroup* group;
            if (_version == 4)
            {
                SHP0v4* header = (SHP0v4*) address;
                *header = new SHP0v4(_loop, (ushort) _numFrames, (ushort) _strings.Count);
                group = header->Group;
            }
            else
            {
                SHP0v3* header = (SHP0v3*) address;
                *header = new SHP0v3(_loop, (ushort) _numFrames, (ushort) _strings.Count);
                group = header->Group;
            }

            *group = new ResourceGroup(Children.Count);

            VoidPtr entryAddress = group->EndAddress;
            VoidPtr dataAddress = entryAddress;

            foreach (SHP0EntryNode n in Children)
            {
                dataAddress += n._entryLen;
            }

            ResourceEntry* rEntry = group->First;
            foreach (SHP0EntryNode n in Children)
            {
                (rEntry++)->_dataOffset = (int) entryAddress - (int) group;

                n._dataAddr = dataAddress;
                n.Rebuild(entryAddress, n._entryLen, true);
                entryAddress += n._entryLen;
                dataAddress += n._dataLen;
            }

            ((SHP0v3*) address)->_stringListOffset = (int) dataAddress - (int) address;

            dataAddress += _strings.Count * 4;

            if (_userEntries.Count > 0 && _version == 4)
            {
                _userEntries.Write(((SHP0v4*) address)->UserData = dataAddress);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _strings.Clear();
            foreach (SHP0EntryNode entry in Children)
            {
                _strings.Add(entry.Name);

                foreach (SHP0VertexSetNode n in entry.Children)
                {
                    _strings.Add(n.Name);
                }
            }

            int size = (Version == 4 ? SHP0v4.Size : SHP0v3.Size) + 0x18 + Children.Count * 0x10 + _strings.Count * 4;
            foreach (SHP0EntryNode entry in Children)
            {
                size += entry.CalculateSize(true) + entry.Children.Count * 4;
            }

            if (_version == 4)
            {
                size += _userEntries.GetSize();
            }

            return size;
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            ResourceGroup* group;
            if (_version == 4)
            {
                SHP0v4* header = (SHP0v4*)dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }

                bint* stringPtr = header->StringEntries;
                for (int i = 0; i < header->_numEntries; i++)
                {
                    stringPtr[i] = (int)stringTable[_strings[i]] + 4 - (int)stringPtr;
                }

                group = header->Group;
            }
            else
            {
                SHP0v3* header = (SHP0v3*)dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }

                bint* stringPtr = header->StringEntries;
                for (int i = 0; i < header->_numEntries; i++)
                {
                    stringPtr[i] = (int)stringTable[_strings[i]] + 4 - (int)stringPtr;
                }

                group = header->Group;
            }


            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SHP0EntryNode n in Children)
            {
                dataAddress = (VoidPtr) group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*) stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }

            if (_version == 4)
            {
                _userEntries.PostProcess(((SHP0v4*) dataAddress)->UserData, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((BRESCommonHeader*) source.Address)->_tag == SHP0v3.Tag ? new SHP0Node() : null;
        }

        public SHP0EntryNode FindOrCreateEntry(string name)
        {
            foreach (SHP0EntryNode t in Children)
            {
                if (t.Name == name)
                {
                    return t;
                }
            }

            SHP0EntryNode entry = new SHP0EntryNode
            {
                Name = name,
                _flags = 3
            };
            AddChild(entry);
            entry.AddChild(new SHP0VertexSetNode(FindName("NewMorphTarget")));
            return entry;
        }

        #region Extra Functions

        /// <summary>
        /// Stretches or compresses all frames of the animation to fit a new frame count specified by the user.
        /// </summary>
        public void Resize()
        {
            FrameCountChanger f = new FrameCountChanger();
            if (f.ShowDialog(FrameCount) == DialogResult.OK)
            {
                Resize(f.NewValue);
            }
        }

        /// <summary>
        /// Stretches or compresses all frames of the animation to fit a new frame count.
        /// </summary>
        public void Resize(int newFrameCount)
        {
            KeyframeEntry kfe = null;
            float ratio = newFrameCount / (float) FrameCount;
            foreach (SHP0EntryNode s in Children)
            {
                foreach (SHP0VertexSetNode e in s.Children)
                {
                    KeyframeArray newArr = new KeyframeArray(newFrameCount);
                    for (int x = 0; x < FrameCount; x++)
                    {
                        if ((kfe = e.GetKeyframe(x)) != null)
                        {
                            int newFrame = (int) (x * ratio + 0.5f);
                            float frameRatio = newFrame == 0 ? 0 : x / (float) newFrame;
                            newArr.SetFrameValue(newFrame, kfe._value)._tangent =
                                kfe._tangent * (float.IsNaN(frameRatio) ? 1 : frameRatio);
                        }
                    }

                    e._keyframes = newArr;
                }
            }

            FrameCount = newFrameCount;
        }

        /// <summary>
        /// Adds an animation opened by the user to the end of this one
        /// </summary>
        public void Append()
        {
            SHP0Node external = null;
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "SHP0 Animation (*.shp0)|*.shp0",
                Title = "Please select an animation to append."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                if ((external = (SHP0Node) NodeFactory.FromFile(null, o.FileName)) != null)
                {
                    Append(external);
                }
            }
        }

        /// <summary>
        /// Adds an animation to the end of this one
        /// </summary>
        public void Append(SHP0Node external)
        {
            KeyframeEntry kfe;

            int origIntCount = FrameCount;
            FrameCount += external.FrameCount;

            foreach (SHP0EntryNode w in external.Children)
            {
                foreach (SHP0VertexSetNode extEntry in w.Children)
                {
                    SHP0VertexSetNode intEntry = null;
                    if ((intEntry = (SHP0VertexSetNode) FindChild(w.Name + "/" + extEntry.Name, false)) == null)
                    {
                        SHP0EntryNode wi = null;
                        if ((wi = (SHP0EntryNode) FindChild(w.Name, false)) == null)
                        {
                            AddChild(wi = new SHP0EntryNode {Name = FindName(null), _flags = w._flags});
                        }

                        SHP0VertexSetNode newIntEntry = new SHP0VertexSetNode(extEntry.Name);
                        newIntEntry.SetSize(extEntry.FrameCount + origIntCount, Loop);
                        for (int x = 0; x < extEntry.FrameCount; x++)
                        {
                            if ((kfe = extEntry.GetKeyframe(x)) != null)
                            {
                                newIntEntry.Keyframes.SetFrameValue(x + origIntCount, kfe._value)._tangent =
                                    kfe._tangent;
                            }
                        }

                        wi.AddChild(newIntEntry);
                    }
                    else
                    {
                        for (int x = 0; x < extEntry.FrameCount; x++)
                        {
                            if ((kfe = extEntry.GetKeyframe(x)) != null)
                            {
                                intEntry.Keyframes.SetFrameValue(x + origIntCount, kfe._value)._tangent = kfe._tangent;
                            }
                        }
                    }
                }
            }
        }

        public void AverageKeys()
        {
            foreach (SHP0EntryNode u in Children)
            {
                foreach (SHP0VertexSetNode w in u.Children)
                {
                    if (w.Keyframes._keyCount > 1)
                    {
                        KeyframeEntry root = w.Keyframes._keyRoot;
                        if (root._next != root && root._prev != root && root._prev != root._next)
                        {
                            float tan = (root._next._tangent + root._prev._tangent) / 2.0f;
                            float val = (root._next._value + root._prev._value) / 2.0f;

                            root._next._tangent = tan;
                            root._prev._tangent = tan;

                            root._next._value = val;
                            root._prev._value = val;
                        }
                    }
                }
            }

            SignalPropertyChange();
        }

        public void AverageKeys(string baseName, string morphName)
        {
            SHP0EntryNode w = FindChild(baseName, false) as SHP0EntryNode;

            SHP0VertexSetNode t = w?.FindChild(morphName, false) as SHP0VertexSetNode;
            if (t == null)
            {
                return;
            }

            if (t.Keyframes._keyCount > 1)
            {
                KeyframeEntry root = t.Keyframes._keyRoot;
                if (root._next != root && root._prev != root && root._prev != root._next)
                {
                    float tan = (root._next._tangent + root._prev._tangent) / 2.0f;
                    float val = (root._next._value + root._prev._value) / 2.0f;

                    root._next._tangent = tan;
                    root._prev._tangent = tan;

                    root._next._value = val;
                    root._prev._value = val;
                }
            }

            SignalPropertyChange();
        }

        #endregion
    }

    public unsafe class SHP0EntryNode : ResourceNode
    {
        internal SHP0Entry* Header => (SHP0Entry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SHP0Entry;
        public override Type[] AllowedChildTypes => new Type[] {typeof(SHP0VertexSetNode)};

        private List<short> _indices;
        public Bin32 _flags = 3;
        public int _indexCount, _fixedFlags;

        [Category("Vertex Morph Entry")]
        public bool Enabled
        {
            get => _flags[0];
            set
            {
                _flags[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Vertex Morph Entry")]
        public bool UpdateVertices
        {
            get => _flags[1];
            set
            {
                _flags[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Vertex Morph Entry")]
        public bool UpdateNormals
        {
            get => _flags[2];
            set
            {
                _flags[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Vertex Morph Entry")]
        public bool UpdateColors
        {
            get => _flags[3];
            set
            {
                _flags[3] = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null && Header->_stringOffset != 0)
            {
                _name = Header->ResourceString;
            }

            _indices = new List<short>();
            for (int i = 0; i < Header->_numIndices; i++)
            {
                _indices.Add(Header->Indicies[i]);
            }

            _flags = (uint) (int) Header->_flags;
            _indexCount = Header->_numIndices;
            _fixedFlags = Header->_fixedFlags;

            return Header->_flags > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < _indexCount; i++)
            {
                string name;
                if (_indices[i] < ((SHP0Node) Parent)._strings.Count && _indices[i] >= 0)
                {
                    name = ((SHP0Node) Parent)._strings[_indices[i]];
                }
                else
                {
                    name = "Unknown";
                }

                SHP0VertexSetNode n = new SHP0VertexSetNode(name);

                if (((_fixedFlags >> i) & 1) == 0)
                {
                    n.Initialize(this, new DataSource(Header->GetEntry(i), 0x14 + Header->_numIndices * 6));
                }
                else
                {
                    n._isFixed = true;
                    _children.Add(n);
                    n._parent = this;

                    n.Keyframes[0] = ((bfloat*) Header->EntryOffset)[i];
                }
            }
        }

        public VoidPtr _dataAddr;
        public int _dataLen, _entryLen;

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            SHP0Entry* header = (SHP0Entry*) dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        public override int OnCalculateSize(bool force)
        {
            _entryLen = (0x14 + Children.Count * 6).Align(4);
            _dataLen = 0;

            foreach (SHP0VertexSetNode p in Children)
            {
                _dataLen += p.CalculateSize(true);
            }

            return _entryLen + _dataLen;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SHP0Entry* header = (SHP0Entry*) address;
            VoidPtr addr = _dataAddr;
            header->_numIndices = (short) Children.Count;
            header->_nameIndex = (short) ((SHP0Node) Parent)._strings.IndexOf(Name);
            header->_flags = (int) (uint) _flags;
            header->_indiciesOffset = 0x14 + Children.Count * 4;
            uint fixedflags = 0;
            foreach (SHP0VertexSetNode p in Children)
            {
                p._dataAddr = addr;
                header->Indicies[p.Index] = (short) ((SHP0Node) Parent)._strings.IndexOf(p.Name);
                if (p._isFixed)
                {
                    float value = 0;
                    KeyframeEntry kf;
                    if ((kf = p.Keyframes.GetKeyframe(0)) != null)
                    {
                        value = kf._value;
                    }

                    ((bfloat*) header->EntryOffset)[p.Index] = value;
                    fixedflags |= 1u << p.Index;
                }
                else
                {
                    header->EntryOffset[p.Index] = (int) p._dataAddr - (int) &header->EntryOffset[p.Index];
                    p.Rebuild(p._dataAddr, p._calcSize, true);
                }

                addr += p._dataLen;
            }

            header->_fixedFlags = (int) fixedflags;
        }

        public SHP0VertexSetNode FindOrCreateEntry(string name)
        {
            foreach (SHP0VertexSetNode t in Children)
            {
                if (t.Name == name)
                {
                    return t;
                }
            }

            SHP0VertexSetNode entry = new SHP0VertexSetNode(name);
            AddChild(entry);
            return entry;
        }

        public void CreateEntry()
        {
            SHP0VertexSetNode morph = new SHP0VertexSetNode(FindName("NewMorphTarget"));
            AddChild(morph);
        }
    }

    public unsafe class SHP0VertexSetNode : ResourceNode, IKeyframeSource
    {
        internal SHP0KeyframeEntries* Header => (SHP0KeyframeEntries*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SHP0VertexSet;

        public int _dataLen;
        public VoidPtr _dataAddr;

        internal void SetSize(int count, bool looped)
        {
            Keyframes.FrameLimit = count + (looped ? 1 : 0);
            Keyframes.Loop = looped;
            SignalPropertyChange();
        }

        [Browsable(false)] public int FrameCount => Keyframes.FrameLimit;

        internal KeyframeArray _keyframes;

        [Browsable(false)]
        public KeyframeArray Keyframes
        {
            get
            {
                if (_keyframes == null)
                {
                    _keyframes =
                        AnimationConverter.DecodeSHP0Keyframes(Header,
                            Parent != null ? Parent.Parent as SHP0Node : null);
                }

                return _keyframes;
            }
        }

        public bool _isFixed;
        public float _fixedValue;

        public SHP0VertexSetNode(string name)
        {
            _name = name;
        }

        public override bool OnInitialize()
        {
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            _isFixed = Keyframes._keyCount <= 1;
            return _dataLen = _isFixed ? 0 : Keyframes._keyCount * 12 + 4;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SHP0KeyframeEntries* header = (SHP0KeyframeEntries*) address;
            header->_numEntries = (short) Keyframes._keyCount;
            header->_unk1 = 0;

            BVec3* entry = header->Entries;
            KeyframeEntry frame, root = Keyframes._keyRoot;
            for (frame = root._next; frame._index != -1; frame = frame._next)
            {
                *entry++ = new Vector3(frame._tangent, frame._index, frame._value);
            }
        }

        public override void Export(string outPath)
        {
            int length = OnCalculateSize(true);
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(length);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    OnRebuild(map.Address, length, true);
                }
            }
        }

        #region Keyframe Management

        public static bool _generateTangents = true;

        public KeyframeEntry GetKeyframe(int index)
        {
            return Keyframes.GetKeyframe(index);
        }

        public void SetKeyframe(int index, float value)
        {
            bool exists = Keyframes.GetKeyframe(index) != null;
            KeyframeEntry k = Keyframes.SetFrameValue(index, value);

            if (!exists && !_generateTangents)
            {
                k.GenerateTangent();
            }

            if (_generateTangents)
            {
                k.GenerateTangent();
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
            }

            SignalPropertyChange();
        }

        public void RemoveKeyframe(int index)
        {
            KeyframeEntry k = Keyframes.Remove(index);
            if (k != null && _generateTangents)
            {
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
                SignalPropertyChange();
            }
        }

        #endregion

        [Browsable(false)] public KeyframeArray[] KeyArrays => new KeyframeArray[] {Keyframes};
    }

    public class SHP0Keyframe
    {
        public float _index, _percentage, _tangent;

        public static implicit operator SHP0Keyframe(Vector3 v)
        {
            return new SHP0Keyframe(v._x, v._y, v._z);
        }

        public static implicit operator Vector3(SHP0Keyframe v)
        {
            return new Vector3(v._tangent, v._index, v._percentage);
        }

        public SHP0Keyframe(float tan, float index, float percent)
        {
            _index = index;
            _percentage = percent;
            _tangent = tan;
        }

        public SHP0Keyframe()
        {
        }

        public float Index
        {
            get => _index;
            set => _index = value;
        }

        public float Percentage
        {
            get => _percentage;
            set => _percentage = value;
        }

        public float Tangent
        {
            get => _tangent;
            set => _tangent = value;
        }

        public override string ToString()
        {
            return $"Index={_index}, Percentage={$"{_percentage * 100.0f}%"}, Tangent={_tangent}";
        }
    }
}
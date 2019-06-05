using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Animations;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SRT0Node : NW4RAnimationNode
    {
        private const string _category = "Texture Coordinate Animation";
        private int _matrixMode;

        public SRT0Node()
        {
            _version = 4;
        }

        internal SRT0v4* Header4 => (SRT0v4*) WorkingUncompressed.Address;
        internal SRT0v5* Header5 => (SRT0v5*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SRT0;
        public override Type[] AllowedChildTypes => new[] {typeof(SRT0EntryNode)};
        public override int[] SupportedVersions => new[] {4, 5};

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

        [Category(_category)]
        [Description(MDL0Node._textureMatrixModeDescription)]
        public TexMatrixMode MatrixMode
        {
            get => (TexMatrixMode) _matrixMode;
            set
            {
                _matrixMode = (int) value;
                SignalPropertyChange();
            }
        }

        protected override void UpdateChildFrameLimits()
        {
            foreach (SRT0EntryNode n in Children)
            foreach (SRT0TextureNode t in n.Children)
                t.SetSize(_numFrames, Loop);
        }

        public void InsertKeyframe(int index)
        {
            FrameCount++;
            foreach (SRT0EntryNode e in Children)
            foreach (SRT0TextureNode c in e.Children)
                c.Keyframes.Insert(index, 0, 1, 2, 3, 4);
        }

        public void DeleteKeyframe(int index)
        {
            foreach (SRT0EntryNode e in Children)
            foreach (SRT0TextureNode c in e.Children)
                c.Keyframes.Delete(index, 0, 1, 2, 3, 4);

            FrameCount--;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_version == 5)
            {
                var header = Header5;
                if (_name == null && header->_stringOffset != 0) _name = header->ResourceString;

                _loop = header->_loop != 0;
                _numFrames = header->_numFrames;
                _matrixMode = header->_matrixMode;

                if (header->_origPathOffset > 0) _originalPath = header->OrigPath;
                (_userEntries = new UserDataCollection()).Read(header->UserData);

                return header->Group->_numEntries > 0;
            }
            else
            {
                var header = Header4;
                if (_name == null && header->_stringOffset != 0) _name = header->ResourceString;

                _loop = header->_loop != 0;
                _numFrames = header->_numFrames;
                _matrixMode = header->_matrixMode;

                if (header->_origPathOffset > 0) _originalPath = header->OrigPath;

                return header->Group->_numEntries > 0;
            }
        }

        public override void OnPopulate()
        {
            VoidPtr addr;
            var group = Header4->Group;
            for (var i = 0; i < group->_numEntries; i++)
                new SRT0EntryNode().Initialize(this,
                    new DataSource(addr = (VoidPtr) group + group->First[i]._dataOffset,
                        ((SRT0Entry*) addr)->DataSize()));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SRT0EntryNode n in Children) table.Add(n.Name);

            if (_version == 5) _userEntries.GetStrings(table);

            if (!string.IsNullOrEmpty(_originalPath)) table.Add(_originalPath);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
            StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            var header = (SRT0v4*) dataAddress;

            if (_version == 5)
            {
                ((SRT0v5*) dataAddress)->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                    ((SRT0v5*) dataAddress)->OrigPathAddress = stringTable[_originalPath] + 4;
            }
            else
            {
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath)) header->OrigPathAddress = stringTable[_originalPath] + 4;
            }

            var group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            var rEntry = group->First;

            var index = 1;
            foreach (SRT0EntryNode n in Children)
            {
                dataAddress = (VoidPtr) group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*) stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }

            if (_version == 5) _userEntries.PostProcess(((SRT0v5*) dataAddress)->UserData, stringTable);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ResourceGroup* group;
            if (_version == 5)
            {
                var header = (SRT0v5*) address;
                *header = new SRT0v5((ushort) _numFrames, _loop, (ushort) Children.Count, _matrixMode);
                group = header->Group;
            }
            else
            {
                var header = (SRT0v4*) address;
                *header = new SRT0v4((ushort) _numFrames, _loop, (ushort) Children.Count, _matrixMode);
                group = header->Group;
            }

            *group = new ResourceGroup(Children.Count);

            var entryAddress = group->EndAddress;
            var dataAddress = entryAddress;
            foreach (SRT0EntryNode n in Children) dataAddress += n._entryLength;

            var rEntry = group->First;
            foreach (SRT0EntryNode n in Children)
            {
                (rEntry++)->_dataOffset = (int) entryAddress - (int) group;

                n._dataAddr = dataAddress;
                n.Rebuild(entryAddress, n._entryLength, true);
                entryAddress += n._entryLength;
                dataAddress += n._dataLength;
            }

            if (_userEntries.Count > 0 && _version == 5)
            {
                var header = (SRT0v5*) address;
                header->UserData = dataAddress;
                _userEntries.Write(dataAddress);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            var size = (Version == 5 ? SRT0v5.Size : SRT0v4.Size) + 0x18 + Children.Count * 0x10;
            foreach (SRT0EntryNode entry in Children) size += entry.CalculateSize(true);

            if (_version == 5) size += _userEntries.GetSize();

            return size;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((BRESCommonHeader*) source.Address)->_tag == SRT0v4.Tag ? new SRT0Node() : null;
        }

        public SRT0TextureNode FindOrCreateEntry(string name, int index, bool ind)
        {
            foreach (SRT0EntryNode t in Children)
                if (t.Name == name)
                {
                    var value = 0;
                    foreach (SRT0TextureNode x in t.Children)
                        if (x._textureIndex == value && x._indirect == ind)
                            return x;

                    var child = new SRT0TextureNode(index, ind);
                    t.AddChild(child);
                    return child;
                }

            var entry = new SRT0EntryNode
            {
                Name = name
            };
            AddChild(entry);
            var tex = new SRT0TextureNode(index, ind);
            entry.AddChild(tex);
            return tex;
        }

        public void CreateEntry()
        {
            AddChild(new SRT0EntryNode {Name = FindName("NewMaterial")});
        }

        #region Extra Functions

        /// <summary>
        ///     Stretches or compresses all frames of the animation to fit a new frame count specified by the user.
        /// </summary>
        public void Resize()
        {
            var f = new FrameCountChanger();
            if (f.ShowDialog(FrameCount) == DialogResult.OK) Resize(f.NewValue);
        }

        /// <summary>
        ///     Stretches or compresses all frames of the animation to fit a new frame count.
        /// </summary>
        public void Resize(int newFrameCount)
        {
            KeyframeEntry kfe = null;
            var ratio = newFrameCount / (float) FrameCount;
            foreach (SRT0EntryNode n in Children)
            foreach (SRT0TextureNode e in n.Children)
            {
                var newCollection = new KeyframeCollection(5, newFrameCount + (Loop ? 1 : 0), 1, 1);
                for (var x = 0; x < FrameCount; x++)
                {
                    var newFrame = (int) (x * ratio + 0.5f);
                    var frameRatio = newFrame == 0 ? 0 : x / (float) newFrame;
                    for (var i = 0; i < 5; i++)
                        if ((kfe = e.GetKeyframe(i, x)) != null)
                            newCollection.SetFrameValue(i, newFrame, kfe._value)._tangent =
                                kfe._tangent * (float.IsNaN(frameRatio) ? 1 : frameRatio);
                }

                e._keyframes = newCollection;
            }

            FrameCount = newFrameCount;
        }

        /// <summary>
        ///     Adds an animation opened by the user to the end of this one
        /// </summary>
        public void Append()
        {
            SRT0Node external = null;
            var o = new OpenFileDialog
            {
                Filter = "SRT0 Animation (*.srt0)|*.srt0",
                Title = "Please select an animation to append."
            };
            if (o.ShowDialog() == DialogResult.OK)
                if ((external = (SRT0Node) NodeFactory.FromFile(null, o.FileName)) != null)
                    Append(external);
        }

        /// <summary>
        ///     Adds an animation to the end of this one
        /// </summary>
        public void Append(SRT0Node external)
        {
            KeyframeEntry kfe;

            var origIntCount = FrameCount;
            FrameCount += external.FrameCount;

            foreach (SRT0EntryNode w in external.Children)
            foreach (SRT0TextureNode extEntry in w.Children)
            {
                SRT0TextureNode intEntry = null;
                if ((intEntry = (SRT0TextureNode) FindChild(w.Name + "/" + extEntry.Name, false)) == null)
                {
                    SRT0EntryNode wi = null;
                    if ((wi = (SRT0EntryNode) FindChild(w.Name, false)) == null)
                        AddChild(wi = new SRT0EntryNode {Name = FindName(null)});

                    var newIntEntry = new SRT0TextureNode(extEntry.Index, extEntry.Indirect) {Name = extEntry.Name};
                    newIntEntry.SetSize(extEntry.FrameCount + origIntCount, Loop);

                    for (var x = 0; x < extEntry.FrameCount; x++)
                    for (var i = 0; i < 5; i++)
                        if ((kfe = extEntry.GetKeyframe(i, x)) != null)
                            newIntEntry.Keyframes.SetFrameValue(i, x + origIntCount, kfe._value)._tangent =
                                kfe._tangent;

                    wi.AddChild(newIntEntry);
                }
                else
                {
                    for (var x = 0; x < extEntry.FrameCount; x++)
                    for (var i = 0; i < 5; i++)
                        if ((kfe = extEntry.GetKeyframe(i, x)) != null)
                            intEntry.Keyframes.SetFrameValue(i, x + origIntCount, kfe._value)._tangent = kfe._tangent;
                }
            }
        }

        public void AverageKeys()
        {
            foreach (SRT0EntryNode u in Children)
            foreach (SRT0TextureNode w in u.Children)
                for (var i = 0; i < 5; i++)
                    if (w.Keyframes._keyArrays[i]._keyCount > 1)
                    {
                        var root = w.Keyframes._keyArrays[i]._keyRoot;
                        if (root._next != root && root._prev != root && root._prev != root._next)
                        {
                            var tan = (root._next._tangent + root._prev._tangent) / 2.0f;
                            var val = (root._next._value + root._prev._value) / 2.0f;

                            root._next._tangent = tan;
                            root._prev._tangent = tan;

                            root._next._value = val;
                            root._prev._value = val;
                        }
                    }

            SignalPropertyChange();
        }

        public void AverageKeys(string matName, int index)
        {
            var w = FindChild(matName, false) as SRT0EntryNode;
            if (w == null) return;

            if (index < 0 || index >= w.Children.Count) return;

            var t = w.Children[index] as SRT0TextureNode;

            for (var i = 0; i < 5; i++)
                if (t.Keyframes._keyArrays[i]._keyCount > 1)
                {
                    var root = t.Keyframes._keyArrays[i]._keyRoot;
                    if (root._next != root && root._prev != root && root._prev != root._next)
                    {
                        var tan = (root._next._tangent + root._prev._tangent) / 2.0f;
                        var val = (root._next._value + root._prev._value) / 2.0f;

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

    public unsafe class SRT0EntryNode : ResourceNode
    {
        internal VoidPtr _dataAddr;

        internal int _entryLength, _dataLength;
        public IndirectTextureIndices _indIndices;

        public TextureIndices _texIndices;

        public int[] _usageIndices = new int[11];
        internal SRT0Entry* Header => (SRT0Entry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SRT0Entry;
        public override Type[] AllowedChildTypes => new[] {typeof(SRT0TextureNode)};

        public override bool OnInitialize()
        {
            if (_name == null && Header->_stringOffset != 0) _name = Header->ResourceString;

            _texIndices = (TextureIndices) (int) Header->_textureIndices;
            _indIndices = (IndirectTextureIndices) (int) Header->_indirectIndices;

            for (var i = 0; i < 8; i++) _usageIndices[i] = (Header->_textureIndices >> i) & 1;

            for (var i = 0; i < 3; i++) _usageIndices[i + 8] = (Header->_indirectIndices >> i) & 1;

            return _texIndices > 0 || _indIndices > 0;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            var header = (SRT0Entry*) dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        public override void OnPopulate()
        {
            VoidPtr addr;
            var index = 0;
            for (var i = 0; i < 11; i++)
                if (_usageIndices[i] == 1)
                    new SRT0TextureNode(i >= 8 ? i - 8 : i, i >= 8).Initialize(this,
                        new DataSource(addr = Header->GetEntry(index++), ((SRT0TextureEntry*) addr)->Code.DataSize()));
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (SRT0Entry*) address;

            header->_textureIndices = (int) _texIndices;
            header->_indirectIndices = (int) _indIndices;

            var offset = 12 + Children.Count * 4;
            var entryAddress = address + offset;

            var prevOffset = 0;
            for (var i = 0; i < Children.Count; i++)
            {
                var n = (SRT0TextureNode) Children[i];

                n._dataAddr = _dataAddr;

                header->SetOffset(i, offset + prevOffset);
                n.Rebuild(entryAddress, n._entryLength, true);

                entryAddress += n._entryLength;
                prevOffset += n._entryLength;
                _dataAddr += n._dataLength;
            }
        }

        public override void Export(string outPath)
        {
            var table = new StringTable {Name};

            var totalLength = OnCalculateSize(true) + table.GetTotalSize();
            using (var stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8,
                FileOptions.RandomAccess))
            {
                stream.SetLength(totalLength);
                using (var map = FileMap.FromStream(stream))
                {
                    _dataAddr = map.Address + _entryLength;
                    Rebuild(map.Address, totalLength, true);
                    table.WriteTable(_dataAddr + _dataLength);
                    PostProcess(map.Address, table);
                }
            }

            //string temp1 = Path.GetTempFileName();
            //string temp2 = Path.GetTempFileName();

            //Parent.Rebuild();
            //Parent.Export(temp1);

            //int index = Parent.Children.IndexOf(this);

            //using (ResourceNode copied = NodeFactory.FromFile(null, temp1)) {
            //    for (int i = 0; i < index; i++) {
            //        copied.RemoveChild(copied.Children[0]);
            //    }
            //    while (copied.Children.Count > 1) {
            //        copied.RemoveChild(copied.Children[1]);
            //    }
            //    copied.Export(temp2);
            //}

            //using (ResourceNode copied = NodeFactory.FromFile(null, temp2)) {
            //    SRT0EntryNode entry = (SRT0EntryNode)copied.Children[0];
            //    int dataLen = entry.OnCalculateSize(true);
            //    using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess)) {
            //        stream.SetLength(dataLen);
            //        using (FileMap map = FileMap.FromStream(stream))
            //            Memory.Move(map.Address, entry.WorkingSource.Address, (uint)dataLen);
            //    }
            //}

            //File.Delete(temp1);
            //File.Delete(temp2);
        }

        public override int OnCalculateSize(bool force)
        {
            _texIndices = 0;
            _indIndices = 0;
            _dataLength = 0;
            _entryLength = 12;
            foreach (SRT0TextureNode n in Children)
            {
                if (n._indirect)
                    _indIndices |= (IndirectTextureIndices) (1 << n._textureIndex);
                else
                    _texIndices |= (TextureIndices) (1 << n._textureIndex);

                n.CalculateSize(true);
                _entryLength += 4 + n._entryLength;
                _dataLength += n._dataLength;
            }

            return _entryLength + _dataLength;
        }

        public void CreateEntry()
        {
            var indirect = false;
            var value = 0;
            foreach (SRT0TextureNode t in Children)
                if (t._textureIndex == value && !t._indirect)
                    value++;

            if (value == 8)
            {
                indirect = true;
                value = 0;
                foreach (SRT0TextureNode t in Children)
                    if (t._textureIndex == value && t._indirect)
                        value++;

                if (value == 3) return;
            }

            var node = new SRT0TextureNode(value, indirect);
            AddChild(node);
        }
    }

    public unsafe class SRT0TextureNode : ResourceNode, IKeyframeSource
    {
        internal SRT0TextureEntry* Header => (SRT0TextureEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SRT0Texture;

#if DEBUG
        [Category("SRT0 Texture Entry")]
        public SRT0Code Flags => _code;
#endif

        public bool _indirect;

        [Category("SRT0 Texture Entry")]
        public bool Indirect
        {
            get => _indirect;
            set
            {
                foreach (SRT0TextureNode t in Parent.Children)
                    if (t.Index != Index && t._textureIndex == _textureIndex && t._indirect == value)
                        return;

                _indirect = value;

                Name = (_indirect ? "Ind" : "") + "Texture" + _textureIndex;

                CheckNext();
                CheckPrev();
            }
        }

        [Category("SRT0 Texture Entry")]
        public int TextureIndex
        {
            get => _textureIndex;
            set
            {
                var val = _indirect ? value.Clamp(0, 2) : value.Clamp(0, 7);

                foreach (SRT0TextureNode t in Parent.Children)
                    if (t.Index != Index && t._textureIndex == val && t._indirect == _indirect)
                        return;

                _textureIndex = val;

                Name = (_indirect ? "Ind" : "") + "Texture" + _textureIndex;

                CheckNext();
                CheckPrev();
            }
        }

        public int _textureIndex;

        public SRT0TextureNode(int index, bool indirect)
        {
            _textureIndex = index;
            _name = (indirect ? "Ind" : "") + "Texture" + index;
            _indirect = indirect;
        }

        public void CheckNext()
        {
            if (Index == Parent.Children.Count - 1) return;

            var index = Index;
            if (_indirect && ((SRT0TextureNode) Parent.Children[Index + 1])._indirect == false ||
                _textureIndex > ((SRT0TextureNode) Parent.Children[Index + 1])._textureIndex &&
                _indirect == ((SRT0TextureNode) Parent.Children[Index + 1])._indirect)
            {
                DoMoveDown();
                if (index != Index) CheckNext();
            }
        }

        public void CheckPrev()
        {
            if (Index == 0) return;

            var index = Index;
            if (_indirect == false && ((SRT0TextureNode) Parent.Children[Index - 1])._indirect ||
                _textureIndex < ((SRT0TextureNode) Parent.Children[Index - 1])._textureIndex &&
                _indirect == ((SRT0TextureNode) Parent.Children[Index - 1])._indirect)
            {
                DoMoveUp();
                if (index != Index) CheckPrev();
            }
        }

        internal void SetSize(int count, bool looped)
        {
            Keyframes.FrameLimit = count + (looped ? 1 : 0);
            Keyframes.Loop = looped;
            SignalPropertyChange();
        }

        [Browsable(false)] public int FrameCount => Keyframes.FrameLimit;

        internal KeyframeCollection _keyframes;

        [Browsable(false)]
        public KeyframeCollection Keyframes
        {
            get
            {
                if (_keyframes == null)
                {
                    Debug.WriteLine((IntPtr) Header);
                    _keyframes = AnimationConverter.DecodeKeyframes(Header,
                        Parent != null ? Parent.Parent as SRT0Node : null, 5, 1, 1);
                }

                return _keyframes;
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null) _name = (_indirect ? "Ind" : "") + "Texture" + _textureIndex;

            _code = Header->Code;

            _keyframes = null;

            return false;
        }

        internal int _dataLength;
        internal int _entryLength;
        internal VoidPtr _dataAddr;
        private SRT0Code _code;

        internal SRT0Code Code => _code;

        public override int OnCalculateSize(bool force)
        {
            _dataLength = AnimationConverter.CalculateSRT0Size(Keyframes, out _entryLength, out _code);
            return _dataLength + _entryLength;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AnimationConverter.EncodeSRT0Keyframes(_keyframes, address, _dataAddr, _code);
        }

        public override void Export(string outPath)
        {
            var dataLen = OnCalculateSize(true);
            using (var stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8,
                FileOptions.RandomAccess))
            {
                stream.SetLength(dataLen);
                using (var map = FileMap.FromStream(stream))
                {
                    AnimationConverter.EncodeSRT0Keyframes(Keyframes, map.Address, map.Address + _entryLength, _code);
                }
            }
        }

        #region Keyframe Management

        public static bool _generateTangents = true;

        public float GetFrameValue(int arrayIndex, float index)
        {
            return Keyframes.GetFrameValue(arrayIndex, index);
        }

        public KeyframeEntry GetKeyframe(int arrayIndex, int index)
        {
            return Keyframes.GetKeyframe(arrayIndex, index);
        }

        public KeyframeEntry SetKeyframe(int arrayIndex, int index, float value)
        {
            var exists = Keyframes.GetKeyframe(arrayIndex, index) != null;
            var k = Keyframes.SetFrameValue(arrayIndex, index, value);

            if (!exists && !_generateTangents) k.GenerateTangent();

            if (_generateTangents)
            {
                k.GenerateTangent();
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
            }

            SignalPropertyChange();
            return k;
        }

        public void SetKeyframe(int index, SRTAnimationFrame frame)
        {
            var v = (float*) &frame;
            for (var i = 0; i < 5; i++) SetKeyframe(i, index, *v++);
        }

        public void SetKeyframeOnlyTrans(int index, SRTAnimationFrame frame)
        {
            var v = (float*) &frame.Translation;
            for (var i = 3; i < 5; i++) SetKeyframe(i, index, *v++);
        }

        public void SetKeyframeOnlyRot(int index, SRTAnimationFrame frame)
        {
            SetKeyframe(2, index, frame.Rotation);
        }

        public void SetKeyframeOnlyScale(int index, SRTAnimationFrame frame)
        {
            var v = (float*) &frame.Scale;
            for (var i = 0; i < 2; i++) SetKeyframe(i, index, *v++);
        }

        public void SetKeyframeOnlyTrans(int index, Vector2 trans)
        {
            var v = (float*) &trans;
            for (var i = 3; i < 5; i++) SetKeyframe(i, index, *v++);
        }

        public void SetKeyframeOnlyRot(int index, float rot)
        {
            SetKeyframe(2, index, rot);
        }

        public void SetKeyframeOnlyScale(int index, Vector2 scale)
        {
            var v = (float*) &scale;
            for (var i = 0; i < 2; i++) SetKeyframe(i, index, *v++);
        }

        public void RemoveKeyframe(int arrayIndex, int index)
        {
            var k = Keyframes.Remove(arrayIndex, index);
            if (k != null && _generateTangents)
            {
                k.GenerateTangent();
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
            }
        }

        public void RemoveKeyframe(int index)
        {
            for (var i = 0; i < 5; i++) RemoveKeyframe(i, index);
        }

        public void RemoveKeyframeOnlyTrans(int index)
        {
            for (var i = 3; i < 5; i++) RemoveKeyframe(i, index);
        }

        public void RemoveKeyframeOnlyRot(int index)
        {
            RemoveKeyframe(2, index);
        }

        public void RemoveKeyframeOnlyScale(int index)
        {
            for (var i = 0; i < 2; i++) RemoveKeyframe(i, index);
        }

        public SRTAnimationFrame GetAnimFrame(int index)
        {
            var frame = new SRTAnimationFrame {Index = index};
            var dPtr = (float*) &frame;
            for (var x = 0; x < 5; x++)
            {
                frame.SetBool(x, Keyframes.GetKeyframe(x, index) != null);
                *dPtr++ = GetFrameValue(x, index);
            }

            return frame;
        }

        #endregion

        [Browsable(false)] public KeyframeArray[] KeyArrays => Keyframes._keyArrays;
    }

    public class SRT0FrameEntry
    {
        public float _index, _value, _tangent;

        public SRT0FrameEntry(float x, float y, float z)
        {
            _index = x;
            _value = y;
            _tangent = z;
        }

        public SRT0FrameEntry()
        {
        }

        public float Index
        {
            get => _index;
            set => _index = value;
        }

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public float Tangent
        {
            get => _tangent;
            set => _tangent = value;
        }

        public static implicit operator SRT0FrameEntry(I12Entry v)
        {
            return new SRT0FrameEntry(v._index, v._value, v._tangent);
        }

        public static implicit operator I12Entry(SRT0FrameEntry v)
        {
            return new I12Entry(v._index, v._value, v._tangent);
        }

        public static implicit operator SRT0FrameEntry(Vector3 v)
        {
            return new SRT0FrameEntry(v._x, v._y, v._z);
        }

        public static implicit operator Vector3(SRT0FrameEntry v)
        {
            return new Vector3(v._index, v._value, v._tangent);
        }

        public override string ToString()
        {
            return string.Format("Index={0}, Value={1}, Tangent={2}", _index, _value, _tangent);
        }
    }
}
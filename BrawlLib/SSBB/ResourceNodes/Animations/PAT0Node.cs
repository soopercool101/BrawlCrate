using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PAT0Node : NW4RAnimationNode
    {
        internal PAT0v3* Header3 => (PAT0v3*) WorkingUncompressed.Address;
        internal PAT0v4* Header4 => (PAT0v4*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.PAT0;
        public override Type[] AllowedChildTypes => new Type[] {typeof(PAT0EntryNode)};
        public override int[] SupportedVersions => new int[] {3, 4};
        public override string Tag => "PAT0";

        internal List<string> _textureFiles = new List<string>();
        internal List<string> _paletteFiles = new List<string>();

        public PAT0Node()
        {
            _version = 3;
        }

        private const string _category = "Texture Pattern Animation";

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
            set => base.Loop = value;
        }

        [Category(_category)] public string[] Textures => _textureFiles.ToArray();
        [Category(_category)] public string[] Palettes => _paletteFiles.ToArray();

        public void RegenerateTextureList()
        {
            int index = -1;

            //Make sure all texture names are assigned before clearing the list
            Populate();

            //Reset list to recollect
            _textureFiles.Clear();

            //First, collect texture names
            foreach (PAT0EntryNode n in Children)
            {
                foreach (PAT0TextureNode t in n.Children)
                {
                    foreach (PAT0TextureEntryNode e in t.Children)
                    {
                        if (t._hasTex && !string.IsNullOrEmpty(e._tex))
                        {
                            if ((index = _textureFiles.IndexOf(e._tex)) < 0)
                            {
                                _textureFiles.Add(e._tex);
                            }
                        }
                        else
                        {
                            e._texFileIndex = 0;
                        }
                    }
                }
            }

            //Sort them
            _textureFiles.Sort();

            //Now assign indices into the sorted list
            foreach (PAT0EntryNode n in Children)
            {
                foreach (PAT0TextureNode t in n.Children)
                {
                    foreach (PAT0TextureEntryNode e in t.Children)
                    {
                        if (t._hasTex && !string.IsNullOrEmpty(e._tex))
                        {
                            e._texFileIndex = (ushort) _textureFiles.IndexOf(e._tex);
                        }
                    }
                }
            }
        }

        public void RegeneratePaletteList()
        {
            int index = -1;

            //Make sure all palette names are assigned before clearing the list
            Populate();

            //Reset list to recollect
            _paletteFiles.Clear();

            //First, collect palette names
            foreach (PAT0EntryNode n in Children)
            {
                foreach (PAT0TextureNode t in n.Children)
                {
                    foreach (PAT0TextureEntryNode e in t.Children)
                    {
                        if (t._hasPlt && !string.IsNullOrEmpty(e._plt))
                        {
                            if ((index = _paletteFiles.IndexOf(e._plt)) < 0)
                            {
                                _paletteFiles.Add(e._plt);
                            }
                        }
                        else
                        {
                            e._pltFileIndex = 0;
                        }
                    }
                }
            }

            //Sort them
            _paletteFiles.Sort();

            //Now assign indices into the sorted list
            foreach (PAT0EntryNode n in Children)
            {
                foreach (PAT0TextureNode t in n.Children)
                {
                    foreach (PAT0TextureEntryNode e in t.Children)
                    {
                        if (t._hasPlt && !string.IsNullOrEmpty(e._plt))
                        {
                            e._pltFileIndex = (ushort) _paletteFiles.IndexOf(e._plt);
                        }
                    }
                }
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _textureFiles.Clear();
            _paletteFiles.Clear();

            int texPtr, pltPtr;
            if (_version == 4)
            {
                PAT0v4* header = Header4;
                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;
                texPtr = header->_numTexPtr;
                pltPtr = header->_numPltPtr;
                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
                }

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                (_userEntries = new UserDataCollection()).Read(header->UserData, WorkingUncompressed);

                //Get texture strings
                for (int i = 0; i < texPtr; i++)
                {
                    _textureFiles.Add(header->GetTexStringEntry(i));
                }

                //Get palette strings
                for (int i = 0; i < pltPtr; i++)
                {
                    _paletteFiles.Add(header->GetPltStringEntry(i));
                }

                return header->Group->_numEntries > 0;
            }
            else
            {
                PAT0v3* header = Header3;
                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;
                texPtr = header->_numTexPtr;
                pltPtr = header->_numPltPtr;

                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
                }

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                //Get texture strings
                for (int i = 0; i < texPtr; i++)
                {
                    _textureFiles.Add(header->GetTexStringEntry(i));
                }

                //Get palette strings
                for (int i = 0; i < pltPtr; i++)
                {
                    _paletteFiles.Add(header->GetPltStringEntry(i));
                }

                return header->Group->_numEntries > 0;
            }
        }

        public override void OnPopulate()
        {
            ResourceGroup* group = Header3->Group;
            for (int i = 0; i < group->_numEntries; i++)
            {
                new PAT0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, PAT0Pattern.Size));
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (PAT0EntryNode n in Children)
            {
                table.Add(n.Name);
                foreach (PAT0TextureNode t in n.Children)
                {
                    foreach (PAT0TextureEntryNode e in t.Children)
                    {
                        table.Add(e.Name);
                    }
                }
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

        public override int OnCalculateSize(bool force)
        {
            RegenerateTextureList();
            RegeneratePaletteList();

            int size = PAT0v3.Size + 0x18 + Children.Count * 0x10;
            size += (_textureFiles.Count + _paletteFiles.Count) * 8;
            foreach (PAT0EntryNode n in Children)
            {
                size += n.CalculateSize(true);
            }

            if (_version == 4)
            {
                size += _userEntries.GetSize();
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            //Set header values
            if (_version == 4)
            {
                PAT0v4* header = (PAT0v4*) address;
                header->_header._tag = PAT0v4.Tag;
                header->_header._version = 4;
                header->_dataOffset = PAT0v4.Size;
                header->_userDataOffset = header->_origPathOffset = 0;
                header->_numFrames = (ushort) _numFrames;
                header->_numEntries = (ushort) Children.Count;
                header->_numTexPtr = (ushort) _textureFiles.Count;
                header->_numPltPtr = (ushort) _paletteFiles.Count;
                header->_loop = _loop ? 1 : 0;
            }
            else
            {
                PAT0v3* header = (PAT0v3*) address;
                header->_header._tag = PAT0v3.Tag;
                header->_header._version = 3;
                header->_dataOffset = PAT0v3.Size;
                header->_origPathOffset = 0;
                header->_numFrames = (ushort) _numFrames;
                header->_numEntries = (ushort) Children.Count;
                header->_numTexPtr = (ushort) _textureFiles.Count;
                header->_numPltPtr = (ushort) _paletteFiles.Count;
                header->_loop = _loop ? 1 : 0;
            }

            PAT0v3* commonHeader = (PAT0v3*) address;

            //Now set header values that are in the same spot between versions

            //Set offsets
            commonHeader->_texTableOffset = length - (_textureFiles.Count + _paletteFiles.Count) * 8;
            commonHeader->_pltTableOffset = commonHeader->_texTableOffset + _textureFiles.Count * 4;

            //Set pointer offsets
            int offset = length - _textureFiles.Count * 4 - _paletteFiles.Count * 4;
            commonHeader->_texPtrTableOffset = offset;
            commonHeader->_pltPtrTableOffset = offset + _textureFiles.Count * 4;

            //Set pointers
            bint* ptr = (bint*) (commonHeader->Address + commonHeader->_texPtrTableOffset);
            for (int i = 0; i < _textureFiles.Count; i++)
            {
                *ptr++ = 0;
            }

            ptr = (bint*) (commonHeader->Address + commonHeader->_pltPtrTableOffset);
            for (int i = 0; i < _paletteFiles.Count; i++)
            {
                *ptr++ = 0;
            }

            ResourceGroup* group = commonHeader->Group;
            *group = new ResourceGroup(Children.Count);

            VoidPtr entryAddress = group->EndAddress;
            VoidPtr dataAddress = entryAddress;
            ResourceEntry* rEntry = group->First;

            foreach (PAT0EntryNode n in Children)
            {
                dataAddress += n._entryLen;
            }

            foreach (PAT0EntryNode n in Children)
            {
                foreach (PAT0TextureNode t in n.Children)
                {
                    n._dataAddrs[t.Index] = dataAddress;
                    if (n._dataLens[t.Index] != -1)
                    {
                        dataAddress += n._dataLens[t.Index];
                    }
                }
            }

            foreach (PAT0EntryNode n in Children)
            {
                (rEntry++)->_dataOffset = (int) entryAddress - (int) group;

                n.Rebuild(entryAddress, n._entryLen, true);
                entryAddress += n._entryLen;
            }

            if (_userEntries.Count > 0 && _version == 4)
            {
                _userEntries.Write(((PAT0v4*) address)->UserData = dataAddress);
            }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            PAT0v3* header = (PAT0v3*) dataAddress;
            if (_version == 4)
            {
                ((PAT0v4*) dataAddress)->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    ((PAT0v4*) dataAddress)->OrigPathAddress = stringTable[_originalPath] + 4;
                }
            }
            else
            {
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }
            }

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (PAT0EntryNode n in Children)
            {
                dataAddress = (VoidPtr) group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*) stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }

            int i = 0;
            bint* strings = header->TexFile;

            for (i = 0; i < _textureFiles.Count; i++)
            {
                if (!string.IsNullOrEmpty(_textureFiles[i]))
                {
                    strings[i] = (int) stringTable[_textureFiles[i]] + 4 - (int) strings;
                }
            }

            strings = header->PltFile;

            for (i = 0; i < _paletteFiles.Count; i++)
            {
                if (!string.IsNullOrEmpty(_paletteFiles[i]))
                {
                    strings[i] = (int) stringTable[_paletteFiles[i]] + 4 - (int) strings;
                }
            }

            if (_version == 4)
            {
                _userEntries.PostProcess(((PAT0v4*) dataAddress)->UserData, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((PAT0v3*) source.Address)->_header._tag == PAT0v3.Tag ? new PAT0Node() : null;
        }

        public PAT0EntryNode CreateEntry()
        {
            return CreateEntry(-1);
        }

        public PAT0EntryNode CreateEntry(int index)
        {
            PAT0EntryNode n = new PAT0EntryNode
            {
                Name = FindName(null)
            };
            InsertChild(n, index);
            n.CreateEntry();
            return n;
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
            float ratio = newFrameCount / (float) FrameCount;
            foreach (PAT0EntryNode e in Children)
            {
                foreach (PAT0TextureNode t in e.Children)
                {
                    foreach (PAT0TextureEntryNode x in t.Children)
                    {
                        x._frame *= ratio;
                    }

                    //t.SortChildren();
                }
            }

            FrameCount = newFrameCount;
        }

        /// <summary>
        /// Adds an animation opened by the user to the end of this one
        /// </summary>
        public void Append()
        {
            PAT0Node external = null;
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "PAT0 Animation (*.pat0)|*.pat0",
                Title = "Please select an animation to append."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                if ((external = (PAT0Node) NodeFactory.FromFile(null, o.FileName)) != null)
                {
                    Append(external);
                }
            }
        }

        /// <summary>
        /// Adds an animation to the end of this one
        /// </summary>
        public void Append(PAT0Node external)
        {
            int origIntCount = FrameCount;
            FrameCount += external.FrameCount;

            foreach (PAT0EntryNode w in external.Children)
            {
                foreach (PAT0TextureNode _extEntry in w.Children)
                {
                    PAT0TextureNode _intEntry = null;
                    if ((_intEntry = (PAT0TextureNode) FindChild(w.Name + "/" + _extEntry.Name, false)) == null)
                    {
                        PAT0EntryNode wi = null;
                        if ((wi = (PAT0EntryNode) FindChild(w.Name, false)) == null)
                        {
                            AddChild(wi = new PAT0EntryNode {Name = FindName(null)});
                        }

                        PAT0TextureNode newIntEntry = new PAT0TextureNode(_extEntry._texFlags, _extEntry.TextureIndex);

                        wi.AddChild(newIntEntry);
                        foreach (PAT0TextureEntryNode e in _extEntry.Children)
                        {
                            PAT0TextureEntryNode q = new PAT0TextureEntryNode {_frame = e._frame + origIntCount};
                            newIntEntry.AddChild(q);

                            q.Texture = e.Texture;
                            q.Palette = e.Palette;
                        }
                    }
                    else
                    {
                        foreach (PAT0TextureEntryNode e in _extEntry.Children)
                        {
                            PAT0TextureEntryNode q = new PAT0TextureEntryNode {_frame = e._frame + origIntCount};
                            _intEntry.AddChild(q);

                            q.Texture = e.Texture;
                            q.Palette = e.Palette;
                        }
                    }
                }
            }
        }

        #endregion
    }

    public unsafe class PAT0EntryNode : ResourceNode
    {
        internal PAT0Pattern* Header => (PAT0Pattern*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.PAT0Entry;
        public override Type[] AllowedChildTypes => new Type[] {typeof(PAT0TextureNode)};

        internal PAT0Flags[] texFlags = new PAT0Flags[8];

        public override bool OnInitialize()
        {
            if (_name == null && Header->_stringOffset != 0)
            {
                _name = Header->ResourceString;
            }

            uint flags = Header->_flags;
            for (int i = 0; i < 8; i++)
            {
                texFlags[i] = (PAT0Flags) ((flags >> (i * 4)) & 0xF);
            }

            return true;
        }

        public override void OnPopulate()
        {
            int count = 0, index = 0;
            foreach (PAT0Flags p in texFlags)
            {
                if (p.HasFlag(PAT0Flags.Enabled))
                {
                    if (!p.HasFlag(PAT0Flags.FixedTexture))
                    {
                        new PAT0TextureNode(p, index).Initialize(this,
                            new DataSource(Header->GetTexTable(count), PAT0Texture.Size));
                    }
                    else
                    {
                        PAT0TextureNode t = new PAT0TextureNode(p, index) {textureCount = 1};
                        t._parent = this;
                        _children.Add(t);
                        PAT0TextureEntryNode entry = new PAT0TextureEntryNode
                        {
                            _frame = 0,
                            _texFileIndex = Header->GetIndex(count, false),
                            _pltFileIndex = Header->GetIndex(count, true),
                            _parent = t
                        };
                        t._children.Add(entry);
                        entry.GetStrings();
                    }

                    count++;
                }

                index++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _dataLens = new int[Children.Count];
            _dataAddrs = new VoidPtr[Children.Count];

            _entryLen = PAT0Pattern.Size + Children.Count * 4;

            foreach (PAT0TextureNode table in Children)
            {
                _dataLens[table.Index] = table.CalculateSize(true);
            }

            //Check to see if any children can be remapped.
            foreach (PAT0TextureNode table in Children)
            {
                table.CompareToAll();
            }

            int size = 0;
            foreach (int i in _dataLens)
            {
                if (i != -1)
                {
                    size += i;
                }
            }

            return size + _entryLen;
        }

        public VoidPtr[] _dataAddrs;
        public int _entryLen;
        public int[] _dataLens;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PAT0Pattern* header = (PAT0Pattern*) address;

            int x = 0;
            foreach (int i in _dataLens)
            {
                if (i == -1)
                {
                    _dataAddrs[x] =
                        ((PAT0EntryNode) Parent.Children[((PAT0TextureNode) Children[x])._matIndex])._dataAddrs[
                            ((PAT0TextureNode) Children[x])._texIndex];
                }

                x++;
            }

            uint flags = 0;
            foreach (PAT0TextureNode table in Children)
            {
                table._texFlags |= PAT0Flags.Enabled;
                if (table.Children.Count > 1)
                {
                    table._texFlags &= ~PAT0Flags.FixedTexture;
                }
                else
                {
                    table._texFlags |= PAT0Flags.FixedTexture;
                }

                bool hasTex = false, hasPlt = false;

                //foreach (PAT0TextureEntryNode e in table.Children)
                //{
                //    if (e.Texture != null)
                //        hasTex = true;
                //    if (e.Palette != null)
                //        hasPlt = true;
                //}

                hasTex = table._hasTex;
                hasPlt = table._hasPlt;

                if (!hasTex)
                {
                    table._texFlags &= ~PAT0Flags.HasTexture;
                }
                else
                {
                    table._texFlags |= PAT0Flags.HasTexture;
                }

                if (!hasPlt)
                {
                    table._texFlags &= ~PAT0Flags.HasPalette;
                }
                else
                {
                    table._texFlags |= PAT0Flags.HasPalette;
                }

                if (table.Children.Count > 1)
                {
                    header->SetTexTableOffset(table.Index, _dataAddrs[table.Index]);
                    if (table._rebuild)
                    {
                        table.Rebuild(_dataAddrs[table.Index],
                            PAT0TextureTable.Size + PAT0Texture.Size * table.Children.Count, true);
                    }
                }
                else
                {
                    PAT0TextureEntryNode entry = (PAT0TextureEntryNode) table.Children[0];
                    PAT0Node node = (PAT0Node) Parent;

                    short i = 0;
                    if (table._hasTex && !string.IsNullOrEmpty(entry.Texture))
                    {
                        i = (short) node._textureFiles.IndexOf(entry.Texture);
                    }

                    if (i < 0)
                    {
                        entry._texFileIndex = 0;
                    }
                    else
                    {
                        entry._texFileIndex = (ushort) i;
                    }

                    i = 0;
                    if (table._hasPlt && !string.IsNullOrEmpty(entry.Palette))
                    {
                        i = (short) node._paletteFiles.IndexOf(entry.Palette);
                    }

                    if (i < 0)
                    {
                        entry._pltFileIndex = 0;
                    }
                    else
                    {
                        entry._pltFileIndex = (ushort) i;
                    }

                    header->SetIndex(table.Index, entry._texFileIndex, false);
                    header->SetIndex(table.Index, entry._pltFileIndex, true);
                }

                flags = (flags & ~((uint) 0xF << (table._textureIndex * 4))) |
                        ((uint) table._texFlags << (table._textureIndex * 4));
            }

            header->_flags = flags;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            PAT0Pattern* header = (PAT0Pattern*) dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        public PAT0TextureNode CreateEntry()
        {
            int value = 0;
            foreach (PAT0TextureNode t in Children)
            {
                if (t._textureIndex == value)
                {
                    value++;
                }
            }

            if (value == 8)
            {
                return null;
            }

            PAT0TextureNode node = new PAT0TextureNode((PAT0Flags) 7, value);
            AddChild(node);
            node.CreateEntry();
            return node;
        }
    }

    public unsafe class PAT0TextureNode : ResourceNode, IVideo
    {
        internal PAT0TextureTable* Header => (PAT0TextureTable*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.PAT0Texture;
        public override Type[] AllowedChildTypes => new Type[] {typeof(PAT0TextureEntryNode)};

        public PAT0Flags _texFlags;
        public int _textureIndex, textureCount;
        public ushort _texNameIndex, _pltNameIndex;
        public bool _hasPlt, _hasTex;
        public float _frameScale;

        public bool _rebuild = true;

        //[Category("PAT0 Texture")]
        //public PAT0Flags TextureFlags { get { return texFlags; } }//set { texFlags = value; SignalPropertyChange(); } }
        //[Category("PAT0 Texture")]
        //public float FrameScale { get { return frameScale; } }
        //[Category("PAT0 Texture")]
        //public int TextureCount { get { return textureCount; } }
        [Category("PAT0 Texture")]
        public bool HasTexture
        {
            get => _hasTex;
            set
            {
                _hasTex = value;
                SignalPropertyChange();
            }
        }

        [Category("PAT0 Texture")]
        public bool HasPalette
        {
            get => _hasPlt;
            set
            {
                _hasPlt = value;
                SignalPropertyChange();
            }
        }

        [Category("PAT0 Texture")]
        public int TextureIndex
        {
            get => _textureIndex;
            set
            {
                foreach (PAT0TextureNode t in Parent.Children)
                {
                    if (t.Index != Index && t._textureIndex == (value > 7 ? 7 : value < 0 ? 0 : value))
                    {
                        return;
                    }
                }

                _textureIndex = value > 7 ? 7 : value < 0 ? 0 : value;

                Name = "Texture" + _textureIndex;

                CheckNext();
                CheckPrev();
            }
        }

        public void CheckNext()
        {
            if (Index == Parent.Children.Count - 1)
            {
                return;
            }

            int index = Index;
            if (_textureIndex > ((PAT0TextureNode) Parent.Children[Index + 1])._textureIndex)
            {
                DoMoveDown();
                if (index != Index)
                {
                    CheckNext();
                }
            }
        }

        public void CheckPrev()
        {
            if (Index == 0)
            {
                return;
            }

            int index = Index;
            if (_textureIndex < ((PAT0TextureNode) Parent.Children[Index - 1])._textureIndex)
            {
                DoMoveUp();
                if (index != Index)
                {
                    CheckPrev();
                }
            }
        }

        /// <summary>
        /// Gets the last applied texture entry before or at the index.
        /// </summary>
        public PAT0TextureEntryNode GetPrevious(float index)
        {
            PAT0TextureEntryNode prev = null;
            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }

                if (prev._frame <= index && next._frame > index)
                {
                    break;
                }

                prev = next;
            }

            return prev;
        }

        /// <summary>
        /// Gets the texture entry at the index, if there is one.
        /// </summary>
        public PAT0TextureEntryNode GetEntry(float index)
        {
            PAT0TextureEntryNode prev = null;
            if (Children.Count == 0)
            {
                return null;
            }

            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }

                if (prev._frame <= index && next._frame > index)
                {
                    break;
                }

                prev = next;
            }

            if ((int) prev._frame == index)
            {
                return prev;
            }

            return null;
        }

        /// <summary>
        /// Gets the applied texture at the index and outputs if the value is a keyframe.
        /// </summary>
        public string GetTexture(float index, out bool kf)
        {
            PAT0TextureEntryNode prev = null;
            if (Children.Count == 0)
            {
                kf = false;
                return null;
            }

            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }

                if (prev._frame <= index && next._frame > index)
                {
                    break;
                }

                prev = next;
            }

            if ((int) prev._frame == index)
            {
                kf = true;
            }
            else
            {
                kf = false;
            }

            return prev.Texture;
        }

        /// <summary>
        /// Gets the applied palette at the index and outputs if the value is a keyframe.
        /// </summary>
        public string GetPalette(float index, out bool kf)
        {
            PAT0TextureEntryNode prev = null;
            if (Children.Count == 0)
            {
                kf = false;
                return null;
            }

            foreach (PAT0TextureEntryNode next in Children)
            {
                if (next.Index == 0)
                {
                    prev = next;
                    continue;
                }

                if (prev._frame <= index && next._frame > index)
                {
                    break;
                }

                prev = next;
            }

            if ((int) prev._frame == index)
            {
                kf = true;
            }
            else
            {
                kf = false;
            }

            return prev.Palette;
        }

        public PAT0TextureNode(PAT0Flags flags, int index)
        {
            _texFlags = flags;
            _hasTex = _texFlags.HasFlag(PAT0Flags.HasTexture);
            _hasPlt = _texFlags.HasFlag(PAT0Flags.HasPalette);
            _textureIndex = index;
            _name = "Texture" + _textureIndex;
        }

        public override bool OnInitialize()
        {
            _frameScale = Header->_frameScale;
            textureCount = Header->_textureCount;

            return textureCount > 0;
        }

        public override void OnPopulate()
        {
            if (!_texFlags.HasFlag(PAT0Flags.FixedTexture))
            {
                PAT0Texture* current = Header->Textures;
                for (int i = 0; i < textureCount; i++, current++)
                {
                    new PAT0TextureEntryNode().Initialize(this, new DataSource(current, PAT0Texture.Size));
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return Children.Count > 1 ? PAT0TextureTable.Size + PAT0Texture.Size * Children.Count : 0;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Children.Count > 1)
            {
                PAT0TextureTable* table = (PAT0TextureTable*) address;
                table->_textureCount = (short) Children.Count;
                float f = (Children[Children.Count - 1] as PAT0TextureEntryNode)._frame;
                table->_frameScale = f == 0 ? 0 : 1.0f / f;
                table->_pad = 0;

                PAT0Texture* entry = table->Textures;
                foreach (PAT0TextureEntryNode n in Children)
                {
                    n.Rebuild(entry++, PAT0Texture.Size, true);
                }
            }
        }

        internal int _matIndex, _texIndex;

        internal void CompareToAll()
        {
            _rebuild = true;
            foreach (PAT0EntryNode e in Parent?.Parent?.Children ?? new List<ResourceNode>())
            {
                foreach (PAT0TextureNode table in e.Children)
                {
                    if (table == this)
                    {
                        return;
                    }

                    if (table != this && table.Children.Count == Children.Count)
                    {
                        bool same = true;
                        for (int l = 0; l < Children.Count; l++)
                        {
                            PAT0TextureEntryNode exte = (PAT0TextureEntryNode) table.Children[l];
                            PAT0TextureEntryNode inte = (PAT0TextureEntryNode) Children[l];

                            if (exte._frame != inte._frame || exte.Texture != inte.Texture ||
                                exte.Palette != inte.Palette)
                            {
                                same = false;
                                break;
                            }
                        }

                        if (same)
                        {
                            _rebuild = false;
                            _matIndex = e.Index;
                            _texIndex = table.Index;
                            ((PAT0EntryNode) Parent)._dataLens[Index] = -1;
                            return;
                        }
                    }
                }
            }
        }

        public PAT0TextureEntryNode CreateEntry()
        {
            float value = Children.Count > 0 ? ((PAT0TextureEntryNode) Children[Children.Count - 1])._frame + 1 : 0;
            PAT0TextureEntryNode node = new PAT0TextureEntryNode {_frame = value};
            AddChild(node);
            node.Texture = "NewTexture";
            return node;
        }

        /// <summary>
        /// Sorts the order of texture entries by their frame index.
        /// </summary>
        public override void SortChildren()
        {
            Top:
            for (int i = 0; i < Children.Count; i++)
            {
                PAT0TextureEntryNode t = Children[i] as PAT0TextureEntryNode;
                int x = t.Index;
                t.CheckNext();
                t.CheckPrev();
                if (t.Index != x)
                {
                    goto Top;
                }
            }
        }

        #region IVideo Interface

        [Browsable(false)] public uint NumFrames => (uint) ((PAT0Node) Parent.Parent).FrameCount;
        [Browsable(false)] public bool Loop => (Parent.Parent as PAT0Node)?.Loop ?? false;

        [Browsable(false)]
        public int GetImageIndexAtFrame(int frame)
        {
            PAT0TextureEntryNode node = GetPrevious(frame);
            if (node != null)
            {
                return node.Index;
            }

            return 0;
        }

        [Browsable(false)] public float FrameRate => 60;
        [Browsable(false)] public IAudioStream Audio => null;
        [Browsable(false)] public uint Frequency => 0;
        [Browsable(false)] public int ImageCount => Children.Count;

        [Browsable(false)]
        public Bitmap GetImage(int index)
        {
            if (index < 0 || index >= Children.Count)
            {
                return null;
            }

            return ((PAT0TextureEntryNode) Children[index]).GetImage(0);
        }

        #endregion
    }

    public unsafe class PAT0TextureEntryNode : ResourceNode, IImageSource
    {
        internal PAT0Texture* Header => (PAT0Texture*) WorkingUncompressed.Address;
        public float _frame;
        public ushort _texFileIndex, _pltFileIndex;

        public string _tex, _plt;

        public override ResourceType ResourceFileType => ResourceType.PAT0TextureEntry;
        public override bool AllowDuplicateNames => true;

        [Category("PAT0 Texture Entry")]
        public float FrameIndex
        {
            get => _frame;
            set
            {
                if (Index == 0)
                {
                    if (Index >= Children.Count - 1)
                    {
                        _frame = 0;
                    }
                    else if (value >= ((PAT0TextureEntryNode) Parent.Children[Index + 1])._frame)
                    {
                        ((PAT0TextureEntryNode) Parent.Children[Index + 1])._frame = 0;
                        Parent.Children[Index + 1].SignalPropertyChange();

                        _frame = value;
                        CheckNext();
                    }

                    SignalPropertyChange();
                    return;
                }

                _frame = value;
                CheckPrev();
                CheckNext();

                SignalPropertyChange();
            }
        }

        public void CheckNext()
        {
            if (Parent == null || Index == Parent.Children.Count - 1)
            {
                return;
            }

            int index = Index;
            if (_frame > ((PAT0TextureEntryNode) Parent.Children[Index + 1])._frame)
            {
                DoMoveDown();
                if (index != Index)
                {
                    CheckNext();
                }
            }
        }

        public void CheckPrev()
        {
            if (Parent == null || Index == 0)
            {
                return;
            }

            int index = Index;
            if (_frame < ((PAT0TextureEntryNode) Parent.Children[Index - 1])._frame)
            {
                DoMoveUp();
                if (index != Index)
                {
                    CheckPrev();
                }
            }
        }

        [Category("PAT0 Texture Entry")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListPAT0Textures))]
        public string Texture
        {
            get => _tex;
            set
            {
                if (Parent == null || !(Parent as PAT0TextureNode)._hasTex || value == _tex)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(value))
                {
                    Name = value;
                }
                else
                {
                    SetTexture(value);
                }

                SignalPropertyChange();
            }
        }

        [Category("PAT0 Texture Entry")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListPAT0Palettes))]
        public string Palette
        {
            get => _plt;
            set
            {
                if (Parent == null || !(Parent as PAT0TextureNode)._hasPlt || value == _plt)
                {
                    return;
                }

                _plt = value;
                _paletteNode = null;
                ((PAT0Node) Parent?.Parent?.Parent)?.RegeneratePaletteList();

                SignalPropertyChange();
            }
        }

        private void SetTexture(string value)
        {
            _tex = value;
            _textureNode = null;
            ((PAT0Node) Parent?.Parent?.Parent)?.RegenerateTextureList();
        }

        [Browsable(false)]
        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                SetTexture(value);
            }
        }

        public override bool OnInitialize()
        {
            _frame = Header->_key;
            _texFileIndex = Header->_texFileIndex;
            _pltFileIndex = Header->_pltFileIndex;

            GetStrings();

            return false;
        }

        public void GetStrings()
        {
            if (Parent == null)
            {
                _name = "<null>";
                return;
            }

            PAT0Node node = (PAT0Node) Parent.Parent.Parent;

            if (((PAT0TextureNode) Parent)._hasPlt && _pltFileIndex < node._paletteFiles.Count)
            {
                _plt = node._paletteFiles[_pltFileIndex];
                _paletteNode = null;
            }

            if (((PAT0TextureNode) Parent)._hasTex && _texFileIndex < node._textureFiles.Count)
            {
                _name = _tex = node._textureFiles[_texFileIndex];
                _textureNode = null;
            }

            if (_name == null && _plt != null)
            {
                _name = _plt;
            }

            if (_name == null)
            {
                _name = "<null>";
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PAT0Node node = (PAT0Node) Parent.Parent.Parent;

            PAT0Texture* header = (PAT0Texture*) address;

            header->_key = _frame;
            header->_texFileIndex = ((PAT0TextureNode) Parent)._hasTex && !string.IsNullOrEmpty(_tex)
                ? _texFileIndex
                : (ushort) 0;
            header->_pltFileIndex = ((PAT0TextureNode) Parent)._hasPlt && !string.IsNullOrEmpty(_plt)
                ? _pltFileIndex
                : (ushort) 0;
        }

        public override int OnCalculateSize(bool force)
        {
            return PAT0Texture.Size;
        }

        //[Browsable(false)]
        public int ImageCount
        {
            get
            {
                if (_textureNode == null)
                {
                    _textureNode = RootNode.FindChildByType(_tex, true, ResourceType.TEX0) as TEX0Node;
                    if (_textureNode == null)
                    {
                        return 0;
                    }
                }

                return _textureNode.ImageCount;
            }
        }

        public Bitmap GetImage(int index)
        {
            if (string.IsNullOrEmpty(_tex))
            {
                return null;
            }

            if (_textureNode == null)
            {
                _textureNode = RootNode.FindChildByType(_tex, true, ResourceType.TEX0) as TEX0Node;
                if (_textureNode == null)
                {
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(_plt) && _paletteNode == null)
            {
                _paletteNode = RootNode.FindChildByType(_plt, true, ResourceType.PLT0) as PLT0Node;
            }

            return _textureNode.GetImage(index, _paletteNode);
        }

        public TEX0Node _textureNode;
        public PLT0Node _paletteNode;
    }
}

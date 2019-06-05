using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class VIS0Node : NW4RAnimationNode
    {
        private const string _category = "Bone Visibility Animation";

        public VIS0Node()
        {
            _version = 3;
        }

        internal VIS0v3* Header3 => (VIS0v3*) WorkingUncompressed.Address;
        internal VIS0v4* Header4 => (VIS0v4*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.VIS0;
        public override Type[] AllowedChildTypes => new[] {typeof(VIS0EntryNode)};
        public override int[] SupportedVersions => new[] {3, 4};

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

        protected override void UpdateChildFrameLimits()
        {
            foreach (VIS0EntryNode e in Children) e.EntryCount = FrameCount;
        }

        public VIS0EntryNode CreateEntry()
        {
            var entry = new VIS0EntryNode
            {
                _entryCount = -1,
                EntryCount = FrameCount,
                Name = FindName(null)
            };
            AddChild(entry);
            return entry;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_version == 4)
            {
                var header = Header4;
                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;
                if (_name == null && header->_stringOffset != 0) _name = header->ResourceString;

                if (header->_origPathOffset > 0) _originalPath = header->OrigPath;
                (_userEntries = new UserDataCollection()).Read(header->UserData);

                return header->Group->_numEntries > 0;
            }
            else
            {
                var header = Header3;
                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;

                if (_name == null && header->_stringOffset != 0) _name = header->ResourceString;

                if (header->_origPathOffset > 0) _originalPath = header->OrigPath;

                return header->Group->_numEntries > 0;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            var size = VIS0v3.Size + 0x18 + Children.Count * 0x10;
            foreach (var e in Children) size += e.CalculateSize(force);

            if (_version == 4) size += _userEntries.GetSize();

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var count = Children.Count;
            ResourceGroup* group;

            if (_version == 4)
            {
                var header = (VIS0v4*) address;
                *header = new VIS0v4(length, (ushort) _numFrames, (ushort) count, _loop);
                group = header->Group;
            }
            else
            {
                var header = (VIS0v3*) address;
                *header = new VIS0v3(length, (ushort) _numFrames, (ushort) count, _loop);
                group = header->Group;
            }

            *group = new ResourceGroup(count);
            var entry = group->First;

            var dataAddress = group->EndAddress;
            foreach (var n in Children)
            {
                (entry++)->_dataOffset = (int) dataAddress - (int) group;

                var len = n._calcSize;
                n.Rebuild(dataAddress, len, force);
                dataAddress += len;
            }

            if (_userEntries.Count > 0 && _version == 4)
                _userEntries.Write(((VIS0v4*) address)->UserData = dataAddress);
        }

        public override void OnPopulate()
        {
            var group = Header3->Group;
            for (var i = 0; i < group->_numEntries; i++)
                new VIS0EntryNode().Initialize(this, new DataSource((VoidPtr) group + group->First[i]._dataOffset, 0));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (VIS0EntryNode n in Children) table.Add(n.Name);

            if (_version == 4) _userEntries.GetStrings(table);

            if (!string.IsNullOrEmpty(_originalPath)) table.Add(_originalPath);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
            StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            var header = (VIS0v3*) dataAddress;

            if (_version == 4)
            {
                ((VIS0v4*) dataAddress)->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                    ((VIS0v4*) dataAddress)->OrigPathAddress = stringTable[_originalPath] + 4;
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
            foreach (VIS0EntryNode n in Children)
            {
                dataAddress = (VoidPtr) group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*) stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }

            if (_version == 4) _userEntries.PostProcess(((VIS0v4*) dataAddress)->UserData, stringTable);
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((VIS0v3*) source.Address)->_header._tag == VIS0v3.Tag ? new VIS0Node() : null;
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
            var ratio = newFrameCount / (float) FrameCount;
            var oldFrameCount = FrameCount;

            var bools = new bool[Children.Count][];

            foreach (VIS0EntryNode e in Children)
                if (!e.Constant)
                {
                    var newBools = new bool[newFrameCount];
                    for (var i = 0; i < FrameCount; i++)
                        if (e.GetEntry(i))
                        {
                            var start = i;
                            var z = i;
                            while (e.GetEntry(++z)) ;

                            var span = z - start;

                            var newSpan = (int) (span * ratio + 0.5f);
                            var newStart = (int) (start * ratio + 0.5f);

                            for (var w = 0; w < newSpan; w++)
                                newBools[(newStart + w).Clamp(0, newBools.Length - 1)] = true;

                            i = z + 1;
                        }

                    bools[e.Index] = newBools;
                }

            FrameCount = newFrameCount;

            var o = -1;
            foreach (var b in bools)
            {
                o++;
                if (b == null) continue;

                var e = Children[o] as VIS0EntryNode;

                e._data = new byte[e._data.Length];
                var u = 0;
                var byteIndex = 0;
                foreach (var i in b)
                {
                    if (u % 8 == 0) byteIndex = u / 8;

                    e._data[byteIndex] |= (byte) ((i ? 1 : 0) << (7 - u % 8));

                    u++;
                }
            }
        }

        /// <summary>
        ///     Adds an animation opened by the user to the end of this one
        /// </summary>
        public void Append()
        {
            VIS0Node external = null;
            var o = new OpenFileDialog
            {
                Filter = "VIS0 Animation (*.vis0)|*.vis0",
                Title = "Please select an animation to append."
            };
            if (o.ShowDialog() == DialogResult.OK)
                if ((external = (VIS0Node) NodeFactory.FromFile(null, o.FileName)) != null)
                    Append(external);
        }

        /// <summary>
        ///     Adds an animation to the end of this one
        /// </summary>
        public void Append(VIS0Node external)
        {
            var origIntCount = FrameCount;
            FrameCount += external.FrameCount;

            foreach (VIS0EntryNode _extEntry in external.Children)
            {
                VIS0EntryNode _intEntry = null;
                if ((_intEntry = (VIS0EntryNode) FindChild(_extEntry.Name, false)) == null)
                {
                    var newIntEntry = new VIS0EntryNode {Name = _extEntry.Name};

                    newIntEntry._entryCount = -1;
                    newIntEntry.EntryCount = _extEntry.EntryCount + origIntCount;
                    newIntEntry._flags = 0;

                    if (_extEntry.Constant)
                    {
                        if (_extEntry.Enabled)
                            for (var i = origIntCount.Align(8) / 8; i < newIntEntry.EntryCount; i++)
                                newIntEntry._data[i] = 0xFF;
                    }
                    else
                    {
                        Array.Copy(_extEntry._data, 0, newIntEntry._data, origIntCount.Align(8) / 8,
                            _extEntry.EntryCount.Align(8) / 8);
                    }

                    AddChild(newIntEntry);
                }
                else
                {
                    if (!_extEntry.Constant && !_intEntry.Constant)
                    {
                        Array.Copy(_extEntry._data, 0, _intEntry._data, origIntCount.Align(8) / 8,
                            _extEntry.EntryCount.Align(8) / 8);
                    }
                    else
                    {
                        var d = new byte[_extEntry._data.Length];
                        if (_intEntry.Constant)
                        {
                            if (_intEntry.Enabled)
                                for (var i = 0; i < origIntCount.Align(8) / 8; i++)
                                    d[i] = 0xFF;
                        }
                        else
                        {
                            Array.Copy(_extEntry._data, 0, _intEntry._data, origIntCount.Align(8) / 8,
                                _extEntry.EntryCount.Align(8) / 8);
                        }

                        _intEntry.Constant = false;
                        if (_extEntry.Constant)
                        {
                            if (_extEntry.Enabled)
                                for (var i = origIntCount.Align(8) / 8; i < _intEntry.EntryCount; i++)
                                    d[i] = 0xFF;
                        }
                        else
                        {
                            Array.Copy(_extEntry._data, 0, d, origIntCount.Align(8) / 8,
                                _extEntry.EntryCount.Align(8) / 8);
                        }

                        _intEntry._data = d;
                    }
                }
            }
        }

        #endregion
    }

    public unsafe class VIS0EntryNode : ResourceNode, IBoolArraySource
    {
        public byte[] _data = new byte[0];
        public int _entryCount;
        public VIS0Flags _flags;
        internal VIS0Entry* Header => (VIS0Entry*) WorkingUncompressed.Address;

        [Browsable(false)]
        public int EntryCount
        {
            get => _entryCount;
            set
            {
                if (_entryCount == 0) return;

                _entryCount = value;
                var len = value.Align(32) / 8;

                if (_data.Length < len)
                {
                    var newArr = new byte[len];
                    Array.Copy(_data, newArr, _data.Length);
                    _data = newArr;
                }

                SignalPropertyChange();
            }
        }

        //[Category("VIS0 Entry")]
        //public VIS0Flags Flags { get { return _flags; } set { _flags = value; SignalPropertyChange(); } }

        [Category("VIS0 Entry")]
        public bool Enabled
        {
            get => _flags.HasFlag(VIS0Flags.Enabled);
            set
            {
                if (value)
                    _flags |= VIS0Flags.Enabled;
                else
                    _flags &= ~VIS0Flags.Enabled;

                SignalPropertyChange();
            }
        }

        [Category("VIS0 Entry")]
        public bool Constant
        {
            get => _flags.HasFlag(VIS0Flags.Constant);
            set
            {
                if (value)
                    MakeConstant(Enabled);
                else
                    MakeAnimated();

                SignalPropertyChange();
                UpdateCurrentControl();
            }
        }

        public bool GetEntry(int index)
        {
            var i = index >> 3;
            if (i >= _data.Length) return false;

            var bit = 1 << (7 - (index & 7));
            return (_data[i] & bit) != 0;
        }

        public void SetEntry(int index, bool value)
        {
            var i = index >> 3;
            if (i >= _data.Length) return;

            var bit = 1 << (7 - (index & 7));
            var mask = ~bit;
            _data[i] = (byte) ((_data[i] & mask) | (value ? bit : 0));
            SignalPropertyChange();
        }

        public void MakeConstant(bool value)
        {
            _flags = VIS0Flags.Constant | (value ? VIS0Flags.Enabled : 0);
            _entryCount = 0;
            SignalPropertyChange();
        }

        public void MakeAnimated()
        {
            _flags = VIS0Flags.None;
            _entryCount = -1;
            EntryCount = ((VIS0Node) _parent).FrameCount;

            //bool e = Enabled;
            //for (int i = 0; i < _entryCount; i++)
            //    SetEntry(i, e);

            SignalPropertyChange();
        }

        public override int OnCalculateSize(bool force)
        {
            if (_entryCount == 0) return 8;

            return _entryCount.Align(32) / 8 + 8;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (VIS0Entry*) address;
            *header = new VIS0Entry(_flags);

            if (_entryCount != 0) Marshal.Copy(_data, 0, header->Data, length - 8);
        }

        public override bool OnInitialize()
        {
            if (_name == null && Header->_stringOffset != 0) _name = Header->ResourceString;

            _flags = Header->Flags;

            if ((_flags & VIS0Flags.Constant) == 0)
            {
                _entryCount = ((VIS0Node) _parent).FrameCount;
                var numBytes = _entryCount.Align(32) / 8;

                SetSizeInternal(numBytes + 8);

                _data = new byte[numBytes];
                Marshal.Copy(Header->Data, _data, 0, numBytes);
            }
            else
            {
                _entryCount = 0;
                _data = new byte[0];
                SetSizeInternal(8);
            }

            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            var header = (VIS0Entry*) dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
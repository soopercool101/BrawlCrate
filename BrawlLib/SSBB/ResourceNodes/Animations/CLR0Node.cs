using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CLR0Node : NW4RAnimationNode
    {
        internal CLR0v3* Header3 => (CLR0v3*) WorkingUncompressed.Address;
        internal CLR0v4* Header4 => (CLR0v4*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.CLR0;
        public override Type[] AllowedChildTypes => new Type[] {typeof(CLR0MaterialNode)};
        public override int[] SupportedVersions => new int[] {3, 4};
        public override string Tag => "CLR0";

        public CLR0Node()
        {
            _version = 3;
        }

        private const string _category = "Material Color Animation";

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
            foreach (CLR0MaterialNode n in Children)
            {
                foreach (CLR0MaterialEntryNode e in n.Children)
                {
                    e.NumEntries = _numFrames;
                }
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_version == 4)
            {
                CLR0v4* header = Header4;

                _numFrames = header->_frames;
                _loop = header->_loop != 0;

                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
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
                CLR0v3* header = Header3;

                _numFrames = header->_frames;
                _loop = header->_loop != 0;

                if (_name == null && header->_stringOffset != 0)
                {
                    _name = header->ResourceString;
                }

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                return header->Group->_numEntries > 0;
            }
        }

        public CLR0MaterialEntryNode CreateEntry()
        {
            CLR0MaterialNode node = new CLR0MaterialNode();
            CLR0MaterialEntryNode entry = new CLR0MaterialEntryNode
            {
                _target = EntryTarget.ColorRegister0
            };
            entry._name = entry._target.ToString();
            entry._numEntries = -1;
            entry.NumEntries = _numFrames;
            entry.Constant = true;
            entry.SolidColor = new ARGBPixel();
            node.Name = FindName("MaterialName");
            AddChild(node);
            node.AddChild(entry);
            return entry;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (_version == 4 ? CLR0v4.Size : CLR0v3.Size) + 0x18 + Children.Count * 0x10;
            foreach (CLR0MaterialNode n in Children)
            {
                size += 8 + n.Children.Count * 8;
                foreach (CLR0MaterialEntryNode e in n.Children)
                {
                    if (e._numEntries != 0)
                    {
                        size += e._colors.Count * 4;
                    }
                }
            }

            if (_version == 4)
            {
                size += _userEntries.GetSize();
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int count = Children.Count;

            CLR0Material* pMat =
                (CLR0Material*) (address + (_version == 4 ? CLR0v4.Size : CLR0v3.Size) + 0x18 + count * 0x10);

            int offset = Children.Count * 8;
            foreach (CLR0MaterialNode n in Children)
            {
                offset += n.Children.Count * 8;
            }

            RGBAPixel* pData = (RGBAPixel*) ((VoidPtr) pMat + offset);

            ResourceGroup* group;
            if (_version == 4)
            {
                CLR0v4* header = (CLR0v4*) address;
                *header = new CLR0v4(length, _numFrames, count, _loop);

                group = header->Group;
            }
            else
            {
                CLR0v3* header = (CLR0v3*) address;
                *header = new CLR0v3(length, _numFrames, count, _loop);

                group = header->Group;
            }

            *group = new ResourceGroup(count);

            ResourceEntry* entry = group->First;
            foreach (CLR0MaterialNode n in Children)
            {
                (entry++)->_dataOffset = (int) pMat - (int) group;

                uint newFlags = 0;

                CLR0MaterialEntry* pMatEntry = (CLR0MaterialEntry*) ((VoidPtr) pMat + 8);
                foreach (CLR0MaterialEntryNode e in n.Children)
                {
                    newFlags |= (uint) ((1 + (e._constant ? 2 : 0)) & 3) << ((int) e._target * 2);
                    if (e._numEntries == 0)
                    {
                        *pMatEntry = new CLR0MaterialEntry(e._colorMask, e._solidColor);
                    }
                    else
                    {
                        *pMatEntry = new CLR0MaterialEntry(e._colorMask, (int) pData - (int) ((VoidPtr) pMatEntry + 4));
                        foreach (ARGBPixel p in e._colors)
                        {
                            *pData++ = p;
                        }
                    }

                    pMatEntry++;
                    e._changed = false;
                }

                pMat->_flags = newFlags;
                pMat = (CLR0Material*) pMatEntry;
                n._changed = false;
            }

            if (_userEntries.Count > 0 && _version == 4)
            {
                CLR0v4* header = (CLR0v4*) address;
                header->UserData = pData;
                _userEntries.Write(pData);
            }
        }

        public override void OnPopulate()
        {
            ResourceGroup* group = Header3->Group;
            for (int i = 0; i < group->_numEntries; i++)
            {
                new CLR0MaterialNode().Initialize(this, new DataSource(group->First[i].DataAddress, 8));
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (CLR0MaterialNode n in Children)
            {
                table.Add(n.Name);
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

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            CLR0v3* header = (CLR0v3*) dataAddress;
            if (_version == 4)
            {
                ((CLR0v4*) header)->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    ((CLR0v4*) dataAddress)->OrigPathAddress = stringTable[_originalPath] + 4;
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
            foreach (CLR0MaterialNode n in Children)
            {
                VoidPtr materialDataAddress = (VoidPtr) group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, materialDataAddress, (BRESString*) stringTable[n.Name]);
                n.PostProcess(materialDataAddress, stringTable);
            }

            if (_version == 4)
            {
                _userEntries.PostProcess(((CLR0v4*) dataAddress)->UserData, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((CLR0v3*) source.Address)->_header._tag == CLR0v3.Tag ? new CLR0Node() : null;
        }

        #region Extra Functions

        //public void Append()
        //{
        //    CLR0Node external = null;
        //    OpenFileDialog o = new OpenFileDialog();
        //    o.Filter = "CLR0 Animation (*.clr0)|*.clr0";
        //    o.Title = "Please select an animation to append.";
        //    if (o.ShowDialog() == DialogResult.OK)
        //        if ((external = (CLR0Node)NodeFactory.FromFile(null, o.FileName)) != null)
        //            Append(external);
        //}
        //public void Append(CLR0Node external)
        //{
        //    int origIntCount = FrameCount;
        //    FrameCount += external.FrameCount;

        //    foreach (CLR0MaterialNode mat in external.Children)
        //    {
        //        foreach (CLR0MaterialEntryNode _extEntry in mat.Children)
        //        {
        //            CLR0MaterialEntryNode _intEntry = null;
        //            if ((_intEntry = (CLR0MaterialEntryNode)FindChild(mat.Name + "/" + _extEntry.Name, false)) == null)
        //            {
        //                CLR0MaterialNode wi = null;
        //                if ((wi = (CLR0MaterialNode)FindChild(mat.Name, false)) == null)
        //                    AddChild(wi = new CLR0MaterialNode() { Name = FindName(null) });

        //                CLR0MaterialEntryNode newIntEntry = new CLR0MaterialEntryNode() { Name = _extEntry.Name };

        //                AddChild(newIntEntry);
        //            }
        //            else
        //            {
        //                //for (int x = 0; x < external.FrameCount; x++)
        //            }
        //        }
        //    }
        //}

        #endregion
    }

    public unsafe class CLR0MaterialNode : ResourceNode
    {
        internal CLR0Material* Header => (CLR0Material*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.CLR0Material;
        public override Type[] AllowedChildTypes => new Type[] {typeof(CLR0MaterialEntryNode)};

        internal CLR0EntryFlags _flags;

        public List<int> _entries;

        public override bool OnInitialize()
        {
            if (_name == null && Header->_stringOffset != 0)
            {
                _name = Header->ResourceString;
            }

            _flags = Header->Flags;
            _entries = new List<int>();

            for (int i = 0; i < 11; i++)
            {
                if ((((uint) _flags >> (i * 2)) & 1) != 0)
                {
                    _entries.Add(i);
                }
            }

            return _entries.Count > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                new CLR0MaterialEntryNode
                    {
                        _target = (EntryTarget) _entries[i],
                        _constant = (((uint) _flags >> (_entries[i] * 2)) & 2) == 2
                    }
                    .Initialize(this, (VoidPtr) Header + 8 + i * 8, 8);
            }
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CLR0Material* header = (CLR0Material*) dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        public CLR0MaterialEntryNode CreateEntry()
        {
            int value = 0;

            Top:
            foreach (CLR0MaterialEntryNode t in Children)
            {
                if ((int) t._target == value)
                {
                    value++;
                    goto Top;
                }
            }

            if (value >= 11)
            {
                return null;
            }

            CLR0MaterialEntryNode entry = new CLR0MaterialEntryNode
            {
                _target = (EntryTarget) value
            };
            entry._name = entry._target.ToString();
            entry._numEntries = -1;
            entry.NumEntries = ((CLR0Node) Parent)._numFrames;
            AddChild(entry);
            return entry;
        }
    }

    public unsafe class CLR0MaterialEntryNode : ResourceNode, IColorSource
    {
        internal CLR0MaterialEntry* Header => (CLR0MaterialEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.CLR0MaterialEntry;

        public bool _constant;

        [Category("CLR0 Material Entry")]
        public bool Constant
        {
            get => _constant;
            set
            {
                if (_constant != value)
                {
                    _constant = value;
                    if (_constant)
                    {
                        MakeSolid(_solidColor);
                    }
                    else
                    {
                        MakeList();
                    }

                    UpdateCurrentControl();
                }
            }
        }

        internal EntryTarget _target;

        [Category("CLR0 Material Entry")]
        public EntryTarget Target
        {
            get => _target;
            set
            {
                foreach (CLR0MaterialEntryNode t in Parent.Children)
                {
                    if (t._target == value)
                    {
                        return;
                    }
                }

                _target = value;
                Name = _target.ToString();
                SignalPropertyChange();
            }
        }

        internal ARGBPixel _colorMask;

        [Browsable(false)]
        public ARGBPixel ColorMask
        {
            get => _colorMask;
            set
            {
                _colorMask = value;
                SignalPropertyChange();
            }
        }

        internal List<ARGBPixel> _colors = new List<ARGBPixel>();

        [Browsable(false)]
        public List<ARGBPixel> Colors
        {
            get => _colors;
            set
            {
                _colors = value;
                SignalPropertyChange();
            }
        }

        internal ARGBPixel _solidColor;

        [Browsable(false)]
        public ARGBPixel SolidColor
        {
            get => _solidColor;
            set
            {
                _solidColor = value;
                SignalPropertyChange();
            }
        }

        internal int _numEntries;

        [Browsable(false)]
        internal int NumEntries
        {
            get => _numEntries;
            set
            {
                if (_numEntries == 0)
                {
                    return;
                }

                if (value > _numEntries)
                {
                    ARGBPixel p = _numEntries > 0 ? _colors[_numEntries - 1] : new ARGBPixel(255, 0, 0, 0);
                    for (int i = value - _numEntries; i-- > 0;)
                    {
                        _colors.Add(p);
                    }
                }
                else if (value < _colors.Count)
                {
                    _colors.RemoveRange(value, _colors.Count - value);
                }

                _numEntries = value;
            }
        }

        public override bool OnInitialize()
        {
            _colors.Clear();

            _colorMask = Header->_colorMask;

            if (_replaced && WorkingUncompressed.Length >= 16 && Header->_data == 8)
            {
                _numEntries = *((bint*) Header + 2);
                _constant = _numEntries == 0;
                RGBAPixel* data = Header->Data;
                if (_constant)
                {
                    _solidColor = *data;
                }
                else
                {
                    int frameCount = ((CLR0Node) Parent.Parent)._numFrames;
                    for (int i = 0; i < frameCount; i++)
                    {
                        _colors.Add(i >= _numEntries ? new ARGBPixel() : (ARGBPixel) (*data++));
                    }

                    _numEntries = frameCount;
                }
            }
            else
            {
                if (_constant)
                {
                    _numEntries = 0;
                    _solidColor = Header->SolidColor;
                }
                else
                {
                    _numEntries = ((CLR0Node) Parent.Parent)._numFrames;
                    RGBAPixel* data = Header->Data;
                    for (int i = 0; i < _numEntries; i++)
                    {
                        _colors.Add(*data++);
                    }
                }
            }

            _name = _target.ToString();

            return false;
        }

        public override void Export(string outPath)
        {
            int length = 12 + (_numEntries != 0 ? _colors.Count * 4 : 4);
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(length);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    CLR0MaterialEntry* entry = (CLR0MaterialEntry*) map.Address;

                    entry->_colorMask = _colorMask;
                    entry->_data = 8;
                    *((bint*) entry + 2) = _numEntries;

                    RGBAPixel* pData = entry->Data;
                    if (_numEntries != 0)
                    {
                        foreach (ARGBPixel p in _colors)
                        {
                            *pData++ = p;
                        }
                    }
                    else
                    {
                        *pData = _solidColor;
                    }
                }
            }
        }

        public void MakeSolid(ARGBPixel color)
        {
            _numEntries = 0;
            _constant = true;
            _solidColor = color;
            SignalPropertyChange();
        }

        public void MakeList()
        {
            _constant = false;
            int entries = ((CLR0Node) Parent._parent)._numFrames;
            _numEntries = _colors.Count;
            NumEntries = entries;
        }

        #region IColorSource Members

        public bool HasPrimary(int id)
        {
            return true;
        }

        public ARGBPixel GetPrimaryColor(int id)
        {
            return _colorMask;
        }

        public void SetPrimaryColor(int id, ARGBPixel color)
        {
            _colorMask = color;
            SignalPropertyChange();
        }

        [Browsable(false)]
        public string PrimaryColorName(int id)
        {
            return "Mask:";
        }

        [Browsable(false)] public int TypeCount => 1;

        [Browsable(false)]
        public int ColorCount(int id)
        {
            return _numEntries == 0 ? 1 : _numEntries;
        }

        public ARGBPixel GetColor(int index, int id)
        {
            return _numEntries == 0 ? _solidColor : _colors[index];
        }

        public void SetColor(int index, int id, ARGBPixel color)
        {
            if (_numEntries == 0)
            {
                _solidColor = color;
            }
            else
            {
                _colors[index] = color;
            }

            SignalPropertyChange();
        }

        public bool GetColorConstant(int id)
        {
            return Constant;
        }

        public void SetColorConstant(int id, bool constant)
        {
            Constant = constant;
        }

        #endregion
    }
}
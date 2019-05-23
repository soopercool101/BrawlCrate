using System;
using System.Collections.Generic;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using System.Collections;
using BrawlLib.SSBB.ResourceNodes;

namespace Ikarus.MovesetFile
{
    public unsafe class ModelVisibility : MovesetEntryNode, IEnumerable<ModelVisReference>
    {
        #region Child Enumeration

        public IEnumerator<ModelVisReference> GetEnumerator() { return _references.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
        
        private List<ModelVisReference> _references;

        public int Count { get { return _references.Count; } }
        public ModelVisReference this[int i]
        {
            get
            {
                if (i >= 0 && i < _references.Count)
                    return _references[i];
                return null;
            }
            set
            {
                if (i >= 0 && i < _references.Count)
                {
                    SignalPropertyChange();
                    _references[i] = value;
                }
            }
        }
        public void Insert(int i, ModelVisReference e)
        {
            if (i >= 0)
            {
                if (i < _references.Count)
                    _references.Insert(i, e);
                else
                    _references.Add(e);
                SignalRebuildChange();
            }
        }
        public void Add(ModelVisReference e)
        {
            _references.Add(e);
            SignalRebuildChange();
        }
        public void RemoveAt(int i)
        {
            if (i >= 0 && i < _references.Count)
            {
                _references.RemoveAt(i);
                SignalRebuildChange();
            }
        }
        public void Clear()
        {
            if (_references.Count != 0)
            {
                _references.Clear();
                SignalRebuildChange();
            }
        }

        #endregion

        sModelDisplay hdr;

        [Category("Model Visibility")]
        public int EntryOffset { get { return hdr._entryOffset; } }
        [Category("Model Visibility")]
        public int EntryCount { get { return hdr._entryCount; } }
        [Category("Model Visibility")]
        public int DefaultsOffset { get { return hdr._defaultsOffset; } }
        [Category("Model Visibility")]
        public int DefaultsCount { get { return hdr._defaultsCount; } }

        protected override void OnParse(VoidPtr address)
        {
            _references = new List<ModelVisReference>();

            sModelDisplay* h = (sModelDisplay*)address;

            hdr = *h;

            VoidPtr entries = BaseAddress + EntryOffset;
            VoidPtr defaults = BaseAddress + DefaultsOffset;

            //Parse references and their switches here.
            //Switches will parse their own groups.
            for (int i = 0; i < (EntryOffset == 0 ? 0 : ((DefaultsOffset == 0 ? _offset : DefaultsOffset) - EntryOffset) / 4); i++)
            {
                ModelVisReference offset = Parse<ModelVisReference>(entries + i * 4);
                _references.Add(offset);

                if (offset.DataOffset == 0)
                    continue;

                if (_root.GetSize(offset.DataOffset) != EntryCount * 8)
                    Console.WriteLine((_root.GetSize(offset.DataOffset) - EntryCount * 8).ToString());

                VoidPtr offAddr = BaseAddress + offset.DataOffset;
                for (int c = 0; c < EntryCount; c++)
                    offset.Add(Parse<ModelVisBoneSwitch>(offAddr + c * 8));
            }

            //Parse defaults
            if (_references.Count > 0)
                for (int i = 0; i < DefaultsCount; i++)
                {
                    sModelDisplayDefaults* def = (sModelDisplayDefaults*)(defaults + i * 8);
                    for (int x = 0; x < ((DefaultsOffset == 0 ? _offset : DefaultsOffset) - EntryOffset) / 4; x++)
                        if (_references[x].Count > 0)
                            (_references[x][def->_switchIndex] as ModelVisBoneSwitch)._defaultGroup = def->_defaultGroup;
                }
        }

        //public override int GetSize()
        //{
        //    int size = 16;
        //    _lookupCount = (Children.Count > 0 ? 1 : 0);

        //    int defCount = 0;
        //    foreach (OffsetValue r in Children)
        //    {
        //        size += 4;

        //        if (r.Children.Count > 0)
        //            _lookupCount++;
                
        //        foreach (ModelVisBoneSwitch b in r.Children)
        //        {
        //            size += 8 + (b._defaultGroup < 0 ? 0 : 8);

        //            if (b._defaultGroup >= 0)
        //                defCount++;

        //            if (b.Children.Count > 0)
        //                _lookupCount++;

        //            foreach (MoveDefModelVisGroupNode o in b.Children)
        //            {
        //                size += 8 + o.Children.Count * 4;
        //                if (o.Children.Count > 0)
        //                    _lookupCount++;
        //            }
        //        }
        //    }

        //    if (defCount > 0)
        //        _lookupCount++;

        //    return size;
        //}

        //protected override void Write(VoidPtr address)
        //{
        //    _lookupOffsets = new List<VoidPtr>();

        //    int mainOff = 0, defOff = 0, offOff = 0, swtchOff = 0, grpOff = 0;
        //    foreach (OffsetValue r in _references)
        //    {
        //        defOff += 4;
        //        foreach (ModelVisBoneSwitch b in r.Children)
        //        {
        //            offOff += 8;
        //            if (b._defaultGroup >= 0)
        //                mainOff += 8;
        //            foreach (MoveDefModelVisGroupNode o in b.Children)
        //            {
        //                swtchOff += 8;
        //                grpOff += o.Children.Count * 4;
        //            }
        //        }
        //    }

        //    //bones
        //    //groups
        //    //switches
        //    //offsets
        //    //defaults
        //    //header

        //    bint* boneAddr = (bint*)address;
        //    FDefListOffset* groupLists = (FDefListOffset*)((VoidPtr)boneAddr + grpOff);
        //    FDefListOffset* switchLists = (FDefListOffset*)((VoidPtr)groupLists + swtchOff);
        //    bint* offsets = (bint*)((VoidPtr)switchLists + offOff);
        //    FDefModelDisplayDefaults* defaults = (FDefModelDisplayDefaults*)((VoidPtr)offsets + defOff);
        //    FDefModelDisplay* header = (FDefModelDisplay*)((VoidPtr)defaults + mainOff);

        //    _rebuildAddr = header;

        //    header->_entryOffset = GetOffset(offsets);

        //    _lookupOffsets.Add(header->_entryOffset.Address);

        //    header->_entryCount = Children[0].Children.Count; //Children 1 child count will be the same

        //    int defCount = 0;
        //    foreach (OffsetValue r in Children)
        //    {
        //        r._rebuildAddr = offsets;
        //        if (r.Children.Count > 0)
        //        {
        //            *offsets = (int)switchLists - (int)RebuildBase;
        //            _lookupOffsets.Add(offsets);
        //        }
        //        offsets++;
        //        foreach (ModelVisBoneSwitch b in r.Children)
        //        {
        //            b._rebuildAddr = switchLists;
        //            if (b._defaultGroup >= 0)
        //            {
        //                defCount++;
        //                defaults->_switchIndex = b.Index;
        //                (defaults++)->_defaultGroup = b._defaultGroup;
        //            }
        //            switchLists->_listCount = b.Children.Count;
        //            if (b.Children.Count > 0)
        //            {
        //                switchLists->_startOffset = (int)groupLists - (int)RebuildBase;
        //                _lookupOffsets.Add(switchLists->_startOffset.Address);
        //            }
        //            else
        //                switchLists->_startOffset = 0;
        //            switchLists++;
        //            foreach (MoveDefModelVisGroupNode o in b.Children)
        //            {
        //                o._rebuildAddr = groupLists;
        //                groupLists->_listCount = o.Children.Count;
        //                if (o.Children.Count > 0)
        //                {
        //                    groupLists->_startOffset = (int)boneAddr - (int)RebuildBase;
        //                    _lookupOffsets.Add(groupLists->_startOffset.Address);
        //                }
        //                else
        //                    groupLists->_startOffset = 0;
        //                groupLists++;
        //                foreach (BoneIndexEntry bone in o.Children)
        //                {
        //                    bone._rebuildAddr = boneAddr;
        //                    *boneAddr++ = bone.boneIndex;
        //                }
        //            }
        //        }
        //    }

        //    if (defCount > 0)
        //    {
        //        header->_defaultsOffset = GetOffset(offsets);
        //        _lookupOffsets.Add(header->_defaultsOffset.Address);
        //    }
        //    else
        //        header->_defaultsOffset = 0;
        //    header->_defaultsCount = defCount;

        //    return RebuildOffset;
        //}
        public void ResetVisibility()
        {
            for (int i = 0; i < _references.Count; i++)
                ResetVisibility(i);
        }
        public void ResetVisibility(int refId)
        {
            if (refId < 0 || refId >= _references.Count)
                return;

            ModelVisReference entry = _references[refId];

            //First, disable bones
            foreach (ModelVisBoneSwitch Switch in entry)
            {
                int i = 0;
                foreach (ModelVisGroup Group in Switch)
                {
                    if (i != Switch._defaultGroup)
                        foreach (BoneIndexValue b in Group._bones)
                            if (b.BoneNode != null)
                                foreach (DrawCall p in b.BoneNode._visDrawCalls)
                                    p._render = false;
                    i++;
                }
            }

            //Now, enable bones
            foreach (ModelVisBoneSwitch Switch in entry)
                if (Switch._defaultGroup >= 0 && Switch._defaultGroup < Switch.Count)
                {
                    ModelVisGroup Group = Switch[Switch._defaultGroup];
                    foreach (BoneIndexValue b in Group._bones)
                        if (b.BoneNode != null)
                            foreach (DrawCall p in b.BoneNode._visDrawCalls)
                                p._render = true;
                }
        }

        public void ApplyVisibility(int refId, int switchID, int groupID)
        {
            if (refId < 0 || refId >= _references.Count)
                return;

            //Get the target reference
            ModelVisReference refEntry = _references[refId];

            //Check if the reference and switch id is usable
            if (switchID >= refEntry.Count || switchID < 0)
                return;

            //Turn off objects
            ModelVisBoneSwitch switchEntry = refEntry[switchID];
            foreach (ModelVisGroup grp in switchEntry)
                foreach (BoneIndexValue b in grp._bones)
                    if (b.BoneNode != null)
                        foreach (DrawCall obj in b.BoneNode._visDrawCalls)
                            obj._render = false;

            //Check if the group id is usable
            if (groupID >= switchEntry.Count || groupID < 0)
                return;

            //Turn on objects
            ModelVisGroup group = switchEntry[groupID];
            if (group != null)
                foreach (BoneIndexValue b in group._bones)
                    if (b.BoneNode != null)
                        foreach (DrawCall obj in b.BoneNode._visDrawCalls)
                            obj._render = true;
        }
    }

    public unsafe class ModelVisReference : OffsetValue, IEnumerable<ModelVisBoneSwitch>
    {
        #region Child Enumeration

        public IEnumerator<ModelVisBoneSwitch> GetEnumerator() { return _switches.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        private List<ModelVisBoneSwitch> _switches;

        public int Count { get { return _switches.Count; } }
        public ModelVisBoneSwitch this[int i]
        {
            get
            {
                if (i >= 0 && i < _switches.Count)
                    return _switches[i];
                return null;
            }
            set
            {
                if (i >= 0 && i < _switches.Count)
                {
                    SignalPropertyChange();
                    _switches[i] = value;
                }
            }
        }
        public void Insert(int i, ModelVisBoneSwitch e)
        {
            if (i >= 0)
            {
                if (i < _switches.Count)
                    _switches.Insert(i, e);
                else
                    _switches.Add(e);
                SignalRebuildChange();
            }
        }
        public void Add(ModelVisBoneSwitch e)
        {
            _switches.Add(e);
            SignalRebuildChange();
        }
        public void RemoveAt(int i)
        {
            if (i >= 0 && i < _switches.Count)
            {
                _switches.RemoveAt(i);
                SignalRebuildChange();
            }
        }
        public void Clear()
        {
            if (_switches.Count != 0)
            {
                _switches.Clear();
                SignalRebuildChange();
            }
        }

        #endregion

        protected override void OnParse(VoidPtr address)
        {
            _switches = new List<ModelVisBoneSwitch>();

            base.OnParse(address);
        }
    }

    public unsafe class ModelVisBoneSwitch : MovesetEntryNode, IEnumerable<ModelVisGroup>
    {
        #region Child Enumeration

        public IEnumerator<ModelVisGroup> GetEnumerator() { return _children.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        private List<ModelVisGroup> _children;

        public int Count { get { return _children.Count; } }
        public ModelVisGroup this[int i]
        {
            get
            {
                if (i >= 0 && i < Count)
                    return _children[i];
                return null;
            }
            set
            {
                if (i >= 0 && i < Count)
                {
                    SignalPropertyChange();
                    _children[i] = value;
                }
            }
        }
        public void Insert(int i, ModelVisGroup e)
        {
            if (i >= 0)
            {
                if (i < Count)
                    _children.Insert(i, e);
                else
                    _children.Add(e);
                SignalRebuildChange();
            }
        }
        public void Add(ModelVisGroup e)
        {
            _children.Add(e);
            SignalRebuildChange();
        }
        public void RemoveAt(int i)
        {
            if (i >= 0 && i < Count)
            {
                _children.RemoveAt(i);
                SignalRebuildChange();
            }
        }
        public void Clear()
        {
            if (Count != 0)
            {
                _children.Clear();
                SignalRebuildChange();
            }
        }

        #endregion

        private int _dataOffset, _count;
        public int _defaultGroup = -1;

        //[Category("Bone Group Switch")]
        //public int DataOffset { get { return _dataOffset; } }
        //[Category("Bone Group Switch")]
        //public int Count { get { return _count; } }
        [Category("Bone Group Switch")]
        public int DefaultGroup { get { return _defaultGroup; } set { _defaultGroup = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            _children = new List<ModelVisGroup>();

            sListOffset* hdr = (sListOffset*)address;
            _dataOffset = hdr->_startOffset;
            _count = hdr->_listCount;

            VoidPtr addr = BaseAddress + _dataOffset;
            for (int i = 0; i < _count; i++)
                _children.Add(Parse<ModelVisGroup>(addr + i * 8));
        }
    }

    public unsafe class ModelVisGroup : MovesetEntryNode
    {
        public List<BoneIndexValue> _bones;
        
        private int _dataOffset, _count;
        public int _defaultGroup = -1;

        [Category("Bone Group Switch")]
        public int IndicesOffset { get { return _dataOffset; } }
        [Category("Bone Group Switch")]
        public int Count { get { return _count; } }
        [Category("Bone Group Switch")]
        public int DefaultGroup { get { return _defaultGroup; } set { _defaultGroup = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            _bones = new List<BoneIndexValue>();

            sListOffset* hdr = (sListOffset*)address;
            _dataOffset = hdr->_startOffset;
            _count = hdr->_listCount;

            VoidPtr addr = BaseAddress + _dataOffset;
            for (int i = 0; i < Count; i++)
                _bones.Add(Parse<BoneIndexValue>(addr + i * 4));
        }
    }
}

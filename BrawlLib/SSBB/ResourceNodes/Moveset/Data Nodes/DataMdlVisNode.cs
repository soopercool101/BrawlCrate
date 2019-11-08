using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefModelVisibilityNode : MoveDefEntryNode
    {
        internal FDefModelDisplay* Header => (FDefModelDisplay*) WorkingUncompressed.Address;

        private FDefModelDisplay hdr;

        [Category("Model Visibility")] public int EntryOffset => hdr._entryOffset;

        [Category("Model Visibility")] public int EntryCount => hdr._entryCount;

        [Category("Model Visibility")] public int DefaultsOffset => hdr._defaultsOffset;

        [Category("Model Visibility")] public int DefaultsCount => hdr._defaultsCount;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Model Visibility";
            hdr = *Header;
            return true;
        }

        public override void OnPopulate()
        {
            VoidPtr entries = BaseAddress + EntryOffset;
            VoidPtr defaults = BaseAddress + DefaultsOffset;

            for (int i = 0;
                i < (EntryOffset == 0 ? 0 : ((DefaultsOffset == 0 ? _offset : DefaultsOffset) - EntryOffset) / 4);
                i++)
            {
                MoveDefModelVisRefNode offset;
                (offset = new MoveDefModelVisRefNode {_name = "Reference" + (i + 1)}).Initialize(this,
                    entries + i * 4, 4);

                if (offset.DataOffset == 0)
                {
                    continue;
                }

                if (Root.GetSize(offset.DataOffset) != EntryCount * 8)
                {
                    Console.WriteLine(Root.GetSize(offset.DataOffset) - EntryCount * 8);
                }

                VoidPtr offAddr = BaseAddress + offset.DataOffset;
                for (int c = 0; c < EntryCount; c++)
                {
                    MoveDefBoneSwitchNode Switch;
                    (Switch = new MoveDefBoneSwitchNode {_name = "BoneSwitch" + c}).Initialize(offset,
                        offAddr + c * 8, 8);
                    int sCount = Switch.Count;
                    VoidPtr gAddr = BaseAddress + Switch.DataOffset;
                    for (int s = 0; s < sCount; s++)
                    {
                        MoveDefModelVisGroupNode Group;
                        (Group = new MoveDefModelVisGroupNode {_name = "BoneGroup" + s}).Initialize(Switch,
                            gAddr + s * 8, 8);
                        int gCount = Group.Count;
                        VoidPtr bAddr = BaseAddress + Group.DataOffset;
                        for (int g = 0; g < gCount; g++)
                        {
                            new MoveDefBoneIndexNode().Initialize(Group, bAddr + g * 4, 4);
                        }
                    }
                }
            }

            if (Children.Count > 0)
            {
                for (int i = 0; i < DefaultsCount; i++)
                {
                    FDefModelDisplayDefaults* def = (FDefModelDisplayDefaults*) (defaults + i * 8);
                    for (int x = 0; x < ((DefaultsOffset == 0 ? _offset : DefaultsOffset) - EntryOffset) / 4; x++)
                    {
                        if (Children[x].Children.Count > 0)
                        {
                            (Children[x].Children[def->_switchIndex] as MoveDefBoneSwitchNode).defaultGroup =
                                def->_defaultGroup;
                        }
                    }
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 16;
            _lookupCount = Children.Count > 0 ? 1 : 0;

            int defCount = 0;
            foreach (MoveDefModelVisRefNode r in Children)
            {
                size += 4;

                if (r.Children.Count > 0)
                {
                    _lookupCount++;
                }

                foreach (MoveDefBoneSwitchNode b in r.Children)
                {
                    size += 8 + (b.defaultGroup < 0 ? 0 : 8);

                    if (b.defaultGroup >= 0)
                    {
                        defCount++;
                    }

                    if (b.Children.Count > 0)
                    {
                        _lookupCount++;
                    }

                    foreach (MoveDefModelVisGroupNode o in b.Children)
                    {
                        size += 8 + o.Children.Count * 4;
                        if (o.Children.Count > 0)
                        {
                            _lookupCount++;
                        }
                    }
                }
            }

            if (defCount > 0)
            {
                _lookupCount++;
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _lookupOffsets = new List<int>();

            int mainOff = 0, defOff = 0, offOff = 0, swtchOff = 0, grpOff = 0;
            foreach (MoveDefModelVisRefNode r in Children)
            {
                defOff += 4;
                foreach (MoveDefBoneSwitchNode b in r.Children)
                {
                    offOff += 8;
                    if (b.defaultGroup >= 0)
                    {
                        mainOff += 8;
                    }

                    foreach (MoveDefModelVisGroupNode o in b.Children)
                    {
                        swtchOff += 8;
                        grpOff += o.Children.Count * 4;
                    }
                }
            }

            //bones
            //groups
            //switches
            //offsets
            //defaults
            //header

            bint* boneAddr = (bint*) address;
            FDefListOffset* groupLists = (FDefListOffset*) ((VoidPtr) boneAddr + grpOff);
            FDefListOffset* switchLists = (FDefListOffset*) ((VoidPtr) groupLists + swtchOff);
            bint* offsets = (bint*) ((VoidPtr) switchLists + offOff);
            FDefModelDisplayDefaults* defaults = (FDefModelDisplayDefaults*) ((VoidPtr) offsets + defOff);
            FDefModelDisplay* header = (FDefModelDisplay*) ((VoidPtr) defaults + mainOff);

            _entryOffset = header;

            header->_entryOffset = (int) offsets - (int) _rebuildBase;

            _lookupOffsets.Add((int) header->_entryOffset.Address - (int) _rebuildBase);

            header->_entryCount = Children[0].Children.Count; //Children 1 child count will be the same

            int defCount = 0;
            foreach (MoveDefModelVisRefNode r in Children)
            {
                r._entryOffset = offsets;
                if (r.Children.Count > 0)
                {
                    *offsets = (int) switchLists - (int) _rebuildBase;
                    _lookupOffsets.Add((int) offsets - (int) _rebuildBase);
                }

                offsets++;
                foreach (MoveDefBoneSwitchNode b in r.Children)
                {
                    b._entryOffset = switchLists;
                    if (b.defaultGroup >= 0)
                    {
                        defCount++;
                        defaults->_switchIndex = b.Index;
                        (defaults++)->_defaultGroup = b.defaultGroup;
                    }

                    switchLists->_listCount = b.Children.Count;
                    if (b.Children.Count > 0)
                    {
                        switchLists->_startOffset = (int) groupLists - (int) _rebuildBase;
                        _lookupOffsets.Add((int) switchLists->_startOffset.Address - (int) _rebuildBase);
                    }
                    else
                    {
                        switchLists->_startOffset = 0;
                    }

                    switchLists++;
                    foreach (MoveDefModelVisGroupNode o in b.Children)
                    {
                        o._entryOffset = groupLists;
                        groupLists->_listCount = o.Children.Count;
                        if (o.Children.Count > 0)
                        {
                            groupLists->_startOffset = (int) boneAddr - (int) _rebuildBase;
                            _lookupOffsets.Add((int) groupLists->_startOffset.Address - (int) _rebuildBase);
                        }
                        else
                        {
                            groupLists->_startOffset = 0;
                        }

                        groupLists++;
                        foreach (MoveDefBoneIndexNode bone in o.Children)
                        {
                            bone._entryOffset = boneAddr;
                            *boneAddr++ = bone.boneIndex;
                        }
                    }
                }
            }

            if (defCount > 0)
            {
                header->_defaultsOffset = (int) offsets - (int) _rebuildBase;
                _lookupOffsets.Add((int) header->_defaultsOffset.Address - (int) _rebuildBase);
            }
            else
            {
                header->_defaultsOffset = 0;
            }

            header->_defaultsCount = defCount;
        }
    }

    public unsafe class MoveDefModelVisRefNode : MoveDefEntryNode
    {
        internal bint* Header => (bint*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDefMdlVisRef;

        internal int i;

        [Category("Offset Entry")] public int DataOffset => i;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            i = *Header;
            if (_name == null)
            {
                _extNode = Root.IsExternal(DataOffset);
                if (_extNode != null && !_extOverride)
                {
                    _name = _extNode.Name;
                    _extNode._refs.Add(this);
                }
            }

            if (_name == null)
            {
                _name = "Offset" + Index;
            }

            return false;
        }
    }

    public unsafe class MoveDefBoneSwitchNode : MoveDefEntryNode
    {
        internal FDefListOffset* Header => (FDefListOffset*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDefMdlVisSwitch;

        internal int i = 0;

        private int offset, count;

        public int defaultGroup = -1;

        [Category("Bone Group Switch")] public int DataOffset => offset;

        [Category("Bone Group Switch")] public int Count => count;

        [Category("Bone Group Switch")]
        public int DefaultGroup
        {
            get => defaultGroup;
            set
            {
                defaultGroup = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            offset = Header->_startOffset;
            count = Header->_listCount;
            return false;
        }
    }

    public unsafe class MoveDefModelVisGroupNode : MoveDefEntryNode
    {
        internal FDefListOffset* Header => (FDefListOffset*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDefMdlVisGroup;

        internal int i = 0;

        private int offset, count;

        [Category("Bone Group")] public int DataOffset => offset;

        [Category("Bone Group")] public int Count => count;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            offset = Header->_startOffset;
            count = Header->_listCount;
            return false;
        }
    }
}
using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARGroupNode : RSAREntryNode
    {
        internal INFOGroupHeader* Header { get { return (INFOGroupHeader*)WorkingUncompressed.Address; } }
#if DEBUG
        [Browsable(true), Category("DEBUG")]
#else
        [Browsable(false)]
#endif
        public override int StringId { get { return Header == null ? -1 : (int)Header->_stringId; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARGroup; } }

        public BindingList<RSARFileNode> _files = new BindingList<RSARFileNode>();

        public override bool OnInitialize()
        {
            base.OnInitialize();

            //Get file references
            RSARNode rsar = RSARNode;
            VoidPtr offset = &rsar.Header->INFOBlock->_collection;
            RuintList* list = Header->GetCollection(offset);
            int count = list->_numEntries;
            for (int i = 0; i < count; i++)
            {
                INFOGroupEntry* entry = (INFOGroupEntry*)list->Get(offset, i);
                int id = entry->_fileId;
                _files.Add(rsar.Files[id] as RSARFileNode);
                rsar.Files[id]._groupRefs.Add(this);
            }

            SetSizeInternal(INFOGroupHeader.Size + 4 + _files.Count * (8 + INFOGroupEntry.Size));

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return INFOGroupHeader.Size + 4 + _files.Count * (8 + INFOGroupEntry.Size);
        }

        internal INFOGroupHeader* _headerAddr;
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            INFOGroupHeader* header = (INFOGroupHeader*)address;
            RuintList* list = (RuintList*)(address + INFOGroupHeader.Size);
            INFOGroupEntry* entries = (INFOGroupEntry*)((VoidPtr)list + 4 + _files.Count * 8);

            _headerAddr = header;

            //I believe these two values are set at runtime
            header->_entryNum = -1;
            header->_extFilePathRef = new ruint(ruint.RefType.Address, 0, 0);

            header->_stringId = _rebuildStringId;
            header->_listOffset = (uint)((VoidPtr)list - _rebuildBase);

            list->_numEntries = _files.Count;
            for (int i = 0; i < _files.Count; ++i)
            {
                list->Entries[i] = (uint)((VoidPtr)(&entries[i]) - _rebuildBase);
                entries[i]._fileId = _files[i]._fileIndex;
            }
        }
    }
}

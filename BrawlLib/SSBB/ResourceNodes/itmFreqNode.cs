using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ItmFreqNode : ARCEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.NoEditFolder; } }
        internal ItmFreqHeader* Header { get { return (ItmFreqHeader*)WorkingUncompressed.Address; } }
        internal ItmFreqTableList* TList
        {
            get
            {
                return (ItmFreqTableList*)(WorkingUncompressed.Address + Header->_DataLength - 0x08);
            }
        }
        ItmFreqOffEntry _t1, _t2, _t3, _t4, _t5;


        // Header variables
        bint _dataLength, _fileSize, _DTableCount, _offCount = 0;

        // Public variables
        public List<bint> _pointerList = new List<bint>();
        public List<ItmFreqOffPair> _DataTable = new List<ItmFreqOffPair>();
        public CompactStringTable _strings = new CompactStringTable();
        public VoidPtr BaseAddress;
        public VoidPtr _pPointerList;
        public VoidPtr _pDataTable;
        public int _numTables;


        public override bool OnInitialize()
        {
            base.OnInitialize();

            _dataLength = Header->_DataLength;
            _fileSize = Header->_Length;
            _DTableCount = Header->_DataTable;
            _offCount = Header->_OffCount;

            BaseAddress = (VoidPtr)Header + 0x20;
            _pPointerList = (VoidPtr)BaseAddress + _dataLength;
            _strings.Add("genParamSet");

            _t1 = TList->_table1;
            _t2 = TList->_table2;
            _t3 = TList->_table3;
            _t4 = TList->_table4;
            _t5 = TList->_table5;

            for (int i = 0; i < 5; i++)
                if (TList->Entries[i]._count > 0)
                    _numTables++;

            // Add the offsets to the pointers list, then the offset to the table list.
            for (int i = 0; i < _offCount; i++)
                _pointerList.Add(*(bint*)(_pPointerList + (i * 4)));

            // Get the location for the Data Table, then add each entry to the list.
            _pDataTable = (VoidPtr)_pPointerList + (_offCount * 4);
            for (int i = 0; i < Header->_DataTable; i++)
                _DataTable.Add(new ItmFreqOffPair(*(bint*)(_pDataTable + (i * 8)), *(bint*)((_pDataTable + (i * 8)) + 4)));

            if (_name == null)
                _name = "Item Generation";

            return _numTables > 0;
        }
        public override void OnPopulate()
        {
            for (int i = 0; i < 5; i++)
            {
                // TODO 
                // This initiates all 5 tables regardless of whether they are filled in or not because
                // if they are not initialized, the size of the file will be thrown off by the
                // size of the missing entries.

                ItmFreqOffEntry* table = (ItmFreqOffEntry*)((int)TList + (int)(i * 8));
                DataSource TableSource = new DataSource(table, 0x08);
                new TableNode().Initialize(this, TableSource);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Update base address for children.
            BaseAddress = (VoidPtr)address + 0x20;
            _pointerList = new List<bint>();

            // Initiate header struct
            ItmFreqHeader* Header = (ItmFreqHeader*)address;
            *Header = new ItmFreqHeader();
            Header->_Length = _fileSize;
            Header->_DataTable = _DTableCount;
            Header->_OffCount = _offCount;
            Header->_DataLength = _dataLength;
            Header->_pad0 = Header->_pad1 =
            Header->_pad2 = Header->_pad3 = 0;

            // Initiate the table list and update pointer to pointer list
            ItmFreqTableList* TList = (ItmFreqTableList*)(address + Header->_DataLength - 0x08);
            *TList = new ItmFreqTableList();
            TList->_table1 = _t1;
            TList->_table2 = _t2;
            TList->_table3 = _t3;
            TList->_table4 = _t4;
            TList->_table5 = _t5;

            _pPointerList = (VoidPtr)TList + 0x28;

            // Rebuild children using new TableList location
            for (int i = 0; i < Children.Count; i++)
                Children[i].Rebuild(TList + (i * 8), 0x08, true);

            // Add Data header offsets to pointer list
            foreach (ItmFreqOffPair p in _DataTable)
                _pointerList.Add(p._offset1);

            // Write pointers to the rebuilt address
            for (int i = 0; i < _pointerList.Count; i++)
                *(bint*)((VoidPtr)_pPointerList + (i * 4)) = _pointerList[i];

            // Update the pointer to the data table and write data table to it
            _pDataTable = (VoidPtr)_pPointerList + (_pointerList.Count * 4);
            for (int i = 0; i < _DataTable.Count; i++)
                *(ItmFreqOffPair*)((VoidPtr)_pDataTable + (i * 8)) = _DataTable[i];

            // Finally, write the string table for the Data and external data tables
            _strings.WriteTable(_pDataTable + (_DataTable.Count * 8));
        }
        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqHeader.Size;
            foreach (TableNode node in Children)
                size += node.CalculateSize(true);
            size += (_pointerList.Count * 4);
            size += _DataTable.Count * 8;
            size += _strings.TotalSize;
            return size;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            ItmFreqHeader* header = (ItmFreqHeader*)source.Address;
            return header->_Length == source.Length &&
                header->_DataLength < source.Length &&
                header->Str == "genParamSet" ? new ItmFreqNode() : null;
        }
    }

    public unsafe class TableNode : ItmFreqBaseNode
    {
        internal ItmFreqOffEntry* Header { get { return (ItmFreqOffEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.NoEditFolder; } }

        private int _entryOffset;
        [Browsable(false)]
        public int Offset { get { return _entryOffset; } set { _entryOffset = value; SignalPropertyChange(); } }

        private int _count;
        [Browsable(false)]
        public int Count { get { return _count; } set { _count = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _entryOffset = Header->_offset;
            _count = Header->_count;

            if (_name == null)
                _name = String.Format("Table[{0}]", Index);

            return _count > 0;
        }
        public override void OnPopulate()
        {
            for (int i = 0; i < Count; i++)
            {
                ItmFreqGroup* group = (ItmFreqGroup*)(BaseAddress + Offset + (i * 0x14));
                DataSource GroupSource = new DataSource(group, 0x14);
                new TableGroupNode().Initialize(this, GroupSource);
            }
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqOffEntry* Header = (ItmFreqOffEntry*)address;
            *Header = new ItmFreqOffEntry();
            Header->_offset = _entryOffset;
            Header->_count = _count;
            //Root._pointerList.Add(((int)address - (int)BaseAddress));

            for (int i = 0; i < Children.Count; i++)
                Children[i].Rebuild(BaseAddress + Header->_offset + (i * 0x14), 0x14, force);
        }
        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqOffEntry.Size;
            foreach (TableGroupNode node in Children)
                size += node.CalculateSize(force);
            return size;
        }
    }
    public unsafe class TableGroupNode : ItmFreqBaseNode
    {
        internal ItmFreqGroup* Header { get { return (ItmFreqGroup*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.NoEditFolder; } }

        private bint _unk0;
        [Category("Group Node")]
        public bint Unknown0 { get { return _unk0; } set { _unk0 = value; SignalPropertyChange(); } }

        private bint _unk1;
        [Category("Group Node")]
        public bint Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }

        private bint _unk2;
        [Category("Group Node")]
        public bint Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }

        private bint _unk3;
        [Category("Group Node")]
        public bint Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }

        private bint _Offset;
        [Browsable(false)]
        public bint Offset { get { return _Offset; } }

        private bint _count;
        [Browsable(false)]
        public bint Count { get { return _count; } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _unk0 = Header->_unknown0;
            _unk1 = Header->_unknown1;
            _unk2 = Header->_unknown2;
            _unk3 = Header->_unknown3;
            _Offset = Header->_entryOffset;
            _count = Header->_entryCount;

            if (_name == null)
                _name = String.Format("Group[{0}]", Index);

            return Header->_entryCount > 0;
        }
        public override void OnPopulate()
        {
            // initiate the frequency entries corrosponding to the current group.
            for (int b = 0; b < _count; b++)
            {
                ItmFreqEntry* entry = (ItmFreqEntry*)(BaseAddress + (_Offset + (b * 0x10)));
                DataSource EntrySource = new DataSource(entry, 0x10);
                new ItmFreqEntryNode().Initialize(this, EntrySource);
            }
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqGroup* Header = (ItmFreqGroup*)address;
            *Header = new ItmFreqGroup();
            Header->_entryCount = _count;
            Header->_entryOffset = _Offset;
            Header->_unknown0 = _unk0;
            Header->_unknown1 = _unk1;
            Header->_unknown2 = _unk2;
            Header->_unknown3 = _unk3;
            Root._pointerList.Add(((int)address - (int)BaseAddress) + 0x0c);

            for (int i = 0; i < Children.Count; i++)
                Children[i].Rebuild(BaseAddress + Header->_entryOffset + (i * 0x10), 0x10, force);
        }
        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqGroup.Size;
            foreach (ItmFreqEntryNode node in Children)
                size += node.CalculateSize(force);
            return size;
        }
    }
    public unsafe class ItmFreqEntryNode : ItmFreqBaseNode
    {
        internal ItmFreqEntry* Header { get { return (ItmFreqEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        private int _id;
        private Item _item;
        [Category("Item")]
        [DisplayName("Item ID")]
        [Description("The ID of the item to spawn.")]
        [TypeConverter(typeof(DropDownListItemIDs))]
        public int ItemID
        {
            get
            {
                return Header->_ID;
            }
            set
            {
                if (Header->_ID < 0 || value < 0) return;
                Header->_ID = value;
                SignalPropertyChange(); UpdateName();
            }
        }

        private int _costumeID;
        [DisplayName("Costume ID")]
        [Category("Item")]
        [Description("Item costume to use. (e.x Present Barrel/Crate)")]
        public int SubID { get { return _costumeID; } set { _costumeID = value; SignalPropertyChange(); } }

        private float _frequency;
        [DisplayName("Frequency")]
        [Category("Item Frequency")]
        [Description("The spawn frequency of the selected item. Higher values mean a higher spawn rate.")]
        public string Frequency { get { return _frequency.ToString("0.00"); } set { _frequency = float.Parse(value); SignalPropertyChange(); } }

        private short _action;
        [DisplayName("Start Action")]
        [Category("Item")]
        [Description("Possible the spawning action of the item.")]
        public short Action { get { return _action; } set { _action = value; SignalPropertyChange(); } }

        private short _subaction;
        [DisplayName("Start Subaction")]
        [Category("Item")]
        [Description("Possible the spawning subaction of the item.")]
        public short Subaction { get { return _subaction; } set { _subaction = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _id = Header->_ID;
            _costumeID = Header->_subItem;
            _frequency = Header->_frequency;
            _action = Header->_action;
            _subaction = Header->_subaction;

            if (_name == null)
                UpdateName();

            return false;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqEntry* Header = (ItmFreqEntry*)address;
            *Header = new ItmFreqEntry();
            Header->_action = _action;
            Header->_frequency = _frequency;
            Header->_ID = _id;
            Header->_subaction = _subaction;
            Header->_subItem = _costumeID;
        }
        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqEntry.Size;
            return size;
        }
        public void UpdateName()
        {
            var item = Item.Items.Where(s => s.ID == ItemID).FirstOrDefault();
			bool changed = this._changed;
            Name = "0x" + ItemID.ToString("X2") + (item == null ? "" : (" - " + item.Name));
			this._changed = changed;
		}

    }

    public unsafe class ItmFreqBaseNode : ResourceNode
    {
        [Browsable(false)]
        public ItmFreqNode Root
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is ItmFreqNode) && (n != null))
                    n = n._parent;
                return n as ItmFreqNode;
            }
        }
        [Browsable(false)]
        public VoidPtr Data { get { return (VoidPtr)WorkingUncompressed.Address; } }
        [Browsable(false)]
        public VoidPtr BaseAddress
        {
            get
            {
                if (Root == null)
                    return 0;
                return Root.BaseAddress;
            }
        }
        [Browsable(false)]
        public int _FileOffset { get { if (Data != null) return (int)Data - (int)BaseAddress; else return 0; } }

        [Browsable(true)]
        [Category("Entry")]
        public string FileOffset { get { return _FileOffset.ToString("x"); } }

        [Browsable(true)]
        [Category("Entry")]
        public string Length { get { return WorkingUncompressed.Length.ToString("x"); } }

    }
}

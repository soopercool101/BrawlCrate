using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ItmFreqNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.ItemFreqNode;
        public override bool supportsCompression => false;

        //internal ItmFreqHeader* Header { get { return (ItmFreqHeader*)WorkingUncompressed.Address; } }
        internal ItmFreqTableList* TableList => (ItmFreqTableList*) (WorkingUncompressed.Address + _dataLength - 0x08);

        private ItmFreqOffEntry _t1, _t2, _t3, _t4, _t5;

        public ItmFreqNode()
        {
            for (int i = 0; i < 5; i++)
            {
                Children.Add(new ItmTableNode {Name = "Table [" + i + "]"});
            }
        }

        // Header variables
        private bint _dataLength, _fileSize, _DTableCount, _offCount = 0;

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

            ItmFreqHeader* Header = (ItmFreqHeader*) WorkingUncompressed.Address;
            _dataLength = Header->_DataLength;
            _fileSize = Header->_Length;
            _DTableCount = Header->_DataTable;
            _offCount = Header->_OffCount;

            BaseAddress = (VoidPtr) Header + 0x20;
            _pPointerList = BaseAddress + _dataLength;
            _strings.Add("genParamSet");

            _t1 = TableList->_table1;
            _t2 = TableList->_table2;
            _t3 = TableList->_table3;
            _t4 = TableList->_table4;
            _t5 = TableList->_table5;

            for (int i = 0; i < 5; i++)
            {
                if (TableList->Entries[i]._count > 0)
                {
                    _numTables++;
                }
            }

            // Add the offsets to the pointers list, then the offset to the table list.
            for (int i = 0; i < _offCount; i++)
            {
                _pointerList.Add(*(bint*) (_pPointerList + i * 4));
            }

            // Get the location for the Data Table, then add each entry to the list.
            _pDataTable = _pPointerList + _offCount * 4;
            for (int i = 0; i < Header->_DataTable; i++)
            {
                _DataTable.Add(new ItmFreqOffPair(*(bint*) (_pDataTable + i * 8), *(bint*) (_pDataTable + i * 8 + 4)));
            }

            if (_name == null)
            {
                _name = "Item Generation";
            }

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

                ItmFreqOffEntry* table = (ItmFreqOffEntry*) ((int) TableList + i * 8);
                DataSource TableSource = new DataSource(table, 0x08);
                new ItmTableNode().Initialize(this, TableSource);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Update base address for children.
            BaseAddress = address + 0x20;
            _pointerList = new List<bint>();

            // Initiate header struct
            ItmFreqHeader* Header = (ItmFreqHeader*) address;
            *Header = new ItmFreqHeader();
            Header->_Length = _fileSize;
            Header->_DataTable = _DTableCount;
            Header->_OffCount = _offCount;
            Header->_DataLength = _dataLength;
            Header->_pad0 = Header->_pad1 =
                Header->_pad2 = Header->_pad3 = 0;

            // Initiate the table list and update pointer to pointer list
            ItmFreqTableList* TList = (ItmFreqTableList*) (address + Header->_DataLength - 0x08);
            *TList = new ItmFreqTableList();
            TList->_table1 = _t1;
            TList->_table2 = _t2;
            TList->_table3 = _t3;
            TList->_table4 = _t4;
            TList->_table5 = _t5;

            _pPointerList = (VoidPtr) TList + 0x28;

            // Rebuild children using new TableList location
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(TList + i * 8, 0x08, true);
            }

            // Add Data header offsets to pointer list
            foreach (ItmFreqOffPair p in _DataTable)
            {
                _pointerList.Add(p._offset1);
            }

            // Write pointers to the rebuilt address
            for (int i = 0; i < _pointerList.Count; i++)
            {
                *(bint*) (_pPointerList + i * 4) = _pointerList[i];
            }

            // Update the pointer to the data table and write data table to it
            _pDataTable = _pPointerList + _pointerList.Count * 4;
            for (int i = 0; i < _DataTable.Count; i++)
            {
                *(ItmFreqOffPair*) (_pDataTable + i * 8) = _DataTable[i];
            }

            // Finally, write the string table for the Data and external data tables
            _strings.WriteTable(_pDataTable + _DataTable.Count * 8);
        }

        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqHeader.Size;
            foreach (ItmTableNode node in Children)
            {
                size += node.CalculateSize(true);
            }

            size += _pointerList.Count * 4;
            size += _DataTable.Count * 8;
            size += _strings.TotalSize;
            return size;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            ItmFreqHeader* header = (ItmFreqHeader*) source.Address;
            return header->_Length == source.Length &&
                   header->_DataLength < source.Length &&
                   header->Str == "genParamSet"
                ? new ItmFreqNode()
                : null;
        }
    }

    public unsafe class ItmTableNode : ItmFreqBaseNode
    {
        //internal ItmFreqOffEntry* Header { get { return (ItmFreqOffEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceFileType => ResourceType.ItemFreqTableNode;
        public override bool supportsCompression => false;

        private int _entryOffset;

        //[Browsable(false)]
        public int Offset
        {
            get => _entryOffset;
            set
            {
                _entryOffset = value;
                SignalPropertyChange();
            }
        }

        private int _count;

        //[Browsable(false)]
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            ItmFreqOffEntry* Header = (ItmFreqOffEntry*) WorkingUncompressed.Address;
            _entryOffset = Header->_offset;
            _count = Header->_count;

            if (_name == null)
            {
                _name = $"Table [{Index}]";
            }

            return _count > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Count; i++)
            {
                ItmFreqGroup* group = (ItmFreqGroup*) (BaseAddress + Offset + i * 0x14);
                DataSource GroupSource = new DataSource(group, 0x14);
                new ItmTableGroupNode().Initialize(this, GroupSource);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqOffEntry* Header = (ItmFreqOffEntry*) address;
            *Header = new ItmFreqOffEntry();
            Header->_offset = _entryOffset;
            Header->_count = _count;
            //Root._pointerList.Add(((int)address - (int)BaseAddress));

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(BaseAddress + Header->_offset + i * 0x14, 0x14, force);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqOffEntry.Size;
            foreach (ItmTableGroupNode node in Children)
            {
                size += node.CalculateSize(force);
            }

            return size;
        }
    }

    public unsafe class ItmTableGroupNode : ItmFreqBaseNode
    {
        //internal ItmFreqGroup* Header { get { return (ItmFreqGroup*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceFileType => ResourceType.ItemFreqTableGroupNode;
        public override bool supportsCompression => false;

        private bint _unk0;

        [Category("Group Node")]
        public int Unknown0
        {
            get => _unk0;
            set
            {
                _unk0 = value;
                SignalPropertyChange();
            }
        }

        private bint _unk1;

        [Category("Group Node")]
        public int Unknown1
        {
            get => _unk1;
            set
            {
                _unk1 = value;
                SignalPropertyChange();
            }
        }

        private bint _unk2;

        [Category("Group Node")]
        public int Unknown2
        {
            get => _unk2;
            set
            {
                _unk2 = value;
                SignalPropertyChange();
            }
        }

        private bint _unk3;

        [Category("Group Node")]
        public int Unknown3
        {
            get => _unk3;
            set
            {
                _unk3 = value;
                SignalPropertyChange();
            }
        }

        private bint _Offset;
        [Browsable(false)] public bint Offset => _Offset;

        private bint _count;
        [Browsable(false)] public bint Count => _count;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            ItmFreqGroup* Header = (ItmFreqGroup*) WorkingUncompressed.Address;
            _unk0 = Header->_unknown0;
            _unk1 = Header->_unknown1;
            _unk2 = Header->_unknown2;
            _unk3 = Header->_unknown3;
            _Offset = Header->_entryOffset;
            _count = Header->_entryCount;

            if (_name == null)
            {
                _name = $"Group [{Index}]";
            }

            return Header->_entryCount > 0;
        }

        public override void OnPopulate()
        {
            // initiate the frequency entries corrosponding to the current group.
            for (int b = 0; b < _count; b++)
            {
                ItmFreqEntry* entry = (ItmFreqEntry*) (BaseAddress + (_Offset + b * 0x10));
                DataSource EntrySource = new DataSource(entry, 0x10);
                new ItmFreqEntryNode().Initialize(this, EntrySource);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqGroup* Header = (ItmFreqGroup*) address;
            *Header = new ItmFreqGroup();
            Header->_entryCount = _count;
            Header->_entryOffset = _Offset;
            Header->_unknown0 = _unk0;
            Header->_unknown1 = _unk1;
            Header->_unknown2 = _unk2;
            Header->_unknown3 = _unk3;
            Root._pointerList.Add((int) address - (int) BaseAddress + 0x0c);

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(BaseAddress + Header->_entryOffset + i * 0x10, 0x10, force);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqGroup.Size;
            foreach (ItmFreqEntryNode node in Children)
            {
                size += node.CalculateSize(force);
            }

            return size;
        }
    }

    public unsafe class ItmFreqEntryNode : ItmFreqBaseNode
    {
        //internal ItmFreqEntry* Header { get { return (ItmFreqEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceFileType => ResourceType.ItemFreqEntryNode;
        public override bool supportsCompression => false;

        private int _id;

        [Category("Item")]
        [DisplayName("Item ID")]
        [Description("The ID of the item to spawn.")]
        [TypeConverter(typeof(DropDownListItemIDs))]
        public int ItemID
        {
            get => _id;
            set
            {
                if (_id < 0 || value < 0)
                {
                    return;
                }

                _id = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        private int _costumeID;

        [DisplayName("SubItem ID")]
        [Category("Item")]
        [Description(
            "Item subset to use. For Barrels, Crates, and Rolling Crates, this determines the costume used. For Bob-Ombs, setting to 1 causes Sudden Death Behavior.")]
        public int SubID
        {
            get => _costumeID;
            set
            {
                _costumeID = value;
                SignalPropertyChange();
            }
        }

        private float _frequency;

        [DisplayName("Frequency")]
        [Category("Item Frequency")]
        [Description("The spawn frequency of the selected item. Higher values mean a higher spawn rate.")]
        public string Frequency
        {
            get => _frequency.ToString("0.00");
            set
            {
                _frequency = float.Parse(value);
                SignalPropertyChange();
            }
        }

        private short _action;

        [DisplayName("Start Action")]
        [Category("Item")]
        [Description("Possible the spawning action of the item.")]
        public short Action
        {
            get => _action;
            set
            {
                _action = value;
                SignalPropertyChange();
            }
        }

        private short _subaction;

        [DisplayName("Start Subaction")]
        [Category("Item")]
        [Description("Possible the spawning subaction of the item.")]
        public short Subaction
        {
            get => _subaction;
            set
            {
                _subaction = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            ItmFreqEntry* Header = (ItmFreqEntry*) WorkingUncompressed.Address;
            _id = Header->_ID;
            _costumeID = Header->_subItem;
            _frequency = Header->_frequency;
            _action = Header->_action;
            _subaction = Header->_subaction;

            if (_name == null)
            {
                UpdateName();
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqEntry* Header = (ItmFreqEntry*) address;
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
            Item item = Item.Items.Where(s => s.ID == ItemID).FirstOrDefault();
            bool changed = _changed;
            Name = "0x" + ItemID.ToString("X2") + (item == null ? "" : " - " + item.Name);
            _changed = changed;
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
                while (!(n is ItmFreqNode) && n != null)
                {
                    n = n._parent;
                }

                return n as ItmFreqNode;
            }
        }

        [Browsable(false)] public VoidPtr Data => WorkingUncompressed.Address;

        [Browsable(false)]
        public VoidPtr BaseAddress
        {
            get
            {
                if (Root == null)
                {
                    return 0;
                }

                return Root.BaseAddress;
            }
        }

        [Browsable(false)]
        public int File_Offset
        {
            get
            {
                if (Data != null)
                {
                    return (int) Data - (int) BaseAddress;
                }
                else
                {
                    return 0;
                }
            }
        }

        [Browsable(true)] [Category("Entry")] public string FileOffset => File_Offset.ToString("x");

        [Browsable(true)] [Category("Entry")] public string Length => WorkingUncompressed.Length.ToString("x");
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ItmFreqNode : ARCEntryNode
    {
        // Header variables
        private bint _dataLength, _fileSize, _DTableCount, _offCount = 0;
        public List<ItmFreqOffPair> _DataTable = new List<ItmFreqOffPair>();
        public int _numTables;
        public VoidPtr _pDataTable;

        // Public variables
        public List<bint> _pointerList = new List<bint>();
        public VoidPtr _pPointerList;
        public CompactStringTable _strings = new CompactStringTable();

        private ItmFreqOffEntry _t1, _t2, _t3, _t4, _t5;
        public VoidPtr BaseAddress;
        public override ResourceType ResourceFileType => ResourceType.NoEditFolder;
        internal ItmFreqHeader* Header => (ItmFreqHeader*) WorkingUncompressed.Address;

        internal ItmFreqTableList* TList =>
            (ItmFreqTableList*) (WorkingUncompressed.Address + Header->_DataLength - 0x08);


        public override bool OnInitialize()
        {
            base.OnInitialize();

            _dataLength = Header->_DataLength;
            _fileSize = Header->_Length;
            _DTableCount = Header->_DataTable;
            _offCount = Header->_OffCount;

            BaseAddress = (VoidPtr) Header + 0x20;
            _pPointerList = BaseAddress + _dataLength;
            _strings.Add("genParamSet");

            _t1 = TList->_table1;
            _t2 = TList->_table2;
            _t3 = TList->_table3;
            _t4 = TList->_table4;
            _t5 = TList->_table5;

            for (var i = 0; i < 5; i++)
                if (TList->Entries[i]._count > 0)
                    _numTables++;

            // Add the offsets to the pointers list, then the offset to the table list.
            for (var i = 0; i < _offCount; i++) _pointerList.Add(*(bint*) (_pPointerList + i * 4));

            // Get the location for the Data Table, then add each entry to the list.
            _pDataTable = _pPointerList + _offCount * 4;
            for (var i = 0; i < Header->_DataTable; i++)
                _DataTable.Add(new ItmFreqOffPair(*(bint*) (_pDataTable + i * 8), *(bint*) (_pDataTable + i * 8 + 4)));

            if (_name == null) _name = "Item Generation";

            return _numTables > 0;
        }

        public override void OnPopulate()
        {
            for (var i = 0; i < 5; i++)
            {
                // TODO 
                // This initiates all 5 tables regardless of whether they are filled in or not because
                // if they are not initialized, the size of the file will be thrown off by the
                // size of the missing entries.

                var table = (ItmFreqOffEntry*) ((int) TList + i * 8);
                var TableSource = new DataSource(table, 0x08);
                new TableNode().Initialize(this, TableSource);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Update base address for children.
            BaseAddress = address + 0x20;
            _pointerList = new List<bint>();

            // Initiate header struct
            var Header = (ItmFreqHeader*) address;
            *Header = new ItmFreqHeader();
            Header->_Length = _fileSize;
            Header->_DataTable = _DTableCount;
            Header->_OffCount = _offCount;
            Header->_DataLength = _dataLength;
            Header->_pad0 = Header->_pad1 =
                Header->_pad2 = Header->_pad3 = 0;

            // Initiate the table list and update pointer to pointer list
            var TList = (ItmFreqTableList*) (address + Header->_DataLength - 0x08);
            *TList = new ItmFreqTableList();
            TList->_table1 = _t1;
            TList->_table2 = _t2;
            TList->_table3 = _t3;
            TList->_table4 = _t4;
            TList->_table5 = _t5;

            _pPointerList = (VoidPtr) TList + 0x28;

            // Rebuild children using new TableList location
            for (var i = 0; i < Children.Count; i++) Children[i].Rebuild(TList + i * 8, 0x08, true);

            // Add Data header offsets to pointer list
            foreach (var p in _DataTable) _pointerList.Add(p._offset1);

            // Write pointers to the rebuilt address
            for (var i = 0; i < _pointerList.Count; i++) *(bint*) (_pPointerList + i * 4) = _pointerList[i];

            // Update the pointer to the data table and write data table to it
            _pDataTable = _pPointerList + _pointerList.Count * 4;
            for (var i = 0; i < _DataTable.Count; i++) *(ItmFreqOffPair*) (_pDataTable + i * 8) = _DataTable[i];

            // Finally, write the string table for the Data and external data tables
            _strings.WriteTable(_pDataTable + _DataTable.Count * 8);
        }

        public override int OnCalculateSize(bool force)
        {
            var size = ItmFreqHeader.Size;
            foreach (TableNode node in Children) size += node.CalculateSize(true);

            size += _pointerList.Count * 4;
            size += _DataTable.Count * 8;
            size += _strings.TotalSize;
            return size;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            var header = (ItmFreqHeader*) source.Address;
            return header->_Length == source.Length &&
                   header->_DataLength < source.Length &&
                   header->Str == "genParamSet"
                ? new ItmFreqNode()
                : null;
        }
    }

    public unsafe class TableNode : ItmFreqBaseNode
    {
        private int _count;

        private int _entryOffset;
        internal ItmFreqOffEntry* Header => (ItmFreqOffEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.NoEditFolder;

        [Browsable(false)]
        public int Offset
        {
            get => _entryOffset;
            set
            {
                _entryOffset = value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)]
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

            _entryOffset = Header->_offset;
            _count = Header->_count;

            if (_name == null) _name = string.Format("Table[{0}]", Index);

            return _count > 0;
        }

        public override void OnPopulate()
        {
            for (var i = 0; i < Count; i++)
            {
                var group = (ItmFreqGroup*) (BaseAddress + Offset + i * 0x14);
                var GroupSource = new DataSource(group, 0x14);
                new TableGroupNode().Initialize(this, GroupSource);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var Header = (ItmFreqOffEntry*) address;
            *Header = new ItmFreqOffEntry();
            Header->_offset = _entryOffset;
            Header->_count = _count;
            //Root._pointerList.Add(((int)address - (int)BaseAddress));

            for (var i = 0; i < Children.Count; i++)
                Children[i].Rebuild(BaseAddress + Header->_offset + i * 0x14, 0x14, force);
        }

        public override int OnCalculateSize(bool force)
        {
            var size = ItmFreqOffEntry.Size;
            foreach (TableGroupNode node in Children) size += node.CalculateSize(force);

            return size;
        }
    }

    public unsafe class TableGroupNode : ItmFreqBaseNode
    {
        private bint _unk0;

        private bint _unk1;

        private bint _unk2;

        private bint _unk3;
        internal ItmFreqGroup* Header => (ItmFreqGroup*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.NoEditFolder;

        [Category("Group Node")]
        public bint Unknown0
        {
            get => _unk0;
            set
            {
                _unk0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Group Node")]
        public bint Unknown1
        {
            get => _unk1;
            set
            {
                _unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Group Node")]
        public bint Unknown2
        {
            get => _unk2;
            set
            {
                _unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Group Node")]
        public bint Unknown3
        {
            get => _unk3;
            set
            {
                _unk3 = value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)] public bint Offset { get; private set; }

        [Browsable(false)] public bint Count { get; private set; }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _unk0 = Header->_unknown0;
            _unk1 = Header->_unknown1;
            _unk2 = Header->_unknown2;
            _unk3 = Header->_unknown3;
            Offset = Header->_entryOffset;
            Count = Header->_entryCount;

            if (_name == null) _name = string.Format("Group[{0}]", Index);

            return Header->_entryCount > 0;
        }

        public override void OnPopulate()
        {
            // initiate the frequency entries corrosponding to the current group.
            for (var b = 0; b < Count; b++)
            {
                var entry = (ItmFreqEntry*) (BaseAddress + (Offset + b * 0x10));
                var EntrySource = new DataSource(entry, 0x10);
                new ItmFreqEntryNode().Initialize(this, EntrySource);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var Header = (ItmFreqGroup*) address;
            *Header = new ItmFreqGroup();
            Header->_entryCount = Count;
            Header->_entryOffset = Offset;
            Header->_unknown0 = _unk0;
            Header->_unknown1 = _unk1;
            Header->_unknown2 = _unk2;
            Header->_unknown3 = _unk3;
            Root._pointerList.Add((int) address - (int) BaseAddress + 0x0c);

            for (var i = 0; i < Children.Count; i++)
                Children[i].Rebuild(BaseAddress + Header->_entryOffset + i * 0x10, 0x10, force);
        }

        public override int OnCalculateSize(bool force)
        {
            var size = ItmFreqGroup.Size;
            foreach (ItmFreqEntryNode node in Children) size += node.CalculateSize(force);

            return size;
        }
    }

    public unsafe class ItmFreqEntryNode : ItmFreqBaseNode
    {
        private short _action;

        private int _costumeID;

        private float _frequency;

        private int _id;

        private short _subaction;
        internal ItmFreqEntry* Header => (ItmFreqEntry*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.Unknown;

        //private readonly Item _item;
        [Category("Item")]
        [DisplayName("Item ID")]
        [Description("The ID of the item to spawn.")]
        [TypeConverter(typeof(DropDownListItemIDs))]
        public int ItemID
        {
            get => Header->_ID;
            set
            {
                if (Header->_ID < 0 || value < 0) return;

                Header->_ID = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        [DisplayName("Costume ID")]
        [Category("Item")]
        [Description("Item costume to use. (e.x Present Barrel/Crate)")]
        public int SubID
        {
            get => _costumeID;
            set
            {
                _costumeID = value;
                SignalPropertyChange();
            }
        }

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

            _id = Header->_ID;
            _costumeID = Header->_subItem;
            _frequency = Header->_frequency;
            _action = Header->_action;
            _subaction = Header->_subaction;

            if (_name == null) UpdateName();

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var Header = (ItmFreqEntry*) address;
            *Header = new ItmFreqEntry();
            Header->_action = _action;
            Header->_frequency = _frequency;
            Header->_ID = _id;
            Header->_subaction = _subaction;
            Header->_subItem = _costumeID;
        }

        public override int OnCalculateSize(bool force)
        {
            var size = ItmFreqEntry.Size;
            return size;
        }

        public void UpdateName()
        {
            var item = Item.Items.Where(s => s.ID == ItemID).FirstOrDefault();
            var changed = _changed;
            Name = "0x" + ItemID.ToString("X2") + (item == null ? "" : " - " + item.Name);
            _changed = changed;
        }
    }

    public class ItmFreqBaseNode : ResourceNode
    {
        [Browsable(false)]
        public ItmFreqNode Root
        {
            get
            {
                var n = _parent;
                while (!(n is ItmFreqNode) && n != null) n = n._parent;

                return n as ItmFreqNode;
            }
        }

        [Browsable(false)] public VoidPtr Data => WorkingUncompressed.Address;

        [Browsable(false)]
        public VoidPtr BaseAddress
        {
            get
            {
                if (Root == null) return 0;

                return Root.BaseAddress;
            }
        }

        [Browsable(false)]
        public int File_Offset
        {
            get
            {
                if (Data != null)
                    return (int) Data - (int) BaseAddress;
                return 0;
            }
        }

        [Browsable(true)] [Category("Entry")] public string FileOffset => File_Offset.ToString("x");

        [Browsable(true)] [Category("Entry")] public string Length => WorkingUncompressed.Length.ToString("x");
    }
}
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ItmFreqNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.ItemFreqNode;
        public override Type[] AllowedChildTypes => new[] {typeof(ItmTableNode)};
        public override bool supportsCompression => false;

        internal ItmFreqHeader* Header => (ItmFreqHeader*)WorkingUncompressed.Address;
        internal ItmFreqTableList* TableList => (ItmFreqTableList*) (WorkingUncompressed.Address + Header->_DataLength - 0x08);

        protected override string GetName()
        {
            return base.GetName("Item Generation");
        }

        public ItmFreqNode()
        {
            for (int i = 0; i < 5; i++)
            {
                Children.Add(new ItmTableNode {Name = "Table [" + i + "]"});
            }
        }

        public override bool OnInitialize()
        {
            return Header->_DataTable > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < 5; i++)
            {
                new ItmTableNode().Initialize(this, (*TableList)[i], ItmFreqOffEntry.Size);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqHeader* header = (ItmFreqHeader*)address;
            *header = new ItmFreqHeader();
            header->_Length = length;
            int offCount = 0; // save for later;
            //header->_OffCount;
            header->_DataTable = 1;
            header->_pad0 = 0;
            header->_pad1 = 0;
            header->_pad2 = 0;
            header->_pad3 = 0;

            List<uint> groupOffsets = new List<uint>();
            List<uint> tableOffsets = new List<uint>();

            uint offset = ItmFreqHeader.Size;
            // First, rebuild the items themselves, updating the group offset to the proper value (also reset both sets of offsets)
            foreach (ItmTableNode table in Children)
            {
                table._offset = 0;
                table._dataSize = 0;
                if (table.HasChildren)
                {
                    offCount++;
                    foreach (ItmTableGroupNode group in table.Children)
                    {
                        group._offset = 0;
                        if (group.HasChildren)
                        {
                            offCount++;
                            group._offset = offset - ItmFreqHeader.Size;
                            foreach (ResourceNode item in group.Children)
                            {
                                int size = item.OnCalculateSize(true);
                                item.OnRebuild(address + offset, size, true);
                                offset += (uint)size;
                                table._dataSize += (uint) size;
                            }
                        }
                    }
                    // Rebuild the table's groups
                    table._offset = offset - ItmFreqHeader.Size;
                    foreach (ResourceNode group in table.Children)
                    {
                        int size = group.OnCalculateSize(true);
                        group.OnRebuild(address + offset, size, true);
                        if (group.HasChildren)
                        {
                            groupOffsets.Add(offset - 0x14);
                        }
                        offset += (uint)size;
                        table._dataSize += (uint)size;
                    }
                }

                // Add padding to match vBrawl
                if (table.HasChildren && (table.NextSibling()?.HasChildren ?? true))
                {
                    buint* ptr = (buint*)(address + offset);
                    ptr[0] = 0;
                    offset += 4;
                }
            }

            header->_OffCount = offCount;
            header->_DataLength = length - (ItmFreqHeader.Size + (offCount + 2) * 0x04 + "genParamSet".UTF8Length() + 1);

            // Finally, rebuild tables
            foreach (ResourceNode table in Children)
            {
                int size = table.OnCalculateSize(true);
                table.OnRebuild(address + offset, size, true);
                if (table.HasChildren)
                {
                    tableOffsets.Add(offset - 0x14 - 0x0C);
                }
                offset += (uint)size;
            }

            foreach (uint i in groupOffsets)
            {
                buint* ptr = (buint*)(address + offset);
                ptr[0] = i;
                offset += 4;
            }

            foreach (uint i in tableOffsets)
            {
                buint* ptr = (buint*)(address + offset);
                ptr[0] = i;
                offset += 4;
            }

            if (tableOffsets.Count > 0)
            {
                buint* ptr = (buint*)(address + offset);
                ptr[0] = tableOffsets[0];
                offset += 4;
            }

            buint* clear = (buint*)(address + offset);
            clear[0] = 0;
            offset += 4;

            (address + offset).WriteUTF8String("genParamSet", true);
        }

        public override int OnCalculateSize(bool force)
        {
            int size = ItmFreqHeader.Size;
            int offcount = 0;
            foreach (ResourceNode table in Children)
            {
                size += table.OnCalculateSize(true);
                if (table.HasChildren)
                {
                    offcount++;
                    foreach (ResourceNode group in table.Children)
                    {
                        size += group.OnCalculateSize(true);
                        if (group.HasChildren)
                        {
                            offcount++;
                            foreach (ResourceNode item in group.Children)
                            {
                                size += item.OnCalculateSize(true);
                            }
                        }
                    }
                    if (table.HasChildren && (table.NextSibling()?.HasChildren ?? true))
                    {
                        size += 4;
                    }
                }
            }

            size += offcount * 0x04;
            size += Name.UTF8Length() + 1;

            return size;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            ItmFreqHeader* header = (ItmFreqHeader*) source.Address;
            return header->_Length == source.Length &&
                   header->_DataLength < source.Length &&
                   header->Str == "genParamSet"
                ? new ItmFreqNode()
                : null;
        }
    }

    public unsafe class ItmTableNode : ResourceNode
    {
        internal ItmFreqOffEntry* Header => (ItmFreqOffEntry*)WorkingUncompressed.Address;
        public override Type[] AllowedChildTypes => new[] { typeof(ItmTableGroupNode) };
        public override ResourceType ResourceFileType => ResourceType.ItemFreqTableNode;
        public override bool supportsCompression => false;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = $"Table [{Index}]";
            }

            return Header->_count > 0;
        }

        public override void OnPopulate()
        {
            VoidPtr ptr = Parent.WorkingUncompressed.Address;
            uint offset = Header->_offset + ItmFreqHeader.Size;
            for (int i = 0; i < Header->_count; i++)
            {
                new ItmTableGroupNode().Initialize(this, ptr + offset, ItmFreqGroup.Size);
                offset += ItmFreqGroup.Size;
            }
        }

        internal uint _offset = 0;
        internal uint _dataSize = 0;
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqOffEntry* header = (ItmFreqOffEntry*)address;
            *header = new ItmFreqOffEntry();
            header->_offset = HasChildren ? _offset : 0;
            header->_count = (uint)Children.Count;
        }

        public override int OnCalculateSize(bool force)
        {
            return ItmFreqOffEntry.Size;
        }
    }

    public unsafe class ItmTableGroupNode : ResourceNode
    {
        internal ItmFreqGroup* Header => (ItmFreqGroup*)WorkingUncompressed.Address;
        public override Type[] AllowedChildTypes => new[] { typeof(ItmFreqEntryNode) };
        public override ResourceType ResourceFileType => ResourceType.ItemFreqTableGroupNode;
        public override bool supportsCompression => false;

        public int ChildCount => Children.Count;

        private int _unk0;

        [Category("Group Node")]
        public int Id
        {
            get => _unk0;
            set
            {
                _unk0 = value;
                SignalPropertyChange();
            }
        }

        private int _unk1;

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

        private int _unk2;

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

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = $"Group [{Index}]";
            }

            _unk0 = Header->_unknown0;
            _unk1 = Header->_unknown1;
            _unk2 = Header->_unknown2;

            return Header->_entryCount > 0;
        }

        public override void OnPopulate()
        {
            VoidPtr ptr = Parent.Parent.WorkingUncompressed.Address;
            uint offset = Header->_entryOffset + ItmFreqHeader.Size;
            for (int i = 0; i < Header->_entryCount; i++)
            {
                new ItmFreqEntryNode().Initialize(this, ptr + offset, ItmFreqEntry.Size);
                offset += ItmFreqEntry.Size;
            }
        }

        internal uint _offset = 0;
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqGroup* header = (ItmFreqGroup*)address;
            *header = new ItmFreqGroup();
            header->_entryOffset = _offset;
            header->_entryCount = (uint)Children.Count;
            header->_unknown0 = _unk0;
            header->_unknown1 = _unk1;
            header->_unknown2 = _unk2;
        }

        public override int OnCalculateSize(bool force)
        {
            return ItmFreqGroup.Size;
        }
    }

    public unsafe class ItmFreqEntryNode : ResourceNode
    {
        internal ItmFreqEntry* Header => (ItmFreqEntry*)WorkingUncompressed.Address;
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
        public float Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                SignalPropertyChange();
            }
        }

        private short _min;

        [DisplayName("Minimum to Spawn")]
        [Category("Item")]
        public short Minimum
        {
            get => _min;
            set
            {
                _min = value;
                SignalPropertyChange();
            }
        }

        private short _max;

        [DisplayName("Maximum to Spawn")]
        [Category("Item")]
        public short Maximum
        {
            get => _max;
            set
            {
                _max = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _id = Header->_ID;
            _costumeID = Header->_subItem;
            _frequency = Header->_frequency;
            _min = Header->_min;
            _max = Header->_max;

            if (_name == null)
            {
                UpdateName();
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ItmFreqEntry* _header = (ItmFreqEntry*) address;
            *_header = new ItmFreqEntry();
            _header->_max = _max;
            _header->_frequency = _frequency;
            _header->_ID = _id;
            _header->_min = _min;
            _header->_subItem = _costumeID;
        }

        public override int OnCalculateSize(bool force)
        {
            return ItmFreqEntry.Size;
        }

        public void UpdateName()
        {
            Item item = Item.Items.Where(s => s.ID == ItemID).FirstOrDefault();
            bool changed = _changed;
            Name = "0x" + ItemID.ToString("X2") + (item == null ? "" : " - " + item.Name);
            _changed = changed;
        }
    }
}
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PathingMiscDataNode : ARCEntryNode
    {
        internal PathingMiscData* Header => (PathingMiscData*) WorkingUncompressed.Address;
        //public override ResourceType ResourceFileType => ResourceType.PathingMiscData;

        public override Type[] AllowedChildTypes => new[] {typeof(PathingMiscDataEntryNode)};

        public float _unknown0x08;

        public float Unknown0x08
        {
            get => _unknown0x08;
            set
            {
                _unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x0C;

        public float Unknown0x0C
        {
            get => _unknown0x0C;
            set
            {
                _unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x10;

        public float Unknown0x10
        {
            get => _unknown0x10;
            set
            {
                _unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x14;

        public float Unknown0x14
        {
            get => _unknown0x14;
            set
            {
                _unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x18;

        public float Unknown0x18
        {
            get => _unknown0x18;
            set
            {
                _unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x1C;

        public float Unknown0x1C
        {
            get => _unknown0x1C;
            set
            {
                _unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source = new DataSource((*Header)[i], (int) PathingMiscDataEntry.Size);
                new PathingMiscDataEntryNode().Initialize(this, source);
                Children.Last().Populate();
            }
        }

        public override bool OnInitialize()
        {
            _unknown0x08 = Header->_unknown0x08;
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x14 = Header->_unknown0x14;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x1C = Header->_unknown0x1C;
            return Header->_count > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (int) Header->_headerSize;
            foreach (ResourceNode n in Children.Where(n => n.HasChildren))
            {
                size += n.OnCalculateSize(true);
                foreach (ResourceNode c in n.Children)
                {
                    size += c.OnCalculateSize(true);
                }
            }

            return size;
        }

        protected override string GetName()
        {
            return base.GetName("Pathing Data");
        }

        internal uint curDataOffset;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            curDataOffset = (uint) (Header->_headerSize + Children.Count * PathingMiscDataEntry.Size);
            uint headerSize = Header->_headerSize;
            PathingMiscData* header = (PathingMiscData*) address;
            *header = new PathingMiscData();
            header->_count = (uint) Children.Count;
            header->_headerSize = headerSize;
            header->_unknown0x08 = _unknown0x08;
            header->_unknown0x0C = _unknown0x0C;
            header->_unknown0x10 = _unknown0x10;
            header->_unknown0x14 = _unknown0x14;
            header->_unknown0x18 = _unknown0x18;
            header->_unknown0x1C = _unknown0x1C;
            uint offset = headerSize;
            ResourceNode[] validChildren = Children.Where(n => n.HasChildren).ToArray();
            foreach (ResourceNode n in validChildren)
            {
                int size = n.CalculateSize(true);
                n.Rebuild(address + offset, size, true);
                offset += (uint) size;
            }

            foreach (ResourceNode n in validChildren)
            {
                foreach (ResourceNode c in n.Children)
                {
                    int size = c.CalculateSize(true);
                    c.Rebuild(address + offset, size, true);
                    offset += (uint)size;
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            PathingMiscData* p = (PathingMiscData*)source.Address;
            uint sizeCheck = PathingMiscData.HeaderSize + PathingMiscDataEntry.Size * p->_count;
            if (p->_headerSize == PathingMiscData.HeaderSize && source.Length > sizeCheck)
            {
                for (int i = 0; i < p->_count; i++)
                {
                    PathingMiscDataEntry* pe = (PathingMiscDataEntry*)(*p)[i];
                    sizeCheck += PathingMiscDataSubEntry.Size * pe->_count;
                    if (sizeCheck > source.Length)
                    {
                        return null;
                    }
                }

                return sizeCheck == source.Length ? new PathingMiscDataNode() : null;
            }

            return null;
        }
    }

    public unsafe class PathingMiscDataEntryNode : ResourceNode
    {
        internal PathingMiscDataEntry* Header => (PathingMiscDataEntry*) WorkingUncompressed.Address;
        public override int MaxNameLength => 0x20;

        public ushort _id;

        [Category("ID")]
        public ushort ID
        {
            get => _id;
            set
            {
                _id = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x08;

        public float Unknown0x08
        {
            get => _unknown0x08;
            set
            {
                _unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x0C;

        public float Unknown0x0C
        {
            get => _unknown0x0C;
            set
            {
                _unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x10;

        public float Unknown0x10
        {
            get => _unknown0x10;
            set
            {
                _unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x14;

        public float Unknown0x14
        {
            get => _unknown0x14;
            set
            {
                _unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x18;

        public float Unknown0x18
        {
            get => _unknown0x18;
            set
            {
                _unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        public float _unknown0x1C;

        public float Unknown0x1C
        {
            get => _unknown0x1C;
            set
            {
                _unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            PathingMiscData* parentData = (PathingMiscData*) Parent.WorkingUncompressed.Address;
            uint currentOffset = Header->_dataOffset;
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source = new DataSource(Parent.WorkingUncompressed.Address + currentOffset,
                    (int) PathingMiscDataSubEntry.Size);
                new PathingMiscDataSubEntryNode { _name = $"Entry [{i}]"}.Initialize(this, source);
                currentOffset += PathingMiscDataSubEntry.Size;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return (int) PathingMiscDataEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PathingMiscDataEntry* header = (PathingMiscDataEntry*) address;
            *header = new PathingMiscDataEntry();
            header->_count = (ushort)(Children?.Count ?? 0);
            header->_id = _id;
            header->_dataOffset = ((PathingMiscDataNode) Parent).curDataOffset;
            foreach (ResourceNode n in Children)
            {
                ((PathingMiscDataNode)Parent).curDataOffset += (uint)n.OnCalculateSize(true);
            }
            header->_unknown0x08 = _unknown0x08;
            header->_unknown0x0C = _unknown0x0C;
            header->_unknown0x10 = _unknown0x10;
            header->_unknown0x14 = _unknown0x14;
            header->_unknown0x18 = _unknown0x18;
            header->_unknown0x1C = _unknown0x1C;
            address.WriteUTF8String(Name, false, 0x20, 0x20);
        }

        public override bool OnInitialize()
        {
            _name = Header->Name;
            _id = Header->_id;
            _unknown0x08 = Header->_unknown0x08;
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x14 = Header->_unknown0x14;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x1C = Header->_unknown0x1C;
            return Header->_dataOffset != 0xFFFFFFFF && Header->_count > 0;
        }
    }

    public unsafe class PathingMiscDataSubEntryNode : ResourceNode
    {
        internal PathingMiscDataSubEntry* Header => (PathingMiscDataSubEntry*)WorkingUncompressed.Address;

        private Vector3 _value;

        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Value
        {
            get => _value;
            set
            {
                _value = value;
                SignalPropertyChange();
            }
        }
        public override int OnCalculateSize(bool force)
        {
            return (int)PathingMiscDataSubEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PathingMiscDataSubEntry* header = (PathingMiscDataSubEntry*)address;
            *header = new PathingMiscDataSubEntry();
            header->_x = Value._x;
            header->_y = Value._y;
            header->_z = Value._z;
        }

        public override bool OnInitialize()
        {
            _value = new Vector3(Header->_x, Header->_y, Header->_z);
            return false;
        }
    }
}
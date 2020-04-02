using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PathingMiscDataNode : ARCEntryNode
    {
        internal PathingMiscData* Header => (PathingMiscData*)WorkingUncompressed.Address;
        //public override ResourceType ResourceFileType => ResourceType.PathingMiscData;

        public override Type[] AllowedChildTypes => new[] { typeof(PathingMiscDataEntryNode) };

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
                DataSource source = new DataSource((*Header)[i], (int)PathingMiscDataEntry.Size);
                new PathingMiscDataEntryNode().Initialize(this, source);
                int length;
                if (i == Header->_count - 1)
                {
                    length = (int)(WorkingUncompressed.Length - ((PathingMiscDataEntry*) (*Header)[i])->_dataOffset);
                }
                else
                {
                    length = (int) (((PathingMiscDataEntry*)(*Header)[i + 1])->_dataOffset - ((PathingMiscDataEntry*)(*Header)[i])->_dataOffset);
                }
                DataSource data = new DataSource(Header->GetDataBlock(i), length);
                new RawDataNode {_name = "Data"}.Initialize(Children.Last(), data);
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
            int size = (int)Header->_headerSize;
            foreach (ResourceNode n in Children.Where(n => n.HasChildren))
            {
                size += (int)PathingMiscDataEntry.Size;
                size += n.Children[0].CalculateSize(force);
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
            curDataOffset = (uint)(Header->_headerSize + Children.Count * PathingMiscDataEntry.Size);
            uint headerSize = Header->_headerSize;
            PathingMiscData* header = (PathingMiscData*)address;
            *header = new PathingMiscData();
            header->_count = (uint)Children.Count;
            header->_headerSize = headerSize;
            header->_unknown0x08 = _unknown0x08;
            header->_unknown0x0C = _unknown0x0C;
            header->_unknown0x10 = _unknown0x10;
            header->_unknown0x14 = _unknown0x14;
            header->_unknown0x18 = _unknown0x18;
            header->_unknown0x1C = _unknown0x1C;
            uint offset = headerSize;
            IEnumerable<ResourceNode> validChildren = Children.Where(n => n.HasChildren);
            foreach(ResourceNode n in validChildren)
            {
                int size = n.CalculateSize(force);
                n.Rebuild(address + offset, size, force);
                offset += (uint)size;
            }

            foreach (ResourceNode n in validChildren)
            {
                int size = n.Children[0].CalculateSize(force);
                n.Children[0].Rebuild(address + offset, size, force);
                offset += (uint)size;
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((PathingMiscData*)source.Address)->_headerSize == 0x20 && ((PathingMiscData*)source.Address)->_count < 10000 ? new PathingMiscDataNode() : null;
        }
    }

    public unsafe class PathingMiscDataEntryNode : ResourceNode
    {
        internal PathingMiscDataEntry* Header => (PathingMiscDataEntry*)WorkingUncompressed.Address;
        public override int MaxNameLength => 0x20;

        public ushort _id1;
        public ushort _id2;

        [Category("ID")]
        public ushort ID1
        {
            get => _id1;
            set
            {
                _id1 = value;
                SignalPropertyChange();
            }
        }

        [Category("ID")]
        public ushort ID2
        {
            get => _id2;
            set
            {
                _id2 = value;
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

        public override int OnCalculateSize(bool force)
        {
            return (int)PathingMiscDataEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PathingMiscDataEntry* header = (PathingMiscDataEntry*)address;
            *header = new PathingMiscDataEntry();
            header->_id1 = header->_id1;
            header->_id2 = header->_id2;
            header->_dataOffset = ((PathingMiscDataNode) Parent).curDataOffset;
            ((PathingMiscDataNode) Parent).curDataOffset += (uint)Children[0].CalculateSize(force);
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
            _id1 = Header->_id1;
            _id2 = Header->_id2;
            _unknown0x08 = Header->_unknown0x08;
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x14 = Header->_unknown0x14;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x1C = Header->_unknown0x1C;
            return Header->_dataOffset != 0xFFFFFFFF;
        }
    }
}
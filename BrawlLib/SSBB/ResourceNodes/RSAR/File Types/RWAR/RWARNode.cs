using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWARNode : RSAREntryNode
    {
        internal RWAR* Header => (RWAR*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->Table->_entryCount > 0;
        }

        public override void OnPopulate()
        {
            RWARTableBlock* table = Header->Table;
            RWARDataBlock* d = Header->Data;

            for (int i = 0; i < table->_entryCount; i++)
            {
                new RWAVNode().Initialize(this, d->GetEntry(table->Entries[i].waveFileRef), 0);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = RWAR.Size + (12 + Children.Count * 12).Align(0x20) + RWARDataBlock.Size;
            foreach (RWAVNode n in Children)
            {
                size += n.WorkingUncompressed.Length;
            }

            return size.Align(0x20);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RWAR* header = (RWAR*) address;
            header->_header._version = 0x100;
            header->_header._tag = RWAR.Tag;
            header->_header.Endian = Endian.Big;
            header->_header._length = length;
            header->_header._firstOffset = 0x20;
            header->_header._numEntries = 2;
            header->_tableOffset = 0x20;

            RWARTableBlock* tabl = (RWARTableBlock*) (address + 0x20);
            tabl->_header._tag = RWARTableBlock.Tag;
            tabl->_header._length = (12 + Children.Count * 12).Align(0x20);
            tabl->_entryCount = (uint) Children.Count;

            header->_tableLength = tabl->_header._length;
            header->_dataOffset = 0x20 + header->_tableLength;

            RWARDataBlock* data = (RWARDataBlock*) (address + 0x20 + tabl->_header._length);
            data->_header._tag = RWARDataBlock.Tag;

            VoidPtr addr = (VoidPtr) data + 0x20;
            foreach (RWAVNode n in Children)
            {
                tabl->Entries[n.Index].waveFileRef = (uint) (addr - data);
                //Memory.Move(addr, n.WorkingSource.Address, (uint)n.WorkingSource.Length);
                n.MoveRaw(addr, n.WorkingUncompressed.Length);
                addr += tabl->Entries[n.Index].waveFileSize = (uint) n.WorkingUncompressed.Length;
            }

            data->_header._length = addr - data;
            header->_dataLength = data->_header._length;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((RWAR*) source.Address)->_header._tag == RWAR.Tag ? new RWARNode() : null;
        }
    }
}
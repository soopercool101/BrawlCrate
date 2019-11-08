using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDDataGroupNode : ResourceNode
    {
        internal RWSD_DATAHeader* Header => (RWSD_DATAHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RWSDDataGroup;

        public override bool OnInitialize()
        {
            _name = "Data";
            return Header->_list._numEntries > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0xC + Children.Count * 8;
            foreach (RSARFileEntryNode g in Children)
            {
                size += g.CalculateSize(true);
            }

            return size.Align(0x20);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RWSD_DATAHeader* header = (RWSD_DATAHeader*) address;
            header->_tag = RWSD_DATAHeader.Tag;
            header->_length = length;
            header->_list._numEntries = Children.Count;
            VoidPtr addr = address + 12 + Children.Count * 8;
            foreach (RWSDDataNode d in Children)
            {
                d._baseAddr = header->_list.Address;
                header->_list[d.Index] = addr - header->_list.Address;
                d.Rebuild(addr, d._calcSize, force);
                addr += d._calcSize;
            }
        }
    }

    public unsafe class RWSDSoundGroupNode : ResourceNode
    {
        internal WAVEHeader* Header => (WAVEHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RSARFileSoundGroup;

        public VoidPtr _audioAddr;

        public override bool OnInitialize()
        {
            _name = "Audio";

            return Header->_numEntries > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_numEntries; i++)
            {
                new WAVESoundNode().Initialize(this, Header->GetEntry(i), 0);
            }

            foreach (WAVESoundNode n in Children)
            {
                n.GetAudio();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0xC + Children.Count * 4;
            foreach (WAVESoundNode g in Children)
            {
                size += g.WorkingUncompressed.Length;
            }

            return size.Align(0x20);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            WAVEHeader* header = (WAVEHeader*) address;
            header->_tag = WAVEHeader.Tag;
            header->_numEntries = Children.Count;
            header->_length = length;
            buint* table = (buint*) header + 3;
            VoidPtr addr = table + Children.Count;
            VoidPtr baseAddr = _audioAddr;
            foreach (WAVESoundNode r in Children)
            {
                table[r.Index] = (uint) (addr - address);

                r.MoveRawUncompressed(addr, r.WorkingUncompressed.Length);

                WaveInfo* wave = (WaveInfo*) addr;
                wave->_dataLocation = (uint) (_audioAddr - baseAddr);

                Memory.Move(_audioAddr, r._streamBuffer.Address, (uint) r._streamBuffer.Length);

                _audioAddr += (uint) r._streamBuffer.Length;
                addr += r.WorkingUncompressed.Length;
            }
        }
    }
}
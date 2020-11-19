using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class REFFNode : NW4RArcEntryNode
    {
        internal REFF* Header => (REFF*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.REFF;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            REFF* header = Header;

            if (_name == null)
            {
                _name = header->IdString;
            }

            return header->Table->_entries > 0;
        }

        public override void OnPopulate()
        {
            REFTypeObjectTable* table = Header->Table;
            REFTypeObjectEntry* Entry = table->First;
            for (int i = 0; i < table->_entries; i++, Entry = Entry->Next)
            {
                new REFFEntryNode {_name = Entry->Name, _offset = Entry->DataOffset, _length = Entry->DataLength}
                    .Initialize(this, new DataSource((byte*) table->Address + Entry->DataOffset, Entry->DataLength));
            }
        }

        private int _tableLen;

        public override int OnCalculateSize(bool force)
        {
            int size = 0x28 + (Name.Length + 1).Align(4);
            _tableLen = 0x8;
            foreach (ResourceNode n in Children)
            {
                _tableLen += n.Name.Length + 11;
                size += n.CalculateSize(force);
            }

            return size + (_tableLen = _tableLen.Align(4));
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            REFF* header = (REFF*) address;
            header->_linkPrev = 0;
            header->_linkNext = 0;
            header->_padding = 0;
            header->_dataLength = length - 0x18;
            header->_header._tag = header->_tag = REFF.Tag;
            header->_header.Endian = Endian.Big;
            header->_header._version = 7;
            header->_header._length = length;
            header->_header._firstOffset = 0x10;
            header->_header._numEntries = 1;
            header->IdString = Name;

            REFTypeObjectTable* table = (REFTypeObjectTable*) ((byte*) header + header->_dataOffset + 0x18);
            table->_entries = (ushort) Children.Count;
            table->_pad = 0;
            table->_length = _tableLen;

            REFTypeObjectEntry* entry = table->First;
            int offset = _tableLen;
            foreach (ResourceNode n in Children)
            {
                entry->Name = n.Name;
                entry->DataOffset = offset;
                entry->DataLength = n._calcSize;
                n.Rebuild((VoidPtr) table + offset, n._calcSize, force);
                offset += n._calcSize;
                entry = entry->Next;
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((REFF*) source.Address)->_tag == REFF.Tag ? new REFFNode() : null;
        }
    }

    public unsafe class REFFEntryNode : ResourceNode
    {
        internal REFFDataHeader* Header => (REFFDataHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.REFFEntry;
        [Category("REFF Entry")] public int REFFOffset => _offset;
        [Category("REFF Entry")] public int DataLength => _length;

        public int _offset;
        public int _length;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return true;
        }

        public override void OnPopulate()
        {
            switch (((REFFNode) Parent).VersionMinor)
            {
                case 7:
                    new REFFEmitterNode7().Initialize(this, (VoidPtr) Header + 8, (int) Header->_headerSize);
                    break;
                case 9:
                case 11: //Uuuuh...
                    new REFFEmitterNode9().Initialize(this, (VoidPtr) Header + 8, (int) Header->_headerSize);
                    break;
            }

            new REFFParticleNode().Initialize(this, Header->Params, (int) Header->Params->headersize);
            new REFFAnimationListNode
                {
                    _ptclTrackCount = *Header->PtclTrackCount,
                    _ptclInitTrackCount = *Header->PtclInitTrackCount,
                    _emitTrackCount = *Header->EmitTrackCount,
                    _emitInitTrackCount = *Header->EmitInitTrackCount,
                    _ptclTrackAddr = Header->PtclTrack,
                    _emitTrackAddr = Header->EmitTrack
                }
                .Initialize(this, Header->Animations,
                    WorkingUncompressed.Length - ((int) Header->Animations - (int) Header));
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 8;
            foreach (ResourceNode r in Children)
            {
                size += r.CalculateSize(true);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            REFFDataHeader* d = (REFFDataHeader*) address;
            d->_headerSize = (uint) Children[0]._calcSize;
            Children[0].Rebuild(d->_descriptor.Address, Children[0]._calcSize, true);
            Children[1].Rebuild(d->Params, Children[1]._calcSize, true);
            Children[2].Rebuild(d->PtclTrackCount, Children[2]._calcSize, true);
        }
    }
}
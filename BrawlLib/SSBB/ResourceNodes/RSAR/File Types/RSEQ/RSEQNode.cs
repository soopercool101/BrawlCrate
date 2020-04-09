using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQNode : RSARFileNode
    {
        internal RSEQHeader* Header => (RSEQHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RSEQ;

        public MMLCommand[] _cmds;
        public MMLCommand[] Commands => _cmds;

        private UnsafeBuffer _dataBuffer;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _dataBuffer = new UnsafeBuffer(Header->_dataLength);
            Memory.Move(_dataBuffer.Address, Header->Data, (uint) Header->_dataLength);
            SetSizeInternal(Header->_dataLength + Header->_header._firstOffset);

            _cmds = MMLParser.Parse(Header->Data + 12);

            return true;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->Labl->_numEntries; i++)
            {
                new RSEQLabelNode().Initialize(this, Header->Labl->Get(i), 0);
            }
        }

        protected override void GetStrings(LabelBuilder builder)
        {
            foreach (RSEQLabelNode node in Children)
            {
                builder.Add(node.Id, node._name);
            }
        }

        private LabelBuilder builder;

        public override int OnCalculateSize(bool force)
        {
            builder = new LabelBuilder();
            foreach (RSEQLabelNode node in Children)
            {
                builder.Add(node.Id, node._name);
            }

            _audioLen = 0;
            return _headerLen = 0x20 + _dataBuffer.Length + builder.GetSize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RSEQHeader* header = (RSEQHeader*) address;
            header->_header.Endian = Endian.Big;
            header->_header._tag = RSEQHeader.Tag;
            header->_header._version = 0x100;
            header->_header._length = length;
            header->_header._numEntries = 2;
            header->_header._firstOffset = 0x20;
            header->_dataOffset = 0x20;
            header->_dataLength = _dataBuffer.Length;
            header->_lablOffset = 0x20 + _dataBuffer.Length;
            header->_lablLength = builder.GetSize();

            //MML Parser is not complete yet, so copy raw data over
            Memory.Move((VoidPtr) header + header->_dataOffset, _dataBuffer.Address, (uint) _dataBuffer.Length);

            builder.Write((VoidPtr) header + header->_lablOffset);
        }

        public override void Remove()
        {
            RSARNode?.Files.Remove(this);

            base.Remove();
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((RSEQHeader*) source.Address)->_header._tag == RSEQHeader.Tag ? new RSEQNode() : null;
        }
    }
}
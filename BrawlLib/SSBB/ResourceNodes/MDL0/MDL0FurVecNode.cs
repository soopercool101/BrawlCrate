using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0FurVecNode : MDL0EntryNode
    {
        internal MDL0FurVecData* Header => (MDL0FurVecData*) WorkingUncompressed.Address;

        public MDL0ObjectNode[] Objects => _objects.ToArray();
        internal List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();
        private MDL0FurVecData hdr;

        [Category("Fur Vector Data")] public int TotalLen => hdr._dataLen;
        [Category("Fur Vector Data")] public int MDL0Offset => hdr._mdl0Offset;
        [Category("Fur Vector Data")] public int DataOffset => hdr._dataOffset;
        [Category("Fur Vector Data")] public int StringOffset => hdr._stringOffset;
        [Category("Fur Vector Data")] public int ID => hdr._index;
        [Category("Fur Vector Data")] public ushort NumEntries => hdr._numEntries;

        public Vector3[] _entries;

        public Vector3[] Vertices
        {
            get => _entries;
            set => _entries = value;
        }

        public override bool OnInitialize()
        {
            hdr = *Header;
            base.OnInitialize();

            SetSizeInternal(hdr._dataLen);

            if (_name == null && Header->_stringOffset != 0)
            {
                _name = Header->ResourceString;
            }

            _entries = new Vector3[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                _entries[i] = Header->Entries[i];
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        public override void Export(string outPath)
        {
            base.Export(outPath);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0FurVecData* header = (MDL0FurVecData*) dataAddress;
            header->_mdl0Offset = (int) mdlAddress - (int) dataAddress;
            header->_stringOffset = (int) stringTable[Name] + 4 - (int) dataAddress;
            header->_index = Index;
        }
    }
}
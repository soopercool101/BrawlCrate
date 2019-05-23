using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0FurVecNode : MDL0EntryNode
    {
        internal MDL0FurVecData* Header { get { return (MDL0FurVecData*)WorkingUncompressed.Address; } }
        
        public MDL0ObjectNode[] Objects { get { return _objects.ToArray(); } }
        internal List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();

        MDL0FurVecData hdr = new MDL0FurVecData();

        [Category("Fur Vector Data")]
        public int TotalLen { get { return hdr._dataLen; } }
        [Category("Fur Vector Data")]
        public int MDL0Offset { get { return hdr._mdl0Offset; } }
        [Category("Fur Vector Data")]
        public int DataOffset { get { return hdr._dataOffset; } }
        [Category("Fur Vector Data")]
        public int StringOffset { get { return hdr._stringOffset; } }
        [Category("Fur Vector Data")]
        public int ID { get { return hdr._index; } }
        [Category("Fur Vector Data")]
        public ushort NumEntries { get { return hdr._numEntries; } }

        public Vector3[] _entries;
        public Vector3[] Vertices
        {
            get { return _entries; }
            set { _entries = value; }
        }

        public override bool OnInitialize()
        {
            hdr = *Header;
            base.OnInitialize();

            SetSizeInternal(hdr._dataLen);

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _entries = new Vector3[NumEntries];
            for (int i = 0; i < NumEntries; i++)
                _entries[i] = Header->Entries[i];

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

        public override unsafe void Export(string outPath)
        {
            base.Export(outPath);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0FurVecData* header = (MDL0FurVecData*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
            header->_index = Index;
        }
    }
}

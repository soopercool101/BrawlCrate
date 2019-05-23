using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Models;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0NormalNode : MDL0EntryNode
    {
        internal MDL0NormalData* Header { get { return (MDL0NormalData*)WorkingUncompressed.Address; } }
        //protected override int DataLength { get { return Header->_dataLen; } }

        public MDL0ObjectNode[] Objects { get { return _objects.ToArray(); } }
        public List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();

        MDL0NormalData _hdr = new MDL0NormalData() { _type = (int)WiiVertexComponentType.Float };

        public enum NormalType
        {
            XYZ = 0,
            NBT = 1, // one index per NBT
            NBT3 = 2, // one index per each of N/B/T
        }

        [Category("Normal Data")]
        public int ID { get { return _hdr._index; } }
        [Category("Normal Data")]
        public NormalType Type { get { return (NormalType)(int)_hdr._isNBT; } }
        [Category("Normal Data")]
        public WiiVertexComponentType Format { get { return (WiiVertexComponentType)(int)_hdr._type; } }
        [Category("Normal Data")]
        public int Divisor { get { return _hdr._divisor; } }
        [Category("Normal Data")]
        public int EntryStride { get { return _hdr._entryStride; } }
        [Category("Normal Data")]
        public int NumEntries { get { return _hdr._numVertices; } }

        public bool ForceRebuild { get { return _forceRebuild; } set { if (_forceRebuild != value) { _forceRebuild = value; SignalPropertyChange(); } } }
        public bool ForceFloat { get { return _forceFloat; } set { if (_forceFloat != value) { _forceFloat = value; } } }

        private Vector3[] _normals;
        public Vector3[] Normals
        {
            get { return _normals == null ? _normals = VertexCodec.ExtractNormals(Header) : _normals; }
            set
            {
                _normals = value;

                _forceRebuild = true;
                if (Format == WiiVertexComponentType.Float)
                    _forceFloat = true;

                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            //Clear the coordinates so they're reparsed,
            //just in case the node has been replaced.
            _normals = null;

            _hdr = *Header;

            //SetSizeInternal(_hdr._dataLen);

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        public VertexCodec _enc;
        public bool _forceRebuild = false;
        public bool _forceFloat = false;
        public override int OnCalculateSize(bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                _enc = new VertexCodec(Normals, false, _forceFloat);
                return _enc._dataLen.Align(0x20) + 0x20;
            }
            else return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                MDL0NormalData* header = (MDL0NormalData*)address;

                header->_dataLen = length;
                header->_dataOffset = 0x20;
                header->_index = _entryIndex;
                header->_isNBT = 0;
                header->_type = (int)_enc._type;
                header->_divisor = (byte)_enc._scale;
                header->_entryStride = (byte)_enc._dstStride;
                header->_numVertices = (ushort)_enc._srcCount;

                _enc.Write(Normals, (byte*)header + 0x20);
                _enc.Dispose();
                _enc = null;

                _forceRebuild = false;
            }
            else
                base.OnRebuild(address, length, force);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0NormalData* header = (MDL0NormalData*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
            header->_index = Index;
        }
    }
}

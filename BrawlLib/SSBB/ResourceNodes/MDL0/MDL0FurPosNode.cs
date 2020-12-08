using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0FurPosNode : MDL0EntryNode
    {
        internal MDL0FurPosData* Header => (MDL0FurPosData*) WorkingUncompressed.Address;

        public MDL0ObjectNode[] Objects => _objects.ToArray();
        internal List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();
        private MDL0FurPosData hdr;

        [Category("Fur Layer Vertex Data")] public int TotalLen => hdr._dataLen;
        [Category("Fur Layer Vertex Data")] public int MDL0Offset => hdr._mdl0Offset;
        [Category("Fur Layer Vertex Data")] public int DataOffset => hdr._dataOffset;
        [Category("Fur Layer Vertex Data")] public int StringOffset => hdr._stringOffset;

        [Category("Fur Layer Vertex Data")] public int ID => hdr._index;
        [Category("Fur Layer Vertex Data")] public bool IsXYZ => hdr._isXYZ != 0;

        [Category("Fur Layer Vertex Data")]
        public WiiVertexComponentType Format => (WiiVertexComponentType) (int) hdr._type;

        [Category("Fur Layer Vertex Data")] public byte Divisor => hdr._divisor;
        [Category("Fur Layer Vertex Data")] public byte EntryStride => hdr._entryStride;
        [Category("Fur Layer Vertex Data")] public short NumVertices => hdr._numVertices;

        [Category("Fur Layer Vertex Data")] public int NumLayers => hdr._numLayers;
        [Category("Fur Layer Vertex Data")] public int LayerOffset => hdr._offsetOfLayer;

        public bool ForceRebuild
        {
            get => _forceRebuild;
            set
            {
                if (_forceRebuild != value)
                {
                    _forceRebuild = value;
                    SignalPropertyChange();
                }
            }
        }

        public bool ForceFloat
        {
            get => _forceFloat;
            set
            {
                if (_forceFloat != value)
                {
                    _forceFloat = value;
                }
            }
        }

        public Vector3[] _vertices;

        public Vector3[] Vertices
        {
            get => _vertices ?? (_vertices = VertexCodec.ExtractVertices(Header));
            set
            {
                _vertices = value;
                SignalPropertyChange();
            }
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

            return false;
        }

        public VertexCodec _enc;
        public bool _forceRebuild;
        public bool _forceFloat;

        public override int OnCalculateSize(bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                _enc = new VertexCodec(Vertices, false, _forceFloat);
                return _enc._dataLen.Align(0x20) + 0x40;
            }

            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                MDL0FurPosData* header = (MDL0FurPosData*) address;

                header->_dataLen = length;
                header->_dataOffset = 0x40;
                header->_index = _entryIndex;
                header->_isXYZ = _enc._hasZ ? 1 : 0;
                header->_type = (int) _enc._type;
                header->_divisor = (byte) _enc._scale;
                header->_entryStride = (byte) _enc._dstStride;
                header->_numVertices = (short) _enc._srcCount;

                header->_numLayers = NumLayers;
                header->_offsetOfLayer = LayerOffset;

                //Write data
                _enc.Write(Vertices, (byte*) address + 0x40);
                _enc.Dispose();
                _enc = null;

                _forceRebuild = false;
            }
            else
            {
                base.OnRebuild(address, length, force);
            }
        }

        public override void Export(string outPath)
        {
            base.Export(outPath);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0FurPosData* header = (MDL0FurPosData*) dataAddress;
            header->_mdl0Offset = (int) mdlAddress - (int) dataAddress;
            header->_stringOffset = (int) stringTable[Name] + 4 - (int) dataAddress;
            header->_index = Index;
        }
    }
}
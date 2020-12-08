using BrawlLib.Internal;
using BrawlLib.Modeling;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0VertexNode : MDL0EntryNode
    {
        internal MDL0VertexData* Header => (MDL0VertexData*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDL0Vertex;
        public MDL0ObjectNode[] Objects => _objects.ToArray();
        public List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();
        private MDL0VertexData _hdr = new MDL0VertexData {_type = (int) WiiVertexComponentType.Float};

        [Category("Vertex Data")] public int ID => _hdr._index;
        [Category("Vertex Data")] public bool IsXYZ => _hdr._isXYZ != 0;
        [Category("Vertex Data")] public WiiVertexComponentType Format => (WiiVertexComponentType) (int) _hdr._type;
        [Category("Vertex Data")] public byte Divisor => _hdr._divisor;
        [Category("Vertex Data")] public byte EntryStride => _hdr._entryStride;
        [Category("Vertex Data")] public ushort NumVertices => _hdr._numVertices;
        [Category("Vertex Data")] public Vector3 EMin => _hdr._eMin;
        [Category("Vertex Data")] public Vector3 EMax => _hdr._eMax;

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

                _forceRebuild = true;
                if (Format == WiiVertexComponentType.Float)
                {
                    _forceFloat = true;
                }

                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            //Clear the coordinates so they're reparsed,
            //just in case the node has been replaced.
            _vertices = null;

            _hdr = *Header;

            //SetSizeInternal(_hdr._dataLen);

            if (_name == null && Header->_stringOffset != 0)
            {
                _name = Header->ResourceString;
            }

            return false;
        }

        public VertexCodec _enc;
        private bool _forceRebuild;
        private bool _forceFloat;

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
                MDL0VertexData* header = (MDL0VertexData*) address;

                header->_dataLen = length;
                header->_dataOffset = 0x40;
                header->_index = _entryIndex;
                header->_isXYZ = _enc._hasZ ? 1 : 0;
                header->_type = (int) _enc._type;
                header->_divisor = (byte) _enc._scale;
                header->_entryStride = (byte) _enc._dstStride;
                header->_numVertices = (ushort) _enc._srcCount;
                header->_eMin = _enc._min;
                header->_eMax = _enc._max;
                header->_pad1 = header->_pad2 = 0;

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
            if (outPath.EndsWith(".obj"))
            {
                Wavefront.Serialize(outPath, this);
            }
            else
            {
                base.Export(outPath);
            }
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0VertexData* header = (MDL0VertexData*) dataAddress;
            header->_mdl0Offset = (int) mdlAddress - (int) dataAddress;
            header->_stringOffset = (int) stringTable[Name] + 4 - (int) dataAddress;
            header->_index = Index;
        }
    }
}
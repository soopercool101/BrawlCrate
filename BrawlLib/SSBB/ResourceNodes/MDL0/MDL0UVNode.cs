using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0UVNode : MDL0EntryNode
    {
        internal MDL0UVData* Header => (MDL0UVData*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDL0UV;
        public MDL0ObjectNode[] Objects => _objects.ToArray();
        public List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();
        private MDL0UVData _hdr = new MDL0UVData {_format = (int) WiiVertexComponentType.Float};

        [Category("UV Data")] public int ID => _hdr._index;
        [Category("UV Data")] public bool IsST => _hdr._isST != 0;
        [Category("UV Data")] public WiiVertexComponentType Format => (WiiVertexComponentType) (int) _hdr._format;
        [Category("UV Data")] public byte Divisor => _hdr._divisor;
        [Category("UV Data")] public byte EntryStride => _hdr._entryStride;
        [Category("UV Data")] public int NumEntries => _hdr._numEntries;

        [Category("UV Data")] public Vector2 Min => _hdr._min;
        [Category("UV Data")] public Vector2 Max => _hdr._max;

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

        private Vector2[] _points;

        public Vector2[] Points
        {
            get => _points ?? (_points = VertexCodec.ExtractUVs(Header));
            set
            {
                _points = value;

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
            _points = null;

            _hdr = *Header;

            //SetSizeInternal(_hdr._dataLen);

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
                _enc = new VertexCodec(Points, _forceFloat);
                return _enc._dataLen.Align(0x20) + 0x40;
            }

            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Model._isImport || _forceRebuild)
            {
                MDL0UVData* header = (MDL0UVData*) address;

                header->_dataLen = length;
                header->_dataOffset = 0x40;
                header->_index = _entryIndex;
                header->_format = (int) _enc._type;
                header->_divisor = (byte) _enc._scale;
                header->_isST = 1;
                header->_entryStride = (byte) _enc._dstStride;
                header->_numEntries = (ushort) _enc._srcCount;
                header->_min = (Vector2) _enc._min;
                header->_max = (Vector2) _enc._max;
                header->_pad1 = header->_pad2 = header->_pad3 = header->_pad4 = 0;

                _enc.Write(Points, (byte*) address + 0x40);
                _enc.Dispose();
                _enc = null;

                _forceRebuild = false;
            }
            else
            {
                base.OnRebuild(address, length, force);
            }
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0UVData* header = (MDL0UVData*) dataAddress;
            header->_mdl0Offset = (int) mdlAddress - (int) dataAddress;
            header->_stringOffset = (int) stringTable[Name] + 4 - (int) dataAddress;
            header->_index = Index;
        }
    }
}
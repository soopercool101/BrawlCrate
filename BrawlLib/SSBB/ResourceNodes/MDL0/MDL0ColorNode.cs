using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ColorNode : MDL0EntryNode, IColorSource
    {
        public override ResourceType ResourceFileType => ResourceType.MDL0Color;

        internal MDL0ColorData* Header => (MDL0ColorData*) WorkingUncompressed.Address;
        public MDL0ObjectNode[] Objects => _objects.ToArray();
        public List<MDL0ObjectNode> _objects = new List<MDL0ObjectNode>();
        private MDL0ColorData _hdr;

        [Category("Color Data")] public int ID => _hdr._index;
        [Category("Color Data")] public bool IsRGBA => _hdr._isRGBA != 0;
        [Category("Color Data")] public WiiColorComponentType Format => (WiiColorComponentType) (int) _hdr._format;
        [Category("Color Data")] public byte EntryStride => _hdr._entryStride;
        [Category("Color Data")] public int NumEntries => _hdr._numEntries;

        private RGBAPixel[] _colors;

        [Browsable(false)]
        public RGBAPixel[] Colors
        {
            get => _colors == null && Header != null
                ? _colors = ColorCodec.ExtractColors(Header).Select(n => (RGBAPixel) n).ToArray()
                : _colors;
            set
            {
                _colors = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            //Clear the colors so they're reparsed,
            //just in case the node has been replaced.
            _colors = null;

            _hdr = *Header;

            //SetSizeInternal(_hdr._dataLen);

            if (_name == null && Header->_stringOffset != 0)
            {
                _name = Header->ResourceString;
            }

            return false;
        }

        public ColorCodec _enc;

        public override int OnCalculateSize(bool force)
        {
            if (Model._isImport || _changed)
            {
                _enc = new ColorCodec(Colors);
                return _enc._dataLen.Align(0x20) + 0x20;
            }

            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (Model._isImport || _changed || Header == null)
            {
                //Write header
                MDL0ColorData* header = (MDL0ColorData*) address;
                header->_dataLen = length;
                header->_dataOffset = 0x20;
                header->_index = _entryIndex;
                header->_isRGBA = _enc._hasAlpha ? 1 : 0;
                header->_format = (int) _enc._outType;
                header->_entryStride = (byte) _enc._dstStride;
                header->_pad = 0;
                header->_numEntries = (ushort) Colors.Length;

                //Write data
                _enc.Write((byte*) header + 0x20);
                _enc.Dispose();
                _enc = null;
            }
            else
            {
                base.OnRebuild(address, length, force);
            }
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0ColorData* header = (MDL0ColorData*) dataAddress;
            header->_mdl0Offset = (int) mdlAddress - (int) dataAddress;
            header->_stringOffset = (int) stringTable[Name] + 4 - (int) dataAddress;
            header->_index = Index;
        }

        #region IColorSource Interface

        public bool HasPrimary(int id)
        {
            return false;
        }

        public ARGBPixel GetPrimaryColor(int id)
        {
            return new ARGBPixel();
        }

        public void SetPrimaryColor(int id, ARGBPixel color)
        {
        }

        public string PrimaryColorName(int id)
        {
            return null;
        }

        [Browsable(false)] public int TypeCount => 1;

        public int ColorCount(int id)
        {
            return Colors.Length;
        }

        public ARGBPixel GetColor(int index, int id)
        {
            if (index >= 0 && index < Colors.Length)
            {
                return Colors[index];
            }

            return new ARGBPixel();
        }

        public void SetColor(int index, int id, ARGBPixel color)
        {
            if (index >= 0 && index < Colors.Length)
            {
                Colors[index] = color;
                SignalPropertyChange();
            }
        }

        public bool GetColorConstant(int id)
        {
            return false;
        }

        public void SetColorConstant(int id, bool constant)
        {
        }

        #endregion
    }
}
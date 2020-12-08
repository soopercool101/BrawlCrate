using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Drawing;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Textures;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class REFTNode : NW4RArcEntryNode, IImageSource
    {
        internal REFT* Header => (REFT*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.REFT;

        [Browsable(false)] public int ImageCount => Children.Count(o => o is IImageSource i && i.ImageCount > 0);

        public Bitmap GetImage(int index)
        {
            return ((IImageSource) Children.Where(o => o is IImageSource i && i.ImageCount > 0).ToArray()[index])
                .GetImage(0);
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            REFT* header = Header;

            if (_name == null)
            {
                _name = header->IdString;
            }

            return header->Table->_entries > 0;
        }

        private int _tableLen;

        public override int OnCalculateSize(bool force)
        {
            int size = 0x60;
            _tableLen = 0x8;
            foreach (ResourceNode n in Children)
            {
                _tableLen += n.Name.Length + 11;
                size += n.CalculateSize(force);
            }

            return size + (_tableLen = _tableLen.Align(0x20));
        }

        public override void OnPopulate()
        {
            REFTypeObjectTable* table = Header->Table;
            REFTypeObjectEntry* Entry = table->First;
            for (int i = 0; i < table->_entries; i++, Entry = Entry->Next)
            {
                new REFTEntryNode {_name = Entry->Name, _offset = Entry->DataOffset, _length = Entry->DataLength + 0x20}
                    .Initialize(this,
                        new DataSource((byte*) table->Address + Entry->DataOffset, Entry->DataLength + 0x20));
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            REFT* header = (REFT*) address;
            header->_linkPrev = 0;
            header->_linkNext = 0;
            header->_padding = 0;
            header->_dataLength = length - 0x18;
            header->_dataOffset = 0x48;
            header->_header._tag = header->_tag = REFT.Tag;
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
                entry->DataLength = n._calcSize - 0x20;
                n.Rebuild((VoidPtr) table + offset, n._calcSize, force);
                offset += n._calcSize;
                entry = entry->Next;
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((REFT*) source.Address)->_tag == REFT.Tag ? new REFTNode() : null;
        }
    }

    public unsafe class REFTEntryNode : ResourceNode, IImageSource, IColorSource
    {
        internal REFTImageHeader* Header => (REFTImageHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.REFTImage;

        public int _offset;
        public int _length;

        private WiiPixelFormat _format;
        private WiiPaletteFormat _pltFormat;
        private int numColors, _imgLen, _pltLen;
        private int _width, _height;
        private uint _unk;
        private int _lod;
        private float _lodBias;
        internal uint _minFltr;
        internal uint _magFltr;

        [Browsable(false)] public bool HasPlt => Header->_colorCount > 0;

        //[Category("REFT Image")]
        //public uint Unknown { get { return _unk; } }
        [Category("REFT Image")] public WiiPixelFormat TextureFormat => _format;
        [Category("REFT Image")] public WiiPaletteFormat PaletteFormat => _pltFormat;
        [Category("REFT Image")] public int Colors => numColors;
        [Category("REFT Image")] public int Width => _width;
        [Category("REFT Image")] public int Height => _height;
        [Category("REFT Image")] public int ImageLength => _imgLen;
        [Category("REFT Image")] public int PaletteLength => _pltLen;
        [Category("REFT Image")] public int LevelOfDetail => _lod;

        [Category("REFT Image")]
        public MatTextureMinFilter MinFilter
        {
            get => (MatTextureMinFilter) _minFltr;
            set
            {
                _minFltr = (uint) value;
                SignalPropertyChange();
            }
        }

        [Category("REFT Image")]
        public MatTextureMagFilter MagFilter
        {
            get => (MatTextureMagFilter) _magFltr;
            set
            {
                _magFltr = (uint) value;
                SignalPropertyChange();
            }
        }

        [Category("REFT Image")]
        public float LODBias
        {
            get => _lodBias;
            set
            {
                _lodBias = value;
                SignalPropertyChange();
            }
        }

        [Category("REFT Entry")] public int REFTOffset => _offset;
        [Category("REFT Entry")] public int DataLength => _length;

        [Browsable(false)] public int ImageCount => LevelOfDetail;

        public Bitmap GetImage(int index)
        {
            try
            {
                if (HasPlt)
                {
                    return TextureConverter.DecodeIndexed((byte*) Header + 0x20, Width, Height, Palette, index + 1,
                        _format);
                }

                return TextureConverter.Decode((byte*) Header + 0x20, Width, Height, index + 1, _format);
            }
            catch
            {
                return null;
            }
        }

        private ColorPalette _palette;

        [Browsable(false)]
        public ColorPalette Palette
        {
            get => HasPlt
                ? _palette ?? (_palette =
                    TextureConverter.DecodePalette((byte*) Header + 0x20 + Header->_imagelen, Colors, _pltFormat))
                : null;
            set
            {
                _palette = value;
                SignalPropertyChange();
            }
        }

        #region IColorSource Members

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

        [Browsable(false)]
        public string PrimaryColorName(int id)
        {
            return null;
        }

        [Browsable(false)] public int TypeCount => 1;

        [Browsable(false)]
        public int ColorCount(int id)
        {
            return Palette != null ? Palette.Entries.Length : 0;
        }

        public ARGBPixel GetColor(int index, int id)
        {
            return Palette != null ? (ARGBPixel) Palette.Entries[index] : new ARGBPixel();
        }

        public void SetColor(int index, int id, ARGBPixel color)
        {
            if (Palette != null)
            {
                Palette.Entries[index] = (Color) color;
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

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _unk = Header->_unknown;
            _format = (WiiPixelFormat) Header->_format;
            _pltFormat = (WiiPaletteFormat) Header->_pltFormat;
            numColors = Header->_colorCount;
            _imgLen = (int) Header->_imagelen;
            _width = Header->_width;
            _height = Header->_height;
            _pltLen = (int) Header->_pltSize;
            _lod = Header->_mipmap + 1;
            _minFltr = Header->_min_filt;
            _magFltr = Header->_mag_filt;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            REFTImageHeader* header = (REFTImageHeader*) address;
            header->Set((byte) _minFltr, (byte) _magFltr, (byte) _lodBias);
        }

        public void Replace(Bitmap bmp)
        {
            FileMap tMap;
            if (HasPlt)
            {
                tMap = TextureConverter.Get(_format).EncodeREFTTextureIndexed(bmp, LevelOfDetail,
                    Palette.Entries.Length, PaletteFormat, QuantizationAlgorithm.MedianCut);
            }
            else
            {
                tMap = TextureConverter.Get(_format).EncodeREFTTexture(bmp, LevelOfDetail, WiiPaletteFormat.IA8);
            }

            ReplaceRaw(tMap);
        }

        public override void Replace(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            Bitmap bmp;
            if (string.Equals(ext, ".tga", StringComparison.OrdinalIgnoreCase))
            {
                bmp = TGA.FromFile(fileName);
            }
            else if (
                string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, ".tif", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, ".tiff", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, ".bmp", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, ".gif", StringComparison.OrdinalIgnoreCase))
            {
                bmp = (Bitmap) Image.FromFile(fileName);
            }
            else
            {
                base.Replace(fileName);
                return;
            }

            using (Bitmap b = bmp)
            {
                Replace(b);
            }
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".png"))
            {
                using (Bitmap bmp = GetImage(0))
                {
                    bmp.Save(outPath, ImageFormat.Png);
                }
            }
            else if (outPath.EndsWith(".tga"))
            {
                using (Bitmap bmp = GetImage(0))
                {
                    bmp.SaveTGA(outPath);
                }
            }
            else if (outPath.EndsWith(".tiff") || outPath.EndsWith(".tif"))
            {
                using (Bitmap bmp = GetImage(0))
                {
                    bmp.Save(outPath, ImageFormat.Tiff);
                }
            }
            else if (outPath.EndsWith(".bmp"))
            {
                using (Bitmap bmp = GetImage(0))
                {
                    bmp.Save(outPath, ImageFormat.Bmp);
                }
            }
            else if (outPath.EndsWith(".jpg") || outPath.EndsWith(".jpeg"))
            {
                using (Bitmap bmp = GetImage(0))
                {
                    bmp.Save(outPath, ImageFormat.Jpeg);
                }
            }
            else if (outPath.EndsWith(".gif"))
            {
                using (Bitmap bmp = GetImage(0))
                {
                    bmp.Save(outPath, ImageFormat.Gif);
                }
            }
            else
            {
                base.Export(outPath);
            }
        }
    }
}
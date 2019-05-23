using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using System.Drawing;
using BrawlLib.IO;
using System.Drawing.Imaging;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TPLNode : ARCEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.TPL; } }
        internal TPLHeader* Header { get { return (TPLHeader*)WorkingUncompressed.Address; } }

        public override string Name
        {
            get
            {
                return _name == null ? String.Format("TPL{0}", Index) : _name;
            }
            set
            {
                base.Name = value;
            }
        }

        public override bool OnInitialize()
        {
            if (_origPath != null && _name == null)
                _name = Path.GetFileNameWithoutExtension(_origPath);

            return Header->_numEntries > 0;
        }

        public override void OnPopulate()
        {
            VoidPtr p;
            for (int i = 0; i < Header->_numEntries; i++)
            {
                if ((p = Header->GetTextureEntry(i)) != null)
                {
                    TPLTextureNode t = new TPLTextureNode() { _dataAddr = (VoidPtr)Header + ((TPLTextureHeader*)p)->_data };
                    t.Initialize(this, p, 0);

                    if ((p = Header->GetPaletteEntry(i)) != null)
                        new TPLPaletteNode() { _dataAddr = (VoidPtr)Header + ((TPLPaletteHeader*)p)->_data }.Initialize(t, p, 0);
                }
            }
        }

        int size = 0, texHdrLen = 0, pltHdrLen = 0, texLen = 0, pltLen = 0;
        public override int OnCalculateSize(bool force)
        {
            texHdrLen = 0;
            pltHdrLen = 0;
            texLen = 0;
            pltLen = 0;

            size = TPLHeader.Size + Children.Count * 8;
            foreach (TPLTextureNode t in Children)
            {
                texHdrLen += TPLTextureHeader.Size;
                texLen += t.CalculateSize(true);
                
                if (t.Children.Count > 0)
                {
                    pltHdrLen += TPLPaletteHeader.Size;
                    pltLen += t.Children[0].CalculateSize(true);
                }
            }

            pltHdrLen = (pltHdrLen + size).Align(0x20) - size;
            pltLen = pltLen.Align(0x20);
            texHdrLen = texHdrLen.Align(0x20);
            texLen = texLen.Align(0x20);

            return size + pltHdrLen + pltLen + texHdrLen + texLen;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TPLHeader* header = (TPLHeader*)address;
            header->_tag = TPLHeader.Tag;
            header->_numEntries = (uint)Children.Count;
            header->_dataOffset = 0xC;

            buint* values = (buint*)address + 3;

            VoidPtr pltHdrs = address + size;
            VoidPtr pltData = pltHdrs + pltHdrLen;
            VoidPtr texHdrs = pltData + pltLen;
            VoidPtr texData = texHdrs + texHdrLen;

            foreach (TPLTextureNode t in Children)
            {
                //Rebuild palette
                if (t.Children.Count > 0)
                {
                    TPLPaletteNode p = t.Children[0] as TPLPaletteNode;

                    values[1] = (uint)(pltHdrs - address);
                    TPLPaletteHeader* plt = (TPLPaletteHeader*)pltHdrs;
                    plt->_numEntries = (ushort)p.Colors;
                    plt->PaletteFormat = p.Format;

                    pltHdrs += TPLPaletteHeader.Size;

                    plt->_data = (uint)(pltData - address);

                    p.Rebuild(pltData, p._calcSize, true);
                    pltData += p._calcSize;
                }
                else
                    values[1] = 0;

                //Rebuild texture
                values[0] = (uint)(texHdrs - address);
                TPLTextureHeader* tex = (TPLTextureHeader*)texHdrs;
                tex->_wrapS = (uint)t._uWrap;
                tex->_wrapT = (uint)t._vWrap;
                tex->_minFilter = (uint)t._minFltr;
                tex->_magFilter = (uint)t._magFltr;
                tex->PixelFormat = t.Format;
                tex->_width = (ushort)t.Width;
                tex->_height = (ushort)t.Height;
                tex->_LODBias = t._lodBias;
                tex->_edgeLODEnable = (byte)t._enableEdgeLod;
                tex->_maxLOD = (byte)(t.LevelOfDetail - 1);
                tex->_minLOD = 0;

                texHdrs += TPLTextureHeader.Size;

                tex->_data = (uint)(texData - address);

                t.Rebuild(texData, t._calcSize, true);
                texData += t._calcSize;

                values += 2;
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((TPLHeader*)source.Address)->_tag == TPLHeader.Tag ? new TPLNode() : null; }
    }

    public unsafe class TPLTextureNode : ResourceNode, IImageSource
    {
        public override ResourceType ResourceType { get { return ResourceType.TPLTexture; } }
        internal TPLTextureHeader* Header { get { return (TPLTextureHeader*)WorkingUncompressed.Address; } }

        internal VoidPtr _dataAddr;

        public override string Name
        {
            get
            {
                return String.Format("Texture{0}", Index);
            }
            set
            {
                base.Name = value;
            }
        }

        int _width, _height;
        WiiPixelFormat _format;
        internal MatWrapMode _uWrap;
        internal MatWrapMode _vWrap;
        internal MatTextureMinFilter _minFltr;
        internal MatTextureMagFilter _magFltr;
        internal float _lodBias;
        internal int _lod;
        internal int _enableEdgeLod;

        [Category("Texture")]
        public int Width { get { return _width; } }
        [Category("Texture")]
        public int Height { get { return _height; } }
        [Category("Texture")]
        public WiiPixelFormat Format { get { return _format; } }
        [Category("Texture")]
        public MatWrapMode UWrapMode { get { return _uWrap; } set { _uWrap = value; SignalPropertyChange(); } }
        [Category("Texture")]
        public MatWrapMode VWrapMode { get { return _vWrap; } set { _vWrap = value; SignalPropertyChange(); } }
        [Category("Texture")]
        public MatTextureMinFilter MinFilter { get { return _minFltr; } set { _minFltr = value; SignalPropertyChange(); } }
        [Category("Texture")]
        public MatTextureMagFilter MagFilter { get { return _magFltr; } set { _magFltr = value; SignalPropertyChange(); } }
        [Category("Texture")]
        public float LODBias { get { return _lodBias; } set { _lodBias = value; SignalPropertyChange(); } }
        [Category("Texture")]
        public int EnableEdgeLOD { get { return _enableEdgeLod; } set { _enableEdgeLod = value; SignalPropertyChange(); } }
        [Category("Texture")]
        public int LevelOfDetail { get { return _lod; } }

        public TPLPaletteNode GetPaletteNode() { return Children.Count > 0 ? Children[0] as TPLPaletteNode : null; }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _name = "Texture";

            _width = Header->_width;
            _height = Header->_height;
            _format = Header->PixelFormat;
            _uWrap = (MatWrapMode)(uint)Header->_wrapS;
            _vWrap = (MatWrapMode)(uint)Header->_wrapT;
            _minFltr = (MatTextureMinFilter)(uint)Header->_minFilter;
            _magFltr = (MatTextureMagFilter)(uint)Header->_magFilter;
            _lod = Header->_maxLOD + 1;
            _enableEdgeLod = Header->_edgeLODEnable;

            if (_replaced)
                _dataAddr = WorkingUncompressed.Address + Header->_data;

            return false;
        }

        [Browsable(false)]
        public int ImageCount { get { return LevelOfDetail; } }
        public Bitmap GetImage(int index)
        {
            try
            {
                TPLPaletteNode plt = GetPaletteNode();
                if (plt != null)
                    return TextureConverter.DecodeIndexed(_dataAddr, Width, Height, plt.Palette, index + 1, _format);
                else
                    return TextureConverter.Decode(_dataAddr, Width, Height, index + 1, _format);
            }
            catch { return null; }
        }

        public void Replace(Bitmap bmp)
        {
            FileMap tMap, pMap;
            if (Children.Count > 0)
            {
                TPLPaletteNode pn = GetPaletteNode();
                tMap = TextureConverter.Get(Format).EncodeTPLTextureIndexed(bmp, pn.Colors, pn.Format, QuantizationAlgorithm.MedianCut, out pMap);
                ReplaceRaw(tMap);
                AddChild(pn);
                pn.ReplaceRaw(pMap);
                UpdateCurrentControl();
            }
            else
            {
                tMap = TextureConverter.Get(Format).EncodeTPLTexture(bmp, 1);
                ReplaceRaw(tMap);
            }
        }

        public override unsafe void Replace(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            Bitmap bmp;
            if (String.Equals(ext, ".tga", StringComparison.OrdinalIgnoreCase))
                bmp = TGA.FromFile(fileName);
            else if (
                String.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(ext, ".tif", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(ext, ".tiff", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(ext, ".bmp", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(ext, ".gif", StringComparison.OrdinalIgnoreCase))
                bmp = (Bitmap)Bitmap.FromFile(fileName);
            else
            {
                base.Replace(fileName);
                return;
            }

            using (Bitmap b = bmp)
                Replace(b);
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".png"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Png);
            else if (outPath.EndsWith(".tga"))
                using (Bitmap bmp = GetImage(0)) bmp.SaveTGA(outPath);
            else if (outPath.EndsWith(".tiff") || outPath.EndsWith(".tif"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Tiff);
            else if (outPath.EndsWith(".bmp"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Bmp);
            else if (outPath.EndsWith(".jpg") || outPath.EndsWith(".jpeg"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Jpeg);
            else if (outPath.EndsWith(".gif"))
                using (Bitmap bmp = GetImage(0)) bmp.Save(outPath, ImageFormat.Gif);
            else
            {
                int dataLen = TPLTextureHeader.Size + CalculateSize(true);
                using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
                {
                    stream.SetLength(dataLen);
                    using (FileMap map = FileMap.FromStream(stream))
                    {
                        TPLTextureHeader* tex = (TPLTextureHeader*)map.Address;
                        tex->_wrapS = (uint)_uWrap;
                        tex->_wrapT = (uint)_vWrap;
                        tex->_minFilter = (uint)_minFltr;
                        tex->_magFilter = (uint)_magFltr;
                        tex->PixelFormat = Format;
                        tex->_width = (ushort)Width;
                        tex->_height = (ushort)Height;
                        tex->_LODBias = _lodBias;
                        tex->_edgeLODEnable = (byte)_enableEdgeLod;
                        tex->_maxLOD = (byte)(LevelOfDetail - 1);
                        tex->_minLOD = 0;

                        VoidPtr data = (VoidPtr)tex + TPLTextureHeader.Size;
                        tex->_data = (uint)(data - map.Address);
                        Rebuild(data, _calcSize, true);
                    }
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TextureConverter.Get(Format).GetMipOffset(Width, Height, LevelOfDetail + 1);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Memory.Move(address, _dataAddr, (uint)length);
            _dataAddr = address;
        }
    }
    
    public unsafe class TPLPaletteNode : ResourceNode, IColorSource
    {
        public override ResourceType ResourceType { get { return ResourceType.TPLPalette; } }
        internal TPLPaletteHeader* Header { get { return (TPLPaletteHeader*)WorkingUncompressed.Address; } }

        //private int _numColors;
        private WiiPaletteFormat _format;

        internal VoidPtr _dataAddr;

        private ColorPalette _palette;
        [Browsable(false)]
        public ColorPalette Palette
        {
            get { return _palette == null ? _palette = TextureConverter.DecodePalette(_dataAddr, Header->_numEntries, Format) : _palette; }
            set { _palette = value; SignalPropertyChange(); }
        }

        [Category("Palette")]
        public int Colors { get { return Palette.Entries.Length; } }// set { _numColors = value; } }
        [Category("Palette")]
        public WiiPaletteFormat Format { get { return _format; } set { _format = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _palette = null;

            _name = "Palette";

            //_numColors = Header->_numEntries;
            _format = Header->PaletteFormat;

            if (_replaced)
                _dataAddr = WorkingUncompressed.Address + Header->_data;

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return Palette.Entries.Length.Align(16) * 2;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TextureConverter.EncodePalette(address, Palette, _format);
            _dataAddr = address;
        }

        #region IColorSource Members

        public bool HasPrimary(int id) { return false; }
        public ARGBPixel GetPrimaryColor(int id) { return new ARGBPixel(); }
        public void SetPrimaryColor(int id, ARGBPixel color) { }
        [Browsable(false)]
        public string PrimaryColorName(int id) { return null; }
        [Browsable(false)]
        public int TypeCount { get { return 1; } }
        [Browsable(false)]
        public int ColorCount(int id) { return Palette.Entries.Length; }
        public ARGBPixel GetColor(int index, int id) { return (ARGBPixel)Palette.Entries[index]; }
        public void SetColor(int index, int id, ARGBPixel color) { Palette.Entries[index] = (Color)color; SignalPropertyChange(); }
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

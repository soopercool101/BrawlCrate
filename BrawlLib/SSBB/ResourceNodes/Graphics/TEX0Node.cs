using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Drawing;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Textures;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEX0Node : BRESEntryNode, IImageSource
    {
        internal TEX0v1* Header1 => (TEX0v1*) WorkingUncompressed.Address;
        internal TEX0v2* Header2 => (TEX0v2*) WorkingUncompressed.Address;
        internal TEX0v3* Header3 => (TEX0v3*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => SharesData ? ResourceType.SharedTEX0 : ResourceType.TEX0;
        public override int DataAlign => 0x20;
        public override int[] SupportedVersions => new int[] {1, 2, 3};

        public TEX0Node()
        {
            _version = 1;
        }

        private bool _sharesData;
        private int _headerLen;
        private int _width, _height;
        private WiiPixelFormat _format;
        private int _lod;
        private bool _hasPalette;

        public int texSortNum;

        // Could improve performance by caching, and making sure to clear the cache when needed.
        // For now, prefer the simplicity of not identifying every situation where clearing the cache would be needed.
        private TEX0Node SourceNode => FindSourceNode();

        [Category("G3D Node")]
        public bool SharesData
        {
            get => _sharesData;
            set
            {
                Bitmap bmp = GetImage(0);
                bool disableRevert = false;
                TEX0Node t = PrevSibling() as TEX0Node;
                if (!_revertingCS && !value)
                {
                    if (MessageBox.Show(
                            "Would you like to revert color smashing for the node and all nodes that share data above it? (If your preview looks correct now, say yes. If your preview looks bugged, say no)",
                            "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        _sharesData = value;
                        SignalPropertyChange();
                        return;
                    }

                    _revertingCS = true;
                    disableRevert = true;
                }

                _sharesData = value;
                SignalPropertyChange();
                if (!value)
                {
                    if (t != null && t.SharesData)
                    {
                        t.SharesData = false;
                    }

                    using (TextureConverterDialog dlg = new TextureConverterDialog())
                    {
                        dlg.Automatic = true;
                        dlg.cboFormat.SelectedItem =
                            dlg.LoadImages(bmp);
                        dlg.ShowDialog(null, this);
                    }
                }

                if (disableRevert)
                {
                    _revertingCS = false;
                }
            }
        }

        private static bool _revertingCS = false;

        [Category("G3D Texture")] public int Width => SharesData ? SourceNode.Width : _width;
        [Category("G3D Texture")] public int Height => SharesData ? SourceNode.Height : _height;
        [Category("G3D Texture")] public WiiPixelFormat Format => SharesData ? SourceNode.Format : _format;
        [Category("G3D Texture")] public int LevelOfDetail => SharesData ? SourceNode.LevelOfDetail : _lod;
        [Category("G3D Texture")] public bool HasPalette => SharesData ? SourceNode.HasPalette : _hasPalette;

        public PLT0Node GetPaletteNode()
        {
            return _parent?._parent == null || !HasPalette
                ? null
                : Parent._parent.FindChild("Palettes(NW4R)/" + Name, false) as PLT0Node;
        }

        private int HeaderSize()
        {
            int headerLen = TEX0v1.Size;
            if (Version == 3)
            {
                headerLen += _userEntries.GetSize();
            }

            return headerLen;
        }

        private int DataSize()
        {
            return TextureConverter.Get(Format).GetMipOffset(Width, Height, LevelOfDetail + 1);
        }

        // The offset from the start of the header to the start of its data.
        // Equal to the length of its header if not shared, the length of all headers between it and its data if shared.
        private int OffsetToData()
        {
            int offset = 0;
            ResourceNode node = this;

            while (node is TEX0Node)
            {
                TEX0Node texNode = node as TEX0Node;
                offset += texNode.HeaderSize();
                if (!texNode.SharesData)
                {
                    return offset;
                }

                node = node.NextSibling();
            }

            return 0;
        }

        // The size of this entry, not including any shared data.
        // Equals the size of the header if shared, header + data if not shared.
        private int ExclusiveEntrySize()
        {
            int size = HeaderSize();
            if (!SharesData)
            {
                size += DataSize();
            }

            return size;
        }

        // This size of this entry, including any shared data and other entries' headers between it and its data.
        private int InclusiveEntrySize()
        {
            return OffsetToData() + DataSize();
        }

        // A TEX0 node we use instead of null when we share with a non-existent node.
        // This way we don't have to write null checks on the return value of SourceNode.
        private static TEX0Node _nullTEX0Node;

        private static TEX0Node NullTEX0Node()
        {
            if (_nullTEX0Node == null)
            {
                TEX0v1 NullTEX0Header = new TEX0v1(0, 0, WiiPixelFormat.CMPR, 1);
                _nullTEX0Node = new TEX0Node();
                _nullTEX0Node.Initialize(null, &NullTEX0Header, sizeof(TEX0v1));
            }

            return _nullTEX0Node;
        }

        private TEX0Node FindSourceNode()
        {
            ResourceNode candidate = this;
            while (candidate is TEX0Node)
            {
                TEX0Node texNode = candidate as TEX0Node;
                if (!texNode.SharesData)
                {
                    return texNode;
                }

                candidate = candidate.NextSibling();
            }

            return NullTEX0Node();
        }

        public override string Name
        {
            get => base.Name;
            set
            {
                if (HasPalette && GetPaletteNode() != null)
                {
                    GetPaletteNode().Name = value;
                }

                base.Name = value;
            }
        }


        public override bool OnInitialize()
        {
            base.OnInitialize();
            _headerLen = Header1->_headerLen;

            _sharesData = _headerLen > HeaderSize();
            if (_sharesData)
            {
                SetSizeInternal(ExclusiveEntrySize());
            }

            if (_version == 2)
            {
                if (_name == null && Header2->_stringOffset != 0)
                {
                    _name = Header2->ResourceString;
                }

                _width = Header2->_width;
                _height = Header2->_height;
                _format = Header2->PixelFormat;
                _lod = Header2->_levelOfDetail;
                _hasPalette = Header2->HasPalette;
            }
            else
            {
                if (_name == null && Header1->_stringOffset != 0)
                {
                    _name = Header1->ResourceString;
                }

                _width = Header1->_width;
                _height = Header1->_height;
                _format = Header1->PixelFormat;
                _lod = Header1->_levelOfDetail;
                _hasPalette = Header1->HasPalette;
            }

            if (_version == 3)
            {
                (_userEntries = new UserDataCollection()).Read(Header3->UserData);
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return ExclusiveEntrySize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int offset = OffsetToData();
            if (!SharesData)
            {
                Memory.Move(address + offset, WorkingUncompressed.Address + offset, (uint) length - (uint) offset);
            }

            switch (Version)
            {
                case 1:
                    *(TEX0v1*) address = new TEX0v1(Width, Height, Format, LevelOfDetail, offset);
                    break;
                case 2:
                    *(TEX0v2*) address = new TEX0v2(Width, Height, Format, LevelOfDetail, offset);
                    break;
                case 3:
                    *(TEX0v3*) address = new TEX0v3(Width, Height, Format, LevelOfDetail, offset);
                    _userEntries.Write(address + TEX0v3.Size);
                    break;
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            if (_version == 3)
            {
                _userEntries.GetStrings(table);
            }

            if (!string.IsNullOrEmpty(_originalPath))
            {
                table.Add(_originalPath);
            }
        }

        [Browsable(false)] public int ImageCount => LevelOfDetail;

        public Bitmap GetImage(int index)
        {
            PLT0Node plt = GetPaletteNode();
            return SharesData ? SourceNode.GetImage(index, plt) : GetImage(index, plt);
        }

        public Bitmap GetImage(int index, PLT0Node plt)
        {
            try
            {
                if (SharesData && SourceNode != this)
                {
                    return SourceNode.GetImage(index, plt);
                }

                if (WorkingUncompressed != DataSource.Empty)
                {
                    if (plt != null)
                    {
                        return TextureConverter.DecodeIndexed(
                            (VoidPtr) CommonHeader + _headerLen, _width, _height, plt.Palette, index + 1, _format);
                    }
                    else
                    {
                        return TextureConverter.Decode(
                            (VoidPtr) CommonHeader + _headerLen, _width, _height, index + 1, _format);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            if (SharesData)
            {
                BRESCommonHeader* commonHeader = (BRESCommonHeader*) dataAddress;
                commonHeader->_size = InclusiveEntrySize();
            }

            if (_version == 2)
            {
                TEX0v2* header = (TEX0v2*) dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }
            }
            else
            {
                TEX0v1* header = (TEX0v1*) dataAddress;
                header->ResourceStringAddress = stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }
            }
        }

        public void Replace(Bitmap bmp)
        {
            FileMap tMap;
            if (HasPalette)
            {
                PLT0Node pn = GetPaletteNode();
                tMap = TextureConverter.Get(Format).EncodeTextureIndexed(bmp, LevelOfDetail, pn.Colors, pn.Format,
                    QuantizationAlgorithm.MedianCut, out FileMap pMap);
                pn.ReplaceRaw(pMap);
            }
            else
            {
                tMap = TextureConverter.Get(Format).EncodeTEX0Texture(bmp, LevelOfDetail);
            }

            ReplaceRaw(tMap);
        }

        public override unsafe void Replace(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (!string.Equals(ext, ".tex0", StringComparison.OrdinalIgnoreCase))
            {
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = fileName;
                    dlg.ShowDialog(null, this);
                }
            }
            else
            {
                base.Replace(fileName);
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

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((TEX0v1*) source.Address)->_header._tag == TEX0v1.Tag ? new TEX0Node() : null;
        }
    }
}
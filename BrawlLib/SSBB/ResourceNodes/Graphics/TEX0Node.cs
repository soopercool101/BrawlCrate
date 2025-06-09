using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Drawing;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using BrawlLib.Wii.Textures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO.Hashing;

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
        public override string Tag => "TEX0";

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
        private string _textureHash => GetTextureHash();
        private string _paletteHash => GetTlutHash();

        public int texSortNum;

        // Could improve performance by caching, and making sure to clear the cache when needed.
        // For now, prefer the simplicity of not identifying every situation where clearing the cache would be needed.
        public TEX0Node SourceNode => FindSourceNode();

        [Category("G3D Node")]
        public bool SharesData
        {
            get => _sharesData;
            set => SetSharesData(value, true);
        }

        public void SetSharesData(bool value, bool showMessages)
        {
            Bitmap bmp = GetImage(0);
            bool disableRevert = false;
            TEX0Node t = PrevSibling() as TEX0Node;
            if (!_revertingCS && !value)
            {
                if (!showMessages || MessageBox.Show(
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

        private static bool _revertingCS = false;

        [Category("G3D Texture")] public int Width => SharesData ? SourceNode.Width : _width;
        [Category("G3D Texture")] public int Height => SharesData ? SourceNode.Height : _height;
        [Category("G3D Texture")] public WiiPixelFormat Format => SharesData ? SourceNode.Format : _format;
        [Category("G3D Texture")] public int LevelOfDetail => SharesData ? SourceNode.LevelOfDetail : _lod;
        [Category("G3D Texture")] public bool HasPalette => SharesData ? SourceNode.HasPalette : _hasPalette;
        [Category("Dolphin")] public string DolphinTextureName => BuildDolphinTextureName();
        [Browsable(false)] public string TextureHash => _textureHash;
        [Browsable(false)] public string PaletteHash => _paletteHash;

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

        private string BuildDolphinTextureName()
        {
            var name = $"tex1_{Width}x{Height}_";
            if (LevelOfDetail > 1)
                name += "m_";
            name += $"{_textureHash}_";
            if (HasPalette && !string.IsNullOrEmpty(_paletteHash))
                name += $"{_paletteHash}_";
            else if (Format == WiiPixelFormat.CI4 || Format == WiiPixelFormat.CI8)
                name += "$_";
            name += $"{(int)Format}";
            return name;
        }

        private ReadOnlySpan<byte> GetRawData()
        {
            // Get size for first mip only
            var length = TextureConverter.Get(Format).GetMipOffset(Width, Height, 2);
            // Get raw pixel data
            byte[] pixelData = new byte[length];
            Marshal.Copy(SourceNode.WorkingUncompressed.Address + HeaderSize(), pixelData, 0, length);
            return pixelData;
        }

        private string GetTextureHash()
        {
            var pixelData = GetRawData();
            return XxHash64.HashToUInt64(pixelData).ToString("x16");
        }

        private static (int start, int length) GetTlutRange(WiiPixelFormat format, ReadOnlySpan<byte> bytes)
        {
            int start = 0xffff, length = 0;

            switch (format)
            {
                case WiiPixelFormat.CI4:
                    foreach (byte b in bytes)
                    {
                        int low_nibble = b & 0xf;
                        int high_nibble = b >> 4;

                        start = Math.Min(start, Math.Min(low_nibble, high_nibble));
                        length = Math.Max(length, Math.Max(low_nibble, high_nibble));
                    }
                    break;

                case WiiPixelFormat.CI8:
                    foreach (byte b in bytes)
                    {
                        int texture_byte = b;
                        start = Math.Min(start, texture_byte);
                        length = Math.Max(length, texture_byte);
                    }
                    break;

                default:
                    break;
            }
            return (start * 2, (length + 1 - start) * 2);
        }

        private string GetTlutHash()
        {
            var image = GetRawData();
            (int start, int length) = GetTlutRange(Format, image);

            var palette = GetPaletteNode();
            if (palette != null)
            {
                var paletteData = palette.GetRawData();
                if (paletteData.Length < length)
                {
                    return "";
                }
                return XxHash64.HashToUInt64(paletteData.Slice(start, length)).ToString("x16");
            }
            return "";
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

            _sharesData = _headerLen > HeaderSize() && new string((sbyte*)WorkingUncompressed.Address + HeaderSize()).StartsWith("TEX0") || WorkingUncompressed.Map != null && _headerLen >= WorkingUncompressed.Map.Length;
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
                (_userEntries = new UserDataCollection()).Read(Header3->UserData, RootNode.WorkingUncompressed);
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

                    return TextureConverter.Decode(
                        (VoidPtr) CommonHeader + _headerLen, _width, _height, index + 1, _format);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public override void Remove()
        {
            Remove(false);
        }

        public void Remove(bool forceRemovePalettes)
        {
            PLT0Node p = GetPaletteNode();
            if (HasPalette && p != null && (forceRemovePalettes ||
                                            MessageBox.Show("Would you like to delete the associated PLT0?",
                                                $"Deleting {Name}", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                while ((p = GetPaletteNode()) != null)
                {
                    p.Dispose();
                    p.Remove();
                }
            }

            base.Remove();
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

        public override void Replace(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(ext) && !string.Equals(ext, ".tmp", StringComparison.OrdinalIgnoreCase) && !string.Equals(ext, ".tex0", StringComparison.OrdinalIgnoreCase))
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

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((TEX0v1*) source.Address)->_header._tag == TEX0v1.Tag ? new TEX0Node() : null;
        }

        public PAT0Node GeneratePAT0()
        {
            return GeneratePAT0(false);
        }

        public PAT0Node GeneratePAT0(bool force)
        {
            if (Parent == null)
            {
                return null;
            }

            if (Parent is BRESGroupNode && Parent.Parent != null && Parent.Parent is BRRESNode)
            {
                // Check if this is part of a sequence
                if (Regex.Match(Name, @"(\.\d+)?$").Success && Name.LastIndexOf(".") > 0 &&
                    Name.LastIndexOf(".") <= Name.Length && int.TryParse(
                        Name.Substring(Name.LastIndexOf(".") + 1,
                            Name.Length - (Name.LastIndexOf(".") + 1)), out int n))
                {
                    //Console.WriteLine(_resource.Name.Substring(0, _resource.Name.LastIndexOf(".")) + " is part of a sequence");
                    //Console.WriteLine(_resource.Name.Substring(_resource.Name.LastIndexOf(".") + 1, _resource.Name.Length - (_resource.Name.LastIndexOf(".") + 1)));
                    // Determine the name to match
                    string matchName = Name.Substring(0, Name.LastIndexOf(".")) + ".";
                    BRESGroupNode paletteGroup = ((BRRESNode)Parent.Parent).GetFolder<PLT0Node>();
                    List<string> textureList = new List<string>();
                    List<PLT0Node> paletteList = new List<PLT0Node>();
                    int highestNum = -1;
                    bool isStock = false;
                    bool isStockEx = false;
                    if (matchName.Equals("InfStc.", StringComparison.OrdinalIgnoreCase))
                    {
                        isStock = true;
                        if (Name.Length >= 11)
                        {
                            isStockEx = true;
                        }
                    }

                    foreach (TEX0Node tx0 in Parent.Children)
                    {
                        if (tx0.Name.StartsWith(matchName) && tx0.Name.LastIndexOf(".") > 0 &&
                            tx0.Name.LastIndexOf(".") <= tx0.Name.Length &&
                            int.TryParse(
                                tx0.Name.Substring(tx0.Name.LastIndexOf(".") + 1,
                                    tx0.Name.Length - (tx0.Name.LastIndexOf(".") + 1)), out int n2) && n2 >= 0 &&
                            !textureList.Contains(tx0.Name))
                        {
                            if (isStock)
                            {
                                if (isStockEx && tx0.Name.Length < 11)
                                {
                                    continue;
                                }
                            }

                            // Add the matching texture to the texture list for the PAT0
                            textureList.Add(tx0.Name);
                            // Determine the highest number used
                            if (n2 > highestNum)
                            {
                                highestNum = n2;
                            }

                            Console.WriteLine(tx0.Name);
                            Console.WriteLine(tx0.HasPalette);
                            if (tx0.HasPalette)
                            {
                                paletteList.Add(tx0.GetPaletteNode());
                            }
                            else
                            {
                                paletteList.Add(null);
                            }
                        }
                    }

                    if (textureList.Count <= 0)
                    {
                        return null;
                    }

                    PAT0Node newPat0 = new PAT0Node
                    {
                        Name = Name.Substring(0, Name.LastIndexOf(".")).Equals("InfStc")
                            ? "InfStockface_TopN__0"
                            : Name.Substring(0, Name.LastIndexOf(".")),
                        _numFrames = highestNum + 1
                    };
                    PAT0EntryNode pat0Entry = new PAT0EntryNode();
                    newPat0.AddChild(pat0Entry);
                    pat0Entry.Name = Name.Substring(0, Name.LastIndexOf(".")).Equals("InfStc")
                        ? "lambert87"
                        : Name.Substring(0, Name.LastIndexOf("."));
                    PAT0TextureNode pat0Tex = new PAT0TextureNode((PAT0Flags)7, 0);
                    pat0Entry.AddChild(pat0Tex);
                    if (((BRRESNode)Parent.Parent).GetFolder<PAT0Node>() != null &&
                        ((BRRESNode)Parent.Parent).GetFolder<PAT0Node>().FindChildrenByName(newPat0.Name)
                        .Length > 0)
                    {
                        if (force)
                        {
                            while (((BRRESNode)Parent.Parent).GetFolder<PAT0Node>()
                                .FindChildrenByName(newPat0.Name).Length > 0)
                            {
                                ((BRRESNode)Parent.Parent).GetFolder<PAT0Node>()
                                    .FindChildrenByName(newPat0.Name)[0].Remove();
                            }
                        }
                        else
                        {
                            DialogResult d = MessageBox.Show(
                                "Would you like to replace the currently existing \"" + newPat0.Name +
                                "\" PAT0 animation?", "PAT0 Generator", MessageBoxButtons.YesNoCancel);
                            if (d == DialogResult.Cancel || d == DialogResult.Abort)
                            {
                                return null;
                            }

                            if (d == DialogResult.Yes)
                            {
                                while (((BRRESNode)Parent.Parent).GetFolder<PAT0Node>()
                                       .FindChildrenByName(newPat0.Name).Length >
                                       0)
                                {
                                    ((BRRESNode)Parent.Parent).GetFolder<PAT0Node>()
                                        .FindChildrenByName(newPat0.Name)[0].Remove();
                                }
                            }
                        }
                    }

                    if (isStock && !isStockEx && !textureList.Contains("InfStc.000"))
                    {
                        textureList.Add("InfStc.000");
                        paletteList.Add(null);
                    }
                    else if (isStock && isStockEx && !textureList.Contains("InfStc.0000"))
                    {
                        textureList.Add("InfStc.0000");
                        paletteList.Add(null);
                    }

                    //foreach(string s in textureList)
                    for (int i = 0; i < textureList.Count; ++i)
                    {
                        string s = textureList[i];
                        if (float.TryParse(s.Substring(s.LastIndexOf(".") + 1, s.Length - (s.LastIndexOf(".") + 1)),
                            out float fr))
                        {
                            PAT0TextureEntryNode pat0texEntry = new PAT0TextureEntryNode();
                            pat0Tex.AddChild(pat0texEntry);
                            pat0texEntry.Name = s;
                            pat0texEntry._frame = fr;
                            if (paletteList[i] != null)
                            {
                                pat0Tex.HasPalette = true;
                                pat0texEntry._plt = paletteList[i].Name;
                            }
                            else if ((s == "InfStc.000" || s == "InfStc.0000") && pat0Tex.HasPalette)
                            {
                                pat0texEntry._plt = s;
                            }

                            if (fr == 0)
                            {
                                PAT0TextureEntryNode pat0texEntryFinal = new PAT0TextureEntryNode();
                                pat0Tex.AddChild(pat0texEntryFinal);
                                pat0texEntryFinal.Name = s;
                                pat0texEntryFinal._frame = highestNum + 1;
                                if (paletteList[i] != null)
                                {
                                    pat0Tex.HasPalette = true;
                                    pat0texEntryFinal.Palette = paletteList[i].Name;
                                }
                                else if ((s == "InfStc.000" || s == "InfStc.0000") && pat0Tex.HasPalette)
                                {
                                    pat0texEntryFinal._plt = s;
                                }
                            }
                        }

                        //newPat0.AddChild
                    }

                    pat0Tex._children = pat0Tex._children.OrderBy(o => ((PAT0TextureEntryNode)o)._frame).ToList();
                    if (isStock && !isStockEx && newPat0.FrameCount < 501)
                    {
                        newPat0.FrameCount = 501;
                    }
                    else if (isStockEx && newPat0.FrameCount < 9201)
                    {
                        newPat0.FrameCount = 9201;
                    }

                    ((BRRESNode)Parent.Parent).GetOrCreateFolder<PAT0Node>().AddChild(newPat0);

                    return newPat0;
                }
                else
                {
                    PAT0Node newPat0 = new PAT0Node
                    {
                        Name = Name,
                        _numFrames = 1
                    };
                    PAT0EntryNode pat0Entry = new PAT0EntryNode();
                    newPat0.AddChild(pat0Entry);
                    pat0Entry.Name = Name;
                    PAT0TextureNode pat0Tex = new PAT0TextureNode((PAT0Flags)7, 0);
                    pat0Entry.AddChild(pat0Tex);
                    PAT0TextureEntryNode pat0texEntry = new PAT0TextureEntryNode();
                    pat0Tex.AddChild(pat0texEntry);
                    pat0texEntry.Name = Name;
                    pat0texEntry._frame = 0;
                    if (HasPalette)
                    {
                        pat0Tex.HasPalette = true;
                        pat0texEntry.Palette = GetPaletteNode().Name;
                    }

                    ((BRRESNode)Parent.Parent).GetOrCreateFolder<PAT0Node>().AddChild(newPat0);
                    return newPat0;
                }
            }

            return null;
        }
    }
}
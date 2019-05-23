using System.Drawing;
using System.Drawing.Imaging;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using BrawlLib.IO;
using System.ComponentModel;
using System.IO;
using BrawlLib;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public partial class TextureConverterDialog : Form
    {
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bitmap Source { get { return _source; } set { _source = value; SourceChanged(); } }

        private Bitmap _base = null, _source, _preview, _indexed;
        private ColorInformation _colorInfo;
        private UnsafeBuffer _cmprBuffer;
        //private ColorPalette _tempPalette;
        private bool _previewing = true, _updating = false;

        private string _imageSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImageSource { get { return _imageSource; } set { _imageSource = value; } }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Drawing.Size? InitialSize;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WiiPixelFormat? InitialFormat;

        private BRRESNode _bresParent;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BRRESNode BRESParentNode { get { return _bresParent; } }
        private TPLNode _tplParent;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TPLNode TPLParentNode { get { return _tplParent; } }
        private REFTNode _reftParent;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public REFTNode REFTParentNode { get { return _reftParent; } }

        private TEX0Node _origTEX0;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TEX0Node TEX0TextureNode { get { return _origTEX0; } }
        private TPLTextureNode _origTPL;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TPLTextureNode TPLTextureNode { get { return _origTPL; } }
        private REFTEntryNode _origREFT;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public REFTEntryNode REFTTextureNode { get { return _origREFT; } }

        private PLT0Node _origPLT0;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PLT0Node PLT0PaletteNode { get { return _origPLT0; } }
        private TPLPaletteNode _origTPLPlt;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TPLPaletteNode TPLPaletteNode { get { return _origTPLPlt; } }
        
        private FileMap _textureData;
        private Label label11;
        private NumericUpDown numH;
        private NumericUpDown numW;
        private Label label10;
        private Button btnApplyDims;
        private CheckBox chkConstrainProps;
        private NumericUpDown numMIPPreview;
        private Label label12;
        private GCHandle? _pixelData;
        private CheckBox chkImportPalette;
        private CheckBox chkSwapRGB;
        private CheckBox chkSwapAlpha;
        private CheckBox chkSwapAlphaRGB;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileMap TextureData { get { return _textureData; } }

        private FileMap _paletteData;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileMap PaletteData { get { return _paletteData; } }

        public TextureConverterDialog()
        {
            InitializeComponent();

            numH.Maximum = numW.Maximum = decimal.MaxValue;

            dlgOpen.Filter = FileFilters.Images;

            foreach (WiiPixelFormat f in Enum.GetValues(typeof(WiiPixelFormat)))
                cboFormat.Items.Add(f);

            foreach (WiiPaletteFormat f in Enum.GetValues(typeof(WiiPaletteFormat)))
                cboPaletteFormat.Items.Add(f);

            foreach (QuantizationAlgorithm f in Enum.GetValues(typeof(QuantizationAlgorithm)))
                cboAlgorithm.Items.Add(f);

            cboAlgorithm.SelectedItem = QuantizationAlgorithm.MedianCut;
        }

        public DialogResult ShowDialog(IWin32Window owner, BRRESNode parent)
        {
            _bresParent = parent;
            _origTEX0 = null;
            _origREFT = null;
            _origPLT0 = null;
            _origTPL = null;
            _origTPLPlt = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        public DialogResult ShowDialog(IWin32Window owner, REFTNode parent)
        {
            _bresParent = null;
            _reftParent = parent;
            _origTEX0 = null;
            _origREFT = null;
            _origPLT0 = null;
            _origTPL = null;
            _origTPLPlt = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        public DialogResult ShowDialog(IWin32Window owner, TPLNode parent)
        {
            _bresParent = null;
            _reftParent = null;
            _tplParent = parent;
            _origTEX0 = null;
            _origREFT = null;
            _origPLT0 = null;
            _origTPL = null;
            _origTPLPlt = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        public DialogResult ShowDialog(IWin32Window owner, TEX0Node original)
        {
            _bresParent = null;
            _origTEX0 = original;
            _origPLT0 = original.GetPaletteNode();
            _origREFT = null; 
            _origTPL = null;
            _origTPLPlt = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        public DialogResult ShowDialog(IWin32Window owner, REFTEntryNode original)
        {
            _bresParent = null;
            _origREFT = original;
            _origTEX0 = null;
            _origTPL = null;
            _origTPLPlt = null;
            _origPLT0 = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        public DialogResult ShowDialog(IWin32Window owner, TPLTextureNode original)
        {
            _bresParent = null;
            _origREFT = null;
            _origTPL = original;
            _origTPLPlt = original.GetPaletteNode();
            _origTEX0 = null;
            _origPLT0 = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        new public DialogResult ShowDialog(IWin32Window owner)
        {
            _bresParent = null;
            _origTEX0 = null;
            _origPLT0 = null;
            _origREFT = null;
            _origTPL = null;
            _origTPLPlt = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_base == null)
                if (_imageSource == null)
                {
                    if (!LoadImages())
                    {
                        Close();
                        return;
                    }
                }
                else if (!LoadImages(_imageSource))
                {
                    Close();
                    return;
                }

            if (_origTEX0 != null)
            {
                _updating = true;

                cboFormat.SelectedItem = _origTEX0.Format;
                numLOD.Value = _origTEX0.LevelOfDetail;

                FixPaletteFields();

                if (_origPLT0 != null)
                {
                    grpPalette.Enabled = true;
                    cboPaletteFormat.SelectedItem = _origPLT0.Format;
                    numPaletteCount.Value = _origPLT0.Colors;
                }

                _updating = false;
                UpdatePreview();
            }
            else if (_origREFT != null)
            {
                _updating = true;
                cboFormat.SelectedItem = _origREFT.TextureFormat;
                numLOD.Value = _origREFT.LevelOfDetail.Clamp((int)numLOD.Minimum, (int)numLOD.Maximum);

                FixPaletteFields();

                _updating = false;
                UpdatePreview();
            }
            else if (_origTPL != null)
            {
                _updating = true;
                cboFormat.SelectedItem = _origTPL.Format;
                numLOD.Value = 1;
                //numLOD.Enabled = false;

                FixPaletteFields();

                if (_origTPLPlt != null)
                {
                    grpPalette.Enabled = true;
                    cboPaletteFormat.SelectedItem = _origTPLPlt.Format;
                    numPaletteCount.Value = _origTPLPlt.Colors;
                }

                _updating = false;
                UpdatePreview();
            }
            else
                Recommend();

			if (InitialSize != null) {
				chkConstrainProps.Checked = false;
				numW.Value = InitialSize.Value.Width;
				numH.Value = InitialSize.Value.Height;
				btnApplyDims.PerformClick();
			}
			if (InitialFormat != null) {
				cboFormat.SelectedItem = InitialFormat;
			}
        }

        public bool LoadImages(string path)
        {
            txtPath.Text = path;
            if (path.EndsWith(".tga", StringComparison.OrdinalIgnoreCase))
                return LoadImages(TGA.FromFile(path));
            else if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                return LoadImagesPreservingPaletteInfo(path);
            else
                return LoadImages((Bitmap)Bitmap.FromFile(path));
        }

        public bool LoadImages()
        {
            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return false;
            return LoadImages(dlgOpen.FileName);
        }

        public bool LoadImages(Bitmap bmp)
        {
            return LoadImages(bmp, null);
        }

        private bool LoadImages(Bitmap bmp, GCHandle? pixelData)
        {
            DisposeImages();
            Source = _base = bmp;
            _pixelData = pixelData;

            return true;
        }

        // Loads a PNG using WPF, and if its format is Indexed8, converts it to a GDI Bitmap with the proper palette info stored.
        // Otherwise reloads using GDI, as normal (which does not retain palette info).
        //
        // May want to extend this to work with Indexed4, Indexed2, or Indexed1 formats in the future.
        private bool LoadImagesPreservingPaletteInfo(string path)
        {
            Stream sourceStream = new FileStream(_imageSource, FileMode.Open, FileAccess.Read, FileShare.Read);
            PngBitmapDecoder decoder = new PngBitmapDecoder(sourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource preservedImage = decoder.Frames[0];
            if (preservedImage.Format == System.Windows.Media.PixelFormats.Indexed8)
            {
                Bitmap bmp;
                int width = Convert.ToInt32(preservedImage.Width);
                int height = Convert.ToInt32(preservedImage.Height);
                byte[] pixels = new byte[width * height];
                preservedImage.CopyPixels(pixels, width, 0);
                GCHandle pixelData = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                bmp = new Bitmap(width, height, width, PixelFormat.Format8bppIndexed, pixelData.AddrOfPinnedObject());

                IList<Media.Color> preservedColors = preservedImage.Palette.Colors;
                ColorPalette newPalette = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.None, preservedColors.Count);
                for (int i = 0; i < preservedColors.Count; i++)
                    newPalette.Entries[i] = Color.FromArgb(preservedColors[i].A, preservedColors[i].R, preservedColors[i].G, preservedColors[i].B);
                bmp.Palette = newPalette;
                return LoadImages(bmp, pixelData);
            }
            else
                return LoadImages((Bitmap)Bitmap.FromFile(path));
        }

        private Bitmap ImportPalette()
        {
            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.None, _source.Palette.Entries.Length.Align(16));
            for (int i = 0; i < _source.Palette.Entries.Length; i++)
                pal.Entries[i] = _source.Palette.Entries[i];
            for (int i = _source.Palette.Entries.Length; i < pal.Entries.Length; i++)
                pal.Entries[i] = Color.FromArgb(0);
            pal.Clamp((WiiPaletteFormat)cboPaletteFormat.SelectedItem);

            Bitmap bmp = (Bitmap)_source.Clone();
            bmp.Palette = pal;
            return bmp;
        }

        private void SourceChanged()
        {
            if (_source == null)
            {
                _preview = null;
                return;
            }

            _preview = new Bitmap(_source.Width, _source.Height, PixelFormat.Format32bppArgb);
            lblSize.Text = String.Format("{0} x {1}", _source.Width, _source.Height);

            _colorInfo = _source.GetColorInformation();
            lblColors.Text = _colorInfo.ColorCount.ToString();
            lblTransparencies.Text = _colorInfo.AlphaColors.ToString();

            //Get max LOD
            int maxLOD = 1;
            for (int w = _source.Width, h = _source.Height; (w != 1) && (h != 1); w >>= 1, h >>= 1, maxLOD++) ;
            numLOD.Maximum = maxLOD;
            numMIPPreview.Maximum = maxLOD;

            if (_updating)
                return;

            numW.Value = _source.Width;
            numH.Value = _source.Height;
        }

        private void DisposeImages()
        {
            pictureBox1.Picture = null;
            if (_base != null) { _base.Dispose(); _base = null; }
            if (_preview != null) { _preview.Dispose(); _preview = null; }
            if (_source != null) { _source.Dispose(); _source = null; }
            if (_indexed != null) { _indexed.Dispose(); _indexed = null; }
            if (_pixelData.HasValue) { _pixelData.Value.Free(); _pixelData = null; }
        }

        private void CopyPreview(Bitmap src)
        {
            Rectangle r = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData srcData = src.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = _preview.LockBits(r, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Memory.Move(dstData.Scan0, srcData.Scan0, (uint)(srcData.Stride * src.Height));

            _preview.UnlockBits(dstData);
            src.UnlockBits(srcData);
        }

        internal void UpdatePreview()
        {
            if (_source == null)
                return;

            if (_cmprBuffer != null) { _cmprBuffer.Dispose(); _cmprBuffer = null; }
            if (_indexed != null) { _indexed.Dispose(); _indexed = null; }

            WiiPixelFormat format = (WiiPixelFormat)cboFormat.SelectedItem;
            switch (format)
            {
                case WiiPixelFormat.I4:
                case WiiPixelFormat.I8:
                case WiiPixelFormat.IA4:
                case WiiPixelFormat.IA8:
                case WiiPixelFormat.RGB565:
                case WiiPixelFormat.RGB5A3:
                case WiiPixelFormat.RGBA8:
                    {
                        CopyPreview(_source);
                        _preview.Clamp(format);
                        break;
                    }
                case WiiPixelFormat.CMPR:
                    {
                        CopyPreview(_source);
                        _cmprBuffer = TextureConverter.CMPR.GeneratePreview(_preview);
                        break;
                    }
                case WiiPixelFormat.CI4:
                case WiiPixelFormat.CI8:
                    {
                        if (chkImportPalette.Enabled && chkImportPalette.Checked)
                            _indexed = ImportPalette();
                        else
                            _indexed = _source.Quantize((QuantizationAlgorithm)cboAlgorithm.SelectedItem, (int)numPaletteCount.Value, format, (WiiPaletteFormat)cboPaletteFormat.SelectedItem, null);
                        CopyPreview(_indexed);
                        break;
                    }
            }

            UpdateSize();

            if (_previewing)
                pictureBox1.Picture = _preview;
            else
                pictureBox1.Picture = _source;
        }
        private void UpdateSize()
        {
            if (_source == null)
                return;

            int w = _source.Width, h = _source.Height;
            if (_origTEX0 != null || _bresParent != null)
            {
                int palSize = PaletteSize(0x40);
                lblDataSize.Text = String.Format("{0:n0}B", TextureConverter.Get((WiiPixelFormat)cboFormat.SelectedItem).GetMipOffset(ref w, ref h, (int)numLOD.Value + 1) + 0x40 + palSize);
            }
            else if (_origREFT != null || _reftParent != null)
            {
                int palSize = PaletteSize(0);
                lblDataSize.Text = String.Format("{0:n0}B", TextureConverter.Get((WiiPixelFormat)cboFormat.SelectedItem).GetMipOffset(ref w, ref h, (int)numLOD.Value + 1) + 0x20 + palSize);
            }
            else if (_origTPL != null || _tplParent != null)
            {
                int palSize = PaletteSize(0xC);
                lblDataSize.Text = String.Format("{0:n0}B", TextureConverter.Get((WiiPixelFormat)cboFormat.SelectedItem).GetMipOffset(ref w, ref h, (int)numLOD.Value + 1) + 0x28 + palSize);
            }
        }
        private int PaletteSize(int formatOverhead)
        {
            if (!grpPalette.Enabled)
                return 0;
            if (chkImportPalette.Enabled && chkImportPalette.Checked)
                return _source.Palette.Entries.Length.Align(16) * 2 + formatOverhead;
            return (int)numPaletteCount.Value * 2 + formatOverhead;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadImages();
            UpdatePreview();
        }

        private void btnRecommend_Click(object sender, EventArgs e)
        {
            Recommend();
        }

        private void Recommend()
        {
            if ((_source == null) || (_updating))
                return;

            _updating = true;
            if (_colorInfo.IsGreyscale)
            {
                if (_colorInfo.ColorCount <= 16)
                    cboFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPixelFormat.I4 : WiiPixelFormat.CI4;
                else if (_colorInfo.ColorCount <= 272)
                    cboFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPixelFormat.I8 : WiiPixelFormat.IA8;
                else
                    cboFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPixelFormat.RGB565 : WiiPixelFormat.RGB5A3;
            }
            else
            {
                if (_colorInfo.ColorCount <= 16)
                    cboFormat.SelectedItem = WiiPixelFormat.CI4;
                else if (_colorInfo.ColorCount <= 272)
                    cboFormat.SelectedItem = WiiPixelFormat.CI8;
                else if (_colorInfo.AlphaColors <= 1)
                    cboFormat.SelectedItem = WiiPixelFormat.CMPR;
                else
                    cboFormat.SelectedItem = WiiPixelFormat.RGB5A3;
            }
            FixPaletteFields();

            if (Source.GuessIfAlphaInverted()) {
                chkSwapAlpha.Checked = true;
                Source = Source.InvertAlpha();
            }

            _updating = false;
            UpdatePreview();
        }

        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((_source == null) || (_updating))
                return;

            _updating = true;

            FixPaletteFields();

            _updating = false;
            UpdatePreview();
        }

        private void FixPaletteFields()
        {
            switch ((WiiPixelFormat)cboFormat.SelectedItem)
            {
                case WiiPixelFormat.I4:
                case WiiPixelFormat.I8:
                case WiiPixelFormat.IA4:
                case WiiPixelFormat.IA8:
                case WiiPixelFormat.RGB565:
                case WiiPixelFormat.RGB5A3:
                case WiiPixelFormat.RGBA8:
                case WiiPixelFormat.CMPR:
                    grpPalette.Enabled = false; break;

                case WiiPixelFormat.CI4:
                    {
                        grpPalette.Enabled = true;
                        numPaletteCount.Maximum = 16;
                        numPaletteCount.Value = 16;
                        cboPaletteFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPaletteFormat.RGB565 : WiiPaletteFormat.RGB5A3;
                        FixImportPaletteFields();
                        break;
                    }
                case WiiPixelFormat.CI8:
                    {
                        grpPalette.Enabled = true;
                        numPaletteCount.Maximum = 256;
                        numPaletteCount.Value = Math.Min(256, _colorInfo.ColorCount.Align(16));
                        cboPaletteFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPaletteFormat.RGB565 : WiiPaletteFormat.RGB5A3;
                        int sourcePaletteSize = _source.Palette.Entries.Length;
                        if (sourcePaletteSize > 0 && sourcePaletteSize <= 256)
                        {
                            numPaletteCount.Value = sourcePaletteSize.Align(16);
                        }
                        FixImportPaletteFields();
                        break;
                    }
            }
        }
        private void FixImportPaletteFields()
        {
            if ((WiiPixelFormat)cboFormat.SelectedItem == WiiPixelFormat.CI8 && _source.PixelFormat == PixelFormat.Format8bppIndexed ||
                (WiiPixelFormat)cboFormat.SelectedItem == WiiPixelFormat.CI4 && _source.PixelFormat == PixelFormat.Format4bppIndexed)
            {
                // Checks if the image is not being resized
                if (_source == _base)
                {
                    chkImportPalette.Enabled = true;
                    FixImportPaletteDependentFields();
                    return;
                }
            }

            chkImportPalette.Enabled = false;
            FixImportPaletteDependentFields();
        }
        private void FixImportPaletteDependentFields()
        {
            if (chkImportPalette.Enabled && chkImportPalette.Checked)
            {
                numPaletteCount.Enabled = false;
                cboAlgorithm.Enabled = false;
                chkSwapRGB.Enabled = false;
                chkSwapAlpha.Enabled = false;
            }
            else
            {
                numPaletteCount.Enabled = true;
                cboAlgorithm.Enabled = true;
                chkSwapRGB.Enabled = true;
                chkSwapAlpha.Enabled = true;
            }
        }

        private void formatChanged(object sender, EventArgs e) { if ((_source != null) && (!_updating)) UpdatePreview(); }
        private void numLOD_ValueChanged(object sender, EventArgs e) { if ((_source != null) || (!_updating)) UpdateSize(); }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (_previewing = chkPreview.Checked)
                pictureBox1.Picture = _preview;
            else
                pictureBox1.Picture = _source;
        }
        private void chkImportPalette_CheckedChanged(object sender, EventArgs e) { FixImportPaletteDependentFields(); UpdatePreview(); }

        private void btnCancel_Click(object sender, EventArgs e) { Close(); }

        public void EncodeSource()
        {
            TextureConverter format = TextureConverter.Get((WiiPixelFormat)cboFormat.SelectedItem);
            if (format.IsIndexed)
            {
                if (_origTEX0 != null || _bresParent != null)
                    _textureData = format.EncodeTEX0TextureIndexed(_indexed, (int)numLOD.Value, (WiiPaletteFormat)cboPaletteFormat.SelectedItem, out _paletteData);
                else if (_origREFT != null || _reftParent != null)
                    _textureData = format.EncodeREFTTextureIndexed(_indexed, (int)numLOD.Value, (WiiPaletteFormat)cboPaletteFormat.SelectedItem);
                else if (_origTPL != null || _tplParent != null)
                    _textureData = format.EncodeTPLTextureIndexed(_indexed, (int)numLOD.Value, (WiiPaletteFormat)cboPaletteFormat.SelectedItem, out _paletteData);
            }
            else
            {
                if ((format.RawFormat == WiiPixelFormat.CMPR) && (_cmprBuffer != null))
                {
                    if (_origTEX0 != null || _bresParent != null)
                        _textureData = ((CMPR)format).EncodeTEX0TextureCached(_source, (int)numLOD.Value, _cmprBuffer);
                    else if (_origREFT != null || _reftParent != null)
                        _textureData = ((CMPR)format).EncodeREFTTextureCached(_source, (int)numLOD.Value, _cmprBuffer);
                    else if (_origTPL != null || _tplParent != null)
                        _textureData = ((CMPR)format).EncodeTPLTextureCached(_source, (int)numLOD.Value, _cmprBuffer);
                }
                else if (_origTEX0 != null || _bresParent != null)
                    _textureData = format.EncodeTEX0Texture(_source, (int)numLOD.Value);
                else if (_origREFT != null || _reftParent != null)
                    _textureData = format.EncodeREFTTexture(_source, (int)numLOD.Value, WiiPaletteFormat.IA8);
                else if (_origTPL != null || _tplParent != null)
                    _textureData = format.EncodeTPLTexture(_source, (int)numLOD.Value);
            }

            if (_bresParent != null)
            {
                _origTEX0 = _bresParent.CreateResource<TEX0Node>(Path.GetFileNameWithoutExtension(_imageSource));
                if (_paletteData != null)
                {
                    _origPLT0 = _bresParent.CreateResource<PLT0Node>(_origTEX0.Name);
                    _origPLT0.Name = _origTEX0.Name;
                    _origPLT0.ReplaceRaw(_paletteData);
                }
                _origTEX0.ReplaceRaw(_textureData);
            }
            else if (_tplParent != null)
            {
                _origTPL = new TPLTextureNode() { Name = "Texture" };
                _tplParent.AddChild(_origTPL);
                _origTPL.ReplaceRaw(_textureData);
                if (_paletteData != null)
                {
                    _origTPLPlt = new TPLPaletteNode() { Name = "Palette" };
                    _origTPL.AddChild(_origTPLPlt);
                    _origTPLPlt.ReplaceRaw(_paletteData);
                }
            }
            else if (_reftParent != null)
            {
                _reftParent.AddChild(_origREFT = new REFTEntryNode() { Name = Path.GetFileNameWithoutExtension(_imageSource) });
                _origREFT.ReplaceRaw(_textureData);
            }
            else if (_origTEX0 != null)
            {
                if (_origPLT0 != null)
                {
                    if (_paletteData != null)
                        _origPLT0.ReplaceRaw(_paletteData);
                    else
                    {
                        _origPLT0.Remove();
                        _origPLT0.Dispose();
                    }
                }
                else if (_paletteData != null)
                {
                    if ((_origTEX0.Parent == null) || (_origTEX0.Parent.Parent == null))
                    {
                        _paletteData.Dispose();
                        _paletteData = null;
                    }
                    else
                    {
                        _bresParent = _origTEX0.Parent.Parent as BRRESNode;
                        _origPLT0 = _bresParent.CreateResource<PLT0Node>(_origTEX0.Name);
                        _origPLT0.Name = _origTEX0.Name;
                        _origPLT0.ReplaceRaw(_paletteData);
                    }
                }
                _origTEX0.ReplaceRaw(_textureData);
            }
            else if (_origREFT != null)
                _origREFT.ReplaceRaw(_textureData);
            else if (_origTPL != null)
            {
                _origTPL.ReplaceRaw(_textureData);
                if (_origTPLPlt != null)
                {
                    if (_paletteData != null)
                    {
                        _origTPL.AddChild(_origTPLPlt);
                        _origTPLPlt.ReplaceRaw(_paletteData);
                    }
                    else
                    {
                        _origTPLPlt.Remove();
                        _origTPLPlt.Dispose();
                    }
                }
                else if (_paletteData != null)
                {
                    if (_origTPL.Parent == null)
                    {
                        _paletteData.Dispose();
                        _paletteData = null;
                    }
                    else
                    {
                        _origTPLPlt = new TPLPaletteNode() { _name = "Palette" };
                        _origTPL.AddChild(_origTPLPlt);
                        _origTPLPlt.ReplaceRaw(_paletteData);
                    }
                }
            }
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            EncodeSource();
            
            DialogResult = DialogResult.OK;
            Close();
        }

        #region Designer

        private CheckBox chkPreview;
        private GroupBox groupBox1;
        private Button btnRecommend;
        private NumericUpDown numLOD;
        private Label label5;
        private ComboBox cboFormat;
        private Label label4;
        private GroupBox groupBox2;
        private Label lblTransparencies;
        private Label lblColors;
        private Label lblSize;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnOkay;
        private Button btnCancel;
        private GroupBox grpPalette;
        private ComboBox cboAlgorithm;
        private Label label8;
        private NumericUpDown numPaletteCount;
        private Label label7;
        private ComboBox cboPaletteFormat;
        private Label label6;
        private GroupBox groupBox4;
        private GoodPictureBox pictureBox1;
        private Panel panel1;
        private Label lblDataSize;
        private TextBox txtPath;
        private Panel panel2;
        private Button button1;
        private Label label9;
        private OpenFileDialog dlgOpen;

        private void InitializeComponent()
        {
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numLOD = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRecommend = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTransparencies = new System.Windows.Forms.Label();
            this.lblDataSize = new System.Windows.Forms.Label();
            this.lblColors = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpPalette = new System.Windows.Forms.GroupBox();
            this.chkImportPalette = new System.Windows.Forms.CheckBox();
            this.cboAlgorithm = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numPaletteCount = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cboPaletteFormat = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkSwapRGB = new System.Windows.Forms.CheckBox();
            this.chkSwapAlpha = new System.Windows.Forms.CheckBox();
            this.chkConstrainProps = new System.Windows.Forms.CheckBox();
            this.btnApplyDims = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.numH = new System.Windows.Forms.NumericUpDown();
            this.numW = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numMIPPreview = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.GoodPictureBox();
            this.chkSwapAlphaRGB = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLOD)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpPalette.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPaletteCount)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMIPPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // chkPreview
            // 
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(9, 15);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(66, 21);
            this.chkPreview.TabIndex = 0;
            this.chkPreview.Text = "Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.numLOD);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboFormat);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(3, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 71);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // numLOD
            // 
            this.numLOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numLOD.Location = new System.Drawing.Point(75, 42);
            this.numLOD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLOD.Name = "numLOD";
            this.numLOD.Size = new System.Drawing.Size(98, 20);
            this.numLOD.TabIndex = 3;
            this.numLOD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLOD.ValueChanged += new System.EventHandler(this.numLOD_ValueChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "MIP Levels:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboFormat
            // 
            this.cboFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Location = new System.Drawing.Point(75, 15);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(98, 21);
            this.cboFormat.TabIndex = 1;
            this.cboFormat.SelectedIndexChanged += new System.EventHandler(this.cboFormat_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Format:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRecommend
            // 
            this.btnRecommend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecommend.Location = new System.Drawing.Point(75, 14);
            this.btnRecommend.Name = "btnRecommend";
            this.btnRecommend.Size = new System.Drawing.Size(98, 21);
            this.btnRecommend.TabIndex = 1;
            this.btnRecommend.Text = "Recommend";
            this.btnRecommend.UseVisualStyleBackColor = true;
            this.btnRecommend.Click += new System.EventHandler(this.btnRecommend_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lblTransparencies);
            this.groupBox2.Controls.Add(this.lblDataSize);
            this.groupBox2.Controls.Add(this.lblColors);
            this.groupBox2.Controls.Add(this.lblSize);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 99);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 20);
            this.label9.TabIndex = 6;
            this.label9.Text = "Data Size:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTransparencies
            // 
            this.lblTransparencies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransparencies.Location = new System.Drawing.Point(97, 51);
            this.lblTransparencies.Name = "lblTransparencies";
            this.lblTransparencies.Size = new System.Drawing.Size(76, 20);
            this.lblTransparencies.TabIndex = 5;
            this.lblTransparencies.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDataSize
            // 
            this.lblDataSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataSize.Location = new System.Drawing.Point(97, 71);
            this.lblDataSize.Name = "lblDataSize";
            this.lblDataSize.Size = new System.Drawing.Size(76, 20);
            this.lblDataSize.TabIndex = 7;
            this.lblDataSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColors
            // 
            this.lblColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColors.Location = new System.Drawing.Point(97, 31);
            this.lblColors.Name = "lblColors";
            this.lblColors.Size = new System.Drawing.Size(76, 20);
            this.lblColors.TabIndex = 3;
            this.lblColors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSize
            // 
            this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSize.Location = new System.Drawing.Point(94, 11);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(79, 20);
            this.lblSize.TabIndex = 1;
            this.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Transparent:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Colors:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Size:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOkay.Location = new System.Drawing.Point(8, 157);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(80, 23);
            this.btnOkay.TabIndex = 11;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(94, 157);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpPalette
            // 
            this.grpPalette.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPalette.Controls.Add(this.chkImportPalette);
            this.grpPalette.Controls.Add(this.cboAlgorithm);
            this.grpPalette.Controls.Add(this.label8);
            this.grpPalette.Controls.Add(this.numPaletteCount);
            this.grpPalette.Controls.Add(this.label7);
            this.grpPalette.Controls.Add(this.cboPaletteFormat);
            this.grpPalette.Controls.Add(this.label6);
            this.grpPalette.Location = new System.Drawing.Point(3, 185);
            this.grpPalette.Name = "grpPalette";
            this.grpPalette.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grpPalette.Size = new System.Drawing.Size(179, 118);
            this.grpPalette.TabIndex = 5;
            this.grpPalette.TabStop = false;
            this.grpPalette.Text = "Palette";
            // 
            // chkImportPalette
            // 
            this.chkImportPalette.Location = new System.Drawing.Point(9, 95);
            this.chkImportPalette.Name = "chkImportPalette";
            this.chkImportPalette.Size = new System.Drawing.Size(164, 17);
            this.chkImportPalette.TabIndex = 6;
            this.chkImportPalette.Text = "Import Palette";
            this.chkImportPalette.UseVisualStyleBackColor = true;
            this.chkImportPalette.CheckedChanged += new System.EventHandler(this.chkImportPalette_CheckedChanged);
            // 
            // cboAlgorithm
            // 
            this.cboAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlgorithm.FormattingEnabled = true;
            this.cboAlgorithm.Location = new System.Drawing.Point(75, 68);
            this.cboAlgorithm.Name = "cboAlgorithm";
            this.cboAlgorithm.Size = new System.Drawing.Size(98, 21);
            this.cboAlgorithm.TabIndex = 5;
            this.cboAlgorithm.SelectedIndexChanged += new System.EventHandler(this.formatChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 20);
            this.label8.TabIndex = 4;
            this.label8.Text = "Algorithm:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPaletteCount
            // 
            this.numPaletteCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numPaletteCount.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPaletteCount.Location = new System.Drawing.Point(75, 42);
            this.numPaletteCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numPaletteCount.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPaletteCount.Name = "numPaletteCount";
            this.numPaletteCount.Size = new System.Drawing.Size(97, 20);
            this.numPaletteCount.TabIndex = 3;
            this.numPaletteCount.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPaletteCount.ValueChanged += new System.EventHandler(this.formatChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 20);
            this.label7.TabIndex = 2;
            this.label7.Text = "Colors:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboPaletteFormat
            // 
            this.cboPaletteFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPaletteFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPaletteFormat.FormattingEnabled = true;
            this.cboPaletteFormat.Location = new System.Drawing.Point(75, 15);
            this.cboPaletteFormat.Name = "cboPaletteFormat";
            this.cboPaletteFormat.Size = new System.Drawing.Size(98, 21);
            this.cboPaletteFormat.TabIndex = 1;
            this.cboPaletteFormat.SelectedIndexChanged += new System.EventHandler(this.formatChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Format:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chkSwapAlphaRGB);
            this.groupBox4.Controls.Add(this.chkSwapRGB);
            this.groupBox4.Controls.Add(this.chkSwapAlpha);
            this.groupBox4.Controls.Add(this.chkConstrainProps);
            this.groupBox4.Controls.Add(this.btnApplyDims);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.numH);
            this.groupBox4.Controls.Add(this.numW);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.btnOkay);
            this.groupBox4.Controls.Add(this.btnRecommend);
            this.groupBox4.Controls.Add(this.chkPreview);
            this.groupBox4.Controls.Add(this.btnCancel);
            this.groupBox4.Location = new System.Drawing.Point(3, 309);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 189);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            // 
            // chkSwapRGB
            // 
            this.chkSwapRGB.AutoSize = true;
            this.chkSwapRGB.Location = new System.Drawing.Point(9, 110);
            this.chkSwapRGB.Name = "chkSwapRGB";
            this.chkSwapRGB.Size = new System.Drawing.Size(79, 17);
            this.chkSwapRGB.TabIndex = 8;
            this.chkSwapRGB.Text = "Swap RGB";
            this.chkSwapRGB.UseVisualStyleBackColor = true;
            this.chkSwapRGB.CheckedChanged += new System.EventHandler(this.chkSwapRGB_CheckedChanged);
            // 
            // chkSwapAlpha
            // 
            this.chkSwapAlpha.AutoSize = true;
            this.chkSwapAlpha.Location = new System.Drawing.Point(94, 110);
            this.chkSwapAlpha.Name = "chkSwapAlpha";
            this.chkSwapAlpha.Size = new System.Drawing.Size(83, 17);
            this.chkSwapAlpha.TabIndex = 9;
            this.chkSwapAlpha.Text = "Swap Alpha";
            this.chkSwapAlpha.UseVisualStyleBackColor = true;
            this.chkSwapAlpha.CheckedChanged += new System.EventHandler(this.chkSwapAlpha_CheckedChanged);
            // 
            // chkConstrainProps
            // 
            this.chkConstrainProps.AutoSize = true;
            this.chkConstrainProps.Checked = true;
            this.chkConstrainProps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConstrainProps.Location = new System.Drawing.Point(9, 87);
            this.chkConstrainProps.Name = "chkConstrainProps";
            this.chkConstrainProps.Size = new System.Drawing.Size(126, 17);
            this.chkConstrainProps.TabIndex = 7;
            this.chkConstrainProps.Text = "Constrain Proportions";
            this.chkConstrainProps.UseVisualStyleBackColor = true;
            this.chkConstrainProps.CheckedChanged += new System.EventHandler(this.chkConstrainProps_CheckedChanged);
            // 
            // btnApplyDims
            // 
            this.btnApplyDims.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyDims.Location = new System.Drawing.Point(125, 60);
            this.btnApplyDims.Name = "btnApplyDims";
            this.btnApplyDims.Size = new System.Drawing.Size(48, 21);
            this.btnApplyDims.TabIndex = 6;
            this.btnApplyDims.Text = "Apply";
            this.btnApplyDims.UseVisualStyleBackColor = true;
            this.btnApplyDims.Click += new System.EventHandler(this.btnApplyDims_Click);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(58, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 21);
            this.label11.TabIndex = 4;
            this.label11.Text = "X";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numH
            // 
            this.numH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numH.Location = new System.Drawing.Point(73, 60);
            this.numH.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numH.Name = "numH";
            this.numH.Size = new System.Drawing.Size(46, 20);
            this.numH.TabIndex = 5;
            this.numH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numH.ValueChanged += new System.EventHandler(this.numH_ValueChanged);
            // 
            // numW
            // 
            this.numW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numW.Location = new System.Drawing.Point(9, 61);
            this.numW.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numW.Name = "numW";
            this.numW.Size = new System.Drawing.Size(46, 20);
            this.numW.TabIndex = 3;
            this.numW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numW.ValueChanged += new System.EventHandler(this.numW_ValueChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 20);
            this.label10.TabIndex = 2;
            this.label10.Text = "Dimensions:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.grpPalette);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(379, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 501);
            this.panel1.TabIndex = 9;
            // 
            // txtPath
            // 
            this.txtPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPath.Location = new System.Drawing.Point(92, 0);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(212, 20);
            this.txtPath.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtPath);
            this.panel2.Controls.Add(this.numMIPPreview);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(379, 20);
            this.panel2.TabIndex = 1;
            // 
            // numMIPPreview
            // 
            this.numMIPPreview.Dock = System.Windows.Forms.DockStyle.Left;
            this.numMIPPreview.Location = new System.Drawing.Point(41, 0);
            this.numMIPPreview.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMIPPreview.Name = "numMIPPreview";
            this.numMIPPreview.Size = new System.Drawing.Size(51, 20);
            this.numMIPPreview.TabIndex = 1;
            this.numMIPPreview.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMIPPreview.Visible = false;
            this.numMIPPreview.ValueChanged += new System.EventHandler(this.numMIPPreview_ValueChanged);
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Left;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 20);
            this.label12.TabIndex = 0;
            this.label12.Text = "MIP:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label12.Visible = false;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(304, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 3;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Picture = null;
            this.pictureBox1.Size = new System.Drawing.Size(379, 481);
            this.pictureBox1.TabIndex = 0;
            // 
            // chkSwapAlphaRGB
            // 
            this.chkSwapAlphaRGB.AutoSize = true;
            this.chkSwapAlphaRGB.Location = new System.Drawing.Point(9, 133);
            this.chkSwapAlphaRGB.Name = "chkSwapAlphaRGB";
            this.chkSwapAlphaRGB.Size = new System.Drawing.Size(131, 17);
            this.chkSwapAlphaRGB.TabIndex = 10;
            this.chkSwapAlphaRGB.Text = "Swap Alpha with RGB";
            this.chkSwapAlphaRGB.UseVisualStyleBackColor = true;
            this.chkSwapAlphaRGB.CheckedChanged += new System.EventHandler(this.chkSwapAlphaRGB_CheckedChanged);
            // 
            // TextureConverterDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(564, 501);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(0, 470);
            this.Name = "TextureConverterDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Texture Converter";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLOD)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.grpPalette.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPaletteCount)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMIPPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private void btnApplyDims_Click(object sender, EventArgs e)
        {
            int w = (int)Math.Round(numW.Value, 0);
            int h = (int)Math.Round(numH.Value, 0);
            ResizeImage(w, h);
        }

        public event Action<int, int> Resized;

        public void ResizeImage(int w, int h)
        {
            _updating = true;
            chkSwapRGB.Checked = chkSwapAlpha.Checked = false;
            if (w == _base.Width && h == _base.Height)
                Source = _base;
            else
                Source = _base.Resize(w, h);
            FixImportPaletteFields();
            UpdatePreview();

            if (Resized != null)
                Resized(w, h);

            _updating = false;
        }

        private void chkSwapRGB_CheckedChanged(object sender, EventArgs e) {
            if ((_source == null) || (_updating))
                return;

            _updating = true;

            Source = Source.InvertColors();

            FixImportPaletteFields();
            UpdatePreview();

            _updating = false;
        }

        private void chkSwapAlpha_CheckedChanged(object sender, EventArgs e) {
            if ((_source == null) || (_updating))
                return;

            _updating = true;

            Source = Source.InvertAlpha();

            FixImportPaletteFields();
            UpdatePreview();

            _updating = false;
        }

        private void chkSwapAlphaRGB_CheckedChanged(object sender, EventArgs e) {
            if ((_source == null) || (_updating))
                return;

            _updating = true;

            try {
                Source = Source.SwapAlphaAndRGB();
            } catch (BitmapExtension.NonMonochromeImageException ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            FixImportPaletteFields();
            UpdatePreview();

            _updating = false;
        }

        private void chkConstrainProps_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConstrainProps.Checked)
            {
                numH.Value = _base.Height;
                numW.Value = _base.Width;
            }
        }

        private void numW_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            if (chkConstrainProps.Checked)
            {
                _updating = true;
                numH.Value = numW.Value / (decimal)_base.Width * (decimal)_base.Height;
                _updating = false;
            }
        }

        private void numH_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            if (chkConstrainProps.Checked)
            {
                _updating = true;
                numW.Value = numH.Value / (decimal)_base.Height * (decimal)_base.Width;
                _updating = false;
            }
        }

        private void numMIPPreview_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _updating = true;
            numMIPPreview.Value = ((int)numMIPPreview.Value).Clamp(1, (int)numLOD.Value);
            _updating = false;
        }
    }
}

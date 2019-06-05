using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BrawlLib;
using BrawlLib.Imaging;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace System.Windows.Forms
{
    public class TextureConverterDialog : Form
    {
        private Bitmap _base, _source, _preview, _indexed;

        private UnsafeBuffer _cmprBuffer;
        private ColorInformation _colorInfo;

        private FileMap _paletteData;

        private GCHandle? _pixelData;

        //private ColorPalette _tempPalette;
        private bool _previewing = true, _updating;

        private Button btnApplyDims;
        private CheckBox chkConstrainProps;
        private CheckBox chkImportPalette;
        private CheckBox chkSwapAlpha;
        private CheckBox chkSwapAlphaRGB;
        private CheckBox chkSwapRGB;

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WiiPixelFormat? InitialFormat;

        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Drawing.Size? InitialSize;

        private Label label10;
        private Label label11;
        private Label label12;
        private NumericUpDown numH;
        private NumericUpDown numMIPPreview;
        private NumericUpDown numW;

        public TextureConverterDialog()
        {
            InitializeComponent();

            numH.Maximum = numW.Maximum = decimal.MaxValue;

            dlgOpen.Filter = FileFilters.Images;

            foreach (WiiPixelFormat f in Enum.GetValues(typeof(WiiPixelFormat))) cboFormat.Items.Add(f);

            foreach (WiiPaletteFormat f in Enum.GetValues(typeof(WiiPaletteFormat))) cboPaletteFormat.Items.Add(f);

            foreach (QuantizationAlgorithm f in Enum.GetValues(typeof(QuantizationAlgorithm)))
                cboAlgorithm.Items.Add(f);

            cboAlgorithm.SelectedItem = QuantizationAlgorithm.MedianCut;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bitmap Source
        {
            get => _source;
            set
            {
                _source = value;
                SourceChanged();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImageSource { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BRRESNode BRESParentNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TPLNode TPLParentNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public REFTNode REFTParentNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TEX0Node TEX0TextureNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TPLTextureNode TPLTextureNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public REFTEntryNode REFTTextureNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PLT0Node PLT0PaletteNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TPLPaletteNode TPLPaletteNode { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileMap TextureData { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileMap PaletteData => _paletteData;

        public DialogResult ShowDialog(IWin32Window owner, BRRESNode parent)
        {
            BRESParentNode = parent;
            TEX0TextureNode = null;
            REFTTextureNode = null;
            PLT0PaletteNode = null;
            TPLTextureNode = null;
            TPLPaletteNode = null;
            _paletteData = TextureData = null;
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally
            {
                DisposeImages();
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, REFTNode parent)
        {
            BRESParentNode = null;
            REFTParentNode = parent;
            TEX0TextureNode = null;
            REFTTextureNode = null;
            PLT0PaletteNode = null;
            TPLTextureNode = null;
            TPLPaletteNode = null;
            _paletteData = TextureData = null;
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally
            {
                DisposeImages();
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, TPLNode parent)
        {
            BRESParentNode = null;
            REFTParentNode = null;
            TPLParentNode = parent;
            TEX0TextureNode = null;
            REFTTextureNode = null;
            PLT0PaletteNode = null;
            TPLTextureNode = null;
            TPLPaletteNode = null;
            _paletteData = TextureData = null;
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally
            {
                DisposeImages();
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, TEX0Node original)
        {
            BRESParentNode = null;
            TEX0TextureNode = original;
            PLT0PaletteNode = original.GetPaletteNode();
            REFTTextureNode = null;
            TPLTextureNode = null;
            TPLPaletteNode = null;
            _paletteData = TextureData = null;
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally
            {
                DisposeImages();
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, REFTEntryNode original)
        {
            BRESParentNode = null;
            REFTTextureNode = original;
            TEX0TextureNode = null;
            TPLTextureNode = null;
            TPLPaletteNode = null;
            PLT0PaletteNode = null;
            _paletteData = TextureData = null;
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally
            {
                DisposeImages();
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, TPLTextureNode original)
        {
            BRESParentNode = null;
            REFTTextureNode = null;
            TPLTextureNode = original;
            TPLPaletteNode = original.GetPaletteNode();
            TEX0TextureNode = null;
            PLT0PaletteNode = null;
            _paletteData = TextureData = null;
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally
            {
                DisposeImages();
            }
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            BRESParentNode = null;
            TEX0TextureNode = null;
            PLT0PaletteNode = null;
            REFTTextureNode = null;
            TPLTextureNode = null;
            TPLPaletteNode = null;
            _paletteData = TextureData = null;
            DialogResult = DialogResult.Cancel;
            try
            {
                return base.ShowDialog(owner);
            }
            //catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally
            {
                DisposeImages();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_base == null)
            {
                if (ImageSource == null)
                {
                    if (!LoadImages())
                    {
                        Close();
                        return;
                    }
                }
                else if (!LoadImages(ImageSource))
                {
                    Close();
                    return;
                }
            }

            if (TEX0TextureNode != null)
            {
                _updating = true;

                cboFormat.SelectedItem = TEX0TextureNode.Format;
                numLOD.Value = TEX0TextureNode.LevelOfDetail;

                FixPaletteFields();

                if (PLT0PaletteNode != null)
                {
                    grpPalette.Enabled = true;
                    cboPaletteFormat.SelectedItem = PLT0PaletteNode.Format;
                    numPaletteCount.Value = PLT0PaletteNode.Colors;
                }

                _updating = false;
                UpdatePreview();
            }
            else if (REFTTextureNode != null)
            {
                _updating = true;
                cboFormat.SelectedItem = REFTTextureNode.TextureFormat;
                numLOD.Value = REFTTextureNode.LevelOfDetail.Clamp((int) numLOD.Minimum, (int) numLOD.Maximum);

                FixPaletteFields();

                _updating = false;
                UpdatePreview();
            }
            else if (TPLTextureNode != null)
            {
                _updating = true;
                cboFormat.SelectedItem = TPLTextureNode.Format;
                numLOD.Value = 1;
                //numLOD.Enabled = false;

                FixPaletteFields();

                if (TPLPaletteNode != null)
                {
                    grpPalette.Enabled = true;
                    cboPaletteFormat.SelectedItem = TPLPaletteNode.Format;
                    numPaletteCount.Value = TPLPaletteNode.Colors;
                }

                _updating = false;
                UpdatePreview();
            }
            else
            {
                Recommend();
            }

            if (InitialSize != null)
            {
                chkConstrainProps.Checked = false;
                numW.Value = InitialSize.Value.Width;
                numH.Value = InitialSize.Value.Height;
                btnApplyDims.PerformClick();
            }

            if (InitialFormat != null) cboFormat.SelectedItem = InitialFormat;
        }

        public bool LoadImages(string path)
        {
            txtPath.Text = path;
            if (path.EndsWith(".tga", StringComparison.OrdinalIgnoreCase))
                return LoadImages(TGA.FromFile(path));
            if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                return LoadImagesPreservingPaletteInfo(path);
            return LoadImages((Bitmap) Image.FromFile(path));
        }

        public bool LoadImages()
        {
            if (dlgOpen.ShowDialog(this) != DialogResult.OK) return false;

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
            Stream sourceStream = new FileStream(ImageSource, FileMode.Open, FileAccess.Read, FileShare.Read);
            var decoder = new PngBitmapDecoder(sourceStream, BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            BitmapSource preservedImage = decoder.Frames[0];
            if (preservedImage.Format == PixelFormats.Indexed8)
            {
                Bitmap bmp;
                var width = Convert.ToInt32(preservedImage.Width);
                var height = Convert.ToInt32(preservedImage.Height);
                var pixels = new byte[width * height];
                preservedImage.CopyPixels(pixels, width, 0);
                var pixelData = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                bmp = new Bitmap(width, height, width, PixelFormat.Format8bppIndexed, pixelData.AddrOfPinnedObject());

                var preservedColors = preservedImage.Palette.Colors;
                var newPalette = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.None, preservedColors.Count);
                for (var i = 0; i < preservedColors.Count; i++)
                    newPalette.Entries[i] = Color.FromArgb(preservedColors[i].A, preservedColors[i].R,
                        preservedColors[i].G, preservedColors[i].B);

                bmp.Palette = newPalette;
                return LoadImages(bmp, pixelData);
            }

            return LoadImages((Bitmap) Image.FromFile(path));
        }

        private Bitmap ImportPalette()
        {
            var pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.None,
                _source.Palette.Entries.Length.Align(16));
            for (var i = 0; i < _source.Palette.Entries.Length; i++) pal.Entries[i] = _source.Palette.Entries[i];

            for (var i = _source.Palette.Entries.Length; i < pal.Entries.Length; i++)
                pal.Entries[i] = Color.FromArgb(0);

            pal.Clamp((WiiPaletteFormat) cboPaletteFormat.SelectedItem);

            var bmp = (Bitmap) _source.Clone();
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
            lblSize.Text = string.Format("{0} x {1}", _source.Width, _source.Height);

            _colorInfo = _source.GetColorInformation();
            lblColors.Text = _colorInfo.ColorCount.ToString();
            lblTransparencies.Text = _colorInfo.AlphaColors.ToString();

            //Get max LOD
            var maxLOD = 1;
            for (int w = _source.Width, h = _source.Height; w != 1 && h != 1; w >>= 1, h >>= 1, maxLOD++) ;

            numLOD.Maximum = maxLOD;
            numMIPPreview.Maximum = maxLOD;

            if (_updating) return;

            numW.Value = _source.Width;
            numH.Value = _source.Height;
        }

        private void DisposeImages()
        {
            pictureBox1.Picture = null;
            if (_base != null)
            {
                _base.Dispose();
                _base = null;
            }

            if (_preview != null)
            {
                _preview.Dispose();
                _preview = null;
            }

            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }

            if (_indexed != null)
            {
                _indexed.Dispose();
                _indexed = null;
            }

            if (_pixelData.HasValue)
            {
                _pixelData.Value.Free();
                _pixelData = null;
            }
        }

        private void CopyPreview(Bitmap src)
        {
            var r = new Rectangle(0, 0, src.Width, src.Height);
            var srcData = src.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var dstData = _preview.LockBits(r, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Memory.Move(dstData.Scan0, srcData.Scan0, (uint) (srcData.Stride * src.Height));

            _preview.UnlockBits(dstData);
            src.UnlockBits(srcData);
        }

        internal void UpdatePreview()
        {
            if (_source == null) return;

            if (_cmprBuffer != null)
            {
                _cmprBuffer.Dispose();
                _cmprBuffer = null;
            }

            if (_indexed != null)
            {
                _indexed.Dispose();
                _indexed = null;
            }

            var format = (WiiPixelFormat) cboFormat.SelectedItem;
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
                        _indexed = _source.Quantize((QuantizationAlgorithm) cboAlgorithm.SelectedItem,
                            (int) numPaletteCount.Value, format, (WiiPaletteFormat) cboPaletteFormat.SelectedItem,
                            null);

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
            if (_source == null) return;

            int w = _source.Width, h = _source.Height;
            if (TEX0TextureNode != null || BRESParentNode != null)
            {
                var palSize = PaletteSize(0x40);
                lblDataSize.Text = string.Format("{0:n0}B",
                    TextureConverter.Get((WiiPixelFormat) cboFormat.SelectedItem)
                        .GetMipOffset(ref w, ref h, (int) numLOD.Value + 1) + 0x40 + palSize);
            }
            else if (REFTTextureNode != null || REFTParentNode != null)
            {
                var palSize = PaletteSize(0);
                lblDataSize.Text = string.Format("{0:n0}B",
                    TextureConverter.Get((WiiPixelFormat) cboFormat.SelectedItem)
                        .GetMipOffset(ref w, ref h, (int) numLOD.Value + 1) + 0x20 + palSize);
            }
            else if (TPLTextureNode != null || TPLParentNode != null)
            {
                var palSize = PaletteSize(0xC);
                lblDataSize.Text = string.Format("{0:n0}B",
                    TextureConverter.Get((WiiPixelFormat) cboFormat.SelectedItem)
                        .GetMipOffset(ref w, ref h, (int) numLOD.Value + 1) + 0x28 + palSize);
            }
        }

        private int PaletteSize(int formatOverhead)
        {
            if (!grpPalette.Enabled) return 0;

            if (chkImportPalette.Enabled && chkImportPalette.Checked)
                return _source.Palette.Entries.Length.Align(16) * 2 + formatOverhead;

            return (int) numPaletteCount.Value * 2 + formatOverhead;
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
            if (_source == null || _updating) return;

            _updating = true;
            if (_colorInfo.IsGreyscale)
            {
                if (_colorInfo.ColorCount <= 16)
                    cboFormat.SelectedItem = _colorInfo.AlphaColors == 0 ? WiiPixelFormat.I4 : WiiPixelFormat.CI4;
                else if (_colorInfo.ColorCount <= 272)
                    cboFormat.SelectedItem = _colorInfo.AlphaColors == 0 ? WiiPixelFormat.I8 : WiiPixelFormat.IA8;
                else
                    cboFormat.SelectedItem =
                        _colorInfo.AlphaColors == 0 ? WiiPixelFormat.RGB565 : WiiPixelFormat.RGB5A3;
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

            if (Source.GuessIfAlphaInverted())
            {
                chkSwapAlpha.Checked = true;
                Source = Source.InvertAlpha();
            }

            _updating = false;
            UpdatePreview();
        }

        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_source == null || _updating) return;

            _updating = true;

            FixPaletteFields();

            _updating = false;
            UpdatePreview();
        }

        private void FixPaletteFields()
        {
            switch ((WiiPixelFormat) cboFormat.SelectedItem)
            {
                case WiiPixelFormat.I4:
                case WiiPixelFormat.I8:
                case WiiPixelFormat.IA4:
                case WiiPixelFormat.IA8:
                case WiiPixelFormat.RGB565:
                case WiiPixelFormat.RGB5A3:
                case WiiPixelFormat.RGBA8:
                case WiiPixelFormat.CMPR:
                    grpPalette.Enabled = false;
                    break;

                case WiiPixelFormat.CI4:
                {
                    grpPalette.Enabled = true;
                    numPaletteCount.Maximum = 16;
                    numPaletteCount.Value = 16;
                    cboPaletteFormat.SelectedItem =
                        _colorInfo.AlphaColors == 0 ? WiiPaletteFormat.RGB565 : WiiPaletteFormat.RGB5A3;
                    FixImportPaletteFields();
                    break;
                }

                case WiiPixelFormat.CI8:
                {
                    grpPalette.Enabled = true;
                    numPaletteCount.Maximum = 256;
                    numPaletteCount.Value = Math.Min(256, _colorInfo.ColorCount.Align(16));
                    cboPaletteFormat.SelectedItem =
                        _colorInfo.AlphaColors == 0 ? WiiPaletteFormat.RGB565 : WiiPaletteFormat.RGB5A3;
                    var sourcePaletteSize = _source.Palette.Entries.Length;
                    if (sourcePaletteSize > 0 && sourcePaletteSize <= 256)
                        numPaletteCount.Value = sourcePaletteSize.Align(16);
                    FixImportPaletteFields();
                    break;
                }
            }
        }

        private void FixImportPaletteFields()
        {
            if ((WiiPixelFormat) cboFormat.SelectedItem == WiiPixelFormat.CI8 &&
                _source.PixelFormat == PixelFormat.Format8bppIndexed ||
                (WiiPixelFormat) cboFormat.SelectedItem == WiiPixelFormat.CI4 &&
                _source.PixelFormat == PixelFormat.Format4bppIndexed)
                // Checks if the image is not being resized
                if (_source == _base)
                {
                    chkImportPalette.Enabled = true;
                    FixImportPaletteDependentFields();
                    return;
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

        private void formatChanged(object sender, EventArgs e)
        {
            if (_source != null && !_updating) UpdatePreview();
        }

        private void numLOD_ValueChanged(object sender, EventArgs e)
        {
            if (_source != null || !_updating) UpdateSize();
        }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (_previewing = chkPreview.Checked)
                pictureBox1.Picture = _preview;
            else
                pictureBox1.Picture = _source;
        }

        private void chkImportPalette_CheckedChanged(object sender, EventArgs e)
        {
            FixImportPaletteDependentFields();
            UpdatePreview();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void EncodeSource()
        {
            var format = TextureConverter.Get((WiiPixelFormat) cboFormat.SelectedItem);
            if (format.IsIndexed)
            {
                if (TEX0TextureNode != null || BRESParentNode != null)
                    TextureData = format.EncodeTEX0TextureIndexed(_indexed, (int) numLOD.Value,
                        (WiiPaletteFormat) cboPaletteFormat.SelectedItem, out _paletteData);
                else if (REFTTextureNode != null || REFTParentNode != null)
                    TextureData = format.EncodeREFTTextureIndexed(_indexed, (int) numLOD.Value,
                        (WiiPaletteFormat) cboPaletteFormat.SelectedItem);
                else if (TPLTextureNode != null || TPLParentNode != null)
                    TextureData = format.EncodeTPLTextureIndexed(_indexed, (int) numLOD.Value,
                        (WiiPaletteFormat) cboPaletteFormat.SelectedItem, out _paletteData);
            }
            else
            {
                if (format.RawFormat == WiiPixelFormat.CMPR && _cmprBuffer != null)
                {
                    if (TEX0TextureNode != null || BRESParentNode != null)
                        TextureData = ((CMPR) format).EncodeTEX0TextureCached(_source, (int) numLOD.Value, _cmprBuffer);
                    else if (REFTTextureNode != null || REFTParentNode != null)
                        TextureData = ((CMPR) format).EncodeREFTTextureCached(_source, (int) numLOD.Value, _cmprBuffer);
                    else if (TPLTextureNode != null || TPLParentNode != null)
                        TextureData = ((CMPR) format).EncodeTPLTextureCached(_source, (int) numLOD.Value, _cmprBuffer);
                }
                else if (TEX0TextureNode != null || BRESParentNode != null)
                {
                    TextureData = format.EncodeTEX0Texture(_source, (int) numLOD.Value);
                }
                else if (REFTTextureNode != null || REFTParentNode != null)
                {
                    TextureData = format.EncodeREFTTexture(_source, (int) numLOD.Value, WiiPaletteFormat.IA8);
                }
                else if (TPLTextureNode != null || TPLParentNode != null)
                {
                    TextureData = format.EncodeTPLTexture(_source, (int) numLOD.Value);
                }
            }

            if (BRESParentNode != null)
            {
                TEX0TextureNode =
                    BRESParentNode.CreateResource<TEX0Node>(Path.GetFileNameWithoutExtension(ImageSource));
                if (_paletteData != null)
                {
                    PLT0PaletteNode = BRESParentNode.CreateResource<PLT0Node>(TEX0TextureNode.Name);
                    PLT0PaletteNode.Name = TEX0TextureNode.Name;
                    PLT0PaletteNode.ReplaceRaw(_paletteData);
                }

                TEX0TextureNode.ReplaceRaw(TextureData);
            }
            else if (TPLParentNode != null)
            {
                TPLTextureNode = new TPLTextureNode {Name = "Texture"};
                TPLParentNode.AddChild(TPLTextureNode);
                TPLTextureNode.ReplaceRaw(TextureData);
                if (_paletteData != null)
                {
                    TPLPaletteNode = new TPLPaletteNode {Name = "Palette"};
                    TPLTextureNode.AddChild(TPLPaletteNode);
                    TPLPaletteNode.ReplaceRaw(_paletteData);
                }
            }
            else if (REFTParentNode != null)
            {
                REFTParentNode.AddChild(REFTTextureNode = new REFTEntryNode
                    {Name = Path.GetFileNameWithoutExtension(ImageSource)});
                REFTTextureNode.ReplaceRaw(TextureData);
            }
            else if (TEX0TextureNode != null)
            {
                if (PLT0PaletteNode != null)
                {
                    if (_paletteData != null)
                    {
                        PLT0PaletteNode.ReplaceRaw(_paletteData);
                    }
                    else
                    {
                        PLT0PaletteNode.Remove();
                        PLT0PaletteNode.Dispose();
                    }
                }
                else if (_paletteData != null)
                {
                    if (TEX0TextureNode.Parent == null || TEX0TextureNode.Parent.Parent == null)
                    {
                        _paletteData.Dispose();
                        _paletteData = null;
                    }
                    else
                    {
                        BRESParentNode = TEX0TextureNode.Parent.Parent as BRRESNode;
                        PLT0PaletteNode = BRESParentNode.CreateResource<PLT0Node>(TEX0TextureNode.Name);
                        PLT0PaletteNode.Name = TEX0TextureNode.Name;
                        PLT0PaletteNode.ReplaceRaw(_paletteData);
                    }
                }

                TEX0TextureNode.ReplaceRaw(TextureData);
            }
            else if (REFTTextureNode != null)
            {
                REFTTextureNode.ReplaceRaw(TextureData);
            }
            else if (TPLTextureNode != null)
            {
                TPLTextureNode.ReplaceRaw(TextureData);
                if (TPLPaletteNode != null)
                {
                    if (_paletteData != null)
                    {
                        TPLTextureNode.AddChild(TPLPaletteNode);
                        TPLPaletteNode.ReplaceRaw(_paletteData);
                    }
                    else
                    {
                        TPLPaletteNode.Remove();
                        TPLPaletteNode.Dispose();
                    }
                }
                else if (_paletteData != null)
                {
                    if (TPLTextureNode.Parent == null)
                    {
                        _paletteData.Dispose();
                        _paletteData = null;
                    }
                    else
                    {
                        TPLPaletteNode = new TPLPaletteNode {_name = "Palette"};
                        TPLTextureNode.AddChild(TPLPaletteNode);
                        TPLPaletteNode.ReplaceRaw(_paletteData);
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

        private void btnApplyDims_Click(object sender, EventArgs e)
        {
            var w = (int) Math.Round(numW.Value, 0);
            var h = (int) Math.Round(numH.Value, 0);
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

            Resized?.Invoke(w, h);

            _updating = false;
        }

        private void chkSwapRGB_CheckedChanged(object sender, EventArgs e)
        {
            if (_source == null || _updating) return;

            _updating = true;

            Source = Source.InvertColors();

            FixImportPaletteFields();
            UpdatePreview();

            _updating = false;
        }

        private void chkSwapAlpha_CheckedChanged(object sender, EventArgs e)
        {
            if (_source == null || _updating) return;

            _updating = true;

            Source = Source.InvertAlpha();

            FixImportPaletteFields();
            UpdatePreview();

            _updating = false;
        }

        private void chkSwapAlphaRGB_CheckedChanged(object sender, EventArgs e)
        {
            if (_source == null || _updating) return;

            _updating = true;

            try
            {
                Source = Source.SwapAlphaAndRGB();
            }
            catch (BitmapExtension.NonMonochromeImageException ex)
            {
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
            if (_updating) return;

            if (chkConstrainProps.Checked)
            {
                _updating = true;
                numH.Value = numW.Value / _base.Width * _base.Height;
                _updating = false;
            }
        }

        private void numH_ValueChanged(object sender, EventArgs e)
        {
            if (_updating) return;

            if (chkConstrainProps.Checked)
            {
                _updating = true;
                numW.Value = numH.Value / _base.Height * _base.Width;
                _updating = false;
            }
        }

        private void numMIPPreview_ValueChanged(object sender, EventArgs e)
        {
            if (_updating) return;

            _updating = true;
            numMIPPreview.Value = ((int) numMIPPreview.Value).Clamp(1, (int) numLOD.Value);
            _updating = false;
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
            chkPreview = new CheckBox();
            groupBox1 = new GroupBox();
            numLOD = new NumericUpDown();
            label5 = new Label();
            cboFormat = new ComboBox();
            label4 = new Label();
            btnRecommend = new Button();
            groupBox2 = new GroupBox();
            label9 = new Label();
            lblTransparencies = new Label();
            lblDataSize = new Label();
            lblColors = new Label();
            lblSize = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            btnOkay = new Button();
            btnCancel = new Button();
            grpPalette = new GroupBox();
            chkImportPalette = new CheckBox();
            cboAlgorithm = new ComboBox();
            label8 = new Label();
            numPaletteCount = new NumericUpDown();
            label7 = new Label();
            cboPaletteFormat = new ComboBox();
            label6 = new Label();
            groupBox4 = new GroupBox();
            chkSwapRGB = new CheckBox();
            chkSwapAlpha = new CheckBox();
            chkConstrainProps = new CheckBox();
            btnApplyDims = new Button();
            label11 = new Label();
            numH = new NumericUpDown();
            numW = new NumericUpDown();
            label10 = new Label();
            panel1 = new Panel();
            txtPath = new TextBox();
            panel2 = new Panel();
            numMIPPreview = new NumericUpDown();
            label12 = new Label();
            button1 = new Button();
            dlgOpen = new OpenFileDialog();
            pictureBox1 = new GoodPictureBox();
            chkSwapAlphaRGB = new CheckBox();
            groupBox1.SuspendLayout();
            ((ISupportInitialize) numLOD).BeginInit();
            groupBox2.SuspendLayout();
            grpPalette.SuspendLayout();
            ((ISupportInitialize) numPaletteCount).BeginInit();
            groupBox4.SuspendLayout();
            ((ISupportInitialize) numH).BeginInit();
            ((ISupportInitialize) numW).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((ISupportInitialize) numMIPPreview).BeginInit();
            SuspendLayout();
            // 
            // chkPreview
            // 
            chkPreview.Checked = true;
            chkPreview.CheckState = CheckState.Checked;
            chkPreview.Location = new Drawing.Point(9, 15);
            chkPreview.Name = "chkPreview";
            chkPreview.Size = new Drawing.Size(66, 21);
            chkPreview.TabIndex = 0;
            chkPreview.Text = "Preview";
            chkPreview.UseVisualStyleBackColor = true;
            chkPreview.CheckedChanged += chkPreview_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                | AnchorStyles.Right;
            groupBox1.Controls.Add(numLOD);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(cboFormat);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new Drawing.Point(3, 108);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Drawing.Size(179, 71);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Image";
            // 
            // numLOD
            // 
            numLOD.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                             | AnchorStyles.Right;
            numLOD.Location = new Drawing.Point(75, 42);
            numLOD.Minimum = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numLOD.Name = "numLOD";
            numLOD.Size = new Drawing.Size(98, 20);
            numLOD.TabIndex = 3;
            numLOD.Value = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numLOD.ValueChanged += numLOD_ValueChanged;
            // 
            // label5
            // 
            label5.Location = new Drawing.Point(6, 42);
            label5.Name = "label5";
            label5.Size = new Drawing.Size(63, 20);
            label5.TabIndex = 2;
            label5.Text = "MIP Levels:";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cboFormat
            // 
            cboFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                | AnchorStyles.Right;
            cboFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFormat.FormattingEnabled = true;
            cboFormat.Location = new Drawing.Point(75, 15);
            cboFormat.Name = "cboFormat";
            cboFormat.Size = new Drawing.Size(98, 21);
            cboFormat.TabIndex = 1;
            cboFormat.SelectedIndexChanged += cboFormat_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.Location = new Drawing.Point(6, 16);
            label4.Name = "label4";
            label4.Size = new Drawing.Size(63, 20);
            label4.TabIndex = 0;
            label4.Text = "Format:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnRecommend
            // 
            btnRecommend.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            btnRecommend.Location = new Drawing.Point(75, 14);
            btnRecommend.Name = "btnRecommend";
            btnRecommend.Size = new Drawing.Size(98, 21);
            btnRecommend.TabIndex = 1;
            btnRecommend.Text = "Recommend";
            btnRecommend.UseVisualStyleBackColor = true;
            btnRecommend.Click += btnRecommend_Click;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                | AnchorStyles.Right;
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(lblTransparencies);
            groupBox2.Controls.Add(lblDataSize);
            groupBox2.Controls.Add(lblColors);
            groupBox2.Controls.Add(lblSize);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Drawing.Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Drawing.Size(179, 99);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Info";
            // 
            // label9
            // 
            label9.Location = new Drawing.Point(6, 71);
            label9.Name = "label9";
            label9.Size = new Drawing.Size(85, 20);
            label9.TabIndex = 6;
            label9.Text = "Data Size:";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTransparencies
            // 
            lblTransparencies.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                        | AnchorStyles.Right;
            lblTransparencies.Location = new Drawing.Point(97, 51);
            lblTransparencies.Name = "lblTransparencies";
            lblTransparencies.Size = new Drawing.Size(76, 20);
            lblTransparencies.TabIndex = 5;
            lblTransparencies.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDataSize
            // 
            lblDataSize.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                  | AnchorStyles.Right;
            lblDataSize.Location = new Drawing.Point(97, 71);
            lblDataSize.Name = "lblDataSize";
            lblDataSize.Size = new Drawing.Size(76, 20);
            lblDataSize.TabIndex = 7;
            lblDataSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblColors
            // 
            lblColors.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                | AnchorStyles.Right;
            lblColors.Location = new Drawing.Point(97, 31);
            lblColors.Name = "lblColors";
            lblColors.Size = new Drawing.Size(76, 20);
            lblColors.TabIndex = 3;
            lblColors.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSize
            // 
            lblSize.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                              | AnchorStyles.Right;
            lblSize.Location = new Drawing.Point(94, 11);
            lblSize.Name = "lblSize";
            lblSize.Size = new Drawing.Size(79, 20);
            lblSize.TabIndex = 1;
            lblSize.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Location = new Drawing.Point(6, 51);
            label3.Name = "label3";
            label3.Size = new Drawing.Size(85, 20);
            label3.TabIndex = 4;
            label3.Text = "Transparent:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Location = new Drawing.Point(6, 31);
            label2.Name = "label2";
            label2.Size = new Drawing.Size(85, 20);
            label2.TabIndex = 2;
            label2.Text = "Colors:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Location = new Drawing.Point(6, 11);
            label1.Name = "label1";
            label1.Size = new Drawing.Size(85, 20);
            label1.TabIndex = 0;
            label1.Text = "Size:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom;
            btnOkay.Location = new Drawing.Point(8, 157);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new Drawing.Size(80, 23);
            btnOkay.TabIndex = 11;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += btnOkay_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom;
            btnCancel.Location = new Drawing.Point(94, 157);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Drawing.Size(80, 23);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // grpPalette
            // 
            grpPalette.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                 | AnchorStyles.Right;
            grpPalette.Controls.Add(chkImportPalette);
            grpPalette.Controls.Add(cboAlgorithm);
            grpPalette.Controls.Add(label8);
            grpPalette.Controls.Add(numPaletteCount);
            grpPalette.Controls.Add(label7);
            grpPalette.Controls.Add(cboPaletteFormat);
            grpPalette.Controls.Add(label6);
            grpPalette.Location = new Drawing.Point(3, 185);
            grpPalette.Name = "grpPalette";
            grpPalette.RightToLeft = RightToLeft.No;
            grpPalette.Size = new Drawing.Size(179, 118);
            grpPalette.TabIndex = 5;
            grpPalette.TabStop = false;
            grpPalette.Text = "Palette";
            // 
            // chkImportPalette
            // 
            chkImportPalette.Location = new Drawing.Point(9, 95);
            chkImportPalette.Name = "chkImportPalette";
            chkImportPalette.Size = new Drawing.Size(164, 17);
            chkImportPalette.TabIndex = 6;
            chkImportPalette.Text = "Import Palette";
            chkImportPalette.UseVisualStyleBackColor = true;
            chkImportPalette.CheckedChanged += chkImportPalette_CheckedChanged;
            // 
            // cboAlgorithm
            // 
            cboAlgorithm.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            cboAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAlgorithm.FormattingEnabled = true;
            cboAlgorithm.Location = new Drawing.Point(75, 68);
            cboAlgorithm.Name = "cboAlgorithm";
            cboAlgorithm.Size = new Drawing.Size(98, 21);
            cboAlgorithm.TabIndex = 5;
            cboAlgorithm.SelectedIndexChanged += formatChanged;
            // 
            // label8
            // 
            label8.Location = new Drawing.Point(6, 69);
            label8.Name = "label8";
            label8.Size = new Drawing.Size(63, 20);
            label8.TabIndex = 4;
            label8.Text = "Algorithm:";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numPaletteCount
            // 
            numPaletteCount.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                      | AnchorStyles.Right;
            numPaletteCount.Increment = new decimal(new[]
            {
                16,
                0,
                0,
                0
            });
            numPaletteCount.Location = new Drawing.Point(75, 42);
            numPaletteCount.Maximum = new decimal(new[]
            {
                256,
                0,
                0,
                0
            });
            numPaletteCount.Minimum = new decimal(new[]
            {
                16,
                0,
                0,
                0
            });
            numPaletteCount.Name = "numPaletteCount";
            numPaletteCount.Size = new Drawing.Size(97, 20);
            numPaletteCount.TabIndex = 3;
            numPaletteCount.Value = new decimal(new[]
            {
                16,
                0,
                0,
                0
            });
            numPaletteCount.ValueChanged += formatChanged;
            // 
            // label7
            // 
            label7.Location = new Drawing.Point(6, 42);
            label7.Name = "label7";
            label7.Size = new Drawing.Size(63, 20);
            label7.TabIndex = 2;
            label7.Text = "Colors:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cboPaletteFormat
            // 
            cboPaletteFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                       | AnchorStyles.Right;
            cboPaletteFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPaletteFormat.FormattingEnabled = true;
            cboPaletteFormat.Location = new Drawing.Point(75, 15);
            cboPaletteFormat.Name = "cboPaletteFormat";
            cboPaletteFormat.Size = new Drawing.Size(98, 21);
            cboPaletteFormat.TabIndex = 1;
            cboPaletteFormat.SelectedIndexChanged += formatChanged;
            // 
            // label6
            // 
            label6.Location = new Drawing.Point(6, 16);
            label6.Name = "label6";
            label6.Size = new Drawing.Size(63, 20);
            label6.TabIndex = 0;
            label6.Text = "Format:";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                | AnchorStyles.Left
                                                | AnchorStyles.Right;
            groupBox4.Controls.Add(chkSwapAlphaRGB);
            groupBox4.Controls.Add(chkSwapRGB);
            groupBox4.Controls.Add(chkSwapAlpha);
            groupBox4.Controls.Add(chkConstrainProps);
            groupBox4.Controls.Add(btnApplyDims);
            groupBox4.Controls.Add(label11);
            groupBox4.Controls.Add(numH);
            groupBox4.Controls.Add(numW);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(btnOkay);
            groupBox4.Controls.Add(btnRecommend);
            groupBox4.Controls.Add(chkPreview);
            groupBox4.Controls.Add(btnCancel);
            groupBox4.Location = new Drawing.Point(3, 309);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Drawing.Size(179, 189);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            // 
            // chkSwapRGB
            // 
            chkSwapRGB.AutoSize = true;
            chkSwapRGB.Location = new Drawing.Point(9, 110);
            chkSwapRGB.Name = "chkSwapRGB";
            chkSwapRGB.Size = new Drawing.Size(79, 17);
            chkSwapRGB.TabIndex = 8;
            chkSwapRGB.Text = "Swap RGB";
            chkSwapRGB.UseVisualStyleBackColor = true;
            chkSwapRGB.CheckedChanged += chkSwapRGB_CheckedChanged;
            // 
            // chkSwapAlpha
            // 
            chkSwapAlpha.AutoSize = true;
            chkSwapAlpha.Location = new Drawing.Point(94, 110);
            chkSwapAlpha.Name = "chkSwapAlpha";
            chkSwapAlpha.Size = new Drawing.Size(83, 17);
            chkSwapAlpha.TabIndex = 9;
            chkSwapAlpha.Text = "Swap Alpha";
            chkSwapAlpha.UseVisualStyleBackColor = true;
            chkSwapAlpha.CheckedChanged += chkSwapAlpha_CheckedChanged;
            // 
            // chkConstrainProps
            // 
            chkConstrainProps.AutoSize = true;
            chkConstrainProps.Checked = true;
            chkConstrainProps.CheckState = CheckState.Checked;
            chkConstrainProps.Location = new Drawing.Point(9, 87);
            chkConstrainProps.Name = "chkConstrainProps";
            chkConstrainProps.Size = new Drawing.Size(126, 17);
            chkConstrainProps.TabIndex = 7;
            chkConstrainProps.Text = "Constrain Proportions";
            chkConstrainProps.UseVisualStyleBackColor = true;
            chkConstrainProps.CheckedChanged += chkConstrainProps_CheckedChanged;
            // 
            // btnApplyDims
            // 
            btnApplyDims.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            btnApplyDims.Location = new Drawing.Point(125, 60);
            btnApplyDims.Name = "btnApplyDims";
            btnApplyDims.Size = new Drawing.Size(48, 21);
            btnApplyDims.TabIndex = 6;
            btnApplyDims.Text = "Apply";
            btnApplyDims.UseVisualStyleBackColor = true;
            btnApplyDims.Click += btnApplyDims_Click;
            // 
            // label11
            // 
            label11.Location = new Drawing.Point(58, 60);
            label11.Name = "label11";
            label11.Size = new Drawing.Size(15, 21);
            label11.TabIndex = 4;
            label11.Text = "X";
            label11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // numH
            // 
            numH.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numH.Location = new Drawing.Point(73, 60);
            numH.Maximum = new decimal(new[]
            {
                1024,
                0,
                0,
                0
            });
            numH.Minimum = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numH.Name = "numH";
            numH.Size = new Drawing.Size(46, 20);
            numH.TabIndex = 5;
            numH.Value = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numH.ValueChanged += numH_ValueChanged;
            // 
            // numW
            // 
            numW.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numW.Location = new Drawing.Point(9, 61);
            numW.Maximum = new decimal(new[]
            {
                1024,
                0,
                0,
                0
            });
            numW.Minimum = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numW.Name = "numW";
            numW.Size = new Drawing.Size(46, 20);
            numW.TabIndex = 3;
            numW.Value = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numW.ValueChanged += numW_ValueChanged;
            // 
            // label10
            // 
            label10.Location = new Drawing.Point(6, 38);
            label10.Name = "label10";
            label10.Size = new Drawing.Size(81, 20);
            label10.TabIndex = 2;
            label10.Text = "Dimensions:";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(groupBox4);
            panel1.Controls.Add(grpPalette);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Drawing.Point(379, 0);
            panel1.Margin = new Padding(3, 3, 0, 3);
            panel1.Name = "panel1";
            panel1.Size = new Drawing.Size(185, 501);
            panel1.TabIndex = 9;
            // 
            // txtPath
            // 
            txtPath.Dock = DockStyle.Fill;
            txtPath.Location = new Drawing.Point(92, 0);
            txtPath.Name = "txtPath";
            txtPath.ReadOnly = true;
            txtPath.Size = new Drawing.Size(212, 20);
            txtPath.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Controls.Add(txtPath);
            panel2.Controls.Add(numMIPPreview);
            panel2.Controls.Add(label12);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Drawing.Size(379, 20);
            panel2.TabIndex = 1;
            // 
            // numMIPPreview
            // 
            numMIPPreview.Dock = DockStyle.Left;
            numMIPPreview.Location = new Drawing.Point(41, 0);
            numMIPPreview.Minimum = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numMIPPreview.Name = "numMIPPreview";
            numMIPPreview.Size = new Drawing.Size(51, 20);
            numMIPPreview.TabIndex = 1;
            numMIPPreview.Value = new decimal(new[]
            {
                1,
                0,
                0,
                0
            });
            numMIPPreview.Visible = false;
            numMIPPreview.ValueChanged += numMIPPreview_ValueChanged;
            // 
            // label12
            // 
            label12.Dock = DockStyle.Left;
            label12.Location = new Drawing.Point(0, 0);
            label12.Name = "label12";
            label12.Size = new Drawing.Size(41, 20);
            label12.TabIndex = 0;
            label12.Text = "MIP:";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            label12.Visible = false;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.Location = new Drawing.Point(304, 0);
            button1.Name = "button1";
            button1.Size = new Drawing.Size(75, 20);
            button1.TabIndex = 3;
            button1.Text = "Browse...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Drawing.Point(0, 20);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Picture = null;
            pictureBox1.Size = new Drawing.Size(379, 481);
            pictureBox1.TabIndex = 0;
            // 
            // chkSwapAlphaRGB
            // 
            chkSwapAlphaRGB.AutoSize = true;
            chkSwapAlphaRGB.Location = new Drawing.Point(9, 133);
            chkSwapAlphaRGB.Name = "chkSwapAlphaRGB";
            chkSwapAlphaRGB.Size = new Drawing.Size(131, 17);
            chkSwapAlphaRGB.TabIndex = 10;
            chkSwapAlphaRGB.Text = "Swap Alpha with RGB";
            chkSwapAlphaRGB.UseVisualStyleBackColor = true;
            chkSwapAlphaRGB.CheckedChanged += chkSwapAlphaRGB_CheckedChanged;
            // 
            // TextureConverterDialog
            // 
            AcceptButton = btnOkay;
            ClientSize = new Drawing.Size(564, 501);
            Controls.Add(pictureBox1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MinimumSize = new Drawing.Size(0, 470);
            Name = "TextureConverterDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Advanced Texture Converter";
            groupBox1.ResumeLayout(false);
            ((ISupportInitialize) numLOD).EndInit();
            groupBox2.ResumeLayout(false);
            grpPalette.ResumeLayout(false);
            ((ISupportInitialize) numPaletteCount).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((ISupportInitialize) numH).EndInit();
            ((ISupportInitialize) numW).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((ISupportInitialize) numMIPPreview).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
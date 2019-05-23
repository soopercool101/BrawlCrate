using System.Drawing;
using BrawlLib.Imaging;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Forms
{
    public class CLRControl : UserControl
    {
        #region Designer

        private Label lblPrimary;
        private Label lblBase;
        private Label lblColor;
        private ContextMenuStrip ctxMenu;
        private IContainer components;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private Panel pnlPrimary;
        private Label lblCNoA;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem allToolStripMenuItem1;
        private ToolStripMenuItem colorToolStripMenuItem1;
        private ToolStripMenuItem alphaToolStripMenuItem1;
        private ListBox lstColors;
    
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblPrimary = new System.Windows.Forms.Label();
            this.lstColors = new System.Windows.Forms.ListBox();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblBase = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.pnlPrimary = new System.Windows.Forms.Panel();
            this.lblCNoA = new System.Windows.Forms.Label();
            this.allToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.colorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.alphaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenu.SuspendLayout();
            this.pnlPrimary.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPrimary
            // 
            this.lblPrimary.Location = new System.Drawing.Point(5, 2);
            this.lblPrimary.Name = "lblPrimary";
            this.lblPrimary.Size = new System.Drawing.Size(61, 20);
            this.lblPrimary.TabIndex = 0;
            this.lblPrimary.Text = "Base Color:";
            this.lblPrimary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstColors
            // 
            this.lstColors.ContextMenuStrip = this.ctxMenu;
            this.lstColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstColors.FormattingEnabled = true;
            this.lstColors.IntegralHeight = false;
            this.lstColors.Location = new System.Drawing.Point(0, 24);
            this.lstColors.Name = "lstColors";
            this.lstColors.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstColors.Size = new System.Drawing.Size(334, 218);
            this.lstColors.TabIndex = 1;
            this.lstColors.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstColors_DrawItem);
            this.lstColors.DoubleClick += new System.EventHandler(this.lstColors_DoubleClick);
            this.lstColors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstColors_KeyDown);
            this.lstColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstColors_MouseDown);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator1,
            this.editToolStripMenuItem});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(165, 104);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem1,
            this.colorToolStripMenuItem1,
            this.alphaToolStripMenuItem1});
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.editToolStripMenuItem.Text = "Edit...";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // lblBase
            // 
            this.lblBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBase.BackColor = System.Drawing.Color.Transparent;
            this.lblBase.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBase.Location = new System.Drawing.Point(72, 2);
            this.lblBase.Name = "lblBase";
            this.lblBase.Size = new System.Drawing.Size(149, 20);
            this.lblBase.TabIndex = 2;
            this.lblBase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBase.Click += new System.EventHandler(this.lblBase_Click);
            // 
            // lblColor
            // 
            this.lblColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(231, 5);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(41, 14);
            this.lblColor.TabIndex = 3;
            this.lblColor.Click += new System.EventHandler(this.lblBase_Click);
            // 
            // pnlPrimary
            // 
            this.pnlPrimary.BackColor = System.Drawing.Color.White;
            this.pnlPrimary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPrimary.Controls.Add(this.lblColor);
            this.pnlPrimary.Controls.Add(this.lblCNoA);
            this.pnlPrimary.Controls.Add(this.lblPrimary);
            this.pnlPrimary.Controls.Add(this.lblBase);
            this.pnlPrimary.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPrimary.Location = new System.Drawing.Point(0, 0);
            this.pnlPrimary.Name = "pnlPrimary";
            this.pnlPrimary.Size = new System.Drawing.Size(334, 24);
            this.pnlPrimary.TabIndex = 4;
            // 
            // lblCNoA
            // 
            this.lblCNoA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCNoA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCNoA.Location = new System.Drawing.Point(271, 5);
            this.lblCNoA.Name = "lblCNoA";
            this.lblCNoA.Size = new System.Drawing.Size(41, 14);
            this.lblCNoA.TabIndex = 4;
            this.lblCNoA.Click += new System.EventHandler(this.lblBase_Click);
            // 
            // allToolStripMenuItem1
            // 
            this.allToolStripMenuItem1.Name = "allToolStripMenuItem1";
            this.allToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.allToolStripMenuItem1.Text = "All";
            this.allToolStripMenuItem1.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // colorToolStripMenuItem1
            // 
            this.colorToolStripMenuItem1.Name = "colorToolStripMenuItem1";
            this.colorToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.colorToolStripMenuItem1.Text = "Color";
            this.colorToolStripMenuItem1.Click += new System.EventHandler(this.colorToolStripMenuItem_Click);
            // 
            // alphaToolStripMenuItem1
            // 
            this.alphaToolStripMenuItem1.Name = "alphaToolStripMenuItem1";
            this.alphaToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.alphaToolStripMenuItem1.Text = "Alpha";
            this.alphaToolStripMenuItem1.Click += new System.EventHandler(this.alphaToolStripMenuItem_Click);
            // 
            // CLRControl
            // 
            this.Controls.Add(this.lstColors);
            this.Controls.Add(this.pnlPrimary);
            this.DoubleBuffered = true;
            this.Name = "CLRControl";
            this.Size = new System.Drawing.Size(334, 242);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CLRControl_KeyDown);
            this.ctxMenu.ResumeLayout(false);
            this.pnlPrimary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ARGBPixel _primaryColor;

        public event EventHandler CurrentColorChanged;

        public int _colorId = 0;
        public int ColorID { get { return _colorId; } set { _colorId = value; SourceChanged(); } }

        private IColorSource _colorSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IColorSource ColorSource
        {
            get { return _colorSource; }
            set { _colorSource = value; SourceChanged(); }
        }

        private GoodColorDialog _dlgColor;
        private GradientDialog _dlgGradient;

        private IList<int> SelectedIndices
        {
            get
            {
                var indices = lstColors.SelectedIndices;
                int[] array = new int[indices.Count];
                for (int i=0; i<array.Length; i++) {
                    array[i] = indices[i];
                }
                return array;
            }
        }

        public CLRControl() 
        { 
            InitializeComponent();
            _dlgColor = new GoodColorDialog();
            _dlgGradient = new GradientDialog();
        }

        private void SourceChanged()
        {
            lstColors.BeginUpdate();
            lstColors.Items.Clear();

            if (_colorSource != null)
            {
                int count = _colorSource.ColorCount(_colorId);
                for (int i = 0; i < count; i++)
                    lstColors.Items.Add(_colorSource.GetColor(i, _colorId));

                if (pnlPrimary.Visible = _colorSource.HasPrimary(_colorId))
                {
                    _primaryColor = _colorSource.GetPrimaryColor(_colorId);
                    lblPrimary.Text = _colorSource.PrimaryColorName(_colorId);
                    UpdateBase();
                }
            }

            lstColors.EndUpdate();
        }

        private void UpdateBase()
        {
            lblBase.Text = _primaryColor.ToString();
            lblColor.BackColor = (Color)_primaryColor;
            lblCNoA.BackColor = Color.FromArgb(_primaryColor.R, _primaryColor.G, _primaryColor.B);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e) { lstColors_DoubleClick(sender, e); }
        private void lstColors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                lstColors_DoubleClick(sender, e);
        }

        int tempIndex = -1;
        void _dlgColor_OnColorChanged(Color selection)
        {
            if (tempIndex >= 0)
            {
                ARGBPixel p = (ARGBPixel)selection;
                lstColors.Items[tempIndex] = p;
                _colorSource.SetColor(tempIndex, _colorId, p);

                if (CurrentColorChanged != null)
                    CurrentColorChanged(this, EventArgs.Empty);
            }
        }
        private void lstColors_DoubleClick(object sender, EventArgs e)
        {
            var indices = SelectedIndices;
            if ((_colorSource == null) || (indices.Count <= 0))
                return;

            int count = indices.Count;
            if (count == 1)
            {
                tempIndex = indices[0];
                ARGBPixel prev = (ARGBPixel)lstColors.Items[tempIndex];
                _dlgColor.Color = (Color)prev;
                _dlgColor.OnColorChanged += _dlgColor_OnColorChanged;
                if (_dlgColor.ShowDialog(this) != DialogResult.OK)
                {
                    if (tempIndex >= 0)
                    {
                        lstColors.Items[tempIndex] = prev;
                        _colorSource.SetColor(tempIndex, _colorId, prev);

                        if (CurrentColorChanged != null)
                            CurrentColorChanged(this, EventArgs.Empty);
                    }
                }
                _dlgColor.OnColorChanged -= _dlgColor_OnColorChanged;
                tempIndex = -1;
            }
            else
            {
                //Sort indices
                int[] sorted = new int[count];
                indices.CopyTo(sorted, 0);
                Array.Sort(sorted);

                _dlgGradient.StartColor = (Color)(ARGBPixel)lstColors.Items[sorted[0]];
                _dlgGradient.EndColor = (Color)(ARGBPixel)lstColors.Items[sorted[count - 1]];
                if (_dlgGradient.ShowDialog(this) == DialogResult.OK)
                {
                    //Interpolate and apply to each in succession.
                    ARGBPixel start = (ARGBPixel)_dlgGradient.StartColor;
                    ARGBPixel end = (ARGBPixel)_dlgGradient.EndColor;
                    float stepA = (end.A - start.A) / (float)count;
                    float stepR = (end.R - start.R) / (float)count;
                    float stepG = (end.G - start.G) / (float)count;
                    float stepB = (end.B - start.B) / (float)count;
                    for (int i = 0; i < count; i++)
                    {
                        ARGBPixel p = new ARGBPixel(
                            (byte)(start.A + (i * stepA)),
                            (byte)(start.R + (i * stepR)),
                            (byte)(start.G + (i * stepG)),
                            (byte)(start.B + (i * stepB)));
                        lstColors.Items[sorted[i]] = p;
                        _colorSource.SetColor(sorted[i], _colorId, p);
                    }
                }
            }
        }

        private static Font _renderFont = new Font(FontFamily.GenericMonospace, 9.0f);
        private void lstColors_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            int index = e.Index;

            g.FillRectangle(Brushes.White, r);

            if (index >= 0)
            {
                ARGBPixel p = (ARGBPixel)lstColors.Items[index];
                int boxWidth = 40;
                int textWidth = 275;

                if ((e.State & DrawItemState.Selected) != 0)
                    g.FillRectangle(Brushes.LightBlue, r.X, r.Y, textWidth, r.Height);

                double n = Math.Floor(Math.Log10(_colorSource.ColorCount(_colorId)) + 1);
                g.DrawString(String.Format("[{0}]  -  {1}", index.ToString().PadLeft((int)n, ' '), p.ToPaddedString()), _renderFont, Brushes.Black, 4.0f, e.Bounds.Y - 2);

                r.X += textWidth;
                r.Width = boxWidth;

                using (Brush b = new SolidBrush((Color)p))
                    g.FillRectangle(b, r);
                g.DrawRectangle(Pens.Black, r);

                p.A = 255;
                r.X += boxWidth;
                using (Brush b = new SolidBrush((Color)p))
                    g.FillRectangle(b, r);
                g.DrawRectangle(Pens.Black, r);
            }
        }

        private void lblBase_Click(object sender, EventArgs e)
        {
            if (_colorSource == null)
                return;

            _dlgColor.Color = (Color)_primaryColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _primaryColor = (ARGBPixel)_dlgColor.Color;
                _colorSource.SetPrimaryColor(_colorId, _primaryColor);
                UpdateBase();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstColors.SelectedItems.Count != 0)
            {
                string s = "";
                foreach (ARGBPixel w in lstColors.SelectedItems)
                    s += w.ToRGBAColorCode() + Environment.NewLine;
                Clipboard.SetText(s);
            }
        }
        readonly string _allowed = "0123456789abcdefABCDEF";

        private List<ARGBPixel> GetCopiedPixels()
        {
            List<ARGBPixel> pixels = new List<ARGBPixel>();
            string s = Clipboard.GetText();
            string[] str = s.Split('\r', '\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < str.Length; i++)
            {
                int x = 0;
                s = "";
                foreach (char c in str[i])
                {
                    if (_allowed.IndexOf(c) >= 0)
                    {
                        s += c;
                        x++;
                    }
                    if (x >= 8)
                        break;
                }

                pixels.Add(RGBAPixel.Parse(s));
            }
            return pixels;
        }

        private void lstColors_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lstColors.IndexFromPoint(e.Location);
                lstColors.SelectedIndex = index;
            }
        }

        private void CLRControl_KeyDown(object sender, KeyEventArgs e)
        {
            Action<ToolStripMenuItem> test = null;
            test = (node) =>
            {
                if (node.ShortcutKeys == e.KeyData)
                    node.PerformClick();
                foreach (ToolStripMenuItem child in node.DropDownItems)
                    test(child);
            };
            foreach (ToolStripMenuItem item in ctxMenu.Items)
                test(item);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstColors.BeginUpdate();
            for (int i = 0; i < lstColors.Items.Count; i++)
                lstColors.SetSelected(i, true);
            lstColors.EndUpdate();
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indices = SelectedIndices;
            if (indices.Count >= 0)
            {
                int v = 0;
                List<ARGBPixel> pixels = GetCopiedPixels();
                foreach (int r in indices)
                {
                    if (v >= pixels.Count)
                        break;

                    ARGBPixel copied = pixels[v++];
                    lstColors.Items[r] = copied;
                    _colorSource.SetColor(r, _colorId, copied);
                }
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indices = SelectedIndices;
            if (indices.Count >= 0)
            {
                int v = 0;
                List<ARGBPixel> pixels = GetCopiedPixels();
                foreach (int r in indices)
                {
                    if (v >= pixels.Count)
                        break;

                    ARGBPixel copied = pixels[v++];
                    ARGBPixel temp = (ARGBPixel)lstColors.Items[r];
                    temp.R = copied.R;
                    temp.G = copied.G;
                    temp.B = copied.B;
                    lstColors.Items[r] = temp;
                    _colorSource.SetColor(r, _colorId, temp);
                }
            }
        }

        private void alphaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indices = SelectedIndices;
            if (indices.Count >= 0)
            {
                int v = 0;
                List<ARGBPixel> pixels = GetCopiedPixels();
                foreach (int r in indices)
                {
                    if (v >= pixels.Count)
                        break;

                    ARGBPixel copied = pixels[v++];
                    ARGBPixel temp = (ARGBPixel)lstColors.Items[r];
                    temp.A = copied.A;
                    lstColors.Items[r] = temp;
                    _colorSource.SetColor(r, _colorId, temp);
                }
            }
        }
    }
}

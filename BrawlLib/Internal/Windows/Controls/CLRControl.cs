using BrawlLib.Imaging;
using BrawlLib.Internal.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
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
            components = new Container();
            lblPrimary = new Label();
            lstColors = new ListBox();
            ctxMenu = new ContextMenuStrip(components);
            selectAllToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            lblBase = new Label();
            lblColor = new Label();
            pnlPrimary = new Panel();
            lblCNoA = new Label();
            allToolStripMenuItem1 = new ToolStripMenuItem();
            colorToolStripMenuItem1 = new ToolStripMenuItem();
            alphaToolStripMenuItem1 = new ToolStripMenuItem();
            ctxMenu.SuspendLayout();
            pnlPrimary.SuspendLayout();
            SuspendLayout();
            // 
            // lblPrimary
            // 
            lblPrimary.Location = new Point(5, 2);
            lblPrimary.Name = "lblPrimary";
            lblPrimary.Size = new Size(61, 20);
            lblPrimary.TabIndex = 0;
            lblPrimary.Text = "Base Color:";
            lblPrimary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lstColors
            // 
            lstColors.ContextMenuStrip = ctxMenu;
            lstColors.Dock = DockStyle.Fill;
            lstColors.DrawMode = DrawMode.OwnerDrawFixed;
            lstColors.FormattingEnabled = true;
            lstColors.IntegralHeight = false;
            lstColors.Location = new Point(0, 24);
            lstColors.Name = "lstColors";
            lstColors.SelectionMode = SelectionMode.MultiExtended;
            lstColors.Size = new Size(334, 218);
            lstColors.TabIndex = 1;
            lstColors.DrawItem += new DrawItemEventHandler(lstColors_DrawItem);
            lstColors.DoubleClick += new EventHandler(lstColors_DoubleClick);
            lstColors.KeyDown += new KeyEventHandler(lstColors_KeyDown);
            lstColors.MouseDown += new MouseEventHandler(lstColors_MouseDown);
            // 
            // ctxMenu
            // 
            ctxMenu.Items.AddRange(new ToolStripItem[]
            {
                selectAllToolStripMenuItem,
                toolStripMenuItem1,
                copyToolStripMenuItem,
                pasteToolStripMenuItem,
                toolStripSeparator1,
                editToolStripMenuItem
            });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.Size = new Size(165, 104);
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            selectAllToolStripMenuItem.Size = new Size(164, 22);
            selectAllToolStripMenuItem.Text = "Select All";
            selectAllToolStripMenuItem.Click += new EventHandler(selectAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(161, 6);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            copyToolStripMenuItem.Size = new Size(164, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += new EventHandler(copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                allToolStripMenuItem1,
                colorToolStripMenuItem1,
                alphaToolStripMenuItem1
            });
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteToolStripMenuItem.Size = new Size(164, 22);
            pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(161, 6);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(164, 22);
            editToolStripMenuItem.Text = "Edit...";
            editToolStripMenuItem.Click += new EventHandler(editToolStripMenuItem_Click);
            // 
            // lblBase
            // 
            lblBase.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                              | AnchorStyles.Right;
            lblBase.BackColor = Color.Transparent;
            lblBase.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBase.Location = new Point(72, 2);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(149, 20);
            lblBase.TabIndex = 2;
            lblBase.TextAlign = ContentAlignment.MiddleLeft;
            lblBase.Click += new EventHandler(lblBase_Click);
            // 
            // lblColor
            // 
            lblColor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblColor.BorderStyle = BorderStyle.FixedSingle;
            lblColor.Location = new Point(231, 5);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(41, 14);
            lblColor.TabIndex = 3;
            lblColor.Click += new EventHandler(lblBase_Click);
            // 
            // pnlPrimary
            // 
            pnlPrimary.BackColor = Color.White;
            pnlPrimary.BorderStyle = BorderStyle.FixedSingle;
            pnlPrimary.Controls.Add(lblColor);
            pnlPrimary.Controls.Add(lblCNoA);
            pnlPrimary.Controls.Add(lblPrimary);
            pnlPrimary.Controls.Add(lblBase);
            pnlPrimary.Dock = DockStyle.Top;
            pnlPrimary.Location = new Point(0, 0);
            pnlPrimary.Name = "pnlPrimary";
            pnlPrimary.Size = new Size(334, 24);
            pnlPrimary.TabIndex = 4;
            // 
            // lblCNoA
            // 
            lblCNoA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCNoA.BorderStyle = BorderStyle.FixedSingle;
            lblCNoA.Location = new Point(271, 5);
            lblCNoA.Name = "lblCNoA";
            lblCNoA.Size = new Size(41, 14);
            lblCNoA.TabIndex = 4;
            lblCNoA.Click += new EventHandler(lblBase_Click);
            // 
            // allToolStripMenuItem1
            // 
            allToolStripMenuItem1.Name = "allToolStripMenuItem1";
            allToolStripMenuItem1.Size = new Size(152, 22);
            allToolStripMenuItem1.Text = "All";
            allToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.V;
            allToolStripMenuItem1.Click += new EventHandler(allToolStripMenuItem_Click);
            // 
            // colorToolStripMenuItem1
            // 
            colorToolStripMenuItem1.Name = "colorToolStripMenuItem1";
            colorToolStripMenuItem1.Size = new Size(152, 22);
            colorToolStripMenuItem1.Text = "Color";
            colorToolStripMenuItem1.Click += new EventHandler(colorToolStripMenuItem_Click);
            // 
            // alphaToolStripMenuItem1
            // 
            alphaToolStripMenuItem1.Name = "alphaToolStripMenuItem1";
            alphaToolStripMenuItem1.Size = new Size(152, 22);
            alphaToolStripMenuItem1.Text = "Alpha";
            alphaToolStripMenuItem1.Click += new EventHandler(alphaToolStripMenuItem_Click);
            // 
            // CLRControl
            // 
            Controls.Add(lstColors);
            Controls.Add(pnlPrimary);
            DoubleBuffered = true;
            Name = "CLRControl";
            Size = new Size(334, 242);
            KeyDown += new KeyEventHandler(CLRControl_KeyDown);
            ctxMenu.ResumeLayout(false);
            pnlPrimary.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ARGBPixel _primaryColor;

        public event EventHandler CurrentColorChanged;

        public int _colorId;

        public int ColorID
        {
            get => _colorId;
            set
            {
                _colorId = value;
                SourceChanged();
            }
        }

        private IColorSource _colorSource;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IColorSource ColorSource
        {
            get => _colorSource;
            set
            {
                _colorSource = value;
                SourceChanged();
            }
        }

        private readonly GoodColorDialog _dlgColor;
        private readonly GradientDialog _dlgGradient;

        private IList<int> SelectedIndices
        {
            get
            {
                ListBox.SelectedIndexCollection indices = lstColors.SelectedIndices;
                int[] array = new int[indices.Count];
                for (int i = 0; i < array.Length; i++)
                {
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
                {
                    lstColors.Items.Add(_colorSource.GetColor(i, _colorId));
                }

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
            lblColor.BackColor = (Color) _primaryColor;
            lblCNoA.BackColor = Color.FromArgb(_primaryColor.R, _primaryColor.G, _primaryColor.B);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstColors_DoubleClick(sender, e);
        }

        private void lstColors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lstColors_DoubleClick(sender, e);
            }
        }

        private int tempIndex = -1;

        private void _dlgColor_OnColorChanged(Color selection)
        {
            if (tempIndex >= 0)
            {
                ARGBPixel p = (ARGBPixel) selection;
                lstColors.Items[tempIndex] = p;
                _colorSource.SetColor(tempIndex, _colorId, p);

                CurrentColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void lstColors_DoubleClick(object sender, EventArgs e)
        {
            IList<int> indices = SelectedIndices;
            if (_colorSource == null || indices.Count <= 0)
            {
                return;
            }

            int count = indices.Count;
            if (count == 1)
            {
                tempIndex = indices[0];
                ARGBPixel prev = (ARGBPixel) lstColors.Items[tempIndex];
                _dlgColor.Color = (Color) prev;
                _dlgColor.OnColorChanged += _dlgColor_OnColorChanged;
                if (_dlgColor.ShowDialog(this) != DialogResult.OK)
                {
                    if (tempIndex >= 0)
                    {
                        lstColors.Items[tempIndex] = prev;
                        _colorSource.SetColor(tempIndex, _colorId, prev);

                        CurrentColorChanged?.Invoke(this, EventArgs.Empty);
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

                _dlgGradient.StartColor = (Color) (ARGBPixel) lstColors.Items[sorted[0]];
                _dlgGradient.EndColor = (Color) (ARGBPixel) lstColors.Items[sorted[count - 1]];
                if (_dlgGradient.ShowDialog(this) == DialogResult.OK)
                {
                    //Interpolate and apply to each in succession.
                    ARGBPixel start = (ARGBPixel) _dlgGradient.StartColor;
                    ARGBPixel end = (ARGBPixel) _dlgGradient.EndColor;
                    float stepA = (end.A - start.A) / ((float) count - 1);
                    float stepR = (end.R - start.R) / ((float) count - 1);
                    float stepG = (end.G - start.G) / ((float) count - 1);
                    float stepB = (end.B - start.B) / ((float) count - 1);
                    for (int i = 0; i < count - 1; i++)
                    {
                        ARGBPixel p = new ARGBPixel(
                            (byte) (start.A + i * stepA),
                            (byte) (start.R + i * stepR),
                            (byte) (start.G + i * stepG),
                            (byte) (start.B + i * stepB));
                        lstColors.Items[sorted[i]] = p;
                        _colorSource.SetColor(sorted[i], _colorId, p);
                    }

                    lstColors.Items[sorted[count - 1]] = end;
                    _colorSource.SetColor(sorted[count - 1], _colorId, end);
                }
            }
        }

        private static readonly Font _renderFont = new Font(FontFamily.GenericMonospace, 9.0f);

        private void lstColors_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            int index = e.Index;

            g.FillRectangle(Brushes.White, r);

            if (index >= 0)
            {
                ARGBPixel p = (ARGBPixel) lstColors.Items[index];
                int boxWidth = 40;
                int textWidth = 275;

                if ((e.State & DrawItemState.Selected) != 0)
                {
                    g.FillRectangle(Brushes.LightBlue, r.X, r.Y, textWidth, r.Height);
                }

                double n = Math.Floor(Math.Log10(_colorSource.ColorCount(_colorId)) + 1);
                g.DrawString($"[{index.ToString().PadLeft((int) n, ' ')}]  -  {p.ToPaddedString()}",
                    _renderFont, Brushes.Black, 4.0f, e.Bounds.Y - 2);

                r.X += textWidth;
                r.Width = boxWidth;

                using (Brush b = new SolidBrush((Color) p))
                {
                    g.FillRectangle(b, r);
                }

                g.DrawRectangle(Pens.Black, r);

                p.A = 255;
                r.X += boxWidth;
                using (Brush b = new SolidBrush((Color) p))
                {
                    g.FillRectangle(b, r);
                }

                g.DrawRectangle(Pens.Black, r);
            }
        }

        private void lblBase_Click(object sender, EventArgs e)
        {
            if (_colorSource == null)
            {
                return;
            }

            _dlgColor.Color = (Color) _primaryColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _primaryColor = (ARGBPixel) _dlgColor.Color;
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
                {
                    s += w.ToRGBAColorCode() + Environment.NewLine;
                }

                Clipboard.SetText(s);
            }
        }

        private readonly string _allowed = "0123456789abcdefABCDEF";

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
                    {
                        break;
                    }
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
                {
                    node.PerformClick();
                }

                foreach (ToolStripMenuItem child in node.DropDownItems)
                {
                    test(child);
                }
            };
            foreach (ToolStripMenuItem item in ctxMenu.Items)
            {
                test(item);
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstColors.BeginUpdate();
            for (int i = 0; i < lstColors.Items.Count; i++)
            {
                lstColors.SetSelected(i, true);
            }

            lstColors.EndUpdate();
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IList<int> indices = SelectedIndices;
            if (indices.Count >= 0)
            {
                int v = 0;
                List<ARGBPixel> pixels = GetCopiedPixels();
                foreach (int r in indices)
                {
                    if (v >= pixels.Count)
                    {
                        break;
                    }

                    ARGBPixel copied = pixels[v++];
                    lstColors.Items[r] = copied;
                    _colorSource.SetColor(r, _colorId, copied);
                }
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IList<int> indices = SelectedIndices;
            if (indices.Count >= 0)
            {
                int v = 0;
                List<ARGBPixel> pixels = GetCopiedPixels();
                foreach (int r in indices)
                {
                    if (v >= pixels.Count)
                    {
                        break;
                    }

                    ARGBPixel copied = pixels[v++];
                    ARGBPixel temp = (ARGBPixel) lstColors.Items[r];
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
            IList<int> indices = SelectedIndices;
            if (indices.Count >= 0)
            {
                int v = 0;
                List<ARGBPixel> pixels = GetCopiedPixels();
                foreach (int r in indices)
                {
                    if (v >= pixels.Count)
                    {
                        break;
                    }

                    ARGBPixel copied = pixels[v++];
                    ARGBPixel temp = (ARGBPixel) lstColors.Items[r];
                    temp.A = copied.A;
                    lstColors.Items[r] = temp;
                    _colorSource.SetColor(r, _colorId, temp);
                }
            }
        }
    }
}
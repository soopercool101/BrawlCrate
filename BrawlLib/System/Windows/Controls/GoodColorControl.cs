using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using BrawlLib.Imaging;

namespace System.Windows.Forms
{
    public class GoodColorControl : UserControl
    {
        private readonly string _allowed = "0123456789abcdefABCDEF";
        private readonly LinearGradientBrush _alphaBrush;

        private readonly LinearGradientBrush _barBrush;

        //private GraphicsPath _squarePath;
        private readonly Color[] _boxColors = {Color.Black, Color.White, Color.Black, Color.Black, Color.Black};

        private readonly NumericUpDown[] _boxes;
        private readonly PathGradientBrush _squareBrush;
        private bool _alphaGrabbing;
        private int _alphaY;
        private bool _barGrabbing;
        private int _barY;

        private int _brushH = -1;

        //private int _hue;
        private HSVPixel _hsv = new HSVPixel(0, 100, 100);

        //private int _alpha = 255;
        private ARGBPixel _rgba;

        private bool _showAlpha = true;
        private bool _squareGrabbing;
        private int _squareX, _squareY;
        private bool _updating;

        public GoodColorControl()
        {
            InitializeComponent();

            _boxes = new[] {numH, numS, numV, numR, numG, numB, numA};
            for (var i = 0; i < _boxes.Length; i++)
            {
                _boxes[i].ValueChanged += OnBoxChanged;
                _boxes[i].Tag = i;
            }

            var r = pnlColorBox.ClientRectangle;

            //_squarePath = new GraphicsPath();
            //_squarePath.AddRectangle(r);
            //_squareBrush = new PathGradientBrush(_squarePath);

            _squareBrush = new PathGradientBrush(new[]
            {
                new PointF(r.Width, 0),
                new PointF(r.Width, r.Height),
                new PointF(0, r.Height),
                new PointF(0, 0),
                new PointF(r.Width, 0)
            })
            {
                CenterPoint = new PointF(r.Width / 2, r.Height / 2)
            };

            var p = r.Height / 6.0f / r.Height;
            _barBrush = new LinearGradientBrush(new Rectangle(0, 0, r.Width, r.Height), Color.Red, Color.Red,
                LinearGradientMode.Vertical);

            var blend = new ColorBlend
            {
                Colors = new[] {Color.Red, Color.Yellow, Color.Lime, Color.Cyan, Color.Blue, Color.Magenta, Color.Red},
                Positions = new[] {0, p, p * 2, p * 3, p * 4, p * 5, 1.0f}
            };

            _barBrush.InterpolationColors = blend;

            _alphaBrush = new LinearGradientBrush(new Rectangle(0, 0, pnlAlpha.Width, pnlAlpha.Height), Color.White,
                Color.Black, LinearGradientMode.Vertical);
        }

        public bool ShowAlpha
        {
            get => _showAlpha;
            set => panel2.Visible = numA.Visible = lblA.Visible = _showAlpha = value;
        }

        public Color Color
        {
            get => (Color) _rgba;
            set
            {
                _rgba = (ARGBPixel) value;
                OnColorChanged(false);
            }
        }

        public event EventHandler ColorChanged;

        protected void OnBoxChanged(object sender, EventArgs e)
        {
            if (_updating) return;

            var box = sender as NumericUpDown;
            var value = (int) box.Value;
            var index = (int) box.Tag;
            switch (index)
            {
                case 0:
                {
                    _hsv.H = (ushort) value;
                    break;
                }

                case 1:
                {
                    _hsv.S = (byte) value;
                    break;
                }

                case 2:
                {
                    _hsv.V = (byte) value;
                    break;
                }

                case 3:
                {
                    _rgba.R = (byte) value;
                    break;
                }

                case 4:
                {
                    _rgba.G = (byte) value;
                    break;
                }

                case 5:
                {
                    _rgba.B = (byte) value;
                    break;
                }

                case 6:
                {
                    pnlAlpha.Invalidate();
                    break;
                }

                default: return;
            }

            if (index == 6)
            {
                _rgba.A = (byte) value;
                txtColorCode.Text = _rgba.ToRGBAColorCode();
                ColorChanged?.Invoke(this, null);
            }
            else
            {
                OnColorChanged(index >= 0 && index < 3);
            }
        }

        protected virtual void OnColorChanged(bool hsvToRgb)
        {
            _updating = true;

            if (hsvToRgb)
            {
                _rgba = (ARGBPixel) _hsv;
                _rgba.A = (byte) numA.Value;
            }
            else
            {
                _hsv = (HSVPixel) _rgba;
            }

            numH.Value = _hsv.H;
            numS.Value = _hsv.S;
            numV.Value = _hsv.V;
            numR.Value = _rgba.R;
            numG.Value = _rgba.G;
            numB.Value = _rgba.B;
            numA.Value = _rgba.A;

            txtColorCode.Text = _rgba.ToRGBAColorCode();

            _updating = false;

            pnlColorBox.Invalidate();
            pnlColorBar.Invalidate();

            ColorChanged?.Invoke(this, null);
        }

        private void txtColorCode_TextChanged(object sender, EventArgs e)
        {
            if (_updating) return;

            var s = "";
            foreach (var c in txtColorCode.Text)
                if (_allowed.IndexOf(c) >= 0)
                    s += c;

            s = s.Substring(0, s.Length.Clamp(0, 8));

            var focused = txtColorCode.Focused;
            var start = txtColorCode.SelectionStart;
            var len = txtColorCode.SelectionLength;

            _updating = true;
            if (txtColorCode.Text != s) txtColorCode.Text = s;

            _rgba.R = s.Length >= 2 ? byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber) : (byte) 0;
            _rgba.G = s.Length >= 4 ? byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber) : (byte) 0;
            _rgba.B = s.Length >= 6 ? byte.Parse(s.Substring(4, 2), NumberStyles.HexNumber) : (byte) 0;
            _rgba.A = s.Length >= 8 ? byte.Parse(s.Substring(6, 2), NumberStyles.HexNumber) : (byte) 0xFF;
            _updating = false;

            OnColorChanged(false);

            txtColorCode.SelectionStart = start;
            txtColorCode.SelectionLength = len;
            if (focused) txtColorCode.Select();
        }

        private void txtColorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            var box = txtColorCode;

            if (e.KeyChar == (char) Keys.Back && box.SelectionStart > 0)
            {
                var start = box.SelectionStart;
                var sb = new StringBuilder(box.Text);
                sb[start - 1] = '0';
                box.Text = sb.ToString();
                box.SelectionStart = start - 1;
                e.Handled = true;
            }
            else if ((!char.IsControl(c) || e.KeyChar == (char) Keys.Delete) && box.SelectionStart < box.TextLength)
            {
                if (_allowed.IndexOf(c) >= 0 || e.KeyChar == (char) Keys.Delete)
                {
                    var start = box.SelectionStart;
                    var sb = new StringBuilder(box.Text);
                    sb[start] = e.KeyChar == (char) Keys.Delete ? '0' : e.KeyChar;
                    box.Text = sb.ToString();
                    box.SelectionStart = start + 1;
                }

                e.Handled = true;
            }
        }

        #region Designer

        private BufferedPanel pnlColorBox;
        private BufferedPanel pnlColorBar;
        private Label lblR;
        private Label label1;
        private Label label2;
        private NumericUpDown numB;
        private NumericUpDown numG;
        private NumericUpDown numR;
        private NumericUpDown numH;
        private NumericUpDown numS;
        private NumericUpDown numV;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown numA;
        private Label lblA;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TextBox txtColorCode;
        private BufferedPanel pnlAlpha;

        private void InitializeComponent()
        {
            lblR = new Label();
            label1 = new Label();
            label2 = new Label();
            numB = new NumericUpDown();
            numG = new NumericUpDown();
            numR = new NumericUpDown();
            numH = new NumericUpDown();
            numS = new NumericUpDown();
            numV = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            numA = new NumericUpDown();
            lblA = new Label();
            panel1 = new Panel();
            pnlColorBox = new BufferedPanel();
            pnlColorBar = new BufferedPanel();
            panel2 = new Panel();
            pnlAlpha = new BufferedPanel();
            panel3 = new Panel();
            txtColorCode = new TextBox();
            ((ISupportInitialize) numB).BeginInit();
            ((ISupportInitialize) numG).BeginInit();
            ((ISupportInitialize) numR).BeginInit();
            ((ISupportInitialize) numH).BeginInit();
            ((ISupportInitialize) numS).BeginInit();
            ((ISupportInitialize) numV).BeginInit();
            ((ISupportInitialize) numA).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // lblR
            // 
            lblR.Font = new Font("Lucida Console", 9.75F, Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
            lblR.Location = new Drawing.Point(3, 77);
            lblR.Name = "lblR";
            lblR.Size = new Drawing.Size(19, 20);
            lblR.TabIndex = 2;
            lblR.Text = "R";
            lblR.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Font = new Font("Lucida Console", 9.75F, Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Drawing.Point(3, 115);
            label1.Name = "label1";
            label1.Size = new Drawing.Size(19, 20);
            label1.TabIndex = 3;
            label1.Text = "B";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Font = new Font("Lucida Console", 9.75F, Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Drawing.Point(3, 96);
            label2.Name = "label2";
            label2.Size = new Drawing.Size(19, 20);
            label2.TabIndex = 4;
            label2.Text = "G";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numB
            // 
            numB.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numB.Location = new Drawing.Point(23, 116);
            numB.Maximum = new decimal(new[]
            {
                255,
                0,
                0,
                0
            });
            numB.Name = "numB";
            numB.Size = new Drawing.Size(47, 20);
            numB.TabIndex = 5;
            // 
            // numG
            // 
            numG.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numG.Location = new Drawing.Point(23, 97);
            numG.Maximum = new decimal(new[]
            {
                255,
                0,
                0,
                0
            });
            numG.Name = "numG";
            numG.Size = new Drawing.Size(47, 20);
            numG.TabIndex = 6;
            // 
            // numR
            // 
            numR.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numR.Location = new Drawing.Point(23, 78);
            numR.Maximum = new decimal(new[]
            {
                255,
                0,
                0,
                0
            });
            numR.Name = "numR";
            numR.Size = new Drawing.Size(47, 20);
            numR.TabIndex = 7;
            // 
            // numH
            // 
            numH.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numH.Location = new Drawing.Point(23, 6);
            numH.Maximum = new decimal(new[]
            {
                360,
                0,
                0,
                0
            });
            numH.Name = "numH";
            numH.Size = new Drawing.Size(47, 20);
            numH.TabIndex = 13;
            // 
            // numS
            // 
            numS.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numS.Location = new Drawing.Point(23, 25);
            numS.Name = "numS";
            numS.Size = new Drawing.Size(47, 20);
            numS.TabIndex = 12;
            // 
            // numV
            // 
            numV.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numV.Location = new Drawing.Point(23, 44);
            numV.Name = "numV";
            numV.Size = new Drawing.Size(47, 20);
            numV.TabIndex = 11;
            // 
            // label3
            // 
            label3.Font = new Font("Lucida Console", 9.75F, Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Drawing.Point(3, 24);
            label3.Name = "label3";
            label3.Size = new Drawing.Size(19, 20);
            label3.TabIndex = 10;
            label3.Text = "S";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Font = new Font("Lucida Console", 9.75F, Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Drawing.Point(3, 43);
            label4.Name = "label4";
            label4.Size = new Drawing.Size(19, 20);
            label4.TabIndex = 9;
            label4.Text = "V";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Font = new Font("Lucida Console", 9.75F, Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Drawing.Point(3, 5);
            label5.Name = "label5";
            label5.Size = new Drawing.Size(19, 20);
            label5.TabIndex = 8;
            label5.Text = "H";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numA
            // 
            numA.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numA.Location = new Drawing.Point(23, 135);
            numA.Maximum = new decimal(new[]
            {
                255,
                0,
                0,
                0
            });
            numA.Name = "numA";
            numA.Size = new Drawing.Size(47, 20);
            numA.TabIndex = 15;
            numA.Value = new decimal(new[]
            {
                255,
                0,
                0,
                0
            });
            // 
            // lblA
            // 
            lblA.Font = new Font("Lucida Console", 9.75F, Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
            lblA.Location = new Drawing.Point(3, 134);
            lblA.Name = "lblA";
            lblA.Size = new Drawing.Size(19, 20);
            lblA.TabIndex = 14;
            lblA.Text = "A";
            lblA.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            panel1.Controls.Add(pnlColorBox);
            panel1.Controls.Add(pnlColorBar);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Drawing.Size(217, 187);
            panel1.TabIndex = 16;
            // 
            // pnlColorBox
            // 
            pnlColorBox.BackColor = Color.Transparent;
            pnlColorBox.Location = new Drawing.Point(3, 3);
            pnlColorBox.Name = "pnlColorBox";
            pnlColorBox.Size = new Drawing.Size(180, 180);
            pnlColorBox.TabIndex = 0;
            pnlColorBox.Paint += pnlColorBox_Paint;
            pnlColorBox.MouseDown += pnlColorBox_MouseDown;
            pnlColorBox.MouseMove += pnlColorBox_MouseMove;
            pnlColorBox.MouseUp += pnlColorBox_MouseUp;
            // 
            // pnlColorBar
            // 
            pnlColorBar.BackColor = Color.Transparent;
            pnlColorBar.Location = new Drawing.Point(189, 3);
            pnlColorBar.Name = "pnlColorBar";
            pnlColorBar.Size = new Drawing.Size(25, 180);
            pnlColorBar.TabIndex = 1;
            pnlColorBar.Paint += pnlColorBar_Paint;
            pnlColorBar.MouseDown += pnlColorBar_MouseDown;
            pnlColorBar.MouseMove += pnlColorBar_MouseMove;
            pnlColorBar.MouseUp += pnlColorBar_MouseUp;
            // 
            // panel2
            // 
            panel2.Controls.Add(pnlAlpha);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Drawing.Point(217, 0);
            panel2.Name = "panel2";
            panel2.Size = new Drawing.Size(20, 187);
            panel2.TabIndex = 17;
            // 
            // pnlAlpha
            // 
            pnlAlpha.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                               | AnchorStyles.Right;
            pnlAlpha.BackColor = Color.Transparent;
            pnlAlpha.Location = new Drawing.Point(3, 3);
            pnlAlpha.Name = "pnlAlpha";
            pnlAlpha.Size = new Drawing.Size(14, 180);
            pnlAlpha.TabIndex = 2;
            pnlAlpha.Paint += pnlAlpha_Paint;
            pnlAlpha.MouseDown += pnlAlpha_MouseDown;
            pnlAlpha.MouseMove += pnlAlpha_MouseMove;
            pnlAlpha.MouseUp += pnlAlpha_MouseUp;
            // 
            // panel3
            // 
            panel3.Controls.Add(txtColorCode);
            panel3.Controls.Add(numH);
            panel3.Controls.Add(lblR);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(numA);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(lblA);
            panel3.Controls.Add(numB);
            panel3.Controls.Add(numG);
            panel3.Controls.Add(numS);
            panel3.Controls.Add(numR);
            panel3.Controls.Add(numV);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label4);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Drawing.Point(237, 0);
            panel3.Name = "panel3";
            panel3.Size = new Drawing.Size(77, 187);
            panel3.TabIndex = 18;
            // 
            // txtColorCode
            // 
            txtColorCode.Location = new Drawing.Point(6, 161);
            txtColorCode.Name = "txtColorCode";
            txtColorCode.Size = new Drawing.Size(64, 20);
            txtColorCode.TabIndex = 16;
            txtColorCode.Text = "000000FF";
            txtColorCode.TextAlign = HorizontalAlignment.Center;
            txtColorCode.TextChanged += txtColorCode_TextChanged;
            txtColorCode.KeyPress += txtColorCode_KeyPress;
            // 
            // GoodColorControl
            // 
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MinimumSize = new Drawing.Size(310, 187);
            Name = "GoodColorControl";
            Size = new Drawing.Size(314, 187);
            ((ISupportInitialize) numB).EndInit();
            ((ISupportInitialize) numG).EndInit();
            ((ISupportInitialize) numR).EndInit();
            ((ISupportInitialize) numH).EndInit();
            ((ISupportInitialize) numS).EndInit();
            ((ISupportInitialize) numV).EndInit();
            ((ISupportInitialize) numA).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        #region ColorBox

        private void pnlColorBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _squareGrabbing = true;
                pnlColorBox_MouseMove(sender, e);
            }
        }

        private void pnlColorBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _squareGrabbing = false;
        }

        private void pnlColorBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_squareGrabbing)
            {
                var x = Math.Min(Math.Max(e.X, 0), pnlColorBox.Width);
                var y = Math.Min(Math.Max(e.Y, 0), pnlColorBox.Height);
                if (x != _squareX || y != _squareY)
                {
                    _squareX = x;
                    _squareY = y;

                    _hsv.V = (byte) ((float) x / pnlColorBox.Width * 100);
                    _hsv.S = (byte) ((float) (pnlColorBox.Height - y) / pnlColorBox.Height * 100);

                    OnColorChanged(true);
                }
            }
        }

        private void pnlColorBox_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            //Update brush if color changed
            if (_brushH != _hsv.H)
            {
                _boxColors[0] = _boxColors[4] = (Color) new HSVPixel(_hsv.H, 100, 100);
                _squareBrush.SurroundColors = _boxColors;
                _squareBrush.CenterColor = (Color) new HSVPixel(_hsv.H, 50, 50);
                _brushH = _hsv.H;
            }

            //Draw square
            //g.FillPath(_squareBrush, _squarePath);
            g.FillRectangle(_squareBrush, pnlColorBox.ClientRectangle);

            //Draw indicator
            var x = (int) (_hsv.V / 100.0f * pnlColorBox.Width);
            var y = (int) ((100 - _hsv.S) / 100.0f * pnlColorBox.Height);
            var r = new Rectangle(x - 3, y - 3, 6, 6);
            var p = _rgba.Inverse();
            p.A = 255;

            using (var pen = new Pen((Color) p))
            {
                g.DrawEllipse(pen, r);
            }

            r.X -= 1;
            r.Y -= 1;
            r.Width += 2;
            r.Height += 2;
            p = p.Lighten(64);

            using (var pen = new Pen((Color) p))
            {
                g.DrawEllipse(pen, r);
            }
        }

        #endregion

        #region HueBar

        private void pnlColorBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _barGrabbing = true;
                pnlColorBar_MouseMove(sender, e);
            }
        }

        private void pnlColorBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _barGrabbing = false;
        }

        private void pnlColorBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_barGrabbing)
            {
                var y = Math.Max(Math.Min(e.Y, pnlColorBar.Height - 1), 0);
                if (y != _barY)
                {
                    _barY = y;

                    _hsv.H = (ushort) ((float) y / (pnlColorBar.Height - 1) * 360);
                    OnColorChanged(true);
                }
            }
        }

        private void pnlColorBar_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            //Draw bar
            g.FillRectangle(_barBrush, pnlColorBar.ClientRectangle);

            //Draw indicator
            var p = ((ARGBPixel) new HSVPixel(_hsv.H, 100, 100)).Inverse();
            var y = (int) (_hsv.H / 360.0f * (pnlColorBar.Height - 1));
            var r = new Rectangle(-1, y - 2, pnlColorBar.Width + 1, 4);

            using (var pen = new Pen((Color) p))
            {
                g.DrawRectangle(pen, r);
            }

            r.Y += 1;
            r.Height -= 2;
            p = p.Lighten(64);

            using (var pen = new Pen((Color) p))
            {
                g.DrawRectangle(pen, r);
            }
        }

        #endregion

        #region AlphaBar

        private void pnlAlpha_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _alphaGrabbing = true;
                pnlAlpha_MouseMove(sender, e);
            }
        }

        private void pnlAlpha_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _alphaGrabbing = false;
        }

        private void pnlAlpha_MouseMove(object sender, MouseEventArgs e)
        {
            if (_alphaGrabbing)
            {
                var y = Math.Max(Math.Min(e.Y, pnlAlpha.Height - 1), 0);
                if (y != _alphaY)
                {
                    _alphaY = y;
                    numA.Value = (byte) (255 - (float) y / (pnlAlpha.Height - 1) * 255);
                    _updating = true;
                    txtColorCode.Text = _rgba.ToRGBAColorCode();
                    _updating = false;
                    ColorChanged?.Invoke(this, null);
                }
            }
        }

        private void pnlAlpha_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            //Draw bar
            g.FillRectangle(_alphaBrush, pnlAlpha.ClientRectangle);

            //Draw indicator
            var col = (byte) (255 - _rgba.A);
            var p = new ARGBPixel(255, col, col, col);
            var y = (int) (col / 255.0f * (pnlAlpha.Height - 1));
            var r = new Rectangle(-1, y - 2, pnlAlpha.Width + 1, 4);

            using (var pen = new Pen((Color) p))
            {
                g.DrawRectangle(pen, r);
            }

            p.Lighten(64);

            r.Y += 1;
            r.Height -= 2;

            using (var pen = new Pen((Color) p))
            {
                g.DrawRectangle(pen, r);
            }
        }

        #endregion
    }
}
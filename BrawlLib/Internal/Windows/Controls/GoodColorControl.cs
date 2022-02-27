using BrawlLib.Imaging;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class GoodColorControl : UserControl
    {
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
            ((System.ComponentModel.ISupportInitialize) numH).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numS).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numV).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numR).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numG).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numB).BeginInit();
            ((System.ComponentModel.ISupportInitialize) numA).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // numH
            // 
            numH.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numH.Location = new Point(23, 6);
            numH.Maximum = new decimal(new int[]
            {
                360,
                0,
                0,
                0
            });
            numH.Name = "numH";
            numH.Size = new Size(47, 20);
            numH.TabIndex = 0;
            // 
            // numS
            // 
            numS.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numS.Location = new Point(23, 25);
            numS.Name = "numS";
            numS.Size = new Size(47, 20);
            numS.TabIndex = 1;
            // 
            // numV
            // 
            numV.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numV.Location = new Point(23, 44);
            numV.Name = "numV";
            numV.Size = new Size(47, 20);
            numV.TabIndex = 2;
            // 
            // numR
            // 
            numR.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numR.Location = new Point(23, 78);
            numR.Maximum = new decimal(new int[]
            {
                255,
                0,
                0,
                0
            });
            numR.Name = "numR";
            numR.Size = new Size(47, 20);
            numR.TabIndex = 3;
            // 
            // numG
            // 
            numG.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numG.Location = new Point(23, 97);
            numG.Maximum = new decimal(new int[]
            {
                255,
                0,
                0,
                0
            });
            numG.Name = "numG";
            numG.Size = new Size(47, 20);
            numG.TabIndex = 4;
            // 
            // numB
            // 
            numB.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numB.Location = new Point(23, 116);
            numB.Maximum = new decimal(new int[]
            {
                255,
                0,
                0,
                0
            });
            numB.Name = "numB";
            numB.Size = new Size(47, 20);
            numB.TabIndex = 5;
            // 
            // numA
            // 
            numA.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                           | AnchorStyles.Right;
            numA.Location = new Point(23, 135);
            numA.Maximum = new decimal(new int[]
            {
                255,
                0,
                0,
                0
            });
            numA.Name = "numA";
            numA.Size = new Size(47, 20);
            numA.Value = new decimal(new int[]
            {
                255,
                0,
                0,
                0
            });
            numA.TabIndex = 6;
            // 
            // lblR
            // 
            lblR.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblR.Location = new Point(3, 77);
            lblR.Name = "lblR";
            lblR.Size = new Size(19, 20);
            lblR.Text = "R";
            lblR.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 115);
            label1.Name = "label1";
            label1.Size = new Size(19, 20);
            label1.Text = "B";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(3, 96);
            label2.Name = "label2";
            label2.Size = new Size(19, 20);
            label2.Text = "G";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(3, 24);
            label3.Name = "label3";
            label3.Size = new Size(19, 20);
            label3.Text = "S";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(3, 43);
            label4.Name = "label4";
            label4.Size = new Size(19, 20);
            label4.Text = "V";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 5);
            label5.Name = "label5";
            label5.Size = new Size(19, 20);
            label5.Text = "H";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblA
            // 
            lblA.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblA.Location = new Point(3, 134);
            lblA.Name = "lblA";
            lblA.Size = new Size(19, 20);
            lblA.Text = "A";
            lblA.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            panel1.Controls.Add(pnlColorBox);
            panel1.Controls.Add(pnlColorBar);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(217, 187);
            // 
            // pnlColorBox
            // 
            pnlColorBox.BackColor = Color.Transparent;
            pnlColorBox.Location = new Point(3, 3);
            pnlColorBox.Name = "pnlColorBox";
            pnlColorBox.Size = new Size(180, 180);
            pnlColorBox.Paint += new PaintEventHandler(pnlColorBox_Paint);
            pnlColorBox.MouseDown += new MouseEventHandler(pnlColorBox_MouseDown);
            pnlColorBox.MouseMove += new MouseEventHandler(pnlColorBox_MouseMove);
            pnlColorBox.MouseUp += new MouseEventHandler(pnlColorBox_MouseUp);
            // 
            // pnlColorBar
            // 
            pnlColorBar.BackColor = Color.Transparent;
            pnlColorBar.Location = new Point(189, 3);
            pnlColorBar.Name = "pnlColorBar";
            pnlColorBar.Size = new Size(25, 180);
            pnlColorBar.Paint += new PaintEventHandler(pnlColorBar_Paint);
            pnlColorBar.MouseDown += new MouseEventHandler(pnlColorBar_MouseDown);
            pnlColorBar.MouseMove += new MouseEventHandler(pnlColorBar_MouseMove);
            pnlColorBar.MouseUp += new MouseEventHandler(pnlColorBar_MouseUp);
            // 
            // panel2
            // 
            panel2.Controls.Add(pnlAlpha);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(217, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(20, 187);
            // 
            // pnlAlpha
            // 
            pnlAlpha.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                               | AnchorStyles.Right;
            pnlAlpha.BackColor = Color.Transparent;
            pnlAlpha.Location = new Point(3, 3);
            pnlAlpha.Name = "pnlAlpha";
            pnlAlpha.Size = new Size(14, 180);
            pnlAlpha.Paint += new PaintEventHandler(pnlAlpha_Paint);
            pnlAlpha.MouseDown += new MouseEventHandler(pnlAlpha_MouseDown);
            pnlAlpha.MouseMove += new MouseEventHandler(pnlAlpha_MouseMove);
            pnlAlpha.MouseUp += new MouseEventHandler(pnlAlpha_MouseUp);
            // 
            // panel3
            // 
            panel3.Controls.Add(numH);
            panel3.Controls.Add(numS);
            panel3.Controls.Add(numV);
            panel3.Controls.Add(numR);
            panel3.Controls.Add(numG);
            panel3.Controls.Add(lblA);
            panel3.Controls.Add(numB);
            panel3.Controls.Add(txtColorCode);
            panel3.Controls.Add(lblR);
            panel3.Controls.Add(numA);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label4);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(237, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(77, 187);
            // 
            // txtColorCode
            // 
            txtColorCode.Location = new Point(6, 161);
            txtColorCode.Name = "txtColorCode";
            txtColorCode.Size = new Size(64, 20);
            txtColorCode.Text = "000000FF";
            txtColorCode.TextAlign = HorizontalAlignment.Center;
            txtColorCode.TextChanged += new EventHandler(txtColorCode_TextChanged);
            txtColorCode.KeyPress += new KeyPressEventHandler(txtColorCode_KeyPress);
            txtColorCode.TabIndex = 7;
            // 
            // GoodColorControl
            // 
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MinimumSize = new Size(310, 187);
            Name = "GoodColorControl";
            Size = new Size(314, 187);
            ((System.ComponentModel.ISupportInitialize) numB).EndInit();
            ((System.ComponentModel.ISupportInitialize) numG).EndInit();
            ((System.ComponentModel.ISupportInitialize) numR).EndInit();
            ((System.ComponentModel.ISupportInitialize) numH).EndInit();
            ((System.ComponentModel.ISupportInitialize) numS).EndInit();
            ((System.ComponentModel.ISupportInitialize) numV).EndInit();
            ((System.ComponentModel.ISupportInitialize) numA).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        //private int _hue;
        private HSVPixel _hsv = new HSVPixel(0, 100, 100);

        //private int _alpha = 255;
        private ARGBPixel _rgba;
        private bool _squareGrabbing;
        private int _squareX, _squareY;
        private bool _barGrabbing;
        private int _barY;
        private bool _alphaGrabbing;
        private int _alphaY;
        private bool _updating;

        private readonly NumericUpDown[] _boxes;

        private int _brushH = -1;

        private readonly PathGradientBrush _squareBrush;

        //private GraphicsPath _squarePath;
        private readonly Color[] _boxColors = new Color[]
            {Color.Black, Color.White, Color.Black, Color.Black, Color.Black};

        private readonly LinearGradientBrush _barBrush;
        private readonly LinearGradientBrush _alphaBrush;

        public event EventHandler ColorChanged;

        private bool _showAlpha = true;

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

        public GoodColorControl()
        {
            InitializeComponent();

            _boxes = new NumericUpDown[] {numH, numS, numV, numR, numG, numB, numA};
            for (int i = 0; i < _boxes.Length; i++)
            {
                _boxes[i].ValueChanged += OnBoxChanged;
                _boxes[i].Tag = i;
            }

            Rectangle r = pnlColorBox.ClientRectangle;

            //_squarePath = new GraphicsPath();
            //_squarePath.AddRectangle(r);
            //_squareBrush = new PathGradientBrush(_squarePath);

            _squareBrush = new PathGradientBrush(new PointF[]
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

            float p = r.Height / 6.0f / r.Height;
            _barBrush = new LinearGradientBrush(new Rectangle(0, 0, r.Width, r.Height), Color.Red, Color.Red,
                LinearGradientMode.Vertical);

            ColorBlend blend = new ColorBlend
            {
                Colors = new Color[]
                    {Color.Red, Color.Yellow, Color.Lime, Color.Cyan, Color.Blue, Color.Magenta, Color.Red},
                Positions = new float[] {0, p, p * 2, p * 3, p * 4, p * 5, 1.0f}
            };

            _barBrush.InterpolationColors = blend;

            _alphaBrush = new LinearGradientBrush(new Rectangle(0, 0, pnlAlpha.Width, pnlAlpha.Height), Color.White,
                Color.Black, LinearGradientMode.Vertical);
        }

        protected void OnBoxChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            NumericUpDown box = sender as NumericUpDown;
            int value = (int) box.Value;
            int index = (int) box.Tag;
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
            if (e.Button == MouseButtons.Left)
            {
                _squareGrabbing = false;
            }
        }

        private void pnlColorBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_squareGrabbing)
            {
                int x = Math.Min(Math.Max(e.X, 0), pnlColorBox.Width);
                int y = Math.Min(Math.Max(e.Y, 0), pnlColorBox.Height);
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
            Graphics g = e.Graphics;

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
            int x = (int) (_hsv.V / 100.0f * pnlColorBox.Width);
            int y = (int) ((100 - _hsv.S) / 100.0f * pnlColorBox.Height);
            Rectangle r = new Rectangle(x - 3, y - 3, 6, 6);
            ARGBPixel p = _rgba.Inverse();
            p.A = 255;

            using (Pen pen = new Pen((Color) p))
            {
                g.DrawEllipse(pen, r);
            }

            r.X -= 1;
            r.Y -= 1;
            r.Width += 2;
            r.Height += 2;
            p = p.Lighten(64);

            using (Pen pen = new Pen((Color) p))
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
            if (e.Button == MouseButtons.Left)
            {
                _barGrabbing = false;
            }
        }

        private void pnlColorBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_barGrabbing)
            {
                int y = Math.Max(Math.Min(e.Y, pnlColorBar.Height - 1), 0);
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
            Graphics g = e.Graphics;

            //Draw bar
            g.FillRectangle(_barBrush, pnlColorBar.ClientRectangle);

            //Draw indicator
            ARGBPixel p = ((ARGBPixel) new HSVPixel(_hsv.H, 100, 100)).Inverse();
            int y = (int) (_hsv.H / 360.0f * (pnlColorBar.Height - 1));
            Rectangle r = new Rectangle(-1, y - 2, pnlColorBar.Width + 1, 4);

            using (Pen pen = new Pen((Color) p))
            {
                g.DrawRectangle(pen, r);
            }

            r.Y += 1;
            r.Height -= 2;
            p = p.Lighten(64);

            using (Pen pen = new Pen((Color) p))
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
            if (e.Button == MouseButtons.Left)
            {
                _alphaGrabbing = false;
            }
        }

        private void pnlAlpha_MouseMove(object sender, MouseEventArgs e)
        {
            if (_alphaGrabbing)
            {
                int y = Math.Max(Math.Min(e.Y, pnlAlpha.Height - 1), 0);
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
            Graphics g = e.Graphics;

            //Draw bar
            g.FillRectangle(_alphaBrush, pnlAlpha.ClientRectangle);

            //Draw indicator
            byte col = (byte) (255 - _rgba.A);
            ARGBPixel p = new ARGBPixel(255, col, col, col);
            int y = (int) (col / 255.0f * (pnlAlpha.Height - 1));
            Rectangle r = new Rectangle(-1, y - 2, pnlAlpha.Width + 1, 4);

            using (Pen pen = new Pen((Color) p))
            {
                g.DrawRectangle(pen, r);
            }

            p.Lighten(64);

            r.Y += 1;
            r.Height -= 2;

            using (Pen pen = new Pen((Color) p))
            {
                g.DrawRectangle(pen, r);
            }
        }

        #endregion

        private readonly string _allowed = "0123456789abcdefABCDEF";

        private void txtColorCode_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            string s = "";
            foreach (char c in txtColorCode.Text)
            {
                if (_allowed.IndexOf(c) >= 0)
                {
                    s += c;
                }
            }

            s = s.Substring(0, s.Length.Clamp(0, 8));

            bool focused = txtColorCode.Focused;
            int start = txtColorCode.SelectionStart;
            int len = txtColorCode.SelectionLength;

            _updating = true;
            if (txtColorCode.Text != s)
            {
                txtColorCode.Text = s;
            }

            _rgba.R = s.Length >= 2
                ? byte.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)
                : (byte) 0;
            _rgba.G = s.Length >= 4
                ? byte.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                : (byte) 0;
            _rgba.B = s.Length >= 6
                ? byte.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)
                : (byte) 0;
            _rgba.A = s.Length >= 8
                ? byte.Parse(s.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)
                : (byte) 0xFF;
            _updating = false;

            OnColorChanged(false);

            txtColorCode.SelectionStart = start;
            txtColorCode.SelectionLength = len;
            if (focused)
            {
                txtColorCode.Select();
            }
        }

        private void txtColorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            TextBox box = txtColorCode;

            if (e.KeyChar == (char) Keys.Back && box.SelectionStart > 0)
            {
                int start = box.SelectionStart;
                StringBuilder sb = new StringBuilder(box.Text);
                sb[start - 1] = '0';
                box.Text = sb.ToString();
                box.SelectionStart = start - 1;
                e.Handled = true;
            }
            else if ((!char.IsControl(c) || e.KeyChar == (char) Keys.Delete) && box.SelectionStart < box.TextLength)
            {
                if (_allowed.IndexOf(c) >= 0 || e.KeyChar == (char) Keys.Delete)
                {
                    int start = box.SelectionStart;
                    StringBuilder sb = new StringBuilder(box.Text);
                    sb[start] = e.KeyChar == (char) Keys.Delete ? '0' : e.KeyChar;
                    box.Text = sb.ToString();
                    box.SelectionStart = start + 1;
                }

                e.Handled = true;
            }
        }
    }
}
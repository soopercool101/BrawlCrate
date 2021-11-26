using BrawlLib.Imaging;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BrawlLib.Internal.Windows.Controls
{
    public delegate void ColorChanged(Color c);

    public class GoodColorControl2 : UserControl
    {
        #region Designer

        private Button btnOkay;
        private GoodColorControl goodColorControl1;
        private Panel pnlColors;
        private BufferedPanel pnlNew;
        private BufferedPanel pnlOld;
        private Label lblOld;
        private Label lblNew;
        private CheckBox chkAlpha;
        private Button btnCancel;

        private void InitializeComponent()
        {
            btnOkay = new Button();
            btnCancel = new Button();
            pnlColors = new Panel();
            lblOld = new Label();
            lblNew = new Label();
            chkAlpha = new CheckBox();
            pnlNew = new BufferedPanel();
            pnlOld = new BufferedPanel();
            goodColorControl1 = new GoodColorControl();
            pnlColors.SuspendLayout();
            SuspendLayout();
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOkay.Location = new Point(200, 221);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new Size(59, 24);
            btnOkay.TabIndex = 2;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(267, 221);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(59, 24);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // pnlColors
            // 
            pnlColors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                | AnchorStyles.Left;
            pnlColors.Controls.Add(pnlNew);
            pnlColors.Controls.Add(pnlOld);
            pnlColors.Location = new Point(14, 207);
            pnlColors.Name = "pnlColors";
            pnlColors.Size = new Size(180, 37);
            // 
            // lblOld
            // 
            lblOld.BackColor = Color.White;
            lblOld.BorderStyle = BorderStyle.FixedSingle;
            lblOld.Location = new Point(9, 198);
            lblOld.Name = "lblOld";
            lblOld.Size = new Size(35, 15);
            lblOld.Text = "Old";
            lblOld.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNew
            // 
            lblNew.BackColor = Color.White;
            lblNew.BorderStyle = BorderStyle.FixedSingle;
            lblNew.Location = new Point(99, 198);
            lblNew.Name = "lblNew";
            lblNew.Size = new Size(35, 15);
            lblNew.Text = "New";
            lblNew.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chkAlpha
            // 
            chkAlpha.Checked = true;
            chkAlpha.CheckState = CheckState.Checked;
            chkAlpha.Location = new Point(221, 200);
            chkAlpha.Name = "chkAlpha";
            chkAlpha.Size = new Size(90, 18);
            chkAlpha.TabIndex = 1;
            chkAlpha.Text = "Show Alpha";
            chkAlpha.UseVisualStyleBackColor = true;
            chkAlpha.CheckedChanged += new EventHandler(chkAlpha_CheckedChanged);
            // 
            // pnlNew
            // 
            pnlNew.Dock = DockStyle.Fill;
            pnlNew.Location = new Point(90, 0);
            pnlNew.Name = "pnlNew";
            pnlNew.Size = new Size(90, 37);
            pnlNew.Paint += new PaintEventHandler(pnlNew_Paint);
            // 
            // pnlOld
            // 
            pnlOld.Dock = DockStyle.Left;
            pnlOld.Location = new Point(0, 0);
            pnlOld.Name = "pnlOld";
            pnlOld.Size = new Size(90, 37);
            pnlOld.Paint += new PaintEventHandler(pnlOld_Paint);
            // 
            // goodColorControl1
            // 
            goodColorControl1.Color = Color.FromArgb(0, 0, 0, 0);
            goodColorControl1.Location = new Point(12, 10);
            goodColorControl1.MinimumSize = new Size(310, 186);
            goodColorControl1.Name = "goodColorControl1";
            goodColorControl1.ShowAlpha = true;
            goodColorControl1.Size = new Size(314, 186);
            goodColorControl1.TabIndex = 0;
            goodColorControl1.ColorChanged += new EventHandler(goodColorControl1_ColorChanged);
            // 
            // GoodColorControl2
            // 
            Controls.Add(chkAlpha);
            Controls.Add(lblNew);
            Controls.Add(lblOld);
            Controls.Add(pnlColors);
            Controls.Add(goodColorControl1);
            Controls.Add(btnCancel);
            Controls.Add(btnOkay);
            Name = "GoodColorControl2";
            Size = new Size(335, 253);
            pnlColors.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public delegate void ColorChangedEvent(Color selection);

        public event ColorChangedEvent OnColorChanged;

        private Color _color;
        private Color _newColor;

        public Color Color
        {
            get => _color;
            set
            {
                goodColorControl1.Color = _color = _newColor = value;
                pnlOld.Invalidate();
                pnlNew.Invalidate();
            }
        }

        public bool EditAlpha
        {
            get => chkAlpha.Visible;
            set
            {
                if (chkAlpha.Visible = goodColorControl1.ShowAlpha = value)
                {
                    Height = 287;
                }
                else
                {
                    Height = 287 - 20;
                }

                pnlOld.Invalidate();
                pnlNew.Invalidate();
            }
        }

        private bool _showOld;

        public bool ShowOldColor
        {
            get => _showOld;
            set => lblOld.Visible = lblNew.Visible = pnlOld.Visible = _showOld = value;
        }

        public GoodColorControl2()
        {
            InitializeComponent();
        }

        public event EventHandler Closed;

        public DialogResult DialogResult;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            _color = _newColor;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void pnlOld_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color c = _color;

            //Draw hatch
            if (chkAlpha.Checked && chkAlpha.Visible)
            {
                g.FillRectangle(GoodPictureBox._brush, pnlOld.ClientRectangle);
            }
            else
            {
                c = Color.FromArgb(c.R, c.G, c.B);
            }

            //Draw background
            using (Brush b = new SolidBrush(c))
            {
                g.FillRectangle(b, pnlOld.ClientRectangle);
            }
        }

        private void pnlNew_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color c = _newColor;

            //Draw hatch
            if (chkAlpha.Checked && chkAlpha.Visible)
            {
                g.FillRectangle(GoodPictureBox._brush, pnlNew.ClientRectangle);
            }
            else
            {
                c = Color.FromArgb(c.R, c.G, c.B);
            }

            //Draw background
            using (Brush b = new SolidBrush(c))
            {
                g.FillRectangle(b, pnlNew.ClientRectangle);
            }
        }

        private void goodColorControl1_ColorChanged(object sender, EventArgs e)
        {
            _newColor = goodColorControl1.Color;
            pnlNew.Invalidate();
            OnColorChanged?.Invoke(_newColor);
        }

        private void chkAlpha_CheckedChanged(object sender, EventArgs e)
        {
            pnlNew.Invalidate();
            pnlOld.Invalidate();
        }
    }

    internal class PropertyGridColorEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private IWindowsFormsEditorService _service;

        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            if (provider != null)
            {
                _service =
                    provider.GetService(
                            typeof(IWindowsFormsEditorService))
                        as IWindowsFormsEditorService;
            }

            if (_service != null)
            {
                GoodColorControl2 selectionControl = new GoodColorControl2();
                selectionControl.Closed += selectionControl_Closed;

                Type t = value.GetType();

                if (t == typeof(ARGBPixel))
                {
                    selectionControl.Color = (Color) (ARGBPixel) value;
                }
                else if (t == typeof(RGBAPixel))
                {
                    selectionControl.Color = (Color) (RGBAPixel) value;
                }
                else if (t == typeof(GXColorS10))
                {
                    selectionControl.Color = (Color) (GXColorS10) value;
                }

                _service.DropDownControl(selectionControl);

                if (selectionControl.DialogResult == DialogResult.OK)
                {
                    if (t == typeof(ARGBPixel))
                    {
                        value = (ARGBPixel) selectionControl.Color;
                    }
                    else if (t == typeof(RGBAPixel))
                    {
                        value = (RGBAPixel) (ARGBPixel) selectionControl.Color;
                    }
                    else if (t == typeof(GXColorS10))
                    {
                        value = (GXColorS10) (ARGBPixel) selectionControl.Color;
                    }
                }

                _service = null;
            }

            return value;
        }

        private void selectionControl_Closed(object sender, EventArgs e)
        {
            _service?.CloseDropDown();
        }
    }
}
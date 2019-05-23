using BrawlLib.Imaging;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace System.Windows.Forms
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
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlColors = new System.Windows.Forms.Panel();
            this.lblOld = new System.Windows.Forms.Label();
            this.lblNew = new System.Windows.Forms.Label();
            this.chkAlpha = new System.Windows.Forms.CheckBox();
            this.pnlNew = new System.Windows.Forms.BufferedPanel();
            this.pnlOld = new System.Windows.Forms.BufferedPanel();
            this.goodColorControl1 = new System.Windows.Forms.GoodColorControl();
            this.pnlColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(200, 221);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(59, 24);
            this.btnOkay.TabIndex = 0;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(267, 221);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlColors
            // 
            this.pnlColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlColors.Controls.Add(this.pnlNew);
            this.pnlColors.Controls.Add(this.pnlOld);
            this.pnlColors.Location = new System.Drawing.Point(14, 207);
            this.pnlColors.Name = "pnlColors";
            this.pnlColors.Size = new System.Drawing.Size(180, 37);
            this.pnlColors.TabIndex = 3;
            // 
            // lblOld
            // 
            this.lblOld.BackColor = System.Drawing.Color.White;
            this.lblOld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOld.Location = new System.Drawing.Point(9, 198);
            this.lblOld.Name = "lblOld";
            this.lblOld.Size = new System.Drawing.Size(35, 15);
            this.lblOld.TabIndex = 4;
            this.lblOld.Text = "Old";
            this.lblOld.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNew
            // 
            this.lblNew.BackColor = System.Drawing.Color.White;
            this.lblNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNew.Location = new System.Drawing.Point(99, 198);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(35, 15);
            this.lblNew.TabIndex = 5;
            this.lblNew.Text = "New";
            this.lblNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkAlpha
            // 
            this.chkAlpha.Checked = true;
            this.chkAlpha.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlpha.Location = new System.Drawing.Point(221, 200);
            this.chkAlpha.Name = "chkAlpha";
            this.chkAlpha.Size = new System.Drawing.Size(90, 18);
            this.chkAlpha.TabIndex = 6;
            this.chkAlpha.Text = "Show Alpha";
            this.chkAlpha.UseVisualStyleBackColor = true;
            this.chkAlpha.CheckedChanged += new System.EventHandler(this.chkAlpha_CheckedChanged);
            // 
            // pnlNew
            // 
            this.pnlNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNew.Location = new System.Drawing.Point(90, 0);
            this.pnlNew.Name = "pnlNew";
            this.pnlNew.Size = new System.Drawing.Size(90, 37);
            this.pnlNew.TabIndex = 6;
            this.pnlNew.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlNew_Paint);
            // 
            // pnlOld
            // 
            this.pnlOld.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlOld.Location = new System.Drawing.Point(0, 0);
            this.pnlOld.Name = "pnlOld";
            this.pnlOld.Size = new System.Drawing.Size(90, 37);
            this.pnlOld.TabIndex = 5;
            this.pnlOld.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlOld_Paint);
            // 
            // goodColorControl1
            // 
            this.goodColorControl1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.goodColorControl1.Location = new System.Drawing.Point(12, 10);
            this.goodColorControl1.MinimumSize = new System.Drawing.Size(310, 186);
            this.goodColorControl1.Name = "goodColorControl1";
            this.goodColorControl1.ShowAlpha = true;
            this.goodColorControl1.Size = new System.Drawing.Size(314, 186);
            this.goodColorControl1.TabIndex = 2;
            this.goodColorControl1.ColorChanged += new System.EventHandler(this.goodColorControl1_ColorChanged);
            // 
            // GoodColorControl2
            // 
            this.Controls.Add(this.chkAlpha);
            this.Controls.Add(this.lblNew);
            this.Controls.Add(this.lblOld);
            this.Controls.Add(this.pnlColors);
            this.Controls.Add(this.goodColorControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Name = "GoodColorControl2";
            this.Size = new System.Drawing.Size(335, 253);
            this.pnlColors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public delegate void ColorChangedEvent(Color selection);
        public event ColorChangedEvent OnColorChanged;

        private Color _color;
        private Color _newColor;
        public Color Color
        {
            get { return _color; }
            set 
            {
                goodColorControl1.Color = _color = _newColor = value;
                pnlOld.Invalidate();
                pnlNew.Invalidate();
            }
        }

        public bool EditAlpha
        {
            get { return chkAlpha.Visible; }
            set
            {
                if(chkAlpha.Visible = goodColorControl1.ShowAlpha = value)
                    this.Height = 287;
                else
                    this.Height = 287 - 20;

                pnlOld.Invalidate();
                pnlNew.Invalidate();
            }
        }

        private bool _showOld = false;
        public bool ShowOldColor
        {
            get { return _showOld; }
            set { lblOld.Visible = lblNew.Visible = pnlOld.Visible = _showOld = value; }
        }

        public GoodColorControl2() { InitializeComponent(); }

        public event EventHandler Closed;
        public event ColorChanged ColorChanged;

        public DialogResult DialogResult;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            if (Closed != null)
                Closed(this, EventArgs.Empty);
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            _color = _newColor;
            if (Closed != null)
                Closed(this, EventArgs.Empty);
        }

        private void pnlOld_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color c = _color;

            //Draw hatch
            if (chkAlpha.Checked && chkAlpha.Visible)
                g.FillRectangle(GoodPictureBox._brush, pnlOld.ClientRectangle);
            else
                c = Color.FromArgb(c.R, c.G, c.B);

            //Draw background
            using (Brush b = new SolidBrush(c))
                g.FillRectangle(b, pnlOld.ClientRectangle);
        }

        private void pnlNew_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color c = _newColor;

            //Draw hatch
            if (chkAlpha.Checked && chkAlpha.Visible)
                g.FillRectangle(GoodPictureBox._brush, pnlNew.ClientRectangle);
            else
                c = Color.FromArgb(c.R, c.G, c.B);

            //Draw background
            using (Brush b = new SolidBrush(c))
                g.FillRectangle(b, pnlNew.ClientRectangle);
        }

        private void goodColorControl1_ColorChanged(object sender, EventArgs e)
        {
            _newColor = goodColorControl1.Color;
            pnlNew.Invalidate();
            if (OnColorChanged != null)
                OnColorChanged(_newColor);
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

        IWindowsFormsEditorService _service = null;

        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            if (provider != null)
                _service =
                    provider.GetService(
                    typeof(IWindowsFormsEditorService))
                    as IWindowsFormsEditorService;

            if (_service != null)
            {
                GoodColorControl2 selectionControl = new GoodColorControl2();
                selectionControl.Closed += selectionControl_Closed;

                Type t = value.GetType();

                if (t == typeof(ARGBPixel))
                    selectionControl.Color = (Color)(ARGBPixel)value;
                else if (t == typeof(RGBAPixel))
                    selectionControl.Color = (Color)(RGBAPixel)value;
                else if (t == typeof(GXColorS10))
                    selectionControl.Color = (Color)(GXColorS10)value;

                _service.DropDownControl(selectionControl);

                if (selectionControl.DialogResult == DialogResult.OK)
                {
                    if (t == typeof(ARGBPixel))
                        value = (ARGBPixel)selectionControl.Color;
                    else if (t == typeof(RGBAPixel))
                        value = (RGBAPixel)(ARGBPixel)selectionControl.Color;
                    else if (t == typeof(GXColorS10))
                        value = (GXColorS10)(ARGBPixel)selectionControl.Color;
                }
                _service = null;
            }

            return value;
        }

        void selectionControl_Closed(object sender, EventArgs e)
        {
            if (_service != null)
                _service.CloseDropDown();
        }
    }
}

using BrawlLib.Imaging;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class GradientDialog : Form
    {
        #region Designer

        private Label lblStartText;
        private Label label2;
        private Label lblEndText;
        private Label label4;
        private Label lblEndColor;
        private Button btnOkay;
        private Button btnCancel;
        private BufferedPanel pnlPreview;
        private Button btnCopy;
        private Label lblStartColor;

        private void InitializeComponent()
        {
            this.lblStartText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStartColor = new System.Windows.Forms.Label();
            this.lblEndText = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblEndColor = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.pnlPreview = new System.Windows.Forms.BufferedPanel();
            this.SuspendLayout();
            // 
            // lblStartText
            // 
            this.lblStartText.BackColor = System.Drawing.Color.White;
            this.lblStartText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStartText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartText.Location = new System.Drawing.Point(69, 8);
            this.lblStartText.Name = "lblStartText";
            this.lblStartText.Size = new System.Drawing.Size(154, 20);
            this.lblStartText.TabIndex = 4;
            this.lblStartText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStartText.Click += new System.EventHandler(this.lblStartText_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(-1, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Start Color:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStartColor
            // 
            this.lblStartColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStartColor.Location = new System.Drawing.Point(222, 8);
            this.lblStartColor.Name = "lblStartColor";
            this.lblStartColor.Size = new System.Drawing.Size(40, 20);
            this.lblStartColor.TabIndex = 2;
            this.lblStartColor.Click += new System.EventHandler(this.lblStartText_Click);
            // 
            // lblEndText
            // 
            this.lblEndText.BackColor = System.Drawing.Color.White;
            this.lblEndText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEndText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndText.Location = new System.Drawing.Point(69, 52);
            this.lblEndText.Name = "lblEndText";
            this.lblEndText.Size = new System.Drawing.Size(154, 20);
            this.lblEndText.TabIndex = 4;
            this.lblEndText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEndText.Click += new System.EventHandler(this.lblEndText_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(2, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "End Color:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEndColor
            // 
            this.lblEndColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEndColor.Location = new System.Drawing.Point(222, 52);
            this.lblEndColor.Name = "lblEndColor";
            this.lblEndColor.Size = new System.Drawing.Size(40, 20);
            this.lblEndColor.TabIndex = 2;
            this.lblEndColor.Click += new System.EventHandler(this.lblEndText_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOkay.Location = new System.Drawing.Point(70, 119);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(65, 23);
            this.btnOkay.TabIndex = 6;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(141, 119);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(113, 30);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(50, 20);
            this.btnCopy.TabIndex = 9;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // pnlPreview
            // 
            this.pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreview.Location = new System.Drawing.Point(12, 81);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(250, 27);
            this.pnlPreview.TabIndex = 8;
            this.pnlPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPreview_Paint);
            // 
            // GradientDialog
            // 
            this.ClientSize = new System.Drawing.Size(276, 154);
            this.Controls.Add(this.lblStartColor);
            this.Controls.Add(this.lblStartText);
            this.Controls.Add(this.lblEndText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblEndColor);
            this.Controls.Add(this.pnlPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GradientDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Gradient Fill";
            this.ResumeLayout(false);

        }

        #endregion

        private Color _startColor;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color StartColor
        {
            get { return _startColor; }
            set { _startColor = value; UpdateStart(); }
        }

        private Color _endColor;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; UpdateEnd(); }
        }

        //private int _startIndex;
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public int StartIndex
        //{
        //    get { return _startIndex; }
        //    set { numStart.Value = _startIndex = value; }
        //}

        //private int _endIndex;
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public int EndIndex
        //{
        //    get { return _endIndex; }
        //    set { numStart.Value = _endIndex = value; }
        //}

        //private int _maxIndex;
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public int MaxIndex
        //{
        //    get { return _maxIndex; }
        //    set { numStart.Maximum = numEnd.Maximum = _maxIndex = value; }
        //}

        private GoodColorDialog _dlgColor;
        private LinearGradientBrush _gradBrush;

        public GradientDialog()
        {
            InitializeComponent();

            _dlgColor = new GoodColorDialog();
            _gradBrush = new LinearGradientBrush(new Rectangle(0, 0, pnlPreview.ClientRectangle.Width, pnlPreview.ClientRectangle.Height), Color.White, Color.Black, LinearGradientMode.Horizontal);
        }

        private void UpdateStart()
        {
            lblStartText.Text = ((ARGBPixel)_startColor).ToString();
            lblStartColor.BackColor = Color.FromArgb(_startColor.R, _startColor.G, _startColor.B);
            UpdateBrush();
        }
        private void UpdateEnd()
        {
            lblEndText.Text = ((ARGBPixel)_endColor).ToString();
            lblEndColor.BackColor = Color.FromArgb(_endColor.R, _endColor.G, _endColor.B);
            UpdateBrush();
        }
        private void UpdateBrush()
        {
            _gradBrush.LinearColors = new Color[] { _startColor, _endColor };
            pnlPreview.Invalidate();
        }

        private void numStart_ValueChanged(object sender, EventArgs e) { } // _startIndex = (int)numStart.Value; }
        private void lblStartText_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = _startColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _startColor = _dlgColor.Color;
                UpdateStart();
            }
        }

        private void numEnd_ValueChanged(object sender, EventArgs e) { } // _endIndex = (int)numEnd.Value; }
        private void lblEndText_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = _endColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _endColor = _dlgColor.Color;
                UpdateEnd();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            _endColor = _startColor;
            UpdateEnd();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void pnlPreview_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(GoodPictureBox._brush, pnlPreview.ClientRectangle);
            g.FillRectangle(_gradBrush, pnlPreview.ClientRectangle);

        }
    }
}

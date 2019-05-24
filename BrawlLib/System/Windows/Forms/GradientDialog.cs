using BrawlLib.Imaging;
using System.ComponentModel;
using System.Drawing;
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
            lblStartText = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblStartColor = new System.Windows.Forms.Label();
            lblEndText = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            lblEndColor = new System.Windows.Forms.Label();
            btnOkay = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            btnCopy = new System.Windows.Forms.Button();
            pnlPreview = new System.Windows.Forms.BufferedPanel();
            SuspendLayout();
            // 
            // lblStartText
            // 
            lblStartText.BackColor = System.Drawing.Color.White;
            lblStartText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblStartText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblStartText.Location = new System.Drawing.Point(69, 8);
            lblStartText.Name = "lblStartText";
            lblStartText.Size = new System.Drawing.Size(154, 20);
            lblStartText.TabIndex = 4;
            lblStartText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblStartText.Click += new System.EventHandler(lblStartText_Click);
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(-1, 8);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(71, 20);
            label2.TabIndex = 3;
            label2.Text = "Start Color:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStartColor
            // 
            lblStartColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblStartColor.Location = new System.Drawing.Point(222, 8);
            lblStartColor.Name = "lblStartColor";
            lblStartColor.Size = new System.Drawing.Size(40, 20);
            lblStartColor.TabIndex = 2;
            lblStartColor.Click += new System.EventHandler(lblStartText_Click);
            // 
            // lblEndText
            // 
            lblEndText.BackColor = System.Drawing.Color.White;
            lblEndText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblEndText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblEndText.Location = new System.Drawing.Point(69, 52);
            lblEndText.Name = "lblEndText";
            lblEndText.Size = new System.Drawing.Size(154, 20);
            lblEndText.TabIndex = 4;
            lblEndText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblEndText.Click += new System.EventHandler(lblEndText_Click);
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(2, 52);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(68, 20);
            label4.TabIndex = 3;
            label4.Text = "End Color:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEndColor
            // 
            lblEndColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblEndColor.Location = new System.Drawing.Point(222, 52);
            lblEndColor.Name = "lblEndColor";
            lblEndColor.Size = new System.Drawing.Size(40, 20);
            lblEndColor.TabIndex = 2;
            lblEndColor.Click += new System.EventHandler(lblEndText_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btnOkay.Location = new System.Drawing.Point(70, 119);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(65, 23);
            btnOkay.TabIndex = 6;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new System.EventHandler(btnOkay_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btnCancel.Location = new System.Drawing.Point(141, 119);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(65, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // btnCopy
            // 
            btnCopy.Location = new System.Drawing.Point(113, 30);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(50, 20);
            btnCopy.TabIndex = 9;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += new System.EventHandler(btnCopy_Click);
            // 
            // pnlPreview
            // 
            pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlPreview.Location = new System.Drawing.Point(12, 81);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new System.Drawing.Size(250, 27);
            pnlPreview.TabIndex = 8;
            pnlPreview.Paint += new System.Windows.Forms.PaintEventHandler(pnlPreview_Paint);
            // 
            // GradientDialog
            // 
            ClientSize = new System.Drawing.Size(276, 154);
            Controls.Add(lblStartColor);
            Controls.Add(lblStartText);
            Controls.Add(lblEndText);
            Controls.Add(label2);
            Controls.Add(btnCopy);
            Controls.Add(label4);
            Controls.Add(lblEndColor);
            Controls.Add(pnlPreview);
            Controls.Add(btnCancel);
            Controls.Add(btnOkay);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GradientDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Gradient Fill";
            ResumeLayout(false);

        }

        #endregion

        private Color _startColor;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color StartColor
        {
            get => _startColor;
            set { _startColor = value; UpdateStart(); }
        }

        private Color _endColor;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color EndColor
        {
            get => _endColor;
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

        private readonly GoodColorDialog _dlgColor;
        private readonly LinearGradientBrush _gradBrush;

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

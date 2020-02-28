using BrawlLib.Imaging;
using BrawlLib.Internal.Windows.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
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
        private CheckBox chkShowAlpha;
        private Label lblStartColor;

        private void InitializeComponent()
        {
            lblStartText = new Label();
            label2 = new Label();
            lblStartColor = new Label();
            lblEndText = new Label();
            label4 = new Label();
            lblEndColor = new Label();
            btnOkay = new Button();
            btnCancel = new Button();
            btnCopy = new Button();
            pnlPreview = new BufferedPanel();
            chkShowAlpha = new CheckBox();
            SuspendLayout();
            // 
            // lblStartText
            // 
            lblStartText.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            lblStartText.BackColor = Color.White;
            lblStartText.BorderStyle = BorderStyle.FixedSingle;
            lblStartText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lblStartText.Location = new Point(69, 8);
            lblStartText.Name = "lblStartText";
            lblStartText.Size = new Size(187, 20);
            lblStartText.TabIndex = 4;
            lblStartText.TextAlign = ContentAlignment.MiddleCenter;
            lblStartText.Click += new EventHandler(lblStartText_Click);
            // 
            // label2
            // 
            label2.Location = new Point(-1, 8);
            label2.Name = "label2";
            label2.Size = new Size(71, 20);
            label2.TabIndex = 3;
            label2.Text = "Start Color:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStartColor
            // 
            lblStartColor.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            lblStartColor.BorderStyle = BorderStyle.FixedSingle;
            lblStartColor.Location = new Point(255, 8);
            lblStartColor.Name = "lblStartColor";
            lblStartColor.Size = new Size(40, 20);
            lblStartColor.TabIndex = 2;
            lblStartColor.Click += new EventHandler(lblStartText_Click);
            // 
            // lblEndText
            // 
            lblEndText.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            lblEndText.BackColor = Color.White;
            lblEndText.BorderStyle = BorderStyle.FixedSingle;
            lblEndText.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lblEndText.Location = new Point(69, 52);
            lblEndText.Name = "lblEndText";
            lblEndText.Size = new Size(187, 20);
            lblEndText.TabIndex = 4;
            lblEndText.TextAlign = ContentAlignment.MiddleCenter;
            lblEndText.Click += new EventHandler(lblEndText_Click);
            // 
            // label4
            // 
            label4.Location = new Point(2, 52);
            label4.Name = "label4";
            label4.Size = new Size(68, 20);
            label4.TabIndex = 3;
            label4.Text = "End Color:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEndColor
            // 
            lblEndColor.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            lblEndColor.BorderStyle = BorderStyle.FixedSingle;
            lblEndColor.Location = new Point(255, 52);
            lblEndColor.Name = "lblEndColor";
            lblEndColor.Size = new Size(40, 20);
            lblEndColor.TabIndex = 2;
            lblEndColor.Click += new EventHandler(lblEndText_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom;
            btnOkay.Location = new Point(86, 119);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new Size(65, 23);
            btnOkay.TabIndex = 6;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom;
            btnCancel.Location = new Point(157, 119);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(65, 23);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // btnCopy
            // 
            btnCopy.Anchor = AnchorStyles.Top;
            btnCopy.Location = new Point(129, 30);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(50, 20);
            btnCopy.TabIndex = 9;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += new EventHandler(btnCopy_Click);
            // 
            // pnlPreview
            // 
            pnlPreview.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            pnlPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlPreview.Location = new Point(12, 81);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(283, 27);
            pnlPreview.TabIndex = 8;
            pnlPreview.Paint += new PaintEventHandler(pnlPreview_Paint);
            // 
            // chkShowAlpha
            // 
            chkShowAlpha.AutoSize = true;
            chkShowAlpha.Checked = true;
            chkShowAlpha.Location = new Point(209, 32);
            chkShowAlpha.Name = "chkShowAlpha";
            chkShowAlpha.Size = new Size(83, 17);
            chkShowAlpha.TabIndex = 10;
            chkShowAlpha.Text = "Show Alpha";
            chkShowAlpha.UseVisualStyleBackColor = true;
            chkShowAlpha.CheckedChanged += new EventHandler(chkShowAlpha_CheckedChanged);
            // 
            // GradientDialog
            // 
            ClientSize = new Size(309, 154);
            Controls.Add(chkShowAlpha);
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
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GradientDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Gradient Fill";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Color _startColor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color StartColor
        {
            get => _startColor;
            set
            {
                _startColor = value;
                UpdateStart(true);
            }
        }

        private Color _endColor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color EndColor
        {
            get => _endColor;
            set
            {
                _endColor = value;
                UpdateEnd(true);
            }
        }

        private readonly GoodColorDialog _dlgColor;
        private readonly LinearGradientBrush _gradBrush;

        public GradientDialog()
        {
            InitializeComponent();

            _dlgColor = new GoodColorDialog();
            _gradBrush =
                new LinearGradientBrush(
                    new Rectangle(0, 0, pnlPreview.ClientRectangle.Width, pnlPreview.ClientRectangle.Height),
                    Color.White, Color.Black, LinearGradientMode.Horizontal);
        }

        private void UpdateStart(bool updateGradient)
        {
            lblStartText.Text = ((ARGBPixel) _startColor).ToString();
            lblStartColor.BackColor = Color.FromArgb(chkShowAlpha.Checked ? _startColor.A : 255, _startColor.R, _startColor.G, _startColor.B);
            if (updateGradient)
            {
                UpdateBrush();
            }
        }

        private void UpdateEnd(bool updateGradient)
        {
            lblEndText.Text = ((ARGBPixel) _endColor).ToString();
            lblEndColor.BackColor = Color.FromArgb(chkShowAlpha.Checked ? _endColor.A : 255, _endColor.R, _endColor.G, _endColor.B);
            if (updateGradient)
            {
                UpdateBrush();
            }
        }

        private void UpdateBrush()
        {
            _gradBrush.LinearColors = new [] { lblStartColor.BackColor, lblEndColor.BackColor };
            pnlPreview.Invalidate();
        }

        private void lblStartText_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = _startColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                StartColor = _dlgColor.Color;
            }
        }

        private void lblEndText_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = _endColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                EndColor = _dlgColor.Color;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            _endColor = _startColor;
            UpdateEnd(true);
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

        private void chkShowAlpha_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStart(false);
            UpdateEnd(false);
            UpdateBrush();
        }
    }
}
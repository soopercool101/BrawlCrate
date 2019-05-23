using System;
using BrawlLib.Imaging;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class GradientDialog2 : Form
    {
        #region Designer

        private Button btnOkay;
        private Button btnCancel;
        private Panel startArrow;
        private Panel endArrow;
        private Panel panel3;
        private BufferedPanel pnlPreview;

        private void InitializeComponent()
        {
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlPreview = new System.Windows.Forms.BufferedPanel();
            this.startArrow = new System.Windows.Forms.Panel();
            this.endArrow = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOkay.Location = new System.Drawing.Point(193, 105);
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
            this.btnCancel.Location = new System.Drawing.Point(264, 105);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlPreview
            // 
            this.pnlPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreview.Location = new System.Drawing.Point(12, 12);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(499, 61);
            this.pnlPreview.TabIndex = 8;
            this.pnlPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPreview_Paint);
            // 
            // startArrow
            // 
            this.startArrow.Location = new System.Drawing.Point(0, 0);
            this.startArrow.Name = "startArrow";
            this.startArrow.Size = new System.Drawing.Size(24, 20);
            this.startArrow.TabIndex = 9;
            // 
            // endArrow
            // 
            this.endArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endArrow.Location = new System.Drawing.Point(475, 0);
            this.endArrow.Name = "endArrow";
            this.endArrow.Size = new System.Drawing.Size(24, 20);
            this.endArrow.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.startArrow);
            this.panel3.Controls.Add(this.endArrow);
            this.panel3.Location = new System.Drawing.Point(12, 79);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(499, 20);
            this.panel3.TabIndex = 11;
            this.panel3.DoubleClick += new System.EventHandler(this.panel3_DoubleClick);
            // 
            // GradientDialog2
            // 
            this.ClientSize = new System.Drawing.Size(523, 140);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnlPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(164, 164);
            this.Name = "GradientDialog2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Gradient Fill";
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected bool isDragging;
        private Point clickPosition;

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

        public GradientDialog2()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(Control_MouseMove);

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

        private void numStart_ValueChanged(object sender, EventArgs e) {}// _startIndex = (int)numStart.Value; }
        private void lblStartText_Click(object sender, EventArgs e)
        {
            _dlgColor.Color = _startColor;
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _startColor = _dlgColor.Color;
                UpdateStart();
            }
        }

        private void numEnd_ValueChanged(object sender, EventArgs e) {}// _endIndex = (int)numEnd.Value; }
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

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            var draggableControl = sender as UserControl;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as UserControl;

            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(this.Parent as UIElement);

                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;
            }
        }


        private void panel3_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}

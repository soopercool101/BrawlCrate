using System.Drawing;
namespace System.Windows.Forms
{
    public class GoodColorDialog : Form
    {
        #region Designer

        private GoodColorControl2 goodColorControl21;
    
        private void InitializeComponent()
        {
            this.goodColorControl21 = new System.Windows.Forms.GoodColorControl2();
            this.SuspendLayout();
            // 
            // goodColorControl21
            // 
            this.goodColorControl21.Color = System.Drawing.Color.Empty;
            this.goodColorControl21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goodColorControl21.EditAlpha = true;
            this.goodColorControl21.Location = new System.Drawing.Point(0, 0);
            this.goodColorControl21.Name = "goodColorControl21";
            //this.goodColorControl21.ShowOldColor = false;
            this.goodColorControl21.Size = new System.Drawing.Size(335, 253);
            this.goodColorControl21.TabIndex = 7;
            // 
            // GoodColorDialog
            // 
            this.ClientSize = new System.Drawing.Size(335, 253);
            this.Controls.Add(this.goodColorControl21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GoodColorDialog";
            this.ShowInTaskbar = false;
            this.Text = "Color Selector";
            this.ResumeLayout(false);

        }

        #endregion

        public event GoodColorControl2.ColorChangedEvent OnColorChanged;

        public Color Color
        {
            get { return goodColorControl21.Color; }
            set { goodColorControl21.Color = value; }
        }
        public bool EditAlpha
        {
            get { return goodColorControl21.EditAlpha; }
            set
            {
                if (goodColorControl21.EditAlpha = value)
                    this.Height = 287;
                else
                    this.Height = 267;
            }
        }
        public bool ShowOldColor
        {
            get { return goodColorControl21.ShowOldColor; }
            set { goodColorControl21.ShowOldColor = value; }
        }

        public GoodColorDialog()
        {
            InitializeComponent(); 
            goodColorControl21.Closed += goodColorControl21_Closed;
            goodColorControl21.OnColorChanged += goodColorControl21_ColorChanged;
        }

        void goodColorControl21_ColorChanged(Color c)
        {
            if (OnColorChanged != null)
                OnColorChanged(c);
        }

        void goodColorControl21_Closed(object sender, EventArgs e)
        {
            DialogResult = goodColorControl21.DialogResult;
            Close();
        }
    }
}

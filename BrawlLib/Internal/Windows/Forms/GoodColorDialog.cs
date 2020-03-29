using BrawlLib.Internal.Windows.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public class GoodColorDialog : Form
    {
        #region Designer

        private GoodColorControl2 goodColorControl21;

        private void InitializeComponent()
        {
            goodColorControl21 = new GoodColorControl2();
            SuspendLayout();
            // 
            // goodColorControl21
            // 
            goodColorControl21.Color = Color.Empty;
            goodColorControl21.Dock = DockStyle.Fill;
            goodColorControl21.EditAlpha = true;
            goodColorControl21.Location = new Point(0, 0);
            goodColorControl21.Name = "goodColorControl21";
            //this.goodColorControl21.ShowOldColor = false;
            goodColorControl21.Size = new Size(335, 253);
            goodColorControl21.TabIndex = 7;
            // 
            // GoodColorDialog
            // 
            ClientSize = new Size(335, 253);
            Controls.Add(goodColorControl21);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GoodColorDialog";
            ShowInTaskbar = false;
            Text = "Color Selector";
            ResumeLayout(false);
        }

        #endregion

        public event GoodColorControl2.ColorChangedEvent OnColorChanged;

        public Color Color
        {
            get => goodColorControl21.Color;
            set => goodColorControl21.Color = value;
        }

        public bool EditAlpha
        {
            get => goodColorControl21.EditAlpha;
            set
            {
                if (goodColorControl21.EditAlpha = value)
                {
                    Height = 287;
                }
                else
                {
                    Height = 267;
                }
            }
        }

        public bool ShowOldColor
        {
            get => goodColorControl21.ShowOldColor;
            set => goodColorControl21.ShowOldColor = value;
        }

        public GoodColorDialog()
        {
            InitializeComponent();
            goodColorControl21.Closed += goodColorControl21_Closed;
            goodColorControl21.OnColorChanged += goodColorControl21_ColorChanged;
        }

        private void goodColorControl21_ColorChanged(Color c)
        {
            OnColorChanged?.Invoke(c);
        }

        private void goodColorControl21_Closed(object sender, EventArgs e)
        {
            DialogResult = goodColorControl21.DialogResult;
            Close();
        }
    }
}
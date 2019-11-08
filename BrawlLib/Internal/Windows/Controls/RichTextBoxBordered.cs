using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public partial class RichTextBoxBordered : UserControl
    {
        public override string Text
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }

        public RichTextBoxBordered()
        {
            InitializeComponent();

            Paint += new PaintEventHandler(UserControl1_Paint);
            Resize += new EventHandler(UserControl1_Resize);
        }

        private void UserControl1_Resize(object sender, EventArgs e)
        {
            textBox.Size = new Size(Width - 4, Height - 2);
            textBox.Location = new Point(2, 1);
        }

        public Color _borderColor = Color.Green;

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, _borderColor, ButtonBorderStyle.Solid);
        }

        public new event EventHandler TextChanged;

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }
    }
}
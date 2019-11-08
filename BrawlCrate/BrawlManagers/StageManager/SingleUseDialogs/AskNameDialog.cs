using BrawlLib.BrawlManagerLib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs
{
    public partial class AskNameDialog : Form
    {
        public AskNameDialog(Bitmap bg)
        {
            InitializeComponent();
            panel1.BackgroundImage = bg;
            panel1.Width = bg.Width;
            panel1.Height = bg.Height;
            textBox1.Text = BitmapUtilities.HasAlpha(bg) && BitmapUtilities.HasNonAlpha(bg)
                ? "MenSelmapMark."
                : "MenSelchrMark.";
            textBox1.Select(textBox1.Text.Length, 0);
            AcceptButton = button1;
        }

        public string NameText
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NameText = Text;
        }
    }
}
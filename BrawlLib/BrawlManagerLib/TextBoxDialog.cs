using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib
{
    public partial class TextBoxDialog : Form
    {
        public string DisplayText
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public TextBoxDialog()
        {
            InitializeComponent();
        }

        public static void ShowDialog(string text, string title = "")
        {
            new TextBoxDialog
            {
                DisplayText = text,
                Text = title
            }.ShowDialog();
        }
    }
}
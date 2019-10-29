using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.StageManager
{
    public partial class EnterSizeDialog : ThemedForm
    {
        public Size SizeEntry
        {
            get => new Size((int) nudWidth.Value, (int) nudHeight.Value);
            set
            {
                nudWidth.Value = value.Width;
                nudHeight.Value = value.Height;
            }
        }

        public EnterSizeDialog()
        {
            InitializeComponent();
        }
    }
}
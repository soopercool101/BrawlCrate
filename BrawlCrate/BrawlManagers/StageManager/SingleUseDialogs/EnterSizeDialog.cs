using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs
{
    public partial class EnterSizeDialog : Form
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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib
{
    public partial class CopyDialog : Form
    {
        public CopyDialog(string pacNew, string pacExisting)
        {
            InitializeComponent();

            CopyDialog dialog = this;
            dialog.lblPacNewName.Text = Path.GetFileName(pacNew);
            dialog.lblPacNewMD5.Text = ByteUtilities.MD5Sum(pacNew);
            dialog.lblPacExistingName.Text = Path.GetFileName(pacExisting);
            dialog.lblPacExistingMD5.Text = ByteUtilities.MD5Sum(pacExisting);

            if (dialog.lblPacNewMD5.Text == dialog.lblPacExistingMD5.Text)
            {
                dialog.lblPacExistingMD5.ForeColor = dialog.lblPacNewMD5.ForeColor = Color.Green;
            }
        }
    }
}
using System;
using System.Audio;
using System.Windows.Forms;

namespace BrawlBox
{
    public partial class AboutForm : Form
    {
        private static AboutForm _instance;
        public static AboutForm Instance { get { return _instance == null ? _instance = new AboutForm() : _instance; } }

        public AboutForm()
        {
            InitializeComponent();
            this.lblName.Text = Program.AssemblyTitle;
            this.txtDescription.Text = Program.AssemblyDescription;
            this.lblCopyright.Text = Program.AssemblyCopyright;
            this.lblBrawlLib.Text = "Using " + Program.BrawlLibTitle;

            AudioProvider provider = AudioProvider.Create(null);
            this.lblAudioBackend.Text = "Audio backend: " + (provider?.ToString() ?? "none");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

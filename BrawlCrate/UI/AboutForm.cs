using BrawlLib.Internal.Audio;
using System;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public partial class AboutForm : Form
    {
        private static AboutForm _instance;
        public static AboutForm Instance => _instance ?? (_instance = new AboutForm());

        public AboutForm()
        {
            InitializeComponent();
            lblName.Text = Program.AssemblyTitleFull;
            txtDescription.Text = Program.AssemblyDescription;
            lblCopyright.Text = Program.AssemblyCopyright;
            lblBrawlLib.Text = "Using " + Program.BrawlLibTitle;

            AudioProvider provider = AudioProvider.Create(null);
            lblAudioBackend.Text = "Audio backend: " + (provider?.ToString() ?? "none");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
using System;
using System.Audio;
using System.Windows.Forms;

namespace BrawlCrate
{
    public partial class AboutForm : Form
    {
        private static AboutForm _instance;

        public AboutForm()
        {
            InitializeComponent();
            lblName.Text = Program.AssemblyTitle;
            txtDescription.Text = Program.AssemblyDescription;
            lblCopyright.Text = Program.AssemblyCopyright;
            lblBrawlLib.Text = "Using " + Program.BrawlLibTitle;

            var provider = AudioProvider.Create(null);
            lblAudioBackend.Text = "Audio backend: " + (provider?.ToString() ?? "none");
        }

        public static AboutForm Instance => _instance ?? (_instance = new AboutForm());

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
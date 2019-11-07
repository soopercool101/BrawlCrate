using System;
using System.Windows.Forms;

namespace Updater.UI
{
    public partial class CanaryChangelogViewer : Form
    {
        public CanaryChangelogViewer(string commitID, string changelog)
        {
            InitializeComponent();
            Text = "Canary Changelog #" + commitID;
            richTextBox1.Text =
                "Here's what's changed since your last update (oldest to newest). Full changelog can be found on Github or Discord";
            richTextBox1.Text += changelog;
            richTextBox1.ReadOnly = true;
        }

        private void CanaryChangelogViewer_Load(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
        }

        protected override void OnShown(EventArgs e)
        {
            Focus();
            base.OnShown(e);
        }
    }
}
using System;
using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public string ProgressTitle
        {
            get => Text;
            set => Text = value;
        }

        public string InProgressLabel
        {
            get => ProgressLabel.Text;
            set => ProgressLabel.Text = value;
        }

        public int ProgressCompletionAt
        {
            get => ProgressBar.Maximum;
            set
            {
                ProgressBar.Maximum = value;
                CheckOkButtonEnabled();
            }
        }

        public int Progress
        {
            get => ProgressBar.Value;
            set
            {
                ProgressBar.Value = value;
                CheckOkButtonEnabled();
            }
        }

        public void AppendLogLine(string value)
        {
            if (logTextBox.Text.Length == 0)
            {
                logTextBox.Text = value;
            }
            else
            {
                logTextBox.AppendText("\n" + value);
            }
        }

        public void ClearLog()
        {
            logTextBox.Clear();
        }

        private void CheckOkButtonEnabled()
        {
            okButton.Enabled = Progress >= ProgressCompletionAt;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
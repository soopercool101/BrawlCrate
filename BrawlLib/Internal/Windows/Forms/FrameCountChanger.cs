using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class FrameCountChanger : Form
    {
        public FrameCountChanger()
        {
            InitializeComponent();
        }

        public int NewValue => (int) numNewCount.Value;

        public DialogResult ShowDialog(int frameCount)
        {
            lblPrevCount.Text = (numNewCount.Value = frameCount).ToString();
            return ShowDialog();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
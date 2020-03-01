using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class PAT0OffsetControl : Form
    {
        public static readonly string Title = "PAT0 Offset";

        public PAT0OffsetControl()
        {
            InitializeComponent();
        }

        public int NewValue => (int) numNewCount.Value;
        public bool IncreaseFrames => chkIncreaseFrames.Checked;
        public bool OffsetOtherTextures => chkOffsetOtherTextures.Checked;

        public new DialogResult ShowDialog()
        {
            Text = Title;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle)
        {
            Text = newTitle;
            return base.ShowDialog();
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
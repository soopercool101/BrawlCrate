namespace System.Windows.Forms
{
    public partial class FrameCountChanger : Form
    {
        public FrameCountChanger()
        {
            InitializeComponent();
        }

        public int NewValue { get { return (int)numNewCount.Value; } }
        public DialogResult ShowDialog(int frameCount)
        {
            lblPrevCount.Text = (numNewCount.Value = frameCount).ToString();
            return base.ShowDialog();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = Forms.DialogResult.Cancel;
            Close();
        }
    }
}

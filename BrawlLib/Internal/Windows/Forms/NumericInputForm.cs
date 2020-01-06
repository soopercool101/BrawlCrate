using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class NumericInputForm : Form
    {
        public string title = "Numeric Entry Box";
        public string lowerText = "Error; No arguments given";

        public NumericInputForm()
        {
            InitializeComponent();
        }

        public int NewValue => (int) numNewCount.Value;

        public float NewFloatValue => numNewCount.Value;

        public new DialogResult ShowDialog()
        {
            Text = title;
            label2.Text = lowerText;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle, string newLower)
        {
            Text = newTitle;
            label2.Text = newLower;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle, string newLower, float val)
        {
            Text = newTitle;
            label2.Text = newLower;
            numNewCount.Value = val;
            return base.ShowDialog();
        }

        public bool Cancellable
        {
            get => btnCancel.Enabled && btnCancel.Visible;
            set
            {
                btnCancel.Enabled = value;
                btnCancel.Visible = value;
            }
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
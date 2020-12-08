using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public class StringInputDialog : Form
    {
        public StringInputDialog()
        {
            InitializeComponent();
        }

        public StringInputDialog(string title, string defaultText)
        {
            InitializeComponent();
            Text = title;
            txtName.Text = defaultText;
        }

        public string resultString = null;

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
            resultString = txtName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        #region Designer

        private TextBox txtName;
        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            txtName = new TextBox();
            btnCancel = new Button();
            btnOkay = new Button();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.HideSelection = false;
            txtName.Location = new System.Drawing.Point(12, 12);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(260, 20);
            txtName.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(197, 38);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOkay.Location = new System.Drawing.Point(116, 38);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // StringInputDialog
            // 
            AcceptButton = btnOkay;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(284, 69);
            Controls.Add(btnOkay);
            Controls.Add(btnCancel);
            Controls.Add(txtName);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "StringInputDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "String Input Dialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
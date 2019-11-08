using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib
{
    public class NameDialog : Form
    {
        public string EntryText
        {
            get => txtName.Text;
            set => txtName.Text = value;
        }

        public string LabelText
        {
            get => label1.Text;
            set
            {
                label1.Text = value;
                int h = 125;
                foreach (char c in value)
                {
                    if (c == '\n')
                    {
                        h += 13;
                    }
                }

                if (h > Height)
                {
                    Height = h;
                }
            }
        }

        public NameDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, string text)
        {
            Text = text;
            return ShowDialog(owner);
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

        private void btnCharmap_Click(object sender, EventArgs e)
        {
            Process.Start("charmap.exe");
        }

        #region Designer

        private TextBox txtName;
        private Button btnCancel;
        private Label label1;
        private Button btnCharmap;
        private Button btnOkay;

        private void InitializeComponent()
        {
            txtName = new TextBox();
            btnCancel = new Button();
            btnOkay = new Button();
            label1 = new Label();
            btnCharmap = new Button();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                 | AnchorStyles.Right);
            txtName.HideSelection = false;
            txtName.Location = new System.Drawing.Point(12, 25);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(260, 20);
            txtName.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(197, 51);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnOkay.Location = new System.Drawing.Point(116, 51);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 3;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(19, 13);
            label1.TabIndex = 0;
            label1.Text = "    ";
            // 
            // btnCharmap
            // 
            btnCharmap.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left);
            btnCharmap.Location = new System.Drawing.Point(12, 51);
            btnCharmap.Name = "btnCharmap";
            btnCharmap.Size = new System.Drawing.Size(75, 23);
            btnCharmap.TabIndex = 2;
            btnCharmap.Text = "Char. Map";
            btnCharmap.UseVisualStyleBackColor = true;
            btnCharmap.Click += new EventHandler(btnCharmap_Click);
            // 
            // NameDialog
            // 
            AcceptButton = btnOkay;
            AutoSize = true;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(284, 86);
            Controls.Add(btnCharmap);
            Controls.Add(label1);
            Controls.Add(btnOkay);
            Controls.Add(btnCancel);
            Controls.Add(txtName);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "NameDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Enter Name";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
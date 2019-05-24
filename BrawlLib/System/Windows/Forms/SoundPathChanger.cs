using System.ComponentModel;

namespace System.Windows.Forms
{
    internal class SoundPathChanger : Form
    {
        #region Designer

        private TextBox txtPath;
        private Button btnOkay;
        private Button btnCancel;
        private Button btnBrowse;
        private Label label1;

        private void InitializeComponent()
        {
            txtPath = new System.Windows.Forms.TextBox();
            btnOkay = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            btnBrowse = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // txtPath
            // 
            txtPath.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            txtPath.Location = new System.Drawing.Point(12, 34);
            txtPath.Name = "txtPath";
            txtPath.Size = new System.Drawing.Size(330, 20);
            txtPath.TabIndex = 0;
            txtPath.TextChanged += new System.EventHandler(txtPath_TextChanged);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btnOkay.Location = new System.Drawing.Point(117, 65);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new System.EventHandler(btnOkay_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btnCancel.Location = new System.Drawing.Point(198, 65);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(367, 21);
            label1.TabIndex = 3;
            label1.Text = "*Changing the path on an internal file will remove it from the RSAR*";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new System.Drawing.Point(348, 34);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(31, 20);
            btnBrowse.TabIndex = 4;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += new System.EventHandler(btnBrowse_Click);
            // 
            // SoundPathChanger
            // 
            ClientSize = new System.Drawing.Size(391, 100);
            Controls.Add(btnBrowse);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnOkay);
            Controls.Add(txtPath);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "SoundPathChanger";
            Text = "File Path";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private string _filePath = "";
        public string FilePath { get => _filePath; set => _filePath = value; }

        public SoundPathChanger() { InitializeComponent(); dlg.FileOk += OnFileOk; }
        ~SoundPathChanger() { dlg.FileOk -= OnFileOk; }

        protected override void OnShown(EventArgs e)
        {
            txtPath.Text = _filePath;
            base.OnShown(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            _filePath = txtPath.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        public void OnFileOk(object sender, CancelEventArgs e)
        {
            if (!dlg.FileName.StartsWith(dlg.InitialDirectory))
            {
                dlg.FileName = dlg.InitialDirectory;
            }
        }

        public OpenFileDialog dlg = new OpenFileDialog()
        {
            DefaultExt =
                "All RSAR Files (*.brstm, *.rwsd, *.rbnk, *.rseq)|*.brstm;*.rwsd;*.rbnk;*.rseq|" +
                "BRSTM Audio (*.brstm)|*.brstm|" +
                "Raw Sound Pack (*.rwsd)|*.rwsd|" +
                "Raw Sound Bank (*.rbnk)|*.rbnk|" +
                "Raw Sound Requence (*.rseq)|*.rseq"
        };
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dlg.FileName;
            }
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (!txtPath.Text.StartsWith(dlg.InitialDirectory))
            {
                txtPath.Text = dlg.InitialDirectory;
            }
        }
    }
}

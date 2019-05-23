using System.ComponentModel;

namespace System.Windows.Forms
{
    class SoundPathChanger : Form
    {
        #region Designer

        private TextBox txtPath;
        private Button btnOkay;
        private Button btnCancel;
        private Button btnBrowse;
        private Label label1;
    
        private void InitializeComponent()
        {
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(12, 34);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(330, 20);
            this.txtPath.TabIndex = 0;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOkay.Location = new System.Drawing.Point(117, 65);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(198, 65);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(367, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "*Changing the path on an internal file will remove it from the RSAR*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(348, 34);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(31, 20);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // SoundPathChanger
            // 
            this.ClientSize = new System.Drawing.Size(391, 100);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SoundPathChanger";
            this.Text = "File Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private string _filePath = "";
        public string FilePath { get { return _filePath; } set { _filePath = value; } }

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
                dlg.FileName = dlg.InitialDirectory;
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
                txtPath.Text = dlg.FileName;
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (!txtPath.Text.StartsWith(dlg.InitialDirectory))
                txtPath.Text = dlg.InitialDirectory;
        }
    }
}

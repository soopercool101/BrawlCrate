namespace System.Windows.Forms
{
    /// <summary>
    /// Summary description for FormGoTo.
    /// </summary>
    public class FormGoTo : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private GroupBox groupBox2;
        private RadioButton chkEnd;
        private RadioButton chkCurrent;
        private RadioButton chkBegin;
        private GroupBox groupBox3;
        private RadioButton chkDec;
        private RadioButton chkHex;
        private Label label1;
        private TextBox txtOffset;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.Container components = null;

        public FormGoTo()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            chkEnd = new System.Windows.Forms.RadioButton();
            chkCurrent = new System.Windows.Forms.RadioButton();
            chkBegin = new System.Windows.Forms.RadioButton();
            groupBox3 = new System.Windows.Forms.GroupBox();
            chkDec = new System.Windows.Forms.RadioButton();
            chkHex = new System.Windows.Forms.RadioButton();
            label1 = new System.Windows.Forms.Label();
            txtOffset = new System.Windows.Forms.TextBox();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(164, 188);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // btnOK
            // 
            btnOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(84, 188);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.Click += new System.EventHandler(btnOK_Click);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkEnd);
            groupBox2.Controls.Add(chkCurrent);
            groupBox2.Controls.Add(chkBegin);
            groupBox2.Location = new System.Drawing.Point(11, 89);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(225, 93);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Offset relative to";
            // 
            // chkEnd
            // 
            chkEnd.AutoSize = true;
            chkEnd.Location = new System.Drawing.Point(7, 68);
            chkEnd.Name = "chkEnd";
            chkEnd.Size = new System.Drawing.Size(109, 17);
            chkEnd.TabIndex = 2;
            chkEnd.Text = "End (Backwards)";
            chkEnd.UseVisualStyleBackColor = true;
            // 
            // chkCurrent
            // 
            chkCurrent.AutoSize = true;
            chkCurrent.Location = new System.Drawing.Point(7, 45);
            chkCurrent.Name = "chkCurrent";
            chkCurrent.Size = new System.Drawing.Size(97, 17);
            chkCurrent.TabIndex = 1;
            chkCurrent.Text = "Current offset";
            chkCurrent.UseVisualStyleBackColor = true;
            // 
            // chkBegin
            // 
            chkBegin.AutoSize = true;
            chkBegin.Checked = true;
            chkBegin.Location = new System.Drawing.Point(7, 22);
            chkBegin.Name = "chkBegin";
            chkBegin.Size = new System.Drawing.Size(55, 17);
            chkBegin.TabIndex = 0;
            chkBegin.TabStop = true;
            chkBegin.Text = "Begin";
            chkBegin.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(chkDec);
            groupBox3.Controls.Add(chkHex);
            groupBox3.Location = new System.Drawing.Point(12, 47);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(224, 36);
            groupBox3.TabIndex = 13;
            groupBox3.TabStop = false;
            // 
            // chkDec
            // 
            chkDec.AutoSize = true;
            chkDec.Location = new System.Drawing.Point(56, 12);
            chkDec.Name = "chkDec";
            chkDec.Size = new System.Drawing.Size(65, 17);
            chkDec.TabIndex = 4;
            chkDec.Text = "Decimal";
            chkDec.UseVisualStyleBackColor = true;
            // 
            // chkHex
            // 
            chkHex.AutoSize = true;
            chkHex.Checked = true;
            chkHex.Location = new System.Drawing.Point(6, 12);
            chkHex.Name = "chkHex";
            chkHex.Size = new System.Drawing.Size(44, 17);
            chkHex.TabIndex = 3;
            chkHex.TabStop = true;
            chkHex.Text = "Hex";
            chkHex.UseVisualStyleBackColor = true;
            chkHex.CheckedChanged += new System.EventHandler(chkHex_CheckedChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(42, 13);
            label1.TabIndex = 14;
            label1.Text = "Offset:";
            // 
            // txtOffset
            // 
            txtOffset.Location = new System.Drawing.Point(12, 25);
            txtOffset.Name = "txtOffset";
            txtOffset.Size = new System.Drawing.Size(224, 22);
            txtOffset.TabIndex = 15;
            txtOffset.TextChanged += new System.EventHandler(textBox1_TextChanged);
            // 
            // FormGoTo
            // 
            AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(248, 218);
            Controls.Add(txtOffset);
            Controls.Add(label1);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            Font = new System.Drawing.Font("Segoe UI", 8.25F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormGoTo";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Goto";
            Activated += new System.EventHandler(FormGoTo_Activated);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        private long _maxVal = long.MaxValue;
        private long current;
        public void SetDefaultValue(long byteIndex)
        {
            current = byteIndex;
            txtOffset.Text = chkHex.Checked ? byteIndex.ToString("X") : byteIndex.ToString();
        }

        public void SetMaxByteIndex(long maxByteIndex)
        {
            _maxVal = maxByteIndex;
        }

        public long GetByteIndex()
        {
            long v = Convert.ToInt64(txtOffset.Text, chkHex.Checked ? 16 : 10);
            return (chkBegin.Checked ? v : chkCurrent.Checked ? current + v : _maxVal - v).Clamp(0, _maxVal);
        }

        private void FormGoTo_Activated(object sender, System.EventArgs e)
        {
            txtOffset.Focus();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public char[] _hexChars = "0123456789ABCDEFabcdef".ToCharArray();
        public char[] _decChars = "0123456789".ToCharArray();
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            char[] arr = chkHex.Checked ? _hexChars : _decChars;

            string t = "";
            foreach (char c in txtOffset.Text)
            {
                if (Array.IndexOf(arr, c) != -1)
                {
                    t += c;
                }
            }

            if (txtOffset.Text != t)
            {
                int i = txtOffset.SelectionStart;
                txtOffset.Text = t;
                if (!string.IsNullOrEmpty(t))
                {
                    txtOffset.Select(i.Clamp(0, txtOffset.TextLength), 0);
                }
            }
        }

        private void chkHex_CheckedChanged(object sender, EventArgs e)
        {
            int i = txtOffset.SelectionStart;
            if (chkHex.Checked)
            {
                long v = Convert.ToInt64(txtOffset.Text, 10);
                txtOffset.Text = v.ToString("X");
            }
            else
            {
                long v = Convert.ToInt64(txtOffset.Text, 16);
                txtOffset.Text = v.ToString();
            }
            if (!string.IsNullOrEmpty(txtOffset.Text))
            {
                txtOffset.Select(i.Clamp(0, txtOffset.TextLength), 0);
            }
        }
    }
}

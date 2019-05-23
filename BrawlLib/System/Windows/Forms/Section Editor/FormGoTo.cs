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
		private System.ComponentModel.Container components = null;

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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkEnd = new System.Windows.Forms.RadioButton();
            this.chkCurrent = new System.Windows.Forms.RadioButton();
            this.chkBegin = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkDec = new System.Windows.Forms.RadioButton();
            this.chkHex = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(164, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(84, 188);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkEnd);
            this.groupBox2.Controls.Add(this.chkCurrent);
            this.groupBox2.Controls.Add(this.chkBegin);
            this.groupBox2.Location = new System.Drawing.Point(11, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 93);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Offset relative to";
            // 
            // chkEnd
            // 
            this.chkEnd.AutoSize = true;
            this.chkEnd.Location = new System.Drawing.Point(7, 68);
            this.chkEnd.Name = "chkEnd";
            this.chkEnd.Size = new System.Drawing.Size(109, 17);
            this.chkEnd.TabIndex = 2;
            this.chkEnd.Text = "End (Backwards)";
            this.chkEnd.UseVisualStyleBackColor = true;
            // 
            // chkCurrent
            // 
            this.chkCurrent.AutoSize = true;
            this.chkCurrent.Location = new System.Drawing.Point(7, 45);
            this.chkCurrent.Name = "chkCurrent";
            this.chkCurrent.Size = new System.Drawing.Size(97, 17);
            this.chkCurrent.TabIndex = 1;
            this.chkCurrent.Text = "Current offset";
            this.chkCurrent.UseVisualStyleBackColor = true;
            // 
            // chkBegin
            // 
            this.chkBegin.AutoSize = true;
            this.chkBegin.Checked = true;
            this.chkBegin.Location = new System.Drawing.Point(7, 22);
            this.chkBegin.Name = "chkBegin";
            this.chkBegin.Size = new System.Drawing.Size(55, 17);
            this.chkBegin.TabIndex = 0;
            this.chkBegin.TabStop = true;
            this.chkBegin.Text = "Begin";
            this.chkBegin.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkDec);
            this.groupBox3.Controls.Add(this.chkHex);
            this.groupBox3.Location = new System.Drawing.Point(12, 47);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(224, 36);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            // 
            // chkDec
            // 
            this.chkDec.AutoSize = true;
            this.chkDec.Location = new System.Drawing.Point(56, 12);
            this.chkDec.Name = "chkDec";
            this.chkDec.Size = new System.Drawing.Size(65, 17);
            this.chkDec.TabIndex = 4;
            this.chkDec.Text = "Decimal";
            this.chkDec.UseVisualStyleBackColor = true;
            // 
            // chkHex
            // 
            this.chkHex.AutoSize = true;
            this.chkHex.Checked = true;
            this.chkHex.Location = new System.Drawing.Point(6, 12);
            this.chkHex.Name = "chkHex";
            this.chkHex.Size = new System.Drawing.Size(44, 17);
            this.chkHex.TabIndex = 3;
            this.chkHex.TabStop = true;
            this.chkHex.Text = "Hex";
            this.chkHex.UseVisualStyleBackColor = true;
            this.chkHex.CheckedChanged += new System.EventHandler(this.chkHex_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Offset:";
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(12, 25);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(224, 22);
            this.txtOffset.TabIndex = 15;
            this.txtOffset.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // FormGoTo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(248, 218);
            this.Controls.Add(this.txtOffset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGoTo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Goto";
            this.Activated += new System.EventHandler(this.FormGoTo_Activated);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        long _maxVal = long.MaxValue;
        long current;
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
                if (Array.IndexOf(arr, c) != -1)
                    t += c;
            if (txtOffset.Text != t)
            {
                int i = txtOffset.SelectionStart;
                txtOffset.Text = t;
                if (!String.IsNullOrEmpty(t))
                    txtOffset.Select(i.Clamp(0, txtOffset.TextLength), 0);
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
            if (!String.IsNullOrEmpty(txtOffset.Text))
                txtOffset.Select(i.Clamp(0, txtOffset.TextLength), 0);
        }
	}
}

namespace BrawlStageManager.SingleUseDialogs {
	partial class RandomSelectEditNamesDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.nameList = new System.Windows.Forms.ListBox();
			this.icon = new System.Windows.Forms.PictureBox();
			this.frontstname = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.frontstname)).BeginInit();
			this.SuspendLayout();
			// 
			// nameList
			// 
			this.nameList.Dock = System.Windows.Forms.DockStyle.Left;
			this.nameList.FormattingEnabled = true;
			this.nameList.IntegralHeight = false;
			this.nameList.Location = new System.Drawing.Point(0, 0);
			this.nameList.Name = "nameList";
			this.nameList.Size = new System.Drawing.Size(120, 261);
			this.nameList.TabIndex = 0;
			// 
			// icon
			// 
			this.icon.Location = new System.Drawing.Point(126, 12);
			this.icon.Name = "icon";
			this.icon.Size = new System.Drawing.Size(64, 56);
			this.icon.TabIndex = 1;
			this.icon.TabStop = false;
			// 
			// frontstname
			// 
			this.frontstname.Location = new System.Drawing.Point(196, 12);
			this.frontstname.Name = "frontstname";
			this.frontstname.Size = new System.Drawing.Size(208, 56);
			this.frontstname.TabIndex = 2;
			this.frontstname.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(126, 74);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(222, 20);
			this.textBox1.TabIndex = 3;
			// 
			// btnReset
			// 
			this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReset.Location = new System.Drawing.Point(354, 73);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(50, 21);
			this.btnReset.TabIndex = 4;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(248, 226);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 5;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(329, 226);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMessage.Location = new System.Drawing.Point(126, 101);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(278, 122);
			this.lblMessage.TabIndex = 7;
			// 
			// RandomSelectEditNamesDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(416, 261);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.frontstname);
			this.Controls.Add(this.icon);
			this.Controls.Add(this.nameList);
			this.Name = "RandomSelectEditNamesDialog";
			this.Text = "Edit Names in Random Select";
			((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.frontstname)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox nameList;
		private System.Windows.Forms.PictureBox icon;
		private System.Windows.Forms.PictureBox frontstname;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblMessage;

	}
}
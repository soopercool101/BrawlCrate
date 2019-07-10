namespace BrawlStageManager {
	partial class ModifyPAT0Dialog {
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
			this.selchrLabel = new System.Windows.Forms.Label();
			this.selchrBox = new System.Windows.Forms.ComboBox();
			this.selmapLabel = new System.Windows.Forms.Label();
			this.selmapBox = new System.Windows.Forms.ComboBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOkay = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// selchrLabel
			// 
			this.selchrLabel.AutoSize = true;
			this.selchrLabel.Location = new System.Drawing.Point(12, 9);
			this.selchrLabel.Name = "selchrLabel";
			this.selchrLabel.Size = new System.Drawing.Size(139, 13);
			this.selchrLabel.TabIndex = 0;
			this.selchrLabel.Text = "MenSelchrMark for stage #:";
			// 
			// selchrBox
			// 
			this.selchrBox.FormattingEnabled = true;
			this.selchrBox.Location = new System.Drawing.Point(15, 25);
			this.selchrBox.Name = "selchrBox";
			this.selchrBox.Size = new System.Drawing.Size(257, 21);
			this.selchrBox.TabIndex = 1;
			// 
			// selmapLabel
			// 
			this.selmapLabel.AutoSize = true;
			this.selmapLabel.Location = new System.Drawing.Point(15, 53);
			this.selmapLabel.Name = "selmapLabel";
			this.selmapLabel.Size = new System.Drawing.Size(144, 13);
			this.selmapLabel.TabIndex = 2;
			this.selmapLabel.Text = "MenSelmapMark for stage #:";
			// 
			// selmapBox
			// 
			this.selmapBox.FormattingEnabled = true;
			this.selmapBox.Location = new System.Drawing.Point(13, 70);
			this.selmapBox.Name = "selmapBox";
			this.selmapBox.Size = new System.Drawing.Size(259, 21);
			this.selmapBox.TabIndex = 3;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(12, 97);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOkay
			// 
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(153, 97);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(119, 23);
			this.btnOkay.TabIndex = 5;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
			// 
			// ModifyPAT0Dialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(284, 132);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.selmapBox);
			this.Controls.Add(this.selmapLabel);
			this.Controls.Add(this.selchrBox);
			this.Controls.Add(this.selchrLabel);
			this.Name = "ModifyPAT0Dialog";
			this.Text = "ModifyPAT0Dialog";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label selchrLabel;
		private System.Windows.Forms.ComboBox selchrBox;
		private System.Windows.Forms.Label selmapLabel;
		private System.Windows.Forms.ComboBox selmapBox;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOkay;
	}
}
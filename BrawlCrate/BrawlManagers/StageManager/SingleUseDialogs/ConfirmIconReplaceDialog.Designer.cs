namespace BrawlStageManager {
	partial class ConfirmIconReplaceDialog {
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOkay = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(12, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(64, 56);
			this.panel1.TabIndex = 0;
			this.panel1.Visible = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 106);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Replace the icon?";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Current";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(86, 71);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "New";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(117, 126);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(36, 126);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 0;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Location = new System.Drawing.Point(86, 12);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(64, 56);
			this.panel2.TabIndex = 1;
			this.panel2.Visible = false;
			// 
			// ConfirmIconReplaceDialog
			// 
			this.AcceptButton = this.btnOkay;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(204, 161);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "ConfirmIconReplaceDialog";
			this.Text = "ConfirmIconReplaceDialog";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Panel panel2;
	}
}
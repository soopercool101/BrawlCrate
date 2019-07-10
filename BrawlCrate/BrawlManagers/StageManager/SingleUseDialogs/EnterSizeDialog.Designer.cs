namespace BrawlStageManager {
	partial class EnterSizeDialog {
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
			this.lblWidth = new System.Windows.Forms.Label();
			this.lblHeight = new System.Windows.Forms.Label();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			this.SuspendLayout();
			// 
			// lblWidth
			// 
			this.lblWidth.Location = new System.Drawing.Point(12, 9);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(50, 20);
			this.lblWidth.TabIndex = 0;
			this.lblWidth.Text = "Width:";
			// 
			// lblHeight
			// 
			this.lblHeight.Location = new System.Drawing.Point(12, 32);
			this.lblHeight.Name = "lblHeight";
			this.lblHeight.Size = new System.Drawing.Size(50, 20);
			this.lblHeight.TabIndex = 1;
			this.lblHeight.Text = "Height:";
			// 
			// nudWidth
			// 
			this.nudWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nudWidth.Location = new System.Drawing.Point(68, 9);
			this.nudWidth.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
			this.nudWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(100, 20);
			this.nudWidth.TabIndex = 2;
			this.nudWidth.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// nudHeight
			// 
			this.nudHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nudHeight.Location = new System.Drawing.Point(68, 30);
			this.nudHeight.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
			this.nudHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(100, 20);
			this.nudHeight.TabIndex = 3;
			this.nudHeight.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(12, 58);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 4;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(93, 58);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// EnterSizeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(180, 93);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.nudHeight);
			this.Controls.Add(this.nudWidth);
			this.Controls.Add(this.lblHeight);
			this.Controls.Add(this.lblWidth);
			this.Name = "EnterSizeDialog";
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.Label lblHeight;
		private System.Windows.Forms.NumericUpDown nudWidth;
		private System.Windows.Forms.NumericUpDown nudHeight;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;

	}
}
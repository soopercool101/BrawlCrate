namespace BrawlCostumeManager {
	partial class SinglePortraitViewer {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.additionalTexturesPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 40);
			this.label1.TabIndex = 1;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(0, 40);
			this.saveButton.Margin = new System.Windows.Forms.Padding(0);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(128, 23);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.label1);
			this.flowLayoutPanel1.Controls.Add(this.additionalTexturesPanel);
			this.flowLayoutPanel1.Controls.Add(this.saveButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(128, 240);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// additionalTexturesPanel
			// 
			this.additionalTexturesPanel.AutoSize = true;
			this.additionalTexturesPanel.Location = new System.Drawing.Point(0, 40);
			this.additionalTexturesPanel.Margin = new System.Windows.Forms.Padding(0);
			this.additionalTexturesPanel.Name = "additionalTexturesPanel";
			this.additionalTexturesPanel.Size = new System.Drawing.Size(0, 0);
			this.additionalTexturesPanel.TabIndex = 3;
			// 
			// SinglePortraitViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowLayoutPanel1);
			this.Name = "SinglePortraitViewer";
			this.Size = new System.Drawing.Size(128, 240);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel additionalTexturesPanel;
	}
}

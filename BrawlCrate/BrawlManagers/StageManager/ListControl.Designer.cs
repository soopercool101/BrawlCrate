namespace BrawlStageManager {
	partial class ListControl {
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
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.msBinEditor1 = new BrawlStageManager.MSBinViewer();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(150, 21);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.msBinEditor1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 21);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(150, 129);
			this.panel1.TabIndex = 1;
			// 
			// msBinEditor1
			// 
			this.msBinEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.msBinEditor1.Location = new System.Drawing.Point(0, 0);
			this.msBinEditor1.Name = "msBinEditor1";
			this.msBinEditor1.Size = new System.Drawing.Size(150, 129);
			this.msBinEditor1.TabIndex = 0;
			// 
			// ListControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.comboBox1);
			this.Name = "ListControl";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Panel panel1;
		private BrawlStageManager.MSBinViewer msBinEditor1;
	}
}

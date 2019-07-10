using System.Windows.Forms;
namespace BrawlCostumeManager {
	partial class ModelManager {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelManager));
			this.modelPanel1 = new System.Windows.Forms.ModelPanel();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// modelPanel1
			// 
			this.modelPanel1.AllowDrop = true;
			this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modelPanel1.Location = new System.Drawing.Point(0, 21);
			this.modelPanel1.Name = "modelPanel1";
			this.modelPanel1.Size = new System.Drawing.Size(292, 252);
			this.modelPanel1.TabIndex = 0;
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(292, 21);
			this.comboBox1.TabIndex = 2;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// ModelManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.modelPanel1);
			this.Controls.Add(this.comboBox1);
			this.Name = "ModelManager";
			this.Size = new System.Drawing.Size(292, 273);
			this.ResumeLayout(false);

		}

		#endregion

		private ModelPanel modelPanel1;
		private ComboBox comboBox1;
	}
}


namespace BrawlSongManager {
	partial class CustomSongVolumeEditor {
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
            this.components = new System.ComponentModel.Container();
            this.btnAdd = new System.Windows.Forms.Button();
            this.nudVolume = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblSongID = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.Location = new System.Drawing.Point(67, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(56, 20);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Remove";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // nudVolume
            // 
            this.nudVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nudVolume.Location = new System.Drawing.Point(129, 0);
            this.nudVolume.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nudVolume.Name = "nudVolume";
            this.nudVolume.Size = new System.Drawing.Size(95, 20);
            this.nudVolume.TabIndex = 1;
            this.nudVolume.ValueChanged += new System.EventHandler(this.nudVolume_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lblSongID
            // 
            this.lblSongID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSongID.Location = new System.Drawing.Point(25, 0);
            this.lblSongID.Name = "lblSongID";
            this.lblSongID.Size = new System.Drawing.Size(98, 20);
            this.lblSongID.TabIndex = 3;
            this.lblSongID.Text = "EEEE";
            this.lblSongID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CustomSongVolumeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblSongID);
            this.Controls.Add(this.nudVolume);
            this.Name = "CustomSongVolumeEditor";
            this.Size = new System.Drawing.Size(227, 20);
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.NumericUpDown nudVolume;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label lblSongID;
	}
}

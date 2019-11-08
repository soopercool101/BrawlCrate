using BrawlLib.Internal.Windows.Controls;

namespace BrawlLib.BrawlManagerLib.Songs
{
    partial class SongPanel {
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
            this.grid = new System.Windows.Forms.PropertyGrid();
            this.app = new AudioPlaybackPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.songNameBar = new BrawlLib.BrawlManagerLib.Songs.SongNameBar();
            this.lblFilename = new System.Windows.Forms.Label();
            this.nudVolume = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.HelpVisible = false;
            this.grid.Location = new System.Drawing.Point(0, 20);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(300, 169);
            this.grid.TabIndex = 0;
            // 
            // app
            // 
            this.app.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.app.Location = new System.Drawing.Point(0, 189);
            this.app.Name = "app";
            this.app.Size = new System.Drawing.Size(300, 111);
            this.app.TabIndex = 1;
            this.app.TargetStreams = null;
            this.app.AudioEnded += new System.EventHandler(this.app_AudioEnded);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.songNameBar);
            this.panel1.Controls.Add(this.lblFilename);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 20);
            this.panel1.TabIndex = 3;
            // 
            // songNameBar
            // 
            this.songNameBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.songNameBar.Index = 0;
            this.songNameBar.Location = new System.Drawing.Point(26, 0);
            this.songNameBar.Name = "songNameBar";
            this.songNameBar.Size = new System.Drawing.Size(274, 20);
            this.songNameBar.TabIndex = 2;
            // 
            // lblFilename
            // 
            this.lblFilename.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFilename.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilename.Location = new System.Drawing.Point(0, 0);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(26, 20);
            this.lblFilename.TabIndex = 3;
            this.lblFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudVolume
            // 
            this.nudVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudVolume.Enabled = false;
            this.nudVolume.Location = new System.Drawing.Point(258, 22);
            this.nudVolume.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nudVolume.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudVolume.Name = "nudVolume";
            this.nudVolume.Size = new System.Drawing.Size(40, 20);
            this.nudVolume.TabIndex = 4;
            this.nudVolume.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nudVolume.ValueChanged += new System.EventHandler(this.nudVolume_ValueChanged);
            // 
            // SongPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudVolume);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.app);
            this.Controls.Add(this.panel1);
            this.Name = "SongPanel";
            this.Size = new System.Drawing.Size(300, 300);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudVolume)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid grid;
        private AudioPlaybackPanel app;
        private BrawlLib.BrawlManagerLib.Songs.SongNameBar songNameBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.NumericUpDown nudVolume;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

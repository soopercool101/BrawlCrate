namespace BrawlSongManager {
	partial class MainForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customSongVolumeEditor1 = new BrawlSongManager.CustomSongVolumeEditor();
            this.songPanel1 = new BrawlManagerLib.SongPanel();
            this.rightLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFallbackDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveInfopacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGCTCodesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.use16ptFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadNamesFromInfopacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadBRSTMPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whenSongEndsStartPlayingNextSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.groupSongsByStageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlyShowSongsWithCSVCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateMumenumainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMusicSongsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importMusicSongsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultSongsListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerTop = new System.Windows.Forms.SplitContainer();
            this.customSongVolumeStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.customSongVolumeEditor1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.songPanel1);
            this.splitContainer1.Panel2.Controls.Add(this.rightLabel);
            this.splitContainer1.Size = new System.Drawing.Size(592, 290);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(180, 269);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // customSongVolumeEditor1
            // 
            this.customSongVolumeEditor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.customSongVolumeEditor1.CSV = null;
            this.customSongVolumeEditor1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.customSongVolumeEditor1.Location = new System.Drawing.Point(0, 269);
            this.customSongVolumeEditor1.Name = "customSongVolumeEditor1";
            this.customSongVolumeEditor1.Size = new System.Drawing.Size(180, 21);
            this.customSongVolumeEditor1.Song = null;
            this.customSongVolumeEditor1.SongFilename = null;
            this.customSongVolumeEditor1.TabIndex = 2;
            this.customSongVolumeEditor1.Value = ((byte)(0));
            this.customSongVolumeEditor1.VolumeIcon = ((System.Drawing.Image)(resources.GetObject("customSongVolumeEditor1.VolumeIcon")));
            this.customSongVolumeEditor1.ValueChanged += new System.EventHandler(this.customSongVolumeEditor1_ValueChanged);
            // 
            // songPanel1
            // 
            this.songPanel1.AllowDrop = true;
            this.songPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songPanel1.Location = new System.Drawing.Point(0, 0);
            this.songPanel1.Name = "songPanel1";
            this.songPanel1.ShowVolumeSpinner = true;
            this.songPanel1.Size = new System.Drawing.Size(408, 290);
            this.songPanel1.TabIndex = 1;
            this.songPanel1.AudioEnded += new System.EventHandler(this.songPanel1_AudioEnded);
            // 
            // rightLabel
            // 
            this.rightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightLabel.Location = new System.Drawing.Point(0, 0);
            this.rightLabel.Name = "rightLabel";
            this.rightLabel.Size = new System.Drawing.Size(408, 290);
            this.rightLabel.TabIndex = 0;
            this.rightLabel.Text = "Right Label";
            this.rightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.statusToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(592, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeDirectoryToolStripMenuItem,
            this.openFallbackDirectoryToolStripMenuItem,
            this.saveInfopacToolStripMenuItem,
            this.saveGCTCodesetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // changeDirectoryToolStripMenuItem
            // 
            this.changeDirectoryToolStripMenuItem.Name = "changeDirectoryToolStripMenuItem";
            this.changeDirectoryToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.changeDirectoryToolStripMenuItem.Text = "Change directory";
            this.changeDirectoryToolStripMenuItem.Click += new System.EventHandler(this.changeDirectoryToolStripMenuItem_Click);
            // 
            // openFallbackDirectoryToolStripMenuItem
            // 
            this.openFallbackDirectoryToolStripMenuItem.Name = "openFallbackDirectoryToolStripMenuItem";
            this.openFallbackDirectoryToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.openFallbackDirectoryToolStripMenuItem.Text = "Open fallback directory";
            this.openFallbackDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openFallbackDirectoryToolStripMenuItem_Click);
            // 
            // saveInfopacToolStripMenuItem
            // 
            this.saveInfopacToolStripMenuItem.Name = "saveInfopacToolStripMenuItem";
            this.saveInfopacToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveInfopacToolStripMenuItem.Text = "Save info.pac";
            this.saveInfopacToolStripMenuItem.Click += new System.EventHandler(this.saveInfopacToolStripMenuItem_Click);
            // 
            // saveGCTCodesetToolStripMenuItem
            // 
            this.saveGCTCodesetToolStripMenuItem.Name = "saveGCTCodesetToolStripMenuItem";
            this.saveGCTCodesetToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveGCTCodesetToolStripMenuItem.Text = "Save GCT codeset";
            this.saveGCTCodesetToolStripMenuItem.Click += new System.EventHandler(this.saveGCTCodesetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.use16ptFontToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // use16ptFontToolStripMenuItem
            // 
            this.use16ptFontToolStripMenuItem.CheckOnClick = true;
            this.use16ptFontToolStripMenuItem.Name = "use16ptFontToolStripMenuItem";
            this.use16ptFontToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.use16ptFontToolStripMenuItem.Text = "Use 16pt font";
            this.use16ptFontToolStripMenuItem.Click += new System.EventHandler(this.use16ptFontToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadNamesFromInfopacToolStripMenuItem,
            this.loadBRSTMPlayerToolStripMenuItem,
            this.whenSongEndsStartPlayingNextSongToolStripMenuItem,
            this.toolStripMenuItem1,
            this.groupSongsByStageToolStripMenuItem,
            this.onlyShowSongsWithCSVCodeToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // loadNamesFromInfopacToolStripMenuItem
            // 
            this.loadNamesFromInfopacToolStripMenuItem.Checked = true;
            this.loadNamesFromInfopacToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadNamesFromInfopacToolStripMenuItem.Name = "loadNamesFromInfopacToolStripMenuItem";
            this.loadNamesFromInfopacToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.loadNamesFromInfopacToolStripMenuItem.Text = "Load names from info.pac";
            this.loadNamesFromInfopacToolStripMenuItem.Click += new System.EventHandler(this.loadNamesFromInfopacToolStripMenuItem_Click);
            // 
            // loadBRSTMPlayerToolStripMenuItem
            // 
            this.loadBRSTMPlayerToolStripMenuItem.Checked = true;
            this.loadBRSTMPlayerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadBRSTMPlayerToolStripMenuItem.Name = "loadBRSTMPlayerToolStripMenuItem";
            this.loadBRSTMPlayerToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.loadBRSTMPlayerToolStripMenuItem.Text = "Load BRSTM player";
            this.loadBRSTMPlayerToolStripMenuItem.Click += new System.EventHandler(this.loadBRSTMPlayerToolStripMenuItem_Click);
            // 
            // whenSongEndsStartPlayingNextSongToolStripMenuItem
            // 
            this.whenSongEndsStartPlayingNextSongToolStripMenuItem.Name = "whenSongEndsStartPlayingNextSongToolStripMenuItem";
            this.whenSongEndsStartPlayingNextSongToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.whenSongEndsStartPlayingNextSongToolStripMenuItem.Text = "When song ends, start playing next song";
            this.whenSongEndsStartPlayingNextSongToolStripMenuItem.Click += new System.EventHandler(this.whenSongEndsStartPlayingNextSongToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(284, 6);
            // 
            // groupSongsByStageToolStripMenuItem
            // 
            this.groupSongsByStageToolStripMenuItem.Name = "groupSongsByStageToolStripMenuItem";
            this.groupSongsByStageToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.groupSongsByStageToolStripMenuItem.Text = "Group songs by stage (SSBB)";
            this.groupSongsByStageToolStripMenuItem.Click += new System.EventHandler(this.groupSongsByStageToolStripMenuItem_Click);
            // 
            // onlyShowSongsWithCSVCodeToolStripMenuItem
            // 
            this.onlyShowSongsWithCSVCodeToolStripMenuItem.Name = "onlyShowSongsWithCSVCodeToolStripMenuItem";
            this.onlyShowSongsWithCSVCodeToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.onlyShowSongsWithCSVCodeToolStripMenuItem.Text = "Only show songs with CSV code";
            this.onlyShowSongsWithCSVCodeToolStripMenuItem.Click += new System.EventHandler(this.onlyShowSongsWithCSVCodeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateMumenumainToolStripMenuItem,
            this.exportMusicSongsToolStripMenuItem,
            this.importMusicSongsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.customSongVolumeStatusToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // updateMumenumainToolStripMenuItem
            // 
            this.updateMumenumainToolStripMenuItem.Name = "updateMumenumainToolStripMenuItem";
            this.updateMumenumainToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.updateMumenumainToolStripMenuItem.Text = "Update mu_menumain";
            this.updateMumenumainToolStripMenuItem.Click += new System.EventHandler(this.updateMumenumainToolStripMenuItem_Click);
            // 
            // exportMusicSongsToolStripMenuItem
            // 
            this.exportMusicSongsToolStripMenuItem.Name = "exportMusicSongsToolStripMenuItem";
            this.exportMusicSongsToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.exportMusicSongsToolStripMenuItem.Text = "Export songs...";
            this.exportMusicSongsToolStripMenuItem.Click += new System.EventHandler(this.exportMusicSongsToolStripMenuItem_Click);
            // 
            // importMusicSongsToolStripMenuItem
            // 
            this.importMusicSongsToolStripMenuItem.Name = "importMusicSongsToolStripMenuItem";
            this.importMusicSongsToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.importMusicSongsToolStripMenuItem.Text = "Import songs...";
            this.importMusicSongsToolStripMenuItem.Click += new System.EventHandler(this.importMusicSongsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultSongsListToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // defaultSongsListToolStripMenuItem
            // 
            this.defaultSongsListToolStripMenuItem.Name = "defaultSongsListToolStripMenuItem";
            this.defaultSongsListToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.defaultSongsListToolStripMenuItem.Text = "Default Songs List";
            this.defaultSongsListToolStripMenuItem.Click += new System.EventHandler(this.defaultSongsListToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusToolStripMenuItem.Enabled = false;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.statusToolStripMenuItem.Text = "test";
            // 
            // splitContainerTop
            // 
            this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTop.Location = new System.Drawing.Point(0, 24);
            this.splitContainerTop.Name = "splitContainerTop";
            this.splitContainerTop.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTop.Panel1
            // 
            this.splitContainerTop.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainerTop.Panel2Collapsed = true;
            this.splitContainerTop.Size = new System.Drawing.Size(592, 290);
            this.splitContainerTop.SplitterDistance = 197;
            this.splitContainerTop.TabIndex = 0;
            // 
            // customSongVolumeStatusToolStripMenuItem
            // 
            this.customSongVolumeStatusToolStripMenuItem.Name = "customSongVolumeStatusToolStripMenuItem";
            this.customSongVolumeStatusToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.customSongVolumeStatusToolStripMenuItem.Text = "Custom Song Volume status";
            this.customSongVolumeStatusToolStripMenuItem.Click += new System.EventHandler(this.customSongVolumeStatusToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(220, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 314);
            this.Controls.Add(this.splitContainerTop);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Brawl Song Manager";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainerTop.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem changeDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadNamesFromInfopacToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadBRSTMPlayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveInfopacToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem groupSongsByStageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defaultSongsListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem updateMumenumainToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainerTop;
		private System.Windows.Forms.Label rightLabel;
		private BrawlManagerLib.SongPanel songPanel1;
		private System.Windows.Forms.ToolStripMenuItem openFallbackDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem onlyShowSongsWithCSVCodeToolStripMenuItem;
		private CustomSongVolumeEditor customSongVolumeEditor1;
		private System.Windows.Forms.ToolStripMenuItem saveGCTCodesetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportMusicSongsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importMusicSongsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem use16ptFontToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem whenSongEndsStartPlayingNextSongToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem customSongVolumeStatusToolStripMenuItem;
    }
}


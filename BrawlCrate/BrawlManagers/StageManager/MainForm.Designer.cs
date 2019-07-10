namespace BrawlStageManager {
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
            System.Windows.Forms.ModelPanelViewport modelPanelViewport1 = new System.Windows.Forms.ModelPanelViewport();
            BrawlLib.OpenGL.GLCamera glCamera1 = new BrawlLib.OpenGL.GLCamera();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.stageContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clbTextures = new System.Windows.Forms.CheckedListBox();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.songContainerPanel = new System.Windows.Forms.Panel();
            this.songPanel1 = new BrawlManagerLib.SongPanel();
            this.listBoxSongs = new System.Windows.Forms.ListBox();
            this.songContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.msBinPanel = new System.Windows.Forms.Panel();
            this.exportpacrelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletepacrelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportbrstmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletebrstmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCustomSSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.currentStageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.common5scselmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCommon5scselmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllMiscData80ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveInfopacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.use16ptFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useTextureConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useAFixedStageListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.loadStagepacsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderModels = new System.Windows.Forms.ToolStripMenuItem();
            this.loadbrstmsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customSoundEngineDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cse2xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cse3xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moduleFileDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useFullrelNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.selmapMarkPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkFormatIA4 = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkFormatI4 = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkFormatAuto = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkFormatCMPR = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkFormatExisting = new System.Windows.Forms.ToolStripMenuItem();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.frontStnameGenerationFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapshotPortraiticonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repaintIconBorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.updateMumenumainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateScselcharacter2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.addMenSelmapMarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listMenSelmapMarkUsageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addmissingPAT0EntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.resizeAllPrevbasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prevbaseSize = new System.Windows.Forms.ToolStripMenuItem();
            this.prevbaseOriginalSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x128ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x96ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x88ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customPrevbaseSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontstnameSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontstnameOriginalSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x56ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selmapMarkOriginalSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x56ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.texturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brawlBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brawlBoxStageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brawlBoxcommon5scselmapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.brawlSplitter5 = new BrawlManagerLib.BrawlSplitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stageInfoControl1 = new BrawlStageManager.StageInfoControl();
            this.brawlSplitter3 = new BrawlManagerLib.BrawlSplitter();
            this.brawlSplitter1 = new BrawlManagerLib.BrawlSplitter();
            this.brawlSplitter2 = new BrawlManagerLib.BrawlSplitter();
            this.portraitViewer1 = new BrawlStageManager.PortraitViewer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.songContainerPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 24);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.clbTextures);
            this.splitContainerLeft.Panel2Collapsed = true;
            this.splitContainerLeft.Size = new System.Drawing.Size(144, 477);
            this.splitContainerLeft.SplitterDistance = 238;
            this.splitContainerLeft.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.stageContextMenu;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(144, 477);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // stageContextMenu
            // 
            this.stageContextMenu.Name = "contextMenuStrip1";
            this.stageContextMenu.Size = new System.Drawing.Size(61, 4);
            this.stageContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.stageContextMenu_Opening);
            // 
            // clbTextures
            // 
            this.clbTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbTextures.FormattingEnabled = true;
            this.clbTextures.Location = new System.Drawing.Point(0, 0);
            this.clbTextures.Name = "clbTextures";
            this.clbTextures.Size = new System.Drawing.Size(150, 46);
            this.clbTextures.TabIndex = 0;
            // 
            // modelPanel1
            // 
            modelPanelViewport1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(213)))), ((int)(((byte)(204)))), ((int)(((byte)(187)))));
            modelPanelViewport1.BackgroundImage = null;
            modelPanelViewport1.BackgroundImageType = BrawlLib.OpenGL.BGImageType.Stretch;
            glCamera1.Aspect = 1.594444F;
            glCamera1.FarDepth = 200000F;
            glCamera1.Height = 180F;
            glCamera1.NearDepth = 1F;
            glCamera1.Orthographic = false;
            glCamera1.VerticalFieldOfView = 45F;
            glCamera1.Width = 287F;
            modelPanelViewport1.Camera = glCamera1;
            modelPanelViewport1.Enabled = true;
            modelPanelViewport1.Region = new System.Drawing.Rectangle(0, 0, 287, 180);
            modelPanelViewport1.RotationScale = 0.4F;
            modelPanelViewport1.TranslationScale = 0.05F;
            modelPanelViewport1.ViewType = BrawlLib.OpenGL.ViewportProjection.Perspective;
            modelPanelViewport1.ZoomScale = 2.5F;
            this.modelPanel1.CurrentViewport = modelPanelViewport1;
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.Location = new System.Drawing.Point(0, 158);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.Size = new System.Drawing.Size(287, 180);
            this.modelPanel1.TabIndex = 2;
            // 
            // songContainerPanel
            // 
            this.songContainerPanel.Controls.Add(this.songPanel1);
            this.songContainerPanel.Controls.Add(this.listBoxSongs);
            this.songContainerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.songContainerPanel.Location = new System.Drawing.Point(0, 346);
            this.songContainerPanel.Name = "songContainerPanel";
            this.songContainerPanel.Size = new System.Drawing.Size(287, 131);
            this.songContainerPanel.TabIndex = 7;
            this.songContainerPanel.Visible = false;
            // 
            // songPanel1
            // 
            this.songPanel1.AllowDrop = true;
            this.songPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.songPanel1.Location = new System.Drawing.Point(90, 0);
            this.songPanel1.MinimumSize = new System.Drawing.Size(0, 131);
            this.songPanel1.Name = "songPanel1";
            this.songPanel1.Size = new System.Drawing.Size(197, 131);
            this.songPanel1.TabIndex = 6;
            // 
            // listBoxSongs
            // 
            this.listBoxSongs.ContextMenuStrip = this.songContextMenu;
            this.listBoxSongs.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxSongs.FormattingEnabled = true;
            this.listBoxSongs.IntegralHeight = false;
            this.listBoxSongs.Location = new System.Drawing.Point(0, 0);
            this.listBoxSongs.Name = "listBoxSongs";
            this.listBoxSongs.Size = new System.Drawing.Size(90, 131);
            this.listBoxSongs.TabIndex = 8;
            this.listBoxSongs.SelectedIndexChanged += new System.EventHandler(this.listBoxSongs_SelectedIndexChanged);
            // 
            // songContextMenu
            // 
            this.songContextMenu.Name = "songContextMenu";
            this.songContextMenu.Size = new System.Drawing.Size(61, 4);
            this.songContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.songContextMenu_Opening);
            // 
            // msBinPanel
            // 
            this.msBinPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msBinPanel.Location = new System.Drawing.Point(0, 56);
            this.msBinPanel.Name = "msBinPanel";
            this.msBinPanel.Size = new System.Drawing.Size(287, 94);
            this.msBinPanel.TabIndex = 3;
            // 
            // exportpacrelToolStripMenuItem
            // 
            this.exportpacrelToolStripMenuItem.Name = "exportpacrelToolStripMenuItem";
            this.exportpacrelToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.exportpacrelToolStripMenuItem.Text = "Export .pac/.rel/images";
            this.exportpacrelToolStripMenuItem.Click += new System.EventHandler(this.exportpacrelToolStripMenuItem_Click);
            // 
            // deletepacrelToolStripMenuItem
            // 
            this.deletepacrelToolStripMenuItem.Name = "deletepacrelToolStripMenuItem";
            this.deletepacrelToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.deletepacrelToolStripMenuItem.Text = "Delete .pac/.rel";
            this.deletepacrelToolStripMenuItem.Click += new System.EventHandler(this.deletepacrelToolStripMenuItem_Click);
            // 
            // exportbrstmToolStripMenuItem
            // 
            this.exportbrstmToolStripMenuItem.Name = "exportbrstmToolStripMenuItem";
            this.exportbrstmToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.exportbrstmToolStripMenuItem.Text = "Export .brstm";
            this.exportbrstmToolStripMenuItem.Click += new System.EventHandler(this.exportbrstmToolStripMenuItem_Click);
            // 
            // deletebrstmToolStripMenuItem
            // 
            this.deletebrstmToolStripMenuItem.Name = "deletebrstmToolStripMenuItem";
            this.deletebrstmToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.deletebrstmToolStripMenuItem.Text = "Delete .brstm";
            this.deletebrstmToolStripMenuItem.Click += new System.EventHandler(this.deletebrstmToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.resizeToolStripMenuItem,
            this.texturesToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.brawlBoxToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(684, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeDirectoryToolStripMenuItem,
            this.loadCustomSSSToolStripMenuItem,
            this.toolStripMenuItem3,
            this.currentStageToolStripMenuItem,
            this.currentSongToolStripMenuItem,
            this.common5scselmapToolStripMenuItem,
            this.saveInfopacToolStripMenuItem,
            this.toolStripMenuItem4,
            this.exportAllToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // changeDirectoryToolStripMenuItem
            // 
            this.changeDirectoryToolStripMenuItem.Name = "changeDirectoryToolStripMenuItem";
            this.changeDirectoryToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.changeDirectoryToolStripMenuItem.Text = "Change directory...";
            this.changeDirectoryToolStripMenuItem.Click += new System.EventHandler(this.changeDirectoryToolStripMenuItem_Click);
            // 
            // loadCustomSSSToolStripMenuItem
            // 
            this.loadCustomSSSToolStripMenuItem.Name = "loadCustomSSSToolStripMenuItem";
            this.loadCustomSSSToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.loadCustomSSSToolStripMenuItem.Text = "Manually load GCT codeset";
            this.loadCustomSSSToolStripMenuItem.Click += new System.EventHandler(this.loadCustomSSSToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(216, 6);
            // 
            // currentStageToolStripMenuItem
            // 
            this.currentStageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportpacrelToolStripMenuItem,
            this.deletepacrelToolStripMenuItem});
            this.currentStageToolStripMenuItem.Name = "currentStageToolStripMenuItem";
            this.currentStageToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.currentStageToolStripMenuItem.Text = "Current stage";
            // 
            // currentSongToolStripMenuItem
            // 
            this.currentSongToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportbrstmToolStripMenuItem,
            this.deletebrstmToolStripMenuItem});
            this.currentSongToolStripMenuItem.Name = "currentSongToolStripMenuItem";
            this.currentSongToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.currentSongToolStripMenuItem.Text = "Current song";
            // 
            // common5scselmapToolStripMenuItem
            // 
            this.common5scselmapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCommon5scselmapToolStripMenuItem,
            this.exportAllMiscData80ToolStripMenuItem});
            this.common5scselmapToolStripMenuItem.Name = "common5scselmapToolStripMenuItem";
            this.common5scselmapToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.common5scselmapToolStripMenuItem.Text = "common5/sc_selmap";
            // 
            // saveCommon5scselmapToolStripMenuItem
            // 
            this.saveCommon5scselmapToolStripMenuItem.Name = "saveCommon5scselmapToolStripMenuItem";
            this.saveCommon5scselmapToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.saveCommon5scselmapToolStripMenuItem.Text = "Save";
            this.saveCommon5scselmapToolStripMenuItem.Click += new System.EventHandler(this.saveCommon5scselmapToolStripMenuItem_Click);
            // 
            // exportAllMiscData80ToolStripMenuItem
            // 
            this.exportAllMiscData80ToolStripMenuItem.Name = "exportAllMiscData80ToolStripMenuItem";
            this.exportAllMiscData80ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exportAllMiscData80ToolStripMenuItem.Text = "Export all (Misc Data [80])";
            this.exportAllMiscData80ToolStripMenuItem.Click += new System.EventHandler(this.exportAllMiscData80ToolStripMenuItem_Click);
            // 
            // saveInfopacToolStripMenuItem
            // 
            this.saveInfopacToolStripMenuItem.Name = "saveInfopacToolStripMenuItem";
            this.saveInfopacToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.saveInfopacToolStripMenuItem.Text = "Save info.pac";
            this.saveInfopacToolStripMenuItem.Click += new System.EventHandler(this.saveInfopacToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(216, 6);
            // 
            // exportAllToolStripMenuItem
            // 
            this.exportAllToolStripMenuItem.Name = "exportAllToolStripMenuItem";
            this.exportAllToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportAllToolStripMenuItem.Text = "Export all stages";
            this.exportAllToolStripMenuItem.Click += new System.EventHandler(this.exportAllToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
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
            this.use16ptFontToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.use16ptFontToolStripMenuItem.Text = "Use 16pt font";
            this.use16ptFontToolStripMenuItem.Click += new System.EventHandler(this.use16ptFontToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useTextureConverterToolStripMenuItem,
            this.useAFixedStageListToolStripMenuItem,
            this.backgroundColorToolStripMenuItem,
            this.toolStripMenuItem5,
            this.loadStagepacsToolStripMenuItem,
            this.renderModels,
            this.loadbrstmsToolStripMenuItem,
            this.customSoundEngineDirectoryToolStripMenuItem,
            this.separator1,
            this.moduleFileDirectoryToolStripMenuItem,
            this.useFullrelNamesToolStripMenuItem,
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem,
            this.separator2,
            this.selmapMarkPreviewToolStripMenuItem,
            this.selmapMarkFormat,
            this.separator3,
            this.frontStnameGenerationFontToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // useTextureConverterToolStripMenuItem
            // 
            this.useTextureConverterToolStripMenuItem.Checked = true;
            this.useTextureConverterToolStripMenuItem.CheckOnClick = true;
            this.useTextureConverterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useTextureConverterToolStripMenuItem.Name = "useTextureConverterToolStripMenuItem";
            this.useTextureConverterToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.useTextureConverterToolStripMenuItem.Text = "Use Texture Converter";
            this.useTextureConverterToolStripMenuItem.Click += new System.EventHandler(this.useTextureConverterToolStripMenuItem_Click);
            // 
            // useAFixedStageListToolStripMenuItem
            // 
            this.useAFixedStageListToolStripMenuItem.CheckOnClick = true;
            this.useAFixedStageListToolStripMenuItem.Name = "useAFixedStageListToolStripMenuItem";
            this.useAFixedStageListToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.useAFixedStageListToolStripMenuItem.Text = "Use stage list from GCT codeset (SSS+ASL)";
            this.useAFixedStageListToolStripMenuItem.Click += new System.EventHandler(this.useAFixedStageListToolStripMenuItem_Click);
            // 
            // backgroundColorToolStripMenuItem
            // 
            this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.backgroundColorToolStripMenuItem.Text = "Right panel BG color...";
            this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.backgroundColorToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(295, 6);
            // 
            // loadStagepacsToolStripMenuItem
            // 
            this.loadStagepacsToolStripMenuItem.Checked = true;
            this.loadStagepacsToolStripMenuItem.CheckOnClick = true;
            this.loadStagepacsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadStagepacsToolStripMenuItem.Name = "loadStagepacsToolStripMenuItem";
            this.loadStagepacsToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.loadStagepacsToolStripMenuItem.Text = "Load stage .pacs";
            this.loadStagepacsToolStripMenuItem.Click += new System.EventHandler(this.loadStagepacsToolStripMenuItem_Click);
            // 
            // renderModels
            // 
            this.renderModels.Checked = true;
            this.renderModels.CheckOnClick = true;
            this.renderModels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.renderModels.Name = "renderModels";
            this.renderModels.Size = new System.Drawing.Size(298, 22);
            this.renderModels.Text = "Render models";
            // 
            // loadbrstmsToolStripMenuItem
            // 
            this.loadbrstmsToolStripMenuItem.Checked = true;
            this.loadbrstmsToolStripMenuItem.CheckOnClick = true;
            this.loadbrstmsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadbrstmsToolStripMenuItem.Name = "loadbrstmsToolStripMenuItem";
            this.loadbrstmsToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.loadbrstmsToolStripMenuItem.Text = "Load .brstms";
            // 
            // customSoundEngineDirectoryToolStripMenuItem
            // 
            this.customSoundEngineDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cse2xToolStripMenuItem,
            this.cse3xToolStripMenuItem});
            this.customSoundEngineDirectoryToolStripMenuItem.Name = "customSoundEngineDirectoryToolStripMenuItem";
            this.customSoundEngineDirectoryToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.customSoundEngineDirectoryToolStripMenuItem.Text = "Custom Sound Engine version";
            // 
            // cse2xToolStripMenuItem
            // 
            this.cse2xToolStripMenuItem.Name = "cse2xToolStripMenuItem";
            this.cse2xToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.cse2xToolStripMenuItem.Text = "2.x";
            this.cse2xToolStripMenuItem.Click += new System.EventHandler(this.cse2xToolStripMenuItem_Click);
            // 
            // cse3xToolStripMenuItem
            // 
            this.cse3xToolStripMenuItem.Checked = true;
            this.cse3xToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cse3xToolStripMenuItem.Name = "cse3xToolStripMenuItem";
            this.cse3xToolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.cse3xToolStripMenuItem.Text = "3.x";
            this.cse3xToolStripMenuItem.Click += new System.EventHandler(this.cse3xToolStripMenuItem_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(295, 6);
            // 
            // moduleFileDirectoryToolStripMenuItem
            // 
            this.moduleFileDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sameToolStripMenuItem,
            this.moduleToolStripMenuItem});
            this.moduleFileDirectoryToolStripMenuItem.Name = "moduleFileDirectoryToolStripMenuItem";
            this.moduleFileDirectoryToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.moduleFileDirectoryToolStripMenuItem.Text = "Module file directory";
            // 
            // sameToolStripMenuItem
            // 
            this.sameToolStripMenuItem.Name = "sameToolStripMenuItem";
            this.sameToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.sameToolStripMenuItem.Text = "Same";
            this.sameToolStripMenuItem.Click += new System.EventHandler(this.sameToolStripMenuItem_Click);
            // 
            // moduleToolStripMenuItem
            // 
            this.moduleToolStripMenuItem.Checked = true;
            this.moduleToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.moduleToolStripMenuItem.Name = "moduleToolStripMenuItem";
            this.moduleToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.moduleToolStripMenuItem.Text = "..\\..\\module";
            this.moduleToolStripMenuItem.Click += new System.EventHandler(this.moduleToolStripMenuItem_Click);
            // 
            // useFullrelNamesToolStripMenuItem
            // 
            this.useFullrelNamesToolStripMenuItem.Checked = true;
            this.useFullrelNamesToolStripMenuItem.CheckOnClick = true;
            this.useFullrelNamesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useFullrelNamesToolStripMenuItem.Name = "useFullrelNamesToolStripMenuItem";
            this.useFullrelNamesToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.useFullrelNamesToolStripMenuItem.Text = "Use full .rel names";
            this.useFullrelNamesToolStripMenuItem.Click += new System.EventHandler(this.useFullrelNamesToolStripMenuItem_Click);
            // 
            // differentrelsForAlternateStagesPM36ToolStripMenuItem
            // 
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem.Checked = true;
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem.CheckOnClick = true;
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem.Name = "differentrelsForAlternateStagesPM36ToolStripMenuItem";
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem.Text = "Different .rels for alternate stages (PM 3.6)";
            this.differentrelsForAlternateStagesPM36ToolStripMenuItem.Click += new System.EventHandler(this.differentrelsForAlternateStagesPM36ToolStripMenuItem_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(295, 6);
            // 
            // selmapMarkPreviewToolStripMenuItem
            // 
            this.selmapMarkPreviewToolStripMenuItem.Checked = true;
            this.selmapMarkPreviewToolStripMenuItem.CheckOnClick = true;
            this.selmapMarkPreviewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selmapMarkPreviewToolStripMenuItem.Name = "selmapMarkPreviewToolStripMenuItem";
            this.selmapMarkPreviewToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.selmapMarkPreviewToolStripMenuItem.Text = "Portrait previews";
            this.selmapMarkPreviewToolStripMenuItem.Click += new System.EventHandler(this.selmapMarkPreviewToolStripMenuItem_Click);
            // 
            // selmapMarkFormat
            // 
            this.selmapMarkFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selmapMarkFormatIA4,
            this.selmapMarkFormatI4,
            this.selmapMarkFormatAuto,
            this.selmapMarkFormatCMPR,
            this.selmapMarkFormatExisting});
            this.selmapMarkFormat.Name = "selmapMarkFormat";
            this.selmapMarkFormat.Size = new System.Drawing.Size(298, 22);
            this.selmapMarkFormat.Text = "SelmapMark format";
            // 
            // selmapMarkFormatIA4
            // 
            this.selmapMarkFormatIA4.Name = "selmapMarkFormatIA4";
            this.selmapMarkFormatIA4.Size = new System.Drawing.Size(142, 22);
            this.selmapMarkFormatIA4.Text = "IA4";
            // 
            // selmapMarkFormatI4
            // 
            this.selmapMarkFormatI4.Name = "selmapMarkFormatI4";
            this.selmapMarkFormatI4.Size = new System.Drawing.Size(142, 22);
            this.selmapMarkFormatI4.Text = "I4";
            // 
            // selmapMarkFormatAuto
            // 
            this.selmapMarkFormatAuto.Name = "selmapMarkFormatAuto";
            this.selmapMarkFormatAuto.Size = new System.Drawing.Size(142, 22);
            this.selmapMarkFormatAuto.Text = "IA4/I4 (Auto)";
            // 
            // selmapMarkFormatCMPR
            // 
            this.selmapMarkFormatCMPR.Name = "selmapMarkFormatCMPR";
            this.selmapMarkFormatCMPR.Size = new System.Drawing.Size(142, 22);
            this.selmapMarkFormatCMPR.Text = "CMPR";
            // 
            // selmapMarkFormatExisting
            // 
            this.selmapMarkFormatExisting.Checked = true;
            this.selmapMarkFormatExisting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selmapMarkFormatExisting.Name = "selmapMarkFormatExisting";
            this.selmapMarkFormatExisting.Size = new System.Drawing.Size(142, 22);
            this.selmapMarkFormatExisting.Text = "Existing";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(295, 6);
            // 
            // frontStnameGenerationFontToolStripMenuItem
            // 
            this.frontStnameGenerationFontToolStripMenuItem.Name = "frontStnameGenerationFontToolStripMenuItem";
            this.frontStnameGenerationFontToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.frontStnameGenerationFontToolStripMenuItem.Text = "FrontStname font...";
            this.frontStnameGenerationFontToolStripMenuItem.Click += new System.EventHandler(this.frontStnameGenerationFontToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snapshotPortraiticonToolStripMenuItem,
            this.repaintIconBorderToolStripMenuItem,
            this.toolStripMenuItem2,
            this.updateMumenumainToolStripMenuItem,
            this.updateScselcharacter2ToolStripMenuItem,
            this.separator4,
            this.addMenSelmapMarksToolStripMenuItem,
            this.listMenSelmapMarkUsageToolStripMenuItem,
            this.toolStripMenuItem1,
            this.addmissingPAT0EntriesToolStripMenuItem,
            this.toolStripMenuItem6,
            this.resizeAllPrevbasesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // snapshotPortraiticonToolStripMenuItem
            // 
            this.snapshotPortraiticonToolStripMenuItem.Name = "snapshotPortraiticonToolStripMenuItem";
            this.snapshotPortraiticonToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.snapshotPortraiticonToolStripMenuItem.Text = "Snapshot -> portrait/icon";
            this.snapshotPortraiticonToolStripMenuItem.Click += new System.EventHandler(this.snapshotPortraiticonToolStripMenuItem_Click);
            // 
            // repaintIconBorderToolStripMenuItem
            // 
            this.repaintIconBorderToolStripMenuItem.Name = "repaintIconBorderToolStripMenuItem";
            this.repaintIconBorderToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.repaintIconBorderToolStripMenuItem.Text = "Repaint icon border ([)";
            this.repaintIconBorderToolStripMenuItem.Click += new System.EventHandler(this.repaintIconBorderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(278, 6);
            // 
            // updateMumenumainToolStripMenuItem
            // 
            this.updateMumenumainToolStripMenuItem.Name = "updateMumenumainToolStripMenuItem";
            this.updateMumenumainToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.updateMumenumainToolStripMenuItem.Text = "Update mu_menumain";
            this.updateMumenumainToolStripMenuItem.Click += new System.EventHandler(this.updateMumenumainToolStripMenuItem_Click);
            // 
            // updateScselcharacter2ToolStripMenuItem
            // 
            this.updateScselcharacter2ToolStripMenuItem.Name = "updateScselcharacter2ToolStripMenuItem";
            this.updateScselcharacter2ToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.updateScselcharacter2ToolStripMenuItem.Text = "Update stage pics in sc_selcharacter2";
            this.updateScselcharacter2ToolStripMenuItem.Click += new System.EventHandler(this.updateScselcharacter2ToolStripMenuItem_Click);
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            this.separator4.Size = new System.Drawing.Size(278, 6);
            // 
            // addMenSelmapMarksToolStripMenuItem
            // 
            this.addMenSelmapMarksToolStripMenuItem.Name = "addMenSelmapMarksToolStripMenuItem";
            this.addMenSelmapMarksToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.addMenSelmapMarksToolStripMenuItem.Text = "Add SelmapMarks/SelchrMarks";
            this.addMenSelmapMarksToolStripMenuItem.Click += new System.EventHandler(this.addMenSelmapMarksToolStripMenuItem_Click);
            // 
            // listMenSelmapMarkUsageToolStripMenuItem
            // 
            this.listMenSelmapMarkUsageToolStripMenuItem.Name = "listMenSelmapMarkUsageToolStripMenuItem";
            this.listMenSelmapMarkUsageToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.listMenSelmapMarkUsageToolStripMenuItem.Text = "List MenSelmapMark usage";
            this.listMenSelmapMarkUsageToolStripMenuItem.Click += new System.EventHandler(this.listMenSelmapMarkUsageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(278, 6);
            // 
            // addmissingPAT0EntriesToolStripMenuItem
            // 
            this.addmissingPAT0EntriesToolStripMenuItem.Name = "addmissingPAT0EntriesToolStripMenuItem";
            this.addmissingPAT0EntriesToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.addmissingPAT0EntriesToolStripMenuItem.Text = "Prepare sc_selmap for expansion stages";
            this.addmissingPAT0EntriesToolStripMenuItem.Click += new System.EventHandler(this.addmissingPAT0EntriesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(278, 6);
            // 
            // resizeAllPrevbasesToolStripMenuItem
            // 
            this.resizeAllPrevbasesToolStripMenuItem.Name = "resizeAllPrevbasesToolStripMenuItem";
            this.resizeAllPrevbasesToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.resizeAllPrevbasesToolStripMenuItem.Text = "Resize all Prevbases";
            this.resizeAllPrevbasesToolStripMenuItem.Click += new System.EventHandler(this.resizeAllPrevbasesToolStripMenuItem_Click);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prevbaseSize,
            this.frontstnameSizeToolStripMenuItem,
            this.selmapMarkSizeToolStripMenuItem});
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.resizeToolStripMenuItem.Text = "Auto-resize";
            // 
            // prevbaseSize
            // 
            this.prevbaseSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prevbaseOriginalSizeToolStripMenuItem,
            this.x128ToolStripMenuItem,
            this.x96ToolStripMenuItem,
            this.x88ToolStripMenuItem,
            this.customPrevbaseSizeToolStripMenuItem});
            this.prevbaseSize.Name = "prevbaseSize";
            this.prevbaseSize.Size = new System.Drawing.Size(168, 22);
            this.prevbaseSize.Text = "Prevbase size:";
            // 
            // prevbaseOriginalSizeToolStripMenuItem
            // 
            this.prevbaseOriginalSizeToolStripMenuItem.Checked = true;
            this.prevbaseOriginalSizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.prevbaseOriginalSizeToolStripMenuItem.Name = "prevbaseOriginalSizeToolStripMenuItem";
            this.prevbaseOriginalSizeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.prevbaseOriginalSizeToolStripMenuItem.Text = "Off";
            // 
            // x128ToolStripMenuItem
            // 
            this.x128ToolStripMenuItem.Name = "x128ToolStripMenuItem";
            this.x128ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.x128ToolStripMenuItem.Text = "128x128";
            // 
            // x96ToolStripMenuItem
            // 
            this.x96ToolStripMenuItem.Name = "x96ToolStripMenuItem";
            this.x96ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.x96ToolStripMenuItem.Text = "96x96";
            this.x96ToolStripMenuItem.Click += new System.EventHandler(this.switchPrevbaseSize);
            // 
            // x88ToolStripMenuItem
            // 
            this.x88ToolStripMenuItem.Name = "x88ToolStripMenuItem";
            this.x88ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.x88ToolStripMenuItem.Text = "88x88";
            // 
            // customPrevbaseSizeToolStripMenuItem
            // 
            this.customPrevbaseSizeToolStripMenuItem.Name = "customPrevbaseSizeToolStripMenuItem";
            this.customPrevbaseSizeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.customPrevbaseSizeToolStripMenuItem.Text = "Custom...";
            // 
            // frontstnameSizeToolStripMenuItem
            // 
            this.frontstnameSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frontstnameOriginalSizeToolStripMenuItem,
            this.x56ToolStripMenuItem});
            this.frontstnameSizeToolStripMenuItem.Name = "frontstnameSizeToolStripMenuItem";
            this.frontstnameSizeToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.frontstnameSizeToolStripMenuItem.Text = "Frontstname size:";
            // 
            // frontstnameOriginalSizeToolStripMenuItem
            // 
            this.frontstnameOriginalSizeToolStripMenuItem.Checked = true;
            this.frontstnameOriginalSizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.frontstnameOriginalSizeToolStripMenuItem.Name = "frontstnameOriginalSizeToolStripMenuItem";
            this.frontstnameOriginalSizeToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.frontstnameOriginalSizeToolStripMenuItem.Text = "Off";
            // 
            // x56ToolStripMenuItem
            // 
            this.x56ToolStripMenuItem.Name = "x56ToolStripMenuItem";
            this.x56ToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.x56ToolStripMenuItem.Text = "104x56";
            // 
            // selmapMarkSizeToolStripMenuItem
            // 
            this.selmapMarkSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selmapMarkOriginalSizeToolStripMenuItem,
            this.x56ToolStripMenuItem1});
            this.selmapMarkSizeToolStripMenuItem.Name = "selmapMarkSizeToolStripMenuItem";
            this.selmapMarkSizeToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.selmapMarkSizeToolStripMenuItem.Text = "Selmap mark size:";
            // 
            // selmapMarkOriginalSizeToolStripMenuItem
            // 
            this.selmapMarkOriginalSizeToolStripMenuItem.Checked = true;
            this.selmapMarkOriginalSizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selmapMarkOriginalSizeToolStripMenuItem.Name = "selmapMarkOriginalSizeToolStripMenuItem";
            this.selmapMarkOriginalSizeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.selmapMarkOriginalSizeToolStripMenuItem.Text = "Off";
            // 
            // x56ToolStripMenuItem1
            // 
            this.x56ToolStripMenuItem1.Name = "x56ToolStripMenuItem1";
            this.x56ToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.x56ToolStripMenuItem1.Text = "60x56";
            // 
            // texturesToolStripMenuItem
            // 
            this.texturesToolStripMenuItem.CheckOnClick = true;
            this.texturesToolStripMenuItem.Name = "texturesToolStripMenuItem";
            this.texturesToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.texturesToolStripMenuItem.Text = "Model Textures";
            this.texturesToolStripMenuItem.Click += new System.EventHandler(this.texturesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // brawlBoxToolStripMenuItem
            // 
            this.brawlBoxToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.brawlBoxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.brawlBoxStageToolStripMenuItem,
            this.brawlBoxcommon5scselmapToolStripMenuItem1});
            this.brawlBoxToolStripMenuItem.Name = "brawlBoxToolStripMenuItem";
            this.brawlBoxToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.brawlBoxToolStripMenuItem.Text = "BrawlBox";
            // 
            // brawlBoxStageToolStripMenuItem
            // 
            this.brawlBoxStageToolStripMenuItem.Name = "brawlBoxStageToolStripMenuItem";
            this.brawlBoxStageToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.brawlBoxStageToolStripMenuItem.Text = "No stage loaded";
            this.brawlBoxStageToolStripMenuItem.Click += new System.EventHandler(this.brawlBoxStageToolStripMenuItem_Click);
            // 
            // brawlBoxcommon5scselmapToolStripMenuItem1
            // 
            this.brawlBoxcommon5scselmapToolStripMenuItem1.Name = "brawlBoxcommon5scselmapToolStripMenuItem1";
            this.brawlBoxcommon5scselmapToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
            this.brawlBoxcommon5scselmapToolStripMenuItem1.Text = "common5/sc_selmap";
            this.brawlBoxcommon5scselmapToolStripMenuItem1.Click += new System.EventHandler(this.brawlBoxcommon5scselmapToolStripMenuItem1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.modelPanel1);
            this.panel2.Controls.Add(this.brawlSplitter5);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.brawlSplitter3);
            this.panel2.Controls.Add(this.songContainerPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(152, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(287, 477);
            this.panel2.TabIndex = 8;
            // 
            // brawlSplitter5
            // 
            this.brawlSplitter5.ControlToHide = this.panel1;
            this.brawlSplitter5.Dock = System.Windows.Forms.DockStyle.Top;
            this.brawlSplitter5.Location = new System.Drawing.Point(0, 150);
            this.brawlSplitter5.Name = "brawlSplitter5";
            this.brawlSplitter5.Size = new System.Drawing.Size(287, 8);
            this.brawlSplitter5.TabIndex = 2;
            this.brawlSplitter5.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.msBinPanel);
            this.panel1.Controls.Add(this.stageInfoControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(287, 150);
            this.panel1.TabIndex = 8;
            // 
            // stageInfoControl1
            // 
            this.stageInfoControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stageInfoControl1.Location = new System.Drawing.Point(0, 0);
            this.stageInfoControl1.Name = "stageInfoControl1";
            this.stageInfoControl1.RelFile = null;
            this.stageInfoControl1.Size = new System.Drawing.Size(287, 56);
            this.stageInfoControl1.TabIndex = 0;
            this.stageInfoControl1.UseRelDescription = false;
            // 
            // brawlSplitter3
            // 
            this.brawlSplitter3.ControlToHide = this.songContainerPanel;
            this.brawlSplitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.brawlSplitter3.Location = new System.Drawing.Point(0, 338);
            this.brawlSplitter3.Name = "brawlSplitter3";
            this.brawlSplitter3.Size = new System.Drawing.Size(287, 8);
            this.brawlSplitter3.TabIndex = 0;
            this.brawlSplitter3.TabStop = false;
            // 
            // brawlSplitter1
            // 
            this.brawlSplitter1.ControlToHide = this.splitContainerLeft;
            this.brawlSplitter1.Location = new System.Drawing.Point(144, 24);
            this.brawlSplitter1.Name = "brawlSplitter1";
            this.brawlSplitter1.Size = new System.Drawing.Size(8, 477);
            this.brawlSplitter1.TabIndex = 8;
            this.brawlSplitter1.TabStop = false;
            // 
            // brawlSplitter2
            // 
            this.brawlSplitter2.AllowResizing = false;
            this.brawlSplitter2.ControlToHide = this.portraitViewer1;
            this.brawlSplitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.brawlSplitter2.Location = new System.Drawing.Point(439, 24);
            this.brawlSplitter2.Name = "brawlSplitter2";
            this.brawlSplitter2.Size = new System.Drawing.Size(8, 477);
            this.brawlSplitter2.TabIndex = 9;
            this.brawlSplitter2.TabStop = false;
            // 
            // portraitViewer1
            // 
            this.portraitViewer1.AutoSize = true;
            this.portraitViewer1.AutoSSS = null;
            this.portraitViewer1.Dock = System.Windows.Forms.DockStyle.Right;
            this.portraitViewer1.Location = new System.Drawing.Point(447, 24);
            this.portraitViewer1.ManualSSS = null;
            this.portraitViewer1.Name = "portraitViewer1";
            this.portraitViewer1.Size = new System.Drawing.Size(237, 477);
            this.portraitViewer1.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 501);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.brawlSplitter1);
            this.Controls.Add(this.splitContainerLeft);
            this.Controls.Add(this.brawlSplitter2);
            this.Controls.Add(this.portraitViewer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Brawl Stage Manager";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.songContainerPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBox1;
		private StageInfoControl stageInfoControl1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem changeDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moduleFileDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moduleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem useFullrelNamesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ModelPanel modelPanel1;
		private System.Windows.Forms.ToolStripMenuItem texturesToolStripMenuItem;
		private System.Windows.Forms.Panel msBinPanel;
		private PortraitViewer portraitViewer1;
		private System.Windows.Forms.ToolStripMenuItem prevbaseSize;
		private System.Windows.Forms.ToolStripMenuItem prevbaseOriginalSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem x128ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem x88ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem frontstnameSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem frontstnameOriginalSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem x56ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renderModels;
		private System.Windows.Forms.ToolStripMenuItem exportAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkOriginalSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem x56ToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkPreviewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem useAFixedStageListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addmissingPAT0EntriesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addMenSelmapMarksToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem updateScselcharacter2ToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip stageContextMenu;
		private System.Windows.Forms.ToolStripMenuItem exportpacrelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkFormat;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkFormatIA4;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkFormatI4;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkFormatAuto;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkFormatCMPR;
		private System.Windows.Forms.ToolStripMenuItem selmapMarkFormatExisting;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem listMenSelmapMarkUsageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem frontStnameGenerationFontToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator separator1;
		private System.Windows.Forms.ToolStripSeparator separator2;
		private System.Windows.Forms.ToolStripSeparator separator3;
		private System.Windows.Forms.ToolStripSeparator separator4;
		private System.Windows.Forms.ToolStripMenuItem backgroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem updateMumenumainToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem snapshotPortraiticonToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem repaintIconBorderToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resizeAllPrevbasesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem customPrevbaseSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem useTextureConverterToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.SplitContainer splitContainerLeft;
		private System.Windows.Forms.CheckedListBox clbTextures;
        private System.Windows.Forms.ToolStripMenuItem deletepacrelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadbrstmsToolStripMenuItem;
		private BrawlManagerLib.SongPanel songPanel1;
		private System.Windows.Forms.ToolStripMenuItem exportbrstmToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deletebrstmToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem currentStageToolStripMenuItem;
		private System.Windows.Forms.Panel songContainerPanel;
		private System.Windows.Forms.ListBox listBoxSongs;
		private System.Windows.Forms.ToolStripMenuItem currentSongToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ContextMenuStrip songContextMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem saveCommon5scselmapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveInfopacToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem common5scselmapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportAllMiscData80ToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ToolStripMenuItem brawlBoxToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem brawlBoxStageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem brawlBoxcommon5scselmapToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem loadCustomSSSToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem loadStagepacsToolStripMenuItem;
        private BrawlManagerLib.BrawlSplitter brawlSplitter1;
        private BrawlManagerLib.BrawlSplitter brawlSplitter2;
		private BrawlManagerLib.BrawlSplitter brawlSplitter5;
        private BrawlManagerLib.BrawlSplitter brawlSplitter3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem differentrelsForAlternateStagesPM36ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem x96ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem use16ptFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customSoundEngineDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cse2xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cse3xToolStripMenuItem;
    }
}


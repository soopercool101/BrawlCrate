using BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers;
using BrawlLib.BrawlManagerLib;

namespace BrawlCrate.BrawlManagers.CostumeManager {
    partial class CostumeManagerForm {
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToOtherPacpcsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.changeDirectory = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.screenshotPortraitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateSSSStockIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateMewtwoHatForCurrentKirbyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.limitModelViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultZoomLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.hidePolygonsCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.swapPortraitsForWarioStylesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cBlissCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.projectMCheckbox = new System.Windows.Forms.ToolStripMenuItem();
            this.separator = new System.Windows.Forms.ToolStripSeparator();
            this.nameportraitPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.otherPVsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.brawlSplitter3 = new BrawlSplitter();
            this.brawlSplitter2 = new BrawlSplitter();
            this.globalPVsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.brawlSplitter1 = new BrawlSplitter();
            this.modelManager1 = new BrawlManagers.CostumeManager.ModelManager();
            this.cssPortraitViewer1 = new CSSPortraitViewer();
            this.infoStockIconViewer1 = new InfoStockIconViewer();
            this.costumeNumberLabel = new CostumeNumberLabel();
            this.battlePortraitViewer1 = new BattleSinglePortraitViewer();
            this.resultPortraitViewer1 = new ResultSinglePortraitViewer();
            this.use16ptFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.otherPVsFlowLayoutPanel.SuspendLayout();
            this.globalPVsFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(206, 187);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Left;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBox2);
            this.splitContainer2.Size = new System.Drawing.Size(206, 381);
            this.splitContainer2.SplitterDistance = 187;
            this.splitContainer2.TabIndex = 1;
            // 
            // listBox2
            // 
            this.listBox2.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(206, 190);
            this.listBox2.TabIndex = 0;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToToolStripMenuItem,
            this.copyToOtherPacpcsToolStripMenuItem,
            this._deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(193, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyToToolStripMenuItem
            // 
            this.copyToToolStripMenuItem.Name = "copyToToolStripMenuItem";
            this.copyToToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.copyToToolStripMenuItem.Text = "Copy to...";
            this.copyToToolStripMenuItem.Click += new System.EventHandler(this.copyToToolStripMenuItem_Click);
            // 
            // copyToOtherPacpcsToolStripMenuItem
            // 
            this.copyToOtherPacpcsToolStripMenuItem.Name = "copyToOtherPacpcsToolStripMenuItem";
            this.copyToOtherPacpcsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.copyToOtherPacpcsToolStripMenuItem.Text = "Copy to other pac/pcs";
            this.copyToOtherPacpcsToolStripMenuItem.Click += new System.EventHandler(this.copyToOtherPacpcsToolStripMenuItem_Click);
            // 
            // _deleteToolStripMenuItem
            // 
            this._deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            this._deleteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this._deleteToolStripMenuItem.Text = "Delete";
            this._deleteToolStripMenuItem.Click += new System.EventHandler(this._deleteToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeDirectory,
            this.toolStripDropDownButton3,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(684, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // changeDirectory
            // 
            this.changeDirectory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.changeDirectory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.changeDirectory.Name = "changeDirectory";
            this.changeDirectory.Size = new System.Drawing.Size(103, 22);
            this.changeDirectory.Text = "Change Directory";
            this.changeDirectory.Click += new System.EventHandler(this.changeDirectory_Click);
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.use16ptFontToolStripMenuItem});
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton3.Text = "View";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.screenshotPortraitsToolStripMenuItem,
            this.updateSSSStockIconsToolStripMenuItem,
            this.updateMewtwoHatForCurrentKirbyToolStripMenuItem});
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(49, 22);
            this.toolStripDropDownButton2.Text = "Tools";
            // 
            // screenshotPortraitsToolStripMenuItem
            // 
            this.screenshotPortraitsToolStripMenuItem.Name = "screenshotPortraitsToolStripMenuItem";
            this.screenshotPortraitsToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.screenshotPortraitsToolStripMenuItem.Text = "Screenshot -> portraits";
            this.screenshotPortraitsToolStripMenuItem.Click += new System.EventHandler(this.screenshotPortraitsToolStripMenuItem_Click);
            // 
            // updateSSSStockIconsToolStripMenuItem
            // 
            this.updateSSSStockIconsToolStripMenuItem.Name = "updateSSSStockIconsToolStripMenuItem";
            this.updateSSSStockIconsToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.updateSSSStockIconsToolStripMenuItem.Text = "Copy stock icons to SSS";
            this.updateSSSStockIconsToolStripMenuItem.Click += new System.EventHandler(this.updateSSSStockIconsToolStripMenuItem_Click);
            // 
            // updateMewtwoHatForCurrentKirbyToolStripMenuItem
            // 
            this.updateMewtwoHatForCurrentKirbyToolStripMenuItem.Name = "updateMewtwoHatForCurrentKirbyToolStripMenuItem";
            this.updateMewtwoHatForCurrentKirbyToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.updateMewtwoHatForCurrentKirbyToolStripMenuItem.Text = "Update Mewtwo hat for current Kirby";
            this.updateMewtwoHatForCurrentKirbyToolStripMenuItem.Click += new System.EventHandler(this.updateMewtwoHatForCurrentKirbyToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.limitModelViewerToolStripMenuItem,
            this.defaultZoomLevelToolStripMenuItem,
            this.separator2,
            this.hidePolygonsCheckbox,
            this.swapPortraitsForWarioStylesToolStripMenuItem,
            this.cBlissCheckbox,
            this.projectMCheckbox,
            this.separator,
            this.nameportraitPreviewToolStripMenuItem,
            this.backgroundColorToolStripMenuItem});
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(62, 22);
            this.toolStripDropDownButton1.Text = "Options";
            // 
            // limitModelViewerToolStripMenuItem
            // 
            this.limitModelViewerToolStripMenuItem.CheckOnClick = true;
            this.limitModelViewerToolStripMenuItem.Name = "limitModelViewerToolStripMenuItem";
            this.limitModelViewerToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.limitModelViewerToolStripMenuItem.Text = "Limit model viewer to 256x320";
            this.limitModelViewerToolStripMenuItem.Click += new System.EventHandler(this.limitModelViewerToolStripMenuItem_Click);
            // 
            // defaultZoomLevelToolStripMenuItem
            // 
            this.defaultZoomLevelToolStripMenuItem.Checked = true;
            this.defaultZoomLevelToolStripMenuItem.CheckOnClick = true;
            this.defaultZoomLevelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultZoomLevelToolStripMenuItem.Name = "defaultZoomLevelToolStripMenuItem";
            this.defaultZoomLevelToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.defaultZoomLevelToolStripMenuItem.Text = "+-20 default X bounds";
            this.defaultZoomLevelToolStripMenuItem.Click += new System.EventHandler(this.defaultZoomLevelToolStripMenuItem_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(271, 6);
            // 
            // hidePolygonsCheckbox
            // 
            this.hidePolygonsCheckbox.Checked = true;
            this.hidePolygonsCheckbox.CheckOnClick = true;
            this.hidePolygonsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hidePolygonsCheckbox.Name = "hidePolygonsCheckbox";
            this.hidePolygonsCheckbox.Size = new System.Drawing.Size(274, 22);
            this.hidePolygonsCheckbox.Text = "Hide certain polygons/textures";
            this.hidePolygonsCheckbox.Click += new System.EventHandler(this.hidePolygonsCheckbox_Click);
            // 
            // swapPortraitsForWarioStylesToolStripMenuItem
            // 
            this.swapPortraitsForWarioStylesToolStripMenuItem.CheckOnClick = true;
            this.swapPortraitsForWarioStylesToolStripMenuItem.Name = "swapPortraitsForWarioStylesToolStripMenuItem";
            this.swapPortraitsForWarioStylesToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.swapPortraitsForWarioStylesToolStripMenuItem.Text = "Swap portraits for Wario styles";
            this.swapPortraitsForWarioStylesToolStripMenuItem.Visible = false;
            this.swapPortraitsForWarioStylesToolStripMenuItem.Click += new System.EventHandler(this.swapPortraitsForWarioStylesToolStripMenuItem_Click);
            // 
            // cBlissCheckbox
            // 
            this.cBlissCheckbox.CheckOnClick = true;
            this.cBlissCheckbox.Name = "cBlissCheckbox";
            this.cBlissCheckbox.Size = new System.Drawing.Size(274, 22);
            this.cBlissCheckbox.Text = "Use cBliss costume/portrait mappings";
            this.cBlissCheckbox.Click += new System.EventHandler(this.cBlissCheckbox_Click);
            // 
            // projectMCheckbox
            // 
            this.projectMCheckbox.CheckOnClick = true;
            this.projectMCheckbox.Name = "projectMCheckbox";
            this.projectMCheckbox.Size = new System.Drawing.Size(274, 22);
            this.projectMCheckbox.Text = "Use Project M 3.6 mappings";
            this.projectMCheckbox.Click += new System.EventHandler(this.projectMCheckbox_Click);
            // 
            // separator
            // 
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(271, 6);
            // 
            // nameportraitPreviewToolStripMenuItem
            // 
            this.nameportraitPreviewToolStripMenuItem.Checked = true;
            this.nameportraitPreviewToolStripMenuItem.CheckOnClick = true;
            this.nameportraitPreviewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nameportraitPreviewToolStripMenuItem.Name = "nameportraitPreviewToolStripMenuItem";
            this.nameportraitPreviewToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.nameportraitPreviewToolStripMenuItem.Text = "Name/portrait preview";
            this.nameportraitPreviewToolStripMenuItem.Click += new System.EventHandler(this.nameportraitPreviewToolStripMenuItem_Click);
            // 
            // backgroundColorToolStripMenuItem
            // 
            this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.backgroundColorToolStripMenuItem.Text = "Right panel BG color...";
            this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.backgroundColorToolStripMenuItem_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(44, 22);
            this.toolStripButton1.Text = "About";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // otherPVsFlowLayoutPanel
            // 
            this.otherPVsFlowLayoutPanel.AutoSize = true;
            this.otherPVsFlowLayoutPanel.Controls.Add(this.costumeNumberLabel);
            this.otherPVsFlowLayoutPanel.Controls.Add(this.battlePortraitViewer1);
            this.otherPVsFlowLayoutPanel.Controls.Add(this.resultPortraitViewer1);
            this.otherPVsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.otherPVsFlowLayoutPanel.Location = new System.Drawing.Point(550, 25);
            this.otherPVsFlowLayoutPanel.Name = "otherPVsFlowLayoutPanel";
            this.otherPVsFlowLayoutPanel.Size = new System.Drawing.Size(134, 381);
            this.otherPVsFlowLayoutPanel.TabIndex = 7;
            // 
            // brawlSplitter3
            // 
            this.brawlSplitter3.ControlToHide = this.splitContainer2;
            this.brawlSplitter3.Location = new System.Drawing.Point(206, 25);
            this.brawlSplitter3.Name = "brawlSplitter3";
            this.brawlSplitter3.Size = new System.Drawing.Size(8, 381);
            this.brawlSplitter3.TabIndex = 11;
            this.brawlSplitter3.TabStop = false;
            // 
            // brawlSplitter2
            // 
            this.brawlSplitter2.AllowResizing = false;
            this.brawlSplitter2.ControlToHide = this.otherPVsFlowLayoutPanel;
            this.brawlSplitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.brawlSplitter2.Location = new System.Drawing.Point(542, 25);
            this.brawlSplitter2.Name = "brawlSplitter2";
            this.brawlSplitter2.Size = new System.Drawing.Size(8, 381);
            this.brawlSplitter2.TabIndex = 10;
            this.brawlSplitter2.TabStop = false;
            // 
            // globalPVsFlowLayoutPanel
            // 
            this.globalPVsFlowLayoutPanel.AutoSize = true;
            this.globalPVsFlowLayoutPanel.Controls.Add(this.cssPortraitViewer1);
            this.globalPVsFlowLayoutPanel.Controls.Add(this.infoStockIconViewer1);
            this.globalPVsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.globalPVsFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.globalPVsFlowLayoutPanel.Location = new System.Drawing.Point(408, 25);
            this.globalPVsFlowLayoutPanel.Name = "globalPVsFlowLayoutPanel";
            this.globalPVsFlowLayoutPanel.Size = new System.Drawing.Size(134, 381);
            this.globalPVsFlowLayoutPanel.TabIndex = 13;
            // 
            // brawlSplitter1
            // 
            this.brawlSplitter1.AllowResizing = false;
            this.brawlSplitter1.ControlToHide = this.globalPVsFlowLayoutPanel;
            this.brawlSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.brawlSplitter1.Location = new System.Drawing.Point(400, 25);
            this.brawlSplitter1.Name = "brawlSplitter1";
            this.brawlSplitter1.Size = new System.Drawing.Size(8, 381);
            this.brawlSplitter1.TabIndex = 9;
            this.brawlSplitter1.TabStop = false;
            // 
            // modelManager1
            // 
            this.modelManager1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelManager1.Location = new System.Drawing.Point(214, 25);
            this.modelManager1.ModelPreviewSize = null;
            this.modelManager1.Name = "modelManager1";
            this.modelManager1.Size = new System.Drawing.Size(186, 381);
            this.modelManager1.TabIndex = 1;
            // 
            // cssPortraitViewer1
            // 
            this.cssPortraitViewer1.Location = new System.Drawing.Point(3, 3);
            this.cssPortraitViewer1.Name = "cssPortraitViewer1";
            this.cssPortraitViewer1.NamePortraitPreview = false;
            this.cssPortraitViewer1.Size = new System.Drawing.Size(128, 328);
            this.cssPortraitViewer1.TabIndex = 3;
            // 
            // infoStockIconViewer1
            // 
            this.infoStockIconViewer1.Location = new System.Drawing.Point(3, 337);
            this.infoStockIconViewer1.Name = "infoStockIconViewer1";
            this.infoStockIconViewer1.Size = new System.Drawing.Size(128, 40);
            this.infoStockIconViewer1.TabIndex = 12;
            // 
            // costumeNumberLabel
            // 
            this.costumeNumberLabel.Location = new System.Drawing.Point(3, 0);
            this.costumeNumberLabel.Name = "costumeNumberLabel";
            this.costumeNumberLabel.Size = new System.Drawing.Size(128, 20);
            this.costumeNumberLabel.TabIndex = 6;
            this.costumeNumberLabel.Text = "No costume selected";
            this.costumeNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // battlePortraitViewer1
            // 
            this.battlePortraitViewer1.Location = new System.Drawing.Point(3, 23);
            this.battlePortraitViewer1.Name = "battlePortraitViewer1";
            this.battlePortraitViewer1.Size = new System.Drawing.Size(128, 120);
            this.battlePortraitViewer1.TabIndex = 5;
            // 
            // resultPortraitViewer1
            // 
            this.resultPortraitViewer1.Location = new System.Drawing.Point(3, 149);
            this.resultPortraitViewer1.Name = "resultPortraitViewer1";
            this.resultPortraitViewer1.Size = new System.Drawing.Size(128, 224);
            this.resultPortraitViewer1.TabIndex = 4;
            // 
            // use16ptFontToolStripMenuItem
            // 
            this.use16ptFontToolStripMenuItem.CheckOnClick = true;
            this.use16ptFontToolStripMenuItem.Name = "use16ptFontToolStripMenuItem";
            this.use16ptFontToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.use16ptFontToolStripMenuItem.Text = "Use 16pt font";
            this.use16ptFontToolStripMenuItem.Click += new System.EventHandler(this.use16ptFontToolStripMenuItem_Click);
            // 
            // CostumeManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 406);
            this.Controls.Add(this.modelManager1);
            this.Controls.Add(this.brawlSplitter3);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.brawlSplitter1);
            this.Controls.Add(this.globalPVsFlowLayoutPanel);
            this.Controls.Add(this.brawlSplitter2);
            this.Controls.Add(this.otherPVsFlowLayoutPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CostumeManagerForm";
            this.Text = "Brawl Costume Manager";
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.otherPVsFlowLayoutPanel.ResumeLayout(false);
            this.globalPVsFlowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private BrawlManagers.CostumeManager.ModelManager modelManager1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listBox2;
        private CSSPortraitViewer cssPortraitViewer1;
        private ResultSinglePortraitViewer resultPortraitViewer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton changeDirectory;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem hidePolygonsCheckbox;
        private System.Windows.Forms.ToolStripMenuItem cBlissCheckbox;
        private System.Windows.Forms.ToolStripSeparator separator;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToToolStripMenuItem;
        private BattleSinglePortraitViewer battlePortraitViewer1;
        private System.Windows.Forms.ToolStripMenuItem updateSSSStockIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripMenuItem swapPortraitsForWarioStylesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToOtherPacpcsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nameportraitPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenshotPortraitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem limitModelViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem defaultZoomLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem projectMCheckbox;
        private System.Windows.Forms.FlowLayoutPanel otherPVsFlowLayoutPanel;
        private System.Windows.Forms.ToolStripMenuItem updateMewtwoHatForCurrentKirbyToolStripMenuItem;
        private CostumeNumberLabel costumeNumberLabel;
        private BrawlSplitter brawlSplitter1;
        private BrawlSplitter brawlSplitter2;
        private BrawlSplitter brawlSplitter3;
        private InfoStockIconViewer infoStockIconViewer1;
        private System.Windows.Forms.FlowLayoutPanel globalPVsFlowLayoutPanel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem use16ptFontToolStripMenuItem;
    }
}
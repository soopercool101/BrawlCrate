using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Modeling;
using System.ComponentModel;
using BrawlLib;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class LeftPanel : UserControl
    {
        #region Designer

        public CheckedListBox lstObjects;
        private CheckBox chkAllObj;
        private Button btnObjects;
        private ProxySplitter spltAnimObj;
        private Panel pnlAnims;
        private Button btnAnims;
        private Panel pnlTextures;
        private CheckedListBox lstTextures;
        private CheckBox chkAllTextures;
        private Button btnTextures;
        private ProxySplitter spltObjTex;
        private ContextMenuStrip ctxTextures;
        private ToolStripMenuItem sourceToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem replaceTextureToolStripMenuItem;
        private ToolStripMenuItem sizeToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem renameTextureTextureToolStripMenuItem;
        private ToolStripMenuItem exportTextureToolStripMenuItem;
        private IContainer components;
        public CheckBox chkSyncVis;
        public ListView listAnims;
        private ColumnHeader nameColumn;
        public ComboBox fileType;
        private Panel panel1;
        private ContextMenuStrip ctxAnim;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem renameToolStripMenuItem;
        public Button btnSaveAnims;
        public Button btnLoad;
        private TransparentPanel overObjPnl;
        private TransparentPanel overTexPnl;
        private ToolStripMenuItem createNewToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem chkLoop;
        private ContextMenuStrip ctxAnimList;
        private ToolStripMenuItem AnimListNewAnim;
        private ToolStripMenuItem inModelsBRRESToolStripMenuItem;
        private ToolStripMenuItem inExternalFileToolStripMenuItem;
        private ToolStripMenuItem matrixModeToolStripMenuItem;
        private ToolStripMenuItem chkMtxMaya;
        private ToolStripMenuItem chkMtxXSI;
        private ToolStripMenuItem chkMtxMax;
        private Panel panel2;
        private TextBox txtSearchAnim;
        public CheckBox chkContains;
        private Splitter spltDrawCalls;
        public CheckedListBox lstDrawCalls;
        private Panel pnlObjects;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Animations", System.Windows.Forms.HorizontalAlignment.Left);
            this.pnlObjects = new System.Windows.Forms.Panel();
            this.lstObjects = new System.Windows.Forms.CheckedListBox();
            this.spltDrawCalls = new System.Windows.Forms.Splitter();
            this.lstDrawCalls = new System.Windows.Forms.CheckedListBox();
            this.chkAllObj = new System.Windows.Forms.CheckBox();
            this.chkSyncVis = new System.Windows.Forms.CheckBox();
            this.btnObjects = new System.Windows.Forms.Button();
            this.pnlAnims = new System.Windows.Forms.Panel();
            this.listAnims = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctxAnimList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AnimListNewAnim = new System.Windows.Forms.ToolStripMenuItem();
            this.inModelsBRRESToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inExternalFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtSearchAnim = new System.Windows.Forms.TextBox();
            this.chkContains = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveAnims = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.fileType = new System.Windows.Forms.ComboBox();
            this.btnAnims = new System.Windows.Forms.Button();
            this.ctxTextures = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTextureTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlTextures = new System.Windows.Forms.Panel();
            this.lstTextures = new System.Windows.Forms.CheckedListBox();
            this.chkAllTextures = new System.Windows.Forms.CheckBox();
            this.btnTextures = new System.Windows.Forms.Button();
            this.ctxAnim = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chkLoop = new System.Windows.Forms.ToolStripMenuItem();
            this.matrixModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkMtxMaya = new System.Windows.Forms.ToolStripMenuItem();
            this.chkMtxXSI = new System.Windows.Forms.ToolStripMenuItem();
            this.chkMtxMax = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overObjPnl = new System.Windows.Forms.TransparentPanel();
            this.spltObjTex = new System.Windows.Forms.ProxySplitter();
            this.spltAnimObj = new System.Windows.Forms.ProxySplitter();
            this.overTexPnl = new System.Windows.Forms.TransparentPanel();
            this.pnlObjects.SuspendLayout();
            this.pnlAnims.SuspendLayout();
            this.ctxAnimList.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ctxTextures.SuspendLayout();
            this.pnlTextures.SuspendLayout();
            this.ctxAnim.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlObjects
            // 
            this.pnlObjects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlObjects.Controls.Add(this.overObjPnl);
            this.pnlObjects.Controls.Add(this.lstObjects);
            this.pnlObjects.Controls.Add(this.spltDrawCalls);
            this.pnlObjects.Controls.Add(this.lstDrawCalls);
            this.pnlObjects.Controls.Add(this.chkAllObj);
            this.pnlObjects.Controls.Add(this.chkSyncVis);
            this.pnlObjects.Controls.Add(this.btnObjects);
            this.pnlObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlObjects.Location = new System.Drawing.Point(0, 182);
            this.pnlObjects.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlObjects.Name = "pnlObjects";
            this.pnlObjects.Size = new System.Drawing.Size(172, 150);
            this.pnlObjects.TabIndex = 0;
            // 
            // lstObjects
            // 
            this.lstObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstObjects.CausesValidation = false;
            this.lstObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObjects.IntegralHeight = false;
            this.lstObjects.Location = new System.Drawing.Point(0, 66);
            this.lstObjects.Margin = new System.Windows.Forms.Padding(0);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(170, 45);
            this.lstObjects.TabIndex = 4;
            this.lstObjects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstPolygons_ItemCheck);
            this.lstObjects.SelectedValueChanged += new System.EventHandler(this.lstPolygons_SelectedValueChanged);
            this.lstObjects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPolygons_KeyDown);
            this.lstObjects.Leave += new System.EventHandler(this.lstObjects_Leave);
            // 
            // spltDrawCalls
            // 
            this.spltDrawCalls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spltDrawCalls.Location = new System.Drawing.Point(0, 111);
            this.spltDrawCalls.Name = "spltDrawCalls";
            this.spltDrawCalls.Size = new System.Drawing.Size(170, 3);
            this.spltDrawCalls.TabIndex = 9;
            this.spltDrawCalls.TabStop = false;
            this.spltDrawCalls.Visible = false;
            // 
            // lstDrawCalls
            // 
            this.lstDrawCalls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstDrawCalls.FormattingEnabled = true;
            this.lstDrawCalls.IntegralHeight = false;
            this.lstDrawCalls.Location = new System.Drawing.Point(0, 114);
            this.lstDrawCalls.Name = "lstDrawCalls";
            this.lstDrawCalls.Size = new System.Drawing.Size(170, 34);
            this.lstDrawCalls.TabIndex = 0;
            this.lstDrawCalls.Visible = false;
            this.lstDrawCalls.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstDrawCalls_ItemCheck);
            this.lstDrawCalls.SelectedIndexChanged += new System.EventHandler(this.lstDrawCalls_SelectedIndexChanged);
            this.lstDrawCalls.DoubleClick += new System.EventHandler(this.lstDrawCalls_DoubleClick);
            // 
            // chkAllObj
            // 
            this.chkAllObj.Checked = true;
            this.chkAllObj.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllObj.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllObj.Location = new System.Drawing.Point(0, 46);
            this.chkAllObj.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllObj.Name = "chkAllObj";
            this.chkAllObj.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllObj.Size = new System.Drawing.Size(170, 20);
            this.chkAllObj.TabIndex = 5;
            this.chkAllObj.Text = "All";
            this.chkAllObj.UseVisualStyleBackColor = false;
            this.chkAllObj.CheckStateChanged += new System.EventHandler(this.chkAllPoly_CheckStateChanged);
            // 
            // chkSyncVis
            // 
            this.chkSyncVis.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkSyncVis.Location = new System.Drawing.Point(0, 26);
            this.chkSyncVis.Margin = new System.Windows.Forms.Padding(0);
            this.chkSyncVis.Name = "chkSyncVis";
            this.chkSyncVis.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkSyncVis.Size = new System.Drawing.Size(170, 20);
            this.chkSyncVis.TabIndex = 7;
            this.chkSyncVis.Text = "Sync VIS0";
            this.chkSyncVis.UseVisualStyleBackColor = false;
            // 
            // btnObjects
            // 
            this.btnObjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnObjects.Location = new System.Drawing.Point(0, 0);
            this.btnObjects.Name = "btnObjects";
            this.btnObjects.Size = new System.Drawing.Size(170, 26);
            this.btnObjects.TabIndex = 6;
            this.btnObjects.Text = "Objects";
            this.btnObjects.UseVisualStyleBackColor = true;
            this.btnObjects.Click += new System.EventHandler(this.btnObjects_Click);
            // 
            // pnlAnims
            // 
            this.pnlAnims.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAnims.Controls.Add(this.listAnims);
            this.pnlAnims.Controls.Add(this.panel2);
            this.pnlAnims.Controls.Add(this.panel1);
            this.pnlAnims.Controls.Add(this.btnAnims);
            this.pnlAnims.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAnims.Location = new System.Drawing.Point(0, 0);
            this.pnlAnims.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlAnims.Name = "pnlAnims";
            this.pnlAnims.Size = new System.Drawing.Size(172, 178);
            this.pnlAnims.TabIndex = 2;
            // 
            // listAnims
            // 
            this.listAnims.AutoArrange = false;
            this.listAnims.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.listAnims.ContextMenuStrip = this.ctxAnimList;
            this.listAnims.Cursor = System.Windows.Forms.Cursors.Default;
            this.listAnims.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup2.Header = "Animations";
            listViewGroup2.Name = "grpAnims";
            this.listAnims.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
            this.listAnims.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listAnims.HideSelection = false;
            this.listAnims.Location = new System.Drawing.Point(0, 73);
            this.listAnims.MultiSelect = false;
            this.listAnims.Name = "listAnims";
            this.listAnims.Size = new System.Drawing.Size(170, 103);
            this.listAnims.TabIndex = 25;
            this.listAnims.UseCompatibleStateImageBehavior = false;
            this.listAnims.View = System.Windows.Forms.View.Details;
            this.listAnims.SelectedIndexChanged += new System.EventHandler(this.listAnims_SelectedIndexChanged);
            this.listAnims.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listAnims_KeyDown);
            this.listAnims.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listAnims_MouseDown);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 160;
            // 
            // ctxAnimList
            // 
            this.ctxAnimList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxAnimList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AnimListNewAnim});
            this.ctxAnimList.Name = "ctxAnim";
            this.ctxAnimList.Size = new System.Drawing.Size(235, 30);
            this.ctxAnimList.Opening += new System.ComponentModel.CancelEventHandler(this.ctxAnimList_Opening);
            // 
            // AnimListNewAnim
            // 
            this.AnimListNewAnim.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inModelsBRRESToolStripMenuItem,
            this.inExternalFileToolStripMenuItem});
            this.AnimListNewAnim.Name = "AnimListNewAnim";
            this.AnimListNewAnim.Size = new System.Drawing.Size(234, 26);
            this.AnimListNewAnim.Text = "Create New Animation";
            this.AnimListNewAnim.Click += new System.EventHandler(this.inModelsBRRESToolStripMenuItem_Click);
            // 
            // inModelsBRRESToolStripMenuItem
            // 
            this.inModelsBRRESToolStripMenuItem.Enabled = false;
            this.inModelsBRRESToolStripMenuItem.Name = "inModelsBRRESToolStripMenuItem";
            this.inModelsBRRESToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.inModelsBRRESToolStripMenuItem.Text = "In Model\'s BRRES";
            this.inModelsBRRESToolStripMenuItem.Visible = false;
            this.inModelsBRRESToolStripMenuItem.Click += new System.EventHandler(this.inModelsBRRESToolStripMenuItem_Click);
            // 
            // inExternalFileToolStripMenuItem
            // 
            this.inExternalFileToolStripMenuItem.Enabled = false;
            this.inExternalFileToolStripMenuItem.Name = "inExternalFileToolStripMenuItem";
            this.inExternalFileToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.inExternalFileToolStripMenuItem.Text = "In External File";
            this.inExternalFileToolStripMenuItem.Visible = false;
            this.inExternalFileToolStripMenuItem.Click += new System.EventHandler(this.inExternalFileToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtSearchAnim);
            this.panel2.Controls.Add(this.chkContains);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(170, 21);
            this.panel2.TabIndex = 32;
            // 
            // txtSearchAnim
            // 
            this.txtSearchAnim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchAnim.ForeColor = System.Drawing.Color.Gray;
            this.txtSearchAnim.Location = new System.Drawing.Point(0, 0);
            this.txtSearchAnim.Name = "txtSearchAnim";
            this.txtSearchAnim.Size = new System.Drawing.Size(82, 22);
            this.txtSearchAnim.TabIndex = 30;
            this.txtSearchAnim.Text = "Search for an animation...";
            this.txtSearchAnim.TextChanged += new System.EventHandler(this.txtSearchAnim_TextChanged);
            this.txtSearchAnim.Enter += new System.EventHandler(this.txtSearchAnim_Enter);
            this.txtSearchAnim.Leave += new System.EventHandler(this.txtSearchAnim_Leave);
            // 
            // chkContains
            // 
            this.chkContains.AutoSize = true;
            this.chkContains.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkContains.Location = new System.Drawing.Point(82, 0);
            this.chkContains.Margin = new System.Windows.Forms.Padding(0);
            this.chkContains.Name = "chkContains";
            this.chkContains.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.chkContains.Size = new System.Drawing.Size(88, 21);
            this.chkContains.TabIndex = 32;
            this.chkContains.Text = "Contains";
            this.chkContains.UseVisualStyleBackColor = true;
            this.chkContains.CheckedChanged += new System.EventHandler(this.chkContains_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Controls.Add(this.btnSaveAnims);
            this.panel1.Controls.Add(this.fileType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(170, 26);
            this.panel1.TabIndex = 27;
            // 
            // btnSaveAnims
            // 
            this.btnSaveAnims.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSaveAnims.Location = new System.Drawing.Point(51, 0);
            this.btnSaveAnims.Name = "btnSaveAnims";
            this.btnSaveAnims.Size = new System.Drawing.Size(60, 26);
            this.btnSaveAnims.TabIndex = 28;
            this.btnSaveAnims.Text = "Save";
            this.btnSaveAnims.UseVisualStyleBackColor = true;
            this.btnSaveAnims.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.Location = new System.Drawing.Point(0, 0);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(51, 26);
            this.btnLoad.TabIndex = 27;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileType
            // 
            this.fileType.Dock = System.Windows.Forms.DockStyle.Right;
            this.fileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType.FormattingEnabled = true;
            this.fileType.Location = new System.Drawing.Point(111, 0);
            this.fileType.Name = "fileType";
            this.fileType.Size = new System.Drawing.Size(59, 24);
            this.fileType.TabIndex = 26;
            this.fileType.SelectedIndexChanged += new System.EventHandler(this.fileType_SelectedIndexChanged);
            // 
            // btnAnims
            // 
            this.btnAnims.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAnims.Location = new System.Drawing.Point(0, 0);
            this.btnAnims.Name = "btnAnims";
            this.btnAnims.Size = new System.Drawing.Size(170, 26);
            this.btnAnims.TabIndex = 7;
            this.btnAnims.Text = "Animations";
            this.btnAnims.UseVisualStyleBackColor = true;
            this.btnAnims.Click += new System.EventHandler(this.btnAnims_Click);
            // 
            // ctxTextures
            // 
            this.ctxTextures.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxTextures.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.sizeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.viewToolStripMenuItem,
            this.exportTextureToolStripMenuItem,
            this.replaceTextureToolStripMenuItem,
            this.renameTextureTextureToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.ctxTextures.Name = "ctxTextures";
            this.ctxTextures.Size = new System.Drawing.Size(147, 192);
            this.ctxTextures.Opening += new System.ComponentModel.CancelEventHandler(this.ctxTextures_Opening);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Enabled = false;
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.sourceToolStripMenuItem.Text = "Source";
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.Enabled = false;
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.sizeToolStripMenuItem.Text = "Size";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.viewToolStripMenuItem.Text = "View...";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // exportTextureToolStripMenuItem
            // 
            this.exportTextureToolStripMenuItem.Name = "exportTextureToolStripMenuItem";
            this.exportTextureToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.exportTextureToolStripMenuItem.Text = "Export...";
            this.exportTextureToolStripMenuItem.Click += new System.EventHandler(this.exportTextureToolStripMenuItem_Click);
            // 
            // replaceTextureToolStripMenuItem
            // 
            this.replaceTextureToolStripMenuItem.Name = "replaceTextureToolStripMenuItem";
            this.replaceTextureToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.replaceTextureToolStripMenuItem.Text = "Replace...";
            this.replaceTextureToolStripMenuItem.Click += new System.EventHandler(this.replaceTextureToolStripMenuItem_Click);
            // 
            // renameTextureTextureToolStripMenuItem
            // 
            this.renameTextureTextureToolStripMenuItem.Name = "renameTextureTextureToolStripMenuItem";
            this.renameTextureTextureToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.renameTextureTextureToolStripMenuItem.Text = "Rename";
            this.renameTextureTextureToolStripMenuItem.Click += new System.EventHandler(this.renameTextureToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.resetToolStripMenuItem.Text = "Reload";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // pnlTextures
            // 
            this.pnlTextures.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTextures.Controls.Add(this.overTexPnl);
            this.pnlTextures.Controls.Add(this.lstTextures);
            this.pnlTextures.Controls.Add(this.chkAllTextures);
            this.pnlTextures.Controls.Add(this.btnTextures);
            this.pnlTextures.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTextures.Location = new System.Drawing.Point(0, 336);
            this.pnlTextures.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlTextures.Name = "pnlTextures";
            this.pnlTextures.Size = new System.Drawing.Size(172, 164);
            this.pnlTextures.TabIndex = 3;
            // 
            // lstTextures
            // 
            this.lstTextures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstTextures.CausesValidation = false;
            this.lstTextures.ContextMenuStrip = this.ctxTextures;
            this.lstTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTextures.IntegralHeight = false;
            this.lstTextures.Location = new System.Drawing.Point(0, 46);
            this.lstTextures.Margin = new System.Windows.Forms.Padding(0);
            this.lstTextures.Name = "lstTextures";
            this.lstTextures.Size = new System.Drawing.Size(170, 116);
            this.lstTextures.TabIndex = 7;
            this.lstTextures.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstTextures_ItemCheck);
            this.lstTextures.SelectedValueChanged += new System.EventHandler(this.lstTextures_SelectedValueChanged);
            this.lstTextures.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstTextures_KeyDown);
            this.lstTextures.Leave += new System.EventHandler(this.lstTextures_Leave);
            this.lstTextures.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstTextures_MouseDown);
            // 
            // chkAllTextures
            // 
            this.chkAllTextures.Checked = true;
            this.chkAllTextures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllTextures.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllTextures.Location = new System.Drawing.Point(0, 26);
            this.chkAllTextures.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllTextures.Name = "chkAllTextures";
            this.chkAllTextures.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllTextures.Size = new System.Drawing.Size(170, 20);
            this.chkAllTextures.TabIndex = 8;
            this.chkAllTextures.Text = "All";
            this.chkAllTextures.UseVisualStyleBackColor = false;
            this.chkAllTextures.CheckStateChanged += new System.EventHandler(this.chkAllTextures_CheckStateChanged);
            // 
            // btnTextures
            // 
            this.btnTextures.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTextures.Location = new System.Drawing.Point(0, 0);
            this.btnTextures.Name = "btnTextures";
            this.btnTextures.Size = new System.Drawing.Size(170, 26);
            this.btnTextures.TabIndex = 9;
            this.btnTextures.Text = "Textures";
            this.btnTextures.UseVisualStyleBackColor = true;
            this.btnTextures.Click += new System.EventHandler(this.btnTextures_Click);
            // 
            // ctxAnim
            // 
            this.ctxAnim.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxAnim.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.chkLoop,
            this.matrixModeToolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.createNewToolStripMenuItem});
            this.ctxAnim.Name = "ctxAnim";
            this.ctxAnim.Size = new System.Drawing.Size(235, 218);
            this.ctxAnim.Opening += new System.ComponentModel.CancelEventHandler(this.ctxAnim_Opening);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(234, 26);
            this.toolStripMenuItem2.Text = "Source";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // chkLoop
            // 
            this.chkLoop.CheckOnClick = true;
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(234, 26);
            this.chkLoop.Text = "Loop";
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // matrixModeToolStripMenuItem
            // 
            this.matrixModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkMtxMaya,
            this.chkMtxXSI,
            this.chkMtxMax});
            this.matrixModeToolStripMenuItem.Name = "matrixModeToolStripMenuItem";
            this.matrixModeToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.matrixModeToolStripMenuItem.Text = "Matrix Mode";
            this.matrixModeToolStripMenuItem.Visible = false;
            // 
            // chkMtxMaya
            // 
            this.chkMtxMaya.CheckOnClick = true;
            this.chkMtxMaya.Name = "chkMtxMaya";
            this.chkMtxMaya.Size = new System.Drawing.Size(139, 26);
            this.chkMtxMaya.Text = "Maya";
            this.chkMtxMaya.CheckedChanged += new System.EventHandler(this.chkMtxMaya_CheckedChanged);
            // 
            // chkMtxXSI
            // 
            this.chkMtxXSI.CheckOnClick = true;
            this.chkMtxXSI.Name = "chkMtxXSI";
            this.chkMtxXSI.Size = new System.Drawing.Size(139, 26);
            this.chkMtxXSI.Text = "XSI";
            this.chkMtxXSI.CheckedChanged += new System.EventHandler(this.chkMtxXSI_CheckedChanged);
            // 
            // chkMtxMax
            // 
            this.chkMtxMax.CheckOnClick = true;
            this.chkMtxMax.Name = "chkMtxMax";
            this.chkMtxMax.Size = new System.Drawing.Size(139, 26);
            this.chkMtxMax.Text = "3ds Max";
            this.chkMtxMax.CheckedChanged += new System.EventHandler(this.chkMtxMax_CheckedChanged);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(234, 26);
            this.toolStripMenuItem3.Text = "Export...";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(234, 26);
            this.toolStripMenuItem4.Text = "Replace...";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // createNewToolStripMenuItem
            // 
            this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            this.createNewToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.createNewToolStripMenuItem.Text = "Create New Animation";
            this.createNewToolStripMenuItem.Click += new System.EventHandler(this.createNewToolStripMenuItem_Click);
            // 
            // overObjPnl
            // 
            this.overObjPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overObjPnl.Location = new System.Drawing.Point(0, 66);
            this.overObjPnl.Name = "overObjPnl";
            this.overObjPnl.Size = new System.Drawing.Size(170, 45);
            this.overObjPnl.TabIndex = 8;
            this.overObjPnl.Paint += new System.Windows.Forms.PaintEventHandler(this.overObjPnl_Paint);
            // 
            // spltObjTex
            // 
            this.spltObjTex.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.spltObjTex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spltObjTex.Location = new System.Drawing.Point(0, 332);
            this.spltObjTex.Name = "spltObjTex";
            this.spltObjTex.Size = new System.Drawing.Size(172, 4);
            this.spltObjTex.TabIndex = 4;
            this.spltObjTex.Dragged += new System.Windows.Forms.SplitterEventHandler(this.spltObjTex_Dragged);
            // 
            // spltAnimObj
            // 
            this.spltAnimObj.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.spltAnimObj.Dock = System.Windows.Forms.DockStyle.Top;
            this.spltAnimObj.Location = new System.Drawing.Point(0, 178);
            this.spltAnimObj.Name = "spltAnimObj";
            this.spltAnimObj.Size = new System.Drawing.Size(172, 4);
            this.spltAnimObj.TabIndex = 1;
            this.spltAnimObj.Dragged += new System.Windows.Forms.SplitterEventHandler(this.spltAnimObj_Dragged);
            // 
            // overTexPnl
            // 
            this.overTexPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overTexPnl.Location = new System.Drawing.Point(0, 46);
            this.overTexPnl.Name = "overTexPnl";
            this.overTexPnl.Size = new System.Drawing.Size(170, 116);
            this.overTexPnl.TabIndex = 9;
            this.overTexPnl.Paint += new System.Windows.Forms.PaintEventHandler(this.overTexPnl_Paint);
            // 
            // LeftPanel
            // 
            this.Controls.Add(this.pnlObjects);
            this.Controls.Add(this.spltObjTex);
            this.Controls.Add(this.spltAnimObj);
            this.Controls.Add(this.pnlAnims);
            this.Controls.Add(this.pnlTextures);
            this.Name = "LeftPanel";
            this.Size = new System.Drawing.Size(172, 500);
            this.pnlObjects.ResumeLayout(false);
            this.pnlAnims.ResumeLayout(false);
            this.ctxAnimList.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ctxTextures.ResumeLayout(false);
            this.pnlTextures.ResumeLayout(false);
            this.ctxAnim.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public bool _closing = false;

        public ModelEditControl _mainWindow;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModelEditControl MainWindow
        {
            get { return _mainWindow; }
            set { _mainWindow = value; }
        }

        public ListViewGroup _AnimGroupBRRES = new ListViewGroup("In BRRES");
        public ListViewGroup _AnimGroupNotBRRES = new ListViewGroup("Not In BRRES");
        //private ListViewGroup _AnimGroupExternal = new ListViewGroup("External File");
        public List<ListViewGroup> _AnimGroupsExternal = new List<ListViewGroup>();

        public bool _syncPat0 = false;
        private bool _updating = false;
        public string _lastSelected = null;
        public SRT0Node _srt0Selection = null;
        public PAT0Node _pat0Selection = null;
        private IObject _selectedObject;
        private MDL0TextureNode _selectedTexture;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IObject SelectedObject { get { return _selectedObject; } set { lstObjects.SelectedItem = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0TextureNode SelectedTexture { get { return _selectedTexture; } set { lstTextures.SelectedItem = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone { get { return _mainWindow.SelectedBone; } set { _mainWindow.SelectedBone = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef
        {
            get { return _mainWindow.TargetTexRef; }
            set
            {
                _mainWindow.TargetTexRef = value; 
                if (_mainWindow.SelectedSRT0 != null && TargetTexRef != null && _mainWindow.KeyframePanel != null)
                    _mainWindow.KeyframePanel.TargetSequence = _mainWindow.SRT0Editor.TexEntry;
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetCollision
        {
            get { return _mainWindow.TargetCollision; }
            set { _mainWindow.TargetCollision = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get { return _mainWindow.SelectedCHR0; }
            set { _mainWindow.SelectedCHR0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0Node SelectedSRT0
        {
            get { return _mainWindow.SelectedSRT0; }
            set { _mainWindow.SelectedSRT0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0Node SelectedSHP0
        {
            get { return _mainWindow.SelectedSHP0; }
            set { _mainWindow.SelectedSHP0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PAT0Node SelectedPAT0
        {
            get { return _mainWindow.SelectedPAT0; }
            set { _mainWindow.SelectedPAT0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0Node SelectedVIS0
        {
            get { return _mainWindow.SelectedVIS0; }
            set { _mainWindow.SelectedVIS0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NW4RAnimType TargetAnimType
        {
            get { return (NW4RAnimType)fileType.SelectedIndex; }
            set
            {
                if (fileType.SelectedIndex != (int)value)
                    fileType.SelectedIndex = (int)value;
            }
        }

        public LeftPanel()
        {
            InitializeComponent();
            listAnims.Groups.Add(_AnimGroupBRRES);
            listAnims.Groups.Add(_AnimGroupNotBRRES);
            //listAnims.Groups.Add(_AnimGroupExternal);
        }

        public bool LoadAnims(
            ResourceNode node,
            NW4RAnimType type,
            string compare,
            bool contains,
            bool inBRRES,
            ListViewGroup externalGroup = null)
        {
            bool ib = inBRRES || (TargetModel != null && TargetModel is MDL0Node && node == ((MDL0Node)TargetModel).BRESNode);

            if (!_mainWindow.chkBRRESAnims.Checked && ib)
                return false;

            bool found = false;
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.U8:
                case ResourceType.U8Folder:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        found = LoadAnims(n, type, compare, contains, ib, externalGroup) || found;
                    break;
                case ResourceType.CHR0: found = true; if (type == NW4RAnimType.CHR) goto Add; break;
                case ResourceType.SRT0: found = true; if (type == NW4RAnimType.SRT) goto Add; break;
                case ResourceType.SHP0: found = true; if (type == NW4RAnimType.SHP) goto Add; break;
                case ResourceType.PAT0: found = true; if (type == NW4RAnimType.PAT) goto Add; break;
                case ResourceType.VIS0: found = true; if (type == NW4RAnimType.VIS) goto Add; break;
                case ResourceType.SCN0: found = true; if (type == NW4RAnimType.SCN) goto Add; break;
                case ResourceType.CLR0: found = true; if (type == NW4RAnimType.CLR) goto Add; break;
            }
            return found;
            
            Add:
            if (String.IsNullOrEmpty(compare) ||
                (contains && node.Name.Contains(compare, StringComparison.OrdinalIgnoreCase)) ||
                node.Name.StartsWith(compare, StringComparison.OrdinalIgnoreCase))
            {
                ListViewGroup u = externalGroup != null ? externalGroup : ib ? _AnimGroupBRRES : _AnimGroupNotBRRES;
                if (u == null)
                    Console.WriteLine();
                listAnims.Items.Add(new ListViewItem(node.Name, (int)node.ResourceType, u) { Tag = node });
            }
            return found;
        }

        public void UpdateAnimations() { UpdateAnimations(TargetAnimType); }
        public void UpdateAnimations(NW4RAnimType type)
        {
            _mainWindow.Updating = true;

            string name = listAnims.SelectedItems != null && listAnims.SelectedItems.Count > 0 ? listAnims.SelectedItems[0].Tag.ToString() : null;
            int frame = CurrentFrame;

            string text = txtSearchAnim.Text;
            bool addAll = String.IsNullOrEmpty(text) || txtSearchAnim.ForeColor == Color.Gray;

            listAnims.BeginUpdate();
            listAnims.Items.Clear();

            listAnims.Groups.Clear();

            if (TargetModel != null && TargetModel is MDL0Node)
            {
                _AnimGroupBRRES.Header = String.Format("In BRRES ({0})", TargetModel.ToString());
                _AnimGroupNotBRRES.Header = String.Format("Not in BRRES ({0})", TargetModel.ToString());

                listAnims.Groups.Add(_AnimGroupBRRES);
                listAnims.Groups.Add(_AnimGroupNotBRRES);

                ResourceNode node = _mainWindow.chkNonBRRESAnims.Checked ? ((MDL0Node)TargetModel).RootNode : ((MDL0Node)TargetModel).BRESNode;
                LoadAnims(node, type, addAll ? null : text, chkContains.Checked, false);
            }

            if (_mainWindow.chkExternalAnims.Checked)
            {
                ResourceNode root = TargetModel == null ? null : ((ResourceNode)TargetModel).RootNode;
                foreach (ResourceNode r in _mainWindow._openedFiles)
                    if (r != root && r != null)
                    {
                        ListViewGroup g = new ListViewGroup(r.Name);
                        listAnims.Groups.Add(g);
                        _AnimGroupsExternal.Add(g);
                        LoadAnims(r, type, addAll ? null : text, chkContains.Checked, false, g);
                    }
            }

            listAnims.EndUpdate();

            //Reselect the animation
            for (int i = 0; i < listAnims.Items.Count; i++)
                if (listAnims.Items[i].Tag.ToString() == name)
                {
                    listAnims.Items[i].Selected = true;
                    break;
                }

            _mainWindow.Updating = false;
            CurrentFrame = frame;

            if ((_mainWindow.GetAnimation(TargetAnimType) == null) && (listAnims.SelectedItems.Count == 0))
                _mainWindow.GetFiles(NW4RAnimType.None);
        }

        public void LoadAnimations(ResourceNode node)
        {
            if (_mainWindow.chkExternalAnims.Checked && node != null)
            {
                ResourceNode root = TargetModel == null ? null : ((ResourceNode)TargetModel).RootNode;
                if (node.RootNode != root)
                {
                    ListViewGroup g = new ListViewGroup(node.Name);
                    listAnims.Groups.Add(g);
                    _AnimGroupsExternal.Add(g);
                    LoadAnims(node, TargetAnimType, null, false, false, g);
                }
            }
        }

        public void UpdateSRT0Selection(SRT0Node selection)
        {
            _srt0Selection = selection;
            if (_srt0Selection == null && _pat0Selection == null)
                overObjPnl.Visible = overTexPnl.Visible = false;
            else
            {
                overObjPnl.Visible = overTexPnl.Visible = true;
                overObjPnl.Invalidate();
                overTexPnl.Invalidate();
            }
        }

        public void UpdatePAT0Selection(PAT0Node selection)
        {
            _pat0Selection = selection;
            if (_pat0Selection == null && _srt0Selection == null)
                overObjPnl.Visible = overTexPnl.Visible = false;
            else
            {
                overObjPnl.Visible = overTexPnl.Visible = true;
                overObjPnl.Invalidate();
                overTexPnl.Invalidate();
            }
        }

        public void UpdateTextures()
        {
            _mainWindow.Updating = true;

            string Name = lstTextures.SelectedItems != null && lstTextures.SelectedItems.Count > 0 ? lstTextures.SelectedItems[0].ToString() : null;

            lstTextures.BeginUpdate();
            lstTextures.Items.Clear();

            chkAllTextures.CheckState = CheckState.Checked;

            ResourceNode n;
            if (_selectedObject != null && _mainWindow.SyncTexturesToObjectList)
            {
                //Add textures the selected object uses
                if (_selectedObject is MDL0ObjectNode)
                    foreach (MDL0MaterialRefNode tref in ((MDL0ObjectNode)_selectedObject)._drawCalls[0].MaterialNode.Children)
                        lstTextures.Items.Add(tref.TextureNode, tref.TextureNode.Enabled);
            }
            else if (TargetModel != null)
            {
                //Add all model textures
                if (TargetModel is MDL0Node && (n = ((ResourceNode)TargetModel).FindChild("Textures", false)) != null)
                    foreach (MDL0TextureNode tref in n.Children)
                        lstTextures.Items.Add(tref, tref.Enabled);
            }
            
            lstTextures.EndUpdate();

            //Reselect the animation
            //lstTextures.Focus();
            for (int i = 0; i < lstTextures.Items.Count; i++)
                if (lstTextures.Items[i].ToString() == Name)
                {
                    lstTextures.SetSelected(i, true);
                    break;
                }

            _mainWindow.Updating = false;
        }

        public void Reset()
        {
            lstObjects.BeginUpdate();
            lstObjects.Items.Clear();
            lstTextures.BeginUpdate();
            lstTextures.Items.Clear();

            _selectedObject = null;

            chkAllObj.CheckState = CheckState.Checked;
            chkAllTextures.CheckState = CheckState.Checked;

			pnlAnims.Enabled = pnlTextures.Enabled = chkSyncVis.Enabled = (TargetCollision == null);

			if (TargetCollision != null)
				foreach (CollisionObject obj in TargetCollision._objects)
					lstObjects.Items.Add(obj, obj._render);
            else if (TargetModel != null)
            {
                UpdateAnimations(TargetAnimType);
                if (TargetModel is MDL0Node)
                {
                    MDL0Node model = TargetModel as MDL0Node;

                    if (model._objList != null)
                        foreach (MDL0ObjectNode poly in model._objList)
                            lstObjects.Items.Add(poly, poly.IsRendering);

                    if (model._texList != null)
                        foreach (MDL0TextureNode tref in model._texList)
                            lstTextures.Items.Add(tref, tref.Enabled);
                }
            }

            lstTextures.EndUpdate();
            lstObjects.EndUpdate();

            //VIS0Indices.Clear(); int i = 0;
            //foreach (MDL0PolygonNode p in lstObjects.Items)
            //{
            //    if (p._bone != null && p._bone.BoneIndex != 0)
            //        if (VIS0Indices.ContainsKey(p._bone.Name))
            //            if (!VIS0Indices[p._bone.Name].Contains(i))
            //                VIS0Indices[p._bone.Name].Add(i); else { }
            //        else VIS0Indices.Add(p._bone.Name, new List<int> { i });
            //    i++;
            //}
        }

        //private void WrapBone(MDL0BoneNode bone)
        //{
        //    lstBones.Items.Add(bone, bone._render);
        //    foreach (MDL0BoneNode b in bone.Children)
        //        WrapBone(b);
        //}

        private void spltAnimObj_Dragged(object sender, SplitterEventArgs e)
        {
            if (e.Y == 0)
                return;

            int texY = pnlTextures.Location.Y;
            int objY = pnlObjects.Location.Y;

            int height = -1;
            if (objY + btnObjects.Height + e.Y >= texY - 6)
            {
                int difference = (objY + btnObjects.Height + e.Y) - (texY - 6);
                if (texY - 6 - e.Y <= objY + btnObjects.Height)
                    if (e.Y > 0) //Only want to push the texture panel down
                    {
                        height = pnlTextures.Height;
                        pnlTextures.Height -= difference;
                    }
            }

            if (height != pnlTextures.Height)
                pnlAnims.Height += e.Y;
        }

        private void spltObjTex_Dragged(object sender, SplitterEventArgs e)
        {
            if (e.Y == 0)
                return;

            int texY = pnlTextures.Location.Y;
            int objY = pnlObjects.Location.Y;

            int height = -1;
            if (texY - 6 + e.Y <= objY + btnObjects.Height)
            {
                int difference = (objY + btnObjects.Height) - (texY - 6 + e.Y);
                if (objY + btnObjects.Height - e.Y >= texY - 6)
                    if (e.Y < 0) //Only want to push the anims panel up
                    {
                        height = pnlAnims.Height;
                        pnlAnims.Height -= difference;
                    }
            }

            if (height != pnlAnims.Height)
                pnlTextures.Height -= e.Y;
        }

        private void lstPolygons_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((_selectedObject = lstObjects.SelectedItem as IObject) != null)
            {
                if (lstDrawCalls.Visible = spltDrawCalls.Visible =
                    _selectedObject is MDL0ObjectNode)
                {
                    MDL0ObjectNode o = _selectedObject as MDL0ObjectNode;

                    _updating = true;

                    lstDrawCalls.DataSource = o._drawCalls;
                    lstDrawCalls.SelectedIndex = o._drawCalls.Count > 0 ? 0 : -1;
                    for (int i = 0; i < lstDrawCalls.Items.Count; i++)
                        lstDrawCalls.SetItemChecked(i, o._drawCalls[i]._render);

                    _updating = false;
                }
            }
            else
                lstDrawCalls.Visible = spltDrawCalls.Visible = false;

            _mainWindow.SelectedPolygonChanged();
            overObjPnl.Invalidate();
            overTexPnl.Invalidate();
        }

        public bool _pat0Updating = false; 
        
        public int _polyIndex = -1;
        public int _boneIndex = -1;
        public int _texIndex = -1;

        private void lstPolygons_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updating)
                return;

            MDL0ObjectNode poly = lstObjects.Items[e.Index] as MDL0ObjectNode;

            if (poly != null)
            {
                bool matches = poly.IsRendering == (e.NewValue == CheckState.Checked);

                if (!matches)
                    poly.IsRendering = e.NewValue == CheckState.Checked;

                if (poly == SelectedObject)
                {
                    _updating = true;
                    for (int i = 0; i < lstDrawCalls.Items.Count; i++)
                        if (i < poly._drawCalls.Count)
                            lstDrawCalls.SetItemChecked(i, poly._drawCalls[i]._render);
                    _updating = false;
                }

                if (!matches && chkSyncVis.Checked)
                    ChecksChanged(poly);
            }

            CollisionObject obj = lstObjects.Items[e.Index] as CollisionObject;
            if (obj != null)
                obj._render = e.NewValue == CheckState.Checked;

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        public void ChecksChanged(MDL0ObjectNode poly)
        {
            bool temp = false;
            if (!_mainWindow.VIS0Updating)
            {
                _mainWindow.VIS0Updating = true;
                temp = true;
            }

            foreach (DrawCall d in poly._drawCalls)
                if (_mainWindow.VIS0Indices.ContainsKey(d.VisibilityBone))
                {
                    var objects = _mainWindow.VIS0Indices[d.VisibilityBone];
                    foreach (var i in objects)
                    {
                        if (i.Key < 0 || i.Key >= lstObjects.Items.Count || i.Value == null)
                            continue;

                        MDL0ObjectNode o = (MDL0ObjectNode)lstObjects.Items[i.Key];
                        foreach (int call in i.Value)
                        {
                            DrawCall other = o._drawCalls[call];
                            if (other._render != d._render && other != d)
                                other._render = d._render;
                        }
                        UpdateObjectCheckState(i.Key);
                    }
                }

            if (temp)
            {
                _mainWindow.VIS0Updating = false;
                temp = false;
            }

            if (!_mainWindow.VIS0Updating)
            {
                _polyIndex = poly.Index;

                if (chkSyncVis.Checked)
                    for (int i = 0; i < poly._drawCalls.Count; i++)
                        _mainWindow.UpdateVis0(_polyIndex, i, poly._drawCalls[i]._render);
            }
        }

        private void lstTextures_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_selectedTexture != null)
                _selectedTexture.Selected = false;

            if ((_selectedTexture = lstTextures.SelectedItem as MDL0TextureNode) != null)
            {
                _selectedTexture.Selected = true;

                overObjPnl.Invalidate();
                overTexPnl.Invalidate();

                if (_mainWindow.SyncTexturesToObjectList)
                    _selectedTexture.ObjOnly = true;

                if (_selectedObject is MDL0ObjectNode && ((MDL0ObjectNode)_selectedObject)._drawCalls[0].MaterialNode != null)
                    TargetTexRef = _selectedObject != null ? ((MDL0ObjectNode)_selectedObject)._drawCalls[0].MaterialNode.FindChild(_selectedTexture.Name, true) as MDL0MaterialRefNode : null;
            }
            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void chkAllPoly_CheckStateChanged(object sender, EventArgs e)
        {
            if (lstObjects.Items.Count == 0)
                return;

            //_updating = true;

            lstObjects.BeginUpdate();
            for (int i = 0; i < lstObjects.Items.Count; i++)
                lstObjects.SetItemCheckState(i, chkAllObj.CheckState);
            lstObjects.EndUpdate();

            //_updating = false;

            _mainWindow.ModelPanel.Invalidate();
        }

        private void btnObjects_Click(object sender, EventArgs e)
        {
            if (lstObjects.Visible)
            {
                pnlObjects.Tag = pnlObjects.Height;
                pnlObjects.Height = btnObjects.Height;
                lstObjects.Visible = chkSyncVis.Visible = chkAllObj.Visible = overObjPnl.Visible = false;
            }
            else
            {
                pnlObjects.Height = (int)pnlObjects.Tag;
                pnlAnims.Dock = DockStyle.Top;
                pnlTextures.Dock = DockStyle.Bottom;
                pnlObjects.Dock = DockStyle.Fill;
                lstObjects.Visible = chkSyncVis.Visible = chkAllObj.Visible = overObjPnl.Visible = true;
            }
        }

        private void btnTextures_Click(object sender, EventArgs e)
        {
            if (lstTextures.Visible)
            {
                pnlTextures.Tag = pnlTextures.Height;
                pnlTextures.Height = btnTextures.Height;
                lstTextures.Visible = chkAllTextures.Visible = spltObjTex.Visible = overTexPnl.Visible = false;
            }
            else
            {
                pnlTextures.Height = (int)pnlTextures.Tag;
                lstTextures.Visible = chkAllTextures.Visible = spltObjTex.Visible = overTexPnl.Visible = true;
            }
        }

        private void btnAnims_Click(object sender, EventArgs e)
        {
            if (listAnims.Visible)
            {
                pnlAnims.Tag = pnlAnims.Height;
                pnlAnims.Height = btnAnims.Height;
                listAnims.Visible = fileType.Visible = spltAnimObj.Visible = false;
            }
            else
            {
                pnlAnims.Height = (int)pnlAnims.Tag;
                listAnims.Visible = fileType.Visible = spltAnimObj.Visible = true;
            }
        }

        private void lstTextures_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0TextureNode tref = lstTextures.Items[e.Index] as MDL0TextureNode;

            tref.Enabled = e.NewValue == CheckState.Checked;

            if (!_updating)
                _mainWindow.ModelPanel.Invalidate();
        }

        private void chkAllTextures_CheckStateChanged(object sender, EventArgs e)
        {
            _updating = true;

            lstTextures.BeginUpdate();
            for (int i = 0; i < lstTextures.Items.Count; i++)
                lstTextures.SetItemCheckState(i, chkAllTextures.CheckState);
            lstTextures.EndUpdate();

            _updating = false;

            _mainWindow.ModelPanel.Invalidate();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTexture == null)
                return;

            new GLTextureWindow().Show(this, _selectedTexture.Texture);
        }

        private void replaceTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = lstTextures.SelectedIndex;
            if ((_selectedTexture != null) && (_selectedTexture.Source is TEX0Node))
            {
                TEX0Node node = _selectedTexture.Source as TEX0Node;
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                    if (dlg.ShowDialog(this, node) == DialogResult.OK)
                    {
                        _updating = true;
                        _selectedTexture.Reload();
                        lstTextures.SetItemCheckState(index, CheckState.Checked);
                        lstTextures.SetSelected(index, false);
                        _updating = false;

                        _mainWindow.ModelPanel.Invalidate();
                    }
            }
        }

        private void ctxTextures_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedTexture == null)
                e.Cancel = true;
            else
            {
                if (_selectedTexture.Source is TEX0Node)
                {
                    viewToolStripMenuItem.Enabled = true;
                    replaceTextureToolStripMenuItem.Enabled = true;
                    exportTextureToolStripMenuItem.Enabled = true;
                    sourceToolStripMenuItem.Text = String.Format("Source: {0}", Path.GetFileName(((ResourceNode)_selectedTexture.Source).RootNode._origPath));
                }
                else if (_selectedTexture.Source is string)
                {
                    viewToolStripMenuItem.Enabled = true;
                    replaceTextureToolStripMenuItem.Enabled = false;
                    exportTextureToolStripMenuItem.Enabled = false;
                    sourceToolStripMenuItem.Text = String.Format("Source: {0}", (string)_selectedTexture.Source);
                }
                else
                {
                    viewToolStripMenuItem.Enabled = false;
                    replaceTextureToolStripMenuItem.Enabled = false;
                    exportTextureToolStripMenuItem.Enabled = false;
                    sourceToolStripMenuItem.Text = "Source: <Not Found>";
                }

                if (_selectedTexture.Texture != null)
                    sizeToolStripMenuItem.Text = String.Format("Size: {0} x {1}", _selectedTexture.Texture.Width, _selectedTexture.Texture.Height);
                else
                    sizeToolStripMenuItem.Text = "Size: <Not Found>";
            }
        }

        private void lstTextures_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstTextures.IndexFromPoint(e.X, e.Y);
            if (lstTextures.SelectedIndex != index)
                lstTextures.SelectedIndex = index;

            if (e.Button == MouseButtons.Right)
            {
                if (_selectedTexture != null)
                    lstTextures.ContextMenuStrip = ctxTextures;
                else
                    lstTextures.ContextMenuStrip = null;
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTexture != null)
            {
                _selectedTexture.Reload();
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void lstTextures_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                lstTextures.SelectedItem = null;
        }
        private void lstPolygons_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                lstObjects.SelectedItem = null;
        }
        private void listAnims_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                listAnims.SelectedItems.Clear();
        }
        private void exportTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((_selectedTexture != null) && (_selectedTexture.Source is TEX0Node))
            {
                TEX0Node node = _selectedTexture.Source as TEX0Node;
                using (SaveFileDialog dlgSave = new SaveFileDialog())
                {
                    dlgSave.FileName = node.Name;
                    dlgSave.Filter = FileFilters.TEX0;
                    if (dlgSave.ShowDialog(this) == DialogResult.OK)
                        node.Export(dlgSave.FileName);
                }
            }
        }

        private void renameTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
                dlg.ShowDialog(this.ParentForm, (_selectedTexture.Source as TEX0Node));
            
            _selectedTexture.Name = (_selectedTexture.Source as TEX0Node).Name;
        }

        private void listAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            _updating = true;
            if (listAnims.SelectedItems.Count > 0)
            {
                createNewToolStripMenuItem.Text = String.Format("Create New {0}0", TargetAnimType.ToString());

                NW4RAnimationNode n = listAnims.SelectedItems[0].Tag as NW4RAnimationNode;
                if (n != null)
                    chkLoop.Checked = n.Loop;

                _mainWindow.TargetAnimation = n;
            }
            else
                _mainWindow.TargetAnimation = null;

            //if (_selectedAnim != null)
            //    portToolStripMenuItem.Enabled = !_selectedAnim.IsPorted;

            _updating = false;
        }

        private void fileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _mainWindow.TargetAnimType = TargetAnimType;
            UpdateAnimations(TargetAnimType);
        }

        #region Animation Context Menu
        private void ctxAnim_Opening(object sender, CancelEventArgs e)
        {
            if (_mainWindow.TargetAnimation == null)
                e.Cancel = true;
            else
            {
                sourceToolStripMenuItem.Text = String.Format("Source: {0}", Path.GetFileName(_mainWindow.TargetAnimation.RootNode._origPath));
                if (matrixModeToolStripMenuItem.Visible = _mainWindow.TargetAnimation is SRT0Node)
                {
                    SRT0Node node = (SRT0Node)_mainWindow.TargetAnimation;
                    chkMtxMax.Checked = node.MatrixMode == BrawlLib.SSBBTypes.TexMatrixMode.Matrix3dsMax;
                    chkMtxXSI.Checked = node.MatrixMode == BrawlLib.SSBBTypes.TexMatrixMode.MatrixXSI;
                    chkMtxMaya.Checked = node.MatrixMode == BrawlLib.SSBBTypes.TexMatrixMode.MatrixMaya;
                }
            }
        }

        private SaveFileDialog dlgSave = new SaveFileDialog();
        private OpenFileDialog dlgOpen = new OpenFileDialog();
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BRESEntryNode node;
            if ((node = _mainWindow.TargetAnimation as BRESEntryNode) == null)
                return;

            dlgSave.FileName = node.Name;
            switch (TargetAnimType)
            {
                case NW4RAnimType.CHR: dlgSave.Filter = FileFilters.CHR0Export; break;
                case NW4RAnimType.SRT: dlgSave.Filter = FileFilters.SRT0; break;
                case NW4RAnimType.SHP: dlgSave.Filter = FileFilters.SHP0; break;
                case NW4RAnimType.PAT: dlgSave.Filter = FileFilters.PAT0; break;
                case NW4RAnimType.VIS: dlgSave.Filter = FileFilters.VIS0; break;
                case NW4RAnimType.SCN: dlgSave.Filter = FileFilters.SCN0; break;
                case NW4RAnimType.CLR: dlgSave.Filter = FileFilters.CLR0; break;
            }
            if (dlgSave.ShowDialog() == DialogResult.OK)
                node.Export(dlgSave.FileName);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BRESEntryNode node;
            if ((node = _mainWindow.TargetAnimation as BRESEntryNode) == null)
                return;

            switch (TargetAnimType)
            {
                case NW4RAnimType.CHR: dlgOpen.Filter = FileFilters.CHR0Import; break;
                case NW4RAnimType.SRT: dlgOpen.Filter = FileFilters.SRT0; break;
                case NW4RAnimType.SHP: dlgOpen.Filter = FileFilters.SHP0; break;
                case NW4RAnimType.PAT: dlgOpen.Filter = FileFilters.PAT0; break;
                case NW4RAnimType.VIS: dlgOpen.Filter = FileFilters.VIS0; break;
                case NW4RAnimType.SCN: dlgOpen.Filter = FileFilters.SCN0; break;
                case NW4RAnimType.CLR: dlgOpen.Filter = FileFilters.CLR0; break;
            }

            if (dlgOpen.ShowDialog() == DialogResult.OK)
                node.Replace(dlgOpen.FileName);
        }

        private unsafe void portToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetAnimType != 0 || SelectedCHR0 == null || !(TargetModel is MDL0Node))
                return;

            SelectedCHR0.Port(TargetModel as MDL0Node);

            _mainWindow.UpdateModel();
        }
        #endregion

        private void listAnims_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (_mainWindow.TargetAnimation != null)
                    listAnims.ContextMenuStrip = ctxAnim;
                else
                    listAnims.ContextMenuStrip = ctxAnimList;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
                dlg.ShowDialog(this.ParentForm, _mainWindow.TargetAnimation);
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r;
            if ((r = _mainWindow.TargetAnimation) != null)
                switch (TargetAnimType)
                {
                    case NW4RAnimType.CHR: ((BRRESNode)r.Parent.Parent).CreateResource<CHR0Node>("NewCHR"); break;
                    case NW4RAnimType.SRT: ((BRRESNode)r.Parent.Parent).CreateResource<SRT0Node>("NewSRT"); break;
                    case NW4RAnimType.SHP: ((BRRESNode)r.Parent.Parent).CreateResource<SHP0Node>("NewSHP"); break;
                    case NW4RAnimType.PAT: ((BRRESNode)r.Parent.Parent).CreateResource<PAT0Node>("NewPAT"); break;
                    case NW4RAnimType.VIS: ((BRRESNode)r.Parent.Parent).CreateResource<VIS0Node>("NewVIS"); break;
                    case NW4RAnimType.SCN: ((BRRESNode)r.Parent.Parent).CreateResource<SCN0Node>("NewSCN"); break;
                    case NW4RAnimType.CLR: ((BRRESNode)r.Parent.Parent).CreateResource<CLR0Node>("NewCLR"); break;
                }
            UpdateAnimations();
            listAnims.Items[listAnims.Items.Count - 1].Selected = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _mainWindow.btnLoadAnimations_Click(this, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _mainWindow.btnSave_Click(this, null);
        }

        private void overObjPnl_Paint(object sender, PaintEventArgs e)
        {
            if (_srt0Selection == null && _pat0Selection == null)
                return;

            Graphics g = e.Graphics;
            for (int i = 0; i < lstObjects.Items.Count; i++)
            {
                MDL0ObjectNode poly = lstObjects.Items[i] as MDL0ObjectNode;
                if (poly != null)
                    foreach (DrawCall c in poly._drawCalls)
                        if (c.MaterialNode != null)
                        {
                            if (_srt0Selection != null)
                            {
                                if (_srt0Selection.FindChildByType(c.MaterialNode.Name, false, ResourceType.SRT0Entry) != null)
                                {
                                    Rectangle r = lstObjects.GetItemRectangle(i);
                                    g.DrawRectangle(Pens.Black, r);
                                }
                            }
                            else if (_pat0Selection != null)
                            {
                                if (_pat0Selection.FindChildByType(c.MaterialNode.Name, false, ResourceType.PAT0Entry) != null)
                                {
                                    Rectangle r = lstObjects.GetItemRectangle(i);
                                    g.DrawRectangle(Pens.Black, r);

                                }
                            }
                        }
            }
        }

        private void overTexPnl_Paint(object sender, PaintEventArgs e)
        {
            if (_srt0Selection == null && _pat0Selection == null)
                return;

            Graphics g = e.Graphics;
            ResourceNode rn = null;

            if (_selectedObject != null && _selectedObject is MDL0ObjectNode)
                for (int i = 0; i < lstTextures.Items.Count; i++)
                {
                    MDL0ObjectNode obj = _selectedObject as MDL0ObjectNode;
                    MDL0TextureNode tex = lstTextures.Items[i] as MDL0TextureNode;
                    foreach (DrawCall c in obj._drawCalls)
                        if (c.MaterialNode != null)
                            if ((rn = c.MaterialNode.FindChild(tex.Name, true)) != null)
                                if (_srt0Selection != null)
                                {
                                    if (_srt0Selection.FindChildByType(c.MaterialNode.Name + "/Texture" + rn.Index, false, ResourceType.SRT0Texture) != null)
                                    {
                                        Rectangle r = lstTextures.GetItemRectangle(i);
                                        g.DrawRectangle(Pens.Black, r);
                                    }
                                }
                                else if (_pat0Selection != null)
                                {
                                    if (_pat0Selection.FindChildByType(c.MaterialNode.Name + "/Texture" + rn.Index, false, ResourceType.PAT0Texture) != null)
                                    {
                                        Rectangle r = lstTextures.GetItemRectangle(i);
                                        g.DrawRectangle(Pens.Black, r);
                                    }
                                }
                }
        }

        private void lstObjects_Leave(object sender, EventArgs e)
        {
            overObjPnl.Invalidate();
            overTexPnl.Invalidate();
        }

        private void lstTextures_Leave(object sender, EventArgs e)
        {
            overObjPnl.Invalidate();
            overTexPnl.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.TargetAnimation.Remove();
            _mainWindow.TargetAnimation = null;
            UpdateAnimations();
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            NW4RAnimationNode n = _mainWindow.TargetAnimation;
            if (n != null)
            {
                if (ModelEditControl.Interpolated.Contains(n.GetType()))
                {
                    _mainWindow.Updating = true;
                    bool loopPrev = n.Loop;
                    if ((n.Loop = chkLoop.Checked) != loopPrev)
                        if (n.Loop)
                        {
                            _mainWindow.PlaybackPanel.numTotalFrames.Value += 1;
                            _mainWindow.PlaybackPanel.numTotalFrames.Minimum = 2;
                            _mainWindow.PlaybackPanel.numFrameIndex.Maximum += 1;
                        }
                        else
                        {
                            _mainWindow.PlaybackPanel.numTotalFrames.Value -= 1;
                            _mainWindow.PlaybackPanel.numTotalFrames.Minimum = 1;
                            _mainWindow.PlaybackPanel.numFrameIndex.Maximum -= 1;
                        }
                    _mainWindow.Updating = false;
                }
                else
                    n.Loop = chkLoop.Checked;
            }
        }

        private void inModelsBRRESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r = TargetModel as ResourceNode;
            if (r == null || r.Parent == null || r.Parent.Parent == null || !(r.Parent.Parent is BRRESNode))
                return;

            AddAnimation((BRRESNode)r.Parent.Parent);
        }

        private void AddAnimation(BRRESNode target)
        {
            Type t = ModelEditorBase.AnimTypeList[(int)TargetAnimType];
            var method = typeof(BRRESNode).GetMethod("CreateResource");
            var generic = method.MakeGenericMethod(t);
            generic.Invoke(target, new object[] { "New" + TargetAnimType.ToString() });
            UpdateAnimations();
            listAnims.Items[listAnims.Items.Count - 1].Selected = true;
        }

        private void inExternalFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (_mainWindow.ExternalAnimationsNode == null)
            //    return;

            //ResourceNode r = _mainWindow.ExternalAnimationsNode;
            //BRRESNode target = null;

            //if (target != null)
            //    AddAnimation(target);
        }

        private void ctxAnimList_Opening(object sender, CancelEventArgs e)
        {
            ResourceNode r = TargetModel as ResourceNode;
            bool targetBRRES = !(r == null || r.Parent == null || r.Parent.Parent == null || !(r.Parent.Parent is BRRESNode));

            if (!targetBRRES)
                e.Cancel = true;

            //inModelsBRRESToolStripMenuItem.Enabled = targetBRRES;
            //inExternalFileToolStripMenuItem.Enabled = _mainWindow.ExternalAnimationsNode != null;
        }

        private void chkMtxMaya_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            _updating = true;

            if (chkMtxMaya.Checked)
            {
                SRT0Node node = (SRT0Node)_mainWindow.TargetAnimation;
                node.MatrixMode = BrawlLib.SSBBTypes.TexMatrixMode.MatrixMaya;
                chkMtxMax.Checked = chkMtxXSI.Checked = false;
            }
            else
                chkMtxMaya.Checked = true;

            _updating = false;
        }

        private void chkMtxXSI_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            _updating = true;

            if (chkMtxXSI.Checked)
            {
                SRT0Node node = (SRT0Node)_mainWindow.TargetAnimation;
                node.MatrixMode = BrawlLib.SSBBTypes.TexMatrixMode.MatrixXSI;
                chkMtxMax.Checked = chkMtxMaya.Checked = false;
            }
            else
                chkMtxXSI.Checked = true;

            _updating = false;
        }

        private void chkMtxMax_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            _updating = true;

            if (chkMtxMax.Checked)
            {
                SRT0Node node = (SRT0Node)_mainWindow.TargetAnimation;
                node.MatrixMode = BrawlLib.SSBBTypes.TexMatrixMode.Matrix3dsMax;
                chkMtxXSI.Checked = chkMtxMaya.Checked = false;
            }
            else
                chkMtxMax.Checked = true;

            _updating = false;
        }

        private void txtSearchAnim_Enter(object sender, EventArgs e)
        {
            if (txtSearchAnim.ForeColor == Color.Gray)
            {
                txtSearchAnim.Text = "";
                txtSearchAnim.Font = new Font(txtSearchAnim.Font, Drawing.FontStyle.Regular);
                txtSearchAnim.ForeColor = Color.Black;
            }
        }

        private void txtSearchAnim_Leave(object sender, EventArgs e)
        {
            if (txtSearchAnim.Text == String.Empty)
            {
                txtSearchAnim.Font = new Font(txtSearchAnim.Font, Drawing.FontStyle.Italic);
                txtSearchAnim.ForeColor = Color.Gray;
                txtSearchAnim.Text = "Search for an animation...";
            }
        }

        private void txtSearchAnim_TextChanged(object sender, EventArgs e)
        {
            UpdateAnimations();
        }

        private void chkContains_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAnimations();
        }

        private void lstDrawCalls_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updating)
                return;

            DrawCall c = lstDrawCalls.SelectedItem as DrawCall;
            if (c != null)
            {
                c._render = !c._render;
                UpdateObjectCheckState(lstObjects.SelectedIndex);
                if (chkSyncVis.Checked)
                    ChecksChanged(c._parentObject);
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        public void UpdateObjectCheckState(int i)
        {
            if (i < 0 || i > lstObjects.Items.Count)
                return;

            MDL0ObjectNode o = lstObjects.Items[i] as MDL0ObjectNode;
            if (o == null)
                return;

            CheckState s = CheckState.Indeterminate;
            bool someRendering = false, someNotRendering = false;
            foreach (DrawCall x in o._drawCalls)
            {
                if (x._render)
                    someRendering = true;
                else
                    someNotRendering = true;

                if (someNotRendering && someRendering)
                    break;
            }
            if (someRendering && !someNotRendering)
                s = CheckState.Checked;
            else if (!someRendering && someNotRendering)
                s = CheckState.Unchecked;

            _updating = true;
            lstObjects.SetItemCheckState(i, s);
            _updating = false;
        }

        private void lstDrawCalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawCall c = lstDrawCalls.SelectedItem as DrawCall;
            if (c != null && c.MaterialNode != null)
                TargetTexRef = _selectedTexture != null ? c.MaterialNode.FindChild(_selectedTexture.Name, true) as MDL0MaterialRefNode : null;
        }

        private void lstDrawCalls_DoubleClick(object sender, EventArgs e)
        {
            
        }

        public void SetRenderState(int objKey, int i, bool render, MDL0ObjectNode obj)
        {
            if (lstObjects.SelectedIndex == objKey)
                lstDrawCalls.SetItemChecked(i, render);
            else
            {
                obj._drawCalls[i]._render = render;
                UpdateObjectCheckState(objKey);
            }
        }
    }
}

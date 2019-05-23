using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using BrawlLib;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Ikarus.ModelViewer;
using Ikarus.MovesetFile;

namespace Ikarus.UI
{
    public class ListsPanel : UserControl
    {
        #region Designer

        private Panel pnlLists;
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
        private ContextMenuStrip ctxAnim;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem portToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem createNewToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ListBox ActionsList;
        public ListBox SubActionsList;
        private AttributeGrid attributeGridMain;
        private AttributeGrid attributeGridSSE;
        public CheckedListBox lstHurtboxes;
        private CheckBox chkAllHurtboxes;
        private Panel pnlHurtboxes;
        private Panel pnlTop;
        public ComboBox movesetEditor;
        private Panel listGroupPanel;
        private ListBox CommonActionsList;
        private ListBox EntryOverridesList;
        private ListBox ExitOverridesList;
        private ListBox FlashOverlayList;
        private ListBox ScreenTintList;
        private ListBox ArticleList;
        private ListBox CommonSubRoutinesList;
        private ListBox lstModules;
        private ListBox SubRoutinesList;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlLists = new System.Windows.Forms.Panel();
            this.listGroupPanel = new System.Windows.Forms.Panel();
            this.pnlHurtboxes = new System.Windows.Forms.Panel();
            this.lstHurtboxes = new System.Windows.Forms.CheckedListBox();
            this.chkAllHurtboxes = new System.Windows.Forms.CheckBox();
            this.SubActionsList = new System.Windows.Forms.ListBox();
            this.SubRoutinesList = new System.Windows.Forms.ListBox();
            this.CommonSubRoutinesList = new System.Windows.Forms.ListBox();
            this.ActionsList = new System.Windows.Forms.ListBox();
            this.CommonActionsList = new System.Windows.Forms.ListBox();
            this.FlashOverlayList = new System.Windows.Forms.ListBox();
            this.ScreenTintList = new System.Windows.Forms.ListBox();
            this.EntryOverridesList = new System.Windows.Forms.ListBox();
            this.ExitOverridesList = new System.Windows.Forms.ListBox();
            this.ArticleList = new System.Windows.Forms.ListBox();
            this.attributeGridSSE = new System.Windows.Forms.AttributeGrid();
            this.attributeGridMain = new System.Windows.Forms.AttributeGrid();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.movesetEditor = new System.Windows.Forms.ComboBox();
            this.ctxTextures = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTextureTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxAnim = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstModules = new System.Windows.Forms.ListBox();
            this.pnlLists.SuspendLayout();
            this.listGroupPanel.SuspendLayout();
            this.pnlHurtboxes.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.ctxAnim.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLists
            // 
            this.pnlLists.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLists.Controls.Add(this.listGroupPanel);
            this.pnlLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLists.Location = new System.Drawing.Point(0, 0);
            this.pnlLists.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlLists.Name = "pnlLists";
            this.pnlLists.Size = new System.Drawing.Size(376, 326);
            this.pnlLists.TabIndex = 2;
            // 
            // listGroupPanel
            // 
            this.listGroupPanel.Controls.Add(this.pnlHurtboxes);
            this.listGroupPanel.Controls.Add(this.SubActionsList);
            this.listGroupPanel.Controls.Add(this.SubRoutinesList);
            this.listGroupPanel.Controls.Add(this.CommonSubRoutinesList);
            this.listGroupPanel.Controls.Add(this.ActionsList);
            this.listGroupPanel.Controls.Add(this.CommonActionsList);
            this.listGroupPanel.Controls.Add(this.FlashOverlayList);
            this.listGroupPanel.Controls.Add(this.ScreenTintList);
            this.listGroupPanel.Controls.Add(this.EntryOverridesList);
            this.listGroupPanel.Controls.Add(this.ExitOverridesList);
            this.listGroupPanel.Controls.Add(this.ArticleList);
            this.listGroupPanel.Controls.Add(this.attributeGridSSE);
            this.listGroupPanel.Controls.Add(this.attributeGridMain);
            this.listGroupPanel.Controls.Add(this.lstModules);
            this.listGroupPanel.Controls.Add(this.pnlTop);
            this.listGroupPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGroupPanel.Location = new System.Drawing.Point(0, 0);
            this.listGroupPanel.Name = "listGroupPanel";
            this.listGroupPanel.Size = new System.Drawing.Size(374, 324);
            this.listGroupPanel.TabIndex = 2;
            // 
            // pnlHurtboxes
            // 
            this.pnlHurtboxes.Controls.Add(this.lstHurtboxes);
            this.pnlHurtboxes.Controls.Add(this.chkAllHurtboxes);
            this.pnlHurtboxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHurtboxes.Location = new System.Drawing.Point(0, 21);
            this.pnlHurtboxes.Name = "pnlHurtboxes";
            this.pnlHurtboxes.Size = new System.Drawing.Size(374, 303);
            this.pnlHurtboxes.TabIndex = 2;
            this.pnlHurtboxes.Visible = false;
            // 
            // lstHurtboxes
            // 
            this.lstHurtboxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstHurtboxes.IntegralHeight = false;
            this.lstHurtboxes.Location = new System.Drawing.Point(0, 17);
            this.lstHurtboxes.Name = "lstHurtboxes";
            this.lstHurtboxes.Size = new System.Drawing.Size(374, 286);
            this.lstHurtboxes.TabIndex = 1;
            this.lstHurtboxes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstHurtboxes_ItemCheck);
            this.lstHurtboxes.SelectedIndexChanged += new System.EventHandler(this.lstHurtboxes_SelectedIndexChanged);
            this.lstHurtboxes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstHurtboxes_KeyDown);
            // 
            // chkAllHurtboxes
            // 
            this.chkAllHurtboxes.AutoSize = true;
            this.chkAllHurtboxes.BackColor = System.Drawing.SystemColors.Control;
            this.chkAllHurtboxes.Checked = true;
            this.chkAllHurtboxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllHurtboxes.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllHurtboxes.Location = new System.Drawing.Point(0, 0);
            this.chkAllHurtboxes.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllHurtboxes.Name = "chkAllHurtboxes";
            this.chkAllHurtboxes.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllHurtboxes.Size = new System.Drawing.Size(374, 17);
            this.chkAllHurtboxes.TabIndex = 0;
            this.chkAllHurtboxes.Text = "All";
            this.chkAllHurtboxes.UseVisualStyleBackColor = false;
            this.chkAllHurtboxes.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SubActionsList
            // 
            this.SubActionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubActionsList.FormattingEnabled = true;
            this.SubActionsList.IntegralHeight = false;
            this.SubActionsList.Location = new System.Drawing.Point(0, 21);
            this.SubActionsList.Name = "SubActionsList";
            this.SubActionsList.Size = new System.Drawing.Size(374, 303);
            this.SubActionsList.TabIndex = 1;
            this.SubActionsList.Visible = false;
            this.SubActionsList.SelectedIndexChanged += new System.EventHandler(this.SubActionsList_SelectedIndexChanged_1);
            this.SubActionsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.List_KeyDown);
            this.SubActionsList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SubActionsList_KeyPress);
            // 
            // SubRoutinesList
            // 
            this.SubRoutinesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubRoutinesList.FormattingEnabled = true;
            this.SubRoutinesList.IntegralHeight = false;
            this.SubRoutinesList.Location = new System.Drawing.Point(0, 21);
            this.SubRoutinesList.Name = "SubRoutinesList";
            this.SubRoutinesList.Size = new System.Drawing.Size(374, 303);
            this.SubRoutinesList.TabIndex = 28;
            this.SubRoutinesList.Visible = false;
            this.SubRoutinesList.SelectedIndexChanged += new System.EventHandler(this.SubRoutinesList_SelectedIndexChanged);
            // 
            // CommonSubRoutinesList
            // 
            this.CommonSubRoutinesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CommonSubRoutinesList.FormattingEnabled = true;
            this.CommonSubRoutinesList.IntegralHeight = false;
            this.CommonSubRoutinesList.Location = new System.Drawing.Point(0, 21);
            this.CommonSubRoutinesList.Name = "CommonSubRoutinesList";
            this.CommonSubRoutinesList.Size = new System.Drawing.Size(374, 303);
            this.CommonSubRoutinesList.TabIndex = 35;
            this.CommonSubRoutinesList.Visible = false;
            this.CommonSubRoutinesList.SelectedIndexChanged += new System.EventHandler(this.CommonSubRoutinesList_SelectedIndexChanged);
            // 
            // ActionsList
            // 
            this.ActionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsList.FormattingEnabled = true;
            this.ActionsList.IntegralHeight = false;
            this.ActionsList.Location = new System.Drawing.Point(0, 21);
            this.ActionsList.Name = "ActionsList";
            this.ActionsList.Size = new System.Drawing.Size(374, 303);
            this.ActionsList.TabIndex = 0;
            this.ActionsList.Visible = false;
            this.ActionsList.SelectedIndexChanged += new System.EventHandler(this.ActionsList_SelectedIndexChanged);
            // 
            // CommonActionsList
            // 
            this.CommonActionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CommonActionsList.FormattingEnabled = true;
            this.CommonActionsList.IntegralHeight = false;
            this.CommonActionsList.Location = new System.Drawing.Point(0, 21);
            this.CommonActionsList.Name = "CommonActionsList";
            this.CommonActionsList.Size = new System.Drawing.Size(374, 303);
            this.CommonActionsList.TabIndex = 29;
            this.CommonActionsList.Visible = false;
            this.CommonActionsList.SelectedIndexChanged += new System.EventHandler(this.CommonActionsList_SelectedIndexChanged);
            // 
            // FlashOverlayList
            // 
            this.FlashOverlayList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlashOverlayList.FormattingEnabled = true;
            this.FlashOverlayList.IntegralHeight = false;
            this.FlashOverlayList.Location = new System.Drawing.Point(0, 21);
            this.FlashOverlayList.Name = "FlashOverlayList";
            this.FlashOverlayList.Size = new System.Drawing.Size(374, 303);
            this.FlashOverlayList.TabIndex = 32;
            this.FlashOverlayList.Visible = false;
            this.FlashOverlayList.SelectedIndexChanged += new System.EventHandler(this.FlashOverlayList_SelectedIndexChanged);
            // 
            // ScreenTintList
            // 
            this.ScreenTintList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScreenTintList.FormattingEnabled = true;
            this.ScreenTintList.IntegralHeight = false;
            this.ScreenTintList.Location = new System.Drawing.Point(0, 21);
            this.ScreenTintList.Name = "ScreenTintList";
            this.ScreenTintList.Size = new System.Drawing.Size(374, 303);
            this.ScreenTintList.TabIndex = 33;
            this.ScreenTintList.Visible = false;
            this.ScreenTintList.SelectedIndexChanged += new System.EventHandler(this.ScreenTintList_SelectedIndexChanged);
            // 
            // EntryOverridesList
            // 
            this.EntryOverridesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntryOverridesList.FormattingEnabled = true;
            this.EntryOverridesList.IntegralHeight = false;
            this.EntryOverridesList.Location = new System.Drawing.Point(0, 21);
            this.EntryOverridesList.Name = "EntryOverridesList";
            this.EntryOverridesList.Size = new System.Drawing.Size(374, 303);
            this.EntryOverridesList.TabIndex = 30;
            this.EntryOverridesList.Visible = false;
            this.EntryOverridesList.SelectedIndexChanged += new System.EventHandler(this.EntryOverridesList_SelectedIndexChanged);
            // 
            // ExitOverridesList
            // 
            this.ExitOverridesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExitOverridesList.FormattingEnabled = true;
            this.ExitOverridesList.IntegralHeight = false;
            this.ExitOverridesList.Location = new System.Drawing.Point(0, 21);
            this.ExitOverridesList.Name = "ExitOverridesList";
            this.ExitOverridesList.Size = new System.Drawing.Size(374, 303);
            this.ExitOverridesList.TabIndex = 31;
            this.ExitOverridesList.Visible = false;
            this.ExitOverridesList.SelectedIndexChanged += new System.EventHandler(this.ExitOverridesList_SelectedIndexChanged);
            // 
            // ArticleList
            // 
            this.ArticleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArticleList.FormattingEnabled = true;
            this.ArticleList.IntegralHeight = false;
            this.ArticleList.Location = new System.Drawing.Point(0, 21);
            this.ArticleList.Name = "ArticleList";
            this.ArticleList.Size = new System.Drawing.Size(374, 303);
            this.ArticleList.TabIndex = 34;
            this.ArticleList.Visible = false;
            this.ArticleList.SelectedIndexChanged += new System.EventHandler(this.ArticleList_SelectedIndexChanged);
            // 
            // attributeGridSSE
            // 
            this.attributeGridSSE.AttributeArray = null;
            this.attributeGridSSE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeGridSSE.Location = new System.Drawing.Point(0, 21);
            this.attributeGridSSE.Name = "attributeGridSSE";
            this.attributeGridSSE.Size = new System.Drawing.Size(374, 303);
            this.attributeGridSSE.TabIndex = 0;
            this.attributeGridSSE.Visible = false;
            // 
            // attributeGridMain
            // 
            this.attributeGridMain.AttributeArray = null;
            this.attributeGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeGridMain.Location = new System.Drawing.Point(0, 21);
            this.attributeGridMain.Margin = new System.Windows.Forms.Padding(0);
            this.attributeGridMain.Name = "attributeGridMain";
            this.attributeGridMain.Size = new System.Drawing.Size(374, 303);
            this.attributeGridMain.TabIndex = 0;
            this.attributeGridMain.Visible = false;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.movesetEditor);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(374, 21);
            this.pnlTop.TabIndex = 27;
            // 
            // movesetEditor
            // 
            this.movesetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.movesetEditor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.movesetEditor.FormattingEnabled = true;
            this.movesetEditor.Items.AddRange(new object[] {
            "Common Actions",
            "Special Actions",
            "SubActions",
            "SubRoutines",
            "Entry Overrides",
            "Exit Overrides",
            "Flash Overlays",
            "Screen Tints",
            "Brawl Attributes",
            "SSE Attributes",
            "Miscellaneous",
            "Articles",
            "Common SubRoutines",
            "Modules"});
            this.movesetEditor.Location = new System.Drawing.Point(0, 0);
            this.movesetEditor.Name = "movesetEditor";
            this.movesetEditor.Size = new System.Drawing.Size(374, 21);
            this.movesetEditor.TabIndex = 27;
            this.movesetEditor.SelectedIndexChanged += new System.EventHandler(this.movesetEditor_SelectedIndexChanged);
            // 
            // ctxTextures
            // 
            this.ctxTextures.Name = "ctxTextures";
            this.ctxTextures.Size = new System.Drawing.Size(61, 4);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Enabled = false;
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.sourceToolStripMenuItem.Text = "Source";
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.Enabled = false;
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.sizeToolStripMenuItem.Text = "Size";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // exportTextureToolStripMenuItem
            // 
            this.exportTextureToolStripMenuItem.Name = "exportTextureToolStripMenuItem";
            this.exportTextureToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // replaceTextureToolStripMenuItem
            // 
            this.replaceTextureToolStripMenuItem.Name = "replaceTextureToolStripMenuItem";
            this.replaceTextureToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // renameTextureTextureToolStripMenuItem
            // 
            this.renameTextureTextureToolStripMenuItem.Name = "renameTextureTextureToolStripMenuItem";
            this.renameTextureTextureToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // ctxAnim
            // 
            this.ctxAnim.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.portToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.createNewToolStripMenuItem});
            this.ctxAnim.Name = "ctxAnim";
            this.ctxAnim.Size = new System.Drawing.Size(195, 164);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(194, 22);
            this.toolStripMenuItem2.Text = "Source";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(194, 22);
            this.toolStripMenuItem3.Text = "Export...";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(194, 22);
            this.toolStripMenuItem4.Text = "Replace...";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.portToolStripMenuItem.Text = "Port...";
            this.portToolStripMenuItem.Click += new System.EventHandler(this.portToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // createNewToolStripMenuItem
            // 
            this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            this.createNewToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.createNewToolStripMenuItem.Text = "Create New Animation";
            this.createNewToolStripMenuItem.Click += new System.EventHandler(this.createNewToolStripMenuItem_Click);
            // 
            // lstModules
            // 
            this.lstModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstModules.FormattingEnabled = true;
            this.lstModules.Location = new System.Drawing.Point(0, 21);
            this.lstModules.Name = "lstModules";
            this.lstModules.Size = new System.Drawing.Size(374, 303);
            this.lstModules.TabIndex = 2;
            // 
            // ListsPanel
            // 
            this.Controls.Add(this.pnlLists);
            this.Name = "ListsPanel";
            this.Size = new System.Drawing.Size(376, 326);
            this.pnlLists.ResumeLayout(false);
            this.listGroupPanel.ResumeLayout(false);
            this.pnlHurtboxes.ResumeLayout(false);
            this.pnlHurtboxes.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.ctxAnim.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public bool _closing = false;

        public MainControl _mainWindow;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MainControl MainWindow
        {
            get { return _mainWindow; }
            set { _mainWindow = value; }
        }

        public bool _syncVis0 = false;
        public bool _syncPat0 = false;

        private bool _updating = false;
        private MDL0ObjectNode _targetObject;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0ObjectNode TargetObject
        {
            get { return _targetObject; }
            set { _targetObject = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0BoneNode TargetBone { get { return _mainWindow.SelectedBone as MDL0BoneNode; } set { _mainWindow.SelectedBone = value; } }
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef
        {
            get { return _mainWindow.TargetTexRef; }
            set
            {
                _mainWindow.TargetTexRef = value; 
                //if (_mainWindow.SelectedSRT0 != null && TargetTexRef != null)
                //    _mainWindow.KeyframePanel.TargetSequence = _mainWindow.SRT0Editor.TexEntry;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _mainWindow.TargetModel as MDL0Node; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get  { return _mainWindow.SelectedCHR0; }
            set  { _mainWindow.SelectedCHR0 = value; }
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
            get { return _mainWindow.TargetAnimType; }
            set { _mainWindow.TargetAnimType = value; }
        }

        //Bone Name - Attached Polygon Indices
        public Dictionary<string, Dictionary<int, List<int>>> VIS0Indices = new Dictionary<string, Dictionary<int, List<int>>>();

        public ListsPanel()
        {
            InitializeComponent();
            movesetEditor.SelectedIndex = 2;
            _animations = new SortedList<string, Dictionary<NW4RAnimType, NW4RAnimationNode>>();
            foreach (var grid in new AttributeGrid[] { attributeGridSSE, attributeGridMain }) {
                grid.AttributeArray = Manager.AttributeArray;
                grid.CellEdited += (o, e) => MainForm.UpdateModelPanel();
                grid.DictionaryChanged += (o, e) => Manager._dictionaryChanged = true;
            }
        }

        public SortedList<string, Dictionary<NW4RAnimType, NW4RAnimationNode>> _animations;

        public bool LoadAnims(ResourceNode node)
        {
            bool found = false;
            NW4RAnimType type;
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.U8:
                case ResourceType.U8Folder:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                default:
                    foreach (ResourceNode n in node.Children)
                        if (found) 
                            LoadAnims(n);
                        else 
                            found = LoadAnims(n);
                    break;

                case ResourceType.MDef:
                    return false;

                case ResourceType.CHR0: found = true; type = NW4RAnimType.CHR; goto Add;
                case ResourceType.SRT0: found = true; type = NW4RAnimType.SRT; goto Add;
                case ResourceType.SHP0: found = true; type = NW4RAnimType.SHP; goto Add;
                case ResourceType.PAT0: found = true; type = NW4RAnimType.PAT; goto Add;
                case ResourceType.VIS0: found = true; type = NW4RAnimType.VIS; goto Add;
                case ResourceType.CLR0: found = true; type = NW4RAnimType.CLR; goto Add;
            }
            return found;
        Add:
            if (!_animations.ContainsKey(node.Name))
                _animations.Add(node.Name, new Dictionary<NW4RAnimType, NW4RAnimationNode>());
            _animations[node.Name].Add(type, node as NW4RAnimationNode);
            return found;
        }

        public CheckState BRRESRelative = CheckState.Unchecked;
        public void UpdateAnimations()
        {
            _mainWindow.Updating = true;

            string name = null;
            int i = SubActionsList.SelectedIndex;
            if (RunTime.Subactions != null && i >= 0 && i < RunTime.Subactions.Count)
                name = RunTime.Subactions[i].Name;

            int frame = CurrentFrame;

            _animations.Clear();
            if (_mainWindow.ExternalAnimationsNode != null)
                LoadAnims(_mainWindow.ExternalAnimationsNode.RootNode);

            //Reselect the animation
            if (name != null)
                for (int x = 0; x < RunTime.Subactions.Count; x++)
                    if (RunTime.Subactions[x].Name == name)
                    {
                        SubActionsList.SetSelected(x, true);
                        break;
                    }

            _mainWindow.Updating = false;
            CurrentFrame = frame;

            if ((_mainWindow.GetAnimation((NW4RAnimType)TargetAnimType) == null) && (SubActionsList.SelectedItems.Count == 0))
            {
                _mainWindow.SelectedCHR0 = null;
                _mainWindow.SelectedSRT0 = null;
                _mainWindow.SelectedSHP0 = null;
                _mainWindow.SelectedPAT0 = null;
                _mainWindow.SelectedVIS0 = null;
                _mainWindow.SelectedCLR0 = null;
                _mainWindow.UpdatePropDisplay();
                _mainWindow.UpdateModel();
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        public bool _vis0Updating = false; 
        public bool _pat0Updating = false; 
        
        public int _polyIndex = -1;
        public int _boneIndex = -1;
        public int _texIndex = -1;

        public bool _syncObjTex;

        #region Animation Context Menu
        private void ctxAnim_Opening(object sender, CancelEventArgs e)
        {
            if (_mainWindow.GetAnimation(TargetAnimType) == null)
                e.Cancel = true;
            else
                sourceToolStripMenuItem.Text = String.Format("Source: {0}", Path.GetFileName(_mainWindow.GetAnimation(TargetAnimType).RootNode._origPath));
        }

        private SaveFileDialog dlgSave = new SaveFileDialog();
        private OpenFileDialog dlgOpen = new OpenFileDialog();
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BRESEntryNode node;
            if ((node = _mainWindow.GetAnimation(TargetAnimType) as BRESEntryNode) == null)
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
            if ((node = _mainWindow.GetAnimation(TargetAnimType) as BRESEntryNode) == null)
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
            if (TargetAnimType != 0 || SelectedCHR0 == null)
                return;

            SelectedCHR0.Port(TargetModel);

            _mainWindow.UpdateModel();
            _mainWindow.ModelPanel.Invalidate();
        }
        #endregion

        private void listAnims_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (_mainWindow.GetAnimation(TargetAnimType) != null)
                    SubActionsList.ContextMenuStrip = ctxAnim;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
                dlg.ShowDialog(this.ParentForm, _mainWindow.GetAnimation(TargetAnimType));
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r;
            if ((r = _mainWindow.GetAnimation(TargetAnimType)) != null)
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
            SubActionsList.SetSelected(SubActionsList.Items.Count - 1, true);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.GetAnimation(TargetAnimType).Remove();
            _mainWindow.GetFiles(NW4RAnimType.None);
            UpdateAnimations();
            _mainWindow.UpdatePropDisplay();
            _mainWindow.UpdateModel();
            _mainWindow.AnimChanged(NW4RAnimType.None);
            _mainWindow.ModelPanel.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (lstHurtboxes.Items.Count == 0)
                return;

            _updating = true;

            lstHurtboxes.BeginUpdate();
            for (int i = 0; i < lstHurtboxes.Items.Count; i++)
                lstHurtboxes.SetItemCheckState(i, chkAllHurtboxes.CheckState);
            lstHurtboxes.EndUpdate();

            _updating = false;

            _mainWindow.ModelPanel.Invalidate();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MiscHurtBox SelectedHurtbox
        {
            get { return _mainWindow._selectedHurtbox; }
            set { _mainWindow.SelectedHurtbox = value; }
        }

        private void lstHurtboxes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lstHurtboxes.SelectedItem = null;
                SelectedHurtbox = null;

                if (!_updating)
                    _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void lstHurtboxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            SelectedHurtbox = lstHurtboxes.SelectedItem as MiscHurtBox;
            _mainWindow.ModelPanel.Invalidate();
        }

        private void lstHurtboxes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updating)
                return;

            _mainWindow.ModelPanel.Invalidate();
        }

        Control _currentControl;
        private void movesetEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control newControl = null;
            lstHurtboxes.SelectedIndex = -1;
            if (_mainWindow != null)
            {
                _mainWindow.scriptPanel.Visible = true;
                _mainWindow.scriptPanel.scriptPanel.Visible = true;
                _mainWindow.scriptPanel.propertyGrid1.Visible = false;
                _mainWindow.DisableHurtboxEditor();
                _mainWindow.MovesetPanel.lblActionName.Text = "";
            }

            switch (movesetEditor.SelectedIndex)
            {
                case 0:
                    newControl = CommonActionsList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Actions;
                    }
                    break;
                case 1:
                    newControl = ActionsList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Actions;
                    }
                    break;
                case 2:
                    newControl = SubActionsList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Subactions;
                    }
                    break;
                case 3:
                    newControl = SubRoutinesList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Subroutines;
                    }
                    break;
                case 4:
                    newControl = EntryOverridesList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Subroutines;
                    }
                    break;
                case 5:
                    newControl = ExitOverridesList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Subroutines;
                    }
                    break;
                case 6:
                    newControl = FlashOverlayList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Subroutines;
                    }
                    break;
                case 7:
                    newControl = ScreenTintList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Subroutines;
                    }
                    break;
                case 8:
                    newControl = attributeGridMain;
                    if (_mainWindow != null)
                        _mainWindow.scriptPanel.Visible = false;
                    break;
                case 9:
                    newControl = attributeGridSSE;
                    if (_mainWindow != null)
                        _mainWindow.scriptPanel.Visible = false;
                    break;
                case 10:
                    newControl = pnlHurtboxes;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.Visible = false;
                        if (SelectedHurtbox != null)
                            _mainWindow.EnableHurtboxEditor();
                    }
                    break;
                case 11:
                    newControl = ArticleList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.scriptPanel.Visible = false;
                        _mainWindow.scriptPanel.propertyGrid1.Visible = true;
                    }
                    break;
                case 12:
                    newControl = CommonSubRoutinesList;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.scriptPanel.Visible = true;
                        _mainWindow.MovesetPanel.ScriptType = ScriptType.Subroutines;
                    }
                    break;
                case 13:
                    newControl = lstModules;
                    if (_mainWindow != null)
                    {
                        _mainWindow.scriptPanel.scriptPanel.Visible = false;
                    }
                    break;
            }

            if (_currentControl != null)
                _currentControl.Visible = false;

            if ((_currentControl = newControl) != null)
                _currentControl.Visible = true;
        }

        enum EditType
        {
            Scripts,
            Attributes,
            Miscellaneous,
            Articles
        }

        public void UpdateMoveset()
        {
            MovesetNode moveset = Manager.Moveset;

            if (moveset == null || moveset.Data == null)
                return;
            
            _updating = true;
            attributeGridMain.TargetNode = moveset.Data._attributes;
            attributeGridSSE.TargetNode = moveset.Data._sseAttributes;

            lstHurtboxes.DataSource = moveset.Data._misc._hurtBoxes;
            for (int i = 0; i < lstHurtboxes.Items.Count; i++)
                lstHurtboxes.SetItemChecked(i, true);

            SubActionsList.DataSource = RunTime.Subactions;
            CommonActionsList.DataSource = RunTime.CommonActions;
            ActionsList.DataSource = RunTime.Actions;
            SubRoutinesList.DataSource = RunTime.Subroutines;
            EntryOverridesList.DataSource = RunTime.EntryOverrides;
            ExitOverridesList.DataSource = RunTime.ExitOverrides;
            FlashOverlayList.DataSource = RunTime.FlashOverlays;
            ScreenTintList.DataSource = RunTime.ScreenTints;
            ArticleList.DataSource = RunTime._articles;
            CommonSubRoutinesList.DataSource = RunTime.CommonSubroutines;

            _updating = false;
        }

        private void ActionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.CurrentAction = ActionsList.SelectedItem as ActionEntry;

            _mainWindow.MaxFrame = 1;
            _mainWindow.GetFiles(NW4RAnimType.None);

            RunTime.SetFrame(-1);
        }

        private void SubRoutinesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.CurrentSubRoutine = SubRoutinesList.SelectedItem as Script;
        }

        public void SubActionsList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.CurrentSubaction = SubActionsList.SelectedItem as SubActionEntry;

            if (SubActionsList.SelectedItems.Count > 0)
            {
                string r = RunTime.CurrentSubaction.Name;
                if (_animations.ContainsKey(r))
                {
                    var x = _animations[r];
                    _mainWindow.TargetAnimation = null;
                    _mainWindow.GetFiles(NW4RAnimType.None);
                    foreach (var c in x)
                        _mainWindow.SetAnimation(c.Value);
                    _mainWindow.AnimChanged(TargetAnimType);
                    //if (_animations[r].ContainsKey(TargetAnimType))
                    //    _mainWindow.TargetAnimation = _animations[r][TargetAnimType];
                    createNewToolStripMenuItem.Text = String.Format("Create New {0}", TargetAnimType.ToString() + "0");
                }
                else
                    _mainWindow.TargetAnimation = null;
            }
            else
                _mainWindow.TargetAnimation = null;

            //_mainWindow.Updating = true;
            //RunTime.Loop = RunTime.CurrentSubaction._flags.HasFlag(AnimationFlags.Loop);
            //_mainWindow.Updating = false;
        }

        private void List_KeyDown(object sender, KeyEventArgs e)
        {
            ListBox b = sender as ListBox;
            if (e.KeyCode == Keys.Escape)
                b.SelectedItems.Clear();
            else if (e.KeyCode == Keys.Space)
            {
                RunTime.TogglePlay();
                e.SuppressKeyPress = true;
            }
        }

        private void SubActionsList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void CommonActionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.CurrentAction = CommonActionsList.SelectedItem as ActionEntry;

            _mainWindow.MaxFrame = 1;
            _mainWindow.GetFiles(NW4RAnimType.None);

            RunTime.SetFrame(-1);
        }

        private void EntryOverridesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            ActionOverrideEntry r = EntryOverridesList.SelectedItem as ActionOverrideEntry;
            if (r != null)
                RunTime.CurrentSubRoutine = r._script;
        }

        private void ExitOverridesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            ActionOverrideEntry r = ExitOverridesList.SelectedItem as ActionOverrideEntry;
            if (r != null)
                RunTime.CurrentSubRoutine = r._script;
        }

        private void FlashOverlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.CurrentSubRoutine = FlashOverlayList.SelectedItem as CommonAction;
        }

        private void ScreenTintList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.CurrentSubRoutine = ScreenTintList.SelectedItem as CommonAction;
        }

        private void ArticleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.MainWindow.scriptPanel.propertyGrid1.SelectedObject = (ArticleList.SelectedItem as ArticleInfo)._article;
        }

        private void CommonSubRoutinesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
                return;

            RunTime.CurrentSubRoutine = CommonSubRoutinesList.SelectedItem as Script;
        }
    }
}

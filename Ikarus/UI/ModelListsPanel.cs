using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using BrawlLib;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ikarus.UI
{
    public class ModelListsPanel : UserControl
    {
        #region Designer

        public CheckedListBox lstObjects;
        private CheckBox chkAllObj;
        private Button btnObjects;
        private Panel pnlTextures;
        private CheckedListBox lstTextures;
        private CheckBox chkAllTextures;
        private Button btnTextures;
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
        private ContextMenuStrip ctxAnim;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem portToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private TransparentPanel overObjPnl;
        private TransparentPanel overTexPnl;
        private ToolStripMenuItem createNewToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private Panel panel1;
        private TransparentPanel transparentPanel1;
        private Button button1;
        private Splitter splitter1;
        private Splitter splitter2;
        private ContextMenuStrip ctxBones;
        private ToolStripMenuItem boneIndex;
        private ToolStripMenuItem renameBoneToolStripMenuItem;
        public BonesPanel bonesPanel1;
        private Panel pnlObjects;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlObjects = new System.Windows.Forms.Panel();
            this.overObjPnl = new System.Windows.Forms.TransparentPanel();
            this.lstObjects = new System.Windows.Forms.CheckedListBox();
            this.chkAllObj = new System.Windows.Forms.CheckBox();
            this.chkSyncVis = new System.Windows.Forms.CheckBox();
            this.btnObjects = new System.Windows.Forms.Button();
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
            this.overTexPnl = new System.Windows.Forms.TransparentPanel();
            this.lstTextures = new System.Windows.Forms.CheckedListBox();
            this.chkAllTextures = new System.Windows.Forms.CheckBox();
            this.btnTextures = new System.Windows.Forms.Button();
            this.ctxAnim = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.transparentPanel1 = new System.Windows.Forms.TransparentPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.ctxBones = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.boneIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.renameBoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bonesPanel1 = new System.Windows.Forms.BonesPanel();
            this.pnlObjects.SuspendLayout();
            this.ctxTextures.SuspendLayout();
            this.pnlTextures.SuspendLayout();
            this.ctxAnim.SuspendLayout();
            this.panel1.SuspendLayout();
            this.transparentPanel1.SuspendLayout();
            this.ctxBones.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlObjects
            // 
            this.pnlObjects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlObjects.Controls.Add(this.overObjPnl);
            this.pnlObjects.Controls.Add(this.lstObjects);
            this.pnlObjects.Controls.Add(this.chkAllObj);
            this.pnlObjects.Controls.Add(this.chkSyncVis);
            this.pnlObjects.Controls.Add(this.btnObjects);
            this.pnlObjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlObjects.Location = new System.Drawing.Point(0, 149);
            this.pnlObjects.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlObjects.Name = "pnlObjects";
            this.pnlObjects.Size = new System.Drawing.Size(156, 162);
            this.pnlObjects.TabIndex = 0;
            // 
            // overObjPnl
            // 
            this.overObjPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overObjPnl.Location = new System.Drawing.Point(0, 61);
            this.overObjPnl.Name = "overObjPnl";
            this.overObjPnl.Size = new System.Drawing.Size(154, 99);
            this.overObjPnl.TabIndex = 8;
            this.overObjPnl.Paint += new System.Windows.Forms.PaintEventHandler(this.overObjPnl_Paint);
            // 
            // lstObjects
            // 
            this.lstObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstObjects.CausesValidation = false;
            this.lstObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObjects.IntegralHeight = false;
            this.lstObjects.Location = new System.Drawing.Point(0, 61);
            this.lstObjects.Margin = new System.Windows.Forms.Padding(0);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(154, 99);
            this.lstObjects.TabIndex = 4;
            this.lstObjects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstPolygons_ItemCheck);
            this.lstObjects.SelectedValueChanged += new System.EventHandler(this.lstPolygons_SelectedValueChanged);
            this.lstObjects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPolygons_KeyDown);
            this.lstObjects.Leave += new System.EventHandler(this.lstObjects_Leave);
            // 
            // chkAllObj
            // 
            this.chkAllObj.Checked = true;
            this.chkAllObj.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllObj.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllObj.Location = new System.Drawing.Point(0, 41);
            this.chkAllObj.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllObj.Name = "chkAllObj";
            this.chkAllObj.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllObj.Size = new System.Drawing.Size(154, 20);
            this.chkAllObj.TabIndex = 5;
            this.chkAllObj.Text = "All";
            this.chkAllObj.UseVisualStyleBackColor = false;
            this.chkAllObj.CheckStateChanged += new System.EventHandler(this.chkAllPoly_CheckStateChanged);
            // 
            // chkSyncVis
            // 
            this.chkSyncVis.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkSyncVis.Location = new System.Drawing.Point(0, 21);
            this.chkSyncVis.Margin = new System.Windows.Forms.Padding(0);
            this.chkSyncVis.Name = "chkSyncVis";
            this.chkSyncVis.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkSyncVis.Size = new System.Drawing.Size(154, 20);
            this.chkSyncVis.TabIndex = 7;
            this.chkSyncVis.Text = "Sync VIS0";
            this.chkSyncVis.UseVisualStyleBackColor = false;
            this.chkSyncVis.CheckedChanged += new System.EventHandler(this.chkSyncVis_CheckedChanged);
            // 
            // btnObjects
            // 
            this.btnObjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnObjects.Location = new System.Drawing.Point(0, 0);
            this.btnObjects.Name = "btnObjects";
            this.btnObjects.Size = new System.Drawing.Size(154, 21);
            this.btnObjects.TabIndex = 6;
            this.btnObjects.Text = "Objects";
            this.btnObjects.UseVisualStyleBackColor = true;
            this.btnObjects.Click += new System.EventHandler(this.btnObjects_Click);
            // 
            // ctxTextures
            // 
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
            this.ctxTextures.Size = new System.Drawing.Size(125, 164);
            this.ctxTextures.Opening += new System.ComponentModel.CancelEventHandler(this.ctxTextures_Opening);
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
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.viewToolStripMenuItem.Text = "View...";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // exportTextureToolStripMenuItem
            // 
            this.exportTextureToolStripMenuItem.Name = "exportTextureToolStripMenuItem";
            this.exportTextureToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exportTextureToolStripMenuItem.Text = "Export...";
            this.exportTextureToolStripMenuItem.Click += new System.EventHandler(this.exportTextureToolStripMenuItem_Click);
            // 
            // replaceTextureToolStripMenuItem
            // 
            this.replaceTextureToolStripMenuItem.Name = "replaceTextureToolStripMenuItem";
            this.replaceTextureToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.replaceTextureToolStripMenuItem.Text = "Replace...";
            this.replaceTextureToolStripMenuItem.Click += new System.EventHandler(this.replaceTextureToolStripMenuItem_Click);
            // 
            // renameTextureTextureToolStripMenuItem
            // 
            this.renameTextureTextureToolStripMenuItem.Name = "renameTextureTextureToolStripMenuItem";
            this.renameTextureTextureToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.renameTextureTextureToolStripMenuItem.Text = "Rename";
            this.renameTextureTextureToolStripMenuItem.Click += new System.EventHandler(this.renameTextureToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
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
            this.pnlTextures.Location = new System.Drawing.Point(0, 314);
            this.pnlTextures.MinimumSize = new System.Drawing.Size(0, 21);
            this.pnlTextures.Name = "pnlTextures";
            this.pnlTextures.Size = new System.Drawing.Size(156, 164);
            this.pnlTextures.TabIndex = 3;
            // 
            // overTexPnl
            // 
            this.overTexPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overTexPnl.Location = new System.Drawing.Point(0, 41);
            this.overTexPnl.Name = "overTexPnl";
            this.overTexPnl.Size = new System.Drawing.Size(154, 121);
            this.overTexPnl.TabIndex = 9;
            this.overTexPnl.Paint += new System.Windows.Forms.PaintEventHandler(this.overTexPnl_Paint);
            // 
            // lstTextures
            // 
            this.lstTextures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstTextures.CausesValidation = false;
            this.lstTextures.ContextMenuStrip = this.ctxTextures;
            this.lstTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTextures.IntegralHeight = false;
            this.lstTextures.Location = new System.Drawing.Point(0, 41);
            this.lstTextures.Margin = new System.Windows.Forms.Padding(0);
            this.lstTextures.Name = "lstTextures";
            this.lstTextures.Size = new System.Drawing.Size(154, 121);
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
            this.chkAllTextures.Location = new System.Drawing.Point(0, 21);
            this.chkAllTextures.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllTextures.Name = "chkAllTextures";
            this.chkAllTextures.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllTextures.Size = new System.Drawing.Size(154, 20);
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
            this.btnTextures.Size = new System.Drawing.Size(154, 21);
            this.btnTextures.TabIndex = 9;
            this.btnTextures.Text = "Textures";
            this.btnTextures.UseVisualStyleBackColor = true;
            this.btnTextures.Click += new System.EventHandler(this.btnTextures_Click);
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
            this.ctxAnim.Size = new System.Drawing.Size(125, 164);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem2.Text = "Source";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem3.Text = "Export...";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem4.Text = "Replace...";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.portToolStripMenuItem.Text = "Port...";
            this.portToolStripMenuItem.Click += new System.EventHandler(this.portToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // createNewToolStripMenuItem
            // 
            this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            this.createNewToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.transparentPanel1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(156, 146);
            this.panel1.TabIndex = 4;
            // 
            // transparentPanel1
            // 
            this.transparentPanel1.Controls.Add(this.bonesPanel1);
            this.transparentPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transparentPanel1.Location = new System.Drawing.Point(0, 21);
            this.transparentPanel1.Name = "transparentPanel1";
            this.transparentPanel1.Size = new System.Drawing.Size(154, 123);
            this.transparentPanel1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 21);
            this.button1.TabIndex = 6;
            this.button1.Text = "Bones";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 146);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(156, 3);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 311);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(156, 3);
            this.splitter2.TabIndex = 0;
            this.splitter2.TabStop = false;
            // 
            // ctxBones
            // 
            this.ctxBones.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boneIndex,
            this.renameBoneToolStripMenuItem});
            this.ctxBones.Name = "ctxBones";
            this.ctxBones.Size = new System.Drawing.Size(133, 48);
            // 
            // boneIndex
            // 
            this.boneIndex.Enabled = false;
            this.boneIndex.Name = "boneIndex";
            this.boneIndex.Size = new System.Drawing.Size(132, 22);
            this.boneIndex.Text = "Bone Index";
            // 
            // renameBoneToolStripMenuItem
            // 
            this.renameBoneToolStripMenuItem.Name = "renameBoneToolStripMenuItem";
            this.renameBoneToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.renameBoneToolStripMenuItem.Text = "Rename";
            // 
            // bonesPanel1
            // 
            this.bonesPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bonesPanel1.Location = new System.Drawing.Point(0, 0);
            this.bonesPanel1.Name = "bonesPanel1";
            this.bonesPanel1.Size = new System.Drawing.Size(154, 123);
            this.bonesPanel1.TabIndex = 0;
            // 
            // ModelListsPanel
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlObjects);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.pnlTextures);
            this.Name = "ModelListsPanel";
            this.Size = new System.Drawing.Size(156, 478);
            this.pnlObjects.ResumeLayout(false);
            this.ctxTextures.ResumeLayout(false);
            this.pnlTextures.ResumeLayout(false);
            this.ctxAnim.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.transparentPanel1.ResumeLayout(false);
            this.ctxBones.ResumeLayout(false);
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

        private MDL0ObjectNode _selectedPolygon;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0ObjectNode SelectedPolygon { get { return _selectedPolygon; } set { lstObjects.SelectedItem = value; } }

        private MDL0TextureNode _selectedTexture;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0TextureNode SelectedTexture { get { return _selectedTexture; } set { lstTextures.SelectedItem = value; } }

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
        public MDL0Node TargetModel
        {
            get { return _mainWindow.TargetModel as MDL0Node; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get { return _mainWindow.SelectedCHR0; }
            set { _mainWindow.SelectedCHR0 = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NW4RAnimType TargetAnimType
        {
            get { return _mainWindow.TargetAnimType; }
            set { _mainWindow.TargetAnimType = value; }
        }

        //Bone Name - Attached Polygon Indices
        public Dictionary<string, List<int>> VIS0Indices = new Dictionary<string, List<int>>();

        public ModelListsPanel()
        {
            InitializeComponent();
        }

        public SRT0Node _srt0Selection = null;
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
        public PAT0Node _pat0Selection = null;
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
            //if (_selectedPolygon != null && _syncObjTex)
            //    foreach (MDL0MaterialRefNode tref in _selectedPolygon.UsableMaterialNode.Children)
            //        lstTextures.Items.Add(tref.TextureNode, tref.TextureNode.Enabled);
            //else 
            if (TargetModel != null && (n = TargetModel.FindChild("Textures", false)) != null)
                foreach (MDL0TextureNode tref in n.Children)
                    lstTextures.Items.Add(tref, tref.Enabled);
            
            lstTextures.EndUpdate();

            //Reselect the animation
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
            bonesPanel1.Reset();

            lstObjects.BeginUpdate();
            lstObjects.Items.Clear();
            lstTextures.BeginUpdate();
            lstTextures.Items.Clear();
            
            _selectedPolygon = null;
            _targetObject = null;

            chkAllObj.CheckState = CheckState.Checked;
            chkAllTextures.CheckState = CheckState.Checked;

            if (TargetModel != null)
            {
                ResourceNode n;

                if ((n = TargetModel.FindChild("Objects", false)) != null)
                    foreach (MDL0ObjectNode poly in n.Children)
                        lstObjects.Items.Add(poly, poly.IsRendering);

                if ((n = TargetModel.FindChild("Textures", false)) != null)
                    foreach (MDL0TextureNode tref in n.Children)
                        lstTextures.Items.Add(tref, tref.Enabled);
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

        private void spltAnimObj_Dragged(object sender, SplitterEventArgs e)
        {
            if (e.Y == 0)
                return;

            //int TexturesTop = pnlTextures.Location.Y;

            //int ObjectsBottom = pnlObjects.Location.Y + pnlObjects.Height;
            //int ObjectsTop = pnlObjects.Location.Y;

            //int AnimsBottom = pnlLists.Location.Y + pnlLists.Height;
            //int AnimsTop = pnlLists.Location.Y;

            //int height = -1;
            //if (ObjectsBottom + e.Y >= TexturesTop - 5)
            //{
            //    int difference = (ObjectsBottom + e.Y) - (TexturesTop - 5);
            //    if (TexturesTop - 5 - e.Y <= ObjectsBottom)
            //        if (e.Y > 0) //Only want to push the texture panel down
            //        {
            //            height = pnlTextures.Height;
            //            pnlTextures.Height -= difference;
            //        }
            //}

            //if (height != pnlTextures.Height)
            pnlObjects.Height -= e.Y;
        }

        private void spltObjTex_Dragged(object sender, SplitterEventArgs e)
        {
            if (e.Y == 0)
                return;

            //int TexturesBottom = pnlTextures.Location.Y + pnlTextures.Height;
            //int TexturesTop = pnlTextures.Location.Y;

            //int ObjectsBottom = pnlObjects.Location.Y + pnlObjects.Height;
            //int ObjectsTop = pnlObjects.Location.Y;

            //int AnimsBottom = pnlLists.Location.Y + pnlLists.Height;
            //int AnimsTop = pnlLists.Location.Y;

            //int height = -1;
            //if (TexturesTop - 6 + e.Y <= ObjectsTop + btnObjects.Height)
            //{
            //    int difference = (ObjectsTop + btnObjects.Height) - (TexturesTop - 6 + e.Y);
            //    if (ObjectsTop + btnObjects.Height - e.Y >= TexturesTop - 6)
            //        if (e.Y < 0) //Only want to push the anims panel up
            //        {
            //            height = pnlLists.Height;
            //            pnlLists.Height -= difference;
            //        }
            //}

            //if (height != pnlLists.Height)
            pnlTextures.Height -= e.Y;
        }

        private void lstPolygons_SelectedValueChanged(object sender, EventArgs e)
        {
            _targetObject = _selectedPolygon = lstObjects.SelectedItem as MDL0ObjectNode;
            //TargetTexRef = _selectedPolygon != null && _selectedTexture != null ? _selectedPolygon.UsableMaterialNode.FindChild(_selectedTexture.Name, true) as MDL0MaterialRefNode : null;
            _mainWindow.SelectedPolygonChanged(this, null);
            overObjPnl.Invalidate();
            overTexPnl.Invalidate();
        }

        public bool _vis0Updating = false; 
        public bool _pat0Updating = false; 
        
        public int _polyIndex = -1;
        public int _boneIndex = -1;
        public int _texIndex = -1;

        public bool _syncObjTex;

        private void lstPolygons_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0ObjectNode poly = lstObjects.Items[e.Index] as MDL0ObjectNode;

            poly.IsRendering = e.NewValue == CheckState.Checked;

            //if (_syncVis0 && poly._visBoneNode != null)
            //{
            //    bool temp = false;
            //    if (!_vis0Updating)
            //    {
            //        _vis0Updating = true;
            //        temp = true;
            //    }

            //    if (VIS0Indices.ContainsKey(poly._visBoneNode.Name))
            //        foreach (int i in VIS0Indices[poly._visBoneNode.Name])
            //            if (((MDL0ObjectNode)lstObjects.Items[i])._render != poly._render)
            //                lstObjects.SetItemChecked(i, poly._render);

            //    if (temp)
            //    {
            //        _vis0Updating = false;
            //        temp = false;
            //    }

            //    if (!_vis0Updating)
            //    {
            //        _polyIndex = e.Index;
            //        _mainWindow.UpdateVis0(e);
            //    }
            //}

            if (!_updating) _mainWindow.ModelPanel.Invalidate();
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

                if (_syncObjTex)
                    _selectedTexture.ObjOnly = true;

                //TargetTexRef = _selectedPolygon != null ? _selectedPolygon.UsableMaterialNode.FindChild(_selectedTexture.Name, true) as MDL0MaterialRefNode : null;
            }
            if (!_updating) _mainWindow.ModelPanel.Invalidate();
        }

        private void chkAllPoly_CheckStateChanged(object sender, EventArgs e)
        {
            if (lstObjects.Items.Count == 0)
                return;

            _updating = true;

            lstObjects.BeginUpdate();
            for (int i = 0; i < lstObjects.Items.Count; i++)
                lstObjects.SetItemCheckState(i, chkAllObj.CheckState);
            lstObjects.EndUpdate();

            _updating = false;

            _mainWindow.ModelPanel.Invalidate();
        }

        private void SetDocks()
        {
            //if (listGroupPanel.Visible)
            //{
            //    pnlLists.Dock = DockStyle.Fill;
            //    pnlTextures.Dock = DockStyle.Bottom;
            //    pnlObjects.Dock = DockStyle.Bottom;
            //}
            //else
            //{
            //    pnlLists.Dock = DockStyle.Top;
            //    if (lstObjects.Visible)
            //    {
            //        pnlObjects.Dock = DockStyle.Fill;
            //        pnlTextures.Dock = DockStyle.Bottom;
            //    }
            //    else
            //    {
            //        pnlObjects.Dock = DockStyle.Top;
            //        if (lstTextures.Visible)
            //            pnlTextures.Dock = DockStyle.Fill;
            //        else
            //            pnlTextures.Dock = DockStyle.Top;
            //    }
            //}
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
                lstObjects.Visible = chkSyncVis.Visible = chkAllObj.Visible = overObjPnl.Visible = true;
            } 
            SetDocks();
        }

        private void btnTextures_Click(object sender, EventArgs e)
        {
            if (lstTextures.Visible)
            {
                pnlTextures.Tag = pnlTextures.Height;
                pnlTextures.Height = btnTextures.Height;
                lstTextures.Visible = chkAllTextures.Visible = overTexPnl.Visible = false;
            }
            else
            {
                pnlTextures.Height = (int)pnlTextures.Tag;
                lstTextures.Visible = chkAllTextures.Visible = overTexPnl.Visible = true;
            }
            SetDocks();
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
            if (_selectedTexture != null)
                using (GLTextureWindow w = new GLTextureWindow())
                {
                    _mainWindow.ModelPanel.Release();
                    w.ShowDialog(this, _selectedTexture.Texture);
                    _mainWindow.ModelPanel.Capture();
                }
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

        private void chkSyncVis_CheckedChanged(object sender, EventArgs e)
        {
            _syncVis0 = chkSyncVis.Checked;
            _mainWindow.Updating = true;
            _mainWindow.SyncVIS0 = _syncVis0;
            _mainWindow.Updating = false;
        }

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

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
                dlg.ShowDialog(this.ParentForm, _mainWindow.GetAnimation(TargetAnimType));
        }


        private void overObjPnl_Paint(object sender, PaintEventArgs e)
        {
            //if (_srt0Selection == null && _pat0Selection == null)
            //    return;
            //Graphics g = e.Graphics;
            //for (int i = 0; i < lstObjects.Items.Count; i++)
            //{
            //    MDL0ObjectNode poly = lstObjects.Items[i] as MDL0ObjectNode;
            //    if (poly.UsableMaterialNode != null)
            //        if (_srt0Selection != null)
            //        {
            //            if (_srt0Selection.FindChildByType(poly.UsableMaterialNode.Name, false, ResourceType.SRT0Entry) != null)
            //            {
            //                Rectangle r = lstObjects.GetItemRectangle(i);
            //                g.DrawRectangle(Pens.Black, r);
            //            }
            //        }
            //        else if (_pat0Selection != null)
            //        {
            //            if (_pat0Selection.FindChildByType(poly.UsableMaterialNode.Name, false, ResourceType.PAT0Entry) != null)
            //            {
            //                Rectangle r = lstObjects.GetItemRectangle(i);
            //                g.DrawRectangle(Pens.Black, r);

            //            }
            //        }
            //}
        }

        private void overTexPnl_Paint(object sender, PaintEventArgs e)
        {
            //if (_srt0Selection == null && _pat0Selection == null)
            //    return;
            //Graphics g = e.Graphics;
            //ResourceNode rn = null;
            //if (_selectedPolygon != null && _selectedPolygon.UsableMaterialNode != null)
            //    for (int i = 0; i < lstTextures.Items.Count; i++)
            //    {
            //        MDL0TextureNode tex = lstTextures.Items[i] as MDL0TextureNode;
            //        if ((rn = _selectedPolygon.UsableMaterialNode.FindChild(tex.Name, true)) != null)
            //            if (_srt0Selection != null)
            //            {
            //                if (_srt0Selection.FindChildByType(_selectedPolygon.UsableMaterialNode.Name + "/Texture" + rn.Index, false, ResourceType.SRT0Texture) != null)
            //                {
            //                    Rectangle r = lstTextures.GetItemRectangle(i);
            //                    g.DrawRectangle(Pens.Black, r);
            //                }
            //            }
            //            else if (_pat0Selection != null)
            //            {
            //                if (_pat0Selection.FindChildByType(_selectedPolygon.UsableMaterialNode.Name + "/Texture" + rn.Index, false, ResourceType.PAT0Texture) != null)
            //                {
            //                    Rectangle r = lstTextures.GetItemRectangle(i);
            //                    g.DrawRectangle(Pens.Black, r);
            //                }
            //            }
            //    }
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

        private void List_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && sender is ListBox)
                (sender as ListBox).SelectedItems.Clear();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0BoneNode SelectedBone
        {
            get { return _mainWindow.SelectedBone as MDL0BoneNode; }
            set { _mainWindow.SelectedBone = value; }
        }

        //private void lstBones_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (SelectedBone != null)
        //        {
        //            lstBones.ContextMenuStrip = ctxBones;
        //            boneIndex.Text = "Bone Index: " + SelectedBone.BoneIndex.ToString();
        //        }
        //        else
        //            lstBones.ContextMenuStrip = null;
        //    }
        //}

        //private void lstBones_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (SelectedBone != null)
        //        SelectedBone._boneColor = SelectedBone._nodeColor = Color.Transparent;

        //    SelectedBone = lstBones.SelectedItem as MDL0BoneNode;

        //    _mainWindow.ModelPanel.Invalidate();
        //}

        //private void chkAllBones_CheckStateChanged(object sender, EventArgs e)
        //{
        //    if (lstBones.Items.Count == 0)
        //        return;

        //    _updating = true;

        //    lstBones.BeginUpdate();
        //    for (int i = 0; i < lstBones.Items.Count; i++)
        //        lstBones.SetItemCheckState(i, chkAllBones.CheckState);
        //    lstBones.EndUpdate();

        //    _updating = false;

        //    _mainWindow.ModelPanel.Invalidate();
        //}

        //private void lstBones_ItemCheck(object sender, ItemCheckEventArgs e)
        //{
        //    MDL0BoneNode bone = lstBones.Items[e.Index] as MDL0BoneNode;

        //    bone._render = e.NewValue == CheckState.Checked;

        //    if (!_updating)
        //        _mainWindow.ModelPanel.Invalidate();
        //}

        //private void renameBoneToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    new RenameDialog().ShowDialog(this, lstBones.SelectedItem as MDL0BoneNode);
        //}
    }
}

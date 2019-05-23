using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using BrawlLib.Modeling;
using OpenTK.Graphics.OpenGL;

namespace System.Windows.Forms
{
    public unsafe class CollisionEditor : UserControl
    {
        #region Designer

       public ModelPanel _modelPanel;
        private SplitContainer undoToolStrip;
        private SplitContainer redoToolStrip;
        private CheckBox chkAllModels;
        private Panel pnlPlaneProps;
        private Label label5;
        private Label labelType;
        private ComboBox cboMaterial;
        private Panel pnlObjProps;
        private ToolStrip toolStrip1;
        private ToolStripButton btnSplit;
        private ToolStripButton btnMerge;
        private ToolStripButton btnDelete;
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private ToolStripMenuItem snapToolStripMenuItem;
        private Panel panel1;
        private TrackBar trackBar1;
        private Button btnResetRot;
        private ToolStripButton btnResetCam;
        private GroupBox groupBox1;
        private CheckBox chkFallThrough;
        private GroupBox groupBox2;
        private CheckBox chkNoWalljump;
        private CheckBox chkRightLedge;
        private CheckBox chkTypeCharacters;
        private CheckBox chkTypeItems;
        private CheckBox chkTypePokemonTrainer;
        private CheckBox chkTypeRotating;
        
        // Advanced unknown flags
        private GroupBox groupBoxUnknownFlags;
        private CheckBox chkFlagUnknown1;
        private CheckBox chkFlagUnknown2;
        private CheckBox chkFlagUnknown3;
        private CheckBox chkFlagUnknown4;

        private Panel pnlPointProps;
        private NumericInputBox numX;
        private Label label2;
        private NumericInputBox numY;
        private Label label1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnSameX;
        private ToolStripButton btnSameY;
        private ToolStripMenuItem newObjectToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private TextBox txtModel;
        private Label label3;
        private Panel panel2;
        private CheckBox chkPoly;
        private Button btnRelink;
        private TextBox txtBone;
        private Label label4;
        private CheckBox chkBones;
        private CheckBox chkLeftLedge;
        private ComboBox cboType;
        private TreeView modelTree;
        private Button btnUnlink;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem assignToolStripMenuItem;
        private ToolStripMenuItem snapToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnResetSnap;
        private ToolStripButton btnUndo;
        private ToolStripButton btnRedo;
        private ToolStripSeparator toolStripSeparator3;
        private CheckBox chkObjModule;
        private CheckBox chkObjUnk;
        private CheckBox chkObjSSEUnk;
        private Button btnPlayAnims;
        private Panel panel4;
        private Panel panel3;
        private Button btnPrevFrame;
        private Button btnNextFrame;
        private ToolStripButton btnHelp;
        private CheckedListBox lstObjects;
        
        // StageBox edits
        private ToolStripSeparator toolStripSeparatorCamera;    // Seperator for Camera controls
        private ToolStripButton btnPerspectiveCam;              // Goes into perspective mode
        private ToolStripButton btnOrthographicCam;             // Goes into orthographic mode

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollisionEditor));
            this.undoToolStrip = new System.Windows.Forms.SplitContainer();
            this.redoToolStrip = new System.Windows.Forms.SplitContainer();
            this.modelTree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.assignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkBones = new System.Windows.Forms.CheckBox();
            this.chkPoly = new System.Windows.Forms.CheckBox();
            this.chkAllModels = new System.Windows.Forms.CheckBox();
            this.lstObjects = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.snapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlObjProps = new System.Windows.Forms.Panel();
            this.chkObjSSEUnk = new System.Windows.Forms.CheckBox();
            this.chkObjModule = new System.Windows.Forms.CheckBox();
            this.chkObjUnk = new System.Windows.Forms.CheckBox();
            this.btnUnlink = new System.Windows.Forms.Button();
            this.btnRelink = new System.Windows.Forms.Button();
            this.txtBone = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlPointProps = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.numY = new System.Windows.Forms.NumericInputBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numX = new System.Windows.Forms.NumericInputBox();
            this.pnlPlaneProps = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.chkTypeItems = new System.Windows.Forms.CheckBox();
            this.chkTypeCharacters = new System.Windows.Forms.CheckBox();
            this.chkTypePokemonTrainer = new System.Windows.Forms.CheckBox();
            this.chkTypeRotating = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkLeftLedge = new System.Windows.Forms.CheckBox();
            this.chkNoWalljump = new System.Windows.Forms.CheckBox();
            this.chkRightLedge = new System.Windows.Forms.CheckBox();
            this.chkFallThrough = new System.Windows.Forms.CheckBox();

            // Advanced flags
            this.groupBoxUnknownFlags = new System.Windows.Forms.GroupBox();
            this.chkFlagUnknown1 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnknown2 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnknown3 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnknown4 = new System.Windows.Forms.CheckBox();

            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnPlayAnims = new System.Windows.Forms.Button();
            this.btnPrevFrame = new System.Windows.Forms.Button();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this._modelPanel = new System.Windows.Forms.ModelPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUndo = new System.Windows.Forms.ToolStripButton();
            this.btnRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSplit = new System.Windows.Forms.ToolStripButton();
            this.btnMerge = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSameX = new System.Windows.Forms.ToolStripButton();
            this.btnSameY = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            
            // StageBox Camera Modes
            this.btnPerspectiveCam = new System.Windows.Forms.ToolStripButton();
            this.btnOrthographicCam = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorCamera = new System.Windows.Forms.ToolStripSeparator();
            
            this.btnResetCam = new System.Windows.Forms.ToolStripButton();
            this.btnResetSnap = new System.Windows.Forms.ToolStripButton();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnResetRot = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.undoToolStrip)).BeginInit();
            this.undoToolStrip.Panel1.SuspendLayout();
            this.undoToolStrip.Panel2.SuspendLayout();
            this.undoToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redoToolStrip)).BeginInit();
            this.redoToolStrip.Panel1.SuspendLayout();
            this.redoToolStrip.Panel2.SuspendLayout();
            this.redoToolStrip.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlObjProps.SuspendLayout();
            this.pnlPointProps.SuspendLayout();
            this.pnlPlaneProps.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxUnknownFlags.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // undoToolStrip
            // 
            this.undoToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoToolStrip.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.undoToolStrip.Location = new System.Drawing.Point(0, 0);
            this.undoToolStrip.Name = "undoToolStrip";
            // 
            // undoToolStrip.Panel1
            // 
            this.undoToolStrip.Panel1.Controls.Add(this.redoToolStrip);
            // 
            // undoToolStrip.Panel2
            // 
            this.undoToolStrip.Panel2.Controls.Add(this._modelPanel);
            this.undoToolStrip.Panel2.Controls.Add(this.panel1);
            this.undoToolStrip.Size = new System.Drawing.Size(694, 467);
            this.undoToolStrip.SplitterDistance = 209;
            this.undoToolStrip.TabIndex = 1;
            // 
            // redoToolStrip
            // 
            this.redoToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redoToolStrip.Location = new System.Drawing.Point(0, 0);
            this.redoToolStrip.Name = "redoToolStrip";
            this.redoToolStrip.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // redoToolStrip.Panel1
            // 
            this.redoToolStrip.Panel1.Controls.Add(this.modelTree);
            this.redoToolStrip.Panel1.Controls.Add(this.panel2);
            // 
            // redoToolStrip.Panel2
            // 
            this.redoToolStrip.Panel2.Controls.Add(this.lstObjects);
            this.redoToolStrip.Panel2.Controls.Add(this.panel3);
            this.redoToolStrip.Panel2.Controls.Add(this.panel4);
            this.redoToolStrip.Size = new System.Drawing.Size(209, 467);
            this.redoToolStrip.SplitterDistance = 242;
            this.redoToolStrip.TabIndex = 2;
            // 
            // modelTree
            // 
            this.modelTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modelTree.CheckBoxes = true;
            this.modelTree.ContextMenuStrip = this.contextMenuStrip2;
            this.modelTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelTree.HideSelection = false;
            this.modelTree.Location = new System.Drawing.Point(0, 17);
            this.modelTree.Name = "modelTree";
            this.modelTree.Size = new System.Drawing.Size(209, 225);
            this.modelTree.TabIndex = 4;
            this.modelTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.modelTree_AfterCheck);
            this.modelTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.modelTree_BeforeSelect);
            this.modelTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.modelTree_AfterSelect);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assignToolStripMenuItem,
            this.snapToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(110, 48);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // assignToolStripMenuItem
            // 
            this.assignToolStripMenuItem.Name = "assignToolStripMenuItem";
            this.assignToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.assignToolStripMenuItem.Text = "Assign";
            this.assignToolStripMenuItem.Click += new System.EventHandler(this.btnRelink_Click);
            // 
            // snapToolStripMenuItem1
            // 
            this.snapToolStripMenuItem1.Name = "snapToolStripMenuItem1";
            this.snapToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            this.snapToolStripMenuItem1.Text = "Snap";
            this.snapToolStripMenuItem1.Click += new System.EventHandler(this.snapToolStripMenuItem1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkBones);
            this.panel2.Controls.Add(this.chkPoly);
            this.panel2.Controls.Add(this.chkAllModels);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(209, 17);
            this.panel2.TabIndex = 3;
            // 
            // chkBones
            // 
            this.chkBones.Location = new System.Drawing.Point(100, 0);
            this.chkBones.Name = "chkBones";
            this.chkBones.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkBones.Size = new System.Drawing.Size(67, 17);
            this.chkBones.TabIndex = 4;
            this.chkBones.Text = "Bones";
            this.chkBones.UseVisualStyleBackColor = true;
            this.chkBones.CheckedChanged += new System.EventHandler(this.chkBones_CheckedChanged);
            // 
            // chkPoly
            // 
            this.chkPoly.Checked = true;
            this.chkPoly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoly.Location = new System.Drawing.Point(44, 0);
            this.chkPoly.Name = "chkPoly";
            this.chkPoly.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkPoly.Size = new System.Drawing.Size(54, 17);
            this.chkPoly.TabIndex = 3;
            this.chkPoly.Text = "Poly";
            this.chkPoly.ThreeState = true;
            this.chkPoly.UseVisualStyleBackColor = true;
            this.chkPoly.CheckStateChanged += new System.EventHandler(this.chkPoly_CheckStateChanged);
            // 
            // chkAllModels
            // 
            this.chkAllModels.Checked = true;
            this.chkAllModels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllModels.Location = new System.Drawing.Point(0, 0);
            this.chkAllModels.Name = "chkAllModels";
            this.chkAllModels.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllModels.Size = new System.Drawing.Size(41, 17);
            this.chkAllModels.TabIndex = 2;
            this.chkAllModels.Text = "All";
            this.chkAllModels.UseVisualStyleBackColor = true;
            this.chkAllModels.CheckedChanged += new System.EventHandler(this.chkAllModels_CheckedChanged);
            // 
            // lstObjects
            // 
            this.lstObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstObjects.ContextMenuStrip = this.contextMenuStrip1;
            this.lstObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObjects.FormattingEnabled = true;
            this.lstObjects.IntegralHeight = false;
            this.lstObjects.Location = new System.Drawing.Point(0, 0);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(209, 82);
            this.lstObjects.TabIndex = 1;
            this.lstObjects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstObjects_ItemCheck);
            this.lstObjects.SelectedValueChanged += new System.EventHandler(this.lstObjects_SelectedValueChanged);
            this.lstObjects.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstObjects_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newObjectToolStripMenuItem,
            this.toolStripMenuItem2,
            this.snapToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 82);
            // 
            // newObjectToolStripMenuItem
            // 
            this.newObjectToolStripMenuItem.Name = "newObjectToolStripMenuItem";
            this.newObjectToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.newObjectToolStripMenuItem.Text = "New Object";
            this.newObjectToolStripMenuItem.Click += new System.EventHandler(this.newObjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(133, 6);
            // 
            // snapToolStripMenuItem
            // 
            this.snapToolStripMenuItem.Name = "snapToolStripMenuItem";
            this.snapToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.snapToolStripMenuItem.Text = "Snap";
            this.snapToolStripMenuItem.Click += new System.EventHandler(this.snapToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(133, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            this.deleteToolStripMenuItem.ShortcutKeys = Keys.Shift | Keys.Delete;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pnlPlaneProps);
            this.panel3.Controls.Add(this.pnlPointProps);
            this.panel3.Controls.Add(this.pnlObjProps);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 82);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(209, 115);
            this.panel3.TabIndex = 16;
            // 
            // pnlObjProps
            // 
            this.pnlObjProps.Controls.Add(this.chkObjSSEUnk);
            this.pnlObjProps.Controls.Add(this.chkObjModule);
            this.pnlObjProps.Controls.Add(this.chkObjUnk);
            this.pnlObjProps.Controls.Add(this.btnUnlink);
            this.pnlObjProps.Controls.Add(this.btnRelink);
            this.pnlObjProps.Controls.Add(this.txtBone);
            this.pnlObjProps.Controls.Add(this.label4);
            this.pnlObjProps.Controls.Add(this.txtModel);
            this.pnlObjProps.Controls.Add(this.label3);
            this.pnlObjProps.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlObjProps.Location = new System.Drawing.Point(0, -15);
            this.pnlObjProps.Name = "pnlObjProps";
            this.pnlObjProps.Size = new System.Drawing.Size(209, 130);
            this.pnlObjProps.TabIndex = 1;
            this.pnlObjProps.Visible = false;
            // 
            // chkObjSSEUnk
            // 
            this.chkObjSSEUnk.AutoSize = true;
            this.chkObjSSEUnk.Location = new System.Drawing.Point(10, 102);
            this.chkObjSSEUnk.Name = "chkObjSSEUnk";
            this.chkObjSSEUnk.Size = new System.Drawing.Size(96, 17);
            this.chkObjSSEUnk.TabIndex = 15;
            this.chkObjSSEUnk.Text = "SSE Unknown";
            this.chkObjSSEUnk.UseVisualStyleBackColor = true;
            this.chkObjSSEUnk.CheckedChanged += new System.EventHandler(this.chkObjSSEUnk_CheckedChanged);
            // 
            // chkObjModule
            // 
            this.chkObjModule.AutoSize = true;
            this.chkObjModule.Location = new System.Drawing.Point(10, 79);
            this.chkObjModule.Name = "chkObjModule";
            this.chkObjModule.Size = new System.Drawing.Size(111, 17);
            this.chkObjModule.TabIndex = 14;
            this.chkObjModule.Text = "Module Controlled";
            this.chkObjModule.UseVisualStyleBackColor = true;
            this.chkObjModule.CheckedChanged += new System.EventHandler(this.chkObjModule_CheckedChanged);
            // 
            // chkObjUnk
            // 
            this.chkObjUnk.AutoSize = true;
            this.chkObjUnk.Location = new System.Drawing.Point(10, 56);
            this.chkObjUnk.Name = "chkObjUnk";
            this.chkObjUnk.Size = new System.Drawing.Size(72, 17);
            this.chkObjUnk.TabIndex = 13;
            this.chkObjUnk.Text = "Unknown";
            this.chkObjUnk.UseVisualStyleBackColor = true;
            this.chkObjUnk.CheckedChanged += new System.EventHandler(this.chkObjUnk_CheckedChanged);
            // 
            // btnUnlink
            // 
            this.btnUnlink.Location = new System.Drawing.Point(177, 22);
            this.btnUnlink.Name = "btnUnlink";
            this.btnUnlink.Size = new System.Drawing.Size(28, 21);
            this.btnUnlink.TabIndex = 12;
            this.btnUnlink.Text = "-";
            this.btnUnlink.UseVisualStyleBackColor = true;
            this.btnUnlink.Click += new System.EventHandler(this.btnUnlink_Click);
            // 
            // btnRelink
            // 
            this.btnRelink.Location = new System.Drawing.Point(177, 2);
            this.btnRelink.Name = "btnRelink";
            this.btnRelink.Size = new System.Drawing.Size(28, 21);
            this.btnRelink.TabIndex = 4;
            this.btnRelink.Text = "+";
            this.btnRelink.UseVisualStyleBackColor = true;
            this.btnRelink.Click += new System.EventHandler(this.btnRelink_Click);
            // 
            // txtBone
            // 
            this.txtBone.Location = new System.Drawing.Point(49, 23);
            this.txtBone.Name = "txtBone";
            this.txtBone.ReadOnly = true;
            this.txtBone.Size = new System.Drawing.Size(126, 20);
            this.txtBone.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Bone:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(49, 3);
            this.txtModel.Name = "txtModel";
            this.txtModel.ReadOnly = true;
            this.txtModel.Size = new System.Drawing.Size(126, 20);
            this.txtModel.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Model:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPointProps
            // 
            this.pnlPointProps.Controls.Add(this.label2);
            this.pnlPointProps.Controls.Add(this.numY);
            this.pnlPointProps.Controls.Add(this.label1);
            this.pnlPointProps.Controls.Add(this.numX);
            this.pnlPointProps.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPointProps.Location = new System.Drawing.Point(0, -85);
            this.pnlPointProps.Name = "pnlPointProps";
            this.pnlPointProps.Size = new System.Drawing.Size(209, 70);
            this.pnlPointProps.TabIndex = 15;
            this.pnlPointProps.Visible = false;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(18, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numY
            // 
            this.numY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numY.Integral = false;
            this.numY.Location = new System.Drawing.Point(59, 32);
            this.numY.MaximumValue = 3.402823E+38F;
            this.numY.MinimumValue = -3.402823E+38F;
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(100, 20);
            this.numY.TabIndex = 2;
            this.numY.Text = "0";
            this.numY.ValueChanged += new System.EventHandler(this.numY_ValueChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numX
            // 
            this.numX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numX.Integral = false;
            this.numX.Location = new System.Drawing.Point(59, 13);
            this.numX.MaximumValue = 3.402823E+38F;
            this.numX.MinimumValue = -3.402823E+38F;
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(100, 20);
            this.numX.TabIndex = 0;
            this.numX.Text = "0";
            this.numX.ValueChanged += new System.EventHandler(this.numX_ValueChanged);
            // 
            // pnlPlaneProps
            // 
            this.pnlPlaneProps.Controls.Add(this.groupBoxUnknownFlags);
            this.pnlPlaneProps.Controls.Add(this.groupBox2);
            this.pnlPlaneProps.Controls.Add(this.groupBox1);
            this.pnlPlaneProps.Controls.Add(this.cboMaterial);
            this.pnlPlaneProps.Controls.Add(this.cboType);
            this.pnlPlaneProps.Controls.Add(this.label5);
            this.pnlPlaneProps.Controls.Add(this.labelType);
            this.pnlPlaneProps.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPlaneProps.Location = new System.Drawing.Point(0, -199);
            this.pnlPlaneProps.Name = "pnlPlaneProps";
            this.pnlPlaneProps.Size = new System.Drawing.Size(209, 114);
            this.pnlPlaneProps.TabIndex = 0;
            this.pnlPlaneProps.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.chkTypeCharacters);
            this.groupBox2.Controls.Add(this.chkTypeItems);
            this.groupBox2.Controls.Add(this.chkTypePokemonTrainer);
            this.groupBox2.Controls.Add(this.chkTypeRotating);
            this.groupBox2.Location = new System.Drawing.Point(101, 49);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox2.Size = new System.Drawing.Size(105, 86);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(66, 4);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(139, 21);
            this.cboType.TabIndex = 5;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // groupBoxUnknownFlags
            // 
            this.groupBoxUnknownFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxUnknownFlags.Controls.Add(this.chkFlagUnknown4);
            this.groupBoxUnknownFlags.Controls.Add(this.chkFlagUnknown3);
            this.groupBoxUnknownFlags.Controls.Add(this.chkFlagUnknown2);
            this.groupBoxUnknownFlags.Controls.Add(this.chkFlagUnknown1);
            this.groupBoxUnknownFlags.Location = new System.Drawing.Point(0, 135);
            this.groupBoxUnknownFlags.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxUnknownFlags.Name = "groupBoxUnknownFlags";
            this.groupBoxUnknownFlags.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxUnknownFlags.Size = new System.Drawing.Size(205, 59);
            this.groupBoxUnknownFlags.TabIndex = 14;
            this.groupBoxUnknownFlags.TabStop = false;
            this.groupBoxUnknownFlags.Text = "Unknown Flags";
            // 
            // chkFlagUnknown1
            // 
            this.chkFlagUnknown1.Location = new System.Drawing.Point(8, 17);
            this.chkFlagUnknown1.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown1.Name = "chkFlagUnknown1";
            this.chkFlagUnknown1.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown1.TabIndex = 3;
            this.chkFlagUnknown1.Text = "1";
            this.chkFlagUnknown1.UseVisualStyleBackColor = true;
            this.chkFlagUnknown1.CheckedChanged += new System.EventHandler(this.chkFlagUnknown1_CheckedChanged);
            // 
            // chkFlagUnknown2
            // 
            this.chkFlagUnknown2.Location = new System.Drawing.Point(60, 17);
            this.chkFlagUnknown2.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown2.Name = "chkFlagUnknown2";
            this.chkFlagUnknown2.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown2.TabIndex = 3;
            this.chkFlagUnknown2.Text = "2";
            this.chkFlagUnknown2.UseVisualStyleBackColor = true;
            this.chkFlagUnknown2.CheckedChanged += new System.EventHandler(this.chkFlagUnknown2_CheckedChanged);
            // 
            // chkFlagUnknown3
            // 
            this.chkFlagUnknown3.Location = new System.Drawing.Point(112, 17);
            this.chkFlagUnknown3.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown3.Name = "chkFlagUnknown3";
            this.chkFlagUnknown3.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown3.TabIndex = 3;
            this.chkFlagUnknown3.Text = "3";
            this.chkFlagUnknown3.UseVisualStyleBackColor = true;
            this.chkFlagUnknown3.CheckedChanged += new System.EventHandler(this.chkFlagUnknown3_CheckedChanged);
            // 
            // chkFlagUnknown4
            // 
            this.chkFlagUnknown4.Location = new System.Drawing.Point(164, 17);
            this.chkFlagUnknown4.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown4.Name = "chkFlagUnknown4";
            this.chkFlagUnknown4.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown4.TabIndex = 3;
            this.chkFlagUnknown4.Text = "4";
            this.chkFlagUnknown4.UseVisualStyleBackColor = true;
            this.chkFlagUnknown4.CheckedChanged += new System.EventHandler(this.chkFlagUnknown4_CheckedChanged);
            // 
            // chkTypeCharacters
            // 
            this.chkTypeCharacters.Location = new System.Drawing.Point(8, 17);
            this.chkTypeCharacters.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeCharacters.Name = "chkTypeCharacters";
            this.chkTypeCharacters.Size = new System.Drawing.Size(86, 18);
            this.chkTypeCharacters.TabIndex = 4;
            this.chkTypeCharacters.Text = "Characters";
            this.chkTypeCharacters.UseVisualStyleBackColor = true;
            this.chkTypeCharacters.CheckedChanged += new System.EventHandler(this.chkTypeCharacters_CheckedChanged);
            // 
            // chkTypeItems
            // 
            this.chkTypeItems.Location = new System.Drawing.Point(8, 33);
            this.chkTypeItems.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeItems.Name = "chkTypeItems";
            this.chkTypeItems.Size = new System.Drawing.Size(86, 18);
            this.chkTypeItems.TabIndex = 3;
            this.chkTypeItems.Text = "Items";
            this.chkTypeItems.UseVisualStyleBackColor = true;
            this.chkTypeItems.CheckedChanged += new System.EventHandler(this.chkTypeItems_CheckedChanged);
            // 
            // chkTypePokemonTrainer
            // 
            this.chkTypePokemonTrainer.Location = new System.Drawing.Point(8, 49);
            this.chkTypePokemonTrainer.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypePokemonTrainer.Name = "chkTypePokemonTrainer";
            this.chkTypePokemonTrainer.Size = new System.Drawing.Size(86, 18);
            this.chkTypePokemonTrainer.TabIndex = 3;
            this.chkTypePokemonTrainer.Text = "PokéTrainer";
            this.chkTypePokemonTrainer.UseVisualStyleBackColor = true;
            this.chkTypePokemonTrainer.CheckedChanged += new System.EventHandler(this.chkTypePokemonTrainer_CheckedChanged);
            // 
            // chkTypeRotating
            // 
            this.chkTypeRotating.Location = new System.Drawing.Point(8, 65);
            this.chkTypeRotating.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeRotating.Name = "chkTypeRotating";
            this.chkTypeRotating.Size = new System.Drawing.Size(86, 18);
            this.chkTypeRotating.TabIndex = 4;
            this.chkTypeRotating.Text = "Rotating";
            this.chkTypeRotating.UseVisualStyleBackColor = true;
            this.chkTypeRotating.CheckedChanged += new System.EventHandler(this.chkTypeRotating_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.chkLeftLedge);
            this.groupBox1.Controls.Add(this.chkNoWalljump);
            this.groupBox1.Controls.Add(this.chkRightLedge);
            this.groupBox1.Controls.Add(this.chkFallThrough);
            this.groupBox1.Location = new System.Drawing.Point(-3, 49);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(104, 86);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flags";
            // 
            // chkLeftLedge
            // 
            this.chkLeftLedge.Location = new System.Drawing.Point(8, 33);
            this.chkLeftLedge.Margin = new System.Windows.Forms.Padding(0);
            this.chkLeftLedge.Name = "chkLeftLedge";
            this.chkLeftLedge.Size = new System.Drawing.Size(86, 18);
            this.chkLeftLedge.TabIndex = 4;
            this.chkLeftLedge.Text = "Left Ledge";
            this.chkLeftLedge.UseVisualStyleBackColor = true;
            this.chkLeftLedge.CheckedChanged += new System.EventHandler(this.chkLeftLedge_CheckedChanged);
            // 
            // chkNoWalljump
            // 
            this.chkNoWalljump.Location = new System.Drawing.Point(8, 65);
            this.chkNoWalljump.Margin = new System.Windows.Forms.Padding(0);
            this.chkNoWalljump.Name = "chkNoWalljump";
            this.chkNoWalljump.Size = new System.Drawing.Size(90, 18);
            this.chkNoWalljump.TabIndex = 2;
            this.chkNoWalljump.Text = "No Walljump";
            this.chkNoWalljump.UseVisualStyleBackColor = true;
            this.chkNoWalljump.CheckedChanged += new System.EventHandler(this.chkNoWalljump_CheckedChanged);
            // 
            // chkRightLedge
            // 
            this.chkRightLedge.Location = new System.Drawing.Point(8, 49);
            this.chkRightLedge.Margin = new System.Windows.Forms.Padding(0);
            this.chkRightLedge.Name = "chkRightLedge";
            this.chkRightLedge.Size = new System.Drawing.Size(86, 18);
            this.chkRightLedge.TabIndex = 1;
            this.chkRightLedge.Text = "Right Ledge";
            this.chkRightLedge.UseVisualStyleBackColor = true;
            this.chkRightLedge.CheckedChanged += new System.EventHandler(this.chkRightLedge_CheckedChanged);
            // 
            // chkFallThrough
            // 
            this.chkFallThrough.Location = new System.Drawing.Point(8, 17);
            this.chkFallThrough.Margin = new System.Windows.Forms.Padding(0);
            this.chkFallThrough.Name = "chkFallThrough";
            this.chkFallThrough.Size = new System.Drawing.Size(90, 18);
            this.chkFallThrough.TabIndex = 0;
            this.chkFallThrough.Text = "Fall-Through";
            this.chkFallThrough.UseVisualStyleBackColor = true;
            this.chkFallThrough.CheckedChanged += new System.EventHandler(this.chkFallThrough_CheckedChanged);
            // 
            // cboMaterial
            // 
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(66, 25);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(139, 21);
            this.cboMaterial.TabIndex = 12;
            this.cboMaterial.SelectedIndexChanged += new System.EventHandler(this.cboMaterial_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "Material:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(7, 4);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(53, 21);
            this.labelType.TabIndex = 8;
            this.labelType.Text = "Type:";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnPlayAnims);
            this.panel4.Controls.Add(this.btnPrevFrame);
            this.panel4.Controls.Add(this.btnNextFrame);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Enabled = false;
            this.panel4.Location = new System.Drawing.Point(0, 197);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(209, 24);
            this.panel4.TabIndex = 17;
            this.panel4.Visible = false;
            // 
            // btnPlayAnims
            // 
            this.btnPlayAnims.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlayAnims.Location = new System.Drawing.Point(35, 0);
            this.btnPlayAnims.Name = "btnPlayAnims";
            this.btnPlayAnims.Size = new System.Drawing.Size(139, 24);
            this.btnPlayAnims.TabIndex = 16;
            this.btnPlayAnims.Text = "Play Animations";
            this.btnPlayAnims.UseVisualStyleBackColor = true;
            this.btnPlayAnims.Click += new System.EventHandler(this.btnPlayAnims_Click);
            // 
            // btnPrevFrame
            // 
            this.btnPrevFrame.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrevFrame.Location = new System.Drawing.Point(0, 0);
            this.btnPrevFrame.Name = "btnPrevFrame";
            this.btnPrevFrame.Size = new System.Drawing.Size(35, 24);
            this.btnPrevFrame.TabIndex = 18;
            this.btnPrevFrame.Text = "<";
            this.btnPrevFrame.UseVisualStyleBackColor = true;
            this.btnPrevFrame.Click += new System.EventHandler(this.btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNextFrame.Location = new System.Drawing.Point(174, 0);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(35, 24);
            this.btnNextFrame.TabIndex = 17;
            this.btnNextFrame.Text = ">";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // _modelPanel
            // 
            this._modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._modelPanel.Location = new System.Drawing.Point(0, 25);
            this._modelPanel.Name = "_modelPanel";
            this._modelPanel.Size = new System.Drawing.Size(481, 442);
            this._modelPanel.TabIndex = 0;
            this._modelPanel.PreRender += new System.Windows.Forms.GLRenderEventHandler(this._modelPanel_PreRender);
            this._modelPanel.PostRender += new System.Windows.Forms.GLRenderEventHandler(this._modelPanel_PostRender);
            this._modelPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this._modelPanel_KeyDown);
            this._modelPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseDown);
            this._modelPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseMove);
            this._modelPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.btnResetRot);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 25);
            this.panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUndo,
            this.btnRedo,
            this.toolStripSeparator3,
            this.btnSplit,
            this.btnMerge,
            this.btnDelete,
            this.toolStripSeparator2,
            this.btnSameX,
            this.btnSameY,
            this.toolStripSeparator1,
            
            this.btnPerspectiveCam,
            this.btnOrthographicCam,
            this.toolStripSeparatorCamera,
            
            this.btnResetCam,
            this.btnResetSnap,
            this.btnHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(335, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnUndo
            // 
            this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUndo.Enabled = false;
            this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(40, 22);
            this.btnUndo.Text = "Undo";
            this.btnUndo.Click += new System.EventHandler(this.Undo);
            // 
            // btnRedo
            // 
            this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRedo.Enabled = false;
            this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(38, 22);
            this.btnRedo.Text = "Redo";
            this.btnRedo.Click += new System.EventHandler(this.Redo);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSplit
            // 
            this.btnSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSplit.Enabled = false;
            this.btnSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(34, 22);
            this.btnSplit.Text = "Split";
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMerge.Enabled = false;
            this.btnMerge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(45, 22);
            this.btnMerge.Text = "Merge";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(44, 22);
            this.btnDelete.Text = "Delete";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSameX
            // 
            this.btnSameX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSameX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSameX.Name = "btnSameX";
            this.btnSameX.Size = new System.Drawing.Size(49, 22);
            this.btnSameX.Text = "Align X";
            this.btnSameX.Click += new System.EventHandler(this.btnSameX_Click);
            // 
            // btnSameY
            // 
            this.btnSameY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSameY.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSameY.Name = "btnSameY";
            this.btnSameY.Size = new System.Drawing.Size(49, 22);
            this.btnSameY.Text = "Align Y";
            this.btnSameY.Click += new System.EventHandler(this.btnSameY_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPerspectiveCam
            // 
            this.btnPerspectiveCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPerspectiveCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPerspectiveCam.Name = "btnPerspectiveCam";
            this.btnPerspectiveCam.Size = new System.Drawing.Size(83, 19);
            this.btnPerspectiveCam.Text = "Perspective";
            this.btnPerspectiveCam.Click += new System.EventHandler(this.btnPerspectiveCam_Click);
            // 
            // btnOrthographicCam
            // 
            this.btnOrthographicCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOrthographicCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOrthographicCam.Name = "btnOrthographicCam";
            this.btnOrthographicCam.Size = new System.Drawing.Size(83, 19);
            this.btnOrthographicCam.Text = "Orthographic";
            this.btnOrthographicCam.Click += new System.EventHandler(this.btnOrthographicCam_Click);
            // 
            // toolStripSeparatorCamera (StageBox)
            // 
            this.toolStripSeparator1.Name = "toolStripSeparatorCamera";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnResetCam
            // 
            this.btnResetCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResetCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetCam.Name = "btnResetCam";
            this.btnResetCam.Size = new System.Drawing.Size(83, 19);
            this.btnResetCam.Text = "Reset Camera";
            this.btnResetCam.Click += new System.EventHandler(this.btnResetCam_Click);
            // 
            // btnResetSnap
            // 
            this.btnResetSnap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResetSnap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetSnap.Name = "btnResetSnap";
            this.btnResetSnap.Size = new System.Drawing.Size(57, 19);
            this.btnResetSnap.Text = "Un-Snap";
            this.btnResetSnap.Click += new System.EventHandler(this.btnResetSnap_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(36, 19);
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnResetRot
            // 
            this.btnResetRot.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnResetRot.Enabled = false;
            this.btnResetRot.FlatAppearance.BorderSize = 0;
            this.btnResetRot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetRot.Location = new System.Drawing.Point(335, 0);
            this.btnResetRot.Name = "btnResetRot";
            this.btnResetRot.Size = new System.Drawing.Size(16, 25);
            this.btnResetRot.TabIndex = 4;
            this.btnResetRot.Text = "*";
            this.btnResetRot.UseVisualStyleBackColor = true;
            this.btnResetRot.Visible = false;
            this.btnResetRot.Click += new System.EventHandler(this.btnResetRot_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.trackBar1.Enabled = false;
            this.trackBar1.Location = new System.Drawing.Point(351, 0);
            this.trackBar1.Maximum = 180;
            this.trackBar1.Minimum = -180;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(130, 25);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Visible = false;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // CollisionEditor
            // 
            this.BackColor = System.Drawing.Color.Lavender;
            this.Controls.Add(this.undoToolStrip);
            this.Name = "CollisionEditor";
            this.Size = new System.Drawing.Size(694, 467);
            this.undoToolStrip.Panel1.ResumeLayout(false);
            this.undoToolStrip.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.undoToolStrip)).EndInit();
            this.undoToolStrip.ResumeLayout(false);
            this.redoToolStrip.Panel1.ResumeLayout(false);
            this.redoToolStrip.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.redoToolStrip)).EndInit();
            this.redoToolStrip.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlObjProps.ResumeLayout(false);
            this.pnlObjProps.PerformLayout();
            this.pnlPointProps.ResumeLayout(false);
            this.pnlPointProps.PerformLayout();
            this.pnlPlaneProps.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxUnknownFlags.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private const float SelectWidth = 7.0f;
        private const float PointSelectRadius = 1.5f;
        private const float SmallIncrement = 0.5f;
        private const float LargeIncrement = 3.0f;

        private CollisionNode _targetNode;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetNode
        {
            get { return _targetNode; }
            set { TargetChanged(value); }
        }

        private bool _updating;
        private CollisionObject _selectedObject;
        private Matrix _snapMatrix;

        private bool _hovering;
        private List<CollisionLink> _selectedLinks = new List<CollisionLink>();
        private List<CollisionPlane> _selectedPlanes = new List<CollisionPlane>();

        private bool _selecting, _selectInverse;
        private Vector3 _selectStart, _selectLast, _selectEnd;
        private bool _creating;

        private CollisionState save;
        private List<CollisionState> undoSaves = new List<CollisionState>();
        private List<CollisionState> redoSaves = new List<CollisionState>();
        private int saveIndex = 0;
        private bool hasMoved = false;

        public CollisionEditor()
        {
            InitializeComponent();

            _modelPanel.AddViewport(ModelPanelViewport.DefaultPerspective);

            _modelPanel.CurrentViewport.DefaultTranslate = new Vector3(0.0f, 10.0f, 250.0f);
            _modelPanel.CurrentViewport.AllowSelection = false;

            pnlObjProps.Dock = DockStyle.Fill;
            pnlPlaneProps.Dock = DockStyle.Fill;
            pnlPointProps.Dock = DockStyle.Fill;

            _updating = true;
            cboMaterial.DataSource = Enum.GetValues(typeof(CollisionPlaneMaterial));
            cboType.DataSource = Enum.GetValues(typeof(CollisionPlaneType));
            _updating = false;
        }

        private void TargetChanged(CollisionNode node)
        {
            ClearSelection();
            trackBar1.Value = 0;
            _snapMatrix = Matrix.Identity;
            _selectedObject = null;

            _modelPanel.ClearAll();

            _targetNode = node;

            PopulateModelList();
            PopulateObjectList();

            if (lstObjects.Items.Count > 0)
            {
                lstObjects.SelectedIndex = 0;
                _selectedObject = lstObjects.Items[0] as CollisionObject;
                SnapObject();
            }
            ObjectSelected();

            _modelPanel.ResetCamera();
        }

        private void SelectionModified()
        {
            _selectedPlanes.Clear();
            foreach (CollisionLink l in _selectedLinks)
                foreach (CollisionPlane p in l._members)
                    if (_selectedLinks.Contains(p._linkLeft) &&
                        _selectedLinks.Contains(p._linkRight) &&
                        !_selectedPlanes.Contains(p))
                        _selectedPlanes.Add(p);

            pnlPlaneProps.Visible = false;
            pnlObjProps.Visible = false;
            pnlPointProps.Visible = false;
            panel3.Height = 0;

            if (_selectedPlanes.Count > 0)
            {
                pnlPlaneProps.Visible = true;
                panel3.Height = 175;
            }
            else if (_selectedLinks.Count == 1)
            {
                pnlPointProps.Visible = true;
                panel3.Height = 70;
            }

            UpdatePropPanels();
        }

        private void UpdatePropPanels()
        {
            _updating = true;
            
            if (pnlPlaneProps.Visible)
            {
                CollisionPlane p = _selectedPlanes[0];

                //Material
                if((byte)p._material >= 32)
                {
                    // Select basic by default (currently cannot display expanded collisions in default previewer)
                    cboMaterial.SelectedItem = (CollisionPlaneMaterial)(0x0);
                }
                else
                {
                    // Otherwise convert to the proper place in the unexpanded list
                    cboMaterial.SelectedItem = (CollisionPlaneMaterial)(p._material);
                }
                //Type
                cboType.SelectedItem = p.Type;
                //Flags
                chkFallThrough.Checked = p.IsFallThrough;
                chkLeftLedge.Checked = p.IsLeftLedge;
                chkRightLedge.Checked = p.IsRightLedge;
                chkNoWalljump.Checked = p.IsNoWalljump;
                chkTypeCharacters.Checked = p.IsCharacters;
                chkTypeItems.Checked = p.IsItems;
                chkTypePokemonTrainer.Checked = p.IsPokemonTrainer;
                chkTypeRotating.Checked = p.IsRotating;
                //UnknownFlags
                chkFlagUnknown1.Checked = p.IsUnknownStageBox;
                chkFlagUnknown2.Checked = p.IsUnknownFlag1;
                chkFlagUnknown3.Checked = p.IsUnknownFlag3;
                chkFlagUnknown4.Checked = p.IsUnknownFlag4;
            }
            else if (pnlPointProps.Visible)
            {
                numX.Value = _selectedLinks[0].Value._x;
                numY.Value = _selectedLinks[0].Value._y;
            }
            else if (pnlObjProps.Visible)
            {
                txtModel.Text = _selectedObject._modelName;
                txtBone.Text = _selectedObject._boneName;
                chkObjUnk.Checked = _selectedObject._flags[0];
                chkObjModule.Checked = _selectedObject._flags[2];
                chkObjSSEUnk.Checked = _selectedObject._flags[3];
            }
            
            _updating = false;
        }

        private void PopulateModelList()
        {
            modelTree.BeginUpdate();
            modelTree.Nodes.Clear();

            if ((_targetNode != null) && (_targetNode._parent != null))
                foreach (MDL0Node n in _targetNode._parent.FindChildrenByType(null, ResourceType.MDL0))
                {
                    TreeNode modelNode = new TreeNode(n._name) { Tag = n, Checked = true };
                    modelTree.Nodes.Add(modelNode);

                    foreach (MDL0BoneNode bone in n._linker.BoneCache)
                        modelNode.Nodes.Add(new TreeNode(bone._name) { Tag = bone, Checked = true });

                    _modelPanel.AddTarget(n);
                    n.ResetToBindState();
                }

            modelTree.EndUpdate();
        }

        #region Object List

        private void PopulateObjectList()
        {
            lstObjects.BeginUpdate();
            lstObjects.Items.Clear();

            if (_targetNode != null)
                foreach (CollisionObject obj in _targetNode._objects)
                {
                    obj._render = true;
                    lstObjects.Items.Add(obj, true);

                    if (!obj._flags[1])
                        foreach (TreeNode n in modelTree.Nodes)
                            foreach (TreeNode b in n.Nodes)
                            {
                                MDL0BoneNode bone = b.Tag as MDL0BoneNode;
                                if (bone != null && bone.Name == obj._boneName && bone.BoneIndex == obj._boneIndex)
                                    obj._linkedBone = bone;
                            }
                }

            lstObjects.EndUpdate();
        }
        private void lstObjects_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstObjects.IndexFromPoint(e.Location);
            lstObjects.SelectedIndex = index;
        }
        private void lstObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            _selectedObject = lstObjects.SelectedItem as CollisionObject;
            ObjectSelected();
        }
        private void snapToolStripMenuItem_Click(object sender, EventArgs e) { SnapObject(); }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
                return;

            _targetNode._objects.Remove(_selectedObject);
            lstObjects.Items.Remove(_selectedObject);
            _selectedObject = null;
            ClearSelection();
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        private void newObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedObject = new CollisionObject();
            _targetNode._objects.Add(_selectedObject);
            lstObjects.Items.Add(_selectedObject, true);
            lstObjects.SelectedItem = _selectedObject;
            //TargetNode.SignalPropertyChange();
        }

        private void ObjectSelected()
        {
            pnlPlaneProps.Visible = false;
            pnlPointProps.Visible = false;
            pnlObjProps.Visible = false;
            panel3.Height = 0;
            if (_selectedObject != null)
            {
                pnlObjProps.Visible = true;
                panel3.Height = 130;
                UpdatePropPanels();
            }
        }

        private void SnapObject()
        {
            if (_selectedObject == null)
                return;

            _updating = true;

            _snapMatrix = Matrix.Identity;

            for (int i = 0; i < lstObjects.Items.Count; i++)
                lstObjects.SetItemChecked(i, false);

            //Set snap matrix
            if (!String.IsNullOrEmpty(_selectedObject._modelName))
                foreach (TreeNode node in modelTree.Nodes)
                    if (node.Text == _selectedObject._modelName)
                    {
                        foreach (TreeNode bNode in node.Nodes)
                            if (bNode.Text == _selectedObject._boneName)
                            {
                                _snapMatrix = ((MDL0BoneNode)bNode.Tag)._inverseBindMatrix;
                                break;
                            }
                        break;
                    }

            //Show objects with similar bones
            for (int i = lstObjects.Items.Count; i-- > 0; )
            {
                CollisionObject obj = lstObjects.Items[i] as CollisionObject;
                if ((obj._modelName == _selectedObject._modelName) && (obj._boneName == _selectedObject._boneName))
                    lstObjects.SetItemChecked(i, true);
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        private void lstObjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CollisionObject obj = lstObjects.Items[e.Index] as CollisionObject;
            obj._render = e.NewValue == CheckState.Checked;

            ClearSelection();

            if (!_updating)
                _modelPanel.Invalidate();
        }


        #endregion

        private void ClearSelection()
        {
            foreach (CollisionLink l in _selectedLinks)
                l._highlight = false;
            _selectedLinks.Clear();
            _selectedPlanes.Clear();
        }

        private void UpdateSelection(bool finish)
        {
            foreach (CollisionObject obj in _targetNode._objects)
                foreach (CollisionLink link in obj._points)
                {
                    link._highlight = false;
                    if (!obj._render)
                        continue;

                    Vector3 point = (Vector3)link.Value;

                    if (_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        if (finish)
                            _selectedLinks.Remove(link);
                        continue;
                    }

                    if (_selectedLinks.Contains(link))
                        link._highlight = true;
                    else if (!_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        link._highlight = true;
                        if (finish)
                            _selectedLinks.Add(link);
                    }
                }
        }
        public void UpdateTools()
        {
            if (_selecting || _hovering || (_selectedLinks.Count == 0))
                btnMerge.Enabled = btnSplit.Enabled = btnSameX.Enabled = btnSameY.Enabled = false;
            else
            {
                btnMerge.Enabled = btnSameX.Enabled = btnSameY.Enabled = _selectedLinks.Count > 1;
                btnSplit.Enabled = true;
            }
        }

        private void _treeObjects_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is CollisionObject)
                (e.Node.Tag as CollisionObject)._render = e.Node.Checked;
            if (e.Node.Tag is CollisionPlane)
                (e.Node.Tag as CollisionPlane)._render = e.Node.Checked;

            _modelPanel.Invalidate();
        }

        private void chkAllModels_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in modelTree.Nodes)
                node.Checked = chkAllModels.Checked;
        }

        private void BeginHover(Vector3 point)
        {
            if (_hovering)
                return;

            if (!hasMoved) //Create undo for first move
            {
                CreateUndo();
                hasMoved = true;
                TargetNode.SignalPropertyChange();
            }
            
            _selectStart = _selectLast = point;
            _hovering = true;
            UpdateTools();
        }
        private void UpdateHover(int x, int y)
        {
            if (!_hovering)
                return;

            _selectEnd = Vector3.IntersectZ(_modelPanel.CurrentViewport.UnProject(x, y, 0.0f), _modelPanel.CurrentViewport.UnProject(x, y, 1.0f), _selectLast._z);
            
            //Apply difference in start/end
            Vector3 diff = _selectEnd - _selectLast;
            _selectLast = _selectEnd;

            //Move points
            foreach (CollisionLink p in _selectedLinks)
                p.Value += diff;
            
            _modelPanel.Invalidate();

            UpdatePropPanels();
        }
        private void CancelHover()
        {
            if (!_hovering)
                return;

            if (hasMoved)
            {
                undoSaves.RemoveAt(undoSaves.Count - 1);
                saveIndex--;
                hasMoved = false;
                if (saveIndex == 0)
                    btnUndo.Enabled = false;
            }

            _hovering = false;

            if (_creating)
            {
                _creating = false;
                //Delete points/plane
                _selectedLinks[0].Pop();
                ClearSelection();
                SelectionModified();
            }
            else
            {
                Vector3 diff = _selectStart - _selectLast;
                foreach (CollisionLink l in _selectedLinks)
                    l.Value += diff;
            }
            _modelPanel.Invalidate();
            UpdatePropPanels();
        }
        private void FinishHover() { _hovering = false; }
        private void BeginSelection(Vector3 point, bool inverse)
        {
            if (_selecting)
                return;

            _selectStart = _selectEnd = point;

            _selectEnd._z += SelectWidth;
            _selectStart._z -= SelectWidth;

            _selecting = true;
            _selectInverse = inverse;

            UpdateTools();
        }
        private void CancelSelection()
        {
            if (!_selecting)
                return;

            _selecting = false;
            _selectStart = _selectEnd = new Vector3(float.MaxValue);
            UpdateSelection(false);
            _modelPanel.Invalidate();
        }
        private void FinishSelection()
        {
            if (!_selecting)
                return;

            _selecting = false;
            UpdateSelection(true);
            _modelPanel.Invalidate();

            SelectionModified();

            //Selection Area Selected.
        }

        private void _modelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bool create = Control.ModifierKeys == Keys.Alt;
                bool add = Control.ModifierKeys == Keys.Shift;
                bool subtract = Control.ModifierKeys == Keys.Control;
                bool move = Control.ModifierKeys == (Keys.Control | Keys.Shift);

                float depth = _modelPanel.GetDepth(e.X, e.Y);
                Vector3 target = _modelPanel.CurrentViewport.UnProject(e.X, e.Y, depth);
                Vector2 point;

                if (!move && (depth < 1.0f))
                {
                    point = (Vector2)target;
                    
                    //Hit-detect points first
                    foreach (CollisionObject obj in _targetNode._objects)
                        if (obj._render)
                            foreach (CollisionLink p in obj._points)
                                if (p.Value.Contained(point, point, PointSelectRadius))
                                {
                                    if (create)
                                    {
                                        //Connect all selected links to point
                                        foreach (CollisionLink l in _selectedLinks)
                                            l.Connect(p);

                                        //Select point
                                        ClearSelection();
                                        p._highlight = true;
                                        _selectedLinks.Add(p);
                                        SelectionModified();

                                        _modelPanel.Invalidate();
                                        return;
                                    }

                                    if (subtract)
                                    {
                                        p._highlight = false;
                                        _selectedLinks.Remove(p);
                                        _modelPanel.Invalidate();
                                        SelectionModified();
                                    }
                                    else if (!_selectedLinks.Contains(p))
                                    {
                                        if (!add)
                                            ClearSelection();

                                        _selectedLinks.Add(p);
                                        p._highlight = true;
                                        _modelPanel.Invalidate();
                                        SelectionModified();
                                    }

                                    if ((!add) && (!subtract))
                                        BeginHover(target);
                                    //Single Link Selected
                                    return;
                                }

                    float dist;
                    float bestDist = float.MaxValue;
                    CollisionPlane bestMatch = null;

                    //Hit-detect planes finding best match
                    foreach (CollisionObject obj in _targetNode._objects)
                        if (obj._render)
                            foreach (CollisionPlane p in obj._planes)
                                if (point.Contained(p.PointLeft, p.PointRight, PointSelectRadius))
                                {
                                    dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) - p.PointLeft.TrueDistance(p.PointRight);
                                    if (dist < bestDist)
                                    { bestDist = dist; bestMatch = p; }
                                }

                    if (bestMatch != null)
                    {
                        if (create)
                        {
                            ClearSelection();

                            _selectedLinks.Add(bestMatch.Split(point));
                            _selectedLinks[0]._highlight = true;
                            SelectionModified();
                            _modelPanel.Invalidate();

                            _creating = true;
                            BeginHover(target);

                            return;
                        }

                        if (subtract)
                        {
                            _selectedLinks.Remove(bestMatch._linkLeft);
                            _selectedLinks.Remove(bestMatch._linkRight);
                            bestMatch._linkLeft._highlight = bestMatch._linkRight._highlight = false;
                            _modelPanel.Invalidate();

                            SelectionModified();
                            return;
                        }

                        //Select both points
                        if (!_selectedLinks.Contains(bestMatch._linkLeft) || !_selectedLinks.Contains(bestMatch._linkRight))
                        {
                            if (!add)
                                ClearSelection();

                            _selectedLinks.Add(bestMatch._linkLeft);
                            _selectedLinks.Add(bestMatch._linkRight);
                            bestMatch._linkLeft._highlight = bestMatch._linkRight._highlight = true;
                            _modelPanel.Invalidate();

                            SelectionModified();
                        }

                        if (!add)
                            BeginHover(target);
                        //Single Platform Selected;
                        return;
                    }
                }

                //Nothing found :(

                //Trace ray to Z axis
                target = Vector3.IntersectZ(target, _modelPanel.CurrentViewport.UnProject(e.X, e.Y, 0.0f), 0.0f);
                point = (Vector2)target;

                if (create)
                {
                    if (_selectedLinks.Count == 0)
                    {
                        if (_selectedObject == null)
                            return;

                        _creating = true;

                        //Create two points and hover
                        CollisionLink point1 = new CollisionLink(_selectedObject, point).Branch(point);

                        _selectedLinks.Add(point1);
                        point1._highlight = true;

                        SelectionModified();
                        BeginHover(target);
                        _modelPanel.Invalidate();
                        return;
                    }
                    else if (_selectedLinks.Count == 1)
                    {
                        //Create new plane extending to point
                        CollisionLink link = _selectedLinks[0];
                        _selectedLinks[0] = link.Branch((Vector2)target);
                        _selectedLinks[0]._highlight = true;
                        link._highlight = false;
                        SelectionModified();
                        _modelPanel.Invalidate();

                        //Hover new point so it can be moved
                        BeginHover(target);
                        return;
                    }
                    else
                    {
                        //Find two closest points and insert between
                        CollisionPlane bestMatch = null;
                        if (_selectedPlanes.Count == 1)
                            bestMatch = _selectedPlanes[0];
                        else
                        {
                            float dist;
                            float bestDist = float.MaxValue;

                            foreach (CollisionPlane p in _selectedPlanes)
                            {
                                dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) - p.PointLeft.TrueDistance(p.PointRight);
                                if (dist < bestDist)
                                { bestDist = dist; bestMatch = p; }
                            }
                        }

                        ClearSelection();

                        _selectedLinks.Add(bestMatch.Split(point));
                        _selectedLinks[0]._highlight = true;
                        SelectionModified();
                        _modelPanel.Invalidate();

                        _creating = true;
                        BeginHover(target);

                        return;
                    }
                }

                if (move)
                {
                    if (_selectedLinks.Count > 0)
                        BeginHover(target);
                    return;
                }

                if (!add && !subtract)
                    ClearSelection();

                BeginSelection(target, subtract);
            }
        }
        private void _modelPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (saveIndex - 1 > 0 && saveIndex - 1 < undoSaves.Count)
                    if (undoSaves[saveIndex - 1]._collisionLinks[0].Value.ToString() == undoSaves[saveIndex - 1]._linkVectors[0].ToString())//If equal to starting point, remove.
                    {
                        undoSaves.RemoveAt(saveIndex - 1);
                        saveIndex--;
                        if (saveIndex == 0)
                            btnUndo.Enabled = false;
                    }
                
                hasMoved = false;
                FinishSelection();
                FinishHover();
                UpdateTools();
            }
        }

        private void _modelPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selecting) //Selection Box
            {
                Vector3 ray1 = _modelPanel.CurrentViewport.UnProject(new Vector3(e.X, e.Y, 0.0f));
                Vector3 ray2 = _modelPanel.CurrentViewport.UnProject(new Vector3(e.X, e.Y, 1.0f));

                _selectEnd = Vector3.IntersectZ(ray1, ray2, 0.0f);
                _selectEnd._z += SelectWidth;

                //Update selection
                UpdateSelection(false);

                _modelPanel.Invalidate();
            }

            UpdateHover(e.X, e.Y);
        }

        private void _modelPanel_PreRender(object sender)
        {

        }

        private unsafe void _modelPanel_PostRender(object sender)
        {
            //Clear depth buffer so we can hit-detect
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            //Render objects
            if (_targetNode != null)
                _targetNode.Render();

            if (_modelPanel.RenderBones)
                foreach (IRenderedObject o in _modelPanel._renderList)
                    if (o is IModel)
                        ((IModel)o).RenderBones(_modelPanel.CurrentViewport);

            //Render selection box
            if (!_selecting)
                return;

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            //Draw lines
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Color4(0.0f, 0.0f, 1.0f, 0.5f);
            TKContext.DrawBox(_selectStart, _selectEnd);

            //Draw box
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Color4(1.0f, 1.0f, 0.0f, 0.2f);
            TKContext.DrawBox(_selectStart, _selectEnd);
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer();
            for (int i = _selectedLinks.Count; --i >= 0; )
                _selectedLinks[i].Split();
            ClearSelection();
            SelectionModified();
            _modelPanel.Invalidate();
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer();

            for (int i = 0; i < _selectedLinks.Count - 1; )
            {
                CollisionLink link = _selectedLinks[i++];
                Vector2 pos = link.Value;
                int count = 1;
                for (int x = i; x < _selectedLinks.Count;)
                {
                    if (link.Merge(_selectedLinks[x]))
                    {
                        pos += _selectedLinks[x].Value;
                        count++;
                        _selectedLinks.RemoveAt(x);
                    }
                    else
                        x++;
                }
                link.Value = pos / count;
            }
            _modelPanel.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e) { _modelPanel.Invalidate(); }
        private void btnResetRot_Click(object sender, EventArgs e) { trackBar1.Value = 0; _modelPanel.Invalidate(); }
        private void btnResetCam_Click(object sender, EventArgs e) { _modelPanel.ResetCamera(); }
        
        // StageBox Perspective viewer
        private void btnPerspectiveCam_Click(object sender, EventArgs e) { 
            _modelPanel.ResetCamera();
            _modelPanel.CurrentViewport.ViewType = ViewportProjection.Perspective;
        }
        
        // StageBox Orthographic viewer
        private void btnOrthographicCam_Click(object sender, EventArgs e) { 
            _modelPanel.ResetCamera();
            _modelPanel.CurrentViewport.ViewType = ViewportProjection.Orthographic;
        }

        private void _modelPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_hovering)
                    CancelHover();
                else if (_selecting)
                    CancelSelection();
                else
                {
                    ClearSelection();
                    _modelPanel.Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (_selectedPlanes.Count > 0)
                {
                    foreach (CollisionPlane plane in _selectedPlanes)
                        plane.Delete();
                    TargetNode.SignalPropertyChange();
                }
                else if (_selectedLinks.Count == 1)
                    _selectedLinks[0].Pop();

                ClearSelection();
                SelectionModified();
                _modelPanel.Invalidate();
            }
            else if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.Z)
                {
                    if (_hovering)
                        CancelHover();
                    else if (btnUndo.Enabled)
                        Undo(this, null);
                }
                else if (e.KeyCode == Keys.Y)
                {
                    if (_hovering)
                        CancelHover();
                    else if (btnRedo.Enabled)
                        Redo(this, null);
                }
            }
            else if (e.KeyCode == Keys.OemOpenBrackets)
            {
                CollisionLink link = null;
                bool two = false;

                if (_selectedPlanes.Count == 1)
                {
                    link = _selectedPlanes[0]._linkLeft;
                    two = true;
                }
                else if (_selectedLinks.Count == 1)
                    link = _selectedLinks[0];

                if (link != null)
                    foreach (CollisionPlane p in link._members)
                        if (p._linkRight == link)
                        {
                            ClearSelection();

                            _selectedLinks.Add(p._linkLeft);
                            p._linkLeft._highlight = true;
                            if (two)
                            {
                                _selectedLinks.Add(p._linkRight);
                                p._linkRight._highlight = true;
                            }

                            SelectionModified();
                            _modelPanel.Invalidate();
                            break;
                        }
            }
            else if (e.KeyCode == Keys.OemCloseBrackets)
            {
                CollisionLink link = null;
                bool two = false;

                if (_selectedPlanes.Count == 1)
                {
                    link = _selectedPlanes[0]._linkRight;
                    two = true;
                }
                else if (_selectedLinks.Count == 1)
                    link = _selectedLinks[0];

                if (link != null)
                    foreach (CollisionPlane p in link._members)
                        if (p._linkLeft == link)
                        {
                            ClearSelection();

                            _selectedLinks.Add(p._linkRight);
                            p._linkRight._highlight = true;
                            if (two)
                            {
                                _selectedLinks.Add(p._linkLeft);
                                p._linkLeft._highlight = true;
                            }
                            SelectionModified();

                            _modelPanel.Invalidate();
                            break;
                        }
            }
            else if (e.KeyCode == Keys.W)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._y += amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.S)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._y -= amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.A)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._x -= amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.D)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._x += amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
        }

        #region Plane Properties

        private void cboMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            foreach (CollisionPlane plane in _selectedPlanes)
                plane._material = (CollisionPlaneMaterial)cboMaterial.SelectedItem;
            TargetNode.SignalPropertyChange();
        }
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            foreach (CollisionPlane plane in _selectedPlanes)
                plane.Type = (CollisionPlaneType)cboType.SelectedItem;
            TargetNode.SignalPropertyChange();
        }

        private void chkTypeCharacters_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsCharacters = chkTypeCharacters.Checked; }
        private void chkTypeItems_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsItems = chkTypeItems.Checked; }
        private void chkTypePokemonTrainer_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsPokemonTrainer = chkTypePokemonTrainer.Checked; }
        private void chkTypeRotating_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsRotating = chkTypeRotating.Checked; }

        private void chkFallThrough_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsFallThrough = chkFallThrough.Checked; }
        private void chkLeftLedge_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsLeftLedge = chkLeftLedge.Checked; }
        private void chkRightLedge_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsRightLedge = chkRightLedge.Checked; }
        private void chkNoWalljump_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsNoWalljump = chkNoWalljump.Checked; }

        private void chkFlagUnknown1_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownStageBox = chkFlagUnknown1.Checked; }
        private void chkFlagUnknown2_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownFlag1 = chkFlagUnknown2.Checked; }
        private void chkFlagUnknown3_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownFlag3 = chkFlagUnknown3.Checked; }
        private void chkFlagUnknown4_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownFlag4 = chkFlagUnknown4.Checked; }

        #endregion

        #region Point Properties

        private void numX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
                {
                    link._rawValue._x = numX.Value;
                } else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(numX.Value, oldValue._y);
                }
            }
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        private void numY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
                {
                    link._rawValue._y = numY.Value;
                } else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(oldValue._x, numY.Value);
                }
            }
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        #endregion

        private void btnSameX_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
                _selectedLinks[i]._rawValue._x = _selectedLinks[0]._rawValue._x;
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        private void btnSameY_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
                _selectedLinks[i]._rawValue._y = _selectedLinks[0]._rawValue._y;
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        private void chkPoly_CheckStateChanged(object sender, EventArgs e)
        {
            _modelPanel.BeginUpdate();
            _modelPanel.RenderPolygons = chkPoly.CheckState == CheckState.Checked;
            _modelPanel.RenderWireframe = chkPoly.CheckState == CheckState.Indeterminate;
            _modelPanel.EndUpdate();
        }

        private void chkBones_CheckedChanged(object sender, EventArgs e)
        {
            _modelPanel.RenderBones = chkBones.Checked;
        }

        private void modelTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is MDL0Node)
            {
                ((MDL0Node)e.Node.Tag).IsRendering = e.Node.Checked;
                if (!_updating)
                {
                    _updating = true;
                    foreach (TreeNode n in e.Node.Nodes)
                        n.Checked = e.Node.Checked;
                    _updating = false;
                }
            }
            else if (e.Node.Tag is MDL0BoneNode)
                ((MDL0BoneNode)e.Node.Tag)._render = e.Node.Checked;

            if (!_updating)
                _modelPanel.Invalidate();
        }

        private void modelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Tag is MDL0BoneNode)
                {
                    MDL0BoneNode bone = e.Node.Tag as MDL0BoneNode;
                    bone._boneColor = Color.FromArgb(255, 0, 0);
                    bone._nodeColor = Color.FromArgb(255, 128, 0);
                    _modelPanel.Invalidate();
                }
            }
        }

        private void modelTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (node != null)
            {
                if (node.Tag is MDL0BoneNode)
                {
                    MDL0BoneNode bone = node.Tag as MDL0BoneNode;
                    bone._nodeColor = bone._boneColor = Color.Transparent;
                    _modelPanel.Invalidate();
                }
            }
        }

        private void btnRelink_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if ((_selectedObject == null) || (node == null) || !(node.Tag is MDL0BoneNode))
                return;

            txtBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = ((MDL0BoneNode)node.Tag);
            txtModel.Text = _selectedObject._modelName = node.Parent.Text;
            TargetNode.SignalPropertyChange();
        }

        private void btnUnlink_Click(object sender, EventArgs e)
        {
            txtBone.Text = "";
            txtModel.Text = "";
            _selectedObject.LinkedBone = null;
            TargetNode.SignalPropertyChange();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if ((modelTree.SelectedNode == null) || !(modelTree.SelectedNode.Tag is MDL0BoneNode))
                e.Cancel = true;
        }

        private void snapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if ((node == null) || !(node.Tag is MDL0BoneNode))
                return;

            _snapMatrix = ((MDL0BoneNode)node.Tag)._inverseBindMatrix;
            _modelPanel.Invalidate();
        }

        private void btnResetSnap_Click(object sender, EventArgs e)
        {
            _snapMatrix = Matrix.Identity;
            _modelPanel.Invalidate();
        }

        private void CreateUndo()
        {
            CheckSaveIndex();
            if (undoSaves.Count > saveIndex)
            {
                undoSaves.RemoveRange(saveIndex, undoSaves.Count - saveIndex);
                redoSaves.Clear();
            }

            save = new CollisionState();
            save._collisionLinks = new List<CollisionLink>();
            save._linkVectors = new List<Vector2>();

            foreach (CollisionLink l in _selectedLinks)
            { save._collisionLinks.Add(l); save._linkVectors.Add(l.Value); }

            undoSaves.Add(save);
            btnUndo.Enabled = true;
            saveIndex++;
            save = null; 
        }

        private void CheckSaveIndex()
        {
            if (saveIndex < 0)
            { saveIndex = 0; }

            if (undoSaves.Count > 25)
            { undoSaves.RemoveAt(0); saveIndex--; }
        }

        private void ClearUndoBuffer()
        {
            saveIndex = 0;
            undoSaves.Clear();
            redoSaves.Clear();
            btnUndo.Enabled = btnRedo.Enabled = false;
        }

        private void Undo(object sender, EventArgs e)
        {
            _selectedLinks.Clear();

            save = new CollisionState();

            if (undoSaves[saveIndex - 1]._linkVectors != null)     //XY Positions changed.
            {
                save._collisionLinks = new List<CollisionLink>();
                save._linkVectors = new List<Vector2>();

                for (int i = 0; i < undoSaves[saveIndex - 1]._collisionLinks.Count; i++)
                {
                    _selectedLinks.Add(undoSaves[saveIndex - 1]._collisionLinks[i]);
                    save._collisionLinks.Add(undoSaves[saveIndex - 1]._collisionLinks[i]);
                    save._linkVectors.Add(undoSaves[saveIndex - 1]._collisionLinks[i].Value);
                    _selectedLinks[i].Value = undoSaves[saveIndex - 1]._linkVectors[i];
                }
            }

            saveIndex--;
            CheckSaveIndex();

            if (saveIndex == 0)
            { btnUndo.Enabled = false; }
            btnRedo.Enabled = true;

            redoSaves.Add(save);
            save = null;
            
            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        private void Redo(object sender, EventArgs e)
        {
            _selectedLinks.Clear();

            for (int i = 0; i < redoSaves[undoSaves.Count - saveIndex - 1]._collisionLinks.Count; i++)
            {
                _selectedLinks.Add(redoSaves[undoSaves.Count - saveIndex - 1]._collisionLinks[i]);
                _selectedLinks[i].Value = redoSaves[undoSaves.Count - saveIndex - 1]._linkVectors[i];
            }

            redoSaves.RemoveAt(undoSaves.Count - saveIndex - 1);
            saveIndex++;

            if (redoSaves.Count == 0)
            { btnRedo.Enabled = false; }
            btnUndo.Enabled = true;

            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        private void chkObjUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating) return;
            _selectedObject._flags[0] = chkObjUnk.Checked;
            TargetNode.SignalPropertyChange();
        }

        private void chkObjIndep_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkObjModule_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating) return;
            _selectedObject._flags[2] = chkObjModule.Checked;
            TargetNode.SignalPropertyChange();
        }

        private void chkObjSSEUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating) return;
            _selectedObject._flags[3] = chkObjSSEUnk.Checked;
            TargetNode.SignalPropertyChange();
        }

        private void btnPlayAnims_Click(object sender, EventArgs e)
        {

        }

        private void btnPrevFrame_Click(object sender, EventArgs e)
        {

        }

        private void btnNextFrame_Click(object sender, EventArgs e)
        {

        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            new ModelViewerHelp().Show(this, true);
        }

        private void btnTranslateAll_Click(object sender, EventArgs e) {
            if (_selectedLinks.Count == 0) {
                MessageBox.Show("You must select at least one collision link.");
                return;
            }
            using (TransformAttributesForm f = new TransformAttributesForm()) {
                f.TwoDimensional = true;
                if (f.ShowDialog() == DialogResult.OK) {
                    Matrix m = f.GetMatrix();
                    foreach (var link in _selectedLinks) {
                        link.Value = m * link.Value;
                    }
                    TargetNode.SignalPropertyChange();
                    _modelPanel.Invalidate();
                }
            }
        }
    }
}

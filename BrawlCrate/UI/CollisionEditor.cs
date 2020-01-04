using BrawlCrate.UI.Model_Previewer;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public unsafe class CollisionEditor : UserControl
    {
        protected virtual bool ErrorChecking => true;

        #region Designer

        public ModelPanel _modelPanel;
        protected SplitContainer undoToolStrip;
        protected SplitContainer redoToolStrip;
        protected CheckBox chkAllModels;
        protected Panel pnlPlaneProps;
        protected Label label5;
        protected Label labelType;
        protected ComboBox cboMaterial;
        protected Panel pnlObjProps;
        protected ToolStrip toolStrip1;
        protected ToolStripButton btnTransform;
        protected ToolStripButton btnSplit;
        protected ToolStripButton btnMerge;
        protected ToolStripButton btnDelete;
        protected ContextMenuStrip contextMenuStrip1;
        protected ContextMenuStrip contextMenuStrip3;
        private IContainer components;
        protected ToolStripMenuItem snapToolStripMenuItem;
        protected Panel panel1;
        protected TrackBar trackBar1;
        protected Button btnResetRot;
        protected ToolStripButton btnResetCam;
        protected GroupBox groupBoxFlags1;
        protected CheckBox chkFallThrough;
        protected GroupBox groupBoxFlags2;
        protected CheckBox chkNoWalljump;
        protected CheckBox chkRightLedge;
        protected CheckBox chkTypeCharacters;
        protected CheckBox chkTypeItems;
        protected CheckBox chkTypePokemonTrainer;
        protected CheckBox chkTypeRotating;

        // Advanced unknown flags
        protected GroupBox groupBoxTargets;
        protected CheckBox chkFlagUnknown1;
        protected CheckBox chkFlagUnknown2;
        protected CheckBox chkFlagUnknown3;
        protected CheckBox chkFlagUnknown4;

        protected Panel pnlPointProps;
        protected NumericInputBox numX;
        protected Label label2;
        protected NumericInputBox numY;
        protected Label label1;
        protected ToolStripSeparator toolStripSeparator1;
        protected ToolStripButton btnSameX;
        protected ToolStripButton btnSameY;
        protected ToolStripMenuItem newObjectToolStripMenuItem;
        protected ToolStripSeparator toolStripMenuItem2;
        protected ToolStripSeparator toolStripMenuItem3;
        protected ToolStripSeparator assignSeperatorToolStripMenuItem;
        protected ToolStripSeparator toolStripMenuItem1;
        protected ToolStripMenuItem _deleteToolStripMenuItem;
        protected TextBox txtModel;
        protected Label label3;
        protected Panel panel2;
        protected CheckBox chkPoly;
        protected Button btnRelink;
        protected TextBox txtBone;
        protected Label label4;
        protected CheckBox chkBones;
        protected CheckBox chkLeftLedge;
        protected ComboBox cboType;
        protected TreeView modelTree;
        protected Button btnUnlink;
        protected ContextMenuStrip contextMenuStrip2;
        protected ToolStripMenuItem assignToolStripMenuItem;
        protected ToolStripMenuItem assignNoMoveToolStripMenuItem;
        protected ToolStripMenuItem unlinkToolStripMenuItem;
        protected ToolStripMenuItem unlinkNoMoveToolStripMenuItem;
        protected ToolStripMenuItem snapToolStripMenuItem1;
        protected ToolStripSeparator toolStripSeparator2;
        protected ToolStripButton btnResetSnap;
        protected ToolStripButton btnUndo;
        protected ToolStripButton btnRedo;
        protected ToolStripSeparator toolStripSeparator3;
        protected CheckBox chkObjModule;
        protected CheckBox chkObjUnk;
        protected CheckBox chkObjSSEUnk;
        protected Button btnPlayAnims;
        protected Panel panel4;
        protected Panel panel3;
        protected Button btnPrevFrame;
        protected Button btnNextFrame;
        protected ToolStripButton btnHelp;
        protected CheckedListBox lstObjects;

        // BrawlCrate buttons
        protected ToolStripSeparator toolStripSeparatorCamera; // Seperator for Camera controls
        protected ToolStripButton btnPerspectiveCam;           // Goes into perspective mode
        protected ToolStripButton btnFlipColl;
        protected ToolStripButton btnOrthographicCam; // Goes into orthographic mode
        protected ToolStripButton btnBoundaries;
        protected ToolStripButton btnSpawns;
        protected ToolStripButton btnItems;
        private ToolStripMenuItem moveToNewObjectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem splitToolStripMenuItem;
        private ToolStripMenuItem mergeToolStripMenuItem;
        private ToolStripMenuItem flipToolStripMenuItem;
        private ToolStripMenuItem _deleteToolStripMenuItem1;
        private ToolStripMenuItem transformToolStripMenuItem;
        private ToolStripMenuItem alignXToolStripMenuItem;
        private ToolStripMenuItem alignYToolStripMenuItem;
        protected ToolStripSeparator toolStripSeparatorOverlays; // Seperator for Overlay controls

        protected void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CollisionEditor));
            undoToolStrip = new SplitContainer();
            redoToolStrip = new SplitContainer();
            modelTree = new TreeView();
            contextMenuStrip2 = new ContextMenuStrip(components);
            assignToolStripMenuItem = new ToolStripMenuItem();
            assignNoMoveToolStripMenuItem = new ToolStripMenuItem();
            assignSeperatorToolStripMenuItem = new ToolStripSeparator();
            snapToolStripMenuItem1 = new ToolStripMenuItem();
            panel2 = new Panel();
            chkBones = new CheckBox();
            chkPoly = new CheckBox();
            chkAllModels = new CheckBox();
            lstObjects = new CheckedListBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            newObjectToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            unlinkToolStripMenuItem = new ToolStripMenuItem();
            unlinkNoMoveToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            snapToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            _deleteToolStripMenuItem = new ToolStripMenuItem();
            panel3 = new Panel();
            pnlPlaneProps = new Panel();
            groupBoxFlags2 = new GroupBox();
            chkFlagUnknown1 = new CheckBox();
            chkFlagUnknown2 = new CheckBox();
            chkFlagUnknown3 = new CheckBox();
            chkFlagUnknown4 = new CheckBox();
            groupBoxFlags1 = new GroupBox();
            chkLeftLedge = new CheckBox();
            chkNoWalljump = new CheckBox();
            chkRightLedge = new CheckBox();
            chkTypeRotating = new CheckBox();
            chkFallThrough = new CheckBox();
            groupBoxTargets = new GroupBox();
            chkTypePokemonTrainer = new CheckBox();
            chkTypeItems = new CheckBox();
            chkTypeCharacters = new CheckBox();
            cboMaterial = new ComboBox();
            cboType = new ComboBox();
            label5 = new Label();
            labelType = new Label();
            pnlPointProps = new Panel();
            label2 = new Label();
            numY = new NumericInputBox();
            label1 = new Label();
            numX = new NumericInputBox();
            pnlObjProps = new Panel();
            chkObjSSEUnk = new CheckBox();
            chkObjModule = new CheckBox();
            chkObjUnk = new CheckBox();
            btnUnlink = new Button();
            btnRelink = new Button();
            txtBone = new TextBox();
            label4 = new Label();
            txtModel = new TextBox();
            label3 = new Label();
            panel4 = new Panel();
            btnPlayAnims = new Button();
            btnPrevFrame = new Button();
            btnNextFrame = new Button();
            _modelPanel = new ModelPanel();
            panel1 = new Panel();
            toolStrip1 = new ToolStrip();
            btnUndo = new ToolStripButton();
            btnRedo = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            btnTransform = new ToolStripButton();
            btnSplit = new ToolStripButton();
            btnMerge = new ToolStripButton();
            btnFlipColl = new ToolStripButton();
            btnDelete = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnSameX = new ToolStripButton();
            btnSameY = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnPerspectiveCam = new ToolStripButton();
            btnOrthographicCam = new ToolStripButton();
            btnResetCam = new ToolStripButton();
            toolStripSeparatorCamera = new ToolStripSeparator();
            btnSpawns = new ToolStripButton();
            btnItems = new ToolStripButton();
            btnBoundaries = new ToolStripButton();
            toolStripSeparatorOverlays = new ToolStripSeparator();
            btnResetSnap = new ToolStripButton();
            btnHelp = new ToolStripButton();
            btnResetRot = new Button();
            trackBar1 = new TrackBar();
            contextMenuStrip3 = new ContextMenuStrip(components);
            moveToNewObjectToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            splitToolStripMenuItem = new ToolStripMenuItem();
            mergeToolStripMenuItem = new ToolStripMenuItem();
            flipToolStripMenuItem = new ToolStripMenuItem();
            _deleteToolStripMenuItem1 = new ToolStripMenuItem();
            transformToolStripMenuItem = new ToolStripMenuItem();
            alignXToolStripMenuItem = new ToolStripMenuItem();
            alignYToolStripMenuItem = new ToolStripMenuItem();
            ((ISupportInitialize) undoToolStrip).BeginInit();
            undoToolStrip.Panel1.SuspendLayout();
            undoToolStrip.Panel2.SuspendLayout();
            undoToolStrip.SuspendLayout();
            ((ISupportInitialize) redoToolStrip).BeginInit();
            redoToolStrip.Panel1.SuspendLayout();
            redoToolStrip.Panel2.SuspendLayout();
            redoToolStrip.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            panel2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel3.SuspendLayout();
            pnlPlaneProps.SuspendLayout();
            groupBoxFlags2.SuspendLayout();
            groupBoxFlags1.SuspendLayout();
            groupBoxTargets.SuspendLayout();
            pnlPointProps.SuspendLayout();
            pnlObjProps.SuspendLayout();
            panel4.SuspendLayout();
            panel1.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((ISupportInitialize) trackBar1).BeginInit();
            contextMenuStrip3.SuspendLayout();
            SuspendLayout();
            // 
            // undoToolStrip
            // 
            undoToolStrip.Dock = DockStyle.Fill;
            undoToolStrip.FixedPanel = FixedPanel.Panel1;
            undoToolStrip.Location = new System.Drawing.Point(0, 0);
            undoToolStrip.Name = "undoToolStrip";
            // 
            // undoToolStrip.Panel1
            // 
            undoToolStrip.Panel1.Controls.Add(redoToolStrip);
            // 
            // undoToolStrip.Panel2
            // 
            undoToolStrip.Panel2.Controls.Add(_modelPanel);
            undoToolStrip.Panel2.Controls.Add(panel1);
            undoToolStrip.Size = new System.Drawing.Size(694, 467);
            undoToolStrip.SplitterDistance = 209;
            undoToolStrip.TabIndex = 1;
            // 
            // redoToolStrip
            // 
            redoToolStrip.Dock = DockStyle.Fill;
            redoToolStrip.Location = new System.Drawing.Point(0, 0);
            redoToolStrip.Name = "redoToolStrip";
            redoToolStrip.Orientation = Orientation.Horizontal;
            // 
            // redoToolStrip.Panel1
            // 
            redoToolStrip.Panel1.Controls.Add(modelTree);
            redoToolStrip.Panel1.Controls.Add(panel2);
            // 
            // redoToolStrip.Panel2
            // 
            redoToolStrip.Panel2.Controls.Add(lstObjects);
            redoToolStrip.Panel2.Controls.Add(panel3);
            redoToolStrip.Panel2.Controls.Add(panel4);
            redoToolStrip.Size = new System.Drawing.Size(209, 467);
            redoToolStrip.SplitterDistance = 242;
            redoToolStrip.TabIndex = 2;
            // 
            // modelTree
            // 
            modelTree.BorderStyle = BorderStyle.None;
            modelTree.CheckBoxes = true;
            modelTree.ContextMenuStrip = contextMenuStrip2;
            modelTree.Dock = DockStyle.Fill;
            modelTree.HideSelection = false;
            modelTree.Location = new System.Drawing.Point(0, 17);
            modelTree.Name = "modelTree";
            modelTree.Size = new System.Drawing.Size(209, 225);
            modelTree.TabIndex = 4;
            modelTree.AfterCheck += new TreeViewEventHandler(modelTree_AfterCheck);
            modelTree.BeforeSelect += new TreeViewCancelEventHandler(modelTree_BeforeSelect);
            modelTree.AfterSelect += new TreeViewEventHandler(modelTree_AfterSelect);
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Items.AddRange(new ToolStripItem[]
            {
                assignToolStripMenuItem,
                assignNoMoveToolStripMenuItem,
                assignSeperatorToolStripMenuItem,
                snapToolStripMenuItem1
            });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new System.Drawing.Size(239, 76);
            contextMenuStrip2.Opening += new CancelEventHandler(contextMenuStrip2_Opening);
            // 
            // assignToolStripMenuItem
            // 
            assignToolStripMenuItem.Name = "assignToolStripMenuItem";
            assignToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            assignToolStripMenuItem.Text = "Assign";
            assignToolStripMenuItem.Click += new EventHandler(btnRelink_Click);
            // 
            // assignNoMoveToolStripMenuItem
            // 
            assignNoMoveToolStripMenuItem.Name = "assignNoMoveToolStripMenuItem";
            assignNoMoveToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            assignNoMoveToolStripMenuItem.Text = "Assign (No relative movement)";
            assignNoMoveToolStripMenuItem.Click += new EventHandler(btnRelinkNoMove_Click);
            // 
            // assignSeperatorToolStripMenuItem
            // 
            assignSeperatorToolStripMenuItem.Name = "assignSeperatorToolStripMenuItem";
            assignSeperatorToolStripMenuItem.Size = new System.Drawing.Size(235, 6);
            // 
            // snapToolStripMenuItem1
            // 
            snapToolStripMenuItem1.Name = "snapToolStripMenuItem1";
            snapToolStripMenuItem1.Size = new System.Drawing.Size(238, 22);
            snapToolStripMenuItem1.Text = "Snap";
            snapToolStripMenuItem1.Click += new EventHandler(snapToolStripMenuItem1_Click);
            // 
            // panel2
            // 
            panel2.Controls.Add(chkBones);
            panel2.Controls.Add(chkPoly);
            panel2.Controls.Add(chkAllModels);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(209, 17);
            panel2.TabIndex = 3;
            // 
            // chkBones
            // 
            chkBones.Location = new System.Drawing.Point(100, 0);
            chkBones.Name = "chkBones";
            chkBones.Padding = new Padding(1, 0, 0, 0);
            chkBones.Size = new System.Drawing.Size(67, 17);
            chkBones.TabIndex = 4;
            chkBones.Text = "Bones";
            chkBones.UseVisualStyleBackColor = true;
            chkBones.CheckedChanged += new EventHandler(chkBones_CheckedChanged);
            // 
            // chkPoly
            // 
            chkPoly.Checked = true;
            chkPoly.CheckState = CheckState.Checked;
            chkPoly.Location = new System.Drawing.Point(44, 0);
            chkPoly.Name = "chkPoly";
            chkPoly.Padding = new Padding(1, 0, 0, 0);
            chkPoly.Size = new System.Drawing.Size(54, 17);
            chkPoly.TabIndex = 3;
            chkPoly.Text = "Poly";
            chkPoly.ThreeState = true;
            chkPoly.UseVisualStyleBackColor = true;
            chkPoly.CheckStateChanged += new EventHandler(chkPoly_CheckStateChanged);
            // 
            // chkAllModels
            // 
            chkAllModels.Checked = true;
            chkAllModels.CheckState = CheckState.Checked;
            chkAllModels.Location = new System.Drawing.Point(0, 0);
            chkAllModels.Name = "chkAllModels";
            chkAllModels.Padding = new Padding(1, 0, 0, 0);
            chkAllModels.Size = new System.Drawing.Size(41, 17);
            chkAllModels.TabIndex = 2;
            chkAllModels.Text = "All";
            chkAllModels.UseVisualStyleBackColor = true;
            chkAllModels.CheckedChanged += new EventHandler(chkAllModels_CheckedChanged);
            // 
            // lstObjects
            // 
            lstObjects.BorderStyle = BorderStyle.None;
            lstObjects.ContextMenuStrip = contextMenuStrip1;
            lstObjects.Dock = DockStyle.Fill;
            lstObjects.FormattingEnabled = true;
            lstObjects.IntegralHeight = false;
            lstObjects.Location = new System.Drawing.Point(0, 0);
            lstObjects.Name = "lstObjects";
            lstObjects.Size = new System.Drawing.Size(209, 82);
            lstObjects.TabIndex = 1;
            lstObjects.ItemCheck += new ItemCheckEventHandler(lstObjects_ItemCheck);
            lstObjects.SelectedValueChanged += new EventHandler(lstObjects_SelectedValueChanged);
            lstObjects.MouseDown += new MouseEventHandler(lstObjects_MouseDown);
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[]
            {
                newObjectToolStripMenuItem,
                toolStripMenuItem3,
                unlinkToolStripMenuItem,
                unlinkNoMoveToolStripMenuItem,
                toolStripMenuItem2,
                snapToolStripMenuItem,
                toolStripMenuItem1,
                _deleteToolStripMenuItem
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(238, 132);
            contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);
            // 
            // newObjectToolStripMenuItem
            // 
            newObjectToolStripMenuItem.Name = "newObjectToolStripMenuItem";
            newObjectToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            newObjectToolStripMenuItem.Text = "New Object";
            newObjectToolStripMenuItem.Click += new EventHandler(newObjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(234, 6);
            // 
            // unlinkToolStripMenuItem
            // 
            unlinkToolStripMenuItem.Name = "unlinkToolStripMenuItem";
            unlinkToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            unlinkToolStripMenuItem.Text = "Unlink";
            unlinkToolStripMenuItem.Click += new EventHandler(btnUnlink_Click);
            // 
            // unlinkNoMoveToolStripMenuItem
            // 
            unlinkNoMoveToolStripMenuItem.Name = "unlinkNoMoveToolStripMenuItem";
            unlinkNoMoveToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            unlinkNoMoveToolStripMenuItem.Text = "Unlink (No relative movement)";
            unlinkNoMoveToolStripMenuItem.Click += new EventHandler(btnUnlinkNoMove_Click);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(234, 6);
            // 
            // snapToolStripMenuItem
            // 
            snapToolStripMenuItem.Name = "snapToolStripMenuItem";
            snapToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            snapToolStripMenuItem.Text = "Snap";
            snapToolStripMenuItem.Click += new EventHandler(snapToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(234, 6);
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Delete;
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            _deleteToolStripMenuItem.Text = "Delete";
            _deleteToolStripMenuItem.Click += new EventHandler(_deleteToolStripMenuItem_Click);
            // 
            // panel3
            // 
            panel3.Controls.Add(pnlPlaneProps);
            panel3.Controls.Add(pnlPointProps);
            panel3.Controls.Add(pnlObjProps);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new System.Drawing.Point(0, 82);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(209, 115);
            panel3.TabIndex = 16;
            // 
            // pnlPlaneProps
            // 
            pnlPlaneProps.Controls.Add(groupBoxFlags2);
            pnlPlaneProps.Controls.Add(groupBoxFlags1);
            pnlPlaneProps.Controls.Add(groupBoxTargets);
            pnlPlaneProps.Controls.Add(cboMaterial);
            pnlPlaneProps.Controls.Add(cboType);
            pnlPlaneProps.Controls.Add(label5);
            pnlPlaneProps.Controls.Add(labelType);
            pnlPlaneProps.Dock = DockStyle.Bottom;
            pnlPlaneProps.Location = new System.Drawing.Point(0, -273);
            pnlPlaneProps.Name = "pnlPlaneProps";
            pnlPlaneProps.Size = new System.Drawing.Size(209, 188);
            pnlPlaneProps.TabIndex = 0;
            pnlPlaneProps.Visible = false;
            // 
            // groupBoxFlags2
            // 
            groupBoxFlags2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                     | AnchorStyles.Left;
            groupBoxFlags2.Controls.Add(chkFlagUnknown1);
            groupBoxFlags2.Controls.Add(chkFlagUnknown2);
            groupBoxFlags2.Controls.Add(chkFlagUnknown3);
            groupBoxFlags2.Controls.Add(chkFlagUnknown4);
            groupBoxFlags2.Location = new System.Drawing.Point(104, 102);
            groupBoxFlags2.Margin = new Padding(0);
            groupBoxFlags2.Name = "groupBoxFlags2";
            groupBoxFlags2.Padding = new Padding(0);
            groupBoxFlags2.Size = new System.Drawing.Size(105, 160);
            groupBoxFlags2.TabIndex = 14;
            groupBoxFlags2.TabStop = false;
            // 
            // chkFlagUnknown1
            // 
            chkFlagUnknown1.Location = new System.Drawing.Point(8, 17);
            chkFlagUnknown1.Margin = new Padding(0);
            chkFlagUnknown1.Name = "chkFlagUnknown1";
            chkFlagUnknown1.Size = new System.Drawing.Size(86, 18);
            chkFlagUnknown1.TabIndex = 3;
            chkFlagUnknown1.Text = "Unknown 1";
            chkFlagUnknown1.UseVisualStyleBackColor = true;
            chkFlagUnknown1.CheckedChanged += new EventHandler(chkFlagUnknown1_CheckedChanged);
            // 
            // chkFlagUnknown2
            // 
            chkFlagUnknown2.Location = new System.Drawing.Point(8, 33);
            chkFlagUnknown2.Margin = new Padding(0);
            chkFlagUnknown2.Name = "chkFlagUnknown2";
            chkFlagUnknown2.Size = new System.Drawing.Size(86, 18);
            chkFlagUnknown2.TabIndex = 3;
            chkFlagUnknown2.Text = "Unknown 2";
            chkFlagUnknown2.UseVisualStyleBackColor = true;
            chkFlagUnknown2.CheckedChanged += new EventHandler(chkFlagUnknown2_CheckedChanged);
            // 
            // chkFlagUnknown3
            // 
            chkFlagUnknown3.Location = new System.Drawing.Point(8, 49);
            chkFlagUnknown3.Margin = new Padding(0);
            chkFlagUnknown3.Name = "chkFlagUnknown3";
            chkFlagUnknown3.Size = new System.Drawing.Size(86, 18);
            chkFlagUnknown3.TabIndex = 3;
            chkFlagUnknown3.Text = "Unknown 3";
            chkFlagUnknown3.UseVisualStyleBackColor = true;
            chkFlagUnknown3.CheckedChanged += new EventHandler(chkFlagUnknown3_CheckedChanged);
            // 
            // chkFlagUnknown4
            // 
            chkFlagUnknown4.Location = new System.Drawing.Point(8, 65);
            chkFlagUnknown4.Margin = new Padding(0);
            chkFlagUnknown4.Name = "chkFlagUnknown4";
            chkFlagUnknown4.Size = new System.Drawing.Size(86, 18);
            chkFlagUnknown4.TabIndex = 3;
            chkFlagUnknown4.Text = "Unknown 4";
            chkFlagUnknown4.UseVisualStyleBackColor = true;
            chkFlagUnknown4.CheckedChanged += new EventHandler(chkFlagUnknown4_CheckedChanged);
            // 
            // groupBoxFlags1
            // 
            groupBoxFlags1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                     | AnchorStyles.Left;
            groupBoxFlags1.Controls.Add(chkLeftLedge);
            groupBoxFlags1.Controls.Add(chkNoWalljump);
            groupBoxFlags1.Controls.Add(chkRightLedge);
            groupBoxFlags1.Controls.Add(chkTypeRotating);
            groupBoxFlags1.Controls.Add(chkFallThrough);
            groupBoxFlags1.Location = new System.Drawing.Point(0, 102);
            groupBoxFlags1.Margin = new Padding(0);
            groupBoxFlags1.Name = "groupBoxFlags1";
            groupBoxFlags1.Padding = new Padding(0);
            groupBoxFlags1.Size = new System.Drawing.Size(104, 160);
            groupBoxFlags1.TabIndex = 13;
            groupBoxFlags1.TabStop = false;
            groupBoxFlags1.Text = "Flags";
            // 
            // chkLeftLedge
            // 
            chkLeftLedge.Location = new System.Drawing.Point(8, 33);
            chkLeftLedge.Margin = new Padding(0);
            chkLeftLedge.Name = "chkLeftLedge";
            chkLeftLedge.Size = new System.Drawing.Size(86, 18);
            chkLeftLedge.TabIndex = 4;
            chkLeftLedge.Text = "Left Ledge";
            chkLeftLedge.UseVisualStyleBackColor = true;
            chkLeftLedge.CheckedChanged += new EventHandler(chkLeftLedge_CheckedChanged);
            // 
            // chkNoWalljump
            // 
            chkNoWalljump.Location = new System.Drawing.Point(8, 65);
            chkNoWalljump.Margin = new Padding(0);
            chkNoWalljump.Name = "chkNoWalljump";
            chkNoWalljump.Size = new System.Drawing.Size(90, 18);
            chkNoWalljump.TabIndex = 2;
            chkNoWalljump.Text = "No Walljump";
            chkNoWalljump.UseVisualStyleBackColor = true;
            chkNoWalljump.CheckedChanged += new EventHandler(chkNoWalljump_CheckedChanged);
            // 
            // chkRightLedge
            // 
            chkRightLedge.Location = new System.Drawing.Point(8, 49);
            chkRightLedge.Margin = new Padding(0);
            chkRightLedge.Name = "chkRightLedge";
            chkRightLedge.Size = new System.Drawing.Size(86, 18);
            chkRightLedge.TabIndex = 1;
            chkRightLedge.Text = "Right Ledge";
            chkRightLedge.UseVisualStyleBackColor = true;
            chkRightLedge.CheckedChanged += new EventHandler(chkRightLedge_CheckedChanged);
            // 
            // chkTypeRotating
            // 
            chkTypeRotating.Location = new System.Drawing.Point(8, 81);
            chkTypeRotating.Margin = new Padding(0);
            chkTypeRotating.Name = "chkTypeRotating";
            chkTypeRotating.Size = new System.Drawing.Size(86, 18);
            chkTypeRotating.TabIndex = 4;
            chkTypeRotating.Text = "Rotating";
            chkTypeRotating.UseVisualStyleBackColor = true;
            chkTypeRotating.CheckedChanged += new EventHandler(chkTypeRotating_CheckedChanged);
            // 
            // chkFallThrough
            // 
            chkFallThrough.Location = new System.Drawing.Point(8, 17);
            chkFallThrough.Margin = new Padding(0);
            chkFallThrough.Name = "chkFallThrough";
            chkFallThrough.Size = new System.Drawing.Size(90, 18);
            chkFallThrough.TabIndex = 0;
            chkFallThrough.Text = "Fall-Through";
            chkFallThrough.UseVisualStyleBackColor = true;
            chkFallThrough.CheckedChanged += new EventHandler(chkFallThrough_CheckedChanged);
            // 
            // groupBoxTargets
            // 
            groupBoxTargets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                      | AnchorStyles.Left;
            groupBoxTargets.Controls.Add(chkTypePokemonTrainer);
            groupBoxTargets.Controls.Add(chkTypeItems);
            groupBoxTargets.Controls.Add(chkTypeCharacters);
            groupBoxTargets.Location = new System.Drawing.Point(0, 50);
            groupBoxTargets.Margin = new Padding(0);
            groupBoxTargets.Name = "groupBoxTargets";
            groupBoxTargets.Padding = new Padding(0);
            groupBoxTargets.Size = new System.Drawing.Size(208, 71);
            groupBoxTargets.TabIndex = 14;
            groupBoxTargets.TabStop = false;
            groupBoxTargets.Text = "Collision Targets";
            // 
            // chkTypePokemonTrainer
            // 
            chkTypePokemonTrainer.Location = new System.Drawing.Point(82, 33);
            chkTypePokemonTrainer.Margin = new Padding(0);
            chkTypePokemonTrainer.Name = "chkTypePokemonTrainer";
            chkTypePokemonTrainer.Size = new System.Drawing.Size(116, 18);
            chkTypePokemonTrainer.TabIndex = 3;
            chkTypePokemonTrainer.Text = "Pokémon Trainer";
            chkTypePokemonTrainer.UseVisualStyleBackColor = true;
            chkTypePokemonTrainer.CheckedChanged += new EventHandler(chkTypePokemonTrainer_CheckedChanged);
            // 
            // chkTypeItems
            // 
            chkTypeItems.Location = new System.Drawing.Point(8, 33);
            chkTypeItems.Margin = new Padding(0);
            chkTypeItems.Name = "chkTypeItems";
            chkTypeItems.Size = new System.Drawing.Size(86, 18);
            chkTypeItems.TabIndex = 3;
            chkTypeItems.Text = "Items";
            chkTypeItems.UseVisualStyleBackColor = true;
            chkTypeItems.CheckedChanged += new EventHandler(chkTypeItems_CheckedChanged);
            // 
            // chkTypeCharacters
            // 
            chkTypeCharacters.Location = new System.Drawing.Point(8, 17);
            chkTypeCharacters.Margin = new Padding(0);
            chkTypeCharacters.Name = "chkTypeCharacters";
            chkTypeCharacters.Size = new System.Drawing.Size(194, 18);
            chkTypeCharacters.TabIndex = 4;
            chkTypeCharacters.Text = "Everything";
            chkTypeCharacters.UseVisualStyleBackColor = true;
            chkTypeCharacters.CheckedChanged += new EventHandler(chkTypeCharacters_CheckedChanged);
            // 
            // cboMaterial
            // 
            cboMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaterial.FormattingEnabled = true;
            cboMaterial.Location = new System.Drawing.Point(66, 25);
            cboMaterial.Name = "cboMaterial";
            cboMaterial.Size = new System.Drawing.Size(139, 21);
            cboMaterial.TabIndex = 12;
            cboMaterial.SelectedIndexChanged += new EventHandler(cboMaterial_SelectedIndexChanged);
            cboMaterial.Anchor = AnchorStyles.Top | AnchorStyles.Right
                                                  | AnchorStyles.Left;
            // 
            // cboType
            // 
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.FormattingEnabled = true;
            cboType.Location = new System.Drawing.Point(66, 4);
            cboType.Name = "cboType";
            cboType.Size = new System.Drawing.Size(139, 21);
            cboType.TabIndex = 5;
            cboType.SelectedIndexChanged += new EventHandler(cboType_SelectedIndexChanged);
            cboType.Anchor = AnchorStyles.Top | AnchorStyles.Right
                                              | AnchorStyles.Left;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(7, 25);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(53, 21);
            label5.TabIndex = 8;
            label5.Text = "Material:";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelType
            // 
            labelType.Location = new System.Drawing.Point(7, 4);
            labelType.Name = "labelType";
            labelType.Size = new System.Drawing.Size(53, 21);
            labelType.TabIndex = 8;
            labelType.Text = "Type:";
            labelType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlPointProps
            // 
            pnlPointProps.Controls.Add(label2);
            pnlPointProps.Controls.Add(numY);
            pnlPointProps.Controls.Add(label1);
            pnlPointProps.Controls.Add(numX);
            pnlPointProps.Dock = DockStyle.Bottom;
            pnlPointProps.Location = new System.Drawing.Point(0, -85);
            pnlPointProps.Name = "pnlPointProps";
            pnlPointProps.Size = new System.Drawing.Size(209, 70);
            pnlPointProps.TabIndex = 15;
            pnlPointProps.Visible = false;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new System.Drawing.Point(18, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(42, 20);
            label2.TabIndex = 3;
            label2.Text = "Y";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numY
            // 
            numY.BorderStyle = BorderStyle.FixedSingle;
            numY.Integral = false;
            numY.Location = new System.Drawing.Point(59, 32);
            numY.MaximumValue = 3.402823E+38F;
            numY.MinimumValue = -3.402823E+38F;
            numY.Name = "numY";
            numY.Size = new System.Drawing.Size(100, 20);
            numY.TabIndex = 2;
            numY.Text = "0";
            numY.ValueChanged += new EventHandler(numY_ValueChanged);
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new System.Drawing.Point(18, 13);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(42, 20);
            label1.TabIndex = 1;
            label1.Text = "X";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numX
            // 
            numX.BorderStyle = BorderStyle.FixedSingle;
            numX.Integral = false;
            numX.Location = new System.Drawing.Point(59, 13);
            numX.MaximumValue = 3.402823E+38F;
            numX.MinimumValue = -3.402823E+38F;
            numX.Name = "numX";
            numX.Size = new System.Drawing.Size(100, 20);
            numX.TabIndex = 0;
            numX.Text = "0";
            numX.ValueChanged += new EventHandler(numX_ValueChanged);
            // 
            // pnlObjProps
            // 
            pnlObjProps.Controls.Add(chkObjSSEUnk);
            pnlObjProps.Controls.Add(chkObjModule);
            pnlObjProps.Controls.Add(chkObjUnk);
            pnlObjProps.Controls.Add(btnUnlink);
            pnlObjProps.Controls.Add(btnRelink);
            pnlObjProps.Controls.Add(txtBone);
            pnlObjProps.Controls.Add(label4);
            pnlObjProps.Controls.Add(txtModel);
            pnlObjProps.Controls.Add(label3);
            pnlObjProps.Dock = DockStyle.Bottom;
            pnlObjProps.Location = new System.Drawing.Point(0, -15);
            pnlObjProps.Name = "pnlObjProps";
            pnlObjProps.Size = new System.Drawing.Size(209, 130);
            pnlObjProps.TabIndex = 1;
            pnlObjProps.Visible = false;
            // 
            // chkObjSSEUnk
            // 
            chkObjSSEUnk.AutoSize = true;
            chkObjSSEUnk.Location = new System.Drawing.Point(10, 102);
            chkObjSSEUnk.Name = "chkObjSSEUnk";
            chkObjSSEUnk.Size = new System.Drawing.Size(96, 17);
            chkObjSSEUnk.TabIndex = 15;
            chkObjSSEUnk.Text = "SSE Unknown";
            chkObjSSEUnk.UseVisualStyleBackColor = true;
            chkObjSSEUnk.CheckedChanged += new EventHandler(chkObjSSEUnk_CheckedChanged);
            // 
            // chkObjModule
            // 
            chkObjModule.AutoSize = true;
            chkObjModule.Location = new System.Drawing.Point(10, 79);
            chkObjModule.Name = "chkObjModule";
            chkObjModule.Size = new System.Drawing.Size(111, 17);
            chkObjModule.TabIndex = 14;
            chkObjModule.Text = "Module Controlled";
            chkObjModule.UseVisualStyleBackColor = true;
            chkObjModule.CheckedChanged += new EventHandler(chkObjModule_CheckedChanged);
            // 
            // chkObjUnk
            // 
            chkObjUnk.AutoSize = true;
            chkObjUnk.Location = new System.Drawing.Point(10, 56);
            chkObjUnk.Name = "chkObjUnk";
            chkObjUnk.Size = new System.Drawing.Size(72, 17);
            chkObjUnk.TabIndex = 13;
            chkObjUnk.Text = "Unknown";
            chkObjUnk.UseVisualStyleBackColor = true;
            chkObjUnk.CheckedChanged += new EventHandler(chkObjUnk_CheckedChanged);
            // 
            // btnUnlink
            // 
            btnUnlink.Location = new System.Drawing.Point(177, 22);
            btnUnlink.Name = "btnUnlink";
            btnUnlink.Size = new System.Drawing.Size(28, 21);
            btnUnlink.TabIndex = 12;
            btnUnlink.Text = "-";
            btnUnlink.UseVisualStyleBackColor = true;
            btnUnlink.Click += new EventHandler(btnUnlink_Click);
            // 
            // btnRelink
            // 
            btnRelink.Location = new System.Drawing.Point(177, 2);
            btnRelink.Name = "btnRelink";
            btnRelink.Size = new System.Drawing.Size(28, 21);
            btnRelink.TabIndex = 4;
            btnRelink.Text = "+";
            btnRelink.UseVisualStyleBackColor = true;
            btnRelink.Click += new EventHandler(btnRelink_Click);
            // 
            // txtBone
            // 
            txtBone.Location = new System.Drawing.Point(49, 23);
            txtBone.Name = "txtBone";
            txtBone.ReadOnly = true;
            txtBone.Size = new System.Drawing.Size(126, 20);
            txtBone.TabIndex = 3;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(4, 23);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(42, 20);
            label4.TabIndex = 2;
            label4.Text = "Bone:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtModel
            // 
            txtModel.Location = new System.Drawing.Point(49, 3);
            txtModel.Name = "txtModel";
            txtModel.ReadOnly = true;
            txtModel.Size = new System.Drawing.Size(126, 20);
            txtModel.TabIndex = 1;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(4, 3);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(42, 20);
            label3.TabIndex = 0;
            label3.Text = "Model:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnPlayAnims);
            panel4.Controls.Add(btnPrevFrame);
            panel4.Controls.Add(btnNextFrame);
            panel4.Dock = DockStyle.Bottom;
            panel4.Enabled = false;
            panel4.Location = new System.Drawing.Point(0, 197);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(209, 24);
            panel4.TabIndex = 17;
            panel4.Visible = false;
            // 
            // btnPlayAnims
            // 
            btnPlayAnims.Dock = DockStyle.Fill;
            btnPlayAnims.Location = new System.Drawing.Point(35, 0);
            btnPlayAnims.Name = "btnPlayAnims";
            btnPlayAnims.Size = new System.Drawing.Size(139, 24);
            btnPlayAnims.TabIndex = 16;
            btnPlayAnims.Text = "Play Animations";
            btnPlayAnims.UseVisualStyleBackColor = true;
            btnPlayAnims.Click += new EventHandler(btnPlayAnims_Click);
            // 
            // btnPrevFrame
            // 
            btnPrevFrame.Dock = DockStyle.Left;
            btnPrevFrame.Location = new System.Drawing.Point(0, 0);
            btnPrevFrame.Name = "btnPrevFrame";
            btnPrevFrame.Size = new System.Drawing.Size(35, 24);
            btnPrevFrame.TabIndex = 18;
            btnPrevFrame.Text = "<";
            btnPrevFrame.UseVisualStyleBackColor = true;
            btnPrevFrame.Click += new EventHandler(btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            btnNextFrame.Dock = DockStyle.Right;
            btnNextFrame.Location = new System.Drawing.Point(174, 0);
            btnNextFrame.Name = "btnNextFrame";
            btnNextFrame.Size = new System.Drawing.Size(35, 24);
            btnNextFrame.TabIndex = 17;
            btnNextFrame.Text = ">";
            btnNextFrame.UseVisualStyleBackColor = true;
            btnNextFrame.Click += new EventHandler(btnNextFrame_Click);
            // 
            // _modelPanel
            // 
            _modelPanel.Dock = DockStyle.Fill;
            _modelPanel.Location = new System.Drawing.Point(0, 25);
            _modelPanel.Name = "_modelPanel";
            _modelPanel.Size = new System.Drawing.Size(481, 442);
            _modelPanel.TabIndex = 0;
            _modelPanel.PreRender += new GLRenderEventHandler(_modelPanel_PreRender);
            _modelPanel.PostRender += new GLRenderEventHandler(_modelPanel_PostRender);
            _modelPanel.KeyDown += new KeyEventHandler(_modelPanel_KeyDown);
            _modelPanel.MouseDown += new MouseEventHandler(_modelPanel_MouseDown);
            _modelPanel.MouseMove += new MouseEventHandler(_modelPanel_MouseMove);
            _modelPanel.MouseUp += new MouseEventHandler(_modelPanel_MouseUp);
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(toolStrip1);
            panel1.Controls.Add(btnResetRot);
            panel1.Controls.Add(trackBar1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(481, 25);
            panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.WhiteSmoke;
            toolStrip1.Dock = DockStyle.Fill;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[]
            {
                btnUndo,
                btnRedo,
                toolStripSeparator3,
                btnTransform,
                btnSplit,
                btnMerge,
                btnFlipColl,
                btnDelete,
                toolStripSeparator2,
                btnSameX,
                btnSameY,
                toolStripSeparator1,
                btnPerspectiveCam,
                btnOrthographicCam,
                btnResetCam,
                toolStripSeparatorCamera,
                btnSpawns,
                btnItems,
                btnBoundaries,
                toolStripSeparatorOverlays,
                btnResetSnap,
                btnHelp
            });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(335, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnUndo
            // 
            btnUndo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnUndo.Enabled = false;
            btnUndo.ImageTransparentColor = Color.Magenta;
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new System.Drawing.Size(40, 22);
            btnUndo.Text = "Undo";
            btnUndo.Click += new EventHandler(Undo);
            // 
            // btnRedo
            // 
            btnRedo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnRedo.Enabled = false;
            btnRedo.ImageTransparentColor = Color.Magenta;
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new System.Drawing.Size(38, 22);
            btnRedo.Text = "Redo";
            btnRedo.Click += new EventHandler(Redo);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnTransform
            // 
            btnTransform.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnTransform.Enabled = false;
            btnTransform.ImageTransparentColor = Color.Magenta;
            btnTransform.Name = "btnTransform";
            btnTransform.Size = new System.Drawing.Size(34, 22);
            btnTransform.Text = "Transform";
            btnTransform.Click += new EventHandler(transformToolStripMenuItem_Click);
            // 
            // btnSplit
            // 
            btnSplit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSplit.Enabled = false;
            btnSplit.ImageTransparentColor = Color.Magenta;
            btnSplit.Name = "btnSplit";
            btnSplit.Size = new System.Drawing.Size(34, 22);
            btnSplit.Text = "Split";
            btnSplit.Click += new EventHandler(btnSplit_Click);
            // 
            // btnMerge
            // 
            btnMerge.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnMerge.Enabled = false;
            btnMerge.ImageTransparentColor = Color.Magenta;
            btnMerge.Name = "btnMerge";
            btnMerge.Size = new System.Drawing.Size(45, 22);
            btnMerge.Text = "Merge";
            btnMerge.Click += new EventHandler(btnMerge_Click);
            // 
            // btnFlipColl
            // 
            btnFlipColl.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnFlipColl.Enabled = false;
            btnFlipColl.ImageTransparentColor = Color.Magenta;
            btnFlipColl.Name = "btnFlipColl";
            btnFlipColl.Size = new System.Drawing.Size(30, 22);
            btnFlipColl.Text = "Flip";
            btnFlipColl.Click += new EventHandler(btnFlipColl_Click);
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnDelete.Enabled = false;
            btnDelete.ImageTransparentColor = Color.Magenta;
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(44, 22);
            btnDelete.Text = "Delete";
            btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSameX
            // 
            btnSameX.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSameX.ImageTransparentColor = Color.Magenta;
            btnSameX.Name = "btnSameX";
            btnSameX.Size = new System.Drawing.Size(49, 22);
            btnSameX.Text = "Align X";
            btnSameX.Click += new EventHandler(btnSameX_Click);
            // 
            // btnSameY
            // 
            btnSameY.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSameY.ImageTransparentColor = Color.Magenta;
            btnSameY.Name = "btnSameY";
            btnSameY.Size = new System.Drawing.Size(49, 19);
            btnSameY.Text = "Align Y";
            btnSameY.Click += new EventHandler(btnSameY_Click);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPerspectiveCam
            // 
            btnPerspectiveCam.Checked = true;
            btnPerspectiveCam.CheckState = CheckState.Checked;
            btnPerspectiveCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnPerspectiveCam.ImageTransparentColor = Color.Magenta;
            btnPerspectiveCam.Name = "btnPerspectiveCam";
            btnPerspectiveCam.Size = new System.Drawing.Size(71, 19);
            btnPerspectiveCam.Text = "Perspective";
            btnPerspectiveCam.Click += new EventHandler(btnPerspectiveCam_Click);
            // 
            // btnOrthographicCam
            // 
            btnOrthographicCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnOrthographicCam.ImageTransparentColor = Color.Magenta;
            btnOrthographicCam.Name = "btnOrthographicCam";
            btnOrthographicCam.Size = new System.Drawing.Size(82, 19);
            btnOrthographicCam.Text = "Orthographic";
            btnOrthographicCam.Click += new EventHandler(btnOrthographicCam_Click);
            // 
            // btnResetCam
            // 
            btnResetCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnResetCam.ImageTransparentColor = Color.Magenta;
            btnResetCam.Name = "btnResetCam";
            btnResetCam.Size = new System.Drawing.Size(67, 19);
            btnResetCam.Text = "Reset Cam";
            btnResetCam.Click += new EventHandler(btnResetCam_Click);
            // 
            // toolStripSeparatorCamera
            // 
            toolStripSeparatorCamera.Name = "toolStripSeparatorCamera";
            toolStripSeparatorCamera.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSpawns
            // 
            btnSpawns.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSpawns.ImageTransparentColor = Color.Magenta;
            btnSpawns.Name = "btnSpawns";
            btnSpawns.Size = new System.Drawing.Size(51, 19);
            btnSpawns.Text = "Spawns";
            btnSpawns.Click += new EventHandler(btnSpawns_Click);
            // 
            // btnItems
            // 
            btnItems.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnItems.ImageTransparentColor = Color.Magenta;
            btnItems.Name = "btnItems";
            btnItems.Size = new System.Drawing.Size(40, 19);
            btnItems.Text = "Items";
            btnItems.Click += new EventHandler(btnItems_Click);
            // 
            // btnBoundaries
            // 
            btnBoundaries.Checked = true;
            btnBoundaries.CheckState = CheckState.Checked;
            btnBoundaries.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnBoundaries.ImageTransparentColor = Color.Magenta;
            btnBoundaries.Name = "btnBoundaries";
            btnBoundaries.Size = new System.Drawing.Size(70, 19);
            btnBoundaries.Text = "Boundaries";
            btnBoundaries.Click += new EventHandler(btnBoundaries_Click);
            // 
            // toolStripSeparatorOverlays
            // 
            toolStripSeparatorOverlays.Name = "toolStripSeparatorOverlays";
            toolStripSeparatorOverlays.Size = new System.Drawing.Size(6, 6);
            // 
            // btnResetSnap
            // 
            btnResetSnap.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnResetSnap.ImageTransparentColor = Color.Magenta;
            btnResetSnap.Name = "btnResetSnap";
            btnResetSnap.Size = new System.Drawing.Size(57, 19);
            btnResetSnap.Text = "Un-Snap";
            btnResetSnap.Click += new EventHandler(btnResetSnap_Click);
            // 
            // btnHelp
            // 
            btnHelp.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnHelp.Image = (Image) resources.GetObject("btnHelp.Image");
            btnHelp.ImageTransparentColor = Color.Magenta;
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new System.Drawing.Size(36, 19);
            btnHelp.Text = "Help";
            btnHelp.Click += new EventHandler(btnHelp_Click);
            // 
            // btnResetRot
            // 
            btnResetRot.Dock = DockStyle.Right;
            btnResetRot.Enabled = false;
            btnResetRot.FlatAppearance.BorderSize = 0;
            btnResetRot.FlatStyle = FlatStyle.Flat;
            btnResetRot.Location = new System.Drawing.Point(335, 0);
            btnResetRot.Name = "btnResetRot";
            btnResetRot.Size = new System.Drawing.Size(16, 25);
            btnResetRot.TabIndex = 4;
            btnResetRot.Text = "*";
            btnResetRot.UseVisualStyleBackColor = true;
            btnResetRot.Visible = false;
            btnResetRot.Click += new EventHandler(btnResetRot_Click);
            // 
            // trackBar1
            // 
            trackBar1.Dock = DockStyle.Right;
            trackBar1.Enabled = false;
            trackBar1.Location = new System.Drawing.Point(351, 0);
            trackBar1.Maximum = 180;
            trackBar1.Minimum = -180;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new System.Drawing.Size(130, 25);
            trackBar1.TabIndex = 3;
            trackBar1.TickStyle = TickStyle.None;
            trackBar1.Visible = false;
            trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
            // 
            // contextMenuStrip3
            // 
            contextMenuStrip3.Items.AddRange(new ToolStripItem[]
            {
                moveToNewObjectToolStripMenuItem,
                transformToolStripMenuItem,
                alignXToolStripMenuItem,
                alignYToolStripMenuItem,
                toolStripSeparator4,
                splitToolStripMenuItem,
                mergeToolStripMenuItem,
                flipToolStripMenuItem,
                _deleteToolStripMenuItem1
            });
            contextMenuStrip3.Name = "contextMenuStrip3";
            contextMenuStrip3.Size = new System.Drawing.Size(184, 208);
            contextMenuStrip3.Opening += new CancelEventHandler(contextMenuStrip3_Opening);
            // 
            // moveToNewObjectToolStripMenuItem
            // 
            moveToNewObjectToolStripMenuItem.Name = "moveToNewObjectToolStripMenuItem";
            moveToNewObjectToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            moveToNewObjectToolStripMenuItem.Text = "Move to New Object";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(180, 6);
            // 
            // splitToolStripMenuItem
            // 
            splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            splitToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            splitToolStripMenuItem.Text = "Split";
            splitToolStripMenuItem.Click += new EventHandler(btnSplit_Click);
            // 
            // mergeToolStripMenuItem
            // 
            mergeToolStripMenuItem.Name = "mergeToolStripMenuItem";
            mergeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            mergeToolStripMenuItem.Text = "Merge";
            mergeToolStripMenuItem.Click += new EventHandler(btnMerge_Click);
            // 
            // flipToolStripMenuItem
            // 
            flipToolStripMenuItem.Name = "flipToolStripMenuItem";
            flipToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            flipToolStripMenuItem.Text = "Flip";
            flipToolStripMenuItem.Click += new EventHandler(btnFlipColl_Click);
            // 
            // _deleteToolStripMenuItem1
            // 
            _deleteToolStripMenuItem1.Name = "_deleteToolStripMenuItem1";
            _deleteToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            _deleteToolStripMenuItem1.Text = "Delete";
            _deleteToolStripMenuItem1.Click += new EventHandler(btnDelete_Click);
            // 
            // transformToolStripMenuItem
            // 
            transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            transformToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            transformToolStripMenuItem.Text = "Transform";
            alignXToolStripMenuItem.Click += new EventHandler(transformToolStripMenuItem_Click);
            // 
            // alignXToolStripMenuItem
            // 
            alignXToolStripMenuItem.Name = "alignXToolStripMenuItem";
            alignXToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            alignXToolStripMenuItem.Text = "Align X";
            alignXToolStripMenuItem.Click += new EventHandler(btnSameX_Click);
            // 
            // alignYToolStripMenuItem
            // 
            alignYToolStripMenuItem.Name = "alignYToolStripMenuItem";
            alignYToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            alignYToolStripMenuItem.Text = "Align Y";
            alignYToolStripMenuItem.Click += new EventHandler(btnSameY_Click);
            // 
            // CollisionEditor
            // 
            BackColor = Color.Lavender;
            Controls.Add(undoToolStrip);
            Name = "CollisionEditor";
            Size = new System.Drawing.Size(694, 467);
            undoToolStrip.Panel1.ResumeLayout(false);
            undoToolStrip.Panel2.ResumeLayout(false);
            ((ISupportInitialize) undoToolStrip).EndInit();
            undoToolStrip.ResumeLayout(false);
            redoToolStrip.Panel1.ResumeLayout(false);
            redoToolStrip.Panel2.ResumeLayout(false);
            ((ISupportInitialize) redoToolStrip).EndInit();
            redoToolStrip.ResumeLayout(false);
            contextMenuStrip2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            pnlPlaneProps.ResumeLayout(false);
            groupBoxFlags2.ResumeLayout(false);
            groupBoxFlags1.ResumeLayout(false);
            groupBoxTargets.ResumeLayout(false);
            pnlPointProps.ResumeLayout(false);
            pnlPointProps.PerformLayout();
            pnlObjProps.ResumeLayout(false);
            pnlObjProps.PerformLayout();
            panel4.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((ISupportInitialize) trackBar1).EndInit();
            contextMenuStrip3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        protected const float SelectWidth = 7.0f;
        protected const float PointSelectRadius = 1.5f;
        protected const float SmallIncrement = 0.5f;
        protected const float LargeIncrement = 3.0f;

        protected CollisionNode _targetNode;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetNode
        {
            get => _targetNode;
            set => TargetChanged(value);
        }

        protected bool _updating;
        protected CollisionObject _selectedObject;
        protected Matrix _snapMatrix;

        protected bool _hovering;
        protected List<CollisionLink> _selectedLinks = new List<CollisionLink>();
        protected List<CollisionPlane> _selectedPlanes = new List<CollisionPlane>();

        protected bool _selecting, _selectInverse;
        protected Vector3 _selectStart, _selectLast, _selectEnd;
        protected bool _creating;

        protected CollisionState save;
        protected List<CollisionState> undoSaves = new List<CollisionState>();
        protected List<CollisionState> redoSaves = new List<CollisionState>();
        protected int saveIndex;
        protected bool hasMoved;

        public CollisionEditor()
        {
            InitializeComponent();

            _modelPanel.AddViewport(ModelPanelViewport.DefaultPerspective);

            _modelPanel.CurrentViewport.DefaultTranslate = new Vector3(0.0f, 10.0f, 250.0f);
            _modelPanel.CurrentViewport.AllowSelection = false;
            _modelPanel.CurrentViewport.BackgroundColor = Color.Black;

            pnlObjProps.Dock = DockStyle.Fill;
            pnlPlaneProps.Dock = DockStyle.Fill;
            pnlPointProps.Dock = DockStyle.Fill;

            _updating = true;
            cboMaterial.DataSource = CollisionTerrain.Terrains.Take(0x20).ToList(); // Take unexpanded collisions
            cboType.DataSource = Enum.GetValues(typeof(CollisionPlaneType));
            _updating = false;
        }

        protected void TargetChanged(CollisionNode node)
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
                //SnapObject();
            }

            ObjectSelected();

            _modelPanel.ResetCamera();
        }

        protected virtual void SelectionModified()
        {
            _selectedPlanes.Clear();
            foreach (CollisionLink l in _selectedLinks)
            {
                foreach (CollisionPlane p in l._members)
                {
                    if (_selectedLinks.Contains(p._linkLeft) &&
                        _selectedLinks.Contains(p._linkRight) &&
                        !_selectedPlanes.Contains(p))
                    {
                        _selectedPlanes.Add(p);
                    }
                }
            }

            pnlPlaneProps.Visible = false;
            pnlObjProps.Visible = false;
            pnlPointProps.Visible = false;
            panel3.Height = 0;

            if (_selectedPlanes.Count > 0)
            {
                pnlPlaneProps.Visible = true;
                panel3.Height = 205;
            }
            else if (_selectedLinks.Count == 1)
            {
                pnlPointProps.Visible = true;
                panel3.Height = 70;
            }

            UpdatePropPanels();
        }

        protected virtual void UpdatePropPanels()
        {
            _updating = true;

            if (pnlPlaneProps.Visible)
            {
                if (_selectedPlanes.Count <= 0)
                {
                    pnlPlaneProps.Visible = false;
                    return;
                }

                CollisionPlane p = _selectedPlanes[0];

                if (p._material >= 32 && cboMaterial.Items.Count <= 32)
                {
                    cboMaterial.DataSource =
                        CollisionTerrain.Terrains.ToList(); // Get the expanded collisions if they're used
                }

                cboMaterial.SelectedItem = cboMaterial.Items[p._material];

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
                chkFlagUnknown1.Checked = p.IsUnknownSSE;
                chkFlagUnknown2.Checked = p.IsUnknownFlag1;
                chkFlagUnknown3.Checked = p.IsUnknownFlag3;
                chkFlagUnknown4.Checked = p.IsUnknownFlag4;
            }
            else if (pnlPointProps.Visible)
            {
                if (_selectedLinks.Count <= 0)
                {
                    pnlPointProps.Visible = false;
                    return;
                }

                numX.Value = _selectedLinks[0].Value._x;
                numY.Value = _selectedLinks[0].Value._y;
            }
            else if (pnlObjProps.Visible)
            {
                if (_selectedObject == null)
                {
                    pnlObjProps.Visible = false;
                    return;
                }

                txtModel.Text = _selectedObject._modelName;
                txtBone.Text = _selectedObject._boneName;
                chkObjUnk.Checked = _selectedObject.UnknownFlag;
                chkObjModule.Checked = _selectedObject.ModuleControlled;
                chkObjSSEUnk.Checked = _selectedObject.UnknownSSEFlag;
            }

            _updating = false;
        }

        protected List<IModel> _models = new List<IModel>();

        protected void PopulateModelList()
        {
            modelTree.BeginUpdate();
            modelTree.Nodes.Clear();
            _models.Clear();

            if (_targetNode?._parent != null)
            {
                foreach (MDL0Node n in _targetNode._parent.FindChildrenByTypeInGroup(null, ResourceType.MDL0,
                    _targetNode.GroupID))
                {
                    TreeNode modelNode = new TreeNode(n._name) {Tag = n, Checked = true};
                    modelTree.Nodes.Add(modelNode);
                    _models.Add(n);

                    foreach (MDL0BoneNode bone in n._linker.BoneCache)
                    {
                        modelNode.Nodes.Add(new TreeNode(bone._name) {Tag = bone, Checked = true});
                    }

                    _modelPanel.AddTarget(n);
                    n.ResetToBindState();
                }
            }

            modelTree.EndUpdate();
        }

        #region Object List

        protected void PopulateObjectList()
        {
            lstObjects.BeginUpdate();
            lstObjects.Items.Clear();

            if (_targetNode != null)
            {
                foreach (CollisionObject obj in _targetNode.Children)
                {
                    obj._render = true;
                    lstObjects.Items.Add(obj, true);

                    MDL0Node model = _models.Where(m => m is MDL0Node && ((ResourceNode) m).Name == obj._modelName)
                                            .FirstOrDefault() as MDL0Node;

                    MDL0BoneNode bone =
                        model?._linker.BoneCache.Where(b => b.Name == obj._boneName)
                             .FirstOrDefault() as MDL0BoneNode;
                    if (bone != null)
                    {
                        obj._linkedBone = bone;
                    }

                    /*if (!obj._flags[1])
                        foreach (TreeNode n in modelTree.Nodes)
                            foreach (TreeNode b in n.Nodes)
                            {
                                MDL0BoneNode bone = b.Tag as MDL0BoneNode;
                                if (bone != null && bone.Name == obj._boneName && bone.BoneIndex == obj._boneIndex)
                                    obj._linkedBone = bone;
                            }*/
                }
            }

            lstObjects.EndUpdate();
        }

        protected void lstObjects_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstObjects.IndexFromPoint(e.Location);
            lstObjects.SelectedIndex = index;
        }

        protected void lstObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            _selectedObject = lstObjects.SelectedItem as CollisionObject;
            ObjectSelected();
        }

        protected void snapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SnapObject();
        }

        protected void _deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
            {
                return;
            }

            int index = lstObjects.SelectedIndex;

            _targetNode.Children.Remove(_selectedObject);
            lstObjects.Items.Remove(_selectedObject);
            _selectedObject = null;
            ClearSelection();
            if (lstObjects.Items.Count > 0)
            {
                if (lstObjects.Items.Count > index)
                {
                    lstObjects.SelectedIndex = index;
                }
                else if (index > 0)
                {
                    lstObjects.SelectedIndex = index - 1;
                }

                ObjectSelected();
            }

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void newObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedObject = new CollisionObject();
            _targetNode.Children.Add(_selectedObject);
            lstObjects.Items.Add(_selectedObject, true);
            lstObjects.SelectedItem = _selectedObject;
            //TargetNode.SignalPropertyChange();
        }

        protected void ObjectSelected()
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

        protected void SnapObject()
        {
            if (_selectedObject == null)
            {
                return;
            }

            _updating = true;

            _snapMatrix = Matrix.Identity;

            for (int i = 0; i < lstObjects.Items.Count; i++)
            {
                lstObjects.SetItemChecked(i, false);
            }

            //Set snap matrix
            if (!string.IsNullOrEmpty(_selectedObject._modelName))
            {
                foreach (TreeNode node in modelTree.Nodes)
                {
                    if (node.Text == _selectedObject._modelName)
                    {
                        foreach (TreeNode bNode in node.Nodes)
                        {
                            if (bNode.Text == _selectedObject._boneName)
                            {
                                _snapMatrix = ((MDL0BoneNode) bNode.Tag)._inverseBindMatrix;
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            //Show objects with similar bones
            for (int i = lstObjects.Items.Count; i-- > 0;)
            {
                CollisionObject obj = lstObjects.Items[i] as CollisionObject;
                if (obj._modelName == _selectedObject._modelName && obj._boneName == _selectedObject._boneName)
                {
                    lstObjects.SetItemChecked(i, true);
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void lstObjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CollisionObject obj = lstObjects.Items[e.Index] as CollisionObject;
            obj._render = e.NewValue == CheckState.Checked;

            ClearSelection();

            if (!_updating)
            {
                _modelPanel.Invalidate();
            }
        }

        #endregion

        protected void ClearSelection()
        {
            foreach (CollisionLink l in _selectedLinks)
            {
                l._highlight = false;
            }

            _selectedLinks.Clear();
            _selectedPlanes.Clear();
        }

        protected void UpdateSelection(bool finish)
        {
            foreach (CollisionObject obj in _targetNode.Children)
            {
                foreach (CollisionLink link in obj._points)
                {
                    link._highlight = false;
                    if (!obj._render)
                    {
                        continue;
                    }

                    Vector3 point = (Vector3) link.Value;

                    if (_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        if (finish)
                        {
                            _selectedLinks.Remove(link);
                        }

                        continue;
                    }

                    if (_selectedLinks.Contains(link))
                    {
                        link._highlight = true;
                    }
                    else if (!_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        link._highlight = true;
                        if (finish)
                        {
                            _selectedLinks.Add(link);
                        }
                    }
                }
            }
        }

        public void UpdateTools()
        {
            if (_selecting || _hovering || _selectedLinks.Count == 0)
            {
                btnDelete.Enabled = btnFlipColl.Enabled = btnTransform.Enabled =
                    btnMerge.Enabled = btnSplit.Enabled = btnSameX.Enabled = btnSameY.Enabled = false;
            }
            else
            {
                btnMerge.Enabled = btnSameX.Enabled = btnSameY.Enabled = _selectedLinks.Count > 1;
                btnDelete.Enabled = btnSplit.Enabled = btnTransform.Enabled = true;
                btnFlipColl.Enabled = _selectedPlanes.Count > 0;
            }
        }

        protected void _treeObjects_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is CollisionObject)
            {
                (e.Node.Tag as CollisionObject)._render = e.Node.Checked;
            }

            if (e.Node.Tag is CollisionPlane)
            {
                (e.Node.Tag as CollisionPlane)._render = e.Node.Checked;
            }

            _modelPanel.Invalidate();
        }

        protected void chkAllModels_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in modelTree.Nodes)
            {
                node.Checked = chkAllModels.Checked;
            }
        }

        protected void BeginHover(Vector3 point)
        {
            if (_hovering)
            {
                return;
            }

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

        protected void UpdateHover(int x, int y)
        {
            if (!_hovering)
            {
                return;
            }

            _selectEnd = Vector3.IntersectZ(_modelPanel.CurrentViewport.UnProject(x, y, 0.0f),
                _modelPanel.CurrentViewport.UnProject(x, y, 1.0f), _selectLast._z);

            //Apply difference in start/end
            Vector3 diff = _selectEnd - _selectLast;
            _selectLast = _selectEnd;

            //Move points
            foreach (CollisionLink p in _selectedLinks)
            {
                p.Value += diff;
            }

            _modelPanel.Invalidate();

            UpdatePropPanels();
        }

        protected void CancelHover()
        {
            if (!_hovering)
            {
                return;
            }

            if (hasMoved)
            {
                undoSaves.RemoveAt(undoSaves.Count - 1);
                saveIndex--;
                hasMoved = false;
                if (saveIndex == 0)
                {
                    btnUndo.Enabled = false;
                }
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
                {
                    l.Value += diff;
                }
            }

            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        protected void FinishHover()
        {
            _hovering = false;
        }

        protected void BeginSelection(Vector3 point, bool inverse)
        {
            if (_selecting)
            {
                return;
            }

            _selectStart = _selectEnd = point;

            _selectEnd._z += SelectWidth;
            _selectStart._z -= SelectWidth;

            _selecting = true;
            _selectInverse = inverse;

            UpdateTools();
        }

        protected void CancelSelection()
        {
            if (!_selecting)
            {
                return;
            }

            _selecting = false;
            _selectStart = _selectEnd = new Vector3(float.MaxValue);
            UpdateSelection(false);
            _modelPanel.Invalidate();
        }

        protected void FinishSelection()
        {
            if (!_selecting)
            {
                return;
            }

            _selecting = false;
            UpdateSelection(true);
            _modelPanel.Invalidate();

            SelectionModified();

            //Selection Area Selected.
        }

        private DateTime _RCstart;

        protected void _modelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bool create = ModifierKeys == Keys.Alt;
                bool add = ModifierKeys == Keys.Shift;
                bool subtract = ModifierKeys == Keys.Control;
                bool move = ModifierKeys == (Keys.Control | Keys.Shift);

                float depth = _modelPanel.GetDepth(e.X, e.Y);
                Vector3 target = _modelPanel.CurrentViewport.UnProject(e.X, e.Y, depth);
                Vector2 point;

                if (!move && depth < 1.0f)
                {
                    point = (Vector2) target;

                    //Hit-detect points first
                    foreach (CollisionObject obj in _targetNode.Children)
                    {
                        if (obj._render)
                        {
                            foreach (CollisionLink p in obj._points)
                            {
                                if (p.Value.Contained(point, point, PointSelectRadius))
                                {
                                    if (create)
                                    {
                                        //Connect all selected links to point
                                        foreach (CollisionLink l in _selectedLinks)
                                        {
                                            l.Connect(p);
                                        }

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
                                        {
                                            ClearSelection();
                                        }

                                        _selectedLinks.Add(p);
                                        p._highlight = true;
                                        _modelPanel.Invalidate();
                                        SelectionModified();
                                    }

                                    if (!add && !subtract)
                                    {
                                        BeginHover(target);
                                    }

                                    //Single Link Selected
                                    return;
                                }
                            }
                        }
                    }

                    float dist;
                    float bestDist = float.MaxValue;
                    CollisionPlane bestMatch = null;

                    //Hit-detect planes finding best match
                    foreach (CollisionObject obj in _targetNode.Children)
                    {
                        if (obj._render)
                        {
                            foreach (CollisionPlane p in obj._planes)
                            {
                                if (point.Contained(p.PointLeft, p.PointRight, PointSelectRadius))
                                {
                                    dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) -
                                           p.PointLeft.TrueDistance(p.PointRight);
                                    if (dist < bestDist)
                                    {
                                        bestDist = dist;
                                        bestMatch = p;
                                    }
                                }
                            }
                        }
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
                        if (!_selectedLinks.Contains(bestMatch._linkLeft) ||
                            !_selectedLinks.Contains(bestMatch._linkRight))
                        {
                            if (!add)
                            {
                                ClearSelection();
                            }

                            _selectedLinks.Add(bestMatch._linkLeft);
                            _selectedLinks.Add(bestMatch._linkRight);
                            bestMatch._linkLeft._highlight = bestMatch._linkRight._highlight = true;
                            _modelPanel.Invalidate();

                            SelectionModified();
                        }

                        if (!add)
                        {
                            BeginHover(target);
                        }

                        //Single Platform Selected;
                        return;
                    }
                }

                //Nothing found :(

                //Trace ray to Z axis
                target = Vector3.IntersectZ(target, _modelPanel.CurrentViewport.UnProject(e.X, e.Y, 0.0f), 0.0f);
                point = (Vector2) target;

                if (create)
                {
                    if (_selectedLinks.Count == 0)
                    {
                        if (_selectedObject == null)
                        {
                            return;
                        }

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
                        _selectedLinks[0] = link.Branch((Vector2) target);
                        _selectedLinks[0]._highlight = true;
                        link._highlight = false;
                        SelectionModified();
                        _modelPanel.Invalidate();

                        //Hover new point so it can be moved
                        BeginHover(target);
                        return;
                    }
                    else if (_selectedPlanes.Count > 0)
                    {
                        //Find two closest points and insert between
                        CollisionPlane bestMatch = null;
                        if (_selectedPlanes.Count == 1)
                        {
                            bestMatch = _selectedPlanes[0];
                        }
                        else
                        {
                            float dist;
                            float bestDist = float.MaxValue;

                            foreach (CollisionPlane p in _selectedPlanes)
                            {
                                dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) -
                                       p.PointLeft.TrueDistance(p.PointRight);
                                if (dist < bestDist)
                                {
                                    bestDist = dist;
                                    bestMatch = p;
                                }
                            }
                        }

                        if (bestMatch == null)
                        {
                            bestMatch = _selectedPlanes[0];
                        }

                        ClearSelection();
                        if (bestMatch != null)
                        {
                            _selectedLinks.Add(bestMatch.Split(point));
                            _selectedLinks[0]._highlight = true;
                            SelectionModified();
                            _modelPanel.Invalidate();

                            _creating = true;
                            BeginHover(target);
                        }

                        return;
                    }
                    else
                    {
                        //Create new planes extending to point
                        CollisionLink link = null;
                        List<CollisionLink> links = new List<CollisionLink>();
                        _creating = true;
                        foreach (CollisionLink l in _selectedLinks)
                        {
                            links.Add(l.Branch((Vector2) target));
                            l._highlight = false;
                        }

                        link = links[0];
                        links.RemoveAt(0);
                        for (int x = 0; x < links.Count;)
                        {
                            if (link.Merge(links[x]))
                            {
                                links.RemoveAt(x);
                            }
                            else
                            {
                                x++;
                            }
                        }

                        _selectedLinks.Clear();
                        _selectedLinks.Add(link);
                        link._highlight = true;
                        SelectionModified();
                        _modelPanel.Invalidate();

                        //Hover new point so it can be moved
                        BeginHover(target);

                        return;
                    }
                }

                if (move)
                {
                    if (_selectedLinks.Count > 0)
                    {
                        BeginHover(target);
                    }

                    return;
                }

                if (!add && !subtract)
                {
                    ClearSelection();
                }

                BeginSelection(target, subtract);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _RCstart = DateTime.UtcNow;
            }
        }

        protected void _modelPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (saveIndex - 1 > 0 && saveIndex - 1 < undoSaves.Count)
                {
                    if (undoSaves[saveIndex - 1]._collisionLinks[0].Value.ToString() ==
                        undoSaves[saveIndex - 1]._linkVectors[0].ToString()) //If equal to starting point, remove.
                    {
                        undoSaves.RemoveAt(saveIndex - 1);
                        saveIndex--;
                        if (saveIndex == 0)
                        {
                            btnUndo.Enabled = false;
                        }
                    }
                }

                hasMoved = false;
                FinishSelection();
                FinishHover();
                UpdateTools();
            }
            else if (e.Button == MouseButtons.Right)
            {
                DateTime _RCend = DateTime.UtcNow;
                if (_RCend - _RCstart <= TimeSpan.FromSeconds(0.5) && _selectedLinks != null &&
                    _selectedLinks.Count > 0)
                {
                    //contextMenuStrip3.Show(Cursor.Position);
                }
            }
        }

        protected void _modelPanel_MouseMove(object sender, MouseEventArgs e)
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

        protected bool PointCollides(Vector3 point)
        {
            return PointCollides(point, out float f);
        }

        protected bool PointCollides(Vector3 point, out float y_result)
        {
            y_result = float.MaxValue;
            Vector2 v2 = new Vector2(point._x, point._y);
            foreach (CollisionObject obj in _targetNode.Children)
            {
                if (obj._render || true)
                {
                    foreach (CollisionPlane plane in obj._planes)
                    {
                        if (plane._type == CollisionPlaneType.Floor && plane.IsCharacters)
                        {
                            if (plane.PointLeft._x <= v2._x && plane.PointRight._x >= v2._x)
                            {
                                float x = v2._x;
                                float m = (plane.PointLeft._y - plane.PointRight._y)
                                          / (plane.PointLeft._x - plane.PointRight._x);
                                float b = plane.PointRight._y - m * plane.PointRight._x;
                                float y_target = m * x + b;
                                //Console.WriteLine(y_target);
                                if (Math.Abs(y_target - v2._y) <= Math.Abs(y_result - v2._y))
                                {
                                    y_result = y_target;
                                }
                            }
                        }
                    }
                }
            }

            return Math.Abs(y_result - v2._y) <= 5;
        }

        protected void _modelPanel_PreRender(object sender)
        {
        }

        protected unsafe void _modelPanel_PostRender(object sender)
        {
            //Clear depth buffer so we can hit-detect
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            //Render objects
            _targetNode?.Render();

            if (_modelPanel.RenderBones)
            {
                foreach (IRenderedObject o in _modelPanel._renderList)
                {
                    (o as IModel)?.RenderBones(_modelPanel.CurrentViewport);
                }
            }

            #region RenderOverlays

            GL.Disable(EnableCap.DepthTest);
            List<MDL0BoneNode> ItemBones = new List<MDL0BoneNode>();

            MDL0Node stgPos = null;

            MDL0BoneNode CamBone0 = null,
                CamBone1 = null,
                DeathBone0 = null,
                DeathBone1 = null;

            foreach (MDL0Node m in _models)
            {
                if (m.IsStagePosition)
                {
                    stgPos = m;
                    break;
                }
            }

            if (stgPos != null)
            {
                foreach (MDL0BoneNode bone in stgPos._linker.BoneCache)
                {
                    if (bone._name == "CamLimit0N")
                    {
                        CamBone0 = bone;
                    }
                    else if (bone.Name == "CamLimit1N")
                    {
                        CamBone1 = bone;
                    }
                    else if (bone.Name == "Dead0N")
                    {
                        DeathBone0 = bone;
                    }
                    else if (bone.Name == "Dead1N")
                    {
                        DeathBone1 = bone;
                    }
                    else if (bone._name.StartsWith("Player") && bone._name.Length == 8 && btnSpawns.Checked)
                    {
                        Vector3 position = bone._frameMatrix.GetPoint();
                        if (PointCollides(position))
                        {
                            GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);
                        }
                        else
                        {
                            GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                        }

                        TKContext.DrawSphere(position, 5.0f, 32);
                        if (int.TryParse(bone._name.Substring(6, 1), out int playernum))
                        {
                            _modelPanel.CurrentViewport.NoSettingsScreenText[playernum.ToString()] =
                                _modelPanel.CurrentViewport.Camera.Project(position) - new Vector3(8.0f, 8.0f, 0);
                        }
                    }
                    else if (bone._name.StartsWith("Rebirth") && bone._name.Length == 9 && btnSpawns.Checked)
                    {
                        GL.Color4(1.0f, 1.0f, 1.0f, 0.1f);
                        TKContext.DrawSphere(bone._frameMatrix.GetPoint(), 5.0f, 32);
                        if (int.TryParse(bone._name.Substring(7, 1), out int playernum))
                        {
                            _modelPanel.CurrentViewport.NoSettingsScreenText[playernum.ToString()] =
                                _modelPanel.CurrentViewport.Camera.Project(bone._frameMatrix.GetPoint()) -
                                new Vector3(8.0f, 8.0f, 0);
                        }
                    }
                    else if (bone._name.Contains("Item"))
                    {
                        ItemBones.Add(bone);
                    }
                }
            }

            //Render item fields if checked
            if (ItemBones != null && btnItems.Checked)
            {
                GL.Color4(0.5f, 0.0f, 1.0f, 0.4f);
                for (int i = 0; i < ItemBones.Count; i += 2)
                {
                    Vector3 pos1, pos2;
                    if (ItemBones[i]._frameMatrix.GetPoint()._y == ItemBones[i + 1]._frameMatrix.GetPoint()._y)
                    {
                        pos1 = new Vector3(ItemBones[i]._frameMatrix.GetPoint()._x,
                            ItemBones[i]._frameMatrix.GetPoint()._y + 1.5f, 1.0f);
                        pos2 = new Vector3(ItemBones[i + 1]._frameMatrix.GetPoint()._x,
                            ItemBones[i + 1]._frameMatrix.GetPoint()._y - 1.5f, 1.0f);
                    }
                    else
                    {
                        pos1 = new Vector3(ItemBones[i]._frameMatrix.GetPoint()._x,
                            ItemBones[i]._frameMatrix.GetPoint()._y, 1.0f);
                        pos2 = new Vector3(ItemBones[i + 1]._frameMatrix.GetPoint()._x,
                            ItemBones[i + 1]._frameMatrix.GetPoint()._y, 1.0f);
                    }


                    if (pos1._x != pos2._x)
                    {
                        TKContext.DrawBox(pos1, pos2);
                    }
                    else
                    {
                        TKContext.DrawSphere(
                            new Vector3(ItemBones[i]._frameMatrix.GetPoint()._x,
                                ItemBones[i]._frameMatrix.GetPoint()._y, pos1._z), 3.0f, 32);
                    }
                }
            }

            //Render boundaries if checked
            if (CamBone0 != null && CamBone1 != null && btnBoundaries.Checked)
            {
                //GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Lighting);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Front);

                GL.Color4(Color.Blue);
                GL.Begin(BeginMode.LineLoop);
                GL.LineWidth(15.0f);

                Vector3
                    camBone0 = CamBone0._frameMatrix.GetPoint(),
                    camBone1 = CamBone1._frameMatrix.GetPoint(),
                    deathBone0 = DeathBone0._frameMatrix.GetPoint(),
                    deathBone1 = DeathBone1._frameMatrix.GetPoint();

                GL.Vertex2(camBone0._x, camBone0._y);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.LineLoop);
                GL.Color4(Color.Red);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.End();
                GL.Color4(0.0f, 0.5f, 1.0f, 0.3f);
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone0._x, camBone0._y);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(camBone0._x, camBone0._y);
                GL.End();
            }

            #endregion

            //Render selection box
            if (!_selecting)
            {
                return;
            }

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

        protected void btnSplit_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer();
            for (int i = _selectedLinks.Count; --i >= 0;)
            {
                _selectedLinks[i].Split();
            }

            ClearSelection();
            SelectionModified();
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer();

            for (int i = 0; i < _selectedLinks.Count - 1;)
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
                    {
                        x++;
                    }
                }

                link.Value = pos / count;
                TargetNode.SignalPropertyChange();
            }

            _modelPanel.Invalidate();
        }

        protected void trackBar1_Scroll(object sender, EventArgs e)
        {
            _modelPanel.Invalidate();
        }

        protected void btnResetRot_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            _modelPanel.Invalidate();
        }

        protected void btnResetCam_Click(object sender, EventArgs e)
        {
            _modelPanel.ResetCamera();
        }

        // BrawlCrate Perspective viewer
        protected void btnPerspectiveCam_Click(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnPerspectiveCam.Checked = true;
            btnOrthographicCam.Checked = false;
            if (_modelPanel.CurrentViewport.ViewType != ViewportProjection.Perspective)
            {
                _modelPanel.ResetCamera();
                _modelPanel.CurrentViewport.ViewType = ViewportProjection.Perspective;
            }
        }

        // BrawlCrate Orthographic viewer
        protected void btnOrthographicCam_Click(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnPerspectiveCam.Checked = false;
            btnOrthographicCam.Checked = true;
            if (_modelPanel.CurrentViewport.ViewType != ViewportProjection.Orthographic)
            {
                _modelPanel.ResetCamera();
                _modelPanel.CurrentViewport.ViewType = ViewportProjection.Orthographic;
            }
        }

        protected void btnSpawns_Click(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnSpawns.Checked = !btnSpawns.Checked;
            _modelPanel.Invalidate();
        }

        protected void btnItems_Click(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnItems.Checked = !btnItems.Checked;
            _modelPanel.Invalidate();
        }

        protected void btnBoundaries_Click(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnBoundaries.Checked = !btnBoundaries.Checked;
            _modelPanel.Invalidate();
        }

        protected void _modelPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_hovering)
                {
                    CancelHover();
                }
                else if (_selecting)
                {
                    CancelSelection();
                }
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
                    {
                        plane.Delete();
                    }

                    TargetNode.SignalPropertyChange();
                }
                else if (_selectedLinks.Count > 0)
                {
                    for (int i = 0; i < _selectedLinks.Count; i++)
                    {
                        _selectedLinks[i].Pop();
                    }

                    TargetNode.SignalPropertyChange();
                }

                ClearSelection();
                SelectionModified();
                _modelPanel.Invalidate();
            }
            else if (ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.Z)
                {
                    if (_hovering)
                    {
                        CancelHover();
                    }
                    else if (btnUndo.Enabled)
                    {
                        Undo(this, null);
                    }
                }
                else if (e.KeyCode == Keys.Y)
                {
                    if (_hovering)
                    {
                        CancelHover();
                    }
                    else if (btnRedo.Enabled)
                    {
                        Redo(this, null);
                    }
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
                {
                    link = _selectedLinks[0];
                }

                if (link != null)
                {
                    foreach (CollisionPlane p in link._members)
                    {
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
                {
                    link = _selectedLinks[0];
                }

                if (link != null)
                {
                    foreach (CollisionPlane p in link._members)
                    {
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
                }
            }
            else if (e.KeyCode == Keys.W)
            {
                CreateUndo();
                float amount = ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                {
                    link._rawValue._y += amount;
                }

                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.S)
            {
                CreateUndo();
                float amount = ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                {
                    link._rawValue._y -= amount;
                }

                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.A)
            {
                CreateUndo();
                float amount = ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                {
                    link._rawValue._x -= amount;
                }

                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.D)
            {
                CreateUndo();
                float amount = ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                {
                    link._rawValue._x += amount;
                }

                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
        }

        #region Plane Properties

        protected void cboMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;
            foreach (CollisionPlane plane in _selectedPlanes)
            {
                plane._material = ((CollisionTerrain) cboMaterial.SelectedItem).ID;
            }

            _updating = false;

            TargetNode.SignalPropertyChange();
        }

        protected void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;
            foreach (CollisionPlane plane in _selectedPlanes)
            {
                plane.Type = (CollisionPlaneType) cboType.SelectedItem;
                if (ErrorChecking && !plane.IsRotating)
                {
                    if (!plane.IsFloor)
                    {
                        plane.IsFallThrough = false;
                        chkFallThrough.Checked = false;
                        plane.IsRightLedge = false;
                        chkRightLedge.Checked = false;
                        plane.IsLeftLedge = false;
                        chkLeftLedge.Checked = false;
                    }

                    if (!plane.IsWall)
                    {
                        plane.IsNoWalljump = false;
                        chkNoWalljump.Checked = false;
                    }
                }
            }

            _updating = false;

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void chkTypeCharacters_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkTypeCharacters_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }


            TargetNode.SignalPropertyChange();
            _updating = true;
            bool selection = chkTypeCharacters.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsCharacters = selection;
                if (p.IsCharacters)
                {
                    p.IsItems = false;
                    chkTypeItems.Checked = false;
                    p.IsPokemonTrainer = false;
                    chkTypePokemonTrainer.Checked = false;
                }
                else
                {
                    p.IsFallThrough = false;
                    chkFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    chkNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    chkRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    chkLeftLedge.Checked = false;
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void chkTypeItems_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkTypeItems_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            TargetNode.SignalPropertyChange();
            _updating = true;
            bool selection = chkTypeItems.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsItems = selection;
                if (p.IsItems)
                {
                    p.IsCharacters = false;
                    chkTypeCharacters.Checked = false;
                    p.IsFallThrough = false;
                    chkFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    chkNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    chkRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    chkLeftLedge.Checked = false;
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void chkTypePokemonTrainer_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkTypePokemonTrainer_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            TargetNode.SignalPropertyChange();
            _updating = true;
            bool selection = chkTypePokemonTrainer.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsPokemonTrainer = selection;
                if (p.IsPokemonTrainer)
                {
                    p.IsCharacters = false;
                    chkTypeCharacters.Checked = false;
                    p.IsFallThrough = false;
                    chkFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    chkNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    chkRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    chkLeftLedge.Checked = false;
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void chkTypeRotating_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkTypeRotating_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            TargetNode.SignalPropertyChange();
            _updating = true;
            bool selection = chkTypeRotating.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool allSameType = true;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRotating = selection;
                if (!p.IsRotating)
                {
                    if (!p.IsFloor)
                    {
                        p.IsFallThrough = false;
                        p.IsRightLedge = false;
                        p.IsLeftLedge = false;
                    }

                    if (!p.IsWall)
                    {
                        p.IsNoWalljump = false;
                    }
                }

                if (allSameType)
                {
                    if (p.IsWall && (firstType == CollisionPlaneType.LeftWall ||
                                     firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0)
            {
                chkTypeRotating.Checked = _selectedPlanes[0].IsRotating;
                if (!_selectedPlanes[0].IsRotating)
                {
                    if (!_selectedPlanes[0].IsFloor)
                    {
                        chkFallThrough.Checked = false;
                        chkRightLedge.Checked = false;
                        chkLeftLedge.Checked = false;
                    }

                    if (!_selectedPlanes[0].IsWall)
                    {
                        chkNoWalljump.Checked = false;
                    }
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void chkFallThrough_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkFallThrough_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            _updating = true;
            TargetNode.SignalPropertyChange();
            bool selection = chkFallThrough.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsFallThrough = selection;
                if (!p.IsFloor && !p.IsRotating || !p.IsCharacters)
                {
                    p.IsFallThrough = false;
                }

                if (allSameType)
                {
                    if (p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }

                    if (p.IsWall && (firstType == CollisionPlaneType.LeftWall ||
                                     firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }

                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0)
            {
                chkFallThrough.Checked = _selectedPlanes[0].IsFallThrough;
                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating || !_selectedPlanes[0].IsCharacters)
                {
                    chkFallThrough.Checked = false;
                }
            }

            if (allNonCharacters)
            {
                chkFallThrough.Checked = false;
            }

            _updating = false;
        }

        protected void chkLeftLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkLeftLedge_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            _updating = true;
            TargetNode.SignalPropertyChange();
            bool selection = chkLeftLedge.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            bool anyNoLedgeFloors = false;
            bool allNoLedge = true;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                bool noLedge = false;
                if (!p.IsRotating)
                {
                    foreach (CollisionPlane x in p._linkLeft._members)
                    {
                        if (x != p)
                        {
                            if ((x.Type == CollisionPlaneType.Floor || x.Type == CollisionPlaneType.RightWall) &&
                                x.IsCharacters)
                            {
                                noLedge = true;
                                if (x.Type == CollisionPlaneType.Floor)
                                {
                                    anyNoLedgeFloors = true;
                                }
                            }
                        }
                    }
                }

                if (!p.IsFloor && !p.IsRotating || !p.IsCharacters)
                {
                    noLedge = true;
                }

                if (!noLedge)
                {
                    allNoLedge = false;
                    p.IsLeftLedge = selection;
                }
                else
                {
                    p.IsLeftLedge = false;
                    continue;
                }

                if (p.IsLeftLedge)
                {
                    p.IsRightLedge = false;
                }

                if (allSameType)
                {
                    if (p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }

                    if (p.IsWall && (firstType == CollisionPlaneType.LeftWall ||
                                     firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }

                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }

            if (allNonCharacters)
            {
                chkLeftLedge.Checked = false;
            }
            else if (allNoLedge)
            {
                chkLeftLedge.Checked = false;
            }
            else if (anyNoLedgeFloors)
            {
                if (chkLeftLedge.Checked != selection)
                {
                    chkLeftLedge.Checked = selection;
                }
            }
            else if (!anyNoLedgeFloors)
            {
                chkRightLedge.Checked = false;
                if (chkLeftLedge.Checked != selection)
                {
                    chkLeftLedge.Checked = selection;
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0 && !anyNoLedgeFloors &&
                !allNonCharacters)
            {
                chkLeftLedge.Checked = _selectedPlanes[0].IsLeftLedge;
                if (_selectedPlanes[0].IsLeftLedge)
                {
                    chkRightLedge.Checked = false;
                }

                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating)
                {
                    chkRightLedge.Checked = false;
                    chkLeftLedge.Checked = false;
                }
            }

            _updating = false;

            _modelPanel.Invalidate();
        }

        protected void chkRightLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkRightLedge_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            _updating = false;
            TargetNode.SignalPropertyChange();
            bool selection = chkRightLedge.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            bool anyNoLedgeFloors = false;
            bool allNoLedge = true;

            foreach (CollisionPlane p in _selectedPlanes)
            {
                bool noLedge = false;

                if (!p.IsRotating)
                {
                    foreach (CollisionPlane x in p._linkRight._members)
                    {
                        if (x != p)
                        {
                            if ((x.Type == CollisionPlaneType.Floor || x.Type == CollisionPlaneType.LeftWall) &&
                                x.IsCharacters)
                            {
                                noLedge = true;
                                if (x.Type == CollisionPlaneType.Floor)
                                {
                                    anyNoLedgeFloors = true;
                                }
                            }
                        }
                    }
                }

                if (!p.IsFloor && !p.IsRotating || !p.IsCharacters)
                {
                    noLedge = true;
                }

                if (!noLedge)
                {
                    allNoLedge = false;
                    p.IsRightLedge = selection;
                }
                else
                {
                    p.IsRightLedge = false;
                    continue;
                }

                if (p.IsRightLedge)
                {
                    p.IsLeftLedge = false;
                }

                if (allSameType)
                {
                    if (p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }

                    if (p.IsWall && (firstType == CollisionPlaneType.RightWall ||
                                     firstType == CollisionPlaneType.LeftWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }

                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }

            if (allNonCharacters)
            {
                chkRightLedge.Checked = false;
            }
            else if (allNoLedge)
            {
                if (chkRightLedge.Checked)
                {
                    chkRightLedge.Checked = false;
                }
            }
            else if (anyNoLedgeFloors)
            {
                if (chkRightLedge.Checked != selection)
                {
                    chkRightLedge.Checked = selection;
                }
            }
            else if (!anyNoLedgeFloors)
            {
                chkLeftLedge.Checked = false;
                if (chkRightLedge.Checked != selection)
                {
                    chkRightLedge.Checked = selection;
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0 && !anyNoLedgeFloors &&
                !allNonCharacters)
            {
                chkRightLedge.Checked = _selectedPlanes[0].IsRightLedge;
                if (_selectedPlanes[0].IsRightLedge)
                {
                    chkLeftLedge.Checked = false;
                }

                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating)
                {
                    chkLeftLedge.Checked = false;
                    chkRightLedge.Checked = false;
                }
            }

            _modelPanel.Invalidate();
            _updating = false;
        }

        protected void chkNoWalljump_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!ErrorChecking)
            {
                chkNoWalljump_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            _updating = true;
            TargetNode.SignalPropertyChange();
            bool selection = chkNoWalljump.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsNoWalljump = selection;
                if (!p.IsWall && !p.IsRotating || !p.IsCharacters)
                {
                    p.IsNoWalljump = false;
                }

                if (allSameType)
                {
                    if (p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }

                    if (p.IsWall && (firstType == CollisionPlaneType.LeftWall ||
                                     firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }

                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0)
            {
                chkNoWalljump.Checked = _selectedPlanes[0].IsNoWalljump;
                if (!_selectedPlanes[0].IsWall && !_selectedPlanes[0].IsRotating)
                {
                    chkNoWalljump.Checked = false;
                }
            }

            if (allNonCharacters)
            {
                chkNoWalljump.Checked = false;
            }

            _updating = false;
        }

        protected void chkTypeCharacters_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsCharacters = chkTypeCharacters.Checked;
            }

            _modelPanel.Invalidate();
        }

        protected void chkTypeItems_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsItems = chkTypeItems.Checked;
            }
        }

        protected void chkTypePokemonTrainer_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsPokemonTrainer = chkTypePokemonTrainer.Checked;
            }
        }

        protected void chkTypeRotating_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRotating = chkTypeRotating.Checked;
            }
        }

        protected void chkFallThrough_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsFallThrough = chkFallThrough.Checked;
            }
        }

        protected void chkLeftLedge_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsLeftLedge = chkLeftLedge.Checked;
            }

            _modelPanel.Invalidate();
        }

        protected void chkRightLedge_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRightLedge = chkRightLedge.Checked;
            }

            _modelPanel.Invalidate();
        }

        protected void chkNoWalljump_CheckedChanged_NoErrorHandling(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsNoWalljump = chkNoWalljump.Checked;
            }
        }

        protected void chkFlagUnknown1_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownSSE = chkFlagUnknown1.Checked;
            }
        }

        protected void chkFlagUnknown2_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag1 = chkFlagUnknown2.Checked;
            }
        }

        protected void chkFlagUnknown3_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag3 = chkFlagUnknown3.Checked;
            }
        }

        protected void chkFlagUnknown4_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag4 = chkFlagUnknown4.Checked;
            }
        }

        #endregion

        #region Point Properties

        protected void numX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (numX.Text == "" && ErrorChecking)
            {
                return;
            }

            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent?.LinkedBone == null)
                {
                    link._rawValue._x = numX.Value;
                }
                else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(numX.Value, oldValue._y);
                }
            }

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void numY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (numY.Text == "" && ErrorChecking)
            {
                return;
            }

            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent?.LinkedBone == null)
                {
                    link._rawValue._y = numY.Value;
                }
                else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(oldValue._x, numY.Value);
                }
            }

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        #endregion

        protected void transformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransformEditor transform = new TransformEditor();
            if (transform.ShowDialog() == DialogResult.OK)
            {
                CreateUndo();

                if (_selectedPlanes.Count > 0)
                {
                    FrameState _centerState =
                        new FrameState(new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0));
                    /*if(transform._transform.ScalingType == xyTransform.ScaleType.FromCenterOfCollisions)
                    {
                        Vector2 v2avg = new Vector2(0, 0);
                        int i = 0;
                        foreach(CollisionLink l in _selectedLinks)
                        {
                            v2avg += l._rawValue;
                            i++;
                        }
                        float newX = (v2avg._x / i) * -1;// * (v2avg._x >= 1 ? -1 : 1);
                        float newY = (v2avg._y / i) * -1;// * (v2avg._y >= 1 ? -1 : 1);
                        Console.WriteLine(new Vector2(newX, newY));
                        _centerState = new FrameState(new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector3(newX, newY, 0));
                    }*/
                    Vector3 v3trans = new Vector3(transform._transform.Translation._x,
                        transform._transform.Translation._y, 0);
                    Vector3 v3rot = new Vector3(0, 0, transform._transform.Rotation);
                    Vector3 v3scale = new Vector3(transform._transform.Scale._x, transform._transform.Scale._y, 1);
                    FrameState _frameState = new FrameState(v3scale, v3rot, v3trans);
                    foreach (CollisionLink l in _selectedLinks)
                    {
                        l._rawValue = _centerState._transform * _frameState._transform * l._rawValue;
                    }
                }
                else
                {
                    foreach (CollisionLink l in _selectedLinks)
                    {
                        l._rawValue += transform._transform.Translation;
                    }
                }

                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
        }

        protected void btnSameX_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
            {
                _selectedLinks[i]._rawValue._x = _selectedLinks[0]._rawValue._x;
            }

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void btnSameY_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
            {
                _selectedLinks[i]._rawValue._y = _selectedLinks[0]._rawValue._y;
            }

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void chkPoly_CheckStateChanged(object sender, EventArgs e)
        {
            _modelPanel.BeginUpdate();
            _modelPanel.RenderPolygons = chkPoly.CheckState == CheckState.Checked;
            _modelPanel.RenderWireframe = chkPoly.CheckState == CheckState.Indeterminate;
            _modelPanel.EndUpdate();
        }

        protected void chkBones_CheckedChanged(object sender, EventArgs e)
        {
            _modelPanel.RenderBones = chkBones.Checked;
        }

        protected void modelTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is MDL0Node node)
            {
                node.IsRendering = e.Node.Checked;
                if (!_updating)
                {
                    _updating = true;
                    foreach (TreeNode n in e.Node.Nodes)
                    {
                        n.Checked = e.Node.Checked;
                    }

                    _updating = false;
                }
            }
            else if (e.Node.Tag is MDL0BoneNode boneNode)
            {
                boneNode._render = e.Node.Checked;
            }

            if (!_updating)
            {
                _modelPanel.Invalidate();
            }
        }

        protected void modelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is MDL0BoneNode)
            {
                MDL0BoneNode bone = e.Node.Tag as MDL0BoneNode;
                bone._boneColor = Color.FromArgb(255, 0, 0);
                bone._nodeColor = Color.FromArgb(255, 128, 0);
                _modelPanel.Invalidate();
            }
        }

        protected void modelTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (node?.Tag is MDL0BoneNode bone)
            {
                bone._nodeColor = bone._boneColor = Color.Transparent;
                _modelPanel.Invalidate();
            }
        }

        protected void btnRelink_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (_selectedObject == null || !(node?.Tag is MDL0BoneNode bone))
            {
                return;
            }

            txtBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = bone;
            txtModel.Text = _selectedObject._modelName = node.Parent.Text;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnRelinkNoMove_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (_selectedObject == null || !(node?.Tag is MDL0BoneNode bone))
            {
                return;
            }

            txtBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = bone;
            txtModel.Text = _selectedObject._modelName = node.Parent.Text;
            if (_selectedObject._points != null)
            {
                foreach (CollisionLink l in _selectedObject._points)
                {
                    l.Value = l._rawValue;
                }
            }

            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnUnlink_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
            {
                return;
            }

            txtBone.Text = "";
            txtModel.Text = "";
            _selectedObject.LinkedBone = null;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnUnlinkNoMove_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
            {
                return;
            }

            if (_selectedObject._points != null)
            {
                foreach (CollisionLink l in _selectedObject._points)
                {
                    l._rawValue = l.Value;
                }
            }

            txtBone.Text = "";
            txtModel.Text = "";
            _selectedObject.LinkedBone = null;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedPlanes.Count > 0)
            {
                foreach (CollisionPlane plane in _selectedPlanes)
                {
                    plane.Delete();
                }

                TargetNode.SignalPropertyChange();
            }
            else if (_selectedLinks.Count > 0)
            {
                for (int i = 0; i < _selectedLinks.Count; i++)
                {
                    _selectedLinks[i].Pop();
                }

                TargetNode.SignalPropertyChange();
            }

            ClearSelection();
            SelectionModified();
            _modelPanel.Invalidate();
        }

        protected void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (!(modelTree.SelectedNode?.Tag is MDL0BoneNode))
            {
                e.Cancel = true;
            }
        }

        protected void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedObject == null)
            {
                contextMenuStrip1.Items[1].Visible = contextMenuStrip1.Items[2].Visible =
                    contextMenuStrip1.Items[3].Visible = contextMenuStrip1.Items[4].Visible =
                        contextMenuStrip1.Items[5].Visible = contextMenuStrip1.Items[6].Visible =
                            contextMenuStrip1.Items[7].Visible = false;
            }
            else
            {
                contextMenuStrip1.Items[1].Visible = contextMenuStrip1.Items[2].Visible =
                    contextMenuStrip1.Items[3].Visible = contextMenuStrip1.Items[4].Visible =
                        contextMenuStrip1.Items[5].Visible = contextMenuStrip1.Items[6].Visible =
                            contextMenuStrip1.Items[7].Visible = true;
            }
        }

        protected void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            mergeToolStripMenuItem.Visible = alignXToolStripMenuItem.Visible =
                alignYToolStripMenuItem.Visible = _selectedLinks != null && _selectedLinks.Count > 1;
            moveToNewObjectToolStripMenuItem.Visible =
                flipToolStripMenuItem.Visible = _selectedPlanes != null && _selectedPlanes.Count > 0;
            moveToNewObjectToolStripMenuItem.Visible = false;
            //contextMenuStrip3.Items[0].Visible = contextMenuStrip3.Items[1].Visible = contextMenuStrip3.Items[2].Visible = contextMenuStrip3.Items[3].Visible = contextMenuStrip3.Items[4].Visible = contextMenuStrip3.Items[6].Visible = contextMenuStrip3.Items[7].Visible = (_selectedPlanes != null && _selectedPlanes.Count > 0);
        }

        protected void snapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (!(node?.Tag is MDL0BoneNode))
            {
                return;
            }

            _snapMatrix = ((MDL0BoneNode) node.Tag)._inverseBindMatrix;
            _modelPanel.Invalidate();
        }

        protected void btnResetSnap_Click(object sender, EventArgs e)
        {
            _snapMatrix = Matrix.Identity;
            _modelPanel.Invalidate();
        }

        protected void btnFlipColl_Click(object sender, EventArgs e)
        {
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.SwapLinks();
            }

            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void CreateUndo()
        {
            CheckSaveIndex();
            if (undoSaves.Count > saveIndex)
            {
                undoSaves.RemoveRange(saveIndex, undoSaves.Count - saveIndex);
                redoSaves.Clear();
            }

            save = new CollisionState
            {
                _collisionLinks = new List<CollisionLink>(),
                _linkVectors = new List<Vector2>()
            };

            foreach (CollisionLink l in _selectedLinks)
            {
                save._collisionLinks.Add(l);
                save._linkVectors.Add(l.Value);
            }

            undoSaves.Add(save);
            btnUndo.Enabled = true;
            saveIndex++;
            save = null;
        }

        protected void CheckSaveIndex()
        {
            if (saveIndex < 0)
            {
                saveIndex = 0;
            }

            if (undoSaves.Count > 25)
            {
                undoSaves.RemoveAt(0);
                saveIndex--;
            }
        }

        protected void ClearUndoBuffer()
        {
            saveIndex = 0;
            undoSaves.Clear();
            redoSaves.Clear();
            btnUndo.Enabled = btnRedo.Enabled = false;
        }

        protected void Undo(object sender, EventArgs e)
        {
            _selectedLinks.Clear();

            save = new CollisionState();

            if (undoSaves[saveIndex - 1]._linkVectors != null) //XY Positions changed.
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
            {
                btnUndo.Enabled = false;
            }

            btnRedo.Enabled = true;

            redoSaves.Add(save);
            save = null;

            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        protected void Redo(object sender, EventArgs e)
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
            {
                btnRedo.Enabled = false;
            }

            btnUndo.Enabled = true;

            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        protected void chkObjUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject.UnknownFlag = chkObjUnk.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void chkObjIndep_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void chkObjModule_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject.ModuleControlled = chkObjModule.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void chkObjSSEUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject.UnknownSSEFlag = chkObjSSEUnk.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void btnPlayAnims_Click(object sender, EventArgs e)
        {
        }

        protected void btnPrevFrame_Click(object sender, EventArgs e)
        {
        }

        protected void btnNextFrame_Click(object sender, EventArgs e)
        {
        }

        protected void btnHelp_Click(object sender, EventArgs e)
        {
            new ModelViewerHelp().Show(this, true);
        }

        protected void btnTranslateAll_Click(object sender, EventArgs e)
        {
            if (_selectedLinks.Count == 0)
            {
                MessageBox.Show("You must select at least one collision link.");
                return;
            }

            using (TransformAttributesForm f = new TransformAttributesForm())
            {
                f.TwoDimensional = true;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Matrix m = f.GetMatrix();
                    foreach (CollisionLink link in _selectedLinks)
                    {
                        link.Value = m * link.Value;
                    }

                    TargetNode.SignalPropertyChange();
                    _modelPanel.Invalidate();
                }
            }
        }
    }
}
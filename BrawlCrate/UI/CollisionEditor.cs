using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CollisionEditor));
            undoToolStrip = new SplitContainer();
            redoToolStrip = new SplitContainer();
            modelTree = new TreeView();
            contextMenuStrip2 = new ContextMenuStrip(components);
            assignToolStripMenuItem = new ToolStripMenuItem();
            snapToolStripMenuItem1 = new ToolStripMenuItem();
            panel2 = new Panel();
            chkBones = new CheckBox();
            chkPoly = new CheckBox();
            chkAllModels = new CheckBox();
            lstObjects = new CheckedListBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            newObjectToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            snapToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            panel3 = new Panel();
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
            pnlPointProps = new Panel();
            label2 = new Label();
            numY = new NumericInputBox();
            label1 = new Label();
            numX = new NumericInputBox();
            pnlPlaneProps = new Panel();
            groupBox2 = new GroupBox();
            cboType = new ComboBox();
            chkTypeItems = new CheckBox();
            chkTypeCharacters = new CheckBox();
            chkTypePokemonTrainer = new CheckBox();
            chkTypeRotating = new CheckBox();
            groupBox1 = new GroupBox();
            chkLeftLedge = new CheckBox();
            chkNoWalljump = new CheckBox();
            chkRightLedge = new CheckBox();
            chkFallThrough = new CheckBox();

            // Advanced flags
            groupBoxUnknownFlags = new GroupBox();
            chkFlagUnknown1 = new CheckBox();
            chkFlagUnknown2 = new CheckBox();
            chkFlagUnknown3 = new CheckBox();
            chkFlagUnknown4 = new CheckBox();

            cboMaterial = new ComboBox();
            label5 = new Label();
            labelType = new Label();
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
            btnSplit = new ToolStripButton();
            btnMerge = new ToolStripButton();
            btnDelete = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnSameX = new ToolStripButton();
            btnSameY = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();

            // StageBox Camera Modes
            btnPerspectiveCam = new ToolStripButton();
            btnOrthographicCam = new ToolStripButton();
            toolStripSeparatorCamera = new ToolStripSeparator();

            btnResetCam = new ToolStripButton();
            btnResetSnap = new ToolStripButton();
            btnHelp = new ToolStripButton();
            btnResetRot = new Button();
            trackBar1 = new TrackBar();
            ((ISupportInitialize)(undoToolStrip)).BeginInit();
            undoToolStrip.Panel1.SuspendLayout();
            undoToolStrip.Panel2.SuspendLayout();
            undoToolStrip.SuspendLayout();
            ((ISupportInitialize)(redoToolStrip)).BeginInit();
            redoToolStrip.Panel1.SuspendLayout();
            redoToolStrip.Panel2.SuspendLayout();
            redoToolStrip.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            panel2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel3.SuspendLayout();
            pnlObjProps.SuspendLayout();
            pnlPointProps.SuspendLayout();
            pnlPlaneProps.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBoxUnknownFlags.SuspendLayout();
            groupBox1.SuspendLayout();
            panel4.SuspendLayout();
            panel1.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((ISupportInitialize)(trackBar1)).BeginInit();
            SuspendLayout();
            // 
            // undoToolStrip
            // 
            undoToolStrip.Dock = DockStyle.Fill;
            undoToolStrip.FixedPanel = FixedPanel.Panel1;
            undoToolStrip.Location = new Drawing.Point(0, 0);
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
            undoToolStrip.Size = new Drawing.Size(694, 467);
            undoToolStrip.SplitterDistance = 209;
            undoToolStrip.TabIndex = 1;
            // 
            // redoToolStrip
            // 
            redoToolStrip.Dock = DockStyle.Fill;
            redoToolStrip.Location = new Drawing.Point(0, 0);
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
            redoToolStrip.Size = new Drawing.Size(209, 467);
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
            modelTree.Location = new Drawing.Point(0, 17);
            modelTree.Name = "modelTree";
            modelTree.Size = new Drawing.Size(209, 225);
            modelTree.TabIndex = 4;
            modelTree.AfterCheck += new TreeViewEventHandler(modelTree_AfterCheck);
            modelTree.BeforeSelect += new TreeViewCancelEventHandler(modelTree_BeforeSelect);
            modelTree.AfterSelect += new TreeViewEventHandler(modelTree_AfterSelect);
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] {
            assignToolStripMenuItem,
            snapToolStripMenuItem1});
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Drawing.Size(110, 48);
            contextMenuStrip2.Opening += new CancelEventHandler(contextMenuStrip2_Opening);
            // 
            // assignToolStripMenuItem
            // 
            assignToolStripMenuItem.Name = "assignToolStripMenuItem";
            assignToolStripMenuItem.Size = new Drawing.Size(109, 22);
            assignToolStripMenuItem.Text = "Assign";
            assignToolStripMenuItem.Click += new EventHandler(btnRelink_Click);
            // 
            // snapToolStripMenuItem1
            // 
            snapToolStripMenuItem1.Name = "snapToolStripMenuItem1";
            snapToolStripMenuItem1.Size = new Drawing.Size(109, 22);
            snapToolStripMenuItem1.Text = "Snap";
            snapToolStripMenuItem1.Click += new EventHandler(snapToolStripMenuItem1_Click);
            // 
            // panel2
            // 
            panel2.Controls.Add(chkBones);
            panel2.Controls.Add(chkPoly);
            panel2.Controls.Add(chkAllModels);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Drawing.Size(209, 17);
            panel2.TabIndex = 3;
            // 
            // chkBones
            // 
            chkBones.Location = new Drawing.Point(100, 0);
            chkBones.Name = "chkBones";
            chkBones.Padding = new Padding(1, 0, 0, 0);
            chkBones.Size = new Drawing.Size(67, 17);
            chkBones.TabIndex = 4;
            chkBones.Text = "Bones";
            chkBones.UseVisualStyleBackColor = true;
            chkBones.CheckedChanged += new EventHandler(chkBones_CheckedChanged);
            // 
            // chkPoly
            // 
            chkPoly.Checked = true;
            chkPoly.CheckState = CheckState.Checked;
            chkPoly.Location = new Drawing.Point(44, 0);
            chkPoly.Name = "chkPoly";
            chkPoly.Padding = new Padding(1, 0, 0, 0);
            chkPoly.Size = new Drawing.Size(54, 17);
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
            chkAllModels.Location = new Drawing.Point(0, 0);
            chkAllModels.Name = "chkAllModels";
            chkAllModels.Padding = new Padding(1, 0, 0, 0);
            chkAllModels.Size = new Drawing.Size(41, 17);
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
            lstObjects.Location = new Drawing.Point(0, 0);
            lstObjects.Name = "lstObjects";
            lstObjects.Size = new Drawing.Size(209, 82);
            lstObjects.TabIndex = 1;
            lstObjects.ItemCheck += new ItemCheckEventHandler(lstObjects_ItemCheck);
            lstObjects.SelectedValueChanged += new EventHandler(lstObjects_SelectedValueChanged);
            lstObjects.MouseDown += new MouseEventHandler(lstObjects_MouseDown);
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
            newObjectToolStripMenuItem,
            toolStripMenuItem2,
            snapToolStripMenuItem,
            toolStripMenuItem1,
            deleteToolStripMenuItem});
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Drawing.Size(137, 82);
            // 
            // newObjectToolStripMenuItem
            // 
            newObjectToolStripMenuItem.Name = "newObjectToolStripMenuItem";
            newObjectToolStripMenuItem.Size = new Drawing.Size(136, 22);
            newObjectToolStripMenuItem.Text = "New Object";
            newObjectToolStripMenuItem.Click += new EventHandler(newObjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Drawing.Size(133, 6);
            // 
            // snapToolStripMenuItem
            // 
            snapToolStripMenuItem.Name = "snapToolStripMenuItem";
            snapToolStripMenuItem.Size = new Drawing.Size(136, 22);
            snapToolStripMenuItem.Text = "Snap";
            snapToolStripMenuItem.Click += new EventHandler(snapToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Drawing.Size(133, 6);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Drawing.Size(136, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
            deleteToolStripMenuItem.ShortcutKeys = Keys.Shift | Keys.Delete;
            // 
            // panel3
            // 
            panel3.Controls.Add(pnlPlaneProps);
            panel3.Controls.Add(pnlPointProps);
            panel3.Controls.Add(pnlObjProps);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Drawing.Point(0, 82);
            panel3.Name = "panel3";
            panel3.Size = new Drawing.Size(209, 115);
            panel3.TabIndex = 16;
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
            pnlObjProps.Location = new Drawing.Point(0, -15);
            pnlObjProps.Name = "pnlObjProps";
            pnlObjProps.Size = new Drawing.Size(209, 130);
            pnlObjProps.TabIndex = 1;
            pnlObjProps.Visible = false;
            // 
            // chkObjSSEUnk
            // 
            chkObjSSEUnk.AutoSize = true;
            chkObjSSEUnk.Location = new Drawing.Point(10, 102);
            chkObjSSEUnk.Name = "chkObjSSEUnk";
            chkObjSSEUnk.Size = new Drawing.Size(96, 17);
            chkObjSSEUnk.TabIndex = 15;
            chkObjSSEUnk.Text = "SSE Unknown";
            chkObjSSEUnk.UseVisualStyleBackColor = true;
            chkObjSSEUnk.CheckedChanged += new EventHandler(chkObjSSEUnk_CheckedChanged);
            // 
            // chkObjModule
            // 
            chkObjModule.AutoSize = true;
            chkObjModule.Location = new Drawing.Point(10, 79);
            chkObjModule.Name = "chkObjModule";
            chkObjModule.Size = new Drawing.Size(111, 17);
            chkObjModule.TabIndex = 14;
            chkObjModule.Text = "Module Controlled";
            chkObjModule.UseVisualStyleBackColor = true;
            chkObjModule.CheckedChanged += new EventHandler(chkObjModule_CheckedChanged);
            // 
            // chkObjUnk
            // 
            chkObjUnk.AutoSize = true;
            chkObjUnk.Location = new Drawing.Point(10, 56);
            chkObjUnk.Name = "chkObjUnk";
            chkObjUnk.Size = new Drawing.Size(72, 17);
            chkObjUnk.TabIndex = 13;
            chkObjUnk.Text = "Unknown";
            chkObjUnk.UseVisualStyleBackColor = true;
            chkObjUnk.CheckedChanged += new EventHandler(chkObjUnk_CheckedChanged);
            // 
            // btnUnlink
            // 
            btnUnlink.Location = new Drawing.Point(177, 22);
            btnUnlink.Name = "btnUnlink";
            btnUnlink.Size = new Drawing.Size(28, 21);
            btnUnlink.TabIndex = 12;
            btnUnlink.Text = "-";
            btnUnlink.UseVisualStyleBackColor = true;
            btnUnlink.Click += new EventHandler(btnUnlink_Click);
            // 
            // btnRelink
            // 
            btnRelink.Location = new Drawing.Point(177, 2);
            btnRelink.Name = "btnRelink";
            btnRelink.Size = new Drawing.Size(28, 21);
            btnRelink.TabIndex = 4;
            btnRelink.Text = "+";
            btnRelink.UseVisualStyleBackColor = true;
            btnRelink.Click += new EventHandler(btnRelink_Click);
            // 
            // txtBone
            // 
            txtBone.Location = new Drawing.Point(49, 23);
            txtBone.Name = "txtBone";
            txtBone.ReadOnly = true;
            txtBone.Size = new Drawing.Size(126, 20);
            txtBone.TabIndex = 3;
            // 
            // label4
            // 
            label4.Location = new Drawing.Point(4, 23);
            label4.Name = "label4";
            label4.Size = new Drawing.Size(42, 20);
            label4.TabIndex = 2;
            label4.Text = "Bone:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtModel
            // 
            txtModel.Location = new Drawing.Point(49, 3);
            txtModel.Name = "txtModel";
            txtModel.ReadOnly = true;
            txtModel.Size = new Drawing.Size(126, 20);
            txtModel.TabIndex = 1;
            // 
            // label3
            // 
            label3.Location = new Drawing.Point(4, 3);
            label3.Name = "label3";
            label3.Size = new Drawing.Size(42, 20);
            label3.TabIndex = 0;
            label3.Text = "Model:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlPointProps
            // 
            pnlPointProps.Controls.Add(label2);
            pnlPointProps.Controls.Add(numY);
            pnlPointProps.Controls.Add(label1);
            pnlPointProps.Controls.Add(numX);
            pnlPointProps.Dock = DockStyle.Bottom;
            pnlPointProps.Location = new Drawing.Point(0, -85);
            pnlPointProps.Name = "pnlPointProps";
            pnlPointProps.Size = new Drawing.Size(209, 70);
            pnlPointProps.TabIndex = 15;
            pnlPointProps.Visible = false;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Drawing.Point(18, 32);
            label2.Name = "label2";
            label2.Size = new Drawing.Size(42, 20);
            label2.TabIndex = 3;
            label2.Text = "Y";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numY
            // 
            numY.BorderStyle = BorderStyle.FixedSingle;
            numY.Integral = false;
            numY.Location = new Drawing.Point(59, 32);
            numY.MaximumValue = 3.402823E+38F;
            numY.MinimumValue = -3.402823E+38F;
            numY.Name = "numY";
            numY.Size = new Drawing.Size(100, 20);
            numY.TabIndex = 2;
            numY.Text = "0";
            numY.ValueChanged += new EventHandler(numY_ValueChanged);
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Drawing.Point(18, 13);
            label1.Name = "label1";
            label1.Size = new Drawing.Size(42, 20);
            label1.TabIndex = 1;
            label1.Text = "X";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numX
            // 
            numX.BorderStyle = BorderStyle.FixedSingle;
            numX.Integral = false;
            numX.Location = new Drawing.Point(59, 13);
            numX.MaximumValue = 3.402823E+38F;
            numX.MinimumValue = -3.402823E+38F;
            numX.Name = "numX";
            numX.Size = new Drawing.Size(100, 20);
            numX.TabIndex = 0;
            numX.Text = "0";
            numX.ValueChanged += new EventHandler(numX_ValueChanged);
            // 
            // pnlPlaneProps
            // 
            pnlPlaneProps.Controls.Add(groupBoxUnknownFlags);
            pnlPlaneProps.Controls.Add(groupBox2);
            pnlPlaneProps.Controls.Add(groupBox1);
            pnlPlaneProps.Controls.Add(cboMaterial);
            pnlPlaneProps.Controls.Add(cboType);
            pnlPlaneProps.Controls.Add(label5);
            pnlPlaneProps.Controls.Add(labelType);
            pnlPlaneProps.Dock = DockStyle.Bottom;
            pnlPlaneProps.Location = new Drawing.Point(0, -199);
            pnlPlaneProps.Name = "pnlPlaneProps";
            pnlPlaneProps.Size = new Drawing.Size(209, 114);
            pnlPlaneProps.TabIndex = 0;
            pnlPlaneProps.Visible = false;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left);
            groupBox2.Controls.Add(chkTypeCharacters);
            groupBox2.Controls.Add(chkTypeItems);
            groupBox2.Controls.Add(chkTypePokemonTrainer);
            groupBox2.Controls.Add(chkTypeRotating);
            groupBox2.Location = new Drawing.Point(101, 49);
            groupBox2.Margin = new Padding(0);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(0);
            groupBox2.Size = new Drawing.Size(105, 86);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            // 
            // cboType
            // 
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.FormattingEnabled = true;
            cboType.Location = new Drawing.Point(66, 4);
            cboType.Name = "cboType";
            cboType.Size = new Drawing.Size(139, 21);
            cboType.TabIndex = 5;
            cboType.SelectedIndexChanged += new EventHandler(cboType_SelectedIndexChanged);
            // 
            // groupBoxUnknownFlags
            // 
            groupBoxUnknownFlags.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left);
            groupBoxUnknownFlags.Controls.Add(chkFlagUnknown4);
            groupBoxUnknownFlags.Controls.Add(chkFlagUnknown3);
            groupBoxUnknownFlags.Controls.Add(chkFlagUnknown2);
            groupBoxUnknownFlags.Controls.Add(chkFlagUnknown1);
            groupBoxUnknownFlags.Location = new Drawing.Point(0, 135);
            groupBoxUnknownFlags.Margin = new Padding(0);
            groupBoxUnknownFlags.Name = "groupBoxUnknownFlags";
            groupBoxUnknownFlags.Padding = new Padding(0);
            groupBoxUnknownFlags.Size = new Drawing.Size(205, 59);
            groupBoxUnknownFlags.TabIndex = 14;
            groupBoxUnknownFlags.TabStop = false;
            groupBoxUnknownFlags.Text = "Unknown Flags";
            // 
            // chkFlagUnknown1
            // 
            chkFlagUnknown1.Location = new Drawing.Point(8, 17);
            chkFlagUnknown1.Margin = new Padding(0);
            chkFlagUnknown1.Name = "chkFlagUnknown1";
            chkFlagUnknown1.Size = new Drawing.Size(86, 18);
            chkFlagUnknown1.TabIndex = 3;
            chkFlagUnknown1.Text = "1";
            chkFlagUnknown1.UseVisualStyleBackColor = true;
            chkFlagUnknown1.CheckedChanged += new EventHandler(chkFlagUnknown1_CheckedChanged);
            // 
            // chkFlagUnknown2
            // 
            chkFlagUnknown2.Location = new Drawing.Point(60, 17);
            chkFlagUnknown2.Margin = new Padding(0);
            chkFlagUnknown2.Name = "chkFlagUnknown2";
            chkFlagUnknown2.Size = new Drawing.Size(86, 18);
            chkFlagUnknown2.TabIndex = 3;
            chkFlagUnknown2.Text = "2";
            chkFlagUnknown2.UseVisualStyleBackColor = true;
            chkFlagUnknown2.CheckedChanged += new EventHandler(chkFlagUnknown2_CheckedChanged);
            // 
            // chkFlagUnknown3
            // 
            chkFlagUnknown3.Location = new Drawing.Point(112, 17);
            chkFlagUnknown3.Margin = new Padding(0);
            chkFlagUnknown3.Name = "chkFlagUnknown3";
            chkFlagUnknown3.Size = new Drawing.Size(86, 18);
            chkFlagUnknown3.TabIndex = 3;
            chkFlagUnknown3.Text = "3";
            chkFlagUnknown3.UseVisualStyleBackColor = true;
            chkFlagUnknown3.CheckedChanged += new EventHandler(chkFlagUnknown3_CheckedChanged);
            // 
            // chkFlagUnknown4
            // 
            chkFlagUnknown4.Location = new Drawing.Point(164, 17);
            chkFlagUnknown4.Margin = new Padding(0);
            chkFlagUnknown4.Name = "chkFlagUnknown4";
            chkFlagUnknown4.Size = new Drawing.Size(86, 18);
            chkFlagUnknown4.TabIndex = 3;
            chkFlagUnknown4.Text = "4";
            chkFlagUnknown4.UseVisualStyleBackColor = true;
            chkFlagUnknown4.CheckedChanged += new EventHandler(chkFlagUnknown4_CheckedChanged);
            // 
            // chkTypeCharacters
            // 
            chkTypeCharacters.Location = new Drawing.Point(8, 17);
            chkTypeCharacters.Margin = new Padding(0);
            chkTypeCharacters.Name = "chkTypeCharacters";
            chkTypeCharacters.Size = new Drawing.Size(86, 18);
            chkTypeCharacters.TabIndex = 4;
            chkTypeCharacters.Text = "Characters";
            chkTypeCharacters.UseVisualStyleBackColor = true;
            chkTypeCharacters.CheckedChanged += new EventHandler(chkTypeCharacters_CheckedChanged);
            // 
            // chkTypeItems
            // 
            chkTypeItems.Location = new Drawing.Point(8, 33);
            chkTypeItems.Margin = new Padding(0);
            chkTypeItems.Name = "chkTypeItems";
            chkTypeItems.Size = new Drawing.Size(86, 18);
            chkTypeItems.TabIndex = 3;
            chkTypeItems.Text = "Items";
            chkTypeItems.UseVisualStyleBackColor = true;
            chkTypeItems.CheckedChanged += new EventHandler(chkTypeItems_CheckedChanged);
            // 
            // chkTypePokemonTrainer
            // 
            chkTypePokemonTrainer.Location = new Drawing.Point(8, 49);
            chkTypePokemonTrainer.Margin = new Padding(0);
            chkTypePokemonTrainer.Name = "chkTypePokemonTrainer";
            chkTypePokemonTrainer.Size = new Drawing.Size(86, 18);
            chkTypePokemonTrainer.TabIndex = 3;
            chkTypePokemonTrainer.Text = "PokéTrainer";
            chkTypePokemonTrainer.UseVisualStyleBackColor = true;
            chkTypePokemonTrainer.CheckedChanged += new EventHandler(chkTypePokemonTrainer_CheckedChanged);
            // 
            // chkTypeRotating
            // 
            chkTypeRotating.Location = new Drawing.Point(8, 65);
            chkTypeRotating.Margin = new Padding(0);
            chkTypeRotating.Name = "chkTypeRotating";
            chkTypeRotating.Size = new Drawing.Size(86, 18);
            chkTypeRotating.TabIndex = 4;
            chkTypeRotating.Text = "Rotating";
            chkTypeRotating.UseVisualStyleBackColor = true;
            chkTypeRotating.CheckedChanged += new EventHandler(chkTypeRotating_CheckedChanged);
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left);
            groupBox1.Controls.Add(chkLeftLedge);
            groupBox1.Controls.Add(chkNoWalljump);
            groupBox1.Controls.Add(chkRightLedge);
            groupBox1.Controls.Add(chkFallThrough);
            groupBox1.Location = new Drawing.Point(-3, 49);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(0);
            groupBox1.Size = new Drawing.Size(104, 86);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Flags";
            // 
            // chkLeftLedge
            // 
            chkLeftLedge.Location = new Drawing.Point(8, 33);
            chkLeftLedge.Margin = new Padding(0);
            chkLeftLedge.Name = "chkLeftLedge";
            chkLeftLedge.Size = new Drawing.Size(86, 18);
            chkLeftLedge.TabIndex = 4;
            chkLeftLedge.Text = "Left Ledge";
            chkLeftLedge.UseVisualStyleBackColor = true;
            chkLeftLedge.CheckedChanged += new EventHandler(chkLeftLedge_CheckedChanged);
            // 
            // chkNoWalljump
            // 
            chkNoWalljump.Location = new Drawing.Point(8, 65);
            chkNoWalljump.Margin = new Padding(0);
            chkNoWalljump.Name = "chkNoWalljump";
            chkNoWalljump.Size = new Drawing.Size(90, 18);
            chkNoWalljump.TabIndex = 2;
            chkNoWalljump.Text = "No Walljump";
            chkNoWalljump.UseVisualStyleBackColor = true;
            chkNoWalljump.CheckedChanged += new EventHandler(chkNoWalljump_CheckedChanged);
            // 
            // chkRightLedge
            // 
            chkRightLedge.Location = new Drawing.Point(8, 49);
            chkRightLedge.Margin = new Padding(0);
            chkRightLedge.Name = "chkRightLedge";
            chkRightLedge.Size = new Drawing.Size(86, 18);
            chkRightLedge.TabIndex = 1;
            chkRightLedge.Text = "Right Ledge";
            chkRightLedge.UseVisualStyleBackColor = true;
            chkRightLedge.CheckedChanged += new EventHandler(chkRightLedge_CheckedChanged);
            // 
            // chkFallThrough
            // 
            chkFallThrough.Location = new Drawing.Point(8, 17);
            chkFallThrough.Margin = new Padding(0);
            chkFallThrough.Name = "chkFallThrough";
            chkFallThrough.Size = new Drawing.Size(90, 18);
            chkFallThrough.TabIndex = 0;
            chkFallThrough.Text = "Fall-Through";
            chkFallThrough.UseVisualStyleBackColor = true;
            chkFallThrough.CheckedChanged += new EventHandler(chkFallThrough_CheckedChanged);
            // 
            // cboMaterial
            // 
            cboMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaterial.FormattingEnabled = true;
            cboMaterial.Location = new Drawing.Point(66, 25);
            cboMaterial.Name = "cboMaterial";
            cboMaterial.Size = new Drawing.Size(139, 21);
            cboMaterial.TabIndex = 12;
            cboMaterial.SelectedIndexChanged += new EventHandler(cboMaterial_SelectedIndexChanged);
            // 
            // label5
            // 
            label5.Location = new Drawing.Point(7, 25);
            label5.Name = "label5";
            label5.Size = new Drawing.Size(53, 21);
            label5.TabIndex = 8;
            label5.Text = "Material:";
            label5.TextAlign = ContentAlignment.MiddleRight;

            // 
            // labelType
            // 
            labelType.Location = new Drawing.Point(7, 4);
            labelType.Name = "labelType";
            labelType.Size = new Drawing.Size(53, 21);
            labelType.TabIndex = 8;
            labelType.Text = "Type:";
            labelType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnPlayAnims);
            panel4.Controls.Add(btnPrevFrame);
            panel4.Controls.Add(btnNextFrame);
            panel4.Dock = DockStyle.Bottom;
            panel4.Enabled = false;
            panel4.Location = new Drawing.Point(0, 197);
            panel4.Name = "panel4";
            panel4.Size = new Drawing.Size(209, 24);
            panel4.TabIndex = 17;
            panel4.Visible = false;
            // 
            // btnPlayAnims
            // 
            btnPlayAnims.Dock = DockStyle.Fill;
            btnPlayAnims.Location = new Drawing.Point(35, 0);
            btnPlayAnims.Name = "btnPlayAnims";
            btnPlayAnims.Size = new Drawing.Size(139, 24);
            btnPlayAnims.TabIndex = 16;
            btnPlayAnims.Text = "Play Animations";
            btnPlayAnims.UseVisualStyleBackColor = true;
            btnPlayAnims.Click += new EventHandler(btnPlayAnims_Click);
            // 
            // btnPrevFrame
            // 
            btnPrevFrame.Dock = DockStyle.Left;
            btnPrevFrame.Location = new Drawing.Point(0, 0);
            btnPrevFrame.Name = "btnPrevFrame";
            btnPrevFrame.Size = new Drawing.Size(35, 24);
            btnPrevFrame.TabIndex = 18;
            btnPrevFrame.Text = "<";
            btnPrevFrame.UseVisualStyleBackColor = true;
            btnPrevFrame.Click += new EventHandler(btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            btnNextFrame.Dock = DockStyle.Right;
            btnNextFrame.Location = new Drawing.Point(174, 0);
            btnNextFrame.Name = "btnNextFrame";
            btnNextFrame.Size = new Drawing.Size(35, 24);
            btnNextFrame.TabIndex = 17;
            btnNextFrame.Text = ">";
            btnNextFrame.UseVisualStyleBackColor = true;
            btnNextFrame.Click += new EventHandler(btnNextFrame_Click);
            // 
            // _modelPanel
            // 
            _modelPanel.Dock = DockStyle.Fill;
            _modelPanel.Location = new Drawing.Point(0, 25);
            _modelPanel.Name = "_modelPanel";
            _modelPanel.Size = new Drawing.Size(481, 442);
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
            panel1.Location = new Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Drawing.Size(481, 25);
            panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.WhiteSmoke;
            toolStrip1.Dock = DockStyle.Fill;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] {
            btnUndo,
            btnRedo,
            toolStripSeparator3,
            btnSplit,
            btnMerge,
            btnDelete,
            toolStripSeparator2,
            btnSameX,
            btnSameY,
            toolStripSeparator1,

            btnPerspectiveCam,
            btnOrthographicCam,
            toolStripSeparatorCamera,

            btnResetCam,
            btnResetSnap,
            btnHelp});
            toolStrip1.Location = new Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Drawing.Size(335, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnUndo
            // 
            btnUndo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnUndo.Enabled = false;
            btnUndo.ImageTransparentColor = Color.Magenta;
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Drawing.Size(40, 22);
            btnUndo.Text = "Undo";
            btnUndo.Click += new EventHandler(Undo);
            // 
            // btnRedo
            // 
            btnRedo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnRedo.Enabled = false;
            btnRedo.ImageTransparentColor = Color.Magenta;
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Drawing.Size(38, 22);
            btnRedo.Text = "Redo";
            btnRedo.Click += new EventHandler(Redo);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Drawing.Size(6, 25);
            // 
            // btnSplit
            // 
            btnSplit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSplit.Enabled = false;
            btnSplit.ImageTransparentColor = Color.Magenta;
            btnSplit.Name = "btnSplit";
            btnSplit.Size = new Drawing.Size(34, 22);
            btnSplit.Text = "Split";
            btnSplit.Click += new EventHandler(btnSplit_Click);
            // 
            // btnMerge
            // 
            btnMerge.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnMerge.Enabled = false;
            btnMerge.ImageTransparentColor = Color.Magenta;
            btnMerge.Name = "btnMerge";
            btnMerge.Size = new Drawing.Size(45, 22);
            btnMerge.Text = "Merge";
            btnMerge.Click += new EventHandler(btnMerge_Click);
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnDelete.Enabled = false;
            btnDelete.ImageTransparentColor = Color.Magenta;
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Drawing.Size(44, 22);
            btnDelete.Text = "Delete";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Drawing.Size(6, 25);
            // 
            // btnSameX
            // 
            btnSameX.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSameX.ImageTransparentColor = Color.Magenta;
            btnSameX.Name = "btnSameX";
            btnSameX.Size = new Drawing.Size(49, 22);
            btnSameX.Text = "Align X";
            btnSameX.Click += new EventHandler(btnSameX_Click);
            // 
            // btnSameY
            // 
            btnSameY.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSameY.ImageTransparentColor = Color.Magenta;
            btnSameY.Name = "btnSameY";
            btnSameY.Size = new Drawing.Size(49, 22);
            btnSameY.Text = "Align Y";
            btnSameY.Click += new EventHandler(btnSameY_Click);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Drawing.Size(6, 25);
            // 
            // btnPerspectiveCam
            // 
            btnPerspectiveCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnPerspectiveCam.ImageTransparentColor = Color.Magenta;
            btnPerspectiveCam.Name = "btnPerspectiveCam";
            btnPerspectiveCam.Size = new Drawing.Size(83, 19);
            btnPerspectiveCam.Text = "Perspective";
            btnPerspectiveCam.Click += new EventHandler(btnPerspectiveCam_Click);
            // 
            // btnOrthographicCam
            // 
            btnOrthographicCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnOrthographicCam.ImageTransparentColor = Color.Magenta;
            btnOrthographicCam.Name = "btnOrthographicCam";
            btnOrthographicCam.Size = new Drawing.Size(83, 19);
            btnOrthographicCam.Text = "Orthographic";
            btnOrthographicCam.Click += new EventHandler(btnOrthographicCam_Click);
            // 
            // toolStripSeparatorCamera (StageBox)
            // 
            toolStripSeparator1.Name = "toolStripSeparatorCamera";
            toolStripSeparator1.Size = new Drawing.Size(6, 25);
            // 
            // btnResetCam
            // 
            btnResetCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnResetCam.ImageTransparentColor = Color.Magenta;
            btnResetCam.Name = "btnResetCam";
            btnResetCam.Size = new Drawing.Size(83, 19);
            btnResetCam.Text = "Reset Camera";
            btnResetCam.Click += new EventHandler(btnResetCam_Click);
            // 
            // btnResetSnap
            // 
            btnResetSnap.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnResetSnap.ImageTransparentColor = Color.Magenta;
            btnResetSnap.Name = "btnResetSnap";
            btnResetSnap.Size = new Drawing.Size(57, 19);
            btnResetSnap.Text = "Un-Snap";
            btnResetSnap.Click += new EventHandler(btnResetSnap_Click);
            // 
            // btnHelp
            // 
            btnHelp.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnHelp.Image = ((Image)(resources.GetObject("btnHelp.Image")));
            btnHelp.ImageTransparentColor = Color.Magenta;
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Drawing.Size(36, 19);
            btnHelp.Text = "Help";
            btnHelp.Click += new EventHandler(btnHelp_Click);
            // 
            // btnResetRot
            // 
            btnResetRot.Dock = DockStyle.Right;
            btnResetRot.Enabled = false;
            btnResetRot.FlatAppearance.BorderSize = 0;
            btnResetRot.FlatStyle = FlatStyle.Flat;
            btnResetRot.Location = new Drawing.Point(335, 0);
            btnResetRot.Name = "btnResetRot";
            btnResetRot.Size = new Drawing.Size(16, 25);
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
            trackBar1.Location = new Drawing.Point(351, 0);
            trackBar1.Maximum = 180;
            trackBar1.Minimum = -180;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Drawing.Size(130, 25);
            trackBar1.TabIndex = 3;
            trackBar1.TickStyle = TickStyle.None;
            trackBar1.Visible = false;
            trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
            // 
            // CollisionEditor
            // 
            BackColor = Color.Lavender;
            Controls.Add(undoToolStrip);
            Name = "CollisionEditor";
            Size = new Drawing.Size(694, 467);
            undoToolStrip.Panel1.ResumeLayout(false);
            undoToolStrip.Panel2.ResumeLayout(false);
            ((ISupportInitialize)(undoToolStrip)).EndInit();
            undoToolStrip.ResumeLayout(false);
            redoToolStrip.Panel1.ResumeLayout(false);
            redoToolStrip.Panel2.ResumeLayout(false);
            ((ISupportInitialize)(redoToolStrip)).EndInit();
            redoToolStrip.ResumeLayout(false);
            contextMenuStrip2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            pnlObjProps.ResumeLayout(false);
            pnlObjProps.PerformLayout();
            pnlPointProps.ResumeLayout(false);
            pnlPointProps.PerformLayout();
            pnlPlaneProps.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBoxUnknownFlags.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((ISupportInitialize)(trackBar1)).EndInit();
            ResumeLayout(false);
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
            get => _targetNode;
            set => TargetChanged(value);
        }

        private bool _updating;
        private CollisionObject _selectedObject;
        private Matrix _snapMatrix;

        private bool _hovering;
        private readonly List<CollisionLink> _selectedLinks = new List<CollisionLink>();
        private readonly List<CollisionPlane> _selectedPlanes = new List<CollisionPlane>();

        private bool _selecting, _selectInverse;
        private Vector3 _selectStart, _selectLast, _selectEnd;
        private bool _creating;

        private CollisionState save;
        private readonly List<CollisionState> undoSaves = new List<CollisionState>();
        private readonly List<CollisionState> redoSaves = new List<CollisionState>();
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
                if ((byte)p._material >= 32)
                {
                    // Select basic by default (currently cannot display expanded collisions in default previewer)
                    cboMaterial.SelectedItem = (CollisionPlaneMaterial)(0x0);
                }
                else
                {
                    // Otherwise convert to the proper place in the unexpanded list
                    cboMaterial.SelectedItem = p._material;
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
            {
                foreach (MDL0Node n in _targetNode._parent.FindChildrenByType(null, ResourceType.MDL0))
                {
                    TreeNode modelNode = new TreeNode(n._name) { Tag = n, Checked = true };
                    modelTree.Nodes.Add(modelNode);

                    foreach (MDL0BoneNode bone in n._linker.BoneCache)
                    {
                        modelNode.Nodes.Add(new TreeNode(bone._name) { Tag = bone, Checked = true });
                    }

                    _modelPanel.AddTarget(n);
                    n.ResetToBindState();
                }
            }

            modelTree.EndUpdate();
        }

        #region Object List

        private void PopulateObjectList()
        {
            lstObjects.BeginUpdate();
            lstObjects.Items.Clear();

            if (_targetNode != null)
            {
                foreach (CollisionObject obj in _targetNode._objects)
                {
                    obj._render = true;
                    lstObjects.Items.Add(obj, true);

                    if (!obj._flags[1])
                    {
                        foreach (TreeNode n in modelTree.Nodes)
                        {
                            foreach (TreeNode b in n.Nodes)
                            {
                                MDL0BoneNode bone = b.Tag as MDL0BoneNode;
                                if (bone != null && bone.Name == obj._boneName && bone.BoneIndex == obj._boneIndex)
                                {
                                    obj._linkedBone = bone;
                                }
                            }
                        }
                    }
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
            {
                return;
            }

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
                                _snapMatrix = ((MDL0BoneNode)bNode.Tag)._inverseBindMatrix;
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
                if ((obj._modelName == _selectedObject._modelName) && (obj._boneName == _selectedObject._boneName))
                {
                    lstObjects.SetItemChecked(i, true);
                }
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
            {
                _modelPanel.Invalidate();
            }
        }


        #endregion

        private void ClearSelection()
        {
            foreach (CollisionLink l in _selectedLinks)
            {
                l._highlight = false;
            }

            _selectedLinks.Clear();
            _selectedPlanes.Clear();
        }

        private void UpdateSelection(bool finish)
        {
            foreach (CollisionObject obj in _targetNode._objects)
            {
                foreach (CollisionLink link in obj._points)
                {
                    link._highlight = false;
                    if (!obj._render)
                    {
                        continue;
                    }

                    Vector3 point = (Vector3)link.Value;

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
            if (_selecting || _hovering || (_selectedLinks.Count == 0))
            {
                btnMerge.Enabled = btnSplit.Enabled = btnSameX.Enabled = btnSameY.Enabled = false;
            }
            else
            {
                btnMerge.Enabled = btnSameX.Enabled = btnSameY.Enabled = _selectedLinks.Count > 1;
                btnSplit.Enabled = true;
            }
        }

        private void _treeObjects_AfterCheck(object sender, TreeViewEventArgs e)
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

        private void chkAllModels_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in modelTree.Nodes)
            {
                node.Checked = chkAllModels.Checked;
            }
        }

        private void BeginHover(Vector3 point)
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
        private void UpdateHover(int x, int y)
        {
            if (!_hovering)
            {
                return;
            }

            _selectEnd = Vector3.IntersectZ(_modelPanel.CurrentViewport.UnProject(x, y, 0.0f), _modelPanel.CurrentViewport.UnProject(x, y, 1.0f), _selectLast._z);

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
        private void CancelHover()
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
        private void FinishHover() { _hovering = false; }
        private void BeginSelection(Vector3 point, bool inverse)
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
        private void CancelSelection()
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
        private void FinishSelection()
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

        private void _modelPanel_MouseDown(object sender, MouseEventArgs e)
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

                if (!move && (depth < 1.0f))
                {
                    point = (Vector2)target;

                    //Hit-detect points first
                    foreach (CollisionObject obj in _targetNode._objects)
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

                                    if ((!add) && (!subtract))
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
                    foreach (CollisionObject obj in _targetNode._objects)
                    {
                        if (obj._render)
                        {
                            foreach (CollisionPlane p in obj._planes)
                            {
                                if (point.Contained(p.PointLeft, p.PointRight, PointSelectRadius))
                                {
                                    dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) - p.PointLeft.TrueDistance(p.PointRight);
                                    if (dist < bestDist)
                                    { bestDist = dist; bestMatch = p; }
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
                        if (!_selectedLinks.Contains(bestMatch._linkLeft) || !_selectedLinks.Contains(bestMatch._linkRight))
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
                point = (Vector2)target;

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
                        {
                            bestMatch = _selectedPlanes[0];
                        }
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
        }
        private void _modelPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (saveIndex - 1 > 0 && saveIndex - 1 < undoSaves.Count)
                {
                    if (undoSaves[saveIndex - 1]._collisionLinks[0].Value.ToString() == undoSaves[saveIndex - 1]._linkVectors[0].ToString())//If equal to starting point, remove.
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
            {
                _targetNode.Render();
            }

            if (_modelPanel.RenderBones)
            {
                foreach (IRenderedObject o in _modelPanel._renderList)
                {
                    if (o is IModel)
                    {
                        ((IModel)o).RenderBones(_modelPanel.CurrentViewport);
                    }
                }
            }

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

        private void btnSplit_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer();
            for (int i = _selectedLinks.Count; --i >= 0;)
            {
                _selectedLinks[i].Split();
            }

            ClearSelection();
            SelectionModified();
            _modelPanel.Invalidate();
        }

        private void btnMerge_Click(object sender, EventArgs e)
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
            }
            _modelPanel.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e) { _modelPanel.Invalidate(); }
        private void btnResetRot_Click(object sender, EventArgs e) { trackBar1.Value = 0; _modelPanel.Invalidate(); }
        private void btnResetCam_Click(object sender, EventArgs e) { _modelPanel.ResetCamera(); }

        // StageBox Perspective viewer
        private void btnPerspectiveCam_Click(object sender, EventArgs e)
        {
            _modelPanel.ResetCamera();
            _modelPanel.CurrentViewport.ViewType = ViewportProjection.Perspective;
        }

        // StageBox Orthographic viewer
        private void btnOrthographicCam_Click(object sender, EventArgs e)
        {
            _modelPanel.ResetCamera();
            _modelPanel.CurrentViewport.ViewType = ViewportProjection.Orthographic;
        }

        private void _modelPanel_KeyDown(object sender, KeyEventArgs e)
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
                else if (_selectedLinks.Count == 1)
                {
                    _selectedLinks[0].Pop();
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

        private void cboMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            foreach (CollisionPlane plane in _selectedPlanes)
            {
                plane._material = (CollisionPlaneMaterial)cboMaterial.SelectedItem;
            }

            TargetNode.SignalPropertyChange();
        }
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            foreach (CollisionPlane plane in _selectedPlanes)
            {
                plane.Type = (CollisionPlaneType)cboType.SelectedItem;
            }

            TargetNode.SignalPropertyChange();
        }

        private void chkTypeCharacters_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsCharacters = chkTypeCharacters.Checked;
            }
        }
        private void chkTypeItems_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsItems = chkTypeItems.Checked;
            }
        }
        private void chkTypePokemonTrainer_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsPokemonTrainer = chkTypePokemonTrainer.Checked;
            }
        }
        private void chkTypeRotating_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRotating = chkTypeRotating.Checked;
            }
        }

        private void chkFallThrough_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsFallThrough = chkFallThrough.Checked;
            }
        }
        private void chkLeftLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsLeftLedge = chkLeftLedge.Checked;
            }
        }
        private void chkRightLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRightLedge = chkRightLedge.Checked;
            }
        }
        private void chkNoWalljump_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsNoWalljump = chkNoWalljump.Checked;
            }
        }

        private void chkFlagUnknown1_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownStageBox = chkFlagUnknown1.Checked;
            }
        }
        private void chkFlagUnknown2_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag1 = chkFlagUnknown2.Checked;
            }
        }
        private void chkFlagUnknown3_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag3 = chkFlagUnknown3.Checked;
            }
        }
        private void chkFlagUnknown4_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag4 = chkFlagUnknown4.Checked;
            }
        }

        #endregion

        #region Point Properties

        private void numX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
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

        private void numY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
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

        private void btnSameX_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
            {
                _selectedLinks[i]._rawValue._x = _selectedLinks[0]._rawValue._x;
            }

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        private void btnSameY_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
            {
                _selectedLinks[i]._rawValue._y = _selectedLinks[0]._rawValue._y;
            }

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
                    {
                        n.Checked = e.Node.Checked;
                    }

                    _updating = false;
                }
            }
            else if (e.Node.Tag is MDL0BoneNode)
            {
                ((MDL0BoneNode)e.Node.Tag)._render = e.Node.Checked;
            }

            if (!_updating)
            {
                _modelPanel.Invalidate();
            }
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
            {
                return;
            }

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
            {
                e.Cancel = true;
            }
        }

        private void snapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if ((node == null) || !(node.Tag is MDL0BoneNode))
            {
                return;
            }

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

            save = new CollisionState
            {
                _collisionLinks = new List<CollisionLink>(),
                _linkVectors = new List<Vector2>()
            };

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
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject._flags[0] = chkObjUnk.Checked;
            TargetNode.SignalPropertyChange();
        }

        private void chkObjIndep_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkObjModule_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject._flags[2] = chkObjModule.Checked;
            TargetNode.SignalPropertyChange();
        }

        private void chkObjSSEUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

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

        private void btnTranslateAll_Click(object sender, EventArgs e)
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

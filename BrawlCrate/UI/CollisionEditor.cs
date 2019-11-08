using BrawlCrate.UI;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

using OpenTK.Graphics.OpenGL;

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    [Serializable]
    public unsafe class CollisionEditor : UserControl
    {
        protected virtual bool _errorChecking => true;

        #region Designer

        public ModelPanel _modelPanel;
        private IContainer components;

        protected SplitContainer mainSplitter;
        protected SplitContainer collisionControlSplitter;

        protected CheckBox chkAllModels;
        protected Panel pnlPlaneProps;
        protected Label materialLabel;
        protected Label labelType;
        protected ComboBox cboMaterial;
        protected Panel pnlObjProps;
        protected Panel toolsStripPanel;
        protected ToolStrip toolsStrip;
        protected ToolStripSeparator toolsStrip_Sep1;
        protected ToolStripSeparator toolsStrip_Sep2;
        protected ToolStripSeparator toolsStrip_Sep3;
        protected ToolStripButton btnSameX;
        protected ToolStripButton btnSameY;
        protected ToolStripButton btnSplit;
        protected ToolStripButton btnMerge;
        protected ToolStripButton btnDelete;
        protected ToolStripMenuItem snapToolStripMenuItem;
        protected Button btnResetRot;
        protected ToolStripButton btnResetCam;

        protected TrackBar trackBar1;

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
        protected Label pnlPointPropsX;
        protected Label pnlPointPropsY;
        protected NumericInputBox numX;
        protected NumericInputBox numY;


        protected ContextMenuStrip lstCollObjectsMenu;
        protected ToolStripMenuItem newObjectToolStripMenuItem;
        protected ToolStripSeparator lstCollObjectsMenu_Sep1;
        protected ToolStripSeparator lstCollObjectsMenu_Sep2;
        protected ToolStripSeparator lstCollObjectsMenu_Sep3;
        protected ToolStripSeparator assignSeperatorToolStripMenuItem;
        protected ToolStripMenuItem _deleteToolStripMenuItem;
        protected TextBox txtModel;
        protected Label boneModel;
        protected Panel visibilityCheckPanel;
        protected CheckBox chkPoly;
        protected Button btnRelink;
        protected TextBox txtBone;
        protected Label boneLabel;
        protected CheckBox chkBones;
        protected CheckBox chkLeftLedge;
        protected ComboBox cboType;
        protected TreeView modelTree;
        protected Button btnUnlink;

        protected ContextMenuStrip modelTreeMenu;
        protected ToolStripMenuItem assignToolStripMenuItem;
        protected ToolStripMenuItem assignNoMoveToolStripMenuItem;
        protected ToolStripMenuItem unlinkToolStripMenuItem;
        protected ToolStripMenuItem unlinkNoMoveToolStripMenuItem;
        protected ToolStripMenuItem snapToolStripMenuItem1;
        protected ToolStripButton btnResetSnap;
        protected ToolStripButton btnUndo;
        protected ToolStripButton btnRedo;

        protected CheckBox chkObjModule;
        protected CheckBox chkObjUnk;
        protected CheckBox chkObjSSEUnk;
        protected Button btnPlayAnims;
        protected Panel animationPanel;

        protected Panel selectedMenuPanel; // A panel shown when a collision is selected (can be a point or planes)
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

        protected ContextMenuStrip collisionOptions;

        // Added so that it allows clipboard operations on a selected collision/points
        private ToolStripMenuItem clipboardCut;
        private ToolStripMenuItem clipboardCopy;
        private ToolStripMenuItem clipboardPaste;
        private ToolStripMenuItem clipboardPaste_PasteDirectly;
        private ToolStripMenuItem clipboardPaste_PasteUI;
        //This allows the collisions to be removed and have it pasted
        private ToolStripMenuItem clipboardPaste_PasteRemoveSelected;
        //This allows the collisions to be removed and combine them together
        private ToolStripMenuItem clipboardPaste_PasteOverrideSelected; 
        private ToolStripSeparator clipboardPaste_Sep1;
        private ToolStripMenuItem clipboardDelete;

        private ToolStripMenuItem moveToNewObjectToolStripMenuItem;
        private ToolStripSeparator collisionOptions_Sep1;
        private ToolStripSeparator collisionOptions_Sep2;
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
            mainSplitter = new SplitContainer();
            collisionControlSplitter = new SplitContainer();
            modelTree = new TreeView();
            modelTreeMenu = new ContextMenuStrip(components);
            assignToolStripMenuItem = new ToolStripMenuItem();
            assignNoMoveToolStripMenuItem = new ToolStripMenuItem();
            assignSeperatorToolStripMenuItem = new ToolStripSeparator();
            snapToolStripMenuItem1 = new ToolStripMenuItem();

            visibilityCheckPanel = new Panel();
            chkBones = new CheckBox();
            chkPoly = new CheckBox();
            chkAllModels = new CheckBox();

            lstObjects = new CheckedListBox();
            lstCollObjectsMenu = new ContextMenuStrip(components);
            newObjectToolStripMenuItem = new ToolStripMenuItem();
            lstCollObjectsMenu_Sep3 = new ToolStripSeparator();
            unlinkToolStripMenuItem = new ToolStripMenuItem();
            unlinkNoMoveToolStripMenuItem = new ToolStripMenuItem();
            lstCollObjectsMenu_Sep2 = new ToolStripSeparator();
            snapToolStripMenuItem = new ToolStripMenuItem();
            lstCollObjectsMenu_Sep1 = new ToolStripSeparator();
            _deleteToolStripMenuItem = new ToolStripMenuItem();

            selectedMenuPanel = new Panel();

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
            materialLabel = new Label();
            labelType = new Label();

            pnlPointProps = new Panel();
            pnlPointPropsY = new Label();
            pnlPointPropsX = new Label();
            numY = new NumericInputBox();
            numX = new NumericInputBox();

            pnlObjProps = new Panel();
            chkObjSSEUnk = new CheckBox();
            chkObjModule = new CheckBox();
            chkObjUnk = new CheckBox();
            btnUnlink = new Button();
            btnRelink = new Button();
            txtBone = new TextBox();
            boneLabel = new Label();
            txtModel = new TextBox();
            boneModel = new Label();

            toolsStrip = new ToolStrip();
            toolsStrip_Sep1 = new ToolStripSeparator();
            toolsStrip_Sep3 = new ToolStripSeparator();
            toolsStrip_Sep2 = new ToolStripSeparator();

            animationPanel = new Panel();
            btnPlayAnims = new Button();
            btnPrevFrame = new Button();
            btnNextFrame = new Button();

            _modelPanel = new ModelPanel();

            toolsStripPanel = new Panel();

            btnUndo = new ToolStripButton();
            btnRedo = new ToolStripButton();
            btnSplit = new ToolStripButton();
            btnMerge = new ToolStripButton();
            btnFlipColl = new ToolStripButton();
            btnDelete = new ToolStripButton();
            btnSameX = new ToolStripButton();
            btnSameY = new ToolStripButton();
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

            //Right-click on selected collision/collision view
            collisionOptions = new ContextMenuStrip(components);
            clipboardCopy = new ToolStripMenuItem();
            clipboardCut = new ToolStripMenuItem();
            clipboardPaste = new ToolStripMenuItem();
            clipboardPaste_PasteDirectly = new ToolStripMenuItem();
            clipboardPaste_PasteUI = new ToolStripMenuItem();
            clipboardPaste_PasteRemoveSelected = new ToolStripMenuItem();
            clipboardPaste_PasteOverrideSelected = new ToolStripMenuItem();
            clipboardDelete = new ToolStripMenuItem();
            clipboardPaste_Sep1 = new ToolStripSeparator();
            collisionOptions_Sep1 = new ToolStripSeparator();
            moveToNewObjectToolStripMenuItem = new ToolStripMenuItem();
            splitToolStripMenuItem = new ToolStripMenuItem();
            mergeToolStripMenuItem = new ToolStripMenuItem();
            collisionOptions_Sep2 = new ToolStripSeparator();
            flipToolStripMenuItem = new ToolStripMenuItem();
            _deleteToolStripMenuItem1 = new ToolStripMenuItem();
            transformToolStripMenuItem = new ToolStripMenuItem();
            alignXToolStripMenuItem = new ToolStripMenuItem();
            alignYToolStripMenuItem = new ToolStripMenuItem();

            ((ISupportInitialize)mainSplitter).BeginInit();
            mainSplitter.Panel1.SuspendLayout();
            mainSplitter.Panel2.SuspendLayout();
            mainSplitter.SuspendLayout();

            ((ISupportInitialize)collisionControlSplitter).BeginInit();
            collisionControlSplitter.Panel1.SuspendLayout();
            collisionControlSplitter.Panel2.SuspendLayout();
            collisionControlSplitter.SuspendLayout();

            modelTreeMenu.SuspendLayout();
            visibilityCheckPanel.SuspendLayout();
            lstCollObjectsMenu.SuspendLayout();
            selectedMenuPanel.SuspendLayout();

            pnlPlaneProps.SuspendLayout();
            groupBoxFlags2.SuspendLayout();
            groupBoxFlags1.SuspendLayout();
            groupBoxTargets.SuspendLayout();
            pnlPointProps.SuspendLayout();
            pnlObjProps.SuspendLayout();
            animationPanel.SuspendLayout();
            toolsStripPanel.SuspendLayout();
            toolsStrip.SuspendLayout();

            ((ISupportInitialize)trackBar1).BeginInit();
            collisionOptions.SuspendLayout();
            SuspendLayout();

            // 
            // mainSplitter
            // 
            mainSplitter.Dock = DockStyle.Fill;
            mainSplitter.FixedPanel = FixedPanel.Panel1;
            mainSplitter.Location = new System.Drawing.Point(0, 0);
            mainSplitter.Name = "mainSplitter";
            // 
            // mainSplitter.Panel1
            // 
            mainSplitter.Panel1.Controls.Add(collisionControlSplitter);
            // 
            // mainSplitter.Panel2
            // 
            mainSplitter.Panel2.Controls.Add(_modelPanel);
            mainSplitter.Panel2.Controls.Add(toolsStripPanel);
            mainSplitter.Size = new System.Drawing.Size(694, 467);
            mainSplitter.SplitterDistance = 209;
            mainSplitter.TabIndex = 1;
            // 
            // collisionControlSplitter
            // 
            collisionControlSplitter.Dock = DockStyle.Fill;
            collisionControlSplitter.Location = new System.Drawing.Point(0, 0);
            collisionControlSplitter.Name = "collisionControlSplitter";
            collisionControlSplitter.Orientation = Orientation.Horizontal;
            // 
            // collisionControlSplitter.Panel1
            // 
            collisionControlSplitter.Panel1.Controls.Add(modelTree);
            collisionControlSplitter.Panel1.Controls.Add(visibilityCheckPanel);
            // 
            // collisionControlSplitter.Panel2
            // 
            collisionControlSplitter.Panel2.Controls.Add(lstObjects);
            collisionControlSplitter.Panel2.Controls.Add(selectedMenuPanel);
            collisionControlSplitter.Panel2.Controls.Add(animationPanel);
            collisionControlSplitter.Size = new System.Drawing.Size(209, 467);
            collisionControlSplitter.SplitterDistance = 242;
            collisionControlSplitter.TabIndex = 2;
            // 
            // modelTree
            // 
            modelTree.BorderStyle = BorderStyle.None;
            modelTree.CheckBoxes = true;
            modelTree.ContextMenuStrip = modelTreeMenu;
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
            // modelTreeMenu
            // 
            modelTreeMenu.Items.AddRange(new ToolStripItem[]
            {
                assignToolStripMenuItem,
                assignNoMoveToolStripMenuItem,
                assignSeperatorToolStripMenuItem,
                snapToolStripMenuItem1
            });
            modelTreeMenu.Name = "modelTreeMenu";
            modelTreeMenu.Size = new System.Drawing.Size(239, 76);
            modelTreeMenu.Opening += new CancelEventHandler(modelTreeMenu_Opening);
            // 
            // assignToolStripMenuItem
            // 
            assignToolStripMenuItem.Name = "assignToolStripMenuItem";
            assignToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            assignToolStripMenuItem.Text = "Assign";
            assignToolStripMenuItem.Click += btnRelink_Click;
            // 
            // assignNoMoveToolStripMenuItem
            // 
            assignNoMoveToolStripMenuItem.Name = "assignNoMoveToolStripMenuItem";
            assignNoMoveToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            assignNoMoveToolStripMenuItem.Text = "Assign (No relative movement)";
            assignNoMoveToolStripMenuItem.Click += btnRelinkNoMove_Click;
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
            snapToolStripMenuItem1.Click += snapToolStripMenuItem1_Click;
            // 
            // visibilityCheckPanel
            // 
            visibilityCheckPanel.Controls.Add(chkBones);
            visibilityCheckPanel.Controls.Add(chkPoly);
            visibilityCheckPanel.Controls.Add(chkAllModels);
            visibilityCheckPanel.Dock = DockStyle.Top;
            visibilityCheckPanel.Location = new System.Drawing.Point(0, 0);
            visibilityCheckPanel.Name = "visibilityCheckPanel";
            visibilityCheckPanel.Size = new System.Drawing.Size(209, 17);
            visibilityCheckPanel.TabIndex = 3;
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
            chkBones.CheckedChanged += chkBones_CheckedChanged;
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
            chkPoly.CheckStateChanged += chkPoly_CheckStateChanged;
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
            chkAllModels.CheckedChanged += chkAllModels_CheckedChanged;
            // 
            // lstObjects
            // 
            lstObjects.BorderStyle = BorderStyle.None;
            lstObjects.ContextMenuStrip = lstCollObjectsMenu;
            lstObjects.Dock = DockStyle.Fill;
            lstObjects.FormattingEnabled = true;
            lstObjects.IntegralHeight = false;
            lstObjects.Location = new System.Drawing.Point(0, 0);
            lstObjects.Name = "lstObjects";
            lstObjects.Size = new System.Drawing.Size(209, 82);
            lstObjects.TabIndex = 1;
            lstObjects.ItemCheck += new ItemCheckEventHandler(lstObjects_ItemCheck);
            lstObjects.SelectedValueChanged += lstObjects_SelectedValueChanged;
            lstObjects.MouseDown += new MouseEventHandler(lstObjects_MouseDown);
            // 
            // lstCollObjectsMenu
            // 
            lstCollObjectsMenu.Items.AddRange(new ToolStripItem[]
            {
                newObjectToolStripMenuItem,
                lstCollObjectsMenu_Sep3,
                unlinkToolStripMenuItem,
                unlinkNoMoveToolStripMenuItem,
                lstCollObjectsMenu_Sep2,
                snapToolStripMenuItem,
                lstCollObjectsMenu_Sep1,
                _deleteToolStripMenuItem
            });
            lstCollObjectsMenu.Name = "lstCollObjectsMenu";
            lstCollObjectsMenu.Size = new System.Drawing.Size(238, 132);
            lstCollObjectsMenu.Opening += new CancelEventHandler(lstCollObjectsMenu_Opening);
            // 
            // lstCollObjectsMenu_Sep1
            // 
            lstCollObjectsMenu_Sep1.Name = "lstCollObjectsMenu_Sep1";
            lstCollObjectsMenu_Sep1.Size = new System.Drawing.Size(234, 6);
            // 
            // lstCollObjectsMenu_Sep2
            // 
            lstCollObjectsMenu_Sep2.Name = "lstCollObjectsMenu_Sep2";
            lstCollObjectsMenu_Sep2.Size = new System.Drawing.Size(234, 6);
            // 
            // lstCollObjectsMenu_Sep3
            // 
            lstCollObjectsMenu_Sep3.Name = "lstCollObjectsMenu_Sep3";
            lstCollObjectsMenu_Sep3.Size = new System.Drawing.Size(234, 6);
            // 
            // newObjectToolStripMenuItem
            // 
            newObjectToolStripMenuItem.Name = "newObjectToolStripMenuItem";
            newObjectToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            newObjectToolStripMenuItem.Text = "New Object";
            newObjectToolStripMenuItem.Click += newObjectToolStripMenuItem_Click;
            // 
            // unlinkToolStripMenuItem
            // 
            unlinkToolStripMenuItem.Name = "unlinkToolStripMenuItem";
            unlinkToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            unlinkToolStripMenuItem.Text = "Unlink";
            unlinkToolStripMenuItem.Click += btnUnlink_Click;
            // 
            // unlinkNoMoveToolStripMenuItem
            // 
            unlinkNoMoveToolStripMenuItem.Name = "unlinkNoMoveToolStripMenuItem";
            unlinkNoMoveToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            unlinkNoMoveToolStripMenuItem.Text = "Unlink (No relative movement)";
            unlinkNoMoveToolStripMenuItem.Click += btnUnlinkNoMove_Click;
            // 
            // snapToolStripMenuItem
            // 
            snapToolStripMenuItem.Name = "snapToolStripMenuItem";
            snapToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            snapToolStripMenuItem.Text = "Snap";
            snapToolStripMenuItem.Click += snapToolStripMenuItem_Click;
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Delete;
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            _deleteToolStripMenuItem.Text = "Delete";
            _deleteToolStripMenuItem.Click += _deleteToolStripMenuItem_Click;

            // 
            // selectedMenuPanel
            // 
            selectedMenuPanel.Controls.Add(pnlPlaneProps);
            selectedMenuPanel.Controls.Add(pnlPointProps);
            selectedMenuPanel.Controls.Add(pnlObjProps);
            selectedMenuPanel.Dock = DockStyle.Bottom;
            selectedMenuPanel.Location = new System.Drawing.Point(0, 82);
            selectedMenuPanel.Name = "selectedMenuPanel";
            selectedMenuPanel.Size = new System.Drawing.Size(209, 115);
            selectedMenuPanel.TabIndex = 16;
            // 
            // pnlPlaneProps
            // 
            pnlPlaneProps.Controls.Add(groupBoxFlags2);
            pnlPlaneProps.Controls.Add(groupBoxFlags1);
            pnlPlaneProps.Controls.Add(groupBoxTargets);
            pnlPlaneProps.Controls.Add(cboMaterial);
            pnlPlaneProps.Controls.Add(cboType);
            pnlPlaneProps.Controls.Add(materialLabel);
            pnlPlaneProps.Controls.Add(labelType);
            pnlPlaneProps.Dock = DockStyle.Bottom;
            pnlPlaneProps.Location = new System.Drawing.Point(0, -273);
            pnlPlaneProps.Name = "pnlPlaneProps";
            pnlPlaneProps.Size = new System.Drawing.Size(209, 188);
            pnlPlaneProps.TabIndex = 0;
            pnlPlaneProps.Visible = false;
            // 
            // cboMaterial
            // 
            cboMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaterial.FormattingEnabled = true;
            cboMaterial.Location = new System.Drawing.Point(66, 25);
            cboMaterial.Name = "cboMaterial";
            cboMaterial.Size = new System.Drawing.Size(139, 21);
            cboMaterial.TabIndex = 12;
            cboMaterial.SelectedIndexChanged += cboMaterial_SelectedIndexChanged;
            cboMaterial.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            // 
            // materialLabel
            // 
            materialLabel.Location = new System.Drawing.Point(7, 25);
            materialLabel.Name = "materialLabel";
            materialLabel.Size = new System.Drawing.Size(53, 21);
            materialLabel.TabIndex = 8;
            materialLabel.Text = "Material:";
            materialLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cboType
            // 
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.FormattingEnabled = true;
            cboType.Location = new System.Drawing.Point(66, 4);
            cboType.Name = "cboType";
            cboType.Size = new System.Drawing.Size(139, 21);
            cboType.TabIndex = 5;
            cboType.SelectedIndexChanged += cboType_SelectedIndexChanged;
            cboType.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
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
            // groupBoxFlags1
            // 
            groupBoxFlags1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
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
            chkLeftLedge.CheckedChanged += chkLeftLedge_CheckedChanged;
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
            chkNoWalljump.CheckedChanged += chkNoWalljump_CheckedChanged;
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
            chkRightLedge.CheckedChanged += chkRightLedge_CheckedChanged;
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
            chkTypeRotating.CheckedChanged += chkTypeRotating_CheckedChanged;
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
            chkFallThrough.CheckedChanged += chkFallThrough_CheckedChanged;
            // 
            // groupBoxTargets
            // 
            groupBoxTargets.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
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
            chkTypePokemonTrainer.CheckedChanged += chkTypePokemonTrainer_CheckedChanged;
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
            chkTypeItems.CheckedChanged += chkTypeItems_CheckedChanged;
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
            chkTypeCharacters.CheckedChanged += chkTypeCharacters_CheckedChanged;

            // 
            // groupBoxFlags2
            // 
            groupBoxFlags2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
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
            chkFlagUnknown1.CheckedChanged += chkFlagUnknown1_CheckedChanged;
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
            chkFlagUnknown2.CheckedChanged += chkFlagUnknown2_CheckedChanged;
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
            chkFlagUnknown3.CheckedChanged += chkFlagUnknown3_CheckedChanged;
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
            chkFlagUnknown4.CheckedChanged += chkFlagUnknown4_CheckedChanged;


            // 
            // pnlPointProps
            // 
            pnlPointProps.Controls.Add(pnlPointPropsY);
            pnlPointProps.Controls.Add(numY);
            pnlPointProps.Controls.Add(pnlPointPropsX);
            pnlPointProps.Controls.Add(numX);
            pnlPointProps.Dock = DockStyle.Bottom;
            pnlPointProps.Location = new System.Drawing.Point(0, -85);
            pnlPointProps.Name = "pnlPointProps";
            pnlPointProps.Size = new System.Drawing.Size(209, 70);
            pnlPointProps.TabIndex = 15;
            pnlPointProps.Visible = false;
            // 
            // pnlPointPropsX
            // 
            pnlPointPropsX.BorderStyle = BorderStyle.FixedSingle;
            pnlPointPropsX.Location = new System.Drawing.Point(18, 13);
            pnlPointPropsX.Name = "pnlPointPropsX";
            pnlPointPropsX.Size = new System.Drawing.Size(20, 20);
            pnlPointPropsX.TabIndex = 1;
            pnlPointPropsX.Text = "X";
            pnlPointPropsX.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlPointPropsY
            // 
            pnlPointPropsY.BorderStyle = BorderStyle.FixedSingle;
            pnlPointPropsY.Location = new System.Drawing.Point(18, 32);
            pnlPointPropsY.Name = "pnlPointPropsY";
            pnlPointPropsY.Size = new System.Drawing.Size(20, 20);
            pnlPointPropsY.TabIndex = 3;
            pnlPointPropsY.Text = "Y";
            pnlPointPropsY.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // numX
            // 
            numX.BorderStyle = BorderStyle.FixedSingle;
            numX.Integral = false;
            numX.Location = new System.Drawing.Point(37, 13);
            numX.MaximumValue = 3.402823E+38F;
            numX.MinimumValue = -3.402823E+38F;
            numX.Name = "numX";
            numX.Size = new System.Drawing.Size(152, 20);
            numX.TabIndex = 0;
            numX.Text = "0";
            numX.ValueChanged += numX_ValueChanged;
            numX.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // numY
            // 
            numY.BorderStyle = BorderStyle.FixedSingle;
            numY.Integral = false;
            numY.Location = new System.Drawing.Point(37, 32);
            numY.MaximumValue = 3.402823E+38F;
            numY.MinimumValue = -3.402823E+38F;
            numY.Name = "numY";
            numY.Size = new System.Drawing.Size(152, 20);
            numY.TabIndex = 2;
            numY.Text = "0";
            numY.ValueChanged += numY_ValueChanged;
            numY.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;


            // 
            // pnlObjProps
            // 
            pnlObjProps.Controls.Add(chkObjSSEUnk);
            pnlObjProps.Controls.Add(chkObjModule);
            pnlObjProps.Controls.Add(chkObjUnk);
            pnlObjProps.Controls.Add(btnUnlink);
            pnlObjProps.Controls.Add(btnRelink);
            pnlObjProps.Controls.Add(txtBone);
            pnlObjProps.Controls.Add(boneLabel);
            pnlObjProps.Controls.Add(txtModel);
            pnlObjProps.Controls.Add(boneModel);
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
            chkObjSSEUnk.CheckedChanged += chkObjSSEUnk_CheckedChanged;
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
            chkObjModule.CheckedChanged += chkObjModule_CheckedChanged;
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
            chkObjUnk.CheckedChanged += chkObjUnk_CheckedChanged;
            // 
            // btnUnlink
            // 
            btnUnlink.Location = new System.Drawing.Point(177, 22);
            btnUnlink.Name = "btnUnlink";
            btnUnlink.Size = new System.Drawing.Size(28, 21);
            btnUnlink.TabIndex = 12;
            btnUnlink.Text = "-";
            btnUnlink.UseVisualStyleBackColor = true;
            btnUnlink.Click += btnUnlink_Click;
            // 
            // btnRelink
            // 
            btnRelink.Location = new System.Drawing.Point(177, 2);
            btnRelink.Name = "btnRelink";
            btnRelink.Size = new System.Drawing.Size(28, 21);
            btnRelink.TabIndex = 4;
            btnRelink.Text = "+";
            btnRelink.UseVisualStyleBackColor = true;
            btnRelink.Click += btnRelink_Click;
            // 
            // txtBone
            // 
            txtBone.Location = new System.Drawing.Point(49, 23);
            txtBone.Name = "txtBone";
            txtBone.ReadOnly = true;
            txtBone.Size = new System.Drawing.Size(126, 20);
            txtBone.TabIndex = 3;
            // 
            // boneLabel
            // 
            boneLabel.Location = new System.Drawing.Point(4, 23);
            boneLabel.Name = "boneLabel";
            boneLabel.Size = new System.Drawing.Size(42, 20);
            boneLabel.TabIndex = 2;
            boneLabel.Text = "Bone:";
            boneLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtModel
            // 
            txtModel.Location = new System.Drawing.Point(49, 3);
            txtModel.Name = "txtModel";
            txtModel.ReadOnly = true;
            txtModel.Size = new System.Drawing.Size(126, 20);
            txtModel.TabIndex = 1;
            // 
            // boneModel
            // 
            boneModel.Location = new System.Drawing.Point(4, 3);
            boneModel.Name = "boneModel";
            boneModel.Size = new System.Drawing.Size(42, 20);
            boneModel.TabIndex = 0;
            boneModel.Text = "Model:";
            boneModel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // animationPanel
            // 
            animationPanel.Controls.Add(btnPlayAnims);
            animationPanel.Controls.Add(btnPrevFrame);
            animationPanel.Controls.Add(btnNextFrame);
            animationPanel.Dock = DockStyle.Bottom;
            animationPanel.Enabled = false;
            animationPanel.Location = new System.Drawing.Point(0, 197);
            animationPanel.Name = "animationPanel";
            animationPanel.Size = new System.Drawing.Size(209, 24);
            animationPanel.TabIndex = 17;
            animationPanel.Visible = false;
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
            btnPlayAnims.Click += btnPlayAnims_Click;
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
            btnPrevFrame.Click += btnPrevFrame_Click;
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
            btnNextFrame.Click += btnNextFrame_Click;
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
            // toolsStripPanel
            // 
            toolsStripPanel.BackColor = Color.WhiteSmoke;
            toolsStripPanel.Controls.Add(toolsStrip);
            toolsStripPanel.Controls.Add(btnResetRot);
            toolsStripPanel.Controls.Add(trackBar1);
            toolsStripPanel.Dock = DockStyle.Top;
            toolsStripPanel.Location = new System.Drawing.Point(0, 0);
            toolsStripPanel.Name = "toolsStripPanel";
            toolsStripPanel.Size = new System.Drawing.Size(481, 25);
            toolsStripPanel.TabIndex = 2;
            // 
            // toolsStrip
            // 
            toolsStrip.BackColor = Color.WhiteSmoke;
            toolsStrip.Dock = DockStyle.Fill;
            toolsStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolsStrip.Items.AddRange(new ToolStripItem[]
            {
                btnUndo,
                btnRedo,
                toolsStrip_Sep3,
                btnSplit,
                btnMerge,
                btnFlipColl,
                btnDelete,
                toolsStrip_Sep2,
                btnSameX,
                btnSameY,
                toolsStrip_Sep1,
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
            toolsStrip.Location = new System.Drawing.Point(0, 0);
            toolsStrip.Name = "toolsStrip";
            toolsStrip.Size = new System.Drawing.Size(335, 25);
            toolsStrip.TabIndex = 1;
            toolsStrip.Text = "toolsStrip";
            // 
            // btnUndo
            // 
            btnUndo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnUndo.Enabled = false;
            btnUndo.ImageTransparentColor = Color.Magenta;
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new System.Drawing.Size(40, 22);
            btnUndo.Text = "Undo";
            btnUndo.Click += Undo;
            // 
            // btnRedo
            // 
            btnRedo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnRedo.Enabled = false;
            btnRedo.ImageTransparentColor = Color.Magenta;
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new System.Drawing.Size(38, 22);
            btnRedo.Text = "Redo";
            btnRedo.Click += Redo;
            // 
            // toolsStrip_Sep1
            // 
            toolsStrip_Sep1.Name = "toolsStrip_Sep1";
            toolsStrip_Sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolsStrip_Sep2
            // 
            toolsStrip_Sep2.Name = "toolsStrip_Sep2";
            toolsStrip_Sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolsStrip_Sep3
            // 
            toolsStrip_Sep3.Name = "toolsStrip_Sep3";
            toolsStrip_Sep3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSplit
            // 
            btnSplit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSplit.Enabled = false;
            btnSplit.ImageTransparentColor = Color.Magenta;
            btnSplit.Name = "btnSplit";
            btnSplit.Size = new System.Drawing.Size(34, 22);
            btnSplit.Text = "Split";
            btnSplit.Click += btnSplit_Click;
            // 
            // btnMerge
            // 
            btnMerge.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnMerge.Enabled = false;
            btnMerge.ImageTransparentColor = Color.Magenta;
            btnMerge.Name = "btnMerge";
            btnMerge.Size = new System.Drawing.Size(45, 22);
            btnMerge.Text = "Merge";
            btnMerge.Click += btnMerge_Click;
            // 
            // btnFlipColl
            // 
            btnFlipColl.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnFlipColl.Enabled = false;
            btnFlipColl.ImageTransparentColor = Color.Magenta;
            btnFlipColl.Name = "btnFlipColl";
            btnFlipColl.Size = new System.Drawing.Size(30, 22);
            btnFlipColl.Text = "Flip";
            btnFlipColl.Click += btnFlipColl_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnDelete.Enabled = false;
            btnDelete.ImageTransparentColor = Color.Magenta;
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(44, 22);
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSameX
            // 
            btnSameX.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSameX.ImageTransparentColor = Color.Magenta;
            btnSameX.Name = "btnSameX";
            btnSameX.Size = new System.Drawing.Size(49, 22);
            btnSameX.Text = "Align X";
            btnSameX.Click += btnSameX_Click;
            // 
            // btnSameY
            // 
            btnSameY.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnSameY.ImageTransparentColor = Color.Magenta;
            btnSameY.Name = "btnSameY";
            btnSameY.Size = new System.Drawing.Size(49, 19);
            btnSameY.Text = "Align Y";
            btnSameY.Click += btnSameY_Click;
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
            btnPerspectiveCam.Click += btnPerspectiveCam_Click;
            // 
            // btnOrthographicCam
            // 
            btnOrthographicCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnOrthographicCam.ImageTransparentColor = Color.Magenta;
            btnOrthographicCam.Name = "btnOrthographicCam";
            btnOrthographicCam.Size = new System.Drawing.Size(82, 19);
            btnOrthographicCam.Text = "Orthographic";
            btnOrthographicCam.Click += btnOrthographicCam_Click;
            // 
            // btnResetCam
            // 
            btnResetCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnResetCam.ImageTransparentColor = Color.Magenta;
            btnResetCam.Name = "btnResetCam";
            btnResetCam.Size = new System.Drawing.Size(67, 19);
            btnResetCam.Text = "Reset Cam";
            btnResetCam.Click += btnResetCam_Click;
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
            btnSpawns.Click += btnSpawns_Click;
            // 
            // btnItems
            // 
            btnItems.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnItems.ImageTransparentColor = Color.Magenta;
            btnItems.Name = "btnItems";
            btnItems.Size = new System.Drawing.Size(40, 19);
            btnItems.Text = "Items";
            btnItems.Click += btnItems_Click;
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
            btnBoundaries.Click += btnBoundaries_Click;
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
            btnResetSnap.Click += btnResetSnap_Click;
            // 
            // btnHelp
            // 
            btnHelp.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnHelp.Image = (Image)resources.GetObject("btnHelp.Image");
            btnHelp.ImageTransparentColor = Color.Magenta;
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new System.Drawing.Size(36, 19);
            btnHelp.Text = "Help";
            btnHelp.Click += btnHelp_Click;
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
            btnResetRot.Click += btnResetRot_Click;
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
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // collisionOptions
            // 
            collisionOptions.Items.AddRange(new ToolStripItem[]
            {
                clipboardCut,
                clipboardCopy,
                clipboardPaste,
                clipboardDelete,
                collisionOptions_Sep1,
                moveToNewObjectToolStripMenuItem,
                transformToolStripMenuItem,
                alignXToolStripMenuItem,
                alignYToolStripMenuItem,
                collisionOptions_Sep2,
                splitToolStripMenuItem,
                mergeToolStripMenuItem,
                flipToolStripMenuItem,
                _deleteToolStripMenuItem1
            });
            collisionOptions.Name = "collisionOptions";
            collisionOptions.Size = new System.Drawing.Size(184, 208);
            collisionOptions.Opening += new CancelEventHandler(collisionOptions_Opening);
            // 
            // moveToNewObjectToolStripMenuItem
            // 
            moveToNewObjectToolStripMenuItem.Name = "moveToNewObjectToolStripMenuItem";
            moveToNewObjectToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            moveToNewObjectToolStripMenuItem.Text = "Move to New Object";
            // 
            // collisionOptions_Sep1
            // 
            collisionOptions_Sep1.Name = "collisionOptions_Sep1";
            collisionOptions_Sep1.Size = new System.Drawing.Size(180, 6);
            // 
            // 
            // collisionOptions_Sep2
            // 
            collisionOptions_Sep2.Name = "collisionOptions_Sep2";
            collisionOptions_Sep2.Size = new System.Drawing.Size(180, 6);
            // 
            // splitToolStripMenuItem
            // 
            splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            splitToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            splitToolStripMenuItem.Text = "Split";
            splitToolStripMenuItem.Click += btnSplit_Click;
            // 
            // mergeToolStripMenuItem
            // 
            mergeToolStripMenuItem.Name = "mergeToolStripMenuItem";
            mergeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            mergeToolStripMenuItem.Text = "Merge";
            mergeToolStripMenuItem.Click += btnMerge_Click;
            // 
            // flipToolStripMenuItem
            // 
            flipToolStripMenuItem.Name = "flipToolStripMenuItem";
            flipToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            flipToolStripMenuItem.Text = "Flip";
            flipToolStripMenuItem.Click += btnFlipColl_Click;
            // 
            // _deleteToolStripMenuItem1
            // 
            _deleteToolStripMenuItem1.Name = "_deleteToolStripMenuItem1";
            _deleteToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            _deleteToolStripMenuItem1.Text = "Delete";
            _deleteToolStripMenuItem1.Click += btnDelete_Click;
            // 
            // transformToolStripMenuItem
            // 
            transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            transformToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            transformToolStripMenuItem.Text = "Transform";
            // 
            // alignXToolStripMenuItem
            // 
            alignXToolStripMenuItem.Name = "alignXToolStripMenuItem";
            alignXToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            alignXToolStripMenuItem.Text = "Align X";
            alignXToolStripMenuItem.Click += btnSameX_Click;
            // 
            // alignYToolStripMenuItem
            // 
            alignYToolStripMenuItem.Name = "alignYToolStripMenuItem";
            alignYToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            alignYToolStripMenuItem.Text = "Align Y";
            alignYToolStripMenuItem.Click += btnSameY_Click;
            // 
            // clipboardCut
            // 
            clipboardCut.Name = "clipboardCut";
            clipboardCut.Size = new System.Drawing.Size(183, 22);
            clipboardCut.Text = "Cut";
            clipboardCut.Click += btnCut_Click;
            // 
            // clipboardCopy
            // 
            clipboardCopy.Name = "clipboardCopy";
            clipboardCopy.Size = new System.Drawing.Size(183, 22);
            clipboardCopy.Text = "Copy";
            clipboardCopy.Click += btnCopy_Click;
            // 
            // clipboardPaste
            // 
            clipboardPaste.DropDown.Items.AddRange(new ToolStripItem[]
            {
                clipboardPaste_PasteDirectly,
                clipboardPaste_PasteUI,
                clipboardPaste_Sep1,
                clipboardPaste_PasteRemoveSelected,
                clipboardPaste_PasteOverrideSelected,
            });
            clipboardPaste.Name = "clipboardPaste";
            clipboardPaste.Size = new System.Drawing.Size(183, 22);
            clipboardPaste.Text = "Paste";
            clipboardPaste.Click += btnPaste_Click;
            // 
            // clipboardPaste_PasteDirectly
            // 
            clipboardPaste_PasteDirectly.Name = "clipboardPaste_PasteDirectly";
            clipboardPaste_PasteDirectly.Size = new System.Drawing.Size(183, 22);
            clipboardPaste_PasteDirectly.Text = "Paste Here";
            clipboardPaste_PasteDirectly.Click += btnPasteDirectly_Click;
            // 
            // clipboardPaste_PasteUI
            // 
            clipboardPaste_PasteUI.Name = "clipboardPaste_PasteUI";
            clipboardPaste_PasteUI.Size = new System.Drawing.Size(183, 22);
            clipboardPaste_PasteUI.Text = "Advanced Paste";
            clipboardPaste_PasteUI.Click += btnPasteUI_Click;
            // 
            // clipboardPaste_PasteRemoveSelected
            // 
            clipboardPaste_PasteRemoveSelected.Name = "clipboardPaste_PasteRemoveSelected";
            clipboardPaste_PasteRemoveSelected.Size = new System.Drawing.Size(183, 22);
            clipboardPaste_PasteRemoveSelected.Text = "Remove and Place Selected Collisions";
            clipboardPaste_PasteRemoveSelected.CheckOnClick = true;
            // 
            // clipboardPaste_PasteOverrideSelected
            // 
            clipboardPaste_PasteOverrideSelected.Name = "clipboardPaste_PasteOverrideSelected";
            clipboardPaste_PasteOverrideSelected.Size = new System.Drawing.Size(183, 22);
            clipboardPaste_PasteOverrideSelected.Text = "Override Selected Collisions";
            clipboardPaste_PasteOverrideSelected.CheckOnClick = true;
            // 
            // clipboardPaste_Sep1
            // 
            clipboardPaste_Sep1.Name = "clipboardPaste_Sep1";
            clipboardPaste_Sep1.Size = new System.Drawing.Size(180, 6);
            // 
            // clipboardDelete
            // 
            clipboardDelete.Name = "clipboardDelete";
            clipboardDelete.Size = new System.Drawing.Size(183, 22);
            clipboardDelete.Text = "Delete Selected";
            clipboardDelete.Click += btnDelete_Click;
            // 
            // CollisionEditor
            // 
            BackColor = Color.Lavender;
            Controls.Add(mainSplitter);
            Name = "CollisionEditor";
            Size = new System.Drawing.Size(694, 467);
            mainSplitter.Panel1.ResumeLayout(false);
            mainSplitter.Panel2.ResumeLayout(false);
            ((ISupportInitialize)mainSplitter).EndInit();
            mainSplitter.ResumeLayout(false);
            collisionControlSplitter.Panel1.ResumeLayout(false);
            collisionControlSplitter.Panel2.ResumeLayout(false);
            ((ISupportInitialize)collisionControlSplitter).EndInit();
            collisionControlSplitter.ResumeLayout(false);
            modelTreeMenu.ResumeLayout(false);
            visibilityCheckPanel.ResumeLayout(false);
            lstCollObjectsMenu.ResumeLayout(false);
            selectedMenuPanel.ResumeLayout(false);
            pnlPlaneProps.ResumeLayout(false);
            groupBoxFlags2.ResumeLayout(false);
            groupBoxFlags1.ResumeLayout(false);
            groupBoxTargets.ResumeLayout(false);
            pnlPointProps.ResumeLayout(false);
            pnlPointProps.PerformLayout();
            pnlObjProps.ResumeLayout(false);
            pnlObjProps.PerformLayout();
            animationPanel.ResumeLayout(false);
            toolsStripPanel.ResumeLayout(false);
            toolsStripPanel.PerformLayout();
            toolsStrip.ResumeLayout(false);
            toolsStrip.PerformLayout();
            ((ISupportInitialize)trackBar1).EndInit();
            collisionOptions.ResumeLayout(false);
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

        //We copy the selected variables from _selected... so that we have it in memory
        protected byte _copyState = 0; //Is this even a good practice? 0 = none, 1 = Copying, 2 = Cutting
        protected List<CollisionLink_S> _copiedLinks = new List<CollisionLink_S>();
        protected List<CollisionPlane_S> _copiedPlanes = new List<CollisionPlane_S>();

        protected SortedList<int, List<CollisionLink_S>> _copiedLinks2 = new SortedList<int, List<CollisionLink_S>>();
        protected List<List<CollisionPlane>> _copiedPlanes2 = new List<List<CollisionPlane>>();
		protected List<CollisionState> _copiedLinks3 = new List<CollisionState>();

        protected bool _selecting, _selectInverse;
        protected Vector3 _selectStart, _selectLast, _selectEnd;
        protected bool _creating;

        protected CollisionState save;
        protected List<CollisionState> undoSaves = new List<CollisionState>();
        protected List<CollisionState> redoSaves = new List<CollisionState>();

        protected int saveIndex = 0;
        // Why limit the amount of saves to 25? An option would be nice.
        public int maxSaveCount = 25;

        protected bool hasMoved = false;

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
            cboMaterial.DataSource = getMaterials(); // Take unexpanded collisions
            cboType.DataSource = getCollisionPlaneTypes();
            _updating = false;
        }

        public List<CollisionTerrain> getMaterials()
        {
            return CollisionTerrain.Terrains.Take(0x20).ToList();
        }
        public Array getCollisionPlaneTypes()
        {
            return Enum.GetValues(typeof(CollisionPlaneType));
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
            CheckPlanes(ref _selectedLinks, ref _selectedPlanes);

            pnlPlaneProps.Visible = false;
            pnlObjProps.Visible = false;
            pnlPointProps.Visible = false;
            selectedMenuPanel.Height = 0;

            //Selected Planes are used for actual planes and overrides the collision data (such as Ground Type, etc.)
            if (_selectedPlanes.Count > 0)
            {
                pnlPlaneProps.Visible = true;
                selectedMenuPanel.Height = 205;
            }
            //Selected links are used for getting an specific point in that collision
            else if (_selectedLinks.Count == 1)
            {
                pnlPointProps.Visible = true;
                selectedMenuPanel.Height = 70;
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
                else if (cboMaterial.Items.Count > 32)
                {
                    cboMaterial.DataSource =
                        CollisionTerrain.Terrains.Take(0x20).ToList(); // Take unexpanded collisions
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

            if (_targetNode != null && _targetNode._parent != null)
            {
                foreach (MDL0Node n in _targetNode._parent.FindChildrenByTypeInGroup(null, ResourceType.MDL0,
                    _targetNode.GroupID))
                {
                    TreeNode modelNode = new TreeNode(n._name) { Tag = n, Checked = true };
                    modelTree.Nodes.Add(modelNode);
                    _models.Add(n);

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

                    MDL0Node model = _models.Where(m => m is MDL0Node && ((ResourceNode)m).Name == obj._modelName)
                        .FirstOrDefault() as MDL0Node;

                    if (model != null)
                    {
                        MDL0BoneNode bone =
                            model._linker.BoneCache.Where(b => b.Name == obj._boneName)
                                .FirstOrDefault() as MDL0BoneNode;
                        if (bone != null)
                        {
                            obj._linkedBone = bone;
                        }
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
            selectedMenuPanel.Height = 0;
            if (_selectedObject != null)
            {
                pnlObjProps.Visible = true;
                selectedMenuPanel.Height = 130;
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
            if (_selecting || _hovering || _selectedLinks.Count == 0)
            {
                btnDelete.Enabled = btnFlipColl.Enabled =
                    btnMerge.Enabled = btnSplit.Enabled = btnSameX.Enabled = btnSameY.Enabled = false;
            }
            else
            {
                btnMerge.Enabled = btnSameX.Enabled = btnSameY.Enabled = _selectedLinks.Count > 1;
                btnDelete.Enabled = btnSplit.Enabled = true;
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
                    point = (Vector2)target;

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

                // Nothing found :(

                // Trace ray to Z axis
                target = Vector3.IntersectZ(target, _modelPanel.CurrentViewport.UnProject(e.X, e.Y, 0.0f), 0.0f);
                point = (Vector2)target;

                if (create)
                {
                    // Create Link (if no other links are selected)
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
                    // If there's a link (aka Point), then create another point/target
                    else if (_selectedLinks.Count == 1)
                    {
                        // Create new plane extending to point
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
                            links.Add(l.Branch((Vector2)target));
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

                        link._highlight = true;
                        _selectedLinks.Clear();
                        _selectedLinks.Add(link);
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

				if (!hasMoved)
				{
					//Removed so that we show the menu regardless of whether or not the user has selected any collisions
					if ((_RCend - _RCstart) <= TimeSpan.FromMilliseconds(150))
					{
						if (_copiedLinks.Count > 0 || _copiedPlanes.Count > 0 || _copiedLinks2.Count > 0 || _copiedLinks3.Count > 0 ||
							_selectedLinks.Count > 0 || _selectedPlanes.Count > 0 || _copiedPlanes2.Count > 0)
							collisionOptions.Show(Cursor.Position);
					}
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

            #region RenderOverlays

            GL.Disable(EnableCap.DepthTest);
            List<MDL0BoneNode> ItemBones = new List<MDL0BoneNode>();

            MDL0Node stgPos = null;

            MDL0BoneNode CamBone0 = null, CamBone1 = null, DeathBone0 = null, DeathBone1 = null;

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
                            _modelPanel.CurrentViewport.ScreenText[playernum.ToString()] =
                                _modelPanel.CurrentViewport.Camera.Project(position) - new Vector3(8.0f, 8.0f, 0);
                        }
                    }
                    else if (bone._name.StartsWith("Rebirth") && bone._name.Length == 9 && btnSpawns.Checked)
                    {
                        GL.Color4(1.0f, 1.0f, 1.0f, 0.1f);
                        TKContext.DrawSphere(bone._frameMatrix.GetPoint(), 5.0f, 32);
                        if (int.TryParse(bone._name.Substring(7, 1), out int playernum))
                        {
                            _modelPanel.CurrentViewport.ScreenText[playernum.ToString()] =
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
                GL.Begin(PrimitiveType.LineLoop);
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
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color4(Color.Red);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.End();
                GL.Color4(0.0f, 0.5f, 1.0f, 0.3f);
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(camBone0._x, camBone0._y);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.End();
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.End();
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.End();
                GL.Begin(PrimitiveType.TriangleFan);
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
            ClearUndoRedoBuffer();

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
            ClearUndoRedoBuffer();

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
                //Moved delete so that it can be used with cutting and don't have to recall this variable multiple times
                DeleteSelected();
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
                plane._material = ((CollisionTerrain)cboMaterial.SelectedItem).ID;
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
                plane.Type = (CollisionPlaneType)cboType.SelectedItem;
                if (!plane.IsRotating)
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

            if (!_errorChecking)
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

            if (!_errorChecking)
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

            if (!_errorChecking)
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

            if (!_errorChecking)
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

            if (!_errorChecking)
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

            if (!_errorChecking)
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

            if (!_errorChecking)
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
                if (chkRightLedge.Checked == true)
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

            if (!_errorChecking)
            {
                chkNoWalljump_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }

            _updating = true;
            TargetNode.SignalPropertyChange();

            bool selection = chkNoWalljump.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;

            bool allSameType = true;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
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

            if (numX.Text == "" && _errorChecking)
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

        protected void numY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (numY.Text == "" && _errorChecking)
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

        protected void modelTree_AfterSelect(object sender, TreeViewEventArgs e)
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

        protected void modelTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
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

        protected void btnRelink_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (_selectedObject == null || node == null || !(node.Tag is MDL0BoneNode))
            {
                return;
            }

            txtBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = (MDL0BoneNode)node.Tag;
            txtModel.Text = _selectedObject._modelName = node.Parent.Text;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnRelinkNoMove_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (_selectedObject == null || node == null || !(node.Tag is MDL0BoneNode))
            {
                return;
            }

            txtBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = (MDL0BoneNode)node.Tag;
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
            DeleteSelected();
        }

        protected void modelTreeMenu_Opening(object sender, CancelEventArgs e)
        {
            if (modelTree.SelectedNode == null || !(modelTree.SelectedNode.Tag is MDL0BoneNode))
            {
                e.Cancel = true;
            }
        }

        protected void lstCollObjectsMenu_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedObject == null)
            {
                lstCollObjectsMenu.Items[1].Visible = lstCollObjectsMenu.Items[2].Visible =
                    lstCollObjectsMenu.Items[3].Visible = lstCollObjectsMenu.Items[4].Visible =
                        lstCollObjectsMenu.Items[5].Visible = lstCollObjectsMenu.Items[6].Visible =
                            lstCollObjectsMenu.Items[7].Visible = false;
            }
            else
            {
                lstCollObjectsMenu.Items[1].Visible = lstCollObjectsMenu.Items[2].Visible =
                    lstCollObjectsMenu.Items[3].Visible = lstCollObjectsMenu.Items[4].Visible =
                        lstCollObjectsMenu.Items[5].Visible = lstCollObjectsMenu.Items[6].Visible =
                            lstCollObjectsMenu.Items[7].Visible = true;
            }
        }

        protected void collisionOptions_Opening(object sender, CancelEventArgs e)
        {
            bool ThereAreCopiedStuff = (_copiedLinks.Count > 0) || (_copiedPlanes.Count > 0) || (_copiedPlanes2.Count > 0) || (_copiedLinks3.Count > 0);

			//This shows the extensive menus if a link is selected. Planes also dictate the visibility.
			if (_selectedLinks != null && _selectedLinks.Count > 0)
            {
                //We show every single collision options items so that we don't have to deal
                //with later code.
                ToggleCollisionOptionsItemVisibility(true);

                //The usual "Control me" algorithm
                mergeToolStripMenuItem.Visible = alignXToolStripMenuItem.Visible =
                    alignYToolStripMenuItem.Visible = _selectedLinks != null && _selectedLinks.Count > 1;
                moveToNewObjectToolStripMenuItem.Visible =
                    flipToolStripMenuItem.Visible = _selectedPlanes != null && _selectedPlanes.Count > 0;
                moveToNewObjectToolStripMenuItem.Visible = false;

                //collisionOptions.Items[0].Visible = collisionOptions.Items[1].Visible = 
                //collisionOptions.Items[2].Visible = collisionOptions.Items[3].Visible = 
                //collisionOptions.Items[4].Visible = collisionOptions.Items[6].Visible = 
                //collisionOptions.Items[7].Visible = (_selectedPlanes != null && _selectedPlanes.Count > 0);
            }
            else
            {
                // We hide every single one of them
                ToggleCollisionOptionsItemVisibility(false);

                // We show paste only if there are copied items on the clipboard.
                clipboardPaste.Visible = ThereAreCopiedStuff;
            }

            //Trace.WriteLine("copiedStuff: " + ThereAreCopiedStuff);
            clipboardPaste.Enabled = ThereAreCopiedStuff;
        }

        //Used so that every item in collisionOptions are visible or not
        //Please change this algorithm if you think there's a better way
        private void ToggleCollisionOptionsItemVisibility(bool visible)
        {
            for (int i = 0; i < collisionOptions.Items.Count; ++i)
                (collisionOptions.Items[i]).Visible = visible;
        }

        #region Clipboard Operations
        #region Clipboard Events
        //Here we check the amount of selected items, copy its selections first then delete the selected collisions
        protected void btnCut_Click(object sender, EventArgs e)
        {
            CopySelected();
            DeleteSelected();

            _copyState = 2;
        }
        //Here we check the amount of selected items and copy it
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            CopySelected();
            _copyState = 1;
        }
        // Here we decide what to paste based on the toggle option
        // Is this even reasonable to make users make checks in terms
        // of having to switch paste kinds or is this okay?
        protected void btnPaste_Click(object sender, EventArgs e)
        {
            if (clipboardPaste_PasteDirectly.Checked)
            {
                btnPasteDirectly_Click(sender, e);
            }
            else if (clipboardPaste_PasteUI.Checked)
            {
                btnPasteUI_Click(sender, e);
            }
        }
        //Here we paste the collisions directly into the collision system
        protected void btnPasteDirectly_Click(object sender, EventArgs e)
        {
            clipboardPaste_PasteDirectly.Checked = true;
            clipboardPaste_PasteUI.Checked = false;

            this.PasteCopiedCollisions();
        }

        // Here we paste the collisions but in a ui that will take special care of it
        protected void btnPasteUI_Click(object sender, EventArgs e)
        {
            clipboardPaste_PasteDirectly.Checked = false;
            clipboardPaste_PasteUI.Checked = true;

            ShowAdvancedPasteOptions();
        }

        public CollisionEditor_PasteOptions editorPO = null;
        public void ShowAdvancedPasteOptions()
        {
            // Show the dialog
            if (editorPO == null)
            {
                editorPO = new CollisionEditor_PasteOptions(this);
                editorPO.Show();
            }
            // Else then we have to either make a new dialog that supports multi-pasting ui options
            // (to be honest, it's actually that good) or stick to letting the user know that a dialog
            // is already open (boo!)
            else
            {
                if (editorPO.Visible)
                    MessageBox.Show("Advanced Paste Options is already open.");
                else
                {
                    editorPO = null;
                    ShowAdvancedPasteOptions();
                }
            }
        }
        #endregion

        // We copy its selected contents first. This will not override copied planes/links if we know that 
        // the counts aren't 1. (This might aswell be broken)
        protected void CopySelected()
        {
            if (_selectedLinks.Count == 0)
            {
                MessageBox.Show("There is nothing to copy.");
                return;
            }


			//_copiedLinks3 = new List<CollisionState>();

			//var copy = new CollisionState 
			//{ 
			//	_collisionLinks = new List<CollisionLink>(),
			//	_linkVectors = new List<Vector2>(),
			//	_create = true
			//};
				

			//for (int index = 0; index < _selectedLinks.Count; index++)
			//{
			//	CollisionLink l = (CollisionLink)_selectedLinks[index].Clone();

			//	copy._collisionLinks.Add(l);
			//	copy._linkVectors.Add(l.Value);
			//}

			//_copiedLinks3.Add(copy);
			//copy = null;


   //       List<List<CollisionPlane>> Planes = new List<List<CollisionPlane>>();

     //       foreach (var link in _selectedLinks)
     //       {
     //           var linkc = (CollisionLink)link.Clone();

     //           if (linkc._members == null || linkc._members.Count == 0)
     //           {
     //               _copiedLinks.Add(new CollisionLink_S(linkc));
     //           }
     //           else
     //           {
     //               List<CollisionPlane> PlanesSub = new List<CollisionPlane>();

     //               foreach (var plane in linkc._members)
     //               {
     //                   var planec = plane.Clone();

     //                   PlanesSub.Add(planec);

     //                   //var p = new CollisionPlane_S(planec);
     //                   //_copiedPlanes.Add(p);
     //               }

     //               Planes.Add(PlanesSub);
     //           }
     //       }

     //       if (Planes.Count > 0)
     //       {
     //           // Now we have to sort the list based on how much they have...

     //           var sorter = new CollisionPlaneComparable();

     //           Planes.Sort(sorter);
     //           //Planes.Reverse();

     //           // Now we have to remove any plane clones that are on the list.
     //           // We start at the last part of the list since the last part has
     //           // more planes than the eariler ones.
     //           for (int pl = Planes.Count - 1; pl > 0; --pl)
     //           {
     //               int prev = pl - 1;

     //               if (prev < 0)
     //                   break;

     //               List<CollisionPlane> planes = Planes[pl];
					//if (planes.Count < 1)
					//	continue;

     //               for (int plp = prev; plp >= 0; --plp)
     //               {
     //                   List<CollisionPlane> planesPrev = Planes[plp];

					//	if (planesPrev.Count < 1)
					//		continue;

     //                   bool planesHighestCount = (planes.Count > planesPrev.Count);
     //                   bool plPrevHighestCount = (planesPrev.Count > planes.Count);
                        
     //                   int highestCount = planesHighestCount ? planes.Count : planesPrev.Count;

     //                   for (int ind = 0; ind < highestCount; ind++)
     //                   {

     //                       if (planesHighestCount)
     //                       {
     //                           this.RemovePrevIndex(ref planes, ref planesPrev, ind, true);
     //                       }
     //                       else if (plPrevHighestCount)
     //                       {
     //                           this.RemovePrevIndex(ref planesPrev, ref planes, ind, false);
     //                       }
     //                       else
     //                       {
     //                           this.RemovePrevIndex(ref planes, ref planesPrev, ind, true);
     //                       }
     //                   }
     //               }
     //           }
     //       }

     //       this._copiedPlanes2 = Planes;



            CopyCollisionLinks(ref _selectedLinks, ref _copiedLinks, true);

            //Utils.CopyList(ref _selectedLinks, ref _copiedLinks, true);
            //Utils.CopyList(ref _selectedPlanes, ref _copiedPlanes, true);

            this.SelectionModified();
        }

        private void RemovePrevIndex(ref List<CollisionPlane> planesPrimary, ref List<CollisionPlane> planesSecondary, 
            int ind, bool removeSecondary)
        {
            int?[] listsToRemove = new int?[planesSecondary.Count];
            CollisionPlane plane1 = planesPrimary[ind];

			Trace.WriteLine("index: "+ind+" | prim: "+planesPrimary.Count+" | sec: "+planesSecondary.Count);

			if (ind >= planesSecondary.Count || 
				planesPrimary == null || planesPrimary.Count < 1 || 
				planesSecondary == null || planesSecondary.Count < 1)
			{
				return;
			}

            for (int i = 0; i < planesSecondary.Count; i++)
            {
                CollisionPlane plane2 = planesSecondary[ind];

                if (CollisionPlane.PlaneEquals(plane1, plane2))
                {
                    listsToRemove[i] = i;
                }
            }

            for (int i = 0; i < listsToRemove.Length; i++)
            {
                if (listsToRemove[i].HasValue)
                {
                    if (removeSecondary)
                        planesSecondary.RemoveAt(listsToRemove[i].Value);
                    else
                        planesPrimary.RemoveAt(listsToRemove[i].Value);
                }
            }
        }

        private void CopyCollisionLinks(ref List<CollisionLink> links, ref List<CollisionLink_S> links_S, bool from)
        {
            //From means that the original links will override/create a new link
            if (from)
            {
                links_S.Clear();

                CollisionLink last = null;
				List<CollisionPlane_S> PlanesID = new List<CollisionPlane_S>();

				int planeID = 0;

				//We get a list of links.
				for (int i = links.Count - 1; i >= 0; --i)
                {
					CollisionLink link = links[i];

					//We then get a list of planes that this link has.
					var planes = link._members;

					for (int ip = planes.Count - 1; ip >= 0; --ip)
					{
						CollisionPlane cp = planes[ip];

						//We first check if the links are not equal to the main link being read.
						if (!CollisionLink.LinkEquals(link, cp._linkRight) && !cp._linkLeft._highlight)
						{
							continue;
						}
						else if (!CollisionLink.LinkEquals(link, cp._linkRight) && !cp._linkRight._highlight)
						{
							continue;
						}

						//We now create a plane that does not use any links yet.
						var cp_s = new CollisionPlane_S(ref cp, planeID, false);



						planeID++;
					}


					CollisionLink_S cl = new CollisionLink_S(link);
                    links_S.Add(cl);

					

                    last = link;
                }
				//foreach (var link in links)
    //            {
    //                //var linkc = (CollisionLink)link.Clone();

    //                //if (last != null)
    //                //{
    //                //    if (last._members != null && last._members.Count > 0)
    //                //    {
    //                //        for (int m = 0; m < last._members.Count; m++)
    //                //        {
    //                //            CollisionPlane cp = last._members[m];

    //                //        }
    //                //    }
    //                //}


    //                CollisionLink_S cl = new CollisionLink_S(link);
    //                links_S.Add(cl);

					

    //                last = link;
    //            }
            }
            else
            {
                links.Clear();

                foreach (var link_s in links_S)
                {
                    
                }
            }
        }

        public void PasteCopiedCollisions()
        {
			//if (_selectedLinks.Count == 0)
			//{
			//    if (_selectedObject == null)
			//    {
			//        MessageBox.Show("You need to select a collision object.");
			//        return;
			//    }
			//}

			//if (_copiedLinks.Count == 0)
			//{
			//    MessageBox.Show("You do not have anything copied.");
			//    return;
			//}

			CreateUndo();

            if (clipboardPaste_PasteOverrideSelected.Checked)
            {
                if (_selectedLinks.Count == 1)
                {
                    //Create new plane extending to point
                    //CollisionLink link = _selectedLinks[0];

                    //_copiedLinks[0]

                    //_selectedLinks[0] = link.Branch((Vector2)target);
                    //_selectedLinks[0]._highlight = true;
                    //link._highlight = false;
                    //SelectionModified();
                    //_modelPanel.Invalidate();

                    ////Hover new point so it can be moved
                    //BeginHover(target);
                }
                else if (_selectedPlanes.Count > 0)
                {

                }

                return;
            }
            else if (clipboardPaste_PasteRemoveSelected.Checked)
            {
                DeleteSelected();
            }
            else
            {
                ClearSelection();
            }

            if (_copiedLinks.Count < 1)
                return;

            _selectedLinks.Clear();



			//if (_copiedLinks3 == null || _copiedLinks3.Count < 1)
			//	return;

			//CollisionState cs = _copiedLinks3[0];

			//for (int i = 0; i < cs._collisionLinks.Count; i++)
			//{
			//	CollisionLink cl = cs._collisionLinks[i];

			//	_selectedLinks.Add(cl);
			//	_selectedLinks[i].Value = cs._linkVectors[i];
			//	_selectedLinks[i]._highlight = true;
			//}

			//SelectionModified();
			//_modelPanel.Invalidate();


			////Failed algorithm #2
			//for (int i = 0; i < _copiedPlanes2.Count; i++)
			//{
			//	List<CollisionPlane> planes = _copiedPlanes2[i];

			//	CollisionLink clL = new CollisionLink(_selectedObject, planes[0]._linkLeft._rawValue);
			//	CollisionLink clR = new CollisionLink(_selectedObject, planes[0]._linkRight._rawValue);
			//	CollisionPlane branch = new CollisionPlane(_selectedObject, clL, clR);
			//	branch._flags = planes[0]._flags;
			//	branch._flags2 = planes[0]._flags2;
			//	branch._type = planes[0]._type;

			//	clL._highlight = true;
			//	clR._highlight = true;

			//	_selectedLinks.Add(clL);
			//	_selectedLinks.Add(clR);
			//	//_selectedPlanes.Add(branch);

			//	for (int i2 = 1; i2 < planes.Count; i2++)
			//	{
			//		CollisionPlane cp = planes[i2];

			//	}

			//	SelectionModified();
			//	_modelPanel.Invalidate();
			//}



			//This is _copiedLinks algorithm
			{
				CollisionLink l = new CollisionLink(_selectedObject, _copiedLinks[0].RawValue);
				//CollisionLink l = new CollisionLink(_selectedObject, _copiedLinks[0].RawValue).Branch(_copiedLinks[0].RawValue);
				_selectedLinks.Add(l);
				l._highlight = true;

				if (_copiedLinks.Count > 1)
				{
					for (int a = 1; a < _copiedLinks.Count; a++)
					{
						CollisionLink_S v = _copiedLinks[a];

						CollisionLink last = _selectedLinks[a - 1];
						CollisionLink recent = new CollisionLink(_selectedObject, v.RawValue);

						CollisionPlane branchPlane = new CollisionPlane(_selectedObject, last, recent);
						if (v.Members != null && v.Members.Count > 0)
						{
							for (int vm = 0; vm < 1/*v.Members.Count*/; vm++)
							{
								CollisionPlane_S p = v.Members[vm];
								p.ApplyToOriginal(ref branchPlane);
							}
						}

						//CollisionLink recent = last.Branch(v.RawValue);
						//_selectedLinks[a - 1] = recent;
						//_selectedLinks[a - 1]._highlight = true;

						recent._highlight = true;

						_selectedLinks.Add(recent);
					}
				}

				SelectionModified();
				_modelPanel.Invalidate();
			}
		}
		#endregion

		// Delete selected items
		// callClearSel means that we will clear the selection
		protected void DeleteSelected(bool callClearSel = true)
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

            if (callClearSel)
                ClearSelection();

            SelectionModified();
            _modelPanel.Invalidate();
        }

        protected void snapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (node == null || !(node.Tag is MDL0BoneNode))
            {
                return;
            }

            _snapMatrix = ((MDL0BoneNode)node.Tag)._inverseBindMatrix;
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

        protected void CheckSaveIndex()
        {
            if (saveIndex < 0)
            {
                saveIndex = 0;
            }

            if (undoSaves.Count > maxSaveCount)
            {
                undoSaves.RemoveAt(0);
                saveIndex--;
            }
        }

        protected void ClearUndoRedoBuffer()
        {
            saveIndex = 0;
            undoSaves.Clear();
            redoSaves.Clear();
            btnUndo.Enabled = btnRedo.Enabled = false;
        }

		//TODO: Why not make a UI that shows you what were undone? That way it is quicker
		//and saves some time.
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

        protected void Undo(object sender, EventArgs e)
        {
            _selectedLinks.Clear();

            save = new CollisionState();

            int index = saveIndex - 1;

            if (undoSaves[index]._linkVectors != null) //XY Positions changed.
            {
                save._collisionLinks = new List<CollisionLink>();
                save._linkVectors = new List<Vector2>();

                for (int i = 0; i < undoSaves[index]._collisionLinks.Count; i++)
                {
                    _selectedLinks.Add(undoSaves[index]._collisionLinks[i]);
                    save._collisionLinks.Add(undoSaves[index]._collisionLinks[i]);
                    save._linkVectors.Add(undoSaves[index]._collisionLinks[i].Value);
                    _selectedLinks[i].Value = undoSaves[index]._linkVectors[i];
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

            int index = undoSaves.Count - saveIndex - 1;
            for (int i = 0; i < redoSaves[index]._collisionLinks.Count; i++)
            {
                _selectedLinks.Add(redoSaves[index]._collisionLinks[i]);
                _selectedLinks[i].Value = redoSaves[index]._linkVectors[i];
            }

            redoSaves.RemoveAt(index);
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

        protected void CheckPlanes(ref List<CollisionLink> _links, ref List<CollisionPlane> _planes)
        {
            _planes.Clear();

            foreach (CollisionLink l in _links)
            {
                foreach (CollisionPlane p in l._members)
                {
                    if (_links.Contains(p._linkLeft) && _links.Contains(p._linkRight) && !_planes.Contains(p))
                    {
                        _planes.Add(p);
                    }
                }
            }
        }
    }

    [Serializable]
    public struct CollisionLink_S
    {
        public CollisionObject Parent;
        public int EncodeIndex;

        public Vector2 RawValue;

        public List<CollisionPlane_S> Members;

        // A way to know which index this object is. For example, because there are a bunch of 
        // links that might be equal in terms of RawValue, Members, and its parent, this index
        // serves to make sure that only a link serves as a parent
        public int LinkIndex;

        public CollisionLink_S(CollisionLink orig,  bool callPlaneMembers = true)
        {
            Parent = orig._parent;
            RawValue = orig._rawValue;
            EncodeIndex = orig._encodeIndex;

            //List<CollisionPlane_S> members = new List<CollisionPlane_S>();
            //if (callPlaneMembers)
            //{
            //    foreach (var member in orig._members)
            //    {
            //        members.Add(new CollisionPlane_S(ref member, -1));
            //    }
            //}

            LinkIndex = 0;
            Members = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="link1"></param>
        /// <param name="link2"></param>
        /// <param name="strict">If set to true, this means that even Parent of that link will have to be the same. </param>
        /// <returns></returns>
        public static bool SameLink(CollisionLink_S link1, CollisionLink_S link2, bool strict = false)
        {
            if (strict)
            {
                if (link1.Parent != link2.Parent)
                    return false;
            }

            if (link1.EncodeIndex != link2.EncodeIndex)
                return false;

            if (link1.RawValue != link2.RawValue)
                return false;
            if (link1.Members.Count != link2.Members.Count)
                return false;

            for (int i = 0; i < link1.Members.Count; i++)
            {
                CollisionPlane_S l1m = link1.Members[i];
                CollisionPlane_S l2m = link2.Members[i];

                if (l1m.EncodeIndex != l2m.EncodeIndex)
                    return false;
                if (l1m.Flags != l2m.Flags)
                    return false;
                if (l1m.Flags2 != l2m.Flags2)
                    return false;
                if (l1m.Type != l2m.Type)
                    return false;
            }

            return true;
        }
    }

    [Serializable]
    public struct CollisionPlane_S
    {
        public CollisionObject Parent;
        public int EncodeIndex;

        public CollisionLink_S LinkLeft;
        public CollisionLink_S LinkRight;

        public CollisionPlaneFlags Flags;
        public CollisionPlaneFlags2 Flags2;
        public CollisionPlaneType Type;

		public int ID;
		public CollisionPlane Reference;

		public CollisionPlane_S(ref CollisionPlane orig, int id, bool makeLinks = true)
        {
            Parent = orig._parent;
            EncodeIndex = orig._encodeIndex;

            Flags = orig._flags;
            Flags2 = orig._flags2;
            Type = orig._type;

			ID = id;
			Reference = orig;

            if (makeLinks)
            {
                LinkLeft = new CollisionLink_S(orig.LinkLeft, false);
                LinkRight = new CollisionLink_S(orig.LinkRight, false);
                return;
            }
			else
			{
				LinkLeft = new CollisionLink_S();
				LinkRight = new CollisionLink_S();
			}
        }

        public void ApplyToOriginal(ref CollisionPlane plane)
        {
            plane._flags = Flags;
            plane._flags2 = Flags2;
            plane._type = Type;
        }
    }

    public class CollisionPlaneComparable : IComparer<List<CollisionPlane>>
    {
        public int Compare(List<CollisionPlane> x, List<CollisionPlane> y)
        {
            if (x == null || y == null)
                return 0;

            return x.Count.CompareTo(y.Count);
        }
    }
}
﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

using OpenTK.Graphics.OpenGL;

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

        protected CheckBox visibilityCheckPanel_ShowAllModels;
        protected Panel selectedPlanePropsPanel;
        protected Label selectedPlanePropsPanel_MaterialLabel;
        protected Label selectedPlanePropsPanel_TypeLabel;
        protected ComboBox selectedPlanePropsPanel_Material;
        protected Panel selectedObjPropsPanel;

        protected Panel toolsStripPanel;
        protected ToolStrip toolsStrip;
        protected ToolStripButton toolsStrip_AlignX;
        protected ToolStripButton toolsStrip_AlignY;
        protected ToolStripButton toolsStrip_Split;
        protected ToolStripButton toolsStrip_Merge;
        protected ToolStripButton toolsStrip_Delete;
        protected ToolStripButton toolsStrip_ResetCam;
        protected ToolStripButton toolsStrip_ResetSnap;
        protected ToolStripButton toolsStrip_Undo;
        protected ToolStripButton toolsStrip_Redo;
        protected ToolStripButton toolsStrip_Help;
        protected ToolStripSplitButton toolsStrip_Options;
        protected ToolStripSeparator toolsStrip_UndoRedoSeparator;
        protected ToolStripSeparator toolsStrip_CollisionManipulationSeparator;
        protected ToolStripSeparator toolsStrip_AlignmentSeparator;
        protected Button UNUSED_toolsStripPanel_ResetRotation;
        protected TrackBar UNUSED_toolsStripPanel_TrackBarRotation;


        protected CheckBox AttributeFlagsGroup_PlnCheckFallThrough;
        protected CheckBox AttributeFlagsGroup_PlnCheckNoWalljump;
        protected CheckBox AttributeFlagsGroup_PlnCheckRightLedge;
        protected CheckBox AttributeFlagsGroup_PlnCheckLeftLedge;
        protected CheckBox AttributeFlagsGroup_PlnCheckRotating;

        protected GroupBox selectedPlanePropsPanel_TargetGroup;
        protected CheckBox TargetGroup_CheckEverything;
        protected CheckBox TargetGroup_CheckItems;
        protected CheckBox TargetGroup_CheckPKMNTrainer;

        // Advanced unknown flags
        protected CheckBox UnknownFlagsGroup_Check1;
        protected CheckBox UnknownFlagsGroup_Check2;
        protected CheckBox UnknownFlagsGroup_Check3;
        protected CheckBox UnknownFlagsGroup_Check4;

        protected Panel selectedPointPropsPanel;
        protected Label selectedPointPropsPanel_XLabel;
        protected Label selectedPointPropsPanel_YLabel;
        protected NumericInputBox selectedPointPropsPanel_XValue;
        protected NumericInputBox selectedPointPropsPanel_YValue;

        protected GroupBox selectedPlanePropsPanel_AttributeFlagsGroup;
        protected GroupBox selectedPlanePropsPanel_UnknownFlagsGroup;
        protected ComboBox selectedPlanePropsPanel_Type;

        protected lstCollObjectsCListBox lstCollObjects;
        protected ContextMenuStrip lstCollObjectsMenu;
        protected ToolStripMenuItem lstCollObjectsMenu_NewObject;
        protected ToolStripMenuItem lstCollObjectsMenu_DeleteObject;
        protected ToolStripMenuItem lstCollObjectsMenu_SnapObject;
        protected ToolStripMenuItem lstCollObjectsMenu_CloneObject;
        protected ToolStripMenuItem lstCollObjectsMenu_AssignObjectColor;
        protected ToolStripMenuItem lstCollObjectsMenu_HowManyLinks;
        protected ToolStripMenuItem lstCollObjectsMenu_HowManyPlanes;
        protected ToolStripMenuItem lstCollObjectsMenu_SetObjectTemporary;
        protected ToolStripMenuItem lstCollObjectsMenu_Unlink;
        protected ToolStripMenuItem lstCollObjectsMenu_UnlinkNoRelaMove;
        protected ToolStripSeparator lstCollObjectsMenu_Sep1;
        protected ToolStripSeparator lstCollObjectsMenu_Sep2;
        protected ToolStripSeparator lstCollObjectsMenu_Sep3;

        protected TreeView modelTree;
        protected ContextMenuStrip modelTreeMenu;
        protected ToolStripMenuItem modelTreeMenu_Assign;
        protected ToolStripMenuItem modelTreeMenu_AssignNoMove;
        protected ToolStripMenuItem modelTreeMenu_Snap;
        protected ToolStripSeparator modelTreeMenu_Sep1;

        protected Panel visibilityCheckPanel;
        protected CheckBox visibilityCheckPanel_ShowPolygons;
        protected CheckBox visibilityCheckPanel_ShowBones;


        protected TextBox selectedObjPropsPanel_TextModel;
        protected Label selectedObjPropsPanel_ModelLabel;
        protected Button selectedObjPropsPanel_Relink;
        protected TextBox selectedObjPropsPanel_TextBone;
        protected Label selectedObjPropsPanel_BoneLabel;
        protected Button selectedObjPropsPanel_Unlink;
        protected CheckBox selectedObjPropsPanel_CheckModuleControlled;
        protected CheckBox selectedObjPropsPanel_CheckUnknown;
        protected CheckBox selectedObjPropsPanel_CheckSSEUnknown;


		// Helps in the selection of collisions to be specific so that points in a small distance can click a plane.
		protected ToolStripMenuItem toolsStrip_Options_ScalePointsWithCamera_SelectOnly;
		// Kind of useless, but there if you'd like to see collision points as you scale.
		protected ToolStripMenuItem toolsStrip_Options_ScalePointsWithCamera_DisplayOnly;
		// Allows the selection of links/planes if they are associated with the object that is currently selected.
		protected ToolStripMenuItem toolsStrip_Options_SelectOnlyIfObjectEquals;
		// Shows the center location of the stage, which is (0, 0).
		protected ToolStripMenuItem toolsStrip_Options_ShowZeroZeroPoint;

        protected Panel animationPanel;
        protected Button animationPanel_PlayAnimations;
		// A panel shown when a collision is selected (can be a point or planes)
        protected Panel selectedMenuPanel;
        protected Button animationPanel_PrevFrame;
        protected Button animationPanel_NextFrame;


        // BrawlCrate buttons
        protected ToolStripSeparator toolsStrip_CameraSeparator; // Seperator for Camera controls
        protected ToolStripButton toolsStrip_PerspectiveCam;     // Goes into perspective mode
        protected ToolStripButton toolsStrip_FlipCollision;
        protected ToolStripButton toolsStrip_OrthographicCam;    // Goes into orthographic mode
        protected ToolStripButton toolsStrip_ShowBoundaries;
        protected ToolStripButton toolsStrip_ShowSpawns;
        protected ToolStripButton toolsStrip_ShowItems;

        protected ContextMenuStrip collisionOptions;

        // Added so that it allows clipboard operations on a selected collision/points
        protected ToolStripMenuItem clipboardCut;
        protected ToolStripMenuItem clipboardCopy;
        protected ToolStripMenuItem clipboardPaste;
        protected ToolStripMenuItem clipboardPaste_PasteDirectly;
        protected ToolStripMenuItem clipboardPaste_AdvancedPasteOptions;
        protected ToolStripMenuItem clipboardDelete;
        protected ToolStripMenuItem clipboardCopyOptions;
		// Ignores other collision objects if they are not the same as the selected one
        protected ToolStripMenuItem clipboardCopyOptions_IgnoreOtherObjects;
        protected ToolStripMenuItem clipboardPasteOptions;
        // This allows the collisions to be removed and have it pasted
        protected ToolStripMenuItem clipboardPasteOptions_PasteRemoveSelected;
        // This allows the collisions to be removed and combine them together
        protected ToolStripMenuItem clipboardPasteOptions_PasteOverrideSelected; 
		// Takes acutal points value when pasting links.
        public ToolStripMenuItem clipboardPasteOptions_ActualPointsValuesAreUsed;

        protected ToolStripMenuItem collisionOptions_MoveToNewObject;
        protected ToolStripSeparator collisionOptions_Sep1;
        protected ToolStripSeparator collisionOptions_Sep2;
        protected ToolStripMenuItem collisionOptions_Split;
        protected ToolStripMenuItem collisionOptions_Merge;
        protected ToolStripMenuItem collisionOptions_Flip;
        protected ToolStripMenuItem collisionOptions_Delete;
        protected ToolStripMenuItem collisionOptions_Transform;
        protected ToolStripMenuItem collisionOptions_AlignX;
        protected ToolStripMenuItem collisionOptions_AlignY;

        protected ToolStripSeparator toolsStrip_OverlaysSeparator; // Seperator for Overlay controls

		protected GoodColorDialog colorDialog;

        protected void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CollisionEditor));
            
			components = new Container();
            
			mainSplitter = new SplitContainer();
            collisionControlSplitter = new SplitContainer();

            modelTree = new TreeView();
            modelTreeMenu = new ContextMenuStrip(components);
            modelTreeMenu_Assign = new ToolStripMenuItem();
            modelTreeMenu_AssignNoMove = new ToolStripMenuItem();
            modelTreeMenu_Sep1 = new ToolStripSeparator();
            modelTreeMenu_Snap = new ToolStripMenuItem();

            visibilityCheckPanel = new Panel();
            visibilityCheckPanel_ShowBones = new CheckBox();
            visibilityCheckPanel_ShowPolygons = new CheckBox();
            visibilityCheckPanel_ShowAllModels = new CheckBox();

            lstCollObjects = new lstCollObjectsCListBox();
            lstCollObjectsMenu = new ContextMenuStrip(components);
            lstCollObjectsMenu_NewObject = new ToolStripMenuItem();
            lstCollObjectsMenu_CloneObject = new ToolStripMenuItem();
            lstCollObjectsMenu_Sep2 = new ToolStripSeparator();
            lstCollObjectsMenu_Unlink = new ToolStripMenuItem();
            lstCollObjectsMenu_UnlinkNoRelaMove = new ToolStripMenuItem();
            lstCollObjectsMenu_Sep1 = new ToolStripSeparator();
            lstCollObjectsMenu_SnapObject = new ToolStripMenuItem();
            lstCollObjectsMenu_DeleteObject = new ToolStripMenuItem();
            lstCollObjectsMenu_Sep3 = new ToolStripSeparator();
			lstCollObjectsMenu_AssignObjectColor = new ToolStripMenuItem();
			lstCollObjectsMenu_HowManyLinks = new ToolStripMenuItem();
			lstCollObjectsMenu_HowManyPlanes = new ToolStripMenuItem();
			lstCollObjectsMenu_SetObjectTemporary = new ToolStripMenuItem();


            selectedMenuPanel = new Panel();


            selectedPlanePropsPanel = new Panel();
            selectedPlanePropsPanel_UnknownFlagsGroup = new GroupBox();
            UnknownFlagsGroup_Check1 = new CheckBox();
            UnknownFlagsGroup_Check2 = new CheckBox();
            UnknownFlagsGroup_Check3 = new CheckBox();
            UnknownFlagsGroup_Check4 = new CheckBox();
            selectedPlanePropsPanel_AttributeFlagsGroup = new GroupBox();
            AttributeFlagsGroup_PlnCheckLeftLedge = new CheckBox();
            AttributeFlagsGroup_PlnCheckNoWalljump = new CheckBox();
            AttributeFlagsGroup_PlnCheckRightLedge = new CheckBox();
            AttributeFlagsGroup_PlnCheckRotating = new CheckBox();
            AttributeFlagsGroup_PlnCheckFallThrough = new CheckBox();
            selectedPlanePropsPanel_TargetGroup = new GroupBox();
            
			TargetGroup_CheckPKMNTrainer = new CheckBox();
            TargetGroup_CheckItems = new CheckBox();
            TargetGroup_CheckEverything = new CheckBox();

            selectedPlanePropsPanel_Material = new ComboBox();
            selectedPlanePropsPanel_Type = new ComboBox();
            selectedPlanePropsPanel_MaterialLabel = new Label();
            selectedPlanePropsPanel_TypeLabel = new Label();

            selectedPointPropsPanel = new Panel();
            selectedPointPropsPanel_YLabel = new Label();
            selectedPointPropsPanel_XLabel = new Label();
            selectedPointPropsPanel_YValue = new NumericInputBox();
            selectedPointPropsPanel_XValue = new NumericInputBox();


            selectedObjPropsPanel = new Panel();
            selectedObjPropsPanel_CheckSSEUnknown = new CheckBox();
            selectedObjPropsPanel_CheckModuleControlled = new CheckBox();
            selectedObjPropsPanel_CheckUnknown = new CheckBox();
            selectedObjPropsPanel_Unlink = new Button();
            selectedObjPropsPanel_Relink = new Button();
            selectedObjPropsPanel_TextBone = new TextBox();
            selectedObjPropsPanel_BoneLabel = new Label();
            selectedObjPropsPanel_TextModel = new TextBox();
            selectedObjPropsPanel_ModelLabel = new Label();


            toolsStrip = new ToolStrip();
            toolsStrip_UndoRedoSeparator = new ToolStripSeparator();
            toolsStrip_AlignmentSeparator = new ToolStripSeparator();
            toolsStrip_CollisionManipulationSeparator = new ToolStripSeparator();


            animationPanel = new Panel();
            animationPanel_PlayAnimations = new Button();
            animationPanel_PrevFrame = new Button();
            animationPanel_NextFrame = new Button();


            _modelPanel = new ModelPanel();


            toolsStripPanel = new Panel();

            toolsStrip_Undo = new ToolStripButton();
            toolsStrip_Redo = new ToolStripButton();
            toolsStrip_Split = new ToolStripButton();
            toolsStrip_Merge = new ToolStripButton();
            toolsStrip_FlipCollision = new ToolStripButton();
            toolsStrip_Delete = new ToolStripButton();
            toolsStrip_AlignX = new ToolStripButton();
            toolsStrip_AlignY = new ToolStripButton();
            toolsStrip_PerspectiveCam = new ToolStripButton();
            toolsStrip_OrthographicCam = new ToolStripButton();
            toolsStrip_ResetCam = new ToolStripButton();
            toolsStrip_CameraSeparator = new ToolStripSeparator();
            toolsStrip_ShowSpawns = new ToolStripButton();
            toolsStrip_ShowItems = new ToolStripButton();
            toolsStrip_ShowBoundaries = new ToolStripButton();
            toolsStrip_OverlaysSeparator = new ToolStripSeparator();
            toolsStrip_ResetSnap = new ToolStripButton();
            toolsStrip_Help = new ToolStripButton();

			toolsStrip_Options = new ToolStripSplitButton();
			toolsStrip_Options_ScalePointsWithCamera_SelectOnly = new ToolStripMenuItem();
			toolsStrip_Options_ScalePointsWithCamera_DisplayOnly = new ToolStripMenuItem();
			toolsStrip_Options_SelectOnlyIfObjectEquals = new ToolStripMenuItem();
			toolsStrip_Options_ShowZeroZeroPoint = new ToolStripMenuItem();
            
			UNUSED_toolsStripPanel_TrackBarRotation = new TrackBar();
			UNUSED_toolsStripPanel_ResetRotation = new Button();


            //Right-click on selected collision/collision view
            collisionOptions = new ContextMenuStrip(components);
            clipboardCopy = new ToolStripMenuItem();
            clipboardCut = new ToolStripMenuItem();
            clipboardPaste = new ToolStripMenuItem();
			clipboardPaste_PasteDirectly = new ToolStripMenuItem();
			clipboardPaste_AdvancedPasteOptions = new ToolStripMenuItem();
			clipboardCopyOptions = new ToolStripMenuItem();
			clipboardCopyOptions_IgnoreOtherObjects = new ToolStripMenuItem();
            clipboardPasteOptions = new ToolStripMenuItem();
			clipboardPasteOptions_PasteRemoveSelected = new ToolStripMenuItem();
			clipboardPasteOptions_PasteOverrideSelected = new ToolStripMenuItem();
			clipboardPasteOptions_ActualPointsValuesAreUsed = new ToolStripMenuItem();
            clipboardDelete = new ToolStripMenuItem();
            collisionOptions_Sep1 = new ToolStripSeparator();
            collisionOptions_MoveToNewObject = new ToolStripMenuItem();
            collisionOptions_Split = new ToolStripMenuItem();
            collisionOptions_Merge = new ToolStripMenuItem();
            collisionOptions_Sep2 = new ToolStripSeparator();
            collisionOptions_Flip = new ToolStripMenuItem();
            collisionOptions_Delete = new ToolStripMenuItem();
            collisionOptions_Transform = new ToolStripMenuItem();
            collisionOptions_AlignX = new ToolStripMenuItem();
            collisionOptions_AlignY = new ToolStripMenuItem();

			colorDialog = new GoodColorDialog();

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

            selectedPlanePropsPanel.SuspendLayout();
            selectedPlanePropsPanel_UnknownFlagsGroup.SuspendLayout();
            selectedPlanePropsPanel_AttributeFlagsGroup.SuspendLayout();
            selectedPlanePropsPanel_TargetGroup.SuspendLayout();
            selectedPointPropsPanel.SuspendLayout();
            selectedObjPropsPanel.SuspendLayout();
            animationPanel.SuspendLayout();
            toolsStripPanel.SuspendLayout();
            toolsStrip.SuspendLayout();

            ((ISupportInitialize)UNUSED_toolsStripPanel_TrackBarRotation).BeginInit();
            collisionOptions.SuspendLayout();
            SuspendLayout();

            // 
            // mainSplitter
            // 
            mainSplitter.Dock = DockStyle.Fill;
            mainSplitter.FixedPanel = FixedPanel.Panel1;
            mainSplitter.Location = new Drawing.Point(0, 0);
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
            mainSplitter.Size = new Drawing.Size(694, 467);
            mainSplitter.SplitterDistance = 209;
            mainSplitter.TabIndex = 1;
            // 
            // collisionControlSplitter
            // 
            collisionControlSplitter.Dock = DockStyle.Fill;
            collisionControlSplitter.Location = new Drawing.Point(0, 0);
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
            collisionControlSplitter.Panel2.Controls.Add(lstCollObjects);
            collisionControlSplitter.Panel2.Controls.Add(selectedMenuPanel);
            collisionControlSplitter.Panel2.Controls.Add(animationPanel);
            collisionControlSplitter.Size = new Drawing.Size(209, 467);
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
            modelTree.Location = new Drawing.Point(0, 17);
            modelTree.Name = "modelTree";
            modelTree.Size = new Drawing.Size(209, 225);
            modelTree.TabIndex = 4;
            modelTree.AfterCheck += new TreeViewEventHandler(modelTree_AfterCheck);
            modelTree.BeforeSelect += new TreeViewCancelEventHandler(modelTree_BeforeSelect);
            modelTree.AfterSelect += new TreeViewEventHandler(modelTree_AfterSelect);


            // 
            // modelTreeMenu
            // 
            modelTreeMenu.Items.AddRange(new ToolStripItem[]
            {
                modelTreeMenu_Assign,
                modelTreeMenu_AssignNoMove,
                modelTreeMenu_Sep1,
                modelTreeMenu_Snap
            });
            modelTreeMenu.Name = "modelTreeMenu";
            modelTreeMenu.Size = new Drawing.Size(239, 76);
            modelTreeMenu.Opening += new CancelEventHandler(modelTreeMenu_Opening);
            // 
            // modelTreeMenu_Assign
            // 
            modelTreeMenu_Assign.Name = "modelTreeMenu_Assign";
            modelTreeMenu_Assign.Size = new Drawing.Size(238, 22);
            modelTreeMenu_Assign.Text = "Assign";
            modelTreeMenu_Assign.Click += selectedObjPropsPanel_Relink_Click;
            // 
            // modelTreeMenu_AssignNoMove
            // 
            modelTreeMenu_AssignNoMove.Name = "modelTreeMenu_AssignNoMove";
            modelTreeMenu_AssignNoMove.Size = new Drawing.Size(238, 22);
            modelTreeMenu_AssignNoMove.Text = "Assign (No relative movement)";
            modelTreeMenu_AssignNoMove.Click += selectedObjPropsPanel_RelinkNoMove_Click;
            // 
            // modelTreeMenu_Sep1
            // 
            modelTreeMenu_Sep1.Name = "modelTreeMenu_Sep1";
            modelTreeMenu_Sep1.Size = new Drawing.Size(235, 6);
            // 
            // modelTreeMenu_Snap
            // 
            modelTreeMenu_Snap.Name = "modelTreeMenu_Snap";
            modelTreeMenu_Snap.Size = new Drawing.Size(238, 22);
            modelTreeMenu_Snap.Text = "Snap";
            modelTreeMenu_Snap.Click += modelTreeMenu_Snap_Click;


			// 
			// visibilityCheckPanel
			// 
			visibilityCheckPanel.Controls.AddRange(new Control[] 
			{ 
				visibilityCheckPanel_ShowBones,
				visibilityCheckPanel_ShowPolygons,
				visibilityCheckPanel_ShowAllModels
			});
            visibilityCheckPanel.Dock = DockStyle.Top;
            visibilityCheckPanel.Location = new Drawing.Point(0, 0);
            visibilityCheckPanel.Name = "visibilityCheckPanel";
            visibilityCheckPanel.Size = new Drawing.Size(209, 17);
            visibilityCheckPanel.TabIndex = 3;
            // 
            // visibilityCheckPanel_ShowBones
            // 
            visibilityCheckPanel_ShowBones.Location = new Drawing.Point(100, 0);
            visibilityCheckPanel_ShowBones.Name = "visibilityCheckPanel_ShowBones";
            visibilityCheckPanel_ShowBones.Padding = new Padding(1, 0, 0, 0);
            visibilityCheckPanel_ShowBones.Size = new Drawing.Size(67, 17);
            visibilityCheckPanel_ShowBones.TabIndex = 4;
            visibilityCheckPanel_ShowBones.Text = "Bones";
            visibilityCheckPanel_ShowBones.UseVisualStyleBackColor = true;
            visibilityCheckPanel_ShowBones.CheckedChanged += visibilityCheckPanel_ShowBones_CheckedChanged;
            // 
            // visibilityCheckPanel_ShowPolygons
            // 
            visibilityCheckPanel_ShowPolygons.Checked = true;
            visibilityCheckPanel_ShowPolygons.CheckState = CheckState.Checked;
            visibilityCheckPanel_ShowPolygons.Location = new Drawing.Point(44, 0);
            visibilityCheckPanel_ShowPolygons.Name = "visibilityCheckPanel_ShowPolygons";
            visibilityCheckPanel_ShowPolygons.Padding = new Padding(1, 0, 0, 0);
            visibilityCheckPanel_ShowPolygons.Size = new Drawing.Size(54, 17);
            visibilityCheckPanel_ShowPolygons.TabIndex = 3;
            visibilityCheckPanel_ShowPolygons.Text = "Poly";
            visibilityCheckPanel_ShowPolygons.ThreeState = true;
            visibilityCheckPanel_ShowPolygons.UseVisualStyleBackColor = true;
            visibilityCheckPanel_ShowPolygons.CheckStateChanged += visibilityCheckPanel_ShowPolygons_CheckStateChanged;
            // 
            // visibilityCheckPanel_ShowAllModels
            // 
            visibilityCheckPanel_ShowAllModels.Checked = true;
            visibilityCheckPanel_ShowAllModels.CheckState = CheckState.Checked;
            visibilityCheckPanel_ShowAllModels.Location = new Drawing.Point(0, 0);
            visibilityCheckPanel_ShowAllModels.Name = "visibilityCheckPanel_ShowAllModels";
            visibilityCheckPanel_ShowAllModels.Padding = new Padding(1, 0, 0, 0);
            visibilityCheckPanel_ShowAllModels.Size = new Drawing.Size(41, 17);
            visibilityCheckPanel_ShowAllModels.TabIndex = 2;
            visibilityCheckPanel_ShowAllModels.Text = "All";
            visibilityCheckPanel_ShowAllModels.UseVisualStyleBackColor = true;
            visibilityCheckPanel_ShowAllModels.CheckedChanged += visibilityCheckPanel_ShowAllModels_CheckedChanged;


			// 
			// lstCollObjects
			// 
            lstCollObjects.BorderStyle = BorderStyle.None;
            lstCollObjects.ContextMenuStrip = lstCollObjectsMenu;
            lstCollObjects.Dock = DockStyle.Fill;
            lstCollObjects.FormattingEnabled = true;
            lstCollObjects.IntegralHeight = false;
            lstCollObjects.Location = new Drawing.Point(0, 0);
            lstCollObjects.Name = "lstCollObjects";
            lstCollObjects.Size = new Drawing.Size(209, 82);
            lstCollObjects.TabIndex = 1;
            lstCollObjects.ItemCheck += new ItemCheckEventHandler(lstCollObjects_ItemCheck);
            lstCollObjects.SelectedValueChanged += lstCollObjects_SelectedValueChanged;
            lstCollObjects.MouseDown += new MouseEventHandler(lstCollObjects_MouseDown);
			lstCollObjects.GotFocus += LstCollObjects_GotFocus;
            // 
            // lstCollObjectsMenu
            // 
            lstCollObjectsMenu.Items.AddRange(new ToolStripItem[]
            {
                lstCollObjectsMenu_NewObject,
                lstCollObjectsMenu_Sep1,
                lstCollObjectsMenu_Unlink,
                lstCollObjectsMenu_UnlinkNoRelaMove,
                lstCollObjectsMenu_Sep2,
				lstCollObjectsMenu_AssignObjectColor,
				lstCollObjectsMenu_SetObjectTemporary,
                lstCollObjectsMenu_SnapObject,
                lstCollObjectsMenu_DeleteObject,
                lstCollObjectsMenu_Sep3,
				lstCollObjectsMenu_HowManyLinks,
				lstCollObjectsMenu_HowManyPlanes
            });
            lstCollObjectsMenu.Name = "lstCollObjectsMenu";
            lstCollObjectsMenu.Size = new Drawing.Size(238, 132);
            lstCollObjectsMenu.Opening += new CancelEventHandler(lstCollObjectsMenu_Opening);
            // 
            // lstCollObjectsMenu_Sep1
            // 
            lstCollObjectsMenu_Sep1.Name = "lstCollObjectsMenu_Sep1";
            lstCollObjectsMenu_Sep1.Size = new Drawing.Size(234, 6);
            // 
            // lstCollObjectsMenu_Sep2
            // 
            lstCollObjectsMenu_Sep2.Name = "lstCollObjectsMenu_Sep2";
            lstCollObjectsMenu_Sep2.Size = new Drawing.Size(234, 6);
            // 
            // lstCollObjectsMenu_Sep3
            // 
            lstCollObjectsMenu_Sep3.Name = "lstCollObjectsMenu_Sep3";
            lstCollObjectsMenu_Sep3.Size = new Drawing.Size(234, 6);
            // 
            // lstCollObjectsMenu_NewObject
            // 
            lstCollObjectsMenu_NewObject.Name = "lstCollObjectsMenu_NewObject";
            lstCollObjectsMenu_NewObject.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_NewObject.Text = "New Object";
            lstCollObjectsMenu_NewObject.Click += newObjectToolStripMenuItem_Click;
			lstCollObjectsMenu_NewObject.ToolTipText = "Creates a new object.";
            // 
            // lstCollObjectsMenu_Unlink
            // 
            lstCollObjectsMenu_Unlink.Name = "lstCollObjectsMenu_Unlink";
            lstCollObjectsMenu_Unlink.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_Unlink.Text = "Unlink";
            lstCollObjectsMenu_Unlink.Click += selectedObjPropsPanel_Unlink_Click;
            // 
            // lstCollObjectsMenu_UnlinkNoRelaMove
            // 
            lstCollObjectsMenu_UnlinkNoRelaMove.Name = "lstCollObjectsMenu_UnlinkNoRelaMove";
            lstCollObjectsMenu_UnlinkNoRelaMove.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_UnlinkNoRelaMove.Text = "Unlink (No relative movement)";
            lstCollObjectsMenu_UnlinkNoRelaMove.Click += lstCollObjectsMenu_UnlinkNoRelaMove_Click;
            // 
            // lstCollObjectsMenu_SnapObject
            // 
            lstCollObjectsMenu_SnapObject.Name = "lstCollObjectsMenu_SnapObject";
            lstCollObjectsMenu_SnapObject.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_SnapObject.Text = "Snap";
            lstCollObjectsMenu_SnapObject.Click += lstCollObjectsMenu_SnapObject_Click;
			lstCollObjectsMenu_SnapObject.ToolTipText = "??";
            // 
            // lstCollObjectsMenu_DeleteObject
            // 
            lstCollObjectsMenu_DeleteObject.Name = "lstCollObjectsMenu_DeleteObject";
            lstCollObjectsMenu_DeleteObject.ShortcutKeys = Keys.Control | Keys.Delete;
            lstCollObjectsMenu_DeleteObject.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_DeleteObject.Text = "Delete";
            lstCollObjectsMenu_DeleteObject.Click += lstCollObjectsMenu_DeleteObject_Click;
			lstCollObjectsMenu_DeleteObject.ToolTipText = "Deletes the selected object, associated points and planes.";
			// 
            // lstCollObjectsMenu_AssignObjectColor
            // 
            lstCollObjectsMenu_AssignObjectColor.Name = "lstCollObjectsMenu_AssignObjectColor";
            lstCollObjectsMenu_AssignObjectColor.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_AssignObjectColor.Text = "Set Color for Collisions";
			lstCollObjectsMenu_AssignObjectColor.Click += LstCollObjectsMenu_AssignObjectColor_Click;
			lstCollObjectsMenu_AssignObjectColor.ToolTipText = "Sets a color for this object." +
			"\n\nIf alpha is set to 0, then the color will be ignored and return back to its default collision display." +
			"\nNote: Assigning a color will not apply it to the actual collision data; it only applies it to the object for display purposes.";
			// 
            // lstCollObjectsMenu_HowManyLinks
            // 
            lstCollObjectsMenu_HowManyLinks.Name = "lstCollObjectsMenu_HowManyLinks";
            lstCollObjectsMenu_HowManyLinks.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_HowManyLinks.Text = "Links: ??";
			lstCollObjectsMenu_HowManyLinks.Enabled = false;
			// 
            // lstCollObjectsMenu_HowManyPlanes
            // 
            lstCollObjectsMenu_HowManyPlanes.Name = "lstCollObjectsMenu_HowManyPlanes";
            lstCollObjectsMenu_HowManyPlanes.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_HowManyPlanes.Text = "Planes: ??";
			lstCollObjectsMenu_HowManyPlanes.Enabled = false;
			// 
            // lstCollObjectsMenu_SetObjectTemporary
            // 
            lstCollObjectsMenu_SetObjectTemporary.Name = "lstCollObjectsMenu_SetObjectTemporary";
            lstCollObjectsMenu_SetObjectTemporary.Size = new Drawing.Size(237, 22);
            lstCollObjectsMenu_SetObjectTemporary.Text = "Set As Temporary Object";
			lstCollObjectsMenu_SetObjectTemporary.CheckOnClick = true;
			lstCollObjectsMenu_SetObjectTemporary.Enabled = false;
			lstCollObjectsMenu_SetObjectTemporary.ToolTipText = "This object will be deleted if this editor is closed.";


            // 
            // selectedMenuPanel
            // 
            selectedMenuPanel.Controls.AddRange(new Panel[] 
			{ 
				selectedPlanePropsPanel, 
				selectedPointPropsPanel, 
				selectedObjPropsPanel
			});
            selectedMenuPanel.Dock = DockStyle.Bottom;
            selectedMenuPanel.Location = new Drawing.Point(0, 82);
            selectedMenuPanel.Name = "selectedMenuPanel";
            selectedMenuPanel.Size = new Drawing.Size(209, 115);
            selectedMenuPanel.TabIndex = 16;

			// 
			// selectedPlanePropsPanel
			// 
			selectedPlanePropsPanel.Controls.AddRange(new Control[]
			{
				selectedPlanePropsPanel_UnknownFlagsGroup,
				selectedPlanePropsPanel_AttributeFlagsGroup,
				selectedPlanePropsPanel_TargetGroup,
				selectedPlanePropsPanel_Material,
				selectedPlanePropsPanel_MaterialLabel,
				selectedPlanePropsPanel_Type,
				selectedPlanePropsPanel_TypeLabel
			});
            selectedPlanePropsPanel.Dock = DockStyle.Bottom;
            selectedPlanePropsPanel.Location = new Drawing.Point(0, -273);
            selectedPlanePropsPanel.Name = "selectedPlanePropsPanel";
            selectedPlanePropsPanel.Size = new Drawing.Size(209, 188);
            selectedPlanePropsPanel.TabIndex = 0;
            selectedPlanePropsPanel.Visible = false;
            // 
            // selectedPlanePropsPanel_Material
            // 
            selectedPlanePropsPanel_Material.DropDownStyle = ComboBoxStyle.DropDownList;
            selectedPlanePropsPanel_Material.FormattingEnabled = true;
            selectedPlanePropsPanel_Material.Location = new Drawing.Point(66, 25);
            selectedPlanePropsPanel_Material.Name = "selectedPlanePropsPanel_Material";
            selectedPlanePropsPanel_Material.Size = new Drawing.Size(139, 21);
            selectedPlanePropsPanel_Material.TabIndex = 12;
            selectedPlanePropsPanel_Material.SelectedIndexChanged += selectedPlanePropsPanel_Material_SelectedIndexChanged;
            selectedPlanePropsPanel_Material.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            // 
            // selectedPlanePropsPanel_MaterialLabel
            // 
            selectedPlanePropsPanel_MaterialLabel.Location = new Drawing.Point(7, 25);
            selectedPlanePropsPanel_MaterialLabel.Name = "selectedPlanePropsPanel_MaterialLabel";
            selectedPlanePropsPanel_MaterialLabel.Size = new Drawing.Size(53, 21);
            selectedPlanePropsPanel_MaterialLabel.TabIndex = 8;
            selectedPlanePropsPanel_MaterialLabel.Text = "Material:";
            selectedPlanePropsPanel_MaterialLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // selectedPlanePropsPanel_Type
            // 
            selectedPlanePropsPanel_Type.DropDownStyle = ComboBoxStyle.DropDownList;
            selectedPlanePropsPanel_Type.FormattingEnabled = true;
            selectedPlanePropsPanel_Type.Location = new Drawing.Point(66, 4);
            selectedPlanePropsPanel_Type.Name = "selectedPlanePropsPanel_Type";
            selectedPlanePropsPanel_Type.Size = new Drawing.Size(139, 21);
            selectedPlanePropsPanel_Type.TabIndex = 5;
            selectedPlanePropsPanel_Type.SelectedIndexChanged += selectedPlanePropsPanel_Type_SelectedIndexChanged;
            selectedPlanePropsPanel_Type.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            // 
            // selectedPlanePropsPanel_TypeLabel
            // 
            selectedPlanePropsPanel_TypeLabel.Location = new Drawing.Point(7, 4);
            selectedPlanePropsPanel_TypeLabel.Name = "selectedPlanePropsPanel_TypeLabel";
            selectedPlanePropsPanel_TypeLabel.Size = new Drawing.Size(53, 21);
            selectedPlanePropsPanel_TypeLabel.TabIndex = 8;
            selectedPlanePropsPanel_TypeLabel.Text = "Type:";
            selectedPlanePropsPanel_TypeLabel.TextAlign = ContentAlignment.MiddleRight;

            // 
            // selectedPlanePropsPanel_AttributeFlagsGroup
            // 
            selectedPlanePropsPanel_AttributeFlagsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			selectedPlanePropsPanel_AttributeFlagsGroup.Controls.AddRange(new Control[]
			{
				AttributeFlagsGroup_PlnCheckLeftLedge,
				AttributeFlagsGroup_PlnCheckRightLedge,
				AttributeFlagsGroup_PlnCheckNoWalljump,
				AttributeFlagsGroup_PlnCheckRotating,
				AttributeFlagsGroup_PlnCheckFallThrough
			});
            selectedPlanePropsPanel_AttributeFlagsGroup.Location = new Drawing.Point(0, 102);
            selectedPlanePropsPanel_AttributeFlagsGroup.Margin = new Padding(0);
            selectedPlanePropsPanel_AttributeFlagsGroup.Name = "selectedPlanePropsPanel_AttributeFlagsGroup";
            selectedPlanePropsPanel_AttributeFlagsGroup.Padding = new Padding(0);
            selectedPlanePropsPanel_AttributeFlagsGroup.Size = new Drawing.Size(104, 160);
            selectedPlanePropsPanel_AttributeFlagsGroup.TabIndex = 13;
            selectedPlanePropsPanel_AttributeFlagsGroup.TabStop = false;
            selectedPlanePropsPanel_AttributeFlagsGroup.Text = "Flags";
            // 
            // AttributeFlagsGroup_PlnCheckLeftLedge
            // 
            AttributeFlagsGroup_PlnCheckLeftLedge.Location = new Drawing.Point(8, 33);
            AttributeFlagsGroup_PlnCheckLeftLedge.Margin = new Padding(0);
            AttributeFlagsGroup_PlnCheckLeftLedge.Name = "AttributeFlagsGroup_PlnCheckLeftLedge";
            AttributeFlagsGroup_PlnCheckLeftLedge.Size = new Drawing.Size(86, 18);
            AttributeFlagsGroup_PlnCheckLeftLedge.TabIndex = 4;
            AttributeFlagsGroup_PlnCheckLeftLedge.Text = "Left Ledge";
            AttributeFlagsGroup_PlnCheckLeftLedge.UseVisualStyleBackColor = true;
            AttributeFlagsGroup_PlnCheckLeftLedge.CheckedChanged += AttributeFlagsGroup_PlnCheckLeftLedge_CheckedChanged;
            // 
            // AttributeFlagsGroup_PlnCheckNoWalljump
            // 
            AttributeFlagsGroup_PlnCheckNoWalljump.Location = new Drawing.Point(8, 65);
            AttributeFlagsGroup_PlnCheckNoWalljump.Margin = new Padding(0);
            AttributeFlagsGroup_PlnCheckNoWalljump.Name = "AttributeFlagsGroup_PlnCheckNoWalljump";
            AttributeFlagsGroup_PlnCheckNoWalljump.Size = new Drawing.Size(90, 18);
            AttributeFlagsGroup_PlnCheckNoWalljump.TabIndex = 2;
            AttributeFlagsGroup_PlnCheckNoWalljump.Text = "No Walljump";
            AttributeFlagsGroup_PlnCheckNoWalljump.UseVisualStyleBackColor = true;
            AttributeFlagsGroup_PlnCheckNoWalljump.CheckedChanged += AttributeFlagsGroup_PlnCheckNoWalljump_CheckedChanged;
            // 
            // AttributeFlagsGroup_PlnCheckRightLedge
            // 
            AttributeFlagsGroup_PlnCheckRightLedge.Location = new Drawing.Point(8, 49);
            AttributeFlagsGroup_PlnCheckRightLedge.Margin = new Padding(0);
            AttributeFlagsGroup_PlnCheckRightLedge.Name = "AttributeFlagsGroup_PlnCheckRightLedge";
            AttributeFlagsGroup_PlnCheckRightLedge.Size = new Drawing.Size(86, 18);
            AttributeFlagsGroup_PlnCheckRightLedge.TabIndex = 1;
            AttributeFlagsGroup_PlnCheckRightLedge.Text = "Right Ledge";
            AttributeFlagsGroup_PlnCheckRightLedge.UseVisualStyleBackColor = true;
            AttributeFlagsGroup_PlnCheckRightLedge.CheckedChanged += AttributeFlagsGroup_PlnCheckRightLedge_CheckedChanged;
            // 
            // AttributeFlagsGroup_PlnCheckRotating
            // 
            AttributeFlagsGroup_PlnCheckRotating.Location = new Drawing.Point(8, 81);
            AttributeFlagsGroup_PlnCheckRotating.Margin = new Padding(0);
            AttributeFlagsGroup_PlnCheckRotating.Name = "AttributeFlagsGroup_PlnCheckRotating";
            AttributeFlagsGroup_PlnCheckRotating.Size = new Drawing.Size(86, 18);
            AttributeFlagsGroup_PlnCheckRotating.TabIndex = 4;
            AttributeFlagsGroup_PlnCheckRotating.Text = "Rotating";
            AttributeFlagsGroup_PlnCheckRotating.UseVisualStyleBackColor = true;
            AttributeFlagsGroup_PlnCheckRotating.CheckedChanged += AttributeFlagsGroup_PlnCheckRotating_CheckedChanged;
            // 
            // AttributeFlagsGroup_PlnCheckFallThrough
            // 
            AttributeFlagsGroup_PlnCheckFallThrough.Location = new Drawing.Point(8, 17);
            AttributeFlagsGroup_PlnCheckFallThrough.Margin = new Padding(0);
            AttributeFlagsGroup_PlnCheckFallThrough.Name = "AttributeFlagsGroup_PlnCheckFallThrough";
            AttributeFlagsGroup_PlnCheckFallThrough.Size = new Drawing.Size(90, 18);
            AttributeFlagsGroup_PlnCheckFallThrough.TabIndex = 0;
            AttributeFlagsGroup_PlnCheckFallThrough.Text = "Fall-Through";
            AttributeFlagsGroup_PlnCheckFallThrough.UseVisualStyleBackColor = true;
            AttributeFlagsGroup_PlnCheckFallThrough.CheckedChanged += AttributeFlagsGroup_PlnCheckFallThrough_CheckedChanged;

            // 
            // selectedPlanePropsPanel_TargetGroup
            // 
            selectedPlanePropsPanel_TargetGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            selectedPlanePropsPanel_TargetGroup.Controls.Add(TargetGroup_CheckPKMNTrainer);
            selectedPlanePropsPanel_TargetGroup.Controls.Add(TargetGroup_CheckItems);
            selectedPlanePropsPanel_TargetGroup.Controls.Add(TargetGroup_CheckEverything);
            selectedPlanePropsPanel_TargetGroup.Location = new Drawing.Point(0, 50);
            selectedPlanePropsPanel_TargetGroup.Margin = new Padding(0);
            selectedPlanePropsPanel_TargetGroup.Name = "selectedPlanePropsPanel_TargetGroup";
            selectedPlanePropsPanel_TargetGroup.Padding = new Padding(0);
            selectedPlanePropsPanel_TargetGroup.Size = new Drawing.Size(208, 71);
            selectedPlanePropsPanel_TargetGroup.TabIndex = 14;
            selectedPlanePropsPanel_TargetGroup.TabStop = false;
            selectedPlanePropsPanel_TargetGroup.Text = "Collision Targets";
            // 
            // TargetGroup_CheckPKMNTrainer
            // 
            TargetGroup_CheckPKMNTrainer.Location = new Drawing.Point(82, 33);
            TargetGroup_CheckPKMNTrainer.Margin = new Padding(0);
            TargetGroup_CheckPKMNTrainer.Name = "TargetGroup_CheckPKMNTrainer";
            TargetGroup_CheckPKMNTrainer.Size = new Drawing.Size(116, 18);
            TargetGroup_CheckPKMNTrainer.TabIndex = 3;
            TargetGroup_CheckPKMNTrainer.Text = "Pokémon Trainer";
            TargetGroup_CheckPKMNTrainer.UseVisualStyleBackColor = true;
            TargetGroup_CheckPKMNTrainer.CheckedChanged += TargetGroup_CheckPKMNTrainer_CheckedChanged;
            // 
            // TargetGroup_CheckItems
            // 
            TargetGroup_CheckItems.Location = new Drawing.Point(8, 33);
            TargetGroup_CheckItems.Margin = new Padding(0);
            TargetGroup_CheckItems.Name = "TargetGroup_CheckItems";
            TargetGroup_CheckItems.Size = new Drawing.Size(86, 18);
            TargetGroup_CheckItems.TabIndex = 3;
            TargetGroup_CheckItems.Text = "Items";
            TargetGroup_CheckItems.UseVisualStyleBackColor = true;
            TargetGroup_CheckItems.CheckedChanged += TargetGroup_CheckItems_CheckedChanged;
            // 
            // TargetGroup_CheckEverything
            // 
            TargetGroup_CheckEverything.Location = new Drawing.Point(8, 17);
            TargetGroup_CheckEverything.Margin = new Padding(0);
            TargetGroup_CheckEverything.Name = "TargetGroup_CheckEverything";
            TargetGroup_CheckEverything.Size = new Drawing.Size(194, 18);
            TargetGroup_CheckEverything.TabIndex = 4;
            TargetGroup_CheckEverything.Text = "Everything";
            TargetGroup_CheckEverything.UseVisualStyleBackColor = true;
            TargetGroup_CheckEverything.CheckedChanged += TargetGroup_CheckEverything_CheckedChanged;

            // 
            // selectedPlanePropsPanel_UnknownFlagsGroup
            // 
            selectedPlanePropsPanel_UnknownFlagsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            selectedPlanePropsPanel_UnknownFlagsGroup.Controls.Add(UnknownFlagsGroup_Check1);
            selectedPlanePropsPanel_UnknownFlagsGroup.Controls.Add(UnknownFlagsGroup_Check2);
            selectedPlanePropsPanel_UnknownFlagsGroup.Controls.Add(UnknownFlagsGroup_Check3);
            selectedPlanePropsPanel_UnknownFlagsGroup.Controls.Add(UnknownFlagsGroup_Check4);
            selectedPlanePropsPanel_UnknownFlagsGroup.Location = new Drawing.Point(104, 102);
            selectedPlanePropsPanel_UnknownFlagsGroup.Margin = new Padding(0);
            selectedPlanePropsPanel_UnknownFlagsGroup.Name = "selectedPlanePropsPanel_UnknownFlagsGroup";
            selectedPlanePropsPanel_UnknownFlagsGroup.Padding = new Padding(0);
            selectedPlanePropsPanel_UnknownFlagsGroup.Size = new Drawing.Size(105, 160);
            selectedPlanePropsPanel_UnknownFlagsGroup.TabIndex = 14;
            selectedPlanePropsPanel_UnknownFlagsGroup.TabStop = false;
            // 
            // UnknownFlagsGroup_Check1
            // 
            UnknownFlagsGroup_Check1.Location = new Drawing.Point(8, 17);
            UnknownFlagsGroup_Check1.Margin = new Padding(0);
            UnknownFlagsGroup_Check1.Name = "UnknownFlagsGroup_Check1";
            UnknownFlagsGroup_Check1.Size = new Drawing.Size(86, 18);
            UnknownFlagsGroup_Check1.TabIndex = 3;
            UnknownFlagsGroup_Check1.Text = "Unknown SSE";
            UnknownFlagsGroup_Check1.UseVisualStyleBackColor = true;
            UnknownFlagsGroup_Check1.CheckedChanged += UnknownFlagsGroup_Check1_CheckedChanged;
            // 
            // UnknownFlagsGroup_Check2
            // 
            UnknownFlagsGroup_Check2.Location = new Drawing.Point(8, 33);
            UnknownFlagsGroup_Check2.Margin = new Padding(0);
            UnknownFlagsGroup_Check2.Name = "UnknownFlagsGroup_Check2";
            UnknownFlagsGroup_Check2.Size = new Drawing.Size(86, 18);
            UnknownFlagsGroup_Check2.TabIndex = 3;
            UnknownFlagsGroup_Check2.Text = "Unknown 2";
            UnknownFlagsGroup_Check2.UseVisualStyleBackColor = true;
            UnknownFlagsGroup_Check2.CheckedChanged += UnknownFlagsGroup_Check2_CheckedChanged;
            // 
            // UnknownFlagsGroup_Check3
            // 
            UnknownFlagsGroup_Check3.Location = new Drawing.Point(8, 49);
            UnknownFlagsGroup_Check3.Margin = new Padding(0);
            UnknownFlagsGroup_Check3.Name = "UnknownFlagsGroup_Check3";
            UnknownFlagsGroup_Check3.Size = new Drawing.Size(86, 18);
            UnknownFlagsGroup_Check3.TabIndex = 3;
            UnknownFlagsGroup_Check3.Text = "Unknown 3";
            UnknownFlagsGroup_Check3.UseVisualStyleBackColor = true;
            UnknownFlagsGroup_Check3.CheckedChanged += UnknownFlagsGroup_Check3_CheckedChanged;
            // 
            // UnknownFlagsGroup_Check4
            // 
            UnknownFlagsGroup_Check4.Location = new Drawing.Point(8, 65);
            UnknownFlagsGroup_Check4.Margin = new Padding(0);
            UnknownFlagsGroup_Check4.Name = "UnknownFlagsGroup_Check4";
            UnknownFlagsGroup_Check4.Size = new Drawing.Size(86, 18);
            UnknownFlagsGroup_Check4.TabIndex = 3;
            UnknownFlagsGroup_Check4.Text = "Unknown 4";
            UnknownFlagsGroup_Check4.UseVisualStyleBackColor = true;
            UnknownFlagsGroup_Check4.CheckedChanged += UnknownFlagsGroup_Check4_CheckedChanged;


            // 
            // selectedPointPropsPanel
            // 
            selectedPointPropsPanel.Controls.Add(selectedPointPropsPanel_YLabel);
            selectedPointPropsPanel.Controls.Add(selectedPointPropsPanel_YValue);
            selectedPointPropsPanel.Controls.Add(selectedPointPropsPanel_XLabel);
            selectedPointPropsPanel.Controls.Add(selectedPointPropsPanel_XValue);
            selectedPointPropsPanel.Dock = DockStyle.Bottom;
            selectedPointPropsPanel.Location = new Drawing.Point(0, -85);
            selectedPointPropsPanel.Name = "selectedPointPropsPanel";
            selectedPointPropsPanel.Size = new Drawing.Size(209, 70);
            selectedPointPropsPanel.TabIndex = 15;
            selectedPointPropsPanel.Visible = false;
            // 
            // selectedPointPropsPanel_XLabel
            // 
            selectedPointPropsPanel_XLabel.BorderStyle = BorderStyle.FixedSingle;
            selectedPointPropsPanel_XLabel.Location = new Drawing.Point(18, 13);
            selectedPointPropsPanel_XLabel.Name = "selectedPointPropsPanel_XLabel";
            selectedPointPropsPanel_XLabel.Size = new Drawing.Size(20, 20);
            selectedPointPropsPanel_XLabel.TabIndex = 1;
            selectedPointPropsPanel_XLabel.Text = "X";
            selectedPointPropsPanel_XLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // selectedPointPropsPanel_YLabel
            // 
            selectedPointPropsPanel_YLabel.BorderStyle = BorderStyle.FixedSingle;
            selectedPointPropsPanel_YLabel.Location = new Drawing.Point(18, 32);
            selectedPointPropsPanel_YLabel.Name = "selectedPointPropsPanel_YLabel";
            selectedPointPropsPanel_YLabel.Size = new Drawing.Size(20, 20);
            selectedPointPropsPanel_YLabel.TabIndex = 3;
            selectedPointPropsPanel_YLabel.Text = "Y";
            selectedPointPropsPanel_YLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // selectedPointPropsPanel_XValue
            // 
            selectedPointPropsPanel_XValue.BorderStyle = BorderStyle.FixedSingle;
            selectedPointPropsPanel_XValue.Integral = false;
            selectedPointPropsPanel_XValue.Location = new Drawing.Point(37, 13);
            selectedPointPropsPanel_XValue.MaximumValue = 3.402823E+38F;
            selectedPointPropsPanel_XValue.MinimumValue = -3.402823E+38F;
            selectedPointPropsPanel_XValue.Name = "selectedPointPropsPanel_XValue";
            selectedPointPropsPanel_XValue.Size = new Drawing.Size(152, 20);
            selectedPointPropsPanel_XValue.TabIndex = 0;
            selectedPointPropsPanel_XValue.Text = "0";
            selectedPointPropsPanel_XValue.ValueChanged += selectedPointPropsPanel_XValue_ValueChanged;
            selectedPointPropsPanel_XValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // selectedPointPropsPanel_YValue
            // 
            selectedPointPropsPanel_YValue.BorderStyle = BorderStyle.FixedSingle;
            selectedPointPropsPanel_YValue.Integral = false;
            selectedPointPropsPanel_YValue.Location = new Drawing.Point(37, 32);
            selectedPointPropsPanel_YValue.MaximumValue = 3.402823E+38F;
            selectedPointPropsPanel_YValue.MinimumValue = -3.402823E+38F;
            selectedPointPropsPanel_YValue.Name = "selectedPointPropsPanel_YValue";
            selectedPointPropsPanel_YValue.Size = new Drawing.Size(152, 20);
            selectedPointPropsPanel_YValue.TabIndex = 2;
            selectedPointPropsPanel_YValue.Text = "0";
            selectedPointPropsPanel_YValue.ValueChanged += selectedPointPropsPanel_YValue_ValueChanged;
            selectedPointPropsPanel_YValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;


			// 
			// selectedObjPropsPanel
			// 
			selectedObjPropsPanel.Controls.AddRange(new Control[] 
			{
				selectedObjPropsPanel_CheckSSEUnknown,
				selectedObjPropsPanel_CheckModuleControlled,
				selectedObjPropsPanel_CheckSSEUnknown,
				selectedObjPropsPanel_Unlink,
				selectedObjPropsPanel_Relink,
				selectedObjPropsPanel_TextBone,
				selectedObjPropsPanel_BoneLabel,
				selectedObjPropsPanel_TextModel,
				selectedObjPropsPanel_ModelLabel
			});
            selectedObjPropsPanel.Dock = DockStyle.Bottom;
            selectedObjPropsPanel.Location = new Drawing.Point(0, -15);
            selectedObjPropsPanel.Name = "selectedObjPropsPanel";
            selectedObjPropsPanel.Size = new Drawing.Size(209, 130);
            selectedObjPropsPanel.TabIndex = 1;
            selectedObjPropsPanel.Visible = false;
            // 
            // selectedObjPropsPanel_CheckSSEUnknown
            // 
            selectedObjPropsPanel_CheckSSEUnknown.AutoSize = true;
            selectedObjPropsPanel_CheckSSEUnknown.Location = new Drawing.Point(10, 102);
            selectedObjPropsPanel_CheckSSEUnknown.Name = "selectedObjPropsPanel_CheckSSEUnknown";
            selectedObjPropsPanel_CheckSSEUnknown.Size = new Drawing.Size(96, 17);
            selectedObjPropsPanel_CheckSSEUnknown.TabIndex = 15;
            selectedObjPropsPanel_CheckSSEUnknown.Text = "SSE Unknown";
            selectedObjPropsPanel_CheckSSEUnknown.UseVisualStyleBackColor = true;
            selectedObjPropsPanel_CheckSSEUnknown.CheckedChanged += selectedObjPropsPanel_CheckSSEUnknown_CheckedChanged;
            // 
            // selectedObjPropsPanel_CheckModuleControlled
            // 
            selectedObjPropsPanel_CheckModuleControlled.AutoSize = true;
            selectedObjPropsPanel_CheckModuleControlled.Location = new Drawing.Point(10, 79);
            selectedObjPropsPanel_CheckModuleControlled.Name = "selectedObjPropsPanel_CheckModuleControlled";
            selectedObjPropsPanel_CheckModuleControlled.Size = new Drawing.Size(111, 17);
            selectedObjPropsPanel_CheckModuleControlled.TabIndex = 14;
            selectedObjPropsPanel_CheckModuleControlled.Text = "Module Controlled";
            selectedObjPropsPanel_CheckModuleControlled.UseVisualStyleBackColor = true;
            selectedObjPropsPanel_CheckModuleControlled.CheckedChanged += selectedObjPropsPanel_CheckModuleControlled_CheckedChanged;
            // 
            // selectedObjPropsPanel_CheckUnknown
            // 
            selectedObjPropsPanel_CheckUnknown.AutoSize = true;
            selectedObjPropsPanel_CheckUnknown.Location = new Drawing.Point(10, 56);
            selectedObjPropsPanel_CheckUnknown.Name = "selectedObjPropsPanel_CheckUnknown";
            selectedObjPropsPanel_CheckUnknown.Size = new Drawing.Size(72, 17);
            selectedObjPropsPanel_CheckUnknown.TabIndex = 13;
            selectedObjPropsPanel_CheckUnknown.Text = "Unknown";
            selectedObjPropsPanel_CheckUnknown.UseVisualStyleBackColor = true;
            selectedObjPropsPanel_CheckUnknown.CheckedChanged += selectedObjPropsPanel_CheckUnknown_CheckedChanged;
			// 
			// selectedObjPropsPanel_Unlink
			// 
			selectedObjPropsPanel_Unlink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectedObjPropsPanel_Unlink.Location = new Drawing.Point(177, 22);
            selectedObjPropsPanel_Unlink.Name = "selectedObjPropsPanel_Unlink";
            selectedObjPropsPanel_Unlink.Size = new Drawing.Size(28, 21);
            selectedObjPropsPanel_Unlink.TabIndex = 12;
            selectedObjPropsPanel_Unlink.Text = "-";
            selectedObjPropsPanel_Unlink.UseVisualStyleBackColor = true;
            selectedObjPropsPanel_Unlink.Click += selectedObjPropsPanel_Unlink_Click;
            // 
            // selectedObjPropsPanel_Relink
            // 
			selectedObjPropsPanel_Relink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectedObjPropsPanel_Relink.Location = new Drawing.Point(177, 2);
            selectedObjPropsPanel_Relink.Name = "selectedObjPropsPanel_Relink";
            selectedObjPropsPanel_Relink.Size = new Drawing.Size(28, 21);
            selectedObjPropsPanel_Relink.TabIndex = 4;
            selectedObjPropsPanel_Relink.Text = "+";
            selectedObjPropsPanel_Relink.UseVisualStyleBackColor = true;
            selectedObjPropsPanel_Relink.Click += selectedObjPropsPanel_Relink_Click;
            // 
            // selectedObjPropsPanel_TextBone
            // 
			selectedObjPropsPanel_TextBone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            selectedObjPropsPanel_TextBone.Location = new Drawing.Point(49, 23);
            selectedObjPropsPanel_TextBone.Name = "selectedObjPropsPanel_TextBone";
            selectedObjPropsPanel_TextBone.ReadOnly = true;
            selectedObjPropsPanel_TextBone.Size = new Drawing.Size(126, 20);
            selectedObjPropsPanel_TextBone.TabIndex = 3;
            // 
            // selectedObjPropsPanel_BoneLabel
            // 
            selectedObjPropsPanel_BoneLabel.Location = new Drawing.Point(4, 23);
            selectedObjPropsPanel_BoneLabel.Name = "selectedObjPropsPanel_BoneLabel";
            selectedObjPropsPanel_BoneLabel.Size = new Drawing.Size(42, 20);
            selectedObjPropsPanel_BoneLabel.TabIndex = 2;
            selectedObjPropsPanel_BoneLabel.Text = "Bone:";
            selectedObjPropsPanel_BoneLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // selectedObjPropsPanel_TextModel
            // 
			selectedObjPropsPanel_TextModel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            selectedObjPropsPanel_TextModel.Location = new Drawing.Point(49, 3);
            selectedObjPropsPanel_TextModel.Name = "selectedObjPropsPanel_TextModel";
            selectedObjPropsPanel_TextModel.ReadOnly = true;
            selectedObjPropsPanel_TextModel.Size = new Drawing.Size(126, 20);
            selectedObjPropsPanel_TextModel.TabIndex = 1;
            // 
            // selectedObjPropsPanel_ModelLabel
            // 
            selectedObjPropsPanel_ModelLabel.Location = new Drawing.Point(4, 3);
            selectedObjPropsPanel_ModelLabel.Name = "selectedObjPropsPanel_ModelLabel";
            selectedObjPropsPanel_ModelLabel.Size = new Drawing.Size(42, 20);
            selectedObjPropsPanel_ModelLabel.TabIndex = 0;
            selectedObjPropsPanel_ModelLabel.Text = "Model:";
            selectedObjPropsPanel_ModelLabel.TextAlign = ContentAlignment.MiddleRight;


            // 
            // animationPanel
            // 
            animationPanel.Controls.Add(animationPanel_PlayAnimations);
            animationPanel.Controls.Add(animationPanel_PrevFrame);
            animationPanel.Controls.Add(animationPanel_NextFrame);
            animationPanel.Dock = DockStyle.Bottom;
            animationPanel.Enabled = false;
            animationPanel.Location = new Drawing.Point(0, 197);
            animationPanel.Name = "animationPanel";
            animationPanel.Size = new Drawing.Size(209, 24);
            animationPanel.TabIndex = 17;
            animationPanel.Visible = false;
            // 
            // animationPanel_PlayAnimations
            // 
            animationPanel_PlayAnimations.Dock = DockStyle.Fill;
            animationPanel_PlayAnimations.Location = new Drawing.Point(35, 0);
            animationPanel_PlayAnimations.Name = "animationPanel_PlayAnimations";
            animationPanel_PlayAnimations.Size = new Drawing.Size(139, 24);
            animationPanel_PlayAnimations.TabIndex = 16;
            animationPanel_PlayAnimations.Text = "Play Animations";
            animationPanel_PlayAnimations.UseVisualStyleBackColor = true;
            animationPanel_PlayAnimations.Click += animationPanel_PlayAnimations_Click;
            // 
            // animationPanel_PrevFrame
            // 
            animationPanel_PrevFrame.Dock = DockStyle.Left;
            animationPanel_PrevFrame.Location = new Drawing.Point(0, 0);
            animationPanel_PrevFrame.Name = "animationPanel_PrevFrame";
            animationPanel_PrevFrame.Size = new Drawing.Size(35, 24);
            animationPanel_PrevFrame.TabIndex = 18;
            animationPanel_PrevFrame.Text = "<";
            animationPanel_PrevFrame.UseVisualStyleBackColor = true;
            animationPanel_PrevFrame.Click += animationPanel_PrevFrame_Click;
            // 
            // animationPanel_NextFrame
            // 
            animationPanel_NextFrame.Dock = DockStyle.Right;
            animationPanel_NextFrame.Location = new Drawing.Point(174, 0);
            animationPanel_NextFrame.Name = "animationPanel_NextFrame";
            animationPanel_NextFrame.Size = new Drawing.Size(35, 24);
            animationPanel_NextFrame.TabIndex = 17;
            animationPanel_NextFrame.Text = ">";
            animationPanel_NextFrame.UseVisualStyleBackColor = true;
            animationPanel_NextFrame.Click += animationPanel_NextFrame_Click;


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
			// toolsStripPanel
			// 
			toolsStripPanel.Controls.AddRange(new Control[] 
			{
				toolsStrip, 
				UNUSED_toolsStripPanel_ResetRotation, 
				UNUSED_toolsStripPanel_TrackBarRotation
			});
            toolsStripPanel.Controls.Add(toolsStrip);
            toolsStripPanel.Controls.Add(UNUSED_toolsStripPanel_ResetRotation);
            toolsStripPanel.Controls.Add(UNUSED_toolsStripPanel_TrackBarRotation);
            toolsStripPanel.BackColor = Color.WhiteSmoke;
            toolsStripPanel.Dock = DockStyle.Top;
            toolsStripPanel.Location = new Drawing.Point(0, 0);
            toolsStripPanel.Name = "toolsStripPanel";
            toolsStripPanel.Size = new Drawing.Size(481, 25);
            toolsStripPanel.TabIndex = 2;
            // 
            // toolsStrip
            // 
            toolsStrip.BackColor = Color.WhiteSmoke;
            toolsStrip.Dock = DockStyle.Fill;
            toolsStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolsStrip.Items.AddRange(new ToolStripItem[]
            {
                toolsStrip_Undo,
                toolsStrip_Redo,
                toolsStrip_UndoRedoSeparator,
                toolsStrip_Split,
                toolsStrip_Merge,
                toolsStrip_FlipCollision,
                toolsStrip_Delete,
                toolsStrip_CollisionManipulationSeparator,
                toolsStrip_AlignX,
                toolsStrip_AlignY,
                toolsStrip_AlignmentSeparator,
                toolsStrip_PerspectiveCam,
                toolsStrip_OrthographicCam,
                toolsStrip_ResetCam,
                toolsStrip_CameraSeparator,
                toolsStrip_ShowSpawns,
                toolsStrip_ShowItems,
                toolsStrip_ShowBoundaries,
                toolsStrip_OverlaysSeparator,
                toolsStrip_ResetSnap,
				toolsStrip_Options,
                toolsStrip_Help
            });
            toolsStrip.Location = new Drawing.Point(0, 0);
            toolsStrip.Name = "toolsStrip";
            toolsStrip.Size = new Drawing.Size(335, 25);
            toolsStrip.TabIndex = 1;
            toolsStrip.Text = "toolsStrip";
            // 
            // toolsStrip_Undo
            // 
            toolsStrip_Undo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_Undo.Enabled = false;
            toolsStrip_Undo.ImageTransparentColor = Color.Magenta;
            toolsStrip_Undo.Name = "toolsStrip_Undo";
            toolsStrip_Undo.Size = new Drawing.Size(40, 22);
            toolsStrip_Undo.Text = "Undo";
            toolsStrip_Undo.Click += Undo;
            // 
            // toolsStrip_Redo
            // 
            toolsStrip_Redo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_Redo.Enabled = false;
            toolsStrip_Redo.ImageTransparentColor = Color.Magenta;
            toolsStrip_Redo.Name = "toolsStrip_Redo";
            toolsStrip_Redo.Size = new Drawing.Size(38, 22);
            toolsStrip_Redo.Text = "Redo";
            toolsStrip_Redo.Click += Redo;
            // 
            // toolsStrip_UndoRedoSeparator
            // 
            toolsStrip_UndoRedoSeparator.Name = "toolsStrip_UndoRedoSeparator";
            toolsStrip_UndoRedoSeparator.Size = new Drawing.Size(6, 25);
            // 
            // toolsStrip_CollisionManipulationSeparator
            // 
            toolsStrip_CollisionManipulationSeparator.Name = "toolsStrip_CollisionManipulationSeparator";
            toolsStrip_CollisionManipulationSeparator.Size = new Drawing.Size(6, 25);
            // 
            // toolsStrip_AlignmentSeparator
            // 
            toolsStrip_AlignmentSeparator.Name = "toolsStrip_AlignmentSeparator";
            toolsStrip_AlignmentSeparator.Size = new Drawing.Size(6, 25);
            // 
            // toolsStrip_Split
            // 
            toolsStrip_Split.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_Split.Enabled = false;
            toolsStrip_Split.ImageTransparentColor = Color.Magenta;
            toolsStrip_Split.Name = "toolsStrip_Split";
            toolsStrip_Split.Size = new Drawing.Size(34, 22);
            toolsStrip_Split.Text = "Split";
            toolsStrip_Split.Click += btnSplit_Click;
            // 
            // toolsStrip_Merge
            // 
            toolsStrip_Merge.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_Merge.Enabled = false;
            toolsStrip_Merge.ImageTransparentColor = Color.Magenta;
            toolsStrip_Merge.Name = "toolsStrip_Merge";
            toolsStrip_Merge.Size = new Drawing.Size(45, 22);
            toolsStrip_Merge.Text = "Merge";
            toolsStrip_Merge.Click += btnMerge_Click;
            // 
            // toolsStrip_FlipCollision
            // 
            toolsStrip_FlipCollision.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_FlipCollision.Enabled = false;
            toolsStrip_FlipCollision.ImageTransparentColor = Color.Magenta;
            toolsStrip_FlipCollision.Name = "toolsStrip_FlipCollision";
            toolsStrip_FlipCollision.Size = new Drawing.Size(30, 22);
            toolsStrip_FlipCollision.Text = "Flip";
            toolsStrip_FlipCollision.Click += btnFlipColl_Click;
            // 
            // toolsStrip_Delete
            // 
            toolsStrip_Delete.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_Delete.Enabled = false;
            toolsStrip_Delete.ImageTransparentColor = Color.Magenta;
            toolsStrip_Delete.Name = "toolsStrip_Delete";
            toolsStrip_Delete.Size = new Drawing.Size(44, 22);
            toolsStrip_Delete.Text = "Delete";
            toolsStrip_Delete.Click += btnDelete_Click;
            // 
            // toolsStrip_AlignX
            // 
            toolsStrip_AlignX.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_AlignX.ImageTransparentColor = Color.Magenta;
            toolsStrip_AlignX.Name = "toolsStrip_AlignX";
            toolsStrip_AlignX.Size = new Drawing.Size(49, 22);
            toolsStrip_AlignX.Text = "Align X";
            toolsStrip_AlignX.Click += btnSameX_Click;
            // 
            // toolsStrip_AlignY
            // 
            toolsStrip_AlignY.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_AlignY.ImageTransparentColor = Color.Magenta;
            toolsStrip_AlignY.Name = "toolsStrip_AlignY";
            toolsStrip_AlignY.Size = new Drawing.Size(49, 19);
            toolsStrip_AlignY.Text = "Align Y";
            toolsStrip_AlignY.Click += btnSameY_Click;
            // 
            // toolsStrip_PerspectiveCam
            // 
            toolsStrip_PerspectiveCam.Checked = true;
            toolsStrip_PerspectiveCam.CheckState = CheckState.Checked;
            toolsStrip_PerspectiveCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_PerspectiveCam.ImageTransparentColor = Color.Magenta;
            toolsStrip_PerspectiveCam.Name = "toolsStrip_PerspectiveCam";
            toolsStrip_PerspectiveCam.Size = new Drawing.Size(71, 19);
            toolsStrip_PerspectiveCam.Text = "Perspective";
            toolsStrip_PerspectiveCam.Click += btnPerspectiveCam_Click;
            // 
            // toolsStrip_OrthographicCam
            // 
            toolsStrip_OrthographicCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_OrthographicCam.ImageTransparentColor = Color.Magenta;
            toolsStrip_OrthographicCam.Name = "toolsStrip_OrthographicCam";
            toolsStrip_OrthographicCam.Size = new Drawing.Size(82, 19);
            toolsStrip_OrthographicCam.Text = "Orthographic";
            toolsStrip_OrthographicCam.Click += btnOrthographicCam_Click;
            // 
            // toolsStrip_ResetCam
            // 
            toolsStrip_ResetCam.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_ResetCam.ImageTransparentColor = Color.Magenta;
            toolsStrip_ResetCam.Name = "toolsStrip_ResetCam";
            toolsStrip_ResetCam.Size = new Drawing.Size(67, 19);
            toolsStrip_ResetCam.Text = "Reset Cam";
            toolsStrip_ResetCam.Click += btnResetCam_Click;
            // 
            // toolsStrip_CameraSeparator
            // 
            toolsStrip_CameraSeparator.Name = "toolsStrip_CameraSeparator";
            toolsStrip_CameraSeparator.Size = new Drawing.Size(6, 25);
            // 
            // toolsStrip_ShowSpawns
            // 
            toolsStrip_ShowSpawns.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_ShowSpawns.ImageTransparentColor = Color.Magenta;
            toolsStrip_ShowSpawns.Name = "toolsStrip_ShowSpawns";
            toolsStrip_ShowSpawns.Size = new Drawing.Size(51, 19);
            toolsStrip_ShowSpawns.Text = "Spawns";
            toolsStrip_ShowSpawns.Click += btnSpawns_Click;
            // 
            // toolsStrip_ShowItems
            // 
            toolsStrip_ShowItems.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_ShowItems.ImageTransparentColor = Color.Magenta;
            toolsStrip_ShowItems.Name = "toolsStrip_ShowItems";
            toolsStrip_ShowItems.Size = new Drawing.Size(40, 19);
            toolsStrip_ShowItems.Text = "Items";
            toolsStrip_ShowItems.Click += btnItems_Click;
            // 
            // toolsStrip_ShowBoundaries
            // 
            toolsStrip_ShowBoundaries.Checked = true;
            toolsStrip_ShowBoundaries.CheckState = CheckState.Checked;
            toolsStrip_ShowBoundaries.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_ShowBoundaries.ImageTransparentColor = Color.Magenta;
            toolsStrip_ShowBoundaries.Name = "toolsStrip_ShowBoundaries";
            toolsStrip_ShowBoundaries.Size = new Drawing.Size(70, 19);
            toolsStrip_ShowBoundaries.Text = "Boundaries";
            toolsStrip_ShowBoundaries.Click += btnBoundaries_Click;
            // 
            // toolsStrip_OverlaysSeparator
            // 
            toolsStrip_OverlaysSeparator.Name = "toolsStrip_OverlaysSeparator";
            toolsStrip_OverlaysSeparator.Size = new Drawing.Size(6, 6);
            // 
            // toolsStrip_ResetSnap
            // 
            toolsStrip_ResetSnap.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_ResetSnap.ImageTransparentColor = Color.Magenta;
            toolsStrip_ResetSnap.Name = "toolsStrip_ResetSnap";
            toolsStrip_ResetSnap.Size = new Drawing.Size(57, 19);
            toolsStrip_ResetSnap.Text = "Un-Snap";
            toolsStrip_ResetSnap.Click += btnResetSnap_Click;

			//
			// toolsStrip_Options
			//
			toolsStrip_Options.Name = "toolsStrip_Options";
			toolsStrip_Options.Text = "Options";
			toolsStrip_Options.DropDownItems.AddRange(new ToolStripItem[]
			{
				toolsStrip_Options_ScalePointsWithCamera_DisplayOnly,
				toolsStrip_Options_ScalePointsWithCamera_SelectOnly,
				toolsStrip_Options_SelectOnlyIfObjectEquals,
				toolsStrip_Options_ShowZeroZeroPoint
			});
			//
			// toolsStrip_Options_ScalePointsWithCamera_DisplayOnly
			//
			toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.Name = "toolsStrip_Options_ScalePointsWithCamera_DisplayOnly";
			toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.Text = "Scale Points With Camera (Display Only)";
			toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.CheckOnClick = true;
			toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.CheckedChanged += ToolsStrip_Options_ScalePointsWithCamera_DisplayOnly_CheckedChanged;
			//
			// toolsStrip_Options_ScalePointsWithCamera_SelectOnly
			//
			toolsStrip_Options_ScalePointsWithCamera_SelectOnly.Name = "toolsStrip_Options_ScalePointsWithCamera_SelectOnly";
			toolsStrip_Options_ScalePointsWithCamera_SelectOnly.Text = "Scale Points With Camera (Selection Only)";
			toolsStrip_Options_ScalePointsWithCamera_SelectOnly.CheckOnClick = true;
			//
			// toolsStrip_Options_SelectOnlyIfObjectEquals
			//
			toolsStrip_Options_SelectOnlyIfObjectEquals.Name = "toolsStrip_Options_ScalePointsWithCamera_SelectOnly";
			toolsStrip_Options_SelectOnlyIfObjectEquals.Text = "Only Select if Collision's Object Equals to Selected Object";
			toolsStrip_Options_SelectOnlyIfObjectEquals.CheckOnClick = true;
			//
			// toolsStrip_Options_ShowZeroZeroPoint
			//
			toolsStrip_Options_ShowZeroZeroPoint.Name = "toolsStrip_Options_ShowZeroZeroPoint";
			toolsStrip_Options_ShowZeroZeroPoint.Text = "Show (0, 0) Rectangle";
			toolsStrip_Options_ShowZeroZeroPoint.CheckOnClick = true;
			toolsStrip_Options_ShowZeroZeroPoint.CheckedChanged += ToolsStrip_Options_ShowZeroZeroPoint_CheckedChanged;

            // 
            // toolsStrip_Help
            // 
            toolsStrip_Help.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolsStrip_Help.Image = (Image)resources.GetObject("toolsStrip_Help.Image");
            toolsStrip_Help.ImageTransparentColor = Color.Magenta;
            toolsStrip_Help.Name = "toolsStrip_Help";
            toolsStrip_Help.Size = new Drawing.Size(36, 19);
            toolsStrip_Help.Text = "Help";
            toolsStrip_Help.Click += btnHelp_Click;
            // 
            // UNUSED_toolsStripPanel_TrackBarRotation
            // 
            UNUSED_toolsStripPanel_TrackBarRotation.Dock = DockStyle.Right;
            UNUSED_toolsStripPanel_TrackBarRotation.Enabled = false;
            UNUSED_toolsStripPanel_TrackBarRotation.Location = new Drawing.Point(351, 0);
            UNUSED_toolsStripPanel_TrackBarRotation.Maximum = 180;
            UNUSED_toolsStripPanel_TrackBarRotation.Minimum = -180;
            UNUSED_toolsStripPanel_TrackBarRotation.Name = "UNUSED_toolsStripPanel_TrackBarRotation";
            UNUSED_toolsStripPanel_TrackBarRotation.Size = new Drawing.Size(130, 25);
            UNUSED_toolsStripPanel_TrackBarRotation.TabIndex = 3;
            UNUSED_toolsStripPanel_TrackBarRotation.TickStyle = TickStyle.None;
            UNUSED_toolsStripPanel_TrackBarRotation.Visible = false;
            UNUSED_toolsStripPanel_TrackBarRotation.Scroll += UNUSED_toolsStripPanel_TrackBarRotation_Scroll;
            // 
            // UNUSED_toolsStripPanel_ResetRotation
            // 
            UNUSED_toolsStripPanel_ResetRotation.Dock = DockStyle.Right;
            UNUSED_toolsStripPanel_ResetRotation.Enabled = false;
            UNUSED_toolsStripPanel_ResetRotation.FlatAppearance.BorderSize = 0;
            UNUSED_toolsStripPanel_ResetRotation.FlatStyle = FlatStyle.Flat;
            UNUSED_toolsStripPanel_ResetRotation.Location = new Drawing.Point(335, 0);
            UNUSED_toolsStripPanel_ResetRotation.Name = "UNUSED_toolsStripPanel_ResetRotation";
            UNUSED_toolsStripPanel_ResetRotation.Size = new Drawing.Size(16, 25);
            UNUSED_toolsStripPanel_ResetRotation.TabIndex = 4;
            UNUSED_toolsStripPanel_ResetRotation.Text = "*";
            UNUSED_toolsStripPanel_ResetRotation.UseVisualStyleBackColor = true;
            UNUSED_toolsStripPanel_ResetRotation.Visible = false;
            UNUSED_toolsStripPanel_ResetRotation.Click += UNUSED_toolsStripPanel_ResetRotation_Click;


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
                collisionOptions_MoveToNewObject,
                collisionOptions_Transform,
                collisionOptions_AlignX,
                collisionOptions_AlignY,
                collisionOptions_Sep2,
                collisionOptions_Split,
                collisionOptions_Merge,
                collisionOptions_Flip,
                collisionOptions_Delete,
				clipboardCopyOptions,
				clipboardPasteOptions
			});
            collisionOptions.Name = "collisionOptions";
            collisionOptions.Size = new Drawing.Size(184, 208);
            collisionOptions.Opening += new CancelEventHandler(collisionOptions_Opening);
            // 
            // collisionOptions_MoveToNewObject
            // 
            collisionOptions_MoveToNewObject.Name = "collisionOptions_MoveToNewObject";
            collisionOptions_MoveToNewObject.Size = new Drawing.Size(183, 22);
            collisionOptions_MoveToNewObject.Text = "Move to New Object";
            // 
            // collisionOptions_Sep1
            // 
            collisionOptions_Sep1.Name = "collisionOptions_Sep1";
            collisionOptions_Sep1.Size = new Drawing.Size(180, 6);
            // 
            // 
            // collisionOptions_Sep2
            // 
            collisionOptions_Sep2.Name = "collisionOptions_Sep2";
            collisionOptions_Sep2.Size = new Drawing.Size(180, 6);
            // 
            // collisionOptions_Split
            // 
            collisionOptions_Split.Name = "collisionOptions_Split";
            collisionOptions_Split.Size = new Drawing.Size(183, 22);
            collisionOptions_Split.Text = "Split";
            collisionOptions_Split.Click += btnSplit_Click;
            // 
            // collisionOptions_Merge
            // 
            collisionOptions_Merge.Name = "collisionOptions_Merge";
            collisionOptions_Merge.Size = new Drawing.Size(183, 22);
            collisionOptions_Merge.Text = "Merge";
            collisionOptions_Merge.Click += btnMerge_Click;
            // 
            // collisionOptions_Flip
            // 
            collisionOptions_Flip.Name = "collisionOptions_Flip";
            collisionOptions_Flip.Size = new Drawing.Size(183, 22);
            collisionOptions_Flip.Text = "Flip";
            collisionOptions_Flip.Click += btnFlipColl_Click;
            // 
            // collisionOptions_Delete
            // 
            collisionOptions_Delete.Name = "collisionOptions_Delete";
            collisionOptions_Delete.Size = new Drawing.Size(183, 22);
            collisionOptions_Delete.Text = "Delete";
            collisionOptions_Delete.Click += btnDelete_Click;
            // 
            // collisionOptions_Transform
            // 
            collisionOptions_Transform.Name = "collisionOptions_Transform";
            collisionOptions_Transform.Size = new Drawing.Size(183, 22);
            collisionOptions_Transform.Text = "Transform";
            // 
            // collisionOptions_AlignX
            // 
            collisionOptions_AlignX.Name = "collisionOptions_AlignX";
            collisionOptions_AlignX.Size = new Drawing.Size(183, 22);
            collisionOptions_AlignX.Text = "Align X";
            collisionOptions_AlignX.Click += btnSameX_Click;
            // 
            // collisionOptions_AlignY
            // 
            collisionOptions_AlignY.Name = "collisionOptions_AlignY";
            collisionOptions_AlignY.Size = new Drawing.Size(183, 22);
            collisionOptions_AlignY.Text = "Align Y";
            collisionOptions_AlignY.Click += btnSameY_Click;
            // 
            // clipboardCut
            // 
            clipboardCut.Name = "clipboardCut";
            clipboardCut.Size = new Drawing.Size(183, 22);
            clipboardCut.Text = "Cut";
            clipboardCut.Click += btnCut_Click;
            // 
            // clipboardCopy
            // 
            clipboardCopy.Name = "clipboardCopy";
            clipboardCopy.Size = new Drawing.Size(183, 22);
            clipboardCopy.Text = "Copy";
            clipboardCopy.Click += btnCopy_Click;
			// 
			// clipboardCopyOptions
			// 
			clipboardCopyOptions.DropDown.Items.AddRange(new ToolStripItem[]
			{
				clipboardCopyOptions_IgnoreOtherObjects
			});
			clipboardCopyOptions.Name = "clipboardCopyOptions";
			clipboardCopyOptions.Size = new Drawing.Size(183, 22);
			clipboardCopyOptions.Text = "Copy Options";
            // 
            // clipboardCopyOptions_IgnoreOtherObjects
            // 
            clipboardCopyOptions_IgnoreOtherObjects.Name = "clipboardCopyOptions_IgnoreOtherObjects";
			clipboardCopyOptions_IgnoreOtherObjects.Size = new Drawing.Size(183, 22);
			clipboardCopyOptions_IgnoreOtherObjects.Text = "Ignore Other Objects When Copying";
			clipboardCopyOptions_IgnoreOtherObjects.CheckOnClick = true;
            // 
            // clipboardPaste
            // 
            clipboardPaste.Name = "clipboardPaste";
            clipboardPaste.Size = new Drawing.Size(183, 22);
            clipboardPaste.Text = "Paste";
            clipboardPaste.Click += btnPaste_Click;
            clipboardPaste.DropDown.Items.AddRange(new ToolStripItem[]
            {
				clipboardPaste_PasteDirectly,
				clipboardPaste_AdvancedPasteOptions
            });
			// 
			// clipboardPaste_PasteDirectly
			// 
			clipboardPaste_PasteDirectly.Name = "clipboardPasteOptions_PasteDirectly";
			clipboardPaste_PasteDirectly.Size = new Drawing.Size(183, 22);
			clipboardPaste_PasteDirectly.Text = "Paste Here";
			clipboardPaste_PasteDirectly.Click += btnPasteDirectly_Click;
			// 
			// clipboardPaste_AdvancedPasteOptions
			// 
			clipboardPaste_AdvancedPasteOptions.Name = "clipboardPaste_AdvancedPasteOptions";
			clipboardPaste_AdvancedPasteOptions.Size = new Drawing.Size(183, 22);
			clipboardPaste_AdvancedPasteOptions.Text = "Advanced Paste";
			clipboardPaste_AdvancedPasteOptions.Click += btnPasteUI_Click;

			// 
			// clipboardPasteOptions
			// 
			clipboardPasteOptions.DropDown.Items.AddRange(new ToolStripItem[]
			{
				clipboardPasteOptions_ActualPointsValuesAreUsed,
				clipboardPasteOptions_PasteRemoveSelected,
				clipboardPasteOptions_PasteOverrideSelected,
			});
            clipboardPasteOptions.Name = "clipboardPasteOptions";
			clipboardPasteOptions.Size = new Drawing.Size(183, 22);
			clipboardPasteOptions.Text = "Paste Options";
			// 
			// clipboardPasteOptions_PasteRemoveSelected
			// 
			clipboardPasteOptions_PasteRemoveSelected.Name = "clipboardPaste_PasteRemoveSelected";
			clipboardPasteOptions_PasteRemoveSelected.Size = new Drawing.Size(183, 22);
			clipboardPasteOptions_PasteRemoveSelected.Text = "Remove Selected Collisions and Place";
			clipboardPasteOptions_PasteRemoveSelected.CheckOnClick = true;
			// 
			// 
			// clipboardPasteOptions_ActualPointsValuesAreUsed
			// 
			clipboardPasteOptions_ActualPointsValuesAreUsed.Name = "clipboardPaste_ActualPointsValuesAreUsed";
			clipboardPasteOptions_ActualPointsValuesAreUsed.Size = new Drawing.Size(183, 22);
			clipboardPasteOptions_ActualPointsValuesAreUsed.Text = "Use Actual Link Values Instead of Raw";
			clipboardPasteOptions_ActualPointsValuesAreUsed.CheckOnClick = true;
			// 
			// clipboardPasteOptions_PasteOverrideSelected
			// 
			clipboardPasteOptions_PasteOverrideSelected.Name = "clipboardPaste_PasteOverrideSelected";
			clipboardPasteOptions_PasteOverrideSelected.Size = new Drawing.Size(183, 22);
			clipboardPasteOptions_PasteOverrideSelected.Text = "Override Selected Collisions";
			clipboardPasteOptions_PasteOverrideSelected.CheckOnClick = true;
            // 
            // clipboardDelete
            // 
            clipboardDelete.Name = "clipboardDelete";
            clipboardDelete.Size = new Drawing.Size(183, 22);
            clipboardDelete.Text = "Delete Selected";
            clipboardDelete.Click += btnDelete_Click;

			colorDialog.OnColorChanged += ColorDialog_OnColorChanged;

            // 
            // CollisionEditor
            // 
            BackColor = Color.Lavender;
            Controls.Add(mainSplitter);
            Name = "CollisionEditor";
            Size = new Drawing.Size(694, 467);
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
            selectedPlanePropsPanel.ResumeLayout(false);
            selectedPlanePropsPanel_UnknownFlagsGroup.ResumeLayout(false);
            selectedPlanePropsPanel_AttributeFlagsGroup.ResumeLayout(false);
            selectedPlanePropsPanel_TargetGroup.ResumeLayout(false);
            selectedPointPropsPanel.ResumeLayout(false);
            selectedPointPropsPanel.PerformLayout();
            selectedObjPropsPanel.ResumeLayout(false);
            selectedObjPropsPanel.PerformLayout();
            animationPanel.ResumeLayout(false);
            toolsStripPanel.ResumeLayout(false);
            toolsStripPanel.PerformLayout();
            toolsStrip.ResumeLayout(false);
            toolsStrip.PerformLayout();
            ((ISupportInitialize)UNUSED_toolsStripPanel_TrackBarRotation).EndInit();
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

		// We copy the selected variables from _selected... so that we have it in memory
		// Next TODO List: Introduce a UI that stores copies and pastes.
		//public byte _copyState = 0; // Is this even a good practice? 0 = none, 1 = Copying, 2 = Cutting
		//public CollisionLink_S[] _copiedLinks = null;
		//public CollisionPlane_S[] _copiedPlanes = null;
		public int CurrentCopySaveState = 0;
		public int MaximumCopySaveState = 10;
		public List<CopiedLinkPlaneState> _copiedStates = new List<CopiedLinkPlaneState>();

		public CollisionEditor_PasteOptions editorPO = null;

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

        //public CollisionEditor(CollisionForm parent)
        public CollisionEditor()
        {
            InitializeComponent();

            _modelPanel.AddViewport(ModelPanelViewport.DefaultPerspective);

            _modelPanel.CurrentViewport.DefaultTranslate = new Vector3(0.0f, 10.0f, 250.0f);
            _modelPanel.CurrentViewport.AllowSelection = false;
            _modelPanel.CurrentViewport.BackgroundColor = Color.Black;

            selectedObjPropsPanel.Dock = DockStyle.Fill;
            selectedPlanePropsPanel.Dock = DockStyle.Fill;
            selectedPointPropsPanel.Dock = DockStyle.Fill;

            _updating = true;
            selectedPlanePropsPanel_Material.DataSource = getMaterials(); // Take unexpanded collisions
            selectedPlanePropsPanel_Type.DataSource = getCollisionPlaneTypes();
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
            UNUSED_toolsStripPanel_TrackBarRotation.Value = 0;
            _snapMatrix = Matrix.Identity;
            _selectedObject = null;

            _modelPanel.ClearAll();

            _targetNode = node;

            PopulateModelList();
            PopulateObjectList();

            if (lstCollObjects.Items.Count > 0)
            {
                lstCollObjects.SelectedIndex = 0;
                _selectedObject = lstCollObjects.Items[0] as CollisionObject;
                //SnapObject();
            }

            ObjectSelected();

            _modelPanel.ResetCamera();
        }

        protected virtual void SelectionModified()
        {
            CheckPlanes(ref _selectedLinks, ref _selectedPlanes);

            selectedPlanePropsPanel.Visible = false;
            selectedObjPropsPanel.Visible = false;
            selectedPointPropsPanel.Visible = false;
            selectedMenuPanel.Height = 0;

            //Selected Planes are used for actual planes and overrides the collision data (such as Ground Type, etc.)
            if (_selectedPlanes.Count > 0)
            {
                selectedPlanePropsPanel.Visible = true;
                selectedMenuPanel.Height = 205;
            }
            //Selected links are used for getting an specific point in that collision
            else if (_selectedLinks.Count == 1)
            {
                selectedPointPropsPanel.Visible = true;
				selectedMenuPanel.Height = 70;

				//TEMP TEST
				if (tslp == null)
				{
					tslp = new TEST_selectedLinkPlacement();
					tslp.Size = lastFormRects.Size;
					tslp.Location = lastFormRects.Location;

					tslp.Visible = true;
				}

				CollisionLink l = _selectedLinks[0];
				tslp.UpdateSelection(ref l);
			}
			else if (_selectedLinks.Count == 0)
            {
				if (this.tslp != null)
				{
					lastFormRects = this.tslp.DisplayRectangle;
					this.tslp.Visible = false;
					this.tslp.Dispose();
					this.tslp = null;
				}
			}

            UpdatePropPanels();
        }

		//TEST STUFF
		private Rectangle lastFormRects;
		private TEST_selectedLinkPlacement tslp = null;

		protected virtual void UpdatePropPanels()
        {
            _updating = true;

			if (selectedPlanePropsPanel.Visible)
			{
				if (_selectedPlanes.Count <= 0)
				{
					selectedPlanePropsPanel.Visible = false;
					return;
				}

				CollisionPlane p = _selectedPlanes[0];

				if (p._material >= 32 && selectedPlanePropsPanel_Material.Items.Count <= 32)
				{
					selectedPlanePropsPanel_Material.DataSource =
						CollisionTerrain.Terrains.ToList(); // Get the expanded collisions if they're used
				}
				else if (selectedPlanePropsPanel_Material.Items.Count > 32)
				{
					selectedPlanePropsPanel_Material.DataSource =
						CollisionTerrain.Terrains.Take(0x20).ToList(); // Take unexpanded collisions
				}

				selectedPlanePropsPanel_Material.SelectedItem = selectedPlanePropsPanel_Material.Items[p._material];
				//Type
				selectedPlanePropsPanel_Type.SelectedItem = p.Type;
				//Flags
				AttributeFlagsGroup_PlnCheckFallThrough.Checked = p.IsFallThrough;
				AttributeFlagsGroup_PlnCheckLeftLedge.Checked = p.IsLeftLedge;
				AttributeFlagsGroup_PlnCheckRightLedge.Checked = p.IsRightLedge;
				AttributeFlagsGroup_PlnCheckNoWalljump.Checked = p.IsNoWalljump;
				TargetGroup_CheckEverything.Checked = p.IsCharacters;
				TargetGroup_CheckItems.Checked = p.IsItems;
				TargetGroup_CheckPKMNTrainer.Checked = p.IsPokemonTrainer;
				AttributeFlagsGroup_PlnCheckRotating.Checked = p.IsRotating;
				//UnknownFlags
				UnknownFlagsGroup_Check1.Checked = p.IsUnknownSSE;
				UnknownFlagsGroup_Check2.Checked = p.IsUnknownFlag1;
				UnknownFlagsGroup_Check3.Checked = p.IsUnknownFlag3;
				UnknownFlagsGroup_Check4.Checked = p.IsUnknownFlag4;
			}
			else if (selectedPointPropsPanel.Visible)
			{
				if (_selectedLinks.Count <= 0)
				{
					selectedPointPropsPanel.Visible = false;
					return;
				}

				CollisionLink l = _selectedLinks[0];

				selectedPointPropsPanel_XValue.Value = l.Value._x;
				selectedPointPropsPanel_YValue.Value = l.Value._y;
			}
            else if (selectedObjPropsPanel.Visible)
            {
                if (_selectedObject == null)
                {
                    selectedObjPropsPanel.Visible = false;
                    return;
                }

                selectedObjPropsPanel_TextModel.Text = _selectedObject._modelName;
                selectedObjPropsPanel_TextBone.Text = _selectedObject._boneName;
                selectedObjPropsPanel_CheckUnknown.Checked = _selectedObject.UnknownFlag;
                selectedObjPropsPanel_CheckModuleControlled.Checked = _selectedObject.ModuleControlled;
                selectedObjPropsPanel_CheckSSEUnknown.Checked = _selectedObject.UnknownSSEFlag;
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
            lstCollObjects.BeginUpdate();
            lstCollObjects.Items.Clear();

            if (_targetNode != null)
            {
                foreach (CollisionObject obj in _targetNode.Children)
                {
                    obj._render = true;
                    lstCollObjects.Items.Add(obj, true);

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

            lstCollObjects.EndUpdate();
        }

        protected void lstCollObjects_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstCollObjects.IndexFromPoint(e.Location);
            lstCollObjects.SelectedIndex = index;
        }

        protected void lstCollObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            _selectedObject = lstCollObjects.SelectedItem as CollisionObject;
            ObjectSelected();
        }

        protected void lstCollObjectsMenu_SnapObject_Click(object sender, EventArgs e)
        {
            SnapObject();
        }

        protected void lstCollObjectsMenu_DeleteObject_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
            {
                return;
            }

            int index = lstCollObjects.SelectedIndex;

            _targetNode.Children.Remove(_selectedObject);
            lstCollObjects.Items.Remove(_selectedObject);
            _selectedObject = null;
            ClearSelection();

            if (lstCollObjects.Items.Count > 0)
            {
                if (lstCollObjects.Items.Count > index)
                {
                    lstCollObjects.SelectedIndex = index;
                }
                else if (index > 0)
                {
                    lstCollObjects.SelectedIndex = index - 1;
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
            lstCollObjects.Items.Add(_selectedObject, true);
            lstCollObjects.SelectedItem = _selectedObject;
            //TargetNode.SignalPropertyChange();
        }

		private void LstCollObjectsMenu_AssignObjectColor_Click(object sender, EventArgs e)
		{
			if (_selectedObject == null)
				return;

			colorDialog.Color = _selectedObject.CollisionObjectColorRepresentation;
			colorDialog.Text = $"Set Object Color for {_selectedObject.ToString()}";

			if (colorDialog.ShowDialog(this) == DialogResult.OK)
			{
				_selectedObject.CollisionObjectColorRepresentation = colorDialog.Color;
				lstCollObjects.Invalidate();
				_modelPanel.Invalidate();
			}
		}

		private void ColorDialog_OnColorChanged(Color selection)
		{
			_selectedObject.CollisionObjectColorRepresentation = selection;
			_modelPanel.Invalidate();
		}

		protected void ObjectSelected()
        {
            selectedPlanePropsPanel.Visible = false;
            selectedPointPropsPanel.Visible = false;
            selectedObjPropsPanel.Visible = false;
            selectedMenuPanel.Height = 0;

            if (_selectedObject != null)
            {
                selectedObjPropsPanel.Visible = true;
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

            for (int i = 0; i < lstCollObjects.Items.Count; i++)
            {
                lstCollObjects.SetItemChecked(i, false);
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
            for (int i = lstCollObjects.Items.Count; i-- > 0;)
            {
                CollisionObject obj = lstCollObjects.Items[i] as CollisionObject;
                if (obj._modelName == _selectedObject._modelName && obj._boneName == _selectedObject._boneName)
                {
                    lstCollObjects.SetItemChecked(i, true);
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void lstCollObjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CollisionObject obj = lstCollObjects.Items[e.Index] as CollisionObject;
            obj._render = e.NewValue == CheckState.Checked;

            ClearSelection();

            if (!_updating)
            {
                _modelPanel.Invalidate();
            }
        }

		private void LstCollObjects_GotFocus(object sender, EventArgs e)
		{
            if (!_updating)
            {
                _modelPanel.Invalidate();
            }
		}

		#endregion

		#region Selection
		protected void UpdateSelection(bool finish)
        {
            foreach (CollisionObject obj in _targetNode.Children)
            {
				if (toolsStrip_Options_SelectOnlyIfObjectEquals.Checked)
				{
					if (obj != _selectedObject)
						continue;
				}

                foreach (CollisionLink link in obj._points)
                {
                    link._highlight = false;

                    if (!obj._render)
						continue;

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

			UpdateToolsStrip();
		}

		protected void ClearSelection()
		{
			foreach (CollisionLink l in _selectedLinks)
			{
				l._highlight = false;
			}

			_selectedLinks.Clear();
			_selectedPlanes.Clear();
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
		#endregion

		public void UpdateToolsStrip()
        {
            if (_selecting || _hovering || _selectedLinks.Count == 0)
            {
                toolsStrip_Delete.Enabled = toolsStrip_FlipCollision.Enabled =
                    toolsStrip_Merge.Enabled = toolsStrip_Split.Enabled = toolsStrip_AlignX.Enabled = toolsStrip_AlignY.Enabled = false;
            }
            else
            {
                toolsStrip_Merge.Enabled = toolsStrip_AlignX.Enabled = toolsStrip_AlignY.Enabled = _selectedLinks.Count > 1;
                toolsStrip_Delete.Enabled = toolsStrip_Split.Enabled = true;
                toolsStrip_FlipCollision.Enabled = _selectedPlanes.Count > 0;
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

        protected void visibilityCheckPanel_ShowAllModels_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in modelTree.Nodes)
            {
                node.Checked = visibilityCheckPanel_ShowAllModels.Checked;
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
            UpdateToolsStrip();
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
                    toolsStrip_Undo.Enabled = false;
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

		#region Mouse Events

        private DateTime RightClickStart;
		private Drawing.Point MouseDownLocationStart;

		protected void _modelPanel_MouseDown(object sender, MouseEventArgs e)
        {
			if (editorPO != null && editorPO.SetPointMode)
			{
				float depth = _modelPanel.GetDepth(e.X, e.Y);
				editorPO.CenterPointSet(_modelPanel.CurrentViewport, e.Location, depth);

				return;
			}

			MouseDownLocationStart = Cursor.Position;

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
						// Check if object gets to render and check if the option is set plus the object is the same.
						if (!obj._render || toolsStrip_Options_SelectOnlyIfObjectEquals.Checked && (obj != _selectedObject))
							continue;

						foreach (CollisionLink p in obj._points)
						{
							float SelectRadius = PointSelectRadius;

							if (toolsStrip_Options_ScalePointsWithCamera_SelectOnly.Checked)
							{
								SelectRadius = p.GetCamScaledDistance(_modelPanel.CurrentViewport.Camera, PointSelectRadius);
							}

							if (p.Value.Contained(point, point, SelectRadius))
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
				//if (_modelPanel.CurrentViewport._grabbing)
				//	_RCstart = new DateTime(0);
				//else if (_modelPanel.CurrentViewport._grabbing)
					RightClickStart = DateTime.UtcNow;
            }
        }

        protected void _modelPanel_MouseUp(object sender, MouseEventArgs e)
        {
			var MouseUpLocation = new Point(e.X, e.Y);

            if (e.Button == MouseButtons.Left)
            {
				int lastIndex = saveIndex - 1;

				if (lastIndex > 0 && lastIndex < undoSaves.Count)
                {
					CollisionState State = undoSaves[lastIndex];

					if (State != null && State._collisionLinks.Count > 0 && State._linkVectors.Count > 0)
					{
						// If it equals to the starting point, remove.
						if (State._collisionLinks[0].Value == State._linkVectors[0]) 
						{
							undoSaves.RemoveAt(lastIndex);
							saveIndex--;

							if (saveIndex == 0)
							{
								toolsStrip_Undo.Enabled = false;
							}
						}
					}
                }

                hasMoved = false;
                FinishSelection();
                FinishHover();
                UpdateToolsStrip();
            }
            else if (e.Button == MouseButtons.Right)
            {
                DateTime RightClickEnd = DateTime.UtcNow;

				if (!hasMoved)
				{
					//Removed so that we show the menu regardless of whether or not the user has selected any collisions
					if ((RightClickEnd - RightClickStart) <= TimeSpan.FromMilliseconds(150))
					{
						if ((_selectedLinks.Count > 0 || _selectedPlanes.Count > 0 || (_copiedStates != null && _copiedStates.Count > 0)) 
						//(_copiedLinks != null && _copiedLinks.Length > 0) || (_copiedPlanes != null && _copiedPlanes.Length > 0)) 
						&& (MouseDownLocationStart == Cursor.Position))
						{
							_modelPanel.CurrentViewport._grabbing = false;
							collisionOptions.Show(Cursor.Position);
						}
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
#endregion

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

		public void CollisionFormShown(CollisionNode _node)
		{
			TargetNode = _node;
			_modelPanel.Capture();
		}
		public void CollisionFormClosing()
		{
			TargetNode = null;
			_modelPanel.Release();

			// Any forms shown here will be closed and then nullified to prevent them from being used outside
			// of the collision editor.
			if (editorPO != null)
			{
				NullifyAdvancedPasteOptions();
			}
		}

		// 0 = No form is focused at the moment, 1 = Only CE is focused, 2 = Another form is focused
		//public int FormFocusedState = 0;
		// The editor has been focused (specifically the form being focused)
		//public void EditorFocused()
		//{
		//	if (editorPO != null && !editorPO.TopMost)
		//	{
		//		//editorPO.TopMost = true;
		//		//editorPO.Visible = true;

		//		ParentForm.Focus();
		//	}

		//	FormFocusedState = 1;
		//}
		//// The editor has been unfocused (specifically the form not being focused)
		//public void EditorUnfocused()
		//{
		//	if (FormFocusedState == 2)
		//		return;

		//	FormFocusedState = 0;

		//	if (editorPO != null)
		//	{
		//		if (editorPO.OnFocus)
		//		{
		//			FormFocusedState = 2;
		//			return;
		//		}

		//		//editorPO.TopMost = false;
		//		//editorPO.Visible = false;
		//	}
		//}

        protected void _modelPanel_PreRender(object sender)
        {
        }

        protected unsafe void _modelPanel_PostRender(object sender)
        {
            //Clear depth buffer so we can hit-detect
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            // Render objects
            if (_targetNode != null)
            {
				// Why not just create a variable that can be passed as ref since 
				// properties do not allow references...
				GLCamera cam = _modelPanel.CurrentViewport.Camera;

				_targetNode.Render(new CollisionObject.CollisionObjectRenderInfo(
				toolsStrip_Options_ScalePointsWithCamera_DisplayOnly.Checked, lstCollObjects.Focused, ref cam));
            }

			// Render bones
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
                    else if (toolsStrip_ShowSpawns.Checked && bone._name.StartsWith("Player") && bone._name.Length == 8)
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
                    else if (toolsStrip_ShowSpawns.Checked && bone._name.StartsWith("Rebirth") && bone._name.Length == 9)
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

            // Render item fields if checked
            if (toolsStrip_ShowItems.Checked && ItemBones != null)
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

            // Render boundaries if checked and camera bones 0 and 1 are not empty
            if (toolsStrip_ShowBoundaries.Checked && CamBone0 != null && CamBone1 != null)
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

			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.CullFace);

			// Render selection box
			if (_selecting)
			{
				// Draw lines
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.Color4(0.0f, 0.0f, 1.0f, 0.5f);
				TKContext.DrawBox(_selectStart, _selectEnd);

				// Draw box
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.Color4(1.0f, 1.0f, 0.0f, 0.2f);
				TKContext.DrawBox(_selectStart, _selectEnd);
			}

			#region Paste Render
			// Here we render ONLY if Advanced Paste Options is visible.
			if (editorPO != null && editorPO.Visible)
			{
				editorPO.Render(_modelPanel.CurrentViewport);
			}
			#endregion
			
			// Render 0, 0 location so that the user has an idea where the center location is.
			if (toolsStrip_Options_ShowZeroZeroPoint.Checked)
			{
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.Color4(0.0f, 1.0f, 1.0f, 1.0f);
				TKContext.DrawBox(new Vector3(0 - 1.0f, 0 - 1.0f, -3.0f), new Vector3(0 + 1.0f, 0 + 1.0f, 3.0f));
			}
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

        protected void UNUSED_toolsStripPanel_TrackBarRotation_Scroll(object sender, EventArgs e)
        {
            _modelPanel.Invalidate();
        }

        protected void UNUSED_toolsStripPanel_ResetRotation_Click(object sender, EventArgs e)
        {
            UNUSED_toolsStripPanel_TrackBarRotation.Value = 0;
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
                return;

            toolsStrip_PerspectiveCam.Checked = true;
            toolsStrip_OrthographicCam.Checked = false;
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
                return;

            toolsStrip_PerspectiveCam.Checked = false;
            toolsStrip_OrthographicCam.Checked = true;
            if (_modelPanel.CurrentViewport.ViewType != ViewportProjection.Orthographic)
            {
                _modelPanel.ResetCamera();
                _modelPanel.CurrentViewport.ViewType = ViewportProjection.Orthographic;
            }
        }

        protected void btnSpawns_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            toolsStrip_ShowSpawns.Checked = !toolsStrip_ShowSpawns.Checked;
            _modelPanel.Invalidate();
        }

        protected void btnItems_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            toolsStrip_ShowItems.Checked = !toolsStrip_ShowItems.Checked;
            _modelPanel.Invalidate();
        }

        protected void btnBoundaries_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            toolsStrip_ShowBoundaries.Checked = !toolsStrip_ShowBoundaries.Checked;
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
                    else if (toolsStrip_Undo.Enabled)
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
                    else if (toolsStrip_Redo.Enabled)
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

        protected void selectedPlanePropsPanel_Material_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;
            foreach (CollisionPlane plane in _selectedPlanes)
            {
                plane._material = ((CollisionTerrain)selectedPlanePropsPanel_Material.SelectedItem).ID;
            }

            _updating = false;

            TargetNode.SignalPropertyChange();
        }

        protected void selectedPlanePropsPanel_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;
            foreach (CollisionPlane plane in _selectedPlanes)
            {
                plane.Type = (CollisionPlaneType)selectedPlanePropsPanel_Type.SelectedItem;
                if (!plane.IsRotating)
                {
                    if (!plane.IsFloor)
                    {
                        plane.IsFallThrough = false;
                        AttributeFlagsGroup_PlnCheckFallThrough.Checked = false;
                        plane.IsRightLedge = false;
                        AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                        plane.IsLeftLedge = false;
                        AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                    }

                    if (!plane.IsWall)
                    {
                        plane.IsNoWalljump = false;
                        AttributeFlagsGroup_PlnCheckNoWalljump.Checked = false;
                    }
                }
            }

            _updating = false;

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void TargetGroup_CheckEverything_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                TargetGroup_CheckEverything_CheckedChanged_NoErrorHandling();
                return;
            }


            TargetNode.SignalPropertyChange();
            _updating = true;

            bool selection = TargetGroup_CheckEverything.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsCharacters = selection;
                if (p.IsCharacters)
                {
                    p.IsItems = false;
                    TargetGroup_CheckItems.Checked = false;
                    p.IsPokemonTrainer = false;
                    TargetGroup_CheckPKMNTrainer.Checked = false;
                }
                else
                {
                    p.IsFallThrough = false;
                    AttributeFlagsGroup_PlnCheckFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    AttributeFlagsGroup_PlnCheckNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void TargetGroup_CheckItems_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                TargetGroup_CheckItems_CheckedChanged_NoErrorHandling();
                return;
            }

            TargetNode.SignalPropertyChange();
            _updating = true;
            bool selection = TargetGroup_CheckItems.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsItems = selection;
                if (p.IsItems)
                {
                    p.IsCharacters = false;
                    TargetGroup_CheckEverything.Checked = false;
                    p.IsFallThrough = false;
                    AttributeFlagsGroup_PlnCheckFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    AttributeFlagsGroup_PlnCheckNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void TargetGroup_CheckPKMNTrainer_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                TargetGroup_CheckPKMNTrainer_CheckedChanged_NoErrorHandling();
                return;
            }

            TargetNode.SignalPropertyChange();
            _updating = true;
            bool selection = TargetGroup_CheckPKMNTrainer.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsPokemonTrainer = selection;
                if (p.IsPokemonTrainer)
                {
                    p.IsCharacters = false;
                    TargetGroup_CheckEverything.Checked = false;
                    p.IsFallThrough = false;
                    AttributeFlagsGroup_PlnCheckFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    AttributeFlagsGroup_PlnCheckNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void AttributeFlagsGroup_PlnCheckRotating_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                AttributeFlagsGroup_PlnCheckRotating_CheckedChanged_NoErrorHandling();
                return;
            }

            TargetNode.SignalPropertyChange();
            _updating = true;
            bool selection = AttributeFlagsGroup_PlnCheckRotating.Checked;
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
                AttributeFlagsGroup_PlnCheckRotating.Checked = _selectedPlanes[0].IsRotating;
                if (!_selectedPlanes[0].IsRotating)
                {
                    if (!_selectedPlanes[0].IsFloor)
                    {
                        AttributeFlagsGroup_PlnCheckFallThrough.Checked = false;
                        AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                        AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                    }

                    if (!_selectedPlanes[0].IsWall)
                    {
                        AttributeFlagsGroup_PlnCheckNoWalljump.Checked = false;
                    }
                }
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void AttributeFlagsGroup_PlnCheckFallThrough_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                AttributeFlagsGroup_PlnCheckFallThrough_CheckedChanged_NoErrorHandling();
                return;
            }

            _updating = true;
            TargetNode.SignalPropertyChange();

            bool selection = AttributeFlagsGroup_PlnCheckFallThrough.Checked;
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
                AttributeFlagsGroup_PlnCheckFallThrough.Checked = _selectedPlanes[0].IsFallThrough;
                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating || !_selectedPlanes[0].IsCharacters)
                {
                    AttributeFlagsGroup_PlnCheckFallThrough.Checked = false;
                }
            }

            if (allNonCharacters)
            {
                AttributeFlagsGroup_PlnCheckFallThrough.Checked = false;
            }

            _updating = false;
        }

        protected void AttributeFlagsGroup_PlnCheckLeftLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                AttributeFlagsGroup_PlnCheckLeftLedge_CheckedChanged_NoErrorHandling();
                return;
            }

            _updating = true;
            TargetNode.SignalPropertyChange();
            bool selection = AttributeFlagsGroup_PlnCheckLeftLedge.Checked;

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
                AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
            }
            else if (allNoLedge)
            {
                AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
            }
            else if (anyNoLedgeFloors)
            {
                if (AttributeFlagsGroup_PlnCheckLeftLedge.Checked != selection)
                {
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = selection;
                }
            }
            else if (!anyNoLedgeFloors)
            {
                AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                if (AttributeFlagsGroup_PlnCheckLeftLedge.Checked != selection)
                {
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = selection;
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0 && !anyNoLedgeFloors &&
                !allNonCharacters)
            {
                AttributeFlagsGroup_PlnCheckLeftLedge.Checked = _selectedPlanes[0].IsLeftLedge;
                if (_selectedPlanes[0].IsLeftLedge)
                {
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                }

                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating)
                {
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                }
            }

            _updating = false;

            _modelPanel.Invalidate();
        }

        protected void AttributeFlagsGroup_PlnCheckRightLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                AttributeFlagsGroup_PlnCheckRightLedge_CheckedChanged_NoErrorHandling();
                return;
            }

            _updating = false;
            TargetNode.SignalPropertyChange();

            bool selection = AttributeFlagsGroup_PlnCheckRightLedge.Checked;
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
                AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
            }
            else if (allNoLedge)
            {
                if (AttributeFlagsGroup_PlnCheckRightLedge.Checked == true)
                {
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                }
            }
            else if (anyNoLedgeFloors)
            {
                if (AttributeFlagsGroup_PlnCheckRightLedge.Checked != selection)
                {
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = selection;
                }
            }
            else if (!anyNoLedgeFloors)
            {
                AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                if (AttributeFlagsGroup_PlnCheckRightLedge.Checked != selection)
                {
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = selection;
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0 && !anyNoLedgeFloors &&
                !allNonCharacters)
            {
                AttributeFlagsGroup_PlnCheckRightLedge.Checked = _selectedPlanes[0].IsRightLedge;
                if (_selectedPlanes[0].IsRightLedge)
                {
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                }

                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating)
                {
                    AttributeFlagsGroup_PlnCheckLeftLedge.Checked = false;
                    AttributeFlagsGroup_PlnCheckRightLedge.Checked = false;
                }
            }

            _modelPanel.Invalidate();
            _updating = false;
        }

        protected void AttributeFlagsGroup_PlnCheckNoWalljump_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!_errorChecking)
            {
                AttributeFlagsGroup_PlnCheckNoWalljump_CheckedChanged_NoErrorHandling();
                return;
            }

            _updating = true;
            TargetNode.SignalPropertyChange();

            bool selection = AttributeFlagsGroup_PlnCheckNoWalljump.Checked;
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
                AttributeFlagsGroup_PlnCheckNoWalljump.Checked = _selectedPlanes[0].IsNoWalljump;
                if (!_selectedPlanes[0].IsWall && !_selectedPlanes[0].IsRotating)
                {
                    AttributeFlagsGroup_PlnCheckNoWalljump.Checked = false;
                }
            }

            if (allNonCharacters)
            {
                AttributeFlagsGroup_PlnCheckNoWalljump.Checked = false;
            }

            _updating = false;
        }

		// Do we really need these: (object sender, EventArgs e)
		// If not, then there's no reason to add it.
		protected void TargetGroup_CheckEverything_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsCharacters = TargetGroup_CheckEverything.Checked;
            }

            _modelPanel.Invalidate();
        }

        protected void TargetGroup_CheckItems_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsItems = TargetGroup_CheckItems.Checked;
            }
        }

        protected void TargetGroup_CheckPKMNTrainer_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsPokemonTrainer = TargetGroup_CheckPKMNTrainer.Checked;
            }
        }

        protected void AttributeFlagsGroup_PlnCheckRotating_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRotating = AttributeFlagsGroup_PlnCheckRotating.Checked;
            }
        }

        protected void AttributeFlagsGroup_PlnCheckFallThrough_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsFallThrough = AttributeFlagsGroup_PlnCheckFallThrough.Checked;
            }
        }

        protected void AttributeFlagsGroup_PlnCheckLeftLedge_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsLeftLedge = AttributeFlagsGroup_PlnCheckLeftLedge.Checked;
            }

            _modelPanel.Invalidate();
        }

        protected void AttributeFlagsGroup_PlnCheckRightLedge_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRightLedge = AttributeFlagsGroup_PlnCheckRightLedge.Checked;
            }

            _modelPanel.Invalidate();
        }

        protected void AttributeFlagsGroup_PlnCheckNoWalljump_CheckedChanged_NoErrorHandling()
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsNoWalljump = AttributeFlagsGroup_PlnCheckNoWalljump.Checked;
            }
        }

        protected void UnknownFlagsGroup_Check1_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownSSE = UnknownFlagsGroup_Check1.Checked;
            }
        }

        protected void UnknownFlagsGroup_Check2_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag1 = UnknownFlagsGroup_Check2.Checked;
            }
        }

        protected void UnknownFlagsGroup_Check3_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag3 = UnknownFlagsGroup_Check3.Checked;
            }
        }

        protected void UnknownFlagsGroup_Check4_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            TargetNode.SignalPropertyChange();
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsUnknownFlag4 = UnknownFlagsGroup_Check4.Checked;
            }
        }

        #endregion

        #region Point Properties

        protected void selectedPointPropsPanel_XValue_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (selectedPointPropsPanel_XValue.Text == "" && _errorChecking)
            {
                return;
            }

            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
                {
                    link._rawValue._x = selectedPointPropsPanel_XValue.Value;
                }
                else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(selectedPointPropsPanel_XValue.Value, oldValue._y);
                }
            }

            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void selectedPointPropsPanel_YValue_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (selectedPointPropsPanel_YValue.Text == "" && _errorChecking)
            {
                return;
            }

            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
                {
                    link._rawValue._y = selectedPointPropsPanel_YValue.Value;
                }
                else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(oldValue._x, selectedPointPropsPanel_YValue.Value);
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

        protected void visibilityCheckPanel_ShowPolygons_CheckStateChanged(object sender, EventArgs e)
        {
            _modelPanel.BeginUpdate();
            _modelPanel.RenderPolygons = visibilityCheckPanel_ShowPolygons.CheckState == CheckState.Checked;
            _modelPanel.RenderWireframe = visibilityCheckPanel_ShowPolygons.CheckState == CheckState.Indeterminate;
            _modelPanel.EndUpdate();
        }

        protected void visibilityCheckPanel_ShowBones_CheckedChanged(object sender, EventArgs e)
        {
            _modelPanel.RenderBones = visibilityCheckPanel_ShowBones.Checked;
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

        protected void selectedObjPropsPanel_Relink_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (_selectedObject == null || node == null || !(node.Tag is MDL0BoneNode))
            {
                return;
            }

            selectedObjPropsPanel_TextBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = (MDL0BoneNode)node.Tag;
            selectedObjPropsPanel_TextModel.Text = _selectedObject._modelName = node.Parent.Text;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void selectedObjPropsPanel_RelinkNoMove_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (_selectedObject == null || node == null || !(node.Tag is MDL0BoneNode))
            {
                return;
            }

            selectedObjPropsPanel_TextBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = (MDL0BoneNode)node.Tag;
            selectedObjPropsPanel_TextModel.Text = _selectedObject._modelName = node.Parent.Text;
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

        protected void selectedObjPropsPanel_Unlink_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
            {
                return;
            }

            selectedObjPropsPanel_TextBone.Text = "";
            selectedObjPropsPanel_TextModel.Text = "";
            _selectedObject.LinkedBone = null;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void lstCollObjectsMenu_UnlinkNoRelaMove_Click(object sender, EventArgs e)
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

            selectedObjPropsPanel_TextBone.Text = "";
            selectedObjPropsPanel_TextModel.Text = "";
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

		// This is the list of collision objects that the collision has. When the menu is opened,
		// it dictates in whether or not all of the object menu items should show up if the selected
		// object is not null/nothing.
        protected void lstCollObjectsMenu_Opening(object sender, CancelEventArgs e)
        {
			bool selectedObjectNotNull = (_selectedObject != null);
			for (int i = lstCollObjectsMenu.Items.Count - 1; i >= 0; --i)
			{
				lstCollObjectsMenu.Items[i].Visible = selectedObjectNotNull;
			}

			if (selectedObjectNotNull)
            {
				lstCollObjectsMenu_HowManyLinks.Text = $"Points/Links: {_selectedObject._points.Count}";
				lstCollObjectsMenu_HowManyPlanes.Text = $"Planes: {_selectedObject._planes.Count}";
            }
            else
            {
				lstCollObjectsMenu.Items[0].Visible = true;
            }
        }

        protected void collisionOptions_Opening(object sender, CancelEventArgs e)
        {
            //bool ThereAreCopiedStuff = (_copiedLinks.Count > 0) || (_copiedPlanes.Count > 0) || (_copiedPlanes2.Count > 0) || (_copiedLinks3.Count > 0);
            //bool ThereAreCopiedStuff = (_copiedLinks != null && _copiedLinks.Length > 0) || (_copiedPlanes != null && _copiedPlanes.Length > 0);
            bool ThereAreCopiedStuff = (_copiedStates != null) && (_copiedStates.Count > 0);

			//This shows the extensive menus if a link is selected. Planes also dictate the visibility.
			if (_selectedLinks != null && _selectedLinks.Count > 0)
            {
                // Show every single collision options items so that we don't have to deal
                // with later code.
                ToggleCollisionOptionsItemVisibility(true);

                // The usual "Control me" algorithm
                collisionOptions_Merge.Visible = collisionOptions_AlignX.Visible =
                    collisionOptions_AlignY.Visible = _selectedLinks != null && _selectedLinks.Count > 1;
                collisionOptions_MoveToNewObject.Visible =
                    collisionOptions_Flip.Visible = _selectedPlanes != null && _selectedPlanes.Count > 0;
                collisionOptions_MoveToNewObject.Visible = false;
            }
            else
            {
                // Hide every single one of them
                ToggleCollisionOptionsItemVisibility(false);

				// ...Except if they are copy/paste options.
				clipboardCopyOptions.Visible = true;
				clipboardPasteOptions.Visible = true;

                // Show paste and its options only if there are copied items on the clipboard.
                clipboardPaste.Visible = ThereAreCopiedStuff;
				clipboardPaste_PasteDirectly.Visible = ThereAreCopiedStuff;
				clipboardPaste_AdvancedPasteOptions.Visible = ThereAreCopiedStuff;
            }

            //Trace.WriteLine("copiedStuff: " + ThereAreCopiedStuff);
            clipboardPaste.Enabled = ThereAreCopiedStuff;
			clipboardPaste_PasteDirectly.Enabled = ThereAreCopiedStuff;
			clipboardPaste_AdvancedPasteOptions.Enabled = ThereAreCopiedStuff;
        }

        // Used so that every item in collisionOptions are visible or not
        // Please change this algorithm if you think there's a better way
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
            DeleteSelected(true);

            //_copyState = 2;
        }
        //Here we check the amount of selected items and copy it
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            CopySelected();
            //_copyState = 1;
        }
        // Here we decide what to paste based on the toggle option
        // Is this even reasonable to make users make checks in terms
        // of having to switch paste kinds or is this okay?
        protected void btnPaste_Click(object sender, EventArgs e)
        {
            //if (clipboardPaste_PasteDirectly.Checked)
            //{
            //    btnPasteDirectly_Click(sender, e);
            //}
            //else if (clipboardPaste_PasteUI.Checked)
            //{
            //    btnPasteUI_Click(sender, e);
            //}
        }
        //Here we paste the collisions directly into the collision system
        protected void btnPasteDirectly_Click(object sender, EventArgs e)
        {
			clipboardPaste_PasteDirectly.Checked = true;
			clipboardPaste_AdvancedPasteOptions.Checked = false;

            PasteCopiedCollisions(false);
        }

        // Here we paste the collisions but in a UI that will take special care of it
        protected void btnPasteUI_Click(object sender, EventArgs e)
        {
			clipboardPaste_PasteDirectly.Checked = false;
			clipboardPaste_AdvancedPasteOptions.Checked = true;

            ShowAdvancedPasteOptions();
        }


		public void ShowAdvancedPasteOptions()
        {
            // Show the dialog and check if there is anything copied.
            if (editorPO == null)
            {
				if (_copiedStates[0] == null)
				{
					MessageBox.Show("Paste Options cannot be used if there is nothing copied.");
				}
				else
				{
					editorPO = new CollisionEditor_PasteOptions(this, _copiedStates[0]);
					editorPO.TopMost = true;
					editorPO.Show();
				}
            }
            // Else then we have to either make a new dialog that supports multi-pasting ui options
            // (to be honest, it's actually that good) or stick to letting the user know that a dialog
            // is already open. (boo!)
            else
            {
                if (editorPO.Visible)
                    MessageBox.Show("Advanced Paste Options is already open.");
                else
                {
					editorPO.TopMost = false;
                    editorPO = null;
                    ShowAdvancedPasteOptions();
                }
            }
        }
		public void NullifyAdvancedPasteOptions()
		{
			if (editorPO == null)
				return;

			editorPO.Dispose();
			editorPO = null;
		}
        #endregion

        protected void CopySelected()
        {
            if (_selectedLinks.Count == 0)
            {
                MessageBox.Show("There is nothing to copy.");
                return;
            }

			_copiedStates.Clear();

			CopiedLinkPlaneState state = new CopiedLinkPlaneState();
			state.CreateCopyLinksAndPlanes(ref _selectedLinks, ref _selectedPlanes, 
			clipboardCopyOptions_IgnoreOtherObjects.Checked, ref _selectedObject);
			_copiedStates.Add(state);

			this.SelectionModified();
        }

		// Returns true if the collision was successfully copied. False if there was at least an issue.
        public bool PasteCopiedCollisions(bool fromPasteOptions)
        {
			if (_copiedStates[0] == null || _copiedStates[0].CopiedLinks.Length == 0)
			{
				MessageBox.Show("You do not have anything copied.");
				return false;
			}
			
			if (_selectedObject == null)
			{
				MessageBox.Show("You cannot paste collisions without first selecting an object.");
				return false;
			}

			if (fromPasteOptions && (editorPO == null || editorPO.copiedState == null))
			{
				return false;
			}

			//CreateUndo();

            if (clipboardPasteOptions_PasteOverrideSelected.Checked)
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

                return true;
            }
            else if (clipboardPasteOptions_PasteRemoveSelected.Checked)
            {
                DeleteSelected();
            }
            else
            {
                ClearSelection();
            }

            //if (_copiedLinks.Length < 1)
                //return;

            _selectedLinks.Clear();

			if (fromPasteOptions)
			{
				CopiedLinkPlaneState ClonedValues = new CopiedLinkPlaneState();

				// Retrieve our points/links so that we can overwrite ClonedValues for 
				// what this has.

				CollisionLink_S[] NewLinks = new CollisionLink_S[editorPO.copiedState.CopiedLinks.Length];

				// Assuming that the amount of links are equal, loop-check and
				// overwrite values.
				for (int i = NewLinks.Length - 1; i >= 0; --i)
				{
					// Perform an overwrite to our ClonedValues.CopiedLinks from our tinkering object.
					CollisionLink_S link = editorPO.copiedState.CopiedLinks[i];

					link.Value = editorPO.CopiedLinkPointsNewLocation[i];
					link.RawValue = editorPO.CopiedLinkPointsNewLocation[i];

					NewLinks[i] = link;
				}

				ClonedValues.CopiedLinks = NewLinks;

				//TODO: For those sections that have the checkbox checked in changing the plane's properties,
				//apply that to here or create a new class.
				CollisionPlane_S[] NewPlanes = new CollisionPlane_S[editorPO.copiedState.CopiedPlanes.Length];

				CollisionPlane_S AllCollisionPropertiesValues = editorPO.GetACPV();
				bool[] SOP = (bool[])AllCollisionPropertiesValues.ExtraData;

				// These are stored in memory so that way code reduction is a thing.
				CollisionPlaneFlags2[] targets = new CollisionPlaneFlags2[] 
				{ 
					CollisionPlaneFlags2.Characters, 
					CollisionPlaneFlags2.Items, 
					CollisionPlaneFlags2.PokemonTrainer 
				};
				CollisionPlaneFlags[] flags = new CollisionPlaneFlags[] 
				{ 
					CollisionPlaneFlags.DropThrough, 
					CollisionPlaneFlags.LeftLedge, 
					CollisionPlaneFlags.RightLedge, 
					CollisionPlaneFlags.NoWalljump, 
					CollisionPlaneFlags.Rotating 
				};
				CollisionPlaneFlags[] unknownflags = new CollisionPlaneFlags[] 
				{ 
					CollisionPlaneFlags.Unknown1, 
					CollisionPlaneFlags.Unknown3, 
					CollisionPlaneFlags.Unknown4 
				};
					
				for (int i = NewPlanes.Length - 1; i >= 0; --i)
				{
					CollisionPlane_S plane = editorPO.copiedState.CopiedPlanes[i];
					
					// Type overrides, if true then AllCollisionPropertiesValues will override.
					if (SOP[0])
					{
						plane.Type = AllCollisionPropertiesValues.Type;
					}
					// Material overrides, if true then AllCollisionPropertiesValues will override.
					if (SOP[1])
					{
						plane.Material = AllCollisionPropertiesValues.Material;
					}

					if (SOP[2] || SOP[3] || SOP[4])
					{
						// Targets (Target to) overrides, if true then AllCollisionPropertiesValues will override.
						// This includes characters, items, and Pokémon Trainer.
						if (SOP[2])
						{
							for (int f = 0; f < targets.Length; ++f)
							{
								CollisionPlaneFlags2 cpf = targets[f];

								plane.SetFlag2(cpf, AllCollisionPropertiesValues.GetFlag2(cpf));
							}
						}
						// Flag Properties overrides, if true then AllCollisionPropertiesValues will override.
						// This includes Drop Through, Left Ledge, Right Ledge, No Walljump, and Rotate.
						if (SOP[3])
						{
							for (int f = 0; f < flags.Length; ++f)
							{
								CollisionPlaneFlags cpf = flags[f];

								plane.SetFlag(cpf, AllCollisionPropertiesValues.GetFlag(cpf));
							}
						}
						// Unknown Flag Properties overrides, if true then AllCollisionPropertiesValues will override.
						// Includes Unknown SSE, Unknown 1, Unknown 3, and Unknown 4.
						if (SOP[4])
						{
							// Since Unknown SSE is in CollisionPlaneFlags2, then the best way is just to
							// call the enumerator directly. 
							plane.SetFlag2(CollisionPlaneFlags2.UnknownSSE, AllCollisionPropertiesValues.GetFlag2(CollisionPlaneFlags2.UnknownSSE));

							for (int f = 0; f < unknownflags.Length; ++f)
							{
								CollisionPlaneFlags cpf = unknownflags[f];

								plane.SetFlag(cpf, AllCollisionPropertiesValues.GetFlag(cpf));
							}
						}
					}

					NewPlanes[i] = plane;
				}

				ClonedValues.CopiedPlanes = NewPlanes;

				// Then create our links and planes (aka perform a new paste).
				ClonedValues.CreateLinksAndPlanes(_selectedObject, true, clipboardPasteOptions_ActualPointsValuesAreUsed.Checked);

				_selectedLinks = ClonedValues.CreatedLinks;
				_selectedPlanes = ClonedValues.CreatedPlanes;

				ClonedValues.ClearLinksAndPlanes();
			}
			else
			{
				// By the way, CreateLinksAndPlanes is the same as pasting the collision.
				_copiedStates[0].CreateLinksAndPlanes(_selectedObject, true, clipboardPasteOptions_ActualPointsValuesAreUsed.Checked);
				
				_selectedLinks = _copiedStates[0].CreatedLinks;
				_selectedPlanes = _copiedStates[0].CreatedPlanes;

				_copiedStates[0].ClearLinksAndPlanes();
			}

			SelectionModified();
			_modelPanel.Invalidate();

			return true;
		}
		#endregion

		// Delete selected items
		// callClearSel means that we will clear the selection
		protected void DeleteSelected(bool fromCut = false)
        {
			if (_selectedPlanes.Count > 0)
			{
				for (int i = 0; i < _selectedPlanes.Count; i++)
				{
					CollisionPlane plane = _selectedPlanes[i];

					if (fromCut && this.clipboardCopyOptions_IgnoreOtherObjects.Checked)
					{
						//if (plane._parent == _selectedObject)
						if (ReferenceEquals(plane._parent, _selectedObject))
							plane.Delete();
					}
					else
						plane.Delete();
				}

				TargetNode.SignalPropertyChange();
			}
			else if (_selectedLinks.Count > 0)
			{
				for (int i = 0; i < _selectedLinks.Count; i++)
				{
					CollisionLink link = _selectedLinks[i];

					if (fromCut)
					{
						if (ReferenceEquals(link._parent, _selectedObject))
							link.Pop();
					}
					else
						link.Pop();
				}

				TargetNode.SignalPropertyChange();
			}

            ClearSelection();

            SelectionModified();
            _modelPanel.Invalidate();
        }

        protected void modelTreeMenu_Snap_Click(object sender, EventArgs e)
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

		private void ToolsStrip_Options_ScalePointsWithCamera_DisplayOnly_CheckedChanged(object sender, EventArgs e)
		{
			_modelPanel.Invalidate();
		}
		private void ToolsStrip_Options_ShowZeroZeroPoint_CheckedChanged(object sender, EventArgs e)
		{
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
            toolsStrip_Undo.Enabled = toolsStrip_Redo.Enabled = false;
        }

		// TODO: Why not make a UI that shows you what were undone? 
		// That way it is quicker and saves some time.
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
            toolsStrip_Undo.Enabled = true;
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
                toolsStrip_Undo.Enabled = false;
            }

            toolsStrip_Redo.Enabled = true;

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
                toolsStrip_Redo.Enabled = false;
            }

            toolsStrip_Undo.Enabled = true;

            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        protected void selectedObjPropsPanel_CheckUnknown_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject.UnknownFlag = selectedObjPropsPanel_CheckUnknown.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void chkObjIndep_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void selectedObjPropsPanel_CheckModuleControlled_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject.ModuleControlled = selectedObjPropsPanel_CheckModuleControlled.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void selectedObjPropsPanel_CheckSSEUnknown_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating)
            {
                return;
            }

            _selectedObject.UnknownSSEFlag = selectedObjPropsPanel_CheckSSEUnknown.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void animationPanel_PlayAnimations_Click(object sender, EventArgs e)
        {
        }

        protected void animationPanel_PrevFrame_Click(object sender, EventArgs e)
        {
        }

        protected void animationPanel_NextFrame_Click(object sender, EventArgs e)
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

        public Vector2 Value;
        public Vector2 RawValue;

		public SortedList<int, PlaneLinkRelationship> CollisionPlaneMembers;

		public bool LinkAlreadyCreated;

		public CollisionLink Reference;

		// A way to know which index this object is. For example, because there are a bunch of 
		// links that might be equal in terms of RawValue, Members, and its parent, this index
		// serves to make sure that only a link serves as a parent
		public int LinkIndex;

        public CollisionLink_S(CollisionLink orig)
        {
            Parent = orig._parent;
            Value = orig.Value;
            RawValue = orig._rawValue;
            EncodeIndex = orig._encodeIndex;
			Reference = null;

            LinkIndex = -1;
			CollisionPlaneMembers = null;
			LinkAlreadyCreated = false;
        }

		public void ApplyToOriginalLink(ref CollisionLink original, bool ApplyParent = false)
		{
			original._encodeIndex = EncodeIndex;
			//original.Value = RawValue;
			
			if (ApplyParent)
				original._parent = Parent;
		}

		public void RemoveLinkReference()
		{
			Reference = null;
		}
    }

    [Serializable]
    public struct CollisionPlane_S
    {
		public bool Created;

        public CollisionObject Parent;
        public int EncodeIndex;

		public int LinkLeft;
		public int LinkRight;

        public CollisionPlaneFlags Flags;
        public CollisionPlaneFlags2 Flags2;
        public CollisionPlaneType Type;
		public byte Material;

		public int ID;
		public CollisionPlane Reference;

		public object ExtraData;

		public CollisionPlane_S(CollisionPlane orig, int identifier)
        {
			Created = true;

            Parent = orig._parent;
            EncodeIndex = orig._encodeIndex;

            Flags = orig._flags;
            Flags2 = orig._flags2;
            Type = orig._type;
			Material = orig._material;

			ID = identifier;
			Reference = null;
			ExtraData = null;

			LinkLeft = -1;
			LinkRight = -1;
		}

		public void ApplyToOriginal(ref CollisionPlane plane)
        {
            plane._flags = Flags;
            plane._flags2 = Flags2;
            plane._type = Type;
			plane._material = Material;
        }

		public void SetPlaneReference(ref CollisionPlane source)
		{
			Reference = source;
		}
		public void RemovePlaneReference()
		{
			Reference = null;
		}

		// Copied directly from CollisionNode.cs since CollisionPlane_S is just a class
		// that is meant to be copied and prevent references from the original CollisionPlane. 
		// Feel free to change into boolean values if such values such as IsRotating, 
		// UnknownSSE, etc. will be used in the future. The only class using the following 
		// GetFlag and SetFlag (and 2 variants) is CollisionEditor_PasteOptions.
		public void SetFlag(CollisionPlaneFlags Value, bool IsTrue)
		{
			Flags = (Flags & ~Value) | (IsTrue ? Value : 0);
		}
		public void SetFlag2(CollisionPlaneFlags2 Value, bool IsTrue)
		{
			Flags2 = (Flags2 & ~Value) | (IsTrue ? Value : 0);
		}
		public bool GetFlag(CollisionPlaneFlags Value)
		{
			return (Flags & Value) != 0;
		}
		public bool GetFlag2(CollisionPlaneFlags2 Value)
		{
			return (Flags2 & Value) != 0;
		}
    }

	public struct PlaneLinkRelationship
	{
		public PlaneLinkPositioning Positioning;
	}

	public enum PlaneLinkPositioning
	{
		Left,
		Right,
		None
	}

	// Keeps copied links and planes to a single state.
	// Plus this is the class where it handles both points (links) and planes's copy state and paste
	// state.
	public class CopiedLinkPlaneState
	{
		public CollisionLink_S[] CopiedLinks;
		public CollisionPlane_S[] CopiedPlanes;

		// These should not be used unless if creating it from CreateLinksAndPlanes.
		public CollisionObject CreatedObject;
		public List<CollisionLink> CreatedLinks;
		public List<CollisionPlane> CreatedPlanes;

		public CopiedLinkPlaneState() { }

		public CopiedLinkPlaneState(CollisionLink_S[] CopiedLinks, CollisionPlane_S[] CopiedPlanes)
		{
			this.CopiedLinks = (CollisionLink_S[])CopiedLinks.Clone();
			this.CopiedPlanes = (CollisionPlane_S[])CopiedPlanes.Clone();
		}

		// Clears copied links and planes from CopiedLinkPlaneState.
		public void CreateCopyLinksAndPlanes(ref List<CollisionLink> TargetLinks, ref List<CollisionPlane> TargetPlanes, 
		bool IgnoreOtherObjects, ref CollisionObject SelectedObject)
		{
			if (TargetLinks == null || TargetLinks.Count == 0)
				return;

			CopiedLinks = new CollisionLink_S[TargetLinks.Count];
			CopiedPlanes = new CollisionPlane_S[TargetPlanes.Count];

			// Helps in assigning a plane an ID so that way the copy operation does not go wrong.
			int PlaneCountID = 0;

			// Iterate through every target link available.
			for (int li = TargetLinks.Count - 1; li >= 0; --li)
			{
				CollisionLink link = TargetLinks[li];

				// First check if IgnoreOtherObjects is set.
				if (IgnoreOtherObjects)
				{
					// If true, then check if the parent is not the same. If it is not, then
					// skip this link.
					if (!ReferenceEquals(link._parent, SelectedObject))
						continue;
				}

				// Create an structure variant of CollisionLink.
				CollisionLink_S link_S = new CollisionLink_S(link);

				// Retrieve our plane members.
				List<CollisionPlane> planes = link._members;

				// Iterate through every associated plane available. This helps in knowing if there are any
				// planes that already exists.
				for (int lip = planes.Count - 1; lip >= 0; --lip)
				{
					CollisionPlane plane = planes[lip];

					// Create a structured plane that do not have any links ID yet.
					CollisionPlane_S plane_S = new CollisionPlane_S();

					// This is how it is known if the plane was referenced and copied from PlanesID.
					bool PlaneReferenced = false;

					int PlaneIDToApply = PlaneCountID;

					// If PlaneCountID is more than 0, it means that there are more planes to check.
					if (PlaneCountID > 0)
					{
						int CPLength = CopiedPlanes.Length;

						// Look up for a plane that has an equal reference since really, when
						// creating lists, we don't just deep clone it, we reference them. (Not
						// unless it were to be really deep-cloned in which ReferenceEquals would
						// always return false)
						for (int lips = PlaneCountID - 1; lips >= 0; --lips)
						{
							if (lips >= CPLength)
								break;

							// Retrieve the copied variant of the plane.
							CollisionPlane_S plane_ST = CopiedPlanes[lips];

							// Check if this plane has an equal reference to the plane stored in
							// the list. Helps with connecting links back into a plane.
							if (CollisionPlane.PlaneEquals(plane_ST.Reference, plane))
							{
								plane_S = plane_ST;
								PlaneReferenced = true;
								PlaneIDToApply = plane_ST.ID;

								break;
							}
						}
					}

					bool linkLeftNotNothing = plane._linkLeft != null;
					bool linkRightNotNothing = plane._linkRight != null;
					bool leftLinkEquals = linkLeftNotNothing && ReferenceEquals(link, plane._linkLeft);
					bool rightLinkEquals = linkRightNotNothing && ReferenceEquals(link, plane._linkRight);

					// First check if the links themselves are not equal to the main link being read.
					// It can of course, be an option if the user wants to still select planes that have
					// the other link not being selected.
					if (linkLeftNotNothing && !leftLinkEquals && !plane._linkLeft._highlight)
						continue;
					if (linkRightNotNothing && !rightLinkEquals && !plane._linkRight._highlight)
						continue;

					if (!plane_S.Created && !PlaneReferenced)
						plane_S = new CollisionPlane_S(plane, PlaneCountID);
					else if (plane_S.Created)
					{
						// This would not even make any sense, but why would at least 2 links
						// have their ID created when all are already taken over?
						if (plane_S.LinkLeft != -1 && plane_S.LinkRight != -1)
							continue;
					}

					// Create a collision plane member, in which it creates a relationship.
					if (link_S.CollisionPlaneMembers == null)
						link_S.CollisionPlaneMembers = new SortedList<int, PlaneLinkRelationship>();

					PlaneLinkRelationship relationship = new PlaneLinkRelationship();

					// Do the thing where it checks if the link is left, right, or nonexistent.
					if (leftLinkEquals)
					{
						relationship.Positioning = PlaneLinkPositioning.Left;
						plane_S.LinkLeft = li;
					}
					else if (rightLinkEquals)
					{
						relationship.Positioning = PlaneLinkPositioning.Right;
						plane_S.LinkRight = li;
					}
					else
					{
						relationship.Positioning = PlaneLinkPositioning.None;
					}

					// Add the relationship to a list of plane members to store.
					// As long as there is no key/value pair from PlaneIDToApply.
					if (!link_S.CollisionPlaneMembers.ContainsKey(PlaneIDToApply))
						link_S.CollisionPlaneMembers.Add(PlaneIDToApply, relationship);

					// Overwrite the copied plane's index to the new Plane_S.
					CopiedPlanes[PlaneIDToApply] = plane_S;
					
					// If the plane was not referenced then a plane reference is set and the
					// plane's count ID is increased.
					if (!PlaneReferenced)
					{
						CopiedPlanes[PlaneCountID].SetPlaneReference(ref plane);
						PlaneCountID = PlaneCountID + 1;
					}
				}

				CopiedLinks[li] = link_S;
			}

			// Check if there are available planes. If so, then remove any plane reference.
			if (CopiedPlanes.Length > 0)
			{
				for (int i = CopiedPlanes.Length - 1; i >= 0; --i)
				{
					CopiedPlanes[i].RemovePlaneReference();
				}
			}
		}

		public void CreateLinksAndPlanes(CollisionObject TargetObject, bool HighlightLinks = true, bool TakeActualLinkValue = false)
		{
			int selectedLinksLinkIndex = 0;

			if (CopiedLinks == null || CopiedLinks.Length < 1)
				return;
			if (CopiedPlanes == null || CopiedPlanes.Length < 1)
				return;

			CreatedObject = TargetObject;
			CreatedLinks = new List<CollisionLink>();
			CreatedPlanes = new List<CollisionPlane>();

			// This is where the magic begins for pasting. Let us get our copied links to memory.
			for (int cl = 0; cl < CopiedLinks.Length; cl++)
			{
				CollisionLink_S Link = CopiedLinks[cl];

				// Then check if the plane members are NOT nothing and make sure that there is a plane reference.
				if (Link.CollisionPlaneMembers != null && Link.CollisionPlaneMembers.Count > 0)
				{
					// Then get our plane reference ID.
					int[] planeReferences = Link.CollisionPlaneMembers.Keys.ToArray();

					// Then check our plane members that was just created in the copy function.
					for (int pa = 0; pa < Link.CollisionPlaneMembers.Count; ++pa)
					{
						// This is important -- we get the reference to the list of copied planes to the plane.
						int reference = planeReferences[pa];

						// If the plane's reference index is less than zero, then it is nonexistent. Since indexes only
						// are obtained at 0.
						if (reference == -1)
							continue;

						// Then retrieve our CollisionPlane_S from our reference.
						CollisionPlane_S plane = CopiedPlanes[reference];

						// If one of the links index is -1, then this plane can be officially skipped.
						if ((plane.LinkLeft == -1) || (plane.LinkRight == -1))
							continue;

						// Then get our CollisionLink_S from the plane's LinkLeft/LinkRight index.
						CollisionLink_S LeftLink_S = CopiedLinks[plane.LinkLeft];
						CollisionLink_S RightLink_S = CopiedLinks[plane.LinkRight];

						// Firct check if both links were already created. If it is, then this plane is skipped
						// since it is not necessary. The plane reference is also checked for just in case it exists, fully
						// stating that these points are already created.
						if (LeftLink_S.LinkAlreadyCreated && RightLink_S.LinkAlreadyCreated && (plane.Reference != null))
							continue;

						// The main reason why they are never initialized is due to how references work.
						// If initialized, then these links will never get deleted due to lack of link referencing.
						// Especially if they contain a reference from either LeftLink_S/RightLink_S.
						CollisionLink leftLink = null;
						CollisionLink rightLink = null;

						// Check of the link was created and has a reference. Reference is used to let us take 
						// the Link reference for a new collision plane linking.
						if (LeftLink_S.LinkAlreadyCreated && LeftLink_S.Reference != null)
							leftLink = LeftLink_S.Reference;
						else
						{
							// Maybe consider bringing the original object of this link?
							leftLink = new CollisionLink(TargetObject, TakeActualLinkValue ? LeftLink_S.Value : LeftLink_S.RawValue);

							// Copies properties from CollisionLink_S to CollisionLink.
							LeftLink_S.ApplyToOriginalLink(ref leftLink);
						}

						// Same procedure as previous.
						if (RightLink_S.LinkAlreadyCreated && RightLink_S.Reference != null)
							rightLink = RightLink_S.Reference;
						else
						{
							rightLink = new CollisionLink(TargetObject, TakeActualLinkValue ? RightLink_S.Value : RightLink_S.RawValue);
							RightLink_S.ApplyToOriginalLink(ref rightLink);
						}

						// If one of left/rightLink is nothing, then this plane will have to be skipped.
						// Errors here are not an option.
						if (leftLink == null || rightLink == null)
							continue;

						// Then a plane is created from leftLink and rightLink.
						CollisionPlane branchedPlane = new CollisionPlane(TargetObject, leftLink, rightLink);

						// Apply the original values that this copied plane used to have once it was copied.
						// If curious as to what it copies, please take a look at ApplyToOriginal in CollisionPlane_S.
						plane.ApplyToOriginal(ref branchedPlane);

						// Give the plane the reference to the created plane.
						plane.Reference = branchedPlane;

						// The links are going to be highlited, if set to.
						leftLink._highlight = HighlightLinks;
						rightLink._highlight = HighlightLinks;

						// Add the links to the list of created links. They are not needed if one already
						// exists in the selection.
						if (LeftLink_S.LinkAlreadyCreated)
							CreatedLinks[LeftLink_S.LinkIndex] = leftLink;
						else
						{
							// Create a link reference so that later LeftLink already have a reference.
							LeftLink_S.Reference = leftLink;
							LeftLink_S.LinkIndex = selectedLinksLinkIndex;
							// Add 1 to our value so that they are not marked as duplicate.
							selectedLinksLinkIndex = selectedLinksLinkIndex + 1;

							// A link is then added to the list of CreatedLinks.
							CreatedLinks.Add(leftLink);
						}
						
						// Same procedure as previous.
						if (RightLink_S.LinkAlreadyCreated)
							CreatedLinks[RightLink_S.LinkIndex] = rightLink;
						else
						{
							RightLink_S.Reference = rightLink;
							RightLink_S.LinkIndex = selectedLinksLinkIndex;
							selectedLinksLinkIndex = selectedLinksLinkIndex + 1;

							CreatedLinks.Add(rightLink);
						}

						// LinkAlreadyCreated makes sure that if there is one created it does not
						// create duplicates when attempting to check for link creation.
						LeftLink_S.LinkAlreadyCreated = true;
						RightLink_S.LinkAlreadyCreated = true;

						// Apply the left/right links of CopiedLinks so that they are overwritten.
						// Structures only pass these as values so it is necessary to overwrite the
						// original.
						CopiedLinks[plane.LinkLeft] = LeftLink_S;
						CopiedLinks[plane.LinkRight] = RightLink_S;

						// Then we go ahead and put our plane. Same as before with this issue.
						CopiedPlanes[reference] = plane;
					}
				}
				else
				{

				}
			}

			// Remove any paste reference that it was used during the operation.
			for (int li = CopiedLinks.Length - 1; li >= 0; --li)
			{
				var l = CopiedLinks[li];

				l.RemoveLinkReference();
				l.LinkAlreadyCreated = false;
				l.LinkIndex = -1;

				CopiedLinks[li] = l;
			}
			// Remove any reference since we don't want to make ties with a collision plane.
			for (int pl = CopiedPlanes.Length - 1; pl >= 0; --pl)
			{
				CopiedPlanes[pl].RemovePlaneReference();
			}
		}

		public void ClearLinksAndPlanes()
		{
			CreatedLinks.Clear();
			CreatedPlanes.Clear();

			CreatedObject = null;
			CreatedLinks = null;
			CreatedPlanes = null;
		}

		// Only copies structure related links and planes. In this case, it is CopiedLinks and CopiedPlanes.
		public CopiedLinkPlaneState CloneValues()
		{
			return new CopiedLinkPlaneState(CopiedLinks, CopiedPlanes);
		}
	}

	// A CheckedListBox that is specifically made for lstCollObjects so that it supports custom color.
	public class lstCollObjectsCListBox : CheckedListBox
	{
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			//Trace.WriteLine("Name: "+Items[e.Index].ToString()+" | State Flags: "+e.State.ToString());

			if (!(e.State.HasFlag(DrawItemState.Selected)))
			{
				// Apparently indexes of negative values can happen.
				if (e.Index < 0)
					return;

				e.DrawBackground();

				// Get System.Drawing.Graphics to render the text and the rectangle.
				Graphics g = e.Graphics;

				// Since lstCollObjects has CollisionObject as items, cast Items and
				// get the collision object.
				CollisionObject @object = (CollisionObject)Items[e.Index];

				// Get the collision object's collision name.
				string DisplayName = @object.ToString();

				// b is Bounds, where the list item's bound lies.
				Rectangle b = e.Bounds;

				//Fills the color that the CollisionObject is using. If fully transparent (Alpha is 0), then don't bother filling it up.
				if (@object.CollisionObjectColorRepresentation.A > 0.0f)
				{
					int A = @object.CollisionObjectColorRepresentation.A;
					int R = @object.CollisionObjectColorRepresentation.R;
					int G = @object.CollisionObjectColorRepresentation.G;
					int B = @object.CollisionObjectColorRepresentation.B;

					g.FillRectangle(new SolidBrush(Color.FromArgb(A, R, G, B)), b);
				}

				VisualStyles.CheckBoxState CBState = Utils.TranslateCheckState(GetItemCheckState(e.Index));
				Drawing.Size glyphSize = CheckBoxRenderer.GetGlyphSize(g, CBState);
				
				int checkPad = (b.Height - glyphSize.Height) / 2;

				// Draws a checkbox using (g) graphics and a text.
				CheckBoxRenderer.DrawCheckBox(g, b.Location.Add(checkPad, checkPad),
					new Rectangle(new Drawing.Point(b.X + b.Height, b.Y), new Drawing.Size(b.Width - b.Height, b.Height)),
					DisplayName, e.Font, TextFormatFlags.Left, false, CBState);
			}
			else
			{
				base.OnDrawItem(e);
			}
		}
	}
}
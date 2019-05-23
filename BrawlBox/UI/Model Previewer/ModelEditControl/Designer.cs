using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using BrawlBox;

namespace System.Windows.Forms
{
    public partial class ModelEditControl : ModelEditorBase
    {
        #region Designer
        public ModelPlaybackPanel pnlPlayback;
        public ColorDialog dlgColor;
        public ModelPanel modelPanel;
        public CHR0Editor chr0Editor;
        public SRT0Editor srt0Editor;
        public VIS0Editor vis0Editor;
        public PAT0Editor pat0Editor;
        public SHP0Editor shp0Editor;
        public CLR0Editor clr0Editor;
        public SCN0Editor scn0Editor;

        public ComboBox models;
        private Button btnLeftToggle;
        private Button btnRightToggle;
        private Button btnBottomToggle;
        private Splitter spltLeft;
        private Button btnTopToggle;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem openModelsToolStripMenuItem;
        private ToolStripMenuItem kinectToolStripMenuItem;
        private ToolStripMenuItem notYetImplementedToolStripMenuItem;
        private ToolStripMenuItem newSceneToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem1;
        private ToolStripMenuItem btnUndo;
        private ToolStripMenuItem btnRedo;
        private ToolStripMenuItem viewportToolStripMenuItem;
        private ToolStripMenuItem startTrackingToolStripMenuItem;
        private Label label1;
        private ToolStripMenuItem syncKinectToolStripMenuItem;
        private ToolStripMenuItem targetModelToolStripMenuItem;
        private ToolStripMenuItem hideFromSceneToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem hideAllOtherModelsToolStripMenuItem;
        private ToolStripMenuItem deleteAllOtherModelsToolStripMenuItem;
        private ToolStripMenuItem modelToolStripMenuItem;
        private ToolStripMenuItem toggleBones;
        private ToolStripMenuItem togglePolygons;
        private ToolStripMenuItem toggleVertices;
        private ToolStripMenuItem toggleCollisions;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem toggleFloor;
        private ToolStripMenuItem resetCameraToolStripMenuItem;
        private ToolStripMenuItem editorsToolStripMenuItem;
        private ToolStripMenuItem showLeft;
        private ToolStripMenuItem showBottom;
        private ToolStripMenuItem showTop;
        private Panel controlPanel;
        private Splitter spltRight;
        private Panel panel1;
        private ToolStripMenuItem fileTypesToolStripMenuItem;
        private ToolStripMenuItem playCHR0ToolStripMenuItem;
        private ToolStripMenuItem playSRT0ToolStripMenuItem;
        private ToolStripMenuItem playSHP0ToolStripMenuItem;
        private ToolStripMenuItem playPAT0ToolStripMenuItem;
        private ToolStripMenuItem playVIS0ToolStripMenuItem;
        private ToolStripMenuItem openAnimationsToolStripMenuItem;
        private ToolStripMenuItem openMovesetToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem btnOpenClose;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        public Panel animEditors;
        private ToolStrip toolStrip1;
        private Panel panel2;
        private ToolStripButton chkBones;
        private ToolStripButton chkPolygons;
        private ToolStripButton chkVertices;
        private ToolStripButton chkFloor;
        private ToolStripButton button1;
        private ToolStripSeparator toolStripSeparator1;
        public ToolStripMenuItem chkExternalAnims;
        private Splitter splitter1;
        public Panel animCtrlPnl;
        private ToolStripButton chkCollisions;
        public ToolStripButton btnSaveCam;
        private ToolStripMenuItem showRight;
        public ToolStripMenuItem showCameraCoordinatesToolStripMenuItem;
        private ToolStripMenuItem sCN0ToolStripMenuItem;
        private ToolStripMenuItem editControlToolStripMenuItem;
        private ToolStripMenuItem rotationToolStripMenuItem;
        private ToolStripMenuItem translationToolStripMenuItem;
        private ToolStripMenuItem scaleToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem playCLR0ToolStripMenuItem;
        private WeightEditor weightEditor;
        private ToolStripMenuItem backgroundToolStripMenuItem;
        private ToolStripMenuItem setColorToolStripMenuItem;
        private ToolStripMenuItem loadImageToolStripMenuItem;
        private ToolStripMenuItem takeScreenshotToolStripMenuItem;
        private ToolStripMenuItem btnExportToImgNoTransparency;
        private ToolStripMenuItem btnExportToImgWithTransparency;
        private ToolStripMenuItem btnExportToAnimatedGIF;
        private ToolStripMenuItem saveLocationToolStripMenuItem;
        public ToolStripMenuItem ScreenCapBgLocText;
        private ToolStripMenuItem displaySettingToolStripMenuItem;
        private ToolStripMenuItem stretchToolStripMenuItem1;
        private ToolStripMenuItem centerToolStripMenuItem1;
        private ToolStripMenuItem resizeToolStripMenuItem1;
        private ToolStripMenuItem imageFormatToolStripMenuItem;
        private ToolStripMenuItem projectionToolStripMenuItem;
        private ToolStripMenuItem perspectiveToolStripMenuItem;
        public ToolStripMenuItem orthographicToolStripMenuItem;
        private VertexEditor vertexEditor;
        private ToolStripMenuItem boundingBoxToolStripMenuItem;
        private ToolStripMenuItem toggleNormals;
        private ToolStripMenuItem wireframeToolStripMenuItem;
        private ToolStripMenuItem interpolationEditorToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem selectedAnimationToolStripMenuItem;
        private ToolStripMenuItem portToolStripMenuItem;
        private ToolStripMenuItem mergeToolStripMenuItem;
        private ToolStripMenuItem appendToolStripMenuItem;
        private ToolStripMenuItem resizeToolStripMenuItem;
        private ToolStripMenuItem playToolStripMenuItem;
        private ToolStripMenuItem interpolationToolStripMenuItem;
        private ToolStripMenuItem averageAllStartEndTangentsToolStripMenuItem;
        private ToolStripMenuItem averageboneStartendTangentsToolStripMenuItem;
        private ToolStripMenuItem SLocalToolStripMenuItem;
        private ToolStripMenuItem SWorldToolStripMenuItem;
        private ToolStripMenuItem RLocalToolStripMenuItem;
        private ToolStripMenuItem RWorldToolStripMenuItem;
        private ToolStripMenuItem TLocalToolStripMenuItem;
        private ToolStripMenuItem TWorldToolStripMenuItem;
        public ToolStripMenuItem chkNonBRRESAnims;
        public ToolStripMenuItem chkBRRESAnims;
        private ToolStripButton chkZoomExtents;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripComboBox cboToolSelect;
        private ToolStripDropDownButton dropdownOverlays;
        private ToolStripMenuItem chkBoundaries;
        private ToolStripMenuItem chkSpawns;
        private ToolStripMenuItem chkItems;
        private ToolStripMenuItem liveTextureFolderToolStripMenuItem;
        public ToolStripMenuItem LiveTextureFolderPath;
        public ToolStripMenuItem EnableLiveTextureFolder;
        public LeftPanel leftPanel;
        private ToolStripMenuItem chkBillboardBones;
        private ToolStripMenuItem chkBBObjects;
        private ToolStripMenuItem chkBBVisBones;
        private ToolStripMenuItem chkBBModels;
        private ToolStripMenuItem chkEditAll;
        private ToolStripMenuItem frontToolStripMenuItem;
        private ToolStripMenuItem backToolStripMenuItem;
        private ToolStripMenuItem leftToolStripMenuItem;
        private ToolStripMenuItem rightToolStripMenuItem;
        private ToolStripMenuItem topToolStripMenuItem;
        private ToolStripMenuItem bottomToolStripMenuItem;
        private ToolStripMenuItem detachViewerToolStripMenuItem;
        private ToolStripMenuItem firstPersonCameraToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem newViewportLeftToolStripMenuItem;
        private ToolStripMenuItem newViewportAboveToolStripMenuItem;
        private ToolStripMenuItem SCameraToolStripMenuItem;
        private ToolStripMenuItem RCameraToolStripMenuItem;
        private ToolStripMenuItem TCameraToolStripMenuItem;
        private ToolStripMenuItem shadersToolStripMenuItem;
        private ToolStripMenuItem playSCN0ToolStripMenuItem;
        private ToolStripMenuItem removeCurrentViewportToolStripMenuItem;
        private ToolStripMenuItem afterRotationToolStripMenuItem;
        private ToolStripMenuItem btnWeightEditor;
        private ToolStripMenuItem btnVertexEditor;
        public RightPanel rightPanel;

        private void InitializeComponent()
        {
            System.Windows.Forms.ModelPanelViewport modelPanelViewport1 = new System.Windows.Forms.ModelPanelViewport();
            BrawlLib.OpenGL.GLCamera glCamera1 = new BrawlLib.OpenGL.GLCamera();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.btnLeftToggle = new System.Windows.Forms.Button();
            this.btnRightToggle = new System.Windows.Forms.Button();
            this.btnBottomToggle = new System.Windows.Forms.Button();
            this.spltLeft = new System.Windows.Forms.Splitter();
            this.btnTopToggle = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenClose = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMovesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.takeScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportToImgNoTransparency = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportToImgWithTransparency = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportToAnimatedGIF = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScreenCapBgLocText = new System.Windows.Forms.ToolStripMenuItem();
            this.imageFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTop = new System.Windows.Forms.ToolStripMenuItem();
            this.showLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.showBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.showRight = new System.Windows.Forms.ToolStripMenuItem();
            this.detachViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displaySettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.centerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SLocalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RLocalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TLocalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afterRotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perspectiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orthographicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleFloor = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCameraCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstPersonCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newViewportLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newViewportAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCurrentViewportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleBones = new System.Windows.Forms.ToolStripMenuItem();
            this.togglePolygons = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleVertices = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleCollisions = new System.Windows.Forms.ToolStripMenuItem();
            this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleNormals = new System.Windows.Forms.ToolStripMenuItem();
            this.boundingBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkBBModels = new System.Windows.Forms.ToolStripMenuItem();
            this.chkBBObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.chkBBVisBones = new System.Windows.Forms.ToolStripMenuItem();
            this.shadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkBillboardBones = new System.Windows.Forms.ToolStripMenuItem();
            this.fileTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playCHR0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSRT0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSHP0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playPAT0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playVIS0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playCLR0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSCN0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sCN0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interpolationEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interpolationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.averageAllStartEndTangentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.averageboneStartendTangentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveTextureFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LiveTextureFolderPath = new System.Windows.Forms.ToolStripMenuItem();
            this.EnableLiveTextureFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.btnWeightEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.btnVertexEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.targetModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkEditAll = new System.Windows.Forms.ToolStripMenuItem();
            this.hideFromSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAllOtherModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllOtherModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkExternalAnims = new System.Windows.Forms.ToolStripMenuItem();
            this.chkBRRESAnims = new System.Windows.Forms.ToolStripMenuItem();
            this.chkNonBRRESAnims = new System.Windows.Forms.ToolStripMenuItem();
            this.kinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncKinectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notYetImplementedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startTrackingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.models = new System.Windows.Forms.ComboBox();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.chkBones = new System.Windows.Forms.ToolStripButton();
            this.chkPolygons = new System.Windows.Forms.ToolStripButton();
            this.chkVertices = new System.Windows.Forms.ToolStripButton();
            this.chkCollisions = new System.Windows.Forms.ToolStripButton();
            this.dropdownOverlays = new System.Windows.Forms.ToolStripDropDownButton();
            this.chkBoundaries = new System.Windows.Forms.ToolStripMenuItem();
            this.chkSpawns = new System.Windows.Forms.ToolStripMenuItem();
            this.chkItems = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chkFloor = new System.Windows.Forms.ToolStripButton();
            this.button1 = new System.Windows.Forms.ToolStripButton();
            this.chkZoomExtents = new System.Windows.Forms.ToolStripButton();
            this.btnSaveCam = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cboToolSelect = new System.Windows.Forms.ToolStripComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.spltRight = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.modelPanel = new System.Windows.Forms.ModelPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.animEditors = new System.Windows.Forms.Panel();
            this.pnlPlayback = new System.Windows.Forms.ModelPlaybackPanel();
            this.animCtrlPnl = new System.Windows.Forms.Panel();
            this.vis0Editor = new System.Windows.Forms.VIS0Editor();
            this.pat0Editor = new System.Windows.Forms.PAT0Editor();
            this.shp0Editor = new System.Windows.Forms.SHP0Editor();
            this.srt0Editor = new System.Windows.Forms.SRT0Editor();
            this.chr0Editor = new System.Windows.Forms.CHR0Editor();
            this.scn0Editor = new System.Windows.Forms.SCN0Editor();
            this.clr0Editor = new System.Windows.Forms.CLR0Editor();
            this.weightEditor = new System.Windows.Forms.WeightEditor();
            this.vertexEditor = new System.Windows.Forms.VertexEditor();
            this.rightPanel = new System.Windows.Forms.RightPanel();
            this.leftPanel = new System.Windows.Forms.LeftPanel();
            this.menuStrip1.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.animEditors.SuspendLayout();
            this.animCtrlPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            this.dlgColor.FullOpen = true;
            // 
            // btnLeftToggle
            // 
            this.btnLeftToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLeftToggle.Location = new System.Drawing.Point(174, 26);
            this.btnLeftToggle.Name = "btnLeftToggle";
            this.btnLeftToggle.Size = new System.Drawing.Size(15, 389);
            this.btnLeftToggle.TabIndex = 5;
            this.btnLeftToggle.TabStop = false;
            this.btnLeftToggle.Text = ">";
            this.btnLeftToggle.UseVisualStyleBackColor = false;
            this.btnLeftToggle.Click += new System.EventHandler(this.btnLeftToggle_Click);
            // 
            // btnRightToggle
            // 
            this.btnRightToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRightToggle.Location = new System.Drawing.Point(586, 26);
            this.btnRightToggle.Name = "btnRightToggle";
            this.btnRightToggle.Size = new System.Drawing.Size(15, 389);
            this.btnRightToggle.TabIndex = 6;
            this.btnRightToggle.TabStop = false;
            this.btnRightToggle.Text = "<";
            this.btnRightToggle.UseVisualStyleBackColor = false;
            this.btnRightToggle.Click += new System.EventHandler(this.btnRightToggle_Click);
            // 
            // btnBottomToggle
            // 
            this.btnBottomToggle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnBottomToggle.Location = new System.Drawing.Point(189, 400);
            this.btnBottomToggle.Name = "btnBottomToggle";
            this.btnBottomToggle.Size = new System.Drawing.Size(397, 15);
            this.btnBottomToggle.TabIndex = 8;
            this.btnBottomToggle.TabStop = false;
            this.btnBottomToggle.UseVisualStyleBackColor = false;
            this.btnBottomToggle.Click += new System.EventHandler(this.btnBottomToggle_Click);
            // 
            // spltLeft
            // 
            this.spltLeft.BackColor = System.Drawing.SystemColors.Control;
            this.spltLeft.Location = new System.Drawing.Point(170, 26);
            this.spltLeft.Name = "spltLeft";
            this.spltLeft.Size = new System.Drawing.Size(4, 389);
            this.spltLeft.TabIndex = 9;
            this.spltLeft.TabStop = false;
            this.spltLeft.Visible = false;
            // 
            // btnTopToggle
            // 
            this.btnTopToggle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTopToggle.Location = new System.Drawing.Point(189, 26);
            this.btnTopToggle.Name = "btnTopToggle";
            this.btnTopToggle.Size = new System.Drawing.Size(397, 15);
            this.btnTopToggle.TabIndex = 11;
            this.btnTopToggle.TabStop = false;
            this.btnTopToggle.UseVisualStyleBackColor = false;
            this.btnTopToggle.Click += new System.EventHandler(this.btnTopToggle_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem1,
            this.toolsToolStripMenuItem,
            this.targetModelToolStripMenuItem,
            this.kinectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(358, 28);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSceneToolStripMenuItem,
            this.openModelsToolStripMenuItem,
            this.openAnimationsToolStripMenuItem,
            this.openMovesetToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSceneToolStripMenuItem
            // 
            this.newSceneToolStripMenuItem.Name = "newSceneToolStripMenuItem";
            this.newSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newSceneToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.newSceneToolStripMenuItem.Text = "New Scene";
            this.newSceneToolStripMenuItem.Click += new System.EventHandler(this.newSceneToolStripMenuItem_Click);
            // 
            // openModelsToolStripMenuItem
            // 
            this.openModelsToolStripMenuItem.Name = "openModelsToolStripMenuItem";
            this.openModelsToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.openModelsToolStripMenuItem.Text = "Load Models";
            this.openModelsToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openAnimationsToolStripMenuItem
            // 
            this.openAnimationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenClose,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.openAnimationsToolStripMenuItem.Name = "openAnimationsToolStripMenuItem";
            this.openAnimationsToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.openAnimationsToolStripMenuItem.Text = "Animations";
            // 
            // btnOpenClose
            // 
            this.btnOpenClose.Name = "btnOpenClose";
            this.btnOpenClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.btnOpenClose.Size = new System.Drawing.Size(225, 26);
            this.btnOpenClose.Text = "Load";
            this.btnOpenClose.Click += new System.EventHandler(this.btnLoadAnimations_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.saveToolStripMenuItem.Text = "Save ";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // openMovesetToolStripMenuItem
            // 
            this.openMovesetToolStripMenuItem.Name = "openMovesetToolStripMenuItem";
            this.openMovesetToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.openMovesetToolStripMenuItem.Text = "Load Moveset";
            this.openMovesetToolStripMenuItem.Visible = false;
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.closeToolStripMenuItem.Text = "Close Window";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUndo,
            this.btnRedo,
            this.takeScreenshotToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.editToolStripMenuItem.Text = "Options";
            // 
            // btnUndo
            // 
            this.btnUndo.Enabled = false;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.btnUndo.Size = new System.Drawing.Size(189, 26);
            this.btnUndo.Text = "Undo";
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Enabled = false;
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.btnRedo.Size = new System.Drawing.Size(189, 26);
            this.btnRedo.Text = "Redo";
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // takeScreenshotToolStripMenuItem
            // 
            this.takeScreenshotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExportToImgNoTransparency,
            this.btnExportToImgWithTransparency,
            this.btnExportToAnimatedGIF,
            this.saveLocationToolStripMenuItem,
            this.imageFormatToolStripMenuItem});
            this.takeScreenshotToolStripMenuItem.Name = "takeScreenshotToolStripMenuItem";
            this.takeScreenshotToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.takeScreenshotToolStripMenuItem.Text = "Take Screenshot";
            // 
            // btnExportToImgNoTransparency
            // 
            this.btnExportToImgNoTransparency.Name = "btnExportToImgNoTransparency";
            this.btnExportToImgNoTransparency.ShortcutKeyDisplayString = "Ctrl+Shift+I";
            this.btnExportToImgNoTransparency.Size = new System.Drawing.Size(354, 26);
            this.btnExportToImgNoTransparency.Text = "With Background";
            this.btnExportToImgNoTransparency.Click += new System.EventHandler(this.btnExportToImgNoTransparency_Click);
            // 
            // btnExportToImgWithTransparency
            // 
            this.btnExportToImgWithTransparency.Name = "btnExportToImgWithTransparency";
            this.btnExportToImgWithTransparency.ShortcutKeyDisplayString = "Ctrl+Alt+I";
            this.btnExportToImgWithTransparency.Size = new System.Drawing.Size(354, 26);
            this.btnExportToImgWithTransparency.Text = "With Transparent Background";
            this.btnExportToImgWithTransparency.Click += new System.EventHandler(this.btnExportToImgWithTransparency_Click);
            // 
            // btnExportToAnimatedGIF
            // 
            this.btnExportToAnimatedGIF.Name = "btnExportToAnimatedGIF";
            this.btnExportToAnimatedGIF.Size = new System.Drawing.Size(354, 26);
            this.btnExportToAnimatedGIF.Text = "To Animated GIF";
            this.btnExportToAnimatedGIF.Click += new System.EventHandler(this.btnExportToAnimatedGIF_Click);
            // 
            // saveLocationToolStripMenuItem
            // 
            this.saveLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScreenCapBgLocText});
            this.saveLocationToolStripMenuItem.Name = "saveLocationToolStripMenuItem";
            this.saveLocationToolStripMenuItem.Size = new System.Drawing.Size(354, 26);
            this.saveLocationToolStripMenuItem.Text = "Save Location";
            // 
            // ScreenCapBgLocText
            // 
            this.ScreenCapBgLocText.Name = "ScreenCapBgLocText";
            this.ScreenCapBgLocText.Size = new System.Drawing.Size(128, 26);
            this.ScreenCapBgLocText.Text = "<null>";
            this.ScreenCapBgLocText.Click += new System.EventHandler(this.ScreenCapBgLocText_Click);
            // 
            // imageFormatToolStripMenuItem
            // 
            this.imageFormatToolStripMenuItem.Name = "imageFormatToolStripMenuItem";
            this.imageFormatToolStripMenuItem.Size = new System.Drawing.Size(354, 26);
            this.imageFormatToolStripMenuItem.Text = "Image Format: PNG";
            this.imageFormatToolStripMenuItem.Click += new System.EventHandler(this.imageFormatToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem1
            // 
            this.viewToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorsToolStripMenuItem,
            this.viewportToolStripMenuItem,
            this.modelToolStripMenuItem,
            this.fileTypesToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            this.viewToolStripMenuItem1.Size = new System.Drawing.Size(53, 24);
            this.viewToolStripMenuItem1.Text = "View";
            // 
            // editorsToolStripMenuItem
            // 
            this.editorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showTop,
            this.showLeft,
            this.showBottom,
            this.showRight,
            this.detachViewerToolStripMenuItem});
            this.editorsToolStripMenuItem.Name = "editorsToolStripMenuItem";
            this.editorsToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.editorsToolStripMenuItem.Text = "Panels";
            // 
            // showTop
            // 
            this.showTop.CheckOnClick = true;
            this.showTop.Name = "showTop";
            this.showTop.Size = new System.Drawing.Size(227, 26);
            this.showTop.Text = "Menu Bar";
            this.showTop.CheckedChanged += new System.EventHandler(this.showTop_CheckedChanged);
            // 
            // showLeft
            // 
            this.showLeft.CheckOnClick = true;
            this.showLeft.Name = "showLeft";
            this.showLeft.Size = new System.Drawing.Size(227, 26);
            this.showLeft.Text = "Left Panel";
            this.showLeft.CheckedChanged += new System.EventHandler(this.showLeft_CheckedChanged);
            // 
            // showBottom
            // 
            this.showBottom.CheckOnClick = true;
            this.showBottom.Name = "showBottom";
            this.showBottom.Size = new System.Drawing.Size(227, 26);
            this.showBottom.Text = "Animation Panel";
            this.showBottom.CheckedChanged += new System.EventHandler(this.showBottom_CheckedChanged);
            // 
            // showRight
            // 
            this.showRight.CheckOnClick = true;
            this.showRight.Name = "showRight";
            this.showRight.Size = new System.Drawing.Size(227, 26);
            this.showRight.Text = "Right Panel";
            this.showRight.CheckedChanged += new System.EventHandler(this.showRight_CheckedChanged);
            // 
            // detachViewerToolStripMenuItem
            // 
            this.detachViewerToolStripMenuItem.Name = "detachViewerToolStripMenuItem";
            this.detachViewerToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
            this.detachViewerToolStripMenuItem.Text = "Detach Model Viewer";
            this.detachViewerToolStripMenuItem.Click += new System.EventHandler(this.detachViewerToolStripMenuItem_Click);
            // 
            // viewportToolStripMenuItem
            // 
            this.viewportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundToolStripMenuItem,
            this.editControlToolStripMenuItem,
            this.projectionToolStripMenuItem,
            this.toggleFloor,
            this.resetCameraToolStripMenuItem,
            this.showCameraCoordinatesToolStripMenuItem,
            this.firstPersonCameraToolStripMenuItem,
            this.newToolStripMenuItem,
            this.removeCurrentViewportToolStripMenuItem});
            this.viewportToolStripMenuItem.Name = "viewportToolStripMenuItem";
            this.viewportToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.viewportToolStripMenuItem.Text = "Viewport";
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setColorToolStripMenuItem,
            this.loadImageToolStripMenuItem,
            this.displaySettingToolStripMenuItem});
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.backgroundToolStripMenuItem.Text = "Background";
            // 
            // setColorToolStripMenuItem
            // 
            this.setColorToolStripMenuItem.Name = "setColorToolStripMenuItem";
            this.setColorToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.setColorToolStripMenuItem.Text = "Set Color";
            this.setColorToolStripMenuItem.Click += new System.EventHandler(this.setColorToolStripMenuItem_Click);
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.loadImageToolStripMenuItem.Text = "Load Image";
            this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);
            // 
            // displaySettingToolStripMenuItem
            // 
            this.displaySettingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stretchToolStripMenuItem1,
            this.centerToolStripMenuItem1,
            this.resizeToolStripMenuItem1});
            this.displaySettingToolStripMenuItem.Name = "displaySettingToolStripMenuItem";
            this.displaySettingToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.displaySettingToolStripMenuItem.Text = "Display Setting";
            // 
            // stretchToolStripMenuItem1
            // 
            this.stretchToolStripMenuItem1.Checked = true;
            this.stretchToolStripMenuItem1.CheckOnClick = true;
            this.stretchToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stretchToolStripMenuItem1.Name = "stretchToolStripMenuItem1";
            this.stretchToolStripMenuItem1.Size = new System.Drawing.Size(130, 26);
            this.stretchToolStripMenuItem1.Text = "Stretch";
            this.stretchToolStripMenuItem1.Click += new System.EventHandler(this.stretchToolStripMenuItem1_Click);
            // 
            // centerToolStripMenuItem1
            // 
            this.centerToolStripMenuItem1.CheckOnClick = true;
            this.centerToolStripMenuItem1.Name = "centerToolStripMenuItem1";
            this.centerToolStripMenuItem1.Size = new System.Drawing.Size(130, 26);
            this.centerToolStripMenuItem1.Text = "Center";
            this.centerToolStripMenuItem1.Click += new System.EventHandler(this.centerToolStripMenuItem1_Click);
            // 
            // resizeToolStripMenuItem1
            // 
            this.resizeToolStripMenuItem1.CheckOnClick = true;
            this.resizeToolStripMenuItem1.Name = "resizeToolStripMenuItem1";
            this.resizeToolStripMenuItem1.Size = new System.Drawing.Size(130, 26);
            this.resizeToolStripMenuItem1.Text = "Resize";
            this.resizeToolStripMenuItem1.Click += new System.EventHandler(this.resizeToolStripMenuItem1_Click);
            // 
            // editControlToolStripMenuItem
            // 
            this.editControlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scaleToolStripMenuItem,
            this.rotationToolStripMenuItem,
            this.translationToolStripMenuItem});
            this.editControlToolStripMenuItem.Name = "editControlToolStripMenuItem";
            this.editControlToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.editControlToolStripMenuItem.Text = "Transform Control";
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.CheckOnClick = true;
            this.scaleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLocalToolStripMenuItem,
            this.SWorldToolStripMenuItem,
            this.SCameraToolStripMenuItem});
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.ShortcutKeyDisplayString = "E Key";
            this.scaleToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.scaleToolStripMenuItem.Text = "Scale";
            this.scaleToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // SLocalToolStripMenuItem
            // 
            this.SLocalToolStripMenuItem.Checked = true;
            this.SLocalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SLocalToolStripMenuItem.Name = "SLocalToolStripMenuItem";
            this.SLocalToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.SLocalToolStripMenuItem.Text = "Local";
            this.SLocalToolStripMenuItem.Click += new System.EventHandler(this.SLocalToolStripMenuItem_Click);
            // 
            // SWorldToolStripMenuItem
            // 
            this.SWorldToolStripMenuItem.Name = "SWorldToolStripMenuItem";
            this.SWorldToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.SWorldToolStripMenuItem.Text = "World";
            this.SWorldToolStripMenuItem.Click += new System.EventHandler(this.SWorldToolStripMenuItem_Click);
            // 
            // SCameraToolStripMenuItem
            // 
            this.SCameraToolStripMenuItem.Enabled = false;
            this.SCameraToolStripMenuItem.Name = "SCameraToolStripMenuItem";
            this.SCameraToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.SCameraToolStripMenuItem.Text = "Screen";
            this.SCameraToolStripMenuItem.Visible = false;
            this.SCameraToolStripMenuItem.Click += new System.EventHandler(this.SCameraToolStripMenuItem_Click);
            // 
            // rotationToolStripMenuItem
            // 
            this.rotationToolStripMenuItem.Checked = true;
            this.rotationToolStripMenuItem.CheckOnClick = true;
            this.rotationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RLocalToolStripMenuItem,
            this.RWorldToolStripMenuItem,
            this.RCameraToolStripMenuItem});
            this.rotationToolStripMenuItem.Name = "rotationToolStripMenuItem";
            this.rotationToolStripMenuItem.ShortcutKeyDisplayString = "R Key";
            this.rotationToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.rotationToolStripMenuItem.Text = "Rotation";
            this.rotationToolStripMenuItem.Click += new System.EventHandler(this.rotationToolStripMenuItem_Click);
            // 
            // RLocalToolStripMenuItem
            // 
            this.RLocalToolStripMenuItem.Checked = true;
            this.RLocalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RLocalToolStripMenuItem.Name = "RLocalToolStripMenuItem";
            this.RLocalToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.RLocalToolStripMenuItem.Text = "Local";
            this.RLocalToolStripMenuItem.Click += new System.EventHandler(this.RLocalToolStripMenuItem_Click);
            // 
            // RWorldToolStripMenuItem
            // 
            this.RWorldToolStripMenuItem.Name = "RWorldToolStripMenuItem";
            this.RWorldToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.RWorldToolStripMenuItem.Text = "World";
            this.RWorldToolStripMenuItem.Click += new System.EventHandler(this.RWorldToolStripMenuItem_Click);
            // 
            // RCameraToolStripMenuItem
            // 
            this.RCameraToolStripMenuItem.Enabled = false;
            this.RCameraToolStripMenuItem.Name = "RCameraToolStripMenuItem";
            this.RCameraToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.RCameraToolStripMenuItem.Text = "Screen";
            this.RCameraToolStripMenuItem.Visible = false;
            this.RCameraToolStripMenuItem.Click += new System.EventHandler(this.RCameraToolStripMenuItem_Click);
            // 
            // translationToolStripMenuItem
            // 
            this.translationToolStripMenuItem.CheckOnClick = true;
            this.translationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TLocalToolStripMenuItem,
            this.TWorldToolStripMenuItem,
            this.TCameraToolStripMenuItem,
            this.afterRotationToolStripMenuItem});
            this.translationToolStripMenuItem.Name = "translationToolStripMenuItem";
            this.translationToolStripMenuItem.ShortcutKeyDisplayString = "T Key";
            this.translationToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.translationToolStripMenuItem.Text = "Translation";
            this.translationToolStripMenuItem.Click += new System.EventHandler(this.translationToolStripMenuItem_Click);
            // 
            // TLocalToolStripMenuItem
            // 
            this.TLocalToolStripMenuItem.Checked = true;
            this.TLocalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TLocalToolStripMenuItem.Name = "TLocalToolStripMenuItem";
            this.TLocalToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.TLocalToolStripMenuItem.Text = "Local";
            this.TLocalToolStripMenuItem.Click += new System.EventHandler(this.TLocalToolStripMenuItem_Click);
            // 
            // TWorldToolStripMenuItem
            // 
            this.TWorldToolStripMenuItem.Name = "TWorldToolStripMenuItem";
            this.TWorldToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.TWorldToolStripMenuItem.Text = "World";
            this.TWorldToolStripMenuItem.Click += new System.EventHandler(this.TWorldToolStripMenuItem_Click);
            // 
            // TCameraToolStripMenuItem
            // 
            this.TCameraToolStripMenuItem.Enabled = false;
            this.TCameraToolStripMenuItem.Name = "TCameraToolStripMenuItem";
            this.TCameraToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.TCameraToolStripMenuItem.Text = "Screen";
            this.TCameraToolStripMenuItem.Visible = false;
            this.TCameraToolStripMenuItem.Click += new System.EventHandler(this.TCameraToolStripMenuItem_Click);
            // 
            // afterRotationToolStripMenuItem
            // 
            this.afterRotationToolStripMenuItem.Name = "afterRotationToolStripMenuItem";
            this.afterRotationToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.afterRotationToolStripMenuItem.Text = "After Rotation";
            this.afterRotationToolStripMenuItem.Click += new System.EventHandler(this.afterRotationToolStripMenuItem_Click);
            // 
            // projectionToolStripMenuItem
            // 
            this.projectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perspectiveToolStripMenuItem,
            this.orthographicToolStripMenuItem,
            this.frontToolStripMenuItem,
            this.backToolStripMenuItem,
            this.leftToolStripMenuItem,
            this.rightToolStripMenuItem,
            this.topToolStripMenuItem,
            this.bottomToolStripMenuItem});
            this.projectionToolStripMenuItem.Name = "projectionToolStripMenuItem";
            this.projectionToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.projectionToolStripMenuItem.Text = "Projection";
            // 
            // perspectiveToolStripMenuItem
            // 
            this.perspectiveToolStripMenuItem.Checked = true;
            this.perspectiveToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.perspectiveToolStripMenuItem.Name = "perspectiveToolStripMenuItem";
            this.perspectiveToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.perspectiveToolStripMenuItem.Text = "Perspective";
            this.perspectiveToolStripMenuItem.Click += new System.EventHandler(this.perspectiveToolStripMenuItem_Click);
            // 
            // orthographicToolStripMenuItem
            // 
            this.orthographicToolStripMenuItem.Name = "orthographicToolStripMenuItem";
            this.orthographicToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.orthographicToolStripMenuItem.Text = "Orthographic";
            this.orthographicToolStripMenuItem.Click += new System.EventHandler(this.orthographicToolStripMenuItem_Click);
            // 
            // frontToolStripMenuItem
            // 
            this.frontToolStripMenuItem.Name = "frontToolStripMenuItem";
            this.frontToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.frontToolStripMenuItem.Text = "Front";
            this.frontToolStripMenuItem.Click += new System.EventHandler(this.frontToolStripMenuItem_Click);
            // 
            // backToolStripMenuItem
            // 
            this.backToolStripMenuItem.Name = "backToolStripMenuItem";
            this.backToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.backToolStripMenuItem.Text = "Back";
            this.backToolStripMenuItem.Click += new System.EventHandler(this.backToolStripMenuItem_Click);
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.leftToolStripMenuItem.Text = "Left";
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.rightToolStripMenuItem.Text = "Right";
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.topToolStripMenuItem.Text = "Top";
            this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // bottomToolStripMenuItem
            // 
            this.bottomToolStripMenuItem.Name = "bottomToolStripMenuItem";
            this.bottomToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.bottomToolStripMenuItem.Text = "Bottom";
            this.bottomToolStripMenuItem.Click += new System.EventHandler(this.bottomToolStripMenuItem_Click);
            // 
            // toggleFloor
            // 
            this.toggleFloor.Name = "toggleFloor";
            this.toggleFloor.ShortcutKeyDisplayString = "F Key";
            this.toggleFloor.Size = new System.Drawing.Size(259, 26);
            this.toggleFloor.Text = "Floor";
            this.toggleFloor.Click += new System.EventHandler(this.toggleRenderFloor_Event);
            // 
            // resetCameraToolStripMenuItem
            // 
            this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
            this.resetCameraToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+R";
            this.resetCameraToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.resetCameraToolStripMenuItem.Text = "Reset Camera";
            this.resetCameraToolStripMenuItem.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click_1);
            // 
            // showCameraCoordinatesToolStripMenuItem
            // 
            this.showCameraCoordinatesToolStripMenuItem.Name = "showCameraCoordinatesToolStripMenuItem";
            this.showCameraCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.showCameraCoordinatesToolStripMenuItem.Text = "Show Camera Coordinates";
            this.showCameraCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.showCameraCoordinatesToolStripMenuItem_Click);
            // 
            // firstPersonCameraToolStripMenuItem
            // 
            this.firstPersonCameraToolStripMenuItem.Name = "firstPersonCameraToolStripMenuItem";
            this.firstPersonCameraToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.firstPersonCameraToolStripMenuItem.Text = "1st Person SCN0 Camera";
            this.firstPersonCameraToolStripMenuItem.Click += new System.EventHandler(this.firstPersonCameraToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newViewportLeftToolStripMenuItem,
            this.newViewportAboveToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.newToolStripMenuItem.Text = "Add New Viewport";
            // 
            // newViewportLeftToolStripMenuItem
            // 
            this.newViewportLeftToolStripMenuItem.Name = "newViewportLeftToolStripMenuItem";
            this.newViewportLeftToolStripMenuItem.Size = new System.Drawing.Size(151, 26);
            this.newViewportLeftToolStripMenuItem.Text = "To the left";
            this.newViewportLeftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem1_Click);
            // 
            // newViewportAboveToolStripMenuItem
            // 
            this.newViewportAboveToolStripMenuItem.Name = "newViewportAboveToolStripMenuItem";
            this.newViewportAboveToolStripMenuItem.Size = new System.Drawing.Size(151, 26);
            this.newViewportAboveToolStripMenuItem.Text = "Above";
            this.newViewportAboveToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem1_Click);
            // 
            // removeCurrentViewportToolStripMenuItem
            // 
            this.removeCurrentViewportToolStripMenuItem.Name = "removeCurrentViewportToolStripMenuItem";
            this.removeCurrentViewportToolStripMenuItem.Size = new System.Drawing.Size(259, 26);
            this.removeCurrentViewportToolStripMenuItem.Text = "Remove Current Viewport";
            this.removeCurrentViewportToolStripMenuItem.Click += new System.EventHandler(this.removeCurrentViewportToolStripMenuItem_Click);
            // 
            // modelToolStripMenuItem
            // 
            this.modelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleBones,
            this.togglePolygons,
            this.toggleVertices,
            this.toggleCollisions,
            this.wireframeToolStripMenuItem,
            this.toggleNormals,
            this.boundingBoxToolStripMenuItem,
            this.shadersToolStripMenuItem,
            this.chkBillboardBones});
            this.modelToolStripMenuItem.Name = "modelToolStripMenuItem";
            this.modelToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.modelToolStripMenuItem.Text = "Model";
            // 
            // toggleBones
            // 
            this.toggleBones.Checked = true;
            this.toggleBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleBones.Name = "toggleBones";
            this.toggleBones.ShortcutKeyDisplayString = "B Key";
            this.toggleBones.Size = new System.Drawing.Size(189, 26);
            this.toggleBones.Text = "Bones";
            this.toggleBones.Click += new System.EventHandler(this.toggleRenderBones_Event);
            // 
            // togglePolygons
            // 
            this.togglePolygons.Checked = true;
            this.togglePolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.togglePolygons.Name = "togglePolygons";
            this.togglePolygons.ShortcutKeyDisplayString = "P Key";
            this.togglePolygons.Size = new System.Drawing.Size(189, 26);
            this.togglePolygons.Text = "Polygons";
            this.togglePolygons.Click += new System.EventHandler(this.toggleRenderPolygons_Event);
            // 
            // toggleVertices
            // 
            this.toggleVertices.Checked = true;
            this.toggleVertices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleVertices.Name = "toggleVertices";
            this.toggleVertices.ShortcutKeyDisplayString = "V Key";
            this.toggleVertices.Size = new System.Drawing.Size(189, 26);
            this.toggleVertices.Text = "Vertices";
            this.toggleVertices.Click += new System.EventHandler(this.toggleRenderVertices_Event);
            // 
            // toggleCollisions
            // 
            this.toggleCollisions.Checked = true;
            this.toggleCollisions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleCollisions.Name = "toggleCollisions";
            this.toggleCollisions.Size = new System.Drawing.Size(189, 26);
            this.toggleCollisions.Text = "Collisions";
            this.toggleCollisions.Click += new System.EventHandler(this.toggleRenderCollisions_Event);
            // 
            // wireframeToolStripMenuItem
            // 
            this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
            this.wireframeToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.wireframeToolStripMenuItem.Text = "Wireframe";
            this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click);
            // 
            // toggleNormals
            // 
            this.toggleNormals.Name = "toggleNormals";
            this.toggleNormals.Size = new System.Drawing.Size(189, 26);
            this.toggleNormals.Text = "Normals";
            this.toggleNormals.Click += new System.EventHandler(this.toggleNormals_Click);
            // 
            // boundingBoxToolStripMenuItem
            // 
            this.boundingBoxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkBBModels,
            this.chkBBObjects,
            this.chkBBVisBones});
            this.boundingBoxToolStripMenuItem.Name = "boundingBoxToolStripMenuItem";
            this.boundingBoxToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.boundingBoxToolStripMenuItem.Text = "Bounding Box";
            // 
            // chkBBModels
            // 
            this.chkBBModels.Name = "chkBBModels";
            this.chkBBModels.Size = new System.Drawing.Size(184, 26);
            this.chkBBModels.Text = "Models";
            this.chkBBModels.Click += new System.EventHandler(this.modelToolStripMenuItem1_Click);
            // 
            // chkBBObjects
            // 
            this.chkBBObjects.Name = "chkBBObjects";
            this.chkBBObjects.Size = new System.Drawing.Size(184, 26);
            this.chkBBObjects.Text = "Objects";
            this.chkBBObjects.Click += new System.EventHandler(this.objectsToolStripMenuItem_Click);
            // 
            // chkBBVisBones
            // 
            this.chkBBVisBones.Name = "chkBBVisBones";
            this.chkBBVisBones.Size = new System.Drawing.Size(184, 26);
            this.chkBBVisBones.Text = "Visibility Bones";
            this.chkBBVisBones.Click += new System.EventHandler(this.visibilityBonesToolStripMenuItem_Click);
            // 
            // shadersToolStripMenuItem
            // 
            this.shadersToolStripMenuItem.Checked = true;
            this.shadersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shadersToolStripMenuItem.Name = "shadersToolStripMenuItem";
            this.shadersToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.shadersToolStripMenuItem.Text = "Shaders";
            this.shadersToolStripMenuItem.Click += new System.EventHandler(this.shadersToolStripMenuItem_Click);
            // 
            // chkBillboardBones
            // 
            this.chkBillboardBones.Checked = true;
            this.chkBillboardBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBillboardBones.Name = "chkBillboardBones";
            this.chkBillboardBones.Size = new System.Drawing.Size(189, 26);
            this.chkBillboardBones.Text = "Billboard Bones";
            this.chkBillboardBones.Click += new System.EventHandler(this.chkBillboardBones_Click);
            // 
            // fileTypesToolStripMenuItem
            // 
            this.fileTypesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.sCN0ToolStripMenuItem});
            this.fileTypesToolStripMenuItem.Name = "fileTypesToolStripMenuItem";
            this.fileTypesToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.fileTypesToolStripMenuItem.Text = "Animations";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playCHR0ToolStripMenuItem,
            this.playSRT0ToolStripMenuItem,
            this.playSHP0ToolStripMenuItem,
            this.playPAT0ToolStripMenuItem,
            this.playVIS0ToolStripMenuItem,
            this.playCLR0ToolStripMenuItem,
            this.playSCN0ToolStripMenuItem});
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(266, 26);
            this.playToolStripMenuItem.Text = "Play";
            // 
            // playCHR0ToolStripMenuItem
            // 
            this.playCHR0ToolStripMenuItem.Checked = true;
            this.playCHR0ToolStripMenuItem.CheckOnClick = true;
            this.playCHR0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playCHR0ToolStripMenuItem.Name = "playCHR0ToolStripMenuItem";
            this.playCHR0ToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.playCHR0ToolStripMenuItem.Text = "CHR0";
            this.playCHR0ToolStripMenuItem.Click += new System.EventHandler(this.playCHR0ToolStripMenuItem_Click);
            // 
            // playSRT0ToolStripMenuItem
            // 
            this.playSRT0ToolStripMenuItem.Checked = true;
            this.playSRT0ToolStripMenuItem.CheckOnClick = true;
            this.playSRT0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSRT0ToolStripMenuItem.Name = "playSRT0ToolStripMenuItem";
            this.playSRT0ToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.playSRT0ToolStripMenuItem.Text = "SRT0";
            this.playSRT0ToolStripMenuItem.Click += new System.EventHandler(this.playSRT0ToolStripMenuItem_Click);
            // 
            // playSHP0ToolStripMenuItem
            // 
            this.playSHP0ToolStripMenuItem.Checked = true;
            this.playSHP0ToolStripMenuItem.CheckOnClick = true;
            this.playSHP0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSHP0ToolStripMenuItem.Name = "playSHP0ToolStripMenuItem";
            this.playSHP0ToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.playSHP0ToolStripMenuItem.Text = "SHP0";
            this.playSHP0ToolStripMenuItem.Click += new System.EventHandler(this.playSHP0ToolStripMenuItem_Click);
            // 
            // playPAT0ToolStripMenuItem
            // 
            this.playPAT0ToolStripMenuItem.Checked = true;
            this.playPAT0ToolStripMenuItem.CheckOnClick = true;
            this.playPAT0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playPAT0ToolStripMenuItem.Name = "playPAT0ToolStripMenuItem";
            this.playPAT0ToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.playPAT0ToolStripMenuItem.Text = "PAT0";
            this.playPAT0ToolStripMenuItem.Click += new System.EventHandler(this.playPAT0ToolStripMenuItem_Click);
            // 
            // playVIS0ToolStripMenuItem
            // 
            this.playVIS0ToolStripMenuItem.Checked = true;
            this.playVIS0ToolStripMenuItem.CheckOnClick = true;
            this.playVIS0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playVIS0ToolStripMenuItem.Name = "playVIS0ToolStripMenuItem";
            this.playVIS0ToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.playVIS0ToolStripMenuItem.Text = "VIS0";
            this.playVIS0ToolStripMenuItem.Click += new System.EventHandler(this.playVIS0ToolStripMenuItem_Click);
            // 
            // playCLR0ToolStripMenuItem
            // 
            this.playCLR0ToolStripMenuItem.Checked = true;
            this.playCLR0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playCLR0ToolStripMenuItem.Name = "playCLR0ToolStripMenuItem";
            this.playCLR0ToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.playCLR0ToolStripMenuItem.Text = "CLR0";
            this.playCLR0ToolStripMenuItem.Click += new System.EventHandler(this.playCLR0ToolStripMenuItem_Click);
            // 
            // playSCN0ToolStripMenuItem
            // 
            this.playSCN0ToolStripMenuItem.Checked = true;
            this.playSCN0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSCN0ToolStripMenuItem.Name = "playSCN0ToolStripMenuItem";
            this.playSCN0ToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.playSCN0ToolStripMenuItem.Text = "SCN0";
            this.playSCN0ToolStripMenuItem.Click += new System.EventHandler(this.playSCN0ToolStripMenuItem1_Click);
            // 
            // sCN0ToolStripMenuItem
            // 
            this.sCN0ToolStripMenuItem.Checked = true;
            this.sCN0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sCN0ToolStripMenuItem.Name = "sCN0ToolStripMenuItem";
            this.sCN0ToolStripMenuItem.Size = new System.Drawing.Size(266, 26);
            this.sCN0ToolStripMenuItem.Text = "Show SCN0 Lights/Cameras";
            this.sCN0ToolStripMenuItem.Click += new System.EventHandler(this.sCN0ToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interpolationEditorToolStripMenuItem,
            this.selectedAnimationToolStripMenuItem,
            this.liveTextureFolderToolStripMenuItem,
            this.btnWeightEditor,
            this.btnVertexEditor});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // interpolationEditorToolStripMenuItem
            // 
            this.interpolationEditorToolStripMenuItem.Name = "interpolationEditorToolStripMenuItem";
            this.interpolationEditorToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.interpolationEditorToolStripMenuItem.Text = "Interpolation Editor";
            this.interpolationEditorToolStripMenuItem.Click += new System.EventHandler(this.interpolationEditorToolStripMenuItem_Click);
            // 
            // selectedAnimationToolStripMenuItem
            // 
            this.selectedAnimationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portToolStripMenuItem,
            this.mergeToolStripMenuItem,
            this.appendToolStripMenuItem,
            this.resizeToolStripMenuItem,
            this.interpolationToolStripMenuItem});
            this.selectedAnimationToolStripMenuItem.Enabled = false;
            this.selectedAnimationToolStripMenuItem.Name = "selectedAnimationToolStripMenuItem";
            this.selectedAnimationToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.selectedAnimationToolStripMenuItem.Text = "Selected Animation";
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Enabled = false;
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.portToolStripMenuItem.Text = "Port";
            this.portToolStripMenuItem.Click += new System.EventHandler(this.portToolStripMenuItem_Click);
            // 
            // mergeToolStripMenuItem
            // 
            this.mergeToolStripMenuItem.Enabled = false;
            this.mergeToolStripMenuItem.Name = "mergeToolStripMenuItem";
            this.mergeToolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.mergeToolStripMenuItem.Text = "Merge";
            this.mergeToolStripMenuItem.Click += new System.EventHandler(this.mergeToolStripMenuItem_Click);
            // 
            // appendToolStripMenuItem
            // 
            this.appendToolStripMenuItem.Enabled = false;
            this.appendToolStripMenuItem.Name = "appendToolStripMenuItem";
            this.appendToolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.appendToolStripMenuItem.Text = "Append";
            this.appendToolStripMenuItem.Click += new System.EventHandler(this.appendToolStripMenuItem_Click);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.Enabled = false;
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.resizeToolStripMenuItem.Text = "Resize";
            this.resizeToolStripMenuItem.Click += new System.EventHandler(this.resizeToolStripMenuItem_Click);
            // 
            // interpolationToolStripMenuItem
            // 
            this.interpolationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.averageAllStartEndTangentsToolStripMenuItem,
            this.averageboneStartendTangentsToolStripMenuItem});
            this.interpolationToolStripMenuItem.Name = "interpolationToolStripMenuItem";
            this.interpolationToolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.interpolationToolStripMenuItem.Text = "Interpolation";
            // 
            // averageAllStartEndTangentsToolStripMenuItem
            // 
            this.averageAllStartEndTangentsToolStripMenuItem.Name = "averageAllStartEndTangentsToolStripMenuItem";
            this.averageAllStartEndTangentsToolStripMenuItem.Size = new System.Drawing.Size(311, 26);
            this.averageAllStartEndTangentsToolStripMenuItem.Text = "Average all start/end keyframes";
            this.averageAllStartEndTangentsToolStripMenuItem.Click += new System.EventHandler(this.averageAllStartEndTangentsToolStripMenuItem_Click);
            // 
            // averageboneStartendTangentsToolStripMenuItem
            // 
            this.averageboneStartendTangentsToolStripMenuItem.Enabled = false;
            this.averageboneStartendTangentsToolStripMenuItem.Name = "averageboneStartendTangentsToolStripMenuItem";
            this.averageboneStartendTangentsToolStripMenuItem.Size = new System.Drawing.Size(311, 26);
            this.averageboneStartendTangentsToolStripMenuItem.Text = "Average entry start/end keyframes";
            this.averageboneStartendTangentsToolStripMenuItem.Click += new System.EventHandler(this.averageboneStartendTangentsToolStripMenuItem_Click);
            // 
            // liveTextureFolderToolStripMenuItem
            // 
            this.liveTextureFolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LiveTextureFolderPath,
            this.EnableLiveTextureFolder});
            this.liveTextureFolderToolStripMenuItem.Name = "liveTextureFolderToolStripMenuItem";
            this.liveTextureFolderToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.liveTextureFolderToolStripMenuItem.Text = "Live Texture Folder";
            // 
            // LiveTextureFolderPath
            // 
            this.LiveTextureFolderPath.Name = "LiveTextureFolderPath";
            this.LiveTextureFolderPath.Size = new System.Drawing.Size(138, 26);
            this.LiveTextureFolderPath.Text = "<path>";
            this.LiveTextureFolderPath.Click += new System.EventHandler(this.LiveTextureFolderPath_Click);
            // 
            // EnableLiveTextureFolder
            // 
            this.EnableLiveTextureFolder.Name = "EnableLiveTextureFolder";
            this.EnableLiveTextureFolder.Size = new System.Drawing.Size(138, 26);
            this.EnableLiveTextureFolder.Text = "Enabled";
            this.EnableLiveTextureFolder.Click += new System.EventHandler(this.EnableLiveTextureFolder_Click);
            // 
            // btnWeightEditor
            // 
            this.btnWeightEditor.Name = "btnWeightEditor";
            this.btnWeightEditor.ShortcutKeyDisplayString = "9 Key";
            this.btnWeightEditor.Size = new System.Drawing.Size(220, 26);
            this.btnWeightEditor.Text = "Weight Editor";
            this.btnWeightEditor.Click += new System.EventHandler(this.btnWeightEditor_Click);
            // 
            // btnVertexEditor
            // 
            this.btnVertexEditor.Name = "btnVertexEditor";
            this.btnVertexEditor.ShortcutKeyDisplayString = "0 Key";
            this.btnVertexEditor.Size = new System.Drawing.Size(220, 26);
            this.btnVertexEditor.Text = "Vertex Editor";
            this.btnVertexEditor.Click += new System.EventHandler(this.btnVertexEditor_Click);
            // 
            // targetModelToolStripMenuItem
            // 
            this.targetModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkEditAll,
            this.hideFromSceneToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.hideAllOtherModelsToolStripMenuItem,
            this.deleteAllOtherModelsToolStripMenuItem,
            this.chkExternalAnims,
            this.chkBRRESAnims,
            this.chkNonBRRESAnims});
            this.targetModelToolStripMenuItem.Name = "targetModelToolStripMenuItem";
            this.targetModelToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.targetModelToolStripMenuItem.Text = "Target Model:";
            // 
            // chkEditAll
            // 
            this.chkEditAll.Name = "chkEditAll";
            this.chkEditAll.Size = new System.Drawing.Size(299, 26);
            this.chkEditAll.Text = "Edit All";
            this.chkEditAll.Click += new System.EventHandler(this.chkEditAll_Click);
            // 
            // hideFromSceneToolStripMenuItem
            // 
            this.hideFromSceneToolStripMenuItem.Name = "hideFromSceneToolStripMenuItem";
            this.hideFromSceneToolStripMenuItem.Size = new System.Drawing.Size(299, 26);
            this.hideFromSceneToolStripMenuItem.Text = "Hide from scene";
            this.hideFromSceneToolStripMenuItem.Click += new System.EventHandler(this.hideFromSceneToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(299, 26);
            this.deleteToolStripMenuItem.Text = "Delete from scene";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // hideAllOtherModelsToolStripMenuItem
            // 
            this.hideAllOtherModelsToolStripMenuItem.Name = "hideAllOtherModelsToolStripMenuItem";
            this.hideAllOtherModelsToolStripMenuItem.Size = new System.Drawing.Size(299, 26);
            this.hideAllOtherModelsToolStripMenuItem.Text = "Hide all other models";
            this.hideAllOtherModelsToolStripMenuItem.Click += new System.EventHandler(this.hideAllOtherModelsToolStripMenuItem_Click);
            // 
            // deleteAllOtherModelsToolStripMenuItem
            // 
            this.deleteAllOtherModelsToolStripMenuItem.Name = "deleteAllOtherModelsToolStripMenuItem";
            this.deleteAllOtherModelsToolStripMenuItem.Size = new System.Drawing.Size(299, 26);
            this.deleteAllOtherModelsToolStripMenuItem.Text = "Delete all other models";
            this.deleteAllOtherModelsToolStripMenuItem.Click += new System.EventHandler(this.deleteAllOtherModelsToolStripMenuItem_Click);
            // 
            // chkExternalAnims
            // 
            this.chkExternalAnims.Checked = true;
            this.chkExternalAnims.CheckOnClick = true;
            this.chkExternalAnims.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExternalAnims.Name = "chkExternalAnims";
            this.chkExternalAnims.Size = new System.Drawing.Size(299, 26);
            this.chkExternalAnims.Text = "Display external animations";
            this.chkExternalAnims.CheckedChanged += new System.EventHandler(this.UpdateAnimList_Event);
            // 
            // chkBRRESAnims
            // 
            this.chkBRRESAnims.Checked = true;
            this.chkBRRESAnims.CheckOnClick = true;
            this.chkBRRESAnims.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBRRESAnims.Name = "chkBRRESAnims";
            this.chkBRRESAnims.Size = new System.Drawing.Size(299, 26);
            this.chkBRRESAnims.Text = "Display animations in BRRES";
            this.chkBRRESAnims.CheckedChanged += new System.EventHandler(this.UpdateAnimList_Event);
            // 
            // chkNonBRRESAnims
            // 
            this.chkNonBRRESAnims.Checked = true;
            this.chkNonBRRESAnims.CheckOnClick = true;
            this.chkNonBRRESAnims.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNonBRRESAnims.Name = "chkNonBRRESAnims";
            this.chkNonBRRESAnims.Size = new System.Drawing.Size(299, 26);
            this.chkNonBRRESAnims.Text = "Display animations not in BRRES";
            this.chkNonBRRESAnims.CheckedChanged += new System.EventHandler(this.UpdateAnimList_Event);
            // 
            // kinectToolStripMenuItem
            // 
            this.kinectToolStripMenuItem.Name = "kinectToolStripMenuItem";
            this.kinectToolStripMenuItem.Size = new System.Drawing.Size(12, 24);
            // 
            // syncKinectToolStripMenuItem
            // 
            this.syncKinectToolStripMenuItem.Name = "syncKinectToolStripMenuItem";
            this.syncKinectToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // notYetImplementedToolStripMenuItem
            // 
            this.notYetImplementedToolStripMenuItem.Name = "notYetImplementedToolStripMenuItem";
            this.notYetImplementedToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // startTrackingToolStripMenuItem
            // 
            this.startTrackingToolStripMenuItem.Name = "startTrackingToolStripMenuItem";
            this.startTrackingToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // models
            // 
            this.models.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.models.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.models.FormattingEnabled = true;
            this.models.Items.AddRange(new object[] {
            "All"});
            this.models.Location = new System.Drawing.Point(349, 1);
            this.models.Name = "models";
            this.models.Size = new System.Drawing.Size(115, 24);
            this.models.TabIndex = 21;
            this.models.SelectedIndexChanged += new System.EventHandler(this.models_SelectedIndexChanged);
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.splitter1);
            this.controlPanel.Controls.Add(this.toolStrip1);
            this.controlPanel.Controls.Add(this.panel2);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(805, 26);
            this.controlPanel.TabIndex = 22;
            this.controlPanel.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(464, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 26);
            this.splitter1.TabIndex = 31;
            this.splitter1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkBones,
            this.chkPolygons,
            this.chkVertices,
            this.chkCollisions,
            this.dropdownOverlays,
            this.toolStripSeparator1,
            this.chkFloor,
            this.button1,
            this.chkZoomExtents,
            this.btnSaveCam,
            this.toolStripSeparator2,
            this.cboToolSelect});
            this.toolStrip1.Location = new System.Drawing.Point(464, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.toolStrip1.Size = new System.Drawing.Size(341, 26);
            this.toolStrip1.TabIndex = 30;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // chkBones
            // 
            this.chkBones.Checked = true;
            this.chkBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBones.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkBones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkBones.Name = "chkBones";
            this.chkBones.Size = new System.Drawing.Size(53, 23);
            this.chkBones.Text = "Bones";
            this.chkBones.Click += new System.EventHandler(this.toggleRenderBones_Event);
            // 
            // chkPolygons
            // 
            this.chkPolygons.Checked = true;
            this.chkPolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPolygons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkPolygons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkPolygons.Name = "chkPolygons";
            this.chkPolygons.Size = new System.Drawing.Size(72, 23);
            this.chkPolygons.Text = "Polygons";
            this.chkPolygons.Click += new System.EventHandler(this.toggleRenderPolygons_Event);
            // 
            // chkVertices
            // 
            this.chkVertices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkVertices.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkVertices.Name = "chkVertices";
            this.chkVertices.Size = new System.Drawing.Size(64, 23);
            this.chkVertices.Text = "Vertices";
            this.chkVertices.Click += new System.EventHandler(this.toggleRenderVertices_Event);
            // 
            // chkCollisions
            // 
            this.chkCollisions.Checked = true;
            this.chkCollisions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCollisions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkCollisions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkCollisions.Name = "chkCollisions";
            this.chkCollisions.Size = new System.Drawing.Size(76, 21);
            this.chkCollisions.Text = "Collisions";
            this.chkCollisions.Visible = false;
            this.chkCollisions.Click += new System.EventHandler(this.toggleRenderCollisions_Event);
            // 
            // dropdownOverlays
            // 
            this.dropdownOverlays.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.dropdownOverlays.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkBoundaries,
            this.chkSpawns,
            this.chkItems});
            this.dropdownOverlays.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownOverlays.Name = "dropdownOverlays";
            this.dropdownOverlays.Size = new System.Drawing.Size(79, 23);
            this.dropdownOverlays.Text = "Overlays";
            // 
            // chkBoundaries
            // 
            this.chkBoundaries.CheckOnClick = true;
            this.chkBoundaries.Name = "chkBoundaries";
            this.chkBoundaries.Size = new System.Drawing.Size(206, 26);
            this.chkBoundaries.Text = "Boundaries";
            this.chkBoundaries.Click += new System.EventHandler(this.chkBoundaries_Click);
            // 
            // chkSpawns
            // 
            this.chkSpawns.CheckOnClick = true;
            this.chkSpawns.Name = "chkSpawns";
            this.chkSpawns.Size = new System.Drawing.Size(206, 26);
            this.chkSpawns.Text = "Spawn/Respawns";
            this.chkSpawns.Click += new System.EventHandler(this.chkBoundaries_Click);
            // 
            // chkItems
            // 
            this.chkItems.CheckOnClick = true;
            this.chkItems.Name = "chkItems";
            this.chkItems.Size = new System.Drawing.Size(206, 26);
            this.chkItems.Text = "Item Spawn Zones";
            this.chkItems.Click += new System.EventHandler(this.chkBoundaries_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // chkFloor
            // 
            this.chkFloor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkFloor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkFloor.Name = "chkFloor";
            this.chkFloor.Size = new System.Drawing.Size(47, 24);
            this.chkFloor.Text = "Floor";
            this.chkFloor.Click += new System.EventHandler(this.toggleRenderFloor_Event);
            // 
            // button1
            // 
            this.button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 24);
            this.button1.Text = "Reset Camera";
            this.button1.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click_1);
            // 
            // chkZoomExtents
            // 
            this.chkZoomExtents.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkZoomExtents.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkZoomExtents.Name = "chkZoomExtents";
            this.chkZoomExtents.Size = new System.Drawing.Size(104, 24);
            this.chkZoomExtents.Text = "Zoom Extents";
            this.chkZoomExtents.Click += new System.EventHandler(this.chkZoomExtents_Click);
            // 
            // btnSaveCam
            // 
            this.btnSaveCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveCam.Name = "btnSaveCam";
            this.btnSaveCam.Size = new System.Drawing.Size(99, 24);
            this.btnSaveCam.Text = "Save Camera";
            this.btnSaveCam.Click += new System.EventHandler(this.btnSaveCam_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // cboToolSelect
            // 
            this.cboToolSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToolSelect.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cboToolSelect.Items.AddRange(new object[] {
            "Translation",
            "Rotation",
            "Scale",
            "None"});
            this.cboToolSelect.Name = "cboToolSelect";
            this.cboToolSelect.Size = new System.Drawing.Size(121, 28);
            this.cboToolSelect.SelectedIndexChanged += new System.EventHandler(this.cboToolSelect_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.models);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(464, 26);
            this.panel2.TabIndex = 29;
            // 
            // spltRight
            // 
            this.spltRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.spltRight.Location = new System.Drawing.Point(601, 26);
            this.spltRight.Name = "spltRight";
            this.spltRight.Size = new System.Drawing.Size(4, 389);
            this.spltRight.TabIndex = 23;
            this.spltRight.TabStop = false;
            this.spltRight.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.modelPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(189, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(397, 359);
            this.panel1.TabIndex = 25;
            // 
            // modelPanel
            // 
            this.modelPanel.BackColor = System.Drawing.Color.Lavender;
            modelPanelViewport1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            modelPanelViewport1.BackgroundImage = null;
            modelPanelViewport1.BackgroundImageType = BrawlLib.OpenGL.BGImageType.Stretch;
            glCamera1.Aspect = 1.10585F;
            glCamera1.FarDepth = 200000F;
            glCamera1.Height = 359F;
            glCamera1.NearDepth = 1F;
            glCamera1.Orthographic = false;
            glCamera1.VerticalFieldOfView = 45F;
            glCamera1.Width = 397F;
            modelPanelViewport1.Camera = glCamera1;
            modelPanelViewport1.Enabled = true;
            modelPanelViewport1.Region = new System.Drawing.Rectangle(0, 0, 397, 359);
            modelPanelViewport1.RotationScale = 0.4F;
            modelPanelViewport1.TranslationScale = 0.05F;
            modelPanelViewport1.ViewType = BrawlLib.OpenGL.ViewportProjection.Perspective;
            modelPanelViewport1.ZoomScale = 2.5F;
            this.modelPanel.CurrentViewport = modelPanelViewport1;
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.Location = new System.Drawing.Point(0, 0);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(397, 359);
            this.modelPanel.TabIndex = 0;
            this.modelPanel.RenderFloorChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_RenderFloorChanged);
            this.modelPanel.FirstPersonCameraChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_FirstPersonCameraChanged);
            this.modelPanel.RenderBonesChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_RenderBonesChanged);
            this.modelPanel.RenderModelBoxChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_RenderModelBoxChanged);
            this.modelPanel.RenderObjectBoxChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_RenderObjectBoxChanged);
            this.modelPanel.RenderVisBoneBoxChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_RenderVisBoneBoxChanged);
            this.modelPanel.RenderOffscreenChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_RenderOffscreenChanged);
            this.modelPanel.RenderVerticesChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.ModelPanel_RenderVerticesChanged);
            this.modelPanel.RenderNormalsChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.modelPanel_RenderNormalsChanged);
            this.modelPanel.RenderPolygonsChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.ModelPanel_RenderPolygonsChanged);
            this.modelPanel.RenderWireframeChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.ModelPanel_RenderWireframeChanged);
            this.modelPanel.UseBindStateBoxesChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.ModelPanel_UseBindStateBoxesChanged);
            this.modelPanel.ApplyBillboardBonesChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.ModelPanel_ApplyBillboardBonesChanged);
            this.modelPanel.RenderShadersChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.ModelPanel_RenderShadersChanged);
            this.modelPanel.ScaleBonesChanged += new System.Windows.Forms.ModelPanel.RenderStateEvent(this.ModelPanel_ScaleBonesChanged);
            this.modelPanel.OnCurrentViewportChanged += new BrawlLib.OpenGL.ViewportAction(this.modelPanel_OnCurrentViewportChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Open";
            // 
            // animEditors
            // 
            this.animEditors.AutoScroll = true;
            this.animEditors.Controls.Add(this.pnlPlayback);
            this.animEditors.Controls.Add(this.animCtrlPnl);
            this.animEditors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.animEditors.Location = new System.Drawing.Point(0, 415);
            this.animEditors.Name = "animEditors";
            this.animEditors.Size = new System.Drawing.Size(805, 60);
            this.animEditors.TabIndex = 29;
            this.animEditors.Visible = false;
            // 
            // pnlPlayback
            // 
            this.pnlPlayback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlayback.Enabled = false;
            this.pnlPlayback.Location = new System.Drawing.Point(264, 0);
            this.pnlPlayback.MinimumSize = new System.Drawing.Size(290, 54);
            this.pnlPlayback.Name = "pnlPlayback";
            this.pnlPlayback.Size = new System.Drawing.Size(541, 60);
            this.pnlPlayback.TabIndex = 15;
            // 
            // animCtrlPnl
            // 
            this.animCtrlPnl.AutoScroll = true;
            this.animCtrlPnl.Controls.Add(this.vis0Editor);
            this.animCtrlPnl.Controls.Add(this.pat0Editor);
            this.animCtrlPnl.Controls.Add(this.shp0Editor);
            this.animCtrlPnl.Controls.Add(this.srt0Editor);
            this.animCtrlPnl.Controls.Add(this.chr0Editor);
            this.animCtrlPnl.Controls.Add(this.scn0Editor);
            this.animCtrlPnl.Controls.Add(this.clr0Editor);
            this.animCtrlPnl.Controls.Add(this.weightEditor);
            this.animCtrlPnl.Controls.Add(this.vertexEditor);
            this.animCtrlPnl.Dock = System.Windows.Forms.DockStyle.Left;
            this.animCtrlPnl.Location = new System.Drawing.Point(0, 0);
            this.animCtrlPnl.Name = "animCtrlPnl";
            this.animCtrlPnl.Size = new System.Drawing.Size(264, 60);
            this.animCtrlPnl.TabIndex = 29;
            // 
            // vis0Editor
            // 
            this.vis0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vis0Editor.Location = new System.Drawing.Point(0, 0);
            this.vis0Editor.MinimumSize = new System.Drawing.Size(210, 55);
            this.vis0Editor.Name = "vis0Editor";
            this.vis0Editor.Padding = new System.Windows.Forms.Padding(4);
            this.vis0Editor.Size = new System.Drawing.Size(264, 60);
            this.vis0Editor.TabIndex = 26;
            this.vis0Editor.Visible = false;
            // 
            // pat0Editor
            // 
            this.pat0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pat0Editor.Location = new System.Drawing.Point(0, 0);
            this.pat0Editor.MinimumSize = new System.Drawing.Size(402, 77);
            this.pat0Editor.Name = "pat0Editor";
            this.pat0Editor.Size = new System.Drawing.Size(402, 77);
            this.pat0Editor.TabIndex = 27;
            this.pat0Editor.Visible = false;
            // 
            // shp0Editor
            // 
            this.shp0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shp0Editor.Location = new System.Drawing.Point(0, 0);
            this.shp0Editor.MinimumSize = new System.Drawing.Size(533, 106);
            this.shp0Editor.Name = "shp0Editor";
            this.shp0Editor.Size = new System.Drawing.Size(533, 106);
            this.shp0Editor.TabIndex = 28;
            this.shp0Editor.Visible = false;
            // 
            // srt0Editor
            // 
            this.srt0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.srt0Editor.Location = new System.Drawing.Point(0, 0);
            this.srt0Editor.MinimumSize = new System.Drawing.Size(483, 78);
            this.srt0Editor.Name = "srt0Editor";
            this.srt0Editor.Size = new System.Drawing.Size(483, 78);
            this.srt0Editor.TabIndex = 20;
            this.srt0Editor.Visible = false;
            // 
            // chr0Editor
            // 
            this.chr0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chr0Editor.Location = new System.Drawing.Point(0, 0);
            this.chr0Editor.MinimumSize = new System.Drawing.Size(582, 78);
            this.chr0Editor.Name = "chr0Editor";
            this.chr0Editor.Size = new System.Drawing.Size(582, 78);
            this.chr0Editor.TabIndex = 19;
            this.chr0Editor.Visible = false;
            // 
            // scn0Editor
            // 
            this.scn0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scn0Editor.Location = new System.Drawing.Point(0, 0);
            this.scn0Editor.Name = "scn0Editor";
            this.scn0Editor.Size = new System.Drawing.Size(264, 60);
            this.scn0Editor.TabIndex = 30;
            this.scn0Editor.Visible = false;
            // 
            // clr0Editor
            // 
            this.clr0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clr0Editor.Location = new System.Drawing.Point(0, 0);
            this.clr0Editor.MinimumSize = new System.Drawing.Size(256, 40);
            this.clr0Editor.Name = "clr0Editor";
            this.clr0Editor.Size = new System.Drawing.Size(264, 60);
            this.clr0Editor.TabIndex = 30;
            this.clr0Editor.Visible = false;
            // 
            // weightEditor
            // 
            this.weightEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weightEditor.Location = new System.Drawing.Point(0, 0);
            this.weightEditor.MinimumSize = new System.Drawing.Size(260, 103);
            this.weightEditor.Name = "weightEditor";
            this.weightEditor.Size = new System.Drawing.Size(264, 103);
            this.weightEditor.TabIndex = 31;
            this.weightEditor.Visible = false;
            // 
            // vertexEditor
            // 
            this.vertexEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vertexEditor.Enabled = false;
            this.vertexEditor.Location = new System.Drawing.Point(0, 0);
            this.vertexEditor.MinimumSize = new System.Drawing.Size(230, 85);
            this.vertexEditor.Name = "vertexEditor";
            this.vertexEditor.Size = new System.Drawing.Size(264, 85);
            this.vertexEditor.TabIndex = 32;
            this.vertexEditor.Visible = false;
            // 
            // rightPanel
            // 
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(605, 26);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(200, 389);
            this.rightPanel.TabIndex = 32;
            this.rightPanel.Visible = false;
            // 
            // leftPanel
            // 
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 26);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(170, 389);
            this.leftPanel.TabIndex = 4;
            this.leftPanel.Visible = false;
            // 
            // ModelEditControl
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnTopToggle);
            this.Controls.Add(this.btnBottomToggle);
            this.Controls.Add(this.btnRightToggle);
            this.Controls.Add(this.spltRight);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.btnLeftToggle);
            this.Controls.Add(this.spltLeft);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.animEditors);
            this.Name = "ModelEditControl";
            this.Size = new System.Drawing.Size(805, 475);
            this.SizeChanged += new System.EventHandler(this.ModelEditControl_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.animEditors.ResumeLayout(false);
            this.animCtrlPnl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Initialization

        public ModelEditControl()
        {
            if (!Instances.Contains(this))
                Instances.Add(this);

            InitializeComponent();

            leftPanel._mainWindow = this;
            rightPanel.pnlKeyframes._mainWindow =
            rightPanel.pnlBones._mainWindow =
            rightPanel.pnlOpenedFiles._mainWindow =
            weightEditor._mainWindow =
            vertexEditor._mainWindow =
            srt0Editor._mainWindow =
            shp0Editor._mainWindow =
            pat0Editor._mainWindow =
            vis0Editor._mainWindow =
            scn0Editor._mainWindow =
            clr0Editor._mainWindow =
            chr0Editor._mainWindow =
            pnlPlayback._mainWindow =
            this;

            PreConstruct();

            ModelPanel.DrawCallSort = rightPanel.pnlOpenedFiles.Sort;

            leftPanel.fileType.DataSource = _editableAnimTypes;
            TargetAnimType = NW4RAnimType.CHR;

            animEditors.HorizontalScroll.Enabled = (!(animEditors.Width - animCtrlPnl.Width >= pnlPlayback.MinimumSize.Width));

            string applicationFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            ScreenCapBgLocText.Text = applicationFolder + "\\ScreenCaptures";
            MDL0TextureNode.TextureOverrideDirectory = 
                LiveTextureFolderPath.Text = 
                applicationFolder;

            _openFileDelegate = new DelegateOpenFile(OpenFile);

            models.DataSource = ModelPanel._renderList;

            PostConstruct();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            panel1.Controls.Add(_interpolationEditor);
            _interpolationEditor.SendToBack();
            _interpolationEditor.Dock = DockStyle.Fill;
            _interpolationEditor.Visible = false;

            cboToolSelect.SelectedIndex = 1;
            chkZoomExtents.Enabled = false;

            _currentProjBox = perspectiveToolStripMenuItem;
            
            shadersToolStripMenuItem.Enabled = TKContext._shadersSupported;

            rightPanel.pnlOpenedFiles.listBox1.DataSource = _openedFiles;
        }

        #endregion

        public static List<ModelEditControl> Instances = new List<ModelEditControl>();

        private void removeCurrentViewportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelPanel.RemoveViewport(ModelPanel.CurrentViewport);
        }

        public void btnLoadAnimations_Click(object sender, EventArgs e)
        {
            rightPanel.pnlOpenedFiles.LoadExternal(false, true, false);
        }
        public void btnSave_Click(object sender, EventArgs e)
        {
            pnlAnimSave(false);
        }
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            pnlAnimSave(true);
        }

        private void pnlAnimSave(bool As)
        {
            ResourceNode o = null;
            if (TargetModel != null)
                o = ((ResourceNode)TargetModel).RootNode;
            else
                o = rightPanel.pnlOpenedFiles.SelectedFile;
            rightPanel.pnlOpenedFiles.SaveExternal(o, As);
        }

        public void AppendTarget(CollisionNode collision)
        {
            if (!_collisions.Contains(collision))
                _collisions.Add(collision);

            foreach (CollisionObject o in collision._objects)
                o._render = true;

            chkCollisions.Visible = _collisions.Count > 0;
        }

        public override void LoadModels(ResourceNode node)
        {
            base.LoadModels(node);

            models.SelectedItem = TargetModel;
        }

        public override void LoadAnimations(ResourceNode node)
        {
            leftPanel.LoadAnimations(node);
        }

        private void RemoveAnimGroup(string nameCompare)
        {
            for (int i = 0; i < leftPanel.listAnims.Groups.Count; i++)
            {
                var x = leftPanel.listAnims.Groups[i];
                if (x.ToString().Contains(nameCompare))
                {
                    for (int r = 0; r < x.Items.Count; r++)
                        leftPanel.listAnims.Items.Remove(x.Items[r--]);
                    leftPanel.listAnims.Groups.RemoveAt(i--);
                }
            }
        }

        public override void UnloadAnimations(ResourceNode r)
        {
            //leftPanel.UpdateAnimations();
            RemoveAnimGroup(r.RootNode.Name);
        }

        public override void LoadEtc(ResourceNode node)
        {
            
        }

        public override void OpenInMainForm(ResourceNode node)
        {
            Program.RootNode = node;
        }

        public override bool ShouldCloseFile(ResourceNode node)
        {
            return Program.RootNode != node;
        }

        private void SLocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[2] = CoordinateType.Local;
            UpdateCoordinateCheckboxes();
        }

        private void SWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[2] = CoordinateType.World;
            UpdateCoordinateCheckboxes();
        }

        private void SCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[2] = CoordinateType.Screen;
            UpdateCoordinateCheckboxes();
        }

        private void RLocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[1] = CoordinateType.Local;
            UpdateCoordinateCheckboxes();
        }

        private void RWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[1] = CoordinateType.World;
            UpdateCoordinateCheckboxes();
        }

        private void RCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[1] = CoordinateType.Screen;
            UpdateCoordinateCheckboxes();
        }

        private void TLocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[0] = CoordinateType.Local;
            UpdateCoordinateCheckboxes();
        }

        private void TWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[0] = CoordinateType.World;
            UpdateCoordinateCheckboxes();
        }

        private void TCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _coordinateTypes[0] = CoordinateType.Screen;
            UpdateCoordinateCheckboxes();
        }

        private void UpdateCoordinateCheckboxes()
        {
            TLocalToolStripMenuItem.Checked = _coordinateTypes[0] == CoordinateType.Local;
            TWorldToolStripMenuItem.Checked = _coordinateTypes[0] == CoordinateType.World;
            TCameraToolStripMenuItem.Checked = _coordinateTypes[0] == CoordinateType.Screen;

            RLocalToolStripMenuItem.Checked = _coordinateTypes[1] == CoordinateType.Local;
            RWorldToolStripMenuItem.Checked = _coordinateTypes[1] == CoordinateType.World;
            RCameraToolStripMenuItem.Checked = _coordinateTypes[1] == CoordinateType.Screen;

            SLocalToolStripMenuItem.Checked = _coordinateTypes[2] == CoordinateType.Local;
            SWorldToolStripMenuItem.Checked = _coordinateTypes[2] == CoordinateType.World;
            SCameraToolStripMenuItem.Checked = _coordinateTypes[2] == CoordinateType.Screen;

            ModelPanel.Invalidate();
        }

        private void afterRotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _translateAfterRotation = afterRotationToolStripMenuItem.Checked = !afterRotationToolStripMenuItem.Checked;
            ModelPanel.Invalidate();
        }

        private void sCN0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelPanel.CurrentViewport.RenderSCN0Controls = (sCN0ToolStripMenuItem.Checked = !sCN0ToolStripMenuItem.Checked);
        }

        protected override void modelPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left && 
                !_vertexSelection.IsMoving())
            {
                weightEditor.TargetVertices = _selectedVertices;
                vertexEditor.TargetVertices = _selectedVertices;
            }

            base.modelPanel1_MouseUp(sender, e);
        }

        private void btnWeightEditor_Click(object sender, EventArgs e)
        {
            ToggleWeightEditor();
        }

        private void btnVertexEditor_Click(object sender, EventArgs e)
        {
            ToggleVertexEditor();
        }
    }
}

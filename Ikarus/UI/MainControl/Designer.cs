using System;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.Drawing;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;
using System.Windows.Forms;
using Ikarus.ModelViewer;
using Ikarus.MovesetFile;

namespace Ikarus.UI
{
    public partial class MainControl : ModelEditorBase
    {
        #region Designer
        private ModelPanel modelPanel;
        private ColorDialog dlgColor;
        private IContainer components;
        private Button btnBottomToggle;
        private Splitter spltLeft;
        private Button btnTopToggle;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem newSceneToolStripMenuItem;
        private ToolStripMenuItem kinectToolStripMenuItem1;
        private ToolStripMenuItem btnUndo;
        private ToolStripMenuItem btnRedo;
        private ToolStripMenuItem backColorToolStripMenuItem;
        private ToolStripMenuItem modelToolStripMenuItem;
        private ToolStripMenuItem toggleBones;
        private ToolStripMenuItem togglePolygons;
        private ToolStripMenuItem movesetToolStripMenuItem1;
        private ToolStripMenuItem hitboxesOffToolStripMenuItem;
        private ToolStripMenuItem hurtboxesOffToolStripMenuItem;
        private ToolStripMenuItem modifyLightingToolStripMenuItem;
        private ToolStripMenuItem toggleFloor;
        private ToolStripMenuItem resetCameraToolStripMenuItem;
        private ToolStripMenuItem editorsToolStripMenuItem;
        private ToolStripMenuItem showLeft;
        private ToolStripMenuItem showOptions;
        private ToolStripMenuItem showRight;
        public CHR0Editor chr0Editor;
        public ComboBox comboCharacters;
        private Panel controlPanel;
        public SRT0Editor srt0Editor;
        private ToolStripMenuItem fileTypesToolStripMenuItem;
        public ToolStripMenuItem playCHR0ToolStripMenuItem;
        public ToolStripMenuItem playSRT0ToolStripMenuItem;
        public ToolStripMenuItem playSHP0ToolStripMenuItem;
        public ToolStripMenuItem playPAT0ToolStripMenuItem;
        public ToolStripMenuItem playVIS0ToolStripMenuItem;
        public VIS0Editor vis0Editor;
        public PAT0Editor pat0Editor;
        public SHP0Editor shp0Editor;
        public Panel animEditors;
        private ToolStrip toolStrip1;
        private ToolStripButton chkHitboxes;
        private Panel panel2;
        private ToolStripButton chkHurtboxes;
        private ToolStripButton chkBones;
        private ToolStripButton chkPolygons;
        private ToolStripButton chkFloor;
        private ToolStripButton button1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator1;
        private Splitter splitter1;
        public Panel panel3;
        private ToolStripButton chkShaders;
        public ToolStripButton btnSaveCam;
        public ToolStripMenuItem showCameraCoordinatesToolStripMenuItem;
        private ToolStripMenuItem sCN0ToolStripMenuItem;
        private ToolStripMenuItem displayAmbienceToolStripMenuItem;
        private ToolStripMenuItem displayLightsToolStripMenuItem;
        private ToolStripMenuItem displayFogToolStripMenuItem;
        private ToolStripMenuItem displayCameraToolStripMenuItem;
        private ToolStripMenuItem displayToolStripMenuItem;
        private ToolStripMenuItem stPersonToolStripMenuItem;
        private ToolStripMenuItem editControlToolStripMenuItem;
        private ToolStripMenuItem rotationToolStripMenuItem;
        private ToolStripMenuItem translationToolStripMenuItem;
        private ToolStripMenuItem scaleToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private CLR0Editor clr0Editor;
        public ToolStripMenuItem playCLR0ToolStripMenuItem;
        private ToolStripMenuItem detachViewerToolStripMenuItem;
        private ToolStripMenuItem backgroundToolStripMenuItem;
        private ToolStripMenuItem setColorToolStripMenuItem;
        private ToolStripMenuItem loadImageToolStripMenuItem;
        private ToolStripMenuItem takeScreenshotToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        public ToolStripMenuItem displayFrameCountDifferencesToolStripMenuItem;
        public ToolStripMenuItem alwaysSyncFrameCountsToolStripMenuItem;
        public ToolStripMenuItem syncAnimationsTogetherToolStripMenuItem;
        public ToolStripMenuItem syncTexObjToolStripMenuItem;
        public ToolStripMenuItem syncObjectsListToVIS0ToolStripMenuItem;
        public ToolStripMenuItem disableBonesWhenPlayingToolStripMenuItem;
        public ToolStripMenuItem syncLoopToAnimationToolStripMenuItem;
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
        private ToolStripMenuItem boundingBoxToolStripMenuItem;
        private ToolStripMenuItem chkDontRenderOffscreen;
        private ToolStripMenuItem saveCurrentSettingsToolStripMenuItem;
        public ToolStripMenuItem clearSavedSettingsToolStripMenuItem;
        public ToolStripMenuItem storeSettingsExternallyToolStripMenuItem;
        private ToolStripMenuItem dontHighlightBonesAndVerticesToolStripMenuItem;
        public ToolStripMenuItem enablePointAndLineSmoothingToolStripMenuItem;
        public ToolStripMenuItem enableTextOverlaysToolStripMenuItem;
        private ToolStripMenuItem btnLoadChar;
        private ToolStripMenuItem btnLoadRoot;
        private ToolStripMenuItem pathToolStripMenuItem;
        private ToolStripMenuItem showAnim;
        public ListsPanel listPanel;
        public System.Windows.Forms.ModelPlaybackPanel pnlPlayback;
        public EditorPanel scriptPanel;
        private HurtboxEditor hurtboxEditor;
        private ToolStripMenuItem muteSFXToolStripMenuItem;
        public ComboBox fileType;
        private Panel panel1;
        private Splitter splitter2;
        private ModelListsPanel modelListsPanel1;
        private Panel panel4;
        private Splitter spltRight;
        private Button button3;
        private Button button2;
        private ToolStripMenuItem saveAllFilesToolStripMenuItem;
        private ToolStripMenuItem viewOpenedFilesToolStripMenuItem;
        private ToolStripMenuItem viewLogToolStripMenuItem;
        private ToolStripMenuItem saveTextInfoToolStripMenuItem;
        public ComboBox comboMdl;

        private void InitializeComponent()
        {
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.btnBottomToggle = new System.Windows.Forms.Button();
            this.spltLeft = new System.Windows.Forms.Splitter();
            this.btnTopToggle = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.pathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadChar = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTextInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.modifyLightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayFrameCountDifferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysSyncFrameCountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncAnimationsTogetherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncTexObjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncObjectsListToVIS0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableBonesWhenPlayingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncLoopToAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkDontRenderOffscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.dontHighlightBonesAndVerticesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enablePointAndLineSmoothingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableTextOverlaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteSFXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.storeSettingsExternallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCurrentSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSavedSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kinectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.showLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.showRight = new System.Windows.Forms.ToolStripMenuItem();
            this.showAnim = new System.Windows.Forms.ToolStripMenuItem();
            this.detachViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displaySettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.centerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perspectiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orthographicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleFloor = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCameraCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleBones = new System.Windows.Forms.ToolStripMenuItem();
            this.togglePolygons = new System.Windows.Forms.ToolStripMenuItem();
            this.boundingBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movesetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hitboxesOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hurtboxesOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playCHR0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSRT0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSHP0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playPAT0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playVIS0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playCLR0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOpenedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sCN0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAmbienceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayLightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayFogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stPersonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboCharacters = new System.Windows.Forms.ComboBox();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.chkHitboxes = new System.Windows.Forms.ToolStripButton();
            this.chkHurtboxes = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.chkBones = new System.Windows.Forms.ToolStripButton();
            this.chkPolygons = new System.Windows.Forms.ToolStripButton();
            this.chkShaders = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chkFloor = new System.Windows.Forms.ToolStripButton();
            this.button1 = new System.Windows.Forms.ToolStripButton();
            this.btnSaveCam = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fileType = new System.Windows.Forms.ComboBox();
            this.comboMdl = new System.Windows.Forms.ComboBox();
            this.modelPanel = new System.Windows.Forms.ModelPanel();
            this.animEditors = new System.Windows.Forms.Panel();
            this.pnlPlayback = new System.Windows.Forms.ModelPlaybackPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.vis0Editor = new System.Windows.Forms.VIS0Editor();
            this.pat0Editor = new System.Windows.Forms.PAT0Editor();
            this.shp0Editor = new System.Windows.Forms.SHP0Editor();
            this.srt0Editor = new System.Windows.Forms.SRT0Editor();
            this.chr0Editor = new System.Windows.Forms.CHR0Editor();
            this.clr0Editor = new System.Windows.Forms.CLR0Editor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.spltRight = new System.Windows.Forms.Splitter();
            this.modelListsPanel1 = new Ikarus.UI.ModelListsPanel();
            this.scriptPanel = new Ikarus.UI.EditorPanel();
            this.listPanel = new Ikarus.UI.ListsPanel();
            this.hurtboxEditor = new Ikarus.UI.HurtboxEditor();
            this.menuStrip1.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.animEditors.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            this.dlgColor.FullOpen = true;
            // 
            // btnBottomToggle
            // 
            this.btnBottomToggle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnBottomToggle.Location = new System.Drawing.Point(0, 495);
            this.btnBottomToggle.Name = "btnBottomToggle";
            this.btnBottomToggle.Size = new System.Drawing.Size(519, 18);
            this.btnBottomToggle.TabIndex = 8;
            this.btnBottomToggle.TabStop = false;
            this.btnBottomToggle.UseVisualStyleBackColor = false;
            this.btnBottomToggle.Click += new System.EventHandler(this.btnPlaybackToggle_Click);
            // 
            // spltLeft
            // 
            this.spltLeft.BackColor = System.Drawing.SystemColors.Control;
            this.spltLeft.Location = new System.Drawing.Point(228, 24);
            this.spltLeft.Name = "spltLeft";
            this.spltLeft.Size = new System.Drawing.Size(4, 513);
            this.spltLeft.TabIndex = 9;
            this.spltLeft.TabStop = false;
            // 
            // btnTopToggle
            // 
            this.btnTopToggle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTopToggle.Location = new System.Drawing.Point(0, 0);
            this.btnTopToggle.Name = "btnTopToggle";
            this.btnTopToggle.Size = new System.Drawing.Size(519, 18);
            this.btnTopToggle.TabIndex = 11;
            this.btnTopToggle.TabStop = false;
            this.btnTopToggle.UseVisualStyleBackColor = false;
            this.btnTopToggle.Click += new System.EventHandler(this.btnOptionToggle_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.kinectToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(150, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSceneToolStripMenuItem,
            this.saveAllFilesToolStripMenuItem,
            this.saveTextInfoToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSceneToolStripMenuItem
            // 
            this.newSceneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadRoot,
            this.btnLoadChar});
            this.newSceneToolStripMenuItem.Name = "newSceneToolStripMenuItem";
            this.newSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newSceneToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newSceneToolStripMenuItem.Text = "Load";
            // 
            // btnLoadRoot
            // 
            this.btnLoadRoot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pathToolStripMenuItem});
            this.btnLoadRoot.Name = "btnLoadRoot";
            this.btnLoadRoot.Size = new System.Drawing.Size(161, 22);
            this.btnLoadRoot.Text = "Root Folder";
            this.btnLoadRoot.Click += new System.EventHandler(this.btnLoadRoot_Click);
            // 
            // pathToolStripMenuItem
            // 
            this.pathToolStripMenuItem.Name = "pathToolStripMenuItem";
            this.pathToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.pathToolStripMenuItem.Text = "<path>";
            this.pathToolStripMenuItem.Click += new System.EventHandler(this.btnLoadRoot_Click);
            // 
            // btnLoadChar
            // 
            this.btnLoadChar.Name = "btnLoadChar";
            this.btnLoadChar.Size = new System.Drawing.Size(161, 22);
            this.btnLoadChar.Text = "Character Folder";
            this.btnLoadChar.Click += new System.EventHandler(this.btnLoadChar_Click);
            // 
            // saveAllFilesToolStripMenuItem
            // 
            this.saveAllFilesToolStripMenuItem.Name = "saveAllFilesToolStripMenuItem";
            this.saveAllFilesToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.saveAllFilesToolStripMenuItem.Text = "Save All Files";
            this.saveAllFilesToolStripMenuItem.Click += new System.EventHandler(this.saveAllFilesToolStripMenuItem_Click);
            // 
            // saveTextInfoToolStripMenuItem
            // 
            this.saveTextInfoToolStripMenuItem.Name = "saveTextInfoToolStripMenuItem";
            this.saveTextInfoToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.saveTextInfoToolStripMenuItem.Text = "Save Text Info";
            this.saveTextInfoToolStripMenuItem.Click += new System.EventHandler(this.saveTextInfoToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUndo,
            this.btnRedo,
            this.takeScreenshotToolStripMenuItem,
            this.modifyLightingToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.storeSettingsExternallyToolStripMenuItem,
            this.saveCurrentSettingsToolStripMenuItem,
            this.clearSavedSettingsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.editToolStripMenuItem.Text = "Options";
            // 
            // btnUndo
            // 
            this.btnUndo.Enabled = false;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.btnUndo.Size = new System.Drawing.Size(199, 22);
            this.btnUndo.Text = "Undo";
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Enabled = false;
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.btnRedo.Size = new System.Drawing.Size(199, 22);
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
            this.takeScreenshotToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.takeScreenshotToolStripMenuItem.Text = "Take Screenshot";
            // 
            // btnExportToImgNoTransparency
            // 
            this.btnExportToImgNoTransparency.Name = "btnExportToImgNoTransparency";
            this.btnExportToImgNoTransparency.ShortcutKeyDisplayString = "Ctrl+Shift+I";
            this.btnExportToImgNoTransparency.Size = new System.Drawing.Size(292, 22);
            this.btnExportToImgNoTransparency.Text = "With Background";
            this.btnExportToImgNoTransparency.Click += new System.EventHandler(this.btnExportToImgNoTransparency_Click);
            // 
            // btnExportToImgWithTransparency
            // 
            this.btnExportToImgWithTransparency.Name = "btnExportToImgWithTransparency";
            this.btnExportToImgWithTransparency.ShortcutKeyDisplayString = "Ctrl+Alt+I";
            this.btnExportToImgWithTransparency.Size = new System.Drawing.Size(292, 22);
            this.btnExportToImgWithTransparency.Text = "With Transparent Background";
            this.btnExportToImgWithTransparency.Click += new System.EventHandler(this.btnExportToImgWithTransparency_Click);
            // 
            // btnExportToAnimatedGIF
            // 
            this.btnExportToAnimatedGIF.Name = "btnExportToAnimatedGIF";
            this.btnExportToAnimatedGIF.Size = new System.Drawing.Size(292, 22);
            this.btnExportToAnimatedGIF.Text = "To Animated GIF";
            this.btnExportToAnimatedGIF.Click += new System.EventHandler(this.btnExportToAnimatedGIF_Click);
            // 
            // saveLocationToolStripMenuItem
            // 
            this.saveLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScreenCapBgLocText});
            this.saveLocationToolStripMenuItem.Name = "saveLocationToolStripMenuItem";
            this.saveLocationToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.saveLocationToolStripMenuItem.Text = "Save Location";
            // 
            // ScreenCapBgLocText
            // 
            this.ScreenCapBgLocText.Name = "ScreenCapBgLocText";
            this.ScreenCapBgLocText.Size = new System.Drawing.Size(110, 22);
            this.ScreenCapBgLocText.Text = "<null>";
            this.ScreenCapBgLocText.Click += new System.EventHandler(this.ScreenCapBgLocText_Click);
            // 
            // imageFormatToolStripMenuItem
            // 
            this.imageFormatToolStripMenuItem.Name = "imageFormatToolStripMenuItem";
            this.imageFormatToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.imageFormatToolStripMenuItem.Text = "Image Format: PNG";
            this.imageFormatToolStripMenuItem.Click += new System.EventHandler(this.imageFormatToolStripMenuItem_Click);
            // 
            // modifyLightingToolStripMenuItem
            // 
            this.modifyLightingToolStripMenuItem.Name = "modifyLightingToolStripMenuItem";
            this.modifyLightingToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.modifyLightingToolStripMenuItem.Text = "Viewer Settings";
            this.modifyLightingToolStripMenuItem.Click += new System.EventHandler(this.modifyLightingToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayFrameCountDifferencesToolStripMenuItem,
            this.alwaysSyncFrameCountsToolStripMenuItem,
            this.syncAnimationsTogetherToolStripMenuItem,
            this.syncTexObjToolStripMenuItem,
            this.syncObjectsListToVIS0ToolStripMenuItem,
            this.disableBonesWhenPlayingToolStripMenuItem,
            this.syncLoopToAnimationToolStripMenuItem,
            this.chkDontRenderOffscreen,
            this.dontHighlightBonesAndVerticesToolStripMenuItem,
            this.enablePointAndLineSmoothingToolStripMenuItem,
            this.enableTextOverlaysToolStripMenuItem,
            this.muteSFXToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // displayFrameCountDifferencesToolStripMenuItem
            // 
            this.displayFrameCountDifferencesToolStripMenuItem.CheckOnClick = true;
            this.displayFrameCountDifferencesToolStripMenuItem.Enabled = false;
            this.displayFrameCountDifferencesToolStripMenuItem.Name = "displayFrameCountDifferencesToolStripMenuItem";
            this.displayFrameCountDifferencesToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.displayFrameCountDifferencesToolStripMenuItem.Text = "Warn if frame counts differ";
            this.displayFrameCountDifferencesToolStripMenuItem.Visible = false;
            // 
            // alwaysSyncFrameCountsToolStripMenuItem
            // 
            this.alwaysSyncFrameCountsToolStripMenuItem.CheckOnClick = true;
            this.alwaysSyncFrameCountsToolStripMenuItem.Enabled = false;
            this.alwaysSyncFrameCountsToolStripMenuItem.Name = "alwaysSyncFrameCountsToolStripMenuItem";
            this.alwaysSyncFrameCountsToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.alwaysSyncFrameCountsToolStripMenuItem.Text = "Always sync frame counts";
            this.alwaysSyncFrameCountsToolStripMenuItem.Visible = false;
            // 
            // syncAnimationsTogetherToolStripMenuItem
            // 
            this.syncAnimationsTogetherToolStripMenuItem.Checked = true;
            this.syncAnimationsTogetherToolStripMenuItem.CheckOnClick = true;
            this.syncAnimationsTogetherToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.syncAnimationsTogetherToolStripMenuItem.Name = "syncAnimationsTogetherToolStripMenuItem";
            this.syncAnimationsTogetherToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.syncAnimationsTogetherToolStripMenuItem.Text = "Retrieve corresponding animations";
            this.syncAnimationsTogetherToolStripMenuItem.CheckedChanged += new System.EventHandler(this.syncAnimationsTogetherToolStripMenuItem_CheckedChanged);
            // 
            // syncTexObjToolStripMenuItem
            // 
            this.syncTexObjToolStripMenuItem.CheckOnClick = true;
            this.syncTexObjToolStripMenuItem.Name = "syncTexObjToolStripMenuItem";
            this.syncTexObjToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.syncTexObjToolStripMenuItem.Text = "Sync texture list with object list";
            this.syncTexObjToolStripMenuItem.CheckedChanged += new System.EventHandler(this.syncTexObjToolStripMenuItem_CheckedChanged);
            // 
            // syncObjectsListToVIS0ToolStripMenuItem
            // 
            this.syncObjectsListToVIS0ToolStripMenuItem.CheckOnClick = true;
            this.syncObjectsListToVIS0ToolStripMenuItem.Name = "syncObjectsListToVIS0ToolStripMenuItem";
            this.syncObjectsListToVIS0ToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.syncObjectsListToVIS0ToolStripMenuItem.Text = "Sync objects list edits to VIS0";
            this.syncObjectsListToVIS0ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.syncObjectsListToVIS0ToolStripMenuItem_CheckedChanged);
            // 
            // disableBonesWhenPlayingToolStripMenuItem
            // 
            this.disableBonesWhenPlayingToolStripMenuItem.Checked = true;
            this.disableBonesWhenPlayingToolStripMenuItem.CheckOnClick = true;
            this.disableBonesWhenPlayingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.disableBonesWhenPlayingToolStripMenuItem.Name = "disableBonesWhenPlayingToolStripMenuItem";
            this.disableBonesWhenPlayingToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.disableBonesWhenPlayingToolStripMenuItem.Text = "Disable bones when playing";
            // 
            // syncLoopToAnimationToolStripMenuItem
            // 
            this.syncLoopToAnimationToolStripMenuItem.Name = "syncLoopToAnimationToolStripMenuItem";
            this.syncLoopToAnimationToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.syncLoopToAnimationToolStripMenuItem.Text = "Sync loop checkbox to animation header(s)";
            // 
            // chkDontRenderOffscreen
            // 
            this.chkDontRenderOffscreen.CheckOnClick = true;
            this.chkDontRenderOffscreen.Enabled = false;
            this.chkDontRenderOffscreen.Name = "chkDontRenderOffscreen";
            this.chkDontRenderOffscreen.Size = new System.Drawing.Size(302, 22);
            this.chkDontRenderOffscreen.Text = "Don\'t render offscreen objects";
            this.chkDontRenderOffscreen.Visible = false;
            this.chkDontRenderOffscreen.CheckedChanged += new System.EventHandler(this.chkDontRenderOffscreen_CheckedChanged);
            // 
            // dontHighlightBonesAndVerticesToolStripMenuItem
            // 
            this.dontHighlightBonesAndVerticesToolStripMenuItem.CheckOnClick = true;
            this.dontHighlightBonesAndVerticesToolStripMenuItem.Name = "dontHighlightBonesAndVerticesToolStripMenuItem";
            this.dontHighlightBonesAndVerticesToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.dontHighlightBonesAndVerticesToolStripMenuItem.Text = "Don\'t highlight bones and vertices";
            // 
            // enablePointAndLineSmoothingToolStripMenuItem
            // 
            this.enablePointAndLineSmoothingToolStripMenuItem.CheckOnClick = true;
            this.enablePointAndLineSmoothingToolStripMenuItem.Name = "enablePointAndLineSmoothingToolStripMenuItem";
            this.enablePointAndLineSmoothingToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.enablePointAndLineSmoothingToolStripMenuItem.Text = "Enable point and line smoothing";
            this.enablePointAndLineSmoothingToolStripMenuItem.CheckedChanged += new System.EventHandler(this.enablePointAndLineSmoothingToolStripMenuItem_CheckedChanged);
            // 
            // enableTextOverlaysToolStripMenuItem
            // 
            this.enableTextOverlaysToolStripMenuItem.CheckOnClick = true;
            this.enableTextOverlaysToolStripMenuItem.Name = "enableTextOverlaysToolStripMenuItem";
            this.enableTextOverlaysToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.enableTextOverlaysToolStripMenuItem.Text = "Enable text overlays";
            this.enableTextOverlaysToolStripMenuItem.CheckedChanged += new System.EventHandler(this.enableTextOverlaysToolStripMenuItem_CheckedChanged);
            // 
            // muteSFXToolStripMenuItem
            // 
            this.muteSFXToolStripMenuItem.CheckOnClick = true;
            this.muteSFXToolStripMenuItem.Name = "muteSFXToolStripMenuItem";
            this.muteSFXToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.muteSFXToolStripMenuItem.Text = "Mute SFX";
            this.muteSFXToolStripMenuItem.CheckedChanged += new System.EventHandler(this.muteSFXToolStripMenuItem_CheckedChanged);
            // 
            // storeSettingsExternallyToolStripMenuItem
            // 
            this.storeSettingsExternallyToolStripMenuItem.CheckOnClick = true;
            this.storeSettingsExternallyToolStripMenuItem.Name = "storeSettingsExternallyToolStripMenuItem";
            this.storeSettingsExternallyToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.storeSettingsExternallyToolStripMenuItem.Text = "Store Settings Externally";
            this.storeSettingsExternallyToolStripMenuItem.CheckedChanged += new System.EventHandler(this.storeSettingsExternallyToolStripMenuItem_CheckedChanged);
            // 
            // saveCurrentSettingsToolStripMenuItem
            // 
            this.saveCurrentSettingsToolStripMenuItem.Name = "saveCurrentSettingsToolStripMenuItem";
            this.saveCurrentSettingsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.saveCurrentSettingsToolStripMenuItem.Text = "Save Current Settings";
            this.saveCurrentSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentSettingsToolStripMenuItem_Click);
            // 
            // clearSavedSettingsToolStripMenuItem
            // 
            this.clearSavedSettingsToolStripMenuItem.Name = "clearSavedSettingsToolStripMenuItem";
            this.clearSavedSettingsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.clearSavedSettingsToolStripMenuItem.Text = "Clear Saved Settings";
            this.clearSavedSettingsToolStripMenuItem.Click += new System.EventHandler(this.clearSavedSettingsToolStripMenuItem_Click);
            // 
            // kinectToolStripMenuItem1
            // 
            this.kinectToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorsToolStripMenuItem,
            this.backColorToolStripMenuItem,
            this.modelToolStripMenuItem,
            this.movesetToolStripMenuItem1,
            this.fileTypesToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.viewOpenedFilesToolStripMenuItem,
            this.viewLogToolStripMenuItem});
            this.kinectToolStripMenuItem1.Name = "kinectToolStripMenuItem1";
            this.kinectToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.kinectToolStripMenuItem1.Text = "View";
            // 
            // editorsToolStripMenuItem
            // 
            this.editorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOptions,
            this.showLeft,
            this.showRight,
            this.showAnim,
            this.detachViewerToolStripMenuItem});
            this.editorsToolStripMenuItem.Name = "editorsToolStripMenuItem";
            this.editorsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.editorsToolStripMenuItem.Text = "Panels";
            // 
            // showOptions
            // 
            this.showOptions.Checked = true;
            this.showOptions.CheckOnClick = true;
            this.showOptions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOptions.Name = "showOptions";
            this.showOptions.Size = new System.Drawing.Size(162, 22);
            this.showOptions.Text = "Menu Bar";
            this.showOptions.CheckedChanged += new System.EventHandler(this.showOptions_CheckedChanged);
            // 
            // showLeft
            // 
            this.showLeft.Checked = true;
            this.showLeft.CheckOnClick = true;
            this.showLeft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showLeft.Name = "showLeft";
            this.showLeft.Size = new System.Drawing.Size(162, 22);
            this.showLeft.Text = "Left Panel";
            this.showLeft.CheckedChanged += new System.EventHandler(this.showAssets_CheckedChanged);
            // 
            // showRight
            // 
            this.showRight.Checked = true;
            this.showRight.CheckOnClick = true;
            this.showRight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showRight.Name = "showRight";
            this.showRight.Size = new System.Drawing.Size(162, 22);
            this.showRight.Text = "Right Panel";
            this.showRight.CheckedChanged += new System.EventHandler(this.showMoveset_CheckedChanged);
            this.showRight.Click += new System.EventHandler(this.showMoveset_Click_1);
            // 
            // showAnim
            // 
            this.showAnim.Checked = true;
            this.showAnim.CheckOnClick = true;
            this.showAnim.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAnim.Name = "showAnim";
            this.showAnim.Size = new System.Drawing.Size(162, 22);
            this.showAnim.Text = "Animation Panel";
            this.showAnim.CheckedChanged += new System.EventHandler(this.showPlay_CheckedChanged);
            // 
            // detachViewerToolStripMenuItem
            // 
            this.detachViewerToolStripMenuItem.Enabled = false;
            this.detachViewerToolStripMenuItem.Name = "detachViewerToolStripMenuItem";
            this.detachViewerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.detachViewerToolStripMenuItem.Text = "Detach Viewer";
            this.detachViewerToolStripMenuItem.Visible = false;
            this.detachViewerToolStripMenuItem.Click += new System.EventHandler(this.detachViewerToolStripMenuItem_Click);
            // 
            // backColorToolStripMenuItem
            // 
            this.backColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundToolStripMenuItem,
            this.editControlToolStripMenuItem,
            this.projectionToolStripMenuItem,
            this.toggleFloor,
            this.resetCameraToolStripMenuItem,
            this.showCameraCoordinatesToolStripMenuItem});
            this.backColorToolStripMenuItem.Name = "backColorToolStripMenuItem";
            this.backColorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.backColorToolStripMenuItem.Text = "Viewer";
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setColorToolStripMenuItem,
            this.loadImageToolStripMenuItem,
            this.displaySettingToolStripMenuItem});
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.backgroundToolStripMenuItem.Text = "Background";
            // 
            // setColorToolStripMenuItem
            // 
            this.setColorToolStripMenuItem.Name = "setColorToolStripMenuItem";
            this.setColorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setColorToolStripMenuItem.Text = "Set Color";
            this.setColorToolStripMenuItem.Click += new System.EventHandler(this.setColorToolStripMenuItem_Click);
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
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
            this.displaySettingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.displaySettingToolStripMenuItem.Text = "Display Setting";
            // 
            // stretchToolStripMenuItem1
            // 
            this.stretchToolStripMenuItem1.Checked = true;
            this.stretchToolStripMenuItem1.CheckOnClick = true;
            this.stretchToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stretchToolStripMenuItem1.Name = "stretchToolStripMenuItem1";
            this.stretchToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.stretchToolStripMenuItem1.Text = "Stretch";
            this.stretchToolStripMenuItem1.CheckedChanged += new System.EventHandler(this.stretchToolStripMenuItem_CheckedChanged);
            // 
            // centerToolStripMenuItem1
            // 
            this.centerToolStripMenuItem1.CheckOnClick = true;
            this.centerToolStripMenuItem1.Name = "centerToolStripMenuItem1";
            this.centerToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.centerToolStripMenuItem1.Text = "Center";
            this.centerToolStripMenuItem1.CheckedChanged += new System.EventHandler(this.centerToolStripMenuItem_CheckedChanged);
            // 
            // resizeToolStripMenuItem1
            // 
            this.resizeToolStripMenuItem1.CheckOnClick = true;
            this.resizeToolStripMenuItem1.Name = "resizeToolStripMenuItem1";
            this.resizeToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.resizeToolStripMenuItem1.Text = "Resize";
            this.resizeToolStripMenuItem1.CheckedChanged += new System.EventHandler(this.resizeToolStripMenuItem_CheckedChanged);
            // 
            // editControlToolStripMenuItem
            // 
            this.editControlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scaleToolStripMenuItem,
            this.rotationToolStripMenuItem,
            this.translationToolStripMenuItem});
            this.editControlToolStripMenuItem.Name = "editControlToolStripMenuItem";
            this.editControlToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.editControlToolStripMenuItem.Text = "Bone Control";
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.CheckOnClick = true;
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.ShortcutKeyDisplayString = "E Key";
            this.scaleToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.scaleToolStripMenuItem.Text = "Scale";
            this.scaleToolStripMenuItem.CheckedChanged += new System.EventHandler(this.scaleToolStripMenuItem_CheckedChanged);
            // 
            // rotationToolStripMenuItem
            // 
            this.rotationToolStripMenuItem.Checked = true;
            this.rotationToolStripMenuItem.CheckOnClick = true;
            this.rotationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotationToolStripMenuItem.Name = "rotationToolStripMenuItem";
            this.rotationToolStripMenuItem.ShortcutKeyDisplayString = "R Key";
            this.rotationToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.rotationToolStripMenuItem.Text = "Rotation";
            this.rotationToolStripMenuItem.CheckedChanged += new System.EventHandler(this.rotationToolStripMenuItem_CheckedChanged);
            // 
            // translationToolStripMenuItem
            // 
            this.translationToolStripMenuItem.CheckOnClick = true;
            this.translationToolStripMenuItem.Name = "translationToolStripMenuItem";
            this.translationToolStripMenuItem.ShortcutKeyDisplayString = "T Key";
            this.translationToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.translationToolStripMenuItem.Text = "Translation";
            this.translationToolStripMenuItem.CheckedChanged += new System.EventHandler(this.translationToolStripMenuItem_CheckedChanged);
            // 
            // projectionToolStripMenuItem
            // 
            this.projectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perspectiveToolStripMenuItem,
            this.orthographicToolStripMenuItem});
            this.projectionToolStripMenuItem.Enabled = false;
            this.projectionToolStripMenuItem.Name = "projectionToolStripMenuItem";
            this.projectionToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.projectionToolStripMenuItem.Text = "Projection";
            this.projectionToolStripMenuItem.Visible = false;
            // 
            // perspectiveToolStripMenuItem
            // 
            this.perspectiveToolStripMenuItem.Checked = true;
            this.perspectiveToolStripMenuItem.CheckOnClick = true;
            this.perspectiveToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.perspectiveToolStripMenuItem.Name = "perspectiveToolStripMenuItem";
            this.perspectiveToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.perspectiveToolStripMenuItem.Text = "Perspective";
            this.perspectiveToolStripMenuItem.CheckedChanged += new System.EventHandler(this.perspectiveToolStripMenuItem_CheckedChanged);
            // 
            // orthographicToolStripMenuItem
            // 
            this.orthographicToolStripMenuItem.CheckOnClick = true;
            this.orthographicToolStripMenuItem.Name = "orthographicToolStripMenuItem";
            this.orthographicToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.orthographicToolStripMenuItem.Text = "Orthographic";
            this.orthographicToolStripMenuItem.CheckedChanged += new System.EventHandler(this.orthographicToolStripMenuItem_CheckedChanged);
            // 
            // toggleFloor
            // 
            this.toggleFloor.Name = "toggleFloor";
            this.toggleFloor.ShortcutKeyDisplayString = "F Key";
            this.toggleFloor.Size = new System.Drawing.Size(214, 22);
            this.toggleFloor.Text = "Floor";
            this.toggleFloor.Click += new System.EventHandler(this.toggleFloor_Click);
            // 
            // resetCameraToolStripMenuItem
            // 
            this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
            this.resetCameraToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+R";
            this.resetCameraToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.resetCameraToolStripMenuItem.Text = "Reset Camera";
            this.resetCameraToolStripMenuItem.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click_1);
            // 
            // showCameraCoordinatesToolStripMenuItem
            // 
            this.showCameraCoordinatesToolStripMenuItem.CheckOnClick = true;
            this.showCameraCoordinatesToolStripMenuItem.Name = "showCameraCoordinatesToolStripMenuItem";
            this.showCameraCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.showCameraCoordinatesToolStripMenuItem.Text = "Show Camera Coordinates";
            this.showCameraCoordinatesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showCameraCoordinatesToolStripMenuItem_CheckedChanged);
            // 
            // modelToolStripMenuItem
            // 
            this.modelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleBones,
            this.togglePolygons,
            this.boundingBoxToolStripMenuItem});
            this.modelToolStripMenuItem.Name = "modelToolStripMenuItem";
            this.modelToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.modelToolStripMenuItem.Text = "Model";
            // 
            // toggleBones
            // 
            this.toggleBones.Checked = true;
            this.toggleBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleBones.Name = "toggleBones";
            this.toggleBones.ShortcutKeyDisplayString = "V Key";
            this.toggleBones.Size = new System.Drawing.Size(159, 22);
            this.toggleBones.Text = "Bones";
            this.toggleBones.Click += new System.EventHandler(this.toggleBonesToolStripMenuItem_Click);
            // 
            // togglePolygons
            // 
            this.togglePolygons.Checked = true;
            this.togglePolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.togglePolygons.Name = "togglePolygons";
            this.togglePolygons.ShortcutKeyDisplayString = "P Key";
            this.togglePolygons.Size = new System.Drawing.Size(159, 22);
            this.togglePolygons.Text = "Polygons";
            this.togglePolygons.Click += new System.EventHandler(this.togglePolygonsToolStripMenuItem_Click);
            // 
            // boundingBoxToolStripMenuItem
            // 
            this.boundingBoxToolStripMenuItem.CheckOnClick = true;
            this.boundingBoxToolStripMenuItem.Name = "boundingBoxToolStripMenuItem";
            this.boundingBoxToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.boundingBoxToolStripMenuItem.Text = "Bounding Box";
            this.boundingBoxToolStripMenuItem.CheckedChanged += new System.EventHandler(this.boundingBoxToolStripMenuItem_CheckedChanged);
            // 
            // movesetToolStripMenuItem1
            // 
            this.movesetToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hitboxesOffToolStripMenuItem,
            this.hurtboxesOffToolStripMenuItem});
            this.movesetToolStripMenuItem1.Name = "movesetToolStripMenuItem1";
            this.movesetToolStripMenuItem1.Size = new System.Drawing.Size(170, 22);
            this.movesetToolStripMenuItem1.Text = "Moveset";
            this.movesetToolStripMenuItem1.Visible = false;
            // 
            // hitboxesOffToolStripMenuItem
            // 
            this.hitboxesOffToolStripMenuItem.Checked = true;
            this.hitboxesOffToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hitboxesOffToolStripMenuItem.Name = "hitboxesOffToolStripMenuItem";
            this.hitboxesOffToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hitboxesOffToolStripMenuItem.Text = "Hitboxes";
            this.hitboxesOffToolStripMenuItem.CheckedChanged += new System.EventHandler(this.RenderStateChanged);
            this.hitboxesOffToolStripMenuItem.Click += new System.EventHandler(this.hitboxesOffToolStripMenuItem_Click);
            // 
            // hurtboxesOffToolStripMenuItem
            // 
            this.hurtboxesOffToolStripMenuItem.Checked = true;
            this.hurtboxesOffToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hurtboxesOffToolStripMenuItem.Name = "hurtboxesOffToolStripMenuItem";
            this.hurtboxesOffToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hurtboxesOffToolStripMenuItem.Text = "Hurtboxes";
            this.hurtboxesOffToolStripMenuItem.CheckedChanged += new System.EventHandler(this.RenderStateChanged);
            this.hurtboxesOffToolStripMenuItem.Click += new System.EventHandler(this.hurtboxesOffToolStripMenuItem_Click);
            // 
            // fileTypesToolStripMenuItem
            // 
            this.fileTypesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playCHR0ToolStripMenuItem,
            this.playSRT0ToolStripMenuItem,
            this.playSHP0ToolStripMenuItem,
            this.playPAT0ToolStripMenuItem,
            this.playVIS0ToolStripMenuItem,
            this.playCLR0ToolStripMenuItem});
            this.fileTypesToolStripMenuItem.Name = "fileTypesToolStripMenuItem";
            this.fileTypesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.fileTypesToolStripMenuItem.Text = "Animations";
            // 
            // playCHR0ToolStripMenuItem
            // 
            this.playCHR0ToolStripMenuItem.Checked = true;
            this.playCHR0ToolStripMenuItem.CheckOnClick = true;
            this.playCHR0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playCHR0ToolStripMenuItem.Name = "playCHR0ToolStripMenuItem";
            this.playCHR0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playCHR0ToolStripMenuItem.Text = "Play CHR0";
            // 
            // playSRT0ToolStripMenuItem
            // 
            this.playSRT0ToolStripMenuItem.Checked = true;
            this.playSRT0ToolStripMenuItem.CheckOnClick = true;
            this.playSRT0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSRT0ToolStripMenuItem.Name = "playSRT0ToolStripMenuItem";
            this.playSRT0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playSRT0ToolStripMenuItem.Text = "Play SRT0";
            // 
            // playSHP0ToolStripMenuItem
            // 
            this.playSHP0ToolStripMenuItem.Checked = true;
            this.playSHP0ToolStripMenuItem.CheckOnClick = true;
            this.playSHP0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSHP0ToolStripMenuItem.Name = "playSHP0ToolStripMenuItem";
            this.playSHP0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playSHP0ToolStripMenuItem.Text = "Play SHP0";
            // 
            // playPAT0ToolStripMenuItem
            // 
            this.playPAT0ToolStripMenuItem.Checked = true;
            this.playPAT0ToolStripMenuItem.CheckOnClick = true;
            this.playPAT0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playPAT0ToolStripMenuItem.Name = "playPAT0ToolStripMenuItem";
            this.playPAT0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playPAT0ToolStripMenuItem.Text = "Play PAT0";
            // 
            // playVIS0ToolStripMenuItem
            // 
            this.playVIS0ToolStripMenuItem.Checked = true;
            this.playVIS0ToolStripMenuItem.CheckOnClick = true;
            this.playVIS0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playVIS0ToolStripMenuItem.Name = "playVIS0ToolStripMenuItem";
            this.playVIS0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playVIS0ToolStripMenuItem.Text = "Play VIS0";
            // 
            // playCLR0ToolStripMenuItem
            // 
            this.playCLR0ToolStripMenuItem.Checked = true;
            this.playCLR0ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playCLR0ToolStripMenuItem.Name = "playCLR0ToolStripMenuItem";
            this.playCLR0ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.playCLR0ToolStripMenuItem.Text = "Play CLR0";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // viewOpenedFilesToolStripMenuItem
            // 
            this.viewOpenedFilesToolStripMenuItem.Name = "viewOpenedFilesToolStripMenuItem";
            this.viewOpenedFilesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.viewOpenedFilesToolStripMenuItem.Text = "View Opened Files";
            this.viewOpenedFilesToolStripMenuItem.Click += new System.EventHandler(this.viewOpenedFilesToolStripMenuItem_Click);
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // sCN0ToolStripMenuItem
            // 
            this.sCN0ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayAmbienceToolStripMenuItem,
            this.displayLightsToolStripMenuItem,
            this.displayFogToolStripMenuItem,
            this.displayCameraToolStripMenuItem});
            this.sCN0ToolStripMenuItem.Name = "sCN0ToolStripMenuItem";
            this.sCN0ToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.sCN0ToolStripMenuItem.Text = "SCN0";
            this.sCN0ToolStripMenuItem.Visible = false;
            // 
            // displayAmbienceToolStripMenuItem
            // 
            this.displayAmbienceToolStripMenuItem.Name = "displayAmbienceToolStripMenuItem";
            this.displayAmbienceToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.displayAmbienceToolStripMenuItem.Text = "Display Ambience";
            // 
            // displayLightsToolStripMenuItem
            // 
            this.displayLightsToolStripMenuItem.Name = "displayLightsToolStripMenuItem";
            this.displayLightsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.displayLightsToolStripMenuItem.Text = "Display Light";
            // 
            // displayFogToolStripMenuItem
            // 
            this.displayFogToolStripMenuItem.Name = "displayFogToolStripMenuItem";
            this.displayFogToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.displayFogToolStripMenuItem.Text = "Display Fog";
            // 
            // displayCameraToolStripMenuItem
            // 
            this.displayCameraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayToolStripMenuItem,
            this.stPersonToolStripMenuItem});
            this.displayCameraToolStripMenuItem.Name = "displayCameraToolStripMenuItem";
            this.displayCameraToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.displayCameraToolStripMenuItem.Text = "Camera";
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // stPersonToolStripMenuItem
            // 
            this.stPersonToolStripMenuItem.CheckOnClick = true;
            this.stPersonToolStripMenuItem.Name = "stPersonToolStripMenuItem";
            this.stPersonToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.stPersonToolStripMenuItem.Text = "1st Person";
            this.stPersonToolStripMenuItem.CheckedChanged += new System.EventHandler(this.stPersonToolStripMenuItem_CheckedChanged);
            // 
            // comboCharacters
            // 
            this.comboCharacters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCharacters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCharacters.Enabled = false;
            this.comboCharacters.FormattingEnabled = true;
            this.comboCharacters.Items.AddRange(new object[] {
            "All"});
            this.comboCharacters.Location = new System.Drawing.Point(153, 2);
            this.comboCharacters.Name = "comboCharacters";
            this.comboCharacters.Size = new System.Drawing.Size(120, 21);
            this.comboCharacters.TabIndex = 21;
            this.comboCharacters.SelectedIndexChanged += new System.EventHandler(this.comboCharacters_SelectedIndexChanged);
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.splitter1);
            this.controlPanel.Controls.Add(this.toolStrip1);
            this.controlPanel.Controls.Add(this.panel2);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(911, 24);
            this.controlPanel.TabIndex = 22;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(365, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 24);
            this.splitter1.TabIndex = 31;
            this.splitter1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkHitboxes,
            this.chkHurtboxes,
            this.toolStripSeparator2,
            this.chkBones,
            this.chkPolygons,
            this.chkShaders,
            this.toolStripSeparator1,
            this.chkFloor,
            this.button1,
            this.btnSaveCam});
            this.toolStrip1.Location = new System.Drawing.Point(365, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.toolStrip1.Size = new System.Drawing.Size(546, 24);
            this.toolStrip1.TabIndex = 30;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // chkHitboxes
            // 
            this.chkHitboxes.Checked = true;
            this.chkHitboxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHitboxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkHitboxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkHitboxes.Name = "chkHitboxes";
            this.chkHitboxes.Size = new System.Drawing.Size(57, 21);
            this.chkHitboxes.Text = "Hitboxes";
            this.chkHitboxes.Click += new System.EventHandler(this.hitboxesOffToolStripMenuItem_Click);
            // 
            // chkHurtboxes
            // 
            this.chkHurtboxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkHurtboxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkHurtboxes.Name = "chkHurtboxes";
            this.chkHurtboxes.Size = new System.Drawing.Size(65, 21);
            this.chkHurtboxes.Text = "Hurtboxes";
            this.chkHurtboxes.Click += new System.EventHandler(this.hurtboxesOffToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 24);
            this.toolStripSeparator2.Visible = false;
            // 
            // chkBones
            // 
            this.chkBones.Checked = true;
            this.chkBones.CheckOnClick = true;
            this.chkBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBones.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkBones.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkBones.Name = "chkBones";
            this.chkBones.Size = new System.Drawing.Size(43, 21);
            this.chkBones.Text = "Bones";
            this.chkBones.CheckedChanged += new System.EventHandler(this.chkBones_CheckedChanged);
            // 
            // chkPolygons
            // 
            this.chkPolygons.Checked = true;
            this.chkPolygons.CheckOnClick = true;
            this.chkPolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPolygons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkPolygons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkPolygons.Name = "chkPolygons";
            this.chkPolygons.Size = new System.Drawing.Size(60, 21);
            this.chkPolygons.Text = "Polygons";
            this.chkPolygons.CheckedChanged += new System.EventHandler(this.chkPolygons_CheckedChanged);
            // 
            // chkShaders
            // 
            this.chkShaders.CheckOnClick = true;
            this.chkShaders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkShaders.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkShaders.Name = "chkShaders";
            this.chkShaders.Size = new System.Drawing.Size(52, 21);
            this.chkShaders.Text = "Shaders";
            this.chkShaders.Visible = false;
            this.chkShaders.CheckedChanged += new System.EventHandler(this.chkShaders_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // chkFloor
            // 
            this.chkFloor.Checked = true;
            this.chkFloor.CheckOnClick = true;
            this.chkFloor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFloor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkFloor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkFloor.Name = "chkFloor";
            this.chkFloor.Size = new System.Drawing.Size(38, 21);
            this.chkFloor.Text = "Floor";
            this.chkFloor.CheckedChanged += new System.EventHandler(this.chkFloor_CheckedChanged);
            // 
            // button1
            // 
            this.button1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 21);
            this.button1.Text = "Reset Camera";
            this.button1.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click_1);
            // 
            // btnSaveCam
            // 
            this.btnSaveCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveCam.Name = "btnSaveCam";
            this.btnSaveCam.Size = new System.Drawing.Size(79, 21);
            this.btnSaveCam.Text = "Save Camera";
            this.btnSaveCam.Click += new System.EventHandler(this.btnSaveCam_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.fileType);
            this.panel2.Controls.Add(this.comboMdl);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Controls.Add(this.comboCharacters);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(365, 24);
            this.panel2.TabIndex = 29;
            // 
            // fileType
            // 
            this.fileType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType.FormattingEnabled = true;
            this.fileType.Items.AddRange(new object[] {
            "CHR",
            "SRT",
            "SHP",
            "PAT",
            "VIS",
            "CLR"});
            this.fileType.Location = new System.Drawing.Point(311, 2);
            this.fileType.Name = "fileType";
            this.fileType.Size = new System.Drawing.Size(51, 21);
            this.fileType.TabIndex = 24;
            this.fileType.SelectedIndexChanged += new System.EventHandler(this.fileType_SelectedIndexChanged);
            // 
            // comboMdl
            // 
            this.comboMdl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboMdl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMdl.FormattingEnabled = true;
            this.comboMdl.Items.AddRange(new object[] {
            "All"});
            this.comboMdl.Location = new System.Drawing.Point(274, 2);
            this.comboMdl.Name = "comboMdl";
            this.comboMdl.Size = new System.Drawing.Size(36, 21);
            this.comboMdl.TabIndex = 23;
            this.comboMdl.SelectedIndexChanged += new System.EventHandler(this.comboMdl_SelectedIndexChanged);
            // 
            // modelPanel
            // 
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.Location = new System.Drawing.Point(0, 0);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(519, 513);
            this.modelPanel.TabIndex = 0;
            // 
            // animEditors
            // 
            this.animEditors.AutoScroll = true;
            this.animEditors.Controls.Add(this.pnlPlayback);
            this.animEditors.Controls.Add(this.panel3);
            this.animEditors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.animEditors.Location = new System.Drawing.Point(0, 537);
            this.animEditors.Name = "animEditors";
            this.animEditors.Size = new System.Drawing.Size(911, 60);
            this.animEditors.TabIndex = 29;
            // 
            // pnlPlayback
            // 
            this.pnlPlayback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlayback.Location = new System.Drawing.Point(264, 0);
            this.pnlPlayback.MinimumSize = new System.Drawing.Size(290, 54);
            this.pnlPlayback.Name = "pnlPlayback";
            this.pnlPlayback.Size = new System.Drawing.Size(647, 60);
            this.pnlPlayback.TabIndex = 30;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.hurtboxEditor);
            this.panel3.Controls.Add(this.vis0Editor);
            this.panel3.Controls.Add(this.pat0Editor);
            this.panel3.Controls.Add(this.shp0Editor);
            this.panel3.Controls.Add(this.srt0Editor);
            this.panel3.Controls.Add(this.chr0Editor);
            this.panel3.Controls.Add(this.clr0Editor);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(264, 60);
            this.panel3.TabIndex = 29;
            // 
            // vis0Editor
            // 
            this.vis0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vis0Editor.Location = new System.Drawing.Point(0, 0);
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
            this.pat0Editor.Name = "pat0Editor";
            this.pat0Editor.Size = new System.Drawing.Size(264, 60);
            this.pat0Editor.TabIndex = 27;
            this.pat0Editor.Visible = false;
            // 
            // shp0Editor
            // 
            this.shp0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shp0Editor.Location = new System.Drawing.Point(0, 0);
            this.shp0Editor.Name = "shp0Editor";
            this.shp0Editor.Size = new System.Drawing.Size(264, 60);
            this.shp0Editor.TabIndex = 28;
            this.shp0Editor.Visible = false;
            // 
            // srt0Editor
            // 
            this.srt0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.srt0Editor.Location = new System.Drawing.Point(0, 0);
            this.srt0Editor.Name = "srt0Editor";
            this.srt0Editor.Size = new System.Drawing.Size(264, 60);
            this.srt0Editor.TabIndex = 20;
            this.srt0Editor.Visible = false;
            // 
            // chr0Editor
            // 
            this.chr0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chr0Editor.Location = new System.Drawing.Point(0, 0);
            this.chr0Editor.Name = "chr0Editor";
            this.chr0Editor.Size = new System.Drawing.Size(264, 60);
            this.chr0Editor.TabIndex = 19;
            this.chr0Editor.Visible = false;
            this.chr0Editor.VisibleChanged += new System.EventHandler(this.chr0Editor_VisibleChanged);
            // 
            // clr0Editor
            // 
            this.clr0Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clr0Editor.Location = new System.Drawing.Point(0, 0);
            this.clr0Editor.Name = "clr0Editor";
            this.clr0Editor.Size = new System.Drawing.Size(264, 60);
            this.clr0Editor.TabIndex = 30;
            this.clr0Editor.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listPanel);
            this.panel1.Controls.Add(this.splitter2);
            this.panel1.Controls.Add(this.scriptPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 513);
            this.panel1.TabIndex = 34;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 261);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(228, 3);
            this.splitter2.TabIndex = 34;
            this.splitter2.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.btnBottomToggle);
            this.panel4.Controls.Add(this.btnTopToggle);
            this.panel4.Controls.Add(this.modelPanel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(232, 24);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(519, 513);
            this.panel4.TabIndex = 36;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Left;
            this.button3.Location = new System.Drawing.Point(0, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(18, 477);
            this.button3.TabIndex = 13;
            this.button3.TabStop = false;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.btnLeftToggle_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.Location = new System.Drawing.Point(501, 18);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(18, 477);
            this.button2.TabIndex = 12;
            this.button2.TabStop = false;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnRightToggle_Click);
            // 
            // spltRight
            // 
            this.spltRight.BackColor = System.Drawing.SystemColors.Control;
            this.spltRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.spltRight.Location = new System.Drawing.Point(751, 24);
            this.spltRight.Name = "spltRight";
            this.spltRight.Size = new System.Drawing.Size(4, 513);
            this.spltRight.TabIndex = 37;
            this.spltRight.TabStop = false;
            // 
            // modelListsPanel1
            // 
            this.modelListsPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.modelListsPanel1.Location = new System.Drawing.Point(755, 24);
            this.modelListsPanel1.Name = "modelListsPanel1";
            this.modelListsPanel1.Size = new System.Drawing.Size(156, 513);
            this.modelListsPanel1.TabIndex = 35;
            // 
            // scriptPanel
            // 
            this.scriptPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scriptPanel.Location = new System.Drawing.Point(0, 264);
            this.scriptPanel.Name = "scriptPanel";
            this.scriptPanel.Size = new System.Drawing.Size(228, 249);
            this.scriptPanel.TabIndex = 33;
            // 
            // listPanel
            // 
            this.listPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPanel.Location = new System.Drawing.Point(0, 0);
            this.listPanel.Name = "listPanel";
            this.listPanel.Size = new System.Drawing.Size(228, 261);
            this.listPanel.TabIndex = 32;
            // 
            // hurtboxEditor
            // 
            this.hurtboxEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hurtboxEditor.Location = new System.Drawing.Point(0, 0);
            this.hurtboxEditor.Name = "hurtboxEditor";
            this.hurtboxEditor.Size = new System.Drawing.Size(264, 60);
            this.hurtboxEditor.TabIndex = 31;
            this.hurtboxEditor.TargetHurtBox = null;
            this.hurtboxEditor.Visible = false;
            // 
            // MainControl
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.spltRight);
            this.Controls.Add(this.spltLeft);
            this.Controls.Add(this.modelListsPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.animEditors);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(911, 597);
            this.SizeChanged += new System.EventHandler(this.ModelEditControl_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ModelEditControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ModelEditControl_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.animEditors.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Initialization
        public MainControl()
        {
            InitializeComponent();
            listPanel._mainWindow =
            scriptPanel.scriptPanel._mainWindow =
            modelListsPanel1._mainWindow =
            this;
            MovesetPanel._mainWindow = this;
            modelListsPanel1.bonesPanel1._mainWindow =
            chr0Editor._mainWindow =
            srt0Editor._mainWindow =
            shp0Editor._mainWindow =
            pat0Editor._mainWindow =
            vis0Editor._mainWindow =
            clr0Editor._mainWindow =
            pnlPlayback._mainWindow =
            this;
            MovesetPanel.comboActionEntry.SelectedIndex = 0;
            _updating = true;

            PreConstruct();

            ScreenCapBgLocText.Text = Application.StartupPath;

            comboCharacters.DataSource = Manager._supportedCharacters;
            comboCharacters.SelectedIndex = Array.IndexOf(Manager._supportedCharacters, Manager.TargetCharacter);

            _targetModels = new List<IModel>();

            Manager.RootChanged += new EventHandler(FolderManager_RootChanged);
            Manager.TargetCharacterChanged += new EventHandler(FolderManager_TargetCharacterChanged);

            _floorHue = Color.FromArgb(255, 99, 101, 107);

            //modelPanel.RenderShaders = false;

            ModelPanelViewport v = modelPanel.CurrentViewport;

            v.BackgroundColor = Color.FromArgb(0, 45, 45, 65);
            v.Ambient = new Vector4(65.0f / 255.0f, 78.0f / 255.0f, 94.0f / 255.0f, 255.0f / 255.0f);
            v.DefaultTranslate = new Vector3(-25.0f, 15.0f, 50.0f);
            v.DefaultRotate = new Vector3(-5.0f, -30.0f, 0.0f);
            v.ResetCamera();

            _updating = false;

            PostConstruct();
        }

        #endregion

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ModelPlaybackPanel PlaybackPanel { get { return pnlPlayback; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ModelPanel ModelPanel { get { return _viewerForm == null ? modelPanel : _viewerForm.modelPanel1; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override CHR0Editor CHR0Editor { get { return chr0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SRT0Editor SRT0Editor { get { return srt0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SHP0Editor SHP0Editor { get { return shp0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override VIS0Editor VIS0Editor { get { return vis0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override PAT0Editor PAT0Editor { get { return pat0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override CLR0Editor CLR0Editor { get { return clr0Editor; } }
        public override ColorDialog ColorDialog { get { return dlgColor; } }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string path = null;
#if DEBUG
            if (Environment.UserName == "David")
                path = "D:/brawlmods/custom1";
            else
                path = Ikarus.Properties.Settings.Default.RootPath;
#else
            path = Ikarus.Properties.Settings.Default.RootPath;
#endif
            if (!String.IsNullOrEmpty(path))
            {
                RunTime._IsRoot = true;
                Program.OpenRootFromPath(pathToolStripMenuItem.Text = path);
                comboCharacters.Enabled = true;
            }

            RunTime.FramesPerSecond = (double)pnlPlayback.numFPS.Value;
            RunTime.UpdatesPerSecond = 0;

            Application.AddMessageFilter(RunTime.ButtonManager._keyFilter);

            fileType.SelectedIndex = 0;
            //TargetAnimType = NW4RAnimType.CHR;
            if (listPanel.SubActionsList.Items.Count > 0)
                listPanel.SubActionsList_SelectedIndexChanged_1(this, null);
        }

        private void FolderManager_RootChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void FolderManager_TargetCharacterChanged(object sender, EventArgs e)
        {
            _updating = true;
            comboCharacters.SelectedIndex = Array.IndexOf(Manager._supportedCharacters, Manager.TargetCharacter.ToString());
            _updating = false;
            Reset();
        }

        private void ModelEditControl_SizeChanged(object sender, EventArgs e)
        {
            CheckDimensions();
        }

        public void CheckDimensions()
        {
            int totalWidth = animEditors.Width;
            int w = panel3.Width, h = animEditors.Height;
            if (_currentControl != null && _currentControl.Visible)
            {
                if (_currentControl is CHR0Editor)
                {
                    h = 78;
                    w = 582;
                }
                else if (_currentControl is SRT0Editor)
                {
                    h = 78;
                    w = 483;
                }
                else if (_currentControl is SHP0Editor)
                {
                    h = 106;
                    w = 533;
                }
                else if (_currentControl is PAT0Editor)
                {
                    h = 78;
                    w = 402;
                }
                else if (_currentControl is VIS0Editor)
                {
                    h = 62;
                    w = 210;
                }
                else if (_currentControl is CLR0Editor)
                {
                    h = 62;
                    w = 168;
                }
                else
                    w = h = 0;
            }
            else
                w = h = 0;

            //See if the scroll bar needs to be visible
            int addedHeight = 0;
            if (w + pnlPlayback.MinimumSize.Width > totalWidth)
            {
                addedHeight = 17;
                animEditors.HorizontalScroll.Visible = true;
            }
            else
                animEditors.HorizontalScroll.Visible = false;

            //Don't update the width and height every time, only if need be
            if (panel3.Width != w)
                panel3.Width = w;
            if (animEditors.Height != h + addedHeight)
                animEditors.Height = h + addedHeight;

            //Dock playback panel if it reaches its minimum size
            if (pnlPlayback.Width <= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Dock = DockStyle.Left;
                pnlPlayback.Width = pnlPlayback.MinimumSize.Width;
            }
            else
                pnlPlayback.Dock = DockStyle.Fill;

            //Stretch playback panel if there's space
            if (animEditors.Width - panel3.Width >= pnlPlayback.MinimumSize.Width)
            {
                pnlPlayback.Width += animEditors.Width - panel3.Width - pnlPlayback.MinimumSize.Width;
                pnlPlayback.Dock = DockStyle.Fill;
            }
            else pnlPlayback.Dock = DockStyle.Left;
        }

        internal void DisableHurtboxEditor()
        {
            if (hurtboxEditor.Visible == true)
                SetCurrentControl();
        }

        internal void EnableHurtboxEditor()
        {
            if (hurtboxEditor.Visible != true)
            {
                Control newControl = hurtboxEditor;
                if (_currentControl != newControl)
                {
                    if (_currentControl != null)
                        _currentControl.Visible = false;
                    if ((_currentControl = newControl) != null)
                    {
                        _currentControl.Visible = true;
                        animEditors.Height = 102;
                        panel3.Width = 288;
                    }
                }
            }
        }

        private void btnLoadRoot_Click(object sender, EventArgs e)
        {
            RunTime._IsRoot = true;
            if (Program.OpenRoot(pathToolStripMenuItem.Text))
                Ikarus.Properties.Settings.Default.RootPath =
                    pathToolStripMenuItem.Text =
                    String.IsNullOrEmpty(Program.RootPath) ? "<null>" : Program.RootPath;
            this.comboCharacters.Enabled = true;
        }
        
        private void btnLoadChar_Click(object sender, EventArgs e)
        {
            RunTime._IsRoot = false;
            if (Program.OpenRoot(pathToolStripMenuItem.Text))
                Ikarus.Properties.Settings.Default.RootPath =
                    pathToolStripMenuItem.Text =
                    String.IsNullOrEmpty(Program.RootPath) ? "<null>" : Program.RootPath;
            this.comboCharacters.Enabled = true;
            
        }
        public void Reset()
        {
            _resetCamera = false;
            modelPanel.ClearAll();
            if (Manager.SelectedInfo != null)
            {
                TargetModel = Manager.TargetModel;
                CollectArticles();

                listPanel.UpdateMoveset();
                listPanel.UpdateAnimations();

                ResetBoneColors();
                RunTime.SetFrame(1);
            }
            else
                TargetModel = null;
        }

        private void CollectArticles()
        {
            if (RunTime._articles != null)
                foreach (ArticleInfo i in RunTime._articles)
                    if (i != null && i._model != null && _targetModels.Contains(i._model))
                        RemoveTarget(i._model);

            if (Manager.Moveset != null && Manager.Moveset.Data != null)
            {
                RunTime._articles = new ArticleInfo[Manager.Moveset.Data._articles.Count];
                foreach (ArticleNode article in Manager.Moveset.Data._articles)
                {
                    ArticleInfo articleInfo = new ArticleInfo(article, null, false);

                    int groupID = article.ARCGroupID;
                    if (groupID >= 0)
                    {
                        LoadArticles(Manager.SelectedInfo.CharacterFiles, groupID, articleInfo, true);
                        LoadArticles(Manager.SelectedInfo.CharacterEtcFiles, groupID, articleInfo, false);
                        LoadArticles(Manager.SelectedInfo.CharacterFinalFiles, groupID, articleInfo, false);
                    }
                    RunTime._articles[article.Index] = articleInfo;
                }
            }
        }

        private void LoadArticles(Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> t1, int groupID, ArticleInfo info, bool addTarget)
        {
            if (t1 != null && t1.ContainsKey(groupID))
            {
                var t2 = t1[groupID];
                if (t2.ContainsKey(ARCFileType.ModelData))
                {
                    List<ARCEntryNode> entries = t2[ARCFileType.ModelData];
                    foreach (ARCEntryNode e in entries)
                    {
                        //Don't load shadow models or main models
                        if (e.FileIndex == 10 || e.GroupID == 0)
                            continue;

                        MDL0Node model = e.Children[0].Children[0] as MDL0Node;
                        info._model = model;

                        MDL0BoneNode cBone = info._article.CharBoneNode;
                        if (cBone != null && info._article.ArticleBoneNode != null)
                        {
                            MDL0BoneNode aBone = info._article.ArticleBoneNode;
                            aBone.OverrideBone = cBone;
                        }

                        if (addTarget)
                        {
                            info.Running = true;
                            info._etcModel = false;
                        }
                        else
                        {
                            info._etcModel = true;
                            info.Running = false;
                        }
                        AppendTarget(info._model);
                    }
                }
                if (t2.ContainsKey(ARCFileType.AnimationData))
                {
                    List<ARCEntryNode> entries = t2[ARCFileType.AnimationData];
                    foreach (ARCEntryNode u in entries)
                    {
                        ARCEntryNode node;
                        if (u.RedirectIndex >= 0 &&
                            u.RedirectIndex != u.Index &&
                            u.Parent != null &&
                            u.RedirectIndex < u.Parent.Children.Count)
                            node = u.Parent.Children[u.RedirectIndex] as ARCEntryNode;
                        else
                            node = u;

                        int index = node.FileIndex;
                        foreach (BRESGroupNode b in node.Children)
                        {
                            NW4RAnimationNode anim = b.Children[0] as NW4RAnimationNode;
                            switch (b.Type)
                            {
                                case BRESGroupNode.BRESGroupType.CHR0:
                                    info._chr0List[index] = anim as CHR0Node;
                                    break;
                                case BRESGroupNode.BRESGroupType.SRT0:
                                    info._srt0List[index] = anim as SRT0Node;
                                    break;
                                case BRESGroupNode.BRESGroupType.SHP0:
                                    info._shp0List[index] = anim as SHP0Node;
                                    break;
                                case BRESGroupNode.BRESGroupType.VIS0:
                                    info._vis0List[index] = anim as VIS0Node;
                                    break;
                                case BRESGroupNode.BRESGroupType.PAT0:
                                    info._pat0List[index] = anim as PAT0Node;
                                    break;
                                case BRESGroupNode.BRESGroupType.CLR0:
                                    info._clr0List[index] = anim as CLR0Node;
                                    break;
                            }
                        }
                    }
                }

            }
        }

        private void muteSFXToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            RunTime._muteSFX = muteSFXToolStripMenuItem.Checked;
        }

        private void comboMdl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Manager.ModelIndexChanged();
        }

        protected override void OnModelChanged()
        {
            if (_targetModel != null)
                listPanel.VIS0Indices = ((MDL0Node)_targetModel).VIS0Indices;

            hurtboxEditor._mainControl_TargetModelChanged(null, null);
            modelListsPanel1.Reset();
            CollectArticles();
            UpdateModel();
            RunTime.ResetSubactionVariables();
        }

        private void fileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TargetAnimType = (NW4RAnimType)fileType.SelectedIndex;
            SetCurrentControl();
        }

        private void saveAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Save();
        }

        OpenedFilesDialog ofd = new OpenedFilesDialog();
        LogDialog ld = new LogDialog();
        private void viewOpenedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ofd.Visible)
            {
                if (ofd.IsDisposed)
                    ofd = new OpenedFilesDialog();
                ofd.Show();
            }
            else
                ofd.Close();
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ld.Visible)
            {
                if (ld.IsDisposed)
                    ld = new LogDialog();
                ld.Show();
            }
            else
                ld.Close();
        }

        private void saveTextInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manager.SaveAllTextData();
        }
    }
}

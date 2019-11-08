using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.UI.Model_Previewer
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
        private ToolStripMenuItem _deleteToolStripMenuItem;
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
            components = new Container();
            ListViewGroup listViewGroup2 = new ListViewGroup("Animations", HorizontalAlignment.Left);
            pnlObjects = new Panel();
            lstObjects = new CheckedListBox();
            spltDrawCalls = new Splitter();
            lstDrawCalls = new CheckedListBox();
            chkAllObj = new CheckBox();
            chkSyncVis = new CheckBox();
            btnObjects = new Button();
            pnlAnims = new Panel();
            listAnims = new ListView();
            nameColumn = new ColumnHeader();
            ctxAnimList = new ContextMenuStrip(components);
            AnimListNewAnim = new ToolStripMenuItem();
            inModelsBRRESToolStripMenuItem = new ToolStripMenuItem();
            inExternalFileToolStripMenuItem = new ToolStripMenuItem();
            panel2 = new Panel();
            txtSearchAnim = new TextBox();
            chkContains = new CheckBox();
            panel1 = new Panel();
            btnSaveAnims = new Button();
            btnLoad = new Button();
            fileType = new ComboBox();
            btnAnims = new Button();
            ctxTextures = new ContextMenuStrip(components);
            sourceToolStripMenuItem = new ToolStripMenuItem();
            sizeToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            viewToolStripMenuItem = new ToolStripMenuItem();
            exportTextureToolStripMenuItem = new ToolStripMenuItem();
            replaceTextureToolStripMenuItem = new ToolStripMenuItem();
            renameTextureTextureToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            pnlTextures = new Panel();
            lstTextures = new CheckedListBox();
            chkAllTextures = new CheckBox();
            btnTextures = new Button();
            ctxAnim = new ContextMenuStrip(components);
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            chkLoop = new ToolStripMenuItem();
            matrixModeToolStripMenuItem = new ToolStripMenuItem();
            chkMtxMaya = new ToolStripMenuItem();
            chkMtxXSI = new ToolStripMenuItem();
            chkMtxMax = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            renameToolStripMenuItem = new ToolStripMenuItem();
            _deleteToolStripMenuItem = new ToolStripMenuItem();
            createNewToolStripMenuItem = new ToolStripMenuItem();
            overObjPnl = new TransparentPanel();
            spltObjTex = new ProxySplitter();
            spltAnimObj = new ProxySplitter();
            overTexPnl = new TransparentPanel();
            pnlObjects.SuspendLayout();
            pnlAnims.SuspendLayout();
            ctxAnimList.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ctxTextures.SuspendLayout();
            pnlTextures.SuspendLayout();
            ctxAnim.SuspendLayout();
            SuspendLayout();
            // 
            // pnlObjects
            // 
            pnlObjects.BorderStyle = BorderStyle.FixedSingle;
            pnlObjects.Controls.Add(overObjPnl);
            pnlObjects.Controls.Add(lstObjects);
            pnlObjects.Controls.Add(spltDrawCalls);
            pnlObjects.Controls.Add(lstDrawCalls);
            pnlObjects.Controls.Add(chkAllObj);
            pnlObjects.Controls.Add(chkSyncVis);
            pnlObjects.Controls.Add(btnObjects);
            pnlObjects.Dock = DockStyle.Fill;
            pnlObjects.Location = new System.Drawing.Point(0, 182);
            pnlObjects.MinimumSize = new System.Drawing.Size(0, 21);
            pnlObjects.Name = "pnlObjects";
            pnlObjects.Size = new System.Drawing.Size(172, 150);
            pnlObjects.TabIndex = 0;
            // 
            // lstObjects
            // 
            lstObjects.BorderStyle = BorderStyle.None;
            lstObjects.CausesValidation = false;
            lstObjects.Dock = DockStyle.Fill;
            lstObjects.IntegralHeight = false;
            lstObjects.Location = new System.Drawing.Point(0, 66);
            lstObjects.Margin = new Padding(0);
            lstObjects.Name = "lstObjects";
            lstObjects.Size = new System.Drawing.Size(170, 45);
            lstObjects.TabIndex = 4;
            lstObjects.ItemCheck += new ItemCheckEventHandler(lstPolygons_ItemCheck);
            lstObjects.SelectedValueChanged += new EventHandler(lstPolygons_SelectedValueChanged);
            lstObjects.KeyDown += new KeyEventHandler(lstPolygons_KeyDown);
            lstObjects.Leave += new EventHandler(lstObjects_Leave);
            // 
            // spltDrawCalls
            // 
            spltDrawCalls.Dock = DockStyle.Bottom;
            spltDrawCalls.Location = new System.Drawing.Point(0, 111);
            spltDrawCalls.Name = "spltDrawCalls";
            spltDrawCalls.Size = new System.Drawing.Size(170, 3);
            spltDrawCalls.TabIndex = 9;
            spltDrawCalls.TabStop = false;
            spltDrawCalls.Visible = false;
            // 
            // lstDrawCalls
            // 
            lstDrawCalls.Dock = DockStyle.Bottom;
            lstDrawCalls.FormattingEnabled = true;
            lstDrawCalls.IntegralHeight = false;
            lstDrawCalls.Location = new System.Drawing.Point(0, 114);
            lstDrawCalls.Name = "lstDrawCalls";
            lstDrawCalls.Size = new System.Drawing.Size(170, 34);
            lstDrawCalls.TabIndex = 0;
            lstDrawCalls.Visible = false;
            lstDrawCalls.ItemCheck += new ItemCheckEventHandler(lstDrawCalls_ItemCheck);
            lstDrawCalls.SelectedIndexChanged += new EventHandler(lstDrawCalls_SelectedIndexChanged);
            lstDrawCalls.DoubleClick += new EventHandler(lstDrawCalls_DoubleClick);
            // 
            // chkAllObj
            // 
            chkAllObj.Checked = true;
            chkAllObj.CheckState = CheckState.Checked;
            chkAllObj.Dock = DockStyle.Top;
            chkAllObj.Location = new System.Drawing.Point(0, 46);
            chkAllObj.Margin = new Padding(0);
            chkAllObj.Name = "chkAllObj";
            chkAllObj.Padding = new Padding(1, 0, 0, 0);
            chkAllObj.Size = new System.Drawing.Size(170, 20);
            chkAllObj.TabIndex = 5;
            chkAllObj.Text = "All";
            chkAllObj.UseVisualStyleBackColor = false;
            chkAllObj.CheckStateChanged += new EventHandler(chkAllPoly_CheckStateChanged);
            // 
            // chkSyncVis
            // 
            chkSyncVis.Dock = DockStyle.Top;
            chkSyncVis.Location = new System.Drawing.Point(0, 26);
            chkSyncVis.Margin = new Padding(0);
            chkSyncVis.Name = "chkSyncVis";
            chkSyncVis.Padding = new Padding(1, 0, 0, 0);
            chkSyncVis.Size = new System.Drawing.Size(170, 20);
            chkSyncVis.TabIndex = 7;
            chkSyncVis.Text = "Sync VIS0";
            chkSyncVis.UseVisualStyleBackColor = false;
            // 
            // btnObjects
            // 
            btnObjects.Dock = DockStyle.Top;
            btnObjects.Location = new System.Drawing.Point(0, 0);
            btnObjects.Name = "btnObjects";
            btnObjects.Size = new System.Drawing.Size(170, 26);
            btnObjects.TabIndex = 6;
            btnObjects.Text = "Objects";
            btnObjects.UseVisualStyleBackColor = true;
            btnObjects.Click += new EventHandler(btnObjects_Click);
            // 
            // pnlAnims
            // 
            pnlAnims.BorderStyle = BorderStyle.FixedSingle;
            pnlAnims.Controls.Add(listAnims);
            pnlAnims.Controls.Add(panel2);
            pnlAnims.Controls.Add(panel1);
            pnlAnims.Controls.Add(btnAnims);
            pnlAnims.Dock = DockStyle.Top;
            pnlAnims.Location = new System.Drawing.Point(0, 0);
            pnlAnims.MinimumSize = new System.Drawing.Size(0, 21);
            pnlAnims.Name = "pnlAnims";
            pnlAnims.Size = new System.Drawing.Size(172, 178);
            pnlAnims.TabIndex = 2;
            // 
            // listAnims
            // 
            listAnims.AutoArrange = false;
            listAnims.Columns.AddRange(new ColumnHeader[]
            {
                nameColumn
            });
            listAnims.ContextMenuStrip = ctxAnimList;
            listAnims.Cursor = Cursors.Default;
            listAnims.Dock = DockStyle.Fill;
            listViewGroup2.Header = "Animations";
            listViewGroup2.Name = "grpAnims";
            listAnims.Groups.AddRange(new ListViewGroup[]
            {
                listViewGroup2
            });
            listAnims.HeaderStyle = ColumnHeaderStyle.None;
            listAnims.HideSelection = false;
            listAnims.Location = new System.Drawing.Point(0, 73);
            listAnims.MultiSelect = false;
            listAnims.Name = "listAnims";
            listAnims.Size = new System.Drawing.Size(170, 103);
            listAnims.TabIndex = 25;
            listAnims.UseCompatibleStateImageBehavior = false;
            listAnims.View = View.Details;
            listAnims.SelectedIndexChanged += new EventHandler(listAnims_SelectedIndexChanged);
            listAnims.KeyDown += new KeyEventHandler(listAnims_KeyDown);
            listAnims.MouseDown += new MouseEventHandler(listAnims_MouseDown);
            // 
            // nameColumn
            // 
            nameColumn.Text = "Name";
            nameColumn.Width = 160;
            // 
            // ctxAnimList
            // 
            ctxAnimList.ImageScalingSize = new System.Drawing.Size(20, 20);
            ctxAnimList.Items.AddRange(new ToolStripItem[]
            {
                AnimListNewAnim
            });
            ctxAnimList.Name = "ctxAnim";
            ctxAnimList.Size = new System.Drawing.Size(235, 30);
            ctxAnimList.Opening += new CancelEventHandler(ctxAnimList_Opening);
            // 
            // AnimListNewAnim
            // 
            AnimListNewAnim.DropDownItems.AddRange(new ToolStripItem[]
            {
                inModelsBRRESToolStripMenuItem,
                inExternalFileToolStripMenuItem
            });
            AnimListNewAnim.Name = "AnimListNewAnim";
            AnimListNewAnim.Size = new System.Drawing.Size(234, 26);
            AnimListNewAnim.Text = "Create New Animation";
            AnimListNewAnim.Click += new EventHandler(inModelsBRRESToolStripMenuItem_Click);
            // 
            // inModelsBRRESToolStripMenuItem
            // 
            inModelsBRRESToolStripMenuItem.Enabled = false;
            inModelsBRRESToolStripMenuItem.Name = "inModelsBRRESToolStripMenuItem";
            inModelsBRRESToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            inModelsBRRESToolStripMenuItem.Text = "In Model\'s BRRES";
            inModelsBRRESToolStripMenuItem.Visible = false;
            inModelsBRRESToolStripMenuItem.Click += new EventHandler(inModelsBRRESToolStripMenuItem_Click);
            // 
            // inExternalFileToolStripMenuItem
            // 
            inExternalFileToolStripMenuItem.Enabled = false;
            inExternalFileToolStripMenuItem.Name = "inExternalFileToolStripMenuItem";
            inExternalFileToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            inExternalFileToolStripMenuItem.Text = "In External File";
            inExternalFileToolStripMenuItem.Visible = false;
            inExternalFileToolStripMenuItem.Click += new EventHandler(inExternalFileToolStripMenuItem_Click);
            // 
            // panel2
            // 
            panel2.Controls.Add(txtSearchAnim);
            panel2.Controls.Add(chkContains);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 52);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(170, 21);
            panel2.TabIndex = 32;
            // 
            // txtSearchAnim
            // 
            txtSearchAnim.Dock = DockStyle.Fill;
            txtSearchAnim.ForeColor = Color.Gray;
            txtSearchAnim.Location = new System.Drawing.Point(0, 0);
            txtSearchAnim.Name = "txtSearchAnim";
            txtSearchAnim.Size = new System.Drawing.Size(82, 22);
            txtSearchAnim.TabIndex = 30;
            txtSearchAnim.Text = "Search for an animation...";
            txtSearchAnim.TextChanged += new EventHandler(txtSearchAnim_TextChanged);
            txtSearchAnim.Enter += new EventHandler(txtSearchAnim_Enter);
            txtSearchAnim.Leave += new EventHandler(txtSearchAnim_Leave);
            // 
            // chkContains
            // 
            chkContains.AutoSize = true;
            chkContains.Dock = DockStyle.Right;
            chkContains.Location = new System.Drawing.Point(82, 0);
            chkContains.Margin = new Padding(0);
            chkContains.Name = "chkContains";
            chkContains.Padding = new Padding(3, 0, 0, 0);
            chkContains.Size = new System.Drawing.Size(88, 21);
            chkContains.TabIndex = 32;
            chkContains.Text = "Contains";
            chkContains.UseVisualStyleBackColor = true;
            chkContains.CheckedChanged += new EventHandler(chkContains_CheckedChanged);
            // 
            // panel1
            // 
            panel1.Controls.Add(btnLoad);
            panel1.Controls.Add(btnSaveAnims);
            panel1.Controls.Add(fileType);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 26);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(170, 26);
            panel1.TabIndex = 27;
            // 
            // btnSaveAnims
            // 
            btnSaveAnims.Dock = DockStyle.Right;
            btnSaveAnims.Location = new System.Drawing.Point(51, 0);
            btnSaveAnims.Name = "btnSaveAnims";
            btnSaveAnims.Size = new System.Drawing.Size(60, 26);
            btnSaveAnims.TabIndex = 28;
            btnSaveAnims.Text = "Save";
            btnSaveAnims.UseVisualStyleBackColor = true;
            btnSaveAnims.Click += new EventHandler(button2_Click);
            // 
            // btnLoad
            // 
            btnLoad.Dock = DockStyle.Fill;
            btnLoad.Location = new System.Drawing.Point(0, 0);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(51, 26);
            btnLoad.TabIndex = 27;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += new EventHandler(button1_Click);
            // 
            // fileType
            // 
            fileType.Dock = DockStyle.Right;
            fileType.DropDownStyle = ComboBoxStyle.DropDownList;
            fileType.FormattingEnabled = true;
            fileType.Location = new System.Drawing.Point(111, 0);
            fileType.Name = "fileType";
            fileType.Size = new System.Drawing.Size(59, 24);
            fileType.TabIndex = 26;
            fileType.SelectedIndexChanged += new EventHandler(fileType_SelectedIndexChanged);
            // 
            // btnAnims
            // 
            btnAnims.Dock = DockStyle.Top;
            btnAnims.Location = new System.Drawing.Point(0, 0);
            btnAnims.Name = "btnAnims";
            btnAnims.Size = new System.Drawing.Size(170, 26);
            btnAnims.TabIndex = 7;
            btnAnims.Text = "Animations";
            btnAnims.UseVisualStyleBackColor = true;
            btnAnims.Click += new EventHandler(btnAnims_Click);
            // 
            // ctxTextures
            // 
            ctxTextures.ImageScalingSize = new System.Drawing.Size(20, 20);
            ctxTextures.Items.AddRange(new ToolStripItem[]
            {
                sourceToolStripMenuItem,
                sizeToolStripMenuItem,
                toolStripMenuItem1,
                viewToolStripMenuItem,
                exportTextureToolStripMenuItem,
                replaceTextureToolStripMenuItem,
                renameTextureTextureToolStripMenuItem,
                resetToolStripMenuItem
            });
            ctxTextures.Name = "ctxTextures";
            ctxTextures.Size = new System.Drawing.Size(147, 192);
            ctxTextures.Opening += new CancelEventHandler(ctxTextures_Opening);
            // 
            // sourceToolStripMenuItem
            // 
            sourceToolStripMenuItem.Enabled = false;
            sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            sourceToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            sourceToolStripMenuItem.Text = "Source";
            // 
            // sizeToolStripMenuItem
            // 
            sizeToolStripMenuItem.Enabled = false;
            sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            sizeToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            sizeToolStripMenuItem.Text = "Size";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            viewToolStripMenuItem.Text = "View...";
            viewToolStripMenuItem.Click += new EventHandler(viewToolStripMenuItem_Click);
            // 
            // exportTextureToolStripMenuItem
            // 
            exportTextureToolStripMenuItem.Name = "exportTextureToolStripMenuItem";
            exportTextureToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            exportTextureToolStripMenuItem.Text = "Export...";
            exportTextureToolStripMenuItem.Click += new EventHandler(exportTextureToolStripMenuItem_Click);
            // 
            // replaceTextureToolStripMenuItem
            // 
            replaceTextureToolStripMenuItem.Name = "replaceTextureToolStripMenuItem";
            replaceTextureToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            replaceTextureToolStripMenuItem.Text = "Replace...";
            replaceTextureToolStripMenuItem.Click += new EventHandler(replaceTextureToolStripMenuItem_Click);
            // 
            // renameTextureTextureToolStripMenuItem
            // 
            renameTextureTextureToolStripMenuItem.Name = "renameTextureTextureToolStripMenuItem";
            renameTextureTextureToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            renameTextureTextureToolStripMenuItem.Text = "Rename";
            renameTextureTextureToolStripMenuItem.Click += new EventHandler(renameTextureToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            resetToolStripMenuItem.Text = "Reload";
            resetToolStripMenuItem.Click += new EventHandler(resetToolStripMenuItem_Click);
            // 
            // pnlTextures
            // 
            pnlTextures.BorderStyle = BorderStyle.FixedSingle;
            pnlTextures.Controls.Add(overTexPnl);
            pnlTextures.Controls.Add(lstTextures);
            pnlTextures.Controls.Add(chkAllTextures);
            pnlTextures.Controls.Add(btnTextures);
            pnlTextures.Dock = DockStyle.Bottom;
            pnlTextures.Location = new System.Drawing.Point(0, 336);
            pnlTextures.MinimumSize = new System.Drawing.Size(0, 21);
            pnlTextures.Name = "pnlTextures";
            pnlTextures.Size = new System.Drawing.Size(172, 164);
            pnlTextures.TabIndex = 3;
            // 
            // lstTextures
            // 
            lstTextures.BorderStyle = BorderStyle.None;
            lstTextures.CausesValidation = false;
            lstTextures.ContextMenuStrip = ctxTextures;
            lstTextures.Dock = DockStyle.Fill;
            lstTextures.IntegralHeight = false;
            lstTextures.Location = new System.Drawing.Point(0, 46);
            lstTextures.Margin = new Padding(0);
            lstTextures.Name = "lstTextures";
            lstTextures.Size = new System.Drawing.Size(170, 116);
            lstTextures.TabIndex = 7;
            lstTextures.ItemCheck += new ItemCheckEventHandler(lstTextures_ItemCheck);
            lstTextures.SelectedValueChanged += new EventHandler(lstTextures_SelectedValueChanged);
            lstTextures.KeyDown += new KeyEventHandler(lstTextures_KeyDown);
            lstTextures.Leave += new EventHandler(lstTextures_Leave);
            lstTextures.MouseDown += new MouseEventHandler(lstTextures_MouseDown);
            // 
            // chkAllTextures
            // 
            chkAllTextures.Checked = true;
            chkAllTextures.CheckState = CheckState.Checked;
            chkAllTextures.Dock = DockStyle.Top;
            chkAllTextures.Location = new System.Drawing.Point(0, 26);
            chkAllTextures.Margin = new Padding(0);
            chkAllTextures.Name = "chkAllTextures";
            chkAllTextures.Padding = new Padding(1, 0, 0, 0);
            chkAllTextures.Size = new System.Drawing.Size(170, 20);
            chkAllTextures.TabIndex = 8;
            chkAllTextures.Text = "All";
            chkAllTextures.UseVisualStyleBackColor = false;
            chkAllTextures.CheckStateChanged += new EventHandler(chkAllTextures_CheckStateChanged);
            // 
            // btnTextures
            // 
            btnTextures.Dock = DockStyle.Top;
            btnTextures.Location = new System.Drawing.Point(0, 0);
            btnTextures.Name = "btnTextures";
            btnTextures.Size = new System.Drawing.Size(170, 26);
            btnTextures.TabIndex = 9;
            btnTextures.Text = "Textures";
            btnTextures.UseVisualStyleBackColor = true;
            btnTextures.Click += new EventHandler(btnTextures_Click);
            // 
            // ctxAnim
            // 
            ctxAnim.ImageScalingSize = new System.Drawing.Size(20, 20);
            ctxAnim.Items.AddRange(new ToolStripItem[]
            {
                toolStripMenuItem2,
                toolStripSeparator1,
                chkLoop,
                matrixModeToolStripMenuItem,
                toolStripMenuItem3,
                toolStripMenuItem4,
                renameToolStripMenuItem,
                _deleteToolStripMenuItem,
                createNewToolStripMenuItem
            });
            ctxAnim.Name = "ctxAnim";
            ctxAnim.Size = new System.Drawing.Size(235, 218);
            ctxAnim.Opening += new CancelEventHandler(ctxAnim_Opening);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Enabled = false;
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(234, 26);
            toolStripMenuItem2.Text = "Source";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // chkLoop
            // 
            chkLoop.CheckOnClick = true;
            chkLoop.Name = "chkLoop";
            chkLoop.Size = new System.Drawing.Size(234, 26);
            chkLoop.Text = "Loop";
            chkLoop.CheckedChanged += new EventHandler(chkLoop_CheckedChanged);
            // 
            // matrixModeToolStripMenuItem
            // 
            matrixModeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                chkMtxMaya,
                chkMtxXSI,
                chkMtxMax
            });
            matrixModeToolStripMenuItem.Name = "matrixModeToolStripMenuItem";
            matrixModeToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            matrixModeToolStripMenuItem.Text = "Matrix Mode";
            matrixModeToolStripMenuItem.Visible = false;
            // 
            // chkMtxMaya
            // 
            chkMtxMaya.CheckOnClick = true;
            chkMtxMaya.Name = "chkMtxMaya";
            chkMtxMaya.Size = new System.Drawing.Size(139, 26);
            chkMtxMaya.Text = "Maya";
            chkMtxMaya.CheckedChanged += new EventHandler(chkMtxMaya_CheckedChanged);
            // 
            // chkMtxXSI
            // 
            chkMtxXSI.CheckOnClick = true;
            chkMtxXSI.Name = "chkMtxXSI";
            chkMtxXSI.Size = new System.Drawing.Size(139, 26);
            chkMtxXSI.Text = "XSI";
            chkMtxXSI.CheckedChanged += new EventHandler(chkMtxXSI_CheckedChanged);
            // 
            // chkMtxMax
            // 
            chkMtxMax.CheckOnClick = true;
            chkMtxMax.Name = "chkMtxMax";
            chkMtxMax.Size = new System.Drawing.Size(139, 26);
            chkMtxMax.Text = "3ds Max";
            chkMtxMax.CheckedChanged += new EventHandler(chkMtxMax_CheckedChanged);
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(234, 26);
            toolStripMenuItem3.Text = "Export...";
            toolStripMenuItem3.Click += new EventHandler(exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(234, 26);
            toolStripMenuItem4.Text = "Replace...";
            toolStripMenuItem4.Click += new EventHandler(_replaceToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += new EventHandler(renameToolStripMenuItem_Click);
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            _deleteToolStripMenuItem.Text = "Delete";
            _deleteToolStripMenuItem.Click += new EventHandler(_deleteToolStripMenuItem_Click);
            // 
            // createNewToolStripMenuItem
            // 
            createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            createNewToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            createNewToolStripMenuItem.Text = "Create New Animation";
            createNewToolStripMenuItem.Click += new EventHandler(createNewToolStripMenuItem_Click);
            // 
            // overObjPnl
            // 
            overObjPnl.Dock = DockStyle.Fill;
            overObjPnl.Location = new System.Drawing.Point(0, 66);
            overObjPnl.Name = "overObjPnl";
            overObjPnl.Size = new System.Drawing.Size(170, 45);
            overObjPnl.TabIndex = 8;
            overObjPnl.Paint += new PaintEventHandler(overObjPnl_Paint);
            // 
            // spltObjTex
            // 
            spltObjTex.Cursor = Cursors.HSplit;
            spltObjTex.Dock = DockStyle.Bottom;
            spltObjTex.Location = new System.Drawing.Point(0, 332);
            spltObjTex.Name = "spltObjTex";
            spltObjTex.Size = new System.Drawing.Size(172, 4);
            spltObjTex.TabIndex = 4;
            spltObjTex.Dragged += new SplitterEventHandler(spltObjTex_Dragged);
            // 
            // spltAnimObj
            // 
            spltAnimObj.Cursor = Cursors.HSplit;
            spltAnimObj.Dock = DockStyle.Top;
            spltAnimObj.Location = new System.Drawing.Point(0, 178);
            spltAnimObj.Name = "spltAnimObj";
            spltAnimObj.Size = new System.Drawing.Size(172, 4);
            spltAnimObj.TabIndex = 1;
            spltAnimObj.Dragged += new SplitterEventHandler(spltAnimObj_Dragged);
            // 
            // overTexPnl
            // 
            overTexPnl.Dock = DockStyle.Fill;
            overTexPnl.Location = new System.Drawing.Point(0, 46);
            overTexPnl.Name = "overTexPnl";
            overTexPnl.Size = new System.Drawing.Size(170, 116);
            overTexPnl.TabIndex = 9;
            overTexPnl.Paint += new PaintEventHandler(overTexPnl_Paint);
            // 
            // LeftPanel
            // 
            Controls.Add(pnlObjects);
            Controls.Add(spltObjTex);
            Controls.Add(spltAnimObj);
            Controls.Add(pnlAnims);
            Controls.Add(pnlTextures);
            Name = "LeftPanel";
            Size = new System.Drawing.Size(172, 500);
            pnlObjects.ResumeLayout(false);
            pnlAnims.ResumeLayout(false);
            ctxAnimList.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            ctxTextures.ResumeLayout(false);
            pnlTextures.ResumeLayout(false);
            ctxAnim.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public bool _closing = false;

        public ModelEditControl.ModelEditControl _mainWindow;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModelEditControl.ModelEditControl MainWindow
        {
            get => _mainWindow;
            set => _mainWindow = value;
        }

        public ListViewGroup _AnimGroupBRRES = new ListViewGroup("In BRRES");

        public ListViewGroup _AnimGroupNotBRRES = new ListViewGroup("Not In BRRES");

        //private ListViewGroup _AnimGroupExternal = new ListViewGroup("External File");
        public List<ListViewGroup> _AnimGroupsExternal = new List<ListViewGroup>();

        public bool _syncPat0 = false;
        private bool _updating;
        public string _lastSelected = null;
        public SRT0Node _srt0Selection;
        public PAT0Node _pat0Selection;
        private IObject _selectedObject;
        private MDL0TextureNode _selectedTexture;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IObject SelectedObject
        {
            get => _selectedObject;
            set => lstObjects.SelectedItem = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0TextureNode SelectedTexture
        {
            get => _selectedTexture;
            set => lstTextures.SelectedItem = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef
        {
            get => _mainWindow.TargetTexRef;
            set
            {
                _mainWindow.TargetTexRef = value;
                if (_mainWindow.SelectedSRT0 != null && TargetTexRef != null && _mainWindow.KeyframePanel != null)
                {
                    _mainWindow.KeyframePanel.TargetSequence = _mainWindow.SRT0Editor.TexEntry;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get => _mainWindow.CurrentFrame;
            set => _mainWindow.CurrentFrame = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetCollision
        {
            get => _mainWindow.TargetCollision;
            set => _mainWindow.TargetCollision = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedCHR0
        {
            get => _mainWindow.SelectedCHR0;
            set => _mainWindow.SelectedCHR0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0Node SelectedSRT0
        {
            get => _mainWindow.SelectedSRT0;
            set => _mainWindow.SelectedSRT0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SHP0Node SelectedSHP0
        {
            get => _mainWindow.SelectedSHP0;
            set => _mainWindow.SelectedSHP0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PAT0Node SelectedPAT0
        {
            get => _mainWindow.SelectedPAT0;
            set => _mainWindow.SelectedPAT0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0Node SelectedVIS0
        {
            get => _mainWindow.SelectedVIS0;
            set => _mainWindow.SelectedVIS0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NW4RAnimType TargetAnimType
        {
            get => (NW4RAnimType) fileType.SelectedIndex;
            set
            {
                if (fileType.SelectedIndex != (int) value)
                {
                    fileType.SelectedIndex = (int) value;
                }
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
            bool ib = inBRRES || TargetModel is MDL0Node && node == ((MDL0Node) TargetModel).BRESNode;

            if (!_mainWindow.chkBRRESAnims.Checked && ib)
            {
                return false;
            }

            bool found = false;
            switch (node.ResourceFileType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.U8:
                case ResourceType.U8Folder:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                    {
                        found = LoadAnims(n, type, compare, contains, ib, externalGroup) || found;
                    }

                    break;
                case ResourceType.CHR0:
                    found = true;
                    if (type == NW4RAnimType.CHR)
                    {
                        goto Add;
                    }

                    break;
                case ResourceType.SRT0:
                    found = true;
                    if (type == NW4RAnimType.SRT)
                    {
                        goto Add;
                    }

                    break;
                case ResourceType.SHP0:
                    found = true;
                    if (type == NW4RAnimType.SHP)
                    {
                        goto Add;
                    }

                    break;
                case ResourceType.PAT0:
                    found = true;
                    if (type == NW4RAnimType.PAT)
                    {
                        goto Add;
                    }

                    break;
                case ResourceType.VIS0:
                    found = true;
                    if (type == NW4RAnimType.VIS)
                    {
                        goto Add;
                    }

                    break;
                case ResourceType.SCN0:
                    found = true;
                    if (type == NW4RAnimType.SCN)
                    {
                        goto Add;
                    }

                    break;
                case ResourceType.CLR0:
                    found = true;
                    if (type == NW4RAnimType.CLR)
                    {
                        goto Add;
                    }

                    break;
            }

            return found;

            Add:
            if (string.IsNullOrEmpty(compare) ||
                contains && node.Name.Contains(compare, StringComparison.OrdinalIgnoreCase) ||
                node.Name.StartsWith(compare, StringComparison.OrdinalIgnoreCase))
            {
                ListViewGroup u = externalGroup ?? (ib ? _AnimGroupBRRES : _AnimGroupNotBRRES);
                if (u == null)
                {
                    Console.WriteLine();
                }

                listAnims.Items.Add(new ListViewItem(node.Name, (int) node.ResourceFileType, u) {Tag = node});
            }

            return found;
        }

        public void UpdateAnimations()
        {
            UpdateAnimations(TargetAnimType);
        }

        public void UpdateAnimations(NW4RAnimType type)
        {
            _mainWindow.Updating = true;

            string name = listAnims.SelectedItems != null && listAnims.SelectedItems.Count > 0
                ? listAnims.SelectedItems[0].Tag.ToString()
                : null;
            int frame = CurrentFrame;

            string text = txtSearchAnim.Text;
            bool addAll = string.IsNullOrEmpty(text) || txtSearchAnim.ForeColor == Color.Gray;

            listAnims.BeginUpdate();
            listAnims.Items.Clear();

            listAnims.Groups.Clear();

            if (TargetModel is MDL0Node)
            {
                _AnimGroupBRRES.Header = string.Format("In BRRES ({0})", TargetModel);
                _AnimGroupNotBRRES.Header = string.Format("Not in BRRES ({0})", TargetModel);

                listAnims.Groups.Add(_AnimGroupBRRES);
                listAnims.Groups.Add(_AnimGroupNotBRRES);

                ResourceNode node = _mainWindow.chkNonBRRESAnims.Checked
                    ? ((MDL0Node) TargetModel).RootNode
                    : ((MDL0Node) TargetModel).BRESNode;
                LoadAnims(node, type, addAll ? null : text, chkContains.Checked, false);
            }

            if (_mainWindow.chkExternalAnims.Checked)
            {
                ResourceNode root = TargetModel == null ? null : ((ResourceNode) TargetModel).RootNode;
                foreach (ResourceNode r in _mainWindow._openedFiles)
                {
                    if (r != root && r != null)
                    {
                        ListViewGroup g = new ListViewGroup(r.Name);
                        listAnims.Groups.Add(g);
                        _AnimGroupsExternal.Add(g);
                        LoadAnims(r, type, addAll ? null : text, chkContains.Checked, false, g);
                    }
                }
            }

            listAnims.EndUpdate();

            //Reselect the animation
            for (int i = 0; i < listAnims.Items.Count; i++)
            {
                if (listAnims.Items[i].Tag.ToString() == name)
                {
                    listAnims.Items[i].Selected = true;
                    break;
                }
            }

            _mainWindow.Updating = false;
            CurrentFrame = frame;

            if (_mainWindow.GetAnimation(TargetAnimType) == null && listAnims.SelectedItems.Count == 0)
            {
                _mainWindow.GetFiles(NW4RAnimType.None);
            }
        }

        public void LoadAnimations(ResourceNode node)
        {
            if (_mainWindow.chkExternalAnims.Checked && node != null)
            {
                ResourceNode root = TargetModel == null ? null : ((ResourceNode) TargetModel).RootNode;
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
            {
                overObjPnl.Visible = overTexPnl.Visible = false;
            }
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
            {
                overObjPnl.Visible = overTexPnl.Visible = false;
            }
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

            string Name = lstTextures.SelectedItems != null && lstTextures.SelectedItems.Count > 0
                ? lstTextures.SelectedItems[0].ToString()
                : null;

            lstTextures.BeginUpdate();
            lstTextures.Items.Clear();

            chkAllTextures.CheckState = CheckState.Checked;

            ResourceNode n;
            if (_selectedObject != null && _mainWindow.SyncTexturesToObjectList)
            {
                //Add textures the selected object uses
                if (_selectedObject is MDL0ObjectNode)
                {
                    foreach (MDL0MaterialRefNode tref in ((MDL0ObjectNode) _selectedObject)
                                                         ._drawCalls[0].MaterialNode.Children)
                    {
                        lstTextures.Items.Add(tref.TextureNode, tref.TextureNode.Enabled);
                    }
                }
            }
            else
            {
                //Add all model textures
                if (TargetModel is MDL0Node && (n = ((ResourceNode) TargetModel).FindChild("Textures", false)) != null)
                {
                    foreach (MDL0TextureNode tref in n.Children)
                    {
                        lstTextures.Items.Add(tref, tref.Enabled);
                    }
                }
            }

            lstTextures.EndUpdate();

            //Reselect the animation
            //lstTextures.Focus();
            for (int i = 0; i < lstTextures.Items.Count; i++)
            {
                if (lstTextures.Items[i].ToString() == Name)
                {
                    lstTextures.SetSelected(i, true);
                    break;
                }
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

            pnlAnims.Enabled = pnlTextures.Enabled = chkSyncVis.Enabled = TargetCollision == null;

            if (TargetCollision != null)
            {
                foreach (CollisionObject obj in TargetCollision.Children)
                {
                    lstObjects.Items.Add(obj, obj._render);
                }
            }
            else if (TargetModel != null)
            {
                UpdateAnimations(TargetAnimType);
                if (TargetModel is MDL0Node)
                {
                    MDL0Node model = TargetModel as MDL0Node;

                    if (model._objList != null)
                    {
                        foreach (MDL0ObjectNode poly in model._objList)
                        {
                            lstObjects.Items.Add(poly, poly.IsRendering);
                        }
                    }

                    if (model._texList != null)
                    {
                        foreach (MDL0TextureNode tref in model._texList)
                        {
                            lstTextures.Items.Add(tref, tref.Enabled);
                        }
                    }
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
            {
                return;
            }

            int texY = pnlTextures.Location.Y;
            int objY = pnlObjects.Location.Y;

            int height = -1;
            if (objY + btnObjects.Height + e.Y >= texY - 6)
            {
                int difference = objY + btnObjects.Height + e.Y - (texY - 6);
                if (texY - 6 - e.Y <= objY + btnObjects.Height)
                {
                    if (e.Y > 0) //Only want to push the texture panel down
                    {
                        height = pnlTextures.Height;
                        pnlTextures.Height -= difference;
                    }
                }
            }

            if (height != pnlTextures.Height)
            {
                pnlAnims.Height += e.Y;
            }
        }

        private void spltObjTex_Dragged(object sender, SplitterEventArgs e)
        {
            if (e.Y == 0)
            {
                return;
            }

            int texY = pnlTextures.Location.Y;
            int objY = pnlObjects.Location.Y;

            int height = -1;
            if (texY - 6 + e.Y <= objY + btnObjects.Height)
            {
                int difference = objY + btnObjects.Height - (texY - 6 + e.Y);
                if (objY + btnObjects.Height - e.Y >= texY - 6)
                {
                    if (e.Y < 0) //Only want to push the anims panel up
                    {
                        height = pnlAnims.Height;
                        pnlAnims.Height -= difference;
                    }
                }
            }

            if (height != pnlAnims.Height)
            {
                pnlTextures.Height -= e.Y;
            }
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
                    {
                        lstDrawCalls.SetItemChecked(i, o._drawCalls[i]._render);
                    }

                    _updating = false;
                }
            }
            else
            {
                lstDrawCalls.Visible = spltDrawCalls.Visible = false;
            }

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
            {
                return;
            }

            MDL0ObjectNode poly = lstObjects.Items[e.Index] as MDL0ObjectNode;

            if (poly != null)
            {
                bool matches = poly.IsRendering == (e.NewValue == CheckState.Checked);

                if (!matches)
                {
                    poly.IsRendering = e.NewValue == CheckState.Checked;
                }

                if (poly == SelectedObject)
                {
                    _updating = true;
                    for (int i = 0; i < lstDrawCalls.Items.Count; i++)
                    {
                        if (i < poly._drawCalls.Count)
                        {
                            lstDrawCalls.SetItemChecked(i, poly._drawCalls[i]._render);
                        }
                    }

                    _updating = false;
                }

                if (!matches && chkSyncVis.Checked)
                {
                    ChecksChanged(poly);
                }
            }

            CollisionObject obj = lstObjects.Items[e.Index] as CollisionObject;
            if (obj != null)
            {
                obj._render = e.NewValue == CheckState.Checked;
            }

            if (!_updating)
            {
                _mainWindow.ModelPanel.Invalidate();
            }
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
            {
                if (_mainWindow.VIS0Indices.ContainsKey(d.VisibilityBone))
                {
                    Dictionary<int, List<int>> objects = _mainWindow.VIS0Indices[d.VisibilityBone];
                    foreach (KeyValuePair<int, List<int>> i in objects)
                    {
                        if (i.Key < 0 || i.Key >= lstObjects.Items.Count || i.Value == null)
                        {
                            continue;
                        }

                        MDL0ObjectNode o = (MDL0ObjectNode) lstObjects.Items[i.Key];
                        foreach (int call in i.Value)
                        {
                            DrawCall other = o._drawCalls[call];
                            if (other._render != d._render && other != d)
                            {
                                other._render = d._render;
                            }
                        }

                        UpdateObjectCheckState(i.Key);
                    }
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
                {
                    for (int i = 0; i < poly._drawCalls.Count; i++)
                    {
                        _mainWindow.UpdateVis0(_polyIndex, i, poly._drawCalls[i]._render);
                    }
                }
            }
        }

        private void lstTextures_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_selectedTexture != null)
            {
                _selectedTexture.Selected = false;
            }

            if ((_selectedTexture = lstTextures.SelectedItem as MDL0TextureNode) != null)
            {
                _selectedTexture.Selected = true;

                overObjPnl.Invalidate();
                overTexPnl.Invalidate();

                if (_mainWindow.SyncTexturesToObjectList)
                {
                    _selectedTexture.ObjOnly = true;
                }

                if ((_selectedObject as MDL0ObjectNode)?._drawCalls[0].MaterialNode != null)
                {
                    TargetTexRef = _selectedObject != null
                        ? ((MDL0ObjectNode) _selectedObject)
                          ._drawCalls[0].MaterialNode.FindChild(_selectedTexture.Name, true) as MDL0MaterialRefNode
                        : null;
                }
            }

            if (!_updating)
            {
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void chkAllPoly_CheckStateChanged(object sender, EventArgs e)
        {
            if (lstObjects.Items.Count == 0)
            {
                return;
            }

            //_updating = true;

            lstObjects.BeginUpdate();
            for (int i = 0; i < lstObjects.Items.Count; i++)
            {
                lstObjects.SetItemCheckState(i, chkAllObj.CheckState);
            }

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
                pnlObjects.Height = (int) pnlObjects.Tag;
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
                pnlTextures.Height = (int) pnlTextures.Tag;
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
                pnlAnims.Height = (int) pnlAnims.Tag;
                listAnims.Visible = fileType.Visible = spltAnimObj.Visible = true;
            }
        }

        private void lstTextures_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0TextureNode tref = lstTextures.Items[e.Index] as MDL0TextureNode;

            tref.Enabled = e.NewValue == CheckState.Checked;

            if (!_updating)
            {
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void chkAllTextures_CheckStateChanged(object sender, EventArgs e)
        {
            _updating = true;

            lstTextures.BeginUpdate();
            for (int i = 0; i < lstTextures.Items.Count; i++)
            {
                lstTextures.SetItemCheckState(i, chkAllTextures.CheckState);
            }

            lstTextures.EndUpdate();

            _updating = false;

            _mainWindow.ModelPanel.Invalidate();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTexture == null)
            {
                return;
            }

            new GLTextureWindow().Show(this, _selectedTexture.Texture);
        }

        private void replaceTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = lstTextures.SelectedIndex;
            if (_selectedTexture?.Source is TEX0Node node)
            {
                if (node.Parent == null)
                {
                    return;
                }

                OpenFileDialog _openDlg = new OpenFileDialog
                {
                    Filter = FileFilters.TEX0
                };
#if !DEBUG
                try
                {
#endif
                    if (_openDlg.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = _openDlg.FileName;
                        node.Replace(fileName);
                        _updating = true;
                        _selectedTexture.Reload(_selectedTexture.Model,
                            _selectedTexture.Parent?.Name.EndsWith("_ExtMtl") ?? false);
                        lstTextures.SetItemCheckState(index, CheckState.Checked);
                        lstTextures.SetSelected(index, false);
                        _updating = false;

                        _mainWindow.ModelPanel.Invalidate();
                    }
#if !DEBUG
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    index = 0;
                }
#endif
            }
        }

        private void ctxTextures_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedTexture == null)
            {
                e.Cancel = true;
            }
            else
            {
                if (_selectedTexture.Source is TEX0Node)
                {
                    viewToolStripMenuItem.Enabled = true;
                    replaceTextureToolStripMenuItem.Enabled = true;
                    exportTextureToolStripMenuItem.Enabled = true;
                    sourceToolStripMenuItem.Text = string.Format("Source: {0}",
                        Path.GetFileName(((ResourceNode) _selectedTexture.Source).RootNode._origPath));
                }
                else if (_selectedTexture.Source is string)
                {
                    viewToolStripMenuItem.Enabled = true;
                    replaceTextureToolStripMenuItem.Enabled = false;
                    exportTextureToolStripMenuItem.Enabled = false;
                    sourceToolStripMenuItem.Text = string.Format("Source: {0}", (string) _selectedTexture.Source);
                }
                else
                {
                    viewToolStripMenuItem.Enabled = false;
                    replaceTextureToolStripMenuItem.Enabled = false;
                    exportTextureToolStripMenuItem.Enabled = false;
                    sourceToolStripMenuItem.Text = "Source: <Not Found>";
                }

                if (_selectedTexture.Texture != null)
                {
                    sizeToolStripMenuItem.Text = string.Format("Size: {0} x {1}", _selectedTexture.Texture.Width,
                        _selectedTexture.Texture.Height);
                }
                else
                {
                    sizeToolStripMenuItem.Text = "Size: <Not Found>";
                }
            }
        }

        private void lstTextures_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstTextures.IndexFromPoint(e.X, e.Y);
            if (lstTextures.SelectedIndex != index)
            {
                lstTextures.SelectedIndex = index;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (_selectedTexture != null)
                {
                    lstTextures.ContextMenuStrip = ctxTextures;
                }
                else
                {
                    lstTextures.ContextMenuStrip = null;
                }
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTexture != null)
            {
                _selectedTexture.Reload(_selectedTexture.Model,
                    _selectedTexture.Parent?.Name.EndsWith("_ExtMtl") ?? false);
                _mainWindow.ModelPanel.Invalidate();
            }
        }

        private void lstTextures_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lstTextures.SelectedItem = null;
            }
        }

        private void lstPolygons_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lstObjects.SelectedItem = null;
            }
        }

        private void listAnims_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                listAnims.SelectedItems.Clear();
            }
        }

        private void exportTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTexture?.Source is TEX0Node)
            {
                TEX0Node node = _selectedTexture.Source as TEX0Node;
                using (SaveFileDialog dlgSave = new SaveFileDialog())
                {
                    dlgSave.FileName = node.Name;
                    dlgSave.Filter = FileFilters.TEX0;
                    if (dlgSave.ShowDialog(this) == DialogResult.OK)
                    {
                        node.Export(dlgSave.FileName);
                    }
                }
            }
        }

        private void renameTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
            {
                dlg.ShowDialog(ParentForm, _selectedTexture.Source as TEX0Node);
            }

            _selectedTexture.Name = (_selectedTexture.Source as TEX0Node).Name;
        }

        private void listAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_closing)
            {
                return;
            }

            _updating = true;
            if (listAnims.SelectedItems.Count > 0)
            {
                createNewToolStripMenuItem.Text = string.Format("Create New {0}0", TargetAnimType.ToString());

                NW4RAnimationNode n = listAnims.SelectedItems[0].Tag as NW4RAnimationNode;
                if (n != null)
                {
                    chkLoop.Checked = n.Loop;
                }

                _mainWindow.TargetAnimation = n;
            }
            else
            {
                _mainWindow.TargetAnimation = null;
            }

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
            {
                e.Cancel = true;
            }
            else
            {
                sourceToolStripMenuItem.Text = string.Format("Source: {0}",
                    Path.GetFileName(_mainWindow.TargetAnimation.RootNode._origPath));
                if (matrixModeToolStripMenuItem.Visible = _mainWindow.TargetAnimation is SRT0Node)
                {
                    SRT0Node node = (SRT0Node) _mainWindow.TargetAnimation;
                    chkMtxMax.Checked = node.MatrixMode == TexMatrixMode.Matrix3dsMax;
                    chkMtxXSI.Checked = node.MatrixMode == TexMatrixMode.MatrixXSI;
                    chkMtxMaya.Checked = node.MatrixMode == TexMatrixMode.MatrixMaya;
                }
            }
        }

        private readonly SaveFileDialog dlgSave = new SaveFileDialog();
        private readonly OpenFileDialog dlgOpen = new OpenFileDialog();

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BRESEntryNode node;
            if ((node = _mainWindow.TargetAnimation as BRESEntryNode) == null)
            {
                return;
            }

            dlgSave.FileName = node.Name;
            switch (TargetAnimType)
            {
                case NW4RAnimType.CHR:
                    dlgSave.Filter = FileFilters.CHR0Export;
                    break;
                case NW4RAnimType.SRT:
                    dlgSave.Filter = FileFilters.SRT0;
                    break;
                case NW4RAnimType.SHP:
                    dlgSave.Filter = FileFilters.SHP0;
                    break;
                case NW4RAnimType.PAT:
                    dlgSave.Filter = FileFilters.PAT0;
                    break;
                case NW4RAnimType.VIS:
                    dlgSave.Filter = FileFilters.VIS0;
                    break;
                case NW4RAnimType.SCN:
                    dlgSave.Filter = FileFilters.SCN0;
                    break;
                case NW4RAnimType.CLR:
                    dlgSave.Filter = FileFilters.CLR0;
                    break;
            }

            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                node.Export(dlgSave.FileName);
            }
        }

        private void _replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BRESEntryNode node;
            if ((node = _mainWindow.TargetAnimation as BRESEntryNode) == null)
            {
                return;
            }

            switch (TargetAnimType)
            {
                case NW4RAnimType.CHR:
                    dlgOpen.Filter = FileFilters.CHR0Import;
                    break;
                case NW4RAnimType.SRT:
                    dlgOpen.Filter = FileFilters.SRT0;
                    break;
                case NW4RAnimType.SHP:
                    dlgOpen.Filter = FileFilters.SHP0;
                    break;
                case NW4RAnimType.PAT:
                    dlgOpen.Filter = FileFilters.PAT0;
                    break;
                case NW4RAnimType.VIS:
                    dlgOpen.Filter = FileFilters.VIS0;
                    break;
                case NW4RAnimType.SCN:
                    dlgOpen.Filter = FileFilters.SCN0;
                    break;
                case NW4RAnimType.CLR:
                    dlgOpen.Filter = FileFilters.CLR0;
                    break;
            }

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                node.Replace(dlgOpen.FileName);
            }
        }

        private unsafe void portToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TargetAnimType != 0 || SelectedCHR0 == null || !(TargetModel is MDL0Node))
            {
                return;
            }

            SelectedCHR0.Port(TargetModel as MDL0Node);

            _mainWindow.UpdateModel();
        }

        #endregion

        private void listAnims_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_mainWindow.TargetAnimation != null)
                {
                    listAnims.ContextMenuStrip = ctxAnim;
                }
                else
                {
                    listAnims.ContextMenuStrip = ctxAnimList;
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RenameDialog dlg = new RenameDialog())
            {
                dlg.ShowDialog(ParentForm, _mainWindow.TargetAnimation);
            }
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r;
            if ((r = _mainWindow.TargetAnimation) != null)
            {
                switch (TargetAnimType)
                {
                    case NW4RAnimType.CHR:
                        ((BRRESNode) r.Parent.Parent).CreateResource<CHR0Node>("NewCHR");
                        break;
                    case NW4RAnimType.SRT:
                        ((BRRESNode) r.Parent.Parent).CreateResource<SRT0Node>("NewSRT");
                        break;
                    case NW4RAnimType.SHP:
                        ((BRRESNode) r.Parent.Parent).CreateResource<SHP0Node>("NewSHP");
                        break;
                    case NW4RAnimType.PAT:
                        ((BRRESNode) r.Parent.Parent).CreateResource<PAT0Node>("NewPAT");
                        break;
                    case NW4RAnimType.VIS:
                        ((BRRESNode) r.Parent.Parent).CreateResource<VIS0Node>("NewVIS");
                        break;
                    case NW4RAnimType.SCN:
                        ((BRRESNode) r.Parent.Parent).CreateResource<SCN0Node>("NewSCN");
                        break;
                    case NW4RAnimType.CLR:
                        ((BRRESNode) r.Parent.Parent).CreateResource<CLR0Node>("NewCLR");
                        break;
                }
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
            {
                return;
            }

            Graphics g = e.Graphics;
            for (int i = 0; i < lstObjects.Items.Count; i++)
            {
                MDL0ObjectNode poly = lstObjects.Items[i] as MDL0ObjectNode;
                if (poly != null)
                {
                    foreach (DrawCall c in poly._drawCalls)
                    {
                        if (c.MaterialNode != null)
                        {
                            if (_srt0Selection != null)
                            {
                                if (_srt0Selection.FindChildByType(c.MaterialNode.Name, false,
                                        ResourceType.SRT0Entry) != null)
                                {
                                    Rectangle r = lstObjects.GetItemRectangle(i);
                                    g.DrawRectangle(Pens.Black, r);
                                }
                            }
                            else
                            {
                                if (_pat0Selection?.FindChildByType(c.MaterialNode.Name, false,
                                        ResourceType.PAT0Entry) != null)
                                {
                                    Rectangle r = lstObjects.GetItemRectangle(i);
                                    g.DrawRectangle(Pens.Black, r);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void overTexPnl_Paint(object sender, PaintEventArgs e)
        {
            if (_srt0Selection == null && _pat0Selection == null)
            {
                return;
            }

            Graphics g = e.Graphics;
            ResourceNode rn = null;

            if (_selectedObject is MDL0ObjectNode)
            {
                for (int i = 0; i < lstTextures.Items.Count; i++)
                {
                    MDL0ObjectNode obj = _selectedObject as MDL0ObjectNode;
                    MDL0TextureNode tex = lstTextures.Items[i] as MDL0TextureNode;
                    foreach (DrawCall c in obj._drawCalls)
                    {
                        if (c.MaterialNode != null)
                        {
                            if ((rn = c.MaterialNode.FindChild(tex.Name, true)) != null)
                            {
                                if (_srt0Selection != null)
                                {
                                    if (_srt0Selection.FindChildByType(c.MaterialNode.Name + "/Texture" + rn.Index,
                                            false, ResourceType.SRT0Texture) != null)
                                    {
                                        Rectangle r = lstTextures.GetItemRectangle(i);
                                        g.DrawRectangle(Pens.Black, r);
                                    }
                                }
                                else
                                {
                                    if (_pat0Selection?.FindChildByType(c.MaterialNode.Name + "/Texture" + rn.Index,
                                            false, ResourceType.PAT0Texture) != null)
                                    {
                                        Rectangle r = lstTextures.GetItemRectangle(i);
                                        g.DrawRectangle(Pens.Black, r);
                                    }
                                }
                            }
                        }
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

        private void _deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.TargetAnimation.Remove();
            _mainWindow.TargetAnimation = null;
            UpdateAnimations();
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            NW4RAnimationNode n = _mainWindow.TargetAnimation;
            if (n != null)
            {
                if (ModelEditorBase.Interpolated.Contains(n.GetType()))
                {
                    _mainWindow.Updating = true;
                    bool loopPrev = n.Loop;
                    if ((n.Loop = chkLoop.Checked) != loopPrev)
                    {
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
                    }

                    _mainWindow.Updating = false;
                }
                else
                {
                    n.Loop = chkLoop.Checked;
                }
            }
        }

        private void inModelsBRRESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r = TargetModel as ResourceNode;
            if (!(r?.Parent?.Parent is BRRESNode))
            {
                return;
            }

            AddAnimation((BRRESNode) r.Parent.Parent);
        }

        private void AddAnimation(BRRESNode target)
        {
            Type t = ModelEditorBase.AnimTypeList[(int) TargetAnimType];
            System.Reflection.MethodInfo method = typeof(BRRESNode).GetMethod("CreateResource");
            System.Reflection.MethodInfo generic = method.MakeGenericMethod(t);
            generic.Invoke(target, new object[] {"New" + TargetAnimType});
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
            bool targetBRRES = !!(r?.Parent?.Parent is BRRESNode);

            if (!targetBRRES)
            {
                e.Cancel = true;
            }

            //inModelsBRRESToolStripMenuItem.Enabled = targetBRRES;
            //inExternalFileToolStripMenuItem.Enabled = _mainWindow.ExternalAnimationsNode != null;
        }

        private void chkMtxMaya_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;

            if (chkMtxMaya.Checked)
            {
                SRT0Node node = (SRT0Node) _mainWindow.TargetAnimation;
                node.MatrixMode = TexMatrixMode.MatrixMaya;
                chkMtxMax.Checked = chkMtxXSI.Checked = false;
            }
            else
            {
                chkMtxMaya.Checked = true;
            }

            _updating = false;
        }

        private void chkMtxXSI_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;

            if (chkMtxXSI.Checked)
            {
                SRT0Node node = (SRT0Node) _mainWindow.TargetAnimation;
                node.MatrixMode = TexMatrixMode.MatrixXSI;
                chkMtxMax.Checked = chkMtxMaya.Checked = false;
            }
            else
            {
                chkMtxXSI.Checked = true;
            }

            _updating = false;
        }

        private void chkMtxMax_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;

            if (chkMtxMax.Checked)
            {
                SRT0Node node = (SRT0Node) _mainWindow.TargetAnimation;
                node.MatrixMode = TexMatrixMode.Matrix3dsMax;
                chkMtxXSI.Checked = chkMtxMaya.Checked = false;
            }
            else
            {
                chkMtxMax.Checked = true;
            }

            _updating = false;
        }

        private void txtSearchAnim_Enter(object sender, EventArgs e)
        {
            if (txtSearchAnim.ForeColor == Color.Gray)
            {
                txtSearchAnim.Text = "";
                txtSearchAnim.Font = new Font(txtSearchAnim.Font, FontStyle.Regular);
                txtSearchAnim.ForeColor = Color.Black;
            }
        }

        private void txtSearchAnim_Leave(object sender, EventArgs e)
        {
            if (txtSearchAnim.Text == string.Empty)
            {
                txtSearchAnim.Font = new Font(txtSearchAnim.Font, FontStyle.Italic);
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
            {
                return;
            }

            DrawCall c = lstDrawCalls.SelectedItem as DrawCall;
            if (c != null)
            {
                c._render = !c._render;
                UpdateObjectCheckState(lstObjects.SelectedIndex);
                if (chkSyncVis.Checked)
                {
                    ChecksChanged(c._parentObject);
                }

                _mainWindow.ModelPanel.Invalidate();
            }
        }

        public void UpdateObjectCheckState(int i)
        {
            if (i < 0 || i > lstObjects.Items.Count)
            {
                return;
            }

            MDL0ObjectNode o = lstObjects.Items[i] as MDL0ObjectNode;
            if (o == null)
            {
                return;
            }

            CheckState s = CheckState.Indeterminate;
            bool someRendering = false, someNotRendering = false;
            foreach (DrawCall x in o._drawCalls)
            {
                if (x._render)
                {
                    someRendering = true;
                }
                else
                {
                    someNotRendering = true;
                }

                if (someNotRendering && someRendering)
                {
                    break;
                }
            }

            if (someRendering && !someNotRendering)
            {
                s = CheckState.Checked;
            }
            else if (!someRendering && someNotRendering)
            {
                s = CheckState.Unchecked;
            }

            _updating = true;
            lstObjects.SetItemCheckState(i, s);
            _updating = false;
        }

        private void lstDrawCalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawCall c = lstDrawCalls.SelectedItem as DrawCall;
            if (c?.MaterialNode != null)
            {
                TargetTexRef = _selectedTexture != null
                    ? c.MaterialNode.FindChild(_selectedTexture.Name, true) as MDL0MaterialRefNode
                    : null;
            }
        }

        private void lstDrawCalls_DoubleClick(object sender, EventArgs e)
        {
        }

        public void SetRenderState(int objKey, int i, bool render, MDL0ObjectNode obj)
        {
            if (lstObjects.SelectedIndex == objKey)
            {
                lstDrawCalls.SetItemChecked(i, render);
            }
            else
            {
                obj._drawCalls[i]._render = render;
                UpdateObjectCheckState(objKey);
            }
        }
    }
}
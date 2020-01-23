using System.Windows.Forms;
using System;

namespace BrawlLib.Internal.Windows.Controls.Hex_Editor
{
    unsafe partial class SectionEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SectionEditor));
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.chkBSSSection = new System.Windows.Forms.CheckBox();
            this.chkCodeSection = new System.Windows.Forms.CheckBox();
            this.grpRelocInfo = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstLinked = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNewCmd = new System.Windows.Forms.Button();
            this.btnDelCmd = new System.Windows.Forms.Button();
            this.btnOpenTarget = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlFunctions = new System.Windows.Forms.Panel();
            this.chkUnresolved = new System.Windows.Forms.CheckBox();
            this.chkDestructor = new System.Windows.Forms.CheckBox();
            this.chkConstructor = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRemoveWord = new System.Windows.Forms.Button();
            this.btnInsertWord = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportInitializedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteOverwriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findPreviousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highlightBlr = new System.Windows.Forms.ToolStripMenuItem();
            this.displayInitialized = new System.Windows.Forms.ToolStripMenuItem();
            this.displayStringsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkAnnotations = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlHexEditor = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.annotationDescription = new System.Windows.Forms.RichTextBox();
            this.btn1underline = new System.Windows.Forms.CheckBox();
            this.btn2underline = new System.Windows.Forms.CheckBox();
            this.btn3underline = new System.Windows.Forms.CheckBox();
            this.btn4underline = new System.Windows.Forms.CheckBox();
            this.btnSaveAnnotation = new System.Windows.Forms.Button();
            this.annotationTitle = new System.Windows.Forms.RichTextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.selectedBytesToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.bitToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.OffsetToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.insertValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpValue = new System.Windows.Forms.GroupBox();
            this.rdo1byte = new System.Windows.Forms.RadioButton();
            this.rdo2byte = new System.Windows.Forms.RadioButton();
            this.rdo4byte = new System.Windows.Forms.RadioButton();
            this.txtByte4 = new System.Windows.Forms.TextBox();
            this.txtByte3 = new System.Windows.Forms.TextBox();
            this.txtByte2 = new System.Windows.Forms.TextBox();
            this.txtByte1 = new System.Windows.Forms.TextBox();
            this.txtBin8 = new System.Windows.Forms.TextBox();
            this.txtBin7 = new System.Windows.Forms.TextBox();
            this.txtBin6 = new System.Windows.Forms.TextBox();
            this.txtBin5 = new System.Windows.Forms.TextBox();
            this.txtBin4 = new System.Windows.Forms.TextBox();
            this.txtBin3 = new System.Windows.Forms.TextBox();
            this.txtBin2 = new System.Windows.Forms.TextBox();
            this.txtBin1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtInt = new System.Windows.Forms.TextBox();
            this.txtFloat = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.ppcDisassembler1 = new PPCDisassembler();
            this.hexBox1 = new HexBox();
            this.grpSettings.SuspendLayout();
            this.grpRelocInfo.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlFunctions.SuspendLayout();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlHexEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.grpValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.chkBSSSection);
            this.grpSettings.Controls.Add(this.chkCodeSection);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSettings.Location = new System.Drawing.Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(171, 58);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Properties";
            // 
            // chkBSSSection
            // 
            this.chkBSSSection.AutoSize = true;
            this.chkBSSSection.Location = new System.Drawing.Point(7, 36);
            this.chkBSSSection.Name = "chkBSSSection";
            this.chkBSSSection.Size = new System.Drawing.Size(97, 17);
            this.chkBSSSection.TabIndex = 14;
            this.chkBSSSection.Text = "Is BSS Section";
            this.chkBSSSection.UseVisualStyleBackColor = true;
            this.chkBSSSection.CheckedChanged += this.chkBSSSection_CheckedChanged;
            // 
            // chkCodeSection
            // 
            this.chkCodeSection.AutoSize = true;
            this.chkCodeSection.Checked = true;
            this.chkCodeSection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCodeSection.Location = new System.Drawing.Point(7, 18);
            this.chkCodeSection.Name = "chkCodeSection";
            this.chkCodeSection.Size = new System.Drawing.Size(101, 17);
            this.chkCodeSection.TabIndex = 13;
            this.chkCodeSection.Text = "Is Code Section";
            this.chkCodeSection.UseVisualStyleBackColor = true;
            this.chkCodeSection.CheckedChanged += this.chkCodeSection_CheckedChanged;
            // 
            // grpRelocInfo
            // 
            this.grpRelocInfo.Controls.Add(this.panel2);
            this.grpRelocInfo.Controls.Add(this.panel3);
            this.grpRelocInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRelocInfo.Location = new System.Drawing.Point(0, 221);
            this.grpRelocInfo.Name = "grpRelocInfo";
            this.grpRelocInfo.Size = new System.Drawing.Size(171, 419);
            this.grpRelocInfo.TabIndex = 2;
            this.grpRelocInfo.TabStop = false;
            this.grpRelocInfo.Text = "Selected Word Info";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lstLinked);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(165, 377);
            this.panel2.TabIndex = 11;
            // 
            // lstLinked
            // 
            this.lstLinked.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstLinked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLinked.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstLinked.FormattingEnabled = true;
            this.lstLinked.IntegralHeight = false;
            this.lstLinked.Location = new System.Drawing.Point(0, 18);
            this.lstLinked.Name = "lstLinked";
            this.lstLinked.Size = new System.Drawing.Size(165, 108);
            this.lstLinked.TabIndex = 10;
            this.lstLinked.DrawItem += this.lstLinked_DrawItem;
            this.lstLinked.DoubleClick += this.lstLinked_DoubleClick;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 18);
            this.label1.TabIndex = 11;
            this.label1.Text = "Linked Commands";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 126);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(165, 3);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.pnlFunctions);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 129);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(165, 248);
            this.panel5.TabIndex = 12;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.propertyGrid1);
            this.panel6.Controls.Add(this.panel1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 80);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(165, 168);
            this.panel6.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 24);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(165, 144);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += this.propertyGrid1_PropertyValueChanged;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNewCmd);
            this.panel1.Controls.Add(this.btnDelCmd);
            this.panel1.Controls.Add(this.btnOpenTarget);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(165, 24);
            this.panel1.TabIndex = 10;
            // 
            // btnNewCmd
            // 
            this.btnNewCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnNewCmd.Location = new System.Drawing.Point(0, 0);
            this.btnNewCmd.Name = "btnNewCmd";
            this.btnNewCmd.Size = new System.Drawing.Size(56, 24);
            this.btnNewCmd.TabIndex = 7;
            this.btnNewCmd.Text = "New";
            this.btnNewCmd.UseVisualStyleBackColor = true;
            this.btnNewCmd.Click += this.btnNewCmd_Click;
            // 
            // btnDelCmd
            // 
            this.btnDelCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnDelCmd.Location = new System.Drawing.Point(55, 0);
            this.btnDelCmd.Name = "btnDelCmd";
            this.btnDelCmd.Size = new System.Drawing.Size(55, 24);
            this.btnDelCmd.TabIndex = 3;
            this.btnDelCmd.Text = "Delete";
            this.btnDelCmd.UseVisualStyleBackColor = true;
            this.btnDelCmd.Click += this.btnDelCmd_Click;
            // 
            // btnOpenTarget
            // 
            this.btnOpenTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnOpenTarget.Location = new System.Drawing.Point(109, 0);
            this.btnOpenTarget.Name = "btnOpenTarget";
            this.btnOpenTarget.Size = new System.Drawing.Size(56, 24);
            this.btnOpenTarget.TabIndex = 8;
            this.btnOpenTarget.Text = "Open";
            this.btnOpenTarget.UseVisualStyleBackColor = true;
            this.btnOpenTarget.Click += this.btnOpenTarget_Click;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 18);
            this.label2.TabIndex = 13;
            this.label2.Text = "Command Info";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlFunctions
            // 
            this.pnlFunctions.Controls.Add(this.chkUnresolved);
            this.pnlFunctions.Controls.Add(this.chkDestructor);
            this.pnlFunctions.Controls.Add(this.chkConstructor);
            this.pnlFunctions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFunctions.Location = new System.Drawing.Point(0, 0);
            this.pnlFunctions.Name = "pnlFunctions";
            this.pnlFunctions.Size = new System.Drawing.Size(165, 62);
            this.pnlFunctions.TabIndex = 11;
            // 
            // chkUnresolved
            // 
            this.chkUnresolved.AutoSize = true;
            this.chkUnresolved.Location = new System.Drawing.Point(4, 42);
            this.chkUnresolved.Name = "chkUnresolved";
            this.chkUnresolved.Size = new System.Drawing.Size(135, 17);
            this.chkUnresolved.TabIndex = 2;
            this.chkUnresolved.Text = "Is Unresolved Function";
            this.chkUnresolved.UseVisualStyleBackColor = true;
            this.chkUnresolved.CheckedChanged += this.chkUnresolved_CheckedChanged;
            // 
            // chkDestructor
            // 
            this.chkDestructor.AutoSize = true;
            this.chkDestructor.Location = new System.Drawing.Point(4, 24);
            this.chkDestructor.Name = "chkDestructor";
            this.chkDestructor.Size = new System.Drawing.Size(130, 17);
            this.chkDestructor.TabIndex = 1;
            this.chkDestructor.Text = "Is Destructor Function";
            this.chkDestructor.UseVisualStyleBackColor = true;
            this.chkDestructor.CheckedChanged += this.chkDestructor_CheckedChanged;
            // 
            // chkConstructor
            // 
            this.chkConstructor.AutoSize = true;
            this.chkConstructor.Location = new System.Drawing.Point(4, 6);
            this.chkConstructor.Name = "chkConstructor";
            this.chkConstructor.Size = new System.Drawing.Size(135, 17);
            this.chkConstructor.TabIndex = 0;
            this.chkConstructor.Text = "Is Constructor Function";
            this.chkConstructor.UseVisualStyleBackColor = true;
            this.chkConstructor.CheckedChanged += this.chkConstructor_CheckedChanged;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRemoveWord);
            this.panel3.Controls.Add(this.btnInsertWord);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(165, 23);
            this.panel3.TabIndex = 16;
            // 
            // btnRemoveWord
            // 
            this.btnRemoveWord.Location = new System.Drawing.Point(81, 0);
            this.btnRemoveWord.Name = "btnRemoveWord";
            this.btnRemoveWord.Size = new System.Drawing.Size(84, 23);
            this.btnRemoveWord.TabIndex = 13;
            this.btnRemoveWord.Text = "Remove Word";
            this.btnRemoveWord.UseVisualStyleBackColor = true;
            this.btnRemoveWord.Click += this.btnRemoveWord_Click;
            // 
            // btnInsertWord
            // 
            this.btnInsertWord.Location = new System.Drawing.Point(0, 0);
            this.btnInsertWord.Name = "btnInsertWord";
            this.btnInsertWord.Size = new System.Drawing.Size(82, 23);
            this.btnInsertWord.TabIndex = 12;
            this.btnInsertWord.Text = "Insert Word";
            this.btnInsertWord.UseVisualStyleBackColor = true;
            this.btnInsertWord.Click += this.btnInsertWord_Click;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.gotoToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.displayToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(500, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.exportInitializedToolStripMenuItem,
            this.importToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += this.exportToolStripMenuItem_Click;
            // 
            // exportInitializedToolStripMenuItem
            // 
            this.exportInitializedToolStripMenuItem.Enabled = false;
            this.exportInitializedToolStripMenuItem.Name = "exportInitializedToolStripMenuItem";
            this.exportInitializedToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exportInitializedToolStripMenuItem.Text = "Export Initialized";
            this.exportInitializedToolStripMenuItem.Visible = false;
            this.exportInitializedToolStripMenuItem.Click += this.exportInitializedToolStripMenuItem_Click;
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.importToolStripMenuItem.Text = "Replace";
            this.importToolStripMenuItem.Click += this.importToolStripMenuItem_Click;
            // 
            // gotoToolStripMenuItem
            // 
            this.gotoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteOverwriteToolStripMenuItem});
            this.gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            this.gotoToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.gotoToolStripMenuItem.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += this.copyToolStripMenuItem_Click;
            // 
            // pasteOverwriteToolStripMenuItem
            // 
            this.pasteOverwriteToolStripMenuItem.Name = "pasteOverwriteToolStripMenuItem";
            this.pasteOverwriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteOverwriteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteOverwriteToolStripMenuItem.Text = "Paste";
            this.pasteOverwriteToolStripMenuItem.Click += this.pasteOverwriteToolStripMenuItem_Click;
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gotoToolStripMenuItem2,
            this.findToolStripMenuItem1,
            this.findNextToolStripMenuItem,
            this.findPreviousToolStripMenuItem});
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.searchToolStripMenuItem.Text = "Search";
            // 
            // gotoToolStripMenuItem2
            // 
            this.gotoToolStripMenuItem2.Name = "gotoToolStripMenuItem2";
            this.gotoToolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.gotoToolStripMenuItem2.Size = new System.Drawing.Size(196, 22);
            this.gotoToolStripMenuItem2.Text = "Goto...";
            this.gotoToolStripMenuItem2.Click += this.gotoToolStripMenuItem2_Click;
            // 
            // findToolStripMenuItem1
            // 
            this.findToolStripMenuItem1.Name = "findToolStripMenuItem1";
            this.findToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.findToolStripMenuItem1.Text = "Find";
            this.findToolStripMenuItem1.Click += this.findToolStripMenuItem1_Click;
            // 
            // findNextToolStripMenuItem
            // 
            this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
            this.findNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.findNextToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.findNextToolStripMenuItem.Text = "Find Next";
            this.findNextToolStripMenuItem.Click += this.findNextToolStripMenuItem_Click;
            // 
            // findPreviousToolStripMenuItem
            // 
            this.findPreviousToolStripMenuItem.Name = "findPreviousToolStripMenuItem";
            this.findPreviousToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
            this.findPreviousToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.findPreviousToolStripMenuItem.Text = "Find Previous";
            this.findPreviousToolStripMenuItem.Click += this.findPreviousToolStripMenuItem_Click;
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highlightBlr,
            this.displayInitialized,
            this.displayStringsToolStripMenuItem,
            this.chkAnnotations});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // highlightBlr
            // 
            this.highlightBlr.Checked = true;
            this.highlightBlr.CheckOnClick = true;
            this.highlightBlr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.highlightBlr.Name = "highlightBlr";
            this.highlightBlr.Size = new System.Drawing.Size(180, 22);
            this.highlightBlr.Text = "Highlight blr";
            this.highlightBlr.CheckedChanged += this.highlightBlr_CheckedChanged;
            // 
            // displayInitialized
            // 
            this.displayInitialized.CheckOnClick = true;
            this.displayInitialized.Name = "displayInitialized";
            this.displayInitialized.Size = new System.Drawing.Size(180, 22);
            this.displayInitialized.Text = "Display Initialized";
            this.displayInitialized.CheckedChanged += this.displayInitialized_CheckedChanged;
            // 
            // displayStringsToolStripMenuItem
            // 
            this.displayStringsToolStripMenuItem.Checked = true;
            this.displayStringsToolStripMenuItem.CheckOnClick = true;
            this.displayStringsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.displayStringsToolStripMenuItem.Name = "displayStringsToolStripMenuItem";
            this.displayStringsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.displayStringsToolStripMenuItem.Text = "Display strings";
            this.displayStringsToolStripMenuItem.CheckedChanged += this.displayStringsToolStripMenuItem_CheckedChanged;
            // 
            // chkAnnotations
            // 
            this.chkAnnotations.Checked = true;
            this.chkAnnotations.CheckOnClick = true;
            this.chkAnnotations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnnotations.Name = "chkAnnotations";
            this.chkAnnotations.Size = new System.Drawing.Size(180, 22);
            this.chkAnnotations.Text = "Display Annotations";
            this.chkAnnotations.CheckedChanged += this.chkAnnotations_CheckedChanged;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.ppcDisassembler1);
            this.pnlLeft.Controls.Add(this.splitter2);
            this.pnlLeft.Controls.Add(this.pnlHexEditor);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(780, 642);
            this.pnlLeft.TabIndex = 5;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(277, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 642);
            this.splitter2.TabIndex = 11;
            this.splitter2.TabStop = false;
            // 
            // pnlHexEditor
            // 
            this.pnlHexEditor.Controls.Add(this.splitContainer1);
            this.pnlHexEditor.Controls.Add(this.menuStrip1);
            this.pnlHexEditor.Controls.Add(this.statusStrip);
            this.pnlHexEditor.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlHexEditor.Location = new System.Drawing.Point(280, 0);
            this.pnlHexEditor.Name = "pnlHexEditor";
            this.pnlHexEditor.Size = new System.Drawing.Size(500, 642);
            this.pnlHexEditor.TabIndex = 11;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.hexBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.annotationDescription);
            this.splitContainer1.Panel2.Controls.Add(this.btn1underline);
            this.splitContainer1.Panel2.Controls.Add(this.btn2underline);
            this.splitContainer1.Panel2.Controls.Add(this.btn3underline);
            this.splitContainer1.Panel2.Controls.Add(this.btn4underline);
            this.splitContainer1.Panel2.Controls.Add(this.btnSaveAnnotation);
            this.splitContainer1.Panel2.Controls.Add(this.annotationTitle);
            this.splitContainer1.Size = new System.Drawing.Size(503, 590);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 19;
            // 
            // annotationDescription
            // 
            this.annotationDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationDescription.BackColor = System.Drawing.SystemColors.Control;
            this.annotationDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.annotationDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.annotationDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.annotationDescription.ForeColor = System.Drawing.Color.Black;
            this.annotationDescription.Location = new System.Drawing.Point(0, 25);
            this.annotationDescription.Name = "annotationDescription";
            this.annotationDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.annotationDescription.Size = new System.Drawing.Size(503, 144);
            this.annotationDescription.TabIndex = 11;
            this.annotationDescription.Text = "No Description Available.";
            this.annotationDescription.LinkClicked += this.annotationDescription_LinkClicked;
            this.annotationDescription.TextChanged += this.description_TextChanged;
            this.annotationDescription.MouseDown += this.annotationDescription_MouseDown;
            // 
            // btn1underline
            // 
            this.btn1underline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn1underline.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn1underline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1underline.Location = new System.Drawing.Point(312, 2);
            this.btn1underline.Name = "btn1underline";
            this.btn1underline.Size = new System.Drawing.Size(24, 23);
            this.btn1underline.TabIndex = 18;
            this.btn1underline.Text = "1";
            this.btn1underline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn1underline.UseVisualStyleBackColor = true;
            this.btn1underline.CheckedChanged += this.btnUnderline_CheckedChanged;
            // 
            // btn2underline
            // 
            this.btn2underline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn2underline.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn2underline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2underline.Location = new System.Drawing.Point(337, 2);
            this.btn2underline.Name = "btn2underline";
            this.btn2underline.Size = new System.Drawing.Size(24, 23);
            this.btn2underline.TabIndex = 17;
            this.btn2underline.Text = "2";
            this.btn2underline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn2underline.UseVisualStyleBackColor = true;
            this.btn2underline.CheckedChanged += this.btnUnderline_CheckedChanged;
            // 
            // btn3underline
            // 
            this.btn3underline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn3underline.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn3underline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3underline.Location = new System.Drawing.Point(362, 2);
            this.btn3underline.Name = "btn3underline";
            this.btn3underline.Size = new System.Drawing.Size(24, 23);
            this.btn3underline.TabIndex = 16;
            this.btn3underline.Text = "3";
            this.btn3underline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn3underline.UseVisualStyleBackColor = true;
            this.btn3underline.CheckedChanged += this.btnUnderline_CheckedChanged;
            // 
            // btn4underline
            // 
            this.btn4underline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn4underline.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn4underline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4underline.Location = new System.Drawing.Point(387, 2);
            this.btn4underline.Name = "btn4underline";
            this.btn4underline.Size = new System.Drawing.Size(24, 23);
            this.btn4underline.TabIndex = 15;
            this.btn4underline.Text = "4";
            this.btn4underline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn4underline.UseVisualStyleBackColor = true;
            this.btn4underline.CheckedChanged += this.btnUnderline_CheckedChanged;
            // 
            // btnSaveAnnotation
            // 
            this.btnSaveAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAnnotation.Location = new System.Drawing.Point(417, 2);
            this.btnSaveAnnotation.Name = "btnSaveAnnotation";
            this.btnSaveAnnotation.Size = new System.Drawing.Size(82, 23);
            this.btnSaveAnnotation.TabIndex = 14;
            this.btnSaveAnnotation.Text = "Save";
            this.btnSaveAnnotation.UseVisualStyleBackColor = true;
            this.btnSaveAnnotation.Click += this.btnSaveAnnotation_Click;
            // 
            // annotationTitle
            // 
            this.annotationTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationTitle.BackColor = System.Drawing.SystemColors.Control;
            this.annotationTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.annotationTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.annotationTitle.Enabled = false;
            this.annotationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.annotationTitle.ForeColor = System.Drawing.Color.Black;
            this.annotationTitle.Location = new System.Drawing.Point(0, 0);
            this.annotationTitle.Name = "annotationTitle";
            this.annotationTitle.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.annotationTitle.Size = new System.Drawing.Size(503, 30);
            this.annotationTitle.TabIndex = 12;
            this.annotationTitle.Text = "No Description Available.";
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.selectedBytesToolStripStatusLabel,
            this.bitToolStripStatusLabel,
            this.toolStripStatusLabel1,
            this.OffsetToolStripStatusLabel,
            this.insertValue});
            this.statusStrip.Location = new System.Drawing.Point(0, 620);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip.Size = new System.Drawing.Size(500, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 10;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.toolStripStatusLabel.Size = new System.Drawing.Size(75, 17);
            this.toolStripStatusLabel.Text = "Ln 0    Col 0";
            // 
            // selectedBytesToolStripStatusLabel
            // 
            this.selectedBytesToolStripStatusLabel.Name = "selectedBytesToolStripStatusLabel";
            this.selectedBytesToolStripStatusLabel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.selectedBytesToolStripStatusLabel.Size = new System.Drawing.Size(88, 17);
            this.selectedBytesToolStripStatusLabel.Text = "Selected: 0x00";
            // 
            // bitToolStripStatusLabel
            // 
            this.bitToolStripStatusLabel.Name = "bitToolStripStatusLabel";
            this.bitToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // OffsetToolStripStatusLabel
            // 
            this.OffsetToolStripStatusLabel.Name = "OffsetToolStripStatusLabel";
            this.OffsetToolStripStatusLabel.Size = new System.Drawing.Size(65, 17);
            this.OffsetToolStripStatusLabel.Text = " Offset: 0x0";
            // 
            // insertValue
            // 
            this.insertValue.Name = "insertValue";
            this.insertValue.Size = new System.Drawing.Size(58, 17);
            this.insertValue.Text = "Overwrite";
            // 
            // pnlRight
            // 
            this.pnlRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRight.Controls.Add(this.grpRelocInfo);
            this.pnlRight.Controls.Add(this.grpValue);
            this.pnlRight.Controls.Add(this.grpSettings);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(783, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(173, 642);
            this.pnlRight.TabIndex = 13;
            // 
            // grpValue
            // 
            this.grpValue.Controls.Add(this.rdo1byte);
            this.grpValue.Controls.Add(this.rdo2byte);
            this.grpValue.Controls.Add(this.rdo4byte);
            this.grpValue.Controls.Add(this.txtByte4);
            this.grpValue.Controls.Add(this.txtByte3);
            this.grpValue.Controls.Add(this.txtByte2);
            this.grpValue.Controls.Add(this.txtByte1);
            this.grpValue.Controls.Add(this.txtBin8);
            this.grpValue.Controls.Add(this.txtBin7);
            this.grpValue.Controls.Add(this.txtBin6);
            this.grpValue.Controls.Add(this.txtBin5);
            this.grpValue.Controls.Add(this.txtBin4);
            this.grpValue.Controls.Add(this.txtBin3);
            this.grpValue.Controls.Add(this.txtBin2);
            this.grpValue.Controls.Add(this.txtBin1);
            this.grpValue.Controls.Add(this.label6);
            this.grpValue.Controls.Add(this.txtInt);
            this.grpValue.Controls.Add(this.txtFloat);
            this.grpValue.Controls.Add(this.label4);
            this.grpValue.Controls.Add(this.label3);
            this.grpValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpValue.Location = new System.Drawing.Point(0, 58);
            this.grpValue.Name = "grpValue";
            this.grpValue.Size = new System.Drawing.Size(171, 163);
            this.grpValue.TabIndex = 15;
            this.grpValue.TabStop = false;
            this.grpValue.Text = "Value";
            // 
            // rdo1byte
            // 
            this.rdo1byte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdo1byte.AutoSize = true;
            this.rdo1byte.Location = new System.Drawing.Point(113, 16);
            this.rdo1byte.Name = "rdo1byte";
            this.rdo1byte.Size = new System.Drawing.Size(55, 17);
            this.rdo1byte.TabIndex = 18;
            this.rdo1byte.TabStop = true;
            this.rdo1byte.Text = "1 Byte";
            this.rdo1byte.UseVisualStyleBackColor = true;
            this.rdo1byte.CheckedChanged += this.byteCount_CheckedChanged;
            // 
            // rdo2byte
            // 
            this.rdo2byte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdo2byte.AutoSize = true;
            this.rdo2byte.Location = new System.Drawing.Point(60, 16);
            this.rdo2byte.Name = "rdo2byte";
            this.rdo2byte.Size = new System.Drawing.Size(55, 17);
            this.rdo2byte.TabIndex = 17;
            this.rdo2byte.TabStop = true;
            this.rdo2byte.Text = "2 Byte";
            this.rdo2byte.UseVisualStyleBackColor = true;
            this.rdo2byte.CheckedChanged += this.byteCount_CheckedChanged;
            // 
            // rdo4byte
            // 
            this.rdo4byte.AutoSize = true;
            this.rdo4byte.Checked = true;
            this.rdo4byte.Location = new System.Drawing.Point(7, 16);
            this.rdo4byte.Name = "rdo4byte";
            this.rdo4byte.Size = new System.Drawing.Size(55, 17);
            this.rdo4byte.TabIndex = 15;
            this.rdo4byte.TabStop = true;
            this.rdo4byte.Text = "4 Byte";
            this.rdo4byte.UseVisualStyleBackColor = true;
            this.rdo4byte.CheckedChanged += this.byteCount_CheckedChanged;
            // 
            // txtByte4
            // 
            this.txtByte4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtByte4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtByte4.Enabled = false;
            this.txtByte4.Location = new System.Drawing.Point(138, 134);
            this.txtByte4.Name = "txtByte4";
            this.txtByte4.Size = new System.Drawing.Size(25, 20);
            this.txtByte4.TabIndex = 16;
            this.txtByte4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtByte3
            // 
            this.txtByte3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtByte3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtByte3.Enabled = false;
            this.txtByte3.Location = new System.Drawing.Point(138, 115);
            this.txtByte3.Name = "txtByte3";
            this.txtByte3.Size = new System.Drawing.Size(25, 20);
            this.txtByte3.TabIndex = 15;
            this.txtByte3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtByte2
            // 
            this.txtByte2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtByte2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtByte2.Enabled = false;
            this.txtByte2.Location = new System.Drawing.Point(138, 96);
            this.txtByte2.Name = "txtByte2";
            this.txtByte2.Size = new System.Drawing.Size(25, 20);
            this.txtByte2.TabIndex = 14;
            this.txtByte2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtByte1
            // 
            this.txtByte1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtByte1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtByte1.Enabled = false;
            this.txtByte1.Location = new System.Drawing.Point(138, 77);
            this.txtByte1.Name = "txtByte1";
            this.txtByte1.Size = new System.Drawing.Size(25, 20);
            this.txtByte1.TabIndex = 13;
            this.txtByte1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBin8
            // 
            this.txtBin8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin8.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin8.Location = new System.Drawing.Point(95, 134);
            this.txtBin8.MaxLength = 4;
            this.txtBin8.Name = "txtBin8";
            this.txtBin8.Size = new System.Drawing.Size(44, 20);
            this.txtBin8.TabIndex = 12;
            this.txtBin8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin8.TextChanged += this.txtBin1_TextChanged;
            // 
            // txtBin7
            // 
            this.txtBin7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin7.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin7.Location = new System.Drawing.Point(52, 134);
            this.txtBin7.MaxLength = 4;
            this.txtBin7.Name = "txtBin7";
            this.txtBin7.Size = new System.Drawing.Size(44, 20);
            this.txtBin7.TabIndex = 11;
            this.txtBin7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin7.TextChanged += this.txtBin1_TextChanged;
            // 
            // txtBin6
            // 
            this.txtBin6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin6.Location = new System.Drawing.Point(95, 115);
            this.txtBin6.MaxLength = 4;
            this.txtBin6.Name = "txtBin6";
            this.txtBin6.Size = new System.Drawing.Size(44, 20);
            this.txtBin6.TabIndex = 10;
            this.txtBin6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin6.TextChanged += this.txtBin1_TextChanged;
            // 
            // txtBin5
            // 
            this.txtBin5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin5.Location = new System.Drawing.Point(52, 115);
            this.txtBin5.MaxLength = 4;
            this.txtBin5.Name = "txtBin5";
            this.txtBin5.Size = new System.Drawing.Size(44, 20);
            this.txtBin5.TabIndex = 9;
            this.txtBin5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin5.TextChanged += this.txtBin1_TextChanged;
            // 
            // txtBin4
            // 
            this.txtBin4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin4.Location = new System.Drawing.Point(95, 96);
            this.txtBin4.MaxLength = 4;
            this.txtBin4.Name = "txtBin4";
            this.txtBin4.Size = new System.Drawing.Size(44, 20);
            this.txtBin4.TabIndex = 8;
            this.txtBin4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin4.TextChanged += this.txtBin1_TextChanged;
            // 
            // txtBin3
            // 
            this.txtBin3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin3.Location = new System.Drawing.Point(52, 96);
            this.txtBin3.MaxLength = 4;
            this.txtBin3.Name = "txtBin3";
            this.txtBin3.Size = new System.Drawing.Size(44, 20);
            this.txtBin3.TabIndex = 7;
            this.txtBin3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin3.TextChanged += this.txtBin1_TextChanged;
            // 
            // txtBin2
            // 
            this.txtBin2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin2.Location = new System.Drawing.Point(95, 77);
            this.txtBin2.MaxLength = 4;
            this.txtBin2.Name = "txtBin2";
            this.txtBin2.Size = new System.Drawing.Size(44, 20);
            this.txtBin2.TabIndex = 6;
            this.txtBin2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin2.TextChanged += this.txtBin1_TextChanged;
            // 
            // txtBin1
            // 
            this.txtBin1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBin1.Location = new System.Drawing.Point(52, 77);
            this.txtBin1.MaxLength = 4;
            this.txtBin1.Name = "txtBin1";
            this.txtBin1.Size = new System.Drawing.Size(44, 20);
            this.txtBin1.TabIndex = 5;
            this.txtBin1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBin1.TextChanged += this.txtBin1_TextChanged;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(7, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 77);
            this.label6.TabIndex = 4;
            this.label6.Text = "Binary:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtInt
            // 
            this.txtInt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInt.Location = new System.Drawing.Point(52, 58);
            this.txtInt.Name = "txtInt";
            this.txtInt.Size = new System.Drawing.Size(111, 20);
            this.txtInt.TabIndex = 3;
            this.txtInt.TextChanged += this.txtInt_TextChanged;
            // 
            // txtFloat
            // 
            this.txtFloat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFloat.Location = new System.Drawing.Point(52, 39);
            this.txtFloat.Name = "txtFloat";
            this.txtFloat.Size = new System.Drawing.Size(111, 20);
            this.txtFloat.TabIndex = 2;
            this.txtFloat.TextChanged += this.txtFloat_TextChanged;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(7, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Integer:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(7, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Float:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter3.Location = new System.Drawing.Point(780, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(3, 642);
            this.splitter3.TabIndex = 11;
            this.splitter3.TabStop = false;
            // 
            // ppcDisassembler1
            // 
            this.ppcDisassembler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppcDisassembler1.Location = new System.Drawing.Point(0, 0);
            this.ppcDisassembler1.Name = "ppcDisassembler1";
            this.ppcDisassembler1.Size = new System.Drawing.Size(277, 642);
            this.ppcDisassembler1.TabIndex = 11;
            // 
            // hexBox1
            // 
            this.hexBox1.BlrColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(100)))));
            this.hexBox1.BranchOffsetColor = System.Drawing.Color.Plum;
            // 
            // 
            // 
            this.hexBox1.BuiltInContextMenu.CopyMenuItemText = "Copy";
            this.hexBox1.BuiltInContextMenu.CutMenuItemText = "Cut";
            this.hexBox1.BuiltInContextMenu.PasteMenuItemText = "Paste";
            this.hexBox1.BuiltInContextMenu.SelectAllMenuItemText = "Select All";
            this.hexBox1.ColumnDividerColor = System.Drawing.Color.Gray;
            this.hexBox1.ColumnInfoVisible = true;
            this.hexBox1.CommandColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(200)))));
            this.hexBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.hexBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox1.GroupSeparatorVisible = true;
            this.hexBox1.InfoForeColor = System.Drawing.Color.Blue;
            this.hexBox1.LineInfoVisible = true;
            this.hexBox1.LinkedBranchColor = System.Drawing.Color.Orange;
            this.hexBox1.Location = new System.Drawing.Point(0, 0);
            this.hexBox1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.SectionEditor = null;
            this.hexBox1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(503, 420);
            this.hexBox1.StringViewVisible = true;
            this.hexBox1.TabIndex = 3;
            this.hexBox1.UseFixedBytesPerLine = true;
            this.hexBox1.VScrollBarVisible = true;
            this.hexBox1.SelectionStartChanged += this.hexBox1_SelectionStartChanged;
            this.hexBox1.SelectionLengthChanged += this.hexBox1_SelectionLengthChanged;
            this.hexBox1.CurrentLineChanged += this.hexBox1_CurrentLineChanged;
            this.hexBox1.CurrentPositionInLineChanged += this.hexBox1_CurrentPositionInLineChanged;
            this.hexBox1.Copied += this.hexBox1_Copied;
            this.hexBox1.CopiedHex += this.hexBox1_CopiedHex;
            // 
            // SectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 642);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.pnlRight);
            this.Icon = BrawlLib.Properties.Resources.Icon;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SectionEditor";
            this.Text = "Section Editor";
            this.Load += this.Form2_Load;
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpRelocInfo.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlFunctions.ResumeLayout(false);
            this.pnlFunctions.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlHexEditor.ResumeLayout(false);
            this.pnlHexEditor.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.grpValue.ResumeLayout(false);
            this.grpValue.PerformLayout();
            this.ResumeLayout(false);

        }

        private void applyChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Apply();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.GroupBox grpRelocInfo;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnNewCmd;
        private System.Windows.Forms.Button btnDelCmd;
        private Panel panel2;
        private Splitter splitter1;
        private Panel panel1;
        public HexBox hexBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem gotoToolStripMenuItem;
        private Panel pnlLeft;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripMenuItem gotoToolStripMenuItem2;
        private ToolStripMenuItem findToolStripMenuItem1;
        private ToolStripMenuItem findNextToolStripMenuItem;
        private ToolStripMenuItem findPreviousToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteOverwriteToolStripMenuItem;
        private CheckBox chkBSSSection;
        private CheckBox chkCodeSection;
        private Panel pnlRight;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripStatusLabel selectedBytesToolStripStatusLabel;
        private ToolStripStatusLabel bitToolStripStatusLabel;
        private Splitter splitter2;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem importToolStripMenuItem;
        private ListBox lstLinked;
        private Label label1;
        private Label label2;
        private Panel panel5;
        private Panel panel6;
        private Panel pnlHexEditor;
        private PPCDisassembler ppcDisassembler1;
        private PropertyGrid propertyGrid1;
        private ToolStripMenuItem exportInitializedToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel OffsetToolStripStatusLabel;
        private Button btnOpenTarget;
        private GroupBox grpValue;
        private TextBox txtInt;
        private TextBox txtFloat;
        private Label label4;
        private Label label3;
        private TextBox txtBin1;
        private Label label6;
        private TextBox txtBin4;
        private TextBox txtBin3;
        private TextBox txtBin2;
        private TextBox txtByte4;
        private TextBox txtByte3;
        private TextBox txtByte2;
        private TextBox txtByte1;
        private TextBox txtBin8;
        private TextBox txtBin7;
        private TextBox txtBin6;
        private TextBox txtBin5;
        private Panel panel3;
        private Button btnRemoveWord;
        private Button btnInsertWord;
        private ToolStripMenuItem displayToolStripMenuItem;
        public ToolStripMenuItem highlightBlr;
        public ToolStripMenuItem displayInitialized;
        private ToolStripStatusLabel insertValue;
        private ToolStripMenuItem displayStringsToolStripMenuItem;
        private Panel pnlFunctions;
        private CheckBox chkUnresolved;
        private CheckBox chkDestructor;
        private CheckBox chkConstructor;
        private Splitter splitter3;
        private ToolStripMenuItem chkAnnotations;
        public RichTextBox annotationDescription;
        public RichTextBox annotationTitle;
        private Button btnSaveAnnotation;
        public RadioButton rdo4byte;
        public RadioButton rdo2byte;
        public RadioButton rdo1byte;
        public CheckBox btn4underline;
        public CheckBox btn1underline;
        public CheckBox btn2underline;
        public CheckBox btn3underline;
        private SplitContainer splitContainer1;
    }
}
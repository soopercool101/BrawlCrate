using BrawlLib.SSBB;
using System.Windows.Forms;
using BrawlLib.Internal.Windows.Controls;

namespace BrawlLib.Internal.Windows.Forms
{
    partial class GCTEditor
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
            this.components = new System.ComponentModel.Container();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rememberAllCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.forgetAllCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllRememberedCodesToGCTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCodesToRememberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRememberedCodesAsNewFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtCode = new RichTextBoxBordered();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDeleteCode = new System.Windows.Forms.Button();
            this.btnAddRemoveCode = new System.Windows.Forms.Button();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtDesc = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNewCode = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstCodes = new System.Windows.Forms.ListView();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBox1);
            this.panel3.Controls.Add(this.txtPath);
            this.panel3.Controls.Add(this.menuStrip1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(464, 24);
            this.panel3.TabIndex = 15;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.White;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(305, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(150, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Save GCT with info";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(126, 2);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(173, 22);
            this.txtPath.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(464, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rememberAllCodesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.forgetAllCodesToolStripMenuItem,
            this.saveAllRememberedCodesToGCTToolStripMenuItem,
            this.loadCodesToRememberToolStripMenuItem,
            this.loadRememberedCodesAsNewFileToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // rememberAllCodesToolStripMenuItem
            // 
            this.rememberAllCodesToolStripMenuItem.Name = "rememberAllCodesToolStripMenuItem";
            this.rememberAllCodesToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.rememberAllCodesToolStripMenuItem.Text = "Remember all codes in this file";
            this.rememberAllCodesToolStripMenuItem.Click += new System.EventHandler(this.rememberAllCodesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(324, 26);
            this.toolStripMenuItem1.Text = "Forget all codes in this file";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // forgetAllCodesToolStripMenuItem
            // 
            this.forgetAllCodesToolStripMenuItem.Name = "forgetAllCodesToolStripMenuItem";
            this.forgetAllCodesToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.forgetAllCodesToolStripMenuItem.Text = "Forget all codes";
            this.forgetAllCodesToolStripMenuItem.Click += new System.EventHandler(this.forgetAllCodesToolStripMenuItem_Click);
            // 
            // saveAllRememberedCodesToGCTToolStripMenuItem
            // 
            this.saveAllRememberedCodesToGCTToolStripMenuItem.Name = "saveAllRememberedCodesToGCTToolStripMenuItem";
            this.saveAllRememberedCodesToGCTToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.saveAllRememberedCodesToGCTToolStripMenuItem.Text = "Save all remembered codes to file";
            this.saveAllRememberedCodesToGCTToolStripMenuItem.Click += new System.EventHandler(this.saveAllRememberedCodesToGCTToolStripMenuItem_Click);
            // 
            // loadCodesToRememberToolStripMenuItem
            // 
            this.loadCodesToRememberToolStripMenuItem.Name = "loadCodesToRememberToolStripMenuItem";
            this.loadCodesToRememberToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.loadCodesToRememberToolStripMenuItem.Text = "Load codes to remember from file";
            this.loadCodesToRememberToolStripMenuItem.Click += new System.EventHandler(this.loadCodesToRememberToolStripMenuItem_Click);
            // 
            // loadRememberedCodesAsNewFileToolStripMenuItem
            // 
            this.loadRememberedCodesAsNewFileToolStripMenuItem.Name = "loadRememberedCodesAsNewFileToolStripMenuItem";
            this.loadRememberedCodesAsNewFileToolStripMenuItem.Size = new System.Drawing.Size(324, 26);
            this.loadRememberedCodesAsNewFileToolStripMenuItem.Text = "Load remembered codes as new file";
            this.loadRememberedCodesAsNewFileToolStripMenuItem.Click += new System.EventHandler(this.loadRememberedCodesAsNewFileToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.splitter2);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(183, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(281, 333);
            this.panel1.TabIndex = 17;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtCode);
            this.panel6.Controls.Add(this.statusStrip);
            this.panel6.Controls.Add(this.panel4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(281, 230);
            this.panel6.TabIndex = 13;
            // 
            // txtCode
            // 
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCode.Location = new System.Drawing.Point(0, 23);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(281, 183);
            this.txtCode.TabIndex = 9;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip.Location = new System.Drawing.Point(0, 206);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(281, 24);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 12;
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 19);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBox1);
            this.panel4.Controls.Add(this.btnDeleteCode);
            this.panel4.Controls.Add(this.btnAddRemoveCode);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(281, 23);
            this.panel4.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 21;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnDeleteCode
            // 
            this.btnDeleteCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteCode.Location = new System.Drawing.Point(100, 0);
            this.btnDeleteCode.Name = "btnDeleteCode";
            this.btnDeleteCode.Size = new System.Drawing.Size(57, 23);
            this.btnDeleteCode.TabIndex = 20;
            this.btnDeleteCode.Text = "Delete";
            this.btnDeleteCode.UseVisualStyleBackColor = true;
            this.btnDeleteCode.Click += new System.EventHandler(this.btnDeleteCode_Click);
            // 
            // btnAddRemoveCode
            // 
            this.btnAddRemoveCode.AutoSize = true;
            this.btnAddRemoveCode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddRemoveCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddRemoveCode.Location = new System.Drawing.Point(157, 0);
            this.btnAddRemoveCode.Name = "btnAddRemoveCode";
            this.btnAddRemoveCode.Size = new System.Drawing.Size(124, 23);
            this.btnAddRemoveCode.TabIndex = 22;
            this.btnAddRemoveCode.Text = "Remember Code";
            this.btnAddRemoveCode.UseVisualStyleBackColor = true;
            this.btnAddRemoveCode.Click += new System.EventHandler(this.btnAddRemoveCode_Click);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 230);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(281, 3);
            this.splitter2.TabIndex = 10;
            this.splitter2.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtDesc);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 233);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(281, 100);
            this.panel5.TabIndex = 11;
            // 
            // txtDesc
            // 
            this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDesc.DetectUrls = false;
            this.txtDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDesc.Location = new System.Drawing.Point(0, 23);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(281, 77);
            this.txtDesc.TabIndex = 6;
            this.txtDesc.Text = "";
            this.txtDesc.TextChanged += new System.EventHandler(this.txtDesc_TextChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(281, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Description";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 46);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game Info";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(144, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(314, 22);
            this.txtName.TabIndex = 4;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name: ";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(29, 18);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(60, 22);
            this.txtID.TabIndex = 2;
            this.txtID.TextChanged += new System.EventHandler(this.txtID_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID: ";
            // 
            // btnNewCode
            // 
            this.btnNewCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNewCode.Location = new System.Drawing.Point(0, 0);
            this.btnNewCode.Name = "btnNewCode";
            this.btnNewCode.Size = new System.Drawing.Size(180, 23);
            this.btnNewCode.TabIndex = 19;
            this.btnNewCode.Text = "New Code";
            this.btnNewCode.UseVisualStyleBackColor = true;
            this.btnNewCode.Click += new System.EventHandler(this.btnNewCode_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lstCodes);
            this.panel2.Controls.Add(this.btnNewCode);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(180, 333);
            this.panel2.TabIndex = 18;
            // 
            // lstCodes
            // 
            this.lstCodes.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.lstCodes.CheckBoxes = true;
            this.lstCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCodes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstCodes.Location = new System.Drawing.Point(0, 23);
            this.lstCodes.MultiSelect = false;
            this.lstCodes.Name = "lstCodes";
            this.lstCodes.ShowGroups = false;
            this.lstCodes.Size = new System.Drawing.Size(180, 310);
            this.lstCodes.TabIndex = 20;
            this.lstCodes.UseCompatibleStateImageBehavior = false;
            this.lstCodes.View = System.Windows.Forms.View.List;
            // 
            // dlgSave
            // 
            this.dlgSave.Filter = FileFilters.GCT;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._moveUpToolStripMenuItem,
            this._moveDownToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 82);
            // 
            // _moveUpToolStripMenuItem
            // 
            this._moveUpToolStripMenuItem.Name = "_moveUpToolStripMenuItem";
            this._moveUpToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this._moveUpToolStripMenuItem.Text = "Move Up";
            // 
            // _moveDownToolStripMenuItem
            // 
            this._moveDownToolStripMenuItem.Name = "_moveDownToolStripMenuItem";
            this._moveDownToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this._moveDownToolStripMenuItem.Text = "Move Down";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(180, 70);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 333);
            this.splitter1.TabIndex = 20;
            this.splitter1.TabStop = false;
            // 
            // GCTEditor
            // 
            this.ClientSize = new System.Drawing.Size(464, 403);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Icon = BrawlLib.Properties.Resources.Icon;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GCTEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Code Manager";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private global::System.Windows.Forms.Panel panel3;
        private global::System.Windows.Forms.Panel panel1;
        private global::System.Windows.Forms.GroupBox groupBox1;
        private global::System.Windows.Forms.TextBox txtName;
        private global::System.Windows.Forms.Label label2;
        private global::System.Windows.Forms.TextBox txtID;
        private global::System.Windows.Forms.Label label1;
        private global::System.Windows.Forms.Label label4;
        private global::System.Windows.Forms.RichTextBox txtDesc;
        private global::System.Windows.Forms.Panel panel4;
        private global::System.Windows.Forms.Button btnDeleteCode;
        private global::System.Windows.Forms.Button btnNewCode;
        private global::System.Windows.Forms.Panel panel2;
        private RichTextBoxBordered txtCode;
        private TextBox txtPath;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private SaveFileDialog dlgSave;
        private CheckBox checkBox1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem _moveUpToolStripMenuItem;
        private ToolStripMenuItem _moveDownToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private TextBox textBox1;
        private Splitter splitter1;
        private Splitter splitter2;
        private Panel panel5;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel status;
        private Panel panel6;
        private Button btnAddRemoveCode;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem rememberAllCodesToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem forgetAllCodesToolStripMenuItem;
        private ToolStripMenuItem saveAllRememberedCodesToGCTToolStripMenuItem;
        private ToolStripMenuItem loadCodesToRememberToolStripMenuItem;
        private ToolStripMenuItem loadRememberedCodesAsNewFileToolStripMenuItem;
        private ListView lstCodes;
    }
}
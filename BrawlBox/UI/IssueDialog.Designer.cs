namespace System.Windows.Forms
{
    partial class IssueDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueDialog));
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.txtStack = new System.Windows.Forms.RichTextBox();
            this.chkForceClose = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstChangedFiles = new System.Windows.Forms.ListBox();
            this.ctxFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spltChangedFiles = new System.Windows.Forms.Splitter();
            this.lblChangedFiles = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ctxFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTitle
            // 
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.ForeColor = System.Drawing.Color.Gray;
            this.txtTitle.Location = new System.Drawing.Point(35, 5);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(264, 20);
            this.txtTitle.TabIndex = 0;
            this.txtTitle.Text = "Write a quick one-sentence summary of the bug";
            this.txtTitle.Enter += new System.EventHandler(this.txtTitle_Enter);
            this.txtTitle.Leave += new System.EventHandler(this.txtTitle_Leave);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5);
            this.label1.Size = new System.Drawing.Size(304, 56);
            this.label1.TabIndex = 1;
            this.label1.Text = "BrawlBox has thrown an error. You have the option to submit a bug report for revi" +
    "ew and try to save your work before the application attempts to continue executi" +
    "on. ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Title:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.ForeColor = System.Drawing.Color.Gray;
            this.txtDescription.Location = new System.Drawing.Point(0, 109);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(304, 104);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.Text = resources.GetString("txtDescription.Text");
            this.txtDescription.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.txtDescription_LinkClicked);
            this.txtDescription.Enter += new System.EventHandler(this.txtDescription_Enter);
            this.txtDescription.Leave += new System.EventHandler(this.txtDescription_Leave);
            this.txtDescription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtDescription_MouseUp);
            // 
            // txtStack
            // 
            this.txtStack.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtStack.ForeColor = System.Drawing.Color.Black;
            this.txtStack.Location = new System.Drawing.Point(0, 216);
            this.txtStack.Name = "txtStack";
            this.txtStack.ReadOnly = true;
            this.txtStack.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtStack.Size = new System.Drawing.Size(304, 102);
            this.txtStack.TabIndex = 4;
            this.txtStack.Text = "";
            this.txtStack.WordWrap = false;
            // 
            // chkForceClose
            // 
            this.chkForceClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkForceClose.Location = new System.Drawing.Point(0, 403);
            this.chkForceClose.Name = "chkForceClose";
            this.chkForceClose.Padding = new System.Windows.Forms.Padding(5);
            this.chkForceClose.Size = new System.Drawing.Size(304, 30);
            this.chkForceClose.TabIndex = 5;
            this.chkForceClose.Text = "Force close the program";
            this.chkForceClose.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(304, 22);
            this.label3.TabIndex = 6;
            this.label3.Text = "Description:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(226, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(91, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Close without sending";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 433);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 29);
            this.panel1.TabIndex = 9;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 213);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(304, 3);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtTitle);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(304, 31);
            this.panel2.TabIndex = 11;
            // 
            // lstChangedFiles
            // 
            this.lstChangedFiles.ContextMenuStrip = this.ctxFile;
            this.lstChangedFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstChangedFiles.FormattingEnabled = true;
            this.lstChangedFiles.IntegralHeight = false;
            this.lstChangedFiles.Location = new System.Drawing.Point(0, 343);
            this.lstChangedFiles.Name = "lstChangedFiles";
            this.lstChangedFiles.Size = new System.Drawing.Size(304, 60);
            this.lstChangedFiles.TabIndex = 12;
            // 
            // ctxFile
            // 
            this.ctxFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.ctxFile.Name = "ctxFile";
            this.ctxFile.Size = new System.Drawing.Size(115, 48);
            this.ctxFile.Opening += new System.ComponentModel.CancelEventHandler(this.ctxFile_Opening);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // spltChangedFiles
            // 
            this.spltChangedFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spltChangedFiles.Location = new System.Drawing.Point(0, 318);
            this.spltChangedFiles.Name = "spltChangedFiles";
            this.spltChangedFiles.Size = new System.Drawing.Size(304, 3);
            this.spltChangedFiles.TabIndex = 13;
            this.spltChangedFiles.TabStop = false;
            // 
            // lblChangedFiles
            // 
            this.lblChangedFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblChangedFiles.Location = new System.Drawing.Point(0, 321);
            this.lblChangedFiles.Name = "lblChangedFiles";
            this.lblChangedFiles.Size = new System.Drawing.Size(304, 22);
            this.lblChangedFiles.TabIndex = 14;
            this.lblChangedFiles.Text = "Changed files:";
            this.lblChangedFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IssueDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 462);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.txtStack);
            this.Controls.Add(this.spltChangedFiles);
            this.Controls.Add(this.lblChangedFiles);
            this.Controls.Add(this.lstChangedFiles);
            this.Controls.Add(this.chkForceClose);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(253, 363);
            this.Name = "IssueDialog";
            this.Text = "Submit a bug report";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ctxFile.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtDescription;
        private System.Windows.Forms.RichTextBox txtStack;
        private System.Windows.Forms.CheckBox chkForceClose;
        private System.Windows.Forms.Label label3;
        private Button button1;
        private Button button2;
        private Panel panel1;
        private Splitter splitter1;
        private Panel panel2;
        public ListBox lstChangedFiles;
        private Splitter spltChangedFiles;
        private ContextMenuStrip ctxFile;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private Label lblChangedFiles;
    }
}
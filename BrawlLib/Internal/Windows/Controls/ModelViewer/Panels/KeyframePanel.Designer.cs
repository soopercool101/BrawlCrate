using BrawlLib.Internal.Windows.Controls.ModelViewer.Editors;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Panels
{
    partial class KeyframePanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listKeyframes = new RefreshableListBox();
            this.visEditor = new VisEditor();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.chkConstant = new System.Windows.Forms.CheckBox();
            this.clrControl = new CLRControl();
            this.ctrlPanel = new System.Windows.Forms.Panel();
            this.lstTypes = new System.Windows.Forms.ComboBox();
            this.visChkPanel = new System.Windows.Forms.Panel();
            this.pnlKeyframes = new System.Windows.Forms.Panel();
            this.ctrlPanel.SuspendLayout();
            this.visChkPanel.SuspendLayout();
            this.pnlKeyframes.SuspendLayout();
            this.SuspendLayout();
            // 
            // listKeyframes
            // 
            this.listKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listKeyframes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listKeyframes.FormattingEnabled = true;
            this.listKeyframes.IntegralHeight = false;
            this.listKeyframes.ItemHeight = 14;
            this.listKeyframes.Location = new System.Drawing.Point(0, 0);
            this.listKeyframes.Name = "listKeyframes";
            this.listKeyframes.Size = new System.Drawing.Size(275, 357);
            this.listKeyframes.TabIndex = 18;
            this.listKeyframes.Visible = false;
            this.listKeyframes.SelectedIndexChanged += new System.EventHandler(this.listKeyframes_SelectedIndexChanged);
            // 
            // visEditor
            // 
            this.visEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visEditor.Location = new System.Drawing.Point(0, 0);
            this.visEditor.Name = "visEditor";
            this.visEditor.Size = new System.Drawing.Size(275, 357);
            this.visEditor.TabIndex = 19;
            this.visEditor.Visible = false;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Enabled = false;
            this.chkEnabled.Location = new System.Drawing.Point(71, 3);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 1;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // chkConstant
            // 
            this.chkConstant.AutoSize = true;
            this.chkConstant.Location = new System.Drawing.Point(3, 3);
            this.chkConstant.Name = "chkConstant";
            this.chkConstant.Size = new System.Drawing.Size(68, 17);
            this.chkConstant.TabIndex = 0;
            this.chkConstant.Text = "Constant";
            this.chkConstant.UseVisualStyleBackColor = true;
            this.chkConstant.CheckedChanged += new System.EventHandler(this.chkConstant_CheckedChanged);
            // 
            // clrControl
            // 
            this.clrControl.ColorID = 0;
            this.clrControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clrControl.Location = new System.Drawing.Point(0, 0);
            this.clrControl.Name = "clrControl";
            this.clrControl.Size = new System.Drawing.Size(275, 357);
            this.clrControl.TabIndex = 21;
            this.clrControl.Visible = false;
            // 
            // ctrlPanel
            // 
            this.ctrlPanel.Controls.Add(this.lstTypes);
            this.ctrlPanel.Controls.Add(this.visChkPanel);
            this.ctrlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctrlPanel.Location = new System.Drawing.Point(0, 0);
            this.ctrlPanel.Name = "ctrlPanel";
            this.ctrlPanel.Size = new System.Drawing.Size(279, 23);
            this.ctrlPanel.TabIndex = 20;
            // 
            // lstTypes
            // 
            this.lstTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstTypes.FormattingEnabled = true;
            this.lstTypes.Items.AddRange(new object[] {
            "Keyframes",
            "Colors",
            "Visibility"});
            this.lstTypes.Location = new System.Drawing.Point(0, 0);
            this.lstTypes.Name = "lstTypes";
            this.lstTypes.Size = new System.Drawing.Size(139, 21);
            this.lstTypes.TabIndex = 0;
            this.lstTypes.SelectedIndexChanged += new System.EventHandler(this.lstTypes_SelectedIndexChanged);
            // 
            // visChkPanel
            // 
            this.visChkPanel.Controls.Add(this.chkEnabled);
            this.visChkPanel.Controls.Add(this.chkConstant);
            this.visChkPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.visChkPanel.Location = new System.Drawing.Point(139, 0);
            this.visChkPanel.Name = "visChkPanel";
            this.visChkPanel.Size = new System.Drawing.Size(140, 23);
            this.visChkPanel.TabIndex = 3;
            // 
            // pnlKeyframes
            // 
            this.pnlKeyframes.AutoScroll = true;
            this.pnlKeyframes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlKeyframes.Controls.Add(this.listKeyframes);
            this.pnlKeyframes.Controls.Add(this.visEditor);
            this.pnlKeyframes.Controls.Add(this.clrControl);
            this.pnlKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKeyframes.Location = new System.Drawing.Point(0, 23);
            this.pnlKeyframes.Name = "pnlKeyframes";
            this.pnlKeyframes.Size = new System.Drawing.Size(279, 361);
            this.pnlKeyframes.TabIndex = 27;
            // 
            // KeyframePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlKeyframes);
            this.Controls.Add(this.ctrlPanel);
            this.Enabled = false;
            this.Name = "KeyframePanel";
            this.Size = new System.Drawing.Size(279, 384);
            this.ctrlPanel.ResumeLayout(false);
            this.visChkPanel.ResumeLayout(false);
            this.visChkPanel.PerformLayout();
            this.pnlKeyframes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public RefreshableListBox listKeyframes;
        public VisEditor visEditor;
        public CheckBox chkEnabled;
        public CheckBox chkConstant;
        public CLRControl clrControl;
        private Panel ctrlPanel;
        private Panel visChkPanel;
        private ComboBox lstTypes;
        private Panel pnlKeyframes;
    }
}

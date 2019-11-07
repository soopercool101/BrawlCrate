using BrawlLib.Internal.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Panels
{
    partial class RightPanel
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
            this.editor = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlOpenedFiles = new OpenedFilesControl();
            this.pnlBones = new BonesPanel();
            this.pnlKeyframes = new KeyframePanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editor
            // 
            this.editor.Dock = System.Windows.Forms.DockStyle.Top;
            this.editor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.editor.FormattingEnabled = true;
            this.editor.Items.AddRange(new object[] {
            "Bones",
            "Keyframes",
            "Files"});
            this.editor.Location = new System.Drawing.Point(0, 0);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(230, 21);
            this.editor.TabIndex = 0;
            this.editor.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlOpenedFiles);
            this.panel1.Controls.Add(this.pnlBones);
            this.panel1.Controls.Add(this.pnlKeyframes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 434);
            this.panel1.TabIndex = 1;
            // 
            // pnlOpenedFiles
            // 
            this.pnlOpenedFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOpenedFiles.Location = new System.Drawing.Point(0, 0);
            this.pnlOpenedFiles.Name = "pnlOpenedFiles";
            this.pnlOpenedFiles.Size = new System.Drawing.Size(230, 434);
            this.pnlOpenedFiles.TabIndex = 2;
            // 
            // pnlBones
            // 
            this.pnlBones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBones.Location = new System.Drawing.Point(0, 0);
            this.pnlBones.Name = "pnlBones";
            this.pnlBones.Size = new System.Drawing.Size(230, 434);
            this.pnlBones.TabIndex = 0;
            // 
            // pnlKeyframes
            // 
            this.pnlKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKeyframes.Enabled = false;
            this.pnlKeyframes.FrameIndex = -1;
            this.pnlKeyframes.Location = new System.Drawing.Point(0, 0);
            this.pnlKeyframes.Name = "pnlKeyframes";
            this.pnlKeyframes.Size = new System.Drawing.Size(230, 434);
            this.pnlKeyframes.TabIndex = 1;
            this.pnlKeyframes.Visible = false;
            // 
            // RightPanel
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.editor);
            this.Name = "RightPanel";
            this.Size = new System.Drawing.Size(230, 455);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private global::System.Windows.Forms.ComboBox editor;
        private global::System.Windows.Forms.Panel panel1;
        public BonesPanel pnlBones;
        public KeyframePanel pnlKeyframes;
        public OpenedFilesControl pnlOpenedFiles;
    }
}

using System.Windows.Forms;
namespace Ikarus.UI
{
    partial class EditorPanel
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
            this.scriptPanel = new ScriptPanel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // scriptPanel
            // 
            this.scriptPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptPanel.Location = new System.Drawing.Point(0, 0);
            this.scriptPanel.MinimumSize = new System.Drawing.Size(185, 0);
            this.scriptPanel.Name = "scriptPanel";
            this.scriptPanel.ScriptType = ScriptType.Subactions;
            this.scriptPanel.Size = new System.Drawing.Size(262, 505);
            this.scriptPanel.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(262, 505);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.Visible = false;
            // 
            // EditorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scriptPanel);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "EditorPanel";
            this.Size = new System.Drawing.Size(262, 505);
            this.ResumeLayout(false);

        }

        #endregion

        public ScriptPanel scriptPanel;
        public PropertyGrid propertyGrid1;
    }
}

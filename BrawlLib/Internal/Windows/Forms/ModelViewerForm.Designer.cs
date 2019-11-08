using BrawlLib.Internal.Windows.Controls.Model_Panel;

namespace BrawlLib.Internal.Windows.Forms
{
    partial class ModelViewerForm
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
            this.modelPanel1 = new ModelPanel();
            this.SuspendLayout();
            // 
            // modelPanel1
            // 
            this.modelPanel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.Location = new System.Drawing.Point(0, 0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.Size = new System.Drawing.Size(521, 439);
            this.modelPanel1.TabIndex = 0;
            // 
            // ModelViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 439);
            this.Controls.Add(this.modelPanel1);
            this.Icon = BrawlLib.Properties.Resources.Icon;
            this.Name = "ModelViewerForm";
            this.Text = "Model Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        public ModelPanel modelPanel1;

    }
}
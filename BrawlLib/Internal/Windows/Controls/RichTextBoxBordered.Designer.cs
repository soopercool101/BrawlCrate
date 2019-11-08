using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    partial class RichTextBoxBordered
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
        public RichTextBox textBox;
        private void InitializeComponent()
        {
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(2, 1);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(236, 243);
            this.textBox.TabIndex = 0;
            this.textBox.Text = "";
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // RichTextBoxBordered
            // 
            this.Controls.Add(this.textBox);
            this.Name = "RichTextBoxBordered";
            this.Size = new System.Drawing.Size(240, 245);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

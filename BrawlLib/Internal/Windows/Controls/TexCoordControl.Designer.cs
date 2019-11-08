using System.Windows.Forms;
using BrawlLib.Internal.Windows.Controls;

namespace BrawlLib.Internal.Windows.Forms
{
    partial class TexCoordControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboObj = new System.Windows.Forms.ComboBox();
            this.comboUVs = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.texCoordRenderer1 = new TexCoordRenderer();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboObj);
            this.panel1.Controls.Add(this.comboUVs);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(382, 28);
            this.panel1.TabIndex = 1;
            // 
            // comboObj
            // 
            this.comboObj.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboObj.FormattingEnabled = true;
            this.comboObj.Location = new System.Drawing.Point(3, 1);
            this.comboObj.Name = "comboObj";
            this.comboObj.Size = new System.Drawing.Size(135, 21);
            this.comboObj.TabIndex = 2;
            this.comboObj.SelectedIndexChanged += new System.EventHandler(this.comboObj_SelectedIndexChanged);
            // 
            // comboUVs
            // 
            this.comboUVs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboUVs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUVs.FormattingEnabled = true;
            this.comboUVs.Location = new System.Drawing.Point(144, 1);
            this.comboUVs.Name = "comboUVs";
            this.comboUVs.Size = new System.Drawing.Size(120, 21);
            this.comboUVs.TabIndex = 1;
            this.comboUVs.SelectedIndexChanged += new System.EventHandler(this.comboUVs_SelectedIndexChanged);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(270, -1);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(109, 23);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Export As Image";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // texCoordRenderer1
            // 
            this.texCoordRenderer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texCoordRenderer1.Location = new System.Drawing.Point(0, 28);
            this.texCoordRenderer1.Name = "texCoordRenderer1";
            this.texCoordRenderer1.Size = new System.Drawing.Size(382, 274);
            this.texCoordRenderer1.TabIndex = 0;
            this.texCoordRenderer1.TargetNode = null;
            // 
            // TexCoordControl
            // 
            this.Controls.Add(this.texCoordRenderer1);
            this.Controls.Add(this.panel1);
            this.Name = "TexCoordControl";
            this.Size = new System.Drawing.Size(382, 302);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TexCoordRenderer texCoordRenderer1;
        private global::System.Windows.Forms.Panel panel1;
        private Button btnExport;
        private ComboBox comboUVs;
        private ComboBox comboObj;
    }
}

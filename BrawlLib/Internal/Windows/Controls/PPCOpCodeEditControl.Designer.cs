using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    partial class PPCOpCodeEditControl
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.cboOpCode = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnGoToBranch = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(287, 76);
            this.propertyGrid1.TabIndex = 3;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "OpCode:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboOpCode
            // 
            this.cboOpCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOpCode.FormattingEnabled = true;
            this.cboOpCode.Location = new System.Drawing.Point(59, 5);
            this.cboOpCode.Name = "cboOpCode";
            this.cboOpCode.Size = new System.Drawing.Size(96, 21);
            this.cboOpCode.TabIndex = 0;
            this.cboOpCode.SelectedIndexChanged += new System.EventHandler(this.cboOpCode_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(287, 76);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnGoToBranch);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.cboOpCode);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(287, 30);
            this.panel3.TabIndex = 6;
            // 
            // btnGoToBranch
            // 
            this.btnGoToBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGoToBranch.Location = new System.Drawing.Point(161, 4);
            this.btnGoToBranch.Name = "btnGoToBranch";
            this.btnGoToBranch.Size = new System.Drawing.Size(123, 23);
            this.btnGoToBranch.TabIndex = 2;
            this.btnGoToBranch.Text = "Go to branch";
            this.btnGoToBranch.UseVisualStyleBackColor = true;
            this.btnGoToBranch.Visible = false;
            this.btnGoToBranch.Click += new System.EventHandler(this.btnGoToBranch_Click);
            // 
            // PPCOpCodeEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Name = "PPCOpCodeEditControl";
            this.Size = new System.Drawing.Size(287, 106);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGrid propertyGrid1;
        private Label label1;
        private ComboBox cboOpCode;
        private Panel panel2;
        private Panel panel3;
        private Button btnGoToBranch;
    }
}

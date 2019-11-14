using System;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public class FindNodeForm : Form
    {
        public string SearchTerm { get; private set; }
        public bool MatchCase { get; private set; }
        public bool MatchWholeWord { get; private set; }

        public FindNodeForm(MainForm parent)
        {
            Parent = parent;
            InitializeComponent();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            SearchTerm = txtFindValue.Text;
            MatchCase = chkMatchCase.Checked;
            MatchWholeWord = chkMatchWhole.Checked;
            Close();
        }

        protected override void OnShown(EventArgs e)
        {
            txtFindValue.Text = SearchTerm;
            chkMatchCase.Checked = MatchCase;
            chkMatchWhole.Checked = MatchWholeWord;
            base.OnShown(e);
            DialogResult = DialogResult.Cancel;
        }

        public void FindObject()
        {
            if (Parent is MainForm m)
            {

            }
        }

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
            this.txtFindValue = new System.Windows.Forms.TextBox();
            this.chkMatchWhole = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtFindValue
            // 
            this.txtFindValue.Location = new System.Drawing.Point(12, 12);
            this.txtFindValue.Name = "txtFindValue";
            this.txtFindValue.Size = new System.Drawing.Size(233, 20);
            this.txtFindValue.TabIndex = 0;
            // 
            // chkMatchWhole
            // 
            this.chkMatchWhole.AutoSize = true;
            this.chkMatchWhole.Location = new System.Drawing.Point(12, 45);
            this.chkMatchWhole.Name = "chkMatchWhole";
            this.chkMatchWhole.Size = new System.Drawing.Size(119, 17);
            this.chkMatchWhole.TabIndex = 1;
            this.chkMatchWhole.Text = "Match Whole Word";
            this.chkMatchWhole.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(12, 68);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(83, 17);
            this.chkMatchCase.TabIndex = 2;
            this.chkMatchCase.Text = "Match Case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(170, 62);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 3;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // FindNodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 97);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.chkMatchWhole);
            this.Controls.Add(this.txtFindValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindNodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Node";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFindValue;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private System.Windows.Forms.CheckBox chkMatchWhole;
        private System.Windows.Forms.Button btnOkay;
    }
}

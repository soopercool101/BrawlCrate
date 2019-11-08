using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    partial class TransformAttributesControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numScaleZ = new NumericInputBox();
            this.numScaleX = new NumericInputBox();
            this.numTransY = new NumericInputBox();
            this.numTransX = new NumericInputBox();
            this.numRotZ = new NumericInputBox();
            this.numRotX = new NumericInputBox();
            this.numRotY = new NumericInputBox();
            this.numScaleY = new NumericInputBox();
            this.numTransZ = new NumericInputBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.numScaleZ, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.numScaleX, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numTransY, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.numTransX, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.numRotZ, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.numRotX, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numRotY, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.numScaleY, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.numTransZ, 3, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 80);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Scale";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Translation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Rotation";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(75, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "X";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(225, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Z";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(150, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Y";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numScaleZ
            // 
            this.numScaleZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScaleZ.Integral = false;
            this.numScaleZ.Location = new System.Drawing.Point(225, 20);
            this.numScaleZ.Margin = new System.Windows.Forms.Padding(0);
            this.numScaleZ.MaximumValue = 3.402823E+38F;
            this.numScaleZ.MinimumValue = -3.402823E+38F;
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(75, 20);
            this.numScaleZ.TabIndex = 17;
            this.numScaleZ.Text = "1";
            // 
            // numScaleX
            // 
            this.numScaleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScaleX.Integral = false;
            this.numScaleX.Location = new System.Drawing.Point(75, 20);
            this.numScaleX.Margin = new System.Windows.Forms.Padding(0);
            this.numScaleX.MaximumValue = 3.402823E+38F;
            this.numScaleX.MinimumValue = -3.402823E+38F;
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(75, 20);
            this.numScaleX.TabIndex = 15;
            this.numScaleX.Text = "1";
            // 
            // numTransY
            // 
            this.numTransY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTransY.Integral = false;
            this.numTransY.Location = new System.Drawing.Point(150, 60);
            this.numTransY.Margin = new System.Windows.Forms.Padding(0);
            this.numTransY.MaximumValue = 3.402823E+38F;
            this.numTransY.MinimumValue = -3.402823E+38F;
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(75, 20);
            this.numTransY.TabIndex = 22;
            this.numTransY.Text = "0";
            // 
            // numTransX
            // 
            this.numTransX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTransX.Integral = false;
            this.numTransX.Location = new System.Drawing.Point(75, 60);
            this.numTransX.Margin = new System.Windows.Forms.Padding(0);
            this.numTransX.MaximumValue = 3.402823E+38F;
            this.numTransX.MinimumValue = -3.402823E+38F;
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(75, 20);
            this.numTransX.TabIndex = 21;
            this.numTransX.Text = "0";
            // 
            // numRotZ
            // 
            this.numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numRotZ.Integral = false;
            this.numRotZ.Location = new System.Drawing.Point(225, 40);
            this.numRotZ.Margin = new System.Windows.Forms.Padding(0);
            this.numRotZ.MaximumValue = 3.402823E+38F;
            this.numRotZ.MinimumValue = -3.402823E+38F;
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(75, 20);
            this.numRotZ.TabIndex = 20;
            this.numRotZ.Text = "0";
            // 
            // numRotX
            // 
            this.numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numRotX.Integral = false;
            this.numRotX.Location = new System.Drawing.Point(75, 40);
            this.numRotX.Margin = new System.Windows.Forms.Padding(0);
            this.numRotX.MaximumValue = 3.402823E+38F;
            this.numRotX.MinimumValue = -3.402823E+38F;
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(75, 20);
            this.numRotX.TabIndex = 18;
            this.numRotX.Text = "0";
            // 
            // numRotY
            // 
            this.numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numRotY.Integral = false;
            this.numRotY.Location = new System.Drawing.Point(150, 40);
            this.numRotY.Margin = new System.Windows.Forms.Padding(0);
            this.numRotY.MaximumValue = 3.402823E+38F;
            this.numRotY.MinimumValue = -3.402823E+38F;
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(75, 20);
            this.numRotY.TabIndex = 19;
            this.numRotY.Text = "0";
            // 
            // numScaleY
            // 
            this.numScaleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScaleY.Integral = false;
            this.numScaleY.Location = new System.Drawing.Point(150, 20);
            this.numScaleY.Margin = new System.Windows.Forms.Padding(0);
            this.numScaleY.MaximumValue = 3.402823E+38F;
            this.numScaleY.MinimumValue = -3.402823E+38F;
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(75, 20);
            this.numScaleY.TabIndex = 16;
            this.numScaleY.Text = "1";
            // 
            // numTransZ
            // 
            this.numTransZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTransZ.Integral = false;
            this.numTransZ.Location = new System.Drawing.Point(225, 60);
            this.numTransZ.Margin = new System.Windows.Forms.Padding(0);
            this.numTransZ.MaximumValue = 3.402823E+38F;
            this.numTransZ.MinimumValue = -3.402823E+38F;
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(75, 20);
            this.numTransZ.TabIndex = 23;
            this.numTransZ.Text = "0";
            // 
            // TransformAttributesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TransformAttributesControl";
            this.Size = new System.Drawing.Size(300, 80);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private Label label6;
        private Label label3;
        private Label label5;
        private Label label4;
        private NumericInputBox numTransY;
        private NumericInputBox numScaleX;
        private NumericInputBox numScaleY;
        private NumericInputBox numRotZ;
        private NumericInputBox numTransZ;
        private NumericInputBox numRotX;
        private NumericInputBox numTransX;
        private NumericInputBox numScaleZ;
        private NumericInputBox numRotY;
    }
}

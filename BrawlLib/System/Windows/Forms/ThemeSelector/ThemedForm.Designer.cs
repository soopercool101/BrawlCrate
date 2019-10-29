using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    partial class ThemedForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        protected override void OnLoad(EventArgs e)
        {
            foreach (Control c in Controls.Cast<Control>())
            {
                if (c.BackColor == System.Drawing.SystemColors.Window || c.BackColor == Color.White)
                {
                    c.BackColor = Color.Black;
                }
                else if (c.BackColor == System.Drawing.SystemColors.Control)
                {
                    c.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                }
                else if (c.BackColor == System.Drawing.SystemColors.ControlLight)
                {
                    c.BackColor = System.Drawing.SystemColors.ControlDark;
                }
                else if (c.BackColor == System.Drawing.SystemColors.ControlLightLight)
                {
                    c.BackColor = System.Drawing.SystemColors.ControlLightLight;
                }

                c.ForeColor = Color.White;
            }
            base.OnLoad(e);
        }

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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "ThemedForm";
        }

        #endregion
    }
}
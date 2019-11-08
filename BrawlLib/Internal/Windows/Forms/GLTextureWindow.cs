using BrawlLib.Internal.Windows.Controls;
using BrawlLib.OpenGL;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public class GLTextureWindow : Form
    {
        private readonly GLTexturePanel panel;

        public GLTextureWindow()
        {
            Controls.Add(panel = new GLTexturePanel {Dock = DockStyle.Fill});
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Text = "Texture Preview";
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
        }

        public DialogResult ShowDialog(IWin32Window owner, GLTexture texture)
        {
            panel.Texture = texture;
            ClientSize = new System.Drawing.Size(texture.Width, texture.Height);
            return ShowDialog(owner);
        }

        public void Show(IWin32Window owner, GLTexture texture)
        {
            panel.Texture = texture;
            ClientSize = new System.Drawing.Size(texture.Width, texture.Height);
            Show(owner);
        }

        protected override void OnClosed(EventArgs e)
        {
            panel.Texture = null;
            base.OnClosed(e);
        }
    }
}
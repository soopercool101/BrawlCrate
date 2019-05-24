using BrawlLib.OpenGL;

namespace System.Windows.Forms
{
    public class GLTextureWindow : Form
    {
        private readonly GLTexturePanel panel;

        public GLTextureWindow()
        {
            Controls.Add(panel = new GLTexturePanel() { Dock = DockStyle.Fill });
            FormBorderStyle = Forms.FormBorderStyle.SizableToolWindow;
            Text = "Texture Preview";
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
        }

        public DialogResult ShowDialog(IWin32Window owner, GLTexture texture)
        {
            panel.Texture = texture;
            ClientSize = new Drawing.Size(texture.Width, texture.Height);
            return ShowDialog(owner);
        }

        public void Show(IWin32Window owner, GLTexture texture)
        {
            panel.Texture = texture;
            ClientSize = new Drawing.Size(texture.Width, texture.Height);
            base.Show(owner);
        }

        protected override void OnClosed(EventArgs e)
        {
            panel.Texture = null;
            base.OnClosed(e);
        }
    }
}

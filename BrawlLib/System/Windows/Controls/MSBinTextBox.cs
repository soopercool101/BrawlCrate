using System.Drawing;

namespace System.Windows.Forms
{
    public class MSBinPreviewLabel : Label
    {
        //private bool _isEditing = false;

        public MSBinPreviewLabel()
        {
            AutoSize = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }
    }
}
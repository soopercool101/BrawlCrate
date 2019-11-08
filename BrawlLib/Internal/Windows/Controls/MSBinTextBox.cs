using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
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
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }
    }
}
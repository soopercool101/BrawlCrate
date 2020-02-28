using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }

        private readonly bool _transparent = true;

        protected override CreateParams CreateParams
        {
            get
            {
                if (_transparent)
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                    return createParams;
                }

                return base.CreateParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                m.Result = (IntPtr) (-1);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (_transparent)
            {
                // Do not paint background.
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }
    }
}
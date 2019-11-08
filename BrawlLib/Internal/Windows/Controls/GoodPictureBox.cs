using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class GoodPictureBox : Panel
    {
        internal static HatchBrush _brush =
            new HatchBrush(HatchStyle.LargeCheckerBoard, Color.LightGray, Color.GhostWhite);

        //private Color _transColor = Color.Magenta;
        //public Color TransparentColor
        //{
        //    get { return _transColor; }
        //    set { _transColor = value; }
        //}

        private Image _target;

        public Image Picture
        {
            get => _target;
            set
            {
                _target = value;
                Invalidate();
            }
        }

        public GoodPictureBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            if (_target == null)
            {
                return;
            }

            int w = _target.Width, h = _target.Height;
            Rectangle client = ClientRectangle;
            Rectangle bounds = new Rectangle(0, 0, w, h);

            float aspect = (float) w / h;
            float newaspect = (float) client.Width / client.Height;

            float scale = newaspect > aspect ? (float) client.Height / h : (float) client.Width / w;

            bounds.Width = (int) (w * scale);
            bounds.Height = (int) (h * scale);
            bounds.X = (client.Width - bounds.Width) >> 1;
            bounds.Y = (client.Height - bounds.Height) >> 1;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingMode = CompositingMode.SourceOver;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.AssumeLinear;

            //g.SetClip(bounds);

            g.FillRectangle(_brush, bounds);
            g.DrawImage(_target, bounds);
            //g.TranslateTransform(bounds.X, bounds.Y);
            //g.ScaleTransform(scale, scale);

            //g.FillRectangle(Brushes.Magenta, 0, 0, w, h);
            //g.DrawImage(_target, 0, 0);

            g.Flush();
        }
    }
}
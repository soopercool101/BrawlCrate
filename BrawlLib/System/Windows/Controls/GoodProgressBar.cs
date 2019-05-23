using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class GoodProgressBar : UserControl
    {
        private float _min = 0.0f, _max = 1.0f, _current = 0.0f;

        public float MinValue { get { return _min; } set { _min = value; } }
        public float MaxValue { get { return _max; } set { _max = value; } }
        public float CurrentValue { get { return _current; } set { _current = Math.Max(Math.Min(value, _max), _min); this.Invalidate(); } }
        public float Percent { get { return (_current - _min) / (_max - _min); } set { CurrentValue = (_max - _min) * value; } }

        public GoodProgressBar()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = this.ClientRectangle;

            float percent = this.Percent;

            bounds.Width = (int)(bounds.Width * percent);
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(bounds.Width, 0);
            //PointF p3 = new PointF(bounds.Width, bounds.Height / 2);
            //PointF p4 = new PointF(0, bounds.Height / 2);

            g.ResetClip();
            g.Clear(this.BackColor);

            if ((bounds.Width != 0) && (bounds.Height != 0))
                //using (PathGradientBrush b = new PathGradientBrush(new PointF[] { p1, p2, p3, p4 }, WrapMode.TileFlipY))
                //{
                //    b.CenterColor = Color.Gray;
                //    b.SurroundColors = new Color[] { Color.Transparent, Color.Transparent, Color.Turquoise, Color.Turquoise };
                //    g.FillRectangle(b, bounds);
                //}
                using (LinearGradientBrush b = new LinearGradientBrush(p1, p2, Color.Red, Color.Blue))
                {
                    g.FillRectangle(b, bounds);
                }

            g.Flush();
        }
    }
}

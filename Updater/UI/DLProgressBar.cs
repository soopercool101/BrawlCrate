using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class DLProgressBar : UserControl
    {
        private float _current;

        public DLProgressBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.Opaque, true);
        }

        public float MinValue { get; set; } = 0.0f;
        public float MaxValue { get; set; } = 1.0f;

        public float CurrentValue
        {
            get => _current;
            set
            {
                _current = Math.Max(Math.Min(value, MaxValue), MinValue);
                Invalidate();
            }
        }

        public float Percent
        {
            get => (_current - MinValue) / (MaxValue - MinValue);
            set => CurrentValue = (MaxValue - MinValue) * value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var bounds = ClientRectangle;

            var percent = Percent;

            bounds.Width = (int) (bounds.Width * percent);
            var p1 = new PointF(0, 0);
            var p2 = new PointF(bounds.Width, 0);
            //PointF p3 = new PointF(bounds.Width, bounds.Height / 2);
            //PointF p4 = new PointF(0, bounds.Height / 2);

            g.ResetClip();
            g.Clear(BackColor);

            if (bounds.Width != 0 && bounds.Height != 0)
                //using (PathGradientBrush b = new PathGradientBrush(new PointF[] { p1, p2, p3, p4 }, WrapMode.TileFlipY))
                //{
                //    b.CenterColor = Color.Gray;
                //    b.SurroundColors = new Color[] { Color.Transparent, Color.Transparent, Color.Turquoise, Color.Turquoise };
                //    g.FillRectangle(b, bounds);
                //}
                using (var b = new LinearGradientBrush(p1, p2, Color.Red, Color.Blue))
                {
                    g.FillRectangle(b, bounds);
                }

            g.Flush();
        }
    }
}
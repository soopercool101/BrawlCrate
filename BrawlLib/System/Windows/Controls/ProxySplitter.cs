namespace System.Windows.Forms
{
    public class ProxySplitter : Control
    {
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set
            {
                base.Dock = value;
                switch (value)
                {
                    case DockStyle.Left:
                    case DockStyle.Right:
                        Cursor = Cursors.VSplit;
                        break;

                    case DockStyle.Top:
                    case DockStyle.Bottom:
                        Cursor = Cursors.HSplit;
                        break;
                }
            }
        }

        public event SplitterEventHandler Dragged;

        public ProxySplitter() 
        { 
            Size = new Drawing.Size(5, 5);
            Dock = DockStyle.Left;
        }

        private bool _dragging = false;
        private int _lastX, _lastY;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _dragging = true;
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _dragging = false;
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            int xDiff = x - _lastX;
            int yDiff = y - _lastY;
            _lastX = x;
            _lastY = y;

            if (_dragging)
                if (Dragged != null)
                    Dragged(this, new SplitterEventArgs(xDiff, yDiff, Left, Top));

            base.OnMouseMove(e);
        }
    }
}

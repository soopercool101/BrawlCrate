using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public class TEST_selectedLinkPlacement : Form
    {
        private CollisionLink _selectedLink = null;

        //0 = unselected, 1 = left, 2 = right
        //this selection only represents how many collision planes that this link has.
        private int[] mySelection = null;

        public void UpdateSelection(ref CollisionLink Link)
        {
            _selectedLink = Link;
            mySelection = new int[_selectedLink._members.Count];

            for (int i = 0; i < _selectedLink._members.Count; i++)
            {
                CollisionPlane p = _selectedLink._members[i];

                if (ReferenceEquals(_selectedLink, p._linkLeft))
                {
                    mySelection[i] = 1;
                }
                else if (ReferenceEquals(_selectedLink, p._linkRight))
                {
                    mySelection[i] = 2;
                }
                else
                {
                    mySelection[i] = 0;
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (mySelection != null && mySelection.Length > 0)
            {
                int maxH = 10;
                int y = 0;

                for (int ms = 0; ms < mySelection.Length; ms++)
                {
                    int order = mySelection[ms];

                    if (y > ClientSize.Height)
                    {
                        break;
                    }

                    Drawing.Brush brush = order == 1 ? Drawing.Brushes.Aqua :
                        order == 2 ? Drawing.Brushes.Turquoise : Drawing.Brushes.Black;
                    int dvBy = order == 1 || order == 2 ? 2 : 1;
                    int locatX = order == 2 ? ClientSize.Width / dvBy : 0;
                    int szw = order == 0 ? ClientSize.Width : ClientSize.Width / dvBy;

                    e.Graphics.FillRectangle(brush, locatX, y, szw, maxH);
                    e.Graphics.DrawLine(new Drawing.Pen(Drawing.Color.Black, 1), 0, y + maxH, ClientSize.Width,
                        y + maxH);

                    y = y + maxH + 1;
                }
            }

            base.OnPaint(e);
        }

        public TEST_selectedLinkPlacement()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
        }


        #region Designer

        public void InitializeComponent()
        {
            ResumeLayout(true);

            Name = "TEST_selectedLinkPlacement";
            Text = "[TEST] Selected Link Associations";
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            TopMost = true;
            Opacity = 0.9d;
            Width = 200;
            Height = 400;

            PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
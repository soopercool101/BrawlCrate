using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib
{
    public class BrawlSplitter : Splitter
    {
        public Button Button { get; private set; }

        [Bindable(true)]
        [DefaultValue(null)]
        [Description("The control that the button in the splitter will show/hide")]
        public Control ControlToHide { get; set; }

        [Bindable(true)]
        [DefaultValue(typeof(Color), "ControlDark")]
        [Description("The color of the seperator on either side of the button")]
        public Color SeparatorColorDark { get; set; }

        [Bindable(true)]
        [DefaultValue(typeof(Color), "ControlLightLight")]
        [Description("The color of the seperator on either side of the button")]
        public Color SeparatorColorLight { get; set; }

        [Bindable(true)]
        [DefaultValue(0.5)]
        [Description("The maximum proportion (out of 1.0) of the length of the splitter taken up by the button")]
        public double MaxProportion { get; set; }

        [Bindable(true)]
        [DefaultValue(64)]
        [Description("Tne length of the button. Ignored if Proportion is not null")]
        public int ButtonLength { get; set; }

        private Cursor OldCursor;

        [Bindable(true)]
        [DefaultValue(true)]
        [Description("Whether the splitter can be moved")]
        public bool AllowResizing { get; set; }

        public BrawlSplitter()
        {
            SeparatorColorDark = SystemColors.ControlDark;
            SeparatorColorLight = SystemColors.ControlLightLight;
            Height = 11;
            MaxProportion = 0.5;
            ButtonLength = 64;
            AllowResizing = true;

            Button = new Button
            {
                Cursor = Cursors.Default,
                FlatStyle = FlatStyle.Popup
            };
            Controls.Add(Button);

            MouseEnter += BrawlSplitter_MouseEnter;

            Resize += BrawlSplitter_Resize;
            Button.Click += Button_Click;
            Button.Paint += Button_Paint;
        }

        private void BrawlSplitter_MouseEnter(object sender, EventArgs e)
        {
            if (!AllowResizing)
            {
                MinSize = SplitPosition;
                MinExtra = Parent.Width - SplitPosition;
                OldCursor = Cursor;
                Cursor = Cursors.Default;
            }
            else if (OldCursor != null)
            {
                Cursor = OldCursor;
                OldCursor = null;
            }
        }

        private void Button_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(SystemColors.ControlText);
            if (ControlToHide != null)
            {
                if (Width > Height)
                {
                    int top = Button.Height / 2 - 2;
                    int bottom = Button.Height / 2 + 2;
                    int left = Button.Width / 2 - 4;
                    int right = Button.Width / 2 + 4;
                    if (ControlToHide.Visible ^ (Dock == DockStyle.Bottom || Dock == DockStyle.Left))
                    {
                        e.Graphics.FillPolygon(brush, new Point[]
                        {
                            new Point(Button.Width / 2, top),
                            new Point(left, bottom),
                            new Point(right, bottom)
                        });
                    }
                    else
                    {
                        e.Graphics.FillPolygon(brush, new Point[]
                        {
                            new Point(Button.Width / 2, bottom),
                            new Point(left, top),
                            new Point(right, top)
                        });
                    }
                }
                else
                {
                    int top = Button.Height / 2 - 4;
                    int bottom = Button.Height / 2 + 4;
                    int left = Button.Width / 2 - 2;
                    int right = Button.Width / 2 + 2;
                    if (ControlToHide.Visible ^ (Dock == DockStyle.Bottom || Dock == DockStyle.Left))
                    {
                        e.Graphics.FillPolygon(brush, new Point[]
                        {
                            new Point(right, Button.Height / 2),
                            new Point(left, top),
                            new Point(left, bottom)
                        });
                    }
                    else
                    {
                        e.Graphics.FillPolygon(brush, new Point[]
                        {
                            new Point(left, Button.Height / 2),
                            new Point(right, top),
                            new Point(right, bottom)
                        });
                    }
                }
            }
        }

        private void BrawlSplitter_Resize(object sender, EventArgs e)
        {
            if (Width > Height)
            {
                // wide
                if (Width * MaxProportion < ButtonLength)
                {
                    Button.Width = (int) (Width * MaxProportion);
                    Button.Height = Height;
                    Button.Top = 0;
                    Button.Left = (int) (Width * (1.0 - MaxProportion) / 2);
                }
                else
                {
                    Button.Width = ButtonLength;
                    Button.Height = Height;
                    Button.Top = 0;
                    Button.Left = (Width - ButtonLength) / 2;
                }
            }
            else if (Width < Height)
            {
                // tall
                if (Height * MaxProportion < ButtonLength)
                {
                    Button.Width = Width;
                    Button.Height = (int) (Height * MaxProportion);
                    Button.Top = (int) (Height * (1.0 - MaxProportion) / 2);
                    Button.Left = 0;
                }
                else
                {
                    Button.Width = Width;
                    Button.Height = ButtonLength;
                    Button.Top = (Height - ButtonLength) / 2;
                    Button.Left = 0;
                }
            }
            else
            {
                // square
                Button.Width = Width;
                Button.Height = Height;
                Button.Top = 0;
                Button.Left = 0;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (ControlToHide != null)
            {
                ControlToHide.Visible = !ControlToHide.Visible;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen pen = new Pen(SeparatorColorDark, 1);
            Pen pen2 = new Pen(SeparatorColorLight, 1);
            if (Width > Height)
            {
                // wide
                int middle = Height / 2;
                e.Graphics.DrawLine(pen, 0, middle, Width, middle);
                e.Graphics.DrawLine(pen2, 0, middle - 1, Width, middle - 1);
            }
            else if (Width < Height)
            {
                // tall
                int middle = Width / 2;
                e.Graphics.DrawLine(pen, middle, 0, middle, Height);
                e.Graphics.DrawLine(pen2, middle - 1, 0, middle - 1, Height);
            }
        }
    }
}
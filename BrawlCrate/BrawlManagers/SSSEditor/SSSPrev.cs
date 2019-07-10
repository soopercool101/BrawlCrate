using System;
using System.Drawing;
using System.Windows.Forms;
using BrawlLib.Wii.Animations;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlManagerLib;

namespace SSSEditor
{
    public partial class SSSPrev : UserControl
    {
        private bool needsReload;

        private BRRESNode miscdata80;

        /// <summary>
        /// Specify a root ResourceNode to search for SSS icons in. The node used will be the BRES containing MenSelmapCursorPly.1.
        /// </summary>
        public BRRESNode MiscData80
        {
            set
            {
                miscdata80 = value;
                needsReload = true;
            }
        }

        private bool _myMusic;

        public bool MyMusic
        {
            get => _myMusic;
            set
            {
                _myMusic = value;
                needsReload = true;
            }
        }

        private Tuple<Image, RectangleF>[] icons;

        private int _numIcons;

        public int NumIcons
        {
            get => _numIcons;
            set
            {
                _numIcons = value;
                numericUpDown1.Value = value;
                needsReload = true;
            }
        }

        private byte[] _iconOrder;

        public byte[] IconOrder
        {
            get => _iconOrder;
            set
            {
                _iconOrder = value;
                needsReload = true;
            }
        }

        public SSSPrev()
        {
            InitializeComponent();
            ResizeRedraw = true;
            _numIcons = 23;
            needsReload = true;
        }

        private static int BRAWLWIDTH = 75;  // an estimate
        private static int BRAWLHEIGHT = 50; // an estimate

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (needsReload)
            {
                needsReload = false;
                ReloadIcons();
            }

            if (icons == null)
            {
                return;
            }

            foreach (Tuple<Image, RectangleF> tuple in icons)
            {
                if (tuple != null)
                {
                    e.Graphics.DrawImage(tuple.Item1,
                        tuple.Item2.X * Width,
                        tuple.Item2.Y * Height,
                        tuple.Item2.Width * Width,
                        tuple.Item2.Height * Height);
                }
            }
        }

        private static int OFFSET = 1;

        private void ReloadIcons()
        {
            if (miscdata80 == null)
            {
                return;
            }

            CHR0Node chr0 =
                miscdata80.FindChild("AnmChr(NW4R)/MenSelmapPos_TopN__" + (MyMusic ? "1" : "0"), false) as CHR0Node;
            if (chr0 == null)
            {
                return;
            }

            icons = new Tuple<Image, RectangleF>[_numIcons + 1];

            CHR0EntryNode entry = chr0.FindChild("MenSelmapPos_TopN", false) as CHR0EntryNode;
            Vector3 offset = entry.GetAnimFrame(_numIcons + OFFSET).Translation;

            for (int i = 1; i <= _numIcons; i++)
            {
                entry = chr0.FindChild("pos" + i.ToString("D2"), false) as CHR0EntryNode;
                CHRAnimationFrame frame = entry.GetAnimFrame(_numIcons + OFFSET, true);
                float x = (BRAWLWIDTH / 2 + frame.Translation._x + offset._x) / BRAWLWIDTH;
                float y = (BRAWLHEIGHT / 2 - frame.Translation._y - offset._y) / BRAWLHEIGHT;
                float w = 6.4f * frame.Scale._x / BRAWLWIDTH;
                float h = 5.6f * frame.Scale._y / BRAWLHEIGHT;
                RectangleF r = new RectangleF(x, y, w, h);

                TextureContainer tc = new TextureContainer(miscdata80,
                    IconOrder != null && i <= IconOrder.Length ? IconOrder[i - 1] : 100);
                if (tc.icon.tex0 == null)
                {
                    continue;
                }

                Image image = tc.icon.tex0.GetImage(0);

                icons[i] = new Tuple<Image, RectangleF>(image, r);
            }

            TEX0Node nexttex0 = miscdata80.FindChild("Textures(NW4R)/MenSelmapIcon_Change.1", false) as TEX0Node;
            if (nexttex0 != null)
            {
                float NEXTOFFSET = 10.8f;
                entry = chr0.FindChild("pos" + (_numIcons + 1).ToString("D2"), false) as CHR0EntryNode;
                CHRAnimationFrame frame2 = entry.GetAnimFrame(_numIcons + OFFSET, true);
                float x2 = (BRAWLWIDTH / 2 + frame2.Translation._x - NEXTOFFSET) / BRAWLWIDTH;
                float y2 = (BRAWLHEIGHT / 2 - frame2.Translation._y) / BRAWLHEIGHT;
                float w2 = 14.4f * frame2.Scale._x / BRAWLWIDTH;
                float h2 = 4.8f * frame2.Scale._y / BRAWLHEIGHT;
                RectangleF r2 = new RectangleF(x2, y2, w2, h2);
                Image image2 = nexttex0.GetImage(0);
                icons[0] = new Tuple<Image, RectangleF>(image2, r2); //pos00 is not used anyway, so let's overwrite it
            }

            Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != _numIcons)
            {
                NumIcons = (int) numericUpDown1.Value;
                Refresh();
            }
        }
    }
}
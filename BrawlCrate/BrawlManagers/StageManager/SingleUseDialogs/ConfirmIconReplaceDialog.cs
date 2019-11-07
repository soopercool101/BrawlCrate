using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs
{
    public partial class ConfirmIconReplaceDialog : Form
    {
        public Bitmap CurrentImage;
        public Bitmap NewImage;

        public ConfirmIconReplaceDialog()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(Color.Black),
                0, 0, Width,
                panel1.Location.Y + panel1.Height / 2);
            e.Graphics.DrawImage((Image) CurrentImage, (Point) panel1.Location);
            e.Graphics.DrawImage((Image) NewImage, (Point) panel2.Location);
        }
    }
}
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.SongManager
{
    public partial class CustomSongVolumeStatusForm : Form
    {
        public CustomSongVolumeStatusForm(CustomSongVolumeEditor editor)
        {
            InitializeComponent();
            lblStatus.Text = editor.VolumeToolTip;
            button1.Click += (o, e) => { editor.SetVolume((byte) numericUpDown1.Value); };
        }
    }
}
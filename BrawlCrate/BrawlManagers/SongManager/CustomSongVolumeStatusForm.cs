using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlSongManager
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
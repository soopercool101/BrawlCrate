using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlCrate.StageManager
{
    public partial class EnterSizeDialog : Form
    {
        public Size SizeEntry
        {
            get => new Size((int) nudWidth.Value, (int) nudHeight.Value);
            set
            {
                nudWidth.Value = value.Width;
                nudHeight.Value = value.Height;
            }
        }

        public EnterSizeDialog()
        {
            InitializeComponent();
        }
    }
}
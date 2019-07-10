using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlStageManager.SingleUseDialogs
{
    public partial class BatchAddPAT0Dialog : Form
    {
        public bool UseSameSelchrMarksForAll => radioCopyFromPrevious.Checked;

        public bool UseSameSelmapMarksForAll => radioCopyFromPrevious.Checked || radioNewSelchrSameSelmap.Checked;

        public bool AddNewTextures => chkAddNewTextures.Checked;

        public BatchAddPAT0Dialog()
        {
            InitializeComponent();
        }
    }
}
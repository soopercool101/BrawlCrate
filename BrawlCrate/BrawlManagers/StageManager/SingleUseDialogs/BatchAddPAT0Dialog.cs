using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs
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
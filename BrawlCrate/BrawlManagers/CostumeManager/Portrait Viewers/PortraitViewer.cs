using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers
{
    public class PortraitViewer : UserControl
    {
        protected string currentDirectory { get; set; }

        public virtual void UpdateDirectory(string directory)
        {
            currentDirectory = directory;
        }

        public virtual bool UpdateImage(int charNum, int costumeNum)
        {
            return false;
        }
    }
}
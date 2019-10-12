using System.Windows.Forms;

namespace BrawlCrate.CostumeManager
{
    public class PortraitViewer : UserControl
    {
        public virtual void UpdateDirectory()
        {
        }

        public virtual bool UpdateImage(int charNum, int costumeNum)
        {
            return false;
        }
    }
}
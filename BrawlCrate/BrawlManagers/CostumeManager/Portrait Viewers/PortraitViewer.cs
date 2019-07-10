using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlCostumeManager
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
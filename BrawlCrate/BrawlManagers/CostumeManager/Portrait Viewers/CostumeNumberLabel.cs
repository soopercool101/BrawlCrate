using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlCostumeManager
{
    public class CostumeNumberLabel : Label
    {
        public void UpdateImage(int charNum, int costumeNum, bool confident)
        {
            Text = string.Format("Char {0} / Costume {1}", charNum, costumeNum);
            if (!confident)
            {
                Text += " (?)";
            }

            BackColor = costumeNum < 0 ? Color.Red
                : confident ? Color.LightGreen
                : Color.Yellow;
        }
    }
}
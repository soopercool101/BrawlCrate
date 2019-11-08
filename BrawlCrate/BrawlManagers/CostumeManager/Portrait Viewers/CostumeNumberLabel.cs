using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers
{
    public class CostumeNumberLabel : Label
    {
        public void UpdateImage(int charNum, int costumeNum, bool confident)
        {
            Text = $"Char {charNum} / Costume {costumeNum}";
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
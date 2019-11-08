using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.CostumeManager
{
    public static class KirbyCopy
    {
        public static void Copy(string kirbypath, string hatpath)
        {
            ResourceNode kirby = NodeFactory.FromFile(null, kirbypath);
            string temphat = Path.GetTempFileName();
            File.Copy(hatpath, temphat, true);
            ResourceNode hat = NodeFactory.FromFile(null, temphat);

            TEX0Node skin = (TEX0Node) kirby.FindChildByType("PlyKirby5KSkin", true, ResourceType.TEX0);
            if (skin == null)
            {
                MessageBox.Show(null, "Could not find the texture PlyKirby5KSkin in " + kirbypath, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string temptex = Path.GetTempFileName();
            skin.Export(temptex);

            TEX0Node hatskin = (TEX0Node) hat.FindChildByType("WpnKirbyKirbyMewtwoCap", true, ResourceType.TEX0);
            if (hatskin == null)
            {
                MessageBox.Show(null, "Could not find the texture WpnKirbyKirbyMewtwoCap in " + hatpath, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hatskin.Replace(temptex);
            hat.Merge();
            hat.Export(hatpath);

            hat.Dispose();
            kirby.Dispose();

            File.Delete(temphat);
            File.Delete(temptex);

            MessageBox.Show(Path.GetFileName(hatpath) + " has been updated.");
        }
    }
}
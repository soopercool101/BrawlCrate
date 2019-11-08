using System.IO;

namespace BrawlCrate.BrawlManagers.CostumeManager
{
    public class FighterFile
    {
        private FileInfo path;
        private int charNum, costumeNum;

        public string FullName => path.FullName;

        public int CharNum => charNum;

        public int CostumeNum => costumeNum;

        public FighterFile(FileInfo path, int charNum, int costumeNum)
        {
            this.path = path;
            this.charNum = charNum;
            this.costumeNum = costumeNum;
        }

        public FighterFile(string path, int charNum, int costumeNum) :
            this(new FileInfo(path), charNum, costumeNum)
        {
        }

        public override string ToString()
        {
            string name = path.Name;
            if (!path.Exists)
            {
                name = "(" + name + ")";
            }

            if (charNum == 17 && costumeNum > 0)
            {
                name += " - do not use this file";
            }

            return name;
        }
    }
}
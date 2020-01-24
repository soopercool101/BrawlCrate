using BrawlLib.SSBB.ResourceNodes;
using System.IO;

namespace BrawlCrate.BrawlManagers.CostumeManager
{
    public class FighterFile
    {
        private FileInfo path;
        private int costumeNum;

        public string FullName => path.FullName;

        public int CharNum { get; }

        public virtual int CostumeNum => costumeNum;

        public FighterFile(FileInfo path, int charNum, int costumeNum)
        {
            this.path = path;
            this.CharNum = charNum;
            this.costumeNum = costumeNum;
        }

        public FighterFile(string path, int charNum, int costumeNum) :
            this(new FileInfo(path), charNum, costumeNum)
        {
        }

        public FighterFile(FighterFile f) :
            this(new FileInfo(f.FullName), f.CharNum, f.CostumeNum)
        {
        }

        public override string ToString()
        {
            string name = path.Name;
            if (!path.Exists)
            {
                name = "(" + name + ")";
            }

            if (CharNum == 17 && costumeNum > 0)
            {
                name += " - do not use this file";
            }

            return name;
        }
    }

    public class GameWatchCostumeFile : FighterFile
    {
        public CLR0Node CLR0Animation { get; }

        public int FrameIndex { get; }

        public override int CostumeNum => base.CostumeNum < 0 ? base.CostumeNum : base.CostumeNum + FrameIndex;

        public GameWatchCostumeFile(FighterFile f, CLR0Node clr, int index) :
            base(f)
        {
            CLR0Animation = clr;
            FrameIndex = index;
        }

        public override string ToString()
        {
            return $"⮡ {FrameIndex}";
        }
    }
}
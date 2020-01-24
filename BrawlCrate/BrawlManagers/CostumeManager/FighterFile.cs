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

        public FighterFile(FileInfo p, int charNum, int costume)
        {
            path = p;
            CharNum = charNum;
            costumeNum = costume;
        }

        public FighterFile(string p, int charNum, int costume) :
            this(new FileInfo(p), charNum, costume)
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

        private int _index;

        public int FrameIndex => _index + 1;

        public override int CostumeNum => base.CostumeNum < 0 ? base.CostumeNum : base.CostumeNum + _index;

        public GameWatchCostumeFile(FighterFile f, CLR0Node clr, int index) :
            base(f)
        {
            CLR0Animation = clr;
            _index = index;
        }

        public override string ToString()
        {
            return $"  ⮡ {_index:D2}";
        }
    }
}

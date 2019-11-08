using BrawlLib.BrawlManagerLib.GCT.ReadWrite;
using BrawlLib.BrawlManagerLib.Songs;
using System.IO;
using System.Linq;

namespace BrawlCrate.BrawlManagers.SongManager
{
    public class SongInfo
    {
        public SongInfo(FileInfo f)
        {
            File = f;
        }

        public SongInfo(string s)
        {
            File = new FileInfo(s + ".brstm");
        }

        public SongInfo(ushort id, CustomSongVolumeCodeset csv)
        {
            string s = (from b in SongIDMap.Songs
                        where b.ID == id
                        select b.Filename).FirstOrDefault() ?? id.ToString("X4");
            File = new FileInfo(s + ".brstm");
            ID = id;
            CSV = csv;
        }

        public FileInfo File { get; private set; }

        private ushort ID;
        private CustomSongVolumeCodeset CSV;

        public override string ToString()
        {
            string s = File.Name;
            if (File.Exists)
            {
                s = "* " + s;
            }
            else
            {
                s = "  " + s;
            }

            if (CSV != null)
            {
                s += " - " + CSV.Settings[ID];
            }

            return s;
        }
    }
}
using System.IO;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public class SongListItem
    {
        public FileInfo File { get; private set; }

        public SongListItem(FileInfo file)
        {
            File = file;
        }

        public SongListItem(string path)
        {
            File = new FileInfo(path);
        }

        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(File.FullName);
        }

        public override bool Equals(object obj)
        {
            if (obj is SongListItem)
            {
                return File.Equals((SongListItem) obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return File.GetHashCode();
        }
    }
}
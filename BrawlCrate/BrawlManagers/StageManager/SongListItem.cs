using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BrawlStageManager
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
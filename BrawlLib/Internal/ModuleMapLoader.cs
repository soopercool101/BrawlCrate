using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal
{
    public static class ModuleMapLoader
    {
        public static readonly Dictionary<string, Dictionary<uint, string>> MapFiles;

        static ModuleMapLoader()
        {
            MapFiles = new Dictionary<string, Dictionary<uint, string>>();
            string mapPath = Path.Combine(Application.StartupPath, "InternalDocumentation", "Module", "maps");
            if (Directory.Exists(mapPath))
            {
                DirectoryInfo d = new DirectoryInfo(mapPath);
                foreach (FileInfo map in d.GetFiles().Where(f => f.Extension.Equals(".map", StringComparison.OrdinalIgnoreCase)))
                {
                    Dictionary<uint, string> currentMap = new Dictionary<uint, string>();
                    List<string> fileList = new List<string>(File.ReadAllLines(map.FullName));
                    foreach (string s in fileList)
                    {
                        if (string.IsNullOrWhiteSpace(s))
                        {
                            continue;
                        }

                        try
                        {
                            uint key = uint.Parse(s.Substring(0, 8), NumberStyles.HexNumber);
                            if (currentMap.ContainsKey(key))
                            {
                                continue;
                            }

                            currentMap.Add(key, s.Substring(9));
                        }
                        catch
                        {
                            // continue
                        }
                    }
                    MapFiles.Add(Path.GetFileNameWithoutExtension(map.FullName), currentMap);
                }
            }
        }
    }
}

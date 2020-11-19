using System;
using System.Collections.Generic;
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
                    string mapName = Path.GetFileNameWithoutExtension(map.FullName);
                    bool mainDol = mapName.Equals("RSBE01", StringComparison.OrdinalIgnoreCase) || mapName.Equals("sora", StringComparison.OrdinalIgnoreCase);
                    foreach (string s in fileList)
                    {
                        if (string.IsNullOrWhiteSpace(s))
                        {
                            continue;
                        }

                        try
                        {
                            string offset = s.Substring(0, 8);
                            
                            uint key = Convert.ToUInt32(offset, 16);
                            if (currentMap.ContainsKey(key))
                            {
                                continue;
                            }

                            currentMap.Add(key, s.Trim().Substring(s.LastIndexOf(' ')).Trim());
                        }
                        catch
                        {
                            // continue
                        }
                    }

                    string mapKey = mainDol ? "main.dol" : mapName;
                    if (!MapFiles.ContainsKey(mapKey))
                    {
                        MapFiles.Add(mapKey, currentMap);
                    }
                }
            }
        }
    }
}

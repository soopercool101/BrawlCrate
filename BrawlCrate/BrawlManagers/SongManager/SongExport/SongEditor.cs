using BrawlLib.BrawlManagerLib.GCT.ReadWrite;
using BrawlLib.BrawlManagerLib.Songs;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.IO;
using System.Linq;

namespace BrawlCrate.BrawlManagers.SongManager.SongExport
{
    internal class SongEditor
    {
        private static readonly string[] GCT_PATHS =
        {
            "RSBE01.gct",
            "/data/gecko/codes/RSBE01.gct",
            "/codes/RSBE01.gct",
            "../../../../codes/RSBE01.gct"
        };

        private static readonly string[] MUM_PATHS =
        {
            "../../menu2/mu_menumain.pac",
            "../../menu2/mu_menumain_en.pac",
            "../../../pfmenu2/mu_menumain.pac",
            "../../../pfmenu2/mu_menumain_en.pac"
        };

        private static readonly string[] INFO_PATHS =
        {
            "..\\..\\info2\\info.pac",
            "..\\..\\info2\\info_en.pac",
            "..\\info.pac"
        };

        private static readonly string[] TRNG_PATHS =
        {
            "..\\..\\info2\\info_training.pac",
            "..\\..\\info2\\info_training_en.pac",
            "..\\info_training.pac"
        };

        // MU_MenuMain Details
        private string mumPath;
        private MSBinNode mumMsbn;

        // INFO Details
        private string infoPath;
        private MSBinNode infoMsbn;

        // TRNG Details
        private string trngPath;
        private MSBinNode trngMsbn;

        // Volume Details
        private string gctPath;
        private CustomSongVolumeCodeset gctCsv;

        public SongEditor()
        {
        }

        public void PrepareAllResources()
        {
            PrepareMUM();
            PrepareINFO();
            PrepareTRNG();
            PrepareGCT();
        }

        public Song ReadSong(string filename)
        {
            Song mapSong = GetDefaultSong(filename);
            if (mapSong == null)
            {
                return null;
            }

            return UpdateSongFromFileData(mapSong);
        }

        public Song GetDefaultSong(string filename)
        {
            return (from s in SongIDMap.Songs
                    where s.Filename == filename
                    select s).FirstOrDefault();
        }

        public void WriteSong(Song song)
        {
            if (song.InfoPacIndex.HasValue)
            {
                if (mumMsbn != null)
                {
                    mumMsbn._strings[song.InfoPacIndex.Value] = song.DefaultName;
                    mumMsbn.SignalPropertyChange();
                }

                if (infoMsbn != null)
                {
                    infoMsbn._strings[song.InfoPacIndex.Value] = song.DefaultName;
                    infoMsbn.SignalPropertyChange();
                }

                if (trngMsbn != null)
                {
                    trngMsbn._strings[song.InfoPacIndex.Value] = song.DefaultName;
                    trngMsbn.SignalPropertyChange();
                }
            }

            if (gctCsv != null)
            {
                if (song.DefaultVolume.HasValue)
                {
                    gctCsv.Settings[song.ID] = song.DefaultVolume.Value;
                }
                else if (gctCsv.Settings.ContainsKey(song.ID))
                {
                    gctCsv.Settings.Remove(song.ID);
                }
            }
        }

        public void SaveResources()
        {
            if (mumMsbn != null && mumPath != null)
            {
                SaveMUM();
            }

            if (infoMsbn != null && infoPath != null)
            {
                SaveINFO();
            }

            if (trngMsbn != null && trngPath != null)
            {
                SaveTRNG();
            }

            if (gctCsv != null && gctPath != null)
            {
                SaveGCT();
            }
        }

        private Song UpdateSongFromFileData(Song song)
        {
            string title = song.DefaultName;
            if (song.InfoPacIndex.HasValue)
            {
                title = mumMsbn._strings[song.InfoPacIndex.Value];
            }

            byte? volume = song.DefaultVolume;
            if (gctCsv != null && gctCsv.Settings.ContainsKey(song.ID))
            {
                volume = gctCsv.Settings[song.ID];
            }

            return new Song(title, song.Filename, song.ID, volume, song.InfoPacIndex);
        }

        public void PrepareMUM()
        {
            mumPath = FindFile(MUM_PATHS);
            mumMsbn = LoadPacMsbn(mumPath, "Misc Data [7]");
        }

        public void SaveMUM()
        {
            mumMsbn.Rebuild();
            SavePacMsbn(mumMsbn, mumPath, "Misc Data [7]");
        }

        public void PrepareINFO()
        {
            infoPath = FindFile(INFO_PATHS);
            infoMsbn = LoadPacMsbn(infoPath, "Misc Data [140]");
        }

        public void SaveINFO()
        {
            infoMsbn.Rebuild();
            SavePacMsbn(infoMsbn, infoPath, "Misc Data [140]");
        }

        public void PrepareTRNG()
        {
            trngPath = FindFile(TRNG_PATHS);
            trngMsbn = LoadPacMsbn(trngPath, "Misc Data [140]");
        }

        public void SaveTRNG()
        {
            trngMsbn.Rebuild();
            SavePacMsbn(trngMsbn, trngPath, "Misc Data [140]");
        }

        public void PrepareGCT()
        {
            gctPath = FindFile(GCT_PATHS);
            gctCsv = new CustomSongVolumeCodeset(File.ReadAllBytes(gctPath));
        }

        public void SaveGCT()
        {
            File.WriteAllBytes(gctPath, gctCsv.ExportGCT());
        }

        private MSBinNode LoadPacMsbn(string path, string childNodeName)
        {
            using (ResourceNode node = NodeFactory.FromFile(null, path))
            {
                MSBinNode childNode = node.FindChild(childNodeName, true) as MSBinNode;
                if (childNode == null)
                {
                    throw new Exception("Node '" + childNodeName + "' not found in '" + path + "'");
                }

                return childNode;
            }
        }

        private void SavePacMsbn(MSBinNode msbn, string pacPath, string childNodeName)
        {
            string tmpPac = Path.GetTempFileName();
            string tmpMsbn = Path.GetTempFileName();
            msbn.Export(tmpMsbn);
            File.Copy(pacPath, tmpPac, true);

            using (ResourceNode tmpPacNode = NodeFactory.FromFile(null, tmpPac))
            {
                MSBinNode tmpPacChildNode = tmpPacNode.FindChild(childNodeName, true) as MSBinNode;
                if (tmpPacChildNode == null)
                {
                    throw new Exception("Error saving '" + pacPath
                                                         + "': The file does not appear to have a '" + childNodeName +
                                                         "'");
                }
                else
                {
                    tmpPacChildNode.Replace(tmpMsbn);
                    tmpPacNode.Merge();
                    tmpPacNode.Export(pacPath);
                }
            }

            File.Delete(tmpPac);
            File.Delete(tmpMsbn);
        }

        private string FindFile(string[] paths)
        {
            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            throw new FileNotFoundException("Could not find any file in: ['"
                                            + string.Join("', '", paths) + "']");
        }
    }
}
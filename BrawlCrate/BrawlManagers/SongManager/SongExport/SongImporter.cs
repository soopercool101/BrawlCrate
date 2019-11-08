using BrawlLib.BrawlManagerLib;
using BrawlLib.BrawlManagerLib.Songs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.SongManager.SongExport
{
    /// <summary>
    /// Contains logic for user to import music song files with a format like
    /// {filename}.{volume}.{title}.brstm e.g. A01.80.Jungle Japes.brstm
    /// </summary>
    public class SongImporter
    {
        private static readonly Regex filenameRegex = new Regex(@"(\w\d\d)\.(\d+)\.(.*)\.brstm");

        private ProgressDialog prog;
        private SongEditor songEditor;
        private List<string> warnings;
        private Dictionary<string, string> importedSongs;

        public SongImporter()
        {
            prog = new ProgressDialog();
            songEditor = new SongEditor();
            warnings = new List<string>();
            importedSongs = new Dictionary<string, string>();
        }

        public void Import(string musicImportDir)
        {
            Log("Importing music from: " + musicImportDir);
            prog.ClearLog();

            DirectoryInfo importDir = new DirectoryInfo(musicImportDir);
            FileInfo[] brstmFiles = importDir.GetFiles("*.brstm", SearchOption.AllDirectories);
            if (brstmFiles.Length == 0)
            {
                MessageBox.Show("No *.brstm files were found in the selected directory!");
                return;
            }

            DialogResult confirmResult = MessageBox.Show("About to import from: " + musicImportDir
                                                                                  + "\nThis will overwrite info.pac, mu_menumain and the GCT codeset. "
                                                                                  + "It is recommended to make a backup before continuing."
                                                                                  + "\nDo you want to continue?",
                "Confirm Import", MessageBoxButtons.OKCancel);
            if (confirmResult != DialogResult.OK)
            {
                return;
            }

            prog.ProgressTitle = "Importing songs...";
            prog.InProgressLabel = "Importing songs...";
            prog.ProgressCompletionAt = 100;

            BackgroundWorker bgw = SetupBackgroundImport(brstmFiles, prog);
            bgw.RunWorkerAsync();

            prog.ShowDialog();
        }

        private BackgroundWorker SetupBackgroundImport(FileInfo[] brstmFiles, ProgressDialog prog)
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                ImportSongs(brstmFiles, sender as BackgroundWorker);
            };
            bgw.WorkerReportsProgress = true;
            bgw.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
            {
                prog.Progress = e.ProgressPercentage;
                if (e.UserState != null)
                {
                    prog.InProgressLabel = (string) e.UserState;
                }
            };
            bgw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                prog.Progress = 100;
                Log("");
                string warnMsg = "";
                if (warnings.Count > 0)
                {
                    Log("WARNING: There were " + warnings.Count + " warnings during import:");
                    foreach (string warn in warnings)
                    {
                        Log(warn);
                    }

                    Log("");
                    warnMsg = " (with " + warnings.Count + " warnings)";
                }

                if (e.Error != null)
                {
                    Warn("Error importing songs." + e.Error.Message);
                }
                else
                {
                    Log("Note: it is recommended to defragment your SD card after importing songs onto it.");
                    Log("Completed import" + warnMsg + "!");
                }
            };
            return bgw;
        }

        private void ImportSongs(FileInfo[] brstmFiles, BackgroundWorker bgw)
        {
            PrepareResources();
            for (int i = 0; i < brstmFiles.Length; i++)
            {
                bgw.ReportProgress(i * 100 / brstmFiles.Length);
                FileInfo file = brstmFiles[i];
                ImportSong(file);
            }

            songEditor.SaveResources();
        }

        private void ImportSong(FileInfo file)
        {
            Match match = filenameRegex.Match(file.Name);
            if (!match.Success)
            {
                Warn("Skipped file due to incorrect filename format: " + file.FullName);
                return;
            }

            if (file.Length == 0)
            {
                Log("Skipped empty file: " + file.FullName);
                return;
            }

            string filename = match.Groups[1].Value;
            byte fileVolume = byte.Parse(match.Groups[2].Value);
            string title = match.Groups[3].Value;

            Song defSong = songEditor.GetDefaultSong(filename);
            Song curSong = songEditor.ReadSong(filename);
            if (defSong == null)
            {
                Log("Skipped unknown song: " + file.FullName);
                return;
            }

            if (importedSongs.ContainsKey(filename))
            {
                string first = importedSongs[filename];
                Warn("Song file '" + filename + "' has already been imported from '"
                     + first + "'. Skipping subsequent file: " + file.Name);
                return;
            }

            importedSongs.Add(filename, file.Name);

            if (FileOperations.SanitizeFilename(curSong.DefaultName) == title)
            {
                title = curSong.DefaultName;
            }

            if (fileVolume > 127)
            {
                Warn("Volume decreased to maximum of 127 for file: " + file.FullName);
                fileVolume = 127;
            }

            byte? volume = fileVolume;
            if (fileVolume == 0)
            {
                Log("Ignoring 0 volume for: " + file.FullName);
                volume = null;
            }
            else if (defSong.DefaultVolume.HasValue
                     && defSong.DefaultVolume.Value == fileVolume)
            {
                volume = null;
            }

            Song song = new Song(title, filename, defSong.ID, volume, defSong.InfoPacIndex);
            songEditor.WriteSong(song);

            File.Copy(file.FullName, new SongInfo(song.Filename).File.FullName, true);
        }

        private void PrepareResources()
        {
            // mu_menumain and info are required, so don't catch their exceptions...
            songEditor.PrepareMUM();
            songEditor.PrepareINFO();
            try
            {
                songEditor.PrepareTRNG();
            }
            catch (Exception trngExc)
            {
                DialogResult confirmResult = MessageBox.Show("Unable to load 'training_info.pac'."
                                                             + " Song title data for training mode and SSE will not be updated."
                                                             + "\nDo you want to continue?", "Confirm Import",
                    MessageBoxButtons.OKCancel);
                if (confirmResult != DialogResult.OK)
                {
                    throw new Exception("User cancelled due to lack of 'training_info.pac': " + trngExc.Message,
                        trngExc);
                }
            }

            try
            {
                songEditor.PrepareGCT();
            }
            catch (Exception gctExc)
            {
                DialogResult confirmResult = MessageBox.Show("Unable to load GCT codes 'RSBE01.gct'."
                                                             + " Song volume data will not be updated."
                                                             + "\nDo you want to continue?", "Confirm Import",
                    MessageBoxButtons.OKCancel);
                if (confirmResult != DialogResult.OK)
                {
                    throw new Exception("User cancelled due to lack of 'RSBE01.gct': " + gctExc.Message, gctExc);
                }
            }
        }

        private void Log(string message)
        {
            if (prog.InvokeRequired)
            {
                prog.BeginInvoke((Action) (() => { prog.AppendLogLine(message); }));
            }
            else
            {
                prog.AppendLogLine(message);
            }
        }

        private void Warn(string message)
        {
            warnings.Add(message);
            if (prog.InvokeRequired)
            {
                prog.BeginInvoke((Action) (() => { prog.AppendLogLine("WARNING: " + message); }));
            }
            else
            {
                prog.AppendLogLine(message);
            }
        }
    }
}
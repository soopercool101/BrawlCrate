using BrawlCrate.BrawlManagers.SongManager.SongExport;
using BrawlCrate.UI;
using BrawlLib.BrawlManagerLib;
using BrawlLib.BrawlManagerLib.GCT.ReadWrite;
using BrawlLib.BrawlManagerLib.Songs;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#if !MONO
using BrawlLib.Internal.Windows.Forms.Ookii.Dialogs;
#endif

namespace BrawlCrate.BrawlManagers.SongManager
{
    public partial class SongManagerForm : Form
    {
        public static readonly string[] GCT_PATHS =
        {
            "RSBE01.gct",
            "/data/gecko/codes/RSBE01.gct",
            "/codes/RSBE01.gct",
            "/LegacyTE/RSBE01.gct",
            "/LegacyXP/RSBE01.gct",
            "../../../../codes/RSBE01.gct",
            "../../../../../../../codes/RSBE01.gct",
            "../../../RSBE01.gct",
            "../../../../RSBE01.gct",
            "../../../../../../RSBE01.gct",
            "../../../../../../../RSBE01.gct"
        };

        /// <summary>
        /// The list of .brstm files in the current directory.
        /// </summary>
        private FileInfo[] brstmFiles;

        /// <summary>
        /// Same as System.Environment.CurrentDirectory.
        /// </summary>
        private string CurrentDirectory
        {
            get => Environment.CurrentDirectory;
            set => Environment.CurrentDirectory = value;
        }

        private string FallbackDirectory;
        private CustomSongVolumeCodeset csv;
        private string csvPath; // used only for save function

        private enum ListType
        {
            FilesInDir,   // default
            GroupByStage, // Brawl
            WithCSV
        }

        private ListType listType;

        /// <summary>
        /// Change the message on the right section of the window.
        /// If the message is not null, the audio player will be hidden.
        /// </summary>
        private string RightControl
        {
            get => songPanel1.Visible ? null : rightLabel.Text;
            set
            {
                songPanel1.Visible = value == null;
                rightLabel.Text = value ?? string.Empty;
            }
        }

        private bool autoplay;
        private bool autoplayNext;

        private const string ChooseLabel = "Choose a stage from the list on the left-hand side.";

        public SongManagerForm(string path, bool loadNames, bool loadBrstms, bool groupSongs)
        {
            if (path == null && !string.IsNullOrEmpty(Properties.Settings.Default.BuildPath))
            {
                Environment.CurrentDirectory = Properties.Settings.Default.BuildPath;
            }

            path = path ?? Environment.CurrentDirectory;

            InitializeComponent();

            loadNamesFromInfopacToolStripMenuItem.Checked = songPanel1.LoadNames = loadNames;
            loadBRSTMPlayerToolStripMenuItem.Checked = songPanel1.LoadBrstms = loadBrstms;
            groupSongsByStageToolStripMenuItem.Checked = groupSongs;
            listType = groupSongs ? ListType.GroupByStage : ListType.FilesInDir;

            Text = "BrawlCrate Song Manager" +
                   Program.AssemblyTitleShort.Substring(
                       Program.AssemblyTitleShort.IndexOf(" ", StringComparison.Ordinal));

            // Later commands to change the titlebar assume there is a hypen in the title somewhere
            Text += " -";

            loadNames = loadNamesFromInfopacToolStripMenuItem.Checked;
            loadBrstms = loadBRSTMPlayerToolStripMenuItem.Checked;
            autoplay = whenSongEndsStartPlayingNextSongToolStripMenuItem.Checked;

            RightControl = ChooseLabel;

            // Drag and drop for the left and right sides of the window. The dragEnter and dragDrop methods will check which panel the file is dropped onto.
            listBox1.AllowDrop = true;
            listBox1.DragEnter += dragEnter;
            listBox1.DragDrop += dragDrop;

            FormClosing += closing;
            FormClosed += closed;

            changeDirectory(path);
        }

        private void open(FileInfo fi)
        {
            if (fi == null)
            {
                // No .brstm file selected (i.e. you just opened the program)
                RightControl = ChooseLabel;
                customSongVolumeEditor1.SongFilename = null;
            }
            else
            {
                fi.Refresh(); // Update file size
                songPanel1.Open(fi, FallbackDirectory);

                string basename = Path.GetFileNameWithoutExtension(fi.FullName);
                customSongVolumeEditor1.SongFilename = basename;

                RightControl = null;
            }

            if (autoplay && autoplayNext)
            {
                if (!BrawlLib.Properties.Settings.Default.AutoPlayAudio)
                {
                    songPanel1.Play();
                }
            }

            autoplayNext = false;

            Refresh();
        }

        private void changeDirectory(string newpath)
        {
            if (newpath == null)
            {
                newpath = CurrentDirectory.LastIndexOf("pf") > 0
                    ? CurrentDirectory.Substring(0, CurrentDirectory.LastIndexOf("pf"))
                    : CurrentDirectory;
            }

            CurrentDirectory = newpath;                                   // Update the program's working directory
            Text = Text.Substring(0, Text.IndexOf('-')) + "- " + newpath; // Update titlebar

            DirectoryInfo dirInfo = new DirectoryInfo(CurrentDirectory);
            RightControl = ChooseLabel;
            brstmFiles = dirInfo.GetFiles("*.brstm");

            // Special code for the root directory of a drive
            if (brstmFiles.Length == 0)
            {
                foreach (string path in new[]
                {
                    "\\private\\wii\\app\\RSBE\\pf\\sound\\strm",
                    "\\projectm\\pf\\sound\\strm"
                })
                {
                    DirectoryInfo search = new DirectoryInfo(dirInfo.FullName + path);
                    if (search.Exists)
                    {
                        changeDirectory(
                            search); // Change to the typical song folder used by the FPC, if it exists on the drive
                        return;
                    }
                }

                string findStrmFolder(string dir)
                {
                    if (dir.EndsWith("\\strm"))
                    {
                        return dir;
                    }

                    try
                    {
                        foreach (string subdir in Directory.EnumerateDirectories(dir))
                        {
                            string possible = findStrmFolder(subdir);
                            if (Directory.Exists(possible))
                            {
                                return possible;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    return null;
                }

                string strmDir = findStrmFolder(CurrentDirectory);
                if (strmDir != null && strmDir != CurrentDirectory)
                {
                    changeDirectory(strmDir);
                    return;
                }
            }

            refreshDirectory();
            findGCT();
        }

        private void changeDirectory(DirectoryInfo path)
        {
            changeDirectory(path.FullName);
        }

        private void findGCT()
        {
            csv = null;
            foreach (string file in GCT_PATHS)
            {
                if (File.Exists(file))
                {
                    csv = new CustomSongVolumeCodeset(File.ReadAllBytes(file));
                    int ct = csv.Settings.Count;
                    Console.WriteLine("Loaded Custom Song Volume (" + ct + " settings)");
                    csvPath = file;
                    break;
                }
            }

            customSongVolumeEditor1.CSV = csv;
            songPanel1.CustomSongTitles = csv?.CNMT?.Map;
        }

        private void refreshDirectory()
        {
            int selected = listBox1.SelectedIndex;

            DirectoryInfo dir = new DirectoryInfo(CurrentDirectory);
            RightControl = ChooseLabel;
            brstmFiles = dir.GetFiles("*.brstm");

            Array.Sort(brstmFiles, delegate(FileInfo f1, FileInfo f2)
            {
                return f1.Name.ToLower().CompareTo(f2.Name.ToLower()); // Sort by filename, case-insensitive
            });

            listBox1.Items.Clear();
            switch (listType)
            {
                case ListType.FilesInDir:
                    listBox1.Items.AddRange(brstmFiles);
                    break;
                case ListType.GroupByStage:
                    List<string> filenamesAdded = new List<string>();
                    listBox1.Items.AddRange(SongsByStage.FromCurrentDir);
                    // make sure we don't add anything twice
                    foreach (object o in SongsByStage.FromCurrentDir)
                    {
                        if (o is SongInfo)
                        {
                            filenamesAdded.Add(((SongInfo) o).File.Name);
                        }
                    }

                    // add remainder
                    foreach (FileInfo f in brstmFiles)
                    {
                        if (!filenamesAdded.Contains(f.Name))
                        {
                            listBox1.Items.Add(new SongInfo(f));
                        }
                    }

                    break;
                case ListType.WithCSV:
                    if (csv != null)
                    {
                        foreach (KeyValuePair<ushort, byte> pair in csv.Settings)
                        {
                            listBox1.Items.Add(new SongInfo(pair.Key, csv));
                        }
                    }

                    break;
            }

            listBox1.Refresh();

            // Re-select and re-load the file that was selected before
            try
            {
                listBox1.SelectedIndex = selected;
            }
            catch (ArgumentOutOfRangeException)
            {
                // This occurs when you delete the last item in the list (and "group songs" is off)
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }

            statusToolStripMenuItem.Text = songPanel1.findInfoFile();
        }

        private void closing(object sender, FormClosingEventArgs e)
        {
            if (songPanel1.IsInfoBarDirty())
            {
                DialogResult res =
                    MessageBox.Show("Save changes to info.pac?", "Closing", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    songPanel1.save();
                }
                else if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (customSongVolumeEditor1.ChangeMadeSinceCSVLoaded)
            {
                DialogResult res = MessageBox.Show("Save changes to " + Path.GetFileName(csvPath) + "?", "Closing",
                    MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    File.WriteAllBytes(csvPath, csv.ExportGCT());
                }
                else if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        public void dragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Must be a file
                string[] s = (string[]) e.Data.GetData(DataFormats.FileDrop);
                if (s.Length == 1)
                {
                    // Can only drag and drop one file
                    string filename = s[0].ToLower();
                    if (filename.EndsWith(".brstm") || filename.EndsWith(".wav"))
                    {
                        if (sender == listBox1 /* || songPanel1.FileOpen*/)
                        {
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                }
            }
        }

        public void dragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[]) e.Data.GetData(DataFormats.FileDrop);
            BeginInvoke(new Action(() =>
            {
                string filepath = s[0].ToLower();
                if (sender == listBox1)
                {
                    using (NameDialog nd = new NameDialog())
                    {
                        nd.EntryText =
                            s[0].Substring(s[0].LastIndexOf('\\') +
                                           1); // Textbox on the dialog ("Text" is already used by C#)
                        if (nd.ShowDialog(this) == DialogResult.OK)
                        {
                            if (!nd.EntryText.ToLower().EndsWith(".brstm"))
                            {
                                nd.EntryText += ".brstm"; // Force .brstm extension so it shows up in the list
                            }

                            if (string.Equals(nd.EntryText, Path.GetFileName(songPanel1.RootPath),
                                StringComparison.InvariantCultureIgnoreCase))
                            {
                                songPanel1.Close(); // in case the file is already open
                            }

                            SongPanel.copyBrstm(filepath, CurrentDirectory + "\\" + nd.EntryText);
                            refreshDirectory();
                        }
                    }
                }
            }));
        }

        /// <summary>
        /// Calls open() on the song selected in listBox1.
        /// </summary>
        private void loadSelectedFile()
        {
            object o = listBox1.SelectedItem;
            if (o is SongInfo)
            {
                SongInfo s = (SongInfo) o;
                s.File.Refresh();
                listBox1.Refresh();
                open(s.File);
            }
            else if (o is FileInfo)
            {
                open((FileInfo) o);
            }
            else if (o is string)
            {
                open(null);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSelectedFile();
        }

        private void changeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !MONO
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
#else
            FolderBrowserDialog fbd = new FolderBrowserDialog();
#endif
            fbd.SelectedPath = CurrentDirectory;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                changeDirectory(fbd.SelectedPath);
            }
        }

        private void openFallbackDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !MONO
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
#else
            FolderBrowserDialog fbd = new FolderBrowserDialog();
#endif
            fbd.SelectedPath = FallbackDirectory;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                FallbackDirectory = fbd.SelectedPath;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm.Instance.ShowDialog(this);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songPanel1.Export();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songPanel1.Rename();
            refreshDirectory();
        }

        private void _deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songPanel1.Delete();
            refreshDirectory();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            listBox1.SelectedIndex = listBox1.IndexFromPoint(listBox1.PointToClient(Cursor.Position));
        }

        #region Options menu actions

        private void loadNamesFromInfopacToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool b = songPanel1.LoadNames = !songPanel1.LoadNames;
            if (sender is ToolStripMenuItem t)
            {
                t.Checked = b;
            }
        }

        private void loadBRSTMPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool b = songPanel1.LoadBrstms = !songPanel1.LoadBrstms;
            if (sender is ToolStripMenuItem t)
            {
                t.Checked = b;
            }
        }

        private void whenSongEndsStartPlayingNextSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool b = autoplay = !autoplay;
            if (sender is ToolStripMenuItem t)
            {
                t.Checked = b;
            }
        }

        private void groupSongsByStageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!groupSongsByStageToolStripMenuItem.Checked)
            {
                groupSongsByStageToolStripMenuItem.Checked = true;
                onlyShowSongsWithCSVCodeToolStripMenuItem.Checked = false;
            }
            else
            {
                groupSongsByStageToolStripMenuItem.Checked = false;
            }

            updateListType();
        }

        private void onlyShowSongsWithCSVCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!onlyShowSongsWithCSVCodeToolStripMenuItem.Checked)
            {
                onlyShowSongsWithCSVCodeToolStripMenuItem.Checked = true;
                groupSongsByStageToolStripMenuItem.Checked = false;
            }
            else
            {
                onlyShowSongsWithCSVCodeToolStripMenuItem.Checked = false;
            }

            updateListType();
        }

        #endregion

        private void updateListType()
        {
            listType = groupSongsByStageToolStripMenuItem.Checked ? ListType.GroupByStage
                : onlyShowSongsWithCSVCodeToolStripMenuItem.Checked ? ListType.WithCSV
                : ListType.FilesInDir;
            refreshDirectory();
        }

        private void saveInfopacToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songPanel1.save();
        }

        private void saveGCTCodesetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Overwrite " + Path.GetFileName(csvPath) + "?", "Overwrite",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                File.WriteAllBytes(csvPath, csv.ExportGCT());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void defaultSongsListToolStripMenuItem_Click(object sender, EventArgs q)
        {
            if (splitContainerTop.Panel2Collapsed)
            {
                ReadOnlySearchableRichTextBox r = new ReadOnlySearchableRichTextBox
                {
                    Dock = DockStyle.Fill,
                    Text = ReadOnlySearchableRichTextBox.HELP + "\n\n" + SongsByStage.DEFAULTS
                };
                splitContainerTop.Panel2.Controls.Add(r);
                splitContainerTop.Panel2Collapsed = false;
            }
            else
            {
                splitContainerTop.Panel2.Controls.Clear();
                splitContainerTop.Panel2Collapsed = true;
            }
        }

        private void SongManagerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageDown)
            {
                e.Handled = true;
                if (listBox1.SelectedIndex == listBox1.Items.Count - 1)
                {
                    listBox1.SelectedIndex = 0;
                }
                else
                {
                    listBox1.SelectedIndex++;
                }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                e.Handled = true;
                if (listBox1.SelectedIndex <= 0)
                {
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
                else
                {
                    listBox1.SelectedIndex--;
                }
            }
        }

        private void updateMumenumainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mu_menumain_path = null;
            string[] lookIn =
            {
                "../../menu2/mu_menumain.pac",
                "../../menu2/mu_menumain_en.pac",
                "../../../pfmenu2/mu_menumain.pac",
                "../../../pfmenu2/mu_menumain_en.pac"
            };
            foreach (string path in lookIn)
            {
                if (File.Exists(path))
                {
                    mu_menumain_path = path;
                    break;
                }
            }

            if (mu_menumain_path == null)
            {
                MessageBox.Show("mu_menumain / mu_menumain_en not found.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (DialogResult.OK == MessageBox.Show("Overwrite the current mu_menumain?", "Overwrite File",
                         MessageBoxButtons.OKCancel))
            {
                string tempfile = Path.GetTempFileName();
                string infotmp = Path.GetTempFileName();
                songPanel1.ExportMSBin(infotmp);
                File.Copy(mu_menumain_path, tempfile, true);

                ResourceNode mu_menumain = NodeFactory.FromFile(null, tempfile);
                MSBinNode m7 = mu_menumain.FindChild("Misc Data [7]", false) as MSBinNode;
                if (m7 == null)
                {
                    MessageBox.Show(ParentForm, "The mu_menumain file does not appear to have a Misc Data [7].",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    m7.Replace(infotmp);
                    mu_menumain.Merge();
                    mu_menumain.Export(mu_menumain_path);
                }

                File.Delete(tempfile);
                File.Delete(infotmp);
            }
        }

        private void customSongVolumeEditor1_ValueChanged(object sender, EventArgs e)
        {
            songPanel1.VolumeByte = customSongVolumeEditor1.Value;
        }

        private void closed(object sender, FormClosedEventArgs e)
        {
            TempFiles.DeleteAll();
        }

        private void CloseCurrentResources()
        {
            songPanel1.Close();
            listBox1.SelectedIndex = -1;
        }

        private void exportMusicSongsToolStripMenuItem_Click(object sender, EventArgs eva)
        {
            CloseCurrentResources();
#if !MONO
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
#else
            FolderBrowserDialog fbd = new FolderBrowserDialog();
#endif
            fbd.Description = "Select folder to export to:";
            fbd.SelectedPath = CurrentDirectory;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SongExporter exporter = new SongExporter();
                    //exporter.ExportStageDirs = groupSongsByStageToolStripMenuItem.Checked;
                    exporter.ExportStageDirs = true;
                    exporter.Export(fbd.SelectedPath);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Error exporting: " + exc);
                }
            }
        }

        private void songPanel1_AudioEnded(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < listBox1.Items.Count - 1 && autoplay)
            {
                autoplayNext = true;
                listBox1.SelectedIndex++;
            }
        }

        private void customSongVolumeStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CustomSongVolumeStatusForm f = new CustomSongVolumeStatusForm(customSongVolumeEditor1))
            {
                f.ShowDialog(this);
            }
        }

        private void importMusicSongsToolStripMenuItem_Click(object sender, EventArgs eva)
        {
            CloseCurrentResources();
#if !MONO
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
#else
            FolderBrowserDialog fbd = new FolderBrowserDialog();
#endif
            fbd.Description = "Select folder to import from:";
            fbd.SelectedPath = CurrentDirectory;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SongImporter importer = new SongImporter();
                    importer.Import(fbd.SelectedPath);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Error importing: " + exc);
                }
            }

            changeDirectory(CurrentDirectory);
        }

        private void use16ptFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float? size = use16ptFontToolStripMenuItem.Checked
                ? 16f
                : (float?) null;

            Font = new Font(Font.FontFamily, size ?? 8.25f, Font.Style);
            menuStrip1.Font = new Font(menuStrip1.Font.FontFamily, size ?? 9f, menuStrip1.Font.Style);
        }
    }
}
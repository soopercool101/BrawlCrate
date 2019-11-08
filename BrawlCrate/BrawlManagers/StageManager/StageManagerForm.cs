using BrawlCrate.BrawlManagers.StageManager.SingleUseDialogs;
using BrawlCrate.UI;
using BrawlLib.BrawlManagerLib;
using BrawlLib.BrawlManagerLib.GCT;
using BrawlLib.BrawlManagerLib.Songs;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

#if !MONO
using BrawlLib.Internal.Windows.Forms.Ookii.Dialogs;
#endif

namespace BrawlCrate.BrawlManagers.StageManager
{
    public partial class StageManagerForm : Form
    {
        private static OpenFileDialog OpenDialog = new OpenFileDialog();
        private static SaveFileDialog SaveDialog = new SaveFileDialog();
#if !MONO
        private static VistaFolderBrowserDialog FolderDialog =
            new VistaFolderBrowserDialog();
#else
        private static FolderBrowserDialog FolderDialog = new FolderBrowserDialog();
#endif

        #region Folder-scope variables

        /// <summary>
        /// The list of .pac files in the current directory.
        /// </summary>
        private FileInfo[] pacFiles;

        /// <summary>
        /// Same as System.Environment.CurrentDirectory.
        /// </summary>
        private string CurrentDirectory
        {
            get => Environment.CurrentDirectory;
            set => Environment.CurrentDirectory = value;
        }

        /// <summary>
        /// Location of the folder with .rel files, relative to the current directory.
        /// </summary>
        private string moduleFolderLocation;

        #endregion

        #region Stage-scope variables

        /// <summary>
        /// The currently opened .pac file's root node.
        /// </summary>
        private ResourceNode _rootNode;

        /// <summary>
        /// The full path to the currently opened .pac file.
        /// </summary>
        private string _rootPath;

        private List<MDL0TextureNode> texNodes;

        #endregion

        #region Window-scope variables

        /// <summary>
        /// Labels for the MSBin viewer area.
        /// </summary>
        private Label noMSBinLabel, couldNotOpenLabel;

        /// <summary>
        /// Change the control used on the upper-middle section of the window (either a label or an MSBinViewer.)
        /// Any existing controls in that panel will be removed, and the new control's Dock property will be set to Fill.
        /// </summary>
        private Control RightControl
        {
            get
            {
                Control.ControlCollection controls = msBinPanel.Controls;
                if (controls.Count > 0)
                {
                    return controls[0];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Control.ControlCollection controls = msBinPanel.Controls;
                controls.Clear();
                if (value != null)
                {
                    value.Dock = DockStyle.Fill;
                }

                if (value != null)
                {
                    controls.Add(value);
                }
            }
        }

        #endregion

        public StageManagerForm(string path, bool useRelDescription)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.BuildPath))
            {
                Environment.CurrentDirectory = Properties.Settings.Default.BuildPath;
            }

            InitializeComponent();

            try
            {
                path = path ?? Environment.CurrentDirectory;
            }
            catch (Exception e)
            {
                TextBoxDialog.ShowDialog("Exception caught: " + e.Message + "\n" + e.StackTrace,
                    e.GetType().ToString());
                path = path ?? Directory.GetCurrentDirectory();
            }

            Text = "BrawlCrate Stage Manager" +
                   Program.AssemblyTitleShort.Substring(
                       Program.AssemblyTitleShort.IndexOf(" ", StringComparison.Ordinal));

            moduleFolderLocation = "../../module";

            // Later commands to change the titlebar assume there is a hyphen in the title somewhere
            Text += " -";

            #region labels

            noMSBinLabel = new Label();
            noMSBinLabel.Name = "noTextLabel";
            noMSBinLabel.Text = "There are no MSBin nodes in this stage.";
            noMSBinLabel.TextAlign = ContentAlignment.MiddleCenter;

            couldNotOpenLabel = new Label();
            couldNotOpenLabel.Name = "couldNotOpenLabel";
            couldNotOpenLabel.Text = "Could not open the .PAC file.";
            couldNotOpenLabel.TextAlign = ContentAlignment.MiddleCenter;

            #endregion

            RightControl = null;

            // The defaults for these options depend on the defaults of the menu items that control them
            stageInfoControl1.UseRelDescription = useFullrelNamesToolStripMenuItem.Checked = useRelDescription;

            // Drag and drop for the left and right sides of the window. The dragEnter and dragDrop methods will check which panel the file is dropped onto.
            panel2.AllowDrop = true;
            panel2.DragEnter += new DragEventHandler(dragEnter);
            panel2.DragDrop += new DragEventHandler(dragDrop);
            listBox1.AllowDrop = true;
            listBox1.DragEnter += new DragEventHandler(dragEnter);
            listBox1.DragDrop += new DragEventHandler(dragDrop);

            foreach (object item in selmapMarkFormat.DropDownItems)
            {
                ((ToolStripMenuItem) item).Click += new EventHandler(switchSelmapMarkFormat);
            }

            foreach (object item in prevbaseSize.DropDownItems)
            {
                ((ToolStripMenuItem) item).Click += new EventHandler(switchPrevbaseSize);
            }

            foreach (object item in frontstnameSizeToolStripMenuItem.DropDownItems)
            {
                ((ToolStripMenuItem) item).Click += new EventHandler(switchFrontstnameSize);
            }

            foreach (object item in selmapMarkSizeToolStripMenuItem.DropDownItems)
            {
                ((ToolStripMenuItem) item).Click += new EventHandler(switchSelmapMarkSize);
            }

            fileToolStripMenuItem.DropDownOpening += (o, e) =>
            {
                saveCommon5scselmapToolStripMenuItem.Enabled = portraitViewer1.IsDirty;
                saveInfopacToolStripMenuItem.Enabled = songPanel1.IsInfoBarDirty();
                MoveToolStripItems(stageContextMenu.Items, currentStageToolStripMenuItem.DropDownItems);
                MoveToolStripItems(songContextMenu.Items, currentSongToolStripMenuItem.DropDownItems);
            };

            FormClosing += StageManagerForm_FormClosing;
            FormClosed += StageManagerForm_FormClosed;

            clbTextures.ItemCheck += (o, e) =>
            {
                texNodes[e.Index].Enabled = e.NewValue == CheckState.Checked;
                modelPanel1.Invalidate();
            };

            portraitViewer1.selmapMarkPreview = selmapMarkPreviewToolStripMenuItem.Checked;
            portraitViewer1.useTextureConverter = useTextureConverterToolStripMenuItem.Checked;
            //LoadFromRegistry();
            changeDirectory(path);
        }

        private static void MoveToolStripItems(ToolStripItemCollection from, ToolStripItemCollection to)
        {
            ToolStripItem[] arr = new ToolStripItem[from.Count];
            from.CopyTo(arr, 0);
            to.AddRange(arr);
        }

        /// <summary>
        /// If the common5/sc_selmap or info.pac is dirty, asks the user whether they want to save them.
        /// </summary>
        /// <returns>true if the files did not need to be saved OR the user saved them; false otherwise.</returns>
        private bool savePacsIfNecessary(string verb = "closing")
        {
            bool s = portraitViewer1.IsDirty;
            bool i = songPanel1.IsInfoBarDirty();
            if (s || i)
            {
                DialogResult result =
                    MessageBox.Show("Would you like to save common5/sc_selmap and info.pac before " + verb + "?", Text,
                        MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
                else if (result == DialogResult.Yes)
                {
                    songPanel1.save();
                    portraitViewer1.save();
                    return true;
                }
                else if (result == DialogResult.No)
                {
                    return true;
                }
            }

            // not dirty
            return true;
        }

        #region Opening files

        private void open(FileInfo fi)
        {
            if (_rootNode != null)
            {
                _rootNode.Dispose();
                _rootNode = null;
                _rootPath = null;
            }

            if (fi == null)
            {
                // No .pac file selected (i.e. you just opened the program)
                RightControl = null;
                return;
            }

            _rootPath = fi.FullName;
            if (renderModels.Checked)
            {
                modelPanel1.ClearAll();
            }

            string relname =
                StageIDMap.RelNameForPac(fi.Name, differentrelsForAlternateStagesPM36ToolStripMenuItem.Checked);
            updateRel(relname);

            try
            {
                fi.Refresh(); // Update file size

                MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
                byte[] hash = md5provider.ComputeHash(File.ReadAllBytes(fi.FullName));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2").ToLower());
                }

                stageInfoControl1.MD5 = sb.ToString();
                _rootNode = loadStagepacsToolStripMenuItem.Checked ? NodeFactory.FromFile(null, _rootPath) : null;
            }
            catch (FileNotFoundException)
            {
                // This might happen if you delete the file from Explorer after this program puts it in the list
                RightControl = couldNotOpenLabel;
            }

            if (_rootNode != null)
            {
                // Set the stage info labels. Equivalent labels for the .rel file are set when RelFile is changed in StageInfoControl
                stageInfoControl1.setStageLabels(fi.Name + ":", _rootNode.Name, "(" + fi.Length + " bytes)");

                #region Scan for 3D models and MSBin text nodes

                List<ResourceNode> allNodes = FindStageARC(_rootNode).Children; // Find all child nodes of "2"
                List<MSBinNode> msBinNodes = new List<MSBinNode>();
                texNodes = new List<MDL0TextureNode>();
                foreach (ResourceNode node in allNodes)
                {
                    if (node.ResourceFileType == ResourceType.MSBin)
                    {
                        msBinNodes.Add((MSBinNode) node); // This is an MSBin node - add it to the list
                    }
                    else if (renderModels.Checked)
                    {
                        ResourceNode modelfolder = node.FindChild("3DModels(NW4R)", false);
                        if (modelfolder != null && _rootNode.Name != "STGBATTLEFIELDII")
                        {
                            foreach (ResourceNode child in modelfolder.Children)
                            {
                                if (child is MDL0Node && !child.Name.StartsWith("MShadow"))
                                {
                                    try
                                    {
                                        MDL0Node model = child as MDL0Node;
                                        model.Populate();
                                        model.ResetToBindState();
                                        if (model.TextureGroup != null)
                                        {
                                            foreach (ResourceNode tex in model.TextureGroup.Children)
                                            {
                                                if (tex is MDL0TextureNode)
                                                {
                                                    texNodes.Add((MDL0TextureNode) tex);
                                                }
                                            }
                                        }

                                        modelPanel1.AddTarget(model);
                                    }
                                    catch (InvalidOperationException e)
                                    {
                                        Console.Error.WriteLine(child.Name + ": " + e.Message);
                                    }
                                }
                            }
                        }
                    }
                }

                if (renderModels.Checked)
                {
                    modelPanel1.SetCamWithBox(new Vector3(-100, -100, -100), new Vector3(100, 100, 100));

                    // Update textures list
                    CheckedListBox.ObjectCollection items = clbTextures.Items;
                    items.Clear();
                    foreach (MDL0TextureNode tex in texNodes)
                    {
                        items.Add(tex, true);
                    }
                }

                if (msBinNodes.Count > 0)
                {
                    ListControl
                        list = new ListControl(msBinNodes); // Have ListControl manage these; make that the right panel
                    RightControl = list;
                }
                else
                {
                    RightControl = noMSBinLabel;
                }

                #endregion
            }

            int stage_id = StageIDMap.StageIDForPac(fi.Name);
            portraitViewer1.UpdateImage(portraitViewer1.BestSSS.IconForStage(stage_id));

            #region finding .brstm

            listBoxSongs.Items.Clear();
            if (!loadbrstmsToolStripMenuItem.Checked)
            {
                songPanel1.Close();
            }
            else
            {
                Song song = portraitViewer1.BestSSS.SongLoaders.GetSong(stage_id);
                if (song != null)
                {
                    songContainerPanel.Visible = true;
                    string dir = song.Filename.StartsWith("0000") && cse2xToolStripMenuItem.Checked
                        ? "../../sound/sfx/"
                        : "../../sound/strm/";
                    listBoxSongs.Items.Add(new SongListItem(dir + song.Filename + ".brstm"));
                    listBoxSongs.SelectedIndex = 0;
                }
                else
                {
                    string[] arr = SongsByStageID.ForPac(portraitViewer1.BestSSS, fi.Name);
                    arr = arr.Select<string, string>(filename =>
                    {
                        Song element = SongIDMap.Songs.SingleOrDefault(s => s.Filename == filename);
                        if (element != null)
                        {
                            return portraitViewer1.BestSSS.GetSong(fi.Name, element).Filename;
                        }
                        else
                        {
                            return filename;
                        }
                    }).ToArray();
                    if (arr != null)
                    {
                        songContainerPanel.Visible = true;
                        listBoxSongs.Items.AddRange(arr);
                        if (listBoxSongs.Items.Count > 0)
                        {
                            listBoxSongs.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        songContainerPanel.Visible = false;
                        songPanel1.Close();
                    }
                }
            }

            if (listBoxSongs.Items.Count > 0)
            {
                listBoxSongs.Visible = true;
                listBoxSongs.SelectedIndex = 0;
            }
            else
            {
                listBoxSongs.Visible = false;
            }

            #endregion

            Refresh();
        }

        /// <summary>
        /// Adds the path to a .rel filename and updates RelFile in StageInfoControl.
        /// </summary>
        /// <param name="relname">Filename (not path) of the .rel file</param>
        private void updateRel(string relname)
        {
            string path = Path.GetFullPath(moduleFolderLocation + "\\" + relname);
            stageInfoControl1.RelFile = new FileInfo(path);
        }

        #endregion

        #region Changing directory

        private void ReloadDirectory()
        {
            if (savePacsIfNecessary("reloading the list"))
            {
                changeDirectory(CurrentDirectory); // Refresh .pac list
            }
        }

        private void changeDirectory(string newpath)
        {
            changeDirectory(new DirectoryInfo(newpath));
        }

        private void changeDirectory(DirectoryInfo path)
        {
            CurrentDirectory =
                path.FullName;                                                  // Update the program's working directory
            Text = Text.Substring(0, Text.IndexOf('-')) + "- " + path.FullName; // Update titlebar

            RightControl = null;

            pacFiles = path.GetFiles("*.pac");

            // Special code for the root directory of a drive
            if (pacFiles.Length == 0)
            {
                foreach (string subpath in new[]
                {
                    "\\private\\wii\\app\\RSBE\\pf\\stage\\melee",
                    "\\projectm\\pf\\stage\\melee"
                })
                {
                    DirectoryInfo search = new DirectoryInfo(path.FullName + subpath);
                    if (search.Exists)
                    {
                        changeDirectory(
                            search); // Change to the typical stage folder used by the FPC, if it exists on the drive
                        return;
                    }
                }

                string findMeleeFolder(string dir)
                {
                    if (dir.EndsWith("\\melee"))
                    {
                        return dir;
                    }

                    try
                    {
                        foreach (string subdir in Directory.EnumerateDirectories(dir))
                        {
                            string possible = findMeleeFolder(subdir);
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

                string meleeDir = findMeleeFolder(CurrentDirectory);
                if (meleeDir != null && meleeDir != CurrentDirectory)
                {
                    changeDirectory(meleeDir);
                    return;
                }
            }

            stageInfoControl1.setStageLabels("", "", "");
            stageInfoControl1.RelFile = null;

            Console.WriteLine((string) songPanel1.findInfoFile());
            songPanel1.CustomSongTitles = portraitViewer1.BestSSS.CNMT.Map;

            portraitViewer1.UpdateDirectory();

            if (useAFixedStageListToolStripMenuItem.Checked)
            {
                List<string> pacNames = StageIDMap.PacFilesBySSSOrder(portraitViewer1.BestSSS);
                for (int i = 0; i < pacNames.Count; i++)
                {
                    string name = pacNames[i];
                    AlternateStageEntry def;
                    if (portraitViewer1.BestSSS.AlternateStageLoaderData.TryGetDefinition(name, out def))
                    {
                        string without_ext = name.Substring(0, name.Length - 4);
                        foreach (char letter in def.ButtonActivated.Concat(def.Random).Select(a => a.Letter).Distinct())
                        {
                            pacNames.Insert(++i, without_ext + "_" + letter + ".pac");
                        }
                    }
                }

                pacFiles = pacNames.Select(s => new FileInfo(s)).ToArray();
            }
            else
            {
                Array.Sort(pacFiles, delegate(FileInfo f1, FileInfo f2)
                {
                    return f1.Name.ToLower().CompareTo(f2.Name.ToLower()); // Sort by filename, case-insensitive
                });
            }

            listBox1.Items.Clear();
            listBox1.Items.AddRange(pacFiles);
            listBox1.Refresh();

            if (portraitViewer1.BestSSS.OtherCodesIgnoredInSameFile > 0)
            {
                MessageBox.Show(this,
                    "More than one Custom SSS code found in the codeset. All but the last one will be ignored.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Replacing & exporting files

        #region drag-and-drop

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
                    if (sender == listBox1)
                    {
                        // The sender is on the left - add a stage
                        if (filename.EndsWith(".pac"))
                        {
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                    else if (_rootPath != null)
                    {
                        // The sender must be on the right - modify an existing stage/module; ignore if no stage is selected
                        if (filename.EndsWith(".pac") || filename.EndsWith(".rel") || Directory.Exists(filename))
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
                string name;
                if (sender == listBox1)
                {
                    NameDialog nd = new NameDialog();
                    nd.Text = "Enter Filename"; // Titlebar
                    nd.EntryText =
                        s[0].Substring(s[0].LastIndexOf('\\') +
                                       1); // Textbox on the dialog ("Text" is already used by C#)
                    nd.LabelText = "Enter the filename to copy to (with or without the .pac extension):";
                    if (nd.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!nd.EntryText.ToLower().EndsWith(".pac"))
                        {
                            nd.EntryText += ".pac"; // Force .pac extension so it shows up in the list
                        }

                        if (FileOperations.Copy(filepath, CurrentDirectory + "\\" + nd.EntryText))
                        {
                            // Use FileOperations (calls Windows shell -> asks for confirmation to overwrite)
                            MessageBox.Show(this, "File copied.");
                            ReloadDirectory();
                        }
                    }
                }
                else if (_rootPath != null)
                {
                    name = new FileInfo(_rootPath).Name;
                    if (_rootNode != null)
                    {
                        _rootNode.Dispose();
                        _rootNode = null; // Close the file before overwriting it!
                    }

                    if (filepath.EndsWith(".pac"))
                    {
                        FileOperations.Copy(filepath, CurrentDirectory + "\\" + name);
                    }
                    else if (filepath.EndsWith(".rel"))
                    {
                        FileOperations.Copy(filepath, stageInfoControl1.RelFile.FullName);
                    }
                    else if (Directory.Exists(filepath))
                    {
                        importDir(filepath);
                    }

                    listBox1_SelectedIndexChanged(null, null);
                }
            }));
        }

        #endregion

        private void importDir(string dirpath)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(dirpath);

            IEnumerable<FileInfo> pacfiles = dirinfo.EnumerateFiles("*.pac");
            IEnumerable<FileInfo> prevbases = dirinfo.EnumerateFiles("*Prevbase.*");
            IEnumerable<FileInfo> icons = dirinfo.EnumerateFiles("*Icon.*").Where(f => !f.Name.Contains("SeriesIcon"));
            IEnumerable<FileInfo> frontstnames = dirinfo.EnumerateFiles("*FrontStname.*");
            IEnumerable<FileInfo> seriesicons =
                dirinfo.EnumerateFiles("*SelchrMark.*").Concat(dirinfo.EnumerateFiles("*SeriesIcon.*"));
            IEnumerable<FileInfo> selmap_marks = dirinfo.EnumerateFiles("*SelmapMark.*");
            IEnumerable<FileInfo> brstms = dirinfo.EnumerateFiles("*.brstm");
            bool any = pacfiles.Any() || prevbases.Any() || icons.Any() || frontstnames.Any() || seriesicons.Any() ||
                       selmap_marks.Any();
            if (!any)
            {
                MessageBox.Show("No .pac files or images found in this folder.");
                return;
            }

            if (pacfiles.Any())
            {
                FileInfo pac = pacfiles.First();

                IEnumerable<FileInfo> relfiles = dirinfo.EnumerateFiles("*.rel");
                string rel = relfiles.Any() ? relfiles.First().FullName : "No .rel file found";

                DialogResult r = new CopyPacRelDialog(pac.FullName, _rootPath, rel, stageInfoControl1.RelFile.FullName)
                    .ShowDialog();
                if (r == DialogResult.Cancel)
                {
                    return;
                }

                if (r == DialogResult.Yes)
                {
                    File.Copy(pac.FullName, _rootPath, true);
                    if (File.Exists(rel))
                    {
                        File.Copy(rel, stageInfoControl1.RelFile.FullName, true);
                    }
                }
            }

            if (portraitViewer1.SelmapLoaded)
            {
                if (prevbases.Any())
                {
                    portraitViewer1.Replace(portraitViewer1.prevbase, prevbases.First().FullName);
                }

                if (icons.Any())
                {
                    portraitViewer1.Replace(portraitViewer1.icon, icons.First().FullName);
                }

                if (frontstnames.Any())
                {
                    portraitViewer1.Replace(portraitViewer1.frontstname, frontstnames.First().FullName);
                }

                if (seriesicons.Any())
                {
                    portraitViewer1.Replace(portraitViewer1.seriesicon, seriesicons.First().FullName);
                }

                if (selmap_marks.Any())
                {
                    portraitViewer1.Replace(portraitViewer1.selmap_mark, selmap_marks.First().FullName);
                }

                if (brstms.Any())
                {
                    songPanel1.Replace(brstms.First().FullName);
                }
            }
        }

        private void exportStage(FileInfo f, string thisdir)
        {
            Directory.CreateDirectory(thisdir);
            IEnumerable<string> pacs = Directory.EnumerateFiles(thisdir, "*.pac");
            if (pacs.Any())
            {
                DialogResult result = MessageBox.Show(this, "This directory already contains a .PAC file. " +
                                                            "Is it okay to remove it and the other stage files in this folder? " +
                                                            "(If the recycle bin is enabled, the files will be sent there.)",
                    "Overwrite", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    foreach (string file in pacs)
                    {
                        // Send pac files to recycle bin (if bin is enabled for this drive)
                        FileOperations.Delete(file, FileOperations.FileOperationFlags.FOF_NOCONFIRMATION);
                    }

                    // Also recycle other files
                    string[] toRecycle =
                    {
                        "st*.rel", "*Prevbase.*", "*Icon.*", "*FrontStname.*",
                        "*SeriesIcon.*", "*SelchrMark.*", "*SelmapMark.*"
                    };
                    foreach (string filename in toRecycle)
                    {
                        FileOperations.Delete(thisdir + "/" + filename,
                            FileOperations.FileOperationFlags.FOF_NOCONFIRMATION);
                    }
                }
                else
                {
                    return;
                }
            }

            try
            {
                string p = readNameFromPac(f);
                FileOperations.Copy(f.FullName, thisdir + "/" + p);
            }
            catch (FileNotFoundException)
            {
            }

            string relname =
                StageIDMap.RelNameForPac(f.Name, differentrelsForAlternateStagesPM36ToolStripMenuItem.Checked);
            FileInfo rel = new FileInfo("../../module/" + relname);
            if (rel.Exists)
            {
                FileOperations.Copy(rel.FullName, thisdir + "/st.rel");
            }

            portraitViewer1.ExportImages(portraitViewer1.BestSSS.IconForStage(StageIDMap.StageIDForPac(f.Name)),
                thisdir);

            Song song = portraitViewer1.BestSSS.SongLoaders.GetSong(StageIDMap.StageIDForPac(f.Name));
            if (song == null)
            {
                SongsByStageID.ForPac(portraitViewer1.BestSSS, f.Name);
            }

            foreach (string dir in new[] {"../../sound/strm/", "../../sound/sfx/"})
            {
                if (song != null && File.Exists(dir + song.Filename + ".brstm"))
                {
                    File.Copy(dir + song.Filename + ".brstm", thisdir + "/song.brstm", true);
                    break;
                }
            }
        }

        #endregion

        #region Reading .pac files

        private static string readNameFromPac(FileInfo f)
        {
            StringBuilder sb = new StringBuilder();
            using (FileStream stream = new FileStream(f.FullName, FileMode.Open, FileAccess.Read))
            {
                stream.Seek(16, SeekOrigin.Begin);
                int b = stream.ReadByte();
                while (b == 0)
                {
                    b = stream.ReadByte();
                }

                while (b != 0)
                {
                    sb.Append((char) b);
                    b = stream.ReadByte();
                }
            }

            if (sb.ToString().IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                return f.Name;
            }

            return sb + ".pac";
        }

        private static ResourceNode FindStageARC(ResourceNode node)
        {
            foreach (ResourceNode child in node.Children)
            {
                foreach (ResourceNode child2 in child.Children)
                {
                    if (child2.GetType() == typeof(CollisionNode))
                    {
                        return child;
                    }
                }
            }

            // fallback
            return node.FindChild("2", false);
        }

        #endregion

        #region event handlers

        private void exportpacrelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FolderDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string outdir = FolderDialog.SelectedPath;
            exportStage(listBox1.SelectedItem as FileInfo, outdir);
        }

        private void deletepacrelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _rootNode.Dispose();
            _rootNode = null;
            if (stageInfoControl1.RelFile != null)
            {
                FileOperations.Delete(stageInfoControl1.RelFile.FullName);
            }

            if (listBox1.SelectedItem as FileInfo != null)
            {
                FileOperations.Delete(((FileInfo) listBox1.SelectedItem).FullName);
            }

            ReloadDirectory();
        }

        private void exportbrstmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songPanel1.Export();
        }

        private void deletebrstmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songPanel1.Delete();
        }

        private void saveCommon5scselmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portraitViewer1.save();
        }

        private void saveInfopacToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songPanel1.save();
        }

        private void exportAllMiscData80ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                portraitViewer1.exportAll(FolderDialog.SelectedPath);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            open((FileInfo) listBox1.SelectedItem);
            if (listBox1.SelectedItem == null)
            {
                brawlCrateStageToolStripMenuItem.Text = "No stage loaded";
            }
            else
            {
                brawlCrateStageToolStripMenuItem.Text = listBox1.SelectedItem.ToString();
            }
        }

        private void stageContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listBox1.SelectedIndex = listBox1.IndexFromPoint(listBox1.PointToClient(Cursor.Position));
            MoveToolStripItems(currentStageToolStripMenuItem.DropDownItems, stageContextMenu.Items);
            e.Cancel = false;
        }

        private void songContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listBoxSongs.SelectedIndex = listBoxSongs.IndexFromPoint(listBoxSongs.PointToClient(Cursor.Position));
            MoveToolStripItems(currentSongToolStripMenuItem.DropDownItems, songContextMenu.Items);
            e.Cancel = false;
        }

        private void changeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderDialog.SelectedPath =
                CurrentDirectory; // Uncomment this if you want the "change directory" dialog to start with the current directory selected
            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                changeDirectory(FolderDialog.SelectedPath);
            }
        }

        private void loadCustomSSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDialog.Filter = "Codeset files (*.txt, *.gct)|*.txt;*.gct";
            OpenDialog.Multiselect = true;
            OpenDialog.InitialDirectory = CurrentDirectory;
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                portraitViewer1.LoadCustomSSS(OpenDialog.FileName);
                ReloadDirectory();
            }
        }

        private void exportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FolderDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string outdir = FolderDialog.SelectedPath;
            if (Directory.Exists(outdir) && Directory.EnumerateFileSystemEntries(outdir).Any())
            {
                DialogResult dr = MessageBox.Show("Is it OK to delete everything in " + outdir + "?", "",
                    MessageBoxButtons.OKCancel);
                if (dr != DialogResult.OK)
                {
                    return;
                }

                Directory.Delete(outdir, true);
            }

            using (ProgressWindow progress = new ProgressWindow((Control) null, "Exporting...", "", true))
            {
                progress.Begin(0, listBox1.Items.Count, 0);
                Directory.CreateDirectory(outdir);
                int i = 0;
                foreach (FileInfo f in listBox1.Items)
                {
                    if (progress.Cancelled)
                    {
                        break;
                    }

                    progress.Update(++i);
                    string thisdir = outdir + "\\" + f.Name.Substring(0, f.Name.LastIndexOf('.'));
                    Directory.CreateDirectory(thisdir);
                    exportStage(f, thisdir);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void useTextureConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portraitViewer1.useTextureConverter = useTextureConverterToolStripMenuItem.Checked;
        }

        private void useAFixedStageListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadDirectory();
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = portraitViewer1.BackColor;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    portraitViewer1.BackColor = cd.Color;
                }
            }
        }

        private void sameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moduleFolderLocation = ".";
            sameToolStripMenuItem.Checked = true;
            moduleToolStripMenuItem.Checked = false;
            if (stageInfoControl1.RelFile != null)
            {
                updateRel(stageInfoControl1.RelFile.Name); // Refresh the .rel
            }
        }

        private void moduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moduleFolderLocation = "../../module";
            sameToolStripMenuItem.Checked = false;
            moduleToolStripMenuItem.Checked = true;
            if (stageInfoControl1.RelFile != null)
            {
                updateRel(stageInfoControl1.RelFile.Name); // Refresh the .rel
            }
        }

        private void useFullrelNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stageInfoControl1.UseRelDescription = useFullrelNamesToolStripMenuItem.Checked;
        }

        private void differentrelsForAlternateStagesPM36ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fi = (FileInfo) listBox1.SelectedItem;
            if (fi != null)
            {
                string relname = StageIDMap.RelNameForPac(fi.Name,
                    differentrelsForAlternateStagesPM36ToolStripMenuItem.Checked);
                updateRel(relname);
            }
        }

        private void selmapMarkPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portraitViewer1.selmapMarkPreview = selmapMarkPreviewToolStripMenuItem.Checked;
            portraitViewer1.UpdateImage();
        }

        private void switchSelmapMarkFormat(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in selmapMarkFormat.DropDownItems)
            {
                item.Checked = item == sender;
            }

            if (sender == selmapMarkFormatIA4)
            {
                portraitViewer1.selmapMarkFormat = WiiPixelFormat.IA4;
            }
            else if (sender == selmapMarkFormatI4)
            {
                portraitViewer1.selmapMarkFormat = WiiPixelFormat.I4;
            }
            else if (sender == selmapMarkFormatAuto)
            {
                portraitViewer1.selmapMarkFormat = null;
                portraitViewer1.useExistingAsFallback = false;
            }
            else if (sender == selmapMarkFormatCMPR)
            {
                portraitViewer1.selmapMarkFormat = WiiPixelFormat.CMPR;
            }
            else if (sender == selmapMarkFormatExisting)
            {
                portraitViewer1.selmapMarkFormat = null;
                portraitViewer1.useExistingAsFallback = true;
            }
        }

        private void frontStnameGenerationFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portraitViewer1.changeFrontStnameFont();
        }

        private void snapshotPortraiticonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = modelPanel1.GetScreenshot(modelPanel1.ClientRectangle, false);

            int size = Math.Min(screenshot.Width, screenshot.Height);
            Bitmap square = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(square))
            {
                g.DrawImage(screenshot,
                    (screenshot.Width - size) / -2,
                    (screenshot.Height - size) / -2);
            }

            string iconFile = TempFiles.Create(".png");
            BitmapUtilities.Resize(square, new Size(64, 56)).Save(iconFile);
            portraitViewer1.Replace(portraitViewer1.icon, iconFile);
            string prevbaseFile = TempFiles.Create(".png");
            BitmapUtilities.Resize(square, portraitViewer1.prevbaseResizeTo ?? new Size(176, 176)).Save(prevbaseFile);
            portraitViewer1.Replace(portraitViewer1.prevbase, prevbaseFile);
        }

        private void repaintIconBorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portraitViewer1.repaintIconBorder();
        }

        private void updateMumenumainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (songPanel1.IsInfoBarDirty())
            {
                DialogResult dr = MessageBox.Show(this,
                    "This will copy the song titles that are currently entered, including those not saved to info.pac yet. Is this OK?",
                    "Note", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.No)
                {
                    return;
                }
            }

            if (songPanel1.InfoLoaded)
            {
                string msbintmp = TempFiles.Create(".msbin");
                songPanel1.ExportMSBin(msbintmp);
                portraitViewer1.updateMuMenumain(msbintmp);
            }
            else
            {
                portraitViewer1.updateMuMenumain();
            }
        }

        private void updateScselcharacter2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portraitViewer1.copyIconsToSelcharacter2();
        }

        private void addMenSelmapMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDialog.Filter = FileFilters.TEX0;
            OpenDialog.Multiselect = true;
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show("Ask for a name for each texture?", Text,
                    MessageBoxButtons.YesNoCancel);
                if (result != DialogResult.Cancel)
                {
                    bool ask = DialogResult.Yes == result;
                    foreach (string file in OpenDialog.FileNames)
                    {
                        bool r = portraitViewer1.AddMenSelmapMark(file, ask);
                        if (!r)
                        {
                            break;
                        }
                    }
                }
            }

            OpenDialog.Multiselect = false;
        }

        private void listMenSelmapMarkUsageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxDialog.ShowDialog(portraitViewer1.MenSelmapMarkUsageReport());
        }

        private void addmissingPAT0EntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portraitViewer1.UpdateImage(portraitViewer1.BestSSS.IconForStage(1));

            BatchAddPAT0Dialog dialog = new BatchAddPAT0Dialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                //TODO ask if they want different MenSelchrMark or MenSelmapMark for each stage
                portraitViewer1.AddPAT0("Misc Data [80]/AnmTexPat(NW4R)/MenSelmapPreview/pasted__stnameshadowM",
                    dialog.AddNewTextures);
                portraitViewer1.AddPAT0("Misc Data [80]/AnmTexPat(NW4R)/MenSelmapPreview/pasted__stnamelogoM",
                    dialog.AddNewTextures,
                    dialog.UseSameSelmapMarksForAll ? "MenSelmapMark.01" : null);
                portraitViewer1.AddPAT0("Misc Data [80]/AnmTexPat(NW4R)/MenSelmapPreview/pasted__stnameM",
                    dialog.AddNewTextures);
                portraitViewer1.AddPAT0("Misc Data [80]/AnmTexPat(NW4R)/MenSelmapPreview/lambert113",
                    dialog.AddNewTextures,
                    dialog.UseSameSelchrMarksForAll ? "MenSelchrMark.20" : null);
                portraitViewer1.AddPAT0("Misc Data [80]/AnmTexPat(NW4R)/MenSelmapPreview/basebgM",
                    dialog.AddNewTextures);
                portraitViewer1.AddPAT0("Misc Data [80]/AnmTexPat(NW4R)/MenSelmapIcon/iconM", dialog.AddNewTextures);
                MessageBox.Show(
                    "Save the common5/sc_selmap file and restart the program for the changes to take effect.");
            }
        }

        private void resizeAllPrevbasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnterSizeDialog dialog = new EnterSizeDialog
            {
                SizeEntry = portraitViewer1.prevbaseResizeTo ?? new Size(176, 176)
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                portraitViewer1.ResizeAllPrevbases(dialog.SizeEntry);
            }
        }

        private void switchPrevbaseSize(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in prevbaseSize.DropDownItems)
            {
                item.Checked = item == sender;
            }

            if (sender == prevbaseOriginalSizeToolStripMenuItem)
            {
                portraitViewer1.prevbaseResizeTo = null;
            }
            else if (sender == x128ToolStripMenuItem)
            {
                portraitViewer1.prevbaseResizeTo = new Size(128, 128);
            }
            else if (sender == x96ToolStripMenuItem)
            {
                portraitViewer1.prevbaseResizeTo = new Size(96, 96);
            }
            else if (sender == x88ToolStripMenuItem)
            {
                portraitViewer1.prevbaseResizeTo = new Size(88, 88);
            }
            else if (sender == customPrevbaseSizeToolStripMenuItem)
            {
                EnterSizeDialog dialog = new EnterSizeDialog();
                dialog.SizeEntry = portraitViewer1.prevbaseResizeTo ?? new Size(128, 128);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    portraitViewer1.prevbaseResizeTo = dialog.SizeEntry;
                }
            }
        }

        private void switchFrontstnameSize(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in frontstnameSizeToolStripMenuItem.DropDownItems)
            {
                item.Checked = item == sender;
            }

            if (sender == frontstnameOriginalSizeToolStripMenuItem)
            {
                portraitViewer1.frontstnameResizeTo = null;
            }
            else if (sender == x56ToolStripMenuItem)
            {
                portraitViewer1.frontstnameResizeTo = new Size(104, 56);
            }
        }

        private void switchSelmapMarkSize(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in selmapMarkSizeToolStripMenuItem.DropDownItems)
            {
                item.Checked = item == sender;
            }

            if (sender == selmapMarkOriginalSizeToolStripMenuItem)
            {
                portraitViewer1.selmapMarkResizeTo = null;
            }
            else if (sender == x56ToolStripMenuItem1)
            {
                portraitViewer1.selmapMarkResizeTo = new Size(104, 56);
            }
        }

        private void texturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainerLeft.Panel2Collapsed = !texturesToolStripMenuItem.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm.Instance.ShowDialog(this);
        }

        private void brawlCrateStageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filepath = ((FileInfo) listBox1.SelectedItem)?.FullName;
            if (!string.IsNullOrEmpty(filepath))
            {
                Program.Open(filepath);
                MainForm.Instance.Focus();
            }
        }

        private void brawlCratecommon5scselmapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(portraitViewer1.OpenFilePath))
            {
                if (!string.IsNullOrEmpty(portraitViewer1.OpenFilePath))
                {
                    MessageBox.Show(this, "Could not find " + portraitViewer1.OpenFilePath + ".");
                }
            }
            else
            {
                Program.Open(new FileInfo(portraitViewer1.OpenFilePath).FullName);
                MainForm.Instance.Focus();
            }
        }

        private void StageManagerForm_KeyDown(object sender, KeyEventArgs e)
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
            else if (e.KeyCode == Keys.Oemtilde)
            {
                e.Handled = true;
                portraitViewer1.openModifyPAT0Dialog();
            }
            else if (e.KeyCode == Keys.OemOpenBrackets)
            {
                e.Handled = true;
                portraitViewer1.repaintIconBorder();
            }
            else if (e.KeyCode == Keys.OemCloseBrackets)
            {
                e.Handled = true;
                portraitViewer1.generateName();
            }
        }

        private void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSongs.SelectedItem is SongListItem sli)
            {
                songPanel1.Open(sli.File);
            }
            else
            {
                string basename = listBoxSongs.SelectedItem.ToString();
                FileInfo fi = new FileInfo("../../sound/strm/" + basename + ".brstm");
                songPanel1.Open(fi);
            }

            exportbrstmToolStripMenuItem.Enabled = deletebrstmToolStripMenuItem.Enabled = songPanel1.FileOpen;
        }

        private void loadStagepacsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderModels.Enabled = loadStagepacsToolStripMenuItem.Checked;
        }

        private void StageManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !savePacsIfNecessary();
        }

        private void StageManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TempFiles.DeleteAll();
        }

        private void cse2xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cse2xToolStripMenuItem.Checked = true;
            cse3xToolStripMenuItem.Checked = false;
        }

        private void cse3xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cse2xToolStripMenuItem.Checked = false;
            cse3xToolStripMenuItem.Checked = true;
        }

        #endregion

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
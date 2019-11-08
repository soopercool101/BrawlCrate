using BrawlCrate.NodeWrappers;
using BrawlCrate.UI;
using BrawlCrate.UI.Model_Previewer.ModelEditControl;
using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using BrawlLib.Modeling.Collada;
using System.Collections.Specialized;
using System.Configuration;
#if !MONO
using BrawlLib.Internal.Windows.Forms.Ookii.Dialogs;
#endif

namespace BrawlCrate
{
    internal static class Program
    {
        /// <summary>
        ///     Used internally by updater protocols. Checks latest release tag name versus this value.
        /// 
        ///     If this isn't equal to the latest release, it assumes it needs to update.
        ///     MAKE SURE THIS IS ALWAYS PROPERLY UPDATED FOR ANY STABLE RELEASE!!!
        /// </summary>
        public static readonly string TagName = "v0.30h3";

        /// <summary>
        ///     Shows upon first launch of a given stable release assuming that automated updating is on.
        /// 
        ///     This mirrors what is included in the GitHub release notes, so if automatic updating is off,
        ///     assume that the user already saw this with the update prompt.
        /// </summary>
        public static readonly string UpdateMessage =
            @"Updated to BrawlCrate v0.30 Hotfix 3! This release is a major rewrite over the latest BrawlBox source. Please view the text changelog for additional information.
- (Hotfix 3) Improve camera for Model Viewers
- (Hotfix 3) Fixes issue in which looping worked incorrectly
- (Hotfix 3) Fixes bug in switching to/from canary builds

Full changelog can be viewed from the help menu.";

        public static readonly string AssemblyTitleFull;
        public static readonly string AssemblyTitleShort;
        public static readonly string AssemblyDescription;
        public static readonly string AssemblyCopyright;
        public static readonly string FullPath;
        public static readonly string BrawlLibTitle;

        private static readonly OpenFileDialog OpenDlg;
        private static readonly OpenFileDialog MultiFileOpenDlg;
        private static readonly SaveFileDialog SaveDlg;
#if !MONO
        private static readonly VistaFolderBrowserDialog FolderDlg;
#else
        private static readonly FolderBrowserDialog FolderDlg;
#endif

        internal static ResourceNode _rootNode;

        public static ResourceNode RootNode
        {
            get => _rootNode;
            set
            {
                _rootNode = value;
                MainForm.Instance.Reset();
            }
        }

        internal static string _rootPath;

        public static readonly string AppPath;

        public static readonly string ApiPath;
        public static readonly string ApiPluginPath;
        public static readonly string ApiLoaderPath;

        public static string RootPath => _rootPath;
#if !DEBUG
        public static readonly bool FirstBoot;
#endif

        static Program()
        {
            Application.EnableVisualStyles();

#if !DEBUG
            if (Properties.Settings.Default.UpdateSettings)
            {
                foreach (Assembly _Assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type _Type in _Assembly.GetTypes())
                    {
                        if (_Type.Name == "Settings" && typeof(SettingsBase).IsAssignableFrom(_Type))
                        {
                            ApplicationSettingsBase settings =
                                (ApplicationSettingsBase) _Type.GetProperty("Default").GetValue(null, null);
                            if (settings != null)
                            {
                                settings.Upgrade();
                                settings.Reload();
                                settings.Save();
                            }
                        }
                    }
                }

                // This is the first time booting this update
                FirstBoot = true;

                // Ensure settings only get updated once
                Properties.Settings.Default.UpdateSettings = false;
                Properties.Settings.Default.Save();
            }
#endif

            FullPath = Process.GetCurrentProcess().MainModule.FileName;
            AppPath = Path.GetDirectoryName(FullPath);
#if CANARY
            AssemblyTitleFull = "BrawlCrate Canary #" + File.ReadAllLines(AppPath + "\\Canary\\New")[2];
            if (BrawlLib.BrawlCrate.PerSessionSettings.Birthday)
            {
                AssemblyTitleFull = AssemblyTitleFull.Replace("BrawlCrate", "PartyBrawl");
            }
            AssemblyTitleShort = AssemblyTitleFull.Substring(0, AssemblyTitleFull.IndexOf('#') + 8);
#else
            AssemblyTitleFull = ((AssemblyTitleAttribute) Assembly.GetExecutingAssembly()
                                                                  .GetCustomAttributes(typeof(AssemblyTitleAttribute),
                                                                      false)[0]).Title;
            if (BrawlLib.BrawlCrate.PerSessionSettings.Birthday)
            {
                AssemblyTitleFull = AssemblyTitleFull.Replace("BrawlCrate", "PartyBrawl");
            }

            AssemblyTitleShort = AssemblyTitleFull.Replace("Hotfix ", "h");
#endif
#if DEBUG
            AssemblyTitleFull += " DEBUG";
            AssemblyTitleShort += " DEBUG";
#endif
            AssemblyDescription =
                ((AssemblyDescriptionAttribute) Assembly
                                                .GetExecutingAssembly()
                                                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0])
                .Description;
            AssemblyCopyright =
                ((AssemblyCopyrightAttribute) Assembly
                                              .GetExecutingAssembly()
                                              .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])
                .Copyright;
            BrawlLibTitle = ((AssemblyTitleAttribute) Assembly
                                                      .GetAssembly(typeof(ResourceNode))
                                                      .GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0])
                .Title;

            OpenDlg = new OpenFileDialog {Title = "Open File"};
            MultiFileOpenDlg = new OpenFileDialog {Title = "Open Files", Multiselect = true};
            SaveDlg = new SaveFileDialog();
#if !MONO
            FolderDlg = new VistaFolderBrowserDialog { UseDescriptionForTitle = true };
#else
            FolderDlg = new FolderBrowserDialog();
#endif
            FolderDlg.Description = "Open Folder";

            ApiPath = Path.Combine(AppPath, "BrawlAPI");
            ApiPluginPath = Path.Combine(ApiPath, "Plugins");
            ApiLoaderPath = Path.Combine(ApiPath, "Loaders");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Set default values for settings immediately, to prevent possible errors down the line
            if (BrawlLib.Properties.Settings.Default.Codes == null)
            {
                BrawlLib.Properties.Settings.Default.Codes = new List<CodeStorage>();
                BrawlLib.Properties.Settings.Default.Save();
            }

            if (BrawlLib.Properties.Settings.Default.ColladaImportOptions == null)
            {
                BrawlLib.Properties.Settings.Default.ColladaImportOptions = new Collada.ImportOptions();
                BrawlLib.Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.ViewerSettings == null)
            {
                Properties.Settings.Default.ViewerSettings = ModelEditorSettings.Default();
                Properties.Settings.Default.ViewerSettingsSet = true;
                Properties.Settings.Default.Save();
            }

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.BuildPath))
            {
                Properties.Settings.Default.BuildPath = AppPath;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.APILoadersWhitelist == null)
            {
                Properties.Settings.Default.APILoadersWhitelist = new StringCollection();
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.APILoadersBlacklist == null)
            {
                Properties.Settings.Default.APILoadersBlacklist = new StringCollection();
                Properties.Settings.Default.Save();
            }

            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.exe");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.bat"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.bat");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "StageBox.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "StageBox.exe");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "BrawlBox.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "BrawlBox.exe");
                }
            }
            catch
            {
                // ignored
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            List<ResourceNode> dirty = GetDirtyFiles();
            Exception ex = e.Exception;
            IssueDialog d = new IssueDialog(ex, dirty);
            d.ShowDialog();
            d.Dispose();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                List<ResourceNode> dirty = GetDirtyFiles();
                IssueDialog d = new IssueDialog(ex, dirty);
                d.ShowDialog();
                d.Dispose();
            }
            else
            {
                MessageBox.Show($"Unhandled exception of type {e.ExceptionObject?.GetType()}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static List<ResourceNode> GetDirtyFiles()
        {
            List<ResourceNode> dirty = new List<ResourceNode>();

            foreach (ModelEditControl control in ModelEditControl.Instances)
            {
                foreach (ResourceNode r in control.rightPanel.pnlOpenedFiles.OpenedFiles)
                {
                    if (r.IsDirty && !dirty.Contains(r))
                    {
                        dirty.Add(r);
                    }
                }
            }

            if (_rootNode != null && _rootNode.IsDirty && !dirty.Contains(_rootNode))
            {
                dirty.Add(_rootNode);
            }

            return dirty;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0].Equals("/gct", StringComparison.OrdinalIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor(AssemblyTitleFull);
                    if (args.Length >= 2)
                    {
                        editor.TargetNode = GCTEditor.LoadGCT(args[1]);
                        if (editor.TargetNode != null)
                        {
                            _rootPath = args[1];
                        }

                        MainForm.Instance?.RecentFilesHandler?.AddFile(args[1]);
                    }

                    MainForm.UpdateDiscordRPC(null, null);
                    Application.Run(editor);
                    try
                    {
                        if (CanRunDiscordRPC)
                        {
                            Discord.DiscordRpc.ClearPresence();
                            Discord.DiscordRpc.Shutdown();
                        }
                    }
                    catch
                    {
                        // Discord RPC doesn't need to work always
                    }

                    return;
                }

                if (args[0].EndsWith(".gct", StringComparison.OrdinalIgnoreCase) ||
                    args[0].EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor(AssemblyTitleFull)
                    {
                        TargetNode = GCTEditor.LoadGCT(args[0])
                    };
                    if (editor.TargetNode != null)
                    {
                        _rootPath = args[0];
                    }

                    MainForm.UpdateDiscordRPC(null, null);
                    MainForm.Instance?.RecentFilesHandler?.AddFile(args[0]);
                    Application.Run(editor);
                    try
                    {
                        if (CanRunDiscordRPC)
                        {
                            Discord.DiscordRpc.ClearPresence();
                            Discord.DiscordRpc.Shutdown();
                        }
                    }
                    catch
                    {
                        // Discord RPC doesn't need to work always
                    }

                    return;
                }

                // Writes the latest changelog to a text file
                if (args[0].Equals("/changelog", StringComparison.OrdinalIgnoreCase))
                {
                    string changelog = UpdateMessage.Substring(UpdateMessage.IndexOf('-'),
                        UpdateMessage.IndexOf(
                            "Full changelog can be found in the installation folder",
                            StringComparison.OrdinalIgnoreCase) -
                        UpdateMessage.IndexOf('-')).Trim('\r', '\n', ' ');
                    string fileName = $@"{AppPath}changelog-newest.txt";
                    using (StreamWriter file = new StreamWriter(fileName))
                    {
                        file.Write(changelog);
                    }

                    return;
                }
            }

            List<string> remainingArgs = new List<string>();
            foreach (string a in args)
            {
                if (a.Equals("/audio:directsound", StringComparison.OrdinalIgnoreCase))
                {
                    AudioProvider.AvailableTypes =
                        AudioProvider.AudioProviderType.DirectSound;
                }
                else if (a.Equals("/audio:openal", StringComparison.OrdinalIgnoreCase))
                {
                    AudioProvider.AvailableTypes = AudioProvider.AudioProviderType.OpenAL;
                }
                else if (a.Equals("/audio:none", StringComparison.OrdinalIgnoreCase))
                {
                    AudioProvider.AvailableTypes = AudioProvider.AudioProviderType.None;
                }
                else
                {
                    remainingArgs.Add(a);
                }
            }

            args = remainingArgs.ToArray();

            try
            {
                if (args.Length >= 1)
                {
                    Open(args[0]);
                }

                if (args.Length >= 2)
                {
                    ResourceNode target =
                        ResourceNode.FindNode(RootNode, args[1], true, StringComparison.OrdinalIgnoreCase);
                    if (target != null)
                    {
                        MainForm.Instance.TargetResource(target);
                    }
                    else
                    {
                        MessageBox.Show($"Error: Unable to find node or path '{args[1]}'!");
                    }
                }

#if !DEBUG //Don't need to see this every time a debug build is compiled
                if (MainForm.Instance.CheckUpdatesOnStartup)
                {
                    MainForm.Instance.CheckUpdates(false);
                }
#endif

                Application.Run(MainForm.Instance);
            }
            catch (FileNotFoundException x)
            {
                if (x.Message.Contains("Could not load file or assembly"))
                {
                    MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                Close(true);
                try
                {
                    if (CanRunDiscordRPC)
                    {
                        Discord.DiscordRpc.ClearPresence();
                        Discord.DiscordRpc.Shutdown();
                    }
                }
                catch
                {
                    // Discord RPC doesn't need to work always
                }
            }
        }

        public static bool New<T>() where T : ResourceNode
        {
            if (!Close())
            {
                return false;
            }

            _rootNode = Activator.CreateInstance<T>();
            _rootNode.Name = $"New{typeof(T).Name}";
            MainForm.Instance.Reset();

            return true;
        }

        public static bool Close()
        {
            return Close(false);
        }

        public static bool Close(bool force)
        {
            //Have to close external files before the root file
            while (ModelEditControl.Instances.Count > 0)
            {
                ModelEditControl control = ModelEditControl.Instances[0];
                if (control.ParentForm != null)
                {
                    control.ParentForm.Close();
                    if (!control.IsDisposed)
                    {
                        return false;
                    }
                }
                else if (!control.Close())
                {
                    return false;
                }
            }

            if (_rootNode != null)
            {
                if (_rootNode.IsDirty && !force)
                {
                    DialogResult res = MessageBox.Show("Save changes?", "Closing", MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.Yes && !Save() || res == DialogResult.Cancel)
                    {
                        return false;
                    }
                }

                _rootNode.Dispose();
                _rootNode = null;

                MainForm.Instance.Reset();
            }

            _rootPath = null;
            MainForm.Instance.UpdateName();
            return true;
        }

        public static bool Open(string path)
        {
            return Open(path, true);
        }

        public static bool Open(string path, bool showErrors)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (path.EndsWith("\\") || path.EndsWith("/"))
            {
                return OpenFolder(path, showErrors);
            }

            if (!File.Exists(path))
            {
                if (showErrors)
                {
                    MessageBox.Show("File does not exist.");
                }

                return false;
            }

            if (path.EndsWith(".gct", StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                GCTEditor editor = new GCTEditor(AssemblyTitleFull)
                {
                    TargetNode = GCTEditor.LoadGCT(path)
                };
                editor.FormClosed += MainForm.UpdateDiscordRPC;
                editor.OpenFileChanged += MainForm.UpdateDiscordRPC;
                editor.Show();
                MainForm.UpdateDiscordRPC(null, null);
                MainForm.Instance.RecentFilesHandler.AddFile(path);
                return true;
            }

            if (!Close())
            {
                return false;
            }
#if !DEBUG
            try
            {
#endif
                if ((_rootNode = NodeFactory.FromFile(null, _rootPath = path)) != null)
                {
                    MainForm.Instance.Reset();
                    MainForm.Instance.RecentFilesHandler.AddFile(path);
                    return true;
                }

                _rootPath = null;
                if (showErrors)
                {
                    MessageBox.Show("Unable to recognize input file.");
                }

                MainForm.Instance.Reset();
#if !DEBUG
            }
            catch (Exception x)
            {
                if (showErrors)
                {
                    MessageBox.Show(x.ToString());
                }
            }
#endif

            Close();

            return false;
        }

        public static bool OpenTemplate(string path)
        {
            return OpenTemplate(path, true);
        }

        public static bool OpenTemplate(string path, bool showErrors)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (!File.Exists(path))
            {
                if (showErrors)
                {
                    MessageBox.Show("Template file does not exist.");
                }

                return false;
            }

            if (!Close())
            {
                return false;
            }
#if !DEBUG
            try
            {
#endif
                if ((_rootNode = NodeFactory.FromFile(null, path)) != null)
                {
                    MainForm.Instance.Reset();
                    return true;
                }

                _rootPath = null;
                if (showErrors)
                {
                    MessageBox.Show("Unable to recognize template file.");
                }

                MainForm.Instance.Reset();
#if !DEBUG
            }
            catch (Exception x)
            {
                if (showErrors)
                {
                    MessageBox.Show(x.ToString());
                }
            }
#endif

            Close();

            return false;
        }

        public static bool OpenFolderFile(out string fileName)
        {
#if !DEBUG
            try
            {
#endif
                if (FolderDlg.ShowDialog() == DialogResult.OK)
                {
                    fileName = FolderDlg.SelectedPath;
                    return true;
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
#endif
            fileName = null;
            return false;
        }

        public static bool OpenFolder(string path)
        {
            return OpenFolder(path, true);
        }

        public static bool OpenFolder(string path, bool showErrors)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

#if !MONO
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }
#else
            if (!path.EndsWith("/"))
            {
                path += "/";
            }
#endif

            if (!Directory.Exists(path))
            {
                if (showErrors)
                {
                    MessageBox.Show("Directory does not exist.");
                }

                return false;
            }

            if (!Close())
            {
                return false;
            }
#if !DEBUG
            try
            {
#endif
                if ((_rootNode = NodeFactory.FromFolder(null, _rootPath = path)) != null)
                {
                    MainForm.Instance.Reset();
                    MainForm.Instance.RecentFilesHandler.AddFile(path);
                    return true;
                }

                _rootPath = null;
                if (showErrors)
                {
                    MessageBox.Show("Unable to recognize input file.");
                }

                MainForm.Instance.Reset();
#if !DEBUG
            }
            catch (Exception x)
            {
                if (showErrors)
                {
                    MessageBox.Show(x.ToString());
                }
            }
#endif
            Close();

            return false;
        }

        public static unsafe void Scan(FileMap map, FileScanNode node)
        {
            using (ProgressWindow progress = new ProgressWindow(MainForm.Instance, "File Scanner",
                "Scanning for known file types, please wait...", true))
            {
                progress.TopMost = true;
                progress.Begin(0, map.Length, 0);

                byte* data = (byte*) map.Address;
                uint i = 0;
                do
                {
                    ResourceNode n = null;
                    DataSource d = new DataSource(&data[i], 0);
                    if ((n = NodeFactory.GetRaw(d)) != null)
                    {
                        if (!(n is MSBinNode))
                        {
                            n.Initialize(node, d);
                            try
                            {
                                i += (uint) n.WorkingSource.Length;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }

                    progress.Update(i + 1);
                } while (++i + 4 < map.Length);

                progress.Finish();
            }
        }

        public static bool Save()
        {
            MainForm.Instance.resourceTree_SelectionChanged("Saving File", EventArgs.Empty);
            if (_rootNode != null)
            {
#if !DEBUG
                try
                {
#endif
                    if (_rootPath == null)
                    {
                        return SaveAs();
                    }

                    bool force = Control.ModifierKeys == (Keys.Control | Keys.Shift);
                    if (!force && !_rootNode.IsDirty)
                    {
                        MessageBox.Show("No changes have been made.");
                        MainForm.Instance.resourceTree_SelectionChanged(null, EventArgs.Empty);
                        return false;
                    }

                    if (!(_rootNode is FolderNode))
                    {
                        _rootNode.Merge(force);
                    }

                    _rootNode.Export(_rootPath);
                    _rootNode.IsDirty = false;
                    MainForm.Instance.resourceTree_SelectionChanged(null, EventArgs.Empty);
                    return true;
#if !DEBUG
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                    _rootNode.SignalPropertyChange();
                }
#endif
            }

            MainForm.Instance.resourceTree_SelectionChanged(null, EventArgs.Empty);
            return false;
        }

        public static string ChooseFolder()
        {
            if (FolderDlg.ShowDialog() == DialogResult.OK)
            {
                return FolderDlg.SelectedPath;
            }

            return null;
        }

        public static bool OpenFile(string filter, out string fileName)
        {
            OpenDlg.Filter = filter;
#if !DEBUG
            try
            {
#endif
                if (OpenDlg.ShowDialog() == DialogResult.OK)
                {
                    fileName = OpenDlg.FileName;
                    return true;
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
#endif
            fileName = null;
            return false;
        }

        public static int OpenFiles(string filter, out string[] fileNames)
        {
            MultiFileOpenDlg.Filter = filter;
#if !DEBUG
            try
            {
#endif
                if (MultiFileOpenDlg.ShowDialog() == DialogResult.OK)
                {
                    fileNames = MultiFileOpenDlg.FileNames;
                    return fileNames.Length;
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
#endif
            fileNames = null;
            return 0;
        }

        public static int SaveFile(string filter, string name, out string fileName)
        {
            return SaveFile(filter, name, out fileName, true);
        }

        public static int SaveFile(string filter, string name, out string fileName, bool categorize)
        {
            int fIndex = 0;
            fileName = null;

            SaveDlg.Filter = filter;
            SaveDlg.FileName = name;
            SaveDlg.FilterIndex = 1;
            if (SaveDlg.ShowDialog() == DialogResult.OK)
            {
                if (categorize && SaveDlg.FilterIndex == 1 && Path.HasExtension(SaveDlg.FileName))
                {
                    fIndex = CategorizeFilter(SaveDlg.FileName, filter);
                }
                else
                {
                    fIndex = SaveDlg.FilterIndex;
                }

                //Fix extension
                fileName = ApplyExtension(SaveDlg.FileName, filter, fIndex - 1);
            }

            return fIndex;
        }

        public static bool SaveFolder(out string folderName)
        {
            folderName = null;
            if (FolderDlg.ShowDialog() == DialogResult.OK)
            {
                folderName = FolderDlg.SelectedPath;
                return true;
            }

            return false;
        }

        public static int CategorizeFilter(string path, string filter)
        {
            string ext = "*" + Path.GetExtension(path);

            string[] split = filter.Split('|');
            for (int i = 3; i < split.Length; i += 2)
            {
                foreach (string s in split[i].Split(';'))
                {
                    if (s.Equals(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        return (i + 1) / 2;
                    }
                }
            }

            return 1;
        }

        public static string ApplyExtension(string path, string filter, int filterIndex)
        {
            if (Path.HasExtension(path) && !int.TryParse(Path.GetExtension(path), out int _))
            {
                return path;
            }

            int index = filter.IndexOfOccurance('|', filterIndex * 2);
            if (index == -1)
            {
                return path;
            }

            index = filter.IndexOf('.', index);
            int len = Math.Max(filter.Length, filter.IndexOfAny(new char[] {';', '|'})) - index;

            string ext = filter.Substring(index, len);

            if (ext.IndexOf('*') >= 0)
            {
                return path;
            }

            return path + ext;
        }

        internal static bool SaveAs()
        {
            MainForm.Instance.resourceTree_SelectionChanged("Saving File", EventArgs.Empty);
            if (MainForm.Instance.RootNode is GenericWrapper)
            {
#if !DEBUG
                try
                {
#endif
                    GenericWrapper w = MainForm.Instance.RootNode as GenericWrapper;
                    string path = w.Export();
                    if (path != null)
                    {
                        _rootPath = path;
                        RootNode._origPath = path;
                        if (w is FolderWrapper)
                        {
                            w.Resource.Name = w.Resource.FileName;
                            w.Text = w.Resource.Name;
                        }

                        MainForm.Instance.UpdateName();
                        w.Resource.IsDirty = false;
                        MainForm.Instance.resourceTree_SelectionChanged(null, EventArgs.Empty);
                        return true;
                    }
#if !DEBUG
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                //finally { }
#endif
            }

            MainForm.Instance.resourceTree_SelectionChanged(null, EventArgs.Empty);
            return false;
        }

        public static bool CanRunGithubApp(bool showMessages, out string path)
        {
            path = $"{AppPath}\\Updater.exe";
            if (!File.Exists(path))
            {
                if (showMessages)
                {
                    MessageBox.Show($"Could not find {path}");
                }

                return false;
            }

            return true;
        }

        public static bool CanRunDiscordRPC => File.Exists($"{AppPath}\\discord-rpc.dll");

        public static void ForceDownloadStable()
        {
            try
            {
                if (CanRunGithubApp(true, out string path))
                {
                    Process git = Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = $"-dlStable {RootPath}"
                    });
                    git?.WaitForExit();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ForceDownloadCanary()
        {
            try
            {
                if (CanRunGithubApp(true, out string path))
                {
                    Process git = Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = $"-dlCanary {RootPath}"
                    });
                    git?.WaitForExit();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
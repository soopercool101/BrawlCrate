using BrawlCrate.NodeWrappers;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using BrawlLib.Modeling;

namespace BrawlCrate
{
    internal static class Program
    {
        //Make sure this matches the tag name of the release on github exactly
        public static readonly string TagName = "BrawlCrate_v0.26Hotfix3";

        public static readonly string AssemblyTitleFull;
        public static readonly string AssemblyTitleShort;
        public static readonly string AssemblyDescription;
        public static readonly string AssemblyCopyright;
        public static readonly string FullPath;
        public static readonly string BrawlLibTitle;

        private static readonly OpenFileDialog _openDlg;
        private static readonly SaveFileDialog _saveDlg;
#if !MONO
        private static readonly Ookii.Dialogs.VistaFolderBrowserDialog _folderDlg;
#else
        private static readonly FolderBrowserDialog _folderDlg;
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

        public static string AppPath;
        public static string RootPath => _rootPath;

        static Program()
        {
            Application.EnableVisualStyles();
            FullPath = Process.GetCurrentProcess().MainModule.FileName;
            AppPath = FullPath.Substring(0, FullPath.LastIndexOf("BrawlCrate.exe"));
#if CANARY
            AssemblyTitleFull = "BrawlCrate NEXT Canary #" + File.ReadAllLines(AppPath + "\\Canary\\New")[2];
            AssemblyTitleShort = AssemblyTitleFull.Substring(0, AssemblyTitleFull.IndexOf('#') + 8);
#else
            AssemblyTitleFull = ((AssemblyTitleAttribute) Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            AssemblyTitleShort = AssemblyTitleFull;
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

            _openDlg = new OpenFileDialog();
            _saveDlg = new SaveFileDialog();
#if !MONO
            _folderDlg = new Ookii.Dialogs.VistaFolderBrowserDialog();
#else
            _folderDlg = new FolderBrowserDialog();
#endif

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
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
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            List<ResourceNode> dirty = GetDirtyFiles();
            Exception ex = e.Exception;
            IssueDialog d = new IssueDialog(ex, dirty);
            d.ShowDialog();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                List<ResourceNode> dirty = GetDirtyFiles();
                Exception ex = e.ExceptionObject as Exception;
                IssueDialog d = new IssueDialog(ex, dirty);
                d.ShowDialog();
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
                    GCTEditor editor = new GCTEditor();
                    if (args.Length >= 2)
                    {
                        editor.TargetNode = GCTEditor.LoadGCT(args[1]);
                        if (editor.TargetNode != null)
                        {
                            _rootPath = args[1];
                        }
                    }

                    MainForm.UpdateDiscordRPC(null, null);
                    Application.Run(editor);

                    if (CanRunDiscordRPC)
                    {
                        Discord.DiscordRpc.ClearPresence();
                        Discord.DiscordRpc.Shutdown();
                    }

                    return;
                }

                if (args[0].EndsWith(".gct", StringComparison.OrdinalIgnoreCase) ||
                    args[0].EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor
                    {
                        TargetNode = GCTEditor.LoadGCT(args[0])
                    };
                    if (editor.TargetNode != null)
                    {
                        _rootPath = args[0];
                    }

                    MainForm.UpdateDiscordRPC(null, null);
                    Application.Run(editor);
                    if (CanRunDiscordRPC)
                    {
                        Discord.DiscordRpc.ClearPresence();
                        Discord.DiscordRpc.Shutdown();
                    }

                    return;
                }
            }

            List<string> remainingArgs = new List<string>();
            foreach (string a in args)
            {
                if (a.Equals("/audio:directsound", StringComparison.OrdinalIgnoreCase))
                {
                    System.Audio.AudioProvider.AvailableTypes =
                        System.Audio.AudioProvider.AudioProviderType.DirectSound;
                }
                else if (a.Equals("/audio:openal", StringComparison.OrdinalIgnoreCase))
                {
                    System.Audio.AudioProvider.AvailableTypes = System.Audio.AudioProvider.AudioProviderType.OpenAL;
                }
                else if (a.Equals("/audio:none", StringComparison.OrdinalIgnoreCase))
                {
                    System.Audio.AudioProvider.AvailableTypes = System.Audio.AudioProvider.AudioProviderType.None;
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
                    ResourceNode target = ResourceNode.FindNode(RootNode, args[1], true);
                    if (target != null)
                    {
                        MainForm.Instance.TargetResource(target);
                    }
                    else
                    {
                        MessageBox.Show($"Error: Unable to find node or path '{args[1]}'!");
                    }
                }

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
                    throw x;
                }
            }
            finally
            {
                Close(true);
                if (CanRunDiscordRPC)
                {
                    Discord.DiscordRpc.ClearPresence();
                    Discord.DiscordRpc.Shutdown();
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
            _rootNode.Name = "NewTree";
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
                GCTEditor editor = new GCTEditor
                {
                    TargetNode = GCTEditor.LoadGCT(path)
                };
                editor.FormClosed += MainForm.UpdateDiscordRPC;
                editor.OpenFileChanged += MainForm.UpdateDiscordRPC;
                editor.Show();
                MainForm.UpdateDiscordRPC(null, null);
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
                    return true;
                }
                else
                {
                    _rootPath = null;
                    if (showErrors)
                    {
                        MessageBox.Show("Unable to recognize input file.");
                    }
                    MainForm.Instance.Reset();
                }
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

                    _rootNode.Merge(force);
                    _rootNode.Export(_rootPath);
                    _rootNode.IsDirty = false;
                    MainForm.Instance.resourceTree_SelectionChanged(null, EventArgs.Empty);
                    return true;
#if !DEBUG
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
#endif
            }
            MainForm.Instance.resourceTree_SelectionChanged(null, EventArgs.Empty);
            return false;
        }

        public static string ChooseFolder()
        {
            if (_folderDlg.ShowDialog() == DialogResult.OK)
            {
                return _folderDlg.SelectedPath;
            }

            return null;
        }

        public static int OpenFile(string filter, out string fileName)
        {
            return OpenFile(filter, out fileName, true);
        }

        public static int OpenFile(string filter, out string fileName, bool categorize)
        {
            _openDlg.Filter = filter;
#if !DEBUG
            try
            {
#endif
                if (_openDlg.ShowDialog() == DialogResult.OK)
                {
                    fileName = _openDlg.FileName;
                    if (categorize && _openDlg.FilterIndex == 1)
                    {
                        return CategorizeFilter(_openDlg.FileName, filter);
                    }
                    else
                    {
                        return _openDlg.FilterIndex;
                    }
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
#endif
            fileName = null;
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

            _saveDlg.Filter = filter;
            _saveDlg.FileName = name;
            _saveDlg.FilterIndex = 1;
            if (_saveDlg.ShowDialog() == DialogResult.OK)
            {
                if (categorize && _saveDlg.FilterIndex == 1 && Path.HasExtension(_saveDlg.FileName))
                {
                    fIndex = CategorizeFilter(_saveDlg.FileName, filter);
                }
                else
                {
                    fIndex = _saveDlg.FilterIndex;
                }

                //Fix extension
                fileName = ApplyExtension(_saveDlg.FileName, filter, fIndex - 1);
            }

            return fIndex;
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
            if (Path.HasExtension(path) && !int.TryParse(Path.GetExtension(path), out int tmp))
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
                    MessageBox.Show("Could not find " + path);
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
                    Process git = Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = $"-dlStable {RootPath}",
                    });
                    git.WaitForExit();
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
                    Process git = Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = $"-dlCanary {RootPath}",
                    });
                    git.WaitForExit();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
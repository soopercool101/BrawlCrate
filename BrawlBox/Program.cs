using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using BrawlBox.NodeWrappers;

namespace BrawlBox
{
    static class Program
    {
        //Make sure this matches the tag name of the release on github exactly
        public static readonly string TagName = "v0.78_h1";

        public static readonly string AssemblyTitle;
        public static readonly string AssemblyDescription;
        public static readonly string AssemblyCopyright;
        public static readonly string FullPath;
        public static readonly string BrawlLibTitle;

        private static OpenFileDialog _openDlg;
        private static SaveFileDialog _saveDlg;
        private static FolderBrowserDialog _folderDlg;

        internal static ResourceNode _rootNode;
        public static ResourceNode RootNode { get { return _rootNode; } set { _rootNode = value; MainForm.Instance.Reset(); } }
        internal static string _rootPath;
        public static string RootPath { get { return _rootPath; } }

        static Program()
        {
            Application.EnableVisualStyles();

            AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            AssemblyDescription = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
            AssemblyCopyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            FullPath = Process.GetCurrentProcess().MainModule.FileName;
            BrawlLibTitle = ((AssemblyTitleAttribute)Assembly.GetAssembly(typeof(ResourceNode)).GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;

            _openDlg = new OpenFileDialog();
            _saveDlg = new SaveFileDialog();
            _folderDlg = new FolderBrowserDialog();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            List<ResourceNode> dirty = GetDirtyFiles();
            Exception ex = e.Exception;
            IssueDialog d = new IssueDialog(ex, dirty);
            d.ShowDialog();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
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

            foreach (var control in ModelEditControl.Instances)
                foreach (ResourceNode r in control.rightPanel.pnlOpenedFiles.OpenedFiles)
                    if (r.IsDirty && !dirty.Contains(r))
                        dirty.Add(r);

            if (_rootNode != null && _rootNode.IsDirty && !dirty.Contains(_rootNode))
                dirty.Add(_rootNode);

            return dirty;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0].Equals("/gct", StringComparison.InvariantCultureIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor();
                    if (args.Length >= 2) editor.TargetNode = GCTEditor.LoadGCT(args[1]);
                    Application.Run(editor);
                    return;
                }
                else if (args[0].EndsWith(".gct", StringComparison.InvariantCultureIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor();
                    editor.TargetNode = GCTEditor.LoadGCT(args[0]);
                    Application.Run(editor);
                    return;
                }
            }

            List<string> remainingArgs = new List<string>();
            foreach (string a in args)
            {
                if (a.Equals("/audio:directsound", StringComparison.InvariantCultureIgnoreCase))
                    System.Audio.AudioProvider.AvailableTypes = System.Audio.AudioProvider.AudioProviderType.DirectSound;
                else if (a.Equals("/audio:openal", StringComparison.InvariantCultureIgnoreCase))
                    System.Audio.AudioProvider.AvailableTypes = System.Audio.AudioProvider.AudioProviderType.OpenAL;
                else if (a.Equals("/audio:none", StringComparison.InvariantCultureIgnoreCase))
                    System.Audio.AudioProvider.AvailableTypes = System.Audio.AudioProvider.AudioProviderType.None;
                else
                    remainingArgs.Add(a);
            }
            args = remainingArgs.ToArray();

            try
            {
                if (args.Length >= 1)
                    Open(args[0]);

                if (args.Length >= 2)
                {
                    ResourceNode target = ResourceNode.FindNode(RootNode, args[1], true);
                    if (target != null)
                        MainForm.Instance.TargetResource(target);
                    else
                        Say(String.Format("Error: Unable to find node or path '{0}'!", args[1]));
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
            finally { Close(true); }
        }

        public static void Say(string msg)
        {
            MessageBox.Show(msg);
        }

        public static bool New<T>() where T : ResourceNode
        {
            if (!Close())
                return false;

            _rootNode = Activator.CreateInstance<T>();
            _rootNode.Name = "NewTree";
            MainForm.Instance.Reset();

            return true;
        }

        public static bool Close() { return Close(false); }
        public static bool Close(bool force)
        {
            //Have to close external files before the root file
            while (ModelEditControl.Instances.Count > 0)
            {
                var control = ModelEditControl.Instances[0];
                if (control.ParentForm != null)
                {
                    control.ParentForm.Close();
                    if (!control.IsDisposed)
                        return false;
                }
                else if (!control.Close())
                    return false;
            }

            if (_rootNode != null)
            {
                if (_rootNode.IsDirty && !force)
                {
                    DialogResult res = MessageBox.Show("Save changes?", "Closing", MessageBoxButtons.YesNoCancel);
                    if ((res == DialogResult.Yes && !Save()) || res == DialogResult.Cancel)
                        return false;
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
            if (String.IsNullOrEmpty(path))
                return false;

            if (!File.Exists(path))
            {
                Say("File does not exist.");
                return false;
            }

            if (path.EndsWith(".gct", StringComparison.InvariantCultureIgnoreCase))
            {
                GCTEditor editor = new GCTEditor();
                editor.TargetNode = GCTEditor.LoadGCT(path);
                editor.Show();
                return true;
            }

            if (!Close())
                return false;

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
                    Say("Unable to recognize input file.");
                    MainForm.Instance.Reset();
                }
#if !DEBUG
            }
            catch (Exception x) { Say(x.ToString()); }
#endif

            Close();

            return false;
        }

        public static unsafe void Scan(FileMap map, FileScanNode node)
        {
            using (ProgressWindow progress = new ProgressWindow(MainForm.Instance, "File Scanner", "Scanning for known file types, please wait...", true))
            {
                progress.TopMost = true;
                progress.Begin(0, map.Length, 0);

                byte* data = (byte*)map.Address;
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
                                i += (uint)n.WorkingSource.Length;
                            }
                            catch
                            {

                            }
                        }
                    }
                    progress.Update(i + 1);
                }
                while (++i + 4 < map.Length);

                progress.Finish();
            }
        }

        public static bool Save()
        {
            if (_rootNode != null)
            {
#if !DEBUG
                try
                {
#endif

                    if (_rootPath == null)
                        return SaveAs();

                    bool force = Control.ModifierKeys == (Keys.Control | Keys.Shift);
                    if (!force && !_rootNode.IsDirty)
                    {
                        MessageBox.Show("No changes have been made.");
                        return false;
                    }

                    _rootNode.Merge(force);
                    _rootNode.Export(_rootPath);
                    _rootNode.IsDirty = false;

                    return true;

#if !DEBUG
                }
                catch (Exception x) { Say(x.Message); }
#endif
            }
            return false;
        }

        public static string ChooseFolder()
        {
            if (_folderDlg.ShowDialog() == DialogResult.OK)
                return _folderDlg.SelectedPath;
            return null;
        }

        public static int OpenFile(string filter, out string fileName) { return OpenFile(filter, out fileName, true); }
        public static int OpenFile(string filter, out string fileName, bool categorize)
        {
            _openDlg.Filter = filter;
            //_openDlg.AutoUpgradeEnabled = false;
#if !DEBUG
            try
            {
#endif
                if (_openDlg.ShowDialog() == DialogResult.OK)
                {
                    fileName = _openDlg.FileName;
                    if ((categorize) && (_openDlg.FilterIndex == 1))
                        return CategorizeFilter(_openDlg.FileName, filter);
                    else
                        return _openDlg.FilterIndex;
                }
#if !DEBUG
            }
            catch (Exception ex) { Say(ex.ToString()); }
#endif
            fileName = null;
            return 0;
        }
        public static int SaveFile(string filter, string name, out string fileName) { return SaveFile(filter, name, out fileName, true); }
        public static int SaveFile(string filter, string name, out string fileName, bool categorize)
        {
            int fIndex = 0;
            fileName = null;

            _saveDlg.Filter = filter;
            _saveDlg.FileName = name;
            _saveDlg.FilterIndex = 1;
            if (_saveDlg.ShowDialog() == DialogResult.OK)
            {
                if ((categorize) && (_saveDlg.FilterIndex == 1) && (Path.HasExtension(_saveDlg.FileName)))
                    fIndex = CategorizeFilter(_saveDlg.FileName, filter);
                else
                    fIndex = _saveDlg.FilterIndex;

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
                foreach (string s in split[i].Split(';'))
                    if (s.Equals(ext, StringComparison.OrdinalIgnoreCase))
                        return (i + 1) / 2;
            return 1;
        }
        public static string ApplyExtension(string path, string filter, int filterIndex)
        {
            int tmp;
            if ((Path.HasExtension(path)) && (!int.TryParse(Path.GetExtension(path), out tmp)))
                return path;

            int index = filter.IndexOfOccurance('|', filterIndex * 2);
            if (index == -1)
                return path;

            index = filter.IndexOf('.', index);
            int len = Math.Max(filter.Length, filter.IndexOfAny(new char[] { ';', '|' })) - index;

            string ext = filter.Substring(index, len);

            if (ext.IndexOf('*') >= 0)
                return path;

            return path + ext;
        }

        internal static bool SaveAs()
        {
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
                        return true;
                    }
#if !DEBUG
                }
                catch (Exception x) { Say(x.Message); }
                //finally { }
#endif
            }
            return false;
        }

        public static bool CanRunGithubApp(bool showMessages, out string path)
        {
            path = $"{Application.StartupPath}\\lib\\Updater.exe";
            if (!File.Exists(path))
            {
                if (showMessages)
                    MessageBox.Show("Could not find " + path);
                return false;
            }

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).
                OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                object o = ndpKey.GetValue("Release");
                if (o == null)
                    return false;

                int releaseKey = Convert.ToInt32(o);
                if (releaseKey < 378389)
                    return false;
            }
            return true;
        }
    }
}

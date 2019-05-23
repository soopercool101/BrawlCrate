using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using Ikarus.UI;

namespace Ikarus
{
    static class Program
    {
        public static readonly string AssemblyTitle;
        public static readonly string AssemblyDescription;
        public static readonly string AssemblyCopyright;
        public static readonly string FullPath;

        private static OpenFileDialog _openDlg;
        private static SaveFileDialog _saveDlg;
        private static FolderSelectDialog _folderDlg;

        static Program()
        {
            Application.EnableVisualStyles();

            AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            AssemblyDescription = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
            AssemblyCopyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            FullPath = Process.GetCurrentProcess().MainModule.FileName;

            _openDlg = new OpenFileDialog();
            _saveDlg = new SaveFileDialog();
            _folderDlg = new FolderSelectDialog();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            SplashForm s = new SplashForm(); s.Show();
            Application.Run(new MainForm(s));
        }

        public static List<ResourceNode> OpenedFiles { get { return Manager.OpenedFiles; } }
        public static BindingList<string> OpenedFilePaths { get { return Manager.OpenedFilePaths; } }
        
        public static void Say(string msg) { MessageBox.Show(msg); }

        public static bool Close() { return Close(false); }
        public static bool Close(bool force)
        {
            if (OpenedFiles != null && OpenedFiles.Count > 0)
            {
                List<ResourceNode> changed = new List<ResourceNode>();
                foreach (ResourceNode r in OpenedFiles)
                    if (r.IsDirty && !force)
                        changed.Add(r);
                
                if (changed.Count > 0)
                {
                    string message = String.Format("Save changes? The following file{0} ha{1} been changed:", changed.Count > 1 ? "s" : "", changed.Count > 1 ? "ve" : "s");
                    foreach (ResourceNode r in changed)
                        message += "\n" + r._origPath;
                    DialogResult res = MessageBox.Show(message, "Closing", MessageBoxButtons.YesNoCancel);
                    if (((res == DialogResult.Yes) && (!Save())) || (res == DialogResult.Cancel))
                        return false;

                    foreach (ResourceNode r in OpenedFiles)
                        r.Dispose();
                }
            }
            Ikarus.Properties.Settings.Default.Save();
            return true;
        }

        public static string ChooseFolder(string basePath)
        {
            _folderDlg.InitialDirectory = basePath;
            if (_folderDlg.ShowDialog() == DialogResult.OK)
                return _folderDlg.SelectedPath;
            return null;
        }

        public static string RootPath = null;
        public static bool OpenRoot(string basePath)
        {
            //_folderDlg.Description = "Choose the root folder of all Brawl files.";
            return OpenRootFromPath(ChooseFolder(basePath));
        }

        public static bool OpenRootFromPath(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
#if !DEBUG
                try
                {
#endif
                    Manager.CloseRoot();
                    RootPath = path;
                    Manager.OpenRoot(path);
                    return true;
#if !DEBUG
                }
                catch (Exception x) { Say(x.ToString()); }
#endif
            }
            return false;
        }

        public static bool Save()
        {
            if (OpenedFiles != null && OpenedFiles.Count > 0)
            {
                try
                {
                    foreach (ResourceNode r in OpenedFiles)
                        if (r.IsDirty)
                        {
                            r.Merge(Control.ModifierKeys == (Keys.Control | Keys.Shift));
                            r.Export(r._origPath);
                            r.IsDirty = false;
                        }

                    return true;
                }
                catch (Exception x) { Say(x.Message); }
            }
            return false;
        }
    }
}

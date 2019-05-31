using BrawlCrate.NodeWrappers;
using BrawlLib.SSBB.ResourceNodes;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BrawlCrate.API
{
    public static class BrawlAPI
    {
        public static bool PythonEnabled
        {
            get
            {
                return Engine.GetSearchPaths().Count > 0;
            }
        }

        public static bool FSharpEnabled
        {
            get
            {
                return Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.OrdinalIgnoreCase) && _fsharpPathFound;
            }
        }
        private static bool _fsharpPathFound = true;

        private static string fsi_path;

        static BrawlAPI()
        {
            ContextMenuHooks = new Dictionary<Type, ToolStripMenuItem[]>();
            Plugins = new List<PluginScript>();
            Loaders = new List<PluginLoader>();
            Engine = Python.CreateEngine();
            Runtime = Engine.Runtime;

            // Setup IronPython engine
            string settingPath = BrawlCrate.Properties.Settings.Default.PythonInstallationPath;
            List<string> searchPaths = new List<string>();
            // First, search the directory found in the settings
            if (!settingPath.Equals("") && Directory.Exists(settingPath))
            {
                if (Directory.Exists(settingPath + "/Lib"))
                {
                    searchPaths.Add($"{ settingPath }/Lib");
                }
                else
                {
                    searchPaths.Add(settingPath);
                }
            }
            // Then check for Python 2.7 (The recommended version for iron python) in its default installation directory
            else if (Directory.Exists("C:/Python27/Lib"))
            {
                searchPaths.Add("C:/Python27/Lib");
            }
            // Finally, search for any other Python installations in their default directories
            else
            {
                foreach (DirectoryInfo d in Directory.CreateDirectory("C:/").GetDirectories().Reverse())
                {
                    if (d.FullName.StartsWith("C:/Python") && Directory.Exists(d.FullName + "/Lib"))
                    {
                        searchPaths.Add($"{ d.FullName }/Lib");
                        break;
                    }
                }
            }
            // Then see if there's a directory included in the installation (This can also be used for additional modules or a primary install, so add it in addition)
            if (Directory.Exists($"{ Application.StartupPath }/Python"))
            {
                if(!searchPaths.Contains($"{ Application.StartupPath }/Python"))
                {
                    searchPaths.Add($"{ Application.StartupPath }/Python");
                }
            }

            if(settingPath.Equals("") || settingPath.Equals("(none)"))
            {
                if(searchPaths.Count > 0)
                {
                    BrawlCrate.Properties.Settings.Default.PythonInstallationPath = searchPaths[0];
                    BrawlCrate.Properties.Settings.Default.Save();
                }
                else if (!settingPath.Equals("(none)", StringComparison.OrdinalIgnoreCase))
                {
                    using (FolderBrowserDialog dlg = new FolderBrowserDialog())
                    {
                        if (MessageBox.Show("Python installation could not be detected, would you like to locate it now? If Python is not installed, the plugin system will be disabled.", "BrawlAPI", MessageBoxButtons.YesNo) == DialogResult.Yes
                            && dlg.ShowDialog() == DialogResult.OK)
                        {
                            searchPaths.Add(dlg.SelectedPath);
                            BrawlCrate.Properties.Settings.Default.PythonInstallationPath = dlg.SelectedPath;
                            BrawlCrate.Properties.Settings.Default.Save();
                        }
                        else
                        {
                            MessageBox.Show("Python installation not found. Python plugins and loaders will be disabled. The python installation path can be changed in the settings.", "BrawlAPI");
                            BrawlCrate.Properties.Settings.Default.PythonInstallationPath = "(none)";
                            BrawlCrate.Properties.Settings.Default.Save();
                        }
                    }
                }
            }

            // If any python installations are found, add them to the search pathsp
            Engine.SetSearchPaths(searchPaths.ToArray());

            //Import BrawlCrate and Brawllib
            Assembly mainAssembly = Assembly.GetExecutingAssembly();
            Assembly brawllib = Assembly.GetAssembly(typeof(ResourceNode));

            Runtime.LoadAssembly(mainAssembly);
            Runtime.LoadAssembly(brawllib);
            Runtime.LoadAssembly(typeof(string).Assembly);
            Runtime.LoadAssembly(typeof(Uri).Assembly);
            Runtime.LoadAssembly(typeof(Form).Assembly);

            // Hook the main form's resourceTree selection changed event to add contextMenu items to nodewrapper
            MainForm.Instance.resourceTree.SelectionChanged += ResourceTree_SelectionChanged;
        }
        internal static ScriptEngine Engine { get; set; }
        internal static ScriptRuntime Runtime { get; set; }

        internal static List<PluginScript> Plugins { get; set; }
        internal static List<PluginLoader> Loaders { get; set; }

        internal static Dictionary<Type, ToolStripMenuItem[]> ContextMenuHooks { get; set; }

        internal static void RunScript(string path)
        {
            if (Path.GetExtension(path) == ".fsx")
            {
                _fsharpPathFound = FSharpEnabled && FSharpInstall(out fsi_path);
                if (FSharpEnabled)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), $"BrawlCrate-{Guid.NewGuid()}.fsx");
                    using (StreamReader srIn = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                    using (StreamWriter swOut = new StreamWriter(new FileStream(tempPath, FileMode.Create, FileAccess.Write)))
                    {
                        swOut.WriteLine($"#r \"{Assembly.GetAssembly(typeof(NodeFactory)).Location.Replace('\\', '/')}\"");
                        swOut.WriteLine($"#r \"{Assembly.GetAssembly(typeof(MainForm)).Location.Replace('\\', '/')}\"");
                        string line;
                        while ((line = srIn.ReadLine()) != null)
                        {
                            swOut.WriteLine(line);
                        }
                    }

                    Process p = Process.Start(new ProcessStartInfo
                    {
                        FileName = fsi_path,
                        Arguments = $"--noninteractive \"{tempPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true
                    });
                    p.EnableRaisingEvents = true;
                    p.Exited += (o, e) =>
                    {
                        File.Delete(tempPath);
                        if (p.ExitCode != 0)
                        {
                            string err = p.StandardError.ReadToEnd().Trim();
                            if (!string.IsNullOrWhiteSpace(err))
                            {
                                MessageBox.Show(err);
                            }
                            else
                            {
                                MessageBox.Show($"fsi.exe quit with exit code {p.ExitCode}");
                            }
                        }
                    };
                }
                else
                {
                    MessageBox.Show("F# was not found to be properly installed on this system. Please install F# and try again.", "BrawlAPI");
                }
            }
            else if (PythonEnabled)
            {
                try
                {
                    ScriptSource script = Engine.CreateScriptSourceFromFile(path);
                    CompiledCode code = script.Compile();
                    ScriptScope scope = Engine.CreateScope();
                    script.Execute();
                }
                catch (SyntaxErrorException e)
                {
                    string msg = $"Syntax error in \"{Path.GetFileName(path)}\"\n{e.Message}";
                    ShowMessage(msg, Path.GetFileName(path));
                }
                catch (SystemExitException e)
                {
                    string msg = $"SystemExit in \"{Path.GetFileName(path)}\"\n{e.Message}";
                    ShowMessage(msg, Path.GetFileName(path));
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("BrawlBox") ||
                        e.Message.Contains("bboxapi"))
                    {
                        ConvertPlugin(path);
                        RunScript(path);
                    }
                    else
                    {
                        string msg = $"Error running script \"{Path.GetFileName(path)}\"\n{e.Message}";
                        ShowMessage(msg, Path.GetFileName(path));
                    }
                }
            }
        }
        internal static bool CreatePlugin(string path, bool loader)
        {
            try
            {
                if (path.EndsWith(".fsx", StringComparison.OrdinalIgnoreCase) && FSharpEnabled && fsi_path == null)
                {
                    _fsharpPathFound = FSharpEnabled && FSharpInstall(out fsi_path);
                }
                if ((path.EndsWith(".py", StringComparison.OrdinalIgnoreCase) && !PythonEnabled) || (path.EndsWith(".fsx", StringComparison.OrdinalIgnoreCase) && !FSharpEnabled))
                {
                    return false;
                }
                ScriptSource script = Engine.CreateScriptSourceFromFile(path);
                CompiledCode code = script.Compile();
                ScriptScope scope = Engine.CreateScope();
                if (!loader)
                {
                    Plugins.Add(new PluginScript(Path.GetFileNameWithoutExtension(path), script, scope));
                }
                else
                {
                    script.Execute();
                }
                return true;
            }
            catch (SyntaxErrorException e)
            {
                string msg = $"Syntax error in \"{Path.GetFileName(path)}\"\n{e.Message}";
                ShowMessage(msg, Path.GetFileName(path));
            }
            catch (SystemExitException e)
            {
                string msg = $"SystemExit in \"{Path.GetFileName(path)}\"\n{e.Message}";
                ShowMessage(msg, Path.GetFileName(path));
            }

            catch (Exception e)
            {
                if (e.Message.Contains("BrawlBox") ||
                    e.Message.Contains("bboxapi"))
                {
                    ConvertPlugin(path);
                    CreatePlugin(path, loader);
                }
                else
                {
                    string msg = $"Error loading plugin or loader \"{Path.GetFileName(path)}\"\n{e.Message}";
                    ShowMessage(msg, Path.GetFileName(path));
                }
            }
            return false;
        }
        internal static void ConvertPlugin(string path)
        {
            string text = File.ReadAllText(path);
            text = text.Replace("BrawlBox", "BrawlCrate");
            text = text.Replace("bboxapi", "BrawlAPI");
            File.WriteAllText(path, text);
        }

        internal static bool FSharpInstall(out string fsi_path)
        {
            fsi_path =
                    new[] {
                        ".",
                        Environment.GetEnvironmentVariable("ProgramFiles"),
                        Environment.GetEnvironmentVariable("ProgramFiles(x86)")
                    }
                    .Where(s => s != null)
                    .SelectMany(s => new[] {
                        Path.Combine(s, "Microsoft SDKs", "F#"),
                        Path.Combine(s, "Microsoft Visual Studio")
                    })
                    .SelectMany(dir => Directory.Exists(dir)
                        ? Directory.GetFiles(dir, "fsi.exe", SearchOption.AllDirectories)
                        : new string[0])
                    .FirstOrDefault(s => File.Exists(s));
            if (fsi_path == null)
            {
                if (DialogResult.OK == MessageBox.Show("F# Interactive (fsi.exe) was not found. Would you like to install the Build Tools for Visual Studio? You may have to restart the program for changes to take effect.", "BrawlAPI", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    Process.Start("https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2017");
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void ResourceTree_SelectionChanged(object sender, EventArgs e)
        {
            TreeView resourceTree = (TreeView)sender;
            if ((resourceTree.SelectedNode is BaseWrapper))
            {
                BaseWrapper wrapper = (BaseWrapper)resourceTree.SelectedNode;
                Type type = wrapper.GetType();

                if (ContextMenuHooks.ContainsKey(type) && ContextMenuHooks[type].Length > 0)
                {
                    if (wrapper.ContextMenuStrip.Items.Count == 0 || wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1].Text != "Plugins")
                    {
                        if (wrapper.ContextMenuStrip.Items.Count != 0)
                        {
                            wrapper.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                        }
                        wrapper.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Plugins"));
                    }
                    (wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1] as ToolStripMenuItem).DropDown.Items.AddRange(ContextMenuHooks[type]);
                }
            }
        }

        #region Exposed API members
        public static ResourceNode RootNode
        {
            get
            {
                if (MainForm.Instance.RootNode != null)
                {
                    return MainForm.Instance.RootNode.Resource;
                }
                else
                {
                    return null;
                }
            }
        }
        public static ResourceNode SelectedNode => ((BaseWrapper)MainForm.Instance.resourceTree.SelectedNode).Resource;
        public static BaseWrapper SelectedNodeWrapper => (BaseWrapper)MainForm.Instance.resourceTree.SelectedNode;

        public static void ShowMessage(string msg, string title)
        {
            MessageBox.Show(msg, title);
        }

        public static bool? ShowYesNoPrompt(string msg, string title)
        {
            DialogResult result = MessageBox.Show(msg, title, MessageBoxButtons.YesNo);
            return result == DialogResult.Yes;
        }

        public static bool? ShowOKCancelPrompt(string msg, string title)
        {
            DialogResult result = MessageBox.Show(msg, title, MessageBoxButtons.OKCancel);
            return result == DialogResult.OK;
        }

        public static void AddLoader(PluginLoader loader)
        {
            Loaders.Add(loader);
        }

        public static void AddContextMenuItem(Type wrapper, params ToolStripMenuItem[] items)
        {
            if (ContextMenuHooks.ContainsKey(wrapper))
            {
                ContextMenuHooks[wrapper].Append(items);
            }
            else
            {
                ContextMenuHooks.Add(wrapper, items);
            }
        }

        public static string OpenFileDialog()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string OpenFolderDialog()
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.SelectedPath;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string SaveFileDialog()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrawlCrate.NodeWrappers;
using BrawlCrate.Properties;
using BrawlLib.SSBB.ResourceNodes;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace BrawlCrate.API
{
    public static class BrawlAPI
    {
        private static string fsi_path;

        static BrawlAPI()
        {
            ContextMenuHooks = new Dictionary<Type, ToolStripMenuItem[]>();
            Plugins = new List<PluginScript>();
            Loaders = new List<PluginLoader>();
            Engine = Python.CreateEngine();
            Runtime = Engine.Runtime;

            // Setup IronPython engine
            PythonInstall();

            fsi_path = Settings.Default.FSharpInstallationPath;

            //Import BrawlCrate and Brawllib
            var mainAssembly = Assembly.GetExecutingAssembly();
            var brawllib = Assembly.GetAssembly(typeof(ResourceNode));

            Runtime.LoadAssembly(mainAssembly);
            Runtime.LoadAssembly(brawllib);
            Runtime.LoadAssembly(typeof(string).Assembly);
            Runtime.LoadAssembly(typeof(Uri).Assembly);
            Runtime.LoadAssembly(typeof(Form).Assembly);

            // Hook the main form's resourceTree selection changed event to add contextMenu items to nodewrapper
            MainForm.Instance.resourceTree.SelectionChanged += ResourceTree_SelectionChanged;
        }

        public static bool PythonEnabled => Engine.GetSearchPaths().Count > 0;

        public static bool FSharpEnabled => Environment.OSVersion.Platform.ToString()
                                                .StartsWith("win", StringComparison.OrdinalIgnoreCase) &&
                                            fsi_path != null && fsi_path != "" && !fsi_path.Equals("(none)",
                                                StringComparison.OrdinalIgnoreCase);

        public static string PythonPath
        {
            get => Engine.GetSearchPaths().Count > 0 ? Engine.GetSearchPaths().ElementAt(0) : "(none)";
            set
            {
                Engine.SetSearchPaths(Engine.GetSearchPaths().Append(value).ToArray());
                Settings.Default.PythonInstallationPath = value;
                Settings.Default.Save();
            }
        }

        public static string FSIPath
        {
            get => fsi_path;
            set
            {
                fsi_path = value;
                Settings.Default.FSharpInstallationPath = value;
                Settings.Default.Save();
            }
        }

        internal static ScriptEngine Engine { get; set; }
        internal static ScriptRuntime Runtime { get; set; }

        internal static List<PluginScript> Plugins { get; set; }
        internal static List<PluginLoader> Loaders { get; set; }

        internal static Dictionary<Type, ToolStripMenuItem[]> ContextMenuHooks { get; set; }

        internal static void RunScript(string path)
        {
            if (Path.GetExtension(path).Equals(".fsx", StringComparison.OrdinalIgnoreCase))
            {
                FSharpInstall();
                if (FSharpEnabled)
                {
                    var tempPath = Path.Combine(Path.GetTempPath(), $"BrawlCrate-{Guid.NewGuid()}.fsx");
                    using (var srIn = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                    using (var swOut = new StreamWriter(new FileStream(tempPath, FileMode.Create, FileAccess.Write)))
                    {
                        swOut.WriteLine(
                            $"#r \"{Assembly.GetAssembly(typeof(NodeFactory)).Location.Replace('\\', '/')}\"");
                        swOut.WriteLine($"#r \"{Assembly.GetAssembly(typeof(MainForm)).Location.Replace('\\', '/')}\"");
                        string line;
                        while ((line = srIn.ReadLine()) != null) swOut.WriteLine(line);
                    }

                    var p = Process.Start(new ProcessStartInfo
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
                            var err = p.StandardError.ReadToEnd().Trim();
                            if (!string.IsNullOrWhiteSpace(err))
                                MessageBox.Show(err);
                            else
                                MessageBox.Show($"fsi.exe quit with exit code {p.ExitCode}");
                        }
                    };
                }
                else
                {
                    MessageBox.Show(
                        "F# was not found to be properly installed on this system. Please install F# and try again.",
                        "BrawlAPI");
                }
            }
            else if (PythonEnabled)
            {
                try
                {
                    var script = Engine.CreateScriptSourceFromFile(path);
                    var code = script.Compile();
                    var scope = Engine.CreateScope();
                    script.Execute();
                }
                catch (SyntaxErrorException e)
                {
                    var msg = $"Syntax error in \"{Path.GetFileName(path)}\"\n{e.Message}";
                    ShowMessage(msg, Path.GetFileName(path));
                }
                catch (SystemExitException e)
                {
                    var msg = $"SystemExit in \"{Path.GetFileName(path)}\"\n{e.Message}";
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
                        var msg = $"Error running script \"{Path.GetFileName(path)}\"\n{e.Message}";
                        ShowMessage(msg, Path.GetFileName(path));
                    }
                }
            }
        }

        internal static bool CreatePlugin(string path, bool loader)
        {
            try
            {
                if (path.EndsWith(".fsx", StringComparison.OrdinalIgnoreCase)) FSharpInstall();
                if (path.EndsWith(".py", StringComparison.OrdinalIgnoreCase) && !PythonEnabled ||
                    path.EndsWith(".fsx", StringComparison.OrdinalIgnoreCase) && !FSharpEnabled) return false;
                var script = Engine.CreateScriptSourceFromFile(path);
                var code = script.Compile();
                var scope = Engine.CreateScope();
                if (!loader)
                    Plugins.Add(new PluginScript(Path.GetFileNameWithoutExtension(path), script, scope));
                else
                    script.Execute();
                return true;
            }
            catch (SyntaxErrorException e)
            {
                var msg = $"Syntax error in \"{Path.GetFileName(path)}\"\n{e.Message}";
                ShowMessage(msg, Path.GetFileName(path));
            }
            catch (SystemExitException e)
            {
                var msg = $"SystemExit in \"{Path.GetFileName(path)}\"\n{e.Message}";
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
                    var msg = $"Error loading plugin or loader \"{Path.GetFileName(path)}\"\n{e.Message}";
                    ShowMessage(msg, Path.GetFileName(path));
                }
            }

            return false;
        }

        internal static void ConvertPlugin(string path)
        {
            var text = File.ReadAllText(path);
            text = text.Replace("BrawlBox", "BrawlCrate");
            text = text.Replace("bboxapi", "BrawlAPI");
            File.WriteAllText(path, text);
        }

        public static void PythonInstall(bool manual = false, bool force = false)
        {
            var settingPath = Settings.Default.PythonInstallationPath;
            var searchPaths = new List<string>();

            // First, search the directory found in the settings (unless force is active)
            if (!force && !settingPath.Equals("") && Directory.Exists(settingPath))
            {
                if (Directory.Exists(settingPath + "\\Lib"))
                    searchPaths.Add($"{settingPath}\\Lib");
                else
                    searchPaths.Add(settingPath);
            }
            // Then check for Python 2.7 (The recommended version for iron python) in its default installation directory
            else if (Directory.Exists("C:\\Python27\\Lib"))
            {
                searchPaths.Add("C:\\Python27\\Lib");
            }
            // Finally, search for any other Python installations in their default directories
            else
            {
                foreach (var d in Directory.CreateDirectory("C:\\").GetDirectories().Reverse())
                    if (d.FullName.StartsWith("C:\\Python") && Directory.Exists(d.FullName + "\\Lib"))
                    {
                        searchPaths.Add($"{d.FullName}\\Lib");
                        break;
                    }
            }

            // Then see if there's a directory included in the installation (This can also be used for additional modules or a primary install, so add it in addition)
            if (Directory.Exists($"{Application.StartupPath}\\Python"))
                if (!searchPaths.Contains($"{Application.StartupPath}\\Python"))
                    searchPaths.Add($"{Application.StartupPath}\\Python");

            if (force || settingPath.Equals("") || settingPath.Equals("(none)"))
            {
                if (searchPaths.Count > 0)
                {
                    if (manual)
                        MessageBox.Show(
                            "Python was found to be installed in: \n" + searchPaths[0] +
                            "\nAdditional modules can be installed in this path or by placing them in the \"Python\" folder in your BrawlCrate installation.",
                            "BrawlAPI");
                    Settings.Default.PythonInstallationPath = searchPaths[0];
                    Settings.Default.Save();
                }
                else if (!settingPath.Equals("(none)", StringComparison.OrdinalIgnoreCase))
                {
                    using (var dlg = new FolderBrowserDialog())
                    {
                        if (MessageBox.Show(
                                "Python installation could not be detected, would you like to locate it now? If Python is not installed, the plugin system will be disabled.",
                                "BrawlAPI", MessageBoxButtons.YesNo) == DialogResult.Yes
                            && dlg.ShowDialog() == DialogResult.OK)
                        {
                            searchPaths.Add(dlg.SelectedPath);
                            Settings.Default.PythonInstallationPath = dlg.SelectedPath;
                            Settings.Default.Save();
                        }
                        else if (!force)
                        {
                            MessageBox.Show(
                                "Python installation not found. Python plugins and loaders will be disabled. The python installation path can be changed in the settings.",
                                "BrawlAPI");
                            Settings.Default.PythonInstallationPath = "(none)";
                            Settings.Default.Save();
                        }
                    }
                }
            }

            // If any python installations are found, set them as the search paths
            if (searchPaths.Count > 0) Engine.SetSearchPaths(searchPaths.ToArray());
        }

        internal static void FSharpInstall(bool manual = false, bool force = false)
        {
            if (Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.OrdinalIgnoreCase) &&
                (force || fsi_path == null || fsi_path == ""))
            {
                fsi_path =
                    new[]
                        {
                            ".",
                            Environment.GetEnvironmentVariable("ProgramFiles"),
                            Environment.GetEnvironmentVariable("ProgramFiles(x86)")
                        }
                        .Where(s => s != null)
                        .SelectMany(s => new[]
                        {
                            Path.Combine(s, "Microsoft SDKs", "F#"),
                            Path.Combine(s, "Microsoft Visual Studio")
                        })
                        .SelectMany(dir => Directory.Exists(dir)
                            ? Directory.GetFiles(dir, "fsi.exe", SearchOption.AllDirectories)
                            : new string[0])
                        .FirstOrDefault(s => File.Exists(s));
                if (fsi_path == null)
                {
                    if (DialogResult.OK == MessageBox.Show(
                            "F# Interactive (fsi.exe) was not found. Would you like to install the Build Tools for Visual Studio? You may have to restart the program for changes to take effect.",
                            "BrawlAPI", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                        Process.Start(
                            "https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2017");
                    else
                        fsi_path = "(none)";
                }

                if (fsi_path != null && fsi_path != "")
                {
                    Settings.Default.FSharpInstallationPath = fsi_path;
                    Settings.Default.Save();
                    if (manual) MessageBox.Show("F# was found to be installed in:\n" + fsi_path);
                }
            }
        }

        private static void ResourceTree_SelectionChanged(object sender, EventArgs e)
        {
            var resourceTree = (TreeView) sender;
            if (resourceTree.SelectedNode is BaseWrapper)
            {
                var wrapper = (BaseWrapper) resourceTree.SelectedNode;
                var type = wrapper.GetType();

                if (ContextMenuHooks.ContainsKey(type) && ContextMenuHooks[type].Length > 0)
                {
                    if (wrapper.ContextMenuStrip.Items.Count == 0 ||
                        wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1].Text != "Plugins")
                    {
                        if (wrapper.ContextMenuStrip.Items.Count != 0)
                            wrapper.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                        wrapper.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Plugins"));
                    }

                    (wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1] as ToolStripMenuItem)
                        .DropDown.Items.AddRange(ContextMenuHooks[type]);
                }
            }
        }

        #region Exposed API members

        public static ResourceNode RootNode
        {
            get
            {
                if (MainForm.Instance.RootNode != null)
                    return MainForm.Instance.RootNode.Resource;
                return null;
            }
        }

        public static ResourceNode SelectedNode => ((BaseWrapper) MainForm.Instance.resourceTree.SelectedNode).Resource;
        public static BaseWrapper SelectedNodeWrapper => (BaseWrapper) MainForm.Instance.resourceTree.SelectedNode;

        public static void ShowMessage(string msg, string title)
        {
            MessageBox.Show(msg, title);
        }

        public static bool? ShowYesNoPrompt(string msg, string title)
        {
            var result = MessageBox.Show(msg, title, MessageBoxButtons.YesNo);
            return result == DialogResult.Yes;
        }

        public static bool? ShowOKCancelPrompt(string msg, string title)
        {
            var result = MessageBox.Show(msg, title, MessageBoxButtons.OKCancel);
            return result == DialogResult.OK;
        }

        public static void AddLoader(PluginLoader loader)
        {
            Loaders.Add(loader);
        }

        public static void AddContextMenuItem(Type wrapper, params ToolStripMenuItem[] items)
        {
            if (ContextMenuHooks.ContainsKey(wrapper))
                ContextMenuHooks[wrapper].Append(items);
            else
                ContextMenuHooks.Add(wrapper, items);
        }

        public static string OpenFileDialog()
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.FileName;
                return string.Empty;
            }
        }

        public static string OpenFolderDialog()
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.SelectedPath;
                return string.Empty;
            }
        }

        public static string SaveFileDialog()
        {
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.FileName;
                return string.Empty;
            }
        }

        #endregion
    }
}
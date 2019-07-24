using BrawlCrate.NodeWrappers;
using BrawlLib.SSBB.ResourceNodes;
using IronPython.Hosting;
using IronPython.Modules;
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
    public static partial class BrawlAPI
    {
        public static bool PythonEnabled => Engine.GetSearchPaths().Count > 0;

        public static bool FSharpEnabled =>
            Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrEmpty(fsi_path) && !fsi_path.Equals("(none)", StringComparison.OrdinalIgnoreCase);

        private static string fsi_path;

        public static string PythonPath
        {
            get => Engine.GetSearchPaths().Count > 0 ? Engine.GetSearchPaths().ElementAt(0) : "(none)";
            set
            {
                Engine.SetSearchPaths(Engine.GetSearchPaths().Append(value).ToArray());
                Properties.Settings.Default.PythonInstallationPath = value;
                Properties.Settings.Default.Save();
            }
        }

        public static string FSIPath
        {
            get => fsi_path;
            set
            {
                fsi_path = value;
                Properties.Settings.Default.FSharpInstallationPath = value;
                Properties.Settings.Default.Save();
            }
        }

        static BrawlAPI()
        {
            ContextMenuHooks = new Dictionary<Type, ToolStripMenuItem[]>();
            Plugins = new List<PluginScript>();
            Loaders = new List<PluginLoader>();
            Engine = Python.CreateEngine();
            Runtime = Engine.Runtime;

            // Setup IronPython engine
            PythonInstall();

            fsi_path = Properties.Settings.Default.FSharpInstallationPath;

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
            if (!string.IsNullOrEmpty(path) &&
                Path.GetExtension(path).Equals(".fsx", StringComparison.OrdinalIgnoreCase))
            {
                FSharpInstall();
                if (FSharpEnabled)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), $"BrawlCrate-{Guid.NewGuid()}.fsx");
                    using (StreamReader srIn = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                    {
                        using (StreamWriter swOut =
                            new StreamWriter(new FileStream(tempPath, FileMode.Create, FileAccess.Write)))
                        {
                            swOut.WriteLine(
                                $"#r \"{Assembly.GetAssembly(typeof(NodeFactory)).Location.Replace('\\', '/')}\"");
                            swOut.WriteLine(
                                $"#r \"{Assembly.GetAssembly(typeof(MainForm)).Location.Replace('\\', '/')}\"");
                            string line;
                            while ((line = srIn.ReadLine()) != null)
                            {
                                swOut.WriteLine(line);
                            }
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
                    MessageBox.Show(
                        "F# was not found to be properly installed on this system. Please install F# and try again.",
                        "BrawlAPI");
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
                    MessageBox.Show(msg, Path.GetFileName(path));
                }
                catch (SystemExitException e)
                {
                    string msg = $"SystemExit in \"{Path.GetFileName(path)}\"\n{e.Message}";
                    MessageBox.Show(msg, Path.GetFileName(path));
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
                        MessageBox.Show(msg, Path.GetFileName(path));
                    }
                }
            }
        }

        internal static bool CreatePlugin(string path, bool loader)
        {
            try
            {
                if (path.EndsWith(".fsx", StringComparison.OrdinalIgnoreCase))
                {
                    FSharpInstall();
                }

                if (path.EndsWith(".py", StringComparison.OrdinalIgnoreCase) && !PythonEnabled ||
                    path.EndsWith(".fsx", StringComparison.OrdinalIgnoreCase) && !FSharpEnabled)
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
                MessageBox.Show(msg, Path.GetFileName(path));
            }
            catch (SystemExitException e)
            {
                string msg = $"SystemExit in \"{Path.GetFileName(path)}\"\n{e.Message}";
                MessageBox.Show(msg, Path.GetFileName(path));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("BrawlBox") ||
                    e.Message.Contains("bboxapi"))
                {
                    ConvertPlugin(path);
                    return CreatePlugin(path, loader);
                }

                string msg = $"Error loading plugin or loader \"{Path.GetFileName(path)}\"\n{e.Message}";
                MessageBox.Show(msg, Path.GetFileName(path));
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

        public static void PythonInstall(bool manual = false, bool force = false)
        {
            string settingPath = Properties.Settings.Default.PythonInstallationPath;
            List<string> searchPaths = new List<string>();

            // First, search the directory found in the settings (unless force is active)
            if (!force && !settingPath.Equals("") && Directory.Exists(settingPath))
            {
                searchPaths.Add(Directory.Exists($"{settingPath}\\Lib") ? $"{settingPath}\\Lib" : settingPath);
            }
            // Then check for Python 2.7 (The recommended version for iron python) in its default installation directory
            else if (Directory.Exists("C:\\Python27\\Lib"))
            {
                searchPaths.Add("C:\\Python27\\Lib");
            }
            // Finally, search for any other Python installations in their default directories
            else
            {
                // Search the new installation path for Python
                foreach (DirectoryInfo d in Directory
                    .CreateDirectory(Environment.SpecialFolder.ApplicationData.ToString()).GetDirectories().Reverse())
                {
                    if (d.FullName.StartsWith(
                            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Python") &&
                        Directory.Exists($"{d.FullName}\\Lib"))
                    {
                        searchPaths.Add($"{d.FullName}\\Lib");
                        break;
                    }
                }

                // Search the old installation path for Python
                if (searchPaths.Count == 0)
                {
                    foreach (DirectoryInfo d in Directory.CreateDirectory("C:\\").GetDirectories().Reverse())
                    {
                        if (d.FullName.StartsWith("C:\\Python") && Directory.Exists($"{d.FullName}\\Lib"))
                        {
                            searchPaths.Add($"{d.FullName}\\Lib");
                            break;
                        }
                    }
                }
            }

            // Then see if there's a directory included in the installation (This can also be used for additional modules or a primary install, so add it in addition)
            if (Directory.Exists($"{Application.StartupPath}\\Python"))
            {
                if (!searchPaths.Contains($"{Application.StartupPath}\\Python"))
                {
                    searchPaths.Add($"{Application.StartupPath}\\Python");
                }
            }

            if (force || string.IsNullOrEmpty(settingPath) || settingPath.Equals("(none)"))
            {
                if (searchPaths.Count > 0)
                {
                    if (manual)
                    {
                        MessageBox.Show(
                            "Python was found to be installed in: \n" + searchPaths[0] +
                            "\nAdditional modules can be installed in this path or by placing them in the \"Python\" folder in your BrawlCrate installation.",
                            "BrawlAPI");
                    }

                    Properties.Settings.Default.PythonInstallationPath = searchPaths[0];
                    Properties.Settings.Default.Save();
                }
                else if (force || !settingPath.Equals("(none)", StringComparison.OrdinalIgnoreCase))
                {
                    if (force && manual)
                    {
                        if (string.IsNullOrEmpty(settingPath))
                        {
                            Properties.Settings.Default.PythonInstallationPath = "(none)";
                            Properties.Settings.Default.Save();
                        }

                        MessageBox.Show(
                            "Python installation could not be automatically detected. Install the latest version of Python 2.7 and try again, or browse manually to your Python installation directory.",
                            "BrawlAPI");
                    }
                    else
                    {
                        using (Ookii.Dialogs.VistaFolderBrowserDialog dlg = new Ookii.Dialogs.VistaFolderBrowserDialog())
                        {
                            if (MessageBox.Show(
                                    "Python installation could not be detected, would you like to locate it now? If Python is not installed, the plugin system will be disabled.",
                                    "BrawlAPI", MessageBoxButtons.YesNo) == DialogResult.Yes
                                && dlg.ShowDialog() == DialogResult.OK)
                            {
                                searchPaths.Add(dlg.SelectedPath);
                                Properties.Settings.Default.PythonInstallationPath = dlg.SelectedPath;
                                Properties.Settings.Default.Save();
                            }
                            else if (!force)
                            {
                                MessageBox.Show(
                                    "Python installation not found. Python plugins and loaders will be disabled. The python installation path can be changed in the settings.",
                                    "BrawlAPI");
                                Properties.Settings.Default.PythonInstallationPath = "(none)";
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                }
            }

            // If any python installations are found, set them as the search paths
            Engine.SetSearchPaths(searchPaths.ToArray());
        }

        internal static void FSharpInstall(bool manual = false, bool force = false)
        {
            fsi_path = Properties.Settings.Default.FSharpInstallationPath;
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
                    {
                        Process.Start(
                            "https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2017");
                    }
                    else
                    {
                        fsi_path = "(none)";
                    }
                }

                if (!string.IsNullOrEmpty(fsi_path))
                {
                    Properties.Settings.Default.FSharpInstallationPath = fsi_path;
                    Properties.Settings.Default.Save();
                    if (manual)
                    {
                        MessageBox.Show("F# was found to be installed in:\n" + fsi_path);
                    }
                }
            }
        }

        private static void ResourceTree_SelectionChanged(object sender, EventArgs e)
        {
            TreeView resourceTree = (TreeView) sender;
            if (resourceTree.SelectedNode is BaseWrapper wrapper && wrapper.ContextMenuStrip != null)
            {
                // Remove plugins list as necessary
                while (wrapper.ContextMenuStrip != null && wrapper.ContextMenuStrip.Items.Count > 0 &&
                       (wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1].Text == "Plugins" ||
                        wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1] is ToolStripSeparator))
                {
                    wrapper.ContextMenuStrip.Items.RemoveAt(wrapper.ContextMenuStrip.Items.Count - 1);
                }

                Type type = wrapper.GetType();

                if (ContextMenuHooks.ContainsKey(type) && ContextMenuHooks[type].Length > 0)
                {
                    foreach (ToolStripMenuItem item in ContextMenuHooks[type])
                    {
                        // Toggle enabled state to activate the "EnabledChanged" event. This will allow conditionals to evaluate
                        item.Enabled = false;
                        item.Enabled = true;

                        // Implementation only allows for single-nested dropdowns, so ensure those get properly initialized as well
                        if (item.DropDownItems.Count > 0)
                        {
                            foreach (ToolStripMenuItem i in item.DropDownItems)
                            {
                                i.Enabled = false;
                                i.Enabled = true;
                                i.Visible = i.Enabled;
                            }
                        }
                    }

                    List<ToolStripItem> items = new List<ToolStripItem>();
                    foreach (ToolStripMenuItem item in ContextMenuHooks[type])
                    {
                        if (item.DropDownItems.Count > 0)
                        {
                            item.Enabled = item.HasDropDownItems;
                        }

                        if (item.Enabled)
                        {
                            items.Add(item);
                        }
                    }

                    if (items.Count <= 0)
                    {
                        return;
                    }

                    if (wrapper.ContextMenuStrip.Items.Count == 0 ||
                        wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1].Text != "Plugins")
                    {
                        if (wrapper.ContextMenuStrip.Items.Count != 0)
                        {
                            wrapper.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                        }

                        wrapper.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Plugins"));
                    }

                    (wrapper.ContextMenuStrip.Items[wrapper.ContextMenuStrip.Items.Count - 1] as ToolStripMenuItem)
                        ?.DropDown.Items.AddRange(items.ToArray());
                }
            }
        }
    }
}
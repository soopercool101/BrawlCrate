using BrawlLib.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace BrawlCrate.SongManager
{
    public static class Program
    {
        private static MainForm form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "--help" || args[0] == "/c")
                {
                    Console.WriteLine(BSMHelp());
                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string dir = null;
            bool loadNames = true, loadBrstms = true, groupSongs = false;
            foreach (string arg in args)
            {
                if (arg == "/n")
                {
                    loadNames = true;
                }
                else if (arg == "/N")
                {
                    loadNames = false;
                }
                else if (arg == "/b")
                {
                    loadBrstms = true;
                }
                else if (arg == "/B")
                {
                    loadBrstms = false;
                }
                else if (arg == "/g")
                {
                    groupSongs = true;
                }
                else if (arg == "/G")
                {
                    groupSongs = false;
                }
                else if (new DirectoryInfo(arg).Exists)
                {
                    dir = arg;
                }
            }

            if (dir == null)
            {
                dir = Directory.GetCurrentDirectory();
            }

            try
            {
                accessLibraries();
            }
            catch (Exception e)
            {
                MessageBox.Show(null, e.Message, e.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            form = new MainForm(dir, loadNames, loadBrstms, groupSongs);
            Application.Run(form);
        }

        private static void accessLibraries()
        {
            Settings D = Settings.Default;
            if (D.GetType().GetProperty("HideMDL0Errors") != null)
            {
                D.GetType().InvokeMember("HideMDL0Errors", System.Reflection.BindingFlags.SetProperty, null, D,
                    new object[] {true});
            }
        }

        private static string BSMHelp()
        {
            return "Usage: " + Process.GetCurrentProcess().ProcessName + " [args] [path to folder with BRSTMs]\n" +
                   "\n" +
                   "Arguments:\n" +
                   "  /N  Don't load names from info.pac\n" +
                   "  /B  Don't load BRSTMs in audio player\n" +
                   "  /g  Group songs by stage (SSBB)\n" +
                   "        This option will list only songs that are present in Brawl. BRSTMS that\n" +
                   "        exist in the folder will be marked with an asterisk (*).";
        }
    }
}
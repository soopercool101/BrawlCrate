using BrawlLib.Properties;
using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace BrawlCrate.StageManager
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

            try
            {
                accessLibraries();
            }
            catch (Exception e)
            {
                MessageBox.Show(null, e.Message, e.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string dir = null;
            bool useRelDescription = true;
            foreach (string arg in args)
            {
                if (arg == "/d")
                {
                    useRelDescription = true;
                }
                else if (arg == "/D")
                {
                    useRelDescription = false;
                }
                else if (new DirectoryInfo(arg).Exists)
                {
                    dir = arg;
                }
            }

            form = new MainForm(dir, useRelDescription);
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
            return "Usage: " + Process.GetCurrentProcess().ProcessName + " [args] [path to stage/melee folder]\n" +
                   "\n" +
                   "Arguments:\n" +
                   "  /d  Show .rel description (default)\n" +
                   "  /D  Show .rel original filename";
        }
    }
}
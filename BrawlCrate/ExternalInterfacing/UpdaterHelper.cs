using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlCrate.ExternalInterfacing
{
    public static class UpdaterHelper
    {
        #region Updater
        
        public static async Task CheckUpdate(bool overwrite)
        {
            await RunWithArgs(overwrite ? "-r" : "-n");
        }

        public static async Task CheckUpdate(bool overwrite, string releaseTag, bool manual, string openFile,
                                             bool checkDocumentation, bool automatic, bool checkForAPI)
        {
            await RunWithArgs("-bu", overwrite ? "1" : "0", releaseTag, manual ? "1" : "0", openFile,
                checkDocumentation ? "1" : "0", automatic ? "1" : "0", checkForAPI ? "1" : "0");
        }

        public static async Task CheckCanaryUpdate(string openFile, bool manual, bool force, bool checkForAPI)
        {
            await RunWithArgs("-buc", openFile, manual ? "1" : "0", force  ? "1" : "0", checkForAPI ? "1" : "0");
        }

        public static async Task ForceDownloadStable(string openFile)
        {
            await RunWithArgs("-dlStable", openFile);
        }
        
        public static async Task BrawlAPICheckUpdates(bool manual)
        {
            await RunWithArgs("-apiUpdate", manual ? "1" : "0");
        }

        public static async Task BrawlAPIInstallUpdate(string repoOwner, string repoName, bool manual)
        {
            await RunWithArgs("-apiInstall", repoOwner, repoName, manual ? "1" : "0");
        }

        public static async Task BrawlAPIUninstall(string repoOwner, string repoName, bool manual)
        {
            await RunWithArgs("-apiUninstall", repoOwner, repoName, manual ? "1" : "0");
        }

        #endregion

        #region Issue Reporter

        public static async Task CreateIssue(
            string TagName,
            string ExceptionMessage,
            string StackTrace,
            string Title,
            string Description)
        {
            await RunWithArgs("-bi", TagName, ExceptionMessage, StackTrace, Title, Description);
        }

        #endregion

        private static async Task RunWithArgs(params string[] args)
        {
            if (Program.CanRunGithubApp(false, out string path))
            {
                string argument = args[0];
                for(int i = 1; i < args.Length; i++)
                {
                    argument += $" \"{args[i]}\"";
                }
                
                // Run as task, ensuring that awaiting will be optional
                Task t = Task.Run(() =>
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = argument
                    })?.WaitForExit();
                });

                await t;
            }
        }
    }
}

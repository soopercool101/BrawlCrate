using System.Diagnostics;
using System.Linq;

namespace BrawlCrate.ExternalInterfacing
{
    internal static class UpdaterHelper
    {
        #region Updater

        internal static void CheckUpdate(bool waitForExit, bool overwrite)
        {
            RunWithArgs(waitForExit, overwrite ? "-r" : "-n");
        }

        internal static void CheckUpdate(bool waitForExit, bool overwrite, string releaseTag, bool manual,
                                         string openFile, bool checkDocumentation, bool automatic, bool checkForAPI)
        {
            RunWithArgs(waitForExit, "-bu", overwrite ? "1" : "0", releaseTag, manual ? "1" : "0", openFile,
                checkDocumentation ? "1" : "0", automatic ? "1" : "0", checkForAPI ? "1" : "0");
        }

        internal static void CheckCanaryUpdate(bool waitForExit, string openFile, bool manual, bool force,
                                               bool checkForAPI)
        {
            RunWithArgs(waitForExit, "-buc", openFile, manual ? "1" : "0", force ? "1" : "0", checkForAPI ? "1" : "0");
        }

        internal static void ForceDownloadStable(bool waitForExit, string openFile)
        {
            RunWithArgs(waitForExit, "-dlStable", openFile);
        }

        internal static void BrawlAPICheckUpdates(bool waitForExit, bool manual)
        {
            RunWithArgs(waitForExit, "-apiUpdate", manual ? "1" : "0");
        }

        internal static void BrawlAPIInstallUpdate(bool waitForExit, string repoOwner, string repoName, bool manual)
        {
            RunWithArgs(waitForExit, "-apiInstall", repoOwner, repoName, manual ? "1" : "0");
        }

        internal static void BrawlAPIUninstall(bool waitForExit, string repoOwner, string repoName, bool manual)
        {
            RunWithArgs(waitForExit, "-apiUninstall", repoOwner, repoName, manual ? "1" : "0");
        }

        #endregion

        #region Issue Reporter

        internal static void CreateIssue(
            bool waitForExit,
            string TagName,
            string ExceptionMessage,
            string StackTrace,
            string Title,
            string Description)
        {
            RunWithArgs(waitForExit, "-bi", TagName, ExceptionMessage, StackTrace, Title, Description);
        }

        #endregion

        private static void RunWithArgs(bool waitForExit, params string[] args)
        {
            if (Program.CanRunGithubApp(false, out string path))
            {
                string argument = args.Aggregate("", (current, arg) => current + $"\"{arg}\" ").Trim();

                // Run the process with the given arguments. 
                Process updater = Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = argument
                });
                if (waitForExit)
                {
                    updater?.WaitForExit();
                }
            }
        }
    }
}
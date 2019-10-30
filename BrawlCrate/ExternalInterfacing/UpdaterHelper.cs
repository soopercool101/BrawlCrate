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
        
        public static async Task BrawlAPICheckUpdates(bool manual)
        {
            await RunWithArgs("-apiUpdate", manual ? "1" : "0");
        }

        public static async Task BrawlAPIUpdate(string repoOwner, string repoName, bool manual)
        {
            await RunWithArgs("-apiInstall", repoOwner, repoName, manual ? "1" : "0");
        }
        
        public static async Task BrawlAPIUninstall(string repoOwner, string repoName, bool manual)
        {
            await RunWithArgs("-apiUninstall", repoOwner, repoName, manual ? "1" : "0");
        }

        #endregion

        #region Issue Reporter

        

        #endregion

        private static async Task RunWithArgs(params string[] args)
        {
            if (Program.CanRunGithubApp(false, out string path))
            {
                string argument = args[0];
                for(int i = 1; i < args.Length; i++)
                {
                    argument += " " + args[i];
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

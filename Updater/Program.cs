using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    internal class Program
    {
        private const string Usage = @"Usage: -n = New Folder";

        private static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            Application.EnableVisualStyles();

            //Prevent crash that occurs when this dll is not present
            if (!File.Exists(Application.StartupPath + "/Octokit.dll"))
            {
                MessageBox.Show("Unable to find Octokit.dll.");
                return;
            }

            bool somethingDone = true;
            Task t;
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-r": //overwrite
                        t = Updater.CheckUpdate(true);
                        t.Wait();
                        break;
                    case "-n": // Update in new folder
                        t = Updater.CheckUpdate(false);
                        t.Wait();
                        break;
                    case "-bu": //BrawlCrate update call
                        t = Updater.CheckUpdate(args[1] != "0", args[2], args[3] != "0", args[4], args[5] != "0",
                            args[6] != "0", args[7] != "0");
                        t.Wait();
                        break;
                    case "-buc": //BrawlCrate Canary update call
                        t = Updater.CheckCanaryUpdate(args.Length > 1 ? args[1] : null,
                            args.Length > 2 && args[2] != "0", false, args[3] != "0");
                        t.Wait();
                        break;
                    case "-bi": //BrawlCrate issue call
                        t = IssueReporter.CreateIssue(args[1], args[2], args[3], args[4], args[5]);
                        t.Wait();
                        break;
                    case "-bcommitTime": //Called on build to ensure time is saved
                        string t4arg1 = args.Length > 1 ? args[1] : null;
                        string t4arg2 = args.Length > 2 ? args[2] : null;
                        string t4arg3 = args.Length > 3 ? args[3] : null;
                        string t4arg4 = args.Length > 4 ? args[4] : null;

                        t = Updater.WriteCanaryTime(t4arg1, t4arg2, t4arg3, t4arg4);
                        t.Wait();
                        break;
                    case "-dlCanary": // Force download the latest Canary build
                        Updater.SetCanaryActive();
                        t = Updater.CheckCanaryUpdate(args.Length > 1 ? args[1] : null, false, true, false);
                        t.Wait();
                        break;
                    case "-dlStable": // Force download the latest Stable build
                        Updater.SetCanaryInactive();
                        t = Updater.ForceDownloadStable(args.Length > 1 ? args[1] : null);
                        t.Wait();
                        break;
                    case "-dlDoc": // Force download the latest Documentation build
                        t = Updater.ForceDownloadDocumentation();
                        t.Wait();
                        break;
                    case "-canarylog": // Show changelog for canary
                        t = Updater.ShowCanaryChangelog();
                        t.Wait();
                        break;
                    case "-canaryOn": // Activate canary build
                        Updater.SetCanaryActive();
                        break;
                    case "-canaryOff":
                        Updater.SetCanaryInactive();
                        break;
                    case "-killAll":
                        t = Updater.KillOpenWindows();
                        t.Wait();
                        break;
                    case "-apiUpdate":
                        t = Updater.BrawlAPICheckUpdates(args.Length > 1 && args[1] == "1");
                        t.Wait();
                        break;
                    case "-apiInstall":
                        t = Updater.BrawlAPIInstallUpdate(args[1], args[2], args.Length > 3 && args[3] == "1");
                        t.Wait();
                        break;
                    case "-apiUninstall":
                        Updater.BrawlAPIUninstall(args[1], args[2], args.Length > 3 && args[3] == "1");
                        break;
                    default:
                        somethingDone = false;
                        break;
                }
            }
            else if (args.Length == 0)
            {
#if CANARY
                t = Updater.CheckCanaryUpdate("", true, false, true);
#else
                t = Updater.CheckUpdate(true);
#endif
                t.Wait();
            }

            if (!somethingDone)
            {
                Console.WriteLine(Usage);
            }
        }

        internal static readonly byte[] RawData =
        {
            0x34, 0x35, 0x31, 0x30, 0x34, 0x31, 0x62, 0x38, 0x65, 0x39, 0x32, 0x64, 0x37, 0x32, 0x66, 0x62, 0x63, 0x36,
            0x38, 0x62, 0x63, 0x66, 0x61, 0x39, 0x36, 0x61, 0x32, 0x65, 0x30, 0x36, 0x64, 0x62, 0x61, 0x33, 0x62, 0x36,
            0x39, 0x32, 0x66, 0x63, 0x20
        };
    }
}
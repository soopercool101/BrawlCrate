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

            bool somethingDone = false;
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-r": //overwrite
                        somethingDone = true;
                        Task t = Updater.CheckUpdate(true);
                        t.Wait();
                        break;
                    case "-n": // Update in new folder
                        somethingDone = true;
                        Task t1 = Updater.CheckUpdate(false);
                        t1.Wait();
                        break;
                    case "-bu": //BrawlCrate update call
                        somethingDone = true;
                        Task t2 = Updater.CheckUpdate(args[1] != "0", args[2], args[3] != "0", args[4], args[5] != "0",
                            args[6] != "0");
                        t2.Wait();
                        break;
                    case "-buc": //BrawlCrate Canary update call
                        somethingDone = true;
                        Task t2c = Updater.CheckCanaryUpdate(args.Length > 1 ? args[1] : null,
                            args.Length > 2 && args[2] != "0", false);
                        t2c.Wait();
                        break;
                    case "-bi": //BrawlCrate issue call
                        somethingDone = true;
                        Task t3 = IssueReporter.CreateIssue(args[1], args[2], args[3], args[4], args[5]);
                        t3.Wait();
                        break;
                    case "-bcommitTime": //Called on build to ensure time is saved
                        somethingDone = true;
                        string t4arg1 = args.Length > 1 ? args[1] : null;
                        string t4arg2 = args.Length > 2 ? args[2] : null;
                        string t4arg3 = args.Length > 3 ? args[3] : null;
                        string t4arg4 = args.Length > 4 ? args[4] : null;

                        Task t4 = Updater.WriteCanaryTime(t4arg1, t4arg2, t4arg3, t4arg4);
                        t4.Wait();
                        break;
                    case "-dlCanary": // Force download the latest Canary build
                        somethingDone = true;
                        Task t5a = Updater.SetCanaryActive();
                        t5a.Wait();
                        Task t5 = Updater.CheckCanaryUpdate(args.Length > 1 ? args[1] : null, false, true);
                        t5.Wait();
                        break;
                    case "-dlStable": // Force download the latest Stable build
                        somethingDone = true;
                        Task t6a = Updater.SetCanaryInactive();
                        t6a.Wait();
                        Task t6 = Updater.ForceDownloadStable(args.Length > 1 ? args[1] : null);
                        t6.Wait();
                        break;
                    case "-dlDoc": // Force download the latest Documentation build
                        somethingDone = true;
                        Task t6d = Updater.ForceDownloadDocumentation();
                        t6d.Wait();
                        break;
                    case "-canarylog": // Show changelog for canary
                        somethingDone = true;
                        Task t7 = Updater.ShowCanaryChangelog();
                        t7.Wait();
                        break;
                    case "-canaryOn": // Activate canary build
                        somethingDone = true;
                        Task t8 = Updater.SetCanaryActive();
                        t8.Wait();
                        break;
                    case "-canaryOff":
                        somethingDone = true;
                        Task t9 = Updater.SetCanaryInactive();
                        t9.Wait();
                        break;
                    case "-killAll":
                        somethingDone = true;
                        Task t10 = Updater.KillOpenWindows();
                        t10.Wait();
                        break;
                }
            }
            else if (args.Length == 0)
            {
                somethingDone = true;
#if CANARY
                Task t = Updater.CheckCanaryUpdate("", true, false);
#else
                Task t = Updater.CheckUpdate(true);
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
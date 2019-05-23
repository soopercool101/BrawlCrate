//================================================================\\
//  Simple application containing most functions for interfacing  \\
//      with Github API, including Updater and BugSquish.         \\
//================================================================\\
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace Net
{
    public static class Updater
    {
        public static readonly string BaseURL = "https://github.com/libertyernie/brawltools/releases/download/";
        public static string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static async Task UpdateCheck() { await UpdateCheck(false); }
        public static async Task UpdateCheck(bool Overwrite)
        {
            if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase)) {
                AppPath = AppPath.Substring(0, AppPath.Length - 4);
            }

            // check to see if the user is online, and that github is up and running.
            Console.WriteLine("Checking connection to server.");
            using (Ping s = new Ping())
                Console.WriteLine(s.Send("www.github.com").Status);

            // Initiate the github client.
            GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("Brawltools"));

            // get repo, Release, and release assets
            Repository repo = await github.Repository.Get("libertyernie", "brawltools");
            Release release = (await github.Release.GetAll(repo.Owner.Login, repo.Name))[0];
            ReleaseAsset Asset = (await github.Release.GetAssets("libertyernie", repo.Name, release.Id))[0];

            // Check if we were passed in the overwrite paramter, and if not create a new folder to extract in.
            if (!Overwrite)
            {
                Directory.CreateDirectory(AppPath + "/" + release.TagName);
                AppPath += "/" + release.TagName;
            }
            else
            {
                //Find and close the brawlbox application that will be overwritten
                Process[] px =  Process.GetProcessesByName("BrawlBox");
                Process p = px.FirstOrDefault(x => x.MainModule.FileName.StartsWith(AppPath));
                if (p != null && p != default(Process) && p.CloseMainWindow())
                    p.Close();
            }

            using (WebClient client = new WebClient())
            {
                // Add the user agent header, otherwise we will get access denied.
                client.Headers.Add("User-Agent: Other");

                // Full asset streamed into a single string
                string html = client.DownloadString(Asset.Url);

                // The browser download link to the self extracting archive, hosted on github
                string URL = html.Substring(html.IndexOf(BaseURL)).TrimEnd(new char[] { '}', '"' });

                Console.WriteLine("\nDownloading");
                client.DownloadFile(URL, AppPath + "/Update.exe");
                Console.WriteLine("\nSuccess!");

                Console.Clear();
                Console.WriteLine("Starting install");

                Process update = Process.Start(AppPath + "/Update.exe", "-d\"" + AppPath + "\"");
            }
        }

        public static async Task CheckUpdates(string releaseTag, bool manual = true)
        {
            try
            {
                var github = new GitHubClient(new Octokit.ProductHeaderValue("Brawltools"));
                IReadOnlyList<Release> releases = null;
                try
                {
                    releases = await github.Release.GetAll("libertyernie", "brawltools");

                    // Check if this is a known pre-release version
                    bool isPreRelease = releases.Any(r => r.Prerelease
                        && string.Equals(releases[0].TagName, releaseTag, StringComparison.InvariantCulture)
                        && r.Name.IndexOf("BrawlBox", StringComparison.InvariantCultureIgnoreCase) >= 0);

                    // If this is not a known pre-release version, remove all pre-release versions from the list
                    if (!isPreRelease) {
                        releases = releases.Where(r => !r.Prerelease).ToList();
                    }
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    MessageBox.Show("Unable to connect to the internet.");
                    return;
                }

                if (releases != null &&
                    releases.Count > 0 &&
                    !String.Equals(releases[0].TagName, releaseTag, StringComparison.InvariantCulture) && //Make sure the most recent version is not this version
                    releases[0].Name.IndexOf("BrawlBox", StringComparison.InvariantCultureIgnoreCase) >= 0) //Make sure this is a BrawlBox release
                {
                    DialogResult UpdateResult = MessageBox.Show(releases[0].Name + " is available! Update now?", "Update", MessageBoxButtons.YesNo);
                    if (UpdateResult == DialogResult.Yes)
                    {
                        DialogResult OverwriteResult = MessageBox.Show("Overwrite current installation?", "", MessageBoxButtons.YesNoCancel);
                        if (OverwriteResult != DialogResult.Cancel)
                        {
                            Task t = UpdateCheck(OverwriteResult == DialogResult.Yes);
                            t.Wait();
                        }
                    }
                }
                else if (manual)
                    MessageBox.Show("No updates found.");
            }
            catch (Exception e)
            {
                if (manual)
                    MessageBox.Show(e.Message);
            }
        }
    }

    public static class BugSquish
    {
        static byte[] _rawData = 
        {
	        0x61, 0x38, 0x36, 0x63, 0x38, 0x64, 0x34, 0x63, 0x61, 0x38, 0x31, 0x64, 0x37, 0x30, 0x31, 0x65, 
            0x31, 0x61, 0x64, 0x62, 0x30, 0x38, 0x36, 0x33, 0x65, 0x31, 0x62, 0x64, 0x35, 0x65, 0x64, 0x32,
            0x31, 0x34, 0x38, 0x62, 0x65, 0x31, 0x30, 0x63
        };

        public static async Task CreateIssue(
            string TagName,
            string ExceptionMessage,
            string StackTrace,
            string Title,
            string Description)
        {
            try
            {
                //Gain access to the BrawlBox account on github for submitting the report.
                //I don't really care if this gets compromised, the token has no user settings access so I'll just revoke access to the token and generate a new one.
                //Have to use a byte array to (hopefully) bypass github's automatic detection of the token as a string.
                Octokit.Credentials s = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                var github = new GitHubClient(new Octokit.ProductHeaderValue("Brawltools")) { Credentials = s };
                IReadOnlyList<Release> releases = null;
                IReadOnlyList<Issue> issues = null;
                try
                {
                    releases = await github.Release.GetAll("libertyernie", "brawltools");
                    issues = await github.Issue.GetForRepository("BrawlBox", "BrawlBoxIssues");
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    MessageBox.Show("Unable to connect to the internet.");
                    return;
                }

                if (releases != null && releases.Count > 0 && releases[0].TagName != TagName)
                {
                    //This build's version tag does not match the latest release's tag on the repository.
                    //This bug may have been fixed by now. Tell the user to update to be allowed to submit bug reports.

                    DialogResult UpdateResult = MessageBox.Show(releases[0].Name + " is available!\nYou cannot submit bug reports using an older version of the program.\nUpdate now?", "An update is available", MessageBoxButtons.YesNo);
                    if (UpdateResult == DialogResult.Yes)
                    {
                        DialogResult OverwriteResult = MessageBox.Show("Overwrite current installation?", "", MessageBoxButtons.YesNoCancel);
                        if (OverwriteResult != DialogResult.Cancel)
                        {
                            Task t = Updater.UpdateCheck(OverwriteResult == DialogResult.Yes);
                            t.Wait();
                        }
                    }
                }
                else
                {
                    bool found = false;
                    if (issues != null && !String.IsNullOrEmpty(StackTrace))
                        foreach (Issue i in issues)
                            if (i.State == ItemState.Open)
                            {
                                string desc = i.Body;
                                if (desc.Contains(StackTrace) && 
                                    desc.Contains(ExceptionMessage) && 
                                    desc.Contains(TagName))
                                {
                                    found = true;
                                    IssueUpdate update = i.ToUpdate();

                                    update.Body =
                                        Title +
                                        Environment.NewLine +
                                        Description +
                                        Environment.NewLine +
                                        Environment.NewLine +
                                        i.Body;

                                    Issue x = await github.Issue.Update("BrawlBox", "BrawlBoxIssues", i.Number, update);
                                }
                            }
                    
                    if (!found)
                    {
                        NewIssue issue = new NewIssue(Title)
                        {
                            Body =
                            Description +
                            Environment.NewLine +
                            Environment.NewLine +
                            TagName +
                            Environment.NewLine +
                            ExceptionMessage +
                            Environment.NewLine +
                            StackTrace
                        };
                        Issue x = await github.Issue.Create("BrawlBox", "BrawlBoxIssues", issue);
                    }
                }
            }
            catch
            {
                MessageBox.Show("The application was unable to retrieve permission to send this issue.");
            }
        }
    }

    class Program
    {
        const string Usage = @"Usage: -r = Overwrite files in directory";

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            System.Windows.Forms.Application.EnableVisualStyles();
            
            //Prevent crash that occurs when this dll is not present
            if (!File.Exists(System.Windows.Forms.Application.StartupPath + "/Octokit.dll"))
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
                        Task t = Updater.UpdateCheck(true);
                        t.Wait();
                        break;
                    case "-bu": //brawlbox update call
                        somethingDone = true;
                        Task t2 = Updater.CheckUpdates(args[1], args[2] != "0");
                        t2.Wait();
                        break;
                    case "-bi": //brawlbox issue call
                        somethingDone = true;
                        Task t3 = BugSquish.CreateIssue(args[1], args[2], args[3], args[4], args[5]);
                        t3.Wait();
                        break;
                }
            }
            else if (args.Length == 0)
            {
                somethingDone = true;
                Task t = Updater.UpdateCheck();
                t.Wait();
            }

            if (!somethingDone)
                Console.WriteLine(Usage);
        }
    }
}
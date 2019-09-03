using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace Updater
{
    public static class Updater
    {
        public static readonly string mainRepo = "soopercool101/BrawlCrateNext";
        public static readonly string mainBranch = "master";
        public static string currentRepo;
        public static string currentBranch;

        private static readonly byte[] _rawData =
        {
            0x34, 0x35, 0x31, 0x30, 0x34, 0x31, 0x62, 0x38, 0x65, 0x39, 0x32, 0x64, 0x37, 0x32, 0x66, 0x62, 0x63, 0x36,
            0x38, 0x62, 0x63, 0x66, 0x61, 0x39, 0x36, 0x61, 0x32, 0x65, 0x30, 0x36, 0x64, 0x62, 0x61, 0x33, 0x62, 0x36,
            0x39, 0x32, 0x66, 0x63, 0x20
        };

        public static string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static readonly GitHubClient github = new GitHubClient(new ProductHeaderValue("BrawlCrate"))
            {Credentials = new Credentials(Encoding.Default.GetString(_rawData))};

        public static string GetCurrentRepo()
        {
            try
            {
                string temp = File.ReadAllLines(AppPath + "\\Canary\\Branch")[1];
                if (temp == null || temp == "")
                {
                    throw new ArgumentNullException();
                }

                return temp;
            }
            catch
            {
                return mainRepo;
            }
        }

        public static string GetCurrentBranch()
        {
            try
            {
                string temp = File.ReadAllLines(AppPath + "\\Canary\\Branch")[0];
                if (temp == null || temp == "")
                {
                    throw new ArgumentNullException();
                }

                return temp;
            }
            catch
            {
                return mainBranch;
            }
        }

        public static async Task CheckUpdate()
        {
            await CheckUpdate(true);
        }

        // Used to check for and download non-canary releases (including documentation updates)
        public static async Task CheckUpdate(bool Overwrite, string releaseTag = "", bool manual = false,
                                             string openFile = null, bool checkDocumentation = false,
                                             bool Automatic = false)
        {
            // If canary is active, disable it
            if (File.Exists(AppPath + "\\Canary\\Active"))
            {
                File.Delete(AppPath + "\\Canary\\Active");
            }

            string repoOwner = mainRepo.Split('/')[0];
            string repoName = mainRepo.Split('/')[1];

            try
            {
                // check to see if the user is online, and that github is up and running.
                Console.Write("Checking connection to server... ");
                try
                {
                    using (Ping s = new Ping())
                    {
                        IPStatus status = s.Send("www.github.com").Status;
                        Console.WriteLine(status);
                        if (status != IPStatus.Success)
                        {
                            Console.WriteLine("Failed to connect");
                            if (manual)
                            {
                                MessageBox.Show("Unable to connect to GitHub. The website may be down.");
                            }

                            return;
                        }
                    }
                }
                catch
                {
                    throw new HttpRequestException();
                }
                // Initiate the github client.

                // get Release
                IReadOnlyList<Release> AllReleases = await github.Repository.Release.GetAll(repoOwner, repoName);
                IReadOnlyList<Release> releases = null;
                Release release = null;
                bool documentation = false;
                if (AllReleases.Count == 0)
                {
                    goto UpdateDL;
                }

                // Remove all pre-release versions from the list (Prerelease versions are exclusively documentation updates)
                releases = AllReleases.Where(r => !r.Prerelease).ToList();
                if (releases[0].TagName != releaseTag)
                {
                    release = releases[0];
                    goto UpdateDL;
                }

                if (checkDocumentation)
                {
                    // Figure out what documentation version you're on
                    string docVer = "";
                    try
                    {
                        docVer = File.ReadAllLines(AppPath + "\\InternalDocumentation\\version.txt")[0];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MessageBox.Show(
                            "ERROR: Documentation Version could not be found. Downloading the latest documentation release.");
                        await ForceDownloadDocumentation();
                        try
                        {
                            docVer = File.ReadAllLines(AppPath + "\\InternalDocumentation\\version.txt")[0];
                            // Documentation has already been updated, no need to check again.
                            checkDocumentation = false;
                        }
                        catch (Exception e2)
                        {
                            Console.WriteLine(e2.Message);
                            MessageBox.Show(
                                "ERROR: Documentation Version still could not be found. Please report this on Discord or Github.\n" +
                                e2.Message);
                        }
                    }

                    // Don't need to check for update unless the latest release is a prerelease (documentation is included in full releases)
                    if (AllReleases[0].Prerelease)
                    {
                        // This track is shared by canary updates. Ensure that a documentation release is found.
                        foreach (Release r in AllReleases)
                        {
                            if (r.TagName == releaseTag || r.TagName == docVer)
                            {
                                // Documentation is already up-to-date if the latest release (either of documentation or the full program) has been downloaded
                                release = null;
                                break;
                            }

                            if (r.Prerelease && r.Name.ToLower().Contains("documentation"))
                            {
                                release = r;
                                documentation = true;
                                break;
                            }
                        }
                    }
                }

                UpdateDL:
                // If there are no releases available, download will fail.
                if (release == null || release.Assets.Count == 0)
                {
                    if (manual)
                    {
                        MessageBox.Show("No updates found.");
                    }

                    return;
                }

                // Show warnings as applicable to those using the automatic updater
                if (Automatic)
                {
                    if (release.Body.Contains("WARNING: "))
                    {
                        if (release.Body.StartsWith("WARNING: "))
                        {
                            DialogResult dr = MessageBox.Show(
                                release.Body.Substring(0, release.Body.IndexOf("\n") - 1) +
                                "\n\nWould you like to continue updating?", "Automatic Update Warning",
                                MessageBoxButtons.YesNo);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show(
                                release.Body.Substring(release.Body.IndexOf("WARNING: ")) +
                                "\n\nWould you like to continue updating?", "Automatic Update Warning",
                                MessageBoxButtons.YesNo);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                    }

                    if (GetOpenWindowsCount() > 1 &&
                        MessageBox.Show(
                            "Update to " + release.Name +
                            " was found. Would you like to download now?\n\nAll current windows will be closed and changes will be lost!",
                            "Updater", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                else // Allow the user to choose whether or not to download a release if not using the automatic updater
                {
                    if (MessageBox.Show(
                            release.Name + " is available!\n\nThis release:\n\n" + release.Body + "\n\nUpdate now?" +
                            (manual ? "\n\nThe program will be closed, and changes will not be saved!" : ""), "Update",
                            MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                await DownloadRelease(release, Overwrite, Automatic, manual, documentation, openFile);
            }
            catch (HttpRequestException)
            {
                if (manual)
                {
                    MessageBox.Show("Unable to connect to the internet.");
                }
            }
            catch (Exception e)
            {
                if (manual)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static async Task CheckCanaryUpdate(string openFile = null, bool manual = false, bool force = false)
        {
            try
            {
                char[] slashes = {'\\', '/'};
                string[] repoData = currentRepo.Split(slashes);
                Release release =
                    await github.Repository.Release.Get(repoData[0], repoData[1], $"Canary-{currentBranch}");

                if (release == null || release.Assets.Count == 0)
                {
                    MessageBox.Show(
                        $"Error: Canary release for {currentRepo}@{currentBranch} could not be found. Update failed.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!force)
                {
                    string oldID = File.ReadAllLines(AppPath + "\\Canary\\New")[2];
                    string newID = release.TargetCommitish;
                    if (oldID.Equals(newID, StringComparison.OrdinalIgnoreCase))
                    {
                        if (manual)
                        {
                            MessageBox.Show("No updates found.");
                        }

                        return;
                    }
                }

                if ((manual || GetOpenWindowsCount() > 1) && MessageBox.Show(
                        "Update to #" + release.TargetCommitish +
                        " was found. Would you like to download now?\n\nAll current windows will be closed and changes will be lost!",
                        "Canary Updater", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                if (!File.Exists(AppPath + "\\Canary\\Old") && File.Exists(AppPath + "\\Canary\\New"))
                {
                    File.Move(AppPath + "\\Canary\\New", AppPath + "\\Canary\\Old");
                }

                await DownloadRelease(release, true, true, manual, false, openFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show(
                    "ERROR: Current Canary version could not be found. Canary has been disabled. The latest stable build will be downloaded instead.");
                await ForceDownloadStable(openFile);
            }
        }

        public static async Task DownloadRelease(Release release, bool Overwrite, bool Automatic, bool manual,
                                                 bool Documentation, string openFile)
        {
            try
            {
                string platform = null;
                try
                {
                    platform = File.ReadAllLines(AppPath + "\\Canary\\New")[5];
                }
                catch
                {
                    platform = "";
                }
                ReleaseAsset Asset = release.Assets.Any(a => a.Name.Contains(platform)) ? release.Assets.First(a => a.Name.Contains(platform)) : release.Assets[0];

                // If open windows need to be closed, ensure they are all properly closed
                if (Overwrite && !Documentation)
                {
                    await KillOpenWindows();
                }

                // Check if we were passed in the overwrite paramter, and if not create a new folder to extract in.
                if (!Overwrite)
                {
                    Directory.CreateDirectory(AppPath + "/" + release.TagName);
                    AppPath += "/" + release.TagName;
                }

                using (WebClient client = new WebClient())
                {
                    // Add the user agent header, otherwise we will get access denied.
                    client.Headers.Add("User-Agent: Other");

                    // Full asset streamed into a single string
                    string html = client.DownloadString(Asset.Url);

                    // The browser download link to the self extracting archive, hosted on github
                    string URL = html.Substring(html.IndexOf("browser_download_url\":\""))
                        .TrimEnd('}', '"');
                    URL = URL.Substring(URL.IndexOf("http"));

                    // Download the update, using a download tracker
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(
                        release.Name + (release.Name.ToLower().Contains("canary")
                            ? " #" + release.TargetCommitish.Substring(0, 7)
                            : ""), AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }

                    dlTrack.Close();
                    dlTrack.Dispose();
                }

                // If the update didn't download properly, throw an error
                if (!File.Exists(AppPath + "/temp.exe") ||
                    new FileInfo(AppPath + "/temp.exe").Length != DLProgressWindow.MaxValue ||
                    new FileInfo(AppPath + "/temp.exe").Length == 0)
                {
                    MessageBox.Show("Error downloading update");
                    if (File.Exists(AppPath + "/temp.exe"))
                    {
                        File.Delete(AppPath + "/temp.exe");
                    }

                    return;
                }

                // Case 1: Cross-platform (Batch files won't work, so user will have to ), documentation update, or non-overwriting update
                if (Process.GetProcessesByName("winlogon").Count() == 0 || Documentation || !Overwrite)
                {
                    try
                    {
                        Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\" -y");
                        // For documentation updates, ensure temp.exe is properly deleted and show changelog if the download was automated.
                        if (Documentation)
                        {
                            update?.WaitForExit();
                            if (File.Exists(AppPath + "\\temp.exe"))
                            {
                                File.Delete(AppPath + "\\temp.exe");
                            }

                            MessageBox.Show("Documentation was successfully updated to " +
                                            (release.Name.StartsWith("BrawlCrate Documentation",
                                                 StringComparison.OrdinalIgnoreCase) && release.Name.Length > 26
                                                ? release.Name.Substring(25)
                                                : release.Name) +
                                            (Automatic ? "\nThis documentation release:\n" + release.Body : ""));
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error: " + e.Message);
                    }

                    return;
                }

                // Case 2: Windows (Can use a batch file to further automate the update)
                WriteBatchScript(openFile);
                Process updateBat = Process.Start(new ProcessStartInfo
                {
                    FileName = AppPath + "/Update.bat",
                    WindowStyle = ProcessWindowStyle.Hidden
                });
            }
            catch (HttpRequestException)
            {
                if (manual)
                {
                    MessageBox.Show("Unable to connect to the internet.");
                }
            }
            catch (Exception e)
            {
                if (manual)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static async Task ForceDownloadDocumentation()
        {
            string repoOwner = mainRepo.Split('/')[0];
            string repoName = mainRepo.Split('/')[1];
            // get Release
            IReadOnlyList<Release> releases = (await github.Repository.Release.GetAll(repoOwner, repoName))
                .Where(r => r.Prerelease).ToList();
            Release release = null;

            // This track is shared by canary updates. Ensure that a documentation release is found.
            foreach (Release r in releases)
            {
                if (r.Name.ToLower().Contains("documentation"))
                {
                    release = r;
                    break;
                }
            }

            if (release != null)
            {
                await DownloadRelease(release, true, true, false, true, "<null>");
            }
        }

        public static async Task ForceDownloadStable(string openFile = null)
        {
            await SetCanaryInactive();
            string repoOwner = mainRepo.Split('/')[0];
            string repoName = mainRepo.Split('/')[1];
            // get Release
            IReadOnlyList<Release> releases = (await github.Repository.Release.GetAll(repoOwner, repoName))
                .Where(r => !r.Prerelease).ToList();
            if (releases.Count > 0)
            {
                await DownloadRelease(releases[0], true, true, false, false, openFile);
            }
        }

        public static void WriteBatchScript(string openFile)
        {
            using (StreamWriter sw = new StreamWriter(AppPath + "/Update.bat"))
            {
                sw.WriteLine("CD /d " + AppPath);

                // Mass delete relevant files (Prevents corruption)

                // Delete exes where found/applicable
                if (File.Exists(AppPath + "/BrawlCrate.exe"))
                {
                    sw.WriteLine("del BrawlCrate.exe /s /f /q");
                }

                if (File.Exists(AppPath + "/BrawlBox.exe"))
                {
                    sw.WriteLine("del BrawlBox.exe /s /f /q");
                }

                if (File.Exists(AppPath + "/BrawlScape.exe"))
                {
                    sw.WriteLine("del BrawlScape.exe /s /f /q");
                }

                if (File.Exists(AppPath + "/SmashBox.exe"))
                {
                    sw.WriteLine("del SmashBox.exe /s /f /q");
                }

                if (File.Exists(AppPath + "/StageBox.exe"))
                {
                    sw.WriteLine("del StageBox.exe /s /f /q");
                }

                if (File.Exists(AppPath + "/color_smash.exe"))
                {
                    sw.WriteLine("del color_smash.exe /s /f /q");
                }

                if (File.Exists(AppPath + "/sawndz.exe"))
                {
                    sw.WriteLine("del sawndz.exe /s /f /q");
                }

                if (File.Exists(AppPath + "/Updater.exe"))
                {
                    sw.WriteLine("del sawndz.exe /s /f /q");
                }

                // Delete DLLs where found/applicable
                if (File.Exists(AppPath + "/BrawlLib.dll"))
                {
                    sw.WriteLine("del BrawlLib.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/Octokit.dll"))
                {
                    sw.WriteLine("del Octokit.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/OpenTK.dll"))
                {
                    sw.WriteLine("del OpenTK.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/discord-rpc.dll"))
                {
                    sw.WriteLine("del discord-rpc.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/IronPython.dll"))
                {
                    sw.WriteLine("del IronPython.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/IronPython.Modules.dll"))
                {
                    sw.WriteLine("del IronPython.Modules.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/IronPython.SQLite.dll"))
                {
                    sw.WriteLine("del IronPython.SQLite.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/IronPython.Wpf.dll"))
                {
                    sw.WriteLine("del IronPython.Wpf.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/Microsoft.Dynamic.dll"))
                {
                    sw.WriteLine("del Microsoft.Dynamic.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/Microsoft.Scripting.AspNet.dll"))
                {
                    sw.WriteLine("del Microsoft.Scripting.AspNet.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/Microsoft.Scripting.dll"))
                {
                    sw.WriteLine("del Microsoft.Scripting.dll /s /f /q");
                }

                if (File.Exists(AppPath + "/Microsoft.Scripting.Metadata.dll"))
                {
                    sw.WriteLine("del Microsoft.Scripting.Metadata.dll /s /f /q");
                }

                sw.WriteLine("START /wait temp.exe -y");
                sw.WriteLine("del temp.exe /s /f /q");
                sw.Write("START BrawlCrate.exe");
                if (openFile != null && openFile != "<null>")
                {
                    sw.Write(" \"" + openFile + "\"");
                }

                sw.Close();
            }
        }

        // Used when building for releases
        public static async Task WriteCanaryTime(string commitid = null, string branchName = null, string repo = null, string platform = null)
        {
            try
            {
                if (commitid != null)
                {
                    Console.WriteLine("Attempting to set Canary using sha: " + commitid);
                }

                branchName = branchName ?? mainBranch;
                repo = repo ?? mainRepo;
                platform = platform ?? "x86";

                DirectoryInfo CanaryDir = Directory.CreateDirectory(AppPath + "\\Canary");
                CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                if (!branchName.Equals(mainBranch, StringComparison.OrdinalIgnoreCase) ||
                    !repo.Equals(mainRepo, StringComparison.OrdinalIgnoreCase))
                {
                    using (StreamWriter sw = new StreamWriter(AppPath + "\\Canary\\Branch"))
                    {
                        if (!repo.Equals(mainRepo, StringComparison.OrdinalIgnoreCase))
                        {
                            sw.WriteLine(branchName);
                            sw.Write(repo);
                        }
                        else
                        {
                            sw.Write(branchName);
                        }

                        sw.Close();
                    }
                }

                string repoOwner = repo.Split('/')[0];
                string repoName = repo.Split('/')[1];

                Branch branch;
                GitHubCommit result;
                DateTimeOffset commitDate;
                branch = await github.Repository.Branch.Get(repoOwner, repoName, branchName);
                result = await github.Repository.Commit.Get(repoOwner, repoName, commitid ?? branch.Commit.Sha);
                commitDate = result.Commit.Author.Date;
                commitDate = commitDate.ToUniversalTime();
                string Filename = AppPath + "\\Canary\\New";
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                using (StreamWriter sw = new StreamWriter(Filename))
                {
                    sw.WriteLine(commitDate.ToString("O"));
                    sw.WriteLine(result.Sha.Substring(0, 7));
                    sw.WriteLine(result.Sha);
                    sw.WriteLine(branchName);
                    sw.WriteLine(repo);
                    sw.Write(platform);
                    sw.Close();
                }

                Console.WriteLine("Canary commit set. Sha was detected to be: " + result.Sha);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async Task SetCanaryActive()
        {
            DirectoryInfo CanaryDir = Directory.CreateDirectory(AppPath + "\\Canary");
            CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            if (!File.Exists(AppPath + "\\Canary\\Active"))
            {
                File.Create(AppPath + "\\Canary\\Active");
            }

            Console.WriteLine("Canary Active");
            await Task.Delay(1);
        }

        public static async Task SetCanaryInactive()
        {
            if (File.Exists(AppPath + "\\Canary\\Active"))
            {
                File.Delete(AppPath + "\\Canary\\Active");
            }

            await Task.Delay(1);
        }

        public static async Task ShowCanaryChangelog()
        {
            string changelog = "";
            string newSha;
            string oldSha;
            string newBranch;
            string oldBranch;
            string newRepo;
            string oldRepo;
            string Filename = AppPath + "\\Canary\\Old";
            bool showErrors = true;
            try
            {
                newSha = File.ReadAllLines(AppPath + "\\Canary\\New")[2];
                oldSha = File.ReadAllLines(AppPath + "\\Canary\\Old")[2];
                try
                {
                    newBranch = File.ReadAllLines(AppPath + "\\Canary\\New")[3];
                    oldBranch = File.ReadAllLines(AppPath + "\\Canary\\Old")[3];
                }
                catch
                {
                    // Assume that this is updating from an old version before branch data was tracked.
                    newBranch = "";
                    oldBranch = "";
                }

                try
                {
                    newRepo = File.ReadAllLines(AppPath + "\\Canary\\New")[4];
                    oldRepo = File.ReadAllLines(AppPath + "\\Canary\\Old")[4];
                }
                catch
                {
                    // Assume that this is updating from an old version before repo data was tracked.
                    newRepo = oldRepo = "";
                }
            }
            catch
            {
                try
                {
                    newSha = File.ReadAllLines(AppPath + "\\Canary\\New")[2];
                    oldSha = "";
                    try
                    {
                        newBranch = File.ReadAllLines(AppPath + "\\Canary\\New")[3];
                        oldBranch = newBranch;
                    }
                    catch
                    {
                        // Assume that this is updating from an old version before branch data was tracked.
                        newBranch = "";
                        oldBranch = "";
                    }

                    try
                    {
                        newRepo = File.ReadAllLines(AppPath + "\\Canary\\New")[4];
                        oldRepo = newRepo;
                    }
                    catch
                    {
                        // Assume that this is updating from an old version before repo data was tracked.
                        newRepo = "";
                        oldRepo = "";
                    }

                    showErrors = false;
                }
                catch
                {
                    MessageBox.Show(
                        "Canary changelog could not be shown. Make sure to never disturb the \"Canary\" folder in the installation folder.");
                    return;
                }
            }

            if (newSha == oldSha)
            {
                MessageBox.Show("Welcome to BrawlCrate Canary! You were already on the latest commit.");
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                return;
            }

            if (newRepo != oldRepo)
            {
                MessageBox.Show("Welcome to BrawlCrate Canary! You are now tracking the " + newBranch +
                                " branch of the " + newRepo + " repository instead of the " + oldBranch +
                                " branch of the " + oldRepo +
                                " repository. Canary changelog is not supported when switching repositories, so please check online to see differences.");
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                return;
            }

            if (newBranch != oldBranch)
            {
                MessageBox.Show("Welcome to BrawlCrate Canary! You are now tracking the " + newBranch +
                                " branch instead of the " + oldBranch +
                                " branch. Canary changelog is not supported when switching branches, so please check the Discord for what's been changed.");
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                return;
            }

            // check to see if the user is online, and that github is up and running.
            Console.WriteLine("Checking connection to server.");
            using (Ping s = new Ping())
            {
                Console.WriteLine(s.Send("www.github.com").Status);
            }

            char[] slashes = {'\\', '/'};
            string[] repoData = currentRepo.Split(slashes);

            try
            {
                Credentials cr = new Credentials(Encoding.Default.GetString(_rawData));
                GitHubClient github = new GitHubClient(new ProductHeaderValue("BrawlCrate")) {Credentials = cr};
                try
                {
                    await github.Repository.Branch.Get(repoData[0], repoData[1], currentBranch);
                }
                catch
                {
                    repoData = mainRepo.Split(slashes);
                    currentBranch = mainBranch;
                    currentRepo = mainRepo;
                }

                ApiOptions options = new ApiOptions
                {
                    PageSize = 120,
                    PageCount = 1
                };
                List<GitHubCommit> commits =
                    (await github.Repository.Commit.GetAll(repoData[0], repoData[1], options)).ToList();
                int i;
                bool foundCurrentCommit = false;
                for (i = 0; i < commits.Count;)
                {
                    GitHubCommit c = commits[i];
                    if (!foundCurrentCommit && c.Sha != newSha)
                    {
                        commits.Remove(c);
                        continue;
                    }

                    foundCurrentCommit = true;
                    if (c.Sha == oldSha || i > 99)
                    {
                        break;
                    }

                    i++;
                }

                if (i == 0)
                {
                    MessageBox.Show("No changes were found.");
                }

                for (int j = i - 1; j >= 0; j--)
                {
                    if (j >= commits.Count)
                    {
                        continue;
                    }

                    if (j == 99 && showErrors)
                    {
                        changelog += "\n\nMax commits reached. Showing last 100.";
                    }

                    GitHubCommit c = new GitHubCommit();
                    try
                    {
                        c = commits[j];
                    }
                    catch
                    {
                        continue;
                    }

                    changelog += "\n\n========================================================\n\n";
                    try
                    {
                        string s = "#" + c.Sha.Substring(0, 7) + "@" + currentRepo + '\\' + currentBranch + " by " +
                                   c.Author.Login + "\n";
                        changelog += s;
                    }
                    catch
                    {
                        changelog += "#" + c.Sha.Substring(0, 7) + "@" + currentRepo + '\\' + currentBranch + "\n";
                    }

                    changelog += c.Commit.Message;
                }

                changelog += "\n\n========================================================";
                if (!string.IsNullOrEmpty(oldSha))
                {
                    MessageBox.Show("Canary successfully updated from #" + oldSha.Substring(0, 7) + " to #" +
                                    newSha.Substring(0,
                                        7)); // For some reason, without this, the changelog window never shows.
                }
                else
                {
                    MessageBox.Show(
                        "The last 100 Canary commits will be shown. For a more in-depth view of changes, visit https://github.com/soopercool101/BrawlCrateNext/commits/master");
                }

                CanaryChangelogViewer logWindow = new CanaryChangelogViewer(newSha.Substring(0, 7), changelog);
                logWindow.ShowDialog();
                DirectoryInfo CanaryDir = Directory.CreateDirectory(AppPath + "\\Canary");
                CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Canary Changelog could not be shown:\n" + e.Message);
            }
        }

        public static async Task KillOpenWindows()
        {
            //Find and close all windows of the BrawlCrate application that will be overwritten
            TRY_AGAIN:
            Process[] px = Process.GetProcessesByName("BrawlCrate");
            Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
            Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
            if (p != null && p != default(Process) && px != null && pToClose != null && pToClose.Length > 1)
            {
                try
                {
                    foreach (Process pNext in pToClose)
                    {
                        pNext.Kill();
                    }

                    await Task.Delay(50);
                }
                catch (Exception xp)
                {
                    MessageBox.Show(xp.Message);
                }

                goto TRY_AGAIN;
            }

            if (p != null && p != default(Process))
            {
                p.Kill();
            }
        }

        public static int GetOpenWindowsCount()
        {
            Process[] px = Process.GetProcessesByName("BrawlCrate");
            Process[] pToFind = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
            return pToFind.Length;
        }
    }

    public static class BugSquish
    {
        private static readonly byte[] _rawData =
        {
            0x34, 0x35, 0x31, 0x30, 0x34, 0x31, 0x62, 0x38, 0x65, 0x39, 0x32, 0x64, 0x37, 0x32, 0x66, 0x62, 0x63, 0x36,
            0x38, 0x62, 0x63, 0x66, 0x61, 0x39, 0x36, 0x61, 0x32, 0x65, 0x30, 0x36, 0x64, 0x62, 0x61, 0x33, 0x62, 0x36,
            0x39, 0x32, 0x66, 0x63, 0x20
        };

        public static async Task CreateIssue(
            string TagName,
            string ExceptionMessage,
            string StackTrace,
            string Title,
            string Description)
        {
            if (File.Exists(Updater.AppPath + "\\Canary\\Active") &&
                !Updater.currentRepo.Equals(Updater.mainRepo, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Issue reporter does not allow reporting issues from forks. Please contact the owner of the repository to report your issue.");
                return;
            }

            try
            {
                Credentials cr = new Credentials(Encoding.Default.GetString(_rawData));
                GitHubClient github = new GitHubClient(new ProductHeaderValue("BrawlCrate")) {Credentials = cr};
                IReadOnlyList<Issue> issues = null;
                if (!TagName.ToLower().Contains("canary"))
                {
                    IReadOnlyList<Release> releases = null;
                    try
                    {
                        releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");

                        // Remove all pre-release (Documentation) versions from the list
                        releases = releases.Where(r => !r.Prerelease).ToList();

                        issues = await github.Issue.GetAllForRepository("BrawlCrate", "BrawlCrateIssues");
                    }
                    catch (HttpRequestException)
                    {
                        MessageBox.Show("Unable to connect to the internet.");
                        return;
                    }

                    if (releases.Count > 0 && releases[0].TagName != TagName)
                    {
                        //This build's version tag does not match the latest release's tag on the repository.
                        //This bug may have been fixed by now. Tell the user to update to be allowed to submit bug reports.

                        DialogResult UpdateResult =
                            MessageBox.Show(
                                releases[0].Name +
                                " is available!\nYou cannot submit bug reports using an older version of the program.\nUpdate now?",
                                "An update is available", MessageBoxButtons.YesNo);
                        if (UpdateResult == DialogResult.Yes)
                        {
                            DialogResult OverwriteResult = MessageBox.Show("Overwrite current installation?", "",
                                MessageBoxButtons.YesNoCancel);
                            if (OverwriteResult != DialogResult.Cancel)
                            {
                                Task t = Updater.ForceDownloadStable();
                                t.Wait();
                            }
                        }

                        return;
                    }
                }

                bool found = false;
                if (issues != null && !string.IsNullOrEmpty(StackTrace))
                {
                    foreach (Issue i in issues)
                    {
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

                                Issue x = await github.Issue.Update("BrawlCrate", "BrawlCrateIssues", i.Number, update);
                            }
                        }
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
                    Issue x = await github.Issue.Create("BrawlCrate", "BrawlCrateIssues", issue);
                }
            }
            catch
            {
                MessageBox.Show("The application was unable to retrieve permission to send this issue.");
            }
        }
    }

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
            Updater.currentRepo = Updater.GetCurrentRepo();
            Updater.currentBranch = Updater.GetCurrentBranch();
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
                        Task t2c = Updater.CheckCanaryUpdate(args[1], args[2] != "0");
                        t2c.Wait();
                        break;
                    case "-bi": //BrawlCrate issue call
                        somethingDone = true;
                        Task t3 = BugSquish.CreateIssue(args[1], args[2], args[3], args[4], args[5]);
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
                Task t = Updater.CheckUpdate(true);
                t.Wait();
            }

            if (!somethingDone)
            {
                Console.WriteLine(Usage);
            }
        }
    }
}
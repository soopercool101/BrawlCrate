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

namespace Updater
{
    public static class Updater
    {
        public static string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static readonly string mainRepo = "soopercool101/BrawlCrateNext";
        public static readonly string mainBranch = "master";

        private static readonly GitHubClient Github = new GitHubClient(new ProductHeaderValue("BrawlCrate"))
        { Credentials = new Credentials(Encoding.Default.GetString(Program.RawData)) };

        #region Canary Variables/Helpers

        public static string currentRepo = GetCurrentRepo();
        public static string currentBranch = GetCurrentBranch();
        public static string platform = GetCurrentPlatform();

        public static string GetCurrentRepo()
        {
            try
            {
                string temp = File.ReadAllLines(AppPath + "\\Canary\\Branch")[1];
                if (string.IsNullOrEmpty(temp))
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
                if (string.IsNullOrEmpty(temp))
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

        public static string GetCurrentPlatform()
        {
            string p;
            try
            {
                p = File.ReadAllLines(AppPath + "\\Canary\\New")[5];
            }
            catch
            {
                p = "x86";
            }

            return p;
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

            char[] slashes = { '\\', '/' };
            string[] repoData = currentRepo.Split(slashes);

            try
            {
                try
                {
                    await Github.Repository.Branch.Get(repoData[0], repoData[1], currentBranch);
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
                    (await Github.Repository.Commit.GetAll(repoData[0], repoData[1], options)).ToList();
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

        #endregion

        #region Release Update Check

        public static async Task CheckUpdate()
        {
            await CheckUpdate(true, "", true, null, true, true);
        }

        public static async Task CheckUpdate(bool overwrite)
        {
            await CheckUpdate(overwrite, "", true, null, true, true);
        }

        // Used to check for and download non-canary releases (including documentation updates)
        public static async Task CheckUpdate(bool overwrite, string releaseTag, bool manual, string openFile,
                                             bool checkDocumentation, bool automatic)
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
                IReadOnlyList<Release> AllReleases = await Github.Repository.Release.GetAll(repoOwner, repoName);
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
                if (automatic)
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

                await DownloadRelease(release, overwrite, automatic, manual, documentation, openFile);
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

        #endregion

        #region Canary Update Check

        public static async Task CheckCanaryUpdate(string openFile, bool manual, bool force)
        {
            try
            {
                char[] slashes = { '\\', '/' };
                string[] repoData = currentRepo.Split(slashes);
                Release release =
                    await Github.Repository.Release.Get(repoData[0], repoData[1], $"Canary-{currentBranch}");

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

                    ReleaseAsset Asset = release.Assets.First(a => a.Name.Contains(platform));
                    if (Asset != null)
                    {
                        GitHubCommit c =
                            await Github.Repository.Commit.Get(repoData[0], repoData[1], release.TargetCommitish);
                        if (Asset.CreatedAt.UtcDateTime <= c.Commit.Committer.Date)
                        {
                            // Asset has not yet been updated
                            return;
                        }
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

        #endregion

        #region Package Downloader

        public static async Task DownloadRelease(Release release, bool overwrite, bool automatic, bool manual,
                                                 bool documentation, string openFile)
        {
            try
            {
                ReleaseAsset Asset = release.Assets.Any(a => a.Name.Contains(platform))
                    ? release.Assets.First(a => a.Name.Contains(platform))
                    : release.Assets[0];

                // If open windows need to be closed, ensure they are all properly closed
                if (overwrite && !documentation)
                {
                    await KillOpenWindows();
                }

                // Check if we were passed in the overwrite parameter, and if not create a new folder to extract in.
                if (!overwrite)
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
                if (Process.GetProcessesByName("winlogon").Count() == 0 || documentation || !overwrite)
                {
                    try
                    {
                        Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\" -y");
                        // For documentation updates, ensure temp.exe is properly deleted and show changelog if the download was automated.
                        if (documentation)
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
                                            (automatic ? "\nThis documentation release:\n" + release.Body : ""));
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

        #endregion

        #region Forced Downloads

        public static async Task ForceDownloadDocumentation()
        {
            string repoOwner = mainRepo.Split('/')[0];
            string repoName = mainRepo.Split('/')[1];
            // get Release
            IReadOnlyList<Release> releases = (await Github.Repository.Release.GetAll(repoOwner, repoName))
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

        public static async Task ForceDownloadStable(string openFile)
        {
            await SetCanaryInactive();
            string repoOwner = mainRepo.Split('/')[0];
            string repoName = mainRepo.Split('/')[1];
            // get Release
            IReadOnlyList<Release> releases = (await Github.Repository.Release.GetAll(repoOwner, repoName))
                                              .Where(r => !r.Prerelease).ToList();
            if (releases.Count > 0)
            {
                await DownloadRelease(releases[0], true, true, false, false, openFile);
            }
        }

        #endregion

        #region Utility

        /// <summary>
        ///     Used when building for releases. Writes relevant information that is necessary to switch to a canary build
        /// </summary>
        /// <param name="commitID">The unique id for the commit</param>
        /// <param name="branchName">The branch that this build was published from</param>
        /// <param name="repo">The repository that this build was published from</param>
        /// <param name="canaryPlatform">The platform that this build was made for (i.e. x86)</param>
        /// <returns></returns>
        public static async Task WriteCanaryTime(string commitID, string branchName, string repo, string canaryPlatform)
        {
            try
            {
                if (commitID != null)
                {
                    Console.WriteLine("Attempting to set Canary using sha: " + commitID);
                }

                branchName = branchName ?? mainBranch;
                repo = repo ?? mainRepo;
                canaryPlatform = canaryPlatform ?? "x86";

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

                Branch branch = await Github.Repository.Branch.Get(repoOwner, repoName, branchName);
                GitHubCommit result =
                    await Github.Repository.Commit.Get(repoOwner, repoName, commitID ?? branch.Commit.Sha);
                DateTimeOffset buildDate = DateTimeOffset.UtcNow;
                string filename = AppPath + "\\Canary\\New";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine(buildDate.ToString("O"));
                    sw.WriteLine(result.Sha.Substring(0, 7));
                    sw.WriteLine(result.Sha);
                    sw.WriteLine(branchName);
                    sw.WriteLine(repo);
                    sw.Write(canaryPlatform);
                    sw.Close();
                }

                Console.WriteLine("Canary commit set. Sha was detected to be: " + result.Sha);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public static int GetOpenWindowsCount()
        {
            Process[] px = Process.GetProcessesByName("BrawlCrate");
            Process[] pToFind = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
            return pToFind.Length;
        }

        public static async Task KillOpenWindows()
        {
            //Find and close all windows of the BrawlCrate application that will be overwritten
            Process[] px = Process.GetProcessesByName("BrawlCrate");
            Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
            Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
            if (p != null && px != null && pToClose != null && pToClose.Length > 1)
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

                await KillOpenWindows();
                return;
            }

            p?.Kill();
        }

        #endregion
    }
}

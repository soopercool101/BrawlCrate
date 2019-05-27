//================================================================\\
//  Simple application containing most functions for interfacing  \\
//      with Github API, including Updater and BugSquish.         \\
//================================================================\\
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Net
{
    public static class Updater
    {
        public static readonly string mainRepo = "soopercool101/BrawlCrate";
        public static readonly string mainBranch = "brawlcrate-master";
        public static string currentRepo = GetCurrentRepo();
        public static string currentBranch = GetCurrentBranch();

        private static string GetCurrentRepo()
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
                }

                return mainRepo;
            }
            catch
            {
                return mainRepo;
            }
        }

        private static string GetCurrentBranch()
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
                }

                return mainBranch;
            }
            catch
            {
                return mainBranch;
            }
        }

        private static readonly byte[] _rawData =
        {
            0x34, 0x35, 0x31, 0x30, 0x34, 0x31, 0x62, 0x38, 0x65, 0x39, 0x32, 0x64, 0x37, 0x32, 0x66, 0x62, 0x63, 0x36,
            0x38, 0x62, 0x63, 0x66, 0x61, 0x39, 0x36, 0x61, 0x32, 0x65, 0x30, 0x36, 0x64, 0x62, 0x61, 0x33, 0x62, 0x36,
            0x39, 0x32, 0x66, 0x63, 0x20
        };

        public static readonly string BaseURL = "https://github.com/soopercool101/BrawlCrate/releases/download/";
        public static string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static async Task UpdateCheck() { await UpdateCheck(false); }
        public static async Task UpdateCheck(bool Overwrite, string openFile = null, bool Documentation = false, bool Automatic = false)
        {
            Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
            try
            {
                if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase))
                {
                    AppPath = AppPath.Substring(0, AppPath.Length - 4);
                }

                // check to see if the user is online, and that github is up and running.
                Console.WriteLine("Checking connection to server.");
                using (Ping s = new Ping())
                {
                    Console.WriteLine(s.Send("www.github.com").Status);
                }

                // Initiate the github client.
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };

                // get Release
                IReadOnlyList<Release> releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");
                if (!Documentation)
                {
                    releases = releases.Where(r => !r.Prerelease).ToList();
                }
                else
                {
                    releases = releases.Where(r => r.Prerelease).ToList();
                }
                // Get Release Assets
                Release release = releases[0];
                ReleaseAsset Asset = (await github.Repository.Release.GetAllAssets("soopercool101", "BrawlCrate", release.Id))[0];
                if (Asset == null)
                {
                    return;
                }

                if (Overwrite && !Documentation)
                {
                //Find and close the BrawlCrate application that will be overwritten
                TRY_AGAIN:
                    Process[] px = Process.GetProcessesByName("BrawlCrate");
                    Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
                    Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
                    if (p != null && p != default(Process) && px != null && pToClose != null && pToClose.Length > 1)
                    {
                        DialogResult continueUpdate = MessageBox.Show("Update cannot proceed unless all open windows of " + AppPath + "\\BrawlCrate.exe are closed. Would you like to force close all open BrawlCrate windows at this time?\n\n" +
                            "Select \"Yes\" if you would like to force close all open BrawlCrate windows\n" +
                            "Select \"No\" after closing all windows manually if you would like to proceed without force closing\n" +
                            "Select \"Cancel\" if you would like to wait to update until another time", releases[0].Name + " Update", MessageBoxButtons.YesNoCancel);
                        if (continueUpdate == DialogResult.Yes)
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
                        else if (continueUpdate == DialogResult.No)
                        {
                            goto TRY_AGAIN;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (p != null && p != default(Process) && Automatic)
                    {
                        p.Kill();
                    }
                    else if (p != null && p != default(Process) && p.CloseMainWindow())
                    {
                        p.WaitForExit();
                        p.Close();
                    }
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
                    string URL = html.Substring(html.IndexOf(BaseURL)).TrimEnd(new char[] { '}', '"' });

                    //client.DownloadFile(URL, AppPath + "/temp.exe");
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(null, releases[0].Name, AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }
                    dlTrack.Close();
                    dlTrack.Dispose();
                    if (!File.Exists(AppPath + "/temp.exe") || (new FileInfo(AppPath + "/temp.exe")).Length != (long)DLProgressWindow.MaxValue || (new FileInfo(AppPath + "/temp.exe")).Length == 0)
                    {
                        MessageBox.Show("Error downloading update");
                        if (File.Exists(AppPath + "/temp.exe"))
                        {
                            File.Delete(AppPath + "/temp.exe");
                        }

                        return;
                    }


                    // Case 1: Wine (Batch files won't work, use old methodology) or documentation update
                    if (Process.GetProcessesByName("winlogon").Count<Process>() == 0 || Documentation || !Overwrite)
                    {
                        try
                        {
                            Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\"" + " -y");
                            if (Documentation)
                            {
                                update.WaitForExit();
                                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe"))
                                {
                                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe");
                                }

                                MessageBox.Show("Documentation was successfully updated to " + ((releases[0].Name.StartsWith("BrawlCrate Documentation", StringComparison.OrdinalIgnoreCase) && releases[0].Name.Length > 26) ? releases[0].Name.Substring(25) : releases[0].Name) + (Automatic ? "\nThis documentation release:\n" + releases[0].Body : ""));
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error: " + e.Message);
                        }
                        return;
                    }
                    // Case 2: Windows (use a batch file to ensure a consistent experience)
                    if (File.Exists(AppPath + "/Update.bat"))
                    {
                        File.Delete(AppPath + "/Update.bat");
                    }

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
                            sw.WriteLine("del OpenTK.dll /s /f /q");
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
                    Process updateBat = Process.Start(new ProcessStartInfo()
                    {
                        FileName = AppPath + "/Update.bat",
                        WindowStyle = ProcessWindowStyle.Hidden,
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public static async Task CheckUpdates(string releaseTag, string openFile, bool manual = true, bool checkDocumentation = false, bool automatic = false)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active");
            }

            Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
            string docVer = null;
            if (checkDocumentation)
            {
                try
                {
                    docVer = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "InternalDocumentation" + '\\' + "version.txt")[0];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    MessageBox.Show("ERROR: Documentation Version could not be found. Downloading the latest documentation release.");
                    await ForceDownloadDocumentation();
                    try
                    {
                        docVer = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "InternalDocumentation" + '\\' + "version.txt")[0];
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine(e2.Message);
                        MessageBox.Show("ERROR: Documentation Version still could not be found. Please report this on Discord or Github.");
                    }
                }
            }
            try
            {
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                IReadOnlyList<Release> AllReleases = null;
                AllReleases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");
                IReadOnlyList<Release> releases = null;
                try
                {
                    // Remove all pre-release versions from the list (Prerelease versions are exclusively documentation updates)
                    releases = AllReleases.Where(r => !r.Prerelease).ToList();

                    if (releases[0].TagName == releaseTag)
                    {
                        if (manual)
                        {
                            MessageBox.Show("No updates found.");
                        }

                        return;
                    }
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    if (manual)
                    {
                        MessageBox.Show("Unable to connect to the internet.");
                    }

                    return;
                }

                if (releases != null &&
                    releases.Count > 0 &&
                    !string.Equals(releases[0].TagName, releaseTag, StringComparison.InvariantCulture) && //Make sure the most recent version is not this version
                    releases[0].Name.IndexOf("BrawlCrate v", StringComparison.InvariantCultureIgnoreCase) >= 0) //Make sure this is a BrawlCrate release
                {
                    if (automatic)
                    {
                        if (releases[0].Body.Contains("WARNING: "))
                        {
                            if (releases[0].Body.StartsWith("WARNING: "))
                            {
                                DialogResult dr = MessageBox.Show(releases[0].Body.Substring(0, releases[0].Body.IndexOf("\n") - 1) + "\n\nWould you like to continue updating?", "Automatic Update Warning", MessageBoxButtons.YesNo);
                                if (dr != DialogResult.Yes)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show(releases[0].Body.Substring(releases[0].Body.IndexOf("WARNING: ")) + "\n\nWould you like to continue updating?", "Automatic Update Warning", MessageBoxButtons.YesNo);
                                if (dr != DialogResult.Yes)
                                {
                                    return;
                                }
                            }
                        }
                        Task t = UpdateCheck(true, openFile, false, true);
                        t.Wait();
                        return;
                    }
                    int descriptionOffset = 0;
                    if (releases[0].Body.Length > 110 && releases[0].Body.Substring(releases[0].Body.Length - 109) == "\nAlso check out the Brawl Stage Compendium for info and research on Stage Modding: https://discord.gg/s7c8763")
                    {
                        descriptionOffset = 110;
                    }

                    DialogResult UpdateResult = MessageBox.Show(releases[0].Name + " is available!\n\nThis release:\n\n" + releases[0].Body.Substring(0, releases[0].Body.Length - descriptionOffset) + "\n\nUpdate now?", "Update", MessageBoxButtons.YesNo);
                    if (UpdateResult == DialogResult.Yes)
                    {
                        DialogResult OverwriteResult = MessageBox.Show("Overwrite current installation?", "", MessageBoxButtons.YesNoCancel);
                        if (OverwriteResult != DialogResult.Cancel)
                        {
                            Task t = UpdateCheck(OverwriteResult == DialogResult.Yes, openFile);
                            t.Wait();
                        }
                    }
                    return;
                }
                else if (manual && !checkDocumentation)
                {
                    MessageBox.Show("No updates found.");
                    return;
                }
                if (checkDocumentation)
                {
                    if (docVer == null)
                    {
                        // Errors have already been thrown
                        //MessageBox.Show("ERROR: Documentation Version could not be found. Will download the latest documentation.");
                        return;
                    }
                    try
                    {
                        releases = AllReleases.ToList();

                        // Ensure that the latest update is, in fact, a documentation update
                        if (!releases[0].Prerelease || !releases[0].Name.Contains("Documentation"))
                        {
                            if (manual)
                            {
                                MessageBox.Show("No updates found.");
                            }

                            return;
                        }

                        // Only get pre-release versions, as they are the pipeline documentation updates will be sent with
                        releases = AllReleases.Where(r => r.Prerelease).ToList();
                    }
                    catch (System.Net.Http.HttpRequestException)
                    {
                        if (manual)
                        {
                            MessageBox.Show("Unable to connect to the internet.");
                        }

                        return;
                    }

                    if (releases != null &&
                        releases.Count > 0 &&
                        !string.Equals(releases[0].TagName, docVer, StringComparison.InvariantCulture) && //Make sure the most recent version is not this version
                        releases[0].Name.IndexOf("Documentation", StringComparison.InvariantCultureIgnoreCase) >= 0) //Make sure this is a Documentation release
                    {
                        int descriptionOffset = 0;
                        if (automatic)
                        {
                            Task t = UpdateCheck(true, openFile, true, true);
                            t.Wait();
                            return;
                        }
                        DialogResult UpdateResult = MessageBox.Show(releases[0].Name + " is available!\n\nThis documentation release:\n\n" + releases[0].Body.Substring(0, releases[0].Body.Length - descriptionOffset) + "\n\nUpdate now?", "Update", MessageBoxButtons.YesNo);
                        if (UpdateResult == DialogResult.Yes)
                        {
                            Task t = UpdateCheck(true, openFile, true);
                            t.Wait();
                        }
                    }
                    else if (manual)
                    {
                        MessageBox.Show("No updates found.");
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                if (manual)
                {
                    MessageBox.Show("Unable to connect to the internet.");
                }

                return;
            }
            catch (Exception e)
            {
                if (manual)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static async Task ForceDownloadRelease(string openFile)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active");
            }

            Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
            try
            {
                if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase))
                {
                    AppPath = AppPath.Substring(0, AppPath.Length - 4);
                }

                // check to see if the user is online, and that github is up and running.
                Console.WriteLine("Checking connection to server.");
                using (Ping s = new Ping())
                {
                    Console.WriteLine(s.Send("www.github.com").Status);
                }

                // Initiate the github client.
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };

                // get Release
                IReadOnlyList<Release> releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");
                releases = releases.Where(r => !r.Prerelease).ToList();
                Release release = releases[0];
                // Get Release Assets
                ReleaseAsset Asset = (await github.Repository.Release.GetAllAssets("soopercool101", "BrawlCrate", release.Id))[0];
                if (Asset == null)
                {
                    return;
                }

            //Find and close the BrawlCrate application that will be overwritten
            TRY_AGAIN:
                Process[] px = Process.GetProcessesByName("BrawlCrate");
                Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
                Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
                if (p != null && p != default(Process) && px != null && pToClose != null && pToClose.Length > 1)
                {
                    DialogResult continueUpdate = MessageBox.Show("Update cannot proceed unless all open windows of " + AppPath + "\\BrawlCrate.exe are closed. Would you like to force close all open BrawlCrate windows at this time?\n\n" +
                        "Select \"Yes\" if you would like to force close all open BrawlCrate windows\n" +
                        "Select \"No\" after closing all windows manually if you would like to proceed without force closing\n" +
                        "Select \"Cancel\" if you would like to wait to update until another time", releases[0].Name + " Update", MessageBoxButtons.YesNoCancel);
                    if (continueUpdate == DialogResult.Yes)
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
                    else if (continueUpdate == DialogResult.No)
                    {
                        goto TRY_AGAIN;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (p != null && p != default(Process))
                {
                    p.Kill();
                }

                using (WebClient client = new WebClient())
                {
                    // Add the user agent header, otherwise we will get access denied.
                    client.Headers.Add("User-Agent: Other");

                    // Full asset streamed into a single string
                    string html = client.DownloadString(Asset.Url);

                    // The browser download link to the self extracting archive, hosted on github
                    string URL = html.Substring(html.IndexOf(BaseURL)).TrimEnd(new char[] { '}', '"' });

                    //client.DownloadFile(URL, AppPath + "/temp.exe");
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(null, releases[0].Name, AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }
                    dlTrack.Close();
                    dlTrack.Dispose();
                    if (!File.Exists(AppPath + "/temp.exe") || (new FileInfo(AppPath + "/temp.exe")).Length != (long)DLProgressWindow.MaxValue || (new FileInfo(AppPath + "/temp.exe")).Length == 0)
                    {
                        MessageBox.Show("Error downloading update");
                        if (File.Exists(AppPath + "/temp.exe"))
                        {
                            File.Delete(AppPath + "/temp.exe");
                        }

                        return;
                    }

                    // Case 1: Wine (Batch files won't work, use old methodology)
                    if (Process.GetProcessesByName("winlogon").Count<Process>() == 0)
                    {
                        try
                        {
                            Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\"" + " -y");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error: " + e.Message);
                        }
                        return;
                    }
                    // Case 2: Windows (use a batch file to ensure a consistent experience)
                    if (File.Exists(AppPath + "/Update.bat"))
                    {
                        File.Delete(AppPath + "/Update.bat");
                    }

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
                            sw.WriteLine("del OpenTK.dll /s /f /q");
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
                        sw.Write("START BrawlCrate.exe \"" + (openFile != null && openFile != "<null>" ? openFile : "null") + "\"");
                        sw.Close();
                    }
                    Process updateBat = Process.Start(new ProcessStartInfo()
                    {
                        FileName = AppPath + "/Update.bat",
                        WindowStyle = ProcessWindowStyle.Hidden,
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public static async Task CheckCanaryUpdate(string openFile, bool manual)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
            {
                await SetCanaryActive();
                MessageBox.Show("ERROR: Current Canary version could not be found. Updating to the latest commit");
                await ForceDownloadCanary(openFile);
                return;
            }
            try
            {
                string oldID = "";
                oldID = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "New")[2];
                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                char[] slashes = { '\\', '/' };
                string[] repoData = currentRepo.Split(slashes);
                Release release = await github.Repository.Release.Get("soopercool101", "BrawlCrate", "Canary");
                string newID = release.TargetCommitish;
                if (oldID.Equals(newID, StringComparison.OrdinalIgnoreCase))
                {
                    if (manual)
                    {
                        MessageBox.Show("No updates found.");
                    }

                    return;
                }
                await ForceDownloadCanary(openFile, newID.Substring(0, 7));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("ERROR: Current Canary version could not be found. Updating to the latest commit");
                await ForceDownloadCanary(openFile);
                return;
            }
        }

        public static async Task ForceDownloadCanary(string openFile, string commitID = null)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
            {
                await SetCanaryActive();
            }

            try
            {
                if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase))
                {
                    AppPath = AppPath.Substring(0, AppPath.Length - 4);
                }

                // check to see if the user is online, and that github is up and running.
                Console.WriteLine("Checking connection to server.");
                using (Ping s = new Ping())
                {
                    Console.WriteLine(s.Send("www.github.com").Status);
                }

                char[] slashes = { '\\', '/' };
                string[] repoData = currentRepo.Split(slashes);

                // Initiate the github client.
                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };

                Release release = await github.Repository.Release.Get("soopercool101", "BrawlCrate", "Canary");
                // Get Release Assets
                ReleaseAsset Asset = (await github.Repository.Release.GetAllAssets("soopercool101", "BrawlCrate", release.Id))[0];

            //Find and close the BrawlCrate application that will be overwritten
            TRY_AGAIN:
                Process[] px = Process.GetProcessesByName("BrawlCrate");
                Process[] pToClose = px.Where(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe")).ToArray();
                Process p = px.FirstOrDefault(x => x.MainModule.FileName.Equals(AppPath + "\\BrawlCrate.exe"));
                if (p != null && p != default(Process) && px != null && pToClose != null && pToClose.Length > 1)
                {
                    DialogResult continueUpdate = MessageBox.Show("Update cannot proceed unless all open windows of " + AppPath + "\\BrawlCrate.exe are closed. Would you like to force close all open BrawlCrate windows at this time?\n\n" +
                        "Select \"Yes\" if you would like to force close all open BrawlCrate windows\n" +
                        "Select \"No\" after closing all windows manually if you would like to proceed without force closing\n" +
                        "Select \"Cancel\" if you would like to wait to update until another time", "Canary Update #" + commitID, MessageBoxButtons.YesNoCancel);
                    if (continueUpdate == DialogResult.Yes)
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
                    else if (continueUpdate == DialogResult.No)
                    {
                        goto TRY_AGAIN;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (p != null && p != default(Process))
                {
                    p.Kill();
                }

                using (WebClient client = new WebClient())
                {
                    // Add the user agent header, otherwise we will get access denied.
                    client.Headers.Add("User-Agent: Other");

                    // Full asset streamed into a single string
                    string html = client.DownloadString(Asset.Url);

                    // The browser download link to the self extracting archive, hosted on github
                    string URL = html.Substring(html.IndexOf(BaseURL)).TrimEnd(new char[] { '}', '"' });

                    //client.DownloadFile(URL, AppPath + "/temp.exe");
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(null, commitID == null ? "BrawlCrate Canary Build" : (currentRepo == mainRepo ? (currentBranch == mainBranch ? "BrawlCrate Canary #" + commitID : "Canary@" + currentBranch + " #" + commitID) : currentRepo + " Canary@" + currentBranch + " #" + commitID), AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }
                    dlTrack.Close();
                    dlTrack.Dispose();
                    if (!File.Exists(AppPath + "/temp.exe") || (new FileInfo(AppPath + "/temp.exe")).Length != (long)DLProgressWindow.MaxValue || (new FileInfo(AppPath + "/temp.exe")).Length == 0)
                    {
                        MessageBox.Show("Error downloading update");
                        if (File.Exists(AppPath + "/temp.exe"))
                        {
                            File.Delete(AppPath + "/temp.exe");
                        }

                        return;
                    }
                    DirectoryInfo CanaryDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");
                    CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    string Filename = AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "New";
                    string oldName = AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Old";
                    if (File.Exists(Filename))
                    {
                        if (!File.Exists(oldName))
                        {
                            File.Move(Filename, oldName);
                        }

                        if (File.Exists(Filename))
                        {
                            File.Delete(Filename);
                        }
                    }
                    //await WriteCanaryTime();
                    // Case 1: Wine (Batch files won't work, use old methodology)
                    if (Process.GetProcessesByName("winlogon").Count<Process>() == 0)
                    {
                        try
                        {
                            Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\"" + " -y");
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error: " + e.Message);
                        }
                        return;
                    }
                    // Case 2: Windows (use a batch file to ensure a consistent experience)
                    if (File.Exists(AppPath + "/Update.bat"))
                    {
                        File.Delete(AppPath + "/Update.bat");
                    }

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
                            sw.WriteLine("del OpenTK.dll /s /f /q");
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
                        sw.Write("START BrawlCrate.exe \"" + (openFile != null && openFile != "<null>" ? openFile : "null") + "\"");
                        sw.Close();
                    }
                    Process updateBat = Process.Start(new ProcessStartInfo()
                    {
                        FileName = AppPath + "/Update.bat",
                        WindowStyle = ProcessWindowStyle.Hidden,
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public static async Task ForceDownloadDocumentation()
        {
            Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
            try
            {
                if (AppPath.EndsWith("lib", StringComparison.CurrentCultureIgnoreCase))
                {
                    AppPath = AppPath.Substring(0, AppPath.Length - 4);
                }

                // check to see if the user is online, and that github is up and running.
                Console.WriteLine("Checking connection to server.");
                using (Ping s = new Ping())
                {
                    Console.WriteLine(s.Send("www.github.com").Status);
                }

                // Initiate the github client.
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };

                // get Release
                IReadOnlyList<Release> releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");
                releases = releases.Where(r => r.Prerelease).ToList();
                Release release = releases[0];
                // Get Release Assets
                ReleaseAsset Asset = (await github.Repository.Release.GetAllAssets("soopercool101", "BrawlCrate", release.Id))[0];
                if (Asset == null)
                {
                    return;
                }

                using (WebClient client = new WebClient())
                {
                    // Add the user agent header, otherwise we will get access denied.
                    client.Headers.Add("User-Agent: Other");

                    // Full asset streamed into a single string
                    string html = client.DownloadString(Asset.Url);

                    // The browser download link to the self extracting archive, hosted on github
                    string URL = html.Substring(html.IndexOf(BaseURL)).TrimEnd(new char[] { '}', '"' });

                    //client.DownloadFile(URL, AppPath + "/temp.exe");
                    DLProgressWindow.finished = false;
                    DLProgressWindow dlTrack = new DLProgressWindow(null, releases[0].Name, AppPath, URL);
                    while (!DLProgressWindow.finished)
                    {
                        // do nothing
                    }
                    dlTrack.Close();
                    dlTrack.Dispose();
                    if (!File.Exists(AppPath + "/temp.exe") || (new FileInfo(AppPath + "/temp.exe")).Length != (long)DLProgressWindow.MaxValue || (new FileInfo(AppPath + "/temp.exe")).Length == 0)
                    {
                        MessageBox.Show("Error downloading update");
                        if (File.Exists(AppPath + "/temp.exe"))
                        {
                            File.Delete(AppPath + "/temp.exe");
                        }

                        return;
                    }

                    try
                    {
                        Process update = Process.Start(AppPath + "/temp.exe", "-o\"" + AppPath + "\"" + " -y");
                        update.WaitForExit();
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe"))
                        {
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe");
                        }

                        MessageBox.Show("Documentation was successfully updated to " + ((releases[0].Name.StartsWith("BrawlCrate Documentation", StringComparison.OrdinalIgnoreCase) && releases[0].Name.Length > 26) ? releases[0].Name.Substring(25) : releases[0].Name) + (true ? "\nThis documentation release:\n" + releases[0].Body : ""));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error: " + e.Message);
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }
        // Used when building for releases
        public static async Task WriteCanaryTime(string commitid)
        {
            try
            {
                if (commitid != null)
                {
                    Console.WriteLine("Attempting to set Canary using sha: " + commitid);
                }

                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                Branch branch;
                GitHubCommit result;
                DateTimeOffset commitDate;
                branch = await github.Repository.Branch.Get("soopercool101", "BrawlCrate", mainBranch);
                result = await github.Repository.Commit.Get("soopercool101", "BrawlCrate", commitid ?? branch.Commit.Sha);
                commitDate = result.Commit.Author.Date;
                currentBranch = mainBranch;
                commitDate = commitDate.ToUniversalTime();
                DirectoryInfo CanaryDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");
                CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                string Filename = AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "New";
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                using (StreamWriter sw = new StreamWriter(Filename))
                {
                    sw.WriteLine(commitDate.ToString("O"));
                    sw.WriteLine(result.Sha.ToString().Substring(0, 7));
                    sw.WriteLine(result.Sha.ToString());
                    sw.WriteLine(currentBranch);
                    sw.Write(currentRepo);
                    sw.Close();
                }
                Console.WriteLine("Canary commit set. Sha was detected to be: " + result.Sha);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public static async Task SetCanaryActive()
        {
            DirectoryInfo CanaryDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");
            CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active");
            }

            await Task.Delay(1);
        }

        public static async Task SetCanaryInactive()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active");
            }

            await Task.Delay(1);
        }

        public static async Task ShowCanaryChangelog()
        {
            string changelog = "";
            string newSha = "";
            string oldSha = "";
            string newBranch = "";
            string oldBranch = "";
            string newRepo = "";
            string oldRepo = "";
            string Filename = AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Old";
            try
            {
                newSha = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "New")[2];
                oldSha = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Old")[2];
                try
                {
                    newBranch = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "New")[3];
                    oldBranch = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Old")[3];
                }
                catch
                {
                    // Assume that this is updating from an old version before branch data was tracked.
                    newBranch = oldBranch = "";
                }
                try
                {
                    newRepo = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "New")[4];
                    oldRepo = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Old")[4];
                }
                catch
                {
                    // Assume that this is updating from an old version before repo data was tracked.
                    newRepo = oldRepo = "";
                }
            }
            catch
            {
                MessageBox.Show("Canary changelog could not be shown. Make sure to never disturb the \"Canary\" folder in the installation folder.");
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                return;
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
                MessageBox.Show("Welcome to BrawlCrate Canary! You are now tracking the " + newBranch + " branch of the " + newRepo + " repository instead of the " + oldBranch + " branch of the " + oldRepo + " repository. Canary changelog is not supported when switching repositories, so please check online to see differences.");
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }

                return;
            }
            if (newBranch != oldBranch)
            {
                MessageBox.Show("Welcome to BrawlCrate Canary! You are now tracking the " + newBranch + " branch instead of the " + oldBranch + " branch. Canary changelog is not supported when switching branches, so please check the Discord for what's been changed.");
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
                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                Branch branch;
                try
                {
                    branch = await github.Repository.Branch.Get(repoData[0], repoData[1], currentBranch);
                }
                catch
                {
                    repoData = mainRepo.Split(slashes);
                    branch = await github.Repository.Branch.Get(repoData[0], repoData[1], mainBranch);
                    currentBranch = mainBranch;
                    currentRepo = mainRepo;
                }
                ApiOptions options = new ApiOptions
                {
                    PageSize = 120,
                    PageCount = 1
                };
                List<GitHubCommit> commits = (await github.Repository.Commit.GetAll(repoData[0], repoData[1], options)).ToList<GitHubCommit>();
                int i = -1;
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
                    //var c = await github.Repository.Commit.Get("soopercool101", "BrawlCrate", branch.Commit.Sha);
                    if (c.Sha == oldSha || i >= 99)
                    {
                        break;
                    }

                    i++;
                }
                for (int j = i; j >= 0; j--)
                {
                    if (j >= commits.Count)
                    {
                        continue;
                    }

                    if (j == 99)
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
                        string s = ("#" + c.Sha.Substring(0, 7) + "@" + currentRepo + '\\' + currentBranch + " by " + c.Author.Login + "\n");
                        changelog += s;
                    }
                    catch
                    {
                        changelog += ("#" + c.Sha.Substring(0, 7) + "@" + currentRepo + '\\' + currentBranch + "\n");
                    }
                    changelog += c.Commit.Message;
                }
                changelog += "\n\n========================================================";
                MessageBox.Show("Canary successfully updated from #" + oldSha.Substring(0, 7) + " to #" + newSha.Substring(0, 7)); // For some reason, without this, the changelog window never shows.
                CanaryChangelogViewer logWindow = new CanaryChangelogViewer(newSha.Substring(0, 7), changelog);
                logWindow.ShowDialog();
                DirectoryInfo CanaryDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");
                CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Canary Changelog could not be shown:\n" + e.Message);
                return;
            }
        }

        public static async Task KillOpenWindows()
        {
        //Find and close the BrawlCrate application that will be overwritten
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
            else if (p != null && p != default(Process))
            {
                p.Kill();
            }
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
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active") && !Updater.currentRepo.Equals(Updater.mainRepo, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Issue reporter does not allow reporting issues from forks. Please contact the owner of the repository to report your issue.");
                return;
            }
            try
            {
                Octokit.Credentials cr = new Credentials(System.Text.Encoding.Default.GetString(_rawData));
                GitHubClient github = new GitHubClient(new Octokit.ProductHeaderValue("BrawlCrate")) { Credentials = cr };
                IReadOnlyList<Release> releases = null;
                IReadOnlyList<Issue> issues = null;
                if (!TagName.StartsWith("BrawlCrate Canary", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        releases = await github.Repository.Release.GetAll("soopercool101", "BrawlCrate");

                        // Remove all pre-release (Documentation) versions from the list
                        releases = releases.Where(r => !r.Prerelease).ToList();

                        issues = await github.Issue.GetAllForRepository("BrawlCrate", "BrawlCrateIssues");
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
                    case "-n": // Update in new folder
                        somethingDone = true;
                        Task t1 = Updater.UpdateCheck(false);
                        t1.Wait();
                        break;
                    case "-bu": //BrawlCrate update call
                        somethingDone = true;
                        Task t2 = Updater.CheckUpdates(args[1], args[5], args[2] != "0", args[3] != "0", args[4] != "0");
                        t2.Wait();
                        break;
                    case "-buc": //BrawlCrate Canary update call
                        somethingDone = true;
                        Task t2c = Updater.CheckCanaryUpdate(args[2], args[1] != "0");
                        t2c.Wait();
                        break;
                    case "-bi": //BrawlCrate issue call
                        somethingDone = true;
                        Task t3 = BugSquish.CreateIssue(args[1], args[2], args[3], args[4], args[5]);
                        t3.Wait();
                        break;
                    case "-bcommitTime": //Called on build to ensure time is saved
                        somethingDone = true;
                        string t4arg = null;
                        if (args.Length > 1)
                        {
                            t4arg = args[1];
                        }

                        Task t4 = Updater.WriteCanaryTime(t4arg);
                        t4.Wait();
                        break;
                    case "-dlCanary": // Force download the latest Canary build
                        somethingDone = true;
                        Task t5 = Updater.ForceDownloadCanary(args[1]);
                        t5.Wait();
                        break;
                    case "-dlStable": // Force download the latest Stable build
                        somethingDone = true;
                        Task t6 = Updater.ForceDownloadRelease(args[1]);
                        t6.Wait();
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
                Task t = Updater.UpdateCheck(true);
                t.Wait();
            }

            if (!somethingDone)
            {
                Console.WriteLine(Usage);
            }
        }
    }
}
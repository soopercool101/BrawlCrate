using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public static class IssueReporter
    {
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
                Issue x = null;
                Credentials cr = new Credentials(Encoding.Default.GetString(Program.RawData));
                GitHubClient github = new GitHubClient(new ProductHeaderValue("BrawlCrate")) {Credentials = cr};
                IReadOnlyList<Issue> issues = null;
                if (!TagName.ToLower().Contains("canary"))
                {
                    IReadOnlyList<Release> releases;
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
                        //This issue may have been fixed by now. Tell the user to update to be allowed to submit reports.

                        DialogResult UpdateResult =
                            MessageBox.Show(
                                releases[0].Name +
                                " is available!\nYou cannot submit bug reports using an older version of the program.\nUpdate now?",
                                "An update is available", MessageBoxButtons.YesNo);
                        if (UpdateResult == DialogResult.Yes)
                        {
                            Task t = Updater.ForceDownloadStable("");
                            t.Wait();
                        }

                        return;
                    }
                }

                TagName += $" {Updater.platform}";
                bool found = false;
                if (issues != null && !string.IsNullOrEmpty(StackTrace))
                {
                    foreach (Issue i in issues)
                    {
                        if (i.State == ItemState.Open)
                        {
                            string desc = i.Body;
                            if (desc.Contains(StackTrace) &&
                                desc.Contains(ExceptionMessage))
                            {
                                found = true;
                                IssueUpdate update = i.ToUpdate();

                                update.Body =
                                    Title +
                                    Environment.NewLine +
                                    Description +
                                    Environment.NewLine +
                                    Environment.NewLine +
                                    TagName +
                                    Environment.NewLine +
                                    i.Body;

                                x = await github.Issue.Update("BrawlCrate", "BrawlCrateIssues", i.Number, update);
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
                            "```" +
                            Environment.NewLine +
                            ExceptionMessage +
                            Environment.NewLine +
                            StackTrace +
                            Environment.NewLine +
                            "```"
                    };
                    x = await github.Issue.Create("BrawlCrate", "BrawlCrateIssues", issue);
                }

                if (x != null)
                {
                    MessageBox.Show(
                        $"Your issue can be found at {x.HtmlUrl}. Please add any clarification on the issue there",
                        "Issue Reported");
                }
            }
            catch
            {
                MessageBox.Show("The application was unable to retrieve permission to send this issue.");
            }
        }
    }
}

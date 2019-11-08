using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public partial class IssueDialog : Form
    {
        private readonly Exception _exception;

        public IssueDialog(Exception e, List<ResourceNode> edited)
        {
            _exception = e;

            InitializeComponent();

            Text = e.GetType().ToString();
            txtStack.Text = e.Message + ' ' + (e.InnerException?.Message ?? "") + '\n' + e.StackTrace;

            lstChangedFiles.Visible =
                lblChangedFiles.Visible =
                    spltChangedFiles.Visible =
                        edited != null && edited.Count > 0;

            lstChangedFiles.Items.AddRange(edited.ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chkForceClose.Checked)
            {
                try
                {
                    if (Program.CanRunDiscordRPC)
                    {
                        BrawlCrate.Discord.DiscordRpc.ClearPresence();
                        BrawlCrate.Discord.DiscordRpc.Shutdown();
                    }
                }
                catch
                {
                    // Discord RPC doesn't need to work always
                }

                Environment.Exit(0);
            }

            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || txtTitle.ForeColor == Color.Gray)
            {
                MessageBox.Show("Please write a short summary for this bug report in the title box before sending.");
                return;
            }

            //Send the issue to the repository
            if (Program.CanRunGithubApp(true, out string path))
            {
#if CANARY
                string programTitle = Program.AssemblyTitleFull;
#else
                string programTitle = Program.TagName;
#endif
                string exceptionMessage = _exception.Message.Replace("\"", "\\\"");
                // Add inner exception if available
                if (_exception.InnerException != null)
                {
                    exceptionMessage += " ";
                    exceptionMessage += _exception.InnerException.Message.Replace("\"", "\\\"");
                }

                string args = string.Format("-bi \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"",
                    programTitle,
                    exceptionMessage,
                    _exception.StackTrace.Replace("\"", "\\\""),
                    txtTitle.Text.Replace("\"", "\\\""),
                    string.IsNullOrEmpty(txtDescription.Text) || txtDescription.ForeColor == Color.Gray
                        ? ""
                        : txtDescription.Text.Replace("\"", "\\\""));

                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = args
                });
            }
            else
            {
                MessageBox.Show(
                    "An error was thrown when sending the issue. Make sure the Updater.exe is undisturbed.");
            }

            if (chkForceClose.Checked)
            {
                Environment.Exit(0);
            }

            Close();
        }

        private void txtTitle_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                txtTitle.ForeColor = Color.Gray;
                txtTitle.Text = "Write a quick one-sentence summary of the bug";
            }
        }

        private void txtTitle_Enter(object sender, EventArgs e)
        {
            if (txtTitle.ForeColor == Color.Gray)
            {
                txtTitle.ForeColor = Color.Black;
                txtTitle.Text = "";
            }
        }

        private void txtDescription_Enter(object sender, EventArgs e)
        {
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.ForeColor = Color.Gray;
                txtDescription.Text =
                    "Explain in detail what you were doing that caused the bug. Reproducable steps and/or links to files that were worked on will make the bug much easier to fix. This will be posted publicly at https://github.com/BrawlCrate/BrawlCrateIssues/issues, so do not put any personal information here. It may be beneficial to you to sign your report with a username unless you wish to stay anonymous. It is also very helpful to report the issue on our Discord at https://discord.gg/s7c8763";
            }
        }

        private bool Save(ResourceNode r)
        {
            if (r._origPath == null)
            {
                return SaveAs(r);
            }

            r.Merge(ModifierKeys == (Keys.Control | Keys.Shift));
            r.Export(r._origPath);
            r.IsDirty = false;
            return true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r = lstChangedFiles.SelectedItem as ResourceNode;
            if (r != null && Save(r))
            {
                MessageBox.Show("Successfully saved " + r.Name);
            }
            else
            {
                MessageBox.Show(r.Name + " was not saved successfully.");
            }
        }

        private bool SaveAs(ResourceNode r)
        {
            if (r != null)
            {
                using (SaveFileDialog d = new SaveFileDialog())
                {
                    d.InitialDirectory = r._origPath.Substring(0, r._origPath.LastIndexOf('\\'));
                    d.Filter = string.Format("(*{0})|*{0}", Path.GetExtension(r._origPath));
                    d.Title = "Please choose a location to save this file.";
                    if (d.ShowDialog(this) == DialogResult.OK)
                    {
                        r.Merge();
                        r.Export(d.FileName);
                        return true;
                    }
                }
            }

            return false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResourceNode r = lstChangedFiles.SelectedItem as ResourceNode;
            if (r != null && SaveAs(r))
            {
                MessageBox.Show("Successfully saved " + r.Name);
            }
            else
            {
                MessageBox.Show(r.Name + " was not saved successfully.");
            }
        }

        private void ctxFile_Opening(object sender, CancelEventArgs e)
        {
            ResourceNode file = lstChangedFiles.SelectedItem as ResourceNode;
            if (file != null)
            {
                saveToolStripMenuItem.Enabled = file.IsDirty;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void txtDescription_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void txtDescription_MouseUp(object sender, MouseEventArgs e)
        {
            if (txtDescription.ForeColor == Color.Gray)
            {
                txtDescription.ForeColor = Color.Black;
                txtDescription.Text = "";
            }
        }
    }
}
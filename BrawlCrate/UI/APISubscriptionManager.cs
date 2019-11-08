using BrawlCrate.ExternalInterfacing;
using BrawlLib.Internal.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public class APISubscriptionManager : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpdateSubscriptions = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpBoxSubscriptions = new System.Windows.Forms.GroupBox();
            this.lstSubs = new System.Windows.Forms.ListView();
            this.subHeader = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.tabsSubInfo = new System.Windows.Forms.TabControl();
            this.tabReadMe = new System.Windows.Forms.TabPage();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.txtReadMe = new System.Windows.Forms.RichTextBox();
            this.lblLastUpdated = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tabScripts = new System.Windows.Forms.TabPage();
            this.lstScripts = new System.Windows.Forms.ListView();
            this.tabLicense = new System.Windows.Forms.TabPage();
            this.txtLicense = new System.Windows.Forms.RichTextBox();
            this.btnAddSub = new System.Windows.Forms.Button();
            this.filesHeader = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize) (this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpBoxSubscriptions.SuspendLayout();
            this.tabsSubInfo.SuspendLayout();
            this.tabReadMe.SuspendLayout();
            this.tabScripts.SuspendLayout();
            this.tabLicense.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdateSubscriptions
            // 
            this.btnUpdateSubscriptions.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateSubscriptions.Location = new System.Drawing.Point(295, 500);
            this.btnUpdateSubscriptions.Name = "btnUpdateSubscriptions";
            this.btnUpdateSubscriptions.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateSubscriptions.TabIndex = 1;
            this.btnUpdateSubscriptions.Text = "Update";
            this.btnUpdateSubscriptions.UseVisualStyleBackColor = true;
            this.btnUpdateSubscriptions.Click += new System.EventHandler(this.BtnUpdateSubscriptions_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpBoxSubscriptions);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabsSubInfo);
            this.splitContainer1.Size = new System.Drawing.Size(382, 494);
            this.splitContainer1.SplitterDistance = 323;
            this.splitContainer1.TabIndex = 2;
            // 
            // grpBoxSubscriptions
            // 
            this.grpBoxSubscriptions.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxSubscriptions.Controls.Add(this.lstSubs);
            this.grpBoxSubscriptions.Location = new System.Drawing.Point(3, 3);
            this.grpBoxSubscriptions.Name = "grpBoxSubscriptions";
            this.grpBoxSubscriptions.Size = new System.Drawing.Size(375, 318);
            this.grpBoxSubscriptions.TabIndex = 0;
            this.grpBoxSubscriptions.TabStop = false;
            this.grpBoxSubscriptions.Text = "Active Subscriptions";
            // 
            // lstSubs
            // 
            this.lstSubs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.subHeader
            });
            this.lstSubs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSubs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstSubs.HideSelection = false;
            this.lstSubs.Location = new System.Drawing.Point(3, 16);
            this.lstSubs.MultiSelect = false;
            this.lstSubs.Name = "lstSubs";
            this.lstSubs.Size = new System.Drawing.Size(369, 299);
            this.lstSubs.TabIndex = 0;
            this.lstSubs.UseCompatibleStateImageBehavior = false;
            this.lstSubs.View = System.Windows.Forms.View.Details;
            this.lstSubs.ItemSelectionChanged +=
                new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.LstSubs_ItemChanged);
            this.lstSubs.Resize += new System.EventHandler(this.lstResize);
            // 
            // subHeader
            // 
            this.subHeader.Text = "Subscriptions";
            this.subHeader.Width = 365;
            // 
            // tabsSubInfo
            // 
            this.tabsSubInfo.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsSubInfo.Controls.Add(this.tabReadMe);
            this.tabsSubInfo.Controls.Add(this.tabScripts);
            this.tabsSubInfo.Controls.Add(this.tabLicense);
            this.tabsSubInfo.Location = new System.Drawing.Point(0, 0);
            this.tabsSubInfo.Name = "tabsSubInfo";
            this.tabsSubInfo.SelectedIndex = 0;
            this.tabsSubInfo.Size = new System.Drawing.Size(382, 167);
            this.tabsSubInfo.TabIndex = 0;
            // 
            // tabReadMe
            // 
            this.tabReadMe.BackColor = System.Drawing.SystemColors.Control;
            this.tabReadMe.Controls.Add(this.btnUninstall);
            this.tabReadMe.Controls.Add(this.txtReadMe);
            this.tabReadMe.Controls.Add(this.lblLastUpdated);
            this.tabReadMe.Controls.Add(this.lblVersion);
            this.tabReadMe.Location = new System.Drawing.Point(4, 22);
            this.tabReadMe.Name = "tabReadMe";
            this.tabReadMe.Padding = new System.Windows.Forms.Padding(3);
            this.tabReadMe.Size = new System.Drawing.Size(374, 141);
            this.tabReadMe.TabIndex = 2;
            this.tabReadMe.Text = "About";
            // 
            // btnUninstall
            // 
            this.btnUninstall.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.btnUninstall.Location = new System.Drawing.Point(290, 12);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(75, 23);
            this.btnUninstall.TabIndex = 4;
            this.btnUninstall.Text = "Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.BtnUninstall_Click);
            // 
            // txtReadMe
            // 
            this.txtReadMe.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReadMe.Location = new System.Drawing.Point(0, 44);
            this.txtReadMe.Name = "txtReadMe";
            this.txtReadMe.ReadOnly = true;
            this.txtReadMe.Size = new System.Drawing.Size(374, 94);
            this.txtReadMe.TabIndex = 3;
            this.txtReadMe.Text = "";
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.AutoSize = true;
            this.lblLastUpdated.Location = new System.Drawing.Point(7, 26);
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(74, 13);
            this.lblLastUpdated.TabIndex = 2;
            this.lblLastUpdated.Text = "Last Updated:";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(7, 7);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version:";
            // 
            // tabScripts
            // 
            this.tabScripts.BackColor = System.Drawing.SystemColors.Control;
            this.tabScripts.Controls.Add(this.lstScripts);
            this.tabScripts.Location = new System.Drawing.Point(4, 22);
            this.tabScripts.Name = "tabScripts";
            this.tabScripts.Padding = new System.Windows.Forms.Padding(3);
            this.tabScripts.Size = new System.Drawing.Size(374, 141);
            this.tabScripts.TabIndex = 1;
            this.tabScripts.Text = "Files";
            // 
            // lstScripts
            // 
            this.lstScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.filesHeader
            });
            this.lstScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstScripts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstScripts.HideSelection = false;
            this.lstScripts.Location = new System.Drawing.Point(3, 3);
            this.lstScripts.MultiSelect = false;
            this.lstScripts.Name = "lstScripts";
            this.lstScripts.Size = new System.Drawing.Size(368, 135);
            this.lstScripts.TabIndex = 0;
            this.lstScripts.UseCompatibleStateImageBehavior = false;
            this.lstScripts.View = System.Windows.Forms.View.Details;
            this.lstScripts.Resize += new System.EventHandler(this.lstResize);
            // 
            // tabLicense
            // 
            this.tabLicense.BackColor = System.Drawing.SystemColors.Control;
            this.tabLicense.Controls.Add(this.txtLicense);
            this.tabLicense.Location = new System.Drawing.Point(4, 22);
            this.tabLicense.Name = "tabLicense";
            this.tabLicense.Padding = new System.Windows.Forms.Padding(3);
            this.tabLicense.Size = new System.Drawing.Size(374, 141);
            this.tabLicense.TabIndex = 0;
            this.tabLicense.Text = "License";
            // 
            // txtLicense
            // 
            this.txtLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLicense.Location = new System.Drawing.Point(3, 3);
            this.txtLicense.Name = "txtLicense";
            this.txtLicense.ReadOnly = true;
            this.txtLicense.Size = new System.Drawing.Size(368, 135);
            this.txtLicense.TabIndex = 0;
            this.txtLicense.Text = "";
            this.txtLicense.WordWrap = false;
            // 
            // btnAddSub
            // 
            this.btnAddSub.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSub.Location = new System.Drawing.Point(214, 500);
            this.btnAddSub.Name = "btnAddSub";
            this.btnAddSub.Size = new System.Drawing.Size(75, 23);
            this.btnAddSub.TabIndex = 3;
            this.btnAddSub.Text = "Add";
            this.btnAddSub.UseVisualStyleBackColor = true;
            this.btnAddSub.Click += new System.EventHandler(this.BtnAddSub_Click);
            // 
            // filesHeader
            // 
            this.filesHeader.Text = "Files";
            // 
            // APISubscriptionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 529);
            this.Controls.Add(this.btnAddSub);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnUpdateSubscriptions);
            this.Name = "APISubscriptionManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BrawlAPI Subscription Manager";
            this.Shown += new System.EventHandler(this.APISubscriptionManager_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpBoxSubscriptions.ResumeLayout(false);
            this.tabsSubInfo.ResumeLayout(false);
            this.tabReadMe.ResumeLayout(false);
            this.tabReadMe.PerformLayout();
            this.tabScripts.ResumeLayout(false);
            this.tabLicense.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnUpdateSubscriptions;
        private SplitContainer splitContainer1;
        private TabControl tabsSubInfo;
        private TabPage tabLicense;
        private RichTextBox txtLicense;
        private TabPage tabScripts;
        private GroupBox grpBoxSubscriptions;
        private TabPage tabReadMe;
        private Label lblLastUpdated;
        private Label lblVersion;
        private RichTextBox txtReadMe;
        private ListView lstScripts;
        private ListView lstSubs;
        private Button btnUninstall;
        private ColumnHeader subHeader;
        private ColumnHeader filesHeader;
        private Button btnAddSub;

        public APISubscriptionManager()
        {
            InitializeComponent();
            Icon = BrawlLib.Properties.Resources.Icon;
        }

        private void APISubscriptionManager_Shown(object sender, EventArgs e)
        {
            RefreshList();
            splitContainer1.Panel2Collapsed = lstSubs.SelectedItems.Count < 1;
        }

        private void RefreshList()
        {
            lstSubs.Items.Clear();
            if (Directory.Exists(Program.ApiPath))
            {
                foreach (FileInfo repo in Directory.CreateDirectory(Program.ApiPath).GetFiles()
                                                   .Where(f => string.IsNullOrWhiteSpace(f.Extension)))
                {
                    string[] repoInfo = repo.Name.Split(' ');
                    if (repoInfo.Length == 2)
                    {
                        lstSubs.Items.Add(new APISubscription(repoInfo[0], repoInfo[1], repo.FullName));
                    }
                }
            }
        }

        private string selected = "";

        private void LstSubs_ItemChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            splitContainer1.Panel2Collapsed = lstSubs.SelectedItems.Count < 1;
            txtReadMe.Text = "";
            txtLicense.Text = "";
            selected = "";
            lblVersion.Text = "Version:";
            lblLastUpdated.Text = "Last Updated:";
            lstScripts.Items.Clear();
            if (e?.Item is APISubscription a)
            {
                selected = a.Name;
                txtReadMe.Text = a.ReadMe;
                txtLicense.Text = a.License;
                lblVersion.Text = $"Version: {a.Version}";
                lblLastUpdated.Text = $"Last Updated: {a.LastUpdateDate}";
                foreach (string s in a.ManagedScripts)
                {
                    lstScripts.Items.Add(s);
                }
            }
        }

        private void BtnAddSub_Click(object sender, EventArgs e)
        {
            using (StringInputDialog d = new StringInputDialog("Github link to a repository:", ""))
            {
                if (d.ShowDialog(this) == DialogResult.OK)
                {
                    Regex rgx = new Regex(".*[Gg][Ii][Tt][Hh][Uu][Bb][.][Cc][Oo][Mm]/([^/\n]+)/([^/\n]+)[^\n]*");
                    Match m = rgx.Match(d.resultString);
                    if (m.Success && m.Groups.Count == 3)
                    {
                        string repoOwner = m.Groups[1].Value;
                        string repoName = m.Groups[2].Value;
                        if (repoOwner.Equals("soopercool101", StringComparison.OrdinalIgnoreCase) ||
                            MessageBox.Show(
                                $"Warning: The {repoOwner}/{repoName} repository is not affiliated with the BrawlCrate Developement Team.\n" +
                                "You should only install scripts from sources you trust. Would you like to proceed?",
                                "API Subscription Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                            DialogResult.Yes)
                        {
                            UpdaterHelper.BrawlAPIInstallUpdate(true, repoOwner, repoName, true);
                            GetNewFiles();
                            RefreshList();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"{d.resultString} is not a valid GitHub repository.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnUpdateSubscriptions_Click(object sender, EventArgs e)
        {
            UpdaterHelper.BrawlAPICheckUpdates(true, true);
            GetNewFiles();
            RefreshList();
        }

        private void BtnUninstall_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(selected))
            {
                string[] repoData = selected.Split('/');
                UpdaterHelper.BrawlAPIUninstall(true, repoData[0], repoData[1], true);
                RefreshList();
                LstSubs_ItemChanged(sender, null);
            }
        }

        private void lstResize(object sender, EventArgs e)
        {
            subHeader.Width = Math.Max(0, lstSubs.Width - 15);
            filesHeader.Width = Math.Max(0, lstScripts.Width - 15);
        }

        public static void GetNewFiles()
        {
            //DepreciatedReplacementStrings.Keys.Any(s => e.Message.Contains(s))
            List<string> newLoaders = new List<string>();
            List<string> dangerousFiles = new List<string>();
            List<FileInfo> newFiles = new List<FileInfo>();
            foreach (FileInfo f in Directory.CreateDirectory(Program.ApiPath).GetFiles("*.new"))
            {
                newLoaders.AddRange(File.ReadAllLines(f.FullName).Where(o => o.StartsWith("Loaders/")));
                newFiles.Add(f);
            }

            while (newFiles.Count > 0)
            {
                try
                {
                    newFiles[0].Delete();
                }
                catch
                {
                    // ignored. It should hopefully not happen, but disabling new loaders is more important.
                }

                newFiles.RemoveAt(0);
            }

            if (newLoaders.Count == 0)
            {
                return;
            }


            // Ensure that the format properly matches that used by the lists
            for (int i = 0; i < newLoaders.Count; i++)
            {
                newLoaders[i] = newLoaders[i].Replace('/', '\\');
                if (newLoaders[i].StartsWith("Loaders\\", StringComparison.OrdinalIgnoreCase))
                {
                    newLoaders[i] = newLoaders[i].Substring(8);
                }
            }

            string message =
                "The following loaders that are currently active have been added or changed by updates: \n\n";
            List<string> loadersToDisable = new List<string>();
            if (Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist)
            {
                foreach (string s in Properties.Settings.Default.APILoadersWhitelist)
                {
                    if (newLoaders.Any(o => o.Equals(s, StringComparison.OrdinalIgnoreCase)))
                    {
                        message += s + '\n';
                        loadersToDisable.Add(s);
                    }
                }
            }
            else
            {
                foreach (string s in newLoaders)
                {
                    if (!Properties.Settings.Default.APILoadersBlacklist.Cast<string>().ToArray()
                                   .Any(o => o.Equals(s, StringComparison.OrdinalIgnoreCase)))
                    {
                        message += s + '\n';
                        loadersToDisable.Add(s);
                    }
                }
            }

            if (loadersToDisable.Count > 0)
            {
                message += "\nLoaders downloaded from the internet may be harmful, so these have been deactivated. " +
                           "It is recommended that you review these files before enabling them again. " +
                           "Would you like to enable these files?\n\n" +
                           "The BrawlCrate team accepts no responsibility for any files that may harm your computer.";
                if (MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) !=
                    DialogResult.Yes)
                {
                    // Deactivate them
                    // Use whitelist if that is what is used currently:
                    if (Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist)
                    {
                        StringCollection sc = new StringCollection();
                        foreach (string s in Properties.Settings.Default.APILoadersWhitelist)
                        {
                            if (!loadersToDisable.Any(o => o.Equals(s, StringComparison.OrdinalIgnoreCase)))
                            {
                                sc.Add(s);
                            }
                        }

                        Properties.Settings.Default.APILoadersWhitelist = sc;
                        Properties.Settings.Default.Save();
                    }
                    else // Add them to the blacklist
                    {
                        StringCollection sc = Properties.Settings.Default.APILoadersBlacklist;
                        sc.AddRange(loadersToDisable.ToArray());

                        Properties.Settings.Default.APILoadersBlacklist = sc;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }
    }

    public class APISubscription : ListViewItem
    {
        public string Version { get; }
        public string LastUpdateDate { get; }
        public List<string> ManagedScripts { get; }
        public string ReadMe { get; }
        public string License { get; }

        public APISubscription(string repoOwner, string repoName, string path)
        {
            Name = $"{repoOwner}/{repoName}";
            ReadMe = "";
            License = "";
            if (File.Exists($"{path} README.txt"))
            {
                ReadMe = File.ReadAllText($"{path} README.txt");
            }

            if (File.Exists($"{path} LICENSE.txt"))
            {
                License = File.ReadAllText($"{path} LICENSE.txt");
            }

            string[] lines = File.ReadAllLines(path);
            // Line 0:  Release Tag. This is checked against to see if there is a new update for the repo.
            // Line 1:  Release Target Commitish. Used to allow continuous integration repos to work.
            // Line 2:  Update date (not used by updater, used instead to view info)
            // Line 3+: Each line is a relative path to a file from the installation.
            //          This is used to delete relevant files when updating,
            //          in case a file is moved or deleted by the update intentionally.
            Version = lines[0];
            LastUpdateDate = lines[2];
            ManagedScripts = new List<string>();
            for (int i = 3; i < lines.Length; i++)
            {
                ManagedScripts.Add(lines[i]);
            }

            Text = ToString();
        }

        public override string ToString()
        {
            return $"{Name} ({Version})";
        }
    }
}
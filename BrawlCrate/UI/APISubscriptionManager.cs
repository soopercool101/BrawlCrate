using BrawlCrate.ExternalInterfacing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public class APISubscriptionManager : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
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
            this.btnUpdateSubscriptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
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
            this.grpBoxSubscriptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
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
            this.lstSubs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSubs.HideSelection = false;
            this.lstSubs.Location = new System.Drawing.Point(3, 16);
            this.lstSubs.MultiSelect = false;
            this.lstSubs.Name = "lstSubs";
            this.lstSubs.Size = new System.Drawing.Size(369, 299);
            this.lstSubs.TabIndex = 0;
            this.lstSubs.UseCompatibleStateImageBehavior = false;
            this.lstSubs.View = System.Windows.Forms.View.List;
            this.lstSubs.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.LstSubs_ItemChanged);
            // 
            // tabsSubInfo
            // 
            this.tabsSubInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
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
            this.btnUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.txtReadMe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
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
            this.lstScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstScripts.HideSelection = false;
            this.lstScripts.Location = new System.Drawing.Point(3, 3);
            this.lstScripts.Name = "lstScripts";
            this.lstScripts.Size = new System.Drawing.Size(368, 135);
            this.lstScripts.TabIndex = 0;
            this.lstScripts.UseCompatibleStateImageBehavior = false;
            this.lstScripts.View = System.Windows.Forms.View.List;
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
            this.btnAddSub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSub.Location = new System.Drawing.Point(214, 500);
            this.btnAddSub.Name = "btnAddSub";
            this.btnAddSub.Size = new System.Drawing.Size(75, 23);
            this.btnAddSub.TabIndex = 3;
            this.btnAddSub.Text = "Add";
            this.btnAddSub.UseVisualStyleBackColor = true;
            this.btnAddSub.Click += new System.EventHandler(this.BtnAddSub_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
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

        private System.Windows.Forms.Button btnUpdateSubscriptions;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabsSubInfo;
        private System.Windows.Forms.TabPage tabLicense;
        private System.Windows.Forms.RichTextBox txtLicense;
        private System.Windows.Forms.TabPage tabScripts;
        private System.Windows.Forms.GroupBox grpBoxSubscriptions;
        private System.Windows.Forms.TabPage tabReadMe;
        private System.Windows.Forms.Label lblLastUpdated;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.RichTextBox txtReadMe;
        private System.Windows.Forms.ListView lstScripts;
        private System.Windows.Forms.ListView lstSubs;
        private Button btnUninstall;
        private System.Windows.Forms.Button btnAddSub;

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
            lstSubs.Clear();
            if (Directory.Exists(Program.ApiPath))
            {
                foreach (FileInfo repo in Directory.CreateDirectory(Program.ApiPath).GetFiles()
                                                   .Where(f => string.IsNullOrWhiteSpace(f.Extension)))
                {
                    string[] repoInfo = repo.Name.Split(' ');
                    if (repoInfo.Length == 2)
                    {
                        lstSubs.Items.Add(new APISubscription(repoInfo[0], repoInfo[1], repo.FullName)
                            { Text = $"{repoInfo[0]}/{repoInfo[1]}" });
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
            lstScripts.Clear();
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

        private async void BtnAddSub_Click(object sender, EventArgs e)
        {
            using (TwoInputStringDialog d = new TwoInputStringDialog())
            {
                if (d.ShowDialog(null, "Add subscription", "Repository Owner", "", "Repository Name", "") ==
                    DialogResult.OK)
                {
                    await UpdaterHelper.BrawlAPIInstallUpdate(d.InputText1, d.InputText2, true);
                    RefreshList();
                }
            }
        }

        private async void BtnUpdateSubscriptions_Click(object sender, EventArgs e)
        {
            await UpdaterHelper.BrawlAPICheckUpdates(true);
        }

        private async void BtnUninstall_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(selected))
            {
                string[] repoData = selected.Split('/');
                await UpdaterHelper.BrawlAPIUninstall(repoData[0], repoData[1], true);
                RefreshList();
                LstSubs_ItemChanged(sender, null);
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
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

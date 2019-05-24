using BrawlLib.SSBB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace BrawlCrate
{
    internal class SettingsDialog : Form
    {
        private bool _updating;

        static SettingsDialog()
        {
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
            {
                if (info._forEditing)
                {
                    foreach (string s in info._extensions)
                    {
                        _assocList.Add(FileAssociation.Get("." + s));
                        _typeList.Add(FileType.Get("SSBB." + s.ToUpper()));
                    }
                }
            }
        }

        private static readonly List<FileAssociation> _assocList = new List<FileAssociation>();
        private static readonly List<FileType> _typeList = new List<FileType>();
        private CheckBox chkUpdatesOnStartup;

        private CheckBox chkShowPropDesc;

        public SettingsDialog()
        {
            InitializeComponent();

            chkUpdatesOnStartup.Enabled = chkUpdatesOnStartup.Visible =
                MainForm.Instance.checkForUpdatesToolStripMenuItem.Enabled;

            listView1.Items.Clear();
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
            {
                if (info._forEditing)
                {
                    foreach (string s in info._extensions)
                    {
                        listView1.Items.Add(new ListViewItem() { Text = string.Format("{0} (*.{1})", info._name, s) });
                    }
                }
            }
        }

        private void Apply()
        {
            try
            {
                bool check;
                int index = 0;
                foreach (ListViewItem i in listView1.Items)
                {
                    if ((check = i.Checked) != (bool)i.Tag)
                    {
                        if (check)
                        {
                            _assocList[index].FileType = _typeList[index];
                            _typeList[index].SetCommand("open", string.Format("\"{0}\" \"%1\"", Program.FullPath));
                        }
                        else
                        {
                            _typeList[index].Delete();
                            _assocList[index].Delete();
                        }
                        i.Tag = check;
                    }
                    index++;
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(null, "Unable to access the registry to set file associations.\nRun the program as administrator and try again.", "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnApply.Enabled = false;
            }
        }
        private void SettingsDialog_Shown(object sender, EventArgs e)
        {
            int index = 0;
            string cmd;
            foreach (ListViewItem i in listView1.Items)
            {
                try
                {
                    if ((_typeList[index] == _assocList[index].FileType) &&
                        (!string.IsNullOrEmpty(cmd = _typeList[index].GetCommand("open"))) &&
                        (cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        i.Tag = i.Checked = true;
                    }
                    else
                    {
                        i.Tag = i.Checked = false;
                    }
                }
                catch
                {

                }
                index++;
            }

            _updating = true;
            chkUpdatesOnStartup.Checked = MainForm.Instance.CheckUpdatesOnStartup;
            chkShowPropDesc.Checked = MainForm.Instance.DisplayPropertyDescriptionsWhenAvailable;
            _updating = false;
            btnApply.Enabled = false;
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            btnApply.Enabled = true;
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            Apply();
        }
        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (btnApply.Enabled)
            {
                Apply();
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region Designer

        private GroupBox groupBox1;
        private ListView listView1;
        private CheckBox checkBox1;
        private Button btnOkay;
        private Button btnCancel;
        private Button btnApply;
        private ColumnHeader columnHeader1;

        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("File Types", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Resource Types", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("ARChive Pack (*.pac)");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Compressed ARChive Pack (*.pcs)");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("ARChive (*.arc)");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Compressed ARChive (*.szs)");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Resource Pack (*.brres)");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Model Pack (*.brmdl)");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Texture Pack (*.brtex)");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("MSBin Message List (*.msbin)");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Sound Archive (*.brsar)");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Sound Stream (*.brstm)");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("Texture (*.tex0)");
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem("Palette (*.plt0)");
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem("Model (*.mdl0)");
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem("Model Animation (*.chr0)");
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem("Texture Animation (*.srt0)");
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem("Vertex Morph (*.shp0)");
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem("Texture Pattern (*.pat0)");
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem("Bone Visibility (*.vis0)");
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem("Scene Settings (*.scn0)");
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem("Color Sequence (*.clr0)");
            System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem("Effect List (*.efls)");
            System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem("Effect Parameters (*.breff)");
            System.Windows.Forms.ListViewItem listViewItem23 = new System.Windows.Forms.ListViewItem("Effect Textures (*.breft)");
            System.Windows.Forms.ListViewItem listViewItem24 = new System.Windows.Forms.ListViewItem("Sound Stream (*.brwsd)");
            System.Windows.Forms.ListViewItem listViewItem25 = new System.Windows.Forms.ListViewItem("Sound Bank (*.brbnk)");
            System.Windows.Forms.ListViewItem listViewItem26 = new System.Windows.Forms.ListViewItem("Sound Sequence (*.brseq)");
            System.Windows.Forms.ListViewItem listViewItem27 = new System.Windows.Forms.ListViewItem("Static Module (*.dol)");
            System.Windows.Forms.ListViewItem listViewItem28 = new System.Windows.Forms.ListViewItem("Relocatable Module (*.rel)");
            System.Windows.Forms.ListViewItem listViewItem29 = new System.Windows.Forms.ListViewItem("Texture Archive (*.tpl)");
            groupBox1 = new System.Windows.Forms.GroupBox();
            checkBox1 = new System.Windows.Forms.CheckBox();
            listView1 = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            btnOkay = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            btnApply = new System.Windows.Forms.Button();
            chkShowPropDesc = new System.Windows.Forms.CheckBox();
            chkUpdatesOnStartup = new System.Windows.Forms.CheckBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Controls.Add(listView1);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(329, 264);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "File Associations";
            // 
            // checkBox1
            // 
            checkBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            checkBox1.Location = new System.Drawing.Point(212, 13);
            checkBox1.Name = "checkBox1";
            checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            checkBox1.Size = new System.Drawing.Size(104, 20);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Check All";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
            // 
            // listView1
            // 
            listView1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            listView1.AutoArrange = false;
            listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            listView1.CheckBoxes = true;
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1});
            listViewGroup1.Header = "File Types";
            listViewGroup1.Name = "grpFileTypes";
            listViewGroup2.Header = "Resource Types";
            listViewGroup2.Name = "grpResTypes";
            listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listView1.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.Tag = "";
            listViewItem2.StateImageIndex = 0;
            listViewItem2.Tag = "";
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem5.Tag = "";
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem8.Tag = "";
            listViewItem9.StateImageIndex = 0;
            listViewItem10.StateImageIndex = 0;
            listViewItem11.StateImageIndex = 0;
            listViewItem12.StateImageIndex = 0;
            listViewItem13.StateImageIndex = 0;
            listViewItem14.StateImageIndex = 0;
            listViewItem15.StateImageIndex = 0;
            listViewItem16.StateImageIndex = 0;
            listViewItem17.StateImageIndex = 0;
            listViewItem18.StateImageIndex = 0;
            listViewItem19.StateImageIndex = 0;
            listViewItem20.StateImageIndex = 0;
            listViewItem21.StateImageIndex = 0;
            listViewItem22.StateImageIndex = 0;
            listViewItem23.StateImageIndex = 0;
            listViewItem24.StateImageIndex = 0;
            listViewItem25.StateImageIndex = 0;
            listViewItem26.StateImageIndex = 0;
            listViewItem27.StateImageIndex = 0;
            listViewItem28.StateImageIndex = 0;
            listViewItem29.StateImageIndex = 0;
            listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19,
            listViewItem20,
            listViewItem21,
            listViewItem22,
            listViewItem23,
            listViewItem24,
            listViewItem25,
            listViewItem26,
            listViewItem27,
            listViewItem28,
            listViewItem29});
            listView1.Location = new System.Drawing.Point(3, 37);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(323, 221);
            listView1.TabIndex = 6;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(listView1_ItemChecked);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 300;
            // 
            // btnOkay
            // 
            btnOkay.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnOkay.Location = new System.Drawing.Point(91, 328);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new System.EventHandler(btnOkay_Click);
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnCancel.Location = new System.Drawing.Point(172, 328);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // btnApply
            // 
            btnApply.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnApply.Location = new System.Drawing.Point(253, 328);
            btnApply.Name = "btnApply";
            btnApply.Size = new System.Drawing.Size(75, 23);
            btnApply.TabIndex = 3;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += new System.EventHandler(btnApply_Click);
            // 
            // chkShowPropDesc
            // 
            chkShowPropDesc.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            chkShowPropDesc.AutoSize = true;
            chkShowPropDesc.Location = new System.Drawing.Point(11, 301);
            chkShowPropDesc.Name = "chkShowPropDesc";
            chkShowPropDesc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            chkShowPropDesc.Size = new System.Drawing.Size(317, 21);
            chkShowPropDesc.TabIndex = 7;
            chkShowPropDesc.Text = "Show property description box when available";
            chkShowPropDesc.UseVisualStyleBackColor = true;
            chkShowPropDesc.CheckedChanged += new System.EventHandler(chkShowPropDesc_CheckedChanged);
            // 
            // chkUpdatesOnStartup
            // 
            chkUpdatesOnStartup.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            chkUpdatesOnStartup.AutoSize = true;
            chkUpdatesOnStartup.Location = new System.Drawing.Point(115, 278);
            chkUpdatesOnStartup.Name = "chkUpdatesOnStartup";
            chkUpdatesOnStartup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            chkUpdatesOnStartup.Size = new System.Drawing.Size(213, 21);
            chkUpdatesOnStartup.TabIndex = 8;
            chkUpdatesOnStartup.Text = "Check for updates on startup";
            chkUpdatesOnStartup.UseVisualStyleBackColor = true;
            chkUpdatesOnStartup.CheckedChanged += new System.EventHandler(chkUpdatesOnStartup_CheckedChanged);
            // 
            // SettingsDialog
            // 
            ClientSize = new System.Drawing.Size(353, 363);
            Controls.Add(chkUpdatesOnStartup);
            Controls.Add(chkShowPropDesc);
            Controls.Add(btnApply);
            Controls.Add(btnCancel);
            Controls.Add(btnOkay);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Name = "SettingsDialog";
            Text = "Settings";
            Shown += new System.EventHandler(SettingsDialog_Shown);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                bool check = checkBox1.Checked;
                foreach (ListViewItem i in listView1.Items)
                {
                    i.Checked = check;
                }
            }
        }

        private void chkShowPropDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.DisplayPropertyDescriptionsWhenAvailable = chkShowPropDesc.Checked;
            }
        }

        private void chkUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.CheckUpdatesOnStartup = chkUpdatesOnStartup.Checked;
            }
        }
    }
}
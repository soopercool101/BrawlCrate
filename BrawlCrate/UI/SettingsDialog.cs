using BrawlCrate.Discord;
using BrawlLib.SSBB;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate
{
    internal class SettingsDialog : Form
    {
        public bool _updating;

        static SettingsDialog()
        {
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
            {
                if (info.Associatable)
                {
                    foreach (string s in info.Extensions)
                    {
                        _assocList.Add(FileAssociation.Get("." + s));
                        _typeList.Add(FileType.Get("SSBB." + s.ToUpper()));
                    }
                }
            }
        }

        private static readonly List<FileAssociation> _assocList = new List<FileAssociation>();
        private static readonly List<FileType> _typeList = new List<FileType>();
        private CheckBox chkShowHex;
        private CheckBox chkDocUpdates;
        private CheckBox chkCanary;
        private TabControl tabControl1;
        private TabPage tabGeneral;
        private TabPage tabUpdater;
        private GroupBox updaterBehaviorGroupbox;
        private TabPage tabFileAssociations;
        private Button btnApply;
        private GroupBox associatiedFilesBox;
        private CheckBox chkBoxAssociateAll;
        private ListView lstViewFileAssociations;
        private ColumnHeader columnHeader1;
        private GroupBox grpBoxCanary;
        private RadioButton rdoAutoUpdate;
        private RadioButton rdoCheckManual;
        private RadioButton rdoCheckStartup;
        private GroupBox grpBoxMainFormGeneral;
        private Label lblAdminApproval;
        private TabPage tabCompression;
        private GroupBox groupBoxFighterCompression;
        private CheckBox chkBoxFighterPacDecompress;
        private CheckBox chkBoxFighterPcsCompress;
        private GroupBox groupBoxStageCompression;
        private CheckBox chkBoxStageCompress;
        private GroupBox groupBoxModuleCompression;
        private CheckBox chkBoxModuleCompress;
        private GroupBox grpBoxAudioGeneral;
        private CheckBox chkBoxAutoPlayAudio;
        private GroupBox grpBoxMDL0General;
        private CheckBox chkBoxMDL0Compatibility;
        private TabPage tabDiscord;
        private GroupBox grpBoxDiscordRPC;
        private CheckBox chkBoxEnableDiscordRPC;
        private GroupBox grpBoxDiscordRPCType;
        private RadioButton rdoDiscordRPCNameInternal;
        private RadioButton rdoDiscordRPCNameDisabled;
        private TextBox DiscordRPCCustomName;
        private RadioButton rdoDiscordRPCNameCustom;
        private RadioButton rdoDiscordRPCNameExternal;
        private GroupBox genericFileAssociationBox;
        private CheckBox binFileAssociation;
        private CheckBox datFileAssociation;
        private GroupBox grpBoxFileNameDisplayGeneral;
        private RadioButton rdoShowShortName;
        private RadioButton rdoShowFullPath;
        private Label lblRecentFiles;
        private NumericInputBox recentFileCountBox;
        private TabPage tabBrawlAPI;
        private GroupBox grpBoxAPIGeneral;
        private CheckBox chkBoxEnableAPI;
        private GroupBox grpBoxFSharpAPI;
        private Button btnFSharpBrowse;
        private Button btnFSharpDetect;
        private Label label2;
        private TextBox txtBoxFSharpPath;
        private GroupBox grpBoxPythonAPI;
        private Button btnPythonBrowse;
        private Button btnPythonDetect;
        private Label label1;
        private TextBox txtBoxPythonPath;
        private CheckBox chkBoxRenderARC;
        private CheckBox chkBoxRenderBRRES;
        private GroupBox groupBox1;
        private TextBox txtBoxDefaultBuildPath;
        private Label lblManagerDefaultPath;
        private Button btnManagerPathBrowse;
        private Label lblAPIRestartNeeded;
        private GroupBox grpBoxLoaders;
        private ListView lstViewLoaders;
        private ColumnHeader columnHeader2;
        private CheckBox chkBoxContextualLoop;
        private GroupBox grpBoxLoaderBehavior;
        private RadioButton rdoAPILoaderWhitelist;
        private RadioButton rdoAPILoaderBlacklist;
        private GroupBox grpBoxAPIUpdate;
        private Button btnManageSubscriptions;
        private CheckBox chkBoxUpdateAPI;
        private CheckBox chkShowPropDesc;

        public SettingsDialog()
        {
            InitializeComponent();
            Icon = BrawlLib.Properties.Resources.Icon;
            _updating = true;
#if DEBUG
            chkShowHex.Text = "Prioritize hex preview for nodes";
#endif
            tabUpdater.Enabled = true;
            tabUpdater.Visible = true;
            tabDiscord.Enabled = true;
            tabDiscord.Visible = true;
            lblAPIRestartNeeded.Visible = false;

            if (!Program.CanRunGithubApp(false, out _))
            {
                tabUpdater.Enabled = false;
                tabUpdater.Visible = false;
                tabControl1.TabPages.Remove(tabUpdater);
            }

            if (!Program.CanRunDiscordRPC)
            {
                tabDiscord.Enabled = false;
                tabDiscord.Visible = false;
                tabControl1.TabPages.Remove(tabDiscord);
            }

            lstViewFileAssociations.Items.Clear();
            lstViewLoaders.Items.Clear();
            foreach (SupportedFileInfo info in SupportedFilesHandler.Files)
            {
                if (info.Associatable)
                {
                    foreach (string s in info.Extensions)
                    {
                        lstViewFileAssociations.Items.Add(new ListViewItem {Text = $"{info.Name} (*.{s})"});
                    }
                }
            }

            foreach (FileInfo script in MainForm.GetScripts(Program.ApiLoaderPath))
            {
                ListViewItem i = new ListViewItem();
                i.Text = script.FullName.Substring(Program.ApiLoaderPath.Length).TrimStart('\\');
                lstViewLoaders.Items.Add(i);
            }

            _updating = false;
        }

        private void Apply()
        {
            try
            {
                int index = 0;
                foreach (ListViewItem i in lstViewFileAssociations.Items)
                {
                    bool check = i.Checked;
                    if (check != (bool) i.Tag)
                    {
                        if (check)
                        {
                            _assocList[index].FileType = _typeList[index];
                            _typeList[index].SetCommand("open", $"\"{Program.FullPath}\" \"%1\"");
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

                lstViewFileAssociations.Sort();
                if (datFileAssociation.Checked)
                {
                    FileAssociation.Get(".dat").FileType = FileType.Get("SSBB.DAT");
                    FileType.Get("SSBB.DAT").SetCommand("open", $"\"{Program.FullPath}\" \"%1\"");
                }
                else
                {
                    FileType.Get("SSBB.DAT").Delete();
                    FileAssociation.Get(".dat").Delete();
                }

                if (binFileAssociation.Checked)
                {
                    FileAssociation.Get(".bin").FileType = FileType.Get("SSBB.BIN");
                    FileType.Get("SSBB.BIN").SetCommand("open", $"\"{Program.FullPath}\" \"%1\"");
                }
                else
                {
                    FileType.Get("SSBB.BIN").Delete();
                    FileAssociation.Get(".bin").Delete();
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(null,
                    "Unable to access the registry to set file associations.\nRun the program as administrator and try again.",
                    "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblAdminApproval.Visible = true;
                btnApply.Visible = false;
                associatiedFilesBox.Enabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show(null,
                    "Unable to access the registry to set file associations.\nRun the program as administrator and try again.",
                    "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblAdminApproval.Visible = true;
                btnApply.Visible = false;
                associatiedFilesBox.Enabled = false;
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
            _updating = true;
            foreach (ListViewItem i in lstViewFileAssociations.Items)
            {
                try
                {
                    if (_typeList[index] == _assocList[index].FileType &&
                        !string.IsNullOrEmpty(cmd = _typeList[index].GetCommand("open")) &&
                        cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0)
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
                    // ignored
                }

                index++;
            }

            try
            {
                datFileAssociation.Checked = !string.IsNullOrEmpty(cmd = FileType.Get("SSBB.DAT").GetCommand("open")) &&
                                             cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch
            {
                // ignored
            }

            try
            {
                binFileAssociation.Checked = !string.IsNullOrEmpty(cmd = FileType.Get("SSBB.BIN").GetCommand("open")) &&
                                             cmd.IndexOf(Program.FullPath, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch
            {
                // ignored
            }

            chkDocUpdates.Checked = MainForm.Instance.GetDocumentationUpdates;
#if CANARY
            updaterBehaviorGroupbox.Enabled = false;
            chkCanary.Checked = true;
#else
            updaterBehaviorGroupbox.Enabled = true;
            chkCanary.Checked = false;
#endif
            if (MainForm.Instance.UpdateAutomatically)
            {
                rdoAutoUpdate.Checked = true;
            }
            else if (MainForm.Instance.CheckUpdatesOnStartup)
            {
                rdoCheckStartup.Checked = true;
            }
            else
            {
                rdoCheckManual.Checked = true;
            }

            rdoShowFullPath.Checked = MainForm.Instance.ShowFullPath;
            rdoShowShortName.Checked = !MainForm.Instance.ShowFullPath;
            chkShowPropDesc.Checked = MainForm.Instance.DisplayPropertyDescriptionsWhenAvailable;
            chkShowHex.Checked = MainForm.Instance.ShowHex;
            chkBoxAutoPlayAudio.Checked = MainForm.Instance.AutoPlayAudio;
            chkBoxFighterPacDecompress.Checked = MainForm.Instance.AutoDecompressFighterPAC;
            chkBoxFighterPcsCompress.Checked = MainForm.Instance.AutoCompressPCS;
            chkBoxStageCompress.Checked = MainForm.Instance.AutoCompressStages;
            chkBoxModuleCompress.Checked = MainForm.Instance.AutoCompressModules;
            chkBoxAutoPlayAudio.Checked = MainForm.Instance.AutoPlayAudio;
            chkBoxMDL0Compatibility.Checked = MainForm.Instance.CompatibilityMode;
            chkBoxRenderBRRES.Checked = MainForm.Instance.ShowBRRESPreviews;
            chkBoxRenderARC.Checked = MainForm.Instance.ShowARCPreviews;
            chkBoxContextualLoop.Checked = BrawlLib.Properties.Settings.Default.ContextualLoopAudio;
            recentFileCountBox.Value = Properties.Settings.Default.RecentFilesMax;
            chkBoxEnableAPI.Checked = Properties.Settings.Default.APIEnabled;
            txtBoxFSharpPath.Text = Properties.Settings.Default.FSharpInstallationPath;
            txtBoxPythonPath.Text = Properties.Settings.Default.PythonInstallationPath;
            txtBoxDefaultBuildPath.Text = Properties.Settings.Default.BuildPath;
            rdoAPILoaderWhitelist.Checked = Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist;
            rdoAPILoaderBlacklist.Checked = !Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist;
            chkBoxUpdateAPI.Checked = Properties.Settings.Default.APIAutoUpdate;
            RefreshLoaderList();
            grpBoxLoaderBehavior.Enabled = chkBoxEnableAPI.Checked;
            grpBoxPythonAPI.Enabled = chkBoxEnableAPI.Checked;
            grpBoxFSharpAPI.Enabled = chkBoxEnableAPI.Checked;
            grpBoxLoaders.Enabled = chkBoxEnableAPI.Checked;

            DiscordSettings.LoadSettings();
            grpBoxDiscordRPCType.Enabled = DiscordSettings.DiscordRPCEnabled;
            chkBoxEnableDiscordRPC.Checked = DiscordSettings.DiscordRPCEnabled;
            DiscordSettings.ModNameType? modNameType = Properties.Settings.Default.DiscordRPCNameType;
            switch (modNameType)
            {
                case DiscordSettings.ModNameType.Disabled:
                    rdoDiscordRPCNameDisabled.Checked = true;
                    break;
                case DiscordSettings.ModNameType.UserDefined:
                    rdoDiscordRPCNameCustom.Checked = true;
                    break;
                case DiscordSettings.ModNameType.AutoInternal:
                    rdoDiscordRPCNameInternal.Checked = true;
                    break;
                case DiscordSettings.ModNameType.AutoExternal:
                    rdoDiscordRPCNameExternal.Checked = true;
                    break;
                default:
                    rdoDiscordRPCNameDisabled.Checked = true;
                    Properties.Settings.Default.DiscordRPCNameType = DiscordSettings.ModNameType.Disabled;
                    Properties.Settings.Default.Save();
                    break;
            }

            DiscordRPCCustomName.Text = Properties.Settings.Default.DiscordRPCNameCustom;
            DiscordRPCCustomName.Enabled = rdoDiscordRPCNameCustom.Checked;
            DiscordRPCCustomName.ReadOnly = !rdoDiscordRPCNameCustom.Checked;

            _updating = false;
            CheckAdminAccess();
            btnApply.Enabled = false;
        }

        // Unimplemented
        private bool CheckAdminAccess()
        {
            try
            {
                lblAdminApproval.Visible = false;
                btnApply.Visible = true;
                associatiedFilesBox.Enabled = true;
                return true;
            }
            catch
            {
                lblAdminApproval.Visible = true;
                btnApply.Visible = false;
                associatiedFilesBox.Enabled = false;
                return false;
            }
        }

        private void RefreshLoaderList()
        {
            bool isUpdating = _updating;
            _updating = true;
            foreach (ListViewItem i in lstViewLoaders.Items)
            {
                i.Checked = Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist ?
                    Properties.Settings.Default.APILoadersWhitelist?.Contains(i.Text) ?? false :
                    !Properties.Settings.Default.APILoadersBlacklist?.Contains(i.Text) ?? true;
            }

            _updating = isUpdating;
        }

        private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnApply.Enabled = true;
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            Apply();
        }

        #region Designer

        private void InitializeComponent()
        {
            this.chkShowPropDesc = new System.Windows.Forms.CheckBox();
            this.chkShowHex = new System.Windows.Forms.CheckBox();
            this.chkDocUpdates = new System.Windows.Forms.CheckBox();
            this.chkCanary = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnManagerPathBrowse = new System.Windows.Forms.Button();
            this.txtBoxDefaultBuildPath = new System.Windows.Forms.TextBox();
            this.lblManagerDefaultPath = new System.Windows.Forms.Label();
            this.grpBoxMDL0General = new System.Windows.Forms.GroupBox();
            this.chkBoxRenderARC = new System.Windows.Forms.CheckBox();
            this.chkBoxRenderBRRES = new System.Windows.Forms.CheckBox();
            this.chkBoxMDL0Compatibility = new System.Windows.Forms.CheckBox();
            this.grpBoxAudioGeneral = new System.Windows.Forms.GroupBox();
            this.chkBoxContextualLoop = new System.Windows.Forms.CheckBox();
            this.chkBoxAutoPlayAudio = new System.Windows.Forms.CheckBox();
            this.grpBoxMainFormGeneral = new System.Windows.Forms.GroupBox();
            this.lblRecentFiles = new System.Windows.Forms.Label();
            this.recentFileCountBox = new System.Windows.Forms.NumericInputBox();
            this.grpBoxFileNameDisplayGeneral = new System.Windows.Forms.GroupBox();
            this.rdoShowShortName = new System.Windows.Forms.RadioButton();
            this.rdoShowFullPath = new System.Windows.Forms.RadioButton();
            this.tabCompression = new System.Windows.Forms.TabPage();
            this.groupBoxModuleCompression = new System.Windows.Forms.GroupBox();
            this.chkBoxModuleCompress = new System.Windows.Forms.CheckBox();
            this.groupBoxStageCompression = new System.Windows.Forms.GroupBox();
            this.chkBoxStageCompress = new System.Windows.Forms.CheckBox();
            this.groupBoxFighterCompression = new System.Windows.Forms.GroupBox();
            this.chkBoxFighterPacDecompress = new System.Windows.Forms.CheckBox();
            this.chkBoxFighterPcsCompress = new System.Windows.Forms.CheckBox();
            this.tabFileAssociations = new System.Windows.Forms.TabPage();
            this.genericFileAssociationBox = new System.Windows.Forms.GroupBox();
            this.binFileAssociation = new System.Windows.Forms.CheckBox();
            this.datFileAssociation = new System.Windows.Forms.CheckBox();
            this.lblAdminApproval = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.associatiedFilesBox = new System.Windows.Forms.GroupBox();
            this.chkBoxAssociateAll = new System.Windows.Forms.CheckBox();
            this.lstViewFileAssociations = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabBrawlAPI = new System.Windows.Forms.TabPage();
            this.lblAPIRestartNeeded = new System.Windows.Forms.Label();
            this.grpBoxLoaders = new System.Windows.Forms.GroupBox();
            this.lstViewLoaders = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpBoxFSharpAPI = new System.Windows.Forms.GroupBox();
            this.txtBoxFSharpPath = new System.Windows.Forms.TextBox();
            this.btnFSharpBrowse = new System.Windows.Forms.Button();
            this.btnFSharpDetect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.grpBoxPythonAPI = new System.Windows.Forms.GroupBox();
            this.txtBoxPythonPath = new System.Windows.Forms.TextBox();
            this.btnPythonBrowse = new System.Windows.Forms.Button();
            this.btnPythonDetect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpBoxAPIGeneral = new System.Windows.Forms.GroupBox();
            this.grpBoxLoaderBehavior = new System.Windows.Forms.GroupBox();
            this.rdoAPILoaderWhitelist = new System.Windows.Forms.RadioButton();
            this.rdoAPILoaderBlacklist = new System.Windows.Forms.RadioButton();
            this.chkBoxEnableAPI = new System.Windows.Forms.CheckBox();
            this.tabDiscord = new System.Windows.Forms.TabPage();
            this.grpBoxDiscordRPC = new System.Windows.Forms.GroupBox();
            this.chkBoxEnableDiscordRPC = new System.Windows.Forms.CheckBox();
            this.grpBoxDiscordRPCType = new System.Windows.Forms.GroupBox();
            this.DiscordRPCCustomName = new System.Windows.Forms.TextBox();
            this.rdoDiscordRPCNameCustom = new System.Windows.Forms.RadioButton();
            this.rdoDiscordRPCNameExternal = new System.Windows.Forms.RadioButton();
            this.rdoDiscordRPCNameInternal = new System.Windows.Forms.RadioButton();
            this.rdoDiscordRPCNameDisabled = new System.Windows.Forms.RadioButton();
            this.tabUpdater = new System.Windows.Forms.TabPage();
            this.grpBoxAPIUpdate = new System.Windows.Forms.GroupBox();
            this.btnManageSubscriptions = new System.Windows.Forms.Button();
            this.chkBoxUpdateAPI = new System.Windows.Forms.CheckBox();
            this.grpBoxCanary = new System.Windows.Forms.GroupBox();
            this.updaterBehaviorGroupbox = new System.Windows.Forms.GroupBox();
            this.rdoAutoUpdate = new System.Windows.Forms.RadioButton();
            this.rdoCheckManual = new System.Windows.Forms.RadioButton();
            this.rdoCheckStartup = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpBoxMDL0General.SuspendLayout();
            this.grpBoxAudioGeneral.SuspendLayout();
            this.grpBoxMainFormGeneral.SuspendLayout();
            this.grpBoxFileNameDisplayGeneral.SuspendLayout();
            this.tabCompression.SuspendLayout();
            this.groupBoxModuleCompression.SuspendLayout();
            this.groupBoxStageCompression.SuspendLayout();
            this.groupBoxFighterCompression.SuspendLayout();
            this.tabFileAssociations.SuspendLayout();
            this.genericFileAssociationBox.SuspendLayout();
            this.associatiedFilesBox.SuspendLayout();
            this.tabBrawlAPI.SuspendLayout();
            this.grpBoxLoaders.SuspendLayout();
            this.grpBoxFSharpAPI.SuspendLayout();
            this.grpBoxPythonAPI.SuspendLayout();
            this.grpBoxAPIGeneral.SuspendLayout();
            this.grpBoxLoaderBehavior.SuspendLayout();
            this.tabDiscord.SuspendLayout();
            this.grpBoxDiscordRPC.SuspendLayout();
            this.grpBoxDiscordRPCType.SuspendLayout();
            this.tabUpdater.SuspendLayout();
            this.grpBoxAPIUpdate.SuspendLayout();
            this.grpBoxCanary.SuspendLayout();
            this.updaterBehaviorGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowPropDesc
            // 
            this.chkShowPropDesc.AutoSize = true;
            this.chkShowPropDesc.Location = new System.Drawing.Point(10, 22);
            this.chkShowPropDesc.Name = "chkShowPropDesc";
            this.chkShowPropDesc.Size = new System.Drawing.Size(242, 17);
            this.chkShowPropDesc.TabIndex = 7;
            this.chkShowPropDesc.Text = "Show property description box when available";
            this.chkShowPropDesc.UseVisualStyleBackColor = true;
            this.chkShowPropDesc.CheckedChanged += new System.EventHandler(this.ChkShowPropDesc_CheckedChanged);
            // 
            // chkShowHex
            // 
            this.chkShowHex.AutoSize = true;
            this.chkShowHex.Location = new System.Drawing.Point(10, 45);
            this.chkShowHex.Name = "chkShowHex";
            this.chkShowHex.Size = new System.Drawing.Size(233, 17);
            this.chkShowHex.TabIndex = 9;
            this.chkShowHex.Text = "Show hexadecimal for files without previews";
            this.chkShowHex.UseVisualStyleBackColor = true;
            this.chkShowHex.CheckedChanged += new System.EventHandler(this.ChkShowHex_CheckedChanged);
            // 
            // chkDocUpdates
            // 
            this.chkDocUpdates.AutoSize = true;
            this.chkDocUpdates.Location = new System.Drawing.Point(10, 91);
            this.chkDocUpdates.Name = "chkDocUpdates";
            this.chkDocUpdates.Size = new System.Drawing.Size(180, 17);
            this.chkDocUpdates.TabIndex = 11;
            this.chkDocUpdates.Text = "Receive documentation updates";
            this.chkDocUpdates.UseVisualStyleBackColor = true;
            this.chkDocUpdates.CheckedChanged += new System.EventHandler(this.ChkDocUpdates_CheckedChanged);
            // 
            // chkCanary
            // 
            this.chkCanary.AutoSize = true;
            this.chkCanary.Location = new System.Drawing.Point(10, 22);
            this.chkCanary.Name = "chkCanary";
            this.chkCanary.Size = new System.Drawing.Size(263, 17);
            this.chkCanary.TabIndex = 13;
            this.chkCanary.Text = "Opt into BrawlCrate Canary (Experimental) updates";
            this.chkCanary.UseVisualStyleBackColor = true;
            this.chkCanary.CheckedChanged += new System.EventHandler(this.ChkCanary_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabCompression);
            this.tabControl1.Controls.Add(this.tabFileAssociations);
            this.tabControl1.Controls.Add(this.tabBrawlAPI);
            this.tabControl1.Controls.Add(this.tabDiscord);
            this.tabControl1.Controls.Add(this.tabUpdater);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 478);
            this.tabControl1.TabIndex = 48;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.ToggleUpdateOff);
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.ToggleUpdateOn);
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabGeneral.Controls.Add(this.groupBox1);
            this.tabGeneral.Controls.Add(this.grpBoxMDL0General);
            this.tabGeneral.Controls.Add(this.grpBoxAudioGeneral);
            this.tabGeneral.Controls.Add(this.grpBoxMainFormGeneral);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(365, 452);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnManagerPathBrowse);
            this.groupBox1.Controls.Add(this.txtBoxDefaultBuildPath);
            this.groupBox1.Controls.Add(this.lblManagerDefaultPath);
            this.groupBox1.Location = new System.Drawing.Point(8, 363);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(349, 73);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Managers";
            // 
            // btnManagerPathBrowse
            // 
            this.btnManagerPathBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManagerPathBrowse.Location = new System.Drawing.Point(319, 36);
            this.btnManagerPathBrowse.Name = "btnManagerPathBrowse";
            this.btnManagerPathBrowse.Size = new System.Drawing.Size(24, 24);
            this.btnManagerPathBrowse.TabIndex = 22;
            this.btnManagerPathBrowse.Text = "...";
            this.btnManagerPathBrowse.UseVisualStyleBackColor = true;
            this.btnManagerPathBrowse.Click += new System.EventHandler(this.BtnManagerPathBrowse_Click);
            // 
            // txtBoxDefaultBuildPath
            // 
            this.txtBoxDefaultBuildPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxDefaultBuildPath.Location = new System.Drawing.Point(11, 38);
            this.txtBoxDefaultBuildPath.Name = "txtBoxDefaultBuildPath";
            this.txtBoxDefaultBuildPath.Size = new System.Drawing.Size(302, 20);
            this.txtBoxDefaultBuildPath.TabIndex = 3;
            this.txtBoxDefaultBuildPath.Text = "(none)";
            this.txtBoxDefaultBuildPath.TextChanged += new System.EventHandler(this.TxtBoxDefaultBuildPath_TextChanged);
            // 
            // lblManagerDefaultPath
            // 
            this.lblManagerDefaultPath.AutoSize = true;
            this.lblManagerDefaultPath.Location = new System.Drawing.Point(8, 22);
            this.lblManagerDefaultPath.Name = "lblManagerDefaultPath";
            this.lblManagerDefaultPath.Size = new System.Drawing.Size(95, 13);
            this.lblManagerDefaultPath.TabIndex = 13;
            this.lblManagerDefaultPath.Text = "Default Build Path:";
            // 
            // grpBoxMDL0General
            // 
            this.grpBoxMDL0General.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxMDL0General.Controls.Add(this.chkBoxRenderARC);
            this.grpBoxMDL0General.Controls.Add(this.chkBoxRenderBRRES);
            this.grpBoxMDL0General.Controls.Add(this.chkBoxMDL0Compatibility);
            this.grpBoxMDL0General.Location = new System.Drawing.Point(8, 263);
            this.grpBoxMDL0General.Name = "grpBoxMDL0General";
            this.grpBoxMDL0General.Size = new System.Drawing.Size(349, 94);
            this.grpBoxMDL0General.TabIndex = 19;
            this.grpBoxMDL0General.TabStop = false;
            this.grpBoxMDL0General.Text = "Models";
            // 
            // chkBoxRenderARC
            // 
            this.chkBoxRenderARC.AutoSize = true;
            this.chkBoxRenderARC.Location = new System.Drawing.Point(10, 68);
            this.chkBoxRenderARC.Name = "chkBoxRenderARC";
            this.chkBoxRenderARC.Size = new System.Drawing.Size(146, 17);
            this.chkBoxRenderARC.TabIndex = 9;
            this.chkBoxRenderARC.Text = "Render previews for ARC";
            this.chkBoxRenderARC.UseVisualStyleBackColor = true;
            this.chkBoxRenderARC.CheckedChanged += new System.EventHandler(this.ChkBoxRenderARC_CheckedChanged);
            // 
            // chkBoxRenderBRRES
            // 
            this.chkBoxRenderBRRES.AutoSize = true;
            this.chkBoxRenderBRRES.Location = new System.Drawing.Point(10, 45);
            this.chkBoxRenderBRRES.Name = "chkBoxRenderBRRES";
            this.chkBoxRenderBRRES.Size = new System.Drawing.Size(161, 17);
            this.chkBoxRenderBRRES.TabIndex = 8;
            this.chkBoxRenderBRRES.Text = "Render previews for BRRES";
            this.chkBoxRenderBRRES.UseVisualStyleBackColor = true;
            this.chkBoxRenderBRRES.CheckedChanged += new System.EventHandler(this.ChkBoxRenderBRRES_CheckedChanged);
            // 
            // chkBoxMDL0Compatibility
            // 
            this.chkBoxMDL0Compatibility.AutoSize = true;
            this.chkBoxMDL0Compatibility.Location = new System.Drawing.Point(10, 22);
            this.chkBoxMDL0Compatibility.Name = "chkBoxMDL0Compatibility";
            this.chkBoxMDL0Compatibility.Size = new System.Drawing.Size(134, 17);
            this.chkBoxMDL0Compatibility.TabIndex = 7;
            this.chkBoxMDL0Compatibility.Text = "Use compatibility mode";
            this.chkBoxMDL0Compatibility.UseVisualStyleBackColor = true;
            this.chkBoxMDL0Compatibility.CheckedChanged += new System.EventHandler(this.ChkBoxMDL0Compatibility_CheckedChanged);
            // 
            // grpBoxAudioGeneral
            // 
            this.grpBoxAudioGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAudioGeneral.Controls.Add(this.chkBoxContextualLoop);
            this.grpBoxAudioGeneral.Controls.Add(this.chkBoxAutoPlayAudio);
            this.grpBoxAudioGeneral.Location = new System.Drawing.Point(8, 182);
            this.grpBoxAudioGeneral.Name = "grpBoxAudioGeneral";
            this.grpBoxAudioGeneral.Size = new System.Drawing.Size(349, 75);
            this.grpBoxAudioGeneral.TabIndex = 18;
            this.grpBoxAudioGeneral.TabStop = false;
            this.grpBoxAudioGeneral.Text = "Audio";
            // 
            // chkBoxContextualLoop
            // 
            this.chkBoxContextualLoop.AutoSize = true;
            this.chkBoxContextualLoop.Location = new System.Drawing.Point(10, 22);
            this.chkBoxContextualLoop.Name = "chkBoxContextualLoop";
            this.chkBoxContextualLoop.Size = new System.Drawing.Size(211, 17);
            this.chkBoxContextualLoop.TabIndex = 8;
            this.chkBoxContextualLoop.Text = "Loop preview for looping audio sources";
            this.chkBoxContextualLoop.UseVisualStyleBackColor = true;
            this.chkBoxContextualLoop.CheckedChanged += new System.EventHandler(this.ChkBoxContextualLoop_CheckedChanged);
            // 
            // chkBoxAutoPlayAudio
            // 
            this.chkBoxAutoPlayAudio.AutoSize = true;
            this.chkBoxAutoPlayAudio.Location = new System.Drawing.Point(10, 45);
            this.chkBoxAutoPlayAudio.Name = "chkBoxAutoPlayAudio";
            this.chkBoxAutoPlayAudio.Size = new System.Drawing.Size(171, 17);
            this.chkBoxAutoPlayAudio.TabIndex = 7;
            this.chkBoxAutoPlayAudio.Text = "Automatically play audio nodes";
            this.chkBoxAutoPlayAudio.UseVisualStyleBackColor = true;
            this.chkBoxAutoPlayAudio.CheckedChanged += new System.EventHandler(this.ChkBoxAutoPlayAudio_CheckedChanged);
            // 
            // grpBoxMainFormGeneral
            // 
            this.grpBoxMainFormGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxMainFormGeneral.Controls.Add(this.lblRecentFiles);
            this.grpBoxMainFormGeneral.Controls.Add(this.recentFileCountBox);
            this.grpBoxMainFormGeneral.Controls.Add(this.grpBoxFileNameDisplayGeneral);
            this.grpBoxMainFormGeneral.Controls.Add(this.chkShowPropDesc);
            this.grpBoxMainFormGeneral.Controls.Add(this.chkShowHex);
            this.grpBoxMainFormGeneral.Location = new System.Drawing.Point(8, 6);
            this.grpBoxMainFormGeneral.Name = "grpBoxMainFormGeneral";
            this.grpBoxMainFormGeneral.Size = new System.Drawing.Size(349, 170);
            this.grpBoxMainFormGeneral.TabIndex = 15;
            this.grpBoxMainFormGeneral.TabStop = false;
            this.grpBoxMainFormGeneral.Text = "Main Form";
            // 
            // lblRecentFiles
            // 
            this.lblRecentFiles.AutoSize = true;
            this.lblRecentFiles.Location = new System.Drawing.Point(8, 68);
            this.lblRecentFiles.Name = "lblRecentFiles";
            this.lblRecentFiles.Size = new System.Drawing.Size(120, 13);
            this.lblRecentFiles.TabIndex = 12;
            this.lblRecentFiles.Text = "Max Recent Files Count";
            // 
            // recentFileCountBox
            // 
            this.recentFileCountBox.Integer = true;
            this.recentFileCountBox.Integral = true;
            this.recentFileCountBox.Location = new System.Drawing.Point(134, 65);
            this.recentFileCountBox.MaximumValue = 3.402823E+38F;
            this.recentFileCountBox.MinimumValue = -3.402823E+38F;
            this.recentFileCountBox.Name = "recentFileCountBox";
            this.recentFileCountBox.Size = new System.Drawing.Size(100, 20);
            this.recentFileCountBox.TabIndex = 11;
            this.recentFileCountBox.Text = "0";
            this.recentFileCountBox.TextChanged += new System.EventHandler(this.RecentFileCountBox_TextChanged);
            // 
            // grpBoxFileNameDisplayGeneral
            // 
            this.grpBoxFileNameDisplayGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxFileNameDisplayGeneral.Controls.Add(this.rdoShowShortName);
            this.grpBoxFileNameDisplayGeneral.Controls.Add(this.rdoShowFullPath);
            this.grpBoxFileNameDisplayGeneral.Location = new System.Drawing.Point(6, 89);
            this.grpBoxFileNameDisplayGeneral.Name = "grpBoxFileNameDisplayGeneral";
            this.grpBoxFileNameDisplayGeneral.Size = new System.Drawing.Size(337, 75);
            this.grpBoxFileNameDisplayGeneral.TabIndex = 10;
            this.grpBoxFileNameDisplayGeneral.TabStop = false;
            this.grpBoxFileNameDisplayGeneral.Text = "Filename Display";
            // 
            // rdoShowShortName
            // 
            this.rdoShowShortName.AutoSize = true;
            this.rdoShowShortName.Location = new System.Drawing.Point(10, 45);
            this.rdoShowShortName.Name = "rdoShowShortName";
            this.rdoShowShortName.Size = new System.Drawing.Size(94, 17);
            this.rdoShowShortName.TabIndex = 1;
            this.rdoShowShortName.TabStop = true;
            this.rdoShowShortName.Text = "Show filename";
            this.rdoShowShortName.UseVisualStyleBackColor = true;
            this.rdoShowShortName.CheckedChanged += new System.EventHandler(this.RdoPathDisplay_CheckedChanged);
            // 
            // rdoShowFullPath
            // 
            this.rdoShowFullPath.AutoSize = true;
            this.rdoShowFullPath.Location = new System.Drawing.Point(10, 22);
            this.rdoShowFullPath.Name = "rdoShowFullPath";
            this.rdoShowFullPath.Size = new System.Drawing.Size(92, 17);
            this.rdoShowFullPath.TabIndex = 0;
            this.rdoShowFullPath.TabStop = true;
            this.rdoShowFullPath.Text = "Show full path";
            this.rdoShowFullPath.UseVisualStyleBackColor = true;
            this.rdoShowFullPath.CheckedChanged += new System.EventHandler(this.RdoPathDisplay_CheckedChanged);
            // 
            // tabCompression
            // 
            this.tabCompression.BackColor = System.Drawing.SystemColors.Control;
            this.tabCompression.Controls.Add(this.groupBoxModuleCompression);
            this.tabCompression.Controls.Add(this.groupBoxStageCompression);
            this.tabCompression.Controls.Add(this.groupBoxFighterCompression);
            this.tabCompression.Location = new System.Drawing.Point(4, 22);
            this.tabCompression.Name = "tabCompression";
            this.tabCompression.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompression.Size = new System.Drawing.Size(365, 452);
            this.tabCompression.TabIndex = 3;
            this.tabCompression.Text = "Compression";
            // 
            // groupBoxModuleCompression
            // 
            this.groupBoxModuleCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxModuleCompression.Controls.Add(this.chkBoxModuleCompress);
            this.groupBoxModuleCompression.Location = new System.Drawing.Point(8, 146);
            this.groupBoxModuleCompression.Name = "groupBoxModuleCompression";
            this.groupBoxModuleCompression.Size = new System.Drawing.Size(349, 53);
            this.groupBoxModuleCompression.TabIndex = 18;
            this.groupBoxModuleCompression.TabStop = false;
            this.groupBoxModuleCompression.Text = "Modules";
            // 
            // chkBoxModuleCompress
            // 
            this.chkBoxModuleCompress.AutoSize = true;
            this.chkBoxModuleCompress.Location = new System.Drawing.Point(10, 22);
            this.chkBoxModuleCompress.Name = "chkBoxModuleCompress";
            this.chkBoxModuleCompress.Size = new System.Drawing.Size(251, 17);
            this.chkBoxModuleCompress.TabIndex = 7;
            this.chkBoxModuleCompress.Text = "Automatically compress files (not recommended)";
            this.chkBoxModuleCompress.UseVisualStyleBackColor = true;
            this.chkBoxModuleCompress.CheckedChanged += new System.EventHandler(this.ChkBoxModuleCompress_CheckedChanged);
            // 
            // groupBoxStageCompression
            // 
            this.groupBoxStageCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStageCompression.Controls.Add(this.chkBoxStageCompress);
            this.groupBoxStageCompression.Location = new System.Drawing.Point(8, 87);
            this.groupBoxStageCompression.Name = "groupBoxStageCompression";
            this.groupBoxStageCompression.Size = new System.Drawing.Size(349, 53);
            this.groupBoxStageCompression.TabIndex = 17;
            this.groupBoxStageCompression.TabStop = false;
            this.groupBoxStageCompression.Text = "Stages";
            // 
            // chkBoxStageCompress
            // 
            this.chkBoxStageCompress.AutoSize = true;
            this.chkBoxStageCompress.Location = new System.Drawing.Point(10, 22);
            this.chkBoxStageCompress.Name = "chkBoxStageCompress";
            this.chkBoxStageCompress.Size = new System.Drawing.Size(157, 17);
            this.chkBoxStageCompress.TabIndex = 7;
            this.chkBoxStageCompress.Text = "Automatically compress files";
            this.chkBoxStageCompress.UseVisualStyleBackColor = true;
            this.chkBoxStageCompress.CheckedChanged += new System.EventHandler(this.ChkBoxStageCompress_CheckedChanged);
            // 
            // groupBoxFighterCompression
            // 
            this.groupBoxFighterCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFighterCompression.Controls.Add(this.chkBoxFighterPacDecompress);
            this.groupBoxFighterCompression.Controls.Add(this.chkBoxFighterPcsCompress);
            this.groupBoxFighterCompression.Location = new System.Drawing.Point(8, 6);
            this.groupBoxFighterCompression.Name = "groupBoxFighterCompression";
            this.groupBoxFighterCompression.Size = new System.Drawing.Size(349, 75);
            this.groupBoxFighterCompression.TabIndex = 16;
            this.groupBoxFighterCompression.TabStop = false;
            this.groupBoxFighterCompression.Text = "Fighters";
            // 
            // chkBoxFighterPacDecompress
            // 
            this.chkBoxFighterPacDecompress.AutoSize = true;
            this.chkBoxFighterPacDecompress.Location = new System.Drawing.Point(10, 22);
            this.chkBoxFighterPacDecompress.Name = "chkBoxFighterPacDecompress";
            this.chkBoxFighterPacDecompress.Size = new System.Drawing.Size(193, 17);
            this.chkBoxFighterPacDecompress.TabIndex = 7;
            this.chkBoxFighterPacDecompress.Text = "Automatically decompress PAC files";
            this.chkBoxFighterPacDecompress.UseVisualStyleBackColor = true;
            this.chkBoxFighterPacDecompress.CheckedChanged += new System.EventHandler(this.ChkBoxFighterPacDecompress_CheckedChanged);
            // 
            // chkBoxFighterPcsCompress
            // 
            this.chkBoxFighterPcsCompress.AutoSize = true;
            this.chkBoxFighterPcsCompress.Location = new System.Drawing.Point(10, 45);
            this.chkBoxFighterPcsCompress.Name = "chkBoxFighterPcsCompress";
            this.chkBoxFighterPcsCompress.Size = new System.Drawing.Size(181, 17);
            this.chkBoxFighterPcsCompress.TabIndex = 9;
            this.chkBoxFighterPcsCompress.Text = "Automatically compress PCS files";
            this.chkBoxFighterPcsCompress.UseVisualStyleBackColor = true;
            this.chkBoxFighterPcsCompress.CheckedChanged += new System.EventHandler(this.ChkBoxFighterPcsCompress_CheckedChanged);
            // 
            // tabFileAssociations
            // 
            this.tabFileAssociations.Controls.Add(this.genericFileAssociationBox);
            this.tabFileAssociations.Controls.Add(this.lblAdminApproval);
            this.tabFileAssociations.Controls.Add(this.btnApply);
            this.tabFileAssociations.Controls.Add(this.associatiedFilesBox);
            this.tabFileAssociations.Location = new System.Drawing.Point(4, 22);
            this.tabFileAssociations.Name = "tabFileAssociations";
            this.tabFileAssociations.Size = new System.Drawing.Size(365, 452);
            this.tabFileAssociations.TabIndex = 2;
            this.tabFileAssociations.Text = "File Associations";
            // 
            // genericFileAssociationBox
            // 
            this.genericFileAssociationBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.genericFileAssociationBox.Controls.Add(this.binFileAssociation);
            this.genericFileAssociationBox.Controls.Add(this.datFileAssociation);
            this.genericFileAssociationBox.Location = new System.Drawing.Point(8, 341);
            this.genericFileAssociationBox.Name = "genericFileAssociationBox";
            this.genericFileAssociationBox.Size = new System.Drawing.Size(349, 75);
            this.genericFileAssociationBox.TabIndex = 6;
            this.genericFileAssociationBox.TabStop = false;
            this.genericFileAssociationBox.Text = "Generic File Types";
            // 
            // binFileAssociation
            // 
            this.binFileAssociation.AutoSize = true;
            this.binFileAssociation.Location = new System.Drawing.Point(10, 45);
            this.binFileAssociation.Name = "binFileAssociation";
            this.binFileAssociation.Size = new System.Drawing.Size(135, 17);
            this.binFileAssociation.TabIndex = 9;
            this.binFileAssociation.Text = "Associate with .bin files";
            this.binFileAssociation.UseVisualStyleBackColor = true;
            this.binFileAssociation.CheckedChanged += new System.EventHandler(this.BinFileAssociation_CheckedChanged);
            // 
            // datFileAssociation
            // 
            this.datFileAssociation.AutoSize = true;
            this.datFileAssociation.Location = new System.Drawing.Point(10, 22);
            this.datFileAssociation.Name = "datFileAssociation";
            this.datFileAssociation.Size = new System.Drawing.Size(136, 17);
            this.datFileAssociation.TabIndex = 8;
            this.datFileAssociation.Text = "Associate with .dat files";
            this.datFileAssociation.UseVisualStyleBackColor = true;
            this.datFileAssociation.CheckedChanged += new System.EventHandler(this.DatFileAssociation_CheckedChanged);
            // 
            // lblAdminApproval
            // 
            this.lblAdminApproval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdminApproval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminApproval.ForeColor = System.Drawing.Color.Red;
            this.lblAdminApproval.Location = new System.Drawing.Point(8, 426);
            this.lblAdminApproval.Name = "lblAdminApproval";
            this.lblAdminApproval.Size = new System.Drawing.Size(349, 18);
            this.lblAdminApproval.TabIndex = 5;
            this.lblAdminApproval.Text = "Administrator access required to make changes";
            this.lblAdminApproval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(287, 424);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // associatiedFilesBox
            // 
            this.associatiedFilesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.associatiedFilesBox.Controls.Add(this.chkBoxAssociateAll);
            this.associatiedFilesBox.Controls.Add(this.lstViewFileAssociations);
            this.associatiedFilesBox.Location = new System.Drawing.Point(8, 6);
            this.associatiedFilesBox.Name = "associatiedFilesBox";
            this.associatiedFilesBox.Size = new System.Drawing.Size(349, 329);
            this.associatiedFilesBox.TabIndex = 1;
            this.associatiedFilesBox.TabStop = false;
            this.associatiedFilesBox.Text = "Wii File Types";
            // 
            // chkBoxAssociateAll
            // 
            this.chkBoxAssociateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBoxAssociateAll.Location = new System.Drawing.Point(242, 303);
            this.chkBoxAssociateAll.Name = "chkBoxAssociateAll";
            this.chkBoxAssociateAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBoxAssociateAll.Size = new System.Drawing.Size(104, 20);
            this.chkBoxAssociateAll.TabIndex = 5;
            this.chkBoxAssociateAll.Text = "Check All";
            this.chkBoxAssociateAll.UseVisualStyleBackColor = true;
            this.chkBoxAssociateAll.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // lstViewFileAssociations
            // 
            this.lstViewFileAssociations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstViewFileAssociations.AutoArrange = false;
            this.lstViewFileAssociations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstViewFileAssociations.CheckBoxes = true;
            this.lstViewFileAssociations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstViewFileAssociations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstViewFileAssociations.HideSelection = false;
            this.lstViewFileAssociations.Location = new System.Drawing.Point(6, 19);
            this.lstViewFileAssociations.MultiSelect = false;
            this.lstViewFileAssociations.Name = "lstViewFileAssociations";
            this.lstViewFileAssociations.Size = new System.Drawing.Size(337, 278);
            this.lstViewFileAssociations.TabIndex = 6;
            this.lstViewFileAssociations.UseCompatibleStateImageBehavior = false;
            this.lstViewFileAssociations.View = System.Windows.Forms.View.Details;
            this.lstViewFileAssociations.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ListView1_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 300;
            // 
            // tabBrawlAPI
            // 
            this.tabBrawlAPI.BackColor = System.Drawing.SystemColors.Control;
            this.tabBrawlAPI.Controls.Add(this.lblAPIRestartNeeded);
            this.tabBrawlAPI.Controls.Add(this.grpBoxLoaders);
            this.tabBrawlAPI.Controls.Add(this.grpBoxFSharpAPI);
            this.tabBrawlAPI.Controls.Add(this.grpBoxPythonAPI);
            this.tabBrawlAPI.Controls.Add(this.grpBoxAPIGeneral);
            this.tabBrawlAPI.Location = new System.Drawing.Point(4, 22);
            this.tabBrawlAPI.Name = "tabBrawlAPI";
            this.tabBrawlAPI.Padding = new System.Windows.Forms.Padding(3);
            this.tabBrawlAPI.Size = new System.Drawing.Size(365, 452);
            this.tabBrawlAPI.TabIndex = 5;
            this.tabBrawlAPI.Text = "BrawlAPI";
            // 
            // lblAPIRestartNeeded
            // 
            this.lblAPIRestartNeeded.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAPIRestartNeeded.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAPIRestartNeeded.ForeColor = System.Drawing.Color.Red;
            this.lblAPIRestartNeeded.Location = new System.Drawing.Point(8, 426);
            this.lblAPIRestartNeeded.Name = "lblAPIRestartNeeded";
            this.lblAPIRestartNeeded.Size = new System.Drawing.Size(349, 18);
            this.lblAPIRestartNeeded.TabIndex = 25;
            this.lblAPIRestartNeeded.Text = "Program restart needed to apply changes";
            this.lblAPIRestartNeeded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpBoxLoaders
            // 
            this.grpBoxLoaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxLoaders.Controls.Add(this.lstViewLoaders);
            this.grpBoxLoaders.Location = new System.Drawing.Point(8, 297);
            this.grpBoxLoaders.Name = "grpBoxLoaders";
            this.grpBoxLoaders.Size = new System.Drawing.Size(349, 126);
            this.grpBoxLoaders.TabIndex = 24;
            this.grpBoxLoaders.TabStop = false;
            this.grpBoxLoaders.Text = "Loaders";
            // 
            // lstViewLoaders
            // 
            this.lstViewLoaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstViewLoaders.AutoArrange = false;
            this.lstViewLoaders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstViewLoaders.CheckBoxes = true;
            this.lstViewLoaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lstViewLoaders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstViewLoaders.HideSelection = false;
            this.lstViewLoaders.Location = new System.Drawing.Point(6, 19);
            this.lstViewLoaders.MultiSelect = false;
            this.lstViewLoaders.Name = "lstViewLoaders";
            this.lstViewLoaders.Size = new System.Drawing.Size(337, 101);
            this.lstViewLoaders.TabIndex = 7;
            this.lstViewLoaders.UseCompatibleStateImageBehavior = false;
            this.lstViewLoaders.View = System.Windows.Forms.View.Details;
            this.lstViewLoaders.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.LstViewLoaders_ItemChecked);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 300;
            // 
            // grpBoxFSharpAPI
            // 
            this.grpBoxFSharpAPI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxFSharpAPI.Controls.Add(this.txtBoxFSharpPath);
            this.grpBoxFSharpAPI.Controls.Add(this.btnFSharpBrowse);
            this.grpBoxFSharpAPI.Controls.Add(this.btnFSharpDetect);
            this.grpBoxFSharpAPI.Controls.Add(this.label2);
            this.grpBoxFSharpAPI.Location = new System.Drawing.Point(8, 218);
            this.grpBoxFSharpAPI.Name = "grpBoxFSharpAPI";
            this.grpBoxFSharpAPI.Size = new System.Drawing.Size(349, 73);
            this.grpBoxFSharpAPI.TabIndex = 23;
            this.grpBoxFSharpAPI.TabStop = false;
            this.grpBoxFSharpAPI.Text = "F#";
            // 
            // txtBoxFSharpPath
            // 
            this.txtBoxFSharpPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxFSharpPath.Location = new System.Drawing.Point(11, 38);
            this.txtBoxFSharpPath.Name = "txtBoxFSharpPath";
            this.txtBoxFSharpPath.Size = new System.Drawing.Size(219, 20);
            this.txtBoxFSharpPath.TabIndex = 3;
            this.txtBoxFSharpPath.Text = "(none)";
            this.txtBoxFSharpPath.TextChanged += new System.EventHandler(this.TxtBoxFSharpPath_TextChanged);
            // 
            // btnFSharpBrowse
            // 
            this.btnFSharpBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFSharpBrowse.Location = new System.Drawing.Point(236, 36);
            this.btnFSharpBrowse.Name = "btnFSharpBrowse";
            this.btnFSharpBrowse.Size = new System.Drawing.Size(24, 24);
            this.btnFSharpBrowse.TabIndex = 21;
            this.btnFSharpBrowse.Text = "...";
            this.btnFSharpBrowse.UseVisualStyleBackColor = true;
            this.btnFSharpBrowse.Click += new System.EventHandler(this.BtnFSharpBrowse_Click);
            // 
            // btnFSharpDetect
            // 
            this.btnFSharpDetect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFSharpDetect.Location = new System.Drawing.Point(266, 36);
            this.btnFSharpDetect.Name = "btnFSharpDetect";
            this.btnFSharpDetect.Size = new System.Drawing.Size(75, 24);
            this.btnFSharpDetect.TabIndex = 22;
            this.btnFSharpDetect.Text = "Auto-Detect";
            this.btnFSharpDetect.UseVisualStyleBackColor = true;
            this.btnFSharpDetect.Click += new System.EventHandler(this.BtnFSharpDetect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Installation Path:";
            // 
            // grpBoxPythonAPI
            // 
            this.grpBoxPythonAPI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxPythonAPI.Controls.Add(this.txtBoxPythonPath);
            this.grpBoxPythonAPI.Controls.Add(this.btnPythonBrowse);
            this.grpBoxPythonAPI.Controls.Add(this.btnPythonDetect);
            this.grpBoxPythonAPI.Controls.Add(this.label1);
            this.grpBoxPythonAPI.Location = new System.Drawing.Point(8, 139);
            this.grpBoxPythonAPI.Name = "grpBoxPythonAPI";
            this.grpBoxPythonAPI.Size = new System.Drawing.Size(349, 73);
            this.grpBoxPythonAPI.TabIndex = 20;
            this.grpBoxPythonAPI.TabStop = false;
            this.grpBoxPythonAPI.Text = "Python";
            // 
            // txtBoxPythonPath
            // 
            this.txtBoxPythonPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxPythonPath.Location = new System.Drawing.Point(11, 38);
            this.txtBoxPythonPath.Name = "txtBoxPythonPath";
            this.txtBoxPythonPath.Size = new System.Drawing.Size(219, 20);
            this.txtBoxPythonPath.TabIndex = 3;
            this.txtBoxPythonPath.Text = "(none)";
            this.txtBoxPythonPath.TextChanged += new System.EventHandler(this.TxtBoxPythonPath_TextChanged);
            // 
            // btnPythonBrowse
            // 
            this.btnPythonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPythonBrowse.Location = new System.Drawing.Point(236, 36);
            this.btnPythonBrowse.Name = "btnPythonBrowse";
            this.btnPythonBrowse.Size = new System.Drawing.Size(24, 24);
            this.btnPythonBrowse.TabIndex = 21;
            this.btnPythonBrowse.Text = "...";
            this.btnPythonBrowse.UseVisualStyleBackColor = true;
            this.btnPythonBrowse.Click += new System.EventHandler(this.BtnPythonBrowse_Click);
            // 
            // btnPythonDetect
            // 
            this.btnPythonDetect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPythonDetect.Location = new System.Drawing.Point(266, 36);
            this.btnPythonDetect.Name = "btnPythonDetect";
            this.btnPythonDetect.Size = new System.Drawing.Size(75, 24);
            this.btnPythonDetect.TabIndex = 22;
            this.btnPythonDetect.Text = "Auto-Detect";
            this.btnPythonDetect.UseVisualStyleBackColor = true;
            this.btnPythonDetect.Click += new System.EventHandler(this.BtnPythonDetect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Installation Path:";
            // 
            // grpBoxAPIGeneral
            // 
            this.grpBoxAPIGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAPIGeneral.Controls.Add(this.grpBoxLoaderBehavior);
            this.grpBoxAPIGeneral.Controls.Add(this.chkBoxEnableAPI);
            this.grpBoxAPIGeneral.Location = new System.Drawing.Point(8, 6);
            this.grpBoxAPIGeneral.Name = "grpBoxAPIGeneral";
            this.grpBoxAPIGeneral.Size = new System.Drawing.Size(349, 127);
            this.grpBoxAPIGeneral.TabIndex = 19;
            this.grpBoxAPIGeneral.TabStop = false;
            this.grpBoxAPIGeneral.Text = "BrawlAPI";
            // 
            // grpBoxLoaderBehavior
            // 
            this.grpBoxLoaderBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxLoaderBehavior.Controls.Add(this.rdoAPILoaderWhitelist);
            this.grpBoxLoaderBehavior.Controls.Add(this.rdoAPILoaderBlacklist);
            this.grpBoxLoaderBehavior.Location = new System.Drawing.Point(6, 45);
            this.grpBoxLoaderBehavior.Name = "grpBoxLoaderBehavior";
            this.grpBoxLoaderBehavior.Size = new System.Drawing.Size(337, 75);
            this.grpBoxLoaderBehavior.TabIndex = 11;
            this.grpBoxLoaderBehavior.TabStop = false;
            this.grpBoxLoaderBehavior.Text = "Loader Behavior";
            // 
            // rdoAPILoaderWhitelist
            // 
            this.rdoAPILoaderWhitelist.AutoSize = true;
            this.rdoAPILoaderWhitelist.Location = new System.Drawing.Point(10, 45);
            this.rdoAPILoaderWhitelist.Name = "rdoAPILoaderWhitelist";
            this.rdoAPILoaderWhitelist.Size = new System.Drawing.Size(269, 17);
            this.rdoAPILoaderWhitelist.TabIndex = 1;
            this.rdoAPILoaderWhitelist.TabStop = true;
            this.rdoAPILoaderWhitelist.Text = "Whitelist wanted loaders (Loaders are off by default)";
            this.rdoAPILoaderWhitelist.UseVisualStyleBackColor = true;
            this.rdoAPILoaderWhitelist.CheckedChanged += new System.EventHandler(this.ChkBoxAPILoaderBehavior_CheckedChanged);
            // 
            // rdoAPILoaderBlacklist
            // 
            this.rdoAPILoaderBlacklist.AutoSize = true;
            this.rdoAPILoaderBlacklist.Location = new System.Drawing.Point(10, 22);
            this.rdoAPILoaderBlacklist.Name = "rdoAPILoaderBlacklist";
            this.rdoAPILoaderBlacklist.Size = new System.Drawing.Size(280, 17);
            this.rdoAPILoaderBlacklist.TabIndex = 0;
            this.rdoAPILoaderBlacklist.TabStop = true;
            this.rdoAPILoaderBlacklist.Text = "Blacklist unwanted loaders (Loaders are on by default)";
            this.rdoAPILoaderBlacklist.UseVisualStyleBackColor = true;
            this.rdoAPILoaderBlacklist.CheckedChanged += new System.EventHandler(this.ChkBoxAPILoaderBehavior_CheckedChanged);
            // 
            // chkBoxEnableAPI
            // 
            this.chkBoxEnableAPI.AutoSize = true;
            this.chkBoxEnableAPI.Location = new System.Drawing.Point(10, 22);
            this.chkBoxEnableAPI.Name = "chkBoxEnableAPI";
            this.chkBoxEnableAPI.Size = new System.Drawing.Size(105, 17);
            this.chkBoxEnableAPI.TabIndex = 7;
            this.chkBoxEnableAPI.Text = "Enable BrawlAPI";
            this.chkBoxEnableAPI.UseVisualStyleBackColor = true;
            this.chkBoxEnableAPI.CheckedChanged += new System.EventHandler(this.ChkBoxEnableAPI_CheckedChanged);
            // 
            // tabDiscord
            // 
            this.tabDiscord.BackColor = System.Drawing.SystemColors.Control;
            this.tabDiscord.Controls.Add(this.grpBoxDiscordRPC);
            this.tabDiscord.Location = new System.Drawing.Point(4, 22);
            this.tabDiscord.Name = "tabDiscord";
            this.tabDiscord.Padding = new System.Windows.Forms.Padding(3);
            this.tabDiscord.Size = new System.Drawing.Size(365, 452);
            this.tabDiscord.TabIndex = 4;
            this.tabDiscord.Text = "Discord";
            // 
            // grpBoxDiscordRPC
            // 
            this.grpBoxDiscordRPC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxDiscordRPC.Controls.Add(this.chkBoxEnableDiscordRPC);
            this.grpBoxDiscordRPC.Controls.Add(this.grpBoxDiscordRPCType);
            this.grpBoxDiscordRPC.Location = new System.Drawing.Point(8, 6);
            this.grpBoxDiscordRPC.Name = "grpBoxDiscordRPC";
            this.grpBoxDiscordRPC.Size = new System.Drawing.Size(349, 172);
            this.grpBoxDiscordRPC.TabIndex = 0;
            this.grpBoxDiscordRPC.TabStop = false;
            this.grpBoxDiscordRPC.Text = "Rich Presence";
            // 
            // chkBoxEnableDiscordRPC
            // 
            this.chkBoxEnableDiscordRPC.AutoSize = true;
            this.chkBoxEnableDiscordRPC.Location = new System.Drawing.Point(10, 22);
            this.chkBoxEnableDiscordRPC.Name = "chkBoxEnableDiscordRPC";
            this.chkBoxEnableDiscordRPC.Size = new System.Drawing.Size(171, 17);
            this.chkBoxEnableDiscordRPC.TabIndex = 1;
            this.chkBoxEnableDiscordRPC.Text = "Enable Discord Rich Presence";
            this.chkBoxEnableDiscordRPC.UseVisualStyleBackColor = true;
            this.chkBoxEnableDiscordRPC.CheckedChanged += new System.EventHandler(this.ChkBoxEnableDiscordRPC_CheckedChanged);
            // 
            // grpBoxDiscordRPCType
            // 
            this.grpBoxDiscordRPCType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxDiscordRPCType.Controls.Add(this.DiscordRPCCustomName);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameCustom);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameExternal);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameInternal);
            this.grpBoxDiscordRPCType.Controls.Add(this.rdoDiscordRPCNameDisabled);
            this.grpBoxDiscordRPCType.Location = new System.Drawing.Point(6, 45);
            this.grpBoxDiscordRPCType.Name = "grpBoxDiscordRPCType";
            this.grpBoxDiscordRPCType.Size = new System.Drawing.Size(337, 119);
            this.grpBoxDiscordRPCType.TabIndex = 0;
            this.grpBoxDiscordRPCType.TabStop = false;
            this.grpBoxDiscordRPCType.Text = "Mod Name Detection";
            // 
            // DiscordRPCCustomName
            // 
            this.DiscordRPCCustomName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DiscordRPCCustomName.Location = new System.Drawing.Point(30, 88);
            this.DiscordRPCCustomName.Name = "DiscordRPCCustomName";
            this.DiscordRPCCustomName.Size = new System.Drawing.Size(301, 20);
            this.DiscordRPCCustomName.TabIndex = 2;
            this.DiscordRPCCustomName.Text = "My Mod";
            this.DiscordRPCCustomName.TextChanged += new System.EventHandler(this.DiscordRPCCustomName_TextChanged);
            // 
            // rdoDiscordRPCNameCustom
            // 
            this.rdoDiscordRPCNameCustom.AutoSize = true;
            this.rdoDiscordRPCNameCustom.Location = new System.Drawing.Point(10, 91);
            this.rdoDiscordRPCNameCustom.Name = "rdoDiscordRPCNameCustom";
            this.rdoDiscordRPCNameCustom.Size = new System.Drawing.Size(14, 13);
            this.rdoDiscordRPCNameCustom.TabIndex = 3;
            this.rdoDiscordRPCNameCustom.TabStop = true;
            this.rdoDiscordRPCNameCustom.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameCustom.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameExternal
            // 
            this.rdoDiscordRPCNameExternal.AutoSize = true;
            this.rdoDiscordRPCNameExternal.Location = new System.Drawing.Point(10, 68);
            this.rdoDiscordRPCNameExternal.Name = "rdoDiscordRPCNameExternal";
            this.rdoDiscordRPCNameExternal.Size = new System.Drawing.Size(126, 17);
            this.rdoDiscordRPCNameExternal.TabIndex = 2;
            this.rdoDiscordRPCNameExternal.TabStop = true;
            this.rdoDiscordRPCNameExternal.Text = "Use external filename";
            this.rdoDiscordRPCNameExternal.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameExternal.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameInternal
            // 
            this.rdoDiscordRPCNameInternal.AutoSize = true;
            this.rdoDiscordRPCNameInternal.Location = new System.Drawing.Point(10, 45);
            this.rdoDiscordRPCNameInternal.Name = "rdoDiscordRPCNameInternal";
            this.rdoDiscordRPCNameInternal.Size = new System.Drawing.Size(123, 17);
            this.rdoDiscordRPCNameInternal.TabIndex = 1;
            this.rdoDiscordRPCNameInternal.TabStop = true;
            this.rdoDiscordRPCNameInternal.Text = "Use internal filename";
            this.rdoDiscordRPCNameInternal.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameInternal.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameDisabled
            // 
            this.rdoDiscordRPCNameDisabled.AutoSize = true;
            this.rdoDiscordRPCNameDisabled.Location = new System.Drawing.Point(10, 22);
            this.rdoDiscordRPCNameDisabled.Name = "rdoDiscordRPCNameDisabled";
            this.rdoDiscordRPCNameDisabled.Size = new System.Drawing.Size(66, 17);
            this.rdoDiscordRPCNameDisabled.TabIndex = 0;
            this.rdoDiscordRPCNameDisabled.TabStop = true;
            this.rdoDiscordRPCNameDisabled.Text = "Disabled";
            this.rdoDiscordRPCNameDisabled.UseVisualStyleBackColor = true;
            this.rdoDiscordRPCNameDisabled.CheckedChanged += new System.EventHandler(this.DiscordRPCNameSettings_CheckedChanged);
            // 
            // tabUpdater
            // 
            this.tabUpdater.Controls.Add(this.grpBoxAPIUpdate);
            this.tabUpdater.Controls.Add(this.grpBoxCanary);
            this.tabUpdater.Controls.Add(this.updaterBehaviorGroupbox);
            this.tabUpdater.Location = new System.Drawing.Point(4, 22);
            this.tabUpdater.Name = "tabUpdater";
            this.tabUpdater.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdater.Size = new System.Drawing.Size(365, 452);
            this.tabUpdater.TabIndex = 1;
            this.tabUpdater.Text = "Updater";
            // 
            // grpBoxAPIUpdate
            // 
            this.grpBoxAPIUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAPIUpdate.Controls.Add(this.btnManageSubscriptions);
            this.grpBoxAPIUpdate.Controls.Add(this.chkBoxUpdateAPI);
            this.grpBoxAPIUpdate.Location = new System.Drawing.Point(8, 191);
            this.grpBoxAPIUpdate.Name = "grpBoxAPIUpdate";
            this.grpBoxAPIUpdate.Size = new System.Drawing.Size(349, 82);
            this.grpBoxAPIUpdate.TabIndex = 15;
            this.grpBoxAPIUpdate.TabStop = false;
            this.grpBoxAPIUpdate.Text = "BrawlAPI Scripts";
            // 
            // btnManageSubscriptions
            // 
            this.btnManageSubscriptions.Location = new System.Drawing.Point(10, 45);
            this.btnManageSubscriptions.Name = "btnManageSubscriptions";
            this.btnManageSubscriptions.Size = new System.Drawing.Size(140, 24);
            this.btnManageSubscriptions.TabIndex = 24;
            this.btnManageSubscriptions.Text = "Manage Subscriptions";
            this.btnManageSubscriptions.UseVisualStyleBackColor = true;
            this.btnManageSubscriptions.Click += new System.EventHandler(this.BtnManageSubscriptions_Click);
            // 
            // chkBoxUpdateAPI
            // 
            this.chkBoxUpdateAPI.AutoSize = true;
            this.chkBoxUpdateAPI.Location = new System.Drawing.Point(10, 22);
            this.chkBoxUpdateAPI.Name = "chkBoxUpdateAPI";
            this.chkBoxUpdateAPI.Size = new System.Drawing.Size(200, 17);
            this.chkBoxUpdateAPI.TabIndex = 11;
            this.chkBoxUpdateAPI.Text = "Update API Scripts on update check";
            this.chkBoxUpdateAPI.UseVisualStyleBackColor = true;
            this.chkBoxUpdateAPI.CheckedChanged += new System.EventHandler(this.ChkBoxUpdateAPI_CheckedChanged);
            // 
            // grpBoxCanary
            // 
            this.grpBoxCanary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxCanary.Controls.Add(this.chkCanary);
            this.grpBoxCanary.Location = new System.Drawing.Point(8, 132);
            this.grpBoxCanary.Name = "grpBoxCanary";
            this.grpBoxCanary.Size = new System.Drawing.Size(349, 53);
            this.grpBoxCanary.TabIndex = 15;
            this.grpBoxCanary.TabStop = false;
            this.grpBoxCanary.Text = "BrawlCrate Canary";
            // 
            // updaterBehaviorGroupbox
            // 
            this.updaterBehaviorGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updaterBehaviorGroupbox.Controls.Add(this.rdoAutoUpdate);
            this.updaterBehaviorGroupbox.Controls.Add(this.rdoCheckManual);
            this.updaterBehaviorGroupbox.Controls.Add(this.rdoCheckStartup);
            this.updaterBehaviorGroupbox.Controls.Add(this.chkDocUpdates);
            this.updaterBehaviorGroupbox.Location = new System.Drawing.Point(8, 6);
            this.updaterBehaviorGroupbox.Name = "updaterBehaviorGroupbox";
            this.updaterBehaviorGroupbox.Size = new System.Drawing.Size(349, 120);
            this.updaterBehaviorGroupbox.TabIndex = 14;
            this.updaterBehaviorGroupbox.TabStop = false;
            this.updaterBehaviorGroupbox.Text = "Updater Behavior";
            // 
            // rdoAutoUpdate
            // 
            this.rdoAutoUpdate.AutoSize = true;
            this.rdoAutoUpdate.Location = new System.Drawing.Point(10, 22);
            this.rdoAutoUpdate.Name = "rdoAutoUpdate";
            this.rdoAutoUpdate.Size = new System.Drawing.Size(72, 17);
            this.rdoAutoUpdate.TabIndex = 2;
            this.rdoAutoUpdate.TabStop = true;
            this.rdoAutoUpdate.Text = "Automatic";
            this.rdoAutoUpdate.UseVisualStyleBackColor = true;
            this.rdoAutoUpdate.CheckedChanged += new System.EventHandler(this.UpdaterBehavior_CheckedChanged);
            // 
            // rdoCheckManual
            // 
            this.rdoCheckManual.AutoSize = true;
            this.rdoCheckManual.Location = new System.Drawing.Point(10, 68);
            this.rdoCheckManual.Name = "rdoCheckManual";
            this.rdoCheckManual.Size = new System.Drawing.Size(60, 17);
            this.rdoCheckManual.TabIndex = 1;
            this.rdoCheckManual.TabStop = true;
            this.rdoCheckManual.Text = "Manual";
            this.rdoCheckManual.UseVisualStyleBackColor = true;
            // 
            // rdoCheckStartup
            // 
            this.rdoCheckStartup.AutoSize = true;
            this.rdoCheckStartup.Location = new System.Drawing.Point(10, 45);
            this.rdoCheckStartup.Name = "rdoCheckStartup";
            this.rdoCheckStartup.Size = new System.Drawing.Size(220, 17);
            this.rdoCheckStartup.TabIndex = 0;
            this.rdoCheckStartup.TabStop = true;
            this.rdoCheckStartup.Text = "Manual, but check for updates on startup";
            this.rdoCheckStartup.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.ClientSize = new System.Drawing.Size(373, 478);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Shown += new System.EventHandler(this.SettingsDialog_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpBoxMDL0General.ResumeLayout(false);
            this.grpBoxMDL0General.PerformLayout();
            this.grpBoxAudioGeneral.ResumeLayout(false);
            this.grpBoxAudioGeneral.PerformLayout();
            this.grpBoxMainFormGeneral.ResumeLayout(false);
            this.grpBoxMainFormGeneral.PerformLayout();
            this.grpBoxFileNameDisplayGeneral.ResumeLayout(false);
            this.grpBoxFileNameDisplayGeneral.PerformLayout();
            this.tabCompression.ResumeLayout(false);
            this.groupBoxModuleCompression.ResumeLayout(false);
            this.groupBoxModuleCompression.PerformLayout();
            this.groupBoxStageCompression.ResumeLayout(false);
            this.groupBoxStageCompression.PerformLayout();
            this.groupBoxFighterCompression.ResumeLayout(false);
            this.groupBoxFighterCompression.PerformLayout();
            this.tabFileAssociations.ResumeLayout(false);
            this.genericFileAssociationBox.ResumeLayout(false);
            this.genericFileAssociationBox.PerformLayout();
            this.associatiedFilesBox.ResumeLayout(false);
            this.tabBrawlAPI.ResumeLayout(false);
            this.grpBoxLoaders.ResumeLayout(false);
            this.grpBoxFSharpAPI.ResumeLayout(false);
            this.grpBoxFSharpAPI.PerformLayout();
            this.grpBoxPythonAPI.ResumeLayout(false);
            this.grpBoxPythonAPI.PerformLayout();
            this.grpBoxAPIGeneral.ResumeLayout(false);
            this.grpBoxAPIGeneral.PerformLayout();
            this.grpBoxLoaderBehavior.ResumeLayout(false);
            this.grpBoxLoaderBehavior.PerformLayout();
            this.tabDiscord.ResumeLayout(false);
            this.grpBoxDiscordRPC.ResumeLayout(false);
            this.grpBoxDiscordRPC.PerformLayout();
            this.grpBoxDiscordRPCType.ResumeLayout(false);
            this.grpBoxDiscordRPCType.PerformLayout();
            this.tabUpdater.ResumeLayout(false);
            this.grpBoxAPIUpdate.ResumeLayout(false);
            this.grpBoxAPIUpdate.PerformLayout();
            this.grpBoxCanary.ResumeLayout(false);
            this.grpBoxCanary.PerformLayout();
            this.updaterBehaviorGroupbox.ResumeLayout(false);
            this.updaterBehaviorGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private void ToggleUpdateOn(object sender, EventArgs e)
        {
            _updating = true;
        }
        private void ToggleUpdateOff(object sender, EventArgs e)
        {
            _updating = false;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                bool check = chkBoxAssociateAll.Checked;
                foreach (ListViewItem i in lstViewFileAssociations.Items)
                {
                    i.Checked = check;
                }
            }
        }

        private void ChkShowPropDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.DisplayPropertyDescriptionsWhenAvailable = chkShowPropDesc.Checked;
            }
        }

        private void ChkShowHex_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.ShowHex = chkShowHex.Checked;
            }
        }

        private void ChkDocUpdates_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.GetDocumentationUpdates = chkDocUpdates.Checked;
            }
        }

        private void ChkCanary_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            _updating = true;
#if CANARY
            DialogResult dc = MessageBox.Show(this,
                    "Are you sure you'd like to receive BrawlCrate canary updates? " +
                    "These updates will happen more often and include features as they are developed, but will come at the cost of stability. " +
                    "If you do take this track, it is highly recommended to join our discord server: https://discord.gg/s7c8763 \n\n" +
                    "If you select yes, the update will begin immediately, so make sure your work is saved.",
                    "BrawlCrate Canary Updater", MessageBoxButtons.YesNo);
            if (dc == DialogResult.Yes)
            {
                Program.ForceDownloadCanary();
            }
            chkCanary.Checked = true;
#else
            DialogResult dc = MessageBox.Show(this, "Are you sure you'd like to return to the stable build? " +
                                                    "Please note that there may be issues saving settings between the old version and the next update. " +
                                                    "If you a bug caused you to move off this build, please report it on our discord server: https://discord.gg/s7c8763 \n\n" +
                                                    "If you select yes, the downgrade will begin immediately, so make sure your work is saved.",
                "BrawlCrate Canary Updater", MessageBoxButtons.YesNo);
            if (dc == DialogResult.Yes)
            {
                Program.ForceDownloadStable();
            }

            chkCanary.Checked = false;
#endif

            _updating = false;
        }

        private void UpdaterBehavior_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            MainForm.Instance.UpdateAutomatically = rdoAutoUpdate.Checked;
            MainForm.Instance.CheckUpdatesOnStartup = rdoAutoUpdate.Checked || rdoCheckStartup.Checked;
        }

        private void BtnCanaryBranch_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                    "Warning: Switching branches or repositories can be unstable unless you know what you're doing. You should generally stay on the brawlcrate-master branch unless directed otherwise for testing purposes. You can reset to the default for either field by leaving it blank.",
                    "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //string cRepo = MainForm.currentRepo;
                //string cBranch = MainForm.currentBranch;
                //TwoInputStringDialog d = new TwoInputStringDialog();
                //if (d.ShowDialog(this, "Enter new repo/branch to track", "Repo:", cRepo, "Branch:", cBranch) == DialogResult.OK)
                //{
                //    if (!d.InputText1.Equals(cRepo, StringComparison.OrdinalIgnoreCase) || !d.InputText2.Equals(cBranch, StringComparison.OrdinalIgnoreCase))
                //    {
                //        MainForm.SetCanaryTracking(d.InputText1, d.InputText2);
                //    }
                //}
            }
        }

        private void ChkBoxAutoPlayAudio_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoPlayAudio = chkBoxAutoPlayAudio.Checked;
            }
        }

        private void ChkBoxFighterPacDecompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoDecompressFighterPAC = chkBoxFighterPacDecompress.Checked;
            }
        }

        private void ChkBoxFighterPcsCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoCompressPCS = chkBoxFighterPcsCompress.Checked;
            }
        }

        private void ChkBoxStageCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.AutoCompressStages = chkBoxStageCompress.Checked;
            }
        }

        private void ChkBoxModuleCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (chkBoxModuleCompress.Checked)
            {
                if (MessageBox.Show(
                        "Warning: Module compression does not save much space and can reduce editablity of modules. Are you sure you want to turn this on?",
                        "Module Compressor", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    _updating = true;
                    chkBoxModuleCompress.Checked = false;
                    _updating = false;
                    return;
                }
            }

            MainForm.Instance.AutoCompressModules = chkBoxModuleCompress.Checked;
        }

        private void ChkBoxMDL0Compatibility_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.CompatibilityMode = chkBoxMDL0Compatibility.Checked;
            }

            chkBoxRenderBRRES.Enabled = !chkBoxMDL0Compatibility.Checked;
            chkBoxRenderARC.Enabled = !chkBoxMDL0Compatibility.Checked;
        }

        private void ChkBoxEnableDiscordRPC_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Properties.Settings.Default.DiscordRPCEnabled = chkBoxEnableDiscordRPC.Checked;
            Properties.Settings.Default.Save();
            grpBoxDiscordRPCType.Enabled = chkBoxEnableDiscordRPC.Checked;
            DiscordSettings.LoadSettings(true);
        }

        private void DiscordRPCNameSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (rdoDiscordRPCNameDisabled.Checked)
            {
                Properties.Settings.Default.DiscordRPCNameType = DiscordSettings.ModNameType.Disabled;
            }
            else if (rdoDiscordRPCNameInternal.Checked)
            {
                Properties.Settings.Default.DiscordRPCNameType = DiscordSettings.ModNameType.AutoInternal;
            }
            else if (rdoDiscordRPCNameExternal.Checked)
            {
                Properties.Settings.Default.DiscordRPCNameType = DiscordSettings.ModNameType.AutoExternal;
            }
            else if (rdoDiscordRPCNameCustom.Checked)
            {
                Properties.Settings.Default.DiscordRPCNameType = DiscordSettings.ModNameType.UserDefined;
            }

            DiscordRPCCustomName.Enabled = rdoDiscordRPCNameCustom.Checked;
            DiscordRPCCustomName.ReadOnly = !rdoDiscordRPCNameCustom.Checked;
            Properties.Settings.Default.Save();
            DiscordSettings.LoadSettings(true);
        }

        private void DiscordRPCCustomName_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DiscordRPCNameCustom = DiscordRPCCustomName.Text;
            Properties.Settings.Default.Save();
            if (rdoDiscordRPCNameCustom.Checked)
            {
                DiscordSettings.LoadSettings(true);
            }
        }

        private void DatFileAssociation_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnApply.Enabled = true;
        }

        private void BinFileAssociation_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            btnApply.Enabled = true;
        }

        private void RdoPathDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            MainForm.Instance.ShowFullPath = rdoShowFullPath.Checked;
        }

        private void RecentFileCountBox_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (int.TryParse(recentFileCountBox.Text, out int i))
            {
                Properties.Settings.Default.RecentFilesMax = i;
                Properties.Settings.Default.Save();
            }
        }

        private void ChkBoxEnableAPI_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                Properties.Settings.Default.APIEnabled = chkBoxEnableAPI.Checked;
                Properties.Settings.Default.Save();
                lblAPIRestartNeeded.Visible = true;
            }

            grpBoxLoaderBehavior.Enabled = chkBoxEnableAPI.Checked;
            grpBoxPythonAPI.Enabled = chkBoxEnableAPI.Checked;
            grpBoxFSharpAPI.Enabled = chkBoxEnableAPI.Checked;
            grpBoxLoaders.Enabled = chkBoxEnableAPI.Checked;
        }

        private void TxtBoxPythonPath_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Properties.Settings.Default.PythonInstallationPath = txtBoxPythonPath.Text;
            Properties.Settings.Default.Save();
            lblAPIRestartNeeded.Visible = true;
        }

        private void BtnPythonBrowse_Click(object sender, EventArgs e)
        {
#if !MONO
            using (Ookii.Dialogs.VistaFolderBrowserDialog f = new Ookii.Dialogs.VistaFolderBrowserDialog { UseDescriptionForTitle = true })
#else
            using (FolderBrowserDialog f = new FolderBrowserDialog())
#endif
            {
                f.Description = "Python Installation Path";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtBoxPythonPath.Text = f.SelectedPath;
                }
            }
        }

        private void BtnPythonDetect_Click(object sender, EventArgs e)
        {
            API.BrawlAPI.PythonInstall(true, true);
            txtBoxPythonPath.Text = Properties.Settings.Default.PythonInstallationPath;
        }

        private void TxtBoxFSharpPath_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Properties.Settings.Default.FSharpInstallationPath = txtBoxFSharpPath.Text;
            Properties.Settings.Default.Save();
            lblAPIRestartNeeded.Visible = true;
        }

        private void BtnFSharpBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog f = new OpenFileDialog())
            {
                f.Filter = "F# Executable (fsi.exe)|fsi.exe";
                f.Title = "F# Installation";
                try
                {
                    if (Directory.Exists(txtBoxFSharpPath.Text.Substring(0, txtBoxFSharpPath.Text.LastIndexOf('\\'))))
                    {
                        f.InitialDirectory =
                            txtBoxFSharpPath.Text.Substring(0, txtBoxFSharpPath.Text.LastIndexOf('\\'));
                    }
                }
                catch
                {
                    // ignored
                }

                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtBoxFSharpPath.Text = f.FileName;
                }
            }
        }

        private void BtnFSharpDetect_Click(object sender, EventArgs e)
        {
            API.BrawlAPI.FSharpInstall(true, true);
            txtBoxFSharpPath.Text = Properties.Settings.Default.FSharpInstallationPath;
        }

        private void ChkBoxAPILoaderBehavior_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist = rdoAPILoaderWhitelist.Checked;
            Properties.Settings.Default.Save();

            RefreshLoaderList();

            lblAPIRestartNeeded.Visible = true;
        }

        private void ChkBoxRenderBRRES_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.ShowBRRESPreviews = chkBoxRenderBRRES.Checked;
            }
        }

        private void ChkBoxRenderARC_CheckedChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                MainForm.Instance.ShowARCPreviews = chkBoxRenderARC.Checked;
            }
        }

        private void BtnManagerPathBrowse_Click(object sender, EventArgs e)
        {
#if !MONO
            using (Ookii.Dialogs.VistaFolderBrowserDialog f = new Ookii.Dialogs.VistaFolderBrowserDialog { UseDescriptionForTitle = true })
#else
            using (FolderBrowserDialog f = new FolderBrowserDialog())
#endif
            {
                f.Description = "Open Build Directory";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtBoxDefaultBuildPath.Text = f.SelectedPath;
                }
            }
        }

        private void TxtBoxDefaultBuildPath_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Properties.Settings.Default.BuildPath = txtBoxDefaultBuildPath.Text;
            Properties.Settings.Default.Save();
        }

        private void ChkBoxContextualLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            BrawlLib.Properties.Settings.Default.ContextualLoopAudio = chkBoxContextualLoop.Checked;
            BrawlLib.Properties.Settings.Default.Save();
        }

        private void LstViewLoaders_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!e.Item.Checked)
            {
                if (!Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist)
                {
                    if (Properties.Settings.Default.APILoadersBlacklist == null && !(Properties.Settings.Default.APILoadersBlacklist?.Contains(e.Item.Text) ?? false))
                    {
                        Properties.Settings.Default.APILoadersBlacklist = new StringCollection();
                    }
                    Properties.Settings.Default.APILoadersBlacklist.Add(e.Item.Text);
                }
                else if (Properties.Settings.Default.APILoadersWhitelist?.Contains(e.Item.Text) ?? false)
                {
                    Properties.Settings.Default.APILoadersWhitelist.Remove(e.Item.Text);
                }
            }
            else
            {
                if (Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist && !(Properties.Settings.Default.APILoadersWhitelist?.Contains(e.Item.Text) ?? false))
                {
                    if (Properties.Settings.Default.APILoadersWhitelist == null)
                    {
                        Properties.Settings.Default.APILoadersWhitelist = new StringCollection();
                    }
                    Properties.Settings.Default.APILoadersWhitelist.Add(e.Item.Text);
                }
                else if (Properties.Settings.Default.APILoadersBlacklist?.Contains(e.Item.Text) ?? false)
                {
                    Properties.Settings.Default.APILoadersBlacklist.Remove(e.Item.Text);
                }
            }
            Properties.Settings.Default.Save();

            lblAPIRestartNeeded.Visible = true;
        }

        private void BtnManageSubscriptions_Click(object sender, EventArgs e)
        {
            MainForm.Instance.ApiSubManager.ShowDialog();
        }

        private void ChkBoxUpdateAPI_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            Properties.Settings.Default.APIAutoUpdate = chkBoxUpdateAPI.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
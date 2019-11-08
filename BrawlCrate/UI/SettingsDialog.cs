using BrawlCrate.API;
using BrawlCrate.Discord;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.SSBB;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;

#if !MONO
    using BrawlLib.Internal.Windows.Forms.Ookii.Dialogs;
#endif

namespace BrawlCrate.UI
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
        private CheckBox chkBoxParseMoveDef;
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
            catch (Exception)
            {
                MessageBox.Show(null,
                    "Unable to access the registry to set file associations.\nRun the program as administrator and try again.",
                    "Insufficient Privileges", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblAdminApproval.Visible = true;
                btnApply.Visible = false;
                associatiedFilesBox.Enabled = false;
                genericFileAssociationBox.Enabled = false;
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
            chkBoxParseMoveDef.Checked = BrawlLib.Properties.Settings.Default.ParseMoveDef;
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
                i.Checked = Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist
                    ? Properties.Settings.Default.APILoadersWhitelist?.Contains(i.Text) ?? false
                    : !Properties.Settings.Default.APILoadersBlacklist?.Contains(i.Text) ?? true;
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
            chkShowPropDesc = new CheckBox();
            chkShowHex = new CheckBox();
            chkDocUpdates = new CheckBox();
            chkCanary = new CheckBox();
            tabControl1 = new TabControl();
            tabGeneral = new TabPage();
            groupBox1 = new GroupBox();
            btnManagerPathBrowse = new Button();
            txtBoxDefaultBuildPath = new TextBox();
            lblManagerDefaultPath = new Label();
            grpBoxMDL0General = new GroupBox();
            chkBoxRenderARC = new CheckBox();
            chkBoxRenderBRRES = new CheckBox();
            chkBoxMDL0Compatibility = new CheckBox();
            grpBoxAudioGeneral = new GroupBox();
            chkBoxContextualLoop = new CheckBox();
            chkBoxAutoPlayAudio = new CheckBox();
            grpBoxMainFormGeneral = new GroupBox();
            chkBoxParseMoveDef = new CheckBox();
            lblRecentFiles = new Label();
            recentFileCountBox = new NumericInputBox();
            grpBoxFileNameDisplayGeneral = new GroupBox();
            rdoShowShortName = new RadioButton();
            rdoShowFullPath = new RadioButton();
            tabCompression = new TabPage();
            groupBoxModuleCompression = new GroupBox();
            chkBoxModuleCompress = new CheckBox();
            groupBoxStageCompression = new GroupBox();
            chkBoxStageCompress = new CheckBox();
            groupBoxFighterCompression = new GroupBox();
            chkBoxFighterPacDecompress = new CheckBox();
            chkBoxFighterPcsCompress = new CheckBox();
            tabFileAssociations = new TabPage();
            genericFileAssociationBox = new GroupBox();
            binFileAssociation = new CheckBox();
            datFileAssociation = new CheckBox();
            lblAdminApproval = new Label();
            btnApply = new Button();
            associatiedFilesBox = new GroupBox();
            chkBoxAssociateAll = new CheckBox();
            lstViewFileAssociations = new ListView();
            columnHeader1 = (ColumnHeader) new ColumnHeader();
            tabBrawlAPI = new TabPage();
            lblAPIRestartNeeded = new Label();
            grpBoxLoaders = new GroupBox();
            lstViewLoaders = new ListView();
            columnHeader2 = (ColumnHeader) new ColumnHeader();
            grpBoxFSharpAPI = new GroupBox();
            txtBoxFSharpPath = new TextBox();
            btnFSharpBrowse = new Button();
            btnFSharpDetect = new Button();
            label2 = new Label();
            grpBoxPythonAPI = new GroupBox();
            txtBoxPythonPath = new TextBox();
            btnPythonBrowse = new Button();
            btnPythonDetect = new Button();
            label1 = new Label();
            grpBoxAPIGeneral = new GroupBox();
            grpBoxLoaderBehavior = new GroupBox();
            rdoAPILoaderWhitelist = new RadioButton();
            rdoAPILoaderBlacklist = new RadioButton();
            chkBoxEnableAPI = new CheckBox();
            tabDiscord = new TabPage();
            grpBoxDiscordRPC = new GroupBox();
            chkBoxEnableDiscordRPC = new CheckBox();
            grpBoxDiscordRPCType = new GroupBox();
            DiscordRPCCustomName = new TextBox();
            rdoDiscordRPCNameCustom = new RadioButton();
            rdoDiscordRPCNameExternal = new RadioButton();
            rdoDiscordRPCNameInternal = new RadioButton();
            rdoDiscordRPCNameDisabled = new RadioButton();
            tabUpdater = new TabPage();
            grpBoxAPIUpdate = new GroupBox();
            btnManageSubscriptions = new Button();
            chkBoxUpdateAPI = new CheckBox();
            grpBoxCanary = new GroupBox();
            updaterBehaviorGroupbox = new GroupBox();
            rdoAutoUpdate = new RadioButton();
            rdoCheckManual = new RadioButton();
            rdoCheckStartup = new RadioButton();
            tabControl1.SuspendLayout();
            tabGeneral.SuspendLayout();
            groupBox1.SuspendLayout();
            grpBoxMDL0General.SuspendLayout();
            grpBoxAudioGeneral.SuspendLayout();
            grpBoxMainFormGeneral.SuspendLayout();
            grpBoxFileNameDisplayGeneral.SuspendLayout();
            tabCompression.SuspendLayout();
            groupBoxModuleCompression.SuspendLayout();
            groupBoxStageCompression.SuspendLayout();
            groupBoxFighterCompression.SuspendLayout();
            tabFileAssociations.SuspendLayout();
            genericFileAssociationBox.SuspendLayout();
            associatiedFilesBox.SuspendLayout();
            tabBrawlAPI.SuspendLayout();
            grpBoxLoaders.SuspendLayout();
            grpBoxFSharpAPI.SuspendLayout();
            grpBoxPythonAPI.SuspendLayout();
            grpBoxAPIGeneral.SuspendLayout();
            grpBoxLoaderBehavior.SuspendLayout();
            tabDiscord.SuspendLayout();
            grpBoxDiscordRPC.SuspendLayout();
            grpBoxDiscordRPCType.SuspendLayout();
            tabUpdater.SuspendLayout();
            grpBoxAPIUpdate.SuspendLayout();
            grpBoxCanary.SuspendLayout();
            updaterBehaviorGroupbox.SuspendLayout();
            SuspendLayout();
            // 
            // chkShowPropDesc
            // 
            chkShowPropDesc.AutoSize = true;
            chkShowPropDesc.Location = new System.Drawing.Point(10, 22);
            chkShowPropDesc.Name = "chkShowPropDesc";
            chkShowPropDesc.Size = new System.Drawing.Size(242, 17);
            chkShowPropDesc.TabIndex = 7;
            chkShowPropDesc.Text = "Show property description box when available";
            chkShowPropDesc.UseVisualStyleBackColor = true;
            chkShowPropDesc.CheckedChanged += new EventHandler(ChkShowPropDesc_CheckedChanged);
            // 
            // chkShowHex
            // 
            chkShowHex.AutoSize = true;
            chkShowHex.Location = new System.Drawing.Point(10, 45);
            chkShowHex.Name = "chkShowHex";
            chkShowHex.Size = new System.Drawing.Size(233, 17);
            chkShowHex.TabIndex = 9;
            chkShowHex.Text = "Show hexadecimal for files without previews";
            chkShowHex.UseVisualStyleBackColor = true;
            chkShowHex.CheckedChanged += new EventHandler(ChkShowHex_CheckedChanged);
            // 
            // chkDocUpdates
            // 
            chkDocUpdates.AutoSize = true;
            chkDocUpdates.Location = new System.Drawing.Point(10, 91);
            chkDocUpdates.Name = "chkDocUpdates";
            chkDocUpdates.Size = new System.Drawing.Size(180, 17);
            chkDocUpdates.TabIndex = 11;
            chkDocUpdates.Text = "Receive documentation updates";
            chkDocUpdates.UseVisualStyleBackColor = true;
            chkDocUpdates.CheckedChanged += new EventHandler(ChkDocUpdates_CheckedChanged);
            // 
            // chkCanary
            // 
            chkCanary.AutoSize = true;
            chkCanary.Location = new System.Drawing.Point(10, 22);
            chkCanary.Name = "chkCanary";
            chkCanary.Size = new System.Drawing.Size(263, 17);
            chkCanary.TabIndex = 13;
            chkCanary.Text = "Opt into BrawlCrate Canary (Experimental) updates";
            chkCanary.UseVisualStyleBackColor = true;
            chkCanary.CheckedChanged += new EventHandler(ChkCanary_CheckedChanged);
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabGeneral);
            tabControl1.Controls.Add(tabCompression);
            tabControl1.Controls.Add(tabFileAssociations);
            tabControl1.Controls.Add(tabBrawlAPI);
            tabControl1.Controls.Add(tabDiscord);
            tabControl1.Controls.Add(tabUpdater);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(373, 511);
            tabControl1.TabIndex = 48;
            tabControl1.SelectedIndexChanged += new EventHandler(ToggleUpdateOff);
            tabControl1.Selected += new TabControlEventHandler(ToggleUpdateOn);
            // 
            // tabGeneral
            // 
            tabGeneral.BackColor = System.Drawing.SystemColors.Control;
            tabGeneral.Controls.Add(groupBox1);
            tabGeneral.Controls.Add(grpBoxMDL0General);
            tabGeneral.Controls.Add(grpBoxAudioGeneral);
            tabGeneral.Controls.Add(grpBoxMainFormGeneral);
            tabGeneral.Location = new System.Drawing.Point(4, 22);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Padding = new Padding(3);
            tabGeneral.Size = new System.Drawing.Size(365, 485);
            tabGeneral.TabIndex = 0;
            tabGeneral.Text = "General";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            groupBox1.Controls.Add(btnManagerPathBrowse);
            groupBox1.Controls.Add(txtBoxDefaultBuildPath);
            groupBox1.Controls.Add(lblManagerDefaultPath);
            groupBox1.Location = new System.Drawing.Point(8, 393);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(349, 73);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Managers";
            // 
            // btnManagerPathBrowse
            // 
            btnManagerPathBrowse.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                           | AnchorStyles.Right);
            btnManagerPathBrowse.Location = new System.Drawing.Point(319, 36);
            btnManagerPathBrowse.Name = "btnManagerPathBrowse";
            btnManagerPathBrowse.Size = new System.Drawing.Size(24, 24);
            btnManagerPathBrowse.TabIndex = 22;
            btnManagerPathBrowse.Text = "...";
            btnManagerPathBrowse.UseVisualStyleBackColor = true;
            btnManagerPathBrowse.Click += new EventHandler(BtnManagerPathBrowse_Click);
            // 
            // txtBoxDefaultBuildPath
            // 
            txtBoxDefaultBuildPath.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                             | AnchorStyles.Right);
            txtBoxDefaultBuildPath.Location = new System.Drawing.Point(11, 38);
            txtBoxDefaultBuildPath.Name = "txtBoxDefaultBuildPath";
            txtBoxDefaultBuildPath.Size = new System.Drawing.Size(302, 20);
            txtBoxDefaultBuildPath.TabIndex = 3;
            txtBoxDefaultBuildPath.Text = "(none)";
            txtBoxDefaultBuildPath.TextChanged += new EventHandler(TxtBoxDefaultBuildPath_TextChanged);
            // 
            // lblManagerDefaultPath
            // 
            lblManagerDefaultPath.AutoSize = true;
            lblManagerDefaultPath.Location = new System.Drawing.Point(8, 22);
            lblManagerDefaultPath.Name = "lblManagerDefaultPath";
            lblManagerDefaultPath.Size = new System.Drawing.Size(95, 13);
            lblManagerDefaultPath.TabIndex = 13;
            lblManagerDefaultPath.Text = "Default Build Path:";
            // 
            // grpBoxMDL0General
            // 
            grpBoxMDL0General.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                        | AnchorStyles.Right);
            grpBoxMDL0General.Controls.Add(chkBoxRenderARC);
            grpBoxMDL0General.Controls.Add(chkBoxRenderBRRES);
            grpBoxMDL0General.Controls.Add(chkBoxMDL0Compatibility);
            grpBoxMDL0General.Location = new System.Drawing.Point(8, 293);
            grpBoxMDL0General.Name = "grpBoxMDL0General";
            grpBoxMDL0General.Size = new System.Drawing.Size(349, 94);
            grpBoxMDL0General.TabIndex = 19;
            grpBoxMDL0General.TabStop = false;
            grpBoxMDL0General.Text = "Models";
            // 
            // chkBoxRenderARC
            // 
            chkBoxRenderARC.AutoSize = true;
            chkBoxRenderARC.Location = new System.Drawing.Point(10, 68);
            chkBoxRenderARC.Name = "chkBoxRenderARC";
            chkBoxRenderARC.Size = new System.Drawing.Size(146, 17);
            chkBoxRenderARC.TabIndex = 9;
            chkBoxRenderARC.Text = "Render previews for ARC";
            chkBoxRenderARC.UseVisualStyleBackColor = true;
            chkBoxRenderARC.CheckedChanged += new EventHandler(ChkBoxRenderARC_CheckedChanged);
            // 
            // chkBoxRenderBRRES
            // 
            chkBoxRenderBRRES.AutoSize = true;
            chkBoxRenderBRRES.Location = new System.Drawing.Point(10, 45);
            chkBoxRenderBRRES.Name = "chkBoxRenderBRRES";
            chkBoxRenderBRRES.Size = new System.Drawing.Size(161, 17);
            chkBoxRenderBRRES.TabIndex = 8;
            chkBoxRenderBRRES.Text = "Render previews for BRRES";
            chkBoxRenderBRRES.UseVisualStyleBackColor = true;
            chkBoxRenderBRRES.CheckedChanged += new EventHandler(ChkBoxRenderBRRES_CheckedChanged);
            // 
            // chkBoxMDL0Compatibility
            // 
            chkBoxMDL0Compatibility.AutoSize = true;
            chkBoxMDL0Compatibility.Location = new System.Drawing.Point(10, 22);
            chkBoxMDL0Compatibility.Name = "chkBoxMDL0Compatibility";
            chkBoxMDL0Compatibility.Size = new System.Drawing.Size(134, 17);
            chkBoxMDL0Compatibility.TabIndex = 7;
            chkBoxMDL0Compatibility.Text = "Use compatibility mode";
            chkBoxMDL0Compatibility.UseVisualStyleBackColor = true;
            chkBoxMDL0Compatibility.CheckedChanged += new EventHandler(ChkBoxMDL0Compatibility_CheckedChanged);
            // 
            // grpBoxAudioGeneral
            // 
            grpBoxAudioGeneral.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                         | AnchorStyles.Right);
            grpBoxAudioGeneral.Controls.Add(chkBoxContextualLoop);
            grpBoxAudioGeneral.Controls.Add(chkBoxAutoPlayAudio);
            grpBoxAudioGeneral.Location = new System.Drawing.Point(8, 212);
            grpBoxAudioGeneral.Name = "grpBoxAudioGeneral";
            grpBoxAudioGeneral.Size = new System.Drawing.Size(349, 75);
            grpBoxAudioGeneral.TabIndex = 18;
            grpBoxAudioGeneral.TabStop = false;
            grpBoxAudioGeneral.Text = "Audio";
            // 
            // chkBoxContextualLoop
            // 
            chkBoxContextualLoop.AutoSize = true;
            chkBoxContextualLoop.Location = new System.Drawing.Point(10, 22);
            chkBoxContextualLoop.Name = "chkBoxContextualLoop";
            chkBoxContextualLoop.Size = new System.Drawing.Size(211, 17);
            chkBoxContextualLoop.TabIndex = 8;
            chkBoxContextualLoop.Text = "Loop preview for looping audio sources";
            chkBoxContextualLoop.UseVisualStyleBackColor = true;
            chkBoxContextualLoop.CheckedChanged += new EventHandler(ChkBoxContextualLoop_CheckedChanged);
            // 
            // chkBoxAutoPlayAudio
            // 
            chkBoxAutoPlayAudio.AutoSize = true;
            chkBoxAutoPlayAudio.Location = new System.Drawing.Point(10, 45);
            chkBoxAutoPlayAudio.Name = "chkBoxAutoPlayAudio";
            chkBoxAutoPlayAudio.Size = new System.Drawing.Size(171, 17);
            chkBoxAutoPlayAudio.TabIndex = 7;
            chkBoxAutoPlayAudio.Text = "Automatically play audio nodes";
            chkBoxAutoPlayAudio.UseVisualStyleBackColor = true;
            chkBoxAutoPlayAudio.CheckedChanged += new EventHandler(ChkBoxAutoPlayAudio_CheckedChanged);
            // 
            // grpBoxMainFormGeneral
            // 
            grpBoxMainFormGeneral.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                            | AnchorStyles.Right);
            grpBoxMainFormGeneral.Controls.Add(chkBoxParseMoveDef);
            grpBoxMainFormGeneral.Controls.Add(lblRecentFiles);
            grpBoxMainFormGeneral.Controls.Add(recentFileCountBox);
            grpBoxMainFormGeneral.Controls.Add(grpBoxFileNameDisplayGeneral);
            grpBoxMainFormGeneral.Controls.Add(chkShowPropDesc);
            grpBoxMainFormGeneral.Controls.Add(chkShowHex);
            grpBoxMainFormGeneral.Location = new System.Drawing.Point(8, 6);
            grpBoxMainFormGeneral.Name = "grpBoxMainFormGeneral";
            grpBoxMainFormGeneral.Size = new System.Drawing.Size(349, 200);
            grpBoxMainFormGeneral.TabIndex = 15;
            grpBoxMainFormGeneral.TabStop = false;
            grpBoxMainFormGeneral.Text = "Main Form";
            // 
            // chkBoxParseMoveDef
            // 
            chkBoxParseMoveDef.AutoSize = true;
            chkBoxParseMoveDef.Location = new System.Drawing.Point(10, 68);
            chkBoxParseMoveDef.Name = "chkBoxParseMoveDef";
            chkBoxParseMoveDef.Size = new System.Drawing.Size(132, 17);
            chkBoxParseMoveDef.TabIndex = 13;
            chkBoxParseMoveDef.Text = "Parse MoveDef nodes";
            chkBoxParseMoveDef.UseVisualStyleBackColor = true;
            chkBoxParseMoveDef.CheckedChanged += new EventHandler(chkBoxParseMoveDef_CheckedChanged);
            // 
            // lblRecentFiles
            // 
            lblRecentFiles.AutoSize = true;
            lblRecentFiles.Location = new System.Drawing.Point(8, 94);
            lblRecentFiles.Name = "lblRecentFiles";
            lblRecentFiles.Size = new System.Drawing.Size(120, 13);
            lblRecentFiles.TabIndex = 12;
            lblRecentFiles.Text = "Max Recent Files Count";
            // 
            // recentFileCountBox
            // 
            recentFileCountBox.Integer = true;
            recentFileCountBox.Integral = true;
            recentFileCountBox.Location = new System.Drawing.Point(133, 91);
            recentFileCountBox.MaximumValue = 3.402823E+38F;
            recentFileCountBox.MinimumValue = -3.402823E+38F;
            recentFileCountBox.Name = "recentFileCountBox";
            recentFileCountBox.Size = new System.Drawing.Size(100, 20);
            recentFileCountBox.TabIndex = 11;
            recentFileCountBox.Text = "0";
            recentFileCountBox.TextChanged += new EventHandler(RecentFileCountBox_TextChanged);
            // 
            // grpBoxFileNameDisplayGeneral
            // 
            grpBoxFileNameDisplayGeneral.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                                      | AnchorStyles.Right);
            grpBoxFileNameDisplayGeneral.Controls.Add(rdoShowShortName);
            grpBoxFileNameDisplayGeneral.Controls.Add(rdoShowFullPath);
            grpBoxFileNameDisplayGeneral.Location = new System.Drawing.Point(6, 119);
            grpBoxFileNameDisplayGeneral.Name = "grpBoxFileNameDisplayGeneral";
            grpBoxFileNameDisplayGeneral.Size = new System.Drawing.Size(337, 75);
            grpBoxFileNameDisplayGeneral.TabIndex = 10;
            grpBoxFileNameDisplayGeneral.TabStop = false;
            grpBoxFileNameDisplayGeneral.Text = "Filename Display";
            // 
            // rdoShowShortName
            // 
            rdoShowShortName.AutoSize = true;
            rdoShowShortName.Location = new System.Drawing.Point(10, 45);
            rdoShowShortName.Name = "rdoShowShortName";
            rdoShowShortName.Size = new System.Drawing.Size(94, 17);
            rdoShowShortName.TabIndex = 1;
            rdoShowShortName.TabStop = true;
            rdoShowShortName.Text = "Show filename";
            rdoShowShortName.UseVisualStyleBackColor = true;
            rdoShowShortName.CheckedChanged += new EventHandler(RdoPathDisplay_CheckedChanged);
            // 
            // rdoShowFullPath
            // 
            rdoShowFullPath.AutoSize = true;
            rdoShowFullPath.Location = new System.Drawing.Point(10, 22);
            rdoShowFullPath.Name = "rdoShowFullPath";
            rdoShowFullPath.Size = new System.Drawing.Size(92, 17);
            rdoShowFullPath.TabIndex = 0;
            rdoShowFullPath.TabStop = true;
            rdoShowFullPath.Text = "Show full path";
            rdoShowFullPath.UseVisualStyleBackColor = true;
            rdoShowFullPath.CheckedChanged += new EventHandler(RdoPathDisplay_CheckedChanged);
            // 
            // tabCompression
            // 
            tabCompression.BackColor = System.Drawing.SystemColors.Control;
            tabCompression.Controls.Add(groupBoxModuleCompression);
            tabCompression.Controls.Add(groupBoxStageCompression);
            tabCompression.Controls.Add(groupBoxFighterCompression);
            tabCompression.Location = new System.Drawing.Point(4, 22);
            tabCompression.Name = "tabCompression";
            tabCompression.Padding = new Padding(3);
            tabCompression.Size = new System.Drawing.Size(365, 485);
            tabCompression.TabIndex = 3;
            tabCompression.Text = "Compression";
            // 
            // groupBoxModuleCompression
            // 
            groupBoxModuleCompression.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                                | AnchorStyles.Right);
            groupBoxModuleCompression.Controls.Add(chkBoxModuleCompress);
            groupBoxModuleCompression.Location = new System.Drawing.Point(8, 146);
            groupBoxModuleCompression.Name = "groupBoxModuleCompression";
            groupBoxModuleCompression.Size = new System.Drawing.Size(349, 53);
            groupBoxModuleCompression.TabIndex = 18;
            groupBoxModuleCompression.TabStop = false;
            groupBoxModuleCompression.Text = "Modules";
            // 
            // chkBoxModuleCompress
            // 
            chkBoxModuleCompress.AutoSize = true;
            chkBoxModuleCompress.Location = new System.Drawing.Point(10, 22);
            chkBoxModuleCompress.Name = "chkBoxModuleCompress";
            chkBoxModuleCompress.Size = new System.Drawing.Size(251, 17);
            chkBoxModuleCompress.TabIndex = 7;
            chkBoxModuleCompress.Text = "Automatically compress files (not recommended)";
            chkBoxModuleCompress.UseVisualStyleBackColor = true;
            chkBoxModuleCompress.CheckedChanged += new EventHandler(ChkBoxModuleCompress_CheckedChanged);
            // 
            // groupBoxStageCompression
            // 
            groupBoxStageCompression.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                               | AnchorStyles.Right);
            groupBoxStageCompression.Controls.Add(chkBoxStageCompress);
            groupBoxStageCompression.Location = new System.Drawing.Point(8, 87);
            groupBoxStageCompression.Name = "groupBoxStageCompression";
            groupBoxStageCompression.Size = new System.Drawing.Size(349, 53);
            groupBoxStageCompression.TabIndex = 17;
            groupBoxStageCompression.TabStop = false;
            groupBoxStageCompression.Text = "Stages";
            // 
            // chkBoxStageCompress
            // 
            chkBoxStageCompress.AutoSize = true;
            chkBoxStageCompress.Location = new System.Drawing.Point(10, 22);
            chkBoxStageCompress.Name = "chkBoxStageCompress";
            chkBoxStageCompress.Size = new System.Drawing.Size(157, 17);
            chkBoxStageCompress.TabIndex = 7;
            chkBoxStageCompress.Text = "Automatically compress files";
            chkBoxStageCompress.UseVisualStyleBackColor = true;
            chkBoxStageCompress.CheckedChanged += new EventHandler(ChkBoxStageCompress_CheckedChanged);
            // 
            // groupBoxFighterCompression
            // 
            groupBoxFighterCompression.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                                 | AnchorStyles.Right);
            groupBoxFighterCompression.Controls.Add(chkBoxFighterPacDecompress);
            groupBoxFighterCompression.Controls.Add(chkBoxFighterPcsCompress);
            groupBoxFighterCompression.Location = new System.Drawing.Point(8, 6);
            groupBoxFighterCompression.Name = "groupBoxFighterCompression";
            groupBoxFighterCompression.Size = new System.Drawing.Size(349, 75);
            groupBoxFighterCompression.TabIndex = 16;
            groupBoxFighterCompression.TabStop = false;
            groupBoxFighterCompression.Text = "Fighters";
            // 
            // chkBoxFighterPacDecompress
            // 
            chkBoxFighterPacDecompress.AutoSize = true;
            chkBoxFighterPacDecompress.Location = new System.Drawing.Point(10, 22);
            chkBoxFighterPacDecompress.Name = "chkBoxFighterPacDecompress";
            chkBoxFighterPacDecompress.Size = new System.Drawing.Size(193, 17);
            chkBoxFighterPacDecompress.TabIndex = 7;
            chkBoxFighterPacDecompress.Text = "Automatically decompress PAC files";
            chkBoxFighterPacDecompress.UseVisualStyleBackColor = true;
            chkBoxFighterPacDecompress.CheckedChanged += new EventHandler(ChkBoxFighterPacDecompress_CheckedChanged);
            // 
            // chkBoxFighterPcsCompress
            // 
            chkBoxFighterPcsCompress.AutoSize = true;
            chkBoxFighterPcsCompress.Location = new System.Drawing.Point(10, 45);
            chkBoxFighterPcsCompress.Name = "chkBoxFighterPcsCompress";
            chkBoxFighterPcsCompress.Size = new System.Drawing.Size(181, 17);
            chkBoxFighterPcsCompress.TabIndex = 9;
            chkBoxFighterPcsCompress.Text = "Automatically compress PCS files";
            chkBoxFighterPcsCompress.UseVisualStyleBackColor = true;
            chkBoxFighterPcsCompress.CheckedChanged += new EventHandler(ChkBoxFighterPcsCompress_CheckedChanged);
            // 
            // tabFileAssociations
            // 
            tabFileAssociations.Controls.Add(genericFileAssociationBox);
            tabFileAssociations.Controls.Add(lblAdminApproval);
            tabFileAssociations.Controls.Add(btnApply);
            tabFileAssociations.Controls.Add(associatiedFilesBox);
            tabFileAssociations.Location = new System.Drawing.Point(4, 22);
            tabFileAssociations.Name = "tabFileAssociations";
            tabFileAssociations.Size = new System.Drawing.Size(365, 485);
            tabFileAssociations.TabIndex = 2;
            tabFileAssociations.Text = "File Associations";
            // 
            // genericFileAssociationBox
            // 
            genericFileAssociationBox.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                                   | AnchorStyles.Right);
            genericFileAssociationBox.Controls.Add(binFileAssociation);
            genericFileAssociationBox.Controls.Add(datFileAssociation);
            genericFileAssociationBox.Location = new System.Drawing.Point(8, 374);
            genericFileAssociationBox.Name = "genericFileAssociationBox";
            genericFileAssociationBox.Size = new System.Drawing.Size(349, 75);
            genericFileAssociationBox.TabIndex = 6;
            genericFileAssociationBox.TabStop = false;
            genericFileAssociationBox.Text = "Generic File Types";
            // 
            // binFileAssociation
            // 
            binFileAssociation.AutoSize = true;
            binFileAssociation.Location = new System.Drawing.Point(10, 45);
            binFileAssociation.Name = "binFileAssociation";
            binFileAssociation.Size = new System.Drawing.Size(135, 17);
            binFileAssociation.TabIndex = 9;
            binFileAssociation.Text = "Associate with .bin files";
            binFileAssociation.UseVisualStyleBackColor = true;
            binFileAssociation.CheckedChanged += new EventHandler(BinFileAssociation_CheckedChanged);
            // 
            // datFileAssociation
            // 
            datFileAssociation.AutoSize = true;
            datFileAssociation.Location = new System.Drawing.Point(10, 22);
            datFileAssociation.Name = "datFileAssociation";
            datFileAssociation.Size = new System.Drawing.Size(136, 17);
            datFileAssociation.TabIndex = 8;
            datFileAssociation.Text = "Associate with .dat files";
            datFileAssociation.UseVisualStyleBackColor = true;
            datFileAssociation.CheckedChanged += new EventHandler(DatFileAssociation_CheckedChanged);
            // 
            // lblAdminApproval
            // 
            lblAdminApproval.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                          | AnchorStyles.Right);
            lblAdminApproval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte) 0);
            lblAdminApproval.ForeColor = System.Drawing.Color.Red;
            lblAdminApproval.Location = new System.Drawing.Point(8, 459);
            lblAdminApproval.Name = "lblAdminApproval";
            lblAdminApproval.Size = new System.Drawing.Size(349, 18);
            lblAdminApproval.TabIndex = 5;
            lblAdminApproval.Text = "Administrator access required to make changes";
            lblAdminApproval.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApply
            // 
            btnApply.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnApply.Location = new System.Drawing.Point(287, 457);
            btnApply.Name = "btnApply";
            btnApply.Size = new System.Drawing.Size(75, 23);
            btnApply.TabIndex = 4;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += new EventHandler(BtnApply_Click);
            // 
            // associatiedFilesBox
            // 
            associatiedFilesBox.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                          | AnchorStyles.Left
                                                                          | AnchorStyles.Right);
            associatiedFilesBox.Controls.Add(chkBoxAssociateAll);
            associatiedFilesBox.Controls.Add(lstViewFileAssociations);
            associatiedFilesBox.Location = new System.Drawing.Point(8, 6);
            associatiedFilesBox.Name = "associatiedFilesBox";
            associatiedFilesBox.Size = new System.Drawing.Size(349, 362);
            associatiedFilesBox.TabIndex = 1;
            associatiedFilesBox.TabStop = false;
            associatiedFilesBox.Text = "Wii File Types";
            // 
            // chkBoxAssociateAll
            // 
            chkBoxAssociateAll.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            chkBoxAssociateAll.Location = new System.Drawing.Point(242, 336);
            chkBoxAssociateAll.Name = "chkBoxAssociateAll";
            chkBoxAssociateAll.RightToLeft = RightToLeft.Yes;
            chkBoxAssociateAll.Size = new System.Drawing.Size(104, 20);
            chkBoxAssociateAll.TabIndex = 5;
            chkBoxAssociateAll.Text = "Check All";
            chkBoxAssociateAll.UseVisualStyleBackColor = true;
            chkBoxAssociateAll.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            // 
            // lstViewFileAssociations
            // 
            lstViewFileAssociations.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                              | AnchorStyles.Left
                                                                              | AnchorStyles.Right);
            lstViewFileAssociations.AutoArrange = false;
            lstViewFileAssociations.BorderStyle = BorderStyle.FixedSingle;
            lstViewFileAssociations.CheckBoxes = true;
            lstViewFileAssociations.Columns.AddRange(new ColumnHeader[]
            {
                columnHeader1
            });
            lstViewFileAssociations.HeaderStyle = ColumnHeaderStyle.None;
            lstViewFileAssociations.HideSelection = false;
            lstViewFileAssociations.Location = new System.Drawing.Point(6, 19);
            lstViewFileAssociations.MultiSelect = false;
            lstViewFileAssociations.Name = "lstViewFileAssociations";
            lstViewFileAssociations.Size = new System.Drawing.Size(337, 311);
            lstViewFileAssociations.TabIndex = 6;
            lstViewFileAssociations.UseCompatibleStateImageBehavior = false;
            lstViewFileAssociations.View = View.Details;
            lstViewFileAssociations.ItemChecked += new ItemCheckedEventHandler(ListView1_ItemChecked);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 300;
            // 
            // tabBrawlAPI
            // 
            tabBrawlAPI.BackColor = System.Drawing.SystemColors.Control;
            tabBrawlAPI.Controls.Add(lblAPIRestartNeeded);
            tabBrawlAPI.Controls.Add(grpBoxLoaders);
            tabBrawlAPI.Controls.Add(grpBoxFSharpAPI);
            tabBrawlAPI.Controls.Add(grpBoxPythonAPI);
            tabBrawlAPI.Controls.Add(grpBoxAPIGeneral);
            tabBrawlAPI.Location = new System.Drawing.Point(4, 22);
            tabBrawlAPI.Name = "tabBrawlAPI";
            tabBrawlAPI.Padding = new Padding(3);
            tabBrawlAPI.Size = new System.Drawing.Size(365, 485);
            tabBrawlAPI.TabIndex = 5;
            tabBrawlAPI.Text = "BrawlAPI";
            // 
            // lblAPIRestartNeeded
            // 
            lblAPIRestartNeeded.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                             | AnchorStyles.Right);
            lblAPIRestartNeeded.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte) 0);
            lblAPIRestartNeeded.ForeColor = System.Drawing.Color.Red;
            lblAPIRestartNeeded.Location = new System.Drawing.Point(8, 459);
            lblAPIRestartNeeded.Name = "lblAPIRestartNeeded";
            lblAPIRestartNeeded.Size = new System.Drawing.Size(349, 18);
            lblAPIRestartNeeded.TabIndex = 25;
            lblAPIRestartNeeded.Text = "Program restart needed to apply changes";
            lblAPIRestartNeeded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpBoxLoaders
            // 
            grpBoxLoaders.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                    | AnchorStyles.Left
                                                                    | AnchorStyles.Right);
            grpBoxLoaders.Controls.Add(lstViewLoaders);
            grpBoxLoaders.Location = new System.Drawing.Point(8, 297);
            grpBoxLoaders.Name = "grpBoxLoaders";
            grpBoxLoaders.Size = new System.Drawing.Size(349, 159);
            grpBoxLoaders.TabIndex = 24;
            grpBoxLoaders.TabStop = false;
            grpBoxLoaders.Text = "Loaders";
            // 
            // lstViewLoaders
            // 
            lstViewLoaders.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                     | AnchorStyles.Left
                                                                     | AnchorStyles.Right);
            lstViewLoaders.AutoArrange = false;
            lstViewLoaders.BorderStyle = BorderStyle.FixedSingle;
            lstViewLoaders.CheckBoxes = true;
            lstViewLoaders.Columns.AddRange(new ColumnHeader[]
            {
                columnHeader2
            });
            lstViewLoaders.HeaderStyle = ColumnHeaderStyle.None;
            lstViewLoaders.HideSelection = false;
            lstViewLoaders.Location = new System.Drawing.Point(6, 19);
            lstViewLoaders.MultiSelect = false;
            lstViewLoaders.Name = "lstViewLoaders";
            lstViewLoaders.Size = new System.Drawing.Size(337, 134);
            lstViewLoaders.TabIndex = 7;
            lstViewLoaders.UseCompatibleStateImageBehavior = false;
            lstViewLoaders.View = View.Details;
            lstViewLoaders.ItemChecked += new ItemCheckedEventHandler(LstViewLoaders_ItemChecked);
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Name";
            columnHeader2.Width = 300;
            // 
            // grpBoxFSharpAPI
            // 
            grpBoxFSharpAPI.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                      | AnchorStyles.Right);
            grpBoxFSharpAPI.Controls.Add(txtBoxFSharpPath);
            grpBoxFSharpAPI.Controls.Add(btnFSharpBrowse);
            grpBoxFSharpAPI.Controls.Add(btnFSharpDetect);
            grpBoxFSharpAPI.Controls.Add(label2);
            grpBoxFSharpAPI.Location = new System.Drawing.Point(8, 218);
            grpBoxFSharpAPI.Name = "grpBoxFSharpAPI";
            grpBoxFSharpAPI.Size = new System.Drawing.Size(349, 73);
            grpBoxFSharpAPI.TabIndex = 23;
            grpBoxFSharpAPI.TabStop = false;
            grpBoxFSharpAPI.Text = "F#";
            // 
            // txtBoxFSharpPath
            // 
            txtBoxFSharpPath.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                       | AnchorStyles.Right);
            txtBoxFSharpPath.Location = new System.Drawing.Point(11, 38);
            txtBoxFSharpPath.Name = "txtBoxFSharpPath";
            txtBoxFSharpPath.Size = new System.Drawing.Size(219, 20);
            txtBoxFSharpPath.TabIndex = 3;
            txtBoxFSharpPath.Text = "(none)";
            txtBoxFSharpPath.TextChanged += new EventHandler(TxtBoxFSharpPath_TextChanged);
            // 
            // btnFSharpBrowse
            // 
            btnFSharpBrowse.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                      | AnchorStyles.Right);
            btnFSharpBrowse.Location = new System.Drawing.Point(236, 36);
            btnFSharpBrowse.Name = "btnFSharpBrowse";
            btnFSharpBrowse.Size = new System.Drawing.Size(24, 24);
            btnFSharpBrowse.TabIndex = 21;
            btnFSharpBrowse.Text = "...";
            btnFSharpBrowse.UseVisualStyleBackColor = true;
            btnFSharpBrowse.Click += new EventHandler(BtnFSharpBrowse_Click);
            // 
            // btnFSharpDetect
            // 
            btnFSharpDetect.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                      | AnchorStyles.Right);
            btnFSharpDetect.Location = new System.Drawing.Point(266, 36);
            btnFSharpDetect.Name = "btnFSharpDetect";
            btnFSharpDetect.Size = new System.Drawing.Size(75, 24);
            btnFSharpDetect.TabIndex = 22;
            btnFSharpDetect.Text = "Auto-Detect";
            btnFSharpDetect.UseVisualStyleBackColor = true;
            btnFSharpDetect.Click += new EventHandler(BtnFSharpDetect_Click);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(8, 22);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(85, 13);
            label2.TabIndex = 13;
            label2.Text = "Installation Path:";
            // 
            // grpBoxPythonAPI
            // 
            grpBoxPythonAPI.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                      | AnchorStyles.Right);
            grpBoxPythonAPI.Controls.Add(txtBoxPythonPath);
            grpBoxPythonAPI.Controls.Add(btnPythonBrowse);
            grpBoxPythonAPI.Controls.Add(btnPythonDetect);
            grpBoxPythonAPI.Controls.Add(label1);
            grpBoxPythonAPI.Location = new System.Drawing.Point(8, 139);
            grpBoxPythonAPI.Name = "grpBoxPythonAPI";
            grpBoxPythonAPI.Size = new System.Drawing.Size(349, 73);
            grpBoxPythonAPI.TabIndex = 20;
            grpBoxPythonAPI.TabStop = false;
            grpBoxPythonAPI.Text = "Python";
            // 
            // txtBoxPythonPath
            // 
            txtBoxPythonPath.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                       | AnchorStyles.Right);
            txtBoxPythonPath.Location = new System.Drawing.Point(11, 38);
            txtBoxPythonPath.Name = "txtBoxPythonPath";
            txtBoxPythonPath.Size = new System.Drawing.Size(219, 20);
            txtBoxPythonPath.TabIndex = 3;
            txtBoxPythonPath.Text = "(none)";
            txtBoxPythonPath.TextChanged += new EventHandler(TxtBoxPythonPath_TextChanged);
            // 
            // btnPythonBrowse
            // 
            btnPythonBrowse.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                      | AnchorStyles.Right);
            btnPythonBrowse.Location = new System.Drawing.Point(236, 36);
            btnPythonBrowse.Name = "btnPythonBrowse";
            btnPythonBrowse.Size = new System.Drawing.Size(24, 24);
            btnPythonBrowse.TabIndex = 21;
            btnPythonBrowse.Text = "...";
            btnPythonBrowse.UseVisualStyleBackColor = true;
            btnPythonBrowse.Click += new EventHandler(BtnPythonBrowse_Click);
            // 
            // btnPythonDetect
            // 
            btnPythonDetect.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                      | AnchorStyles.Right);
            btnPythonDetect.Location = new System.Drawing.Point(266, 36);
            btnPythonDetect.Name = "btnPythonDetect";
            btnPythonDetect.Size = new System.Drawing.Size(75, 24);
            btnPythonDetect.TabIndex = 22;
            btnPythonDetect.Text = "Auto-Detect";
            btnPythonDetect.UseVisualStyleBackColor = true;
            btnPythonDetect.Click += new EventHandler(BtnPythonDetect_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(8, 22);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 13);
            label1.TabIndex = 13;
            label1.Text = "Installation Path:";
            // 
            // grpBoxAPIGeneral
            // 
            grpBoxAPIGeneral.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                       | AnchorStyles.Right);
            grpBoxAPIGeneral.Controls.Add(grpBoxLoaderBehavior);
            grpBoxAPIGeneral.Controls.Add(chkBoxEnableAPI);
            grpBoxAPIGeneral.Location = new System.Drawing.Point(8, 6);
            grpBoxAPIGeneral.Name = "grpBoxAPIGeneral";
            grpBoxAPIGeneral.Size = new System.Drawing.Size(349, 127);
            grpBoxAPIGeneral.TabIndex = 19;
            grpBoxAPIGeneral.TabStop = false;
            grpBoxAPIGeneral.Text = "BrawlAPI";
            // 
            // grpBoxLoaderBehavior
            // 
            grpBoxLoaderBehavior.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                              | AnchorStyles.Right);
            grpBoxLoaderBehavior.Controls.Add(rdoAPILoaderWhitelist);
            grpBoxLoaderBehavior.Controls.Add(rdoAPILoaderBlacklist);
            grpBoxLoaderBehavior.Location = new System.Drawing.Point(6, 45);
            grpBoxLoaderBehavior.Name = "grpBoxLoaderBehavior";
            grpBoxLoaderBehavior.Size = new System.Drawing.Size(337, 75);
            grpBoxLoaderBehavior.TabIndex = 11;
            grpBoxLoaderBehavior.TabStop = false;
            grpBoxLoaderBehavior.Text = "Loader Behavior";
            // 
            // rdoAPILoaderWhitelist
            // 
            rdoAPILoaderWhitelist.AutoSize = true;
            rdoAPILoaderWhitelist.Location = new System.Drawing.Point(10, 45);
            rdoAPILoaderWhitelist.Name = "rdoAPILoaderWhitelist";
            rdoAPILoaderWhitelist.Size = new System.Drawing.Size(269, 17);
            rdoAPILoaderWhitelist.TabIndex = 1;
            rdoAPILoaderWhitelist.TabStop = true;
            rdoAPILoaderWhitelist.Text = "Whitelist wanted loaders (Loaders are off by default)";
            rdoAPILoaderWhitelist.UseVisualStyleBackColor = true;
            rdoAPILoaderWhitelist.CheckedChanged += new EventHandler(ChkBoxAPILoaderBehavior_CheckedChanged);
            // 
            // rdoAPILoaderBlacklist
            // 
            rdoAPILoaderBlacklist.AutoSize = true;
            rdoAPILoaderBlacklist.Location = new System.Drawing.Point(10, 22);
            rdoAPILoaderBlacklist.Name = "rdoAPILoaderBlacklist";
            rdoAPILoaderBlacklist.Size = new System.Drawing.Size(280, 17);
            rdoAPILoaderBlacklist.TabIndex = 0;
            rdoAPILoaderBlacklist.TabStop = true;
            rdoAPILoaderBlacklist.Text = "Blacklist unwanted loaders (Loaders are on by default)";
            rdoAPILoaderBlacklist.UseVisualStyleBackColor = true;
            rdoAPILoaderBlacklist.CheckedChanged += new EventHandler(ChkBoxAPILoaderBehavior_CheckedChanged);
            // 
            // chkBoxEnableAPI
            // 
            chkBoxEnableAPI.AutoSize = true;
            chkBoxEnableAPI.Location = new System.Drawing.Point(10, 22);
            chkBoxEnableAPI.Name = "chkBoxEnableAPI";
            chkBoxEnableAPI.Size = new System.Drawing.Size(105, 17);
            chkBoxEnableAPI.TabIndex = 7;
            chkBoxEnableAPI.Text = "Enable BrawlAPI";
            chkBoxEnableAPI.UseVisualStyleBackColor = true;
            chkBoxEnableAPI.CheckedChanged += new EventHandler(ChkBoxEnableAPI_CheckedChanged);
            // 
            // tabDiscord
            // 
            tabDiscord.BackColor = System.Drawing.SystemColors.Control;
            tabDiscord.Controls.Add(grpBoxDiscordRPC);
            tabDiscord.Location = new System.Drawing.Point(4, 22);
            tabDiscord.Name = "tabDiscord";
            tabDiscord.Padding = new Padding(3);
            tabDiscord.Size = new System.Drawing.Size(365, 485);
            tabDiscord.TabIndex = 4;
            tabDiscord.Text = "Discord";
            // 
            // grpBoxDiscordRPC
            // 
            grpBoxDiscordRPC.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                       | AnchorStyles.Right);
            grpBoxDiscordRPC.Controls.Add(chkBoxEnableDiscordRPC);
            grpBoxDiscordRPC.Controls.Add(grpBoxDiscordRPCType);
            grpBoxDiscordRPC.Location = new System.Drawing.Point(8, 6);
            grpBoxDiscordRPC.Name = "grpBoxDiscordRPC";
            grpBoxDiscordRPC.Size = new System.Drawing.Size(349, 172);
            grpBoxDiscordRPC.TabIndex = 0;
            grpBoxDiscordRPC.TabStop = false;
            grpBoxDiscordRPC.Text = "Rich Presence";
            // 
            // chkBoxEnableDiscordRPC
            // 
            chkBoxEnableDiscordRPC.AutoSize = true;
            chkBoxEnableDiscordRPC.Location = new System.Drawing.Point(10, 22);
            chkBoxEnableDiscordRPC.Name = "chkBoxEnableDiscordRPC";
            chkBoxEnableDiscordRPC.Size = new System.Drawing.Size(171, 17);
            chkBoxEnableDiscordRPC.TabIndex = 1;
            chkBoxEnableDiscordRPC.Text = "Enable Discord Rich Presence";
            chkBoxEnableDiscordRPC.UseVisualStyleBackColor = true;
            chkBoxEnableDiscordRPC.CheckedChanged += new EventHandler(ChkBoxEnableDiscordRPC_CheckedChanged);
            // 
            // grpBoxDiscordRPCType
            // 
            grpBoxDiscordRPCType.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                              | AnchorStyles.Right);
            grpBoxDiscordRPCType.Controls.Add(DiscordRPCCustomName);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameCustom);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameExternal);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameInternal);
            grpBoxDiscordRPCType.Controls.Add(rdoDiscordRPCNameDisabled);
            grpBoxDiscordRPCType.Location = new System.Drawing.Point(6, 45);
            grpBoxDiscordRPCType.Name = "grpBoxDiscordRPCType";
            grpBoxDiscordRPCType.Size = new System.Drawing.Size(337, 119);
            grpBoxDiscordRPCType.TabIndex = 0;
            grpBoxDiscordRPCType.TabStop = false;
            grpBoxDiscordRPCType.Text = "Mod Name Detection";
            // 
            // DiscordRPCCustomName
            // 
            DiscordRPCCustomName.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                           | AnchorStyles.Right);
            DiscordRPCCustomName.Location = new System.Drawing.Point(30, 88);
            DiscordRPCCustomName.Name = "DiscordRPCCustomName";
            DiscordRPCCustomName.Size = new System.Drawing.Size(301, 20);
            DiscordRPCCustomName.TabIndex = 2;
            DiscordRPCCustomName.Text = "My Mod";
            DiscordRPCCustomName.TextChanged += new EventHandler(DiscordRPCCustomName_TextChanged);
            // 
            // rdoDiscordRPCNameCustom
            // 
            rdoDiscordRPCNameCustom.AutoSize = true;
            rdoDiscordRPCNameCustom.Location = new System.Drawing.Point(10, 91);
            rdoDiscordRPCNameCustom.Name = "rdoDiscordRPCNameCustom";
            rdoDiscordRPCNameCustom.Size = new System.Drawing.Size(14, 13);
            rdoDiscordRPCNameCustom.TabIndex = 3;
            rdoDiscordRPCNameCustom.TabStop = true;
            rdoDiscordRPCNameCustom.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameCustom.CheckedChanged += new EventHandler(DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameExternal
            // 
            rdoDiscordRPCNameExternal.AutoSize = true;
            rdoDiscordRPCNameExternal.Location = new System.Drawing.Point(10, 68);
            rdoDiscordRPCNameExternal.Name = "rdoDiscordRPCNameExternal";
            rdoDiscordRPCNameExternal.Size = new System.Drawing.Size(126, 17);
            rdoDiscordRPCNameExternal.TabIndex = 2;
            rdoDiscordRPCNameExternal.TabStop = true;
            rdoDiscordRPCNameExternal.Text = "Use external filename";
            rdoDiscordRPCNameExternal.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameExternal.CheckedChanged += new EventHandler(DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameInternal
            // 
            rdoDiscordRPCNameInternal.AutoSize = true;
            rdoDiscordRPCNameInternal.Location = new System.Drawing.Point(10, 45);
            rdoDiscordRPCNameInternal.Name = "rdoDiscordRPCNameInternal";
            rdoDiscordRPCNameInternal.Size = new System.Drawing.Size(123, 17);
            rdoDiscordRPCNameInternal.TabIndex = 1;
            rdoDiscordRPCNameInternal.TabStop = true;
            rdoDiscordRPCNameInternal.Text = "Use internal filename";
            rdoDiscordRPCNameInternal.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameInternal.CheckedChanged += new EventHandler(DiscordRPCNameSettings_CheckedChanged);
            // 
            // rdoDiscordRPCNameDisabled
            // 
            rdoDiscordRPCNameDisabled.AutoSize = true;
            rdoDiscordRPCNameDisabled.Location = new System.Drawing.Point(10, 22);
            rdoDiscordRPCNameDisabled.Name = "rdoDiscordRPCNameDisabled";
            rdoDiscordRPCNameDisabled.Size = new System.Drawing.Size(66, 17);
            rdoDiscordRPCNameDisabled.TabIndex = 0;
            rdoDiscordRPCNameDisabled.TabStop = true;
            rdoDiscordRPCNameDisabled.Text = "Disabled";
            rdoDiscordRPCNameDisabled.UseVisualStyleBackColor = true;
            rdoDiscordRPCNameDisabled.CheckedChanged += new EventHandler(DiscordRPCNameSettings_CheckedChanged);
            // 
            // tabUpdater
            // 
            tabUpdater.Controls.Add(grpBoxAPIUpdate);
            tabUpdater.Controls.Add(grpBoxCanary);
            tabUpdater.Controls.Add(updaterBehaviorGroupbox);
            tabUpdater.Location = new System.Drawing.Point(4, 22);
            tabUpdater.Name = "tabUpdater";
            tabUpdater.Padding = new Padding(3);
            tabUpdater.Size = new System.Drawing.Size(365, 485);
            tabUpdater.TabIndex = 1;
            tabUpdater.Text = "Updater";
            // 
            // grpBoxAPIUpdate
            // 
            grpBoxAPIUpdate.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                      | AnchorStyles.Right);
            grpBoxAPIUpdate.Controls.Add(btnManageSubscriptions);
            grpBoxAPIUpdate.Controls.Add(chkBoxUpdateAPI);
            grpBoxAPIUpdate.Location = new System.Drawing.Point(8, 191);
            grpBoxAPIUpdate.Name = "grpBoxAPIUpdate";
            grpBoxAPIUpdate.Size = new System.Drawing.Size(349, 82);
            grpBoxAPIUpdate.TabIndex = 15;
            grpBoxAPIUpdate.TabStop = false;
            grpBoxAPIUpdate.Text = "BrawlAPI Scripts";
            // 
            // btnManageSubscriptions
            // 
            btnManageSubscriptions.Location = new System.Drawing.Point(10, 45);
            btnManageSubscriptions.Name = "btnManageSubscriptions";
            btnManageSubscriptions.Size = new System.Drawing.Size(140, 24);
            btnManageSubscriptions.TabIndex = 24;
            btnManageSubscriptions.Text = "Manage Subscriptions";
            btnManageSubscriptions.UseVisualStyleBackColor = true;
            btnManageSubscriptions.Click += new EventHandler(BtnManageSubscriptions_Click);
            // 
            // chkBoxUpdateAPI
            // 
            chkBoxUpdateAPI.AutoSize = true;
            chkBoxUpdateAPI.Location = new System.Drawing.Point(10, 22);
            chkBoxUpdateAPI.Name = "chkBoxUpdateAPI";
            chkBoxUpdateAPI.Size = new System.Drawing.Size(200, 17);
            chkBoxUpdateAPI.TabIndex = 11;
            chkBoxUpdateAPI.Text = "Update API Scripts on update check";
            chkBoxUpdateAPI.UseVisualStyleBackColor = true;
            chkBoxUpdateAPI.CheckedChanged += new EventHandler(ChkBoxUpdateAPI_CheckedChanged);
            // 
            // grpBoxCanary
            // 
            grpBoxCanary.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                   | AnchorStyles.Right);
            grpBoxCanary.Controls.Add(chkCanary);
            grpBoxCanary.Location = new System.Drawing.Point(8, 132);
            grpBoxCanary.Name = "grpBoxCanary";
            grpBoxCanary.Size = new System.Drawing.Size(349, 53);
            grpBoxCanary.TabIndex = 15;
            grpBoxCanary.TabStop = false;
            grpBoxCanary.Text = "BrawlCrate Canary";
            // 
            // updaterBehaviorGroupbox
            // 
            updaterBehaviorGroupbox.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                              | AnchorStyles.Right);
            updaterBehaviorGroupbox.Controls.Add(rdoAutoUpdate);
            updaterBehaviorGroupbox.Controls.Add(rdoCheckManual);
            updaterBehaviorGroupbox.Controls.Add(rdoCheckStartup);
            updaterBehaviorGroupbox.Controls.Add(chkDocUpdates);
            updaterBehaviorGroupbox.Location = new System.Drawing.Point(8, 6);
            updaterBehaviorGroupbox.Name = "updaterBehaviorGroupbox";
            updaterBehaviorGroupbox.Size = new System.Drawing.Size(349, 120);
            updaterBehaviorGroupbox.TabIndex = 14;
            updaterBehaviorGroupbox.TabStop = false;
            updaterBehaviorGroupbox.Text = "Updater Behavior";
            // 
            // rdoAutoUpdate
            // 
            rdoAutoUpdate.AutoSize = true;
            rdoAutoUpdate.Location = new System.Drawing.Point(10, 22);
            rdoAutoUpdate.Name = "rdoAutoUpdate";
            rdoAutoUpdate.Size = new System.Drawing.Size(72, 17);
            rdoAutoUpdate.TabIndex = 2;
            rdoAutoUpdate.TabStop = true;
            rdoAutoUpdate.Text = "Automatic";
            rdoAutoUpdate.UseVisualStyleBackColor = true;
            rdoAutoUpdate.CheckedChanged += new EventHandler(UpdaterBehavior_CheckedChanged);
            // 
            // rdoCheckManual
            // 
            rdoCheckManual.AutoSize = true;
            rdoCheckManual.Location = new System.Drawing.Point(10, 68);
            rdoCheckManual.Name = "rdoCheckManual";
            rdoCheckManual.Size = new System.Drawing.Size(60, 17);
            rdoCheckManual.TabIndex = 1;
            rdoCheckManual.TabStop = true;
            rdoCheckManual.Text = "Manual";
            rdoCheckManual.UseVisualStyleBackColor = true;
            // 
            // rdoCheckStartup
            // 
            rdoCheckStartup.AutoSize = true;
            rdoCheckStartup.Location = new System.Drawing.Point(10, 45);
            rdoCheckStartup.Name = "rdoCheckStartup";
            rdoCheckStartup.Size = new System.Drawing.Size(220, 17);
            rdoCheckStartup.TabIndex = 0;
            rdoCheckStartup.TabStop = true;
            rdoCheckStartup.Text = "Manual, but check for updates on startup";
            rdoCheckStartup.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            ClientSize = new System.Drawing.Size(373, 511);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "SettingsDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            Shown += new EventHandler(SettingsDialog_Shown);
            tabControl1.ResumeLayout(false);
            tabGeneral.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            grpBoxMDL0General.ResumeLayout(false);
            grpBoxMDL0General.PerformLayout();
            grpBoxAudioGeneral.ResumeLayout(false);
            grpBoxAudioGeneral.PerformLayout();
            grpBoxMainFormGeneral.ResumeLayout(false);
            grpBoxMainFormGeneral.PerformLayout();
            grpBoxFileNameDisplayGeneral.ResumeLayout(false);
            grpBoxFileNameDisplayGeneral.PerformLayout();
            tabCompression.ResumeLayout(false);
            groupBoxModuleCompression.ResumeLayout(false);
            groupBoxModuleCompression.PerformLayout();
            groupBoxStageCompression.ResumeLayout(false);
            groupBoxStageCompression.PerformLayout();
            groupBoxFighterCompression.ResumeLayout(false);
            groupBoxFighterCompression.PerformLayout();
            tabFileAssociations.ResumeLayout(false);
            genericFileAssociationBox.ResumeLayout(false);
            genericFileAssociationBox.PerformLayout();
            associatiedFilesBox.ResumeLayout(false);
            tabBrawlAPI.ResumeLayout(false);
            grpBoxLoaders.ResumeLayout(false);
            grpBoxFSharpAPI.ResumeLayout(false);
            grpBoxFSharpAPI.PerformLayout();
            grpBoxPythonAPI.ResumeLayout(false);
            grpBoxPythonAPI.PerformLayout();
            grpBoxAPIGeneral.ResumeLayout(false);
            grpBoxAPIGeneral.PerformLayout();
            grpBoxLoaderBehavior.ResumeLayout(false);
            grpBoxLoaderBehavior.PerformLayout();
            tabDiscord.ResumeLayout(false);
            grpBoxDiscordRPC.ResumeLayout(false);
            grpBoxDiscordRPC.PerformLayout();
            grpBoxDiscordRPCType.ResumeLayout(false);
            grpBoxDiscordRPCType.PerformLayout();
            tabUpdater.ResumeLayout(false);
            grpBoxAPIUpdate.ResumeLayout(false);
            grpBoxAPIUpdate.PerformLayout();
            grpBoxCanary.ResumeLayout(false);
            grpBoxCanary.PerformLayout();
            updaterBehaviorGroupbox.ResumeLayout(false);
            updaterBehaviorGroupbox.PerformLayout();
            ResumeLayout(false);
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
#if !CANARY
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

            chkCanary.Checked = false;
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

            chkCanary.Checked = true;
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
            using (VistaFolderBrowserDialog f = new VistaFolderBrowserDialog
                {UseDescriptionForTitle = true})
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
            BrawlAPI.PythonInstall(true, true);
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
            BrawlAPI.FSharpInstall(true, true);
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
            using (VistaFolderBrowserDialog f = new VistaFolderBrowserDialog
                {UseDescriptionForTitle = true})
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
                    if (Properties.Settings.Default.APILoadersBlacklist == null &&
                        !(Properties.Settings.Default.APILoadersBlacklist?.Contains(e.Item.Text) ?? false))
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
                if (Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist &&
                    !(Properties.Settings.Default.APILoadersWhitelist?.Contains(e.Item.Text) ?? false))
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

        private void chkBoxParseMoveDef_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            BrawlLib.Properties.Settings.Default.ParseMoveDef = chkBoxParseMoveDef.Checked;
            BrawlLib.Properties.Settings.Default.Save();
        }
    }
}
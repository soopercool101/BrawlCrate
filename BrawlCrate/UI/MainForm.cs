using Be.Windows.Forms;
using BrawlCrate.API;
using BrawlCrate.NodeWrappers;
using BrawlCrate.Properties;
using BrawlLib;
using BrawlLib.Imaging;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using IronPython.Runtime;
using System;
using System.Audio;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate
{
    public class MainForm : Form
    {
        private static MainForm _instance;
        public static MainForm Instance => _instance ?? (_instance = new MainForm());

        public BaseWrapper RootNode { get; private set; }

        private SettingsDialog _settings;
        private SettingsDialog Settings => _settings ?? (_settings = new SettingsDialog());

        public readonly RecentFileHandler RecentFilesHandler;

        private InterpolationForm _interpolationForm;

        public InterpolationForm InterpolationForm
        {
            get
            {
                if (_interpolationForm == null)
                {
                    _interpolationForm = new InterpolationForm(null);
                    _interpolationForm.FormClosed += _interpolationForm_FormClosed;
                    _interpolationForm.Show();
                }

                return _interpolationForm;
            }
        }

        private void _interpolationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _interpolationForm = null;
        }

        private void _enableEditMenu(object sender, EventArgs e)
        {
            BaseWrapper w = resourceTree?.SelectedNode as BaseWrapper;
            if (w == null)
            {
                editToolStripMenuItem.Enabled = false;
                return;
            }
            editToolStripMenuItem.Enabled = (editToolStripMenuItem.DropDown =
                                                Instance.resourceTree.SelectedNodes.Count > 1
                                                    ? Instance.resourceTree.GetMultiSelectMenuStrip()
                                                    : w?.ContextMenuStrip) != null;
        }

        private void _disableEditMenu(object sender, EventArgs e)
        {
            editToolStripMenuItem.DropDown = null;
            editToolStripMenuItem.Enabled = false;
        }

        public MainForm()
        {
            InitializeComponent();
            Text = Program.AssemblyTitleFull;

            _autoUpdate = Properties.Settings.Default.UpdateAutomatically;
            _displayPropertyDescription = Properties.Settings.Default.DisplayPropertyDescriptionWhenAvailable;
            _updatesOnStartup = Properties.Settings.Default.CheckUpdatesAtStartup;
            _docUpdates = Properties.Settings.Default.GetDocumentationUpdates;
            _showHex = Properties.Settings.Default.ShowHex;
            _autoCompressModules = BrawlLib.Properties.Settings.Default.AutoCompressModules;
            _autoCompressPCS = BrawlLib.Properties.Settings.Default.AutoCompressFighterPCS;
            _autoDecompressPAC = BrawlLib.Properties.Settings.Default.AutoDecompressFighterPAC;
            _autoCompressStages = BrawlLib.Properties.Settings.Default.AutoCompressStages;
            _autoPlayAudio = BrawlLib.Properties.Settings.Default.AutoPlayAudio;
            _showFullPath = Properties.Settings.Default.ShowFullPath;
            _showBRRESPreviews = Properties.Settings.Default.PreviewBRRESModels;
            _showARCPreviews = Properties.Settings.Default.PreviewARCModels;

            Activated += _enableEditMenu;
            Deactivate += _disableEditMenu;

#if !DEBUG //Don't need to see this every time a debug build is compiled
            if (CheckUpdatesOnStartup)
            {
                CheckUpdates(false);
            }
#endif

            soundPackControl1._grid = propertyGrid1;
            soundPackControl1.lstSets.SmallImageList = Icons.ImageList;
            foreach (Control c in splitContainer2.Panel2.Controls)
            {
                c.Visible = false;
                c.Dock = DockStyle.Fill;
            }

            m_DelegateOpenFile = new DelegateOpenFile(Program.Open);
            _instance = this;

            _currentControl = modelPanel1;

            modelPanel1.CurrentViewport._allowSelection = false;

            RecentFilesHandler = new RecentFileHandler(components)
            {
                RecentFileToolStripItem = recentFilesToolStripMenuItem
            };

            if (Properties.Settings.Default.APIEnabled)
            {
                BrawlAPI.Plugins.Clear();
                BrawlAPI.ResourceParsers.Clear();

                pluginToolStripMenuItem.DropDown.Items.Clear();
                if (Directory.Exists(Program.ApiPluginPath))
                {
                    reloadPluginsToolStripMenuItem_Click(null, null);
                }

                if (Directory.Exists(Program.ApiLoaderPath))
                {
                    foreach (FileInfo f in GetScripts(Program.ApiLoaderPath))
                    {
                        // Only load loaders that should be loaded.
                        //  If blacklist behavior is on, load all scripts excluding those blacklisted
                        //  If whitelist behavior is on, load only whitelisted scripts
                        if (Properties.Settings.Default.APIOnlyAllowLoadersFromWhitelist
                            ? Properties.Settings.Default.APILoadersWhitelist?.Contains(
                                    f.FullName.Substring(Program.ApiLoaderPath.Length).TrimStart('\\')) ??
                                false
                            : !Properties.Settings.Default.APILoadersBlacklist?.Contains(
                                    f.FullName.Substring(Program.ApiLoaderPath.Length).TrimStart('\\')) ??
                                true)
                        {
                            BrawlAPI.CreatePlugin(f.FullName, true);
                        }
                    }
                }
            }
            else
            {
                pluginToolStripMenuItem.Visible = false;
                pluginToolStripMenuItem.Enabled = false;
            }
        }

        private delegate bool DelegateOpenFile(string s);

        private readonly DelegateOpenFile m_DelegateOpenFile;

        private void CheckUpdates(bool manual = true)
        {
            try
            {
                if (Program.CanRunGithubApp(manual, out string path))
                {
#if CANARY
                    Process git = Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = $"-buc \"{Program.RootPath ?? "<null>"}\" {(manual ? "1" : "0")}"
                    });
                    git?.WaitForExit();
                    if (File.Exists(Program.AppPath + "\\Canary\\Old"))
                    {
                        Process changelog = Process.Start(new ProcessStartInfo()
                        {
                            FileName = path,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = "-canarylog"
                        });
                        changelog?.WaitForExit();
                    }
#else
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments =
                            $"-bu 1 \"{Program.TagName}\" {(manual ? "1" : "0")} \"{Program.RootPath ?? "<null>"}\" {(_docUpdates ? "1" : "0")} {(!manual && _autoUpdate ? "1" : "0")}"
                    });
#endif
                }
                else
                {
                    if (manual)
                    {
                        MessageBox.Show("The updater could not be found.");
                    }

                    checkForUpdatesToolStripMenuItem.Enabled =
                        checkForUpdatesToolStripMenuItem.Visible = false;
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

        public bool DisplayPropertyDescriptionsWhenAvailable
        {
            get => _displayPropertyDescription;
            set
            {
                _displayPropertyDescription = value;

                Properties.Settings.Default.DisplayPropertyDescriptionWhenAvailable = _displayPropertyDescription;
                Properties.Settings.Default.Save();
                UpdatePropertyDescriptionBox(propertyGrid1.SelectedGridItem);
            }
        }

        private bool _displayPropertyDescription;

        public bool CheckUpdatesOnStartup
        {
            get => _updatesOnStartup;
            set
            {
                _updatesOnStartup = value;

                Properties.Settings.Default.CheckUpdatesAtStartup = _updatesOnStartup;
                Properties.Settings.Default.Save();
            }
        }

        private bool _updatesOnStartup;

        public bool GetDocumentationUpdates
        {
            get => _docUpdates;
            set
            {
                _docUpdates = value;

                Properties.Settings.Default.GetDocumentationUpdates = _docUpdates;
                Properties.Settings.Default.Save();
            }
        }

        private bool _docUpdates;

        public bool AutoCompressPCS
        {
            get => _autoCompressPCS;
            set
            {
                _autoCompressPCS = value;

                BrawlLib.Properties.Settings.Default.AutoCompressFighterPCS = _autoCompressPCS;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }

        private bool _autoCompressPCS;

        public bool AutoDecompressFighterPAC
        {
            get => _autoDecompressPAC;
            set
            {
                _autoDecompressPAC = value;

                BrawlLib.Properties.Settings.Default.AutoDecompressFighterPAC = _autoDecompressPAC;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }

        private bool _autoDecompressPAC;

        public bool AutoCompressStages
        {
            get => _autoCompressStages;
            set
            {
                _autoCompressStages = value;

                BrawlLib.Properties.Settings.Default.AutoCompressStages = _autoCompressStages;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }

        private bool _autoCompressStages;

        public bool AutoCompressModules
        {
            get => _autoCompressModules;
            set
            {
                _autoCompressModules = value;

                BrawlLib.Properties.Settings.Default.AutoCompressStages = _autoCompressModules;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }

        private bool _autoCompressModules;

        public bool AutoPlayAudio
        {
            get => _autoPlayAudio;
            set
            {
                _autoPlayAudio = value;

                BrawlLib.Properties.Settings.Default.AutoPlayAudio = _autoPlayAudio;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }

        private bool _autoPlayAudio;

        public bool UpdateAutomatically

        {
            get => _autoUpdate;
            set
            {
                _autoUpdate = value;

                Properties.Settings.Default.UpdateAutomatically = _autoUpdate;
                Properties.Settings.Default.Save();
            }
        }

        private bool _autoUpdate;

        public bool ShowHex
        {
            get => _showHex;
            set
            {
                _showHex = value;

                Properties.Settings.Default.ShowHex = _showHex;
                Properties.Settings.Default.Save();
                resourceTree_SelectionChanged(null, null);
            }
        }

        private bool _showHex;

        public bool CompatibilityMode
        {
            get => _compatibilityMode;
            set
            {
                _compatibilityMode = value;

                BrawlLib.Properties.Settings.Default.HideMDL0Errors =
                    BrawlLib.Properties.Settings.Default.CompatibilityMode = _compatibilityMode;
                BrawlLib.Properties.Settings.Default.Save();
                resourceTree_SelectionChanged(null, null);
            }
        }

        private bool _compatibilityMode;

        public bool ShowFullPath
        {
            get => _showFullPath;
            set
            {
                _showFullPath = value;

                Properties.Settings.Default.ShowFullPath = _showFullPath;
                Properties.Settings.Default.Save();
                UpdateName();
            }
        }

        private bool _showFullPath;

        public bool ShowBRRESPreviews
        {
            get => _showBRRESPreviews;
            set
            {
                _showBRRESPreviews = value;

                Properties.Settings.Default.PreviewBRRESModels = _showBRRESPreviews;
                Properties.Settings.Default.Save();
                resourceTree_SelectionChanged(null, null);
            }
        }

        private bool _showBRRESPreviews;

        public bool ShowARCPreviews
        {
            get => _showARCPreviews;
            set
            {
                _showARCPreviews = value;

                Properties.Settings.Default.PreviewARCModels = _showARCPreviews;
                Properties.Settings.Default.Save();
                resourceTree_SelectionChanged(null, null);
            }
        }

        private bool _showARCPreviews;

        private void UpdatePropertyDescriptionBox(GridItem item)
        {
            if (!DisplayPropertyDescriptionsWhenAvailable)
            {
                if (propertyGrid1.HelpVisible)
                {
                    propertyGrid1.HelpVisible = false;
                }
            }
            else
            {
                propertyGrid1.HelpVisible = item?.PropertyDescriptor != null && !string.IsNullOrEmpty(item.PropertyDescriptor.Description);
            }
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (DisplayPropertyDescriptionsWhenAvailable)
            {
                UpdatePropertyDescriptionBox(e.NewSelection);
            }
        }

        public void Reset()
        {
            RootNode = null;
            resourceTree.SelectedNode = null;
            resourceTree.Clear();

            if (Program.RootNode != null)
            {
                RootNode = BaseWrapper.Wrap(this, Program.RootNode);
                resourceTree.BeginUpdate();
                resourceTree.Nodes.Add((TreeNode) RootNode);
                resourceTree.SelectedNode = RootNode;
                RootNode.Expand();
                resourceTree.EndUpdate();

                closeToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;

                Program.RootNode._mainForm = this;
            }
            else
            {
                closeToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
            }

            resourceTree_SelectionChanged(null, null);

            UpdateName();
            UpdateDiscordRPC(null, null);
        }




        public void UpdateName()
        {
            if (Program.RootPath != null)
            {
                string fileName = ShowFullPath
                    ? Program.RootPath
                    : Program.RootPath.TrimEnd('\\').Substring(Program.RootPath.TrimEnd('\\').LastIndexOf('\\') + 1);
                Text = $"{Program.AssemblyTitleShort} - {fileName}";
            }
            else
            {
                Text = Program.AssemblyTitleFull;
            }
        }

        public void TargetResource(ResourceNode n)
        {
            if (RootNode != null)
            {
                resourceTree.SelectedNode = RootNode.FindResource(n, true);
            }
        }

        public Control _currentControl;
        private Control _secondaryControl;
        private Type selectedType;

        public unsafe void resourceTree_SelectionChanged(object sender, EventArgs e)
        {
            audioPlaybackPanel1.TargetSource = null;
            animEditControl.TargetSequence = null;
            texAnimEditControl.TargetSequence = null;
            shpAnimEditControl.TargetSequence = null;
            articleAttributeGrid.TargetNode = null;
            offsetEditor1.TargetNode = null;
            msBinEditor1.CurrentNode = null;
            //soundPackControl1.TargetNode = null;
            movesetEditor1.TargetNode = null;
            attributeControl.TargetNode = null;
            clrControl.ColorSource = null;
            visEditor.TargetNode = null;
            scN0CameraEditControl1.TargetSequence = null;
            scN0LightEditControl1.TargetSequence = null;
            scN0FogEditControl1.TargetSequence = null;
            texCoordControl1.TargetNode = null;
            ppcDisassembler1.SetTarget(null, 0, null);
            modelPanel1.ClearAll();
            mdL0ObjectControl1.SetTarget(null);
            ((DynamicFileByteProvider) hexBox1.ByteProvider)?.Dispose();

            Control newControl = null;
            Control newControl2 = null;

            BaseWrapper w;
            ResourceNode node = null;
            bool disable2nd = false;
            if (!(sender?.ToString().Equals("Saving File") ?? false) &&
                resourceTree.SelectedNode is BaseWrapper b &&
                (node = (w = b).Resource) != null)
            {
                Action setScrollOffset = null;
                if (selectedType == resourceTree.SelectedNode.GetType())
                {
                    foreach (Control c in propertyGrid1.Controls)
                    {
                        if (c.GetType().Name == "PropertyGridView")
                        {
                            object scrollOffset = c.GetType().GetMethod("GetScrollOffset").Invoke(c, null);
                            setScrollOffset = () =>
                                c.GetType().GetMethod("SetScrollOffset").Invoke(c, new object[] {scrollOffset});
                            break;
                        }
                    }
                }
                else
                {
                    foreach (Control c in propertyGrid1.Controls)
                    {
                        if (c.GetType().Name == "PropertyGridView")
                        {
                            setScrollOffset = () =>
                                c.GetType().GetMethod("SetScrollOffset").Invoke(c, new object[] {0});
                            break;
                        }
                    }
                }

                propertyGrid1.SelectedObject = node;
                setScrollOffset?.Invoke();

                if (ShowHex && node is IBufferNode d)
                {
                    if (d.IsValid())
                    {
                        hexBox1.ByteProvider = new Be.Windows.Forms.DynamicFileByteProvider(new UnmanagedMemoryStream(
                                (byte*) d.GetAddress(),
                                d.GetLength(),
                                d.GetLength(),
                                FileAccess.ReadWrite))
                            {_supportsInsDel = false};
                        newControl = hexBox1;
                    }
                }
#if DEBUG
                else if (ShowHex && !(node is RELEntryNode || node is RELNode) && node.WorkingUncompressed.Length > 0)
                {
                    hexBox1.ByteProvider = new Be.Windows.Forms.DynamicFileByteProvider(new UnmanagedMemoryStream(
                            (byte*)node.WorkingUncompressed.Address,
                            node.WorkingUncompressed.Length,
                            node.WorkingUncompressed.Length,
                            FileAccess.ReadWrite))
                        { _supportsInsDel = false };
                    newControl = hexBox1;
                }
#endif
                else if (node is RSARGroupNode groupNode)
                {
                    rsarGroupEditor.LoadGroup(groupNode);
                    newControl = rsarGroupEditor;
                }
                else if (node is RELMethodNode methodNode)
                {
                    ppcDisassembler1.SetTarget(methodNode);
                    newControl = ppcDisassembler1;
                }
                else if (node is IVideo video)
                {
                    videoPlaybackPanel1.TargetSource = video;
                    newControl = videoPlaybackPanel1;
                }
                else if (node is MDL0MaterialRefNode)
                {
                    newControl = texCoordControl1;
                }
                else if (node is MDL0ObjectNode)
                {
                    newControl = mdL0ObjectControl1;
                }
                else if (node is MSBinNode binNode)
                {
                    msBinEditor1.CurrentNode = binNode;
                    newControl = msBinEditor1;
                }
                else if (node is CHR0EntryNode chr0EntryNode)
                {
                    animEditControl.TargetSequence = chr0EntryNode;
                    newControl = animEditControl;
                }
                else if (node is SRT0TextureNode textureNode)
                {
                    texAnimEditControl.TargetSequence = textureNode;
                    newControl = texAnimEditControl;
                }
                else if (node is SHP0VertexSetNode setNode)
                {
                    shpAnimEditControl.TargetSequence = setNode;
                    newControl = shpAnimEditControl;
                }
                else if (node is RSARNode rsarNode)
                {
                    soundPackControl1.TargetNode = rsarNode;
                    newControl = soundPackControl1;
                }
                else if (node is VIS0EntryNode entryNode)
                {
                    visEditor.TargetNode = entryNode;
                    newControl = visEditor;
                }
                else if (node is MoveDefActionNode actionNode)
                {
                    movesetEditor1.TargetNode = actionNode;
                    newControl = movesetEditor1;
                }
                else if (node is MoveDefEventOffsetNode offsetNode)
                {
                    offsetEditor1.TargetNode = offsetNode;
                    newControl = offsetEditor1;
                }
                else if (node is MoveDefEventNode eventNode)
                {
                    //if (node.Parent is MoveDefLookupEntry1Node)
                    //    eventDescription1.SetTarget((node as MoveDefLookupEntry1Node).EventInfo, -1);
                    //else
                    eventDescription1.SetTarget(eventNode.EventInfo, -1);
                    newControl = eventDescription1;
                }
                else if (node is MoveDefEventParameterNode)
                {
                    //if (node.Parent is MoveDefLookupEntry1Node)
                    //    eventDescription1.SetTarget((node.Parent as MoveDefLookupEntry1Node).EventInfo, node.Index == -1 ? -2 : node.Index);
                    //else
                    eventDescription1.SetTarget((node.Parent as MoveDefEventNode).EventInfo,
                        node.Index == -1 ? -2 : node.Index);
                    newControl = eventDescription1;
                }
                else if (node is MoveDefAttributeNode attributeNode)
                {
                    attributeControl.TargetNode = attributeNode;
                    newControl = attributeControl;
                }
                else if (node is MoveDefSectionParamNode paramNode)
                {
                    articleAttributeGrid.TargetNode = paramNode;
                    newControl = articleAttributeGrid;
                }
                else if (node is SCN0CameraNode cameraNode)
                {
                    scN0CameraEditControl1.TargetSequence = cameraNode;
                    newControl = scN0CameraEditControl1;
                }
                else if (node is SCN0LightNode lightNode)
                {
                    scN0LightEditControl1.TargetSequence = lightNode;
                    newControl = scN0LightEditControl1;
                    disable2nd = true;
                }
                else if (node is SCN0FogNode fogNode)
                {
                    scN0FogEditControl1.TargetSequence = fogNode;
                    newControl = scN0FogEditControl1;
                    disable2nd = true;
                }
                else if (node is IAudioSource audioSource)
                {
                    audioPlaybackPanel1.TargetSource = audioSource;
                    IAudioStream[] sources = audioPlaybackPanel1.TargetSource.CreateStreams();
                    if (sources != null && sources.Length > 0 && sources[0] != null)
                    {
                        newControl = audioPlaybackPanel1;
                    }
                }
                else if (node is CollisionNode || node is CollisionObject || !CompatibilityMode &&
                         (node is IRenderedObject io && io.DrawCalls.Count > 0 ||
                          ShowARCPreviews && node is ARCNode arcNode && arcNode.NumTriangles > 0 ||
                          ShowBRRESPreviews && node is BRRESNode brresNode && brresNode.NumTriangles > 0))
                {
                    newControl = modelPanel1;
                    RenderSelected(node);
                }
                else if (node is IImageSource i && i.ImageCount > 0)
                {
                    previewPanel2.RenderingTarget = i;
                    newControl = previewPanel2;
                }
                else if (node is StageTableNode stageTableNode)
                {
                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(stageTableNode.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = stageTableNode;
                    newControl = attributeGrid1;
                }

                if (node is IColorSource source && !disable2nd)
                {
                    clrControl.ColorSource = source;
                    if (source.ColorCount(0) > 0)
                    {
                        if (newControl != null)
                        {
                            newControl2 = clrControl;
                        }
                        else
                        {
                            newControl = clrControl;
                        }
                    }
                }
#if DEBUG
                if (newControl == null && !(node is RELEntryNode || node is RELNode))
#else
                if (newControl == null && ShowHex && !(node is RELEntryNode || node is RELNode))
#endif
                {
                    if (node.WorkingUncompressed.Length > 0)
                    {
                        hexBox1.ByteProvider = new Be.Windows.Forms.DynamicFileByteProvider(new UnmanagedMemoryStream(
                                (byte*) node.WorkingUncompressed.Address,
                                node.WorkingUncompressed.Length,
                                node.WorkingUncompressed.Length,
                                FileAccess.ReadWrite))
                            {_supportsInsDel = false};
                        newControl = hexBox1;
                    }
                }
                _enableEditMenu(this, null);
            }
            else
            {
                propertyGrid1.SelectedObject = null;
                try
                {
                    editToolStripMenuItem.DropDown = null;
                }
                catch
                {
                    // ignored
                }

                editToolStripMenuItem.Enabled = false;
            }

            if (_secondaryControl != newControl2)
            {
                if (_secondaryControl != null)
                {
                    _secondaryControl.Dock = DockStyle.Fill;
                    _secondaryControl.Visible = false;
                }

                _secondaryControl = newControl2;
                if (_secondaryControl != null)
                {
                    _secondaryControl.Dock = DockStyle.Right;
                    _secondaryControl.Visible = true;
                    _secondaryControl.Width = 340;
                }
            }

            if (_currentControl != newControl)
            {
                if (_currentControl != null)
                {
                    _currentControl.Visible = false;
                }

                _currentControl = newControl;
                if (_currentControl != null)
                {
                    _currentControl.Visible = true;
                }
            }
            else if (_currentControl != null && !_currentControl.Visible)
            {
                _currentControl.Visible = true;
            }

            if (_currentControl != null)
            {
                if (_secondaryControl != null)
                {
                    _currentControl.Width = splitContainer2.Panel2.Width - _secondaryControl.Width;
                }

                _currentControl.Dock = DockStyle.Fill;
            }

            if (_currentControl is MDL0ObjectControl)
            {
                mdL0ObjectControl1.SetTarget(node as MDL0ObjectNode);
            }
            else if (_currentControl is TexCoordControl)
            {
                texCoordControl1.TargetNode = (MDL0MaterialRefNode) node;
            }

            selectedType = resourceTree.SelectedNode == null ? null : resourceTree.SelectedNode.GetType();
        }

        #region Rendering

        public static void RenderSelected(ResourceNode node)
        {
            float? minX = null;
            float? minY = null;
            float? minZ = null;
            float? maxX = null;
            float? maxY = null;
            float? maxZ = null;
            if (node == null)
            {
                return;
            }

            Instance.modelPanel1.CurrentViewport.SetProjectionType(ViewportProjection.Perspective);
            switch (node)
            {
                case CollisionNode collNode:
                    Instance.modelPanel1.CurrentViewport.SetProjectionType(ViewportProjection.Orthographic);
                    Instance.modelPanel1.AddTarget(collNode, false);
                    collNode.CalculateCamBoundaries(out minX, out minY, out maxX, out maxY);
                    break;
                case CollisionObject collObj:
                    CollisionNode collNodeTemp = new CollisionNode();
                    collNodeTemp.Children.Add(collObj);
                    Instance.modelPanel1.CurrentViewport.SetProjectionType(ViewportProjection.Orthographic);
                    Instance.modelPanel1.AddTarget(collNodeTemp, false);
                    collNodeTemp.CalculateCamBoundaries(out minX, out minY, out maxX, out maxY);
                    break;
                case ARCNode arcNode:
                    RenderARC(arcNode, out minX, out minY, out minZ, out maxX, out maxY, out maxZ);
                    break;
                case BRRESNode brresNode:
                    RenderBRRES(brresNode, out minX, out minY, out minZ, out maxX, out maxY, out maxZ);
                    break;
                case IRenderedObject o:
                    //Model panel has to be loaded first to display model correctly
                    if (node._children == null)
                    {
                        node.Populate(0);
                    }

                    if (o is IModel m && ModelEditControl.Instances.Count == 0)
                    {
                        m.ResetToBindState();
                    }

                    Instance.modelPanel1.AddTarget(o, false);
                    Instance.modelPanel1.SetCamWithBox(o.GetBox());
                    return;
            }

            Instance.modelPanel1.SetCamWithBox(new Vector3(minX ?? 0, minY ?? 0, minZ ?? 0),
                new Vector3(maxX ?? 0, maxY ?? 0, maxZ ?? 0));
        }

        public static void RenderARC(ARCNode arcNode, out float? minX, out float? minY, out float? minZ,
                                     out float? maxX,
                                     out float? maxY, out float? maxZ)
        {
            minX = null;
            minY = null;
            minZ = null;
            maxX = null;
            maxY = null;
            maxZ = null;
            MDL0Node stagePosition = null;
            foreach (ResourceNode resource in arcNode.Children)
            {
                if (resource is BRRESNode brresNode)
                {
                    if (brresNode.NumTriangles > 0)
                    {
                        RenderBRRES(brresNode, out float? bMinX, out float? bMinY, out float? bMinZ, out float? bMaxX,
                            out float? bMaxY, out float? bMaxZ);
                        if (minX == null || bMinX < minX)
                        {
                            minX = bMinX;
                        }

                        if (minY == null || bMinY < minY)
                        {
                            minY = bMinY;
                        }

                        if (minZ == null || bMinZ < minZ)
                        {
                            minZ = bMinZ;
                        }

                        if (maxX == null || bMaxX > maxX)
                        {
                            maxX = bMaxX;
                        }

                        if (maxY == null || bMaxY > maxY)
                        {
                            maxY = bMaxY;
                        }

                        if (maxZ == null || bMaxZ > maxZ)
                        {
                            maxZ = bMaxZ;
                        }
                    }
                    else if (stagePosition == null && brresNode.NumModels == 1 && brresNode.NumTriangles == 0)
                    {
                        try
                        {
                            MDL0Node check = brresNode.GetFolder<MDL0Node>().Children[0] as MDL0Node;
                            if (check.Name.Equals("stgposition", StringComparison.OrdinalIgnoreCase) ||
                                check.Name.Equals("stageposition", StringComparison.OrdinalIgnoreCase))
                            {
                                stagePosition = check;
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                else if (resource is ARCNode subArcNode && subArcNode.NumTriangles > 0)
                {
                    RenderARC(subArcNode, out float? aMinX, out float? aMinY, out float? aMinZ, out float? aMaxX,
                        out float? aMaxY, out float? aMaxZ);
                    if (minX == null || aMinX < minX)
                    {
                        minX = aMinX;
                    }

                    if (minY == null || aMinY < minY)
                    {
                        minY = aMinY;
                    }

                    if (minZ == null || aMinZ < minZ)
                    {
                        minZ = aMinZ;
                    }

                    if (maxX == null || aMaxX > maxX)
                    {
                        maxX = aMaxX;
                    }

                    if (maxY == null || aMaxY > maxY)
                    {
                        maxY = aMaxY;
                    }

                    if (maxZ == null || aMaxZ > maxZ)
                    {
                        maxZ = aMaxZ;
                    }
                }
            }

            // Use stage position to determine boundaries programatically
            if (stagePosition != null)
            {
                MDL0BoneNode cam0N = stagePosition.FindBone("CamLimit0N");
                MDL0BoneNode cam1N = stagePosition.FindBone("CamLimit1N");
                if (cam0N != null && cam1N != null)
                {
                    minX = cam0N.Translation._x;
                    minY = cam0N.Translation._y;
                    minZ = 0;
                    maxX = cam1N.Translation._x;
                    maxY = cam1N.Translation._y;
                    maxZ = 0;
                }
            }
        }

        public static void RenderBRRES(BRRESNode brresNode, out float? minX, out float? minY, out float? minZ,
                                       out float? maxX,
                                       out float? maxY, out float? maxZ)
        {
            minX = null;
            minY = null;
            minZ = null;
            maxX = null;
            maxY = null;
            maxZ = null;
            BRESGroupNode modelGroup = brresNode.GetFolder<MDL0Node>();
            foreach (MDL0Node model in modelGroup.Children)
            {
                if (model._children == null)
                {
                    model.Populate(0);
                }

                model.ResetToBindState();

                Instance.modelPanel1.AddTarget(model, false);
                Box b = model.GetBox();

                if (minX == null || b.Min._x < minX)
                {
                    minX = b.Min._x;
                }

                if (minY == null || b.Min._y < minY)
                {
                    minY = b.Min._y;
                }

                if (minZ == null || b.Min._z < minZ)
                {
                    minZ = b.Min._z;
                }

                if (maxX == null || b.Max._x > maxX)
                {
                    maxX = b.Max._x;
                }

                if (maxY == null || b.Max._y > maxY)
                {
                    maxY = b.Max._y;
                }

                if (maxZ == null || b.Max._z > maxZ)
                {
                    maxZ = b.Max._z;
                }
            }
        }

        #endregion

        public static void UpdateDiscordRPC(object sender, EventArgs e)
        {
            try
            {
                if (Program.CanRunDiscordRPC)
                {
                    if (Discord.DiscordSettings.DiscordControllerSet)
                    {
                        Discord.DiscordSettings.Update();
                    }
                    else
                    {
                        Process[] px = Process.GetProcessesByName("BrawlCrate");
                        if (px.Length == 1)
                        {
                            Discord.DiscordRpc.ClearPresence();
                        }

                        Discord.DiscordSettings.LoadSettings(true);
                    }
                }
            }
            catch
            {
                // Discord RPC doesn't need to work always
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UpdateDiscordRPC(null, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Program.Close())
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.OpenFile(SupportedFilesHandler.CompleteFilterEditableOnly, out string inFile))
            {
                Program.Open(inFile);
            }
        }

        #region File Menu

        private void aRCArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.New<ARCNode>();
        }

        private void u8FileArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.New<U8Node>();
        }

        private void brresPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.New<BRRESNode>();
        }

        private void tPLTextureArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.New<TPLNode>();
        }

        private void eFLSEffectListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.New<EFLSNode>();
        }

        private void rEFFParticlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.New<REFFNode>();
        }

        private void rEFTParticleTexturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.New<REFTNode>();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Settings._updating = true;
            Settings.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm.Instance.ShowDialog(this);
        }

        private void bRStmAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.OpenFile("PCM Audio (*.wav)|*.wav", out string path))
            {
                if (Program.New<RSTMNode>())
                {
                    using (BrstmConverterDialog dlg = new BrstmConverterDialog())
                    {
                        dlg.AudioSource = path;
                        if (dlg.ShowDialog(this) == DialogResult.OK)
                        {
                            Program.RootNode.Name = Path.GetFileNameWithoutExtension(dlg.AudioSource);
                            Program.RootNode.ReplaceRaw(dlg.AudioData);
                        }
                        else
                        {
                            Program.Close(true);
                        }
                    }
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            Array a = (Array) e.Data.GetData(DataFormats.FileDrop);
            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    BeginInvoke(m_DelegateOpenFile, new object[] {s});
                }
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        private void GCTEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCTEditor g = new GCTEditor(Program.AssemblyTitleFull);
            g.FormClosed += UpdateDiscordRPC;
            g.OpenFileChanged += UpdateDiscordRPC;
            g.Show();
            UpdateDiscordRPC(null, null);
        }

        private void CostumeManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CostumeManager.CostumeManagerForm m = new CostumeManager.CostumeManagerForm();
            m.FormClosed += UpdateDiscordRPC;
            m.Show();
            UpdateDiscordRPC(null, null);
        }

        private void SongManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongManager.SongManagerForm m = new SongManager.SongManagerForm(null, true, true, false);
            m.FormClosed += UpdateDiscordRPC;
            m.Show();
            UpdateDiscordRPC(null, null);
        }

        private void StageManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StageManager.StageManagerForm m = new StageManager.StageManagerForm(null, true);
            m.FormClosed += UpdateDiscordRPC;
            m.Show();
            UpdateDiscordRPC(null, null);
        }

        private void recentFilesToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string fileName = ((RecentFileHandler.FileMenuItem) e.ClickedItem).FileName;
            Program.Open(fileName);
        }

        private void checkForUpdatesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CheckUpdates();
        }

        private void splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            ((SplitContainer) sender).IsSplitterFixed = true;
        }

        private void splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            ((SplitContainer) sender).IsSplitterFixed = false;
        }

        private void splitContainer_MouseMove(object sender, MouseEventArgs e)
        {
            SplitContainer splitter = (SplitContainer) sender;
            if (splitter.IsSplitterFixed)
            {
                if (e.Button.Equals(MouseButtons.Left))
                {
                    if (splitter.Orientation.Equals(Orientation.Vertical))
                    {
                        if (e.X > 0 && e.X < splitter.Width)
                        {
                            splitter.SplitterDistance = e.X;
                            splitter.Refresh();
                        }
                    }
                    else
                    {
                        if (e.Y > 0 && e.Y < splitter.Height)
                        {
                            splitter.SplitterDistance = e.Y;
                            splitter.Refresh();
                        }
                    }
                }
                else
                {
                    splitter.IsSplitterFixed = false;
                }
            }
        }

        private void onPluginClicked(object sender, EventArgs e)
        {
            PluginScript plg = BrawlAPI.Plugins.Find(x => x.Name == ((ToolStripItem) sender).Text);
            plg?.Execute();
        }

        private void runScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = FileFilters.APIScripts
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    BrawlAPI.RunScript(dlg.FileName);
                }
            }
        }

        private void reloadPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrawlAPI.Plugins.Clear();
            pluginToolStripMenuItem.DropDown.Items.Clear();
            AddPlugins(pluginToolStripMenuItem, Program.ApiPluginPath);
        }

        public static List<FileInfo> GetScripts(string path)
        {
            DirectoryInfo dir = Directory.CreateDirectory(path);
            List<FileInfo> scripts = new List<FileInfo>();

            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                ToolStripMenuItem folder = new ToolStripMenuItem();
                folder.Name = folder.Text = d.Name;
                scripts.AddRange(GetScripts(d.FullName));
            }

            foreach (FileInfo script in dir.GetFiles().Where(f =>
                f.Extension.Equals(".py", StringComparison.OrdinalIgnoreCase) ||
                f.Extension.Equals(".fsx", StringComparison.OrdinalIgnoreCase)))
            {
                scripts.Add(script);
            }

            return scripts;
        }

        private void AddPlugins(ToolStripMenuItem menu, string path)
        {
            DirectoryInfo dir = Directory.CreateDirectory(path);
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                ToolStripMenuItem folder = new ToolStripMenuItem();
                folder.Name = folder.Text = d.Name;
                AddPlugins(folder, d.FullName);
                if (folder.DropDownItems.Count == 0)
                {
                    continue;
                }

                menu.DropDownItems.Add(folder);
            }

            foreach (string str in new[] {"*.py", "*.fsx"}.SelectMany(p => Directory.EnumerateFiles(path, p)))
            {
                if (BrawlAPI.CreatePlugin(str, false))
                {
                    menu.DropDownItems.Add(Path.GetFileNameWithoutExtension(str), null, onPluginClicked);
                }
            }
        }

        private void ShowChangelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if CANARY
            if (Program.CanRunGithubApp(true, out string path))
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = path,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = "-canarylog",
                });
            }
#else
            Process.Start(new ProcessStartInfo
            {
                FileName = $"{Application.StartupPath}\\Changelog.txt",
                WindowStyle = ProcessWindowStyle.Hidden,
            });
#endif
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.OpenFolderFile(out string inFile))
            {
                Program.OpenFolder(inFile);
            }
        }

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
            if (disposing)
            {
                components?.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ModelPanelViewport modelPanelViewport1 = new System.Windows.Forms.ModelPanelViewport();
            BrawlLib.OpenGL.GLCamera glCamera1 = new BrawlLib.OpenGL.GLCamera();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.resourceTree = new BrawlCrate.ResourceTree();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.archivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aRCFileArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bRRESResourcePackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.u8FileArchiveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tPLTextureArchiveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bRSTMAudioStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.effectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eFLSEffectListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rEFFParticlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rEFTParticleTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.costumeManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stageManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCTEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.runScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showChangelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.hexBox1 = new Be.Windows.Forms.HexBox();
            this.mdL0ObjectControl1 = new System.Windows.Forms.MDL0ObjectControl();
            this.ppcDisassembler1 = new System.Windows.Forms.PPCDisassembler();
            this.texCoordControl1 = new System.Windows.Forms.TexCoordControl();
            this.attributeGrid1 = new System.Windows.Forms.MultipleInterpretationAttributeGrid();
            this.videoPlaybackPanel1 = new System.Windows.Forms.VideoPlaybackPanel();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.previewPanel2 = new System.Windows.Forms.PreviewPanel();
            this.scN0FogEditControl1 = new System.Windows.Forms.SCN0FogEditControl();
            this.scN0LightEditControl1 = new System.Windows.Forms.SCN0LightEditControl();
            this.scN0CameraEditControl1 = new System.Windows.Forms.SCN0CameraEditControl();
            this.animEditControl = new System.Windows.Forms.AnimEditControl();
            this.shpAnimEditControl = new System.Windows.Forms.ShpAnimEditControl();
            this.texAnimEditControl = new System.Windows.Forms.TexAnimEditControl();
            this.audioPlaybackPanel1 = new System.Windows.Forms.AudioPlaybackPanel();
            this.visEditor = new System.Windows.Forms.VisEditor();
            this.clrControl = new System.Windows.Forms.CLRControl();
            this.rsarGroupEditor = new System.Windows.Forms.RSARGroupEditor();
            this.soundPackControl1 = new System.Windows.Forms.SoundPackControl();
            this.msBinEditor1 = new System.Windows.Forms.MSBinEditor();


            this.eventDescription1 = new System.Windows.Forms.EventDescription();
            this.attributeControl = new System.Windows.Forms.AttributeGrid2();
            this.articleAttributeGrid = new System.Windows.Forms.ArticleAttributeGrid();
            this.offsetEditor1 = new System.Windows.Forms.OffsetEditor();
            this.movesetEditor1 = new System.Windows.Forms.ScriptEditor();

            ((System.ComponentModel.ISupportInitialize) (this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.resourceTree);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(676, 428);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.TabStop = false;
            this.splitContainer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseDown);
            this.splitContainer1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseMove);
            this.splitContainer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseUp);
            // 
            // resourceTree
            // 
            this.resourceTree.AllowDrop = true;
            this.resourceTree.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.resourceTree.HideSelection = false;
            this.resourceTree.ImageIndex = 0;
            this.resourceTree.Indent = 20;
            this.resourceTree.Location = new System.Drawing.Point(0, 26);
            this.resourceTree.Name = "resourceTree";
            this.resourceTree.SelectedImageIndex = 0;
            this.resourceTree.ShowIcons = true;
            this.resourceTree.Size = new System.Drawing.Size(236, 404);
            this.resourceTree.TabIndex = 0;
            this.resourceTree.SelectionChanged += new System.EventHandler(this.resourceTree_SelectionChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.fileToolStripMenuItem,
                this.editToolStripMenuItem,
                this.toolsToolStripMenuItem,
                this.pluginToolStripMenuItem,
                this.helpToolStripMenuItem
            });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(236, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.newToolStripMenuItem,
                this.openToolStripMenuItem,
                this.openFolderToolStripMenuItem,
                this.saveToolStripMenuItem,
                this.saveAsToolStripMenuItem,
                this.closeToolStripMenuItem,
                this.toolStripMenuItem1,
                this.recentFilesToolStripMenuItem,
                this.toolStripSeparator1,
                this.exitToolStripMenuItem
            });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.archivesToolStripMenuItem,
                this.bRSTMAudioStreamToolStripMenuItem,
                this.effectsToolStripMenuItem
            });
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // archivesToolStripMenuItem
            // 
            this.archivesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.aRCFileArchiveToolStripMenuItem,
                this.bRRESResourcePackToolStripMenuItem,
                this.u8FileArchiveToolStripMenuItem1,
                this.tPLTextureArchiveToolStripMenuItem1
            });
            this.archivesToolStripMenuItem.Name = "archivesToolStripMenuItem";
            this.archivesToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.archivesToolStripMenuItem.Text = "Archives";
            // 
            // aRCFileArchiveToolStripMenuItem
            // 
            this.aRCFileArchiveToolStripMenuItem.Name = "aRCFileArchiveToolStripMenuItem";
            this.aRCFileArchiveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.aRCFileArchiveToolStripMenuItem.Text = "ARC File Archive";
            this.aRCFileArchiveToolStripMenuItem.Click +=
                new System.EventHandler(this.aRCArchiveToolStripMenuItem_Click);
            // 
            // bRRESResourcePackToolStripMenuItem
            // 
            this.bRRESResourcePackToolStripMenuItem.Name = "bRRESResourcePackToolStripMenuItem";
            this.bRRESResourcePackToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.bRRESResourcePackToolStripMenuItem.Text = "BRRES Resource Pack";
            this.bRRESResourcePackToolStripMenuItem.Click +=
                new System.EventHandler(this.brresPackToolStripMenuItem_Click);
            // 
            // u8FileArchiveToolStripMenuItem1
            // 
            this.u8FileArchiveToolStripMenuItem1.Name = "u8FileArchiveToolStripMenuItem1";
            this.u8FileArchiveToolStripMenuItem1.Size = new System.Drawing.Size(186, 22);
            this.u8FileArchiveToolStripMenuItem1.Text = "U8 File Archive";
            this.u8FileArchiveToolStripMenuItem1.Click +=
                new System.EventHandler(this.u8FileArchiveToolStripMenuItem_Click);
            // 
            // tPLTextureArchiveToolStripMenuItem1
            // 
            this.tPLTextureArchiveToolStripMenuItem1.Name = "tPLTextureArchiveToolStripMenuItem1";
            this.tPLTextureArchiveToolStripMenuItem1.Size = new System.Drawing.Size(186, 22);
            this.tPLTextureArchiveToolStripMenuItem1.Text = "TPL Texture Archive";
            this.tPLTextureArchiveToolStripMenuItem1.Click +=
                new System.EventHandler(this.tPLTextureArchiveToolStripMenuItem_Click);
            // 
            // bRSTMAudioStreamToolStripMenuItem
            // 
            this.bRSTMAudioStreamToolStripMenuItem.Name = "bRSTMAudioStreamToolStripMenuItem";
            this.bRSTMAudioStreamToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.bRSTMAudioStreamToolStripMenuItem.Text = "BRSTM Audio Stream";
            this.bRSTMAudioStreamToolStripMenuItem.Click +=
                new System.EventHandler(this.bRStmAudioToolStripMenuItem_Click);
            // 
            // effectsToolStripMenuItem
            // 
            this.effectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.eFLSEffectListToolStripMenuItem,
                this.rEFFParticlesToolStripMenuItem,
                this.rEFTParticleTexturesToolStripMenuItem
            });
            this.effectsToolStripMenuItem.Name = "effectsToolStripMenuItem";
            this.effectsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.effectsToolStripMenuItem.Text = "Particle Effects";
            // 
            // eFLSEffectListToolStripMenuItem
            // 
            this.eFLSEffectListToolStripMenuItem.Name = "eFLSEffectListToolStripMenuItem";
            this.eFLSEffectListToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.eFLSEffectListToolStripMenuItem.Text = "EFLS Effect List";
            this.eFLSEffectListToolStripMenuItem.Click +=
                new System.EventHandler(this.eFLSEffectListToolStripMenuItem_Click);
            // 
            // rEFFParticlesToolStripMenuItem
            // 
            this.rEFFParticlesToolStripMenuItem.Name = "rEFFParticlesToolStripMenuItem";
            this.rEFFParticlesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.rEFFParticlesToolStripMenuItem.Text = "REFF Particles";
            this.rEFFParticlesToolStripMenuItem.Click +=
                new System.EventHandler(this.rEFFParticlesToolStripMenuItem_Click);
            // 
            // rEFTParticleTexturesToolStripMenuItem
            // 
            this.rEFTParticleTexturesToolStripMenuItem.Name = "rEFTParticleTexturesToolStripMenuItem";
            this.rEFTParticleTexturesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.rEFTParticleTexturesToolStripMenuItem.Text = "REFT Particle Textures";
            this.rEFTParticleTexturesToolStripMenuItem.Click +=
                new System.EventHandler(this.rEFTParticleTexturesToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys =
                ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openFolerToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.ShortcutKeys =
                ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift |
                                               System.Windows.Forms.Keys.O)));
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.openFolderToolStripMenuItem.Text = "Open &Folder...";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys =
                ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys =
                ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift |
                                               System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(162, 6);
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.recentFilesToolStripMenuItem.Text = "Recent Files";
            this.recentFilesToolStripMenuItem.DropDownItemClicked +=
                new System.Windows.Forms.ToolStripItemClickedEventHandler(this
                    .recentFilesToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.settingsToolStripMenuItem,
                this.managersToolStripMenuItem,
                this.toolStripSeparator2,
                this.runScriptToolStripMenuItem,
                this.reloadPluginsToolStripMenuItem
            });
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click_1);
            //
            // managersToolStripMenuItem
            //
            this.managersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.gCTEditorToolStripMenuItem,
                this.costumeManagerToolStripMenuItem,
                this.songManagerToolStripMenuItem,
                this.stageManagerToolStripMenuItem
            });
            this.managersToolStripMenuItem.Name = "managersToolStripMenuItem";
            this.managersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.managersToolStripMenuItem.Text = "Managers";
            // 
            // gCTEditorToolStripMenuItem
            // 
            this.gCTEditorToolStripMenuItem.Name = "gCTEditorToolStripMenuItem";
            this.gCTEditorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gCTEditorToolStripMenuItem.Text = "Code Manager";
            this.gCTEditorToolStripMenuItem.Click += new System.EventHandler(this.GCTEditorToolStripMenuItem_Click);
            // 
            // costumeManagerToolStripMenuItem
            // 
            this.costumeManagerToolStripMenuItem.Name = "costumeManagerToolStripMenuItem";
            this.costumeManagerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.costumeManagerToolStripMenuItem.Text = "Costume Manager";
            this.costumeManagerToolStripMenuItem.Click +=
                new System.EventHandler(this.CostumeManagerToolStripMenuItem_Click);
            // 
            // songManagerToolStripMenuItem
            // 
            this.songManagerToolStripMenuItem.Name = "songManagerToolStripMenuItem";
            this.songManagerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.songManagerToolStripMenuItem.Text = "Song Manager";
            this.songManagerToolStripMenuItem.Click += new System.EventHandler(this.SongManagerToolStripMenuItem_Click);
            // 
            // stageManagerToolStripMenuItem
            // 
            this.stageManagerToolStripMenuItem.Name = "stageManagerToolStripMenuItem";
            this.stageManagerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stageManagerToolStripMenuItem.Text = "Stage Manager";
            this.stageManagerToolStripMenuItem.Click +=
                new System.EventHandler(this.StageManagerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // runScriptToolStripMenuItem
            // 
            this.runScriptToolStripMenuItem.Name = "runScriptToolStripMenuItem";
            this.runScriptToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.runScriptToolStripMenuItem.Text = "Run Script..";
            this.runScriptToolStripMenuItem.Click += new System.EventHandler(this.runScriptToolStripMenuItem_Click);
            // 
            // reloadPluginsToolStripMenuItem
            // 
            this.reloadPluginsToolStripMenuItem.Name = "reloadPluginsToolStripMenuItem";
            this.reloadPluginsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.reloadPluginsToolStripMenuItem.Text = "Reload Plugins";
            this.reloadPluginsToolStripMenuItem.Click +=
                new System.EventHandler(this.reloadPluginsToolStripMenuItem_Click);
            // 
            // pluginToolStripMenuItem
            // 
            this.pluginToolStripMenuItem.Name = "pluginToolStripMenuItem";
            this.pluginToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.pluginToolStripMenuItem.Text = "&Plugins";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.aboutToolStripMenuItem,
                this.showChangelogToolStripMenuItem,
                this.checkForUpdatesToolStripMenuItem
            });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // showChangelogToolStripMenuItem
            // 
            this.showChangelogToolStripMenuItem.Name = "showChangelogToolStripMenuItem";
            this.showChangelogToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showChangelogToolStripMenuItem.Text = "Show Changelog";
            this.showChangelogToolStripMenuItem.Click +=
                new System.EventHandler(this.ShowChangelogToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            this.checkForUpdatesToolStripMenuItem.Click +=
                new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click_1);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.hexBox1);
            this.splitContainer2.Panel2.Controls.Add(this.mdL0ObjectControl1);
            this.splitContainer2.Panel2.Controls.Add(this.ppcDisassembler1);
            this.splitContainer2.Panel2.Controls.Add(this.texCoordControl1);
            this.splitContainer2.Panel2.Controls.Add(this.attributeGrid1);
            this.splitContainer2.Panel2.Controls.Add(this.videoPlaybackPanel1);
            this.splitContainer2.Panel2.Controls.Add(this.modelPanel1);
            this.splitContainer2.Panel2.Controls.Add(this.previewPanel2);
            this.splitContainer2.Panel2.Controls.Add(this.scN0FogEditControl1);
            this.splitContainer2.Panel2.Controls.Add(this.scN0LightEditControl1);
            this.splitContainer2.Panel2.Controls.Add(this.scN0CameraEditControl1);
            this.splitContainer2.Panel2.Controls.Add(this.animEditControl);
            this.splitContainer2.Panel2.Controls.Add(this.shpAnimEditControl);
            this.splitContainer2.Panel2.Controls.Add(this.texAnimEditControl);
            this.splitContainer2.Panel2.Controls.Add(this.audioPlaybackPanel1);
            this.splitContainer2.Panel2.Controls.Add(this.visEditor);
            this.splitContainer2.Panel2.Controls.Add(this.clrControl);
            this.splitContainer2.Panel2.Controls.Add(this.rsarGroupEditor);
            this.splitContainer2.Panel2.Controls.Add(this.eventDescription1);
            this.splitContainer2.Panel2.Controls.Add(this.attributeControl);
            this.splitContainer2.Panel2.Controls.Add(this.articleAttributeGrid);
            this.splitContainer2.Panel2.Controls.Add(this.movesetEditor1);
            this.splitContainer2.Panel2.Controls.Add(this.offsetEditor1);
            this.splitContainer2.Panel2.Controls.Add(this.soundPackControl1);
            this.splitContainer2.Panel2.Controls.Add(this.msBinEditor1);
            this.splitContainer2.Size = new System.Drawing.Size(436, 428);
            this.splitContainer2.SplitterDistance = 211;
            this.splitContainer2.TabIndex = 3;
            this.splitContainer2.TabStop = false;
            this.splitContainer2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseDown);
            this.splitContainer2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseMove);
            this.splitContainer2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer_MouseUp);


            // 
            // articleAttributeGrid
            // 
            this.articleAttributeGrid.Location = new System.Drawing.Point(0, 0);
            this.articleAttributeGrid.Name = "articleAttributeGrid";
            this.articleAttributeGrid.Size = new System.Drawing.Size(479, 305);
            this.articleAttributeGrid.TabIndex = 9;
            this.articleAttributeGrid.Visible = false;

            // 
            // attributeControl
            // 
            this.attributeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeControl.Location = new System.Drawing.Point(0, 0);
            this.attributeControl.Name = "attributeControl";
            this.attributeControl.Size = new System.Drawing.Size(399, 202);
            this.attributeControl.TabIndex = 0;
            this.attributeControl.Visible = false;

            // 
            // eventDescription1
            // 
            this.eventDescription1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventDescription1.Location = new System.Drawing.Point(0, 0);
            this.eventDescription1.Name = "eventDescription1";
            this.eventDescription1.Size = new System.Drawing.Size(399, 202);
            this.eventDescription1.TabIndex = 1;
            this.eventDescription1.Visible = false;

            // 
            // movesetEditor1
            // 
            this.movesetEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.movesetEditor1.Location = new System.Drawing.Point(0, 0);
            this.movesetEditor1.Name = "movesetEditor1";
            this.movesetEditor1.Padding = new System.Windows.Forms.Padding(1);
            this.movesetEditor1.Size = new System.Drawing.Size(399, 202);
            this.movesetEditor1.TabIndex = 0;
            this.movesetEditor1.Visible = false;

            // 
            // offsetEditor1
            // 
            this.offsetEditor1.Location = new System.Drawing.Point(0, 0);
            this.offsetEditor1.Name = "offsetEditor1";
            this.offsetEditor1.Size = new System.Drawing.Size(296, 105);
            this.offsetEditor1.TabIndex = 8;
            this.offsetEditor1.Visible = false;


            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(436, 211);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.SelectedGridItemChanged +=
                new System.Windows.Forms.SelectedGridItemChangedEventHandler(this
                    .propertyGrid1_SelectedGridItemChanged);
            // 
            // hexBox1
            // 
            this.hexBox1.BlrColor = System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (255)))),
                ((int) (((byte) (100)))));
            this.hexBox1.BranchOffsetColor = System.Drawing.Color.Plum;
            this.hexBox1.ColumnDividerColor = System.Drawing.Color.Empty;
            this.hexBox1.CommandColor = System.Drawing.Color.FromArgb(((int) (((byte) (200)))),
                ((int) (((byte) (255)))), ((int) (((byte) (200)))));
            this.hexBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.hexBox1.InfoForeColor = System.Drawing.Color.Empty;
            this.hexBox1.LinkedBranchColor = System.Drawing.Color.Orange;
            this.hexBox1.Location = new System.Drawing.Point(85, 27);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.ReadOnly = true;
            this.hexBox1.SectionEditor = null;
            this.hexBox1.SelectedColor = System.Drawing.Color.FromArgb(((int) (((byte) (200)))),
                ((int) (((byte) (255)))), ((int) (((byte) (255)))));
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int) (((byte) (100)))),
                ((int) (((byte) (60)))), ((int) (((byte) (188)))),
                ((int) (((byte) (255)))));
            this.hexBox1.Size = new System.Drawing.Size(215, 130);
            this.hexBox1.StringViewVisible = true;
            this.hexBox1.TabIndex = 22;
            this.hexBox1.UseFixedBytesPerLine = true;
            this.hexBox1.Visible = false;
            this.hexBox1.VScrollBarVisible = true;
            // 
            // mdL0ObjectControl1
            // 
            this.mdL0ObjectControl1.Location = new System.Drawing.Point(139, 56);
            this.mdL0ObjectControl1.Margin = new System.Windows.Forms.Padding(4);
            this.mdL0ObjectControl1.Name = "mdL0ObjectControl1";
            this.mdL0ObjectControl1.Size = new System.Drawing.Size(652, 397);
            this.mdL0ObjectControl1.TabIndex = 21;
            // 
            // ppcDisassembler1
            // 
            this.ppcDisassembler1.Location = new System.Drawing.Point(0, 0);
            this.ppcDisassembler1.Margin = new System.Windows.Forms.Padding(4);
            this.ppcDisassembler1.Name = "ppcDisassembler1";
            this.ppcDisassembler1.Size = new System.Drawing.Size(398, 199);
            this.ppcDisassembler1.TabIndex = 20;
            this.ppcDisassembler1.Visible = false;
            // 
            // texCoordControl1
            // 
            this.texCoordControl1.Location = new System.Drawing.Point(0, 0);
            this.texCoordControl1.Name = "texCoordControl1";
            this.texCoordControl1.Size = new System.Drawing.Size(396, 199);
            this.texCoordControl1.TabIndex = 19;
            this.texCoordControl1.TargetNode = null;
            this.texCoordControl1.Visible = false;
            // 
            // attributeGrid1
            // 
            this.attributeGrid1.AttributeArray = null;
            this.attributeGrid1.Location = new System.Drawing.Point(46, 56);
            this.attributeGrid1.Name = "attributeGrid1";
            this.attributeGrid1.Size = new System.Drawing.Size(479, 305);
            this.attributeGrid1.TabIndex = 18;
            this.attributeGrid1.Visible = false;
            // 
            // videoPlaybackPanel1
            // 
            this.videoPlaybackPanel1.Location = new System.Drawing.Point(85, -16);
            this.videoPlaybackPanel1.Name = "videoPlaybackPanel1";
            this.videoPlaybackPanel1.Size = new System.Drawing.Size(536, 111);
            this.videoPlaybackPanel1.TabIndex = 17;
            this.videoPlaybackPanel1.Visible = false;
            // 
            // modelPanel1
            // 
            modelPanelViewport1.BackgroundColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))),
                ((int) (((byte) (240)))), ((int) (((byte) (240)))),
                ((int) (((byte) (240)))));
            modelPanelViewport1.BackgroundImage = null;
            modelPanelViewport1.BackgroundImageType = BrawlLib.OpenGL.BGImageType.Stretch;
            glCamera1.Aspect = 2.254438F;
            glCamera1.FarDepth = 200000F;
            glCamera1.Height = 169F;
            glCamera1.NearDepth = 1F;
            glCamera1.Orthographic = false;
            glCamera1.VerticalFieldOfView = 45F;
            glCamera1.Width = 381F;
            modelPanelViewport1.Camera = glCamera1;
            modelPanelViewport1.Enabled = true;
            modelPanelViewport1.Region = new System.Drawing.Rectangle(0, 0, 381, 169);
            modelPanelViewport1.RotationScale = 0.4F;
            modelPanelViewport1.TranslationScale = 0.05F;
            modelPanelViewport1.ViewType = BrawlLib.OpenGL.ViewportProjection.Perspective;
            modelPanelViewport1.ZoomScale = 2.5F;
            this.modelPanel1.CurrentViewport = modelPanelViewport1;
            this.modelPanel1.Location = new System.Drawing.Point(0, 0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.Size = new System.Drawing.Size(381, 169);
            this.modelPanel1.TabIndex = 15;
            this.modelPanel1.TabStop = false;
            this.modelPanel1.Visible = false;
            // 
            // previewPanel2
            // 
            this.previewPanel2.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.previewPanel2.CurrentIndex = 0;
            this.previewPanel2.DisposeImage = true;
            this.previewPanel2.Location = new System.Drawing.Point(0, 0);
            this.previewPanel2.Name = "previewPanel2";
            this.previewPanel2.RenderingTarget = null;
            this.previewPanel2.Size = new System.Drawing.Size(401, 136);
            this.previewPanel2.TabIndex = 16;
            this.previewPanel2.Visible = false;
            // 
            // scN0FogEditControl1
            // 
            this.scN0FogEditControl1.Location = new System.Drawing.Point(-111, -119);
            this.scN0FogEditControl1.Name = "scN0FogEditControl1";
            this.scN0FogEditControl1.Size = new System.Drawing.Size(293, 276);
            this.scN0FogEditControl1.TabIndex = 13;
            this.scN0FogEditControl1.Visible = false;
            // 
            // scN0LightEditControl1
            // 
            this.scN0LightEditControl1.Location = new System.Drawing.Point(139, -190);
            this.scN0LightEditControl1.Name = "scN0LightEditControl1";
            this.scN0LightEditControl1.Size = new System.Drawing.Size(293, 276);
            this.scN0LightEditControl1.TabIndex = 12;
            this.scN0LightEditControl1.Visible = false;
            // 
            // scN0CameraEditControl1
            // 
            this.scN0CameraEditControl1.Location = new System.Drawing.Point(104, -191);
            this.scN0CameraEditControl1.Name = "scN0CameraEditControl1";
            this.scN0CameraEditControl1.Size = new System.Drawing.Size(286, 276);
            this.scN0CameraEditControl1.TabIndex = 11;
            this.scN0CameraEditControl1.Visible = false;
            // 
            // animEditControl
            // 
            this.animEditControl.Location = new System.Drawing.Point(0, 0);
            this.animEditControl.Name = "animEditControl";
            this.animEditControl.Size = new System.Drawing.Size(384, 169);
            this.animEditControl.TabIndex = 1;
            this.animEditControl.Visible = false;
            // 
            // shpAnimEditControl
            // 
            this.shpAnimEditControl.Location = new System.Drawing.Point(0, 0);
            this.shpAnimEditControl.Name = "shpAnimEditControl";
            this.shpAnimEditControl.Size = new System.Drawing.Size(384, 169);
            this.shpAnimEditControl.TabIndex = 7;
            this.shpAnimEditControl.Visible = false;
            // 
            // texAnimEditControl
            // 
            this.texAnimEditControl.Location = new System.Drawing.Point(0, 0);
            this.texAnimEditControl.Name = "texAnimEditControl";
            this.texAnimEditControl.Size = new System.Drawing.Size(300, 212);
            this.texAnimEditControl.TabIndex = 7;
            this.texAnimEditControl.Visible = false;
            // 
            // audioPlaybackPanel1
            // 
            this.audioPlaybackPanel1.Location = new System.Drawing.Point(149, 92);
            this.audioPlaybackPanel1.Name = "audioPlaybackPanel1";
            this.audioPlaybackPanel1.Size = new System.Drawing.Size(70, 111);
            this.audioPlaybackPanel1.TabIndex = 4;
            this.audioPlaybackPanel1.TargetStreams = null;
            this.audioPlaybackPanel1.Visible = false;
            this.audioPlaybackPanel1.Volume = null;
            // 
            // visEditor
            // 
            this.visEditor.Location = new System.Drawing.Point(0, 0);
            this.visEditor.Name = "visEditor";
            this.visEditor.Size = new System.Drawing.Size(78, 87);
            this.visEditor.TabIndex = 6;
            this.visEditor.Visible = false;
            // 
            // clrControl
            // 
            this.clrControl.ColorID = 0;
            this.clrControl.Location = new System.Drawing.Point(0, 0);
            this.clrControl.Name = "clrControl";
            this.clrControl.Size = new System.Drawing.Size(98, 47);
            this.clrControl.TabIndex = 5;
            this.clrControl.Visible = false;
            // 
            // rsarGroupEditor
            // 
            this.rsarGroupEditor.Location = new System.Drawing.Point(0, 0);
            this.rsarGroupEditor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rsarGroupEditor.Name = "rsarGroupEditor";
            this.rsarGroupEditor.Size = new System.Drawing.Size(98, 47);
            this.rsarGroupEditor.TabIndex = 5;
            this.rsarGroupEditor.Visible = false;
            // 
            // soundPackControl1
            // 
            this.soundPackControl1.Location = new System.Drawing.Point(13, 101);
            this.soundPackControl1.Name = "soundPackControl1";
            this.soundPackControl1.Size = new System.Drawing.Size(130, 65);
            this.soundPackControl1.TabIndex = 3;
            this.soundPackControl1.TargetNode = null;
            this.soundPackControl1.Visible = false;
            // 
            // msBinEditor1
            // 
            this.msBinEditor1.Location = new System.Drawing.Point(104, 4);
            this.msBinEditor1.Name = "msBinEditor1";
            this.msBinEditor1.Size = new System.Drawing.Size(146, 82);
            this.msBinEditor1.TabIndex = 2;
            this.msBinEditor1.Visible = false;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 428);
            this.Controls.Add(this.splitContainer1);
            this.Icon = BrawlLib.Properties.Resources.Icon;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public ResourceTree resourceTree;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        public MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem openFolderToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        public PropertyGrid propertyGrid1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private AnimEditControl animEditControl;
        private MSBinEditor msBinEditor1;
        private MultipleInterpretationAttributeGrid attributeGrid1;
        private SoundPackControl soundPackControl1;
        private AudioPlaybackPanel audioPlaybackPanel1;
        private CLRControl clrControl;
        private RSARGroupEditor rsarGroupEditor;
        private VisEditor visEditor;
        private ScriptEditor movesetEditor1;
        private EventDescription eventDescription1;
        private TexAnimEditControl texAnimEditControl;
        private ShpAnimEditControl shpAnimEditControl;
        private AttributeGrid2 attributeControl;
        private OffsetEditor offsetEditor1;
        private ArticleAttributeGrid articleAttributeGrid;
        private SCN0LightEditControl scN0LightEditControl1;
        private SCN0CameraEditControl scN0CameraEditControl1;
        private SCN0FogEditControl scN0FogEditControl1;
        public ModelPanel modelPanel1;
        private PreviewPanel previewPanel2;
        public ToolStripMenuItem editToolStripMenuItem;
        private VideoPlaybackPanel videoPlaybackPanel1;
        private ToolStripMenuItem gCTEditorToolStripMenuItem;
        private ToolStripMenuItem managersToolStripMenuItem;
        private ToolStripMenuItem costumeManagerToolStripMenuItem;
        private ToolStripMenuItem songManagerToolStripMenuItem;
        private ToolStripMenuItem stageManagerToolStripMenuItem;
        private ToolStripMenuItem recentFilesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        public ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private ToolStripMenuItem archivesToolStripMenuItem;
        private ToolStripMenuItem aRCFileArchiveToolStripMenuItem;
        private ToolStripMenuItem bRRESResourcePackToolStripMenuItem;
        private ToolStripMenuItem u8FileArchiveToolStripMenuItem1;
        private ToolStripMenuItem tPLTextureArchiveToolStripMenuItem1;
        private ToolStripMenuItem bRSTMAudioStreamToolStripMenuItem;
        private ToolStripMenuItem effectsToolStripMenuItem;
        private ToolStripMenuItem eFLSEffectListToolStripMenuItem;
        private ToolStripMenuItem rEFFParticlesToolStripMenuItem;
        private ToolStripMenuItem rEFTParticleTexturesToolStripMenuItem;
        private TexCoordControl texCoordControl1;
        private PPCDisassembler ppcDisassembler1;
        private MDL0ObjectControl mdL0ObjectControl1;
        private Be.Windows.Forms.HexBox hexBox1;
        private ToolStripMenuItem pluginToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem runScriptToolStripMenuItem;
        private ToolStripMenuItem reloadPluginsToolStripMenuItem;
        private ToolStripMenuItem showChangelogToolStripMenuItem;
    }

    public class RecentFileHandler : Component
    {
        private IContainer components;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
        }

        public class FileMenuItem : ToolStripMenuItem
        {
            private string fileName;

            public string FileName
            {
                get => fileName;
                set => fileName = value;
            }

            public FileMenuItem(string fileName)
            {
                this.fileName = fileName;
            }

            public override string Text
            {
                get
                {
                    ToolStripMenuItem parent = (ToolStripMenuItem) OwnerItem;
                    int index = parent.DropDownItems.IndexOf(this);
                    return $"{index + 1} {fileName}";
                }
                set { }
            }
        }

        public RecentFileHandler()
        {
            InitializeComponent();

            Init();
        }

        public RecentFileHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Init();
        }

        private void Init()
        {
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
        }

        public void AddFile(string fileName)
        {
            try
            {
                if (recentFileToolStripItem == null)
                {
                    throw new OperationCanceledException("recentFileToolStripItem can not be null!");
                }

                // check if the file is already in the collection
                int alreadyIn = GetIndexOfRecentFile(fileName);
                if (alreadyIn != -1) // remove it
                {
                    Settings.Default.RecentFiles.RemoveAt(alreadyIn);
                    if (recentFileToolStripItem.DropDownItems.Count > alreadyIn)
                    {
                        recentFileToolStripItem.DropDownItems.RemoveAt(alreadyIn);
                    }
                }
                else if (alreadyIn == 0) // it´s the latest file so return
                {
                    return;
                }

                // insert the file on top of the list
                Settings.Default.RecentFiles.Insert(0, fileName);
                recentFileToolStripItem.DropDownItems.Insert(0, new FileMenuItem(fileName));

                // remove any beyond the max size, if max size is reached
                while (Settings.Default.RecentFiles.Count > Settings.Default.RecentFilesMax)
                {
                    Settings.Default.RecentFiles.RemoveAt(Settings.Default.RecentFilesMax);
                }

                while (recentFileToolStripItem.DropDownItems.Count > Settings.Default.RecentFilesMax)
                {
                    recentFileToolStripItem.DropDownItems.RemoveAt(Settings.Default.RecentFilesMax);
                }

                // enable the menu item if it´s disabled
                if (!recentFileToolStripItem.Enabled)
                {
                    recentFileToolStripItem.Enabled = true;
                }

                // save the changes
                Settings.Default.Save();
            }
            catch
            {
                // ignored
            }
        }

        private int GetIndexOfRecentFile(string filename)
        {
            for (int i = 0; i < Settings.Default.RecentFiles.Count; i++)
            {
                string currentFile = Settings.Default.RecentFiles[i];
                if (string.Equals(currentFile, filename, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private ToolStripMenuItem recentFileToolStripItem;

        public ToolStripMenuItem RecentFileToolStripItem
        {
            get => recentFileToolStripItem;
            set
            {
                if (recentFileToolStripItem == value)
                {
                    return;
                }

                recentFileToolStripItem = value;

                ReCreateItems();
            }
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RecentFilesMax")
            {
                ReCreateItems();
            }
        }

        private void ReCreateItems()
        {
            if (recentFileToolStripItem == null)
            {
                return;
            }

            if (Settings.Default.RecentFiles == null)
            {
                Settings.Default.RecentFiles = new StringCollection();
            }

            recentFileToolStripItem.DropDownItems.Clear();
            recentFileToolStripItem.Enabled = Settings.Default.RecentFiles.Count > 0;

            int fileItemCount = Math.Min(Settings.Default.RecentFilesMax, Settings.Default.RecentFiles.Count);
            for (int i = 0; i < fileItemCount; i++)
            {
                string file = Settings.Default.RecentFiles[i];
                recentFileToolStripItem.DropDownItems.Add(new FileMenuItem(file));
            }
        }

        public void Clear()
        {
            Settings.Default.RecentFiles.Clear();
            ReCreateItems();
        }
    }
}
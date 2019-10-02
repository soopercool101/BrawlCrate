using BrawlCrate.API;
using BrawlCrate.NodeWrappers;
using BrawlCrate.Properties;
using BrawlLib.Imaging;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Audio;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate
{
    public partial class MainForm : Form
    {
        private static MainForm _instance;
        public static MainForm Instance => _instance ?? (_instance = new MainForm());

        private BaseWrapper _root;
        public BaseWrapper RootNode => _root;

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
            _autoPlayAudio = Properties.Settings.Default.AutoPlayAudio;
            _showFullPath = Properties.Settings.Default.ShowFullPath;
            _showBRRESPreviews = Properties.Settings.Default.PreviewBRRESModels;
            _showARCPreviews = Properties.Settings.Default.PreviewARCModels;

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

            m_DelegateOpenFile = Program.Open;
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
                string plugins = $"{Application.StartupPath}/Plugins";
                string loaders = $"{Application.StartupPath}/Loaders";

                pluginToolStripMenuItem.DropDown.Items.Clear();
                if (Directory.Exists(plugins))
                {
                    reloadPluginsToolStripMenuItem_Click(null, null);
                }

                if (Directory.Exists(loaders) && Properties.Settings.Default.APILoadersEnabled)
                {
                    foreach (string str in Directory.EnumerateFiles(loaders, "*.py"))
                    {
                        BrawlAPI.CreatePlugin(str, true);
                    }
                }
            }
            else
            {
                // TO-DO: Delete plugin-centric toolbar items
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
                    Process.Start(new ProcessStartInfo()
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

                Properties.Settings.Default.AutoPlayAudio = _autoPlayAudio;
                Properties.Settings.Default.Save();
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
                propertyGrid1.HelpVisible = item != null && item.PropertyDescriptor != null &&
                                            !string.IsNullOrEmpty(item.PropertyDescriptor.Description);
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
            _root = null;
            resourceTree.SelectedNode = null;
            resourceTree.Clear();

            if (Program.RootNode != null)
            {
                _root = BaseWrapper.Wrap(this, Program.RootNode);
                resourceTree.BeginUpdate();
                resourceTree.Nodes.Add(_root);
                resourceTree.SelectedNode = _root;
                _root.Expand();
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
                Text = string.Format("{0} - {1}", Program.AssemblyTitleShort,
                    ShowFullPath
                        ? Program.RootPath
                        : Program.RootPath.Substring(Program.RootPath.LastIndexOf('\\') + 1));
            }
            else
            {
                Text = Program.AssemblyTitleFull;
            }
        }

        public void TargetResource(ResourceNode n)
        {
            if (_root != null)
            {
                resourceTree.SelectedNode = _root.FindResource(n, true);
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
            if (hexBox1.ByteProvider != null)
            {
                ((Be.Windows.Forms.DynamicFileByteProvider) hexBox1.ByteProvider).Dispose();
            }

            Control newControl = null;
            Control newControl2 = null;

            BaseWrapper w;
            ResourceNode node = null;
            bool disable2nd = false;
            if (!(sender != null && sender.ToString().Equals("Saving File")) &&
                resourceTree.SelectedNode is BaseWrapper &&
                (node = (w = resourceTree.SelectedNode as BaseWrapper).Resource) != null)
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

                if (node is IBufferNode && ShowHex)
                {
                    IBufferNode d = node as IBufferNode;
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
                else if (node is RSARGroupNode)
                {
                    rsarGroupEditor.LoadGroup(node as RSARGroupNode);
                    newControl = rsarGroupEditor;
                }
                else if (node is RELMethodNode)
                {
                    ppcDisassembler1.SetTarget((RELMethodNode) node);
                    newControl = ppcDisassembler1;
                }
                else if (node is IVideo)
                {
                    videoPlaybackPanel1.TargetSource = node as IVideo;
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
                else if (node is MSBinNode)
                {
                    msBinEditor1.CurrentNode = node as MSBinNode;
                    newControl = msBinEditor1;
                }
                else if (node is CHR0EntryNode)
                {
                    animEditControl.TargetSequence = node as CHR0EntryNode;
                    newControl = animEditControl;
                }
                else if (node is SRT0TextureNode)
                {
                    texAnimEditControl.TargetSequence = node as SRT0TextureNode;
                    newControl = texAnimEditControl;
                }
                else if (node is SHP0VertexSetNode)
                {
                    shpAnimEditControl.TargetSequence = node as SHP0VertexSetNode;
                    newControl = shpAnimEditControl;
                }
                else if (node is RSARNode)
                {
                    soundPackControl1.TargetNode = node as RSARNode;
                    newControl = soundPackControl1;
                }
                else if (node is VIS0EntryNode)
                {
                    visEditor.TargetNode = node as VIS0EntryNode;
                    newControl = visEditor;
                }
                else if (node is MoveDefActionNode)
                {
                    movesetEditor1.TargetNode = node as MoveDefActionNode;
                    newControl = movesetEditor1;
                }
                else if (node is MoveDefEventOffsetNode)
                {
                    offsetEditor1.TargetNode = node as MoveDefEventOffsetNode;
                    newControl = offsetEditor1;
                }
                else if (node is MoveDefEventNode)
                {
                    //if (node.Parent is MoveDefLookupEntry1Node)
                    //    eventDescription1.SetTarget((node as MoveDefLookupEntry1Node).EventInfo, -1);
                    //else
                    eventDescription1.SetTarget((node as MoveDefEventNode).EventInfo, -1);
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
                else if (node is MoveDefAttributeNode)
                {
                    attributeControl.TargetNode = node as MoveDefAttributeNode;
                    newControl = attributeControl;
                }
                else if (node is MoveDefSectionParamNode)
                {
                    articleAttributeGrid.TargetNode = node as MoveDefSectionParamNode;
                    newControl = articleAttributeGrid;
                }
                else if (node is SCN0CameraNode)
                {
                    scN0CameraEditControl1.TargetSequence = node as SCN0CameraNode;
                    newControl = scN0CameraEditControl1;
                }
                else if (node is SCN0LightNode)
                {
                    scN0LightEditControl1.TargetSequence = node as SCN0LightNode;
                    newControl = scN0LightEditControl1;
                    disable2nd = true;
                }
                else if (node is SCN0FogNode)
                {
                    scN0FogEditControl1.TargetSequence = node as SCN0FogNode;
                    newControl = scN0FogEditControl1;
                    disable2nd = true;
                }
                else if (node is IAudioSource)
                {
                    audioPlaybackPanel1.TargetSource = node as IAudioSource;
                    IAudioStream[] sources = audioPlaybackPanel1.TargetSource.CreateStreams();
                    if (sources != null && sources.Length > 0 && sources[0] != null)
                    {
                        newControl = audioPlaybackPanel1;
                    }
                }
                else if (node is CollisionNode || node is CollisionObject || !CompatibilityMode &&
                         (node is IRenderedObject ||
                          ShowARCPreviews && node is ARCNode arcNode && arcNode.NumTriangles > 0 ||
                          ShowBRRESPreviews && node is BRRESNode brresNode && brresNode.NumTriangles > 0))
                {
                    newControl = modelPanel1;
                    RenderSelected(node);
                }
                else if (node is IImageSource)
                {
                    previewPanel2.RenderingTarget = (IImageSource) node;
                    newControl = previewPanel2;
                }
                else if (node is StageTableNode stageTableNode)
                {
                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(stageTableNode.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = stageTableNode;
                    newControl = attributeGrid1;
                }

                if (node is IColorSource && !disable2nd)
                {
                    clrControl.ColorSource = node as IColorSource;
                    if (((IColorSource) node).ColorCount(0) > 0)
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

                if (newControl == null && Instance.ShowHex && !(node is RELEntryNode || node is RELNode))
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

                if ((editToolStripMenuItem.DropDown =
                    Instance.resourceTree.SelectedNodes.Count > 1
                    ? Instance.resourceTree.GetMultiSelectMenuStrip()
                    : w?.ContextMenuStrip) != null)
                {
                    editToolStripMenuItem.Enabled = true;
                }
                else
                {
                    editToolStripMenuItem.Enabled = false;
                }
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
                    else if (stagePosition == null && brresNode.NumNodes >= 30)
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
            if (Program.OpenFile(SupportedFilesHandler.CompleteFilterEditableOnly, out string inFile) != 0)
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

        private void fileResizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (FileResizer res = new FileResizer())
            //    res.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Settings.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm.Instance.ShowDialog(this);
        }

        private void bRStmAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.OpenFile("PCM Audio (*.wav)|*.wav", out string path) > 0)
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
            GCTEditor g = new GCTEditor();
            g.FormClosed += UpdateDiscordRPC;
            g.OpenFileChanged += UpdateDiscordRPC;
            g.Show();
            UpdateDiscordRPC(null, null);
        }

        private void CostumeManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CostumeManager.MainForm m = new CostumeManager.MainForm();
            m.FormClosed += UpdateDiscordRPC;
            m.Show();
            UpdateDiscordRPC(null, null);
        }

        private void SongManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongManager.MainForm m = new SongManager.MainForm(null, true, true, false);
            m.FormClosed += UpdateDiscordRPC;
            m.Show();
            UpdateDiscordRPC(null, null);
        }

        private void StageManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StageManager.MainForm m = new StageManager.MainForm(null, true);
            m.FormClosed += UpdateDiscordRPC;
            m.Show();
            UpdateDiscordRPC(null, null);
        }

        private void recentFilesToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Program.Open(((RecentFileHandler.FileMenuItem) e.ClickedItem).FileName);
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
            using (OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter =
                    "All supported files (.py, .fsx)|*.py;*.fsx|Python file (.py)|*.py|F# script (.fsx)|*.fsx|All Files|*"
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
            AddPlugins(pluginToolStripMenuItem, $"{Application.StartupPath}/Plugins");
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
            Process.Start(new ProcessStartInfo()
            {
                FileName = $"{Application.StartupPath}\\Changelog.txt",
                WindowStyle = ProcessWindowStyle.Hidden,
            });
#endif
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.OpenFolderFile(out string inFile) != 0)
            {
                Program.OpenFolder(inFile);
            }
        }
    }

    public class RecentFileHandler : Component
    {
        private IContainer components;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
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
                    return string.Format("{0} {1}", index + 1, fileName);
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
            Settings.Default.PropertyChanged += Default_PropertyChanged;
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
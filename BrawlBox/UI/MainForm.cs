using BrawlBox.Properties;
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
using System.Windows.Forms;
using System.Reflection;
using BrawlBox.API;
using BrawlBox.NodeWrappers;
using System.Linq;

namespace BrawlBox
{
    public partial class MainForm : Form
    {
        private static MainForm _instance;
        public static MainForm Instance { get { return _instance == null ? _instance = new MainForm() : _instance; } }

        private BaseWrapper _root;
        public BaseWrapper RootNode { get { return _root; } }

        private SettingsDialog _settings;
        private SettingsDialog Settings { get { return _settings == null ? _settings = new SettingsDialog() : _settings; } }

        RecentFileHandler RecentFileHandler;

        private InterpolationForm _interpolationForm = null;
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

        void _interpolationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _interpolationForm = null;
        }

        public MainForm()
        {
            InitializeComponent();
            Text = Program.AssemblyTitle;

            _displayPropertyDescription = BrawlBox.Properties.Settings.Default.DisplayPropertyDescriptionWhenAvailable;
            _updatesOnStartup = BrawlBox.Properties.Settings.Default.CheckUpdatesAtStartup;

#if !DEBUG //Don't need to see this every time a debug build is compiled
            if (CheckUpdatesOnStartup)
                CheckUpdates(false);
#else
            Text += " DEBUG";
#endif

            foreach (string filename in new string[] {
                "BrawlLib.dll",
                "OpenTK.dll",
                "Octokit.dll",
                "Updater.exe"
            }) {
                string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (File.Exists(Path.Combine(directory, filename))) {
                    if (File.Exists(Path.Combine(directory, "lib", filename))) {
                        MessageBox.Show(@"The directory structure of BrawlBox has changed.
Please delete the files BrawlLib.dll, OpenTK.dll, Octokit.dll, and Updater.exe;
newer versions of these files should be located in the Lib directory.");
                        Enabled = false;
                        break;
                    }
                }
            }

            soundPackControl1._grid = propertyGrid1;
            soundPackControl1.lstSets.SmallImageList = ResourceTree.Images;
            foreach (Control c in splitContainer2.Panel2.Controls)
            {
                c.Visible = false;
                c.Dock = DockStyle.Fill;
            }
            m_DelegateOpenFile = new DelegateOpenFile(Program.Open);
            _instance = this;

            _currentControl = modelPanel1;

            modelPanel1.CurrentViewport._allowSelection = false;

            RecentFileHandler = new RecentFileHandler(this.components);
            RecentFileHandler.RecentFileToolStripItem = this.recentFilesToolStripMenuItem;

            var plugins = $"{Application.StartupPath}/Plugins";
            var loaders = $"{Application.StartupPath}/Loaders";

            API.bboxapi.Plugins.Clear();
            API.bboxapi.Loaders.Clear();
            pluginToolStripMenuItem.DropDown.Items.Clear();
            if (Directory.Exists(plugins))
            {
                foreach (var str in Directory.EnumerateFiles(plugins, "*.fsx"))
                {
                    pluginToolStripMenuItem.DropDownItems.Add(Path.GetFileNameWithoutExtension(str), null, onPluginClicked);
                }
            }
            if (Directory.Exists(loaders))
            {
                foreach (var str in Directory.EnumerateFiles(loaders, "*.py"))
                {
                    API.bboxapi.CreatePlugin(str, true);
                }
            }
        }

        private delegate bool DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;

        private void CheckUpdates(bool manual = true)
        {
            try
            {
                string path;
                if (Program.CanRunGithubApp(manual, out path))
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = String.Format("-bu {0} {1}",
                        Program.TagName, manual ? "1" : "0"),
                    });
                }
                else
                {
                    if (manual)
                        MessageBox.Show(".NET version 4.5 is required to run the updater.");
                    checkForUpdatesToolStripMenuItem.Enabled =
                    checkForUpdatesToolStripMenuItem.Visible = false;
                }
            }
            catch (Exception e)
            {
                if (manual)
                    MessageBox.Show(e.Message);
            }
        }

        public bool DisplayPropertyDescriptionsWhenAvailable
        {
            get { return _displayPropertyDescription; }
            set
            {
                _displayPropertyDescription = value;

                BrawlBox.Properties.Settings.Default.DisplayPropertyDescriptionWhenAvailable = _displayPropertyDescription;
                BrawlBox.Properties.Settings.Default.Save();
                UpdatePropertyDescriptionBox(propertyGrid1.SelectedGridItem);
            }
        }
        bool _displayPropertyDescription;

        public bool CheckUpdatesOnStartup
        {
            get { return _updatesOnStartup; }
            set
            {
                _updatesOnStartup = value;

                BrawlBox.Properties.Settings.Default.CheckUpdatesAtStartup = _updatesOnStartup;
                BrawlBox.Properties.Settings.Default.Save();
            }
        }
        bool _updatesOnStartup;

        private void UpdatePropertyDescriptionBox(GridItem item)
        {
            if (!DisplayPropertyDescriptionsWhenAvailable)
            {
                if (propertyGrid1.HelpVisible != false)
                    propertyGrid1.HelpVisible = false;
            }
            else
                propertyGrid1.HelpVisible = item != null && item.PropertyDescriptor != null && !String.IsNullOrEmpty(item.PropertyDescriptor.Description);
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (DisplayPropertyDescriptionsWhenAvailable)
                UpdatePropertyDescriptionBox(e.NewSelection);
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
        }

        public void UpdateName()
        {
            if (Program.RootPath != null)
                Text = String.Format("{0} - {1}", Program.AssemblyTitle, Program.RootPath);
            else
                Text = Program.AssemblyTitle;

#if DEBUG
            Text += " DEBUG";
#endif
        }

        public void TargetResource(ResourceNode n)
        {
            if (_root != null)
                resourceTree.SelectedNode = _root.FindResource(n, true);
        }

        public Control _currentControl = null;
        private Control _secondaryControl = null;
        public unsafe void resourceTree_SelectionChanged(object sender, EventArgs e)
        {
            audioPlaybackPanel1.TargetSource = null;
            animEditControl.TargetSequence = null;
            texAnimEditControl.TargetSequence = null;
            shpAnimEditControl.TargetSequence = null;
            msBinEditor1.CurrentNode = null;
            //soundPackControl1.TargetNode = null;
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
                ((Be.Windows.Forms.DynamicFileByteProvider)hexBox1.ByteProvider).Dispose();

            Control newControl = null;
            Control newControl2 = null;

            BaseWrapper w;
            ResourceNode node = null;
            bool disable2nd = false;
            if ((resourceTree.SelectedNode is BaseWrapper) && ((node = (w = resourceTree.SelectedNode as BaseWrapper).Resource) != null))
            {
                Action setScrollOffset = null;
                foreach (Control c in propertyGrid1.Controls) {
                    if (c.GetType().Name == "PropertyGridView") {
                        object scrollOffset = c.GetType().GetMethod("GetScrollOffset").Invoke(c, null);
                        setScrollOffset = () => c.GetType().GetMethod("SetScrollOffset").Invoke(c, new object[] { scrollOffset });
                        break;
                    }
                }

                propertyGrid1.SelectedObject = node;
                propertyGrid1.ExpandAllGridItems();
                
                setScrollOffset?.Invoke();

#if DEBUG
                if (node is IBufferNode)
                {
                    IBufferNode d = node as IBufferNode;
                    if (d.IsValid())
                    {
                        hexBox1.ByteProvider = new Be.Windows.Forms.DynamicFileByteProvider(new UnmanagedMemoryStream(
                                (byte*)d.GetAddress(),
                                d.GetLength(),
                                d.GetLength(),
                                FileAccess.ReadWrite))
                        { _supportsInsDel = false };
                        newControl = hexBox1;
                    }
                }
                else
#endif
                if (node is RSARGroupNode)
                {
                    rsarGroupEditor.LoadGroup(node as RSARGroupNode);
                    newControl = rsarGroupEditor;
                }
                else if (node is RELMethodNode)
                {
                    ppcDisassembler1.SetTarget((RELMethodNode)node);
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
                        newControl = audioPlaybackPanel1;
                }
                else if (node is IImageSource)
                {
                    previewPanel2.RenderingTarget = ((IImageSource)node);
                    newControl = previewPanel2;
                }
                else if (node is IRenderedObject)
                {
                    newControl = modelPanel1;
                }
                else if (node is STDTNode)
                {
                    STDTNode stdt = (STDTNode)node;

                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(stdt.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = stdt;
                    newControl = attributeGrid1;
                }

                if (node is IColorSource && !disable2nd)
                {
                    clrControl.ColorSource = node as IColorSource;
                    if (((IColorSource)node).ColorCount(0) > 0)
                        if (newControl != null)
                            newControl2 = clrControl;
                        else
                            newControl = clrControl;
                }

                if ((editToolStripMenuItem.DropDown = w.ContextMenuStrip) != null)
                    editToolStripMenuItem.Enabled = true;
                else
                    editToolStripMenuItem.Enabled = false;
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
                    _currentControl.Visible = false;
                _currentControl = newControl;
                if (_currentControl != null)
                    _currentControl.Visible = true;
            }
            else if (_currentControl != null && !_currentControl.Visible)
                _currentControl.Visible = true;
            if (_currentControl != null)
            {
                if (_secondaryControl != null)
                    _currentControl.Width = splitContainer2.Panel2.Width - _secondaryControl.Width;
                _currentControl.Dock = DockStyle.Fill;
            }

            //Model panel has to be loaded first to display model correctly
            if (_currentControl is ModelPanel)
            {
                if (node._children == null)
                    node.Populate(0);

                if (node is IModel && ModelEditControl.Instances.Count == 0)
                {
                    IModel m = node as IModel;
                    m.ResetToBindState();
                }

                IRenderedObject o = node as IRenderedObject;
                modelPanel1.AddTarget(o);
                modelPanel1.SetCamWithBox(o.GetBox());
            }
            else if (_currentControl is MDL0ObjectControl)
                mdL0ObjectControl1.SetTarget(node as MDL0ObjectNode);
            else if (_currentControl is TexCoordControl)
                texCoordControl1.TargetNode = ((MDL0MaterialRefNode)node);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Program.Close())
                e.Cancel = true;

            base.OnClosing(e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inFile;
            int i = Program.OpenFile(SupportedFilesHandler.CompleteFilterEditableOnly, out inFile);
            if (i != 0 && Program.Open(inFile))
                RecentFileHandler.AddFile(inFile);
        }

        #region File Menu
        private void aRCArchiveToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<ARCNode>(); }
        private void u8FileArchiveToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<U8Node>(); }
        private void brresPackToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<BRRESNode>(); }
        private void tPLTextureArchiveToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<TPLNode>(); }
        private void eFLSEffectListToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<EFLSNode>(); }
        private void rEFFParticlesToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<REFFNode>(); }
        private void rEFTParticleTexturesToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<REFTNode>(); }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { Program.Save(); }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) { Program.SaveAs(); }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) { Program.Close(); }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { AboutForm.Instance.ShowDialog(this); }

        private void bRStmAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;
            if (Program.OpenFile("PCM Audio (*.wav)|*.wav", out path) > 0)
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
                            Program.Close(true);
                    }
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { s });
                }
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void gCTEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GCTEditor().Show();
        }

        private void recentFilesToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Program.Open(((RecentFileHandler.FileMenuItem)e.ClickedItem).FileName);
        }

        private void checkForUpdatesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CheckUpdates();
        }

        private void splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            ((SplitContainer)sender).IsSplitterFixed = true;
        }
        private void splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            ((SplitContainer)sender).IsSplitterFixed = false;
        }
        private void splitContainer_MouseMove(object sender, MouseEventArgs e)
        {
            SplitContainer splitter = (SplitContainer)sender;
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
                    splitter.IsSplitterFixed = false;
            }
        }

        private void onPluginClicked(object sender, EventArgs e)
        {
            var plg = API.bboxapi.Plugins.Find(x => x.Name == ((ToolStripItem)sender).Text);
            plg?.Execute();
        }

        private void runScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog() { Filter = "All supported files (.py, .fsx)|*.py;*.fsx|Python file (.py)|*.py|F# script (.fsx)|*.fsx|All Files|*" })
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    bboxapi.RunScript(dlg.FileName);
                }
            }
        }

        private void reloadPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            API.bboxapi.Plugins.Clear();
            pluginToolStripMenuItem.DropDown.Items.Clear();
            foreach (var str in new[] { "*.py", "*.fsx" }.SelectMany(p => Directory.EnumerateFiles($"{Application.StartupPath}/Plugins", p)))
            {
                API.bboxapi.CreatePlugin(str, false);
                pluginToolStripMenuItem.DropDownItems.Add(Path.GetFileNameWithoutExtension(str), null, onPluginClicked);
            }
        }
    }

    public class RecentFileHandler : Component
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        public class FileMenuItem : ToolStripMenuItem
        {
            string fileName;

            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }

            public FileMenuItem(string fileName)
            {
                this.fileName = fileName;
            }

            public override string Text
            {
                get
                {
                    ToolStripMenuItem parent = (ToolStripMenuItem)this.OwnerItem;
                    int index = parent.DropDownItems.IndexOf(this);
                    return string.Format("{0} {1}", index + 1, fileName);
                }
                set
                {
                }
            }
        }

        public const int MaxRecentFiles = 24;

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

        void Init()
        {
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
        }

        public void AddFile(string fileName)
        {
            try
            {
                if (this.recentFileToolStripItem == null)
                    throw new OperationCanceledException("recentFileToolStripItem can not be null!");

                // check if the file is already in the collection
                int alreadyIn = GetIndexOfRecentFile(fileName);
                if (alreadyIn != -1) // remove it
                {
                    Settings.Default.RecentFiles.RemoveAt(alreadyIn);
                    if (recentFileToolStripItem.DropDownItems.Count > alreadyIn)
                        recentFileToolStripItem.DropDownItems.RemoveAt(alreadyIn);
                }
                else if (alreadyIn == 0) // it´s the latest file so return
                    return;

                // insert the file on top of the list
                Settings.Default.RecentFiles.Insert(0, fileName);
                recentFileToolStripItem.DropDownItems.Insert(0, new FileMenuItem(fileName));

                // remove the last one, if max size is reached
                if (Settings.Default.RecentFiles.Count > MaxRecentFiles)
                    Settings.Default.RecentFiles.RemoveAt(MaxRecentFiles);
                if (Settings.Default.RecentFiles.Count > Settings.Default.RecentFilesMax)
                    recentFileToolStripItem.DropDownItems.RemoveAt(Settings.Default.RecentFilesMax);

                // enable the menu item if it´s disabled
                if (!recentFileToolStripItem.Enabled)
                    recentFileToolStripItem.Enabled = true;

                // save the changes
                Settings.Default.Save();
            }
            catch { }
        }

        int GetIndexOfRecentFile(string filename)
        {
            for (int i = 0; i < Settings.Default.RecentFiles.Count; i++)
            {
                string currentFile = Settings.Default.RecentFiles[i];
                if (string.Equals(currentFile, filename, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        ToolStripMenuItem recentFileToolStripItem;

        public ToolStripMenuItem RecentFileToolStripItem
        {
            get { return recentFileToolStripItem; }
            set
            {
                if (recentFileToolStripItem == value)
                    return;

                recentFileToolStripItem = value;

                ReCreateItems();
            }
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RecentFilesMax")
            {
                ReCreateItems();
            }
        }

        void ReCreateItems()
        {
            if (recentFileToolStripItem == null)
                return;

            if (Settings.Default.RecentFiles == null)
                Settings.Default.RecentFiles = new StringCollection();

            recentFileToolStripItem.DropDownItems.Clear();
            recentFileToolStripItem.Enabled = (Settings.Default.RecentFiles.Count > 0);

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

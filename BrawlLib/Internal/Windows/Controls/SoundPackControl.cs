using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class SoundPackControl : UserControl
    {
        #region Designer

        public ListView lstSets;
        private ColumnHeader clmIndex;
        private ColumnHeader clmName;
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private ToolStripMenuItem mnuExport;
        private ToolStripMenuItem mnuReplace;
        private ToolStripMenuItem mnuPath;
        private ColumnHeader clmType;
        private ColumnHeader clmDataOffset;
        private ColumnHeader clmAudioOffset;
        private ColumnHeader clmEntryOffset;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem newFileToolStripMenuItem;
        private ToolStripMenuItem rWSDToolStripMenuItem;
        private ToolStripMenuItem rSEQToolStripMenuItem;
        private ToolStripMenuItem rBNKToolStripMenuItem;
        private ToolStripMenuItem externalReferenceToolStripMenuItem;
        private ToolStripMenuItem rSTMToolStripMenuItem;
        private AudioPlaybackPanel audioPlaybackPanel1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem makeAllExternalToolStripMenuItem;
        private ToolStripMenuItem makeAllInternalToolStripMenuItem;
        private ToolStripMenuItem _deleteToolStripMenuItem;

        private void InitializeComponent()
        {
            components = new Container();
            clmIndex = new ColumnHeader();
            clmName = new ColumnHeader();
            lstSets = new ListView();
            clmType = new ColumnHeader();
            clmDataOffset = new ColumnHeader();
            clmAudioOffset = new ColumnHeader();
            clmEntryOffset = new ColumnHeader();
            contextMenuStrip1 = new ContextMenuStrip(components);
            mnuPath = new ToolStripMenuItem();
            mnuExport = new ToolStripMenuItem();
            mnuReplace = new ToolStripMenuItem();
            _deleteToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            newFileToolStripMenuItem = new ToolStripMenuItem();
            rWSDToolStripMenuItem = new ToolStripMenuItem();
            rSEQToolStripMenuItem = new ToolStripMenuItem();
            rBNKToolStripMenuItem = new ToolStripMenuItem();
            rSTMToolStripMenuItem = new ToolStripMenuItem();
            externalReferenceToolStripMenuItem = new ToolStripMenuItem();
            audioPlaybackPanel1 = new AudioPlaybackPanel();
            editToolStripMenuItem = new ToolStripMenuItem();
            makeAllExternalToolStripMenuItem = new ToolStripMenuItem();
            makeAllInternalToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // clmIndex
            // 
            clmIndex.Text = "Index";
            clmIndex.Width = 40;
            // 
            // clmName
            // 
            clmName.Text = "Name";
            clmName.Width = 40;
            // 
            // lstSets
            // 
            lstSets.AutoArrange = false;
            lstSets.BorderStyle = BorderStyle.None;
            lstSets.Columns.AddRange(new ColumnHeader[]
            {
                clmIndex,
                clmType,
                clmName,
                clmDataOffset,
                clmAudioOffset,
                clmEntryOffset
            });
            lstSets.ContextMenuStrip = contextMenuStrip1;
            lstSets.Dock = DockStyle.Fill;
            lstSets.FullRowSelect = true;
            lstSets.GridLines = true;
            lstSets.HideSelection = false;
            lstSets.LabelWrap = false;
            lstSets.Location = new System.Drawing.Point(0, 28);
            lstSets.MultiSelect = false;
            lstSets.Name = "lstSets";
            lstSets.Size = new System.Drawing.Size(389, 105);
            lstSets.TabIndex = 0;
            lstSets.UseCompatibleStateImageBehavior = false;
            lstSets.View = View.Details;
            lstSets.ColumnClick += new ColumnClickEventHandler(lstSets_ColumnClick);
            lstSets.SelectedIndexChanged += new EventHandler(lstSets_SelectedIndexChanged);
            lstSets.DoubleClick += new EventHandler(lstSets_DoubleClick);
            lstSets.KeyDown += new KeyEventHandler(lstSets_KeyDown);
            // 
            // clmType
            // 
            clmType.Text = "Type";
            // 
            // clmDataOffset
            // 
            clmDataOffset.Text = "Data Offset";
            clmDataOffset.Width = 70;
            // 
            // clmAudioOffset
            // 
            clmAudioOffset.Text = "Audio Offset";
            clmAudioOffset.Width = 70;
            // 
            // clmEntryOffset
            // 
            clmEntryOffset.Text = "Entry Offset";
            clmEntryOffset.Width = 80;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[]
            {
                mnuPath,
                mnuExport,
                mnuReplace,
                _deleteToolStripMenuItem
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(138, 108);
            contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);
            // 
            // mnuPath
            // 
            mnuPath.Name = "mnuPath";
            mnuPath.Size = new System.Drawing.Size(137, 26);
            mnuPath.Text = "Path...";
            mnuPath.Click += new EventHandler(mnuPath_Click);
            // 
            // mnuExport
            // 
            mnuExport.Name = "mnuExport";
            mnuExport.Size = new System.Drawing.Size(137, 26);
            mnuExport.Text = "Export";
            mnuExport.Click += new EventHandler(mnuExport_Click);
            // 
            // mnuReplace
            // 
            mnuReplace.Name = "mnuReplace";
            mnuReplace.Size = new System.Drawing.Size(137, 26);
            mnuReplace.Text = "Replace";
            mnuReplace.Click += new EventHandler(mnuReplace_Click);
            // 
            // _deleteToolStripMenuItem
            // 
            _deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            _deleteToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            _deleteToolStripMenuItem.Text = "Delete";
            _deleteToolStripMenuItem.Click += new EventHandler(_deleteToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[]
            {
                newFileToolStripMenuItem,
                editToolStripMenuItem
            });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(389, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // newFileToolStripMenuItem
            // 
            newFileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                rWSDToolStripMenuItem,
                rSEQToolStripMenuItem,
                rBNKToolStripMenuItem,
                rSTMToolStripMenuItem,
                externalReferenceToolStripMenuItem
            });
            newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            newFileToolStripMenuItem.Size = new System.Drawing.Size(78, 24);
            newFileToolStripMenuItem.Text = "New File";
            // 
            // rWSDToolStripMenuItem
            // 
            rWSDToolStripMenuItem.Name = "rWSDToolStripMenuItem";
            rWSDToolStripMenuItem.Size = new System.Drawing.Size(207, 26);
            rWSDToolStripMenuItem.Text = "RWSD";
            rWSDToolStripMenuItem.Click += new EventHandler(rWSDToolStripMenuItem_Click);
            // 
            // rSEQToolStripMenuItem
            // 
            rSEQToolStripMenuItem.Name = "rSEQToolStripMenuItem";
            rSEQToolStripMenuItem.Size = new System.Drawing.Size(207, 26);
            rSEQToolStripMenuItem.Text = "RSEQ";
            rSEQToolStripMenuItem.Click += new EventHandler(rSEQToolStripMenuItem_Click);
            // 
            // rBNKToolStripMenuItem
            // 
            rBNKToolStripMenuItem.Name = "rBNKToolStripMenuItem";
            rBNKToolStripMenuItem.Size = new System.Drawing.Size(207, 26);
            rBNKToolStripMenuItem.Text = "RBNK";
            rBNKToolStripMenuItem.Click += new EventHandler(rBNKToolStripMenuItem_Click);
            // 
            // rSTMToolStripMenuItem
            // 
            rSTMToolStripMenuItem.Name = "rSTMToolStripMenuItem";
            rSTMToolStripMenuItem.Size = new System.Drawing.Size(207, 26);
            rSTMToolStripMenuItem.Text = "RSTM";
            rSTMToolStripMenuItem.Click += new EventHandler(rSTMToolStripMenuItem_Click);
            // 
            // externalReferenceToolStripMenuItem
            // 
            externalReferenceToolStripMenuItem.Name = "externalReferenceToolStripMenuItem";
            externalReferenceToolStripMenuItem.Size = new System.Drawing.Size(207, 26);
            externalReferenceToolStripMenuItem.Text = "External Reference";
            externalReferenceToolStripMenuItem.Click += new EventHandler(externalReferenceToolStripMenuItem_Click);
            // 
            // audioPlaybackPanel1
            // 
            audioPlaybackPanel1.Dock = DockStyle.Bottom;
            audioPlaybackPanel1.Location = new System.Drawing.Point(0, 133);
            audioPlaybackPanel1.Name = "audioPlaybackPanel1";
            audioPlaybackPanel1.Size = new System.Drawing.Size(389, 120);
            audioPlaybackPanel1.TabIndex = 2;
            audioPlaybackPanel1.TargetStreams = null;
            audioPlaybackPanel1.Visible = false;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                makeAllExternalToolStripMenuItem,
                makeAllInternalToolStripMenuItem
            });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            editToolStripMenuItem.Text = "Edit";
            // 
            // makeAllExternalToolStripMenuItem
            // 
            makeAllExternalToolStripMenuItem.Name = "makeAllExternalToolStripMenuItem";
            makeAllExternalToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            makeAllExternalToolStripMenuItem.Text = "Make all external";
            makeAllExternalToolStripMenuItem.Click += new EventHandler(makeAllExternalToolStripMenuItem_Click);
            // 
            // makeAllInternalToolStripMenuItem
            // 
            makeAllInternalToolStripMenuItem.Name = "makeAllInternalToolStripMenuItem";
            makeAllInternalToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            makeAllInternalToolStripMenuItem.Text = "Make all internal";
            makeAllInternalToolStripMenuItem.Click += new EventHandler(makeAllInternalToolStripMenuItem_Click);
            // 
            // SoundPackControl
            // 
            Controls.Add(lstSets);
            Controls.Add(audioPlaybackPanel1);
            Controls.Add(menuStrip1);
            Name = "SoundPackControl";
            Size = new System.Drawing.Size(389, 253);
            DoubleClick += new EventHandler(lstSets_DoubleClick);
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public PropertyGrid _grid = null;

        private RSARNode _targetNode;

        public RSARNode TargetNode
        {
            get => _targetNode;
            set
            {
                if (value == _targetNode)
                {
                    return;
                }

                _targetNode = value;
                NodeChanged();
            }
        }

        private SoundPackItem _selectedItem;
        private readonly ListViewColumnSorter lvwColumnSorter;

        public SoundPackControl()
        {
            InitializeComponent();

            lvwColumnSorter = new ListViewColumnSorter();
            lstSets.ListViewItemSorter = lvwColumnSorter;

            backgroundWorker1 = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
        }

        private void Update(ListView list)
        {
            list.BeginUpdate();
            list.Items.Clear();
            if (_targetNode != null)
            {
                foreach (RSARFileNode file in _targetNode.Files)
                {
                    list.Items.Add(new SoundPackItem(file));
                }
            }

            list.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            list.EndUpdate();
        }

        private void AddFile(ListView list, RSARFileNode file)
        {
            list.BeginUpdate();
            if (_targetNode != null)
            {
                list.Items.Add(new SoundPackItem(file));
            }

            list.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            list.EndUpdate();
        }

        private delegate void delUpdate(ListView list);

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (lstSets.InvokeRequired)
            {
                delUpdate callbackMethod = new delUpdate(Update);
                Invoke(callbackMethod, lstSets);
            }
            else
            {
                Update(lstSets);
            }
        }

        private readonly BackgroundWorker backgroundWorker1;

        private void NodeChanged()
        {
            //if (backgroundWorker1.IsBusy != true && _targetNode != null)
            //    backgroundWorker1.RunWorkerAsync();
            Update(lstSets);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedItem == null)
            {
                e.Cancel = true;
            }
            else
            {
                mnuExport.Enabled = !(_selectedItem._node is RSARExtFileNode);
            }
        }

        private void mnuPath_Click(object sender, EventArgs e)
        {
            using (SoundPathChanger dlg = new SoundPathChanger())
            {
                RSARExtFileNode ext = _selectedItem._node as RSARExtFileNode;
                dlg.FilePath = ext.FullExtPath;
                dlg.dlg.InitialDirectory = TargetNode._origPath.Substring(0, TargetNode._origPath.LastIndexOf('\\'));
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ext.FullExtPath = dlg.FilePath;
                    _selectedItem.SubItems[2].Text = ext._extPath;
                }
            }
        }

        private void lstSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSets.SelectedIndices.Count == 0)
            {
                _selectedItem = null;
            }
            else
            {
                _selectedItem = lstSets.SelectedItems[0] as SoundPackItem;
            }

            if (_selectedItem != null && (audioPlaybackPanel1.Visible = _selectedItem._node is RSTMNode))
            {
                audioPlaybackPanel1.TargetSource = _selectedItem._node as RSTMNode;
            }
            else if (audioPlaybackPanel1.TargetSource != null)
            {
                audioPlaybackPanel1.TargetSource = null;
            }

            if (_grid != null && _selectedItem != null)
            {
                _grid.SelectedObject = _selectedItem._node;
            }
        }

        private void mnuExport_Click(object sender, EventArgs e)
        {
            if (_selectedItem.SubItems[1].Text != "External")
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.FileName = _selectedItem.SubItems[2].Text.Replace('/', '_');
                    switch (_selectedItem.SubItems[1].Text)
                    {
                        case "RWSD":
                            dlg.Filter = FileFilters.RWSD;
                            break;
                        case "RBNK":
                            dlg.Filter = FileFilters.RBNK;
                            break;
                        case "RSEQ":
                            dlg.Filter = FileFilters.RSEQ;
                            break;
                        case "RSAR":
                            dlg.Filter = FileFilters.RSAR;
                            break;
                    }

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _selectedItem._node.Export(dlg.FileName);
                    }
                }
            }
        }

        private void lstSets_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedItem._node is RSARExtFileNode)
            {
                RSARExtFileNode ext = _selectedItem._node as RSARExtFileNode;
                if (File.Exists(ext.FullExtPath))
                {
                    Process.Start(ext.FullExtPath);
                }
                else
                {
                    mnuPath_Click(this, null);
                }
            }
            else if (!(_selectedItem._node is RSTMNode))
            {
                new EditRSARFileDialog().ShowDialog(this, _selectedItem._node);
            }
        }

        private void lstSets_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstSets.Sort();
        }

        private void lstSets_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lstSets.SelectedItems.Clear();
                if (_grid != null)
                {
                    _grid.SelectedObject = _targetNode;
                }
            }
        }

        private void mnuReplace_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = SupportedFilesHandler.GetCompleteFilter("brwsd", "brbnk", "brseq", "brstm");
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _selectedItem._node.Replace(dlg.FileName);
                }
            }
        }

        private void _deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedItem._node.Remove();
            lstSets.Items.Remove(_selectedItem);
        }

        private void rWSDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RWSDNode node = new RWSDNode
            {
                _name = $"[{_targetNode.Files.Count}] RWSD",
                _fileIndex = _targetNode.Files.Count
            };
            node.InitGroups();
            node._parent = _targetNode;
            _targetNode.Files.Add(node);
            node.SignalPropertyChange();
            AddFile(lstSets, node);
        }

        private void rSEQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RSEQNode node = new RSEQNode
            {
                _name = $"[{_targetNode.Files.Count}] RSEQ",
                _fileIndex = _targetNode.Files.Count
            };
            node._parent = _targetNode;
            _targetNode.Files.Add(node);
            node.SignalPropertyChange();
            AddFile(lstSets, node);
        }

        private void rBNKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RBNKNode node = new RBNKNode
            {
                _name = $"[{_targetNode.Files.Count}] RBNK",
                _fileIndex = _targetNode.Files.Count
            };
            node.InitGroups();
            node._parent = _targetNode;
            _targetNode.Files.Add(node);
            node.SignalPropertyChange();
            AddFile(lstSets, node);
        }

        private void externalReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RSARExtFileNode node = new RSARExtFileNode
            {
                _name = $"[{_targetNode.Files.Count}] External",
                _fileIndex = _targetNode.Files.Count
            };
            node._parent = _targetNode;
            _targetNode.Files.Add(node);
            node.SignalPropertyChange();
            AddFile(lstSets, node);
        }

        private void rSTMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog {Filter = SupportedFilesHandler.GetCompleteFilter("wav")})
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    RSTMNode r = new RSTMNode {_fileIndex = _targetNode.Files.Count};
                    using (BrstmConverterDialog dlg = new BrstmConverterDialog())
                    {
                        dlg.AudioSource = ofd.FileName;
                        if (dlg.ShowDialog(this) == DialogResult.OK)
                        {
                            r.Name = $"[{_targetNode.Files.Count}] {Path.GetFileNameWithoutExtension(dlg.AudioSource)}";
                            r.ReplaceRaw(dlg.AudioData);
                        }
                    }

                    r._parent = _targetNode;
                    _targetNode.Files.Add(r);
                    r.SignalPropertyChange();
                    AddFile(lstSets, r);
                }
            }
        }

        private void makeAllExternalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (SoundPackItem i in lstSets.Items)
            {
                if (i._node is RSARExtFileNode)
                {
                    continue;
                }

                ListViewItem v = i;
                string type = v.SubItems[1].Text;
                string dir = "\\" + type + "\\";
                string fileName = i._node.Name.Replace('/', '_').Replace('<', '(').Replace('>', ')') + ".b" +
                                  type.ToLower();

                string newPath = _targetNode._origPath.Substring(0, _targetNode._origPath.LastIndexOf('\\')) + dir;
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                i._node.Export(newPath + fileName);

                RSARGroupNode[] groups = i._node.GroupRefNodes;

                //Contains lists of all instances of this file in each group
                List<int>[] refs = new List<int>[groups.Length];

                //Remove all references to this file
                for (int n = 0; n < groups.Length; ++n)
                {
                    refs[n] = new List<int>();
                    RSARGroupNode g = groups[n];
                    for (int m = 0; m < g._files.Count; ++m)
                    {
                        if (g._files[m] == i._node)
                        {
                            refs[n].Add(m);
                            g._files.RemoveAt(m--);
                        }
                    }
                }
                //if (i._node is RWSDNode)
                //{
                //    if (i._node.Children.Count > 0)
                //        foreach (RWSDDataNode d in i._node.Children[0].Children)
                //        {
                //            foreach (RSARSoundNode r in d._refs)
                //            {

                //            }
                //        }
                //}
                //else if (i._node is RSEQNode)
                //{
                //    foreach (RSEQLabelNode d in i._node.Children)
                //    {

                //    }
                //}
                //else 
                List<RSARBankNode> rbnkRefs = null;
                if (i._node is RBNKNode)
                {
                    RBNKNode rbnk = i._node as RBNKNode;
                    rbnkRefs = rbnk._rsarBankEntries;
                    for (int j = 0; j < rbnk._rsarBankEntries.Count; ++j)
                    {
                        RSARBankNode b = rbnk._rsarBankEntries[j];
                        b.BankNode = null;
                    }
                    //if (i._node.Children.Count > 0)
                    //    foreach (RBNKDataEntryNode d in i._node.Children[0].Children)
                    //    {

                    //    }
                }

                _targetNode.Files.RemoveAt(i.Index);

                RSARExtFileNode ext = new RSARExtFileNode
                {
                    _fileIndex = i.Index,
                    _parent = _targetNode
                };
                _targetNode.Files.Insert(i.Index, ext);

                ext.ExtPath = (dir + fileName).Replace('\\', '/').Substring(1);
                ext.Name = $"[{i.Index}] {ext.ExtPath}";

                if ((i._node is RBNKNode || i._node is RSARExtFileNode) && rbnkRefs != null)
                {
                    RBNKNode rbnk = i._node as RBNKNode;
                    foreach (RSARBankNode b in rbnkRefs)
                    {
                        b.BankNode = ext;
                    }
                }

                //Remake references
                for (int groupID = 0; groupID < refs.Length; ++groupID)
                {
                    int r = 0;
                    RSARGroupNode group = groups[groupID];
                    foreach (int occurrence in refs[groupID])
                    {
                        group._files.Insert(occurrence + r++, ext);
                    }

                    ext._groupRefs.Add(group);
                }

                i._node = ext;
            }

            _targetNode.IsDirty = true;
            Update(lstSets);
        }

        private void makeAllInternalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (SoundPackItem i in lstSets.Items)
            {
                if (!(i._node is RSARExtFileNode))
                {
                    continue;
                }

                RSARExtFileNode w = i._node as RSARExtFileNode;
                string path = w.FullExtPath;
                if (!File.Exists(path))
                {
                    continue;
                }

                RSARFileNode ext = NodeFactory.FromFile(null, path) as RSARFileNode;
                if (ext == null)
                {
                    continue;
                }

                ListViewItem v = i;
                string type = v.SubItems[1].ToString();

                i._node.Remove();
                _targetNode.Files.RemoveAt(i.Index);

                ext._parent = _targetNode;
                ext._fileIndex = i.Index;
                i._node = ext;
                _targetNode.Files.Insert(i.Index, ext);
            }
        }
    }

    public class SoundPackItem : ListViewItem
    {
        public RSARFileNode _node;

        public SoundPackItem(RSARFileNode file)
        {
            ImageIndex = (byte) file.ResourceFileType;

            Text = file.FileNodeIndex.ToString();

            _node = file;

            string s = file.ResourceFileType.ToString();
            if (file is RSARExtFileNode)
            {
                s = "External";
            }

            SubItems.Add(s);
            int i = Helpers.FindFirst(file.Name, 0, ']');
            SubItems.Add(file.Name.Substring(i + 1));
            //SubItems.Add(file.ExtPath);
            SubItems.Add("0x" + file.DataOffset);
            SubItems.Add("0x" + file.AudioOffset);
            SubItems.Add("0x" + file.InfoHeaderOffset);
        }
    }

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;

        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;

        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private readonly CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem) x;
            listviewY = (ListViewItem) y;

            // Compare the two items
            if (ColumnToSort == 0)
            {
                compareResult = ObjectCompare.Compare(int.Parse(listviewX.SubItems[ColumnToSort].Text),
                    int.Parse(listviewY.SubItems[ColumnToSort].Text));
            }
            else if (ColumnToSort >= 4)
            {
                compareResult = ObjectCompare.Compare(
                    int.Parse(listviewX.SubItems[ColumnToSort].Text.Substring(2),
                        System.Globalization.NumberStyles.HexNumber),
                    int.Parse(listviewY.SubItems[ColumnToSort].Text.Substring(2),
                        System.Globalization.NumberStyles.HexNumber));
            }
            else
            {
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text,
                    listviewY.SubItems[ColumnToSort].Text);
            }

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }

            if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return -compareResult;
            }

            // Return '0' to indicate they are equal
            return 0;
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set => ColumnToSort = value;
            get => ColumnToSort;
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set => OrderOfSort = value;
            get => OrderOfSort;
        }
    }
}
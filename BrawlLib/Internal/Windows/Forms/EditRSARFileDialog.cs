using BrawlLib.Internal.Audio;
using BrawlLib.Internal.Windows.Controls;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public class EditRSARFileDialog : Form
    {
        public EditRSARFileDialog()
        {
            InitializeComponent();
        }

        private AudioPlaybackPanel audioPlaybackPanel1;
        private ContextMenuStrip ctxData;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem dataReplace;
        private ToolStripMenuItem dataExport;
        private ContextMenuStrip ctxSounds;
        private ToolStripMenuItem sndReplace;
        private ToolStripMenuItem sndExport;
        private Button button1;
        private ToolStripMenuItem dataNew;
        private ToolStripMenuItem dataDelete;
        private ToolStripMenuItem sndNew;
        private ToolStripMenuItem sndDelete;
        private ToolStripMenuItem dataNewNullEntry;
        private ToolStripMenuItem dataNewInstParam;
        private ToolStripMenuItem dataNewRange;
        private ToolStripMenuItem dataNewIndex;
        private RSARFileNode _targetNode;

        public RSARFileNode TargetNode
        {
            get => _targetNode;
            set
            {
                _targetNode = value;
                TargetChanged();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            audioPlaybackPanel1.TargetSource = null;
            base.OnClosing(e);
        }

        public DialogResult ShowDialog(IWin32Window owner, RSARFileNode node)
        {
            TargetNode = node;
            TargetNode.UpdateControl += OnUpdateCurrControl;
            return ShowDialog();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            audioPlaybackPanel1.TargetSource = null;
            TargetNode.UpdateControl -= OnUpdateCurrControl;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected internal virtual void OnUpdateCurrControl(object sender, EventArgs e)
        {
            soundsListBox_SelectedIndexChanged(this, null);
        }

        #region Designer

        private Panel panel1;
        private Panel panel2;
        private Panel panel4;
        private Splitter splitter2;
        private Panel panel3;
        private Splitter splitter1;
        private PropertyGrid propertyGrid;
        private ListBox soundsListBox;
        private Label label2;
        private ListBox dataListBox;
        private Label label1;
        private Button btnOkay;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnOkay = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            dataListBox = new ListBox();
            ctxData = new ContextMenuStrip(components);
            dataReplace = new ToolStripMenuItem();
            dataExport = new ToolStripMenuItem();
            dataNew = new ToolStripMenuItem();
            dataNewNullEntry = new ToolStripMenuItem();
            dataNewInstParam = new ToolStripMenuItem();
            dataNewRange = new ToolStripMenuItem();
            dataNewIndex = new ToolStripMenuItem();
            dataDelete = new ToolStripMenuItem();
            label1 = new Label();
            splitter2 = new Splitter();
            panel4 = new Panel();
            soundsListBox = new ListBox();
            ctxSounds = new ContextMenuStrip(components);
            sndReplace = new ToolStripMenuItem();
            sndExport = new ToolStripMenuItem();
            sndNew = new ToolStripMenuItem();
            sndDelete = new ToolStripMenuItem();
            label2 = new Label();
            splitter1 = new Splitter();
            propertyGrid = new PropertyGrid();
            audioPlaybackPanel1 = new AudioPlaybackPanel();
            button1 = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ctxData.SuspendLayout();
            panel4.SuspendLayout();
            ctxSounds.SuspendLayout();
            SuspendLayout();
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOkay.Location = new System.Drawing.Point(369, 3);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Done";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // panel1
            // 
            panel1.Controls.Add(btnOkay);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 320);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(447, 31);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(splitter2);
            panel2.Controls.Add(panel4);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.MinimumSize = new System.Drawing.Size(54, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(107, 320);
            panel2.TabIndex = 4;
            // 
            // panel3
            // 
            panel3.Controls.Add(dataListBox);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Margin = new Padding(0);
            panel3.MinimumSize = new System.Drawing.Size(0, 15);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(107, 160);
            panel3.TabIndex = 1;
            // 
            // dataListBox
            // 
            dataListBox.ContextMenuStrip = ctxData;
            dataListBox.Dock = DockStyle.Fill;
            dataListBox.FormattingEnabled = true;
            dataListBox.IntegralHeight = false;
            dataListBox.ItemHeight = 16;
            dataListBox.Location = new System.Drawing.Point(0, 21);
            dataListBox.Name = "dataListBox";
            dataListBox.Size = new System.Drawing.Size(107, 139);
            dataListBox.TabIndex = 3;
            dataListBox.SelectedIndexChanged += new EventHandler(dataListBox_SelectedIndexChanged);
            // 
            // ctxData
            // 
            ctxData.ImageScalingSize = new System.Drawing.Size(20, 20);
            ctxData.Items.AddRange(new ToolStripItem[]
            {
                dataReplace,
                dataExport,
                dataNew,
                dataDelete
            });
            ctxData.Name = "contextMenuStrip1";
            ctxData.Size = new System.Drawing.Size(182, 136);
            // 
            // dataReplace
            // 
            dataReplace.Name = "dataReplace";
            dataReplace.Size = new System.Drawing.Size(181, 26);
            dataReplace.Text = "Replace";
            dataReplace.Click += new EventHandler(_replaceToolStripMenuItem_Click);
            // 
            // dataExport
            // 
            dataExport.Name = "dataExport";
            dataExport.Size = new System.Drawing.Size(181, 26);
            dataExport.Text = "Export";
            dataExport.Click += new EventHandler(exportToolStripMenuItem_Click);
            // 
            // dataNew
            // 
            dataNew.DropDownItems.AddRange(new ToolStripItem[]
            {
                dataNewNullEntry,
                dataNewInstParam,
                dataNewRange,
                dataNewIndex
            });
            dataNew.Name = "dataNew";
            dataNew.Size = new System.Drawing.Size(181, 26);
            dataNew.Text = "New";
            dataNew.Click += new EventHandler(dataNew_Click);
            // 
            // dataNewNullEntry
            // 
            dataNewNullEntry.Name = "dataNewNullEntry";
            dataNewNullEntry.Size = new System.Drawing.Size(215, 26);
            dataNewNullEntry.Text = "Null Entry";
            dataNewNullEntry.Click += new EventHandler(nullEntryToolStripMenuItem1_Click);
            // 
            // dataNewInstParam
            // 
            dataNewInstParam.Name = "dataNewInstParam";
            dataNewInstParam.Size = new System.Drawing.Size(215, 26);
            dataNewInstParam.Text = "Instance Parameters";
            dataNewInstParam.Click += new EventHandler(instanceParametersToolStripMenuItem_Click);
            // 
            // dataNewRange
            // 
            dataNewRange.Name = "dataNewRange";
            dataNewRange.Size = new System.Drawing.Size(215, 26);
            dataNewRange.Text = "Range Group";
            dataNewRange.Click += new EventHandler(rangeGroupToolStripMenuItem1_Click);
            // 
            // dataNewIndex
            // 
            dataNewIndex.Name = "dataNewIndex";
            dataNewIndex.Size = new System.Drawing.Size(215, 26);
            dataNewIndex.Text = "Index Group";
            dataNewIndex.Click += new EventHandler(indexGroupToolStripMenuItem1_Click);
            // 
            // dataDelete
            // 
            dataDelete.Name = "dataDelete";
            dataDelete.Size = new System.Drawing.Size(181, 26);
            dataDelete.Text = "Delete";
            dataDelete.Click += new EventHandler(dataDelete_Click);
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Dock = DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(107, 21);
            label1.TabIndex = 2;
            label1.Text = "Data";
            label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitter2
            // 
            splitter2.Dock = DockStyle.Bottom;
            splitter2.Location = new System.Drawing.Point(0, 160);
            splitter2.Name = "splitter2";
            splitter2.Size = new System.Drawing.Size(107, 3);
            splitter2.TabIndex = 0;
            splitter2.TabStop = false;
            splitter2.SplitterMoved += new SplitterEventHandler(splitter2_SplitterMoved);
            // 
            // panel4
            // 
            panel4.Controls.Add(soundsListBox);
            panel4.Controls.Add(label2);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new System.Drawing.Point(0, 163);
            panel4.Margin = new Padding(0);
            panel4.MinimumSize = new System.Drawing.Size(0, 15);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(107, 157);
            panel4.TabIndex = 2;
            // 
            // soundsListBox
            // 
            soundsListBox.ContextMenuStrip = ctxSounds;
            soundsListBox.Dock = DockStyle.Fill;
            soundsListBox.FormattingEnabled = true;
            soundsListBox.IntegralHeight = false;
            soundsListBox.ItemHeight = 16;
            soundsListBox.Location = new System.Drawing.Point(0, 21);
            soundsListBox.Name = "soundsListBox";
            soundsListBox.Size = new System.Drawing.Size(107, 136);
            soundsListBox.TabIndex = 2;
            soundsListBox.SelectedIndexChanged += new EventHandler(soundsListBox_SelectedIndexChanged);
            // 
            // ctxSounds
            // 
            ctxSounds.ImageScalingSize = new System.Drawing.Size(20, 20);
            ctxSounds.Items.AddRange(new ToolStripItem[]
            {
                sndReplace,
                sndExport,
                sndNew,
                sndDelete
            });
            ctxSounds.Name = "contextMenuStrip1";
            ctxSounds.Size = new System.Drawing.Size(138, 108);
            // 
            // sndReplace
            // 
            sndReplace.Name = "sndReplace";
            sndReplace.Size = new System.Drawing.Size(137, 26);
            sndReplace.Text = "Replace";
            sndReplace.Click += new EventHandler(toolStripMenuItem1_Click);
            // 
            // sndExport
            // 
            sndExport.Name = "sndExport";
            sndExport.Size = new System.Drawing.Size(137, 26);
            sndExport.Text = "Export";
            sndExport.Click += new EventHandler(toolStripMenuItem2_Click);
            // 
            // sndNew
            // 
            sndNew.Name = "sndNew";
            sndNew.Size = new System.Drawing.Size(137, 26);
            sndNew.Text = "New";
            sndNew.Click += new EventHandler(sndNew_Click);
            // 
            // sndDelete
            // 
            sndDelete.Name = "sndDelete";
            sndDelete.Size = new System.Drawing.Size(137, 26);
            sndDelete.Text = "Delete";
            sndDelete.Click += new EventHandler(sndDelete_Click);
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Dock = DockStyle.Top;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(107, 21);
            label2.TabIndex = 1;
            label2.Text = "Sounds";
            label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitter1
            // 
            splitter1.Location = new System.Drawing.Point(107, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(3, 320);
            splitter1.TabIndex = 0;
            splitter1.TabStop = false;
            // 
            // propertyGrid
            // 
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.HelpVisible = false;
            propertyGrid.Location = new System.Drawing.Point(110, 24);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new System.Drawing.Size(337, 185);
            propertyGrid.TabIndex = 5;
            // 
            // audioPlaybackPanel1
            // 
            audioPlaybackPanel1.Dock = DockStyle.Bottom;
            audioPlaybackPanel1.Location = new System.Drawing.Point(110, 209);
            audioPlaybackPanel1.Name = "audioPlaybackPanel1";
            audioPlaybackPanel1.Size = new System.Drawing.Size(337, 111);
            audioPlaybackPanel1.TabIndex = 6;
            audioPlaybackPanel1.TargetStreams = null;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Top;
            button1.Location = new System.Drawing.Point(110, 0);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(337, 24);
            button1.TabIndex = 8;
            button1.Text = "View Entries";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(button1_Click);
            // 
            // EditRSARFileDialog
            // 
            AcceptButton = btnOkay;
            ClientSize = new System.Drawing.Size(447, 351);
            Controls.Add(propertyGrid);
            Controls.Add(button1);
            Controls.Add(audioPlaybackPanel1);
            Controls.Add(splitter1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "EditRSARFileDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit RSAR File";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ctxData.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ctxSounds.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public void TargetChanged()
        {
            dataListBox.Items.Clear();
            soundsListBox.Items.Clear();

            if (TargetNode is RSEQNode)
            {
                splitter2.Visible = panel4.Visible = audioPlaybackPanel1.Visible = false;

                dataListBox.Items.AddRange(TargetNode.Children.ToArray());
                if (dataListBox.Items.Count > 0)
                {
                    dataListBox.SelectedIndex = 0;
                }
            }
            else if (TargetNode is RBNKNode || TargetNode is RWSDNode)
            {
                splitter2.Visible = panel4.Visible = audioPlaybackPanel1.Visible = true;

                dataListBox.Items.AddRange(TargetNode.Children[0].Children.ToArray());
                if (dataListBox.Items.Count > 0)
                {
                    dataListBox.SelectedIndex = 0;
                }

                if (TargetNode.Children.Count > 1)
                {
                    soundsListBox.Items.AddRange(TargetNode.Children[1].Children.ToArray());
                }

                if (soundsListBox.Items.Count > 0)
                {
                    soundsListBox.SelectedIndex = 0;
                }
            }

            button1.Visible =
                dataNewNullEntry.Visible =
                    dataNewInstParam.Visible =
                        dataNewRange.Visible =
                            dataNewIndex.Visible = TargetNode is RBNKNode;

            if (TargetNode != null)
            {
                Text = "Edit RSAR File - " + TargetNode.Name;
            }
            else
            {
                Text = "Edit RSAR File";
            }
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }

        private void dataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataListBox.SelectedIndex < 0)
            {
                return;
            }

            ResourceNode r = dataListBox.Items[dataListBox.SelectedIndex] as ResourceNode;
            propertyGrid.SelectedObject = r;
            int w;
            if (TargetNode is RWSDNode)
            {
                w = (r as RWSDDataNode)._part3._waveIndex;
                if (w < soundsListBox.Items.Count)
                {
                    soundsListBox.SelectedIndex = w;
                }
            }
            else if (TargetNode is RBNKNode && r is RBNKDataInstParamNode)
            {
                w = (r as RBNKDataInstParamNode).hdr._waveIndex;
                if (w < soundsListBox.Items.Count)
                {
                    soundsListBox.SelectedIndex = w;
                }
            }

            button1.Enabled = !(button1.Text != "Back" && r is RBNKDataEntryNode);
        }

        private void soundsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (soundsListBox.SelectedIndex < 0)
            {
                return;
            }

            ResourceNode r = soundsListBox.Items[soundsListBox.SelectedIndex] as ResourceNode;
            if (r is IAudioSource)
            {
                audioPlaybackPanel1.TargetSource = r as IAudioSource;
            }
        }

        private void _replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataListBox.SelectedIndex < 0)
            {
                return;
            }

            ResourceNode r = dataListBox.Items[dataListBox.SelectedIndex] as ResourceNode;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = FileFilters.Raw;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    r.Replace(dlg.FileName);
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataListBox.SelectedIndex < 0)
            {
                return;
            }

            ResourceNode r = dataListBox.Items[dataListBox.SelectedIndex] as ResourceNode;
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.FileName = r.Name;
                dlg.Filter = FileFilters.Raw;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    r.Export(dlg.FileName);
                }
            }
        }

        private RBNKEntryNode _baseEntry;

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "View Entries")
            {
                button1.Text = "Back";

                if (dataListBox.SelectedIndex < 0)
                {
                    return;
                }

                RBNKEntryNode entry = dataListBox.Items[dataListBox.SelectedIndex] as RBNKEntryNode;
                _baseEntry = entry;
                label1.Text = entry.Name;
                dataListBox.Items.Clear();
                dataListBox.Items.AddRange(entry.Children.ToArray());
                if (dataListBox.Items.Count > 0)
                {
                    dataListBox.SelectedIndex = 0;
                }
            }
            else
            {
                button1.Text = "View Entries";
                dataListBox.Items.Clear();
                dataListBox.Items.AddRange(TargetNode.Children[0].Children.ToArray());
                if (dataListBox.Items.Count > 0)
                {
                    dataListBox.SelectedIndex = 0;
                }

                label1.Text = "Data";
                _baseEntry = null;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (soundsListBox.SelectedIndex < 0)
            {
                return;
            }

            ResourceNode r = soundsListBox.Items[soundsListBox.SelectedIndex] as ResourceNode;
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.FileName = r.Name;
                dlg.Filter = FileFilters.WAV + "|" + FileFilters.Raw;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    r.Export(dlg.FileName);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (soundsListBox.SelectedIndex < 0)
            {
                return;
            }

            ResourceNode r = soundsListBox.Items[soundsListBox.SelectedIndex] as ResourceNode;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = FileFilters.WAV + "|" + FileFilters.Raw;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    r.Replace(dlg.FileName);
                }
            }
        }

        private void dataNew_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RWSDNode))
            {
                return;
            }

            RWSDDataNode d = new RWSDDataNode
            {
                _name = $"[{TargetNode.Children[0].Children.Count}]Data"
            };

            d.Parent = TargetNode.Children[0];
            TargetChanged();
            ReselectBaseEntry();
        }

        private void dataDelete_Click(object sender, EventArgs e)
        {
            ResourceNode entry = dataListBox.Items[dataListBox.SelectedIndex] as ResourceNode;
            entry.Remove();
            TargetChanged();
            ReselectBaseEntry();
        }

        private void sndNew_Click(object sender, EventArgs e)
        {
            WAVESoundNode s = new WAVESoundNode
            {
                Name = $"[{_targetNode.Children[1].Children.Count}]Audio",
                Parent = _targetNode.Children[1]
            };
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = SupportedFilesHandler.GetCompleteFilter("wav");
                if (ofd.ShowDialog() != DialogResult.Cancel)
                {
                    s.Replace(ofd.FileName);
                    TargetChanged();
                    ReselectBaseEntry();
                }
            }
        }

        private void sndDelete_Click(object sender, EventArgs e)
        {
            ResourceNode entry = soundsListBox.Items[soundsListBox.SelectedIndex] as ResourceNode;
            entry.Remove();
            TargetChanged();
            ReselectBaseEntry();
        }

        private void ReselectBaseEntry()
        {
            if (_baseEntry != null)
            {
                label1.Text = _baseEntry.Name;
                dataListBox.Items.Clear();
                dataListBox.Items.AddRange(_baseEntry.Children.ToArray());
                if (dataListBox.Items.Count > 0)
                {
                    dataListBox.SelectedIndex = 0;
                }
            }
        }

        private void nullEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
            {
                return;
            }

            RBNKNullNode n = new RBNKNullNode();
            ResourceNode r = _baseEntry == null ? TargetNode : (ResourceNode) _baseEntry;
            n.Parent = r;
        }

        private void instanceParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
            {
                return;
            }

            RBNKDataInstParamNode n = new RBNKDataInstParamNode();
            ResourceNode r = _baseEntry == null ? TargetNode : (ResourceNode) _baseEntry;
            n.Parent = r;
        }

        private void rangeGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
            {
                return;
            }

            RBNKDataRangeTableNode n = new RBNKDataRangeTableNode();
            ResourceNode r = _baseEntry == null ? TargetNode : (ResourceNode) _baseEntry;
            n.Parent = r;
        }

        private void indexGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
            {
                return;
            }

            RBNKDataIndexTableNode n = new RBNKDataIndexTableNode();
            ResourceNode r = _baseEntry == null ? TargetNode : (ResourceNode) _baseEntry;
            n.Parent = r;
        }
    }
}
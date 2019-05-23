using BrawlLib.SSBB.ResourceNodes;
using System.Audio;
using BrawlLib;
using BrawlLib.SSBB;

namespace System.Windows.Forms
{
    public class EditRSARFileDialog : Form
    {
        public EditRSARFileDialog() { InitializeComponent(); }

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
        public RSARFileNode TargetNode { get { return _targetNode; } set { _targetNode = value; TargetChanged(); } }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            audioPlaybackPanel1.TargetSource = null;
            base.OnClosing(e);
        }

        public DialogResult ShowDialog(IWin32Window owner, RSARFileNode node)
        {
            TargetNode = node;
            TargetNode.UpdateControl += OnUpdateCurrControl;
            return base.ShowDialog();
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            audioPlaybackPanel1.TargetSource = null;
            TargetNode.UpdateControl -= OnUpdateCurrControl;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        internal protected virtual void OnUpdateCurrControl(object sender, EventArgs e)
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
            this.components = new System.ComponentModel.Container();
            this.btnOkay = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataListBox = new System.Windows.Forms.ListBox();
            this.ctxData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dataReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.dataExport = new System.Windows.Forms.ToolStripMenuItem();
            this.dataNew = new System.Windows.Forms.ToolStripMenuItem();
            this.dataNewNullEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.dataNewInstParam = new System.Windows.Forms.ToolStripMenuItem();
            this.dataNewRange = new System.Windows.Forms.ToolStripMenuItem();
            this.dataNewIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.dataDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.soundsListBox = new System.Windows.Forms.ListBox();
            this.ctxSounds = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sndReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.sndExport = new System.Windows.Forms.ToolStripMenuItem();
            this.sndNew = new System.Windows.Forms.ToolStripMenuItem();
            this.sndDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.audioPlaybackPanel1 = new System.Windows.Forms.AudioPlaybackPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.ctxData.SuspendLayout();
            this.panel4.SuspendLayout();
            this.ctxSounds.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(369, 3);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "&Done";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOkay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 320);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 31);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.splitter2);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.MinimumSize = new System.Drawing.Size(54, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(107, 320);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataListBox);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.MinimumSize = new System.Drawing.Size(0, 15);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(107, 160);
            this.panel3.TabIndex = 1;
            // 
            // dataListBox
            // 
            this.dataListBox.ContextMenuStrip = this.ctxData;
            this.dataListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataListBox.FormattingEnabled = true;
            this.dataListBox.IntegralHeight = false;
            this.dataListBox.ItemHeight = 16;
            this.dataListBox.Location = new System.Drawing.Point(0, 21);
            this.dataListBox.Name = "dataListBox";
            this.dataListBox.Size = new System.Drawing.Size(107, 139);
            this.dataListBox.TabIndex = 3;
            this.dataListBox.SelectedIndexChanged += new System.EventHandler(this.dataListBox_SelectedIndexChanged);
            // 
            // ctxData
            // 
            this.ctxData.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataReplace,
            this.dataExport,
            this.dataNew,
            this.dataDelete});
            this.ctxData.Name = "contextMenuStrip1";
            this.ctxData.Size = new System.Drawing.Size(182, 136);
            // 
            // dataReplace
            // 
            this.dataReplace.Name = "dataReplace";
            this.dataReplace.Size = new System.Drawing.Size(181, 26);
            this.dataReplace.Text = "Replace";
            this.dataReplace.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // dataExport
            // 
            this.dataExport.Name = "dataExport";
            this.dataExport.Size = new System.Drawing.Size(181, 26);
            this.dataExport.Text = "Export";
            this.dataExport.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // dataNew
            // 
            this.dataNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataNewNullEntry,
            this.dataNewInstParam,
            this.dataNewRange,
            this.dataNewIndex});
            this.dataNew.Name = "dataNew";
            this.dataNew.Size = new System.Drawing.Size(181, 26);
            this.dataNew.Text = "New";
            this.dataNew.Click += new System.EventHandler(this.dataNew_Click);
            // 
            // dataNewNullEntry
            // 
            this.dataNewNullEntry.Name = "dataNewNullEntry";
            this.dataNewNullEntry.Size = new System.Drawing.Size(215, 26);
            this.dataNewNullEntry.Text = "Null Entry";
            this.dataNewNullEntry.Click += new System.EventHandler(this.nullEntryToolStripMenuItem1_Click);
            // 
            // dataNewInstParam
            // 
            this.dataNewInstParam.Name = "dataNewInstParam";
            this.dataNewInstParam.Size = new System.Drawing.Size(215, 26);
            this.dataNewInstParam.Text = "Instance Parameters";
            this.dataNewInstParam.Click += new System.EventHandler(this.instanceParametersToolStripMenuItem_Click);
            // 
            // dataNewRange
            // 
            this.dataNewRange.Name = "dataNewRange";
            this.dataNewRange.Size = new System.Drawing.Size(215, 26);
            this.dataNewRange.Text = "Range Group";
            this.dataNewRange.Click += new System.EventHandler(this.rangeGroupToolStripMenuItem1_Click);
            // 
            // dataNewIndex
            // 
            this.dataNewIndex.Name = "dataNewIndex";
            this.dataNewIndex.Size = new System.Drawing.Size(215, 26);
            this.dataNewIndex.Text = "Index Group";
            this.dataNewIndex.Click += new System.EventHandler(this.indexGroupToolStripMenuItem1_Click);
            // 
            // dataDelete
            // 
            this.dataDelete.Name = "dataDelete";
            this.dataDelete.Size = new System.Drawing.Size(181, 26);
            this.dataDelete.Text = "Delete";
            this.dataDelete.Click += new System.EventHandler(this.dataDelete_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Data";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 160);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(107, 3);
            this.splitter2.TabIndex = 0;
            this.splitter2.TabStop = false;
            this.splitter2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter2_SplitterMoved);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.soundsListBox);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 163);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.MinimumSize = new System.Drawing.Size(0, 15);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(107, 157);
            this.panel4.TabIndex = 2;
            // 
            // soundsListBox
            // 
            this.soundsListBox.ContextMenuStrip = this.ctxSounds;
            this.soundsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundsListBox.FormattingEnabled = true;
            this.soundsListBox.IntegralHeight = false;
            this.soundsListBox.ItemHeight = 16;
            this.soundsListBox.Location = new System.Drawing.Point(0, 21);
            this.soundsListBox.Name = "soundsListBox";
            this.soundsListBox.Size = new System.Drawing.Size(107, 136);
            this.soundsListBox.TabIndex = 2;
            this.soundsListBox.SelectedIndexChanged += new System.EventHandler(this.soundsListBox_SelectedIndexChanged);
            // 
            // ctxSounds
            // 
            this.ctxSounds.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxSounds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sndReplace,
            this.sndExport,
            this.sndNew,
            this.sndDelete});
            this.ctxSounds.Name = "contextMenuStrip1";
            this.ctxSounds.Size = new System.Drawing.Size(138, 108);
            // 
            // sndReplace
            // 
            this.sndReplace.Name = "sndReplace";
            this.sndReplace.Size = new System.Drawing.Size(137, 26);
            this.sndReplace.Text = "Replace";
            this.sndReplace.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // sndExport
            // 
            this.sndExport.Name = "sndExport";
            this.sndExport.Size = new System.Drawing.Size(137, 26);
            this.sndExport.Text = "Export";
            this.sndExport.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // sndNew
            // 
            this.sndNew.Name = "sndNew";
            this.sndNew.Size = new System.Drawing.Size(137, 26);
            this.sndNew.Text = "New";
            this.sndNew.Click += new System.EventHandler(this.sndNew_Click);
            // 
            // sndDelete
            // 
            this.sndDelete.Name = "sndDelete";
            this.sndDelete.Size = new System.Drawing.Size(137, 26);
            this.sndDelete.Text = "Delete";
            this.sndDelete.Click += new System.EventHandler(this.sndDelete_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sounds";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(107, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 320);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new System.Drawing.Point(110, 24);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(337, 185);
            this.propertyGrid.TabIndex = 5;
            // 
            // audioPlaybackPanel1
            // 
            this.audioPlaybackPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.audioPlaybackPanel1.Location = new System.Drawing.Point(110, 209);
            this.audioPlaybackPanel1.Name = "audioPlaybackPanel1";
            this.audioPlaybackPanel1.Size = new System.Drawing.Size(337, 111);
            this.audioPlaybackPanel1.TabIndex = 6;
            this.audioPlaybackPanel1.TargetStreams = null;
            this.audioPlaybackPanel1.Volume = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(110, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(337, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "View Entries";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EditRSARFileDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(447, 351);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.audioPlaybackPanel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "EditRSARFileDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit RSAR File";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ctxData.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ctxSounds.ResumeLayout(false);
            this.ResumeLayout(false);

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
                if (dataListBox.Items.Count > 0) dataListBox.SelectedIndex = 0;
            }
            else if (TargetNode is RBNKNode || TargetNode is RWSDNode)
            {
                splitter2.Visible = panel4.Visible = audioPlaybackPanel1.Visible = true;

                dataListBox.Items.AddRange(TargetNode.Children[0].Children.ToArray());
                if (dataListBox.Items.Count > 0) 
                    dataListBox.SelectedIndex = 0;

                if (TargetNode.Children.Count > 1)
                    soundsListBox.Items.AddRange(TargetNode.Children[1].Children.ToArray());
                if (soundsListBox.Items.Count > 0) 
                    soundsListBox.SelectedIndex = 0;

                
            }
            button1.Visible = 
            dataNewNullEntry.Visible = 
            dataNewInstParam.Visible =
                dataNewRange.Visible = 
                dataNewIndex.Visible = TargetNode is RBNKNode;

            if (TargetNode != null)
                Text = "Edit RSAR File - " + TargetNode.Name;
            else
                Text = "Edit RSAR File";
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void dataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataListBox.SelectedIndex < 0)
                return;

            ResourceNode r = dataListBox.Items[dataListBox.SelectedIndex] as ResourceNode;
            propertyGrid.SelectedObject = r;
            int w;
            if (TargetNode is RWSDNode)
            {
                w = (r as RWSDDataNode)._part3._waveIndex;
                if (w < soundsListBox.Items.Count)
                    soundsListBox.SelectedIndex = w;
            } 
            else if (TargetNode is RBNKNode && r is RBNKDataInstParamNode)
            {
                w = (r as RBNKDataInstParamNode).hdr._waveIndex;
                if (w < soundsListBox.Items.Count)
                    soundsListBox.SelectedIndex = w;
            }
            button1.Enabled = !(button1.Text != "Back" && r is RBNKDataEntryNode);
        }

        private void soundsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (soundsListBox.SelectedIndex < 0) return;
            ResourceNode r = soundsListBox.Items[soundsListBox.SelectedIndex] as ResourceNode;
            if (r is IAudioSource)
                audioPlaybackPanel1.TargetSource = r as IAudioSource;
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataListBox.SelectedIndex < 0)
                return;

            ResourceNode r = dataListBox.Items[dataListBox.SelectedIndex] as ResourceNode;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = FileFilters.Raw;
                if (dlg.ShowDialog() == DialogResult.OK)
                    r.Replace(dlg.FileName);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataListBox.SelectedIndex < 0)
                return;

            ResourceNode r = dataListBox.Items[dataListBox.SelectedIndex] as ResourceNode;
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.FileName = r.Name;
                dlg.Filter = FileFilters.Raw;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    r.Export(dlg.FileName);
            }
        }

        RBNKEntryNode _baseEntry = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "View Entries")
            {
                button1.Text = "Back";

                if (dataListBox.SelectedIndex < 0)
                    return;

                RBNKEntryNode entry = dataListBox.Items[dataListBox.SelectedIndex] as RBNKEntryNode;
                _baseEntry = entry;
                label1.Text = entry.Name;
                dataListBox.Items.Clear();
                dataListBox.Items.AddRange(entry.Children.ToArray());
                if (dataListBox.Items.Count > 0) dataListBox.SelectedIndex = 0;
            }
            else
            {
                button1.Text = "View Entries";
                dataListBox.Items.Clear();
                dataListBox.Items.AddRange(TargetNode.Children[0].Children.ToArray());
                if (dataListBox.Items.Count > 0) dataListBox.SelectedIndex = 0;
                label1.Text = "Data";
                _baseEntry = null;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (soundsListBox.SelectedIndex < 0)
                return;

            ResourceNode r = soundsListBox.Items[soundsListBox.SelectedIndex] as ResourceNode;
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.FileName = r.Name;
                dlg.Filter = FileFilters.WAV + "|" + FileFilters.Raw;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    r.Export(dlg.FileName);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (soundsListBox.SelectedIndex < 0)
                return;

            ResourceNode r = soundsListBox.Items[soundsListBox.SelectedIndex] as ResourceNode;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = FileFilters.WAV + "|" + FileFilters.Raw;
                if (dlg.ShowDialog() == DialogResult.OK)
                    r.Replace(dlg.FileName);
            }
        }

        private void dataNew_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RWSDNode))
                return;

            RWSDDataNode d = new RWSDDataNode()
            {
                _name = String.Format("[{0}]Data", TargetNode.Children[0].Children.Count)
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
            WAVESoundNode s = new WAVESoundNode();
            s.Name = String.Format("[{0}]Audio", _targetNode.Children[1].Children.Count);
            s.Parent = _targetNode.Children[1];
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
                    dataListBox.SelectedIndex = 0;
            }
        }

        private void nullEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
                return;

            RBNKNullNode n = new RBNKNullNode();
            ResourceNode r = _baseEntry == null ? (ResourceNode)TargetNode : (ResourceNode)_baseEntry;
            n.Parent = r;
        }

        private void instanceParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
                return;

            RBNKDataInstParamNode n = new RBNKDataInstParamNode();
            ResourceNode r = _baseEntry == null ? (ResourceNode)TargetNode : (ResourceNode)_baseEntry;
            n.Parent = r;
        }

        private void rangeGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
                return;

            RBNKDataRangeTableNode n = new RBNKDataRangeTableNode();
            ResourceNode r = _baseEntry == null ? (ResourceNode)TargetNode : (ResourceNode)_baseEntry;
            n.Parent = r;
        }

        private void indexGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!(TargetNode is RBNKNode))
                return;

            RBNKDataIndexTableNode n = new RBNKDataIndexTableNode();
            ResourceNode r = _baseEntry == null ? (ResourceNode)TargetNode : (ResourceNode)_baseEntry;
            n.Parent = r;
        }
    }
}

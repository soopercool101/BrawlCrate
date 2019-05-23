using Be.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.PowerPcAssembly;

namespace System.Windows.Forms
{
    public unsafe partial class SectionEditor : Form
    {
        public ModuleSectionNode _section = null;
        public RelocationManager _manager;

        public static List<SectionEditor> _openedSections = new List<SectionEditor>();

        public SectionEditor(ModuleSectionNode section)
        {
            _startIndex = int.MaxValue;
            _endIndex = int.MinValue;

            InitializeComponent();

            ppcDisassembler1._editor = this;
            ppcDisassembler1.ppcOpCodeEditControl1._canFollowBranch = true;
            ppcDisassembler1.ppcOpCodeEditControl1.OnBranchFollowed += ppcOpCodeEditControl1_OnBranchFollowed;

            if ((_section = section) != null)
            {
                _section._linkedEditor = this;
                _manager = new RelocationManager(_section);
            }
            _openedSections.Add(this);

            Text = String.Format("Module Section Editor - {0}", _section.Name);

            hexBox1.SectionEditor = this;
            chkCodeSection.Checked = _section._isCodeSection;
            chkBSSSection.Checked = _section._isBSSSection;

            if (section.Root is RELNode)
            {
                RELNode r = (RELNode)section.Root;
                if (r.PrologSection == section.Index)
                    _manager._constructorIndex = (int)r._prologOffset / 4;
                if (r.EpilogSection == section.Index)
                    _manager._destructorIndex = (int)r._epilogOffset / 4;
                if (r.UnresolvedSection == section.Index)
                    _manager._unresolvedIndex = (int)r._unresolvedOffset / 4;
                //if (r._nameReloc != null && r._nameReloc._section == section)
                //    _nameReloc = r._nameReloc;
            }

            panel5.Enabled = true;
        }

        void ppcOpCodeEditControl1_OnBranchFollowed()
        {
            //See if the target is already in this REL
            if (TargetBranchOffsetRelocation != null)
                OpenRelocation(TargetBranchOffsetRelocation); //Navigate to it
            else if (SelectedRelocationIndex >= 0)
            {
                //If the target module id isn't this REL nor in the opened rel list, 
                //ask the user to open the file, add it to the list and then navigate to it.

                //if (SelectedRelocationIndex.Command != null)
                //{
                //    if (SelectedRelocationIndex.Command.Command == RELCommandType.SetBranchDestination)
                //    {

                //    }
                //}
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Apply();

            _openedSections.Remove(this);
            if (_section != null)
                _section._linkedEditor = null;

            SelectedRelocationIndex = -1;

            base.OnClosed(e);
        }

        void ByteProvider_LengthChanged(object sender, EventArgs e)
        {
            //UpdateFileSizeStatus();
        }

        private void Init()
        {
            SetByteProvider();
            //UpdateFileSizeStatus();

            //ppcDisassembler1.TargetNode = _section;
        }

        private void SetByteProvider()
        {
            if (hexBox1.ByteProvider != null)
                ((DynamicFileByteProvider)hexBox1.ByteProvider).Dispose();

            hexBox1.ByteProvider = new DynamicFileByteProvider(new UnmanagedMemoryStream((byte*)_section._dataBuffer.Address, _section._dataBuffer.Length, _section._dataBuffer.Length, FileAccess.ReadWrite)) { _supportsInsDel = false };
            //hexBox1.ByteProvider.LengthChanged += ByteProvider_LengthChanged;
            hexBox1.InsertActiveChanged += hexBox1_InsertActiveChanged;
        }

        void hexBox1_InsertActiveChanged(object sender, EventArgs e)
        {
            insertValue.Text = hexBox1.InsertActive ? "Insert" : "Overwrite";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Init();
        }

        void UpdateSelectedBytesStatus()
        {
            if (hexBox1.ByteProvider == null)
                selectedBytesToolStripStatusLabel.Text = String.Empty;
            else
                selectedBytesToolStripStatusLabel.Text = "Selected: 0x" + hexBox1.SelectionLength.ToString("X");
        }

        string GetDisplayBytes(long size)
        {
            const long multi = 1024;
            long kb = multi;
            long mb = kb * multi;
            long gb = mb * multi;
            long tb = gb * multi;

            const string BYTES = "Bytes";
            const string KB = "KB";
            const string MB = "MB";
            const string GB = "GB";
            const string TB = "TB";

            string result;
            if (size < kb)
                result = string.Format("{0} {1}", size, BYTES);
            else if (size < mb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, kb), KB, ConvertBytesDisplay(size));
            else if (size < gb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, mb), MB, ConvertBytesDisplay(size));
            else if (size < tb)
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, gb), GB, ConvertBytesDisplay(size));
            else
                result = string.Format("{0} {1} ({2} Bytes)",
                    ConvertToOneDigit(size, tb), TB, ConvertBytesDisplay(size));

            return result;
        }

        string ConvertBytesDisplay(long size)
        {
            return size.ToString("###,###,###,###,###", CultureInfo.CurrentCulture);
        }

        string ConvertToOneDigit(long size, long quan)
        {
            double quotient = (double)size / (double)quan;
            string result = quotient.ToString("0.#", CultureInfo.CurrentCulture);
            return result;
        }

        private void hexBox1_Copied(object sender, EventArgs e)
        {
            EnableButtons();
        }

        private void hexBox1_CopiedHex(object sender, EventArgs e)
        {
            EnableButtons();
        }

        private void hexBox1_CurrentLineChanged(object sender, EventArgs e)
        {
            //PosChanged();
        }

        private void hexBox1_CurrentPositionInLineChanged(object sender, EventArgs e)
        {
            //PosChanged();
        }

        void PosChanged()
        {
            this.toolStripStatusLabel.Text = string.Format("Ln {0}    Col {1}",
                hexBox1.CurrentLine, hexBox1.CurrentPositionInLine);

            long offset = hexBox1.SelectionStart;
            long t = offset.RoundDown(4);

            _updating = true;

            if (offset < hexBox1.ByteProvider.Length && offset >= 0)
                SelectedRelocationIndex = (int)(offset.RoundDown(4) / 4);
            else
                SelectedRelocationIndex = -1;

            grpValue.Text = "Value @ 0x" + t.ToString("X");
            if (t + 3 < hexBox1.ByteProvider.Length)
            {
                grpValue.Enabled = !_section._isBSSSection;
                byte[] bytes = new byte[]
                {
                    //Read in little endian
                    hexBox1.ByteProvider.ReadByte(t + 3),
                    hexBox1.ByteProvider.ReadByte(t + 2),
                    hexBox1.ByteProvider.ReadByte(t + 1),
                    hexBox1.ByteProvider.ReadByte(t + 0),
                };

                //Reverse byte order to big endian
                txtByte1.Text = bytes[3].ToString("X2");
                txtByte2.Text = bytes[2].ToString("X2");
                txtByte3.Text = bytes[1].ToString("X2");
                txtByte4.Text = bytes[0].ToString("X2");

                //BitConverter converts from little endian
                float f = BitConverter.ToSingle(bytes, 0);
                float z;
                if (float.TryParse(txtFloat.Text, out z))
                {
                    if (z != f)
                        txtFloat.Text = f.ToString();
                }
                else
                    txtFloat.Text = f.ToString();

                int i = BitConverter.ToInt32(bytes, 0);
                int w;
                if (int.TryParse(txtInt.Text, out w))
                {
                    if (w != i)
                    {
                        txtInt.Text = i.ToString();
                        //if (_section.HasCode && ppcDisassembler1.Visible)
                        //    ppcDisassembler1.UpdateRow(SelectedRelocationIndex - _startIndex);
                    }
                }
                else
                    txtInt.Text = i.ToString();

                string bin = ((Bin32)(uint)i).ToString();
                string[] bins = bin.Split(' ');

                txtBin1.Text = bins[0];
                txtBin2.Text = bins[1];
                txtBin3.Text = bins[2];
                txtBin4.Text = bins[3];
                txtBin5.Text = bins[4];
                txtBin6.Text = bins[5];
                txtBin7.Text = bins[6];
                txtBin8.Text = bins[7];
            }
            else
                grpValue.Enabled = false;

            OffsetToolStripStatusLabel.Text = String.Format("Offset: 0x{0}", offset.ToString("X"));

            if (_section.HasCode && ppcDisassembler1.Visible && !ppcDisassembler1._updating)
            {
                int i = SelectedRelocationIndex - _startIndex;
                if (i >= 0 &&
                    i < ppcDisassembler1.grdDisassembler.Rows.Count &&
                    !(ppcDisassembler1.grdDisassembler.SelectedRows.Count > 0 &&
                    ppcDisassembler1.grdDisassembler.SelectedRows[0].Index == i))
                {
                    ppcDisassembler1.grdDisassembler.ClearSelection();
                    ppcDisassembler1.grdDisassembler.Rows[i].Selected = true;
                    ppcDisassembler1.grdDisassembler.FirstDisplayedScrollingRowIndex = i;
                }
            }

            _updating = false;
        }

        public long Position
        {
            get { return hexBox1.SelectionStart; }
            set
            {
                if (hexBox1.SelectionStart == value)
                {
                    if (!_updating)
                        PosChanged();
                }
                else
                    hexBox1.SelectionStart = value;
            }
        }

        public PPCBranch TargetBranch { get { return _targetBranch; } }
        PPCBranch _targetBranch;

        public RelocationTarget TargetBranchOffsetRelocation { get { return _targetBranchOffsetRelocation; } }
        RelocationTarget _targetBranchOffsetRelocation;

        int _selectedRelocationIndex;
        int _startIndex, _endIndex;
        public int SelectedRelocationIndex
        {
            get { return _selectedRelocationIndex; }
            set
            {
                lstLinked.Items.Clear();
                _targetBranch = null;

                _selectedRelocationIndex = value;

                int index = _selectedRelocationIndex;

                lstLinked.Items.Clear();
                var linked = _manager.GetLinked(index);
                if (linked != null)
                    lstLinked.Items.AddRange(linked.ToArray());
                var branched = _manager.GetBranched(index);
                if (branched != null)
                    lstLinked.Items.AddRange(branched.ToArray());

                if (_section.HasCode && ppcDisassembler1.Visible)
                {
                    //Get the method that the cursor lies in and display it
                    if (index < 0 || index > _section._dataBuffer.Length / 4 - 1)
                        ppcDisassembler1.SetTarget(null, 0, null);
                    else if (index < _startIndex || index > _endIndex)
                    {
                        int
                            startIndex = index,
                            endIndex = index;

                        //TODO: Branch to new relocation and continue adding codes until blr/bctr

                        //Backtrack setting the method start index until we hit a branch.
                        //Do not include this branch, as it is not part of the method.
                        while (startIndex > 0 && !(_manager.GetCode(startIndex - 1) is PPCblr))
                            startIndex--;

                        //Now iterate down the code until we hit a branch to set the end.
                        //Include this branch, because it ends the method.
                        while (endIndex < _section._dataBuffer.Length / 4 - 1 && !(_manager.GetCode(endIndex) is PPCblr))
                            endIndex++;

                        _startIndex = startIndex;
                        _endIndex = endIndex;
                        List<PPCOpCode> w = new List<PPCOpCode>();
                        for (int i = startIndex; i <= endIndex; i++)
                            w.Add(_manager.GetCode(i));

                        //ppcDisassembler1._baseOffset = (int)_section.RootOffset;
                        ppcDisassembler1.SetTarget(w, startIndex * 4, _manager);
                    }

                    if (_section.Root is RELNode)
                    {
                        RELNode r = (RELNode)_section.Root;
                        bool u = _updating;
                        _updating = true;
                        chkConstructor.Checked =
                            _selectedRelocationIndex == _manager._constructorIndex && _section.Index == r._prologSection;
                        chkDestructor.Checked =
                            _selectedRelocationIndex == _manager._destructorIndex && _section.Index == r._epilogSection;
                        chkUnresolved.Checked =
                            _selectedRelocationIndex == _manager._unresolvedIndex && _section.Index == r._unresolvedSection;
                        _updating = u;
                    }

                    //Set the target branch code
                    PPCOpCode code = _manager.GetCode(_selectedRelocationIndex);
                    if (code is PPCBranch && !(code is PPCblr))
                    {
                        _targetBranch = (PPCBranch)code;
                        _targetBranchOffsetRelocation = GetBranchOffsetRelocation();
                    }
                    else if (_targetBranchOffsetRelocation != null)
                        _targetBranchOffsetRelocation = null;
                }

                CommandChanged();
            }
        }

        private void CommandChanged()
        {
            //if (!SelectedRelocationIndex.HasValue)
            //{
            //    propertyGrid1.SelectedObject = null;
            //    btnNewCmd.Enabled = false;
            //    btnDelCmd.Enabled = false;
            //    btnOpenTarget.Enabled = false;
            //    btnRemoveWord.Enabled = false;
            //}
            //else 
            if ((propertyGrid1.SelectedObject = _manager.GetCommand(SelectedRelocationIndex)) != null)
            {
                btnNewCmd.Enabled = false;
                btnDelCmd.Enabled = true;
                btnOpenTarget.Enabled = _manager.GetCommand(SelectedRelocationIndex).GetTargetRelocation() != null;
                btnRemoveWord.Enabled = true;
            }
            else
            {
                btnNewCmd.Enabled = true;
                btnDelCmd.Enabled = false;
                btnOpenTarget.Enabled = false;
                btnRemoveWord.Enabled = true;
            }

            hexBox1.Invalidate();
        }

        void EnableButtons()
        {
            copyToolStripMenuItem.Enabled = hexBox1.CanCopy();
            pasteOverwriteToolStripMenuItem.Enabled = hexBox1.CanPaste();
        }

        private void hexBox1_SelectionLengthChanged(object sender, EventArgs e)
        {
            UpdateSelectedBytesStatus();
            EnableButtons();
        }

        private void hexBox1_SelectionStartChanged(object sender, EventArgs e)
        {
            UpdateSelectedBytesStatus();
            EnableButtons();
            PosChanged();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hexBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hexBox1.CopyHex();
        }

        private void pasteInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hexBox1.PasteHex(false);
        }

        private void pasteOverwriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hexBox1.PasteHex(true);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hexBox1.Delete();
        }

        internal FindOptions _findOptions = new FindOptions();
        FormFind _formFind = null;
        FormFind ShowFind()
        {
            if (_formFind == null || _formFind.IsDisposed)
            {
                _formFind = new FormFind(this);
                _formFind.Show(this);
            }
            else
                _formFind.Focus();

            return _formFind;
        }

        FormGoTo _formGoto = new FormGoTo();
        private void gotoToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _formGoto.SetMaxByteIndex(hexBox1.ByteProvider.Length);
            _formGoto.SetDefaultValue(hexBox1.SelectionStart);
            if (_formGoto.ShowDialog() == DialogResult.OK)
            {
                hexBox1.SelectionStart = _formGoto.GetByteIndex();
                hexBox1.Focus();
            }
        }

        private void findToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowFind();
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Find(false);
        }

        private void findPreviousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Find(true);
        }

        public void Find(bool backwards)
        {
            if (!_findOptions.IsValid)
                return;

            long res = backwards ? hexBox1.FindPrev(_findOptions) : hexBox1.FindNext(_findOptions);

            if (res == -1) // -1 = no match
                MessageBox.Show("Unable to find a match.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (res == -2) // -2 = find was aborted
                return;
            else if (!hexBox1.Focused)
                hexBox1.Focus();

            Message m = new Message() { WParam = (IntPtr)(16 | 65536) };
            hexBox1._ki.PreProcessWmKeyUp(ref m);
        }

        private void chkBSSSection_CheckedChanged(object sender, EventArgs e)
        {
            bool isBSS = chkBSSSection.Checked;

            if (_updating)
            {
                menuStrip1.Enabled = !isBSS;
                grpValue.Enabled = !isBSS;
                hexBox1.ReadOnly = isBSS;
                return;
            }

            //There can only be one BSS section
            foreach (ModuleSectionNode s in ((ModuleNode)_section.Root).Sections)
            {
                if (s != _section)
                {
                    if (isBSS)
                    {
                        //Turn off any other BSS section
                        if (s._linkedEditor != null)
                        {
                            s._linkedEditor._updating = true;
                            s._linkedEditor.chkBSSSection.Checked = false;
                            s._linkedEditor._updating = false;
                        }
                        else
                            s._isBSSSection = false;
                    }
                    else
                    {
                        //Make sure there is another BSS section
                        bool found = false;
                        if (s._linkedEditor != null)
                        {
                            if (s._linkedEditor.chkBSSSection.Checked)
                            {
                                found = true;
                                break;
                            }
                        }
                        else
                            if (s._isBSSSection)
                        {
                            found = true;
                            break;
                        }
                        if (!found)
                            return;
                    }
                }
            }

            menuStrip1.Enabled = !isBSS;
            grpValue.Enabled = !isBSS;
            hexBox1.ReadOnly = isBSS;
        }

        private void chkCodeSection_CheckedChanged(object sender, EventArgs e)
        {
            pnlHexEditor.Dock = chkCodeSection.Checked ? DockStyle.Right : DockStyle.Fill;
            pnlFunctions.Visible = ppcDisassembler1.Visible = splitter2.Visible = chkCodeSection.Checked;
            txtFloat.Enabled = txtInt.Enabled = !chkCodeSection.Checked;

            if (chkCodeSection.Checked)
            {
                pnlHexEditor.Width = 500;
                Width = 972;
                displayStringsToolStripMenuItem.Checked = false;
            }
            else
            {
                Width = 840;
                displayStringsToolStripMenuItem.Checked = true;
            }

            //if (ppcDisassembler1.Visible)
            //    foreach (Relocation r in ppcDisassembler1._relocations)
            //        r.Color = Color.FromArgb(255, 255, 200, 200);
            //else
            //    foreach (Relocation r in ppcDisassembler1._relocations)
            //        r.Color = Color.Transparent;

            hexBox1.Invalidate();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog d = new SaveFileDialog())
            {
                d.Filter = "Raw Data (*.*)|*.*";
                d.FileName = _section.Name;
                d.Title = "Choose a place to export to.";
                if (d.ShowDialog() == Forms.DialogResult.OK)
                    _section.Export(d.FileName);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                d.Filter = "Raw Data (*.*)|*.*";
                if (d.ShowDialog() == Forms.DialogResult.OK)
                {
                    _section.Replace(d.FileName);
                    Init();
                }
            }
        }

        private void exportInitializedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (SaveFileDialog d = new SaveFileDialog())
            //{
            //    d.Filter = "Raw Data (*.*)|*.*";
            //    d.FileName = _section.Name;
            //    if (d.ShowDialog() == Forms.DialogResult.OK)
            //        _section.ExportInitialized(d.FileName);
            //}
        }

        private void btnNewCmd_Click(object sender, EventArgs e)
        {
            RelCommand cmd = new RelCommand(
                (_section.Root as ModuleNode).ID,
                _section,
                new RELLink());

            if (hexBox1.SelectionLength > 0)
            {
                for (int i = 0; i < (hexBox1.SelectionLength.RoundUp(4) / 4); i++)
                {
                    _manager.SetCommand(SelectedRelocationIndex + i, cmd);
                }
            }
            else
            {
                _manager.SetCommand(SelectedRelocationIndex, cmd);
            }

            CommandChanged();
            hexBox1.Focus();
            _relocationsChanged = true;
        }

        private void btnDelCmd_Click(object sender, EventArgs e)
        {
            _manager.ClearCommand(SelectedRelocationIndex);

            CommandChanged();
            hexBox1.Focus();
            _relocationsChanged = true;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGrid1.Refresh();
            hexBox1.Invalidate();
            _relocationsChanged = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //pnlHexEditor.Visible = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //ppcDisassembler1.Visible = radioButton2.Checked;
        }

        private void txtSize_TextChanged(object sender, EventArgs e)
        {

        }

        public void OpenRelocation(RelocationTarget target)
        {
            if (target == null)
                return;

            if (target._sectionID != _section.Index)
            {
                foreach (SectionEditor r in _openedSections)
                    if (r._section.Index == target._sectionID)
                    {
                        r.Position = target._index * 4;
                        r.Focus();
                        r.hexBox1.Focus();
                        return;
                    }

                //TODO: use target module id here
                ModuleSectionNode section = ((ModuleNode)_section.Root).Sections[target._sectionID];
                SectionEditor x = new SectionEditor(section);
                x.Show();

                x.Position = target._index * 4;
                x.hexBox1.Focus();
            }
            else
            {
                Position = target._index * 4;
                hexBox1.Focus();
            }
        }

        private void btnOpenTarget_Click(object sender, EventArgs e)
        {
            RelCommand cmd = _manager.GetCommand(SelectedRelocationIndex);
            if (cmd != null)
                OpenRelocation(cmd.GetTargetRelocation());
        }

        private void lstLinked_DoubleClick(object sender, EventArgs e)
        {
            if (lstLinked.SelectedItem != null)
                OpenRelocation(lstLinked.SelectedItem as RelocationTarget);
        }
        private void lstLinked_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                RelocationTarget r = lstLinked.Items[e.Index] as RelocationTarget;
                if (r != null && r.Section != null)
                {
                    PPCOpCode code = r.Section._manager.GetCode(r._index);
                    Color c = code is PPCBranch ? Color.Blue : Color.Red;
                    e.Graphics.DrawString(r.ToString(), lstLinked.Font, new SolidBrush(c), e.Bounds);
                }
            }
            e.DrawFocusRectangle();
        }

        bool _updating = false;
        private void txtFloat_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float f;
            if (float.TryParse(txtFloat.Text, out f))
            {
                long t = Position.RoundDown(4);
                byte* b = (byte*)&f;
                hexBox1.ByteProvider.WriteByte(t + 3, b[0]);
                hexBox1.ByteProvider.WriteByte(t + 2, b[1]);
                hexBox1.ByteProvider.WriteByte(t + 1, b[2]);
                hexBox1.ByteProvider.WriteByte(t + 0, b[3]);
                hexBox1.Invalidate();
                PosChanged();
            }
        }

        private void txtInt_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            int i;
            if (int.TryParse(txtInt.Text, out i))
            {
                long t = Position.RoundDown(4);
                byte* b = (byte*)&i;
                hexBox1.ByteProvider.WriteByte(t + 3, b[0]);
                hexBox1.ByteProvider.WriteByte(t + 2, b[1]);
                hexBox1.ByteProvider.WriteByte(t + 1, b[2]);
                hexBox1.ByteProvider.WriteByte(t + 0, b[3]);
                hexBox1.Invalidate();
                PosChanged();
            }
        }

        private void txtBin1_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            string text = (sender as TextBox).Text;
            if (text.Length != 4)
                return;

            foreach (char s in text)
                if (s != '0' && s != '1')
                    return;

            Bin32 b = Bin32.FromString(txtBin1.Text + " " + txtBin2.Text + " " + txtBin3.Text + " " + txtBin4.Text + " " + txtBin5.Text + " " + txtBin6.Text + " " + txtBin7.Text + " " + txtBin8.Text);
            long t = Position.RoundDown(4);

            byte
                b1 = (byte)((b >> 00) & 0xFF),
                b2 = (byte)((b >> 08) & 0xFF),
                b3 = (byte)((b >> 16) & 0xFF),
                b4 = (byte)((b >> 24) & 0xFF);

            txtByte1.Text = b1.ToString();
            txtByte2.Text = b2.ToString();
            txtByte3.Text = b3.ToString();
            txtByte4.Text = b4.ToString();

            hexBox1.ByteProvider.WriteByte(t + 3, b1);
            hexBox1.ByteProvider.WriteByte(t + 2, b2);
            hexBox1.ByteProvider.WriteByte(t + 1, b3);
            hexBox1.ByteProvider.WriteByte(t + 0, b4);

            hexBox1.Invalidate();
            PosChanged();
        }

        public bool _relocationsChanged = false;
        private void Apply()
        {
            if (hexBox1.ByteProvider == null)
                return;

            try
            {
                if (_section._isBSSSection != chkBSSSection.Checked || _section._isCodeSection != chkCodeSection.Checked)
                {
                    _section._isBSSSection = chkBSSSection.Checked;
                    _section._isCodeSection = chkCodeSection.Checked;
                    _section.SignalPropertyChange();
                }

                if (_section.Root is RELNode)
                {
                    RELNode r = _section.Root as RELNode;

                    if (r.PrologSection == _section.Index && r._prologOffset / 4 != _manager._constructorIndex)
                    {
                        r._prologOffset = (uint)_manager._constructorIndex * 4;
                        r.SignalPropertyChange();
                    }

                    if (r.EpilogSection == _section.Index && r._epilogOffset / 4 != _manager._destructorIndex)
                    {
                        r._epilogOffset = (uint)_manager._destructorIndex * 4;
                        r.SignalPropertyChange();
                    }

                    if (r.UnresolvedSection == _section.Index && r._unresolvedOffset / 4 != _manager._unresolvedIndex)
                    {
                        r._unresolvedOffset = (uint)_manager._unresolvedIndex * 4;
                        r.SignalPropertyChange();
                    }
                }

                DynamicFileByteProvider d = hexBox1.ByteProvider as DynamicFileByteProvider;
                if (!d.HasChanges())
                    return;

                UnsafeBuffer newBuffer = new UnsafeBuffer((int)d.Length);

                int amt = Math.Min(_section._dataBuffer.Length, newBuffer.Length);
                if (amt > 0)
                {
                    Memory.Move(newBuffer.Address, _section._dataBuffer.Address, (uint)amt);
                    if (newBuffer.Length - amt > 0)
                        Memory.Fill(newBuffer.Address + amt, (uint)(newBuffer.Length - amt), 0);
                }

                if (d._stream != null)
                    d._stream.Dispose();
                d._stream = new UnmanagedMemoryStream((byte*)newBuffer.Address, newBuffer.Length, newBuffer.Length, FileAccess.ReadWrite);

                d.ApplyChanges();

                _section._dataBuffer.Dispose();
                _section._dataBuffer = newBuffer;
                _section.SignalPropertyChange();

                //if (_relocationsChanged)
                //{
                //    Relocation[] temp = new Relocation[_relocations.Count];
                //    _relocations.CopyTo(temp);
                //    List<Relocation> temp2 = temp.ToList();

                //    _section._relocations = temp2;
                //    _section._firstCommand = _firstCommand;

                //    ResourceNode a = _section.Root;
                //    if (a != null && a != _section.Root)
                //        a.SignalPropertyChange();
                //}

                hexBox1.Invalidate();
                hexBox1.Focus();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                EnableButtons();
            }
        }

        private void btnInsertWord_Click(object sender, EventArgs e)
        {
            long offset = Position.RoundDown(4);
            long index = offset / 4;

            DynamicFileByteProvider d = hexBox1.ByteProvider as DynamicFileByteProvider;
            d._supportsInsDel = true;
            d.InsertBytes(offset, new byte[] { 0, 0, 0, 0 });
            d._supportsInsDel = false;
            FixRelocations(1, offset);

            PosChanged();
        }

        private void FixRelocations(int amt, long offset)
        {
            foreach (ModuleDataNode s in ((ModuleNode)_section.Root).Sections)
            {
                foreach (RelCommand command in s._manager._commands.Values)
                {
                    if (command.TargetSectionID == _section.Index && command._addend >= offset)
                        command._addend = (uint)((int)command._addend + (amt * 4));
                }
                if (s.Index == _section.Index)
                {
                    var keysToUpdate = new List<KeyValuePair<int, RelCommand>>(_section._manager._commands.Where(x => x.Key >= offset / 4));

                    // TODO fix this mess. Right now we need to clear all commands that need
                    // updating before setting them with their new index or commands seated
                    // right next to eachother will be removed after having just been updated.
                    foreach (var rel in keysToUpdate)
                    {
                        _section._manager.ClearCommand(rel.Key);
                    }
                    foreach (var rel in keysToUpdate)
                    {
                        _section._manager.SetCommand(rel.Key + 1, rel.Value);
                    }
                }
            }
        }

        private void btnRemoveWord_Click(object sender, EventArgs e)
        {
            long offset = Position.RoundDown(4);
            long index = offset / 4;

            DynamicFileByteProvider d = hexBox1.ByteProvider as DynamicFileByteProvider;
            d._supportsInsDel = true;
            d.DeleteBytes(offset, 4);
            d._supportsInsDel = false;

            //_manager.ClearWord(index);
            //_relocations.RemoveAt((int)index);
            //foreach (ModuleDataNode s in ((ModuleNode)_section.Root).Sections)
            //    foreach (Relocation r in s.Relocations)
            //        FixRelocation(r, -1, offset);

            //for (int i = (int)index; i < _relocations.Count; i++)
            //    _relocations[i]._index--;

            PosChanged();
        }

        private void highlightBlr_CheckedChanged(object sender, EventArgs e)
        {
            hexBox1.Invalidate();
        }

        private void displayInitialized_CheckedChanged(object sender, EventArgs e)
        {
            hexBox1.Invalidate();
        }

        private void displayStringsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            hexBox1.StringViewVisible = displayStringsToolStripMenuItem.Checked;
        }

        //public Relocation
        //    _prologReloc = null,
        //    _epilogReloc = null,
        //    _unresReloc = null,
        //    _nameReloc = null;

        private void chkConstructor_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || SelectedRelocationIndex == null)
                return;

            if (SelectedRelocationIndex == _manager._constructorIndex)
            {
                if (!chkConstructor.Checked)
                    _manager._constructorIndex = -1;
            }
            else
            {
                if (chkConstructor.Checked)
                {
                    if (_manager._constructorIndex != -1)
                        _manager._constructorIndex = -1;
                    _manager._constructorIndex = SelectedRelocationIndex;
                }
            }
        }

        private void chkDestructor_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || SelectedRelocationIndex == null)
                return;

            if (SelectedRelocationIndex == _manager._destructorIndex)
            {
                if (!chkDestructor.Checked)
                    _manager._destructorIndex = -1;
            }
            else
            {
                if (chkDestructor.Checked)
                {
                    if (_manager._destructorIndex != -1)
                        _manager._destructorIndex = -1;
                    _manager._destructorIndex = SelectedRelocationIndex;
                }
            }
        }

        private void chkUnresolved_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating || SelectedRelocationIndex == null)
                return;

            if (SelectedRelocationIndex == _manager._unresolvedIndex)
            {
                if (!chkUnresolved.Checked)
                    _manager._unresolvedIndex = -1;
            }
            else
            {
                if (chkUnresolved.Checked)
                {
                    if (_manager._unresolvedIndex != -1)
                        _manager._unresolvedIndex = -1;
                    _manager._unresolvedIndex = SelectedRelocationIndex;
                }
            }
        }

        public RelocationTarget GetBranchOffsetRelocation()
        {
            if (TargetBranch != null &&
                !(TargetBranch is PPCblr) &&
                !(TargetBranch is PPCbctr))
                return new RelocationTarget(_section.ModuleID, _section.Index,
                    !TargetBranch.Absolute ?
                    (SelectedRelocationIndex * 4 + (TargetBranch.DataOffset).RoundDown(4)) / 4 :
                    0 //Absolute from start of section, start of file, or start of memory?
                );

            return null;
        }
    }
}

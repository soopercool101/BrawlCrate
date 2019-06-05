using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using BrawlLib.Properties;
using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public partial class GCTEditor : Form
    {
        private GCTCodeEntryNode _codeEntry;
        public int _codeEntrySavedIndex = -1;

        private bool _isOpen;

        private GCTNode _targetNode;

        private bool _updating;

        public GCTEditor()
        {
            InitializeComponent();
            txtCode.TextChanged += txtCode_TextChanged;
            lstCodes.ItemChecked += lstCodes_ItemChecked;
            lstCodes.SelectedIndexChanged += lstCodes_SelectedIndexChanged;

            var title =
                ((AssemblyTitleAttribute) Attribute.GetCustomAttribute(
                    Assembly.GetEntryAssembly(), typeof(AssemblyTitleAttribute), false)).Title;

            Text = title + " - Code Manager";

            checkBox1.Checked = Settings.Default.SaveGCTWithInfo;
        }

        public GCTNode TargetNode
        {
            get => _targetNode;
            set
            {
                if (_targetNode != null && _targetNode.IsDirty)
                {
                    var res = MessageBox.Show("Save changes?", "Closing", MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.Yes && !Save(_targetNode, checkBox1.Checked) ||
                        res == DialogResult.Cancel) return;
                }

                _updating = true;
                txtCode.Text = "";
                txtName.Text = "";
                txtDesc.Text = "";
                txtID.Text = "";
                txtPath.Text = "";
                textBox1.Text = "";
                lstCodes.Items.Clear();
                if ((_targetNode = value) != null)
                {
                    txtPath.Text = _targetNode._origPath;
                    txtID.Text = _targetNode._name;
                    txtName.Text = _targetNode.GameName;
                    lstCodes.Items.AddRange(_targetNode.Children.Select(s => new ListViewItem
                        {Text = s.Name, Checked = ((GCTCodeEntryNode) s)._enabled, Tag = s}).ToArray());
                    if (_targetNode.Children.Count > 0) lstCodes.Items[0].Selected = true;
                }

                _updating = false;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            _isOpen = true;
            base.OnShown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            TargetNode = null;

            if (TargetNode != null) e.Cancel = true;

            Settings.Default.Save();

            base.OnClosing(e);
        }

        public GCTNode LoadGCT()
        {
            var d = new OpenFileDialog
            {
                Filter = "GCT/Text File|*.gct;*.txt|GCT File|*.gct|Text File|*.txt"
            };
            if (d.ShowDialog(this) != DialogResult.OK) return null;

            return LoadGCT(d.FileName);
        }

        public static GCTNode LoadGCT(string path)
        {
            GCTNode node;

            if (Path.GetExtension(path).ToUpper() == ".TXT")
            {
                node = GCTNode.FromTXT(path);
                return node;
            }

            if ((node = GCTNode.IsParsable(path)) != null) return node;

            return null;
        }

        public bool Save(GCTNode node, bool writeInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(node._origPath)) return SaveAs(node, writeInfo);

                foreach (ListViewItem e in lstCodes.Items) (e.Tag as GCTCodeEntryNode)._enabled = e.Checked;

                node._writeInfo = writeInfo;
                node.Merge();
                node.Export(TargetNode._origPath);
                node.IsDirty = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveAs(GCTNode node, bool writeInfo)
        {
            if (dlgSave.ShowDialog(this) != DialogResult.OK) return false;

            var path = dlgSave.FileName;
            if (!string.IsNullOrEmpty(path))
                try
                {
                    foreach (ListViewItem e in lstCodes.Items) (e.Tag as GCTCodeEntryNode)._enabled = e.Checked;

                    if (Path.GetExtension(path).ToUpper() == ".TXT")
                    {
                        node.ToTXT(path);
                    }
                    else
                    {
                        node._writeInfo = writeInfo;
                        node.Merge();
                        node.Export(node._origPath = path);
                        node.IsDirty = false;
                        txtPath.Text = path;
                    }

                    return true;
                }
                catch
                {
                    return false;
                }

            return false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var node = LoadGCT();
            if (node != null) TargetNode = node;
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveAs(TargetNode, checkBox1.Checked);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save(TargetNode, checkBox1.Checked);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            TargetNode = null;
        }

        private void lstCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (panel1.Enabled = lstCodes.SelectedIndices.Count > 0)
            {
                _updating = true;
                _codeEntry = lstCodes.Items[lstCodes.SelectedIndices[0]].Tag as GCTCodeEntryNode;
                txtDesc.Text = _codeEntry._description;
                txtCode.Text = _codeEntry.DisplayLines;
                textBox1.Text = _codeEntry._name;

                var s = _codeEntry.LinesNoSpaces;
                var i = 0;
                _codeEntrySavedIndex = -1;
                foreach (var c in Settings.Default.Codes)
                {
                    if (c._code == s)
                    {
                        btnAddRemoveCode.Text = "Forget Code";
                        _codeEntrySavedIndex = i;
                        break;
                    }

                    i++;
                }

                if (_codeEntrySavedIndex == -1) btnAddRemoveCode.Text = "Remember Code";

                _updating = false;
            }

            status.Text = "";
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            if (_updating) return;

            var s = txtID.Text;

            var temp = false;
            if (TargetNode == null)
            {
                TargetNode = new GCTNode();
                temp = true;
            }

            TargetNode.Name = txtID.Text = s;
            if (temp) txtID.Select(txtID.Text.Length, 0);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (_updating) return;

            var s = txtName.Text;

            var temp = false;
            if (TargetNode == null)
            {
                TargetNode = new GCTNode();
                temp = true;
            }

            TargetNode.GameName = txtName.Text = s;
            if (temp) txtName.Select(txtName.Text.Length, 0);
        }

        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            if (TargetNode == null || _codeEntry == null || _updating) return;

            _codeEntry._description = txtDesc.Text;

            if (_codeEntrySavedIndex != -1)
                Settings.Default.Codes[_codeEntrySavedIndex]._description = _codeEntry._description;
        }

        private void btnDeleteCode_Click(object sender, EventArgs e)
        {
            if (TargetNode == null || _codeEntry == null || _updating) return;

            _codeEntry.Remove();
            var i = lstCodes.SelectedIndices[0];
            lstCodes.Items[i].Remove();
            i = i.Clamp(-1, TargetNode.Children.Count - 1);
            if (i >= 0 && i < lstCodes.Items.Count) lstCodes.Items[i].Selected = true;
        }

        private void btnNewCode_Click(object sender, EventArgs e)
        {
            if (_updating) return;

            if (TargetNode == null) TargetNode = new GCTNode();

            var n = new GCTCodeEntryNode {_name = "New Code"};
            TargetNode.AddChild(n);
            lstCodes.Items.Add(new ListViewItem {Text = n.Name, Checked = n._enabled, Tag = n});
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (TargetNode == null || _codeEntry == null || _updating) return;

            var i = txtCode.textBox.SelectionStart;
            txtCode.Text = txtCode.Text.ToUpper();
            txtCode.textBox.Select(i, 0);

            if ((txtCode._borderColor = CheckCode(out var lines)) == Color.Green)
            {
                _codeEntry._lines = lines.ToArray();

                if (_codeEntrySavedIndex != -1)
                    Settings.Default.Codes[_codeEntrySavedIndex]._code = _codeEntry.LinesNoSpaces;
            }

            txtCode.Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (TargetNode == null || _codeEntry == null || _updating) return;

            _codeEntry.Name = textBox1.Text;

            if (_codeEntrySavedIndex != -1) Settings.Default.Codes[_codeEntrySavedIndex]._name = _codeEntry._name;
        }

        private Color Error(int x, string text)
        {
            status.Text = string.Format("Problem on line {0}: {1}", x, text);
            return Color.Red;
        }

        public Color CheckCode(out List<GCTCodeLine> lines)
        {
            lines = new List<GCTCodeLine>();

            var code = txtCode.Text;
            var values = code.Split('\n');
            var x = 0;
            foreach (var s in values)
            {
                ++x;
                if (string.IsNullOrEmpty(s)) continue;

                var line = s.StartsWith("* ") ? s.Substring(2) : s;

                var split = line.Split(' ');
                if (split.Length < 1) continue;

                if (split[0].Length != 8) return Error(x, "First value must be 8 characters long.");

                if (!uint.TryParse(split[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var val1))
                    return Error(x, "First value is not a hex integer.");

                if (split.Length < 2 || string.IsNullOrWhiteSpace(split[1])) return Error(x, "Needs two values.");

                if (split[1].Length != 8) return Error(x, "Second value must be 8 characters long.");

                if (!uint.TryParse(split[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var val2))
                    return Error(x, "Second value is not a hex integer.");

                if (split.Length > 2)
                    for (var i = 2; i < split.Length; i++)
                        if (!string.IsNullOrWhiteSpace(split[i]))
                            return Error(x, "Too many values.");

                lines.Add(new GCTCodeLine(val1, val2));
            }

            status.Text = "Code successfully parsed";
            return Color.Green;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating) return;

            _updating = true;
            if (!checkBox1.Checked)
                if (MessageBox.Show(this,
                        "Are you sure you don't want the info written in the GCT?\nOnly codes you have set to remember will be readable.",
                        "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.No)
                    checkBox1.Checked = true;

            _updating = false;

            Settings.Default.SaveGCTWithInfo = checkBox1.Checked;
        }

        private void btnAddRemoveCode_Click(object sender, EventArgs e)
        {
            if (_codeEntrySavedIndex == -1)
            {
                var c = new CodeStorage
                {
                    _code = _codeEntry.LinesNoSpaces,
                    _name = _codeEntry._name,
                    _description = _codeEntry._description
                };

                Settings.Default.Codes.Add(c);
                _codeEntrySavedIndex = Settings.Default.Codes.Count - 1;
                btnAddRemoveCode.Text = "Forget Code";
            }
            else
            {
                Settings.Default.Codes.RemoveAt(_codeEntrySavedIndex);

                _codeEntrySavedIndex = -1;
                btnAddRemoveCode.Text = "Remember Code";
            }
        }

        private void rememberAllCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_targetNode == null) return;

            foreach (GCTCodeEntryNode r in _targetNode.Children)
            {
                var c = r.LinesNoSpaces;
                var found = false;
                foreach (var w in Settings.Default.Codes)
                    if (w._code == c)
                    {
                        w._name = r._name;
                        w._description = r._description;
                        found = true;
                        break;
                    }

                if (!found)
                    Settings.Default.Codes.Add(new CodeStorage
                        {_name = r._name, _description = r._description, _code = r.LinesNoSpaces});
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_targetNode == null) return;

            foreach (GCTCodeEntryNode r in _targetNode.Children)
            {
                var c = r.LinesNoSpaces;
                var i = 0;
                foreach (var w in Settings.Default.Codes)
                {
                    if (w._code == c) break;

                    i++;
                }

                if (i != Settings.Default.Codes.Count) Settings.Default.Codes.RemoveAt(i);
            }
        }

        private void forgetAllCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to forget all codes?", "Are you sure?",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                if (MessageBox.Show("Are you really sure?", "Are you sure?", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                    Settings.Default.Codes.Clear();
        }

        private void saveAllRememberedCodesToGCTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var g = new GCTNode();
            foreach (var w in Settings.Default.Codes)
            {
                var r = new GCTCodeEntryNode
                {
                    LinesNoSpaces = w._code,
                    _name = w._name,
                    _description = w._description
                };

                g.AddChild(r);
            }

            SaveAs(g, true);
        }

        private void loadCodesToRememberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var l = LoadGCT();
            if (l == null) return;

            foreach (GCTCodeEntryNode r in l.Children)
            {
                var c = r.LinesNoSpaces;
                var found = false;
                foreach (var w in Settings.Default.Codes)
                    if (w._code == c)
                    {
                        w._name = r._name;
                        w._description = r._description;
                        found = true;
                        break;
                    }

                if (!found)
                    Settings.Default.Codes.Add(new CodeStorage
                        {_name = r._name, _description = r._description, _code = r.LinesNoSpaces});
            }

            l.Dispose();
        }

        private void loadRememberedCodesAsNewFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = new GCTNode
            {
                _name = "CODE",
                _gameName = "Code List"
            };
            foreach (var w in Settings.Default.Codes)
                node.AddChild(new GCTCodeEntryNode
                    {_name = w._name, _description = w._description, LinesNoSpaces = w._code});

            TargetNode = node;
        }

        private void lstCodes_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!_isOpen || _updating || TargetNode == null) return;

            TargetNode.SignalPropertyChange();
        }
    }
}
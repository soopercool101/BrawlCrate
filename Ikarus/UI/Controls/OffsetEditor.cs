using System.ComponentModel;
using Ikarus.MovesetFile;
using Ikarus.ModelViewer;
using BrawlLib.SSBBTypes;

namespace System.Windows.Forms
{
    public class OffsetEditor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            this.listBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.indexBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.typeBox = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.listBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listBox.FormattingEnabled = true;
            this.listBox.Items.AddRange(new object[] {
            "Actions",
            "Animations",
            "SubRoutines",
            "External",
            "Null"});
            this.listBox.Location = new System.Drawing.Point(49, 3);
            this.listBox.Name = "comboBox1";
            this.listBox.Size = new System.Drawing.Size(121, 21);
            this.listBox.TabIndex = 0;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "List:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Action:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox2
            // 
            this.indexBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.indexBox.FormattingEnabled = true;
            this.indexBox.Location = new System.Drawing.Point(49, 27);
            this.indexBox.Name = "comboBox2";
            this.indexBox.Size = new System.Drawing.Size(121, 21);
            this.indexBox.TabIndex = 2;
            this.indexBox.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Type:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox3
            // 
            this.typeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeBox.FormattingEnabled = true;
            this.typeBox.Location = new System.Drawing.Point(216, 3);
            this.typeBox.Name = "comboBox3";
            this.typeBox.Size = new System.Drawing.Size(74, 21);
            this.typeBox.TabIndex = 4;
            this.typeBox.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Black;
            this.richTextBox1.Location = new System.Drawing.Point(0, 52);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(296, 53);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.listBox);
            this.panel1.Controls.Add(this.indexBox);
            this.panel1.Controls.Add(this.typeBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 52);
            this.panel1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(215, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OffsetEditor
            // 
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.panel1);
            this.Name = "OffsetEditor";
            this.Size = new System.Drawing.Size(296, 105);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        int index = -2;
        private ComboBox listBox;
        private Label label1;
        private Label label2;
        private ComboBox indexBox;
        private Label label3;
        private ComboBox typeBox;
        public RichTextBox richTextBox1;
        private Panel panel1;
        private Button button1;

        private EventOffset _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventOffset TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; TargetChanged(); }
        }

        private void TargetChanged()
        {
            if (_targetNode == null)
                return;

            _updating = true;

            if (((MovesetNode)_targetNode._root).DataCommon != null)
            {
                listBox.Items.Clear();
                listBox.Items.AddRange(new object[] {
                "Actions",
                "Animations",
                "SubRoutines",
                "External",
                "Null",
                "Screen Tints",
                "Flash Overlays"});
            }
            else
            {
                listBox.Items.AddRange(new object[] {
                "Actions",
                "Animations",
                "SubRoutines",
                "External",
                "Null"});
            }

            listBox.SelectedIndex = (int)_targetNode._offsetInfo._list;
            if (_targetNode._offsetInfo._type != TypeValue.None)
                typeBox.SelectedIndex = (int)_targetNode._offsetInfo._type;
            if (_targetNode._offsetInfo._index != -1 && indexBox.Items.Count > _targetNode._offsetInfo._index)
                indexBox.SelectedIndex = _targetNode._offsetInfo._index;

            //if (_targetNode.list < 3)
            //{
            //    _targetNode.action = _targetNode.Root.GetAction(_targetNode.list, _targetNode.type, _targetNode.index);
            //    if (_targetNode.action == null)
            //        _targetNode.action = _targetNode.GetAction();
            //}
            //else
            //    _targetNode.action = null;

            _updating = false;
            UpdateText();
        }

        private bool _updating = false;

        public OffsetEditor() { InitializeComponent(); }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            typeBox.Items.Clear();
            typeBox.Visible = label3.Visible = listBox.SelectedIndex < 2;
            indexBox.Visible = label2.Visible = listBox.SelectedIndex != 4;
            MovesetNode node = ((MovesetNode)_targetNode._root);
            switch (listBox.SelectedIndex)
            {
                case 0:
                    typeBox.Items.Add("Entry");
                    typeBox.Items.Add("Exit");
                    indexBox.DataSource = node.Actions;
                    break;
                case 1:
                    typeBox.Items.Add("Main");
                    typeBox.Items.Add("GFX");
                    typeBox.Items.Add("SFX");
                    typeBox.Items.Add("Other");
                    if (node.Data != null && node.Data.SubActions != null)
                        indexBox.DataSource = node.Data.SubActions;
                    break;
                case 2:
                    indexBox.DataSource = node.SubRoutines;
                    break;
                case 3:
                    indexBox.DataSource = node.ReferenceList;
                    break;
                case 4:

                    break;
                case 5:
                    indexBox.DataSource = node.DataCommon.ScreenTints;
                    break;
                case 6:
                    indexBox.DataSource = node.DataCommon.FlashOverlays;
                    break;

            }
            if (!_updating)
                UpdateText();
        }

        private void UpdateText()
        {
            if (listBox.SelectedIndex == 4)
                richTextBox1.Text = "Go nowhere.";
            else
                richTextBox1.Text = "Go to " + indexBox.Text + (listBox.SelectedIndex >= 2 ? "" : " - " + typeBox.Text) + " in the " + listBox.Text + " list.";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
                UpdateText();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
                UpdateText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_targetNode._script != null)
            {
                _targetNode.Data = -1;
                (_targetNode as EventOffset)._script._actionRefs.Remove(_targetNode);
            }
            if (listBox.SelectedIndex >= 3)
            {
                if (listBox.SelectedIndex == 3 && indexBox.SelectedIndex >= 0 && indexBox.SelectedIndex < _targetNode._root.ReferenceList.Count)
                {
                    if (_targetNode._externalEntry != null)
                    {
                        _targetNode._externalEntry.References.Remove(_targetNode);
                        _targetNode._externalEntry = null;
                    }
                    (_targetNode._externalEntry = _targetNode._root.ReferenceList[indexBox.SelectedIndex]).References.Add(_targetNode);
                    _targetNode._name = _targetNode._externalEntry.Name;
                }
            }
            else
            {
                if (_targetNode._externalEntry != null)
                {
                    _targetNode._externalEntry.References.Remove(_targetNode);
                    _targetNode._externalEntry = null;
                }
            }

            _targetNode._offsetInfo = new ScriptOffsetInfo(
                (ListValue)listBox.SelectedIndex,
                (TypeValue)(listBox.SelectedIndex >= 2 ? -1 : typeBox.SelectedIndex),
                (listBox.SelectedIndex == 4 ? -1 : indexBox.SelectedIndex));


            _targetNode._script = ((MovesetNode)_targetNode._root).GetScript(_targetNode._offsetInfo);
            if (_targetNode._script != null)
                _targetNode.Data = _targetNode._script._offset;
            _targetNode.SignalPropertyChange();
        }
    }
}

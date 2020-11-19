using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Moveset
{
    public class OffsetEditor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            listBox = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            indexBox = new ComboBox();
            label3 = new Label();
            typeBox = new ComboBox();
            richTextBox1 = new RichTextBox();
            panel1 = new Panel();
            button1 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // comboBox1
            // 
            listBox.DropDownStyle = ComboBoxStyle.DropDownList;
            listBox.FormattingEnabled = true;
            listBox.Items.AddRange(new object[]
            {
                "Actions",
                "Animations",
                "SubRoutines",
                "External",
                "Null"
            });
            listBox.Location = new Point(49, 3);
            listBox.Name = "comboBox1";
            listBox.Size = new Size(121, 21);
            listBox.TabIndex = 0;
            listBox.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 6);
            label1.Name = "label1";
            label1.Size = new Size(26, 13);
            label1.TabIndex = 1;
            label1.Text = "List:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 30);
            label2.Name = "label2";
            label2.Size = new Size(40, 13);
            label2.TabIndex = 3;
            label2.Text = "Action:";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // comboBox2
            // 
            indexBox.DropDownStyle = ComboBoxStyle.DropDownList;
            indexBox.FormattingEnabled = true;
            indexBox.Location = new Point(49, 27);
            indexBox.Name = "comboBox2";
            indexBox.Size = new Size(121, 21);
            indexBox.TabIndex = 2;
            indexBox.SelectedIndexChanged += new EventHandler(comboBox2_SelectedIndexChanged);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(176, 6);
            label3.Name = "label3";
            label3.Size = new Size(34, 13);
            label3.TabIndex = 5;
            label3.Text = "Type:";
            label3.TextAlign = ContentAlignment.TopRight;
            // 
            // comboBox3
            // 
            typeBox.DropDownStyle = ComboBoxStyle.DropDownList;
            typeBox.FormattingEnabled = true;
            typeBox.Location = new Point(216, 3);
            typeBox.Name = "comboBox3";
            typeBox.Size = new Size(74, 21);
            typeBox.TabIndex = 4;
            typeBox.SelectedIndexChanged += new EventHandler(comboBox3_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.Control;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Cursor = Cursors.Default;
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point,
                (byte) 0);
            richTextBox1.ForeColor = Color.Black;
            richTextBox1.Location = new Point(0, 52);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(296, 53);
            richTextBox1.TabIndex = 6;
            richTextBox1.Text = "";
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(listBox);
            panel1.Controls.Add(indexBox);
            panel1.Controls.Add(typeBox);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(296, 52);
            panel1.TabIndex = 7;
            // 
            // button1
            // 
            button1.Location = new Point(215, 26);
            button1.Name = "button1";
            button1.Size = new Size(76, 23);
            button1.TabIndex = 6;
            button1.Text = "Okay";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(button1_Click);
            // 
            // OffsetEditor
            // 
            Controls.Add(richTextBox1);
            Controls.Add(panel1);
            Name = "OffsetEditor";
            Size = new Size(296, 105);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox listBox;
        private Label label1;
        private Label label2;
        private ComboBox indexBox;
        private Label label3;
        private ComboBox typeBox;
        public RichTextBox richTextBox1;
        private Panel panel1;
        private Button button1;

        private MoveDefEventOffsetNode _targetNode;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MoveDefEventOffsetNode TargetNode
        {
            get => _targetNode;
            set
            {
                _targetNode = value;
                TargetChanged();
            }
        }

        private void TargetChanged()
        {
            if (_targetNode == null)
            {
                return;
            }

            _updating = true;

            if (_targetNode.Root.dataCommon != null)
            {
                listBox.Items.Clear();
                listBox.Items.AddRange(new object[]
                {
                    "Actions",
                    "Animations",
                    "SubRoutines",
                    "External",
                    "Null",
                    "Screen Tints",
                    "Flash Overlays"
                });
            }
            else
            {
                listBox.Items.AddRange(new object[]
                {
                    "Actions",
                    "Animations",
                    "SubRoutines",
                    "External",
                    "Null"
                });
            }

            listBox.SelectedIndex = _targetNode.list;
            if (_targetNode.type != -1)
            {
                typeBox.SelectedIndex = _targetNode.type;
            }

            if (_targetNode.index != -1 && indexBox.Items.Count > _targetNode.index)
            {
                indexBox.SelectedIndex = _targetNode.index;
            }

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

        private bool _updating;

        public OffsetEditor()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == 0)
            {
                typeBox.Items.Clear();
                typeBox.Items.Add("Entry");
                typeBox.Items.Add("Exit");

                indexBox.Items.Clear();
                indexBox.Items.AddRange(_targetNode.Root._actions.Children.ToArray());
            }

            if (listBox.SelectedIndex == 1)
            {
                typeBox.Items.Clear();
                typeBox.Items.Add("Main");
                typeBox.Items.Add("GFX");
                typeBox.Items.Add("SFX");
                typeBox.Items.Add("Other");

                indexBox.Items.Clear();
                if (TargetNode.Root._subActions != null)
                {
                    indexBox.Items.AddRange(_targetNode.Root._subActions.Children.ToArray());
                }
            }

            if (listBox.SelectedIndex >= 2)
            {
                typeBox.Visible = label3.Visible = false;
            }
            else
            {
                typeBox.Visible = label3.Visible = true;
            }

            if (listBox.SelectedIndex == 4)
            {
                indexBox.Visible = label2.Visible = false;
            }
            else
            {
                indexBox.Visible = label2.Visible = true;
            }

            if (listBox.SelectedIndex == 2)
            {
                indexBox.Items.Clear();
                indexBox.Items.AddRange(_targetNode.Root._subRoutineList.ToArray());
            }

            if (listBox.SelectedIndex == 3)
            {
                indexBox.Items.Clear();
                indexBox.Items.AddRange(_targetNode.Root._externalRefs.ToArray());
            }

            if (listBox.SelectedIndex == 5)
            {
                indexBox.Items.Clear();
                indexBox.Items.AddRange(_targetNode.Root.dataCommon._screenTint.Children.ToArray());
            }

            if (listBox.SelectedIndex == 6)
            {
                indexBox.Items.Clear();
                indexBox.Items.AddRange(_targetNode.Root.dataCommon._flashOverlay.Children.ToArray());
            }

            if (!_updating)
            {
                UpdateText();
            }
        }

        private void UpdateText()
        {
            if (listBox.SelectedIndex == 4)
            {
                richTextBox1.Text = "Go nowhere.";
            }
            else
            {
                richTextBox1.Text = "Go to " + indexBox.Text +
                                    (listBox.SelectedIndex >= 2 ? "" : " - " + typeBox.Text) + " in the " +
                                    listBox.Text + " list.";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                UpdateText();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                UpdateText();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_targetNode.action != null)
            {
                _targetNode._value = -1;
                _targetNode.action._actionRefs.Remove(_targetNode);
            }

            if (listBox.SelectedIndex >= 3)
            {
                if (listBox.SelectedIndex == 3 && indexBox.SelectedIndex >= 0 &&
                    indexBox.SelectedIndex < _targetNode.Root._externalRefs.Count)
                {
                    if (_targetNode._extNode != null)
                    {
                        _targetNode._extNode._refs.Remove(_targetNode);
                        _targetNode._extNode = null;
                    }

                    (_targetNode._extNode =
                            _targetNode.Root._externalRefs[indexBox.SelectedIndex] as MoveDefExternalNode)._refs
                        .Add(
                            _targetNode);
                    _targetNode.Name = _targetNode._extNode.Name;
                }
            }
            else
            {
                if (_targetNode._extNode != null)
                {
                    _targetNode._extNode._refs.Remove(_targetNode);
                    _targetNode._extNode = null;
                }
            }

            _targetNode.list = listBox.SelectedIndex;
            _targetNode.type = listBox.SelectedIndex >= 2 ? -1 : typeBox.SelectedIndex;
            _targetNode.index = listBox.SelectedIndex == 4 ? -1 : indexBox.SelectedIndex;
            _targetNode.action = _targetNode.Root.GetAction(_targetNode.list, _targetNode.type, _targetNode.index);
            if (_targetNode.action != null)
            {
                _targetNode._value = _targetNode.action._offset;
            }

            _targetNode.SignalPropertyChange();
        }
    }
}
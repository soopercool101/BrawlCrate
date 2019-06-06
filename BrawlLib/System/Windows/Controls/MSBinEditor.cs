using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class MSBinEditor : UserControl
    {
        private MSBinNode _node;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MSBinNode CurrentNode
        {
            get => _node;
            set
            {
                if (_node == value)
                {
                    return;
                }

                _node = value;
                InitNode();
            }
        }

        public MSBinEditor()
        {
            InitializeComponent();
            txtEditor.LostFocus += txtEditor_Leave;
        }

        private void InitNode()
        {
            txtEditor.Text = "";
            listBox1.BeginUpdate();

            listBox1.Items.Clear();

            if (_node != null)
            {
                foreach (string s in _node._strings)
                {
                    listBox1.Items.Add(s);
                }

                txtEditor.Enabled = false;
            }
            else
            {
                txtEditor.Enabled = false;
            }

            listBox1.EndUpdate();
        }

        private void Apply()
        {
            if (_node == null)
            {
                return;
            }

            string s1 = txtEditor.Text;
            string s2 = listBox1.SelectedItem as string;
            if (s2 == null || s1 == s2)
            {
                return;
            }

            int index = listBox1.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            _node._strings[index] = s1;
            _node.SignalPropertyChange();
            listBox1.Items[index] = s1;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_node == null)
            {
                return;
            }

            string s = "";
            int index = listBox1.Items.Count;

            _node._strings.Add(s);
            _node.SignalPropertyChange();
            listBox1.Items.Add(s);
            listBox1.SelectedIndex = index;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (_node == null)
            {
                return;
            }

            int index = listBox1.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            _node._strings.RemoveAt(index);
            _node.SignalPropertyChange();
            listBox1.Items.RemoveAt(index);

            if (listBox1.Items.Count == index)
            {
                listBox1.SelectedIndex = index - 1;
            }
            else
            {
                listBox1.SelectedIndex = index;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                txtEditor.Text = "";
                txtEditor.Enabled = false;
            }
            else
            {
                txtEditor.Text = listBox1.SelectedItem as string;
                txtEditor.Enabled = true;
            }
        }

        private void txtEditor_TextChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Invalidate();
        }

        private void txtEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (_node == null)
            {
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                txtEditor.Text = listBox1.SelectedItem as string;
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void txtEditor_Leave(object sender, EventArgs e)
        {
            Apply();
        }

        #region Designer

        private ListBox listBox1;
        private Panel panel2;
        private Button btnRemove;
        private TextBox txtEditor;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Button btnAdd;

        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            panel2 = new Panel();
            btnRemove = new Button();
            btnAdd = new Button();
            txtEditor = new TextBox();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            panel2.SuspendLayout();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.Location = new Drawing.Point(0, 0);
            listBox1.Margin = new Padding(0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Drawing.Size(256, 85);
            listBox1.TabIndex = 1;
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            // 
            // panel2
            // 
            panel2.Controls.Add(btnRemove);
            panel2.Controls.Add(btnAdd);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Drawing.Point(256, 0);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Drawing.Size(29, 85);
            panel2.TabIndex = 2;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Drawing.Point(3, 32);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Drawing.Size(23, 23);
            btnRemove.TabIndex = 4;
            btnRemove.Text = "-";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += new EventHandler(btnRemove_Click);
            // 
            // btnAdd
            // 
            btnAdd.Location = new Drawing.Point(3, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Drawing.Size(23, 23);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += new EventHandler(btnAdd_Click);
            // 
            // txtEditor
            // 
            txtEditor.Dock = DockStyle.Fill;
            txtEditor.Font = new Drawing.Font("Courier New", 8.25F, Drawing.FontStyle.Regular,
                Drawing.GraphicsUnit.Point, 0);
            txtEditor.Location = new Drawing.Point(0, 0);
            txtEditor.Margin = new Padding(0);
            txtEditor.Multiline = true;
            txtEditor.Name = "txtEditor";
            txtEditor.ScrollBars = ScrollBars.Both;
            txtEditor.Size = new Drawing.Size(285, 122);
            txtEditor.TabIndex = 3;
            txtEditor.TextChanged += new EventHandler(txtEditor_TextChanged);
            txtEditor.KeyDown += new KeyEventHandler(txtEditor_KeyDown);
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(txtEditor);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Paint += new PaintEventHandler(splitContainer1_Panel2_Paint);
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Size = new Drawing.Size(285, 122);
            splitContainer1.SplitterDistance = 74;
            splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(listBox1);
            splitContainer2.Panel1.Controls.Add(panel2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(splitContainer1);
            splitContainer2.Size = new Drawing.Size(285, 211);
            splitContainer2.SplitterDistance = 85;
            splitContainer2.TabIndex = 6;
            // 
            // MSBinEditor
            // 
            Controls.Add(splitContainer2);
            Name = "MSBinEditor";
            Size = new Drawing.Size(285, 211);
            panel2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}
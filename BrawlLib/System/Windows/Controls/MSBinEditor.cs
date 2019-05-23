using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class MSBinEditor : UserControl
    {
        private MSBinNode _node;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MSBinNode CurrentNode
        {
            get { return _node; }
            set
            {
                if (_node == value)
                    return;

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
                    listBox1.Items.Add(s);
                txtEditor.Enabled = false;
            }
            else
                txtEditor.Enabled = false;

            listBox1.EndUpdate();
        }

        private void Apply()
        {
            if (_node == null)
                return;

            string s1 = txtEditor.Text;
            string s2 = listBox1.SelectedItem as string;
            if ((s2 == null) || (s1 == s2))
                return;

            int index = listBox1.SelectedIndex;
            if (index < 0)
                return;

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
                return;

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
                return;

            int index = listBox1.SelectedIndex;
            if (index < 0)
                return;

            _node._strings.RemoveAt(index);
            _node.SignalPropertyChange();
            listBox1.Items.RemoveAt(index);

            if (listBox1.Items.Count == index)
                listBox1.SelectedIndex = index - 1;
            else
                listBox1.SelectedIndex = index;
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
                return;

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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtEditor = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Margin = new System.Windows.Forms.Padding(0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(256, 85);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(256, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(29, 85);
            this.panel2.TabIndex = 2;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(3, 32);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtEditor
            // 
            this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditor.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEditor.Location = new System.Drawing.Point(0, 0);
            this.txtEditor.Margin = new System.Windows.Forms.Padding(0);
            this.txtEditor.Multiline = true;
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEditor.Size = new System.Drawing.Size(285, 122);
            this.txtEditor.TabIndex = 3;
            this.txtEditor.TextChanged += new System.EventHandler(this.txtEditor_TextChanged);
            this.txtEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEditor_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(285, 122);
            this.splitContainer1.SplitterDistance = 74;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBox1);
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(285, 211);
            this.splitContainer2.SplitterDistance = 85;
            this.splitContainer2.TabIndex = 6;
            // 
            // MSBinEditor
            // 
            this.Controls.Add(this.splitContainer2);
            this.Name = "MSBinEditor";
            this.Size = new System.Drawing.Size(285, 211);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

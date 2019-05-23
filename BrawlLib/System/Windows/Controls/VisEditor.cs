using System.ComponentModel;
using System.Drawing;
using BrawlLib.SSBB;

namespace System.Windows.Forms
{
    public class VisEditor : UserControl
    {
        #region Designer

        public ListBox listBox1;
        private Button btnAll;
        private Button btnInvert;
        private Button btnToggle;
        private Button btnSet;
        private Button btnClear;
        private Panel panel1;

        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnToggle = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnInvert = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 10;
            this.listBox1.Location = new System.Drawing.Point(0, 20);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(310, 264);
            this.listBox1.TabIndex = 0;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnToggle);
            this.panel1.Controls.Add(this.btnSet);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnInvert);
            this.panel1.Controls.Add(this.btnAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 20);
            this.panel1.TabIndex = 1;
            // 
            // btnToggle
            // 
            this.btnToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggle.Location = new System.Drawing.Point(257, 0);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(50, 20);
            this.btnToggle.TabIndex = 5;
            this.btnToggle.Text = "&Toggle";
            this.btnToggle.UseVisualStyleBackColor = true;
            this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSet.Location = new System.Drawing.Point(206, 0);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(50, 20);
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "&Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(155, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(50, 20);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnInvert
            // 
            this.btnInvert.Location = new System.Drawing.Point(64, 0);
            this.btnInvert.Name = "btnInvert";
            this.btnInvert.Size = new System.Drawing.Size(50, 20);
            this.btnInvert.TabIndex = 2;
            this.btnInvert.Text = "&Invert";
            this.btnInvert.UseVisualStyleBackColor = true;
            this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(3, 0);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(60, 20);
            this.btnAll.TabIndex = 1;
            this.btnAll.Text = "Select &All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // VisEditor
            // 
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel1);
            this.Name = "VisEditor";
            this.Size = new System.Drawing.Size(310, 284);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //public VIS0Editor _mainWindow;

        public EventHandler EntryChanged;
        public EventHandler IndexChanged;

        private IBoolArraySource _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoolArraySource TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; TargetChanged(); }
        }

        public VisEditor() { InitializeComponent(); }
        
        private void TargetChanged()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            if (_targetNode != null)
                for (int i = 0; i < _targetNode.EntryCount; i++)
                    listBox1.Items.Add(_targetNode.GetEntry(i));

            listBox1.EndUpdate();
        }

        private void Toggle()
        {
            listBox1.BeginUpdate();

            int[] indices = new int[listBox1.SelectedIndices.Count];
            listBox1.SelectedIndices.CopyTo(indices, 0);
            foreach (int i in indices)
            {
                bool val = !(bool)listBox1.Items[i];
                listBox1.Items[i] = val;
                _targetNode.SetEntry(i, val);
            }
            foreach (int i in indices)
                listBox1.SelectedIndices.Add(i);

            listBox1.EndUpdate();

            if (EntryChanged != null)
                EntryChanged(this, null);
        }
        private void Clear()
        {
            listBox1.BeginUpdate();

            int[] indices = new int[listBox1.SelectedIndices.Count];
            listBox1.SelectedIndices.CopyTo(indices, 0);
            foreach (int i in indices)
            {
                listBox1.Items[i] = false;
                _targetNode.SetEntry(i, false);
            }
            foreach (int i in indices)
                listBox1.SelectedIndices.Add(i);

            listBox1.EndUpdate();

            if (EntryChanged != null)
                EntryChanged(this, null);
        }
        private void Set()
        {
            listBox1.BeginUpdate();

            int[] indices = new int[listBox1.SelectedIndices.Count];
            listBox1.SelectedIndices.CopyTo(indices, 0);
            foreach (int i in indices)
            {
                listBox1.Items[i] = true;
                _targetNode.SetEntry(i, true);
            }
            foreach (int i in indices)
                listBox1.SelectedIndices.Add(i);

            listBox1.EndUpdate();

            if (EntryChanged != null)
                EntryChanged(this, null);
        }
        private void SelectAll()
        {
            listBox1.BeginUpdate();
            _updating = true;
            for (int i = 0; i < listBox1.Items.Count; i++)
                listBox1.SelectedIndices.Add(i);
            _updating = false;
            listBox1.EndUpdate();
        }
        private void SelectInverse()
        {
            listBox1.BeginUpdate();
            _updating = true;
            int x;
            int count = listBox1.SelectedIndices.Count;
            int[] indices = new int[count];

            listBox1.SelectedIndices.CopyTo(indices, 0);
            listBox1.SelectedIndices.Clear();

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                for (x = 0; x < count; x++)
                    if (indices[x] == i)
                        break;

                if (x >= count)
                    listBox1.SelectedIndices.Add(i);
            }
            _updating = false;
            listBox1.EndUpdate();
        }

        private static Font _renderFont = new Font(FontFamily.GenericMonospace, 9.0f);
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            int index = e.Index;

            g.FillRectangle(Brushes.White, r);
            if (index >= 0)
            {
                if ((e.State & DrawItemState.Selected) != 0)
                    g.FillRectangle(Brushes.LightBlue, r.X, r.Y, 210, r.Height);

                g.DrawString(String.Format(" [{0:d2}]", index), _renderFont, Brushes.Black, 4.0f, e.Bounds.Y - 4);

                r.X += 100;
                r.Width = 30;

                if ((bool)listBox1.Items[index])
                {
                    g.FillRectangle(Brushes.Gray, r);
                    g.DrawString("✔", new Font("", 7), Brushes.Black, r.X + 9, r.Y - 1);
                }

                g.DrawRectangle(Pens.Black, r);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) Toggle(); }
        private void listBox1_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) Toggle(); }
        private void btnAll_Click(object sender, EventArgs e) { SelectAll(); }
        private void btnInvert_Click(object sender, EventArgs e) { SelectInverse(); }
        private void btnClear_Click(object sender, EventArgs e) { Clear(); }
        private void btnSet_Click(object sender, EventArgs e) { Set(); }
        private void btnToggle_Click(object sender, EventArgs e) { Toggle(); }

        public bool _updating = false;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            if (IndexChanged != null)
                IndexChanged(this, null);
            //if (_mainWindow != null && !_updating)
            //    _mainWindow._mainWindow.SetFrame(listBox1.SelectedIndex);
        }
    }
}

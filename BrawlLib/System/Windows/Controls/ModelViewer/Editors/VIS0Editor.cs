using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;

namespace System.Windows.Forms
{
    public class VIS0Editor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(202, 47);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // VIS0Editor
            // 
            this.Controls.Add(this.listBox1);
            this.MinimumSize = new System.Drawing.Size(210, 55);
            this.Name = "VIS0Editor";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(210, 55);
            this.ResumeLayout(false);

        }

        #endregion

        public ListBox listBox1;

        public ModelEditorBase _mainWindow;

        public VIS0Editor() { InitializeComponent(); }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0Node SelectedAnimation
        {
            get { return _mainWindow.SelectedVIS0; }
            set { _mainWindow.SelectedVIS0 = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0EntryNode TargetVisEntry { get { return _mainWindow.TargetVisEntry; } set { _mainWindow.TargetVisEntry = value; } }
        
        public void AnimationChanged()
        {
            listBox1.Items.Clear();
            listBox1.BeginUpdate();
            if (_mainWindow.SelectedVIS0 != null)
                foreach (VIS0EntryNode n in _mainWindow.SelectedVIS0.Children)
                    listBox1.Items.Add(n);

            listBox1.EndUpdate();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
                TargetVisEntry = listBox1.Items[listBox1.SelectedIndex] as VIS0EntryNode;
            //if (_mainWindow.CurrentFrame > 0 && _mainWindow.CurrentFrame < _mainWindow.KeyframePanel.visEditor.listBox1.Items.Count)
            //    _mainWindow.KeyframePanel.visEditor.listBox1.SelectedIndex = _mainWindow.CurrentFrame - 1;
        }

        public void UpdateEntry()
        {
            _mainWindow.KeyframePanel.visEditor.listBox1.BeginUpdate();
            _mainWindow.KeyframePanel.visEditor.listBox1.Items.Clear();

            if (_mainWindow.KeyframePanel.visEditor.TargetNode != null && _mainWindow.KeyframePanel.visEditor.TargetNode.EntryCount > -1)
                for (int i = 0; i < _mainWindow.KeyframePanel.visEditor.TargetNode.EntryCount; i++)
                    _mainWindow.KeyframePanel.visEditor.listBox1.Items.Add(_mainWindow.KeyframePanel.visEditor.TargetNode.GetEntry(i));

            _mainWindow.KeyframePanel.visEditor.listBox1.EndUpdate();
        }

        public void EntryChanged()
        {
            _mainWindow.ApplyVIS0ToInterface();
        }
    }
}
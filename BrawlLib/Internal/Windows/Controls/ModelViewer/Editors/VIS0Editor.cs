using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
{
    public class VIS0Editor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.Location = new System.Drawing.Point(4, 4);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(202, 47);
            listBox1.TabIndex = 3;
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            // 
            // VIS0Editor
            // 
            Controls.Add(listBox1);
            MinimumSize = new System.Drawing.Size(210, 55);
            Name = "VIS0Editor";
            Padding = new Padding(4);
            Size = new System.Drawing.Size(210, 55);
            ResumeLayout(false);
        }

        #endregion

        public ListBox listBox1;

        public ModelEditorBase _mainWindow;

        public VIS0Editor()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0Node SelectedAnimation
        {
            get => _mainWindow.SelectedVIS0;
            set => _mainWindow.SelectedVIS0 = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VIS0EntryNode TargetVisEntry
        {
            get => _mainWindow.TargetVisEntry;
            set => _mainWindow.TargetVisEntry = value;
        }

        public void AnimationChanged()
        {
            listBox1.Items.Clear();
            listBox1.BeginUpdate();
            if (_mainWindow.SelectedVIS0 != null)
            {
                foreach (VIS0EntryNode n in _mainWindow.SelectedVIS0.Children)
                {
                    listBox1.Items.Add(n);
                }
            }

            listBox1.EndUpdate();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < listBox1.Items.Count)
            {
                TargetVisEntry = listBox1.Items[listBox1.SelectedIndex] as VIS0EntryNode;
            }

            //if (_mainWindow.CurrentFrame > 0 && _mainWindow.CurrentFrame < _mainWindow.KeyframePanel.visEditor.listBox1.Items.Count)
            //    _mainWindow.KeyframePanel.visEditor.listBox1.SelectedIndex = _mainWindow.CurrentFrame - 1;
        }

        public void UpdateEntry()
        {
            _mainWindow.KeyframePanel.visEditor.listBox1.BeginUpdate();
            _mainWindow.KeyframePanel.visEditor.listBox1.Items.Clear();

            if (_mainWindow.KeyframePanel.visEditor.TargetNode != null &&
                _mainWindow.KeyframePanel.visEditor.TargetNode.EntryCount > -1)
            {
                for (int i = 0; i < _mainWindow.KeyframePanel.visEditor.TargetNode.EntryCount; i++)
                {
                    _mainWindow.KeyframePanel.visEditor.listBox1.Items.Add(
                        _mainWindow.KeyframePanel.visEditor.TargetNode.GetEntry(i));
                }
            }

            _mainWindow.KeyframePanel.visEditor.listBox1.EndUpdate();
        }

        public void EntryChanged()
        {
            _mainWindow.ApplyVIS0ToInterface();
        }
    }
}
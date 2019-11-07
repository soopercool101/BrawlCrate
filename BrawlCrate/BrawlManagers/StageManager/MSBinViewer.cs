using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public class MSBinViewer : UserControl
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

        public MSBinViewer()
        {
            InitializeComponent();
        }

        private void InitNode()
        {
            txtEditor.Text = "";

            if (_node != null)
            {
                foreach (string s in _node._strings)
                {
                    txtEditor.Text += s + "\r\n";
                }
            }
        }

        #region Designer

        private TextBox txtEditor;

        private void InitializeComponent()
        {
            txtEditor = new TextBox();
            SuspendLayout();
            // 
            // txtEditor
            // 
            txtEditor.Dock = DockStyle.Fill;
            txtEditor.Location = new Point(0, 0);
            txtEditor.Margin = new Padding(0);
            txtEditor.Multiline = true;
            txtEditor.Name = "txtEditor";
            txtEditor.ReadOnly = true;
            txtEditor.ScrollBars = ScrollBars.Both;
            txtEditor.Size = new Size(285, 211);
            txtEditor.TabIndex = 3;
            // 
            // MSBinViewer
            // 
            Controls.Add(txtEditor);
            Name = "MSBinViewer";
            Size = new Size(285, 211);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
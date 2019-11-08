using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
{
    public class CLR0Editor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            label1 = new Label();
            lstTarget = new ComboBox();
            listBox1 = new ListBox();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new System.Drawing.Point(-1, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 20);
            label1.TabIndex = 1;
            label1.Text = "Target:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstTarget
            // 
            lstTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            lstTarget.FormattingEnabled = true;
            lstTarget.Items.AddRange(new object[]
            {
                "Color0",
                "Color1",
                "Ambient0",
                "Ambient1",
                "TevColorReg0",
                "TevColorReg1",
                "TevColorReg2",
                "TevKonstReg0",
                "TevKonstReg1",
                "TevKonstReg2",
                "TevKonstReg3"
            });
            lstTarget.Location = new System.Drawing.Point(-1, 18);
            lstTarget.Name = "lstTarget";
            lstTarget.Size = new System.Drawing.Size(100, 21);
            lstTarget.TabIndex = 2;
            lstTarget.SelectedIndexChanged += new EventHandler(lstTarget_SelectedIndexChanged);
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.Location = new System.Drawing.Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(156, 74);
            listBox1.TabIndex = 4;
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(lstTarget);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new System.Drawing.Point(156, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(100, 74);
            panel1.TabIndex = 5;
            // 
            // CLR0Editor
            // 
            Controls.Add(listBox1);
            Controls.Add(panel1);
            MinimumSize = new System.Drawing.Size(256, 74);
            Name = "CLR0Editor";
            Size = new System.Drawing.Size(256, 74);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public ModelEditorBase _mainWindow;

        private Label label1;
        private ListBox listBox1;
        private Panel panel1;
        public ComboBox lstTarget;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef
        {
            get => _mainWindow.TargetTexRef;
            set => _mainWindow.TargetTexRef = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get => _mainWindow.CurrentFrame;
            set => _mainWindow.CurrentFrame = value;
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
        public CLR0Node SelectedAnimation
        {
            get => _mainWindow.SelectedCLR0;
            set => _mainWindow.SelectedCLR0 = value;
        }

        public void AnimationChanged()
        {
            listBox1.Items.Clear();
            listBox1.BeginUpdate();
            if (_mainWindow.SelectedCLR0 != null)
            {
                foreach (CLR0MaterialNode n in _mainWindow.SelectedCLR0.Children)
                {
                    listBox1.Items.Add(n);
                }

                Enabled = true;
            }
            else
            {
                Enabled = false;
            }

            listBox1.EndUpdate();
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }

            lstTarget.SelectedIndex = 0;
        }

        public CLR0MaterialNode _mat;
        public CLR0MaterialEntryNode _entry;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _mat = listBox1.Items[listBox1.SelectedIndex] as CLR0MaterialNode;
            if (_mat.Children.Count > 0)
            {
                lstTarget.SelectedIndex = (int) ((CLR0MaterialEntryNode) _mat.Children[0]).Target;
            }
            else
            {
                lstTarget.SelectedIndex = 0;
            }

            lstTarget_SelectedIndexChanged(null, null);
        }

        public CLR0Editor()
        {
            InitializeComponent();
        }

        private void lstTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_mat == null)
            {
                if (_mainWindow.KeyframePanel != null)
                {
                    _mainWindow.KeyframePanel.chkEnabled.Checked = false;
                    _mainWindow.KeyframePanel.chkConstant.Checked = false;
                }

                return;
            }

            _entry = _mat.FindChild(lstTarget.SelectedItem as string, false) as CLR0MaterialEntryNode;

            if (_mainWindow.KeyframePanel != null)
            {
                _mainWindow.KeyframePanel.chkEnabled.Checked = _entry != null;
                _mainWindow.KeyframePanel.chkConstant.Checked = _entry != null ? _entry.Constant : false;
                _mainWindow.KeyframePanel.TargetSequence = _entry;
            }
        }
    }
}
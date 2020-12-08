using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
{
    public class PAT0Editor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            label1 = new Label();
            texBox = new StringInputComboBox();
            pltBox = new StringInputComboBox();
            label2 = new Label();
            grpEdit = new GroupBox();
            btnPaste = new Button();
            btnCopy = new Button();
            btnCut = new Button();
            grpEdit.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 23);
            label1.Name = "label1";
            label1.Size = new Size(46, 13);
            label1.TabIndex = 1;
            label1.Text = "Texture:";
            // 
            // texBox
            // 
            texBox.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                             | AnchorStyles.Right;
            texBox.FormattingEnabled = true;
            texBox.Location = new Point(108, 20);
            texBox.Name = "texBox";
            texBox.Size = new Size(288, 21);
            texBox.TabIndex = 2;
            texBox.ValueChanged += new EventHandler(TexChanged);
            // 
            // pltBox
            // 
            pltBox.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                             | AnchorStyles.Right;
            pltBox.BackColor = SystemColors.Window;
            pltBox.FormattingEnabled = true;
            pltBox.Location = new Point(108, 47);
            pltBox.Name = "pltBox";
            pltBox.Size = new Size(288, 21);
            pltBox.TabIndex = 4;
            pltBox.ValueChanged += new EventHandler(TexChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(59, 50);
            label2.Name = "label2";
            label2.Size = new Size(43, 13);
            label2.TabIndex = 3;
            label2.Text = "Palette:";
            // 
            // grpEdit
            // 
            grpEdit.Controls.Add(btnPaste);
            grpEdit.Controls.Add(pltBox);
            grpEdit.Controls.Add(btnCopy);
            grpEdit.Controls.Add(label1);
            grpEdit.Controls.Add(texBox);
            grpEdit.Controls.Add(btnCut);
            grpEdit.Controls.Add(label2);
            grpEdit.Dock = DockStyle.Fill;
            grpEdit.Location = new Point(0, 0);
            grpEdit.Name = "grpEdit";
            grpEdit.Size = new Size(402, 77);
            grpEdit.TabIndex = 28;
            grpEdit.TabStop = false;
            grpEdit.Text = "Edit Frame";
            // 
            // btnPaste
            // 
            btnPaste.Location = new Point(3, 54);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(50, 20);
            btnPaste.TabIndex = 26;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += new EventHandler(btnPaste_Click);
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(3, 35);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(50, 20);
            btnCopy.TabIndex = 25;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += new EventHandler(btnCopy_Click);
            // 
            // btnCut
            // 
            btnCut.Location = new Point(3, 16);
            btnCut.Name = "btnCut";
            btnCut.Size = new Size(50, 20);
            btnCut.TabIndex = 24;
            btnCut.Text = "Cut";
            btnCut.UseVisualStyleBackColor = true;
            btnCut.Click += new EventHandler(btnCut_Click);
            // 
            // PAT0Editor
            // 
            Controls.Add(grpEdit);
            MinimumSize = new Size(402, 77);
            Name = "PAT0Editor";
            Size = new Size(402, 77);
            grpEdit.ResumeLayout(false);
            grpEdit.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private StringInputComboBox texBox;
        private StringInputComboBox pltBox;
        private Label label2;
        private GroupBox grpEdit;
        private Button btnPaste;
        private Button btnCopy;
        private Button btnCut;

        public ModelEditorBase _mainWindow;

        public PAT0Editor()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
        }

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
        public PAT0Node SelectedAnimation
        {
            get => _mainWindow.SelectedPAT0;
            set => _mainWindow.SelectedPAT0 = value;
        }

        public void UpdatePropDisplay()
        {
            if (!Enabled)
            {
                return;
            }

            ResetTexture();
            ResetPalette();
        }

        private void ResetPalette()
        {
            MDL0MaterialRefNode mr = TargetTexRef;
            PAT0TextureNode node;

            if (mr == null)
            {
                return;
            }

            _updating = true;
            if (SelectedAnimation != null && CurrentFrame > 0 &&
                (node = SelectedAnimation.FindChild(mr._parent.Name + "/Texture" + mr.Index,
                    true) as PAT0TextureNode) != null)
            {
                string e = node.GetPalette(CurrentFrame - 1, out bool kf);
                pltBox.Value = e;
                if (!kf)
                {
                    pltBox.BackColor = Color.White;
                }
                else
                {
                    pltBox.BackColor = Color.Yellow;
                }
            }
            else
            {
                pltBox.Value = null;
                pltBox.BackColor = Color.White;
            }

            _updating = false;
        }

        private void ResetTexture()
        {
            MDL0MaterialRefNode mr = TargetTexRef;
            PAT0TextureNode node;

            if (mr == null)
            {
                return;
            }

            _updating = true;
            if (SelectedAnimation != null && CurrentFrame > 0 &&
                (node = SelectedAnimation.FindChild(mr._parent.Name + "/Texture" + mr.Index,
                    true) as PAT0TextureNode) != null)
            {
                string e = node.GetTexture(CurrentFrame - 1, out bool kf);
                texBox.Value = e;
                if (!kf)
                {
                    texBox.BackColor = Color.White;
                }
                else
                {
                    texBox.BackColor = Color.Yellow;
                }
            }
            else
            {
                texBox.Value = null;
                texBox.BackColor = Color.White;
            }

            _updating = false;
        }

        private bool _updating;

        internal void TexChanged(object sender, EventArgs e)
        {
            MDL0MaterialRefNode mr = TargetTexRef;
            PAT0TextureNode node;

            if (mr == null || _updating)
            {
                return;
            }

            if (SelectedAnimation != null && CurrentFrame > 0)
            {
                node = SelectedAnimation.FindChild(mr._parent.Name + "/Texture" + mr.Index, true) as PAT0TextureNode;

                if (node != null)
                {
                    PAT0TextureEntryNode tex = node.GetEntry(CurrentFrame - 1);
                    if (tex == null)
                    {
                        if (!string.IsNullOrEmpty(texBox.Text) || !string.IsNullOrEmpty(pltBox.Text))
                        {
                            tex = new PAT0TextureEntryNode
                            {
                                _frame = CurrentFrame - 1
                            };
                            if (node.Children.Count > 0)
                            {
                                node.InsertChild(tex, true, node.GetPrevious(CurrentFrame - 1).Index + 1);
                            }
                            else
                            {
                                node.AddChild(tex, true);
                            }

                            tex.Texture = texBox.Text;
                            tex.Palette = pltBox.Text;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(texBox.Text) || !string.IsNullOrEmpty(pltBox.Text))
                        {
                            tex.Texture = texBox.Text;
                            tex.Palette = pltBox.Text;
                        }
                        else
                        {
                            tex.Remove();
                        }
                    }
                }
            }

            TargetModel.ApplyPAT(SelectedAnimation, CurrentFrame);
            ResetTexture();
            ResetPalette();
            _mainWindow.ModelPanel.Invalidate();
        }

        public void UpdateBoxes()
        {
            if (TargetModel == null)
            {
                return;
            }

            texBox.Items.Clear();
            pltBox.Items.Clear();

            if (TargetModel != null)
            {
                foreach (ResourceNode s in ((ResourceNode) TargetModel).RootNode.FindChildrenByType(null,
                    ResourceType.TEX0))
                {
                    texBox.Items.Add(s.Name);
                }

                foreach (ResourceNode s in ((ResourceNode) TargetModel).RootNode.FindChildrenByType(null,
                    ResourceType.PLT0))
                {
                    pltBox.Items.Add(s.Name);
                }
            }

            foreach (ResourceNode r in _mainWindow._openedFiles)
            {
                foreach (ResourceNode s in r.FindChildrenByType(null, ResourceType.TEX0))
                {
                    texBox.Items.Add(s.Name);
                }

                foreach (ResourceNode s in r.FindChildrenByType(null, ResourceType.PLT0))
                {
                    pltBox.Items.Add(s.Name);
                }
            }
        }

        private string Texture, Palette;

        private void btnCut_Click(object sender, EventArgs e)
        {
            Texture = texBox.Text;
            Palette = pltBox.Text;
            texBox.Text = null;
            pltBox.Text = null;
            TexChanged(this, null);
            UpdatePropDisplay();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Texture = texBox.Text;
            Palette = pltBox.Text;
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            texBox.Text = Texture;
            pltBox.Text = Palette;
            TexChanged(this, null);
            UpdatePropDisplay();
        }
    }
}
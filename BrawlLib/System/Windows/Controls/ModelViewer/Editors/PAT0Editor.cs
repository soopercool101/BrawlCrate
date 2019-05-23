using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Modeling;

namespace System.Windows.Forms
{
    public class PAT0Editor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.texBox = new System.Windows.Forms.StringInputComboBox();
            this.pltBox = new System.Windows.Forms.StringInputComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCut = new System.Windows.Forms.Button();
            this.grpEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Texture:";
            // 
            // texBox
            // 
            this.texBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.texBox.FormattingEnabled = true;
            this.texBox.Location = new System.Drawing.Point(108, 20);
            this.texBox.Name = "texBox";
            this.texBox.Size = new System.Drawing.Size(288, 21);
            this.texBox.TabIndex = 2;
            this.texBox.ValueChanged += new System.EventHandler(this.TexChanged);
            // 
            // pltBox
            // 
            this.pltBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pltBox.BackColor = System.Drawing.SystemColors.Window;
            this.pltBox.FormattingEnabled = true;
            this.pltBox.Location = new System.Drawing.Point(108, 47);
            this.pltBox.Name = "pltBox";
            this.pltBox.Size = new System.Drawing.Size(288, 21);
            this.pltBox.TabIndex = 4;
            this.pltBox.ValueChanged += new System.EventHandler(this.TexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Palette:";
            // 
            // grpEdit
            // 
            this.grpEdit.Controls.Add(this.btnPaste);
            this.grpEdit.Controls.Add(this.pltBox);
            this.grpEdit.Controls.Add(this.btnCopy);
            this.grpEdit.Controls.Add(this.label1);
            this.grpEdit.Controls.Add(this.texBox);
            this.grpEdit.Controls.Add(this.btnCut);
            this.grpEdit.Controls.Add(this.label2);
            this.grpEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEdit.Location = new System.Drawing.Point(0, 0);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.Size = new System.Drawing.Size(402, 77);
            this.grpEdit.TabIndex = 28;
            this.grpEdit.TabStop = false;
            this.grpEdit.Text = "Edit Frame";
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(3, 54);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(50, 20);
            this.btnPaste.TabIndex = 26;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(3, 35);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(50, 20);
            this.btnCopy.TabIndex = 25;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCut
            // 
            this.btnCut.Location = new System.Drawing.Point(3, 16);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(50, 20);
            this.btnCut.TabIndex = 24;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // PAT0Editor
            // 
            this.Controls.Add(this.grpEdit);
            this.MinimumSize = new System.Drawing.Size(402, 77);
            this.Name = "PAT0Editor";
            this.Size = new System.Drawing.Size(402, 77);
            this.grpEdit.ResumeLayout(false);
            this.grpEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private System.ComponentModel.IContainer components;
        private StringInputComboBox texBox;
        private StringInputComboBox pltBox;
        private Label label2;
        private GroupBox grpEdit;
        private Button btnPaste;
        private Button btnCopy;
        private Button btnCut;

        public ModelEditorBase _mainWindow;

        StringInputComboBox texture, palette;

        public PAT0Editor() { InitializeComponent(); }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone { get { return _mainWindow.SelectedBone; } set { _mainWindow.SelectedBone = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef { get { return _mainWindow.TargetTexRef; } set { _mainWindow.TargetTexRef = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PAT0Node SelectedAnimation
        {
            get { return _mainWindow.SelectedPAT0; }
            set { _mainWindow.SelectedPAT0 = value; }
        }
        public void UpdatePropDisplay()
        {
            if (!Enabled)
                return;

            ResetTexture();
            ResetPalette();
        }

        private void ResetPalette()
        {
            MDL0MaterialRefNode mr = TargetTexRef;
            PAT0TextureNode node;

            if (mr == null)
                return;

            _updating = true;
            if ((SelectedAnimation != null) && (CurrentFrame > 0) && ((node = SelectedAnimation.FindChild(mr._parent.Name + "/Texture" + mr.Index, true) as PAT0TextureNode) != null))
            {
                bool kf;
                string e = node.GetPalette(CurrentFrame - 1, out kf);
                pltBox.Value = e;
                if (!kf)
                    pltBox.BackColor = Color.White;
                else
                    pltBox.BackColor = Color.Yellow;
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
                return;

            _updating = true;
            if ((SelectedAnimation != null) && (CurrentFrame > 0) && ((node = SelectedAnimation.FindChild(mr._parent.Name + "/Texture" + mr.Index, true) as PAT0TextureNode) != null))
            {
                bool kf;
                string e = node.GetTexture(CurrentFrame - 1, out kf);
                texBox.Value = e;
                if (!kf)
                    texBox.BackColor = Color.White;
                else
                    texBox.BackColor = Color.Yellow;
            }
            else
            {
                texBox.Value = null;
                texBox.BackColor = Color.White;
            }
            _updating = false;
        }

        bool _updating = false;
        internal unsafe void TexChanged(object sender, EventArgs e)
        {
            MDL0MaterialRefNode mr = TargetTexRef;
            PAT0TextureNode node;

            if (mr == null || _updating)
                return;

            if (SelectedAnimation != null && CurrentFrame > 0)
            {
                node = SelectedAnimation.FindChild(mr._parent.Name + "/Texture" + mr.Index, true) as PAT0TextureNode;

                if (node != null)
                {
                    PAT0TextureEntryNode tex = node.GetEntry(CurrentFrame - 1);
                    if (tex == null)
                    {
                        if (!String.IsNullOrEmpty(texBox.Text) || !String.IsNullOrEmpty(pltBox.Text))
                        {
                            tex = new PAT0TextureEntryNode();
                            tex._frame = CurrentFrame - 1;
                            if (node.Children.Count > 0)
                                node.InsertChild(tex, true, node.GetPrevious(CurrentFrame - 1).Index + 1);
                            else
                                node.AddChild(tex, true);
                            tex.Texture = texBox.Text;
                            tex.Palette = pltBox.Text;
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(texBox.Text) || !String.IsNullOrEmpty(pltBox.Text))
                        {
                            tex.Texture = texBox.Text;
                            tex.Palette = pltBox.Text;
                        }
                        else
                            tex.Remove();
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
                return;

            texBox.Items.Clear();
            pltBox.Items.Clear();

            if (TargetModel != null)
            {
                foreach (ResourceNode s in ((ResourceNode)TargetModel).RootNode.FindChildrenByType(null, ResourceType.TEX0))
                    texBox.Items.Add(s.Name);
                foreach (ResourceNode s in ((ResourceNode)TargetModel).RootNode.FindChildrenByType(null, ResourceType.PLT0))
                    pltBox.Items.Add(s.Name);
            }
            foreach (ResourceNode r in _mainWindow._openedFiles)
            {
                foreach (ResourceNode s in r.FindChildrenByType(null, ResourceType.TEX0))
                    texBox.Items.Add(s.Name);
                foreach (ResourceNode s in r.FindChildrenByType(null, ResourceType.PLT0))
                    pltBox.Items.Add(s.Name);
            }
        }

        string Texture, Palette;
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

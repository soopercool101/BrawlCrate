using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;

namespace BrawlBox
{
    class ModelForm : Form
    {
        #region Designer

        private ModelEditControl modelEditControl1;
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelForm));
            this.modelEditControl1 = new System.Windows.Forms.ModelEditControl();
            this.SuspendLayout();
            // 
            // modelEditControl1
            // 
            this.modelEditControl1.AllowDrop = true;
            this.modelEditControl1.BackColor = System.Drawing.Color.Lavender;
            this.modelEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelEditControl1.ScreenCaptureType = 0;
            this.modelEditControl1.Location = new System.Drawing.Point(0, 0);
            this.modelEditControl1.Name = "modelEditControl1";
            this.modelEditControl1.Size = new System.Drawing.Size(639, 528);
            this.modelEditControl1.TabIndex = 0;
            this.modelEditControl1.TargetAnimation = null;
            this.modelEditControl1.TargetAnimType = System.Windows.Forms.NW4RAnimType.CHR;
            this.modelEditControl1.TargetModelChanged += new System.EventHandler(this.TargetModelChanged);
            this.modelEditControl1.ModelViewerChanged += new System.EventHandler(this.ModelViewerChanged);
            // 
            // ModelForm
            // 
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(639, 528);
            this.Controls.Add(this.modelEditControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModelForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelForm_FormClosing);
            this.ResumeLayout(false);

        }
        #endregion

        public ModelForm() { InitializeComponent(); }

        private List<IModel> _models = new List<IModel>();
        private List<CollisionNode> _collisions = new List<CollisionNode>();

        public DialogResult ShowDialog(List<IModel> models) { return ShowDialog(null, models); }
        public DialogResult ShowDialog(IWin32Window owner, List<IModel> models,
            List<CollisionNode> collisions = null)
        {
            _models = models;
            _collisions = collisions ?? _collisions;
            try { return base.ShowDialog(owner); }
            finally { _models = null; }
        }
        public DialogResult ShowDialog(IModel model) { return ShowDialog(null, model); }
        public DialogResult ShowDialog(IWin32Window owner, IModel model)
        {
            _models.Add(model);
            try { return base.ShowDialog(owner); }
            finally { _models = null; }
        }

        public void Show(IWin32Window owner, List<IModel> models,
            List<CollisionNode> collisions = null)
        {
            _models = models;
            _collisions = collisions ?? _collisions;
            base.Show(owner);
        }

        public void Show(List<IModel> models) { Show(null, models); }
        public void Show(IWin32Window owner, List<IModel> models)
        {
            _models = models;
            base.Show(owner);
        }
        public void Show(IModel model) { Show(null, model); }
        public void Show(IWin32Window owner, IModel model)
        {
            _models.Add(model);
            base.Show(owner);
        }

        public unsafe void ReadSettings()
        {
            BrawlBox.Properties.Settings settings = BrawlBox.Properties.Settings.Default;

            ModelEditorSettings viewerSettings = settings.ViewerSettingsSet ? settings.ViewerSettings : ModelEditorSettings.Default();

            if (viewerSettings == null)
                return;

            modelEditControl1.DistributeSettings(viewerSettings);
            modelEditControl1.ModelPanel.ResetCamera();

            if (viewerSettings.Maximize)
                WindowState = FormWindowState.Maximized;
            else if (viewerSettings.SavePosition)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(viewerSettings._posX, viewerSettings._posY);
                if (viewerSettings._height > 0)
                    Height = viewerSettings._height;
                if (viewerSettings._width > 0)
                    Width = viewerSettings._width;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            modelEditControl1._openedFiles.Add(Program.RootNode);

            MainForm.Instance.Visible = 
                !BrawlBox.Properties.Settings.Default.ViewerSettings.HideMainWindow;

            if (_models.Count != 0)
            {
                for (int i = 0; i < _models.Count; i++)
                    if (_models[i] != null)
                        modelEditControl1.AppendTarget(_models[i]);

                modelEditControl1.TargetModel = _models[0];
                modelEditControl1.EditingAll = _models.Count > 1;
                modelEditControl1.ResetBoneColors();
            }
            else
                modelEditControl1.TargetModel = null;

            if (_collisions.Count != 0)
                foreach (CollisionNode node in _collisions)
                {
                    modelEditControl1.AppendTarget(node);

                    // Link bones
                    foreach (CollisionObject obj in node._objects)
                    {
                        if (obj._modelName == "" || obj._boneName == "")
                            continue;

                        MDL0Node model = _models.
                            Where(m => m is MDL0Node && ((ResourceNode)m).Name == obj._modelName).
                            FirstOrDefault() as MDL0Node;

                        if (model != null)
                        {
                            MDL0BoneNode bone = model._linker.BoneCache.Where(b => b.Name == obj._boneName).FirstOrDefault() as MDL0BoneNode;
                            if (bone != null) 
                                obj._linkedBone = bone;
                        }
                    }
                }

            modelEditControl1.ModelPanel.Capture();
            ReadSettings();

            base.OnShown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            MainForm.Instance.Visible = 
                BrawlBox.Properties.Settings.Default.ViewerSettings.HideMainWindow ? 
                ModelEditControl.Instances.Count == 0 : true;

            MainForm.Instance.modelPanel1.Capture();
            MainForm.Instance.resourceTree_SelectionChanged(this, null);
            MainForm.Instance.Refresh();
        }

        private void ModelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !modelEditControl1.Close();
        }

        private void ModelViewerChanged(object sender, EventArgs e)
        {
            //Ain't nobody got time fo dis fancy mdi stuff
            if (modelEditControl1.ModelViewerForm != null)
            {
                //IsMdiContainer = true;
                //modelEditControl1.ModelViewerForm.MdiParent = this;
                Application.AddMessageFilter(mouseMessageFilter = new MouseMoveMessageFilter() { TargetForm = this });
            }
            else
            {
                //IsMdiContainer = false;
                Application.RemoveMessageFilter(mouseMessageFilter);
            }
        }

        private void TargetModelChanged(object sender, EventArgs e)
        {
            if (modelEditControl1.TargetModel != null)
                Text = String.Format("{1} - Advanced Model Editor - {0}", ((ResourceNode)modelEditControl1.TargetModel).Name, Program.AssemblyTitle);
            else
                Text = Text = Program.AssemblyTitle + " - Advanced Model Editor";
        }

        private MouseMoveMessageFilter mouseMessageFilter;
        class MouseMoveMessageFilter : IMessageFilter
        {
            public ModelForm TargetForm { get; set; }
            bool _mainWindowFocused = false;
            public bool PreFilterMessage(ref Message m)
            {
                int numMsg = m.Msg;
                if (numMsg == 0x0200) //WM_MOUSEMOVE
                {
                    if (TargetForm.modelEditControl1.ModelViewerForm != null)
                    if (InForm(TargetForm.modelEditControl1.ModelViewerForm, Control.MousePosition))
                    {
                        if (_mainWindowFocused)
                        {
                            TargetForm.modelEditControl1.ModelViewerForm.Focus();
                            _mainWindowFocused = false;
                        }
                    }
                    else if (InForm(TargetForm, Control.MousePosition))
                    {
                        if (!_mainWindowFocused)
                        {
                            TargetForm.Focus();
                            _mainWindowFocused = true;
                        }
                    }
                }
                return false;
            }
            private bool InForm(Form f, Point screenPoint)
            {
                Point p = f.PointToClient(screenPoint);
                return p.X > 0 && p.X < f.Size.Width && p.Y > 0 && p.Y < f.Size.Height;
            }
        }
    }
}

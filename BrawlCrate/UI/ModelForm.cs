using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrawlCrate.Properties;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using Resources = BrawlLib.Properties.Resources;

namespace BrawlCrate
{
    internal class ModelForm : Form
    {
        private List<CollisionNode> _collisions = new List<CollisionNode>();

        private List<IModel> _models = new List<IModel>();

        private MouseMoveMessageFilter mouseMessageFilter;

        public ModelForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(List<IModel> models)
        {
            return ShowDialog(null, models);
        }

        public DialogResult ShowDialog(IWin32Window owner, List<IModel> models,
            List<CollisionNode> collisions = null)
        {
            _models = models;
            _collisions = collisions ?? _collisions;
            try
            {
                return ShowDialog(owner);
            }
            finally
            {
                _models = null;
            }
        }

        public DialogResult ShowDialog(IModel model)
        {
            return ShowDialog(null, model);
        }

        public DialogResult ShowDialog(IWin32Window owner, IModel model)
        {
            _models.Add(model);
            try
            {
                return ShowDialog(owner);
            }
            finally
            {
                _models = null;
            }
        }

        public void Show(IWin32Window owner, List<IModel> models,
            List<CollisionNode> collisions = null)
        {
            _models = models;
            _collisions = collisions ?? _collisions;
            Show(owner);
        }

        public void Show(List<IModel> models)
        {
            Show(null, models);
        }

        public void Show(IWin32Window owner, List<IModel> models)
        {
            _models = models;
            Show(owner);
        }

        public void Show(IModel model)
        {
            Show(null, model);
        }

        public void Show(IWin32Window owner, IModel model)
        {
            _models.Add(model);
            Show(owner);
        }

        public void ReadSettings()
        {
            var settings = Settings.Default;

            var viewerSettings = settings.ViewerSettingsSet ? settings.ViewerSettings : ModelEditorSettings.Default();

            if (viewerSettings == null) return;

            modelEditControl1.DistributeSettings(viewerSettings);
            modelEditControl1.ModelPanel.ResetCamera();

            if (viewerSettings.Maximize)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (viewerSettings.SavePosition)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(viewerSettings._posX, viewerSettings._posY);
                if (viewerSettings._height > 0) Height = viewerSettings._height;

                if (viewerSettings._width > 0) Width = viewerSettings._width;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            modelEditControl1._openedFiles.Add(Program.RootNode);

            MainForm.Instance.Visible =
                !Settings.Default.ViewerSettings.HideMainWindow;

            if (_models.Count != 0)
            {
                for (var i = 0; i < _models.Count; i++)
                    if (_models[i] != null)
                        modelEditControl1.AppendTarget(_models[i]);

                modelEditControl1.TargetModel = _models[0];
                modelEditControl1.EditingAll = _models.Count > 1;
                modelEditControl1.ResetBoneColors();
            }
            else
            {
                modelEditControl1.TargetModel = null;
            }

            if (_collisions.Count != 0)
                foreach (var node in _collisions)
                {
                    modelEditControl1.AppendTarget(node);

                    // Link bones
                    foreach (var obj in node._objects)
                    {
                        if (obj._modelName == "" || obj._boneName == "") continue;

                        var model = _models.Where(m => m is MDL0Node && ((ResourceNode) m).Name == obj._modelName)
                            .FirstOrDefault() as MDL0Node;

                        if (model != null)
                        {
                            var bone = model._linker.BoneCache.Where(b => b.Name == obj._boneName).FirstOrDefault();
                            if (bone != null) obj._linkedBone = bone;
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
                Settings.Default.ViewerSettings.HideMainWindow ? ModelEditControl.Instances.Count == 0 : true;

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
                //IsMdiContainer = true;
                //modelEditControl1.ModelViewerForm.MdiParent = this;
                Application.AddMessageFilter(mouseMessageFilter = new MouseMoveMessageFilter {TargetForm = this});
            else
                //IsMdiContainer = false;
                Application.RemoveMessageFilter(mouseMessageFilter);
        }

        private void TargetModelChanged(object sender, EventArgs e)
        {
            if (modelEditControl1.TargetModel != null)
                Text = string.Format("{1} - Advanced Model Editor - {0}",
                    ((ResourceNode) modelEditControl1.TargetModel).Name, Program.AssemblyTitle);
            else
                Text = Text = Program.AssemblyTitle + " - Advanced Model Editor";
        }

        private class MouseMoveMessageFilter : IMessageFilter
        {
            private bool _mainWindowFocused;
            public ModelForm TargetForm { get; set; }

            public bool PreFilterMessage(ref Message m)
            {
                var numMsg = m.Msg;
                if (numMsg == 0x0200) //WM_MOUSEMOVE
                    if (TargetForm.modelEditControl1.ModelViewerForm != null)
                    {
                        if (InForm(TargetForm.modelEditControl1.ModelViewerForm, MousePosition))
                        {
                            if (_mainWindowFocused)
                            {
                                TargetForm.modelEditControl1.ModelViewerForm.Focus();
                                _mainWindowFocused = false;
                            }
                        }
                        else if (InForm(TargetForm, MousePosition))
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
                var p = f.PointToClient(screenPoint);
                return p.X > 0 && p.X < f.Size.Width && p.Y > 0 && p.Y < f.Size.Height;
            }
        }

        #region Designer

        private ModelEditControl modelEditControl1;

        private void InitializeComponent()
        {
            var resources = new ComponentResourceManager(typeof(ModelForm));
            modelEditControl1 = new ModelEditControl();
            SuspendLayout();
            // 
            // modelEditControl1
            // 
            modelEditControl1.AllowDrop = true;
            modelEditControl1.BackColor = Color.Lavender;
            modelEditControl1.Dock = DockStyle.Fill;
            modelEditControl1.ScreenCaptureType = 0;
            modelEditControl1.Location = new Point(0, 0);
            modelEditControl1.Name = "modelEditControl1";
            modelEditControl1.Size = new Size(639, 528);
            modelEditControl1.TabIndex = 0;
            modelEditControl1.TargetAnimation = null;
            modelEditControl1.TargetAnimType = NW4RAnimType.CHR;
            modelEditControl1.TargetModelChanged += TargetModelChanged;
            modelEditControl1.ModelViewerChanged += ModelViewerChanged;
            // 
            // ModelForm
            // 
            BackColor = Color.PowderBlue;
            ClientSize = new Size(639, 528);
            Controls.Add(modelEditControl1);
            Icon = Resources.Icon;
            Name = "ModelForm";
            FormClosing += ModelForm_FormClosing;
            ResumeLayout(false);
        }

        #endregion
    }
}
using BrawlLib.Imaging.GIF;
using BrawlLib.Internal.Drawing;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        #region Constructor

        protected void PreConstruct()
        {
            _interpolationEditor = new InterpolationEditor(this);

            _boneTransform = new ApplyLocalBoneTransformFunc[]
            {
                ApplyTranslation,
                ApplyAngle,
                ApplyScale
            };
            _mouseMoveTargetType = new MouseMoveTargetType[]
            {
                MouseMoveTargetBone,
                MouseMoveTargetVertex
            };
        }

        protected void PostConstruct()
        {
            _timer = new CoolTimer();
            _timer.RenderFrame += _timer_RenderFrame;

            if (KeyframePanel != null)
            {
                KeyframePanel.visEditor.EntryChanged += new EventHandler(VISEntryChanged);
                KeyframePanel.visEditor.IndexChanged += new EventHandler(VISIndexChanged);
            }

            ModelPanel.PreRender += EventPreRender = new GLRenderEventHandler(modelPanel1_PreRender);
            ModelPanel.PostRender += EventPostRender = new GLRenderEventHandler(modelPanel1_PostRender);
            ModelPanel.MouseDown += EventMouseDown = new MouseEventHandler(modelPanel1_MouseDown);
            ModelPanel.MouseMove += EventMouseMove = new MouseEventHandler(modelPanel1_MouseMove);
            ModelPanel.MouseUp += EventMouseUp = new MouseEventHandler(modelPanel1_MouseUp);

            if (PlaybackPanel != null)
            {
                if (PlaybackPanel.Width <= PlaybackPanel.MinimumSize.Width)
                {
                    PlaybackPanel.Dock = DockStyle.Left;
                    PlaybackPanel.Width = PlaybackPanel.MinimumSize.Width;
                }
                else
                {
                    PlaybackPanel.Dock = DockStyle.Fill;
                }
            }

            InitHotkeyList();

            _hotKeysDown = new Dictionary<Keys, Func<bool>>();
            _hotKeysUp = new Dictionary<Keys, Func<bool>>();
            foreach (HotKeyInfo key in _hotkeyList)
            {
                if (key._keyDown)
                {
                    _hotKeysDown.Add(key.KeyCode, key._function);
                }

                if (key._keyUp)
                {
                    _hotKeysUp.Add(key.KeyCode, key._function);
                }
            }
        }

        public virtual void LinkModelPanel(ModelPanel p)
        {
            p.PreRender += EventPreRender;
            p.PostRender += EventPostRender;
            p.MouseDown += EventMouseDown;
            p.MouseMove += EventMouseMove;
            p.MouseUp += EventMouseUp;
        }

        public virtual void UnlinkModelPanel(ModelPanel p)
        {
            p.PreRender -= EventPreRender;
            p.PostRender -= EventPostRender;
            p.MouseDown -= EventMouseDown;
            p.MouseMove -= EventMouseMove;
            p.MouseUp -= EventMouseUp;
        }

        public virtual void OnModelPanelChanged()
        {
            ModelViewerChanged?.Invoke(this, null);
        }

        #endregion

        #region Models

        public virtual void AppendTarget(IModel model)
        {
            if (!_targetModels.Contains(model))
            {
                _targetModels.Add(model);
            }

            ModelPanel.AddTarget(model, false);
            model.ResetToBindState();
        }

        protected virtual void ModelChanged(IModel newModel)
        {
            if (newModel != null && !_targetModels.Contains(newModel))
            {
                _targetModels.Add(newModel);
            }

            if (_targetModel != null)
            {
                _targetModel.IsTargetModel = false;
            }

            if ((_targetModel = newModel) != null)
            {
                ModelPanel.AddTarget(_targetModel, false);
                _targetModel.IsTargetModel = true;
                ClearSelectedVertices();
            }
            else
            {
                EditingAll = true; //No target model so all is the only option
            }

            if (_resetCamera)
            {
                ModelPanel.ResetCamera();
                SetFrame(0);
            }
            else
            {
                _resetCamera = true;
            }

            OnModelChanged();
            Invalidate();
            TargetModelChanged?.Invoke(this, null);
        }

        protected virtual void OnModelChanged()
        {
        }

        protected virtual void OnSelectedVerticesChanged()
        {
            //Force the average vertex location to be recalculated
            _vertexLoc = null;
            ModelPanel.Invalidate();
        }

        protected virtual void OnSelectedBoneChanged()
        {
        }

        #endregion

        #region Viewer Background

        public virtual ColorDialog ColorDialog => null;

        public virtual void ChooseBackgroundColor()
        {
            if (ColorDialog != null && ColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                ModelPanel.CurrentViewport.BackgroundColor = ColorDialog.Color;
            }
        }

        public void ChooseOrClearBackgroundImage()
        {
            if (ModelPanel.CurrentViewport.BackgroundImage == null)
            {
                OpenFileDialog d = new OpenFileDialog
                {
                    Filter = FileFilters.Images,
                    Title = "Select an image to load"
                };

                if (d.ShowDialog() == DialogResult.OK)
                {
                    ModelPanel.CurrentViewport.BackgroundImage = Image.FromFile(d.FileName);
                }
            }
            else
            {
                ModelPanel.CurrentViewport.BackgroundImage = null;
            }
        }

        #endregion

        #region Playback Panel

        public void pnlPlayback_Resize(object sender, EventArgs e)
        {
            if (PlaybackPanel.Width <= PlaybackPanel.MinimumSize.Width)
            {
                PlaybackPanel.Dock = DockStyle.Left;
                PlaybackPanel.Width = PlaybackPanel.MinimumSize.Width;
            }
            else
            {
                PlaybackPanel.Dock = DockStyle.Fill;
            }
        }

        public virtual void numFrameIndex_ValueChanged(object sender, EventArgs e)
        {
            int val = (int) PlaybackPanel.numFrameIndex.Value;
            if (val != _animFrame)
            {
                int difference = val - _animFrame;
                if (TargetAnimation != null)
                {
                    SetFrame(_animFrame + difference);
                }
            }
        }

        public virtual void numFPS_ValueChanged(object sender, EventArgs e)
        {
            _timer.TargetRenderFrequency = (double) PlaybackPanel.numFPS.Value;
        }

        public virtual void PlaybackPanel_LoopChanged()
        {
            _loop = PlaybackPanel.chkLoop.Checked;
            //if (TargetAnimation != null)
            //    TargetAnimation.Loop = _loop;
        }

        public virtual void numTotalFrames_ValueChanged(object sender, EventArgs e)
        {
            if (TargetAnimation == null || _updating)
            {
                return;
            }

            int max = (int) PlaybackPanel.numTotalFrames.Value;
            PlaybackPanel.numFrameIndex.Maximum = max;

            if (Interpolated.Contains(TargetAnimation.GetType()) && TargetAnimation.Loop)
            {
                max--;
            }

            _maxFrame = max;
            TargetAnimation.FrameCount = max;
        }

        public virtual void btnPrevFrame_Click(object sender, EventArgs e)
        {
            PlaybackPanel.numFrameIndex.Value--;
        }

        public virtual void btnNextFrame_Click(object sender, EventArgs e)
        {
            PlaybackPanel.numFrameIndex.Value++;
        }

        public virtual void TogglePlay()
        {
            if (_timer.IsRunning)
            {
                StopAnim();
            }
            else
            {
                PlayAnim();
            }
        }

        #endregion

        public virtual void SaveSettings()
        {
        }

        public virtual void SetDefaultSettings()
        {
        }

        private void RenderToGIF(List<Image> images, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string outPath = "";
            try
            {
                outPath = path;
                if (!Directory.Exists(outPath))
                {
                    Directory.CreateDirectory(outPath);
                }

                DirectoryInfo dir = new DirectoryInfo(outPath);
                FileInfo[] files = dir.GetFiles();
                int i = 0;
                string name = "Animation";
                Top:
                foreach (FileInfo f in files)
                {
                    if (f.Name == name + i + ".gif")
                    {
                        i++;
                        goto Top;
                    }
                }

                outPath += "\\" + name + i + ".gif";
            }
            catch
            {
                // ignored
            }

            AnimatedGifEncoder e = new AnimatedGifEncoder();
            e.Start(outPath);
            e.SetDelay(1000 / (int) PlaybackPanel.numFPS.Value);
            e.SetRepeat(0);
            e.SetQuality(10);
            using (ProgressWindow progress = new ProgressWindow(this, "GIF Encoder", "Encoding, please wait...", true))
            {
                progress.TopMost = true;
                progress.Begin(0, images.Count, 0);
                for (int i = 0, count = images.Count; i < count; i++)
                {
                    if (progress.Cancelled)
                    {
                        break;
                    }

                    //GIF transparency support is pretty poor, flickers a lot
                    //e.SetTransparent(ModelPanel.CurrentViewport.BackgroundColor);

                    e.AddFrame(images[i]);
                    progress.Update(progress.CurrentValue + 1);
                }

                progress.Finish();
                e.Finish();
            }

            _loop = PlaybackPanel.chkLoop.Checked;

            if (MessageBox.Show(this,
                "Animated GIF successfully saved to \"" + outPath.Replace("\\", "/") + "\".\nOpen the folder now?",
                "GIF saved", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("explorer.exe", path);
            }
        }

        protected void SaveBitmap(Bitmap bmp, string path, string extension)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Application.StartupPath + "\\ScreenCaptures";
            }

            if (string.IsNullOrEmpty(extension))
            {
                extension = ".png";
            }

            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(extension))
            {
                try
                {
                    string outPath = path;
                    if (!Directory.Exists(outPath))
                    {
                        Directory.CreateDirectory(outPath);
                    }

                    DirectoryInfo dir = new DirectoryInfo(outPath);
                    FileInfo[] files = dir.GetFiles();
                    int i = 0;
                    string name = "ScreenCapture";
                    Top:
                    foreach (FileInfo f in files)
                    {
                        if (f.Name == name + i + extension)
                        {
                            i++;
                            goto Top;
                        }
                    }

                    outPath += "\\" + name + i + extension;
                    bool okay = true;
                    if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.Save(outPath, ImageFormat.Png);
                    }
                    else if (extension.Equals(".tga", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.SaveTGA(outPath);
                    }
                    else if (extension.Equals(".tiff", StringComparison.OrdinalIgnoreCase) ||
                             extension.Equals(".tif", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.Save(outPath, ImageFormat.Tiff);
                    }
                    else if (extension.Equals(".bmp", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.Save(outPath, ImageFormat.Bmp);
                    }
                    else if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                             outPath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.Save(outPath, ImageFormat.Jpeg);
                    }
                    else if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        bmp.Save(outPath, ImageFormat.Gif);
                    }
                    else
                    {
                        okay = false;
                    }

                    if (okay)
                    {
                        if (MessageBox.Show(this,
                            "Screenshot successfully saved to \"" + outPath.Replace("\\", "/") +
                            "\".\nOpen the folder containing the screenshot now?", "Screenshot saved",
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Process.Start("explorer.exe", path);
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

            bmp.Dispose();
        }

        public virtual void LoadModels(ResourceNode node)
        {
            if (_targetModels == null)
            {
                _targetModels = new List<IModel>();
            }

            List<IModel> modelList = ModelPanel.CollectModels(node);
            foreach (IModel m in modelList)
            {
                AppendTarget(m);
            }

            ModelPanel.RefreshReferences();

            if (TargetModel == null && _targetModels.Count > 0)
            {
                TargetModel = _targetModels[0];
            }

            Invalidate();
        }

        public virtual void LoadAnimations(ResourceNode node)
        {
        }

        public virtual void LoadEtc(ResourceNode node)
        {
        }

        public virtual void OpenFile(string file)
        {
            OpenFile(file, true, true, true);
        }

        public virtual void OpenFile(string file, bool models, bool animations = true, bool etc = true)
        {
            try
            {
                ResourceNode node;
                if ((node = NodeFactory.FromFile(null, file)) != null)
                {
                    _openedFiles.Add(node);
                    ModelPanel.AddReference(node);

                    if (models)
                    {
                        LoadModels(node);
                    }

                    if (animations)
                    {
                        LoadAnimations(node);
                    }

                    if (etc)
                    {
                        LoadEtc(node);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Unable to recognize input file.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error loading from file.");
            }
        }
    }
}
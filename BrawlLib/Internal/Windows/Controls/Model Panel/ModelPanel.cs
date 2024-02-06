using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace BrawlLib.Internal.Windows.Controls.Model_Panel
{
    public delegate void GLRenderEventHandler(ModelPanelViewport sender);

    public class ModelPanel : GLPanel
    {
        public ModelPanel()
        {
            MDL0TextureNode._folderWatcher.Changed += _folderWatcher_Changed;
            MDL0TextureNode._folderWatcher.Created += _folderWatcher_Changed;
            MDL0TextureNode._folderWatcher.Deleted += _folderWatcher_Changed;
            MDL0TextureNode._folderWatcher.Renamed += _folderWatcher_Renamed;
            MDL0TextureNode._folderWatcher.Error += _folderWatcher_Error;
        }

        ~ModelPanel()
        {
            MDL0TextureNode._folderWatcher.Changed -= _folderWatcher_Changed;
            MDL0TextureNode._folderWatcher.Created -= _folderWatcher_Changed;
            MDL0TextureNode._folderWatcher.Deleted -= _folderWatcher_Changed;
            MDL0TextureNode._folderWatcher.Renamed -= _folderWatcher_Renamed;
            MDL0TextureNode._folderWatcher.Error -= _folderWatcher_Error;
        }

        private readonly List<KeyValuePair<ModelPanelViewport, DragFlags>> _dragging =
            new List<KeyValuePair<ModelPanelViewport, DragFlags>>();

        public bool _draggingViewports;

        public BindingList<IRenderedObject> _renderList = new BindingList<IRenderedObject>();
        public List<DrawCallBase> _drawCalls = new List<DrawCallBase>();
        public List<ResourceNode> _resourceList = new List<ResourceNode>();

        public event GLRenderEventHandler PreRender, PostRender;

        #region Camera

        public GLCamera Camera => CurrentViewport.Camera;

        public void ResetCamera()
        {
            CurrentViewport.ResetCamera();
            Invalidate();
        }

        public void SetCamWithBox(Box value)
        {
            SetCamWithBox(value.Min, value.Max);
        }

        public void SetCamWithBox(Vector3 min, Vector3 max)
        {
            GLCamera cam = CurrentViewport.Camera;

            //Get the position of the midpoint of the bounding box plane closer to the camera
            Vector3 frontMidPt = new Vector3((max._x + min._x) / 2.0f, (max._y + min._y) / 2.0f, max._z);
            float tan = (float) Math.Tan(cam.VerticalFieldOfView / 2.0f * Maths._deg2radf), distX = 0, distY = 0;

            //The tangent value would only be 0 if the FOV was 0,
            //meaning nothing would be visible anyway
            if (tan != 0)
            {
                //Calculate lengths
                Vector3 extents = max - min;
                Vector3 halfExtents = extents / 2.0f;
                if (halfExtents._y != 0.0f)
                {
                    float ratio = halfExtents._x / halfExtents._y;
                    distY = halfExtents._y / tan; //The camera's distance from the model's midpoint in respect to Y
                    distX = distY * ratio;
                }
                else if (halfExtents._x != 0.0f)
                {
                    float ratio = halfExtents._y / halfExtents._x;
                    distX = halfExtents._x / tan; //The camera's distance from the model's midpoint in respect to Y
                    distY = distX * ratio;
                }
            }

            cam.Reset();

            cam.Translate(frontMidPt._x, frontMidPt._y, 0);
            if (CurrentViewport.GetProjectionType() == ViewportProjection.Orthographic)
            {
                cam.Translate(0, 0, Maths.Max(distX, distY, max._z) + 2.0f);
                cam.Zoom(Maths.Max(distX, distY, max._z) / (Maths.Max(Size.Height, Size.Width) * 0.01f));
            }
            else if (CurrentViewport.GetProjectionType() == ViewportProjection.Perspective)
            {
                CurrentViewport.Zoom(Maths.Max(distX, distY, max._z) + 2.0f, true);
            }

            Invalidate();
        }

        #endregion

        public new ModelPanelViewport CurrentViewport
        {
            get => base.CurrentViewport as ModelPanelViewport;
            set => base.CurrentViewport = value;
        }

        public new ModelPanelViewport HighlightedViewport => base.HighlightedViewport as ModelPanelViewport;

        public override void CreateDefaultViewport()
        {
            AddViewport(ModelPanelViewport.DefaultPerspective);
        }

        #region Targets

        public void ClearAll()
        {
            ClearTargets();
            ClearReferences();

            if (_ctx != null)
            {
                _ctx.Unbind();
                _ctx._states["_Node_Refs"] = _resourceList;
            }
        }

        public void AddTarget(IRenderedObject target)
        {
            AddTarget(target, true);
        }

        public void AddTarget(IRenderedObject target, bool invalidate)
        {
            if (_renderList.Contains(target))
            {
                return;
            }

            _renderList.Add(target);
            target.DrawCallsChanged += target_DrawCallsChanged;

            target.Attach();

            foreach (DrawCallBase call in target.DrawCalls)
            {
                call.Bind();
            }

            _drawCalls.AddRange(target.DrawCalls);
            if (_drawCalls.Count <= 0)
            {
                return;
            }

            _drawCalls.Sort(DrawCallSort);

            if (target is ResourceNode)
            {
                _resourceList.Add(target as ResourceNode);
            }

            if (invalidate)
            {
                Invalidate();
            }
        }

        public void AddTarget(CollisionNode target, bool invalidate)
        {
            if (_collisions.Contains(target))
            {
                return;
            }

            _collisions.Add(target);

            if (invalidate)
            {
                Invalidate();
            }
        }

        public void RemoveTarget(IRenderedObject target, bool refreshReferences = true)
        {
            if (!_renderList.Contains(target))
            {
                return;
            }

            _renderList.Remove(target);

            target.Detach();

            if (target is ResourceNode)
            {
                RemoveReference(target as ResourceNode, refreshReferences);
            }

            target.DrawCallsChanged -= target_DrawCallsChanged;

            _drawCalls = _renderList.SelectMany(x => x.DrawCalls).ToList();
            _drawCalls.Sort(DrawCallSort);
        }

        private Comparison<DrawCallBase> _drawCallSort = DrawCallBase.Sort;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Comparison<DrawCallBase> DrawCallSort
        {
            get => _drawCallSort ?? DrawCallBase.Sort;
            set => _drawCallSort = value;
        }

        public void target_DrawCallsChanged(object sender, EventArgs e)
        {
            _drawCalls = _renderList.SelectMany(x => x.DrawCalls).ToList();
            _drawCalls.Sort(DrawCallSort);
            Invalidate();
        }

        public void ClearTargets()
        {
            for (int i = 0; i < _renderList.Count; i++)
            {
                _renderList[i].Detach();
            }

            _renderList.Clear();
            _drawCalls.Clear();
            _collisions.Clear();
        }

        public void AddReference(ResourceNode node, bool refreshReferences = true)
        {
            if (_resourceList.Contains(node))
            {
                return;
            }

            _resourceList.Add(node);
            if (refreshReferences)
            {
                RefreshReferences();
            }
        }

        public void RemoveReference(ResourceNode node, bool refreshReferences = true)
        {
            if (!_resourceList.Contains(node))
            {
                return;
            }

            _resourceList.Remove(node);
            if (refreshReferences)
            {
                RefreshReferences();
            }
        }

        public void ClearReferences()
        {
            _resourceList.Clear();
            RefreshReferences();
        }

        public void RefreshReferences()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshReferences));
                return;
            }

            foreach (IRenderedObject o in _renderList)
            {
                o.Refresh();
            }

            Invalidate();
        }

        #endregion

        #region Mouse

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            float z = e.Delta / 120.0f;
            if (ModifierKeys == Keys.Shift)
            {
                z *= 32;
            }

            ModelPanelViewport v = HighlightedViewport;
            v.Zoom(-z * v._zoomFactor);

            if (ModifierKeys == Keys.Alt)
            {
                if (z < 0)
                {
                    v._multiplier /= 0.9f;
                }
                else
                {
                    v._multiplier *= 0.9f;
                }
            }

            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            ModelPanelViewport highlighted = HighlightedViewport;
            if (CurrentViewport != highlighted)
            {
                CurrentViewport = highlighted;
                CurrentViewport._lastX = e.X - CurrentViewport.Region.X;
                CurrentViewport._lastY = e.Y - CurrentViewport.Region.Y;
            }

            switch (e.Button)
            {
                case MouseButtons.Left:
                    _mouseDown = true;
                    if (_dragging.Count > 0 && _viewports.Count > 1)
                    {
                        _draggingViewports = true;
                    }
                    else
                    {
                        CurrentViewport.HandleLeftMouseDown(e);
                    }

                    break;

                case MouseButtons.Right:
                    CurrentViewport._grabbing = true;
                    break;

                case MouseButtons.Middle:
                    CurrentViewport._scrolling = true;
                    break;
            }

            if (e.Button != MouseButtons.Left)
            {
                CurrentViewport.HandleOtherMouseDown(e);
            }

            base.OnMouseDown(e);
        }

        private bool _mouseDown;

        protected override void OnMouseUp(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _mouseDown = false;
                    if (_draggingViewports)
                    {
                        _draggingViewports = false;
                        int range = 8;
                        for (int i = 0; i < _viewports.Count; i++)
                        {
                            GLViewport v = _viewports[i];
                            if (v.Region.Width < range || v.Region.Height < range)
                            {
                                v.OnInvalidate -= Invalidate;
                                RemoveViewport(i--);
                            }
                        }
                    }
                    else
                    {
                        CurrentViewport.HandleLeftMouseUp(e);
                    }

                    break;

                case MouseButtons.Right:
                    CurrentViewport._grabbing = false;
                    break;

                case MouseButtons.Middle:
                    CurrentViewport._scrolling = false;
                    break;

                default:
                    base.OnMouseUp(e);
                    return;
            }

            if (e.Button != MouseButtons.Left)
            {
                CurrentViewport.HandleOtherMouseUp(e);
            }

            Invalidate();
            base.OnMouseUp(e);
        }

        private enum DragFlags
        {
            xMin,
            yMin,
            xMax,
            yMax
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //Reset the cursor to default first, then override it later
            //Parent control mouse move functions are called after this one
            if (!_mouseDown && !CurrentViewport._grabbing && !CurrentViewport._scrolling && Cursor != Cursors.Default)
            {
                Invalidate();
                Cursor = Cursors.Default;
            }

            if (!Enabled)
            {
                return;
            }

            base.OnMouseMove(e);

            int x = e.X.Clamp(0, Width),
                y = Height - e.Y.Clamp(0, Height);

            if (_draggingViewports)
            {
                float
                    yP = (float) y / Height,
                    xP = (float) x / Width;

                foreach (KeyValuePair<ModelPanelViewport, DragFlags> t in _dragging)
                {
                    //TODO: don't allow the user to drag over another viewport
                    //bool cont = false;
                    bool isX = ((int) t.Value & 1) == 0;
                    float p = isX ? xP : yP;
                    //foreach (ModelPanelViewport v in _viewports)
                    //{
                    //    foreach (var t2 in _dragging)
                    //        if (t2.Key == v)
                    //            cont = true;
                    //    if (cont)
                    //    {
                    //        cont = false;
                    //        continue;
                    //    }
                    //    float ep = v.Percentages[(int)t.Value];
                    //    if (ep > p)
                    //    {
                    //        cont = true;
                    //        break;
                    //    }
                    //}
                    //if (cont)
                    //    continue;
                    t.Key.SetPercentageIndex((int) t.Value, p);
                }

                Invalidate();
            }
            else if (_viewports.Count > 1)
            {
                _dragging.Clear();
                int range = 8;
                bool xd = false, yd = false;

                Func<DragFlags, Rectangle, bool> inRange = (flag, region) =>
                {
                    switch (flag)
                    {
                        case DragFlags.xMin:
                            return xd = Math.Abs(x - region.X) <= range && x > range;
                        case DragFlags.xMax:
                            int xT = region.X + region.Width;
                            return xd = Math.Abs(x - xT) <= range && Math.Abs(Width - xT) > range;
                        case DragFlags.yMin:
                            return yd = Math.Abs(y - region.Y) <= range && y > range;
                        case DragFlags.yMax:
                            int yT = region.Y + region.Height;
                            return yd = Math.Abs(y - yT) <= range && Math.Abs(Height - yT) > range;
                    }

                    return false;
                };
                Action<ModelPanelViewport, DragFlags> add = (w, flag) =>
                {
                    _dragging.Add(new KeyValuePair<ModelPanelViewport, DragFlags>(w, flag));
                };

                //TODO: allow the user to drag only the viewports that share a line,
                //but make sure they can't break up the line (viewports must take up all of the screen)
                //At the moment, you can only drag a whole line on the screen
                foreach (ModelPanelViewport w in _viewports)
                {
                    //if (y > w.Region.Y - range && y < w.Region.Y + w.Region.Height + range)
                    if (inRange(DragFlags.xMin, w.Region))
                    {
                        add(w, DragFlags.xMin);
                    }
                    else if (inRange(DragFlags.xMax, w.Region))
                    {
                        add(w, DragFlags.xMax);
                    }

                    //if (x > w.Region.X - range && x < w.Region.X + w.Region.Width + range)
                    if (inRange(DragFlags.yMin, w.Region))
                    {
                        add(w, DragFlags.yMin);
                    }
                    else if (inRange(DragFlags.yMax, w.Region))
                    {
                        add(w, DragFlags.yMax);
                    }
                }

                if (xd && yd)
                {
                    Cursor = Cursors.SizeAll;
                }
                else if (xd)
                {
                    Cursor = Cursors.SizeWE;
                }
                else if (yd)
                {
                    Cursor = Cursors.SizeNS;
                }
                else
                {
                    CurrentViewport.HandleMouseMove(_ctx, e);
                }
            }
            else
            {
                CurrentViewport.HandleMouseMove(_ctx, e);
            }
        }

        #endregion

        #region Keys

        protected override bool IsInputKey(Keys keyData)
        {
            keyData &= (Keys) 0xFFFF;
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return true;

                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            CurrentViewport.HandleKeyUp(e);
        }

        public delegate bool KeyMessageEventHandler(ref Message m);

        public KeyMessageEventHandler EventProcessKeyMessage;

        protected override bool ProcessKeyMessage(ref Message m)
        {
            if (EventProcessKeyMessage != null)
            {
                EventProcessKeyMessage(ref m);
            }
            else if (Enabled && m.Msg == 0x100)
            {
                if (CurrentViewport.ProcessKeys((Keys) m.WParam, ModifierKeys))
                {
                    return true;
                }
            }

            return base.ProcessKeyMessage(ref m);
        }

        #endregion

        #region OpenGL

        internal override void OnInit(TKContext ctx)
        {
            Vector3 v = (Vector3) BackColor;
            GL.ClearColor(v._x, v._y, v._z, 0.0f);
            GL.ClearDepth(1.0);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.ShadeModel(ShadingModel.Smooth);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.GenerateMipmapHint, HintMode.Nicest);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.1f);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.PointSize(3.0f);
            GL.LineWidth(2.0f);
            GL.Disable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.PolygonSmooth);
            GL.Disable(EnableCap.LineSmooth);

            GL.Enable(EnableCap.Normalize);

            //Set client states
            ctx._states["_Node_Refs"] = _resourceList;
        }

        public List<CollisionNode> _collisions = new List<CollisionNode>();

        protected override void OnRenderViewport(PaintEventArgs e, GLViewport v)
        {
            ModelPanelViewport viewport = v as ModelPanelViewport;

            Rectangle r = viewport.Region;

            GL.Viewport(r);
            GL.Enable(EnableCap.ScissorTest);
            GL.Scissor(r.X, r.Y, r.Width, r.Height);

            viewport.RenderBackground();

            viewport.Camera.LoadProjection();
            viewport.Camera.LoadModelView();

            viewport.RecalcLight();

            PreRender?.Invoke(viewport);

            GL.PushAttrib(AttribMask.AllAttribBits);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.DepthTest);

            //Allow each model to set up any data specific to this frame and viewport
            foreach (IRenderedObject o in _renderList)
            {
                o.PreRender(viewport);
            }

            //Render objects in specifically sorted order
            foreach (DrawCallBase call in _drawCalls)
            {
                call.Render(viewport);
            }

            if (viewport._renderAttrib._renderShaders)
            {
                GL.UseProgram(0);
            }

            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();

            GL.PopAttrib();

            PostRender?.Invoke(viewport);

            viewport.RenderForeground(v == CurrentViewport, _viewports.Count == 1);

            foreach (CollisionNode c in _collisions)
            {
                foreach (CollisionObject o in c.Children)
                {
                    o._render = true;
                }

                c.Render();
            }

            GL.Disable(EnableCap.ScissorTest);
        }

        public Bitmap GetScreenshot(Rectangle region, bool withTransparency)
        {
            GL.ReadBuffer(ReadBufferMode.Front);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            Bitmap bmp = new Bitmap(region.Width, region.Height);
            BitmapData data;
            if (withTransparency)
            {
                data = bmp.LockBits(new Rectangle(0, 0, region.Width, region.Height), ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);
                GL.ReadPixels(region.X, region.Y, region.Width, region.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte, data.Scan0);
            }
            else
            {
                data = bmp.LockBits(new Rectangle(0, 0, region.Width, region.Height), ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
                GL.ReadPixels(region.X, region.Y, region.Width, region.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
                    PixelType.UnsignedByte, data.Scan0);
            }

            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        #endregion

        #region Events

        private void _folderWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            RefreshReferences();
        }

        private void _folderWatcher_Error(object sender, ErrorEventArgs e)
        {
            RefreshReferences();
        }

        private void _folderWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            RefreshReferences();
        }

        protected override void OnContextChanged(bool isNowCurrent)
        {
            base.OnContextChanged(isNowCurrent);

            MDL0TextureNode._folderWatcher.SynchronizingObject = isNowCurrent ? this : null;
        }

        public delegate void RenderStateEvent(ModelPanel panel, bool value);

        public event RenderStateEvent
            RenderFloorChanged,
            RenderMetalsChanged,
            FirstPersonCameraChanged,
            RenderBonesChanged,
            RenderModelBoxChanged,
            RenderObjectBoxChanged,
            RenderVisBoneBoxChanged,
            RenderOffscreenChanged,
            RenderVerticesChanged,
            RenderNormalsChanged,
            RenderPolygonsChanged,
            RenderWireframeChanged,
            UseBindStateBoxesChanged,
            ApplyBillboardBonesChanged,
            RenderShadersChanged,
            ScaleBonesChanged;

        #endregion

        #region Property

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FirstPersonCamera
        {
            get => CurrentViewport._firstPersonCamera;
            set
            {
                CurrentViewport._firstPersonCamera = value;

                Invalidate();

                FirstPersonCameraChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderFloor
        {
            get => CurrentViewport._renderFloor;
            set
            {
                CurrentViewport._renderFloor = value;

                Invalidate();

                RenderFloorChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderMetals
        {
            get => CurrentViewport._renderAttrib._renderMetal;
            set
            {
                CurrentViewport._renderAttrib._renderMetal = value;

                Invalidate();

                RenderMetalsChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderBones
        {
            get => CurrentViewport._renderAttrib._renderBones;
            set
            {
                CurrentViewport._renderAttrib._renderBones = value;

                Invalidate();

                RenderBonesChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderVertices
        {
            get => CurrentViewport._renderAttrib._renderVertices;
            set
            {
                CurrentViewport._renderAttrib._renderVertices = value;

                Invalidate();

                RenderVerticesChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderNormals
        {
            get => CurrentViewport._renderAttrib._renderNormals;
            set
            {
                CurrentViewport._renderAttrib._renderNormals = value;

                Invalidate();

                RenderNormalsChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderPolygons
        {
            get => CurrentViewport._renderAttrib._renderPolygons;
            set
            {
                CurrentViewport._renderAttrib._renderPolygons = value;

                Invalidate();

                RenderPolygonsChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderWireframe
        {
            get => CurrentViewport._renderAttrib._renderWireframe;
            set
            {
                CurrentViewport._renderAttrib._renderWireframe = value;

                Invalidate();

                RenderWireframeChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderShaders
        {
            get => CurrentViewport._renderAttrib._renderShaders;
            set
            {
                CurrentViewport._renderAttrib._renderShaders = value;

                Invalidate();

                RenderShadersChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderModelBox
        {
            get => CurrentViewport._renderAttrib._renderModelBox;
            set
            {
                CurrentViewport._renderAttrib._renderModelBox = value;

                Invalidate();

                RenderModelBoxChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderObjectBox
        {
            get => CurrentViewport._renderAttrib._renderObjectBoxes;
            set
            {
                CurrentViewport._renderAttrib._renderObjectBoxes = value;

                Invalidate();

                RenderObjectBoxChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderVisBoneBox
        {
            get => CurrentViewport._renderAttrib._renderBoneBoxes;
            set
            {
                CurrentViewport._renderAttrib._renderBoneBoxes = value;

                Invalidate();

                RenderVisBoneBoxChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseBindStateBoxes
        {
            get => CurrentViewport._renderAttrib._useBindStateBoxes;
            set
            {
                CurrentViewport._renderAttrib._useBindStateBoxes = value;

                Invalidate();

                UseBindStateBoxesChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DontRenderOffscreen
        {
            get => CurrentViewport._renderAttrib._dontRenderOffscreen;
            set
            {
                CurrentViewport._renderAttrib._dontRenderOffscreen = value;

                Invalidate();

                RenderOffscreenChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ApplyBillboardBones
        {
            get => CurrentViewport._renderAttrib._applyBillboardBones;
            set
            {
                CurrentViewport._renderAttrib._applyBillboardBones = value;

                Invalidate();

                ApplyBillboardBonesChanged?.Invoke(this, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ScaleBones
        {
            get => CurrentViewport._renderAttrib._scaleBones;
            set
            {
                CurrentViewport._renderAttrib._scaleBones = value;

                Invalidate();

                ScaleBonesChanged?.Invoke(this, value);
            }
        }

        #endregion

        #region Models

        public static List<IModel> CollectModels(ResourceNode node)
        {
            List<IModel> models = new List<IModel>();
            GetModelsRecursive(node, models);
            return models;
        }

        private static void GetModelsRecursive(ResourceNode node, List<IModel> models)
        {
            switch (node.ResourceFileType)
            {
                case ResourceType.ARC:
                case ResourceType.RARC:
                case ResourceType.RARCFolder:
                case ResourceType.MRG:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                case ResourceType.U8:
                case ResourceType.U8Folder:
                    foreach (ResourceNode n in node.Children)
                    {
                        GetModelsRecursive(n, models);
                    }

                    break;
                case ResourceType.MDL0:
                case ResourceType.BMD:
                    models.Add((IModel) node);
                    break;
            }
        }

        #endregion
    }
}
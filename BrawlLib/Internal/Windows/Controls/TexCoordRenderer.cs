using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public unsafe partial class TexCoordRenderer : GLPanel
    {
        public TexCoordRenderer()
        {
            InitializeComponent();
            CurrentViewport.ViewType = ViewportProjection.Front;
        }

        internal bool _updating = false;

        private List<ResourceNode> _attached;

        private List<RenderInfo> _renderInfo;
        private List<int> _uvSetIndices, _objIndices;
        public int _uvIndex = -1, _objIndex = -1;

        private MDL0MaterialRefNode _targetMatRef;
        private MDL0TextureNode Tex0 => _targetMatRef == null ? null : _targetMatRef.TextureNode;
        private GLTexture GLTex => Tex0 == null ? null : Tex0.Texture;

        public BindingList<string> UVSetNames => _uvSetNames;
        private readonly BindingList<string> _uvSetNames = new BindingList<string>();
        public BindingList<string> ObjectNames => _objNames;
        private readonly BindingList<string> _objNames = new BindingList<string>();

        public MDL0MaterialRefNode TargetNode
        {
            get => _targetMatRef;
            set => SetTarget(value);
        }

        private void SetTarget(MDL0MaterialRefNode texture)
        {
            if (_targetMatRef != texture && texture != null)
            {
                CurrentViewport.Camera.Reset();
            }

            if (_attached == null)
            {
                _attached = new List<ResourceNode>();
            }

            Tex0?.Unbind();

            _attached.Clear();

            _targetMatRef = texture;
            if (_targetMatRef == null)
            {
                return;
            }

            _attached.Add(_targetMatRef);

            Tex0?.Prepare(_targetMatRef, -1);

            //Dispose of all old UV buffers
            if (_renderInfo != null)
            {
                foreach (RenderInfo info in _renderInfo)
                {
                    info._renderBuffer?.Dispose();
                }
            }

            //Recreate lists
            _objIndices = new List<int>();
            _uvSetIndices = new List<int>();
            _renderInfo = new List<RenderInfo>();
            _objNames.Clear();
            _uvSetNames.Clear();

            int coordID = _targetMatRef.TextureCoordId;
            if (coordID < 0)
            {
                _objNames.Add("None");
                _uvSetNames.Add("None");
                Invalidate();
                return;
            }

            if (_targetMatRef.Material.Objects.Length > 1)
            {
                _objNames.Add("All");

                //TODO: need to change the UV dropdown box to show only texcoords 0-7 that exist
                //not the uv set names
                coordID = -1;
            }

            Vector2 min = new Vector2(float.MaxValue);
            Vector2 max = new Vector2(float.MinValue);

            foreach (MDL0ObjectNode obj in _targetMatRef.Material.Objects)
            {
                _objNames.Add(obj.Name);
                RenderInfo info = new RenderInfo(obj._manager);
                _renderInfo.Add(info);
                min.Min(info._min);
                max.Max(info._max);
            }

            if (_objNames.Count == 0)
            {
                _objNames.Add("None");
                _uvSetNames.Add("None");
                Invalidate();
                return;
            }

            //TODO: zoom the camera out to the nearest whole texture repetition
            //to display all texture coordinates.

            //Change from [0, 1] to [-0.5, 0.5]
            //min -= 0.5f;
            //max -= 0.5f;

            //float xCorrect = 1.0f, yCorrect = 1.0f;
            //GLTexture t = _targetMatRef.TextureNode.Texture;
            //float texWidth = t.Width;
            //float texHeight = t.Height;
            //float tAspect = (float)texWidth / texHeight;
            //float wAspect = (float)Width / Height;

            //int minXmult = (int)(min._x + (min._x < 0 ? -1 : 0));
            //int maxXmult = (int)(max._x + (max._x < 0 ? 0 : 1));
            //int minYmult = (int)(min._y + (min._y < 0 ? -1 : 0));
            //int maxYmult = (int)(max._y + (max._y < 0 ? 0 : 1));

            //if (tAspect > wAspect)
            //{
            //    yCorrect = tAspect / wAspect;
            //}
            //else
            //{
            //    xCorrect = wAspect / tAspect;
            //}

            //CurrentViewport.Camera.Translate(
            //    ((float)(minXmult + maxXmult) / 2.0f - 0.5f) / xCorrect * Width,
            //    ((float)(minYmult + maxYmult) / 2.0f - 0.5f) / yCorrect * -Height, 0);
            //float scale = Math.Max(maxXmult - minXmult, maxYmult - minYmult);
            //CurrentViewport.Camera.Scale(scale, scale, scale);

            SetObjectIndex(-1);
            SetUVIndex(coordID);
        }

        public event EventHandler UVIndexChanged, ObjIndexChanged;

        public void SetUVIndex(int index)
        {
            _uvIndex = _uvSetNames.Count == 1 ? 0 : index >= 0 ? index.Clamp(0, _uvSetNames.Count - 2) : -1;
            UVIndexChanged?.Invoke(this, EventArgs.Empty);

            UpdateDisplay();
        }

        public void SetObjectIndex(int index)
        {
            _uvSetIndices.Clear();
            if ((_objIndex = _objNames.Count == 1 ? 0 : index >= 0 ? index.Clamp(0, _objNames.Count - 2) : -1) >= 0)
            {
                if (_targetMatRef.Material.Objects.Length != 0)
                {
                    foreach (MDL0UVNode uv in _targetMatRef.Material.Objects[_objIndex]._uvSet)
                    {
                        if (uv != null)
                        {
                            _uvSetIndices.Add(uv.Index);
                        }
                    }
                }
            }
            else
            {
                foreach (MDL0ObjectNode obj in _targetMatRef.Material.Objects)
                {
                    foreach (MDL0UVNode uv in obj._uvSet)
                    {
                        if (uv != null)
                        {
                            _uvSetIndices.Add(uv.Index);
                        }
                    }
                }
            }

            if (_targetMatRef != null)
            {
                MDL0Node model = _targetMatRef.Model;
                string name = null;
                if (_uvSetNames.Count > 0 && _uvIndex >= 0 && _uvIndex < _uvSetNames.Count)
                {
                    name = _uvSetNames[_uvIndex];
                }

                _uvSetNames.Clear();
                _uvSetNames.Add(_uvSetIndices.Count == 1 ? model._uvList[_uvSetIndices[0]].Name : "All");
                if (model?._uvList != null && _uvSetIndices.Count != 1)
                {
                    foreach (int i in _uvSetIndices)
                    {
                        _uvSetNames.Add(model._uvList[i].Name);
                    }
                }

                SetUVIndex(string.IsNullOrEmpty(name) ? _uvSetNames.IndexOf(name) : -1);
            }

            ObjIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateDisplay()
        {
            bool singleObj = _objIndex >= 0;
            bool singleUV = _uvIndex >= 0;
            int r = 0;
            for (int i = 0; i < _renderInfo.Count; i++)
            {
                RenderInfo info = _renderInfo[i];
                info._dirty = true;
                if (singleObj)
                {
                    if (i != _objIndex)
                    {
                        info._isEnabled = false;
                        info._enabled = new bool[8];
                    }
                    else
                    {
                        info._isEnabled = true;
                        info._enabled = new bool[8];
                        for (int x = 0; x < 8; x++)
                        {
                            if (info._manager?._faceData[x + 4] != null)
                            {
                                info._enabled[x] = !singleUV || _uvIndex == x;
                            }
                        }
                    }
                }
                else
                {
                    info._isEnabled = true;
                    info._enabled = new bool[8];
                    for (int x = 0; x < 8; x++)
                    {
                        if (info._manager?._faceData[x + 4] != null)
                        {
                            info._enabled[x] = !singleUV || _uvIndex == r++;
                        }
                    }
                }
            }

            Invalidate();
        }

        internal override void OnInit(TKContext ctx)
        {
            //Set caps
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            _attached = new List<ResourceNode>();
            ctx._states["_Node_Refs"] = _attached;
        }

        public void Render(GLCamera cam, bool renderBG)
        {
            cam.LoadProjection();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Color4(Color.White);
            GL.Enable(EnableCap.Texture2D);

            float
                halfW = Width / 2.0f,
                halfH = Height / 2.0f;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();

            if (renderBG)
            {
                GL.PushAttrib(AttribMask.TextureBit);

                GLTexture bgTex = TKContext.FindOrCreate("TexBG", GLTexturePanel.CreateBG);
                bgTex.Bind();

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                    (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                    (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int) TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int) TextureMagFilter.Nearest);

                float
                    s = Width / (float) bgTex.Width,
                    t = Height / (float) bgTex.Height;

                GL.Begin(BeginMode.Quads);

                GL.TexCoord2(0.0f, 0.0f);
                GL.Vertex2(-halfW, -halfH);
                GL.TexCoord2(s, 0.0f);
                GL.Vertex2(halfW, -halfH);
                GL.TexCoord2(s, t);
                GL.Vertex2(halfW, halfH);
                GL.TexCoord2(0.0f, t);
                GL.Vertex2(-halfW, halfH);

                GL.End();

                GL.PopAttrib();
            }

            if (Tex0 == null)
            {
                return;
            }

            Tex0.Prepare(_targetMatRef, -1);
            GLTexture texture = GLTex;
            if (texture == null || texture._texId <= 0)
            {
                return;
            }

            MDL0TextureNode.ApplyGLTextureParameters(_targetMatRef);

            //These are used to match up the UV overlay to the texture underneath
            Vector2 topLeft = new Vector2();
            Vector2 bottomRight = new Vector2();

            float texWidth = texture.Width;
            float texHeight = texture.Height;

            float tAspect = texWidth / texHeight;
            float wAspect = (float) Width / Height;

            float[] texCoord = new float[8];

            //These are used to compensate for padding added on an axis
            float xCorrect = 1.0f, yCorrect = 1.0f;
            //if (tAspect > wAspect)
            //{
            //    //Texture is wider, use horizontal fit
            //    //X touches the edges of the window, Y has top and bottom padding
            //
            //    //X
            //    texCoord[0] = texCoord[6] = 0.0f;
            //    texCoord[2] = texCoord[4] = 1.0f;
            //
            //    //Y
            //    texCoord[1] = texCoord[3] = (yCorrect = tAspect / wAspect) / 2.0f + 0.5f;
            //    texCoord[5] = texCoord[7] = 1.0f - texCoord[1];
            //
            //    bottomRight = new Vector2(halfW,
            //        ((Height - Width / texWidth * texHeight) / Height / 2.0f - 0.5f) * Height);
            //    topLeft = new Vector2(-halfW, -bottomRight._y);
            //}
            //else
            //{
                //Window is wider, use vertical fit
                //Y touches the edges of the window, X has left and right padding

                //Y
                texCoord[1] = texCoord[3] = 1.0f;
                texCoord[5] = texCoord[7] = 0.0f;

                //X
                texCoord[2] = texCoord[4] = (xCorrect = wAspect / tAspect) / 2.0f + 0.5f;
                texCoord[0] = texCoord[6] = 1.0f - texCoord[2];

                bottomRight =
                    new Vector2(1.0f - ((Width - Height / texHeight * texWidth) / Width / 2.0f - 0.5f) * Width, -halfH);
                topLeft = new Vector2(-bottomRight._x, halfH);
            //}

            //Apply the texcoord bind transform first
            TextureFrameState state = _targetMatRef._bindState;
            GL.MultMatrix((float*) &state._transform);

            //Translate the texture coordinates to match where the user dragged the camera
            //Divide by width and height to convert window units (0 to w, 0 to h) to texcoord units (0 to 1)
            //Then multiply by the correction value if the window is bigger than the texture on an axis
            Vector3 point = cam.GetPoint();
            GL.Translate(point._x / Width * xCorrect, -point._y / Height * yCorrect, 0);

            //Now to scale the texture after translating.
            //The scale origin is the top left of the texture on the window (not of the window itself),
            //so we need to translate the center of the texture to that origin, 
            //scale it up or down, then translate it back to where it was.
            OpenTK.Vector3 trans =
                new OpenTK.Vector3(-topLeft._x / Width * xCorrect, topLeft._y / Height * yCorrect, 0.0f);
            GL.Translate(trans);
            GL.Scale((OpenTK.Vector3) cam._scale);
            GL.Translate(-trans);

            //Bind the material ref's texture
            GL.BindTexture(TextureTarget.Texture2D, texture._texId);

            //Draw a quad across the screen and render the texture with the calculated texcoords
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(texCoord[0], texCoord[1]);
            GL.Vertex2(-halfW, -halfH);
            GL.TexCoord2(texCoord[2], texCoord[3]);
            GL.Vertex2(halfW, -halfH);
            GL.TexCoord2(texCoord[4], texCoord[5]);
            GL.Vertex2(halfW, halfH);
            GL.TexCoord2(texCoord[6], texCoord[7]);
            GL.Vertex2(-halfW, halfH);

            GL.End();
            GL.Disable(EnableCap.Texture2D);

            //Now load the camera transform and draw the UV overlay over the texture
            cam.LoadModelView();

            //Color the lines limegreen, a bright color that probably won't be in a texture
            GL.Color4(Color.LimeGreen);

            Vector2 mdlScale = new Vector2(bottomRight._x - topLeft._x, bottomRight._y - topLeft._y);
            GL.Translate(topLeft._x, topLeft._y, 0.0f);
            GL.Scale(mdlScale._x, mdlScale._y, 1.0f);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(1);

            //Render texture coordinates as vertex points
            foreach (RenderInfo info in _renderInfo)
            {
                info.PrepareStream();
            }
        }

        protected override void OnRender(PaintEventArgs e)
        {
            Rectangle r = CurrentViewport.Region;

            GL.Viewport(r);
            GL.Enable(EnableCap.ScissorTest);
            GL.Scissor(r.X, r.Y, r.Width, r.Height);

            Render(CurrentViewport.Camera, true);

            GL.Disable(EnableCap.ScissorTest);
        }

        private bool _grabbing;
        private int _lastX, _lastY;
        private readonly float _transFactor = 0.05f;
        private readonly float _zoomFactor = 2.5f;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            Zoom(-e.Delta / 120.0f * _zoomFactor, e.X, e.Y);

            base.OnMouseWheel(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_ctx != null && _grabbing)
            {
                lock (_ctx)
                {
                    int xDiff = e.X - _lastX;
                    int yDiff = e.Y - _lastY;

                    Translate(
                        -xDiff * _transFactor,
                        yDiff * _transFactor,
                        0.0f);
                }
            }

            _lastX = e.X;
            _lastY = e.Y;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _grabbing = true;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _grabbing)
            {
                _grabbing = false;
                Invalidate();
            }
        }

        private void Translate(float x, float y, float z)
        {
            if (CurrentViewport.Camera._ortho)
            {
                x *= 20.0f;
                y *= 20.0f;
            }

            CurrentViewport.Camera.Translate(x, y, z);
            Invalidate();
        }

        private void Zoom(float amt, float originX, float originY)
        {
            float scale = amt >= 0 ? amt / 2.0f : 2.0f / -amt;
            Scale(scale, scale, scale);
        }

        private void Scale(float x, float y, float z)
        {
            CurrentViewport.Camera.Scale(x, y, z);
            Invalidate();
        }

        protected override bool ProcessKeyMessage(ref Message m)
        {
            if (!Enabled)
            {
                return false;
            }

            if (m.Msg == 0x100)
            {
                Keys mod = ModifierKeys;
                bool ctrl = (mod & Keys.Control) != 0;
                bool shift = (mod & Keys.Shift) != 0;
                bool alt = (mod & Keys.Alt) != 0;
                switch ((Keys) m.WParam)
                {
                    case Keys.NumPad8:
                    case Keys.Up:
                    {
                        Translate(0.0f, -_transFactor * 8, 0.0f);
                        return true;
                    }

                    case Keys.NumPad2:
                    case Keys.Down:
                    {
                        Translate(0.0f, _transFactor * 8, 0.0f);
                        return true;
                    }

                    case Keys.NumPad6:
                    case Keys.Right:
                    {
                        Translate(_transFactor * 8, 0.0f, 0.0f);
                        return true;
                    }

                    case Keys.NumPad4:
                    case Keys.Left:
                    {
                        Translate(-_transFactor * 8, 0.0f, 0.0f);
                        return true;
                    }

                    case Keys.Add:
                    case Keys.Oemplus:
                    {
                        Zoom(-_zoomFactor, 0, 0);
                        return true;
                    }

                    case Keys.Subtract:
                    case Keys.OemMinus:
                    {
                        Zoom(_zoomFactor, 0, 0);
                        return true;
                    }
                }
            }

            return base.ProcessKeyMessage(ref m);
        }

        private class RenderInfo
        {
            public RenderInfo(PrimitiveManager manager)
            {
                _manager = manager;
                for (int i = 0; i < 8; i++)
                {
                    _enabled[i] = true;
                }

                _isEnabled = true;
                CalcMinMax();
            }

            public bool _isEnabled;
            public int _stride;
            public PrimitiveManager _manager;
            public UnsafeBuffer _renderBuffer;
            public bool[] _enabled = new bool[8];
            public bool _dirty;

            public Vector2 _min, _max;

            public void CalcMinMax()
            {
                _min = new Vector2(float.MaxValue);
                _max = new Vector2(float.MinValue);
                if (_manager?._faceData != null)
                {
                    for (int i = 4; i < _manager._faceData.Length; i++)
                    {
                        if (_manager._faceData[i] != null && _enabled[i - 4])
                        {
                            Vector2* pSrc = (Vector2*) _manager._faceData[i].Address;
                            for (int x = 0; x < _manager._pointCount; x++, pSrc++)
                            {
                                _min.Min(*pSrc);
                                _max.Max(*pSrc);
                            }
                        }
                    }
                }
            }

            public void CalcStride()
            {
                _stride = 0;
                if (_isEnabled)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (_manager._faceData[i + 4] != null && _enabled[i])
                        {
                            _stride += 8;
                        }
                    }
                }
            }

            public void PrepareStream()
            {
                if (!_isEnabled)
                {
                    return;
                }

                CalcStride();
                int bufferSize = _manager._pointCount * _stride;

                if (_renderBuffer != null && _renderBuffer.Length != bufferSize)
                {
                    _renderBuffer.Dispose();
                    _renderBuffer = null;
                }

                if (bufferSize <= 0)
                {
                    return;
                }

                if (_renderBuffer == null)
                {
                    _renderBuffer = new UnsafeBuffer(bufferSize);
                    _dirty = true;
                }

                if (_dirty)
                {
                    _dirty = false;
                    for (int i = 0; i < 8; i++)
                    {
                        UpdateStream(i);
                    }
                }

                GL.EnableClientState(ArrayCap.VertexArray);

                Vector2* pData = (Vector2*) _renderBuffer.Address;
                for (int i = 4; i < _manager._faceData.Length; i++)
                {
                    if (_manager._faceData[i] != null && _enabled[i - 4])
                    {
                        GL.VertexPointer(2, VertexPointerType.Float, _stride, (IntPtr) pData++);
                    }
                }

                _manager._triangles?.Render();

                _manager._lines?.Render();

                _manager._points?.Render();

                GL.DisableClientState(ArrayCap.VertexArray);
            }

            private void UpdateStream(int index)
            {
                index += 4;
                if (_manager._faceData[index] == null || !_enabled[index - 4])
                {
                    return;
                }

                //Set starting address
                byte* pDst = (byte*) _renderBuffer.Address;
                for (int i = 4; i < index; i++)
                {
                    if (_manager._faceData[i] != null && _enabled[i - 4])
                    {
                        pDst += 8;
                    }
                }

                Vector2* pSrc = (Vector2*) _manager._faceData[index].Address;
                for (int i = 0; i < _manager._pointCount; i++, pDst += _stride)
                {
                    *(Vector2*) pDst = *pSrc++;
                }
            }
        }
    }
}
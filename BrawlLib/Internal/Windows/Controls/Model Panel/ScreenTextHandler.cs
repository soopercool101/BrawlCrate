using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrawlLib.Internal.Windows.Controls.Model_Panel
{
    public class ScreenTextHandler
    {
        private class TextData
        {
            public string _string;
            public List<Vector3> _positions = new List<Vector3>();
        }

        public static int _fontSize = 12;
        private static readonly Font _textFont = new Font("Arial", _fontSize);
        private readonly ModelPanelViewport _viewport;
        private readonly Dictionary<string, TextData> _text = new Dictionary<string, TextData>();
        public int Count => _text.Count;

        private Size _size;
        private Bitmap _bitmap;
        private int _texId = -1;

        public Vector3 this[string text]
        {
            set
            {
                if (!_text.ContainsKey(text))
                {
                    _text.Add(text, new TextData {_string = text});
                }

                _text[text]._positions.Add(value);
            }
        }

        public ScreenTextHandler(ModelPanelViewport viewport)
        {
            _text = new Dictionary<string, TextData>();
            _viewport = viewport;
        }

        public void Clear()
        {
            _text.Clear();
        }

        public void Draw()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);

            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float) TextureEnvMode.Replace);

            if (_size != _viewport.Region.Size ||
                _viewport.Region.Size.Width == 0 ||
                _viewport.Region.Size.Height == 0)
            {
                _size = _viewport.Region.Size;

                _bitmap?.Dispose();

                if (_texId != -1)
                {
                    GL.DeleteTexture(_texId);
                    _texId = -1;
                }

                if (_size.Width == 0 || _size.Height == 0)
                {
                    return;
                }

                //Create a texture over the whole model panel
                _bitmap = new Bitmap(_size.Width, _size.Height);

                _bitmap.MakeTransparent();

                _texId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, _texId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) All.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) All.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _bitmap.Width, _bitmap.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            }

            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                g.Clear(Color.Transparent);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                List<Vector2> _used = new List<Vector2>();

                foreach (TextData d in _text.Values)
                {
                    foreach (Vector3 v in d._positions)
                    {
                        if (v._x + d._string.Length * 10 > 0 && v._x < _viewport.Width &&
                            v._y > -10.0f && v._y < _viewport.Height &&
                            v._z > 0 && v._z < 1 && //near and far depth values
                            !_used.Contains(new Vector2(v._x, v._y)))
                        {
                            g.DrawString(d._string, _textFont, Brushes.Black, new PointF(v._x, v._y));
                            _used.Add(new Vector2(v._x, v._y));
                        }
                    }
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, _texId);

            BitmapData data = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, _bitmap.Width, _bitmap.Height,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            _bitmap.UnlockBits(data);

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(0.0f, 0.0f);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(_size.Width, 0.0f);

            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(_size.Width, _size.Height);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(0.0f, _size.Height);

            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
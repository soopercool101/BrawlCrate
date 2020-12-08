using BrawlLib.Imaging;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class GLTexturePanel : GLPanel
    {
        private GLTexture _currentTexture;

        public GLTexture Texture
        {
            get => _currentTexture;
            set
            {
                if (_currentTexture == value)
                {
                    return;
                }

                _currentTexture = value;
            }
        }

        internal override void OnInit(TKContext ctx)
        {
            //Set caps
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            UpdateProjection();
        }

        protected override void OnRender(PaintEventArgs e)
        {
            RenderBGTexture(Width, Height, _currentTexture);
        }

        protected override void OnResize(EventArgs e)
        {
            _ctx?.Update();

            UpdateProjection();

            Invalidate();
        }

        public void UpdateProjection()
        {
            Capture();

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0f, 1.0f, 1.0f, 0.0f, -0.1f, 1.0f);
        }

        public static void RenderBGTexture(int width, int height, GLTexture texture)
        {
            float[] points = new float[8];
            RenderBGTexture(width, height, texture, ref points);
        }

        public static void RenderBGTexture(int width, int height, GLTexture texture, ref float[] points)
        {
            GLTexture bgTex = TKContext.FindOrCreate("TexBG", CreateBG);
            bgTex.Bind();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);

            //Draw BG
            float s = width / (float) bgTex.Width, t = height / (float) bgTex.Height;

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(0.0f, 0.0f);
            GL.TexCoord2(s, 0.0f);
            GL.Vertex2(1.0, 0.0f);
            GL.TexCoord2(s, t);
            GL.Vertex2(1.0, 1.0);
            GL.TexCoord2(0, t);
            GL.Vertex2(0.0f, 1.0);

            GL.End();

            //Draw texture
            if (texture != null && texture._texId != 0)
            {
                float tAspect = (float) texture.Width / texture.Height;
                float wAspect = (float) width / height;

                if (tAspect > wAspect) //Texture is wider, use horizontal fit
                {
                    points[0] = points[6] = 0.0f;
                    points[2] = points[4] = 1.0f;

                    points[1] = points[3] = (height - (float) width / texture.Width * texture.Height) / height / 2.0f;
                    points[5] = points[7] = 1.0f - points[1];
                }
                else
                {
                    points[1] = points[3] = 0.0f;
                    points[5] = points[7] = 1.0f;

                    points[0] = points[6] = (width - (float) height / texture.Height * texture.Width) / width / 2.0f;
                    points[2] = points[4] = 1.0f - points[0];
                }

                GL.BindTexture(TextureTarget.Texture2D, texture._texId);

                GL.Begin(BeginMode.Quads);

                GL.TexCoord2(0.0f, 0.0f);
                GL.Vertex2(points[0], points[1]);
                GL.TexCoord2(1.0f, 0.0f);
                GL.Vertex2(points[2], points[3]);
                GL.TexCoord2(1.0f, 1.0f);
                GL.Vertex2(points[4], points[5]);
                GL.TexCoord2(0.0f, 1.0f);
                GL.Vertex2(points[6], points[7]);

                GL.End();
            }
        }

        public static RGBAPixel _left = new RGBAPixel(192, 192, 192, 255), _right = new RGBAPixel(240, 240, 240, 255);

        public static unsafe GLTexture CreateBG()
        {
            GLTexture tex = new GLTexture(16, 16)
            {
                _texId = GL.GenTexture()
            };
            GL.BindTexture(TextureTarget.Texture2D, tex._texId);

            int* pixelData = stackalloc int[16 * 16];
            RGBAPixel* p = (RGBAPixel*) pixelData;

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    *p++ = (x & 8) == (y & 8) ? _left : _right;
                }
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) MatTextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) MatTextureMagFilter.Nearest);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, 16, 16, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, (VoidPtr) pixelData);

            return tex;
        }
    }
}
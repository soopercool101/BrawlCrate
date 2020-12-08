using BrawlLib.Imaging;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrawlLib.OpenGL
{
    public class GLTexture
    {
        public string _name;
        public int _texId;

        public IImageSource _source;

        private bool _remake = true;
        private Bitmap[] _textures;

        private PixelInternalFormat ifmt = PixelInternalFormat.Four;

        internal int _width, _height;
        public int Width => _width;
        public int Height => _height;

        public GLTexture()
        {
        }

        public GLTexture(int width, int height)
        {
            _width = width;
            _height = height;
            _source = null;
        }

        public GLTexture(Bitmap b)
        {
            _width = b.Width;
            _height = b.Height;
            ClearImages();
            ClearTexture();
            _textures = new Bitmap[] {b};
            _remake = true;
            _source = null;
        }

        public int Initialize()
        {
            if (_remake && _textures != null)
            {
                ClearTexture();

                _texId = GL.GenTexture();

                GL.BindTexture(TextureTarget.Texture2D, _texId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, _textures.Length - 1);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinLod, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLod, _textures.Length - 1);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);

                for (int i = 0; i < _textures.Length; i++)
                {
                    Bitmap bmp = _textures[i];
                    if (bmp != null)
                    {
                        BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, i, ifmt, data.Width, data.Height, 0,
                            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bmp.UnlockBits(data);
                    }
                }

                _remake = false;
                ClearImages();
            }

            return _texId;
        }

        private void ClearImages()
        {
            if (_textures != null)
            {
                foreach (Bitmap bmp in _textures)
                {
                    bmp?.Dispose();
                }

                _textures = null;
            }
        }

        private void ClearTexture()
        {
            if (_texId != 0)
            {
                int id = _texId;
                GL.DeleteTexture(id);
                _texId = 0;
            }
        }

        public void SetPalette(PLT0Node plt)
        {
            if (_source is TEX0Node)
            {
                Attach((TEX0Node) _source, plt);
            }
        }

        public void Attach(TEX0Node tex, PLT0Node plt)
        {
            ClearImages();

            _source = tex;

            _textures = new Bitmap[tex.LevelOfDetail];
            for (int i = 0; i < tex.LevelOfDetail; i++)
            {
                _textures[i] = tex.GetImage(i, plt);
            }

            if (_textures.Length != 0 && _textures[0] != null)
            {
                _width = _textures[0].Width;
                _height = _textures[0].Height;
            }

            switch (tex.Format)
            {
                case WiiPixelFormat.I4:
                case WiiPixelFormat.I8:
                    ifmt = PixelInternalFormat.Intensity;
                    break;
                case WiiPixelFormat.IA4:
                case WiiPixelFormat.IA8:
                    ifmt = PixelInternalFormat.Luminance8Alpha8;
                    break;
                default:
                    ifmt = PixelInternalFormat.Four;
                    break;
            }

            _remake = true;
            Initialize();
        }

        public void Attach(Bitmap bmp)
        {
            ClearImages();

            _source = null;

            _textures = new Bitmap[] {bmp};

            if (_textures.Length != 0)
            {
                _width = _textures[0].Width;
                _height = _textures[0].Height;
            }

            ifmt = PixelInternalFormat.Four;

            _remake = true;
            Initialize();
        }

        public void Default()
        {
            Bitmap b = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics grp = Graphics.FromImage(b))
            {
                grp.FillRectangle(Brushes.White, 0, 0, 32, 32);
            }

            Attach(b);
        }

        public void Bind()
        {
            Bind(-1, -1);
        }

        public void Bind(int index = -1, int program = -1)
        {
            if (program != -1 && index >= 0 && index < 8)
            {
                GL.Uniform1(GL.GetUniformLocation(program, "texture" + index), index);
            }

            GL.BindTexture(TextureTarget.Texture2D, Initialize());
        }

        public void Delete()
        {
            ClearImages();
            ClearTexture();
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms
{
    public partial class TexCoordControl : UserControl
    {
        public TexCoordControl()
        {
            InitializeComponent();

            texCoordRenderer1.UVIndexChanged += texCoordRenderer1_UVIndexChanged;
            texCoordRenderer1.ObjIndexChanged += texCoordRenderer1_ObjIndexChanged;
        }

        private bool _updating;

        private void texCoordRenderer1_ObjIndexChanged(object sender, EventArgs e)
        {
            if (comboObj.Items.Count == 1)
            {
                return;
            }

            if (comboObj.SelectedIndex != texCoordRenderer1._objIndex + 1 && comboObj.Items.Count != 0)
            {
                _updating = true;
                comboObj.SelectedIndex = texCoordRenderer1._objIndex + 1;
                _updating = false;
            }
        }

        private void texCoordRenderer1_UVIndexChanged(object sender, EventArgs e)
        {
            if (comboUVs.Items.Count == 1)
            {
                return;
            }

            if (comboUVs.SelectedIndex != texCoordRenderer1._uvIndex + 1 && comboUVs.Items.Count != 0)
            {
                _updating = true;
                comboUVs.SelectedIndex = texCoordRenderer1._uvIndex + 1;
                _updating = false;
            }
        }

        public MDL0MaterialRefNode TargetNode
        {
            get => texCoordRenderer1.TargetNode;
            set
            {
                if ((texCoordRenderer1.TargetNode = value) != null)
                {
                    comboObj.DataSource = texCoordRenderer1.ObjectNames;
                    comboUVs.DataSource = texCoordRenderer1.UVSetNames;

                    texCoordRenderer1_ObjIndexChanged(null, null);
                    texCoordRenderer1_UVIndexChanged(null, null);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //TODO: get this working

            int height = 512; //temporary - need to calculate width
            int width = 512;  //temporary - need to calculate height


            texCoordRenderer1.Capture();

            GL.GenTextures(1, out uint colorTex);
            GL.BindTexture(TextureTarget.Texture2D, colorTex);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Clamp);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, height, width, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Ext.GenFramebuffers(1, out uint bufferHandle);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, bufferHandle);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext,
                TextureTarget.Texture2D, colorTex, 0);

            GL.DrawBuffer((DrawBufferMode) FramebufferAttachment.ColorAttachment0Ext);

            GL.PushAttrib(AttribMask.ViewportBit);
            GL.Viewport(0, 0, height, width);

            //TODO: Make a copy of the renderer camera here, but with no transformations
            GLCamera cam = new GLCamera
            {
                _projectionMatrix = texCoordRenderer1.CurrentViewport.Camera._projectionMatrix,
                _projectionInverse = texCoordRenderer1.CurrentViewport.Camera._projectionInverse,
                _matrix = Matrix.Identity,
                _matrixInverse = Matrix.Identity
            };
            texCoordRenderer1.Render(cam, false);

            GL.PopAttrib();
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
            GL.DrawBuffer(DrawBufferMode.Back);
        }

        private void comboObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                texCoordRenderer1.SetObjectIndex(comboObj.SelectedIndex - 1);
            }
        }

        private void comboUVs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_updating)
            {
                texCoordRenderer1.SetUVIndex(comboUVs.SelectedIndex - 1);
            }
        }
    }
}
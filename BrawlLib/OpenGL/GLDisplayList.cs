using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace BrawlLib.OpenGL
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GLDisplayList
    {
        public int _id = -1;

        //public GLDisplayList(uint id) { _id = id; }
        public GLDisplayList()
        {
            _id = GL.GenLists(1);
        }

        public void Begin()
        {
            GL.NewList(_id, ListMode.Compile);
        }

        public void Begin(ListMode mode)
        {
            GL.NewList(_id, mode);
        }

        public void End()
        {
            GL.EndList();
        }

        public void Call()
        {
            GL.CallList(_id);
        }

        public void Delete()
        {
            if (_id >= 0)
            {
                GL.DeleteLists(_id, 1);
                _id = 0;
            }
        }
    }
}
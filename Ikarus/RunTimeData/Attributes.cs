using OpenTK.Graphics.OpenGL;
using Ikarus.MovesetFile;

namespace Ikarus
{
    public static unsafe class Attributes
    {
        public static void PreRender()
        {
            AttributeList list = Manager.GetAttributes();
            if (list == null)
                return;

            float size = list.GetFloat(45);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Scale(size, size, size);
        }
        public static void PostRender()
        {
            //AttributeList list = Manager.GetAttributes();
            //if (list == null)
            //    return;


        }
    }
}

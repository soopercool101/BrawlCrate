using System.Collections.Generic;
using BrawlLib.OpenGL;

namespace BrawlLib.Modeling
{
    public interface IObject : IRenderedObject
    {
        List<Vertex3> Vertices { get; }
    }
}
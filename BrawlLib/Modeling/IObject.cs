using BrawlLib.OpenGL;
using System.Collections.Generic;

namespace BrawlLib.Modeling
{
    public interface IObject : IRenderedObject
    {
        List<Vertex3> Vertices { get; }
    }
}
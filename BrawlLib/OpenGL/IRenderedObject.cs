using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrawlLib.Modeling;

namespace BrawlLib.OpenGL
{
    public abstract class DrawCallBase
    {
        public bool _render = true;
        public virtual IObject Parent => null;

        public virtual void Render(ModelPanelViewport viewport)
        {
        }

        public static int Sort(DrawCallBase x, DrawCallBase y)
        {
            return x.CompareTo(y);
        }

        public virtual int CompareTo(DrawCallBase y)
        {
            return 0;
        }

        public virtual void Bind()
        {
        }
    }

    public interface IRenderedObject
    {
        List<DrawCallBase> DrawCalls { get; }
        bool IsRendering { get; set; }
        bool Attached { get; }
        event EventHandler DrawCallsChanged;
        void Attach();
        void Detach();
        void Refresh();
        void PreRender(ModelPanelViewport v);
        Box GetBox();
    }
}
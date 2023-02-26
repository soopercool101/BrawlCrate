using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;

namespace BrawlLib.OpenGL
{
    public abstract class DrawCallBase
    {
        public virtual IObject Parent => null;
        public bool _render = true;

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

    public interface IRenderedLink
    {
        List<ResourceNode> RenderTargets { get; }
    }

    public interface IRenderedObject
    {
        event EventHandler DrawCallsChanged;

        List<DrawCallBase> DrawCalls { get; }
        bool IsRendering { get; set; }
        bool Attached { get; }
        void Attach();
        void Detach();
        void Refresh();
        void PreRender(ModelPanelViewport v);
        Box GetBox();
    }
}
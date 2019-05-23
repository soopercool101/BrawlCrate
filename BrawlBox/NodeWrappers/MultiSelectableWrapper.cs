using System.Windows.Forms;

namespace BrawlBox.NodeWrappers
{
    //Contains generic members inherited by all sub-classed nodes
    public interface MultiSelectableWrapper
    {
        ContextMenuStrip MultiSelectMenuStrip { get; }
    }
}

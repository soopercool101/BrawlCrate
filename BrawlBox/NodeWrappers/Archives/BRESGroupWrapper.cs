using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.BRESGroup)]
    public class BRESGroupWrapper : GenericWrapper
    {
        private static ContextMenuStrip _menu;
        static BRESGroupWrapper() 
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripMenuItem("&Default Name", null, DefaultAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void DefaultAction(object sender, EventArgs e) { GetInstance<BRESGroupWrapper>().Default(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[1].Enabled = _menu.Items[4].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BRESGroupWrapper w = GetInstance<BRESGroupWrapper>();
            _menu.Items[0].Enabled = w.PrevNode != null;
            _menu.Items[1].Enabled = w.NextNode != null;
            _menu.Items[4].Enabled = w.Parent != null;
        }

        public BRESGroupWrapper() { ContextMenuStrip = _menu; }

        public void Default()
        {
            switch (((BRESGroupNode)_resource).Type)
            {
                case BRESGroupNode.BRESGroupType.Textures:
                    ((BRESGroupNode)_resource).Name = "Textures(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.Palettes:
                    ((BRESGroupNode)_resource).Name = "Palettes(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.Models:
                    ((BRESGroupNode)_resource).Name = "3DModels(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.CHR0:
                    ((BRESGroupNode)_resource).Name = "AnmChr(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.CLR0:
                    ((BRESGroupNode)_resource).Name = "AnmClr(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.SRT0:
                    ((BRESGroupNode)_resource).Name = "AnmTexSrt(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.SHP0:
                    ((BRESGroupNode)_resource).Name = "AnmShp(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.VIS0:
                    ((BRESGroupNode)_resource).Name = "AnmVis(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.SCN0:
                    ((BRESGroupNode)_resource).Name = "AnmScn(NW4R)"; break;
                case BRESGroupNode.BRESGroupType.PAT0:
                    ((BRESGroupNode)_resource).Name = "AnmTexPat(NW4R)"; break;
                default:
                    break;
            }
        }
    }
}

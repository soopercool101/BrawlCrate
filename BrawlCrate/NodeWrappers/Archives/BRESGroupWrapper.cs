using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.BRESGroup)]
    public class BRESGroupWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        private static readonly ToolStripMenuItem RestoreToolStripMenuItem =
            new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T);

        private static readonly ToolStripMenuItem MoveUpToolStripMenuItem =
            new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up);

        private static readonly ToolStripMenuItem MoveDownToolStripMenuItem =
            new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down);

        private static readonly ToolStripMenuItem DeleteToolStripMenuItem =
            new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete);

        static BRESGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripMenuItem("&Default Name", null, DefaultAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ReplaceToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BRESGroupWrapper w = GetInstance<BRESGroupWrapper>();

            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        protected static void DefaultAction(object sender, EventArgs e)
        {
            GetInstance<BRESGroupWrapper>().Default();
        }

        public BRESGroupWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void Default()
        {
            switch (((BRESGroupNode) _resource).Type)
            {
                case BRESGroupNode.BRESGroupType.Textures:
                    ((BRESGroupNode) _resource).Name = "Textures(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.Palettes:
                    ((BRESGroupNode) _resource).Name = "Palettes(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.Models:
                    ((BRESGroupNode) _resource).Name = "3DModels(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.CHR0:
                    ((BRESGroupNode) _resource).Name = "AnmChr(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.CLR0:
                    ((BRESGroupNode) _resource).Name = "AnmClr(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.SRT0:
                    ((BRESGroupNode) _resource).Name = "AnmTexSrt(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.SHP0:
                    ((BRESGroupNode) _resource).Name = "AnmShp(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.VIS0:
                    ((BRESGroupNode) _resource).Name = "AnmVis(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.SCN0:
                    ((BRESGroupNode) _resource).Name = "AnmScn(NW4R)";
                    break;
                case BRESGroupNode.BRESGroupType.PAT0:
                    ((BRESGroupNode) _resource).Name = "AnmTexPat(NW4R)";
                    break;
                default:
                    break;
            }
        }
    }
}
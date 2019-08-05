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
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static BRESGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripMenuItem("&Default Name", null, DefaultAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = true;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = true;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = true;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = true;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<BRESGroupWrapper>();

            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
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
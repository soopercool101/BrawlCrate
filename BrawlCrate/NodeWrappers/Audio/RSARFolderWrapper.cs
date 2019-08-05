using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSARFolder)]
    public class RSARFolderWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static RSARFolderWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("New", null,
                new ToolStripMenuItem("Folder", null, NewFolderAction),
                new ToolStripMenuItem("Sound", null,
                    new ToolStripMenuItem("New", null, NewSoundAction),
                    //new ToolStripMenuItem("From File", null, ImportSoundAction),
                    new ToolStripMenuItem("From Existing", null, CopySoundAction)),
                new ToolStripMenuItem("Bank", null, NewBankAction),
                new ToolStripMenuItem("PlayerInfo", null, NewTypeAction),
                new ToolStripMenuItem("Group", null, NewGroupAction)
            ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewFolderAction(object sender, EventArgs e)
        {
            GetInstance<RSARFolderWrapper>().NewFolder();
        }

        protected static void NewSoundAction(object sender, EventArgs e)
        {
            GetInstance<RSARFolderWrapper>().NewSound();
        }

        //protected static void ImportSoundAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().ImportSound(); }
        protected static void CopySoundAction(object sender, EventArgs e)
        {
            GetInstance<RSARFolderWrapper>().CopySound();
        }

        protected static void NewBankAction(object sender, EventArgs e)
        {
            GetInstance<RSARFolderWrapper>().NewBank();
        }

        protected static void NewTypeAction(object sender, EventArgs e)
        {
            GetInstance<RSARFolderWrapper>().NewType();
        }

        protected static void NewGroupAction(object sender, EventArgs e)
        {
            GetInstance<RSARFolderWrapper>().NewGroup();
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
            var w = GetInstance<RSARFolderWrapper>();

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

        #endregion

        public RSARFolderWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewFolder()
        {
            RSARFolderNode node = new RSARFolderNode
            {
                Name = _resource.FindName("NewFolder")
            };
            _resource.AddChild(node);
        }

        public void NewSound()
        {
            RSARSoundNode node = new RSARSoundNode
            {
                Name = _resource.FindName("NewSound")
            };
            RSARFolderNode folder = _resource as RSARFolderNode;
            RSARNode rsar = folder.RSARNode;
            if (rsar != null)
            {
                node._infoIndex = rsar._infoCache[0].Count;
                rsar._infoCache[0].Add(node);
            }

            _resource.AddChild(node);
        }

        public void CopySound()
        {
            using (CloneSoundDialog dlg = new CloneSoundDialog())
            {
                dlg.ShowDialog(null, Resource as RSARFolderNode);
            }
        }

        public void NewBank()
        {
            RSARBankNode node = new RSARBankNode
            {
                Name = _resource.FindName("NewBank")
            };
            RSARFolderNode folder = _resource as RSARFolderNode;
            RSARNode rsar = folder.RSARNode;
            if (rsar != null)
            {
                node._infoIndex = rsar._infoCache[1].Count;
                rsar._infoCache[1].Add(node);
            }

            _resource.AddChild(node);
        }

        public void NewType()
        {
            RSARPlayerInfoNode node = new RSARPlayerInfoNode
            {
                Name = _resource.FindName("NewPlayerInfo")
            };
            RSARFolderNode folder = _resource as RSARFolderNode;
            RSARNode rsar = folder.RSARNode;
            if (rsar != null)
            {
                node._infoIndex = rsar._infoCache[2].Count;
                rsar._infoCache[2].Add(node);
            }

            _resource.AddChild(node);
        }

        public void NewGroup()
        {
            RSARGroupNode node = new RSARGroupNode
            {
                Name = _resource.FindName("NewGroup")
            };
            RSARFolderNode folder = _resource as RSARFolderNode;
            RSARNode rsar = folder.RSARNode;
            if (rsar != null)
            {
                node._infoIndex = rsar._infoCache[4].Count;
                rsar._infoCache[4].Add(node);
            }

            _resource.AddChild(node);
        }
    }
}
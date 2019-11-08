using BrawlCrate.UI;
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

        private static readonly ToolStripMenuItem DuplicateToolStripMenuItem =
            new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D);

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
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
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
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSARFolderWrapper w = GetInstance<RSARFolderWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
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
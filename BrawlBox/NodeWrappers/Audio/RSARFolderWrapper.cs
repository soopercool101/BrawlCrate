using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.RSARFolder)]
    public class RSARFolderWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
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
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewFolderAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().NewFolder(); }
        protected static void NewSoundAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().NewSound(); }
        //protected static void ImportSoundAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().ImportSound(); }
        protected static void CopySoundAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().CopySound(); }
        protected static void NewBankAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().NewBank(); }
        protected static void NewTypeAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().NewType(); }
        protected static void NewGroupAction(object sender, EventArgs e) { GetInstance<RSARFolderWrapper>().NewGroup(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = _menu.Items[3].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            GenericWrapper w = GetInstance<GenericWrapper>();
            _menu.Items[2].Enabled = w.PrevNode != null;
            _menu.Items[3].Enabled = w.NextNode != null;
        }

        #endregion

        public RSARFolderWrapper() 
        {
            ContextMenuStrip = _menu; 
        }

        public void NewFolder()
        {
            RSARFolderNode node = new RSARFolderNode();
            node.Name = _resource.FindName("NewFolder");
            _resource.AddChild(node);
        }

        public void NewSound()
        {
            RSARSoundNode node = new RSARSoundNode();
            node.Name = _resource.FindName("NewSound");
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
                dlg.ShowDialog(null, Resource as RSARFolderNode);
        }

        public void NewBank()
        {
            RSARBankNode node = new RSARBankNode();
            node.Name = _resource.FindName("NewBank");
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
            RSARPlayerInfoNode node = new RSARPlayerInfoNode();
            node.Name = _resource.FindName("NewPlayerInfo");
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
            RSARGroupNode node = new RSARGroupNode();
            node.Name = _resource.FindName("NewGroup");
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

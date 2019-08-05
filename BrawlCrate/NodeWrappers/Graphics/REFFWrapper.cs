using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.REFF)]
    public class REFFWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static REFFWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("New Entry", null, NewEntryAction, Keys.Control | Keys.I));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewEntryAction(object sender, EventArgs e)
        {
            GetInstance<REFFWrapper>().NewEntry();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (replaceToolStripMenuItem != null)
            {
                replaceToolStripMenuItem.Enabled = true;
            }

            if (restoreToolStripMenuItem != null)
            {
                restoreToolStripMenuItem.Enabled = true;
            }

            if (moveUpToolStripMenuItem != null)
            {
                moveUpToolStripMenuItem.Enabled = true;
            }

            if (moveDownToolStripMenuItem != null)
            {
                moveDownToolStripMenuItem.Enabled = true;
            }

            if (deleteToolStripMenuItem != null)
            {
                deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<REFFWrapper>();

            if (replaceToolStripMenuItem != null)
            {
                replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (restoreToolStripMenuItem != null)
            {
                restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (moveUpToolStripMenuItem != null)
            {
                moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (moveDownToolStripMenuItem != null)
            {
                moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (deleteToolStripMenuItem != null)
            {
                deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
        }

        #endregion

        public REFFWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.REFF;

        public void NewEntry()
        {
            REFFEntryNode e = new REFFEntryNode();

            REFFNode node = _resource as REFFNode;
            ResourceNode emitter;
            if (node.VersionMinor == 9)
            {
                e.AddChild(emitter = new REFFEmitterNode9() {_name = "Emitter"});
            }
            else
            {
                e.AddChild(emitter = new REFFEmitterNode7() {_name = "Emitter"});
            }

            emitter.AddChild(new REFFTEVStage(0));
            emitter.AddChild(new REFFTEVStage(1));
            emitter.AddChild(new REFFTEVStage(2));
            emitter.AddChild(new REFFTEVStage(3));
            e.AddChild(new REFFParticleNode() {_name = "Particle"});
            e.AddChild(new REFFAnimationListNode() {_name = "Animations"});
            _resource.AddChild(e);
            BaseWrapper w = FindResource(e, true);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
        }
    }
}
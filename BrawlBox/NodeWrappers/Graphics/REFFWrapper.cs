using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.REFF)]
    public class REFFWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static REFFWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("New Entry", null, NewEntryAction, Keys.Control | Keys.I));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<REFFWrapper>().NewEntry(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[10].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            REFFWrapper w = GetInstance<REFFWrapper>();
            _menu.Items[3].Enabled = _menu.Items[10].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }
        
        #endregion

        public REFFWrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.REFF; } }

        public void NewEntry()
        {
            REFFEntryNode e = new REFFEntryNode();

            REFFNode node = _resource as REFFNode;
            ResourceNode emitter;
            if (node.VersionMinor == 9)
                e.AddChild(emitter = new REFFEmitterNode9() { _name = "Emitter" });
            else
                e.AddChild(emitter = new REFFEmitterNode7() { _name = "Emitter" });
            emitter.AddChild(new REFFTEVStage(0));
            emitter.AddChild(new REFFTEVStage(1));
            emitter.AddChild(new REFFTEVStage(2));
            emitter.AddChild(new REFFTEVStage(3));
            e.AddChild(new REFFParticleNode() { _name = "Particle" });
            e.AddChild(new REFFAnimationListNode() { _name = "Animations" });
            _resource.AddChild(e);
            BaseWrapper w = this.FindResource(e, true);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
        }
    }
}

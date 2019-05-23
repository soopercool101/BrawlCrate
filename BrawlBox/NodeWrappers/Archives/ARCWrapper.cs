using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib;
using BrawlLib.Modeling;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.ARC)]
    public class ARCWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static ARCWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("ARChive", null, NewARCAction),
                new ToolStripMenuItem("BRResource Pack", null, NewBRESAction),
                new ToolStripMenuItem("Collision", null, NewCollisionAction),
                new ToolStripMenuItem("BLOC", null, NewBLOCAction),
                new ToolStripMenuItem("MSBin", null, NewMSBinAction)
                ));
            _menu.Items.Add(new ToolStripMenuItem("&Import", null,
                new ToolStripMenuItem("ARChive", null, ImportARCAction),
                new ToolStripMenuItem("BRResource Pack", null, ImportBRESAction),
                new ToolStripMenuItem("BLOC", null, ImportBLOCAction),
                new ToolStripMenuItem("MSBin", null, ImportMSBinAction)
                ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Preview All Models", null, PreviewAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
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
        protected static void NewBRESAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewBRES(); }
        protected static void NewARCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewARC(); }
        protected static void NewMSBinAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewMSBin(); }
        protected static void NewCollisionAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewCollision(); }
        protected static void NewBLOCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewBLOC(); }
        protected static void ImportBRESAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportBRES(); }
        protected static void ImportARCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportARC(); }
        protected static void ImportBLOCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportBLOC(); }
        protected static void ImportCollisionAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportCollision(); }
        protected static void ImportMSBinAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ImportMSBin(); }
        protected static void PreviewAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().PreviewAll(); }
        protected static void ExportAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ExportAll(); }
        protected static void ReplaceAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ReplaceAll(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[8].Enabled = _menu.Items[9].Enabled = _menu.Items[11].Enabled = _menu.Items[12].Enabled = _menu.Items[15].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            ARCWrapper w = GetInstance<ARCWrapper>();
                _menu.Items[8].Enabled = _menu.Items[15].Enabled = w.Parent != null;
                _menu.Items[9].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
                _menu.Items[11].Enabled = w.PrevNode != null;
                _menu.Items[12].Enabled = w.NextNode != null;
        }
        #endregion

        public override string ExportFilter
        {
            get
            {
                return "PAC Archive (*.pac)|*.pac|" +
                    "Compressed PAC Archive (*.pcs)|*.pcs|" +
                    "Archive Pair (*.pair)|*.pair|" +
                    "Multiple Resource Group (*.mrg)|*.mrg|" +
                    "Compressed MRG (*.mrgc)|*.mrgc";
            }
        }

        public ARCWrapper() { ContextMenuStrip = _menu; }

        public ARCNode NewARC()
        {
            ARCNode node = new ARCNode() { Name = _resource.FindName("NewARChive"), FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public BRRESNode NewBRES()
        {
            BRRESNode node = new BRRESNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public CollisionNode NewCollision()
        {
            CollisionNode node = new CollisionNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public BLOCNode NewBLOC()
        {
            BLOCNode node = new BLOCNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }
        public MSBinNode NewMSBin()
        {
            MSBinNode node = new MSBinNode() { FileType = ARCFileType.MiscData };
            _resource.AddChild(node);

            BaseWrapper w = this.FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public void ImportARC()
        {
            string path;
            if (Program.OpenFile("ARChive (*.pac,*.pcs)|*.pac;*.pcs", out path) > 0)
                NewARC().Replace(path);
        }
        public void ImportBRES()
        {
            string path;
            if (Program.OpenFile(FileFilters.BRES, out path) > 0)
                NewBRES().Replace(path);
        }
        public void ImportBLOC()
        {
            string path;
            if (Program.OpenFile(FileFilters.BLOC, out path) > 0)
                NewBLOC().Replace(path);
        }
        public void ImportCollision()
        {
            string path;
            if (Program.OpenFile(FileFilters.CollisionDef, out path) > 0)
                NewBRES().Replace(path);
        }
        public void ImportMSBin()
        {
            string path;
            if (Program.OpenFile(FileFilters.MSBin, out path) > 0)
                NewMSBin().Replace(path);
        }
        
        public override void OnExport(string outPath, int filterIndex)
        {
            switch (filterIndex)
            {
                case 1: ((ARCNode)_resource).Export(outPath); break;
                case 2: ((ARCNode)_resource).ExportPCS(outPath); break;
                case 3: ((ARCNode)_resource).ExportPair(outPath); break;
                case 4: ((ARCNode)_resource).ExportAsMRG(outPath); break;
            }
        }

        private void LoadModels(ResourceNode node, List<IModel> models, List<CollisionNode> collisions)
        {
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.U8:
                case ResourceType.U8Folder:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        LoadModels(n, models, collisions);
                    break;
                case ResourceType.MDL0:
                    models.Add((IModel)node);
                    break;
                case ResourceType.CollisionDef:
                    collisions.Add((CollisionNode)node);
                    break;
            }
        }

        public void PreviewAll()
        {
            List<IModel> models = new List<IModel>();
            List<CollisionNode> collisions = new List<CollisionNode>();
            LoadModels(_resource, models, collisions);
            new ModelForm().Show(_owner, models, collisions);
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((ARCNode)_resource).ExtractToFolder(path);
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((ARCNode)_resource).ReplaceFromFolder(path);
        }
    }
}

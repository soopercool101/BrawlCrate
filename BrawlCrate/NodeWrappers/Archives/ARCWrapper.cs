using BrawlLib;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.ARC)]
    public class ARCWrapper : GenericWrapper
    {
        #region Menu

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
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewBRESAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().NewBRES();
        }

        protected static void NewARCAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().NewARC();
        }

        protected static void NewMSBinAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().NewMSBin();
        }

        protected static void NewCollisionAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().NewCollision();
        }

        protected static void NewBLOCAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().NewBLOC();
        }

        protected static void ImportBRESAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportBRES();
        }

        protected static void ImportARCAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportARC();
        }

        protected static void ImportBLOCAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportBLOC();
        }

        protected static void ImportCollisionAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportCollision();
        }

        protected static void ImportMSBinAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ImportMSBin();
        }

        protected static void PreviewAllAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().PreviewAll();
        }

        protected static void ExportAllAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ExportAll();
        }

        protected static void ReplaceAllAction(object sender, EventArgs e)
        {
            GetInstance<ARCWrapper>().ReplaceAll();
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
            ARCWrapper w = GetInstance<ARCWrapper>();

            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public override string ExportFilter => FileFilters.ARCExport;
        public override string ImportFilter => FileFilters.ARCImport;

        public ARCWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public ARCNode NewARC()
        {
            ARCNode node = new ARCNode() {Name = _resource.FindName("NewARChive"), FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public BRRESNode NewBRES()
        {
            BRRESNode node = new BRRESNode() {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public CollisionNode NewCollision()
        {
            CollisionNode node = new CollisionNode() {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public BLOCNode NewBLOC()
        {
            BLOCNode node = new BLOCNode() {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public MSBinNode NewMSBin()
        {
            MSBinNode node = new MSBinNode() {FileType = ARCFileType.MiscData};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public void ImportARC()
        {
            if (Program.OpenFile(FileFilters.ARCImport, out string path) > 0)
            {
                NewARC().Replace(path);
            }
        }

        public void ImportBRES()
        {
            if (Program.OpenFile(FileFilters.BRES, out string path) > 0)
            {
                NewBRES().Replace(path);
            }
        }

        public void ImportBLOC()
        {
            if (Program.OpenFile(FileFilters.BLOC, out string path) > 0)
            {
                NewBLOC().Replace(path);
            }
        }

        public void ImportCollision()
        {
            if (Program.OpenFile(FileFilters.CollisionDef, out string path) > 0)
            {
                NewBRES().Replace(path);
            }
        }

        public void ImportMSBin()
        {
            if (Program.OpenFile(FileFilters.MSBin, out string path) > 0)
            {
                NewMSBin().Replace(path);
            }
        }

        public override void OnExport(string outPath, int filterIndex)
        {
            ((ARCNode) _resource).Export(outPath);
        }

        public void LoadModels(ResourceNode node, List<IModel> models, List<CollisionNode> collisions)
        {
            switch (node.ResourceFileType)
            {
                case ResourceType.ARC:
                case ResourceType.MRG:
                case ResourceType.U8:
                case ResourceType.U8Folder:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                    {
                        LoadModels(n, models, collisions);
                    }

                    break;
                case ResourceType.MDL0:
                    models.Add((IModel) node);
                    break;
                case ResourceType.CollisionDef:
                    collisions.Add((CollisionNode) node);
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
            {
                return;
            }

            ((ARCNode) _resource).ExtractToFolder(path);
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
            {
                return;
            }

            ((ARCNode) _resource).ReplaceFromFolder(path);
        }
    }
}
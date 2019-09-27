using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    //Contains generic members inherited by all sub-classed nodes
    public class GenericWrapper : BaseWrapper
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

        static GenericWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
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

        protected static void MoveUpAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().MoveUp();
        }

        protected static void MoveDownAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().MoveDown();
        }

        protected static void ExportAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().Export();
        }

        protected static void DuplicateAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().Duplicate();
        }

        protected static void ReplaceAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().Replace();
        }

        protected static void RestoreAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().Restore();
        }

        protected static void DeleteAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().Delete();
        }

        protected static void RenameAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().Rename();
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
            GenericWrapper w = GetInstance<GenericWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public GenericWrapper(IWin32Window owner)
        {
            _owner = owner;
            ContextMenuStrip = _menu;
        }

        public GenericWrapper()
        {
            _owner = null;
            ContextMenuStrip = _menu;
        }

        public void MoveUp()
        {
            MoveUp(true);
        }

        public virtual void MoveUp(bool select)
        {
            if (PrevVisibleNode == null)
            {
                return;
            }

            if (_resource.MoveUp())
            {
                int index = Index - 1;
                TreeNode parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                _resource.OnMoved();
                if (select)
                {
                    TreeView.SelectedNode = this;
                }

                TreeView.EndUpdate();
            }
        }

        public void MoveDown()
        {
            MoveDown(true);
        }

        public virtual void MoveDown(bool select)
        {
            if (NextVisibleNode == null)
            {
                return;
            }

            if (_resource.MoveDown())
            {
                int index = Index + 1;
                TreeNode parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                _resource.OnMoved();
                if (select)
                {
                    TreeView.SelectedNode = this;
                }

                TreeView.EndUpdate();
            }
        }

        public virtual string ExportFilter => BrawlLib.FileFilters.Raw;
        public virtual string ImportFilter => ExportFilter;
        public virtual string ReplaceFilter => ImportFilter;

        public static int CategorizeFilter(string path, string filter)
        {
            string ext = "*" + Path.GetExtension(path);

            string[] split = filter.Split('|');
            for (int i = 3; i < split.Length; i += 2)
            {
                foreach (string s in split[i].Split(';'))
                {
                    if (s.Equals(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        return (i + 1) / 2;
                    }
                }
            }

            return 1;
        }

        public virtual string Export()
        {
            int index = Program.SaveFile(ExportFilter, Text, out string outPath);
            if (index != 0)
            {
                if (Parent == null)
                {
                    _resource.Merge(Control.ModifierKeys == (Keys.Control | Keys.Shift));
                }

                OnExport(outPath, index);
            }

            return outPath;
        }

        public virtual void OnExport(string outPath, int filterIndex)
        {
            _resource.Export(outPath);
        }

        public virtual void Replace()
        {
            if (Parent == null)
            {
                return;
            }

            int index = Program.OpenFile(ReplaceFilter, out string inPath);
            if (index != 0)
            {
                OnReplace(inPath, index);
                Link(_resource);
            }
        }

        public virtual void OnReplace(string inStream, int filterIndex)
        {
            _resource.Replace(inStream);
        }

        public void Restore()
        {
            _resource.Restore();
        }

        public void Delete()
        {
            if (Parent == null || Form.ActiveForm != MainForm.Instance)
            {
                return;
            }

            _resource.Dispose();
            _resource.Remove();
        }

        public void Rename()
        {
            using (RenameDialog dlg = new RenameDialog())
            {
                dlg.ShowDialog(MainForm.Instance, _resource);
            }
        }

        public virtual void Duplicate()
        {
            if (_resource.Parent == null)
            {
                return;
            }
            string tempPath = Path.GetTempFileName();
            _resource.Export(tempPath);
            ResourceNode rNode2 = NodeFactory.FromFile(null, tempPath, _resource.GetType());
            int n = 0;
            int index = 0;
            while (_resource.Parent.FindChildrenByName(rNode2.Name).Length >= 1)
            {
                index = _resource.Parent.FindChildrenByName(rNode2.Name).Last().Index;
                rNode2.Name = $"{_resource.Name} ({++n})";
            }
            _resource.Parent.InsertChild(rNode2, true, index + 1);
        }
    }
}
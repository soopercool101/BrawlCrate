using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    //Contains generic members inherited by all sub-classed nodes
    public class GenericWrapper : BaseWrapper
    {
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

        public virtual string ExportFilter => FileFilters.Raw;
        public virtual string ImportFilter => ExportFilter;
        public virtual string ReplaceFilter => ImportFilter;

        public void MoveUp()
        {
            MoveUp(true);
        }

        public virtual void MoveUp(bool select)
        {
            if (PrevVisibleNode == null) return;

            if (_resource.MoveUp())
            {
                var index = Index - 1;
                var parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                _resource.OnMoved();
                if (select) TreeView.SelectedNode = this;

                TreeView.EndUpdate();
            }
        }

        public void MoveDown()
        {
            MoveDown(true);
        }

        public virtual void MoveDown(bool select)
        {
            if (NextVisibleNode == null) return;

            if (_resource.MoveDown())
            {
                var index = Index + 1;
                var parent = Parent;
                TreeView.BeginUpdate();
                Remove();
                parent.Nodes.Insert(index, this);
                _resource.OnMoved();
                if (select) TreeView.SelectedNode = this;

                TreeView.EndUpdate();
            }
        }

        public static int CategorizeFilter(string path, string filter)
        {
            var ext = "*" + Path.GetExtension(path);

            var split = filter.Split('|');
            for (var i = 3; i < split.Length; i += 2)
                foreach (var s in split[i].Split(';'))
                    if (s.Equals(ext, StringComparison.OrdinalIgnoreCase))
                        return (i + 1) / 2;

            return 1;
        }

        public virtual string Export()
        {
            var index = Program.SaveFile(ExportFilter, Text, out var outPath);
            if (index != 0)
            {
                if (Parent == null) _resource.Merge(Control.ModifierKeys == (Keys.Control | Keys.Shift));

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
            if (Parent == null) return;

            var index = Program.OpenFile(ReplaceFilter, out var inPath);
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
            if (Parent == null) return;

            _resource.Dispose();
            _resource.Remove();
        }

        public void Rename()
        {
            using (var dlg = new RenameDialog())
            {
                dlg.ShowDialog(MainForm.Instance, _resource);
            }
        }

        #region Menu

        private static readonly ContextMenuStrip _menu;

        static GenericWrapper()
        {
            _menu = new ContextMenuStrip();
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
            _menu.Items[1].Enabled = _menu.Items[2].Enabled =
                _menu.Items[4].Enabled = _menu.Items[5].Enabled = _menu.Items[8].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<GenericWrapper>();
            _menu.Items[1].Enabled = _menu.Items[8].Enabled = w.Parent != null;
            _menu.Items[2].Enabled = w._resource.IsDirty || w._resource.IsBranch;
            _menu.Items[4].Enabled = w.PrevNode != null;
            _menu.Items[5].Enabled = w.NextNode != null;
        }

        #endregion
    }
}
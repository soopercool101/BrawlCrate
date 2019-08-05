using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    //Contains generic members inherited by all sub-classed nodes
    public class GenericWrapper : BaseWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static GenericWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(_replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(_restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
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
            GenericWrapper w = GetInstance<GenericWrapper>();

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
    }
}
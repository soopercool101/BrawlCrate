using BrawlCrate.UI;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    //Contains generic members inherited by all sub-classed nodes
    public class GenericWrapper : BaseWrapper, MultiSelectableWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static readonly ContextMenuStrip MultiSelectMenu;

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

        private static readonly ToolStripMenuItem ExportSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Export Selected", null, ExportSelectedAction, Keys.Control | Keys.E);
        
        private static readonly ToolStripMenuItem DeleteSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Delete Selected", null, DeleteSelectedAction, Keys.Control | Keys.Delete);

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

            MultiSelectMenu = new ContextMenuStrip();
            MultiSelectMenu.Items.Add(ExportSelectedToolStripMenuItem);
            MultiSelectMenu.Items.Add(DeleteSelectedToolStripMenuItem);
            MultiSelectMenu.Opening += MultiMenuOpening;
            MultiSelectMenu.Closing += MultiMenuClosing;
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

        protected static void ExportSelectedAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().ExportSelected();
        }

        protected static void DeleteSelectedAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().DeleteSelected();
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

        protected static void SortAction(object sender, EventArgs e)
        {
            GetInstance<GenericWrapper>().Sort();
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
        
        private static void MultiMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            DeleteSelectedToolStripMenuItem.Visible = true;
            DeleteSelectedToolStripMenuItem.Enabled = true;
        }

        private static void MultiMenuOpening(object sender, CancelEventArgs e)
        {
            GenericWrapper w = GetInstance<GenericWrapper>();
            foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
            {
                if (!(n is GenericWrapper g) || g._resource.Parent == null)
                {
                    DeleteSelectedToolStripMenuItem.Visible = false;
                    DeleteSelectedToolStripMenuItem.Enabled = false;
                    break;
                }
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

        public virtual ContextMenuStrip MultiSelectMenuStrip => MultiSelectMenu;

        public virtual void Sort()
        {
            _resource.SortChildren();
            RefreshView(_resource);
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

        public virtual string ExportFilter => FileFilters.Raw;
        public virtual string ImportFilter => ExportFilter;
        public virtual string ReplaceFilter => ImportFilter;

        public void DeleteSelected()
        {
            while (MainForm.Instance.resourceTree.SelectedNodes.Count > 0)
            {
                if (!(MainForm.Instance.resourceTree.SelectedNodes[0] is GenericWrapper g))
                {
                    break;
                }

                MainForm.Instance.resourceTree.SelectedNodes.RemoveAt(0);
                g.Delete();
            }
        }
        
        public void ExportSelected()
        {
            string folder = Program.ChooseFolder();
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            Dictionary<Type, string> extensions = new Dictionary<Type, string>();
            List<ResourceNode> nodes = new List<ResourceNode>();
            foreach (TreeNode tNode in MainForm.Instance.resourceTree.SelectedNodes)
            {
                if (tNode is GenericWrapper g)
                {
                    nodes.Add(g._resource);
                    if (!extensions.ContainsKey(g._resource.GetType()))
                    {
                        extensions.Add(g._resource.GetType(), g.ExportFilter);
                    }
                }
            }

            Dictionary<Type, string> chosenExtensions = new Dictionary<Type, string>();
            foreach (KeyValuePair<Type, string> ext in extensions)
            {
                ExportAllFormatDialog dialog = new ExportAllFormatDialog("Export Selected", ext.Key, ext.Value);

                if (dialog.AutoSelect || dialog.Valid && dialog.ShowDialog() == DialogResult.OK)
                {
                    chosenExtensions.Add(ext.Key, dialog.SelectedExtension);
                }
            }

            string invalidChars =
                System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            foreach (ResourceNode n in nodes)
            {
                chosenExtensions.TryGetValue(n.GetType(), out string ext);
                if (!string.IsNullOrEmpty(ext) && !ext.StartsWith("."))
                {
                    ext = ext.Insert(0, ".");
                }

                n.Export(
                    $"{folder}\\{System.Text.RegularExpressions.Regex.Replace($"{n.Name}{ext ?? ""}", invalidRegStr, "")}");
            }

            MessageBox.Show($"{nodes.Count} nodes successfully exported to {folder}", "Export Selected");
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

            if (Program.OpenFile(ReplaceFilter, out string inPath))
            {
                OnReplace(inPath);
                Link(_resource);
            }
        }

        public virtual void OnReplace(string inStream)
        {
            _resource.Replace(inStream);
        }

        public void Restore()
        {
            _resource.Restore();
        }

        public virtual void Delete()
        {
            if (Parent == null)
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
            if (rNode2 == null)
            {
                MessageBox.Show("The node could not be duplicated correctly.");
                return;
            }

            int n = 0;
            int index = _resource.Index;
            // Copy ARCEntryNode data, which is contained in the containing ARC, not the node itself
            if (rNode2 is ARCEntryNode)
            {
                ((ARCEntryNode) rNode2).FileIndex = ((ARCEntryNode) _resource).FileIndex;
                ((ARCEntryNode) rNode2).FileType = ((ARCEntryNode) _resource).FileType;
                ((ARCEntryNode) rNode2).GroupID = ((ARCEntryNode) _resource).GroupID;
                ((ARCEntryNode) rNode2).RedirectIndex = ((ARCEntryNode) _resource).RedirectIndex;
            }

            // Copy the name directly in cases where name isn't saved
            rNode2.Name = _resource.Name;
            // Set the name programatically (based on Windows' implementation)
            while (_resource.Parent.FindChildrenByName(rNode2.Name).Length >= 1)
            {
                // Get the last index of the last duplicated node in order to place it after that one
                index = Math.Max(index, _resource.Parent.FindChildrenByName(rNode2.Name).Last().Index);
                // Set the name based on the number of duplicate nodes found
                rNode2.Name = $"{_resource.Name} ({++n})";
            }

            // Place the node in the same containing parent, after the last duplicated node.
            _resource.Parent.InsertChild(rNode2, true, index + 1);
        }
    }
}
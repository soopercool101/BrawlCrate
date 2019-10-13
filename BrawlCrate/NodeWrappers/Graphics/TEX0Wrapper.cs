using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TEX0)]
    [NodeWrapper(ResourceType.SharedTEX0)]
    public class TEX0Wrapper : GenericWrapper
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

        private static readonly ToolStripMenuItem ColorSmashSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Color Smash", null, ColorSmash.ColorSmashTex0, Keys.Control | Keys.Shift | Keys.C);

        private static readonly ToolStripMenuItem ExportSelectedToolStripMenuItem =
            new ToolStripMenuItem("&Export Selected", null, ExportSelectedAction, Keys.Control | Keys.E);

        static TEX0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Re-Encode", null, ReEncodeAction));
            _menu.Items.Add(new ToolStripSeparator());
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
            MultiSelectMenu.Items.Add(ColorSmashSelectedToolStripMenuItem);
            MultiSelectMenu.Items.Add(new ToolStripSeparator());
            MultiSelectMenu.Items.Add(ExportSelectedToolStripMenuItem);
            MultiSelectMenu.Opening += MultiMenuOpening;
            MultiSelectMenu.Closing += MultiMenuClosing;
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
            TEX0Wrapper w = GetInstance<TEX0Wrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        private static void MultiMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ColorSmashSelectedToolStripMenuItem.Enabled = true;
        }

        private static void MultiMenuOpening(object sender, CancelEventArgs e)
        {
            TEX0Wrapper w = GetInstance<TEX0Wrapper>();
            if (!ColorSmash.CanRunColorSmash)
            {
                ColorSmashSelectedToolStripMenuItem.Enabled = false;
            }
            else
            {
                foreach (TreeNode n in MainForm.Instance.resourceTree.SelectedNodes)
                {
                    if (!(n is TEX0Wrapper t) || t._resource.Parent == null || t._resource.Parent != w._resource.Parent)
                    {
                        ColorSmashSelectedToolStripMenuItem.Enabled = false;
                        break;
                    }
                }
            }
        }

        protected static void ReEncodeAction(object sender, EventArgs e)
        {
            GetInstance<TEX0Wrapper>().ReEncode();
        }

        #endregion

        public override ContextMenuStrip MultiSelectMenuStrip => MultiSelectMenu;

        public TEX0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.TEX0;

        public override void OnReplace(string inStream)
        {
            if (inStream.EndsWith(".tex0", StringComparison.OrdinalIgnoreCase) || !inStream.Contains("."))
            {
                base.OnReplace(inStream);
            }
            else
            {
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = inStream;
                    dlg.ShowDialog(MainForm.Instance, Resource as TEX0Node);
                }
            }
        }

        public void ReEncode()
        {
            using (TextureConverterDialog dlg = new TextureConverterDialog())
            {
                dlg.LoadImages((Resource as TEX0Node).GetImage(0));
                dlg.ShowDialog(MainForm.Instance, Resource as TEX0Node);
            }
        }

        protected internal override void OnPropertyChanged(ResourceNode node)
        {
            RefreshView(node);
        }
    }
}
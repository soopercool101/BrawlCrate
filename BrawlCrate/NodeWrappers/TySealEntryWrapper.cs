using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.Sticker)]
    internal class TySealWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem _openBrresToolStripMenuItem;
        private static ToolStripSeparator _openBrresToolStripSeparator;

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

        static TySealWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(_openBrresToolStripMenuItem =
                new ToolStripMenuItem("Open BRRES", null, OpenBRRESAction));
            _menu.Items.Add(_openBrresToolStripSeparator = new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void OpenBRRESAction(object sender, EventArgs e)
        {
            GetInstance<TySealWrapper>().OpenBRRES();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _openBrresToolStripMenuItem.Enabled = true;
            _openBrresToolStripMenuItem.Visible = true;
            _openBrresToolStripSeparator.Visible = true;
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            TySealWrapper w = GetInstance<TySealWrapper>();

            TySealNode tySealNode = (TySealNode) w._resource;
            if (File.Exists(Path.Combine(w._resource.RootNode.DirectoryName, tySealNode.BRRES)))
            {
                _openBrresToolStripMenuItem.Enabled = true;
                _openBrresToolStripMenuItem.Visible = true;
                _openBrresToolStripSeparator.Visible = true;
            }
            else
            {
                _openBrresToolStripMenuItem.Enabled = false;
                _openBrresToolStripMenuItem.Visible = false;
                _openBrresToolStripSeparator.Visible = false;
            }

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
        }

        #endregion

        public void OpenBRRES()
        {
            string file = Path.Combine(Resource.RootNode.DirectoryName, ((TySealNode) Resource).BRRES);
            if (File.Exists(file))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "\\BrawlCrate.exe",
                    Arguments = "\"" + file + "\""
                });
            }
        }

        public TySealWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }
}
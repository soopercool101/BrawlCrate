using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Group)]
    public class MDL0GroupWrapper : GenericWrapper
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

        static MDL0GroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Create New Node", null, CreateAction, Keys.Control | Keys.N));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void CreateAction(object sender, EventArgs e)
        {
            GetInstance<MDL0GroupWrapper>().CreateNode();
        }

        private void CreateNode()
        {
            MDL0GroupNode group = _resource as MDL0GroupNode;
            MDL0Node model = group.Parent as MDL0Node;

            switch (group._type.ToString("g"))
            {
                case "Bones":
                    MDL0BoneNode bone = new MDL0BoneNode {Name = "NewBone"};
                    model._boneGroup.InsertChild(bone, false, 0);
                    bone._boneFlags = (BoneFlags) 284;
                    bone.Scale = new Vector3(1, 1, 1);
                    bone._bindMatrix = Matrix.Identity;
                    bone._inverseBindMatrix = Matrix.Identity;
                    bone.OnMoved();
                    break;
            }
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
            MDL0GroupWrapper w = GetInstance<MDL0GroupWrapper>();

            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
        }

        #endregion

        public MDL0GroupWrapper()
        {
            ContextMenuStrip = null;
        }
    }
}
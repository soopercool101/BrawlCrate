using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
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
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

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
                    MDL0BoneNode bone = new MDL0BoneNode() {Name = "NewBone"};
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
            var w = GetInstance<MDL0GroupWrapper>();

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

        public MDL0GroupWrapper()
        {
            ContextMenuStrip = null;
        }
    }
}
using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Group)]
    public class MDL0GroupWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static MDL0GroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Create New Node", null, CreateAction, Keys.Control | Keys.N));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void CreateAction(object sender, EventArgs e) { GetInstance<MDL0GroupWrapper>().CreateNode(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0GroupWrapper w = GetInstance<MDL0GroupWrapper>();
        }
        private void CreateNode()
        {
            MDL0GroupNode group = _resource as MDL0GroupNode;
            MDL0Node model = group.Parent as MDL0Node;
            
            switch (group._type.ToString("g"))
            {
                case "Bones":
                    MDL0BoneNode bone = new MDL0BoneNode() { Name = "NewBone" };
                    model._boneGroup.InsertChild(bone, false, 0);
                    bone._boneFlags = (BoneFlags)284;
                    bone.Scale = new Vector3(1, 1, 1);
                    bone._bindMatrix = Matrix.Identity;
                    bone._inverseBindMatrix = Matrix.Identity;
                    bone.OnMoved();
                    break;
            }
        }
        #endregion

        public MDL0GroupWrapper() { ContextMenuStrip = null; }
    }
}

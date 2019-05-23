using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;

namespace Ikarus.UI
{
    public partial class MainControl : ModelEditorBase
    {
        protected override NW4RAnimationNode FindCorrespondingAnimation(NW4RAnimationNode focusFile, NW4RAnimType targetType)
        {
            string name = focusFile.Name;
            if (listPanel._animations.ContainsKey(name) && 
                listPanel._animations[name].ContainsKey(targetType))
                return listPanel._animations[name][targetType];
            return null;
        }

        public override void ApplyVIS0ToInterface()
        {
            base.ApplyVIS0ToInterface();
            return;

            if (_animFrame == 0 || modelListsPanel1.lstObjects.Items.Count == 0)
                return;

            VIS0Updating = true;
            if (_vis0 != null)
            {
                //if (TargetAnimation != null && _vis0.FrameCount != TargetAnimation.tFrameCount)
                //    UpdateVis0(null, null);

                //foreach (string n in VIS0Indices.Keys)
                //{
                //    VIS0EntryNode node = null;
                //    List<int> indices = VIS0Indices[n];
                //    for (int i = 0; i < indices.Count; i++)
                //    {
                //        if ((node = (VIS0EntryNode)_vis0.FindChild(((MDL0ObjectNode)modelListsPanel1.lstObjects.Items[indices[i]])._visBoneNode.Name, true)) != null)
                //        {
                //            if (node._entryCount != 0 && _animFrame > 0)
                //                modelListsPanel1.lstObjects.SetItemChecked(indices[i], node.GetEntry((int)_animFrame - 1));
                //            else
                //                modelListsPanel1.lstObjects.SetItemChecked(indices[i], node._flags.HasFlag(VIS0Flags.Enabled));
                //        }
                //    }
                //}
            }
            VIS0Updating = false;
        }
    }
}

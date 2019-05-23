using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public partial class ModelEditControl : ModelEditorBase
    {
        public int prevHeight = 0, prevWidth = 0;
        public void ToggleWeightEditor()
        {
            animEditors.Visible = true;
            if (!(btnWeightEditor.Checked = !btnWeightEditor.Checked))
            {
                animEditors.Height = prevHeight;
                animCtrlPnl.Width = prevWidth;
                weightEditor.Visible = false;
                _currentControl.Visible = true;
            }
            else
            {
                if (vertexEditor.Visible)
                    ToggleVertexEditor();

                prevHeight = animEditors.Height;
                prevWidth = animCtrlPnl.Width;
                animCtrlPnl.Width = weightEditor.MinimumSize.Width;
                animEditors.Height = weightEditor.MinimumSize.Height;
                weightEditor.Visible = true;
                _currentControl.Visible = false;
            }

            CheckDimensions();
        }
        public void ToggleVertexEditor()
        {
            animEditors.Visible = true;
            if (!(btnVertexEditor.Checked = !btnVertexEditor.Checked))
            {
                animEditors.Height = prevHeight;
                animCtrlPnl.Width = prevWidth;
                vertexEditor.Visible = false;
                _currentControl.Visible = true;
            }
            else
            {
                if (weightEditor.Visible)
                    ToggleWeightEditor();

                prevHeight = animEditors.Height;
                prevWidth = animCtrlPnl.Width;
                animCtrlPnl.Width = 230;
                animEditors.Height = 82;
                vertexEditor.Visible = true;
                _currentControl.Visible = false;
            }

            CheckDimensions();
        }

        bool _retainAspect = true;

        /// <summary>
        /// Call this after the frame is set.
        /// </summary>
        private void HandleFirstPersonCamera()
        {
            if (FirstPersonCamera && _scn0 != null && scn0Editor._camera != null)
                scn0Editor._camera.SetCamera(ModelPanel.CurrentViewport, CurrentFrame - 1, _retainAspect);
        }

        public override void OnAnimationChanged()
        {
            NW4RAnimationNode node = TargetAnimation;

            selectedAnimationToolStripMenuItem.Enabled = node != null;

            portToolStripMenuItem.Enabled = node is CHR0Node;
            mergeToolStripMenuItem.Enabled = node != null && Array.IndexOf(Mergeable, node.GetType()) >= 0;
            resizeToolStripMenuItem.Enabled = node != null && Array.IndexOf(Resizable, node.GetType()) >= 0;
            appendToolStripMenuItem.Enabled = node != null && Array.IndexOf(Appendable, node.GetType()) >= 0;

            int i = -1;
            bool hasKeys = node != null && !(node is SCN0Node) && (i = Array.IndexOf(Interpolated, node.GetType())) >= 0;
            string s = 
                i == 0 ? (SelectedBone != null ? SelectedBone.Name : "entry") :
                i == 1 ? (TargetTexRef != null ? TargetTexRef.Name : "entry") :
                i == 2 ? (shp0Editor.VertexSetDest != null ? shp0Editor.VertexSetDest.Name : "entry") :
            "entry";

            averageboneStartendTangentsToolStripMenuItem.Enabled = hasKeys && s != "entry";
            averageboneStartendTangentsToolStripMenuItem.Text = String.Format("Average {0} start/end keyframes", s);

            averageAllStartEndTangentsToolStripMenuItem.Enabled = node != null && Array.IndexOf(Interpolated, node.GetType()) >= 0;
            //syncStartendTangentsToolStripMenuItem.Enabled = node != null && Array.IndexOf(Interpolated, node.GetType()) >= 0;
        }
    }
}

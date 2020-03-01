using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public partial class MDL0ObjectControl : UserControl
    {
        public MDL0ObjectControl()
        {
            InitializeComponent();

            modelPanel.RenderFloorChanged += modelPanel_RenderFloorChanged;
            modelPanel.RenderBonesChanged += modelPanel_RenderBonesChanged;
            modelPanel.RenderShadersChanged += modelPanel_RenderShadersChanged;
        }

        private MDL0ObjectNode _targetObject;

        private void modelPanel_RenderShadersChanged(ModelPanel panel, bool value)
        {
            shadersToolStripMenuItem.Checked = value;
        }

        private void modelPanel_RenderBonesChanged(ModelPanel panel, bool value)
        {
            bonesToolStripMenuItem.Checked = value;
        }

        private void modelPanel_RenderFloorChanged(ModelPanel panel, bool value)
        {
            floorToolStripMenuItem.Checked = value;
        }

        public bool SetTarget(MDL0ObjectNode o)
        {
            //lstDrawCalls.Items.Clear();
            lstDrawCalls.DataSource = null;
            modelPanel.ClearAll();
            cboMaterial.Items.Clear();
            cboVisBone.Items.Clear();
            try
            {
                if ((_targetObject = o) != null)
                {
                    _targetObject.IsRendering = true;

                    cboMaterial.Items.AddRange(o.Model.MaterialList.ToArray());
                    cboVisBone.Items.AddRange(o.Model._linker.BoneCache.ToArray());
                    lstDrawCalls.DataSource = o._drawCalls;
                    //lstDrawCalls.DisplayMember = "";
                    //lstDrawCalls.ValueMember = "_isXLU";

                    modelPanel.AddTarget(o);
                    //if (o._drawCalls.Count > 0)
                    //    lstDrawCalls.SelectedIndex = 0;
                    modelPanel.SetCamWithBox(o.GetBox());
                    return true;
                }
            }
            catch
            {
                // ignored
            }

            if (_targetObject != null)
            {
                try
                {
                    _targetObject.IsRendering = false;
                }
                catch
                {
                    // ignored
                }
            }

            _targetObject = null;
            lstDrawCalls.DataSource = null;
            modelPanel.ClearAll();
            cboMaterial.Items.Clear();
            cboVisBone.Items.Clear();

            return false;
        }

        private bool _updating;

        private void lstDrawCalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawCall drawCall = lstDrawCalls.SelectedItem as DrawCall;
            if (drawCall != null)
            {
                _updating = true;

                cboMaterial.SelectedIndex = drawCall.MaterialNode != null ? drawCall.MaterialNode.Index : -1;
                cboVisBone.SelectedIndex =
                    drawCall.VisibilityBoneNode != null ? drawCall.VisibilityBoneNode.BoneIndex : -1;

                _prevDrawOrder = numDrawOrder.Value = drawCall.DrawPriority;
                numDrawOrder.Enabled = !(chkDoesntMatter.Checked = drawCall.DrawPriority == 0);
                cboDrawPass.SelectedIndex = (int) drawCall.DrawPass;
                lstDrawCalls.SetItemChecked(lstDrawCalls.SelectedIndex, drawCall._render);

                _updating = false;

                //drawCall._render = true;
            }
        }

        private void cboMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            DrawCall drawCall = lstDrawCalls.SelectedItem as DrawCall;
            if (drawCall != null)
            {
                drawCall.MaterialNode = cboMaterial.SelectedItem as MDL0MaterialNode;
                drawCall._parentObject.SignalPropertyChange();

                //lstDrawCalls.BeginUpdate();
                //int i = lstDrawCalls.SelectedIndex;
                //lstDrawCalls.DataSource = _targetObject._drawCalls;
                //lstDrawCalls.SelectedIndex = i;
                //lstDrawCalls.EndUpdate();
            }
        }

        private void cboVisBone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            DrawCall drawCall = lstDrawCalls.SelectedItem as DrawCall;
            if (drawCall != null)
            {
                drawCall.VisibilityBoneNode = cboVisBone.SelectedItem as MDL0BoneNode;
                drawCall._parentObject.Model.RegenerateVIS0Indices();
                drawCall._parentObject.SignalPropertyChange();
            }
        }

        private void numDrawOrder_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            DrawCall drawCall = lstDrawCalls.SelectedItem as DrawCall;
            if (drawCall != null)
            {
                drawCall.DrawPriority = (byte) numDrawOrder.Value;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (_targetObject != null)
            {
                DrawCall d = new DrawCall(_targetObject);
                _targetObject._drawCalls.Add(d);
                _targetObject.OnDrawCallsChanged();
                _targetObject.Model.RegenerateVIS0Indices();
                _targetObject.SignalPropertyChange();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstDrawCalls.SelectedIndices != null)
            {
                for (int x = lstDrawCalls.SelectedIndices.Count - 1; x >= 0; x--)
                {
                    DrawCall drawCall = lstDrawCalls.Items[lstDrawCalls.SelectedIndices[x]] as DrawCall;
                    if (drawCall != null)
                    {
                        MDL0ObjectNode o = drawCall._parentObject;

                        o._drawCalls.Remove(drawCall);
                        o.OnDrawCallsChanged();
                        o.Model.RegenerateVIS0Indices();
                        o.SignalPropertyChange();
                    }
                }
            }
        }

        private void floorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelPanel.RenderFloor = !modelPanel.RenderFloor;
        }

        private void shadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelPanel.RenderShaders = !modelPanel.RenderShaders;
        }

        private void bonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelPanel.RenderBones = !modelPanel.RenderBones;
        }

        private float _prevDrawOrder;

        private void chkDoesntMatter_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (!(numDrawOrder.Enabled = !chkDoesntMatter.Checked))
            {
                _prevDrawOrder = numDrawOrder.Value;
                numDrawOrder.Value = 0;
            }
            else
            {
                numDrawOrder.Value = _prevDrawOrder.Clamp(1, 255);
            }

            numDrawOrder_ValueChanged(null, null);
        }

        private void cboDrawPass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            DrawCall drawCall = lstDrawCalls.SelectedItem as DrawCall;
            if (drawCall != null)
            {
                drawCall.DrawPass = (DrawCall.DrawPassType) cboDrawPass.SelectedIndex;
            }
        }

        private void lstDrawCalls_DoubleClick(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            DrawCall drawCall = lstDrawCalls.SelectedItem as DrawCall;
            if (drawCall != null)
            {
                drawCall._render = !drawCall._render;
                lstDrawCalls.SetItemChecked(lstDrawCalls.SelectedIndex, drawCall._render);

                if (_targetObject?.Model != null)
                {
                    TKContext.InvalidateModelPanels(_targetObject.Model);
                }
            }
        }
    }
}
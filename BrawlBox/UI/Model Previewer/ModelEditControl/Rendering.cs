using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace System.Windows.Forms
{
    public partial class ModelEditControl : ModelEditorBase
    {
        public unsafe override void modelPanel1_PostRender(ModelPanelViewport panel)
        {
            RenderBrawlStageData(panel);
            base.modelPanel1_PostRender(panel);
        }

        public void RenderBrawlStageData(ModelPanelViewport panel)
        {
            //If you ever make changes to GL attributes (enabled and disabled things)
            //and don't want to keep track of what you changed,
            //you can push all attributes and then pop them when you're done, like this.
            //This will make sure the GL state is back to how it was before you changed it.
            GL.PushAttrib(AttribMask.AllAttribBits);

            GL.Disable(EnableCap.DepthTest);

            if (RenderCollisions)
                foreach (CollisionNode node in _collisions)
                    node.Render();
            
            #region RenderOverlays
            List<MDL0BoneNode> ItemBones = new List<MDL0BoneNode>();

            MDL0Node stgPos = null;

            MDL0BoneNode CamBone0 = null, CamBone1 = null,
                         DeathBone0 = null, DeathBone1 = null;

            //Get bones and render spawns if checked
            if (_targetModel != null &&
                _targetModel is MDL0Node &&
                ((((ResourceNode)_targetModel).Name.Contains("StgPosition")) ||
                ((ResourceNode)_targetModel).Name.Contains("stagePosition")))
                stgPos = _targetModel as MDL0Node;
            else if (_targetModels != null)
                stgPos = _targetModels.Find(x => x is MDL0Node &&
                    ((ResourceNode)x).Name.Contains("StgPosition") ||
                    ((ResourceNode)x).Name.Contains("stagePosition")) as MDL0Node;

            if (stgPos != null) 
                foreach (MDL0BoneNode bone in stgPos._linker.BoneCache)
                {
                    if (bone._name == "CamLimit0N") { CamBone0 = bone; }
                    else if (bone.Name == "CamLimit1N") { CamBone1 = bone; }
                    else if (bone.Name == "Dead0N") { DeathBone0 = bone; }
                    else if (bone.Name == "Dead1N") { DeathBone1 = bone; }
                    else if (bone._name.Contains("Player") && chkSpawns.Checked)
                    {
                        Vector3 position = bone._frameMatrix.GetPoint();

                        if (PointCollides(position))
                            GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);
                        else 
                            GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);

                        TKContext.DrawSphere(position, 5.0f, 32);
                    }
                    else if (bone._name.Contains("Rebirth") && chkSpawns.Checked)
                    {
                        GL.Color4(1.0f, 1.0f, 1.0f, 0.1f);
                        TKContext.DrawSphere(bone._frameMatrix.GetPoint(), 5.0f, 32);
                    }
                    else if (bone._name.Contains("Item"))
                        ItemBones.Add(bone);
                }

            //Render item fields if checked
            if (ItemBones != null && chkItems.Checked)
            {
                GL.Color4(0.5f, 0.0f, 1.0f, 0.4f);
                for (int i = 0; i < ItemBones.Count; i += 2)
                {
                    Vector3 pos1 = new Vector3(ItemBones[i]._frameMatrix.GetPoint()._x, ItemBones[i]._frameMatrix.GetPoint()._y + 3.0f, 1.0f);
                    Vector3 pos2 = new Vector3(ItemBones[i + 1]._frameMatrix.GetPoint()._x, ItemBones[i + 1]._frameMatrix.GetPoint()._y - 3.0f, 1.0f);

                    TKContext.DrawBox(pos1, pos2);
                }
            }

            //Render boundaries if checked
            if (CamBone0 != null && CamBone1 != null && chkBoundaries.Checked)
            {
                //GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Lighting);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Front);

                GL.Color4(Color.Blue);
                GL.Begin(BeginMode.LineLoop);
                GL.LineWidth(15.0f);

                Vector3
                    camBone0 = CamBone0._frameMatrix.GetPoint(),
                    camBone1 = CamBone1._frameMatrix.GetPoint(),
                    deathBone0 = DeathBone0._frameMatrix.GetPoint(),
                    deathBone1 = DeathBone1._frameMatrix.GetPoint();

                GL.Vertex2(camBone0._x, camBone0._y);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.LineLoop);
                GL.Color4(Color.Red);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.End();
                GL.Color4(0.0f, 0.5f, 1.0f, 0.3f);
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone0._x, camBone0._y);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(camBone0._x, camBone0._y);
                GL.End();
            }

            #endregion

            GL.PopAttrib();
        }
    }
}

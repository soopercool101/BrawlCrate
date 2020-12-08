using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        public virtual void modelPanel1_PreRender(ModelPanelViewport vp)
        {
            if (vp != null)
            {
                if (vp._renderFloor)
                {
                    OnRenderFloor();
                }

                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Lequal);
            }
        }

        public virtual void modelPanel1_PostRender(ModelPanelViewport vp)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.Lighting);

            if (_targetModels != null)
            {
                foreach (IModel m in _targetModels)
                {
                    PostRender(m, vp);
                }
            }

            GL.Disable(EnableCap.DepthTest);

            if (RenderLightDisplay /* && vp == ModelPanel.CurrentViewport*/)
            {
                OnRenderLightDisplay(vp);
            }

            if (TargetAnimType == NW4RAnimType.SCN && vp.RenderSCN0Controls)
            {
                RenderSCN0Controls(vp);
            }

            //For now we'll clear the depth buffer bit here.
            //We're not using the model depth in any way so it doesn't matter
            //The problem with not doing this at the moment is the rotation control clips with the model.
            //This is because the rotation axes need to be lequal depth tested against an invisible sphere
            //I don't know how to test a sub depth buffer and then always make it pass on the actual buffer
            GL.Clear(ClearBufferMask.DepthBufferBit);

            RenderTransformControls(vp);
            RenderDepth(vp);
        }

        #region SCN0 Controls

        public unsafe void RenderSCN0Controls(ModelPanelViewport vp)
        {
            if (_scn0 == null)
            {
                return;
            }

            int frame = CurrentFrame - 1;
            if (frame < 0)
            {
                return;
            }

            GL.Color3(Color.Blue);
            GL.Disable(EnableCap.Lighting);

            if (_SCN0LightSet != null)
            {
                int i = 0;
                foreach (SCN0LightNode l in _SCN0LightSet._lights)
                {
                    if (l == null)
                    {
                        i++;
                        continue;
                    }

                    Vector3 start = l.GetStart(frame);
                    Vector3 end = l.GetEnd(frame);

                    switch (l.LightType)
                    {
                        case LightType.Spotlight:
                        case LightType.Directional:
                            GL.Begin(BeginMode.Lines);
                            GL.Color3((Color) l.GetColor(frame, 0));
                            GL.Vertex3((OpenTK.Vector3) start);
                            if (l.SpecularEnabled)
                            {
                                GL.Color3((Color) l.GetColor(frame, 1));
                            }

                            GL.Vertex3((OpenTK.Vector3) end);
                            GL.End();
                            if (l.LightType == LightType.Spotlight)
                            {
                                float radius = start.TrueDistance(end) *
                                               (float) Math.Tan(Maths._deg2radf * l.SpotCut.GetFrameValue(frame));
                                Matrix x = Matrix.TransformMatrix(
                                    new Vector3(radius),
                                    Maths._rad2degf * end.LookatAngles(start),
                                    end);
                                GL.PushMatrix();
                                GL.MultMatrix((float*) &x);
                                TKContext.GetRingList().Call();
                                GL.PopMatrix();
                                break;
                            }

                            break;
                        case LightType.Point:
                            GL.Color4((Color) l.GetColor(frame, 0));
                            GL.PushMatrix();
                            Matrix m = Matrix.TransformMatrix(
                                new Vector3(l.RefDist.GetFrameValue(frame)),
                                CameraFacingRotation(vp.Camera, start),
                                start);
                            GL.MultMatrix((float*) &m);
                            TKContext.GetRingList().Call();
                            GL.PopMatrix();
                            break;
                    }

                    foreach (ModelPanelViewport v in ModelPanel)
                    {
                        v.SettingsScreenText[l.Name] = v.Camera.Project(start);
                        //v.SettingsScreenText[l.Name] = v.Camera.Project(end);
                    }

                    //GL.Color4(Color.MediumPurple);
                    //GL.Begin(PrimitiveType.LineStrip);
                    //for (int i = 0; i < MaxFrame; i++)
                    //    GL.Vertex3(l.GetFrameValue(LightKeyframeMode.StartX, i), l.GetFrameValue(LightKeyframeMode.StartY, i), l.GetFrameValue(LightKeyframeMode.StartZ, i));
                    //GL.End();

                    //GL.Color4(Color.ForestGreen);
                    //GL.Begin(PrimitiveType.LineStrip);
                    //for (int i = 0; i < MaxFrame; i++)
                    //    GL.Vertex3(l.GetFrameValue(LightKeyframeMode.EndX, i), l.GetFrameValue(LightKeyframeMode.EndY, i), l.GetFrameValue(LightKeyframeMode.EndZ, i));
                    //GL.End();

                    //Render these if selected
                    //if ((_lightStartSelected || _lightEndSelected) && l == scn0Editor._light)
                    //{
                    //    Matrix m;
                    //    float s1 = start.TrueDistance(CamLoc) / _orbRadius * 0.1f;
                    //    float e1 = end.TrueDistance(CamLoc) / _orbRadius * 0.1f;
                    //    GLDisplayList axis = GetAxes();
                    //    if (_lightStartSelected)
                    //    {
                    //        m = Matrix.TransformMatrix(new Vector3(s1), new Vector3(), start);

                    //        GL.PushMatrix();
                    //        GL.MultMatrix((float*)&m);

                    //        axis.Call();
                    //        GL.PopMatrix();
                    //    }
                    //    if (_lightEndSelected)
                    //    {
                    //        m = Matrix.TransformMatrix(new Vector3(e1), new Vector3(), end);

                    //        GL.PushMatrix();
                    //        GL.MultMatrix((float*)&m);

                    //        axis.Call();
                    //        GL.PopMatrix();
                    //    }
                    //}
                    i++;
                }
            }

            if (_SCN0Camera != null)
            {
                Vector3 start = _SCN0Camera.GetStart(frame);
                Vector3 end = new Vector3();

                if (_SCN0Camera.Type == SCN0CameraType.Aim)
                {
                    end = _SCN0Camera.GetEnd(frame);
                }
                else
                {
                    Matrix r = Matrix.TranslationMatrix(new Vector3(0.0f, 0.0f, -1.0f) *
                                                        Matrix.RotationMatrix(_SCN0Camera.GetRotate(frame)));
                    end = r * start;
                }

                GL.Color3(Color.Green);
                GL.Begin(BeginMode.Lines);

                GL.Vertex3((OpenTK.Vector3) start);
                GL.Vertex3((OpenTK.Vector3) end);

                GL.End();

                foreach (ModelPanelViewport v in ModelPanel)
                {
                    v.SettingsScreenText[_SCN0Camera.Name] = v.Camera.Project(start);
                    //v.SettingsScreenText["Camera Aim"] = v.Camera.Project(end);
                }

                //GL.Color4(Color.OrangeRed);
                //GL.Begin(PrimitiveType.LineStrip);
                //for (int i = 0; i < MaxFrame; i++)
                //    GL.Vertex3(c.GetFrameValue(CameraKeyframeMode.PosX, i), c.GetFrameValue(CameraKeyframeMode.PosY, i), c.GetFrameValue(CameraKeyframeMode.PosZ, i));
                //GL.End();

                //GL.Color4(Color.SkyBlue);
                //GL.Begin(PrimitiveType.LineStrip);
                //for (int i = 0; i < MaxFrame; i++)
                //    GL.Vertex3(c.GetFrameValue(CameraKeyframeMode.AimX, i), c.GetFrameValue(CameraKeyframeMode.AimY, i), c.GetFrameValue(CameraKeyframeMode.AimZ, i));
                //GL.End();

                GL.Color3(Color.Black);

                //Render these if selected
                //if (_lightStartSelected || _lightEndSelected)
                //{
                //    Matrix m;
                //    float s = start.TrueDistance(CamLoc) / _orbRadius * 0.1f;
                //    float e = end.TrueDistance(CamLoc) / _orbRadius * 0.1f;
                //    GLDisplayList axis = GetAxes();
                //    if (_lightStartSelected)
                //    {
                //        m = Matrix.TransformMatrix(new Vector3(s), new Vector3(), start);

                //        GL.PushMatrix();
                //        GL.MultMatrix((float*)&m);

                //        axis.Call();
                //        GL.PopMatrix();
                //    }
                //    if (_lightEndSelected)
                //    {
                //        m = Matrix.TransformMatrix(new Vector3(e), new Vector3(), end);

                //        GL.PushMatrix();
                //        GL.MultMatrix((float*)&m);

                //        axis.Call();
                //        GL.PopMatrix();
                //    }
                //}
            }
        }

        #endregion

        public virtual void PostRender(IModel model, ModelPanelViewport vp)
        {
            if (vp._renderAttrib._renderVertices)
            {
                model.RenderVertices(false, SelectedBone, vp.Camera);
            }

            if (vp._renderAttrib._renderNormals)
            {
                model.RenderNormals();
            }

            if (vp._renderAttrib._renderBones)
            {
                model.RenderBones(vp);
            }

            model.RenderBoxes(
                vp._renderAttrib._renderModelBox,
                vp._renderAttrib._renderObjectBoxes,
                vp._renderAttrib._renderBoneBoxes,
                vp._renderAttrib._useBindStateBoxes);
        }

        public enum CoordinateType
        {
            Local,
            World,
            Screen
        }

        public CoordinateType[] _coordinateTypes = new CoordinateType[]
        {
            CoordinateType.Local, //T
            CoordinateType.Local, //R
            CoordinateType.Local  //S
        };

        #region Transform Control Rendering

        public void RenderTransformControls(ModelPanelViewport panel)
        {
            if (_playing || ControlType == TransformType.None)
            {
                return;
            }

            bool hasBone = SelectedBone != null;
            if (hasBone || VertexLoc.HasValue)
            {
                Vector3 pos;
                Matrix rot = Matrix.Identity;
                float radius;

                if (hasBone)
                {
                    pos = BoneLoc(SelectedBone);
                    radius = OrbRadius(pos, panel.Camera);
                    switch (_coordinateTypes[(int) ControlType])
                    {
                        case CoordinateType.Local:
                            rot = GetBoneWorldMtx().GetRotationMatrix();
                            break;
                        case CoordinateType.World:
                            //rot = Matrix.Identity; //Already set to identity above
                            break;
                        case CoordinateType.Screen:
                            //rot = CameraFacingRotationMatrix(panel, pos);
                            rot = Matrix.RotationMatrix(panel.Camera._rotation);
                            break;
                    }

                    RenderTransformControl(pos, rot, radius, panel, _boneSelection);
                }

                if (VertexLoc.HasValue)
                {
                    pos = VertexLoc.Value;
                    radius = OrbRadius(pos, panel.Camera);
                    switch (_coordinateTypes[(int) ControlType])
                    {
                        case CoordinateType.Local:
                        case CoordinateType.World:
                            //rot = Matrix.Identity; //Already set to identity above
                            break;
                        case CoordinateType.Screen:
                            rot = CameraFacingRotationMatrix(panel, pos);
                            break;
                    }

                    RenderTransformControl(pos, rot, radius, panel, _vertexSelection);
                }
            }
        }

        public void RenderTransformControl(
            Vector3 pos,
            Matrix rot,
            float radius,
            ModelPanelViewport panel,
            SelectionParams selection)
        {
            switch (ControlType)
            {
                case TransformType.Translation:
                    RenderTranslationControl(pos, radius, rot, panel, selection);
                    break;
                case TransformType.Rotation:
                    RenderRotationControl(pos, radius, rot, panel, selection);
                    break;
                case TransformType.Scale:
                    RenderScaleControl(pos, radius, rot, panel, selection);
                    break;
            }
        }

        public unsafe void RenderTranslationControl(
            Vector3 position,
            float radius,
            Matrix rotation,
            ModelPanelViewport panel,
            SelectionParams selection)
        {
            Matrix m = Matrix.TransformMatrix(new Vector3(radius * 0.25f), new Vector3(), position) *
                       CameraFacingRotationMatrix(panel, position);

            GL.PushMatrix();
            GL.MultMatrix((float*) &m);

            GL.Color4(selection._hiCirc || selection._snapCirc ? Color.Yellow : Color.Gray);

            GL.Begin(BeginMode.LineLoop);

            GL.Vertex2(-0.5f, -0.5f);
            GL.Vertex2(-0.5f, 0.5f);
            GL.Vertex2(0.5f, 0.5f);
            GL.Vertex2(0.5f, -0.5f);
            GL.Vertex2(-0.5f, -0.5f);

            GL.End();

            GL.PopMatrix();

            //Enter local space
            m = Matrix.TransformMatrix(new Vector3(radius), new Vector3(), position) * rotation;
            GL.PushMatrix();
            GL.MultMatrix((float*) &m);

            GetTranslationAxes(selection).Call();

            GL.PopMatrix();

            panel.SettingsScreenText["X"] = panel.Camera.Project(new Vector3(_axisLDist + 0.1f, 0, 0) * m) -
                                            new Vector3(8.0f, 8.0f, 0);
            panel.SettingsScreenText["Y"] = panel.Camera.Project(new Vector3(0, _axisLDist + 0.1f, 0) * m) -
                                            new Vector3(8.0f, 8.0f, 0);
            panel.SettingsScreenText["Z"] = panel.Camera.Project(new Vector3(0, 0, _axisLDist + 0.1f) * m) -
                                            new Vector3(8.0f, 8.0f, 0);
        }

        public unsafe void RenderScaleControl(
            Vector3 pos,
            float radius,
            Matrix rotation,
            ModelPanelViewport panel,
            SelectionParams selection)
        {
            //Enter local space
            Matrix m = Matrix.TransformMatrix(new Vector3(radius), new Vector3(), pos) * rotation;
            GL.PushMatrix();
            GL.MultMatrix((float*) &m);

            GetScaleAxes(selection).Call();

            GL.PopMatrix();

            panel.SettingsScreenText["X"] = panel.Camera.Project(new Vector3(_axisLDist + 0.1f, 0, 0) * m) -
                                            new Vector3(8.0f, 8.0f, 0);
            panel.SettingsScreenText["Y"] = panel.Camera.Project(new Vector3(0, _axisLDist + 0.1f, 0) * m) -
                                            new Vector3(8.0f, 8.0f, 0);
            panel.SettingsScreenText["Z"] = panel.Camera.Project(new Vector3(0, 0, _axisLDist + 0.1f) * m) -
                                            new Vector3(8.0f, 8.0f, 0);
        }

        public unsafe void RenderRotationControl(
            Vector3 position,
            float radius,
            Matrix rotation,
            ModelPanelViewport panel,
            SelectionParams selection)
        {
            Matrix m = Matrix.TransformMatrix(new Vector3(radius), new Vector3(), position) *
                       CameraFacingRotationMatrix(panel, position);

            GL.PushMatrix();
            GL.MultMatrix((float*) &m);

            GLDisplayList sphere = TKContext.GetCircleList();
            GLDisplayList circle = TKContext.GetRingList();

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.PushAttrib(AttribMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            //Orb
            GL.Color4(0.7f, 0.7f, 0.7f, 0.15f);
            sphere.Call();

            GL.Disable(EnableCap.DepthTest);

            //Container
            GL.Color4(0.4f, 0.4f, 0.4f, 1.0f);
            circle.Call();

            //Circ
            if (selection._snapCirc || selection._hiCirc)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(1.0f, 0.8f, 0.5f, 1.0f);
            }

            GL.Scale(_circOrbScale, _circOrbScale, _circOrbScale);
            circle.Call();

            //Pop
            GL.PopMatrix();

            GL.Enable(EnableCap.DepthTest);

            //Enter local space
            m = Matrix.TransformMatrix(new Vector3(radius), new Vector3(), position) * rotation;

            panel.SettingsScreenText["X"] =
                panel.Camera.Project(new Vector3(1.1f, 0, 0) * m) - new Vector3(8.0f, 8.0f, 0);
            panel.SettingsScreenText["Y"] =
                panel.Camera.Project(new Vector3(0, 1.1f, 0) * m) - new Vector3(8.0f, 8.0f, 0);
            panel.SettingsScreenText["Z"] =
                panel.Camera.Project(new Vector3(0, 0, 1.1f) * m) - new Vector3(8.0f, 8.0f, 0);

            GL.PushMatrix();
            GL.MultMatrix((float*) &m);

            //Z
            if (selection._snapZ || selection._hiZ)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(0.0f, 0.0f, 1.0f, 1.0f);
            }

            circle.Call();
            GL.Rotate(90.0f, 0.0f, 1.0f, 0.0f);

            //X
            if (selection._snapX || selection._hiX)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Red);
            }

            circle.Call();
            GL.Rotate(90.0f, 1.0f, 0.0f, 0.0f);

            //Y
            if (selection._snapY || selection._hiY)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Green);
            }

            circle.Call();

            //Pop
            GL.PopMatrix();

            GL.PopAttrib();
        }

        #endregion

        #region Scale/Translation Display Lists

        public const float _axisLDist = 2.0f;
        public const float _axisHalfLDist = 0.75f;
        public const float _apthm = 0.075f;
        public const float _dst = 1.5f;

        public GLDisplayList GetTranslationAxes(SelectionParams selection)
        {
            //Create the axes.
            GLDisplayList axis = new GLDisplayList();
            axis.Begin();

            //Disable culling so square bases for the arrows aren't necessary to draw
            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Begin(BeginMode.Lines);

            //X

            if (selection._snapX && selection._snapY || selection._hiX && selection._hiY)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Red);
            }

            GL.Vertex3(_axisHalfLDist, 0.0f, 0.0f);
            GL.Vertex3(_axisHalfLDist, _axisHalfLDist, 0.0f);

            if (selection._snapX && selection._snapZ || selection._hiX && selection._hiZ)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Red);
            }

            GL.Vertex3(_axisHalfLDist, 0.0f, 0.0f);
            GL.Vertex3(_axisHalfLDist, 0.0f, _axisHalfLDist);

            if (selection._snapX || selection._hiX)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Red);
            }

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(_dst, 0.0f, 0.0f);

            GL.End();

            GL.Begin(BeginMode.Triangles);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, _apthm, -_apthm);
            GL.Vertex3(_dst, _apthm, _apthm);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, -_apthm, _apthm);
            GL.Vertex3(_dst, -_apthm, -_apthm);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, _apthm, _apthm);
            GL.Vertex3(_dst, -_apthm, _apthm);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, -_apthm, -_apthm);
            GL.Vertex3(_dst, _apthm, -_apthm);

            GL.End();

            GL.Begin(BeginMode.Lines);

            //Y

            if (selection._snapY && selection._snapX || selection._hiY && selection._hiX)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Green);
            }

            GL.Vertex3(0.0f, _axisHalfLDist, 0.0f);
            GL.Vertex3(_axisHalfLDist, _axisHalfLDist, 0.0f);

            if (selection._snapY && selection._snapZ || selection._hiY && selection._hiZ)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Green);
            }

            GL.Vertex3(0.0f, _axisHalfLDist, 0.0f);
            GL.Vertex3(0.0f, _axisHalfLDist, _axisHalfLDist);

            if (selection._snapY || selection._hiY)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Green);
            }

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, _dst, 0.0f);

            GL.End();

            GL.Begin(BeginMode.Triangles);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(_apthm, _dst, -_apthm);
            GL.Vertex3(_apthm, _dst, _apthm);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(-_apthm, _dst, _apthm);
            GL.Vertex3(-_apthm, _dst, -_apthm);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(_apthm, _dst, _apthm);
            GL.Vertex3(-_apthm, _dst, _apthm);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(-_apthm, _dst, -_apthm);
            GL.Vertex3(_apthm, _dst, -_apthm);

            GL.End();

            GL.Begin(BeginMode.Lines);

            //Z

            if (selection._snapZ && selection._snapX || selection._hiZ && selection._hiX)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Blue);
            }

            GL.Vertex3(0.0f, 0.0f, _axisHalfLDist);
            GL.Vertex3(_axisHalfLDist, 0.0f, _axisHalfLDist);

            if (selection._snapZ && selection._snapY || selection._hiZ && selection._hiY)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Blue);
            }

            GL.Vertex3(0.0f, 0.0f, _axisHalfLDist);
            GL.Vertex3(0.0f, _axisHalfLDist, _axisHalfLDist);

            if (selection._snapZ || selection._hiZ)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Blue);
            }

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, _dst);

            GL.End();

            GL.Begin(BeginMode.Triangles);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(_apthm, -_apthm, _dst);
            GL.Vertex3(_apthm, _apthm, _dst);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(-_apthm, _apthm, _dst);
            GL.Vertex3(-_apthm, -_apthm, _dst);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(_apthm, _apthm, _dst);
            GL.Vertex3(-_apthm, _apthm, _dst);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(-_apthm, -_apthm, _dst);
            GL.Vertex3(_apthm, -_apthm, _dst);

            GL.End();

            axis.End();

            return axis;
        }

        public const float _scaleHalf1LDist = 0.8f;
        public const float _scaleHalf2LDist = 1.2f;

        public GLDisplayList GetScaleAxes(SelectionParams selection)
        {
            //Create the axes.
            GLDisplayList axis = new GLDisplayList();
            axis.Begin();

            //Disable culling so square bases for the arrows aren't necessary to draw
            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Begin(BeginMode.Lines);

            //X
            if (selection._snapY && selection._snapZ || selection._hiY && selection._hiZ)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Red);
            }

            GL.Vertex3(0.0f, _scaleHalf1LDist, 0.0f);
            GL.Vertex3(0.0f, 0.0f, _scaleHalf1LDist);
            GL.Vertex3(0.0f, _scaleHalf2LDist, 0.0f);
            GL.Vertex3(0.0f, 0.0f, _scaleHalf2LDist);

            if (selection._snapX || selection._hiX)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Red);
            }

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(_dst, 0.0f, 0.0f);

            GL.End();

            GL.Begin(BeginMode.Triangles);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, _apthm, -_apthm);
            GL.Vertex3(_dst, _apthm, _apthm);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, -_apthm, _apthm);
            GL.Vertex3(_dst, -_apthm, -_apthm);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, _apthm, _apthm);
            GL.Vertex3(_dst, -_apthm, _apthm);

            GL.Vertex3(_axisLDist, 0.0f, 0.0f);
            GL.Vertex3(_dst, -_apthm, -_apthm);
            GL.Vertex3(_dst, _apthm, -_apthm);

            GL.End();

            GL.Begin(BeginMode.Lines);

            //Y
            if (selection._snapZ && selection._snapX || selection._hiZ && selection._hiX)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Green);
            }

            GL.Vertex3(0.0f, 0.0f, _scaleHalf1LDist);
            GL.Vertex3(_scaleHalf1LDist, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, _scaleHalf2LDist);
            GL.Vertex3(_scaleHalf2LDist, 0.0f, 0.0f);

            if (selection._snapY || selection._hiY)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Green);
            }

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, _dst, 0.0f);

            GL.End();

            GL.Begin(BeginMode.Triangles);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(_apthm, _dst, -_apthm);
            GL.Vertex3(_apthm, _dst, _apthm);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(-_apthm, _dst, _apthm);
            GL.Vertex3(-_apthm, _dst, -_apthm);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(_apthm, _dst, _apthm);
            GL.Vertex3(-_apthm, _dst, _apthm);

            GL.Vertex3(0.0f, _axisLDist, 0.0f);
            GL.Vertex3(-_apthm, _dst, -_apthm);
            GL.Vertex3(_apthm, _dst, -_apthm);

            GL.End();

            GL.Begin(BeginMode.Lines);

            //Z
            if (selection._snapX && selection._snapY || selection._hiX && selection._hiY)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Blue);
            }

            GL.Vertex3(0.0f, _scaleHalf1LDist, 0.0f);
            GL.Vertex3(_scaleHalf1LDist, 0.0f, 0.0f);
            GL.Vertex3(0.0f, _scaleHalf2LDist, 0.0f);
            GL.Vertex3(_scaleHalf2LDist, 0.0f, 0.0f);

            if (selection._snapZ || selection._hiZ)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.Blue);
            }

            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, _dst);

            GL.End();

            GL.Begin(BeginMode.Triangles);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(_apthm, -_apthm, _dst);
            GL.Vertex3(_apthm, _apthm, _dst);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(-_apthm, _apthm, _dst);
            GL.Vertex3(-_apthm, -_apthm, _dst);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(_apthm, _apthm, _dst);
            GL.Vertex3(-_apthm, _apthm, _dst);

            GL.Vertex3(0.0f, 0.0f, _axisLDist);
            GL.Vertex3(-_apthm, -_apthm, _dst);
            GL.Vertex3(_apthm, -_apthm, _dst);

            GL.End();

            axis.End();

            return axis;
        }

        #endregion

        #region Depth Rendering

#if DEBUG
        protected bool _renderDepth;
#endif
        public virtual void RenderDepth(ModelPanelViewport v)
        {
            if (v._grabbing || v._scrolling || _playing)
            {
                return;
            }

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Always);

            GL.Color4(Color.Black);
#if DEBUG
            GL.ColorMask(_renderDepth, false, false, false);
#else
            GL.ColorMask(false, false, false, false);
#endif

            if (v._renderAttrib._renderVertices)
            {
                if (EditingAll && _targetModels != null)
                {
                    foreach (IModel m in _targetModels)
                    {
                        m.RenderVertices(true, SelectedBone, v.Camera);
                    }
                }
                else
                {
                    TargetModel?.RenderVertices(true, SelectedBone, v.Camera);
                }
            }

            if (v._renderAttrib._renderBones)
            {
                //Render invisible depth orbs
                GLDisplayList list = TKContext.GetSphereList();
                bool doScale = v._renderAttrib._scaleBones;
                if (EditingAll)
                {
                    foreach (IModel m in _targetModels)
                    {
                        foreach (IBoneNode bone in m.BoneCache)
                        {
                            if (bone != SelectedBone)
                            {
                                RenderOrb(bone, list, v, doScale);
                            }
                        }
                    }
                }
                else if (TargetModel != null)
                {
                    foreach (IBoneNode bone in _targetModel.BoneCache)
                    {
                        if (bone != SelectedBone)
                        {
                            RenderOrb(bone, list, v, doScale);
                        }
                    }
                }
            }

            GL.ColorMask(true, true, true, true);
        }

        #endregion

        #region Orb Point Distance

        /// <summary>
        /// Use this for transforming points
        /// </summary>
        public bool GetTransformPoint(
            Vector2 mousePoint,
            out Vector3 point,
            ModelPanelViewport panel,
            Vector3 center,
            SelectionParams selection)
        {
            return GetTransformPoint(mousePoint, out point, panel, Matrix.TranslationMatrix(center), selection);
        }

        /// <summary>
        /// Gets world-point of specified mouse point projected onto the selected bone's local space if rotating or in world space if translating or scaling.
        /// Intersects the projected ray with the appropriate plane using the snap flags.
        /// </summary>
        public bool GetTransformPoint(
            Vector2 mousePoint,
            out Vector3 point,
            ModelPanelViewport panel,
            Matrix localTransform,
            SelectionParams selection)
        {
            Vector3 lineStart = panel.UnProject(mousePoint._x, mousePoint._y, 0.0f);
            Vector3 lineEnd = panel.UnProject(mousePoint._x, mousePoint._y, 1.0f);
            Vector3 center = localTransform.GetPoint();
            Vector3 camera = panel.Camera.GetPoint();
            Vector3 normal = new Vector3();

            bool axisSnap = selection._snapX || selection._snapY || selection._snapZ;
            CoordinateType coord = _coordinateTypes[(int) ControlType];

            switch (ControlType)
            {
                case TransformType.Rotation:

                    float radius = CamDistance(center, ModelPanel.CurrentViewport);
                    if (axisSnap)
                    {
                        switch (coord)
                        {
                            case CoordinateType.Screen:
                                if (selection._snapX || selection._snapY || selection._snapZ)
                                {
                                    normal = camera.Normalize(center);
                                }

                                break;
                            case CoordinateType.Local:
                                normal = (localTransform * new Vector3(
                                    selection._snapX ? 1.0f : 0.0f,
                                    selection._snapY ? 1.0f : 0.0f,
                                    selection._snapZ ? 1.0f : 0.0f)).Normalize(center);
                                break;
                            case CoordinateType.World:
                                normal = new Vector3(
                                    selection._snapX ? 1.0f : 0.0f,
                                    selection._snapY ? 1.0f : 0.0f,
                                    selection._snapZ ? 1.0f : 0.0f);
                                break;
                        }
                    }
                    else if (selection._snapCirc)
                    {
                        radius *= _circOrbScale;
                        normal = camera.Normalize(center);
                    }
                    else if (Maths.LineSphereIntersect(lineStart, lineEnd, center, radius, out point))
                    {
                        return true;
                    }
                    else
                    {
                        normal = camera.Normalize(center);
                    }

                    if (Maths.LinePlaneIntersect(lineStart, lineEnd, center, normal, out point))
                    {
                        point = Maths.PointAtLineDistance(center, point, radius);
                        return true;
                    }

                    break;

                case TransformType.Translation:
                case TransformType.Scale:

                    if (axisSnap)
                    {
                        if (selection._snapX && selection._snapY && selection._snapZ)
                        {
                            normal = Vector3.UnitZ * Matrix.RotationMatrix(panel.Camera._rotation);
                        }
                        else
                        {
                            switch (coord)
                            {
                                case CoordinateType.Screen:
                                    normal = Vector3.UnitZ * Matrix.RotationMatrix(panel.Camera._rotation);
                                    break;
                                case CoordinateType.Local:
                                case CoordinateType.World:

                                    //Remove local rotation
                                    if (coord == CoordinateType.World)
                                    {
                                        localTransform =
                                            Matrix.TranslationMatrix(center) *
                                            Matrix.ScaleMatrix(localTransform.GetScale());
                                    }

                                    if (selection._snapX && selection._snapY)
                                    {
                                        normal = (localTransform * Vector3.UnitZ).Normalize(center);
                                    }
                                    else if (selection._snapX && selection._snapZ)
                                    {
                                        normal = (localTransform * Vector3.UnitY).Normalize(center);
                                    }
                                    else if (selection._snapY && selection._snapZ)
                                    {
                                        normal = (localTransform * Vector3.UnitX).Normalize(center);
                                    }
                                    else //One of the snaps
                                    {
                                        Vector3 unitSnapAxis = new Vector3(
                                            selection._snapX ? 1.0f : 0.0f,
                                            selection._snapY ? 1.0f : 0.0f,
                                            selection._snapZ ? 1.0f : 0.0f);

                                        float camDist = camera.TrueDistance(center);
                                        Vector3 camVec = camera.Normalize(center);
                                        float ratio = camVec.Dot(unitSnapAxis) /
                                                      (camVec.TrueDistance() * unitSnapAxis.TrueDistance());
                                        float lineDist = camDist * ratio;
                                        Vector3 endPoint = localTransform * (unitSnapAxis * lineDist);
                                        normal = camera.Normalize(endPoint);
                                    }

                                    break;
                            }
                        }
                    }
                    else
                    {
                        normal = Vector3.UnitZ * Matrix.RotationMatrix(panel.Camera._rotation);
                    }

                    break;
            }

            return Maths.LinePlaneIntersect(lineStart, lineEnd, center, normal, out point);
        }

        public Vertex3 CompareVertexDistance(Vector3 point)
        {
            if (TargetModel == null)
            {
                return null;
            }

            if (TargetModel.SelectedObjectIndex != -1)
            {
                IObject o = TargetModel.Objects[TargetModel.SelectedObjectIndex];
                if (o.IsRendering)
                {
                    foreach (Vertex3 v in o.Vertices)
                    {
                        float t = v.WeightedPosition.TrueDistance(point);
                        if (Math.Abs(t) < 0.02f)
                        {
                            return v;
                        }
                    }
                }
                else
                {
                    foreach (IObject w in TargetModel.Objects)
                    {
                        if (w.IsRendering)
                        {
                            foreach (Vertex3 v in w.Vertices)
                            {
                                float t = v.WeightedPosition.TrueDistance(point);
                                if (Math.Abs(t) < 0.02f)
                                {
                                    return v;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (IObject o in TargetModel.Objects)
                {
                    if (o.IsRendering)
                    {
                        foreach (Vertex3 v in o.Vertices)
                        {
                            float t = v.WeightedPosition.TrueDistance(point);
                            if (Math.Abs(t) < 0.02f)
                            {
                                return v;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private bool CompareBoneDistanceRecursive(IBoneNode bone, Vector3 point, ref IBoneNode match,
                                                  ModelPanelViewport v, bool doScale)
        {
            float dist = bone.Matrix.GetPoint().TrueDistance(point) / (doScale ? OrbRadius(bone, v) : 1.0f);
            if (Math.Abs(dist - MDL0BoneNode._nodeRadius) < 0.01f)
            {
                match = bone;
                return true;
            }

            foreach (IBoneNode b in ((ResourceNode) bone).Children)
            {
                if (CompareBoneDistanceRecursive(b, point, ref match, v, doScale))
                {
                    return true;
                }
            }

            return false;
        }

        public unsafe void RenderOrb(IBoneNode bone, GLDisplayList list, ModelPanelViewport v, bool doScale)
        {
            float radius = MDL0BoneNode._nodeRadius * (doScale ? OrbRadius(bone, v) : 1.0f);

            Matrix m = Matrix.TransformMatrix(new Vector3(radius), new Vector3(), bone.Matrix.GetPoint());
            GL.PushMatrix();
            GL.MultMatrix((float*) &m);

            list.Call();
            GL.PopMatrix();
        }

        #endregion

        public static void OnRenderLightDisplay(ModelPanelViewport v)
        {
            GL.PushAttrib(AttribMask.AllAttribBits);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();

            GL.Color4(Color.Blue);
            GL.Disable(EnableCap.Lighting);
            //GL.Disable(EnableCap.DepthTest);

            Vector4 lightPos = v.LightPosition;

            if (!v.LightDirectional)
            {
                GL.Enable(EnableCap.DepthTest);

                Vector3 pos = (Vector3) v._posLight;

                GLDisplayList list = TKContext.GetSphereList();
                GL.Translate((OpenTK.Vector3) pos);

                GL.Color4(Color.FromArgb(120, 100, 100, 255));

                //Matrix m = CameraFacingRotationMatrix(v.Camera, pos);
                //GL.MultMatrix((float*)&m);

                GL.Scale(lightPos._x, lightPos._x, lightPos._x);

                list.Call();
            }
            else
            {
                GL.Disable(EnableCap.DepthTest);

                GL.Scale(lightPos._x, lightPos._x, lightPos._x);

                GL.Rotate(90.0f, 1, 0, 0);

                float
                    azimuth = lightPos._y.RemapToRange(-180.0f, 180.0f),
                    elevation = lightPos._z.RemapToRange(-180.0f, 180.0f);

                if (Math.Abs(azimuth) == Math.Abs(elevation) && azimuth % 180.0f == 0 && elevation % 180.0f == 0)
                {
                    azimuth = 0;
                    elevation = 0;
                }

                int i;
                float e = azimuth, x;

                bool flip = false;
                if (e < 0)
                {
                    e = -e;
                    flip = true;
                    GL.Rotate(180.0f, 1, 0, 0);
                }

                float f = (int) e;
                float diff = (float) Math.Round(e - f, 1);

                GL.Begin(BeginMode.Lines);
                for (i = 0; i < f; i++)
                {
                    GL.Vertex2(Math.Cos(i * Maths._deg2radf), Math.Sin(i * Maths._deg2radf));
                    GL.Vertex2(Math.Cos((i + 1) * Maths._deg2radf), Math.Sin((i + 1) * Maths._deg2radf));
                }

                for (x = 0; x < diff; x += 0.1f)
                {
                    GL.Vertex2(Math.Cos((x + i) * Maths._deg2radf), Math.Sin((x + i) * Maths._deg2radf));
                    GL.Vertex2(Math.Cos((x + 0.1f + i) * Maths._deg2radf), Math.Sin((x + 0.1f + i) * Maths._deg2radf));
                }

                GL.End();

                if (flip)
                {
                    GL.Rotate(-180.0f, 1, 0, 0);
                }

                GL.Rotate(90.0f, 0, 1, 0);
                GL.Rotate(90.0f, 0, 0, 1);
                GL.Rotate(180.0f, 1, 0, 0);
                GL.Rotate(90.0f - azimuth, 0, 1, 0);

                e = elevation;

                if (e < 0)
                {
                    e = -e;
                    GL.Rotate(180.0f, 1, 0, 0);
                }

                f = (int) e;
                diff = (float) Math.Round(e - f, 1);

                GL.Begin(BeginMode.Lines);
                for (i = 0; i < f; i++)
                {
                    GL.Vertex2(Math.Cos(i * Maths._deg2radf), Math.Sin(i * Maths._deg2radf));
                    GL.Vertex2(Math.Cos((i + 1) * Maths._deg2radf), Math.Sin((i + 1) * Maths._deg2radf));
                }

                for (x = 0; x < diff; x += 0.1f)
                {
                    GL.Vertex2(Math.Cos((x + i) * Maths._deg2radf), Math.Sin((x + i) * Maths._deg2radf));
                    GL.Vertex2(Math.Cos((x + 0.1f + i) * Maths._deg2radf), Math.Sin((x + 0.1f + i) * Maths._deg2radf));
                }

                GL.Vertex2(Math.Cos((x + i) * Maths._deg2radf), Math.Sin((x + i) * Maths._deg2radf));
                GL.Color4(Color.Orange);
                GL.Vertex3(0, 0, 0);
                GL.End();

                GL.Scale(0.01f, 0.01f, 0.01f);
                GL.Rotate(azimuth, 0, 1, 0);
                GL.Enable(EnableCap.DepthTest);

                GL.PopAttrib();
                GL.PopMatrix();
            }
        }

        public static void OnRenderFloor()
        {
            float s = 10.0f, t = 10.0f;
            float e = 30.0f;

            GL.PushAttrib(AttribMask.AllAttribBits);

            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Lighting);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);

            //So that the model clips with the floor
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();

            GLTexture bgTex = TKContext.FindOrCreate("TexBG", GLTexturePanel.CreateBG);
            bgTex.Bind();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);

            GL.Color4(_floorHue);

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-e, 0.0f, -e);
            GL.TexCoord2(s, 0.0f);
            GL.Vertex3(e, 0.0f, -e);
            GL.TexCoord2(s, t);
            GL.Vertex3(e, 0.0f, e);
            GL.TexCoord2(0.0f, t);
            GL.Vertex3(-e, 0.0f, e);

            GL.End();

            GL.Disable(EnableCap.Texture2D);

            GL.PopAttrib();
        }
    }
}
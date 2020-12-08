using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        #region Mouse Down

        #region Targeting

        private void MouseDownTargetBone(MouseEventArgs e, ModelPanel panel)
        {
            //Re-target selected bone
            if (SelectedBone != null)
            {
                _boneSelection.ApplySnaps();

                //Targeting functions are done in HighlightStuff
                if (!_boneSelection.SnappingAny() && !_boneSelection._hiSphere)
                {
                    //Orb selection missed. Assign bone and move to next step.
                    if (_hiVertex == null && !Ctrl)
                    {
                        SelectedBone = null;
                    }

                    goto GetBone;
                }

                //Bone re-targeted. Get frame values and local point aligned to snapping plane.
                if (GetTransformPoint(new Vector2(e.X, e.Y), out Vector3 point, panel.CurrentViewport,
                    GetBoneWorldMtx(), _boneSelection))
                {
                    _boneSelection.Update(ControlType, point, GetBoneInvWorldMtx() * point, SelectedBone.FrameState);
                    if (_boneSelection.IsMoving())
                    {
                        BoneChange(SelectedBone);
                    }

                    panel.CurrentViewport.AllowSelection = false;
                }
            }

            GetBone:

            //Try selecting new bone
            if (SelectedBone == null && panel.RenderBones)
            {
                SelectedBone = _hiBone;
            }
        }

        public bool MouseDownTargetVertex(MouseEventArgs e, ModelPanel panel)
        {
            bool ok = false;

            //Re-target selected vertex
            if (VertexLoc.HasValue)
            {
                _vertexSelection.ApplySnaps();

                if (!_vertexSelection.SnappingAny() && !_vertexSelection._hiSphere)
                {
                    if (NotCtrlAlt && SelectedBone == null)
                    {
                        ClearSelectedVertices();
                    }

                    ok = true;
                    goto GetVertex;
                }

                //Vertex re-targeted. Get translation and point (aligned to snapping plane).
                if (GetTransformPoint(new Vector2(e.X, e.Y), out Vector3 point, panel.CurrentViewport, VertexLoc.Value,
                    _vertexSelection))
                {
                    panel.CurrentViewport.AllowSelection = false;
                    _vertexSelection.Update(ControlType, point, point * Matrix.TranslationMatrix(-VertexLoc.Value),
                        Vector3.Zero, VertexLoc.Value, Vector3.One);
                    if (_vertexSelection.IsMoving())
                    {
                        VertexChange(_selectedVertices);
                    }
                }
            }
            else
            {
                ok = true;
            }

            if (!ok)
            {
                return true;
            }

            GetVertex:

            bool verticesChanged = false;
            if (ok)
            {
                if (_hiVertex != null)
                {
                    if (Ctrl)
                    {
                        if (!_selectedVertices.Contains(_hiVertex))
                        {
                            SelectVertex(_hiVertex, true);
                            verticesChanged = true;
                        }
                        else
                        {
                            SelectVertex(_hiVertex, false);
                            verticesChanged = true;
                        }
                    }
                    else if (Alt)
                    {
                        if (_selectedVertices.Contains(_hiVertex))
                        {
                            SelectVertex(_hiVertex, false);
                            verticesChanged = true;
                        }
                    }
                    else
                    {
                        ClearSelectedVertices();
                        if (_hiVertex != null)
                        {
                            SelectVertex(_hiVertex, true);
                            verticesChanged = true;
                        }
                    }
                }
                else if (NotCtrlAlt && SelectedBone == null)
                {
                    ClearSelectedVertices();
                }

                if (verticesChanged)
                {
                    OnSelectedVerticesChanged();
                }
            }

            return verticesChanged;
        }

        #endregion

        protected virtual void modelPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            _createdNewBone = false;

            ModelPanel panel = sender as ModelPanel;
            ModelPanelViewport viewport = panel.CurrentViewport;

            if (panel._draggingViewports)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (DoNotHighlightOnMouseMove)
                {
                    HighlightStuff(e, panel);

                    //Reset the cursor (HighlightStuff sets the cursor)
                    panel.Cursor = Cursors.Default;
                }

                //Reset snap flags
                _boneSelection.ResetSnaps();
                _vertexSelection.ResetSnaps();

                MouseDownTargetBone(e, panel);

                if (!MouseDownTargetVertex(e, panel))
                {
                    if (CurrentFrame == 0 &&
                        TargetAnimType == NW4RAnimType.CHR &&
                        CHR0Editor.chkMoveBoneOnly.Checked && TargetModel is MDL0Node)
                    {
                        MDL0Node m = TargetModel as MDL0Node;
                        m._dontUpdateMesh = true;
                    }
                }

                //Ensure a redraw so the snapping indicators are correct
                panel.Invalidate();
            }
        }

        #endregion

        #region Mouse Up

        protected virtual void modelPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_createdNewBone && SelectedBone != null)
            {
                SelectedBone.BindState = SelectedBone.FrameState;
                SelectedBone.RecalcBindState(false, false, false);
            }

            _createdNewBone = false;

            bool temp = TargetModel is MDL0Node;
            if (temp)
            {
                MDL0Node m = TargetModel as MDL0Node;
                m._dontUpdateMesh = false;
            }

            if (e.Button == MouseButtons.Left)
            {
                ModelPanel panel = sender as ModelPanel;

                panel.CurrentViewport.AllowSelection = true;

                if (_vertexSelection.IsMoving() && VertexLoc.HasValue)
                {
                    VertexChange(_selectedVertices);
                }

                if (_boneSelection.IsMoving() && SelectedBone != null)
                {
                    if (temp &&
                        CHR0Editor.chkUpdateBindPose.Checked &&
                        TargetAnimType == NW4RAnimType.CHR &&
                        CurrentFrame == 0 &&
                        SelectedBone != null)
                    {
                        SelectedBone.RecalcBindState(true, !CHR0Editor.chkMoveBoneOnly.Checked);
                        UpdateModel(TargetModel, CurrentFrame);
                    }

                    BoneChange(SelectedBone);
                }

                _boneSelection.ResetAll();
                _vertexSelection.ResetAll();
            }
        }

        #endregion

        #region Mouse Move

        private Vector3 GetLocalTransform(
            MouseEventArgs e,
            ModelPanelViewport viewport,
            Matrix localTransform,
            Matrix invLocalTransform,
            Matrix localParentTransform,
            SelectionParams selection,
            out Vector3? worldPoint)
        {
            worldPoint = null;

            if (GetTransformPoint(new Vector2(e.X, e.Y), out Vector3 point, viewport, localTransform, selection))
            {
                Vector3 transform = new Vector3(), lPoint;

                CoordinateType coord = _coordinateTypes[(int) ControlType];
                switch (ControlType)
                {
                    case TransformType.Rotation:

                        lPoint = invLocalTransform * point;

                        if (selection._lastPointLocal == lPoint)
                        {
                            return new Vector3();
                        }

                        //Get matrix with new rotation applied
                        Matrix m = localParentTransform * Matrix.AxisAngleMatrix(selection._lastPointLocal, lPoint);

                        //Derive angles from matrices, get difference
                        transform = (m.GetAngles() - localParentTransform.GetAngles()).RemappedToRange(-180.0f, 180.0f);

                        break;

                    case TransformType.Translation:
                    case TransformType.Scale:

                        if (ControlType == TransformType.Scale &&
                            selection._snapX &&
                            selection._snapY &&
                            selection._snapZ)
                        {
                            //Get scale factor
                            //TODO: calculate with distance between screen points instead
                            transform = new Vector3((point / selection._lastPointWorld)._y);
                            break;
                        }

                        lPoint = invLocalTransform * point;

                        if (selection._lastPointLocal == lPoint)
                        {
                            return new Vector3();
                        }

                        if (!(ControlType == TransformType.Translation && selection._snapCirc))
                        {
                            switch (coord)
                            {
                                case CoordinateType.World:

                                    //Limit world point with snaps
                                    if (!selection._snapX)
                                    {
                                        point._x = selection._lastPointWorld._x;
                                    }

                                    if (!selection._snapY)
                                    {
                                        point._y = selection._lastPointWorld._y;
                                    }

                                    if (!selection._snapZ)
                                    {
                                        point._z = selection._lastPointWorld._z;
                                    }

                                    //Remake local point with edited world point
                                    lPoint = invLocalTransform * point;

                                    break;
                                case CoordinateType.Local:

                                    //Limit local point with snaps
                                    if (!selection._snapX)
                                    {
                                        lPoint._x = selection._lastPointLocal._x;
                                    }

                                    if (!selection._snapY)
                                    {
                                        lPoint._y = selection._lastPointLocal._y;
                                    }

                                    if (!selection._snapZ)
                                    {
                                        lPoint._z = selection._lastPointLocal._z;
                                    }

                                    break;
                                case CoordinateType.Screen:

                                    break;
                            }
                        }

                        if (ControlType == TransformType.Scale)
                        {
                            transform = localParentTransform * lPoint /
                                        (localParentTransform * selection._lastPointLocal);
                        }
                        else
                        {
                            transform = localParentTransform * lPoint -
                                        localParentTransform * selection._lastPointLocal;
                        }

                        break;
                }

                worldPoint = point;
                selection._lastPointWorld = point;

                return transform;
            }

            return new Vector3();
        }

        public delegate void ApplyLocalBoneTransformFunc(int index, float offset);

        public ApplyLocalBoneTransformFunc[] _boneTransform;

        //Transforms are in order T, R, S
        //We have to edit the transform to before rotation has been applied for translation
        public bool _translateAfterRotation;

        public Matrix GetBoneInvWorldMtx()
        {
            if (SelectedBone == null)
            {
                return Matrix.Identity;
            }

            if (!_translateAfterRotation &&
                ControlType == TransformType.Translation &&
                _coordinateTypes[0] == CoordinateType.Local)
            {
                return SelectedBone.FrameState._transform.GetRotationMatrix() * SelectedBone.InverseMatrix;
            }

            return SelectedBone.InverseMatrix;
        }

        public Matrix GetBoneWorldMtx()
        {
            if (SelectedBone == null)
            {
                return Matrix.Identity;
            }

            if (!_translateAfterRotation &&
                ControlType == TransformType.Translation &&
                _coordinateTypes[0] == CoordinateType.Local)
            {
                return SelectedBone.Matrix * SelectedBone.FrameState._iTransform.GetRotationMatrix();
            }

            return SelectedBone.Matrix;
        }

        public Matrix GetBoneParentTransformMtx()
        {
            if (SelectedBone == null)
            {
                return Matrix.Identity;
            }

            if (!_translateAfterRotation &&
                ControlType == TransformType.Translation &&
                _coordinateTypes[0] == CoordinateType.Local)
            {
                return SelectedBone.FrameState._transform * SelectedBone.FrameState._iTransform.GetRotationMatrix();
            }

            return SelectedBone.FrameState._transform;
        }

        private bool _createdNewBone;

        protected virtual void modelPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_playing)
            {
                return;
            }

            ModelPanel panel = sender as ModelPanel;
            ModelPanelViewport viewport = panel.CurrentViewport;

            Vector3? point;
            if (_boneSelection.IsMoving() && SelectedBone != null)
            {
                Vector3 transform = GetLocalTransform(e, viewport,
                    GetBoneWorldMtx(),
                    GetBoneInvWorldMtx(),
                    GetBoneParentTransformMtx(),
                    _boneSelection,
                    out point);

                if (Alt && !_createdNewBone)
                {
                    if (SelectedBone is MDL0BoneNode)
                    {
                        _createdNewBone = true;

                        MDL0BoneNode b = SelectedBone as MDL0BoneNode;
                        MDL0Node model = b.Model;
                        MDL0BoneNode newBone = new MDL0BoneNode();
                        string name = "NewBone";
                        if (model != null)
                        {
                            name += "0";
                            int id = 1;
                            Top:
                            foreach (MDL0BoneNode x in model._linker.BoneCache)
                            {
                                if (x.Name == name)
                                {
                                    name = "NewBone" + id++;
                                    goto Top;
                                }
                            }

                            newBone._entryIndex = model._linker.BoneCache.Length;
                        }

                        newBone.Name = name;
                        newBone.FrameState = newBone.BindState = FrameState.Neutral;

                        b.AddChild(newBone);

                        newBone.RecalcFrameState();
                        newBone.RecalcBindState(false, false, false);

                        model?._linker.RegenerateBoneCache();
                        BonesPanel?.Reset();

                        SelectedBone = newBone;
                    }
                }

                if (point != null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        _boneTransform[(int) ControlType](i, transform[i]);
                    }

                    _boneSelection._lastPointLocal = GetBoneInvWorldMtx() * point.Value;
                }
            }

            if (_vertexSelection.IsMoving() && VertexLoc != null)
            {
                Vector3 center = VertexLoc.Value;
                Vector3 transform = GetLocalTransform(e, viewport,
                    Matrix.TranslationMatrix(center),
                    Matrix.TranslationMatrix(-center),
                    Matrix.Identity,
                    _vertexSelection,
                    out point);

                if (point != null)
                {
                    switch (ControlType)
                    {
                        case TransformType.Scale:
                            foreach (Vertex3 vertex in _selectedVertices)
                            {
                                vertex.WeightedPosition =
                                    Maths.ScaleAboutPoint(vertex.WeightedPosition, center, transform);
                            }

                            break;

                        case TransformType.Rotation:
                            foreach (Vertex3 vertex in _selectedVertices)
                            {
                                vertex.WeightedPosition =
                                    Maths.RotateAboutPoint(vertex.WeightedPosition, center, transform);
                            }

                            break;

                        case TransformType.Translation:
                            foreach (Vertex3 vertex in _selectedVertices)
                            {
                                vertex.WeightedPosition += transform;
                            }

                            break;
                    }

                    _vertexLoc = null;
                    _vertexSelection._lastPointLocal = Matrix.TranslationMatrix(-VertexLoc.Value) * point.Value;
                }

                UpdateModel();
            }

            bool allowHighlight = !DoNotHighlightOnMouseMove;
            bool draggingSelection = viewport.Selecting;

            //if not dragging a point AND (highlighting is allowed, or not but selecting)
            if (!_boneSelection.IsMoving() &&
                !_vertexSelection.IsMoving() &&
                (allowHighlight || !allowHighlight && draggingSelection))
            {
                HighlightStuff(e, panel);
            }
        }

        #endregion

        #region Highlighting

        #region Targeting

        private delegate void
            MouseMoveTargetType(ModelPanel panel, MouseEventArgs e, float depth, ModelPanelViewport v);

        private MouseMoveTargetType[] _mouseMoveTargetType;

        private void MouseMoveTargetBone(
            ModelPanel panel,
            MouseEventArgs e,
            float depth,
            ModelPanelViewport v)
        {
            if (SelectedBone != null)
            {
                MouseMoveTarget(
                    panel, e, depth, v,
                    GetBoneWorldMtx(),
                    GetBoneInvWorldMtx(),
                    _boneSelection);
            }

            GetBone(panel, e, depth, v);
        }

        private void MouseMoveTargetVertex(
            ModelPanel panel,
            MouseEventArgs e,
            float depth,
            ModelPanelViewport v)
        {
            if (VertexLoc.HasValue /* && v._renderAttrib._renderVertices*/)
            {
                Vector3 center = VertexLoc.Value;
                MouseMoveTarget(
                    panel, e, depth, v,
                    Matrix.TranslationMatrix(center),
                    Matrix.TranslationMatrix(-center),
                    _vertexSelection);
            }

            GetVertex(panel, e, depth, v);
        }

        private void MouseMoveTarget(
            ModelPanel panel,
            MouseEventArgs e,
            float depth,
            ModelPanelViewport v,
            Matrix transformMatrix,
            Matrix invTransformMatrix,
            SelectionParams selection)
        {
            if (ControlType == TransformType.None)
            {
                return;
            }

            CoordinateType coord = _coordinateTypes[(int) ControlType];

            //Get the location of the bone
            Vector3 center = transformMatrix.GetPoint();

            //Standard radius scaling snippet. This is used for orb scaling depending on camera distance.
            float radius = CamDistance(center, v);

            bool snapFound = false;
            if (ControlType == TransformType.Rotation)
            {
                //Get point projected onto our orb.
                Vector3 point = v.ProjectCameraSphere(new Vector2(e.X, e.Y), center, radius, false);

                //Get the distance of the mouse point from the bone
                float distance = point.TrueDistance(center);

                if (Math.Abs(distance - radius) < radius * _selectOrbScale) //Point lies within orb radius
                {
                    selection._hiSphere = true;

                    //Determine axis snapping
                    Vector3 angles = (invTransformMatrix * point).GetAngles() * Maths._rad2degf;
                    angles._x = Math.Abs(angles._x);
                    angles._y = Math.Abs(angles._y);
                    angles._z = Math.Abs(angles._z);

                    if (Math.Abs(angles._y - 90.0f) <= _axisSnapRange)
                    {
                        selection._hiX = true;
                    }
                    else if (angles._x >= 180.0f - _axisSnapRange || angles._x <= _axisSnapRange)
                    {
                        selection._hiY = true;
                    }
                    else if (angles._y >= 180.0f - _axisSnapRange || angles._y <= _axisSnapRange)
                    {
                        selection._hiZ = true;
                    }
                }
                else if (Math.Abs(distance - radius * _circOrbScale) < radius * _selectOrbScale
                ) //Point lies on circ line
                {
                    selection._hiCirc = true;
                }

                if (selection._hiX || selection._hiY || selection._hiZ || selection._hiCirc)
                {
                    snapFound = true;
                }
            }
            else if (ControlType == TransformType.Translation || ControlType == TransformType.Scale)
            {
                //No more need to use depth!!! Just some nice math

                if (_coordinateTypes[(int) ControlType] == CoordinateType.World)
                {
                    transformMatrix = Matrix.TranslationMatrix(center) * Matrix.ScaleMatrix(transformMatrix.GetScale());
                }

                Vector3[] planePoints = new Vector3[3]; //xy, yz, xz
                v.ProjectCameraPlanes(new Vector2(e.X, e.Y), transformMatrix,
                    out planePoints[0], out planePoints[1], out planePoints[2]);

                List<Vector3> testDiffs = new List<Vector3>();
                foreach (Vector3 planePoint in planePoints)
                {
                    Vector3 d =
                        coord == CoordinateType.World ? (planePoint - center) / radius :
                        coord == CoordinateType.Local ? invTransformMatrix * planePoint / radius :
                        (panel.Camera._matrix * planePoint - panel.Camera._matrix * center) / radius;

                    if (d._x > -_axisSelectRange && d._x < _axisLDist + 0.01f &&
                        d._y > -_axisSelectRange && d._y < _axisLDist + 0.01f &&
                        d._z > -_axisSelectRange && d._z < _axisLDist + 0.01f)
                    {
                        testDiffs.Add(d);
                    }
                }

                //Check if point lies on a specific axis
                foreach (Vector3 diff in testDiffs)
                {
                    float errorRange = _axisSelectRange;

                    if (diff._x > _axisHalfLDist &&
                        Math.Abs(diff._y) < errorRange &&
                        Math.Abs(diff._z) < errorRange)
                    {
                        selection._hiX = true;
                    }

                    if (diff._y > _axisHalfLDist &&
                        Math.Abs(diff._x) < errorRange &&
                        Math.Abs(diff._z) < errorRange)
                    {
                        selection._hiY = true;
                    }

                    if (diff._z > _axisHalfLDist &&
                        Math.Abs(diff._x) < errorRange &&
                        Math.Abs(diff._y) < errorRange)
                    {
                        selection._hiZ = true;
                    }

                    if (snapFound = selection._hiX || selection._hiY || selection._hiZ)
                    {
                        break;
                    }
                }

                if (!snapFound)
                {
                    foreach (Vector3 diff in testDiffs)
                    {
                        if (ControlType == TransformType.Translation)
                        {
                            if (diff._x < _axisHalfLDist &&
                                diff._y < _axisHalfLDist &&
                                diff._z < _axisHalfLDist)
                            {
                                //Point lies inside the double drag areas
                                if (diff._x > _axisSelectRange)
                                {
                                    selection._hiX = true;
                                }

                                if (diff._y > _axisSelectRange)
                                {
                                    selection._hiY = true;
                                }

                                if (diff._z > _axisSelectRange)
                                {
                                    selection._hiZ = true;
                                }

                                selection._hiCirc =
                                    !selection._hiX &&
                                    !selection._hiY &&
                                    !selection._hiZ;

                                snapFound = true;
                            }
                        }
                        else if (ControlType == TransformType.Scale)
                        {
                            //Determine if the point is in the double or triple drag triangles
                            float halfDist = _scaleHalf2LDist;
                            float centerDist = _scaleHalf1LDist;
                            if (diff.IsInTriangle(new Vector3(), new Vector3(halfDist, 0, 0),
                                new Vector3(0, halfDist, 0)))
                            {
                                if (diff.IsInTriangle(new Vector3(), new Vector3(centerDist, 0, 0),
                                    new Vector3(0, centerDist, 0)))
                                {
                                    selection._hiX = selection._hiY = selection._hiZ = true;
                                }
                                else
                                {
                                    selection._hiX = selection._hiY = true;
                                }
                            }
                            else if (diff.IsInTriangle(new Vector3(), new Vector3(halfDist, 0, 0),
                                new Vector3(0, 0, halfDist)))
                            {
                                if (diff.IsInTriangle(new Vector3(), new Vector3(centerDist, 0, 0),
                                    new Vector3(0, 0, centerDist)))
                                {
                                    selection._hiX = selection._hiY = selection._hiZ = true;
                                }
                                else
                                {
                                    selection._hiX = selection._hiZ = true;
                                }
                            }
                            else if (diff.IsInTriangle(new Vector3(), new Vector3(0, halfDist, 0),
                                new Vector3(0, 0, halfDist)))
                            {
                                if (diff.IsInTriangle(new Vector3(), new Vector3(0, centerDist, 0),
                                    new Vector3(0, 0, centerDist)))
                                {
                                    selection._hiX = selection._hiY = selection._hiZ = true;
                                }
                                else
                                {
                                    selection._hiY = selection._hiZ = true;
                                }
                            }

                            snapFound = selection._hiX || selection._hiY || selection._hiZ;
                        }

                        if (snapFound)
                        {
                            break;
                        }
                    }
                }
            }

            if (snapFound)
            {
                panel.Cursor = Cursors.Hand;
                panel.Invalidate();
            }
        }

        #region Get Highlighted Point

        private void GetBone(ModelPanel panel, MouseEventArgs e, float depth, ModelPanelViewport v)
        {
            if (!_boneSelection.IsMoving() && depth < 1.0f)
            {
                IBoneNode o = null;

                Vector3 point = v.UnProject(e.X, e.Y, depth);
                bool doScale = v._renderAttrib._scaleBones;

                //Find orb near chosen point
                if (EditingAll)
                {
                    foreach (IModel m in _targetModels)
                    {
                        foreach (IBoneNode b in m.RootBones)
                        {
                            if (CompareBoneDistanceRecursive(b, point, ref o, v, doScale))
                            {
                                break;
                            }
                        }
                    }
                }
                else if (_targetModel != null)
                {
                    foreach (IBoneNode b in _targetModel.RootBones)
                    {
                        if (CompareBoneDistanceRecursive(b, point, ref o, v, doScale))
                        {
                            break;
                        }
                    }
                }

                bool update = false;
                if (_hiBone != null && _hiBone != SelectedBone && (update = _hiBone.NodeColor != Color.Transparent))
                {
                    _hiBone.NodeColor = Color.Transparent;
                }

                if ((_hiBone = o) != null)
                {
                    _hiBone.NodeColor = Color.FromArgb(255, 128, 0);
                    panel.Cursor = Cursors.Hand;
                    update = true;
                }

                if (update)
                {
                    panel.Invalidate();
                }
            }
            else if (_hiBone != null)
            {
                if (_hiBone != SelectedBone)
                {
                    _hiBone.NodeColor = Color.Transparent;
                    panel.Invalidate();
                }

                _hiBone = null;
            }
        }

        private void GetVertex(ModelPanel panel, MouseEventArgs e, float depth, ModelPanelViewport v)
        {
            //Try targeting a vertex
            if (RenderVertices)
            {
                if (panel.CurrentViewport.Selecting)
                {
                    if (NotCtrlAlt)
                    {
                        ClearSelectedVertices();
                    }

                    bool selected = false;
                    if (TargetModel != null)
                    {
                        if (TargetModel.SelectedObjectIndex < 0)
                        {
                            foreach (IObject o in TargetModel.Objects)
                            {
                                if (o.IsRendering)
                                {
                                    SelectVertices(o, panel);
                                    selected = true;
                                }
                            }
                        }
                        else
                        {
                            IObject w = TargetModel.Objects[TargetModel.SelectedObjectIndex];
                            if (w.IsRendering)
                            {
                                SelectVertices(w, panel);
                                selected = true;
                            }
                            else
                            {
                                foreach (IObject h in TargetModel.Objects)
                                {
                                    if (h.IsRendering)
                                    {
                                        SelectVertices(h, panel);
                                        selected = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (_targetModels != null)
                    {
                        foreach (IModel m in _targetModels)
                        {
                            if (m.SelectedObjectIndex < 0)
                            {
                                foreach (IObject o in m.Objects)
                                {
                                    if (o.IsRendering)
                                    {
                                        SelectVertices(o, panel);
                                        selected = true;
                                    }
                                }
                            }
                            else
                            {
                                IObject w = m.Objects[m.SelectedObjectIndex];
                                if (w.IsRendering)
                                {
                                    SelectVertices(w, panel);
                                    selected = true;
                                }
                                else
                                {
                                    foreach (IObject h in m.Objects)
                                    {
                                        if (h.IsRendering)
                                        {
                                            SelectVertices(h, panel);
                                            selected = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (selected)
                    {
                        OnSelectedVerticesChanged();
                    }
                }
                else
                {
                    if (depth < 1.0f && _targetModel != null)
                    {
                        Vector3 point = v.UnProject(e.X, e.Y, depth);
                        Vertex3 vertex = CompareVertexDistance(point);
                        bool update = false;
                        if (_hiVertex != null && !_hiVertex.Selected)
                        {
                            update = true;
                            _hiVertex.HighlightColor = Color.Transparent;
                            ModelPanel.CurrentViewport.AllowSelection = true;
                        }

                        if ((_hiVertex = vertex) != null)
                        {
                            update = true;
                            _hiVertex.HighlightColor = Color.Orange;
                            panel.Cursor = Cursors.Cross;
                            ModelPanel.CurrentViewport.AllowSelection = false;
                        }

                        if (update)
                        {
                            panel.Invalidate();
                        }
                    }
                    else if (_hiVertex != null)
                    {
                        if (!_hiVertex.Selected)
                        {
                            _hiVertex.HighlightColor = Color.Transparent;
                            ModelPanel.CurrentViewport.AllowSelection = true;
                            panel.Invalidate();
                        }

                        _hiVertex = null;
                    }
                }
            }
        }

        #endregion

        #endregion

        public IBoneNode _hiBone;
        public Vertex3 _hiVertex;

        public void HighlightStuff(MouseEventArgs e, ModelPanel panel)
        {
            panel.Capture();

            _boneSelection.ResetHighlights();
            _vertexSelection.ResetHighlights();

            float depth = panel.GetDepth(e.X, e.Y);
            ModelPanelViewport v = panel.HighlightedViewport;

            foreach (MouseMoveTargetType targetFunc in _mouseMoveTargetType)
            {
                targetFunc(panel, e, depth, v);
            }

#if DEBUG
            if (_renderDepth)
            {
                v.SettingsScreenText["Depth: " + depth.ToString()] = new Vector3(5.0f, v.Height - 20.0f, 0.5f);
                panel.Invalidate();
            }
#endif
        }

        /// <summary>
        /// Does not call OnSelectedVerticesChanged()!
        /// </summary>
        private void SelectVertices(IObject o, ModelPanel panel)
        {
            foreach (Vertex3 v in o.Vertices)
            {
                //Project each vertex into screen coordinates.
                //Then check to see if the 2D coordinates lie within the selection box.
                //In Soviet Russia, vertices come to YOUUUUU
                Vector3 screenTemp = panel.CurrentViewport.Camera.Project(v.WeightedPosition);
                Vector2 screenPos = (Vector2) screenTemp;

                //This is the absolute depth value, regardless of obstructions
                float vertexDepth = screenTemp._z;

                Point start = panel.CurrentViewport.SelectionStart, end = panel.CurrentViewport.SelectionEnd;
                Vector2 min = new Vector2(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
                Vector2 max = new Vector2(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));
                if (screenPos <= max && screenPos >= min)
                {
                    //Test visible depth, this time taking obstructions into consideration
                    //The tested visible depth must be very close to the actual depth
                    //if (Math.Abs(panel.GetDepth(screenPos) - vertexDepth) < 0.001f)
                    {
                        if (Alt)
                        {
                            SelectVertex(v, false);
                        }
                        else if (!v.Selected)
                        {
                            SelectVertex(v, true);
                        }
                    }
                }
            }
        }

        public void ResetBoneColors()
        {
            if (_targetModels != null)
            {
                foreach (IModel m in _targetModels)
                {
                    foreach (IBoneNode b in m.BoneCache)
                    {
                        b.BoneColor = b.NodeColor = Color.Transparent;
                    }
                }
            }
        }

        public void SelectVertex(Vertex3 v, bool selected)
        {
            if (v.Selected = selected)
            {
                if (!_selectedVertices.Contains(v))
                {
                    _selectedVertices.Add(v);
                }
            }
            else
            {
                if (_selectedVertices.Contains(v))
                {
                    _selectedVertices.Remove(v);
                }
            }
        }

        public void ClearSelectedVertices()
        {
            if (_targetModels != null)
            {
                foreach (IModel m in _targetModels)
                {
                    foreach (IObject o in m.Objects)
                    {
                        if (o.Vertices != null)
                        {
                            foreach (Vertex3 v in o.Vertices)
                            {
                                v.Selected = false;
                            }
                        }
                    }
                }
            }

            _selectedVertices = new List<Vertex3>();
            OnSelectedVerticesChanged();
        }

        public void SelectAllVertices(IModel mdl)
        {
            if (mdl.SelectedObjectIndex >= 0 && mdl.SelectedObjectIndex < mdl.Objects.Length)
            {
                IObject o = mdl.Objects[mdl.SelectedObjectIndex];
                if (o.IsRendering)
                {
                    foreach (Vertex3 v in o.Vertices)
                    {
                        SelectVertex(v, true);
                    }
                }
            }
            else
            {
                foreach (IObject o in mdl.Objects)
                {
                    if (o.IsRendering)
                    {
                        foreach (Vertex3 v in o.Vertices)
                        {
                            SelectVertex(v, true);
                        }
                    }
                }
            }

            OnSelectedVerticesChanged();
        }

        public void SetSelectedVertices(List<Vertex3> list)
        {
            ClearSelectedVertices();
            _selectedVertices = list;
            foreach (Vertex3 v in _selectedVertices)
            {
                v.Selected = true;
            }

            OnSelectedVerticesChanged();
        }

        private void ResetHighlights()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
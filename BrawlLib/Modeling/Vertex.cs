using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;

namespace BrawlLib.Modeling
{
    public class Vertex3 : IMatrixNodeUser
    {
        internal float _baseWeight = 0;
        internal Vector3 _bCenter = new Vector3();

        public List<int> _faceDataIndices = new List<int>();

        //Contains all the facepoints with the same position and influence.
        //Note that the normal, etc indices may differ per facepoint
        public List<Facepoint> _facepoints = new List<Facepoint>();

        public Color _highlightColor = Color.Transparent;
        internal IMatrixNode _matrixNode;
        internal MDL0VertexNode[] _nodes;
        public Vector3 _position;
        public bool _selected;

        private bool _updateAssets = true;
        public Vector3 _weightedPosition;
        internal float[] _weights;

        public Vertex3()
        {
        }

        public Vertex3(Vector3 position)
        {
            Position = position;
        }

        public Vertex3(Vector3 position, IMatrixNode influence)
        {
            Position = position;
            MatrixNode = influence;
        }

        //normals, colors and uvs aren't stored in this class
        //because this stores a unique weighted point in space.
        //Multiple vertices may have different normals etc but the same weighted position

        [Browsable(false)] public IMatrixNodeUser Parent { get; set; }

        public Facepoint[] Facepoints => _facepoints.ToArray();

        [Browsable(true)]
        public string Influence => MatrixNode == null ? "(none)" :
            MatrixNode.IsPrimaryNode ? ((ResourceNode) MatrixNode).Name : "(multiple)";

        [Browsable(true)]
        [Category("Vertex")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 WeightedPosition
        {
            get => _weightedPosition;
            set
            {
                _weightedPosition = value;
                Unweight();
            }
        }

        [Browsable(false)]
        [Category("Vertex")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        [Browsable(false)]
        public bool Selected
        {
            get => _selected;
            set => _highlightColor = _selected = value ? Color.Orange : Color.Transparent;
        }

        public void DeferUpdateAssets()
        {
            _updateAssets = false;
        }

        [Browsable(false)]
        public IMatrixNode MatrixNode
        {
            get => _matrixNode;
            set
            {
                if (_matrixNode == value) return;

                ChangeInfluence(value);

                if (_matrixNode != null && _matrixNode.Users.Contains(this)) _matrixNode.Users.Remove(this);

                if ((_matrixNode = value) != null && !_matrixNode.Users.Contains(this)) _matrixNode.Users.Add(this);
            }
        }

        public IMatrixNode GetMatrixNode()
        {
            if (Parent != null && Parent.MatrixNode != null)
                return Parent.MatrixNode;
            if (MatrixNode != null) return MatrixNode;

            return null;
        }

        public Matrix GetMatrix()
        {
            var node = GetMatrixNode();
            if (node != null) return node.Matrix;

            return Matrix.Identity;
        }

        public Matrix GetInvMatrix()
        {
            var node = GetMatrixNode();
            if (node != null) return node.InverseMatrix;

            return Matrix.Identity;
        }

        public List<BoneWeight> GetBoneWeights()
        {
            return MatrixNode == null ? Parent.MatrixNode.Weights : MatrixNode.Weights;
        }

        public IBoneNode[] GetBones()
        {
            var b = GetBoneWeights();
            return b == null ? null : b.Select(x => x.Bone).ToArray();
        }

        public float[] GetWeightValues()
        {
            var b = GetBoneWeights();
            return b == null ? null : b.Select(x => x.Weight).ToArray();
        }

        public int IndexOfBone(IBoneNode b)
        {
            return Array.IndexOf(GetBones(), b);
        }

        public BoneWeight WeightForBone(IBoneNode b)
        {
            var i = IndexOfBone(b);
            if (i == -1) return null;
            return GetBoneWeights()[i];
        }

        public void ChangeInfluence(IMatrixNode newMatrixNode)
        {
            if (Parent == null) return;

            var oldMatrixNode = GetMatrixNode();
            if (newMatrixNode is IBoneNode && (oldMatrixNode is Influence || oldMatrixNode == null))
            {
                //Move to local
                var inv = ((IBoneNode) newMatrixNode).InverseBindMatrix;

                _position *= inv;
                UpdateNormals(Matrix.Identity, inv);

                if (_updateAssets)
                {
                    SetPosition();
                    SetNormals();
                }
            }
            else if ((newMatrixNode is Influence || newMatrixNode == null) && oldMatrixNode is IBoneNode)
            {
                //Move to world
                var m = ((IBoneNode) oldMatrixNode).BindMatrix;

                _position *= m;
                UpdateNormals(m, Matrix.Identity);

                if (_updateAssets)
                {
                    SetPosition();
                    SetNormals();
                }
            }
            else if (newMatrixNode is IBoneNode && oldMatrixNode is IBoneNode)
            {
                //Update local position
                var m = ((IBoneNode) oldMatrixNode).BindMatrix;
                var inv = ((IBoneNode) newMatrixNode).InverseBindMatrix;

                _position *= m;
                _position *= inv;
                UpdateNormals(m, inv);

                if (_updateAssets)
                {
                    SetPosition();
                    SetNormals();
                }
            }

            _updateAssets = true;

            //If both are null or an influence, they're already in world position
        }

        private void SetNormals()
        {
            var obj = Parent as MDL0ObjectNode;
            if (obj != null) obj.SetEditedNormals();
        }

        private unsafe void UpdateNormals(Matrix m, Matrix inv)
        {
            var obj = Parent as MDL0ObjectNode;
            if (obj != null && obj._manager._faceData[1] != null)
            {
                var pData = (Vector3*) obj._manager._faceData[1].Address;
                for (var i = 0; i < _faceDataIndices.Count; i++)
                {
                    var n = pData[_faceDataIndices[i]];
                    n *= m.GetRotationMatrix();
                    n *= inv.GetRotationMatrix();
                    pData[_faceDataIndices[i]] = n;
                }
            }
        }

        //Pre-multiply vertex using influence.
        //Influences must have already been calculated.
        public void Weight()
        {
            _weightedPosition = GetMatrix() * Position;

            _weights = null;
            _nodes = null;
        }

        //Moves an edited world position back into the stored position
        public void Unweight(bool updateVertexSets = true)
        {
            //Unweight overall position
            _position = GetInvMatrix() * WeightedPosition;

            //Distribute weights for the position across all vertex set influences
            if (updateVertexSets)
            {
                if (_weights != null && _nodes != null)
                {
                    var trans = _weightedPosition - _bCenter;
                    for (var i = 0; i < _nodes.Length; i++)
                    {
                        var set = _nodes[i];
                        SetPosition(set,
                            GetInvMatrix() * (GetMatrix() * set.Vertices[_facepoints[0]._vertexIndex] + trans));
                    }

                    var obj = (MDL0ObjectNode) Parent;
                    SetPosition(obj._vertexNode,
                        GetInvMatrix() * (GetMatrix() * obj._vertexNode.Vertices[_facepoints[0]._vertexIndex] + trans));
                }
                else
                {
                    SetPosition();
                }
            }
        }

        public void SetPosition(MDL0VertexNode node, Vector3 pos)
        {
            //if (node == null)
            //    return;

            //node.Vertices[_facepoints[0]._vertexIndex] = pos;
            //node.ForceRebuild = true;
            //if (node.Format == WiiVertexComponentType.Float)
            //    node.ForceFloat = true;

            //Have to use this function instead of setting the vertices directly
            //This is because the vertex set may be used by other objects
            var obj = Parent as MDL0ObjectNode;
            if (obj != null) obj.SetEditedVertices();
        }

        public void SetPosition()
        {
            var node = ((MDL0ObjectNode) Parent)._vertexNode;

            if (node == null) return;

            node.Vertices[_facepoints[0]._vertexIndex] = Position;
            node.ForceRebuild = true;
            if (node.Format == WiiVertexComponentType.Float) node.ForceFloat = true;
        }

        //Call only after weighting
        public void Morph(Vector3 dest, float percent)
        {
            _weightedPosition.Lerp(dest, percent);
        }

        public Color GetWeightColor(IBoneNode targetBone)
        {
            var weight = -1.0f;
            if (_matrixNode == null || targetBone == null) return Color.Transparent;

            if (_matrixNode is MDL0BoneNode)
            {
                if (_matrixNode == targetBone)
                    weight = 1.0f;
                else
                    return Color.Transparent;
            }
            else
            {
                foreach (var b in ((Influence) _matrixNode).Weights)
                    if (b.Bone == targetBone)
                    {
                        weight = b.Weight;
                        break;
                    }
            }

            if (weight < 0.0f || weight > 1.0f) return Color.Transparent;

            var r = ((int) (weight * 255.0f)).Clamp(0, 0xFF);
            return Color.FromArgb(r, 0, 0xFF - r);
        }

        public bool Equals(Vertex3 v)
        {
            if (ReferenceEquals(this, v)) return true;

            return Position == v.Position && _matrixNode == v._matrixNode;
        }
    }
}
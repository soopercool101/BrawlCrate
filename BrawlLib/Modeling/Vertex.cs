using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace BrawlLib.Modeling
{
    public class Vertex3 : IMatrixNodeUser
    {
        public Vector3 _position;
        public Vector3 _weightedPosition;
        internal IMatrixNode _matrixNode;

        //normals, colors and uvs aren't stored in this class
        //because this stores a unique weighted point in space.
        //Multiple vertices may have different normals etc but the same weighted position

        [Browsable(false)]
        public IMatrixNodeUser Parent
        {
            get => _parent;
            set => _parent = value;
        }

        private IMatrixNodeUser _parent;

        public List<int> FaceDataIndices = new List<int>();

        //Contains all the facepoints with the same position and influence.
        //Note that the normal, etc indices may differ per facepoint
        public List<Facepoint> Facepoints = new List<Facepoint>();

        public IMatrixNode GetMatrixNode()
        {
            if (_parent?.MatrixNode != null)
            {
                return _parent.MatrixNode;
            }

            return MatrixNode;
        }

        public Matrix GetMatrix()
        {
            IMatrixNode node = GetMatrixNode();
            if (node != null)
            {
                return node.Matrix;
            }

            return Matrix.Identity;
        }

        public Matrix GetInvMatrix()
        {
            IMatrixNode node = GetMatrixNode();
            if (node != null)
            {
                return node.InverseMatrix;
            }

            return Matrix.Identity;
        }

        public List<BoneWeight> GetBoneWeights()
        {
            return MatrixNode == null ? _parent.MatrixNode.Weights : MatrixNode.Weights;
        }

        public IBoneNode[] GetBones()
        {
            List<BoneWeight> b = GetBoneWeights();
            return b?.Select(x => x.Bone).ToArray();
        }

        public float[] GetWeightValues()
        {
            List<BoneWeight> b = GetBoneWeights();
            return b?.Select(x => x.Weight).ToArray();
        }

        public int IndexOfBone(IBoneNode b)
        {
            return Array.IndexOf(GetBones(), b);
        }

        public BoneWeight WeightForBone(IBoneNode b)
        {
            int i = IndexOfBone(b);
            if (i == -1)
            {
                return null;
            }

            return GetBoneWeights()[i];
        }

        [Browsable(true)]
        public string Influence => MatrixNode == null ? "(none)" :
            MatrixNode.IsPrimaryNode ? ((ResourceNode) MatrixNode).Name : "(multiple)";

        private bool _updateAssets = true;

        public void ChangeInfluence(IMatrixNode newMatrixNode)
        {
            if (_parent == null)
            {
                return;
            }

            IMatrixNode oldMatrixNode = GetMatrixNode();
            if (newMatrixNode is IBoneNode && (oldMatrixNode is Influence || oldMatrixNode == null))
            {
                //Move to local
                Matrix inv = ((IBoneNode) newMatrixNode).InverseBindMatrix;

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
                Matrix m = ((IBoneNode) oldMatrixNode).BindMatrix;

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
                Matrix m = ((IBoneNode) oldMatrixNode).BindMatrix;
                Matrix inv = ((IBoneNode) newMatrixNode).InverseBindMatrix;

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
            MDL0ObjectNode obj = _parent as MDL0ObjectNode;
            obj?.SetEditedNormals();
        }

        private unsafe void UpdateNormals(Matrix m, Matrix inv)
        {
            MDL0ObjectNode obj = _parent as MDL0ObjectNode;
            if (obj?._manager._faceData[1] != null)
            {
                Vector3* pData = (Vector3*) obj._manager._faceData[1].Address;
                for (int i = 0; i < FaceDataIndices.Count; i++)
                {
                    Vector3 n = pData[i];
                    n *= m.GetRotationMatrix();
                    n *= inv.GetRotationMatrix();
                    pData[i] = n;
                }
            }
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
                if (_matrixNode == value)
                {
                    return;
                }

                ChangeInfluence(value);

                if (_matrixNode != null && _matrixNode.Users.Contains(this))
                {
                    _matrixNode.Users.Remove(this);
                }

                if ((_matrixNode = value) != null && !_matrixNode.Users.Contains(this))
                {
                    _matrixNode.Users.Add(this);
                }
            }
        }

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
                    Vector3 trans = _weightedPosition - _bCenter;
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        MDL0VertexNode set = _nodes[i];
                        SetPosition(set,
                            GetInvMatrix() * (GetMatrix() * set.Vertices[Facepoints[0]._vertexIndex] + trans));
                    }

                    MDL0ObjectNode obj = (MDL0ObjectNode) _parent;
                    SetPosition(obj._vertexNode,
                        GetInvMatrix() * (GetMatrix() * obj._vertexNode.Vertices[Facepoints[0]._vertexIndex] + trans));
                }
                else
                {
                    SetPosition();
                }
            }
        }

        internal float _baseWeight = 0;
        internal float[] _weights;
        internal MDL0VertexNode[] _nodes;
        internal Vector3 _bCenter = new Vector3();

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
            MDL0ObjectNode obj = _parent as MDL0ObjectNode;
            obj?.SetEditedVertices();
        }

        public void SetPosition()
        {
            MDL0VertexNode node = ((MDL0ObjectNode) _parent)?._vertexNode;

            if (node == null || Facepoints.Count < 1 || node.Vertices.Length <= Facepoints[0]._vertexIndex)
            {
                return;
            }

            node.Vertices[Facepoints[0]._vertexIndex] = Position;
            node.ForceRebuild = true;
            if (node.Format == WiiVertexComponentType.Float)
            {
                node.ForceFloat = true;
            }
        }

        //Call only after weighting
        public void Morph(Vector3 dest, float percent)
        {
            _weightedPosition.Lerp(dest, percent);
        }

        public Color GetWeightColor(IBoneNode targetBone)
        {
            float weight = -1.0f;
            if (_matrixNode == null || targetBone == null)
            {
                return Color.Transparent;
            }

            if (_matrixNode is MDL0BoneNode mNode)
            {
                if (mNode == targetBone)
                {
                    weight = 1.0f;
                }
                else
                {
                    return Color.Transparent;
                }
            }
            else
            {
                foreach (BoneWeight b in ((Influence) _matrixNode).Weights)
                {
                    if (b.Bone == targetBone)
                    {
                        weight = b.Weight;
                        break;
                    }
                }
            }

            if (weight < 0.0f || weight > 1.0f)
            {
                return Color.Transparent;
            }

            int r = ((int) (weight * 255.0f)).Clamp(0, 0xFF);
            return Color.FromArgb(r, 0, 0xFF - r);
        }

        public bool Equals(Vertex3 v)
        {
            if (ReferenceEquals(this, v))
            {
                return true;
            }

            return Position == v.Position && _matrixNode == v._matrixNode;
        }

        public Color HighlightColor = Color.Transparent;
        private bool _selected;

        [Browsable(false)]
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                HighlightColor = _selected ? Color.Orange : Color.Transparent;
            }
        }
    }
}
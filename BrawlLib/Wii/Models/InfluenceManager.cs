using BrawlLib.Internal;
using BrawlLib.Modeling;
using BrawlLib.Modeling.Collada;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.Wii.Models
{
    /// <summary>
    /// Managed collection of influences. Only influences with references should be used.
    /// It is up to the implementation to properly manage this collection.
    /// </summary>
    public class InfluenceManager
    {
        internal List<Influence> _influences = new List<Influence>();
        public List<Influence> Influences => _influences;

        public Influence FindOrCreate(Influence inf)
        {
            //Search for influence in list. If it exists, return it.
            foreach (Influence i in _influences)
            {
                if (i.Equals(inf))
                {
                    return i;
                }
            }

            //Not found, add it to the list.
            _influences.Add(inf);

            return inf;
        }

        public int Count => _influences.Count;

        public void Remove(Influence inf, IMatrixNodeUser user)
        {
            for (int i = 0; i < Count; i++)
            {
                if (ReferenceEquals(_influences[i], inf) && inf.Users.Contains(user))
                {
                    inf.Users.Remove(user);
                    if (inf.Users.Count <= 0)
                    {
                        _influences.RemoveAt(i);
                    }

                    break;
                }
            }
        }

        //Get all weighted influences
        public Influence[] GetWeighted()
        {
            List<Influence> list = new List<Influence>(_influences.Count);
            foreach (Influence i in _influences)
            {
                if (i.IsWeighted)
                {
                    list.Add(i);
                }
            }

            return list.ToArray();
        }

        //Remove all influences without references
        public void Clean()
        {
            int i = 0;
            while (i < Count)
            {
                if (_influences[i].Users.Count <= 0)
                {
                    _influences.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        //Sorts influences
        public void Sort()
        {
            _influences.Sort(Influence.Compare);
        }
    }

    public class Influence : IMatrixNode
    {
        public override string ToString()
        {
            return "";
        }

        internal List<IMatrixNodeUser> _references = new List<IMatrixNodeUser>();
        internal int _index;
        internal Matrix _matrix;
        internal Matrix? _invMatrix;
        private List<BoneWeight> _weights;

        /// <summary>
        /// Don't modify this array!
        /// </summary>
        public List<BoneWeight> Weights => _weights;

        public List<IMatrixNodeUser> Users
        {
            get => _references;
            set => _references = value;
        }

        public void AddWeight(BoneWeight weight)
        {
            _weights.Add(weight);
            if (weight.Bone != null && !weight.Bone.LinkedInfluences.Contains(this))
            {
                weight.Bone.LinkedInfluences.Add(this);
            }
        }

        public void RemoveWeight(BoneWeight weight)
        {
            if (_weights.Contains(weight))
            {
                _weights.Remove(weight);
                if (weight.Bone != null && weight.Bone.LinkedInfluences.Contains(this))
                {
                    weight.Bone.LinkedInfluences.Remove(this);
                }
            }
        }

        public void SetWeights(List<BoneWeight> newWeights)
        {
            foreach (BoneWeight b in _weights)
            {
                if (b.Bone != null && b.Bone.LinkedInfluences.Contains(this))
                {
                    b.Bone.LinkedInfluences.Remove(this);
                }
            }

            _weights = newWeights;
            foreach (BoneWeight b in _weights)
            {
                if (b.Bone != null && !b.Bone.LinkedInfluences.Contains(this))
                {
                    b.Bone.LinkedInfluences.Add(this);
                }
            }
        }

        //Makes sure all weights add up to 1.0f.
        //Does not modify any locked weights.
        public void Normalize()
        {
            //Denominator and numerator to convert each unlocked weight with
            float denom = 0.0f, num = 1.0f;

            foreach (BoneWeight b in Weights)
            {
                if (b.Locked)
                {
                    num -= b.Weight;
                }
                else
                {
                    denom += b.Weight;
                }
            }

            //Don't do anything if all weights are locked
            if (denom != 0.0f && num != 0.0f)
            {
                foreach (BoneWeight b in Weights)
                {
                    if (!b.Locked) //Only normalize unlocked weights used in the calculation
                    {
                        b.Weight = (float) Math.Round(b.Weight / denom * num, 7);
                    }
                }
            }
        }

        public Influence Clone()
        {
            Influence i = new Influence();
            foreach (BoneWeight b in _weights)
            {
                i.AddWeight(new BoneWeight(b.Bone, b.Weight) {Locked = b.Locked});
            }

            return i;
        }

        [Browsable(false)] public int NodeIndex => _index;
        [Browsable(false)] public Matrix Matrix => _matrix;

        [Browsable(false)]
        public Matrix InverseMatrix
        {
            get
            {
                if (_invMatrix == null)
                {
                    try
                    {
                        _invMatrix = Matrix.Invert(_matrix);
                    }
                    catch
                    {
                        _invMatrix = Matrix.Identity;
                    }
                }

                return (Matrix) _invMatrix;
            }
        }

        [Browsable(false)] public bool IsPrimaryNode => false;
        [Browsable(false)] public bool IsWeighted => _weights.Count > 1;
        [Browsable(false)] public IBoneNode Bone => _weights[0].Bone;

        public Influence()
        {
            _weights = new List<BoneWeight>();
        }

        public Influence(List<BoneWeight> weights)
        {
            _weights = weights;
            foreach (BoneWeight b in _weights)
            {
                if (b.Bone != null && !b.Bone.LinkedInfluences.Contains(this))
                {
                    b.Bone.LinkedInfluences.Add(this);
                }
            }
        }

        public Influence(IBoneNode bone)
        {
            _weights = new List<BoneWeight> {new BoneWeight(bone)};
            if (!bone.LinkedInfluences.Contains(this))
            {
                bone.LinkedInfluences.Add(this);
            }
        }

        ~Influence()
        {
            if (_weights != null)
            {
                foreach (BoneWeight b in _weights)
                {
                    if (b.Bone != null && b.Bone.LinkedInfluences.Contains(this))
                    {
                        b.Bone.LinkedInfluences.Remove(this);
                    }
                }
            }
        }

        public void CalcMatrix()
        {
            if (IsWeighted)
            {
                _matrix = Matrix.InfluenceMatrix(_weights);
                _invMatrix = Matrix.ReverseInfluenceMatrix(_weights);
            }
            else if (_weights.Count == 1 && Bone != null)
            {
                _matrix = Bone.Matrix;
                _invMatrix = Bone.InverseMatrix;
            }
            else
            {
                _invMatrix = _matrix = Matrix.Identity;
            }
        }

        public static int Compare(Influence i1, Influence i2)
        {
            if (i1._weights.Count < i2._weights.Count)
            {
                return -1;
            }

            if (i1._weights.Count > i2._weights.Count)
            {
                return 1;
            }

            if (i1.Users.Count > i2.Users.Count)
            {
                return -1;
            }

            if (i1.Users.Count < i2.Users.Count)
            {
                return 1;
            }

            return 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Influence)
            {
                return Equals(obj as Influence);
            }

            return false;
        }

        public bool Equals(Influence inf)
        {
            bool found;

            if (ReferenceEquals(this, inf))
            {
                return true;
            }

            if (ReferenceEquals(inf, null))
            {
                return false;
            }

            if (_weights.Count != inf._weights.Count)
            {
                return false;
            }

            foreach (BoneWeight w1 in _weights)
            {
                found = false;
                foreach (BoneWeight w2 in inf._weights)
                {
                    if (w1 == w2)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(Influence i1, Influence i2)
        {
            return i1.Equals(i2);
        }

        public static bool operator !=(Influence i1, Influence i2)
        {
            return !i1.Equals(i2);
        }
    }

    public class BoneWeight
    {
        public override string ToString()
        {
            return Bone.Name + " - " + Weight * 100.0f + "%";
        }

        public IBoneNode Bone;
        public float Weight;

        public bool Locked
        {
            get => Bone.Locked;
            set => Bone.Locked = value;
        }

        public BoneWeight() : this(null, 1.0f)
        {
        }

        public BoneWeight(IBoneNode bone) : this(bone, 1.0f)
        {
        }

        public BoneWeight(IBoneNode bone, float weight)
        {
            Bone = bone;
            Weight = weight;
        }

        public static bool operator ==(BoneWeight b1, BoneWeight b2)
        {
            if (ReferenceEquals(b1, b2))
            {
                return true;
            }

            return b1.Equals(b2);
        }

        public static bool operator !=(BoneWeight b1, BoneWeight b2)
        {
            return !(b1 == b2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is BoneWeight)
            {
                if (Bone == ((BoneWeight) obj).Bone && Math.Abs(Weight - ((BoneWeight) obj).Weight) <
                    Collada._importOptions._weightPrecision)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
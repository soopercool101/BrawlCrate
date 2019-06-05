using System;
using System.Collections.Generic;
using System.ComponentModel;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Wii.Models
{
    /// <summary>
    ///     Managed collection of influences. Only influences with references should be used.
    ///     It is up to the implementation to properly manage this collection.
    /// </summary>
    public class InfluenceManager
    {
        internal List<Influence> _influences = new List<Influence>();
        public List<Influence> Influences => _influences;

        public int Count => _influences.Count;

        public Influence FindOrCreate(Influence inf)
        {
            //Search for influence in list. If it exists, return it.
            foreach (var i in _influences)
                if (i.Equals(inf))
                    return i;

            //Not found, add it to the list.
            _influences.Add(inf);

            return inf;
        }

        public void Remove(Influence inf, IMatrixNodeUser user)
        {
            for (var i = 0; i < Count; i++)
                if (ReferenceEquals(_influences[i], inf) && inf.Users.Contains(user))
                {
                    inf.Users.Remove(user);
                    if (inf.Users.Count <= 0) _influences.RemoveAt(i);

                    break;
                }
        }

        //Get all weighted influences
        public Influence[] GetWeighted()
        {
            var list = new List<Influence>(_influences.Count);
            foreach (var i in _influences)
                if (i.IsWeighted)
                    list.Add(i);

            return list.ToArray();
        }

        //Remove all influences without references
        public void Clean()
        {
            var i = 0;
            while (i < Count)
                if (_influences[i].Users.Count <= 0)
                    _influences.RemoveAt(i);
                else
                    i++;
        }

        //Sorts influences
        public void Sort()
        {
            _influences.Sort(Influence.Compare);
        }
    }

    public class Influence : IMatrixNode
    {
        internal int _index;
        internal Matrix? _invMatrix;
        internal Matrix _matrix;

        internal List<IMatrixNodeUser> _references = new List<IMatrixNodeUser>();

        public Influence()
        {
            Weights = new List<BoneWeight>();
        }

        public Influence(List<BoneWeight> weights)
        {
            Weights = weights;
            foreach (var b in Weights)
                if (b.Bone != null && !b.Bone.LinkedInfluences.Contains(this))
                    b.Bone.LinkedInfluences.Add(this);
        }

        public Influence(IBoneNode bone)
        {
            Weights = new List<BoneWeight> {new BoneWeight(bone)};
            if (!bone.LinkedInfluences.Contains(this)) bone.LinkedInfluences.Add(this);
        }

        [Browsable(false)] public bool IsWeighted => Weights.Count > 1;

        [Browsable(false)] public IBoneNode Bone => Weights[0].Bone;

        /// <summary>
        ///     Don't modify this array!
        /// </summary>
        public List<BoneWeight> Weights { get; private set; }

        public List<IMatrixNodeUser> Users
        {
            get => _references;
            set => _references = value;
        }

        [Browsable(false)] public int NodeIndex => _index;

        [Browsable(false)] public Matrix Matrix => _matrix;

        [Browsable(false)]
        public Matrix InverseMatrix
        {
            get
            {
                if (_invMatrix == null)
                    try
                    {
                        _invMatrix = Matrix.Invert(_matrix);
                    }
                    catch
                    {
                        _invMatrix = Matrix.Identity;
                    }

                return (Matrix) _invMatrix;
            }
        }

        [Browsable(false)] public bool IsPrimaryNode => false;

        public override string ToString()
        {
            return "";
        }

        public void AddWeight(BoneWeight weight)
        {
            Weights.Add(weight);
            if (weight.Bone != null && !weight.Bone.LinkedInfluences.Contains(this))
                weight.Bone.LinkedInfluences.Add(this);
        }

        public void RemoveWeight(BoneWeight weight)
        {
            if (Weights.Contains(weight))
            {
                Weights.Remove(weight);
                if (weight.Bone != null && weight.Bone.LinkedInfluences.Contains(this))
                    weight.Bone.LinkedInfluences.Remove(this);
            }
        }

        public void SetWeights(List<BoneWeight> newWeights)
        {
            foreach (var b in Weights)
                if (b.Bone != null && b.Bone.LinkedInfluences.Contains(this))
                    b.Bone.LinkedInfluences.Remove(this);

            Weights = newWeights;
            foreach (var b in Weights)
                if (b.Bone != null && !b.Bone.LinkedInfluences.Contains(this))
                    b.Bone.LinkedInfluences.Add(this);
        }

        //Makes sure all weights add up to 1.0f.
        //Does not modify any locked weights.
        public void Normalize()
        {
            //Denominator and numerator to convert each unlocked weight with
            float denom = 0.0f, num = 1.0f;

            foreach (var b in Weights)
                if (b.Locked)
                    num -= b.Weight;
                else
                    denom += b.Weight;

            //Don't do anything if all weights are locked
            if (denom != 0.0f && num != 0.0f)
                foreach (var b in Weights)
                    if (!b.Locked) //Only normalize unlocked weights used in the calculation
                        b.Weight = (float) Math.Round(b.Weight / denom * num, 7);
        }

        public Influence Clone()
        {
            var i = new Influence();
            foreach (var b in Weights) i.AddWeight(new BoneWeight(b.Bone, b.Weight) {Locked = b.Locked});

            return i;
        }

        ~Influence()
        {
            if (Weights != null)
                foreach (var b in Weights)
                    if (b.Bone != null && b.Bone.LinkedInfluences.Contains(this))
                        b.Bone.LinkedInfluences.Remove(this);
        }

        public void CalcMatrix()
        {
            if (IsWeighted)
            {
                _matrix = Matrix.InfluenceMatrix(Weights);
                _invMatrix = Matrix.ReverseInfluenceMatrix(Weights);
            }
            else if (Weights.Count == 1 && Bone != null)
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
            if (i1.Weights.Count < i2.Weights.Count) return -1;

            if (i1.Weights.Count > i2.Weights.Count) return 1;

            if (i1.Users.Count > i2.Users.Count) return -1;

            if (i1.Users.Count < i2.Users.Count) return 1;

            return 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Influence) return Equals(obj as Influence);

            return false;
        }

        public bool Equals(Influence inf)
        {
            bool found;

            if (ReferenceEquals(this, inf)) return true;

            if (ReferenceEquals(inf, null)) return false;

            if (Weights.Count != inf.Weights.Count) return false;

            foreach (var w1 in Weights)
            {
                found = false;
                foreach (var w2 in inf.Weights)
                    if (w1 == w2)
                    {
                        found = true;
                        break;
                    }

                if (!found) return false;
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
        public IBoneNode Bone;
        public float Weight;

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

        public bool Locked
        {
            get => Bone.Locked;
            set => Bone.Locked = value;
        }

        public override string ToString()
        {
            return Bone.Name + " - " + Weight * 100.0f + "%";
        }

        public static bool operator ==(BoneWeight b1, BoneWeight b2)
        {
            if (ReferenceEquals(b1, b2)) return true;

            return b1.Equals(b2);
        }

        public static bool operator !=(BoneWeight b1, BoneWeight b2)
        {
            return !(b1 == b2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is BoneWeight)
                if (Bone == ((BoneWeight) obj).Bone && Math.Abs(Weight - ((BoneWeight) obj).Weight) <
                    Collada._importOptions._weightPrecision)
                    return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
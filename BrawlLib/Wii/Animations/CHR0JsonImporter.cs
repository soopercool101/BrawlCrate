using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace BrawlLib.Wii.Animations
{
    internal class CHR0JsonImporter
    {
        public static CHR0Node Convert(string input)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(BoneAnimation));
            Stream inputFile = new FileStream(input, FileMode.Open, FileAccess.Read);
            BoneAnimation data = (BoneAnimation) serializer.ReadObject(inputFile);

            CHR0Node node = data.ToCHR0Node();
            node.Name = Path.GetFileNameWithoutExtension(input);
            return node;
        }
    }

    [DataContract]
    internal class BoneAnimation
    {
        [DataMember] private readonly int FrameCount;
        [DataMember] private readonly bool Loop;
        [DataMember] private readonly ICollection<Bone> Bones;

        public BoneAnimation()
        {
            FrameCount = 1;
            Loop = false;
            Bones = new List<Bone>();
        }

        public CHR0Node ToCHR0Node()
        {
            CHR0Node node = new CHR0Node
            {
                Version = 4,
                FrameCount = FrameCount,
                Loop = Loop
            };

            foreach (Bone bone in Bones)
            {
                bone.AddToNode(node);
            }

            return node;
        }
    }

    [DataContract]
    internal class Bone
    {
        [DataMember] private readonly string Name;

        [DataMember] private readonly ICollection<Keyframe> Keyframes;

        public Bone()
        {
            Name = "TopN";
            Keyframes = new List<Keyframe>();
        }

        public void AddToNode(CHR0Node parentNode)
        {
            CHR0EntryNode entryNode = new CHR0EntryNode();
            parentNode.AddChild(entryNode);
            entryNode.SetSize(parentNode.FrameCount, parentNode.Loop);
            entryNode.Name = Name;

            foreach (Keyframe keyframe in Keyframes)
            {
                keyframe.AddToNode(entryNode);
            }

            //entryNode.Keyframes.Clean();
        }
    }

    [DataContract]
    internal class Keyframe
    {
        [DataMember] private readonly int Frame;
        private float?[] _transformations;

        internal Keyframe()
        {
            Frame = 0;
        }

        [DataMember]
        private float? ScaleX
        {
            get => _transformations[0];
            set => SetTransformation(0, value);
        }

        [DataMember]
        private float? ScaleY
        {
            get => _transformations[1];
            set => SetTransformation(1, value);
        }

        [DataMember]
        private float? ScaleZ
        {
            get => _transformations[2];
            set => SetTransformation(2, value);
        }

        [DataMember]
        private float? RotationX
        {
            get => _transformations[3];
            set => SetTransformation(3, value);
        }

        [DataMember]
        private float? RotationY
        {
            get => _transformations[4];
            set => SetTransformation(4, value);
        }

        [DataMember]
        private float? RotationZ
        {
            get => _transformations[5];
            set => SetTransformation(5, value);
        }

        [DataMember]
        private float? TranslationX
        {
            get => _transformations[6];
            set => SetTransformation(6, value);
        }

        [DataMember]
        private float? TranslationY
        {
            get => _transformations[7];
            set => SetTransformation(7, value);
        }

        [DataMember]
        private float? TranslationZ
        {
            get => _transformations[8];
            set => SetTransformation(8, value);
        }

        private void SetTransformation(int index, float? value)
        {
            if (_transformations == null)
            {
                _transformations = new float?[9];
            }

            _transformations[index] = value;
        }

        public void AddToNode(CHR0EntryNode node)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_transformations[i].HasValue)
                {
                    node.SetKeyframe(i, Frame, _transformations[i].Value, false, true);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Wii.Animations
{
    class CHR0JsonImporter
    {
        public static CHR0Node Convert(string input)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(BoneAnimation));
            Stream inputFile = new FileStream(input, FileMode.Open, FileAccess.Read);
            BoneAnimation data = (BoneAnimation)serializer.ReadObject(inputFile);

            CHR0Node node = data.ToCHR0Node();
            node.Name = Path.GetFileNameWithoutExtension(input);
            return node;
        }
    }

    [DataContract]
    class BoneAnimation
    {
        [DataMember]
        int FrameCount;
        [DataMember]
        bool Loop;
        [DataMember]
        ICollection<Bone> Bones;

        public CHR0Node ToCHR0Node()
        {
            CHR0Node node = new CHR0Node();
            node.Version = 4;
            node.FrameCount = FrameCount;
            node.Loop = Loop;

            foreach (Bone bone in Bones) {
                bone.AddToNode(node);
            }

            return node;
        }
    }

    [DataContract]
    class Bone
    {
        [DataMember]
        string Name;

        [DataMember]
        ICollection<Keyframe> Keyframes;

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
    class Keyframe
    {
        [DataMember]
        int Frame;

        float?[] _transformations;

        [DataMember]
        float? ScaleX
        {
            get { return _transformations[0]; }
            set { SetTransformation(0, value); }
        }
        [DataMember]
        float? ScaleY
        {
            get { return _transformations[1]; }
            set { SetTransformation(1, value); }
        }
        [DataMember]
        float? ScaleZ
        {
            get { return _transformations[2]; }
            set { SetTransformation(2, value); }
        }

        [DataMember]
        float? RotationX
        {
            get { return _transformations[3]; }
            set { SetTransformation(3, value); }
        }
        [DataMember]
        float? RotationY
        {
            get { return _transformations[4]; }
            set { SetTransformation(4, value); }
        }
        [DataMember]
        float? RotationZ
        {
            get { return _transformations[5]; }
            set { SetTransformation(5, value); }
        }

        [DataMember]
        float? TranslationX
        {
            get { return _transformations[6]; }
            set { SetTransformation(6, value); }
        }
        [DataMember]
        float? TranslationY
        {
            get { return _transformations[7]; }
            set { SetTransformation(7, value); }
        }
        [DataMember]
        float? TranslationZ
        {
            get { return _transformations[8]; }
            set { SetTransformation(8, value); }
        }

        void SetTransformation(int index, float? value)
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

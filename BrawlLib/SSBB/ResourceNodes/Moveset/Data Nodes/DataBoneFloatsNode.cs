using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefUnk17Node : MoveDefEntryNode
    {
        internal Unk17Entry* First => (Unk17Entry*) WorkingUncompressed.Address;
        private int Count;

        public override bool OnInitialize()
        {
            _extOverride = true;
            base.OnInitialize();
            if (Size % 0x1C != 0 && Size % 0x1C != 4)
            {
                Console.WriteLine(Size % 0x1C);
            }

            Count = WorkingUncompressed.Length / 0x1C;
            return Count > 0;
        }

        public override void OnPopulate()
        {
            Unk17Entry* addr = First;
            for (int i = 0; i < Count; i++)
            {
                new MoveDefUnk17EntryNode().Initialize(this, addr++, 28);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 0x1C;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            Unk17Entry* data = (Unk17Entry*) address;
            foreach (MoveDefUnk17EntryNode e in Children)
            {
                e.Rebuild(data++, 0x1C, true);
            }
        }
    }

    public unsafe class MoveDefUnk17EntryNode : MoveDefEntryNode
    {
        internal Unk17Entry* Header => (Unk17Entry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private int boneIndex;
        private float f1, f2, f3, f4, f5, f6;

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get
            {
                if (Model == null)
                {
                    return null;
                }

                if (boneIndex >= Model._linker.BoneCache.Length || boneIndex < 0)
                {
                    return null;
                }

                return (MDL0BoneNode) Model._linker.BoneCache[boneIndex];
            }
            set
            {
                boneIndex = value.BoneIndex;
                Name = value.Name;
            }
        }

        [Category("Unknown Entry")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone
        {
            get => BoneNode == null ? boneIndex.ToString() : BoneNode.Name;
            set
            {
                if (Model == null)
                {
                    boneIndex = Convert.ToInt32(value);
                    Name = boneIndex.ToString();
                }
                else
                {
                    BoneNode = string.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value);
                }

                SignalPropertyChange();
            }
        }

        [Category("Unknown Entry")]
        public float Float1
        {
            get => f1;
            set
            {
                f1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown Entry")]
        public float Float2
        {
            get => f2;
            set
            {
                f2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown Entry")]
        public float Float3
        {
            get => f3;
            set
            {
                f3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown Entry")]
        public float Float4
        {
            get => f4;
            set
            {
                f4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown Entry")]
        public float Float5
        {
            get => f5;
            set
            {
                f5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown Entry")]
        public float Float6
        {
            get => f6;
            set
            {
                f6 = value;
                SignalPropertyChange();
            }
        }

        public override string Name => Bone;

        public override bool OnInitialize()
        {
            boneIndex = Header->_boneIndex;

            f1 = Header->_unkVec1._x;
            f2 = Header->_unkVec1._y;
            f3 = Header->_unkVec1._z;
            f4 = Header->_unkVec2._x;
            f5 = Header->_unkVec2._y;
            f6 = Header->_unkVec2._z;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 0x1C;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            Unk17Entry* data = (Unk17Entry*) address;
            data->_boneIndex = boneIndex;
            data->_unkVec1._x = f1;
            data->_unkVec1._y = f2;
            data->_unkVec1._z = f3;
            data->_unkVec2._x = f4;
            data->_unkVec2._y = f5;
            data->_unkVec2._z = f6;
        }
    }
}
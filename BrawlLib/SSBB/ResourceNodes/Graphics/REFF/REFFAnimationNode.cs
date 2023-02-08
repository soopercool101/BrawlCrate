using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class REFFAnimationListNode : ResourceNode
    {
        internal VoidPtr First => WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Container;
        public ushort _ptclTrackCount, _ptclInitTrackCount, _emitTrackCount, _emitInitTrackCount;
        public buint* _ptclTrackAddr, _emitTrackAddr;
        public List<uint> _ptclTrack, _emitTrack;

        [Category("Animation Table")] public ushort PtclTrackCount => _ptclTrackCount;
        [Category("Animation Table")] public ushort PtclInitTrackCount => _ptclInitTrackCount;
        [Category("Animation Table")] public ushort EmitTrackCount => _emitTrackCount;
        [Category("Animation Table")] public ushort EmitInitTrackCount => _emitInitTrackCount;

        public override bool OnInitialize()
        {
            _name = "Animations";

            return PtclTrackCount > 0 || EmitTrackCount > 0;
        }

        public override void OnPopulate()
        {
            int offset = 0;
            buint* addr = _ptclTrackAddr;
            addr += PtclTrackCount; //skip nulled pointers to size list
            for (int i = 0; i < PtclTrackCount; i++)
            {
                new REFFAnimationNode {AnimationType = REFFAnimationNode.AnimType.Particle}.Initialize(this, First + offset, (int) *addr);
                offset += (int) *addr++;
            }

            addr = _emitTrackAddr;
            addr += EmitTrackCount; //skip nulled pointers to size list
            for (int i = 0; i < EmitTrackCount; i++)
            {
                new REFFAnimationNode().Initialize(this, First + offset, (int) *addr);
                offset += (int) *addr++;
            }
        }

        public ushort ptcl, emit;

        public override int OnCalculateSize(bool force)
        {
            ptcl = 0;
            emit = 0;
            int size = 8;
            size += Children.Count * 8;
            foreach (REFFAnimationNode e in Children)
            {
                if (e.AnimationType == REFFAnimationNode.AnimType.Particle)
                {
                    ptcl++;
                }
                else
                {
                    emit++;
                }

                size += e.CalculateSize(true);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            buint* addr = (buint*) address;
            ((bushort*) addr)[0] = ptcl;
            ((bushort*) addr)[1] = _ptclInitTrackCount;
            addr += ptcl + 1;
            foreach (REFFAnimationNode e in Children)
            {
                if (e.AnimationType == REFFAnimationNode.AnimType.Particle)
                {
                    *addr++ = (uint) e._calcSize;
                }
            }

            ((bushort*) addr)[0] = emit;
            ((bushort*) addr)[1] = _emitInitTrackCount;
            addr += emit + 1;
            foreach (REFFAnimationNode e in Children)
            {
                if (e.AnimationType != REFFAnimationNode.AnimType.Particle)
                {
                    *addr++ = (uint) e._calcSize;
                }
            }

            VoidPtr ptr = addr;
            foreach (REFFAnimationNode e in Children)
            {
                if (e.AnimationType == REFFAnimationNode.AnimType.Particle)
                {
                    e.Rebuild(ptr, e._calcSize, true);
                    ptr += e._calcSize;
                }
            }

            foreach (REFFAnimationNode e in Children)
            {
                if (e.AnimationType != REFFAnimationNode.AnimType.Particle)
                {
                    e.Rebuild(ptr, e._calcSize, true);
                    ptr += e._calcSize;
                }
            }
        }
    }

    public unsafe class REFFAnimationNode : ResourceNode
    {
        internal AnimCurveHeader* Header => (AnimCurveHeader*) WorkingUncompressed.Address;
        internal AnimCurveHeader Data = new AnimCurveHeader();
        
        public enum AnimType : byte
        {
            Particle = 0xAB,
            Emitter = 0xAC
        }

        [Browsable(false)]
        public AnimType AnimationType
        {
            get => (AnimType)Data.identifier;
            set
            {
                Data.identifier = (byte)value;
                SignalPropertyChange();
            }
        }

        //public byte identifier;         // 0x00 - 0xAB: Particle | 0xAC: Emitter
        //public byte kindType;           // 0x01
        public enum AnimKind : byte
        {
            ZERO = 0x00,
            COLOR0PRI = 0x01,
            ALPHA0PRI = 0x02,
            SCALE = 0x0A,
            ACMPREF0 = 0x0B,
            ACMPREF1 = 0x0C,
            TEXTURE1ROTATE = 0x0E,
            CHILD = 0x1A,
            FIELD_RANDOM = 0x21
        }

        public AnimKind Kind
        {
            get => (AnimKind)Data.kindType;
            set
            {
                Data.kindType = (byte)value;
                Name = value.ToString();
                SignalPropertyChange();
            }
        }

        //public byte curveFlag;          // 0x02
        public byte CurveType
        {
            get => Data.curveFlag;
            set
            {
                Data.curveFlag = value;
                SignalPropertyChange();
            }
        }

        //public Bin8 dimensionFlags;     // 0x03 - (1 = X, 2 = Y, 4 = Z. None being active disables the animation)
        public bool DimensionX
        {
            get => Data.dimensionFlags[0];
            set
            {
                Data.dimensionFlags[0] = value;
            }
        }
        public bool DimensionY
        {
            get => Data.dimensionFlags[1];
            set
            {
                Data.dimensionFlags[1] = value;
            }
        }
        public bool DimensionZ
        {
            get => Data.dimensionFlags[2];
            set
            {
                Data.dimensionFlags[2] = value;
            }
        }

        //public byte processFlag;        // 0x04 - often 0
        public byte ProcessFlag
        {
            get => Data.processFlag;
            set
            {
                Data.processFlag = value;
                SignalPropertyChange();
            }
        }

        //public byte loopCount;          // 0x05 - often 0
        public byte LoopCount
        {
            get => Data.loopCount;
            set
            {
                Data.loopCount = value;
                SignalPropertyChange();
            }
        }

        //public bushort randomSeed;      // 0x06
        [TypeConverter(typeof(HexUShortConverter))]
        public ushort RandomSeed
        {
            get => Data.randomSeed;
            set
            {
                Data.randomSeed = value;
                SignalPropertyChange();
            }
        }

        //public bushort frameLength;     // 0x08
        public ushort FrameLength
        {
            get => Data.frameLength;
            set
            {
                Data.frameLength = value;
                SignalPropertyChange();
            }
        }

        //public bushort padding;         // 0x0A
        public ushort Unknown0x0A
        {
            get => Data.padding;
            set
            {
                Data.padding = value;
                SignalPropertyChange();
            }
        }

        //public buint keyTableSize;      // 0x0C
        //public buint rangeTableSize;    // 0x10 - Offset = KeyTable
        //public buint randomTableSize;   // 0x14 - Offset = KeyTable + RangeTable
        //public buint nameTableSize;     // 0x18 - Offset = KeyTable + RangeTable + RandomTable
        //public buint infoTableSize;

        public override bool OnInitialize()
        {
            Data = *Header;
            _name = Kind.ToString();
            return WorkingUncompressed.Length > 0x20;
        }

        public override void OnPopulate()
        {
            var offset = 0x20;
            ResourceNode n = NodeFactory.FromSource(this, new DataSource(WorkingUncompressed.Address + offset, (int)Data.keyTableSize), typeof(RawDataNode), false);
            n._name = "KeyTable";
            offset += (int)Data.keyTableSize;
            n = NodeFactory.FromSource(this, new DataSource(WorkingUncompressed.Address + offset, (int)Data.rangeTableSize), typeof(RawDataNode), false);
            n._name = "RangeTable";
            offset += (int)Data.rangeTableSize;
            n = NodeFactory.FromSource(this, new DataSource(WorkingUncompressed.Address + offset, (int)Data.randomTableSize), typeof(RawDataNode), false);
            n._name = "RandomTable";
            offset += (int)Data.randomTableSize;
            n = NodeFactory.FromSource(this, new DataSource(WorkingUncompressed.Address + offset, (int)Data.nameTableSize), typeof(RawDataNode), false);
            n._name = "NameTable";
            offset += (int)Data.nameTableSize;
            n = NodeFactory.FromSource(this, new DataSource(WorkingUncompressed.Address + offset, (int)Data.infoTableSize), typeof(RawDataNode), false);
            n._name = "InfoTable";
        }

        public override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }
    }

    public unsafe class REFFPostFieldInfoNode : ResourceNode
    {
        internal PostFieldInfo* Header => (PostFieldInfo*) WorkingUncompressed.Address;

        private PostFieldInfo hdr;

        [Category("Post Field Info")] public Vector3 Scale => hdr.mAnimatableParams.mSize;
        [Category("Post Field Info")] public Vector3 Rotation => hdr.mAnimatableParams.mRotate;
        [Category("Post Field Info")] public Vector3 Translation => hdr.mAnimatableParams.mTranslate;
        [Category("Post Field Info")] public float ReferenceSpeed => hdr.mReferenceSpeed;

        [Category("Post Field Info")]
        public PostFieldInfo.ControlSpeedType ControlSpeedType =>
            (PostFieldInfo.ControlSpeedType) hdr.mControlSpeedType;

        [Category("Post Field Info")]
        public PostFieldInfo.CollisionShapeType CollisionShapeType =>
            (PostFieldInfo.CollisionShapeType) hdr.mCollisionShapeType;

        [Category("Post Field Info")]
        public PostFieldInfo.ShapeOption ShapeOption =>
            CollisionShapeType == PostFieldInfo.CollisionShapeType.Sphere ||
            CollisionShapeType == PostFieldInfo.CollisionShapeType.Plane
                ? (PostFieldInfo.ShapeOption) (((int) CollisionShapeType << 2) | hdr.mCollisionShapeOption)
                : PostFieldInfo.ShapeOption.None;

        [Category("Post Field Info")]
        public PostFieldInfo.CollisionType CollisionType => (PostFieldInfo.CollisionType) hdr.mCollisionType;

        [Category("Post Field Info")]
        public PostFieldInfo.CollisionOption CollisionOption =>
            (PostFieldInfo.CollisionOption) (short) hdr.mCollisionOption;

        [Category("Post Field Info")] public ushort StartFrame => hdr.mStartFrame;
        [Category("Post Field Info")] public Vector3 SpeedFactor => hdr.mSpeedFactor;

        public override bool OnInitialize()
        {
            _name = "Entry" + Index;
            hdr = *Header;
            return false;
        }

        public override void OnPopulate()
        {
            base.OnPopulate();
        }
    }

    public class REFFKeyframeArray
    {
    }
}
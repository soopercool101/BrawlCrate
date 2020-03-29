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
                new REFFAnimationNode {_isPtcl = true}.Initialize(this, First + offset, (int) *addr);
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
                if (e._isPtcl)
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
                if (e._isPtcl)
                {
                    *addr++ = (uint) e._calcSize;
                }
            }

            ((bushort*) addr)[0] = emit;
            ((bushort*) addr)[1] = _emitInitTrackCount;
            addr += emit + 1;
            foreach (REFFAnimationNode e in Children)
            {
                if (!e._isPtcl)
                {
                    *addr++ = (uint) e._calcSize;
                }
            }

            VoidPtr ptr = addr;
            foreach (REFFAnimationNode e in Children)
            {
                if (e._isPtcl)
                {
                    e.Rebuild(ptr, e._calcSize, true);
                    ptr += e._calcSize;
                }
            }

            foreach (REFFAnimationNode e in Children)
            {
                if (!e._isPtcl)
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
        public override ResourceType ResourceFileType => ResourceType.REFFAnimationList;

        internal AnimCurveHeader _hdr;

        public bool _isPtcl;

        public enum AnimType
        {
            Particle,
            Emitter
        }

        [Category("Effect Animation")]
        public AnimType Type
        {
            get => _isPtcl ? AnimType.Particle : AnimType.Emitter;
            set
            {
                _isPtcl = value == AnimType.Particle;
                SignalPropertyChange();
            }
        }

        //[Category("Animation")]
        //public byte Magic { get { return _hdr.magic; } }
        [Category("Effect Animation")]
        [TypeConverter(typeof(DropDownListReffAnimType))]
        public string KindType
        {
            get
            {
                if (((REFFNode) Parent.Parent.Parent).VersionMinor == 9)
                {
                    switch (CurveFlag)
                    {
                        case AnimCurveType.ParticleByte:
                        case AnimCurveType.ParticleFloat:
                            return ((v9AnimCurveTargetByteFloat) _hdr.kindType).ToString();
                        case AnimCurveType.ParticleRotate:
                            return ((v9AnimCurveTargetRotateFloat) _hdr.kindType).ToString();
                        case AnimCurveType.ParticleTexture:
                            return ((v9AnimCurveTargetPtclTex) _hdr.kindType).ToString();
                        case AnimCurveType.Child:
                            return ((v9AnimCurveTargetChild) _hdr.kindType).ToString();
                        case AnimCurveType.Field:
                            return ((v9AnimCurveTargetField) _hdr.kindType).ToString();
                        case AnimCurveType.PostField:
                            return ((v9AnimCurveTargetPostField) _hdr.kindType).ToString();
                        case AnimCurveType.EmitterFloat:
                            return ((v9AnimCurveTargetEmitterFloat) _hdr.kindType).ToString();
                    }
                }
                else
                {
                    return ((AnimCurveTarget7) _hdr.kindType).ToString();
                }

                //switch (CurveFlag)
                //{
                //    case AnimCurveType.ParticleByte:
                //    case AnimCurveType.ParticleFloat:
                //        return ((v7AnimCurveTargetByteFloat)_hdr.kindType).ToString();
                //    case AnimCurveType.ParticleRotate:
                //        return ((v7AnimCurveTargetRotateFloat)_hdr.kindType).ToString();
                //    case AnimCurveType.ParticleTexture:
                //        return ((v7AnimCurveTargetPtclTex)_hdr.kindType).ToString();
                //    case AnimCurveType.Child:
                //        return ((v7AnimCurveTargetChild)_hdr.kindType).ToString();
                //    case AnimCurveType.Field:
                //        return ((v7AnimCurveTargetField)_hdr.kindType).ToString();
                //    case AnimCurveType.PostField:
                //        return ((v7AnimCurveTargetPostField)_hdr.kindType).ToString();
                //    case AnimCurveType.EmitterFloat:
                //        return ((v7AnimCurveTargetEmitterFloat)_hdr.kindType).ToString();
                //}
                return null;
            }
            set
            {
                int i = 0;
                if (((REFFNode) Parent.Parent.Parent).VersionMinor == 9)
                {
                    switch (CurveFlag)
                    {
                        case AnimCurveType.ParticleByte:
                        case AnimCurveType.ParticleFloat:
                            v9AnimCurveTargetByteFloat a;
                            if (Enum.TryParse(value, true, out a))
                            {
                                _hdr.kindType = (byte) a;
                            }
                            else if (int.TryParse(value, out i))
                            {
                                _hdr.kindType = (byte) i;
                            }

                            break;
                        case AnimCurveType.ParticleRotate:
                            v9AnimCurveTargetRotateFloat b;
                            if (Enum.TryParse(value, true, out b))
                            {
                                _hdr.kindType = (byte) b;
                            }
                            else if (int.TryParse(value, out i))
                            {
                                _hdr.kindType = (byte) i;
                            }

                            break;
                        case AnimCurveType.ParticleTexture:
                            v9AnimCurveTargetPtclTex c;
                            if (Enum.TryParse(value, true, out c))
                            {
                                _hdr.kindType = (byte) c;
                            }
                            else if (int.TryParse(value, out i))
                            {
                                _hdr.kindType = (byte) i;
                            }

                            break;
                        case AnimCurveType.Child:
                            v9AnimCurveTargetChild d;
                            if (Enum.TryParse(value, true, out d))
                            {
                                _hdr.kindType = (byte) d;
                            }
                            else if (int.TryParse(value, out i))
                            {
                                _hdr.kindType = (byte) i;
                            }

                            break;
                        case AnimCurveType.Field:
                            v9AnimCurveTargetField e;
                            if (Enum.TryParse(value, true, out e))
                            {
                                _hdr.kindType = (byte) e;
                            }
                            else if (int.TryParse(value, out i))
                            {
                                _hdr.kindType = (byte) i;
                            }

                            break;
                        case AnimCurveType.PostField:
                            v9AnimCurveTargetPostField f;
                            if (Enum.TryParse(value, true, out f))
                            {
                                _hdr.kindType = (byte) f;
                            }
                            else if (int.TryParse(value, out i))
                            {
                                _hdr.kindType = (byte) i;
                            }

                            break;
                        case AnimCurveType.EmitterFloat:
                            v9AnimCurveTargetEmitterFloat g;
                            if (Enum.TryParse(value, true, out g))
                            {
                                _hdr.kindType = (byte) g;
                            }
                            else if (int.TryParse(value, out i))
                            {
                                _hdr.kindType = (byte) i;
                            }

                            break;
                    }
                }
                else
                {
                    if (int.TryParse(value, out i))
                    {
                        _hdr.kindType = (byte) i;
                    }
                }

                SignalPropertyChange();
            }
        }

        [Category("Effect Animation")]
        public AnimCurveType CurveFlag =>
            (AnimCurveType) _hdr.curveFlag; //set { hdr.curveFlag = (byte)value; SignalPropertyChange(); } }

        [Category("Effect Animation")] public byte KindEnable => _hdr.kindEnable;

        [Category("Effect Animation")]
        public AnimCurveHeaderProcessFlagType ProcessFlag => (AnimCurveHeaderProcessFlagType) _hdr.processFlag;

        [Category("Effect Animation")] public byte LoopCount => _hdr.loopCount;

        [Category("Effect Animation")]
        public ushort RandomSeed
        {
            get => _hdr.randomSeed;
            set
            {
                _random = new Random(_hdr.randomSeed = value);
                SignalPropertyChange();
            }
        }

        [Category("Effect Animation")]
        public ushort FrameCount
        {
            get => _hdr.frameLength;
            set
            {
                _hdr.frameLength = value;
                SignalPropertyChange();
            }
        }
        //[Category("Animation")]
        //public ushort Padding { get { return _hdr.padding; } }

        [Category("Effect Animation")] public uint KeyTableSize => _hdr.keyTable;
        [Category("Effect Animation")] public uint RangeTableSize => _hdr.rangeTable;
        [Category("Effect Animation")] public uint RandomTableSize => _hdr.randomTable;
        [Category("Effect Animation")] public uint NameTableSize => _hdr.nameTable;
        [Category("Effect Animation")] public uint InfoTableSize => _hdr.infoTable;

        private Random _random;

        [Category("Name Table")]
        public string[] Names
        {
            get => _names.ToArray();
            set
            {
                _names = value.ToList();
                SignalPropertyChange();
            }
        }

        public List<string> _names = new List<string>();

        public override bool OnInitialize()
        {
            _hdr = *Header;
            _name = KindType;
            _random = new Random(RandomSeed);

            Bin8 enabled = Header->kindEnable;
            List<int> enabledIndices = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                if (enabled[i])
                {
                    enabledIndices.Add(i);
                }
            }

            int size = 0;
            switch (CurveFlag)
            {
                case AnimCurveType.ParticleByte:
                    size = 1;
                    break;
                case AnimCurveType.ParticleFloat:
                    size = 4;
                    break;
                case AnimCurveType.ParticleRotate:
                    size = 1;
                    break;
                case AnimCurveType.ParticleTexture:
                    break;
                case AnimCurveType.Child:
                    break;
                case AnimCurveType.EmitterFloat:
                    break;
                case AnimCurveType.Field:
                    break;
                case AnimCurveType.PostField:
                    break;
            }

            VoidPtr offset = (VoidPtr) Header + 0x20;
            if (KeyTableSize > 4)
            {
                AnimCurveTableHeader* hdr = (AnimCurveTableHeader*) offset;
                AnimCurveKeyHeader* key = hdr->First;
                for (int i = 0; i < hdr->_count; i++, key = key->Next(enabledIndices.Count, size))
                {
                    key->GetFrameIndex(enabledIndices.Count, size);
                }
            }

            offset += KeyTableSize;
            if (RangeTableSize > 4)
            {
                AnimCurveTableHeader* hdr = (AnimCurveTableHeader*) offset;
            }

            offset += RangeTableSize;
            if (RandomTableSize > 4)
            {
                AnimCurveTableHeader* hdr = (AnimCurveTableHeader*) offset;
            }

            offset += RandomTableSize;
            if (NameTableSize > 4)
            {
                if (offset + NameTableSize <= size)
                {
                    AnimCurveTableHeader* hdr = (AnimCurveTableHeader*) offset;

                    _names = new List<string>();
                    bushort* addr = (bushort*) ((VoidPtr) hdr + 4 + hdr->_count * 4);
                    for (int i = 0; i < hdr->_count; i++, addr = (bushort*) ((VoidPtr) addr + 2 + *addr))
                    {
                        _names.Add(new string((sbyte*) addr + 2));
                    }

                    offset += NameTableSize;
                }
            }
            else
            {
                offset += NameTableSize;
            }

            if (InfoTableSize > 4)
            {
                AnimCurveTableHeader* hdr = (AnimCurveTableHeader*) offset;
                //switch ((v9AnimCurveTargetField)_hdr.kindType)
                //{

                //}
            }

#if DEBUG
            if (CurveFlag == AnimCurveType.EmitterFloat || CurveFlag == AnimCurveType.PostField)
            {
                System.Windows.Forms.MessageBox.Show(TreePath);
            }
#endif

            switch (CurveFlag)
            {
                case AnimCurveType.ParticleByte:
                    break;
                case AnimCurveType.ParticleFloat:
                    break;
                case AnimCurveType.ParticleRotate:
                    break;
                case AnimCurveType.ParticleTexture:
                    break;
                case AnimCurveType.Child:
                    break;
            }

            return false;
        }

        public override void OnPopulate()
        {
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
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GCATNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GCATEntryNode);
        protected override string baseName => "Catapults";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GCAT" ? new GCATNode() : null;
        }
    }

    public unsafe class GCATEntryNode : ResourceNode
    {
        internal GCATEntry* Header => (GCATEntry*)WorkingUncompressed.Address;
        internal GCATEntry Data;
        public List<ResourceNode> RenderTargets
        {
            get
            {
                List<ResourceNode> _targets = new List<ResourceNode>();
                if (Parent?.Parent?.Parent is ARCNode a)
                {
                    if (ModelDataIndex != byte.MaxValue)
                    {
                        ResourceNode model = a.Children.FirstOrDefault(c => c is ARCEntryNode ae && ae.FileType == ARCFileType.ModelData && ae.FileIndex == ModelDataIndex);
                        if (model != null)
                            _targets.Add(model);
                    }
                }
                return _targets;
            }
        }

        public override bool supportsCompression => false;

        private MotionPathDataClass _motionPathData;
        [Category("GCAT")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public MotionPathDataClass MotionPathData
        {
            get => _motionPathData;
            set
            {
                _motionPathData = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("GCAT")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 AreaOffsetPosition
        {
            get => new Vector2(Data._areaOffsetPosX, Data._areaOffsetPosY);
            set
            {
                Data._areaOffsetPosX = value.X;
                Data._areaOffsetPosY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GCAT")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 AreaRange
        {
            get => new Vector2(Data._areaRangeX, Data._areaRangeY);
            set
            {
                Data._areaRangeX = value.X;
                Data._areaRangeY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("GCAT")]
        public float FramesBeforeStartMove
        {
            get => Data._framesBeforeStartMove;
            set
            {
                Data._framesBeforeStartMove = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [Category("GCAT")]
        public float LaunchAngle
        {
            get => Data._vector;
            set
            {
                Data._vector = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x40
        {
            get => Data._unknown0x40;
            set
            {
                Data._unknown0x40 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x44
        {
            get => Data._unknown0x44;
            set
            {
                Data._unknown0x44 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x48
        {
            get => Data._unknown0x48;
            set
            {
                Data._unknown0x48 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x4C
        {
            get => Data._unknown0x4C;
            set
            {
                Data._unknown0x4C = value;
                SignalPropertyChange();
            }
        }

        [Category("GCAT")]
        public byte ModelDataIndex
        {
            get => Data._modelDataIndex;
            set
            {
                Data._modelDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("GCAT")]
        public bool IsFaceLeft
        {
            get => Data._isFaceLeft == 1;
            set
            {
                Data._isFaceLeft = value ? (byte)1 : (byte)0;
                SignalPropertyChange();
            }
        }

        [Category("GCAT")]
        public bool UseNoHelperWarp
        {
            get => Data._useNoHelperWarp == 1;
            set
            {
                Data._useNoHelperWarp = value ? (byte)1 : (byte)0;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x53
        {
            get => Data._unknown0x53;
            set
            {
                Data._unknown0x53 = value;
                SignalPropertyChange();
            }
        }

        public GCATEntryNode()
        {
            Data = new GCATEntry();
            _motionPathData = new MotionPathDataClass(this);
        }

        public override int OnCalculateSize(bool force)
        {
            return GCATEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *Header;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GCATEntry* hdr = (GCATEntry*)address;
            *hdr = Data;
        }
    }
}

using BrawlLib.Internal;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Navigation
{
    public class GWAPNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GWAPEntryNode);
        protected override string baseName => "Warp Zone";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GWAP" ? new GWAPNode() : null;
        }
    }

    public unsafe class GWAPEntryNode : ResourceNode, IRenderedLink
    {
        public GWAPEntry Data;
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

        public MotionPathDataClass _motionPathData;
        [Category("Warp Zone")]
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

        [Category("Warp Zone")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 AreaOffset
        {
            get => new Vector2(Data._areaOffsetPositionX, Data._areaOffsetPositionY);
            set
            {
                Data._areaOffsetPositionX = value.X;
                Data._areaOffsetPositionY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("Warp Zone")]
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

        [Category("Warp Zone")]
        public string BoneName
        {
            get => Data.BoneName;
            set
            {
                Data.BoneName = value;
                SignalPropertyChange();
            }
        }

        [Category("Warp Zone")]
        public byte ModelDataIndex
        {
            get => Data._modelDataIndex;
            set
            {
                Data._modelDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x51
        {
            get => Data._unknown0x51;
            set
            {
                Data._unknown0x51 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x52
        {
            get => Data._unknown0x52;
            set
            {
                Data._unknown0x52 = value;
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

        [Category("Warp Zone")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 Position
        {
            get => new Vector2(Data._positionX, Data._positionY);
            set
            {
                Data._positionX = value.X;
                Data._positionY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("Warp Zone")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint SoundId1
        {
            get => Data._soundId1;
            set
            {
                Data._soundId1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Warp Zone")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint SoundId2
        {
            get => Data._soundId2;
            set
            {
                Data._soundId2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x64
        {
            get => Data._unknown0x64;
            set
            {
                Data._unknown0x64 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x68
        {
            get => Data._unknown0x68;
            set
            {
                Data._unknown0x68 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _warpTriggerData;
        [Category("Warp Zone")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass WarpTriggerData
        {
            get => _warpTriggerData;
            set
            {
                _warpTriggerData = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x70
        {
            get => Data._unknown0x70;
            set
            {
                Data._unknown0x70 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x74
        {
            get => Data._unknown0x74;
            set
            {
                Data._unknown0x74 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public uint Unknown0x78
        {
            get => Data._unknown0x78;
            set
            {
                Data._unknown0x78 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _isValidTriggerData;
        [Category("Warp Zone")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public TriggerDataClass IsValidTriggerData
        {
            get => _isValidTriggerData;
            set
            {
                _isValidTriggerData = value;
                SignalPropertyChange();
            }
        }

        public GWAPEntryNode()
        {
            _motionPathData = new MotionPathDataClass(this);
            _warpTriggerData = new TriggerDataClass(this);
            _isValidTriggerData = new TriggerDataClass(this);
        }

        public override bool OnInitialize()
        {
            Data = *(GWAPEntry*)WorkingUncompressed.Address;
            _motionPathData = new MotionPathDataClass(this, Data._motionPathData);
            _warpTriggerData = new TriggerDataClass(this, Data._warpTriggerData);
            _isValidTriggerData = new TriggerDataClass(this, Data._isValidTriggerData);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GWAPEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GWAPEntry* hdr = (GWAPEntry*)address;
            Data._motionPathData = _motionPathData;
            Data._warpTriggerData = _warpTriggerData;
            Data._isValidTriggerData = _isValidTriggerData;
            *hdr = Data;
        }
    }
}

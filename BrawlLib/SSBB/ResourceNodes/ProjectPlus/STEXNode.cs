using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class STEXNode : ResourceNode
    {
        internal STEX* Header => (STEX*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.STEX;

        public override Type[] AllowedChildTypes => new[] {typeof(RawDataNode)};

        [Flags]
        public enum StageFlags : ushort
        {
            None = 0x0000,
            Flat = 0x0001,
            FixedCamera = 0x0002,
            SlowStart = 0x0004,
            DualLoad = 0x0008,
            DualShuffle = 0x0010,
            OldSubstage = 0x0020
        }

        public enum VariantType : byte
        {
            Normal = 0x00,
            RandomVariance = 0x01,
            SequentialTransform = 0x02,
            RandomTransform = 0x03,
            TimeBasedVariance = 0x04,
            None = 0xFF
        }

        private string _stageName;
        [Category("Stage Parameters")]
        public string StageName
        {
            get => _stageName;
            set
            {
                _stageName = value;
                SignalPropertyChange();
            }
        }

        private string _trackList;
        [Category("Stage Parameters")]
        public string TrackList
        {
            get => _trackList;
            set
            {
                _trackList = value;
                SignalPropertyChange();
            }
        }

        private string _module;
        [Category("Stage Parameters")]
        public string Module
        {
            get => _module;
            set
            {
                _module = value;
                SignalPropertyChange();
            }
        }

        private RGBAPixel _rgbaOverlay;
        [Category("Stage Parameters")]
        public RGBAPixel CharacterOverlay
        {
            get => _rgbaOverlay;
            set
            {
                _rgbaOverlay = value;
                SignalPropertyChange();
            }
        }


        private ushort _soundBank;
        [Category("Stage Parameters")]
        public string SoundBank
        {
            get => "0x" + _soundBank.ToString("X4");
            set
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _soundBank = Convert.ToUInt16(field0, fromBase);
                SignalPropertyChange();
            }
        }

        private ushort _effectBank;
        [Category("Stage Parameters")]
        public string EffectBank
        {
            get => "0x" + _effectBank.ToString("X4");
            set
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _effectBank = Convert.ToUInt16(field0, fromBase);
                SignalPropertyChange();
            }
        }

        private StageFlags _flags;
#if !DEBUG
        [Browsable(false)]
#endif
        public StageFlags Flags
        {
            get => _flags;
            set
            {
                _flags = value;
                SignalPropertyChange();
            }
        }

        [Category("Stage Flags")]
        public bool IsFlat
        {
            get => (_flags & StageFlags.Flat) != 0;
            set
            {
                _flags = (_flags & ~StageFlags.Flat) |
                         (value ? StageFlags.Flat : 0);
                SignalPropertyChange();
            }
        }

        [Category("Stage Flags")]
        public bool IsFixedCamera
        {
            get => (_flags & StageFlags.FixedCamera) != 0;
            set
            {
                _flags = (_flags & ~StageFlags.FixedCamera) |
                         (value ? StageFlags.FixedCamera : 0);
                SignalPropertyChange();
            }
        }

        [Category("Stage Flags")]
        public bool IsSlowStart
        {
            get => (_flags & StageFlags.SlowStart) != 0;
            set
            {
                _flags = (_flags & ~StageFlags.SlowStart) |
                         (value ? StageFlags.SlowStart : 0);
                SignalPropertyChange();
            }
        }

        [Category("Substage Flags")]
        public bool IsDualLoad
        {
            get => (_flags & StageFlags.DualLoad) != 0;
            set
            {
                _flags = (_flags & ~StageFlags.DualLoad) |
                         (value ? StageFlags.DualLoad : 0);
                SignalPropertyChange();
            }
        }

        [Category("Substage Flags")]
        public bool IsDualShuffle
        {
            get => (_flags & StageFlags.DualShuffle) != 0;
            set
            {
                _flags = (_flags & ~StageFlags.DualShuffle) |
                         (value ? StageFlags.DualShuffle : 0);
                SignalPropertyChange();
            }
        }

        [Category("Substage Flags")]
        public bool IsOldSubstage
        {
            get => (_flags & StageFlags.OldSubstage) != 0;
            set
            {
                _flags = (_flags & ~StageFlags.OldSubstage) |
                         (value ? StageFlags.OldSubstage : 0);
                SignalPropertyChange();
            }
        }

        private byte _stageType;
        [Category("Substage Parameters")]
        public VariantType SubstageVarianceType
        {
            get => (VariantType)_stageType;
            set
            {
                _stageType = (byte)value;
                SignalPropertyChange();
            }
        }

        private byte _subStageRange;
        [Category("Substage Parameters")]
        public byte SubstageRange
        {
            get => _subStageRange;
            set
            {
                _subStageRange = value;
                SignalPropertyChange();
            }
        }

        private uint _memoryAllocation;
        [Category("Stage Parameters")]
        public string MemoryAllocation
        {
            get => "0x" + _memoryAllocation.ToString("X8");
            set
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _memoryAllocation = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        private float _wildSpeed;
        [Category("Stage Parameters")]
        [Description("The speed at which the stage operates in \"Wild\" Mode in Project+")]
        public float WildSpeed
        {
            get => _wildSpeed;
            set
            {
                _wildSpeed = value;
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            int i;
            for (i = 0; i < Header->subStageCount; i++)
            {
                new RawDataNode { _name = Header->subStageName(i) }.Initialize(this, null, 0);
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }
            _stageName = Header->stageName;
            _trackList = Header->trackListName;
            _module = Header->moduleName;
            _rgbaOverlay = (uint)Header->_rgbaOverlay;
            _soundBank = Header->_soundBank;
            _effectBank = Header->_effectBank;
            _flags = (StageFlags)(ushort)Header->_flags;
            _stageType = Header->_stageType;
            _subStageRange = Header->_subStageRange;
            _memoryAllocation = Header->_memoryAllocation;
            _wildSpeed = Header->_wildSpeed;

            return Header->subStageCount > 0 || Header->_subStageRange > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (int)STEX.HeaderSize;

            size += 4 * Children.Count;

            if (!string.IsNullOrEmpty(StageName))
            {
                size += StageName.UTF8Length() + 1;
            }
            if (!string.IsNullOrEmpty(TrackList))
            {
                size += TrackList.UTF8Length() + 1;
            }
            if (!string.IsNullOrEmpty(Module))
            {
                size += Module.UTF8Length() + 1;
            }

            foreach (ResourceNode n in Children)
            {
                size += n.Name.UTF8Length() + 1;
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            uint strOffset = STEX.HeaderSize + (uint)Children.Count * 4;

            STEX* header = (STEX*)address;
            *header = new STEX();
            header->_tag = STEX.Tag;
            header->_stringOffset = strOffset;
            header->_size = (uint)length;
            header->_rgbaOverlay = (uint)CharacterOverlay;
            header->_soundBank = _soundBank;
            header->_effectBank = _effectBank;
            header->_flags = (ushort)_flags;
            header->_stageType = _stageType;
            header->_subStageRange = _subStageRange;

            uint curStrOffset = 0x0;
            if (!string.IsNullOrEmpty(TrackList))
            {
                header->_trackListOffset = curStrOffset;
                curStrOffset += (uint)TrackList.UTF8Length() + 1;
            }
            else
            {
                header->_trackListOffset = 0xFFFFFFFF;
            }
            if (!string.IsNullOrEmpty(StageName))
            {
                header->_stageNameOffset = curStrOffset;
                curStrOffset += (uint)StageName.UTF8Length() + 1;
            }
            else
            {
                header->_stageNameOffset = 0xFFFFFFFF;
            }
            if (!string.IsNullOrEmpty(Module))
            {
                header->_moduleNameOffset = curStrOffset;
                curStrOffset += (uint)Module.UTF8Length() + 1;
            }
            else
            {
                header->_moduleNameOffset = 0xFFFFFFFF;
            }
            header->_memoryAllocation = _memoryAllocation;
            header->_wildSpeed = _wildSpeed;

            uint offset = STEX.HeaderSize;
            foreach (ResourceNode n in Children)
            {
                buint* ptr = (buint*) (address + offset);
                ptr[0] = curStrOffset;
                curStrOffset += (uint)n.Name.UTF8Length() + 1;
                offset += 4;
            }

            if (TrackList?.UTF8Length() > 0)
            {
                offset += address.WriteUTF8String(TrackList, true, offset);
            }
            if (StageName?.UTF8Length() > 0)
            {
                offset += address.WriteUTF8String(StageName, true, offset);
            }
            if (Module?.UTF8Length() > 0)
            {
                offset += address.WriteUTF8String(Module, true, offset);
            }

            foreach (ResourceNode n in Children)
            {
                offset += address.WriteUTF8String(n.Name, true, offset);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return source.Tag == "STEX" ? new STEXNode() : null;
        }
    }
}
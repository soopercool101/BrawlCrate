using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class STEXNode : ResourceNode
    {
        internal STEX* Header => (STEX*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.STEX;
        [Browsable(false)] public override bool AllowDuplicateNames => true;

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
                _module = value.Substring(0, value.Length > 32 ? 32 : value.Length);
                while (_module.UTF8Length() > 32)
                {
                    _module = value.Substring(0, _module.Length - 1);
                }
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
        [TypeConverter(typeof(HexUShortConverter))]
        public ushort SoundBank
        {
            get => _soundBank;
            set
            {
                _soundBank = value;
                SignalPropertyChange();
            }
        }

        private ushort _effectBank;
        [Category("Stage Parameters")]
        [TypeConverter(typeof(HexUShortConverter))]
        public ushort EffectBank
        {
            get => _effectBank;
            set
            {
                _effectBank = value;
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
        [TypeConverter(typeof(HexUIntConverter))]
        public uint MemoryAllocation
        {
            get => _memoryAllocation;
            set
            {
                _memoryAllocation = value;
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

            List<string> names = new List<string>();
            foreach (ResourceNode n in Children)
            {
                if (names.Contains(n.Name))
                {
                    continue;
                }
                names.Add(n.Name);
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
            header->_rgbaOverlay = ((uint)_rgbaOverlay).Reverse();
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
            Dictionary<string, uint> names = new Dictionary<string, uint>();
            foreach (ResourceNode n in Children)
            {
                buint* ptr = (buint*) (address + offset);
                offset += 4;
                if (names.ContainsKey(n.Name))
                {
                    ptr[0] = names[n.Name];
                    continue;
                }
                ptr[0] = curStrOffset;
                names.Add(n.Name, curStrOffset);
                curStrOffset += (uint)n.Name.UTF8Length() + 1;
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

            foreach (var name in names)
            {
                offset += address.WriteUTF8String(name.Key, true, offset);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "STEX" ? new STEXNode() : null;
        }
    }
}
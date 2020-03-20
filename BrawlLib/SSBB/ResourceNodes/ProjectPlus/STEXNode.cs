using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using BrawlLib.SSBB.Types.Subspace;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class STEXNode : ResourceNode
    {
        internal STEX* Header => (STEX*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.STEX;


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

        private List<string> _substages = new List<string>();

        public List<string> Substages
        {
            get => _substages;
            set
            {
                _substages = value;
                SignalPropertyChange();
            }
        }

        public RGBAPixel _rgbaOverlay;

        public RGBAPixel CharacterOverlay
        {
            get => _rgbaOverlay;
            set
            {
                _rgbaOverlay = value;
                SignalPropertyChange();
            }
        }


        public ushort _soundBank;
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

        public ushort _effectBank;
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

        public StageFlags _flags;

        public StageFlags Flags
        {
            get => _flags;
            set
            {
                _flags = value;
                SignalPropertyChange();
            }
        }

        public byte _stageType;

        public VariantType SubstageVarianceType
        {
            get => (VariantType)_stageType;
            set
            {
                _stageType = (byte)value;
                SignalPropertyChange();
            }
        }

        public byte _subStageRange;
        public byte SubstageRange
        {
            get => _subStageRange;
            set
            {
                _subStageRange = value;
                SignalPropertyChange();
            }
        }

        public float _wildSpeed;
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
                Children.Add(new RawDataNode { _name = Header->subStageName(i) });
            }

            for (; i < Header->_subStageRange; i++)
            {
                Children.Add(new RawDataNode { _name = Children.Count.ToString() });
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
            _wildSpeed = Header->_wildSpeed;
            for (int i = 0; i < Header->subStageCount; i++)
            {
                _substages.Add(Header->subStageName(i));
            }

            return Header->subStageCount > 0 || Header->_subStageRange > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (int)STEX.HeaderSize;

            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return source.Tag == "STEX" ? new STEXNode() : null;
        }
    }
}
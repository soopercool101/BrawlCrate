using BrawlLib.CustomLists;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.SSEEX;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.SSEEX
{
    public unsafe class SELCNode : ResourceNode
    {
        internal SELC* Header => (SELC*)WorkingUncompressed.Address;

        public override Type[] AllowedChildTypes => new[] { typeof(SELCTeamNode) };
        public override bool supportsCompression => false;

        public int Entries => Children.Sum(c => c.Children?.Count ?? 0);

        public byte _characterCount;
        public byte CharacterCount
        {
            get => _characterCount;
            set
            {
                _characterCount = value.Clamp(0, 10);
                SignalPropertyChange();
            }
        }

        public sbyte _stockCount;
        public sbyte StockCount
        {
            get => _stockCount;
            set
            {
                _stockCount = value.Clamp(-1, 10);
                SignalPropertyChange();
            }
        }


        public enum SSEEXUnlockSettings : byte
        {
            FullUnlockedRoster = 0,
            SELCUnlockedRoster = 1,
            SELCCompleteRoster = 2
        }
        public SSEEXUnlockSettings _unlockSetting;
        public SSEEXUnlockSettings UnlockSetting
        {
            get => _unlockSetting;
            set
            {
                _unlockSetting = value;
                SignalPropertyChange();
            }
        }

        public bool _disableSubfighterSelection;
        public bool DisableSubfighterSelection
        {
            get => _disableSubfighterSelection;
            set
            {
                _disableSubfighterSelection = value;
                SignalPropertyChange();
            }
        }

        public bool _teamAffectsSublevel;
        public bool TeamAffectsSublevel
        {
            get => _teamAffectsSublevel;
            set
            {
                _teamAffectsSublevel = value;
                SignalPropertyChange();
            }
        }

        public byte _randomCharacters;
        public byte RandomCharacters
        {
            get => _randomCharacters;
            set
            {
                _randomCharacters = value;
                SignalPropertyChange();
            }
        }


        public byte _minimumUnlocks;
        public byte MinimumUnlocks
        {
            get => _minimumUnlocks;
            set
            {
                _minimumUnlocks = value;
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            var currentOffset = 0;
            DataSource source;
            for (int i = 0; i < 8; i++)
            {
                var teamCount = Header->TeamXCount((byte)i);
                source = new DataSource((*Header)[currentOffset], teamCount);
                currentOffset += teamCount;
                new SELCTeamNode {_name = $"Team {i+1}"}.Initialize(this, source);
            }
            source = new DataSource((*Header)[currentOffset], WorkingUncompressed.Length - (currentOffset + SELC.Size));
            new SELCTeamNode { _name = "No Team (Extra for random)" }.Initialize(this, source);
        }

        public override bool AllowDuplicateNames => true;

        public override bool OnInitialize()
        {
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            _characterCount = Header->_characterCount;
            _stockCount = Header->_stockCount;
            _unlockSetting = (SSEEXUnlockSettings) Header->_unlockSetting;
            _disableSubfighterSelection = Header->_disableSubfighterSelection == 1;
            _teamAffectsSublevel = Header->_teamAffectsSublevel == 1;
            _randomCharacters = Header->_randomCharacters;
            _minimumUnlocks = Header->_minimumUnlocks;
            return true;
        }

        public override int OnCalculateSize(bool force)
        {
            return SELC.Size + Children.Sum(n => n.OnCalculateSize(force));
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SELC* header = (SELC*)address;
            *header = new SELC();
            header->_characterCount = CharacterCount;
            header->_stockCount = StockCount;
            header->_unlockSetting = (byte) _unlockSetting;
            header->_disableSubfighterSelection = (byte)(_disableSubfighterSelection ? 1 : 0);
            header->_teamAffectsSublevel = (byte)(_teamAffectsSublevel ? 1 : 0);
            header->_randomCharacters = _randomCharacters;
            header->_minimumUnlocks = _minimumUnlocks;
            header->_pad0x7 = 0;
            header->_team1Count = (byte)Children[0].Children.Count;
            header->_team2Count = (byte)Children[1].Children.Count;
            header->_team3Count = (byte)Children[2].Children.Count;
            header->_team4Count = (byte)Children[3].Children.Count;
            header->_team5Count = (byte)Children[4].Children.Count;
            header->_team6Count = (byte)Children[5].Children.Count;
            header->_team7Count = (byte)Children[6].Children.Count;
            header->_team8Count = (byte)Children[7].Children.Count;
            
            uint offset = SELC.Size;
            foreach (ResourceNode n in Children)
            {
                int size = n.CalculateSize(true);
                n.Rebuild(address + offset, size, true);
                offset += (uint)size;
            }
        }
    }

    public unsafe class SELCTeamNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.SELCTeam;
        public override Type[] AllowedChildTypes => new[] { typeof(SELCTeamNode) };
        public override bool supportsCompression => false;
        public override int OnCalculateSize(bool force)
        {
            return Children.Count;
        }
        public override bool OnInitialize()
        {
            return WorkingUncompressed.Length > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < WorkingUncompressed.Length; i++)
            {
                DataSource source = new DataSource(WorkingUncompressed.Address + i, 1);
                new SELCEntryNode().Initialize(this, source);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            uint offset = 0;
            foreach (ResourceNode n in Children)
            {
                int size = n.CalculateSize(true);
                n.Rebuild(address + offset, size, true);
                offset += (uint)size;
            }
        }
    }

    public unsafe class SELCEntryNode : ResourceNode
    {
        public byte _cssID;
        [DisplayName("CSS ID")]
        [TypeConverter(typeof(DropDownListBrawlExCSSIDs))]
        public byte CSSID
        {
            get => _cssID;
            set
            {
                _cssID = value;
                Name = FighterNameGenerators.FromID(value, FighterNameGenerators.cssSlotIDIndex, "+S");
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 1;
        }

        public override bool OnInitialize()
        {
            _cssID = WorkingUncompressed.Address.Byte;
            _name = FighterNameGenerators.FromID(_cssID, FighterNameGenerators.cssSlotIDIndex, "+S");
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(byte*)(address) = _cssID;
        }
    }
}
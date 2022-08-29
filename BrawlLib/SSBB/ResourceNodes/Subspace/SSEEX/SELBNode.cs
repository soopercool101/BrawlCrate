using BrawlLib.CustomLists;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.SSEEX;
using System;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.SSEEX
{
    public unsafe class SELBNode : ResourceNode
    {
        internal SELB* Header => (SELB*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SELB;

        public override Type[] AllowedChildTypes => new[] { typeof(SELBEntryNode) };

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
        [Description(@"Number of stocks for players. Special values:
-1: Use the character count
-2: Keep previous stock count
-3: Make stock count = character count + previous stock count
-4: Make character count = character count + previous stock count
-5: Make character and stock count = character count + previous stock count")]
        public sbyte StockCount
        {
            get => _stockCount;
            set
            {
                _stockCount = value.Clamp(-5, 10);
                SignalPropertyChange();
            }
        }
        
        public SELCNode.SublevelChanges _sublevelChanger;
        public SELCNode.SublevelChanges SublevelChanger
        {
            get => _sublevelChanger;
            set
            {
                _sublevelChanger = value;
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            var maxEntryCount = Math.Floor((double)(WorkingUncompressed.Length - SELB.Size) / SELBEntry.Size);
            for (int i = 0; i < maxEntryCount; i++)
            {
                DataSource source = new DataSource((*Header)[i], SELBEntry.Size);
                if (source.Address.Byte == 0xFF)
                    break;
                new SELBEntryNode().Initialize(this, source);
            }
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
            return WorkingUncompressed.Length > SELB.Size;
        }

        public override int OnCalculateSize(bool force)
        {
            return SELB.Size + Children.Count * SELBEntry.Size + 1;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SELB* header = (SELB*)address;
            *header = new SELB();
            header->_characterCount = CharacterCount;
            header->_stockCount = StockCount;
            header->_sublevelChanger = (byte) SublevelChanger;
            header->_pad = 0;

            uint offset = SELB.Size;
            foreach (ResourceNode n in Children)
            {
                int size = n.CalculateSize(true);
                n.Rebuild(address + offset, size, true);
                offset += (uint)size;
            }

            *(byte*) (address + offset) = 0xFF;
        }
    }

    public unsafe class SELBEntryNode : ResourceNode
    {
        internal SELBEntry* Header => (SELBEntry*)WorkingUncompressed.Address;
        public override bool AllowDuplicateNames => true;
        public override bool supportsCompression => false;

        public byte _cssID;
        [DisplayName("CSS ID")]
        [TypeConverter(typeof(DropDownListBrawlExCSSIDs))]
        public byte CSSID
        {
            get => _cssID;
            set
            {
                _cssID = value;
                Name = FighterNameGenerators.FromID(value, FighterNameGenerators.cssSlotIDIndex,  "+S");
                SignalPropertyChange();
            }
        }

        public float _cursorX;
        public float CursorX
        {
            get => _cursorX;
            set
            {
                _cursorX = value;
                SignalPropertyChange();
            }
        }

        public float _cursorY;
        public float CursorY
        {
            get => _cursorY;
            set
            {
                _cursorY = value;
                SignalPropertyChange();
            }
        }

        public float _nameX;
        public float NameX
        {
            get => _nameX;
            set
            {
                _nameX = value;
                SignalPropertyChange();
            }
        }

        public float _nameY;
        public float NameY
        {
            get => _nameY;
            set
            {
                _nameY = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return (int)SELBEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SELBEntry* header = (SELBEntry*)address;
            *header = new SELBEntry();
            header->_cssID = _cssID;
            header->_pad = new BInt24(0);
            header->_cursorX = _cursorX;
            header->_cursorY = _cursorY;
            header->_nameX = _nameX;
            header->_nameY = _nameY;
        }

        public override bool OnInitialize()
        {
            //CSS
            _cssID = Header->_cssID;
            _name = FighterNameGenerators.FromID(_cssID, FighterNameGenerators.cssSlotIDIndex, "+S");
            _cursorX = Header->_cursorX;
            _cursorY = Header->_cursorY;
            _nameX = Header->_nameX;
            _nameY = Header->_nameY;
            return false;
        }
    }
}
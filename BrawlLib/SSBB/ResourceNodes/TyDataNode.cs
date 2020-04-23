using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TyDataNode : ResourceNode
    {
        internal TyDataHeader* Header => (TyDataHeader*)WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = FileName;
            }
            return true;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_entries; i++)
            {
                int length;
                if (i < Header->_entries - 1)
                {
                    length = (int) ((*Header)[i + 1].UInt - (*Header)[i].UInt);
                }
                else
                {
                    length = (int) (Header->_dataOffset - (*Header)[i].UInt);
                }

                string name = Header->GetEntryName(i);

                ResourceNode node;

                switch (name)
                {
                    case "tySealList":
                        node = new TySealList();
                        break;
                    default:
                        node = new RawDataNode();
                        break;
                }
                node._name = name;
                node.Initialize(this, Header->GetEntry(i), length);
            }
        }

        internal static ResourceNode TryParseGeneric(DataSource source, ResourceNode parent)
        {
            VoidPtr addr = source.Address;
            TyDataHeader* header = (TyDataHeader*)addr;

            if (header->_size != source.Length || header->_pad1 != 0 || header->_pad2 != 0 || header->_pad3 != 0 || header->_pad4 != 0 || header->_dataOffset > source.Length || header->_dataOffset + header->_dataEntries * 4 > source.Length)
            {
                return null;
            }

            return new TyDataNode();
        }
    }

    public unsafe class TySealList : ResourceNode
    {
        public override bool OnInitialize()
        {
            return WorkingUncompressed.Length >= 100;
        }

        public override void OnPopulate()
        {
            TyDataHeader* parentData = (TyDataHeader*)Parent.WorkingUncompressed.Address;
            uint offset = 0;
            while (offset + 100 <= WorkingUncompressed.Length)
            {
                //StickerEntry* e = (StickerEntry*) parentData->GetStickerEntry(Children.Count);
                new TySealNode().Initialize(this, WorkingUncompressed.Address + offset, 100);
                offset += 100;
            }
        }
    }

    public class StickerData
    {
        public string InternalName;
        public string BrresName;
    }

    public unsafe class TySealNode : ResourceNode
    {
        internal TySeal* Header => (TySeal*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Sticker;

        internal string _brres;

        [Category("Sticker Data")]
        public string BRRES
        {
            get => _brres;
            set
            {
                _brres = value;
                SignalPropertyChange();
            }
        }


        internal int _id;

        [Category("Sticker Data")]
        public int ID
        {
            get => _id;
            set
            {
                _id = value;
                SignalPropertyChange();
            }
        }

        public int _unknown0x0C;

        [Category("Sticker Data")]
        public int Unknown0x0C
        {
            get => _unknown0x0C;
            set
            {
                _unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        public int _unknown0x10;

        [Category("Sticker Data")]
        public int Unknown0x10
        {
            get => _unknown0x10;
            set
            {
                _unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        public int _unknown0x14;


        [Category("Sticker Data")]
        public int Unknown0x14
        {
            get => _unknown0x14;
            set
            {
                _unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        public short _order1;

        [Category("Sticker Data")]
        public short Order1
        {
            get => _order1;
            set
            {
                _order1 = value;
                SignalPropertyChange();
            }
        }

        public short _order2;

        [Category("Sticker Data")]
        public short Order2
        {
            get => _order2;
            set
            {
                _order2 = value;
                SignalPropertyChange();
            }
        }

        public short _unknown0x20;

        [Category("Sticker Data")]
        public short Unknown0x20
        {
            get => _unknown0x20;
            set
            {
                _unknown0x20 = value;
                SignalPropertyChange();
            }
        }

        public long _characterFlags;

        [Category("Sticker Data")]
        public long CharacterFlags
        {
            get => _characterFlags;
            set
            {
                _characterFlags = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            VoidPtr tydata = Parent.Parent.WorkingUncompressed.Address;
            _id = Header->_id;
            _name = (tydata + TyDataHeader.HeaderSize).GetUTF8String(Header->_nameOffset);
            _brres = (tydata + TyDataHeader.HeaderSize).GetUTF8String(Header->_brresOffset);
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x14 = Header->_unknown0x14;
            _order1 = Header->_order1;
            _order2 = Header->_order2;
            _unknown0x20 = Header->_unknown0x20;
            _characterFlags = Header->_characterFlags;
            return false;
        }
    }
}

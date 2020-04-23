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

        public short _alphabeticalOrder;

        [Category("Sticker Data")]
        public short AlphabeticalOrder
        {
            get => _alphabeticalOrder;
            set
            {
                _alphabeticalOrder = value;
                SignalPropertyChange();
            }
        }

        public short _unknown0x18;

        [Category("Sticker Data")]
        public short Unknown0x18
        {
            get => _unknown0x18;
            set
            {
                _unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        public short _unknown0x1A;

        [Category("Sticker Data")]
        public short Unknown0x1A
        {
            get => _unknown0x1A;
            set
            {
                _unknown0x1A = value;
                SignalPropertyChange();
            }
        }

        public CharacterStickerFlags _characterFlags;

        [Category("Sticker Data")]
        public CharacterStickerFlags CharacterFlags
        {
            get => _characterFlags;
            set
            {
                _characterFlags = value;
                SignalPropertyChange();
            }
        }

        [Category("Sticker Data")] public string CharacterLoadLong => ((long) _characterFlags).ToString("X16");

        [Flags]
        public enum CharacterStickerFlags : long
        {
            Mario = 0x0000000100000000,
            DonkeyKong = 0x0000000200000000,
            Link = 0x0000000400000000,
            SamusZSS = 0x0000000800000000,
            Yoshi = 0x0000002000000000,
            Kirby = 0x0000004000000000,
            Fox = 0x0000008000000000,
            Pikachu = 0x0000010000000000,
            Luigi = 0x0000020000000000,
            CaptainFalcon = 0x0000040000000000,
            Ness = 0x0000080000000000,
            Bowser = 0x0000100000000000,
            Peach = 0x0000200000000000,
            Zelda = 0x0000400000000000,
            Marth = 0x0008000000000000,
            IceClimbers = 0x0001000000000000,
            MrGameAndWatch = 0x0010000000000000,
            Falco = 0x0020000000000000,
            Ganondorf = 0x0040000000000000,
            Wario = 0x0080000000000000,
            MetaKnight = 0x0100000000000000,
            Pit = 0x0200000000000000,
            Olimar = 0x0400000000000000,
            Lucas = 0x0800000000000000,
            DiddyKong = 0x1000000000000000,
            PokemonTrainer = 0x2000000000000000,
            Dedede = 0x0000000000000008,
            Lucario = 0x0000000000000010,
            Ike = 0x0000000000000020,
            ROB = 0x0000000000000040,
            Jigglypuff = 0x0000000000000080,
            ToonLink = 0x0000000000000100,
            Wolf = 0x0000000000000200,
            Snake = 0x0000000000000400,
            Sonic = 0x0000000000000800,
            AllCharacters = 0x3FF97FEF00000FF8
        }

        public override bool OnInitialize()
        {
            VoidPtr tydata = Parent.Parent.WorkingUncompressed.Address;
            _id = Header->_id;
            _name = (tydata + TyDataHeader.HeaderSize).GetUTF8String(Header->_nameOffset);
            _brres = (tydata + TyDataHeader.HeaderSize).GetUTF8String(Header->_brresOffset);
            _unknown0x0C = Header->_unknown0x0C;
            _unknown0x10 = Header->_unknown0x10;
            _order1 = Header->_order1;
            _alphabeticalOrder = Header->_alphabeticalOrder;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x1A = Header->_unknown0x1A;
            _characterFlags = (CharacterStickerFlags)(long)Header->_characterFlags;
            return false;
        }
    }
}

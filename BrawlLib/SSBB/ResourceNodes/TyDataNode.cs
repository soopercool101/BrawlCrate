using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TyDataNode : ARCEntryNode
    {
        internal TyDataHeader* Header => (TyDataHeader*)WorkingUncompressed.Address;
        public override Type[] AllowedChildTypes => new[] {typeof(TyEntryNode)};

#if !DEBUG
        [Browsable(false)]
#endif
        public List<string> DataEntries { get; set; } = new List<string>();

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
            for (int i = 0; i < Header->_dataEntries; i++)
            {
                DataEntries.Add("0x" + Header->GetDataEntryOffset(i).UInt.ToString("X8"));
            }

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
                    case "tySealVertData":
                        node = new TySealVertDataNode();
                        break;
                    case "tySealVertList":
                        node = new TySealVertListNode();
                        break;
                    case "tyDataList":
                        node = new TyDataListNode();
                        break;
                    default:
                        node = new TyEntryNode();
                        break;
                }
                node._name = name;
                node.Initialize(this, Header->GetEntry(i), length);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            VoidPtr addr = source.Address;
            TyDataHeader* header = (TyDataHeader*)addr;

            if (header->_size != source.Length || header->_pad1 != 0 || header->_pad2 != 0 || header->_pad3 != 0 ||
                header->_pad4 != 0 || header->_dataOffset > source.Length || header->_entries <= 0 ||
                header->_dataOffset + header->_dataEntries * 4 > source.Length)
            {
                return null;
            }

            return header->GetEntryName(0).StartsWith("ty") ? new TyDataNode() : null;
        }

        public override int OnCalculateSize(bool force)
        {
            int sizeCalc = (int)TyDataHeader.HeaderSize;
            foreach (TyEntryNode t in Children)
            {
                sizeCalc += (int)t.GetStringsLength();
                sizeCalc += (int)(t.GetDataEntryCount() * 4);
                sizeCalc += t.OnCalculateSize(true);
                sizeCalc += (int)TyEntry.Size;
                sizeCalc += t.Name.UTF8Length() + 1;
            }
            return sizeCalc;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TyDataHeader* header = (TyDataHeader*)address;
            *header = new TyDataHeader();
            header->_size = (uint) length;
            uint dataEntries = 0;
            // Calculate how many data entries there are
            foreach (TyEntryNode t in Children)
            {
                dataEntries += t.GetDataEntryCount();
            }
            header->_dataEntries = dataEntries;
            uint offset = TyDataHeader.HeaderSize;
            // Write string table
            foreach (TyEntryNode t in Children)
            {
                offset = t.WriteStrings(address, offset);
            }
            // Write each child
            foreach (TyEntryNode n in Children)
            {
                n.EntryOffset = offset;
                int size = n.OnCalculateSize(true);
                n.OnRebuild(address + offset, size, true);
                offset += (uint)size;
            }
            // Write data section
            header->_dataOffset = offset - TyDataHeader.HeaderSize;
            foreach (TyEntryNode t in Children)
            {
                offset = t.WriteData(address, offset);
            }
            // Write entries
            header->_entries = (uint)Children.Count;
            uint strOffset = 0;
            foreach (TyEntryNode t in Children)
            {
                TyEntry* entry = (TyEntry*)(address + offset);
                entry->_offset = t.EntryOffset - TyDataHeader.HeaderSize;
                entry->_strOffset = strOffset;
                strOffset += (uint)t.Name.UTF8Length() + 1;
                offset += TyEntry.Size;
            }
            // Write names
            foreach (TyEntryNode t in Children)
            {
                (address + offset).WriteUTF8String(t.Name, true);
                offset += (uint)t.Name.UTF8Length() + 1;
            }

            header->_pad1 = 0;
            header->_pad2 = 0;
            header->_pad3 = 0;
            header->_pad4 = 0;
        }
    }

    public class TyEntryNode : ResourceNode
    {
        public override bool supportsCompression => false;

        public virtual uint WriteStrings(VoidPtr address, uint initialOffset)
        {
            return initialOffset;
        }

        public virtual uint GetStringsLength()
        {
            return 0;
        }

        public virtual uint GetDataEntryCount()
        {
            return 0;
        }

        internal uint EntryOffset;
        public virtual uint WriteData(VoidPtr address, uint initialOffset)
        {
            return initialOffset;
        }
    }

    public unsafe class TySealList : TyEntryNode
    {
        public override Type[] AllowedChildTypes => new[] {typeof(TySealNode)};

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
                new TySealNode().Initialize(this, WorkingUncompressed.Address + offset, 100);
                offset += 100;
            }
        }

        public override uint WriteStrings(VoidPtr address, uint initialOffset)
        {
            uint offset = initialOffset;
            foreach(TySealNode sticker in Children)
            {
                if ((sticker.Name == "<null>" || string.IsNullOrEmpty(sticker.Name)) &&
                    string.IsNullOrEmpty(sticker.BRRES) || sticker.Name.Equals(sticker.BRRES))
                {
                    uint lengthCalc = (uint)(sticker.BRRES.UTF8Length() + 1).Align(4);
                    address.WriteUTF8String(sticker.BRRES, true, offset, lengthCalc);
                    sticker.NameOffset = offset - TyDataHeader.HeaderSize;
                    sticker.BrresOffset = offset - TyDataHeader.HeaderSize;
                    offset += lengthCalc + 4;
                }
                else
                {
                    uint lengthCalc = (uint)(sticker.Name.UTF8Length() + 1).Align(4);
                    address.WriteUTF8String(sticker.Name, true, offset, lengthCalc);
                    sticker.NameOffset = offset - TyDataHeader.HeaderSize;
                    offset += lengthCalc;
                    lengthCalc = (uint)(sticker.BRRES.UTF8Length() + 1).Align(4);
                    address.WriteUTF8String(sticker.BRRES, true, offset, lengthCalc);
                    sticker.BrresOffset = offset - TyDataHeader.HeaderSize;
                    offset += lengthCalc;
                }
            }

            return offset;
        }

        public override uint GetStringsLength()
        {
            uint sizeCalc = 0;
            foreach (TySealNode sticker in Children)
            {
                if ((sticker.Name == "<null>" || string.IsNullOrEmpty(sticker.Name)) &&
                    string.IsNullOrEmpty(sticker.BRRES) || sticker.Name.Equals(sticker.BRRES))
                {
                    uint lengthCalc = (uint)(sticker.BRRES.UTF8Length() + 1).Align(4);
                    sizeCalc += lengthCalc + 4;
                }
                else
                {
                    uint lengthCalc = (uint)(sticker.Name.UTF8Length() + 1).Align(4);
                    sizeCalc += lengthCalc;
                    lengthCalc = (uint)(sticker.BRRES.UTF8Length() + 1).Align(4);
                    sizeCalc += lengthCalc;
                }
            }
            return sizeCalc;
        }

        public override uint GetDataEntryCount()
        {
            return (uint) (Children.Count * 2);
        }

        public override uint WriteData(VoidPtr address, uint initialOffset)
        {
            uint offset = initialOffset;
            foreach (TySealNode s in Children)
            {
                ((buint*)(address + offset))[0] = s.Offset - 0x1C;
                offset += 4;
                ((buint*)(address + offset))[0] = (s.Offset - 0x1C) + 4;
                offset += 4;
            }

            return offset;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int offset = 0;
            foreach (TySealNode n in Children)
            {
                int size = n.OnCalculateSize(true);
                n.OnRebuild(address + offset, size, true);
                n.Offset = EntryOffset + (uint)offset;
                offset += size;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0;
            foreach (ResourceNode n in Children)
            {
                size += n.OnCalculateSize(true);
            }

            return size;
        }
    }

    public unsafe class TySealNode : ResourceNode
    {
        internal TySeal* Header => (TySeal*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Sticker;


        private string _brres;

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


        private int _id;

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

        private int _unknown0x0C;

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

        private int _unknown0x10;

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


        private short _unknown0x14;

        [Category("Sticker Data")]
        public short Unknown0x14
        {
            get => _unknown0x14;
            set
            {
                _unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        private short _alphabeticalOrder;

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

        private short _unknown0x18;

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

        private short _unknown0x1A;

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

        private CharacterStickerFlags _characterFlags;

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

        private int _unknown0x24;

        [Category("Sticker Data")]
        public int Unknown0x24
        {
            get => _unknown0x24;
            set
            {
                _unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        private uint _unknown0x28;

        [Category("Sticker Data")]
        public uint Unknown0x28
        {
            get => _unknown0x28;
            set
            {
                _unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        private int _effectType;

        [Category("Sticker Data")]
        public int EffectType
        {
            get => _effectType;
            set
            {
                _effectType = value;
                SignalPropertyChange();
            }
        }

        private float _effectStrength;

        [Category("Sticker Data")]
        public float EffectStrength
        {
            get => _effectStrength;
            set
            {
                _effectStrength = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x34;

        [Category("Sticker Data")]
        public float Unknown0x34
        {
            get => _unknown0x34;
            set
            {
                _unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x38;

        [Category("Sticker Data")]
        public float Unknown0x38
        {
            get => _unknown0x38;
            set
            {
                _unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x3C;

        [Category("Padding?")]
        public int Pad0x3C
        {
            get => _pad0x3C;
            set
            {
                _pad0x3C = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x40;

        [Category("Padding?")]
        public int Pad0x40
        {
            get => _pad0x40;
            set
            {
                _pad0x40 = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x44;

        [Category("Padding?")]
        public int Pad0x44
        {
            get => _pad0x44;
            set
            {
                _pad0x44 = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x48;

        [Category("Padding?")]
        public int Pad0x48
        {
            get => _pad0x48;
            set
            {
                _pad0x48 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x4C;

        [Category("Sticker Data")]
        public float Unknown0x4C
        {
            get => _unknown0x4C;
            set
            {
                _unknown0x4C = value;
                SignalPropertyChange();
            }
        }

        private short _textureWidth;

        [Category("Sticker Data")]
        public short TextureWidth
        {
            get => _textureWidth;
            set
            {
                _textureWidth = value;
                _textureWidthSmall = (short)Math.Ceiling(value / 2.0f);
                SignalPropertyChange();
            }
        }

        private short _textureLength;

        [Category("Sticker Data")]
        public short TextureLength
        {
            get => _textureLength;
            set
            {
                _textureLength = value;
                _textureLengthSmall = (short) Math.Ceiling(value / 2.0f);
                SignalPropertyChange();
            }
        }

        private short _textureWidthSmall;

        [Category("Sticker Data")]
        public short TextureWidthSmall
        {
            get => _textureWidthSmall;
            set
            {
                _textureWidthSmall = value;
                SignalPropertyChange();
            }
        }

        private short _textureLengthSmall;

        [Category("Sticker Data")]
        public short TextureLengthSmall
        {
            get => _textureLengthSmall;
            set
            {
                _textureLengthSmall = value;
                SignalPropertyChange();
            }
        }

        private int _sizeOrder;

        [Category("Sticker Data")]
        public int SizeOrder
        {
            get => _sizeOrder;
            set
            {
                _sizeOrder = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x5C;

        [Category("Padding?")]
        public int Pad0x5C
        {
            get => _pad0x5C;
            set
            {
                _pad0x5C = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x60;

        [Category("Padding?")]
        public int Pad0x60
        {
            get => _pad0x60;
            set
            {
                _pad0x60 = value;
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
            _alphabeticalOrder = Header->_alphabeticalOrder;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x1A = Header->_unknown0x1A;
            _characterFlags = (CharacterStickerFlags)(long)Header->_characterFlags;
            _unknown0x24 = Header->_unknown0x24;
            _unknown0x28 = Header->_unknown0x28;
            _effectType = Header->_effectType;
            _effectStrength = Header->_effectStrength;
            _unknown0x34 = Header->_unknown0x34;
            _unknown0x38 = Header->_unknown0x38;
            _pad0x40 = Header->_pad0x40;
            _pad0x40 = Header->_pad0x40;
            _pad0x44 = Header->_pad0x44;
            _pad0x48 = Header->_pad0x48;
            _unknown0x4C = Header->_unknown0x4C;
            _textureWidthSmall = Header->_textureWidthSmall;
            _textureLengthSmall = Header->_textureLengthSmall;
            _textureWidth = Header->_textureWidth;
            _textureLength = Header->_textureLength;
            _sizeOrder = Header->_sizeOrder;
            _pad0x5C = Header->_pad0x5C;
            _pad0x60 = Header->_pad0x60;
            return false;
        }

        internal uint NameOffset;
        internal uint BrresOffset;
        internal uint Offset;
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TySeal* header = (TySeal*)address;
            *header = new TySeal();
            header->_id = _id;
            header->_nameOffset = NameOffset;
            header->_brresOffset = BrresOffset;
            header->_unknown0x0C = _unknown0x0C;
            header->_unknown0x10 = _unknown0x10;
            header->_unknown0x14 = _unknown0x14;
            header->_alphabeticalOrder = _alphabeticalOrder;
            header->_unknown0x18 = _unknown0x18;
            header->_unknown0x1A = _unknown0x1A;
            header->_characterFlags = (long)_characterFlags;
            header->_unknown0x24 = _unknown0x24;
            header->_unknown0x28 = _unknown0x28;
            header->_effectType = _effectType;
            header->_effectStrength = _effectStrength;
            header->_unknown0x34 = _unknown0x34;
            header->_unknown0x38 = _unknown0x38;
            header->_pad0x3C = _pad0x3C;
            header->_pad0x40 = _pad0x40;
            header->_pad0x44 = _pad0x44;
            header->_pad0x48 = _pad0x48;
            header->_unknown0x4C = _unknown0x4C;
            header->_textureWidthSmall = _textureWidthSmall;
            header->_textureLengthSmall = _textureLengthSmall;
            header->_textureWidth = _textureWidth;
            header->_textureLength = _textureLength;
            header->_sizeOrder = _sizeOrder;
            header->_pad0x5C = _pad0x5C;
            header->_pad0x60 = _pad0x60;
        }

        public override int OnCalculateSize(bool force)
        {
            return TySeal.Size;
        }
    }

    public unsafe class TySealVertDataNode : TyEntryNode
    {
        internal TySealVertData* Header => (TySealVertData*)WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            return Header->_entries > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_entries; i++)
            {
                new TySealVertDataEntryNode { _name = $"Entry [{i}]" }.Initialize(this, (*Header)[i], (int)TySealVertDataEntry.Size);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (int)TySealVertData.HeaderSize;
            foreach (ResourceNode n in Children)
            {
                size += n.OnCalculateSize(true);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TySealVertData* header = (TySealVertData*)address;
            *header = new TySealVertData();
            header->_entries = (uint)Children.Count;
            uint offset = TySealVertData.HeaderSize;
            foreach (ResourceNode n in Children)
            {
                int size = n.OnCalculateSize(true);
                n.OnRebuild(address + offset, size, true);
                offset += (uint)size;
            }
        }
    }

    public unsafe class TySealVertDataEntryNode : ResourceNode
    {
        internal TySealVertDataEntry* Header => (TySealVertDataEntry*)WorkingUncompressed.Address;

        private int _unknown0x00;

        public int VertListIndex1
        {
            get => _unknown0x00;
            set
            {
                _unknown0x00 = value;
                SignalPropertyChange();
            }
        }

        private int _unknown0x04;
        public int VertListIndex2
        {
            get => _unknown0x04;
            set
            {
                _unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _unknown0x00 = Header->_unknown0x00;
            _unknown0x04 = Header->_unknown0x04;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return (int)TySealVertDataEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TySealVertDataEntry* header = (TySealVertDataEntry*)address;
            *header = new TySealVertDataEntry();
            header->_unknown0x00 = _unknown0x00;
            header->_unknown0x04 = _unknown0x04;
        }
    }

    public unsafe class TyDataListNode : TyEntryNode
    {
        public override Type[] AllowedChildTypes => new[] {typeof(TyDataListEntryNode)};
        public override ResourceType ResourceFileType => ResourceType.TrophyList;

        public override uint WriteStrings(VoidPtr address, uint initialOffset)
        {
            uint offset = initialOffset;
            foreach (TyDataListEntryNode toy in Children)
            {
                if ((toy.Name == "<null>" || string.IsNullOrEmpty(toy.Name)) &&
                    string.IsNullOrEmpty(toy.BRRES) || toy.Name.Equals(toy.BRRES))
                {
                    uint lengthCalc = (uint)(toy.BRRES.UTF8Length() + 1).Align(4);
                    address.WriteUTF8String(toy.BRRES, true, offset, lengthCalc);
                    toy.NameOffset = offset - TyDataHeader.HeaderSize;
                    toy.BrresOffset = offset - TyDataHeader.HeaderSize;
                    offset += lengthCalc;
                }
                else
                {
                    uint lengthCalc = (uint)(toy.Name.UTF8Length() + 1).Align(4);
                    address.WriteUTF8String(toy.Name, true, offset, lengthCalc);
                    toy.NameOffset = offset - TyDataHeader.HeaderSize;
                    offset += lengthCalc;
                    lengthCalc = (uint)(toy.BRRES.UTF8Length() + 1).Align(4);
                    address.WriteUTF8String(toy.BRRES, true, offset, lengthCalc);
                    toy.BrresOffset = offset - TyDataHeader.HeaderSize;
                    offset += lengthCalc;
                }
            }

            return offset;
        }

        public override uint GetStringsLength()
        {
            uint sizeCalc = 0;
            foreach (TyDataListEntryNode toy in Children)
            {
                if ((toy.Name == "<null>" || string.IsNullOrEmpty(toy.Name)) &&
                    string.IsNullOrEmpty(toy.BRRES) || toy.Name.Equals(toy.BRRES))
                {
                    uint lengthCalc = (uint)(toy.BRRES.UTF8Length() + 1).Align(4);
                    sizeCalc += lengthCalc;
                }
                else
                {
                    uint lengthCalc = (uint)(toy.Name.UTF8Length() + 1).Align(4);
                    sizeCalc += lengthCalc;
                    lengthCalc = (uint)(toy.BRRES.UTF8Length() + 1).Align(4);
                    sizeCalc += lengthCalc;
                }
            }
            return sizeCalc;
        }

        public override uint GetDataEntryCount()
        {
            return (uint)(Children.Count * 2);
        }

        public override uint WriteData(VoidPtr address, uint initialOffset)
        {
            uint offset = initialOffset;
            foreach (TyDataListEntryNode t in Children)
            {
                ((buint*)(address + offset))[0] = t.Offset - 0x1C;
                offset += 4;
                ((buint*)(address + offset))[0] = (t.Offset - 0x1C) + 4;
                offset += 4;
            }

            return offset;
        }

        public override bool OnInitialize()
        {
            return WorkingUncompressed.Length >= TyDataListEntry.Size;
        }

        public override void OnPopulate()
        {
            TyDataHeader* parentData = (TyDataHeader*)Parent.WorkingUncompressed.Address;
            uint offset = 0;
            while (offset + TyDataListEntry.Size <= WorkingUncompressed.Length)
            {
                new TyDataListEntryNode().Initialize(this, WorkingUncompressed.Address + offset, (int)TyDataListEntry.Size);
                offset += TyDataListEntry.Size;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int offset = 0;
            foreach (TyDataListEntryNode n in Children)
            {
                int size = n.OnCalculateSize(true);
                n.OnRebuild(address + offset, size, true);
                n.Offset = EntryOffset + (uint)offset;
                offset += size;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0;
            foreach (ResourceNode n in Children)
            {
                size += n.OnCalculateSize(true);
            }

            return size;
        }
    }

    public unsafe class TyDataListEntryNode : ResourceNode
    {
        internal TyDataListEntry* Header => (TyDataListEntry*)WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Trophy;

        private string _brres;

        [Category("Trophy Data")]
        public string BRRES
        {
            get => _brres;
            set
            {
                _brres = value;
                SignalPropertyChange();
            }
        }

        private int _id;

        [Category("Trophy Data")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                SignalPropertyChange();
            }
        }

        private int _thumbnail;

        [Category("Trophy Data")]
        [Description("The index of the trophy's thumbnail in figure.brres")]
        public int ThumbnailIndex
        {
            get => _thumbnail;
            set
            {
                _thumbnail = value;
                SignalPropertyChange();
            }
        }

        private int _gameIcon1;

        [Category("Trophy Data")]
        [Description("The index of the trophy's first game icon in the PAT0 in figure.brres")]
        public int GameIcon1
        {
            get => _gameIcon1;
            set
            {
                _gameIcon1 = value;
                SignalPropertyChange();
            }
        }

        private int _gameIcon2;

        [Category("Trophy Data")]
        [Description("The index of the trophy's second game icon in the PAT0 in figure.brres")]
        public int GameIcon2
        {
            get => _gameIcon2;
            set
            {
                _gameIcon2 = value;
                SignalPropertyChange();
            }
        }

        private int _nameIndex;

        [Category("Trophy Data")]
        [Description("The index of the trophy's display name in ty_fig_name_list")]
        public int NameIndex
        {
            get => _nameIndex;
            set
            {
                _nameIndex = value;
                SignalPropertyChange();
            }
        }

        private int _gameIndex;

        [Category("Trophy Data")]
        [Description("The index of the trophy's game appearance(s) in ty_fig_name_list")]
        public int GameIndex
        {
            get => _gameIndex;
            set
            {
                _gameIndex = value;
                SignalPropertyChange();
            }
        }

        private int _descriptionIndex;

        [Category("Trophy Data")]
        [Description("The index of the trophy's description in ty_fig_ext_list")]
        public int DescriptionIndex
        {
            get => _descriptionIndex;
            set
            {
                _descriptionIndex = value;
                SignalPropertyChange();
            }
        }

        private int _series;

        [Category("Trophy Data")]
        [Description("The index of the trophy's series in ty_fig_category")]
        public int SeriesIndex
        {
            get => _series;
            set
            {
                _series = value;
                SignalPropertyChange();
            }
        }

        private int _category;

        [Category("Trophy Data")]
        [Description("The index of the trophy's category in ty_fig_category")]
        public int CategoryIndex
        {
            get => _category;
            set
            {
                _category = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x28;

        [Category("Padding?")]
        public int Pad0x28
        {
            get => _pad0x28;
            set
            {
                _pad0x28 = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x2C;

        [Category("Padding?")]
        public int Pad0x2C
        {
            get => _pad0x2C;
            set
            {
                _pad0x2C = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x30;

        [Category("Padding?")]
        public int Pad0x30
        {
            get => _pad0x30;
            set
            {
                _pad0x30 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x34;

        [Category("Trophy Data")]
        public float Unknown0x34
        {
            get => _unknown0x34;
            set
            {
                _unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x38;

        [Category("Trophy Data")]
        public float Unknown0x38
        {
            get => _unknown0x38;
            set
            {
                _unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x3C;

        [Category("Padding?")]
        public int Pad0x3C
        {
            get => _pad0x3C;
            set
            {
                _pad0x3C = value;
                SignalPropertyChange();
            }
        }

        private int _unknown0x40;

        [Category("Trophy Data")]
        public int Unknown0x40
        {
            get => _unknown0x40;
            set
            {
                _unknown0x40 = value;
                SignalPropertyChange();
            }
        }

        private int _unknown0x44;

        [Category("Trophy Data")]
        public int Unknown0x44
        {
            get => _unknown0x44;
            set
            {
                _unknown0x44 = value;
                SignalPropertyChange();
            }
        }

        private int _pad0x48;

        [Category("Padding?")]
        public int Pad0x48
        {
            get => _pad0x48;
            set
            {
                _pad0x48 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x50;

        [Category("Trophy Data")]
        public float Unknown0x50
        {
            get => _unknown0x50;
            set
            {
                _unknown0x50 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x54;

        [Category("Trophy Data")]
        public float Unknown0x54
        {
            get => _unknown0x54;
            set
            {
                _unknown0x54 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x58;

        [Category("Trophy Data")]
        public float Unknown0x58
        {
            get => _unknown0x58;
            set
            {
                _unknown0x58 = value;
                SignalPropertyChange();
            }
        }

        private float _unknown0x5C;

        [Category("Trophy Data")]
        public float Unknown0x5C
        {
            get => _unknown0x5C;
            set
            {
                _unknown0x5C = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            VoidPtr tydata = Parent.Parent.WorkingUncompressed.Address;
            _name = (tydata + TyDataHeader.HeaderSize).GetUTF8String(Header->_nameOffset);
            _brres = (tydata + TyDataHeader.HeaderSize).GetUTF8String(Header->_brresOffset);
            _id = Header->_id;
            _gameIcon1 = Header->_gameIcon1;
            _gameIcon2 = Header->_gameIcon2;
            _nameIndex = Header->_nameIndex;
            _gameIndex = Header->_gameIndex;
            _descriptionIndex = Header->_descriptionIndex;
            _series = Header->_series;
            _category = Header->_category;
            _pad0x28 = Header->_pad0x28;
            _pad0x2C = Header->_pad0x2C;
            _pad0x30 = Header->_pad0x30;
            _unknown0x34 = Header->_unknown0x34;
            _unknown0x38 = Header->_unknown0x38;
            _pad0x3C = Header->_pad0x3C;
            _unknown0x40 = Header->_unknown0x40;
            _unknown0x44 = Header->_unknown0x44;
            _pad0x48 = Header->_pad0x48;
            _thumbnail = Header->_thumbnail;
            _unknown0x50 = Header->_unknown0x50;
            _unknown0x54 = Header->_unknown0x54;
            _unknown0x58 = Header->_unknown0x58;
            _unknown0x5C = Header->_unknown0x5C;
            return false;
        }

        internal uint NameOffset;
        internal uint BrresOffset;
        internal uint Offset;
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TyDataListEntry* header = (TyDataListEntry*)address;
            *header = new TyDataListEntry();
            header->_id = _id;
            header->_nameOffset = NameOffset;
            header->_brresOffset = BrresOffset;
            header->_gameIcon1 = _gameIcon1;
            header->_gameIcon2 = _gameIcon2;
            header->_nameIndex = _nameIndex;
            header->_gameIndex = _gameIndex;
            header->_descriptionIndex = _descriptionIndex;
            header->_series = _series;
            header->_category = _category;
            header->_pad0x28 = _pad0x28;
            header->_pad0x2C = _pad0x2C;
            header->_pad0x30 = _pad0x30;
            header->_unknown0x34 = _unknown0x34;
            header->_unknown0x38 = _unknown0x38;
            header->_pad0x3C = _pad0x3C;
            header->_unknown0x40 = _unknown0x40;
            header->_unknown0x44 = _unknown0x44;
            header->_pad0x48 = _pad0x48;
            header->_thumbnail = _thumbnail;
            header->_unknown0x50 = _unknown0x50;
            header->_unknown0x54 = _unknown0x54;
            header->_unknown0x58 = _unknown0x58;
            header->_unknown0x5C = _unknown0x5C;
        }

        public override int OnCalculateSize(bool force)
        {
            return (int)TyDataListEntry.Size;
        }
    }

    public class TySealVertListNode : TyEntryNode
    {
        public override bool OnInitialize()
        {
            return WorkingUncompressed.Length / 4 > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < WorkingUncompressed.Length / 4; i++)
            {
                new TySealVertListEntryNode { _name = $"Entry [{i}]" }.Initialize(this, WorkingUncompressed.Address[i, 4], (int)TySealVertListEntry.Size);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0;
            foreach (ResourceNode n in Children)
            {
                size += n.OnCalculateSize(true);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            uint offset = 0;
            foreach (ResourceNode n in Children)
            {
                int size = n.OnCalculateSize(true);
                n.OnRebuild(address + offset, size, true);
                offset += (uint)size;
            }
        }
    }

    public unsafe class TySealVertListEntryNode : ResourceNode
    {
        private float _value;

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _value = ((bfloat*)WorkingUncompressed.Address)->Value;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return (int)TySealVertListEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TySealVertListEntry* header = (TySealVertListEntry*)address;
            *header = new TySealVertListEntry();
            header->_value = _value;
        }
    }
}

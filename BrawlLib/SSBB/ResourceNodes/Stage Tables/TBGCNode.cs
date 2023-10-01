using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGCNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBGC;
        public override Type SubEntryType => typeof(TBGCEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGC" ? new TBGCNode() : null;
        }
    }

    public unsafe class TBGCEntryNode : ResourceNode
    {
        private TBGCEntry Data;

        public enum VillagerIDs : byte
        {
            [Description("Sable (Model Data [31])")]
            Asami = 0x00,
            [Description("Blanca (Model Data [32])")]
            AyasiNeko = 0x01,
            [Description("Male Villager #1 (Model Data [33])")]
            Boy01 = 0x02,
            [Description("Male Villager #2 (Model Data [34])")]
            Boy02 = 0x03,
            [Description("Male Villager #3 (Model Data [35])")]
            Boy03 = 0x04,
            [Description("Cornimer (Model Data [36])")]
            Donguri = 0x05,
            [Description("Celeste (Model Data [37])")]
            Fuko = 0x06,
            [Description("Blathers (Model Data [38])")]
            Futa = 0x07,
            [Description("Female Villager #1 (Model Data [39])")]
            Girl01 = 0x08,
            [Description("Female Villager #2 (Model Data [40])")]
            Girl02 = 0x09,
            [Description("Gracie (Model Data [41])")]
            Grace = 0x0A,
            [Description("Gracie's Car (Model Data [8])")]
            GraceCar = 0x0B,
            [Description("Katrina (Model Data [42])")]
            Hakkemi = 0x0C,
            [Description("Lyle (Model Data [43])")]
            Honma = 0x0D,
            [Description("Joan (Model Data [44])")]
            Kaburiba = 0x0E,
            [Description("Harriet (Model Data [45])")]
            Katorinu = 0x0F,
            [Description("Mabel (Model Data [46])")]
            Kinuyo = 0x10,
            [Description("Katie (Model Data [47])")]
            Maigo = 0x11,
            [Description("Timmy or Tommy (Model Data [48])")]
            MameTubu =  0x12,
            [Description("Brewster (Model Data [49])")]
            Master = 0x13,
            [Description("The Roost (Model Data [9])")]
            Hatonosu = 0x14,
            [Description("Rover (Model Data [50])")]
            MisiranuNeko = 0x15,
            [Description("Copper (Model Data [51])")]
            MonbanA = 0x16,
            [Description("Booker (Model Data [52])")]
            MonbanB = 0x17,
            [Description("Kaitlin (Model Data [53])")]
            MaigoMama = 0x18,
            [Description("Pelly (Model Data [54])")]
            Periko = 0x19,
            [Description("Phyllis (Model Data [55])")]
            Perimi = 0x1A,
            [Description("Pascal (Model Data [56])")]
            Rakosuke = 0x1B,
            [Description("Saharah (Model Data [57])")]
            Rouran = 0x1C,
            [Description("Wendell (Model Data [58])")]
            Seiiti = 0x1D,
            [Description("Dr. Shrunk (Model Data [59])")]
            Sisyou = 0x1E,
            [Description("Tortimer (Model Data [60])")]
            Kotobuki = 0x1F,
            [Description("Tom Nook - Nookington's (Model Data [61])")]
            TanukiD = 0x20,
            [Description("Tom Nook - Nook 'n' Go (Model Data [62])")]
            TanukiK = 0x21,
            [Description("Tom Nook - Nookway (Model Data [63])")]
            TanukiS = 0x22,
            [Description("Tom Nook - Nook's Cranny (Model Data [64])")]
            TanukiZ = 0x23,
            [Description("K.K. Slider (Model Data [65])")]
            Totakeke = 0x24,
            [Description("Redd (Model Data [66])")]
            Tunekiti = 0x25,
            [Description("Kapp'n (Model Data [67])")]
            Untensyu = 0x26,
            None = 0xFA
        }

        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager1
        {
            get => Data._unk0x00;
            set
            {
                Data._unk0x00 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager2
        {
            get => Data._unk0x01;
            set
            {
                Data._unk0x01 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager3
        {
            get => Data._unk0x02;
            set
            {
                Data._unk0x02 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager4
        {
            get => Data._unk0x03;
            set
            {
                Data._unk0x03 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager5
        {
            get => Data._unk0x04;
            set
            {
                Data._unk0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager6
        {
            get => Data._unk0x05;
            set
            {
                Data._unk0x05 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager7
        {
            get => Data._unk0x06;
            set
            {
                Data._unk0x06 = value;
                SignalPropertyChange();
            }
        }

        [Category("TBGCEntry")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager8
        {
            get => Data._unk0x07;
            set
            {
                Data._unk0x07 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TBGCEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(TBGCEntry*)WorkingUncompressed.Address;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(TBGCEntry*)address = Data;
        }
    }
}
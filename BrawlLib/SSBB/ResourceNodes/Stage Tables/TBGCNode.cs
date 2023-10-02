using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGCNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBGC;
        public override Type SubEntryType => typeof(TBGCEntryNode);

        protected override string GetName()
        {
            return base.GetName("Smashville Cameo Spawnlists");
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGC" ? new TBGCNode() : null;
        }
    }

    public class LinkedModelDataAttribute : Attribute
    {
        public byte LinkedModelData { get; private set; }

        public LinkedModelDataAttribute(byte linkedModelData)
        {
            LinkedModelData = linkedModelData;
        }
    }

    public class VillagerNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public VillagerNameAttribute(string name)
        {
            Name = name;
        }
    }

    public class VillagerDropdown : UserInputHexByteEnumDropdown<VillagerID> { }
    public enum VillagerID : byte
    {
        [LinkedModelData(31)]
        [VillagerName("Sable")]
        [Description("Sable (Model Data [31])")]
        Asami = 0x00,

        [LinkedModelData(32)]
        [VillagerName("Bianca")]
        [Description("Blanca (Model Data [32])")]
        AyasiNeko = 0x01,

        [LinkedModelData(33)]
        [VillagerName("Male Villager #1")]
        [Description("Male Villager #1 (Model Data [33])")]
        Boy01 = 0x02,

        [LinkedModelData(34)]
        [VillagerName("Male Villager #2")]
        [Description("Male Villager #2 (Model Data [34])")]
        Boy02 = 0x03,

        [LinkedModelData(35)]
        [VillagerName("Male Villager #3")]
        [Description("Male Villager #3 (Model Data [35])")]
        Boy03 = 0x04,

        [LinkedModelData(36)]
        [VillagerName("Cornimer")]
        [Description("Cornimer (Model Data [36])")]
        Donguri = 0x05,

        [LinkedModelData(37)]
        [VillagerName("Celeste")]
        [Description("Celeste (Model Data [37])")]
        Fuko = 0x06,

        [LinkedModelData(38)]
        [VillagerName("Blathers")]
        [Description("Blathers (Model Data [38])")]
        Futa = 0x07,

        [LinkedModelData(39)]
        [VillagerName("Female Villager #1")]
        [Description("Female Villager #1 (Model Data [39])")]
        Girl01 = 0x08,

        [LinkedModelData(40)]
        [VillagerName("Female Villager #2")]
        [Description("Female Villager #2 (Model Data [40])")]
        Girl02 = 0x09,

        [LinkedModelData(41)]
        [VillagerName("Gracie")]
        [Description("Gracie (Model Data [41])")]
        Grace = 0x0A,

        [LinkedModelData(8)]
        [VillagerName("Gracie's Car")]
        [Description("Gracie's Car (Model Data [8])")]
        GraceCar = 0x0B,

        [LinkedModelData(42)]
        [VillagerName("Katrina")]
        [Description("Katrina (Model Data [42])")]
        Hakkemi = 0x0C,

        [LinkedModelData(43)]
        [VillagerName("Lyle")]
        [Description("Lyle (Model Data [43])")]
        Honma = 0x0D,

        [LinkedModelData(44)]
        [VillagerName("Joan")]
        [Description("Joan (Model Data [44])")]
        Kaburiba = 0x0E,

        [LinkedModelData(45)]
        [VillagerName("Harriet")]
        [Description("Harriet (Model Data [45])")]
        Katorinu = 0x0F,

        [LinkedModelData(46)]
        [VillagerName("Mabel")]
        [Description("Mabel (Model Data [46])")]
        Kinuyo = 0x10,

        [LinkedModelData(47)]
        [VillagerName("Katie")]
        [Description("Katie (Model Data [47])")]
        Maigo = 0x11,

        [LinkedModelData(48)]
        [VillagerName("Timmy or Tommy")]
        [Description("Timmy or Tommy (Model Data [48])")]
        MameTubu = 0x12,

        [LinkedModelData(49)]
        [VillagerName("Brewster")]
        [Description("Brewster (Model Data [49])")]
        Master = 0x13,

        [LinkedModelData(9)]
        [VillagerName("The Roost")]
        [Description("The Roost (Model Data [9])")]
        Hatonosu = 0x14,

        [LinkedModelData(50)]
        [VillagerName("Rover")]
        [Description("Rover (Model Data [50])")]
        MisiranuNeko = 0x15,

        [LinkedModelData(51)]
        [VillagerName("Copper")]
        [Description("Copper (Model Data [51])")]
        MonbanA = 0x16,

        [LinkedModelData(52)]
        [VillagerName("Booker")]
        [Description("Booker (Model Data [52])")]
        MonbanB = 0x17,

        [LinkedModelData(53)]
        [VillagerName("Kaitlin")]
        [Description("Kaitlin (Model Data [53])")]
        MaigoMama = 0x18,

        [LinkedModelData(54)]
        [VillagerName("Pelly")]
        [Description("Pelly (Model Data [54])")]
        Periko = 0x19,

        [LinkedModelData(55)]
        [VillagerName("Phyllis")]
        [Description("Phyllis (Model Data [55])")]
        Perimi = 0x1A,

        [LinkedModelData(56)]
        [VillagerName("Pascal")]
        [Description("Pascal (Model Data [56])")]
        Rakosuke = 0x1B,

        [LinkedModelData(57)]
        [VillagerName("Saharah")]
        [Description("Saharah (Model Data [57])")]
        Rouran = 0x1C,

        [LinkedModelData(58)]
        [VillagerName("Wendell")]
        [Description("Wendell (Model Data [58])")]
        Seiiti = 0x1D,

        [LinkedModelData(59)]
        [VillagerName("Dr. Shrunk")]
        [Description("Dr. Shrunk (Model Data [59])")]
        Sisyou = 0x1E,

        [LinkedModelData(60)]
        [VillagerName("Tortimer")]
        [Description("Tortimer (Model Data [60])")]
        Kotobuki = 0x1F,

        [LinkedModelData(61)]
        [VillagerName("Tom Nook (Nookington's)")]
        [Description("Tom Nook - Nookington's (Model Data [61])")]
        TanukiD = 0x20,

        [LinkedModelData(62)]
        [VillagerName("Tom Nook (Nook 'n' Go)")]
        [Description("Tom Nook - Nook 'n' Go (Model Data [62])")]
        TanukiK = 0x21,

        [LinkedModelData(63)]
        [VillagerName("Tom Nook (Nookway)")]
        [Description("Tom Nook - Nookway (Model Data [63])")]
        TanukiS = 0x22,

        [LinkedModelData(64)]
        [VillagerName("Tom Nook (Nook's Cranny)")]
        [Description("Tom Nook - Nook's Cranny (Model Data [64])")]
        TanukiZ = 0x23,

        [LinkedModelData(65)]
        [VillagerName("K.K. Slider")]
        [Description("K.K. Slider (Model Data [65])")]
        Totakeke = 0x24,

        [LinkedModelData(66)]
        [VillagerName("Redd")]
        [Description("Redd (Model Data [66])")]
        Tunekiti = 0x25,

        [LinkedModelData(67)]
        [VillagerName("Kapp'n")]
        [Description("Kapp'n (Model Data [67])")]
        Untensyu = 0x26,

        None = 0xFA
    }

    public unsafe class TBGCEntryNode : ResourceNode
    {
        private TBGCEntry Data;

        [Category("Cameos")]
        [Description("Spawns at the transLeftChrA bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo1
        {
            get => Data._cameo1;
            set
            {
                Data._cameo1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Cameos")]
        [Description("Spawns at the transLeftChrB bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo2
        {
            get => Data._cameo2;
            set
            {
                Data._cameo2 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Cameos")]
        [Description("Spawns at the transCenterChrA bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo3
        {
            get => Data._cameo3;
            set
            {
                Data._cameo3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Cameos")]
        [Description("Spawns at the transCenterChrB bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo4
        {
            get => Data._cameo4;
            set
            {
                Data._cameo4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Cameos")]
        [Description("Spawns at the transGreas bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo5
        {
            get => Data._cameo5;
            set
            {
                Data._cameo5 = value;
                SignalPropertyChange();
            }
        }

        [Category("Cameos")]
        [Description("Spawns at the transGreasCar bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo6
        {
            get => Data._cameo6;
            set
            {
                Data._cameo6 = value;
                SignalPropertyChange();
            }
        }

        [Category("Cameos")]
        [Description("Spawns at the transHatoNoSu bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo7
        {
            get => Data._cameo7;
            set
            {
                Data._cameo7 = value;
                SignalPropertyChange();
            }
        }

        [Category("Cameos")]
        [Description("Spawns at the transMaster bone on the StgVillageMainStage model")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Cameo8
        {
            get => Data._cameo8;
            set
            {
                Data._cameo8 = value;
                SignalPropertyChange();
            }
        }

        public byte? GetModelData(byte villagerId)
        {
            if (Enum.GetValues(typeof(VillagerID)).Cast<VillagerID>().Any(o => (byte)o == villagerId))
            {
                VillagerID villager = (VillagerID)villagerId;
                return ((LinkedModelDataAttribute)villager.GetType().GetField(villager.ToString())
                    .GetCustomAttributes(true)
                    .FirstOrDefault(a => a is LinkedModelDataAttribute))?.LinkedModelData;
            }

            return null;
        }

        [Browsable(false)] public byte? Cameo1ModelData => GetModelData(Cameo1);
        [Browsable(false)] public byte? Cameo2ModelData => GetModelData(Cameo2);
        [Browsable(false)] public byte? Cameo3ModelData => GetModelData(Cameo3);
        [Browsable(false)] public byte? Cameo4ModelData => GetModelData(Cameo4);
        [Browsable(false)] public byte? Cameo5ModelData => GetModelData(Cameo5);
        [Browsable(false)] public byte? Cameo6ModelData => GetModelData(Cameo6);
        [Browsable(false)] public byte? Cameo7ModelData => GetModelData(Cameo7);
        [Browsable(false)] public byte? Cameo8ModelData => GetModelData(Cameo8);

        public override int OnCalculateSize(bool force)
        {
            return TBGCEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(TBGCEntry*)WorkingUncompressed.Address;

            if (_name == null)
            {
                _name = $"Cameo List [{Index}]";
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(TBGCEntry*)address = Data;
        }
    }
}
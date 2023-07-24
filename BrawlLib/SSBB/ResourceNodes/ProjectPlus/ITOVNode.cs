using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class ITOVv1Node : ARCEntryNode
    {
        public ITOVHeader Data;

        [Category("ITOV")]
        public string CommonItemOverride
        {
            get => Data.CommonOverride;
            set
            {
                Data.CommonOverride = value;
                SignalPropertyChange();
            }
        }

        [Category("ITOV")]
        public string PokeAssistOverride
        {
            get => Data.PokeAssistOverride;
            set
            {
                Data.PokeAssistOverride = value;
                SignalPropertyChange();
            }
        }

        protected override string GetName()
        {
            return GetName("Item Override");
        }

        public override bool OnInitialize()
        {
            Data = *(ITOVHeader*)WorkingUncompressed.Address;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return ITOVHeader.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(ITOVHeader*) address = Data;
            address.WriteUTF8String("ITOV", false, 0, 4);
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == ITOVHeader.Tag && source.Length == ITOVHeader.Size ? new ITOVv1Node() : null;
        }
    }


    public unsafe class ITOVNode : ARCEntryNode
    {
        public ITOVv2 Data;

        [Category("ITOV")]
        public string CommonItemOverride
        {
            get => Data.CommonOverride;
            set
            {
                Data.CommonOverride = value;
                SignalPropertyChange();
            }
        }

        [Category("ITOV")]
        public string PokeAssistOverride
        {
            get => Data.PokeAssistOverride;
            set
            {
                Data.PokeAssistOverride = value;
                SignalPropertyChange();
            }
        }

        [Category("ITOV")]
        public string StageItemFolder
        {
            get => Data.StageItemFolder;
            set
            {
                Data.StageItemFolder = value;
                SignalPropertyChange();
            }
        }

        [Category("ITOV")]
        [Description("Adds this number to the default maximum Pokémon (5 in vBrawl) which can spawn at one time")]
        public sbyte PokemonOverload
        {
            get => Data._pokemonOverload;
            set => Data._pokemonOverload = value.Clamp(-5, 7);
        }

        [Category("ITOV")]
        public ItemOverrideSetting CommonOverride
        {
            get => Data._overrideCommon;
            set => Data._overrideCommon = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Torchic
        {
            get => Data._overrideTorchic;
            set => Data._overrideTorchic = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Celebi
        {
            get => Data._overrideCelebi;
            set => Data._overrideCelebi = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Chikorita
        {
            get => Data._overrideChikorita;
            set => Data._overrideChikorita = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting ChikoritaShot
        {
            get => Data._overrideChikoritaShot;
            set => Data._overrideChikoritaShot = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Entei
        {
            get => Data._overrideEntei;
            set => Data._overrideEntei = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Moltres
        {
            get => Data._overrideMoltres;
            set => Data._overrideMoltres = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Munchlax
        {
            get => Data._overrideMunchlax;
            set => Data._overrideMunchlax = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Deoxys
        {
            get => Data._overrideDeoxys;
            set => Data._overrideDeoxys = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Groudon
        {
            get => Data._overrideGroudon;
            set => Data._overrideGroudon = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Gulpin
        {
            get => Data._overrideGulpin;
            set => Data._overrideGulpin = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Staryu
        {
            get => Data._overrideStaryu;
            set => Data._overrideStaryu = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting StaryuShot
        {
            get => Data._overrideStaryuShot;
            set => Data._overrideStaryuShot = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting HoOh
        {
            get => Data._overrideHoOh;
            set => Data._overrideHoOh = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting HoOhShot
        {
            get => Data._overrideHoOhShot;
            set => Data._overrideHoOhShot = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Jirachi
        {
            get => Data._overrideJirachi;
            set => Data._overrideJirachi = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Snorlax
        {
            get => Data._overrideSnorlax;
            set => Data._overrideSnorlax = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Bellosom
        {
            get => Data._overrideBellosom;
            set => Data._overrideBellosom = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Kyogre
        {
            get => Data._overrideKyogre;
            set => Data._overrideKyogre = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting KyogreShot
        {
            get => Data._overrideKyogreShot;
            set => Data._overrideKyogreShot = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting LatiasLatios
        {
            get => Data._overrideLatiasLatios;
            set => Data._overrideLatiasLatios = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Lugia
        {
            get => Data._overrideLugia;
            set => Data._overrideLugia = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting LugiaShot
        {
            get => Data._overrideLugiaShot;
            set => Data._overrideLugiaShot = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Manaphy
        {
            get => Data._overrideManaphy;
            set => Data._overrideManaphy = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Weavile
        {
            get => Data._overrideWeavile;
            set => Data._overrideWeavile = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Electrode
        {
            get => Data._overrideElectrode;
            set => Data._overrideElectrode = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Metagross
        {
            get => Data._overrideMetagross;
            set => Data._overrideMetagross = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Mew
        {
            get => Data._overrideMew;
            set => Data._overrideMew = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Meowth
        {
            get => Data._overrideMeowth;
            set => Data._overrideMeowth = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting MeowthShot
        {
            get => Data._overrideMeowthShot;
            set => Data._overrideMeowthShot = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Piplup
        {
            get => Data._overridePiplup;
            set => Data._overridePiplup = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Togepi
        {
            get => Data._overrideTogepi;
            set => Data._overrideTogepi = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Goldeen
        {
            get => Data._overrideGoldeen;
            set => Data._overrideGoldeen = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Gardevoir
        {
            get => Data._overrideGardevoir;
            set => Data._overrideGardevoir = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Wobbuffet
        {
            get => Data._overrideWobbuffet;
            set => Data._overrideWobbuffet = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Suicune
        {
            get => Data._overrideSuicune;
            set => Data._overrideSuicune = value;
        }

        [Category("ITOV Pokémon Override Settings")]
        public ItemOverrideSetting Bonsly
        {
            get => Data._overrideBonsly;
            set => Data._overrideBonsly = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Andross
        {
            get => Data._overrideAndross;
            set => Data._overrideAndross = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting AndrossShot
        {
            get => Data._overrideAndrossShot;
            set => Data._overrideAndrossShot = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Barbara
        {
            get => Data._overrideBarbara;
            set => Data._overrideBarbara = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting GrayFox
        {
            get => Data._overrideGrayFox;
            set => Data._overrideGrayFox = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting RayMKII
        {
            get => Data._overrideRayMKII;
            set => Data._overrideRayMKII = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting RayMKIIBomb
        {
            get => Data._overrideRayMKIIBomb;
            set => Data._overrideRayMKIIBomb = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting RayMKIIGun
        {
            get => Data._overrideRayMKIIGun;
            set => Data._overrideRayMKIIGun = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting SamuraiGoroh
        {
            get => Data._overrideSamuraiGoroh;
            set => Data._overrideSamuraiGoroh = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Devil
        {
            get => Data._overrideDevil;
            set => Data._overrideDevil = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Excitebike
        {
            get => Data._overrideExcitebike;
            set => Data._overrideExcitebike = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Jeff
        {
            get => Data._overrideJeff;
            set => Data._overrideJeff = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting JeffPencilBullet
        {
            get => Data._overrideJeffPencilBullet;
            set => Data._overrideJeffPencilBullet = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting JeffPencilRocket
        {
            get => Data._overrideJeffPencilRocket;
            set => Data._overrideJeffPencilRocket = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Lakitu
        {
            get => Data._overrideLakitu;
            set => Data._overrideLakitu = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting KnuckleJoe
        {
            get => Data._overrideKnuckleJoe;
            set => Data._overrideKnuckleJoe = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting KnuckleJoeShot
        {
            get => Data._overrideKnuckleJoeShot;
            set => Data._overrideKnuckleJoeShot = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting HammerBro
        {
            get => Data._overrideHammerBro;
            set => Data._overrideHammerBro = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting HammerBroHammer
        {
            get => Data._overrideHammerBroHammer;
            set => Data._overrideHammerBroHammer = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Helirin
        {
            get => Data._overrideHelirin;
            set => Data._overrideHelirin = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Kat
        {
            get => Data._overrideKat;
            set => Data._overrideKat = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Ana
        {
            get => Data._overrideAna;
            set => Data._overrideAna = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting JillDozer
        {
            get => Data._overrideJillDozer;
            set => Data._overrideJillDozer = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Lyn
        {
            get => Data._overrideLyn;
            set => Data._overrideLyn = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting LittleMac
        {
            get => Data._overrideLittleMac;
            set => Data._overrideLittleMac = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Metroid
        {
            get => Data._overrideMetroid;
            set => Data._overrideMetroid = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Nintendog
        {
            get => Data._overrideNintendog;
            set => Data._overrideNintendog = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting NintendogFull
        {
            get => Data._overrideNintendogFull;
            set => Data._overrideNintendogFull = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting MrResetti
        {
            get => Data._overrideMrResetti;
            set => Data._overrideMrResetti = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Isaac
        {
            get => Data._overrideIsaac;
            set => Data._overrideIsaac = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting IsaacShot
        {
            get => Data._overrideIsaacShot;
            set => Data._overrideIsaacShot = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Saki
        {
            get => Data._overrideSaki;
            set => Data._overrideSaki = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting SakiShot1
        {
            get => Data._overrideSakiShot1;
            set => Data._overrideSakiShot1 = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting SakiShot2
        {
            get => Data._overrideSakiShot2;
            set => Data._overrideSakiShot2 = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Shadow
        {
            get => Data._overrideShadow;
            set => Data._overrideShadow = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting WarInfantry
        {
            get => Data._overrideWarInfantry;
            set => Data._overrideWarInfantry = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting WarInfantryShot
        {
            get => Data._overrideWarInfantryShot;
            set => Data._overrideWarInfantryShot = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Starfy
        {
            get => Data._overrideStarfy;
            set => Data._overrideStarfy = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting WarTank
        {
            get => Data._overrideWarTank;
            set => Data._overrideWarTank = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting WarTankShot
        {
            get => Data._overrideWarTankShot;
            set => Data._overrideWarTankShot = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Tingle
        {
            get => Data._overrideTingle;
            set => Data._overrideTingle = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting LakituSpiny
        {
            get => Data._overrideLakituSpiny;
            set => Data._overrideLakituSpiny = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting Waluigi
        {
            get => Data._overrideWaluigi;
            set => Data._overrideWaluigi = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting DrWright
        {
            get => Data._overrideDrWright;
            set => Data._overrideDrWright = value;
        }

        [Category("ITOV Assist Override Settings")]
        public ItemOverrideSetting DrWrightBuilding
        {
            get => Data._overrideDrWrightBuilding;
            set => Data._overrideDrWrightBuilding = value;
        }

        protected override string GetName()
        {
            return GetName("Item Override");
        }

        public override bool OnInitialize()
        {
            Data = *(ITOVv2*)WorkingUncompressed.Address;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return ITOVv2.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(ITOVv2*)address = Data;
            address.WriteUTF8String("ITOV", false, 0, 4);
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == ITOVHeader.Tag && source.Length == ITOVv2.Size ? new ITOVNode() : null;
        }
    }
}

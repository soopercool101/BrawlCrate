using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.ProjectPlus
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ITOVHeader
    {
        public const int Size = 0x1C;
        public const string Tag = "ITOV";

        public buint _tag;
        public fixed byte _commonOverride[12];
        public fixed byte _pokeAssistOverride[12];

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public string CommonOverride
        {
            get => Address.GetUTF8String(0x4, 12);
            set => Address.WriteUTF8String(value, true, 0x4, 12);
        }

        public string PokeAssistOverride
        {
            get => Address.GetUTF8String(0x10, 12);
            set => Address.WriteUTF8String(value, true, 0x10, 12);
        }
    }

    public enum ItemOverrideSetting : byte
    {
        None = 0x0,
        Brres = 0x1,
        Param = 0x2,
        Both = 0x3,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ITOVv2
    {
        public const int Size = 0x7A;
        public ITOVHeader _header;
        public sbyte _pokemonOverload;
        public ItemOverrideSetting _overrideCommon;
        public ItemOverrideSetting _overrideTorchic;
        public ItemOverrideSetting _overrideCelebi;
        public ItemOverrideSetting _overrideChikorita;
        public ItemOverrideSetting _overrideChikoritaShot;
        public ItemOverrideSetting _overrideEntei;
        public ItemOverrideSetting _overrideMoltres;
        public ItemOverrideSetting _overrideMunchlax;
        public ItemOverrideSetting _overrideDeoxys;
        public ItemOverrideSetting _overrideGroudon;
        public ItemOverrideSetting _overrideGulpin;
        public ItemOverrideSetting _overrideStaryu;
        public ItemOverrideSetting _overrideStaryuShot;
        public ItemOverrideSetting _overrideHoOh;
        public ItemOverrideSetting _overrideHoOhShot;
        public ItemOverrideSetting _overrideJirachi;
        public ItemOverrideSetting _overrideSnorlax;
        public ItemOverrideSetting _overrideBellosom;
        public ItemOverrideSetting _overrideKyogre;
        public ItemOverrideSetting _overrideKyogreShot;
        public ItemOverrideSetting _overrideLatiasLatios;
        public ItemOverrideSetting _overrideLugia;
        public ItemOverrideSetting _overrideLugiaShot;
        public ItemOverrideSetting _overrideManaphy;
        public ItemOverrideSetting _overrideWeavile;
        public ItemOverrideSetting _overrideElectrode;
        public ItemOverrideSetting _overrideMetagross;
        public ItemOverrideSetting _overrideMew;
        public ItemOverrideSetting _overrideMeowth;
        public ItemOverrideSetting _overrideMeowthShot;
        public ItemOverrideSetting _overridePiplup;
        public ItemOverrideSetting _overrideTogepi;
        public ItemOverrideSetting _overrideGoldeen;
        public ItemOverrideSetting _overrideGardevoir;
        public ItemOverrideSetting _overrideWobbuffet;
        public ItemOverrideSetting _overrideSuicune;
        public ItemOverrideSetting _overrideBonsly;
        public ItemOverrideSetting _overrideAndross;
        public ItemOverrideSetting _overrideAndrossShot;
        public ItemOverrideSetting _overrideBarbara;
        public ItemOverrideSetting _overrideGrayFox;
        public ItemOverrideSetting _overrideRayMKII;
        public ItemOverrideSetting _overrideRayMKIIBomb;
        public ItemOverrideSetting _overrideRayMKIIGun;
        public ItemOverrideSetting _overrideSamuraiGoroh;
        public ItemOverrideSetting _overrideDevil;
        public ItemOverrideSetting _overrideExcitebike;
        public ItemOverrideSetting _overrideJeff;
        public ItemOverrideSetting _overrideJeffPencilBullet;
        public ItemOverrideSetting _overrideJeffPencilRocket;
        public ItemOverrideSetting _overrideLakitu;
        public ItemOverrideSetting _overrideKnuckleJoe;
        public ItemOverrideSetting _overrideKnuckleJoeShot;
        public ItemOverrideSetting _overrideHammerBro;
        public ItemOverrideSetting _overrideHammerBroHammer;
        public ItemOverrideSetting _overrideHelirin;
        public ItemOverrideSetting _overrideKat;
        public ItemOverrideSetting _overrideAna;
        public ItemOverrideSetting _overrideJillDozer;
        public ItemOverrideSetting _overrideLyn;
        public ItemOverrideSetting _overrideLittleMac;
        public ItemOverrideSetting _overrideMetroid;
        public ItemOverrideSetting _overrideNintendog;
        public ItemOverrideSetting _overrideNintendogFull;
        public ItemOverrideSetting _overrideMrResetti;
        public ItemOverrideSetting _overrideIsaac;
        public ItemOverrideSetting _overrideIsaacShot;
        public ItemOverrideSetting _overrideSaki;
        public ItemOverrideSetting _overrideSakiShot1;
        public ItemOverrideSetting _overrideSakiShot2;
        public ItemOverrideSetting _overrideShadow;
        public ItemOverrideSetting _overrideWarInfantry;
        public ItemOverrideSetting _overrideWarInfantryShot;
        public ItemOverrideSetting _overrideStarfy;
        public ItemOverrideSetting _overrideWarTank;
        public ItemOverrideSetting _overrideWarTankShot;
        public ItemOverrideSetting _overrideTingle;
        public ItemOverrideSetting _overrideLakituSpiny;
        public ItemOverrideSetting _overrideWaluigi;
        public ItemOverrideSetting _overrideDrWright;
        public ItemOverrideSetting _overrideDrWrightBuilding;
        public fixed byte _stageItemFolder[12];

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public string CommonOverride
        {
            get => _header.CommonOverride;
            set => _header.CommonOverride = value;
        }

        public string PokeAssistOverride
        {
            get => _header.PokeAssistOverride;
            set => _header.PokeAssistOverride = value;
        }

        public string StageItemFolder
        {
            get => Address.GetUTF8String(0x6E, 12);
            set => Address.WriteUTF8String(value, true, 0x6E, 12);
        }
    }
}

using System.ComponentModel;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using static BrawlLib.SSBB.Types.ProjectPlus.ENOV;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class ENOVNode : ARCEntryNode
    {
        public ENOV Data;

        [Category("Enemy Override")]
        public string EnemyOverrideFolder { get; set; }

        [Category("Enemy Override")]
        public string StageItemFolder { get; set; }


        [Category("Enemy Override")]
        public bool UseIndividualFolders
        {
            get => Data.UseIndividualFolders;
            set => Data.UseIndividualFolders = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleCommon
        {
            get => Data.OverrideModuleCommon;
            set => Data.OverrideModuleCommon = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideCommon
        {
            get => Data.OverrideCommon;
            set => Data.OverrideCommon = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGoomba
        {
            get => Data.FaceIndexGoomba;
            set => Data.FaceIndexGoomba = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGoomba
        {
            get => Data.OverrideModuleGoomba;
            set => Data.OverrideModuleGoomba = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGoomba
        {
            get => Data.OverrideGoomba;
            set => Data.OverrideGoomba = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPoppant
        {
            get => Data.FaceIndexPoppant;
            set => Data.FaceIndexPoppant = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePoppant
        {
            get => Data.OverrideModulePoppant;
            set => Data.OverrideModulePoppant = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePoppant
        {
            get => Data.OverridePoppant;
            set => Data.OverridePoppant = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexFeyesh
        {
            get => Data.FaceIndexFeyesh;
            set => Data.FaceIndexFeyesh = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleFeyesh
        {
            get => Data.OverrideModuleFeyesh;
            set => Data.OverrideModuleFeyesh = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideFeyesh
        {
            get => Data.OverrideFeyesh;
            set => Data.OverrideFeyesh = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexJyk
        {
            get => Data.FaceIndexJyk;
            set => Data.FaceIndexJyk = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleJyk
        {
            get => Data.OverrideModuleJyk;
            set => Data.OverrideModuleJyk = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideJyk
        {
            get => Data.OverrideJyk;
            set => Data.OverrideJyk = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexAuroros
        {
            get => Data.FaceIndexAuroros;
            set => Data.FaceIndexAuroros = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleAuroros
        {
            get => Data.OverrideModuleAuroros;
            set => Data.OverrideModuleAuroros = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideAuroros
        {
            get => Data.OverrideAuroros;
            set => Data.OverrideAuroros = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexCymul
        {
            get => Data.FaceIndexCymul;
            set => Data.FaceIndexCymul = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleCymul
        {
            get => Data.OverrideModuleCymul;
            set => Data.OverrideModuleCymul = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideCymul
        {
            get => Data.OverrideCymul;
            set => Data.OverrideCymul = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexRoturret
        {
            get => Data.FaceIndexRoturret;
            set => Data.FaceIndexRoturret = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleRoturret
        {
            get => Data.OverrideModuleRoturret;
            set => Data.OverrideModuleRoturret = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideRoturret
        {
            get => Data.OverrideRoturret;
            set => Data.OverrideRoturret = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexBorboras
        {
            get => Data.FaceIndexBorboras;
            set => Data.FaceIndexBorboras = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleBorboras
        {
            get => Data.OverrideModuleBorboras;
            set => Data.OverrideModuleBorboras = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideBorboras
        {
            get => Data.OverrideBorboras;
            set => Data.OverrideBorboras = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGiantGoomba
        {
            get => Data.FaceIndexGiantGoomba;
            set => Data.FaceIndexGiantGoomba = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGiantGoomba
        {
            get => Data.OverrideModuleGiantGoomba;
            set => Data.OverrideModuleGiantGoomba = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGiantGoomba
        {
            get => Data.OverrideGiantGoomba;
            set => Data.OverrideGiantGoomba = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexBuckot
        {
            get => Data.FaceIndexBuckot;
            set => Data.FaceIndexBuckot = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleBuckot
        {
            get => Data.OverrideModuleBuckot;
            set => Data.OverrideModuleBuckot = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideBuckot
        {
            get => Data.OverrideBuckot;
            set => Data.OverrideBuckot = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexBucculus
        {
            get => Data.FaceIndexBucculus;
            set => Data.FaceIndexBucculus = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleBucculus
        {
            get => Data.OverrideModuleBucculus;
            set => Data.OverrideModuleBucculus = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideBucculus
        {
            get => Data.OverrideBucculus;
            set => Data.OverrideBucculus = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGreap
        {
            get => Data.FaceIndexGreap;
            set => Data.FaceIndexGreap = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGreap
        {
            get => Data.OverrideModuleGreap;
            set => Data.OverrideModuleGreap = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGreap
        {
            get => Data.OverrideGreap;
            set => Data.OverrideGreap = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexArmight
        {
            get => Data.FaceIndexArmight;
            set => Data.FaceIndexArmight = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleArmight
        {
            get => Data.OverrideModuleArmight;
            set => Data.OverrideModuleArmight = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideArmight
        {
            get => Data.OverrideArmight;
            set => Data.OverrideArmight = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexBulletBill
        {
            get => Data.FaceIndexBulletBill;
            set => Data.FaceIndexBulletBill = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleBulletBill
        {
            get => Data.OverrideModuleBulletBill;
            set => Data.OverrideModuleBulletBill = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideBulletBill
        {
            get => Data.OverrideBulletBill;
            set => Data.OverrideBulletBill = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexRoader
        {
            get => Data.FaceIndexRoader;
            set => Data.FaceIndexRoader = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleRoader
        {
            get => Data.OverrideModuleRoader;
            set => Data.OverrideModuleRoader = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideRoader
        {
            get => Data.OverrideRoader;
            set => Data.OverrideRoader = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexSpaak
        {
            get => Data.FaceIndexSpaak;
            set => Data.FaceIndexSpaak = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleSpaak
        {
            get => Data.OverrideModuleSpaak;
            set => Data.OverrideModuleSpaak = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideSpaak
        {
            get => Data.OverrideSpaak;
            set => Data.OverrideSpaak = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexMite
        {
            get => Data.FaceIndexMite;
            set => Data.FaceIndexMite = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleMite
        {
            get => Data.OverrideModuleMite;
            set => Data.OverrideModuleMite = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideMite
        {
            get => Data.OverrideMite;
            set => Data.OverrideMite = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexTicken
        {
            get => Data.FaceIndexTicken;
            set => Data.FaceIndexTicken = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleTicken
        {
            get => Data.OverrideModuleTicken;
            set => Data.OverrideModuleTicken = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideTicken
        {
            get => Data.OverrideTicken;
            set => Data.OverrideTicken = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexTowtow
        {
            get => Data.FaceIndexTowtow;
            set => Data.FaceIndexTowtow = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleTowtow
        {
            get => Data.OverrideModuleTowtow;
            set => Data.OverrideModuleTowtow = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideTowtow
        {
            get => Data.OverrideTowtow;
            set => Data.OverrideTowtow = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexHammerBro
        {
            get => Data.FaceIndexHammerBro;
            set => Data.FaceIndexHammerBro = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleHammerBro
        {
            get => Data.OverrideModuleHammerBro;
            set => Data.OverrideModuleHammerBro = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideHammerBro
        {
            get => Data.OverrideHammerBro;
            set => Data.OverrideHammerBro = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexBytan
        {
            get => Data.FaceIndexBytan;
            set => Data.FaceIndexBytan = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleBytan
        {
            get => Data.OverrideModuleBytan;
            set => Data.OverrideModuleBytan = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideBytan
        {
            get => Data.OverrideBytan;
            set => Data.OverrideBytan = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexFloow
        {
            get => Data.FaceIndexFloow;
            set => Data.FaceIndexFloow = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleFloow
        {
            get => Data.OverrideModuleFloow;
            set => Data.OverrideModuleFloow = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideFloow
        {
            get => Data.OverrideFloow;
            set => Data.OverrideFloow = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPuppit
        {
            get => Data.FaceIndexPuppit;
            set => Data.FaceIndexPuppit = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePuppit
        {
            get => Data.OverrideModulePuppit;
            set => Data.OverrideModulePuppit = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePuppit
        {
            get => Data.OverridePuppit;
            set => Data.OverridePuppit = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPrimid
        {
            get => Data.FaceIndexPrimid;
            set => Data.FaceIndexPrimid = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePrimid
        {
            get => Data.OverrideModulePrimid;
            set => Data.OverrideModulePrimid = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePrimid
        {
            get => Data.OverridePrimid;
            set => Data.OverridePrimid = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexShellpod
        {
            get => Data.FaceIndexShellpod;
            set => Data.FaceIndexShellpod = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleShellpod
        {
            get => Data.OverrideModuleShellpod;
            set => Data.OverrideModuleShellpod = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideShellpod
        {
            get => Data.OverrideShellpod;
            set => Data.OverrideShellpod = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexKoopa
        {
            get => Data.FaceIndexKoopa;
            set => Data.FaceIndexKoopa = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleKoopa
        {
            get => Data.OverrideModuleKoopa;
            set => Data.OverrideModuleKoopa = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideKoopa
        {
            get => Data.OverrideKoopa;
            set => Data.OverrideKoopa = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexShaydas
        {
            get => Data.FaceIndexShaydas;
            set => Data.FaceIndexShaydas = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleShaydas
        {
            get => Data.OverrideModuleShaydas;
            set => Data.OverrideModuleShaydas = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideShaydas
        {
            get => Data.OverrideShaydas;
            set => Data.OverrideShaydas = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexBombed
        {
            get => Data.FaceIndexBombed;
            set => Data.FaceIndexBombed = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleBombed
        {
            get => Data.OverrideModuleBombed;
            set => Data.OverrideModuleBombed = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideBombed
        {
            get => Data.OverrideBombed;
            set => Data.OverrideBombed = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPrimidMetal
        {
            get => Data.FaceIndexPrimidMetal;
            set => Data.FaceIndexPrimidMetal = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePrimidMetal
        {
            get => Data.OverrideModulePrimidMetal;
            set => Data.OverrideModulePrimidMetal = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePrimidMetal
        {
            get => Data.OverridePrimidMetal;
            set => Data.OverridePrimidMetal = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexNagagog
        {
            get => Data.FaceIndexNagagog;
            set => Data.FaceIndexNagagog = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleNagagog
        {
            get => Data.OverrideModuleNagagog;
            set => Data.OverrideModuleNagagog = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideNagagog
        {
            get => Data.OverrideNagagog;
            set => Data.OverrideNagagog = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexTrowlon
        {
            get => Data.FaceIndexTrowlon;
            set => Data.FaceIndexTrowlon = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleTrowlon
        {
            get => Data.OverrideModuleTrowlon;
            set => Data.OverrideModuleTrowlon = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideTrowlon
        {
            get => Data.OverrideTrowlon;
            set => Data.OverrideTrowlon = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPrimidBig
        {
            get => Data.FaceIndexPrimidBig;
            set => Data.FaceIndexPrimidBig = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePrimidBig
        {
            get => Data.OverrideModulePrimidBig;
            set => Data.OverrideModulePrimidBig = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePrimidBig
        {
            get => Data.OverridePrimidBig;
            set => Data.OverridePrimidBig = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPrimidBoom
        {
            get => Data.FaceIndexPrimidBoom;
            set => Data.FaceIndexPrimidBoom = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePrimidBoom
        {
            get => Data.OverrideModulePrimidBoom;
            set => Data.OverrideModulePrimidBoom = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePrimidBoom
        {
            get => Data.OverridePrimidBoom;
            set => Data.OverridePrimidBoom = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPrimidFire
        {
            get => Data.FaceIndexPrimidFire;
            set => Data.FaceIndexPrimidFire = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePrimidFire
        {
            get => Data.OverrideModulePrimidFire;
            set => Data.OverrideModulePrimidFire = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePrimidFire
        {
            get => Data.OverridePrimidFire;
            set => Data.OverridePrimidFire = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPrimidScope
        {
            get => Data.FaceIndexPrimidScope;
            set => Data.FaceIndexPrimidScope = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePrimidScope
        {
            get => Data.OverrideModulePrimidScope;
            set => Data.OverrideModulePrimidScope = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePrimidScope
        {
            get => Data.OverridePrimidScope;
            set => Data.OverridePrimidScope = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPrimidSword
        {
            get => Data.FaceIndexPrimidSword;
            set => Data.FaceIndexPrimidSword = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePrimidSword
        {
            get => Data.OverrideModulePrimidSword;
            set => Data.OverrideModulePrimidSword = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePrimidSword
        {
            get => Data.OverridePrimidSword;
            set => Data.OverridePrimidSword = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGamyga
        {
            get => Data.FaceIndexGamyga;
            set => Data.FaceIndexGamyga = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGamyga
        {
            get => Data.OverrideModuleGamyga;
            set => Data.OverrideModuleGamyga = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGamyga
        {
            get => Data.OverrideGamyga;
            set => Data.OverrideGamyga = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexROBBlaster
        {
            get => Data.FaceIndexROBBlaster;
            set => Data.FaceIndexROBBlaster = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleROBBlaster
        {
            get => Data.OverrideModuleROBBlaster;
            set => Data.OverrideModuleROBBlaster = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideROBBlaster
        {
            get => Data.OverrideROBBlaster;
            set => Data.OverrideROBBlaster = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexROBDistance
        {
            get => Data.FaceIndexROBDistance;
            set => Data.FaceIndexROBDistance = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleROBDistance
        {
            get => Data.OverrideModuleROBDistance;
            set => Data.OverrideModuleROBDistance = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideROBDistance
        {
            get => Data.OverrideROBDistance;
            set => Data.OverrideROBDistance = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexROBLauncher
        {
            get => Data.FaceIndexROBLauncher;
            set => Data.FaceIndexROBLauncher = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleROBLauncher
        {
            get => Data.OverrideModuleROBLauncher;
            set => Data.OverrideModuleROBLauncher = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideROBLauncher
        {
            get => Data.OverrideROBLauncher;
            set => Data.OverrideROBLauncher = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexROBSentry
        {
            get => Data.FaceIndexROBSentry;
            set => Data.FaceIndexROBSentry = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleROBSentry
        {
            get => Data.OverrideModuleROBSentry;
            set => Data.OverrideModuleROBSentry = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideROBSentry
        {
            get => Data.OverrideROBSentry;
            set => Data.OverrideROBSentry = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexAutolance
        {
            get => Data.FaceIndexAutolance;
            set => Data.FaceIndexAutolance = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleAutolance
        {
            get => Data.OverrideModuleAutolance;
            set => Data.OverrideModuleAutolance = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideAutolance
        {
            get => Data.OverrideAutolance;
            set => Data.OverrideAutolance = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexArmank
        {
            get => Data.FaceIndexArmank;
            set => Data.FaceIndexArmank = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleArmank
        {
            get => Data.OverrideModuleArmank;
            set => Data.OverrideModuleArmank = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideArmank
        {
            get => Data.OverrideArmank;
            set => Data.OverrideArmank = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGlire
        {
            get => Data.FaceIndexGlire;
            set => Data.FaceIndexGlire = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGlire
        {
            get => Data.OverrideModuleGlire;
            set => Data.OverrideModuleGlire = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGlire
        {
            get => Data.OverrideGlire;
            set => Data.OverrideGlire = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGlice
        {
            get => Data.FaceIndexGlice;
            set => Data.FaceIndexGlice = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGlice
        {
            get => Data.OverrideModuleGlice;
            set => Data.OverrideModuleGlice = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGlice
        {
            get => Data.OverrideGlice;
            set => Data.OverrideGlice = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGlunder
        {
            get => Data.FaceIndexGlunder;
            set => Data.FaceIndexGlunder = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGlunder
        {
            get => Data.OverrideModuleGlunder;
            set => Data.OverrideModuleGlunder = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGlunder
        {
            get => Data.OverrideGlunder;
            set => Data.OverrideGlunder = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPeteyPiranha
        {
            get => Data.FaceIndexPeteyPiranha;
            set => Data.FaceIndexPeteyPiranha = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePeteyPiranha
        {
            get => Data.OverrideModulePeteyPiranha;
            set => Data.OverrideModulePeteyPiranha = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePeteyPiranha
        {
            get => Data.OverridePeteyPiranha;
            set => Data.OverridePeteyPiranha = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGamygaBase01
        {
            get => Data.FaceIndexGamygaBase01;
            set => Data.FaceIndexGamygaBase01 = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGamygaBase01
        {
            get => Data.OverrideModuleGamygaBase01;
            set => Data.OverrideModuleGamygaBase01 = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGamygaBase01
        {
            get => Data.OverrideGamygaBase01;
            set => Data.OverrideGamygaBase01 = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGamygaBase02
        {
            get => Data.FaceIndexGamygaBase02;
            set => Data.FaceIndexGamygaBase02 = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGamygaBase02
        {
            get => Data.OverrideModuleGamygaBase02;
            set => Data.OverrideModuleGamygaBase02 = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGamygaBase02
        {
            get => Data.OverrideGamygaBase02;
            set => Data.OverrideGamygaBase02 = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGamygaBase03
        {
            get => Data.FaceIndexGamygaBase03;
            set => Data.FaceIndexGamygaBase03 = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGamygaBase03
        {
            get => Data.OverrideModuleGamygaBase03;
            set => Data.OverrideModuleGamygaBase03 = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGamygaBase03
        {
            get => Data.OverrideGamygaBase03;
            set => Data.OverrideGamygaBase03 = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGamygaBase04
        {
            get => Data.FaceIndexGamygaBase04;
            set => Data.FaceIndexGamygaBase04 = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGamygaBase04
        {
            get => Data.OverrideModuleGamygaBase04;
            set => Data.OverrideModuleGamygaBase04 = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGamygaBase04
        {
            get => Data.OverrideGamygaBase04;
            set => Data.OverrideGamygaBase04 = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexGalleom
        {
            get => Data.FaceIndexGalleom;
            set => Data.FaceIndexGalleom = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleGalleom
        {
            get => Data.OverrideModuleGalleom;
            set => Data.OverrideModuleGalleom = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideGalleom
        {
            get => Data.OverrideGalleom;
            set => Data.OverrideGalleom = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexRidley
        {
            get => Data.FaceIndexRidley;
            set => Data.FaceIndexRidley = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleRidley
        {
            get => Data.OverrideModuleRidley;
            set => Data.OverrideModuleRidley = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideRidley
        {
            get => Data.OverrideRidley;
            set => Data.OverrideRidley = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexRayquaza
        {
            get => Data.FaceIndexRayquaza;
            set => Data.FaceIndexRayquaza = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleRayquaza
        {
            get => Data.OverrideModuleRayquaza;
            set => Data.OverrideModuleRayquaza = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideRayquaza
        {
            get => Data.OverrideRayquaza;
            set => Data.OverrideRayquaza = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexDuon
        {
            get => Data.FaceIndexDuon;
            set => Data.FaceIndexDuon = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleDuon
        {
            get => Data.OverrideModuleDuon;
            set => Data.OverrideModuleDuon = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideDuon
        {
            get => Data.OverrideDuon;
            set => Data.OverrideDuon = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexPorky
        {
            get => Data.FaceIndexPorky;
            set => Data.FaceIndexPorky = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModulePorky
        {
            get => Data.OverrideModulePorky;
            set => Data.OverrideModulePorky = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverridePorky
        {
            get => Data.OverridePorky;
            set => Data.OverridePorky = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexMetaRidley
        {
            get => Data.FaceIndexMetaRidley;
            set => Data.FaceIndexMetaRidley = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleMetaRidley
        {
            get => Data.OverrideModuleMetaRidley;
            set => Data.OverrideModuleMetaRidley = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideMetaRidley
        {
            get => Data.OverrideMetaRidley;
            set => Data.OverrideMetaRidley = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexFalconFlyer
        {
            get => Data.FaceIndexFalconFlyer;
            set => Data.FaceIndexFalconFlyer = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleFalconFlyer
        {
            get => Data.OverrideModuleFalconFlyer;
            set => Data.OverrideModuleFalconFlyer = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideFalconFlyer
        {
            get => Data.OverrideFalconFlyer;
            set => Data.OverrideFalconFlyer = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexTabuu
        {
            get => Data.FaceIndexTabuu;
            set => Data.FaceIndexTabuu = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleTabuu
        {
            get => Data.OverrideModuleTabuu;
            set => Data.OverrideModuleTabuu = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideTabuu
        {
            get => Data.OverrideTabuu;
            set => Data.OverrideTabuu = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexMasterhand
        {
            get => Data.FaceIndexMasterhand;
            set => Data.FaceIndexMasterhand = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleMasterhand
        {
            get => Data.OverrideModuleMasterhand;
            set => Data.OverrideModuleMasterhand = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideMasterhand
        {
            get => Data.OverrideMasterhand;
            set => Data.OverrideMasterhand = value;
        }

        [Category("Enemy Override")]
        public byte FaceIndexCrazyhand
        {
            get => Data.FaceIndexCrazyhand;
            set => Data.FaceIndexCrazyhand = value;
        }

        [Category("Enemy Override")]
        public bool OverrideModuleCrazyhand
        {
            get => Data.OverrideModuleCrazyhand;
            set => Data.OverrideModuleCrazyhand = value;
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideCrazyhand
        {
            get => Data.OverrideCrazyhand;
            set => Data.OverrideCrazyhand = value;
        }

        public override bool OnInitialize()
        {
            Data = *(ENOV*)WorkingUncompressed.Address;
            EnemyOverrideFolder = WorkingUncompressed.Address.GetUTF8String(4, 12);
            StageItemFolder = WorkingUncompressed.Address.GetUTF8String(16, 12);
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return ENOV.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(ENOV*)address = Data;
            address.WriteUTF8String(ENOV.Tag, false, 0, 4);
            address.WriteUTF8String(EnemyOverrideFolder, false, 4, 12);
            address.WriteUTF8String(StageItemFolder, false, 16, 12);
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == ENOV.Tag ? new ENOVNode() : null;
        }
    }
}

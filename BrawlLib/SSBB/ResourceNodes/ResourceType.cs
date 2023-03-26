namespace BrawlLib.SSBB.ResourceNodes
{
    //Lower byte is resource type (used for icon index)
    //Upper byte is entry type/flags
    public enum ResourceType
    {
        //Base types
        Unknown,
        Container,

        //Archives
        ARC,
        ARCEntry,
        U8,
        U8Folder,
        BRES,
        BRESEntry,
        BFRES,
        BFRESEntry,
        BFRESGroup,
        MRG,
        BLOC,
        Redirect,
        RARC,
        RARCEntry,
        RARCFolder,
        SARC,
        SARCEntry,

        //Effects
        EFLS,
        EFLSEntry,
        REFF,
        REFFAnimationList,
        REFFEntry,
        REFT,
        REFTImage,

        //Modules
        REL,
        RELImport,
        RELSection,
        DOL,
        RELMethod,
        RELExternalMethod,
        RELInheritance,

        //Misc
        CollisionDef,
        CollisionObj,
        MSBin,
        STPM,
        STDT,
        SCLA,
        SndBgmTitleDataEntry,
        SndBgmTitleDataFolder,
        ClassicStageTbl,
        Sticker,
        TrophyList,
        Trophy,

        //Stage Tables
        TBCL,
        TBGC,
        TBGD,
        TBGM,
        TBLV,
        TBRM,
        TBST,

        //AI
        AI,
        CE,
        CEEntry,
        CEEvent,
        CEString,
        AIPD,
        ATKD,

        //Textures
        TPL,
        TPLTexture,
        TPLPalette,

        //NW4R
        TEX0,
        SharedTEX0,
        PLT0,

        MDL0,
        MDL0Group,
        MDL0Bone,
        MDL0Object,
        MDL0Shader,
        MDL0Color,
        MDL0UV,
        MDL0Normal,
        MDL0Texture,
        MDL0Vertex,
        TEVStage,
        MDL0Material,
        MDL0MaterialEntry,

        CHR0,
        CHR0Entry,

        CLR0,
        CLR0Material,
        CLR0MaterialEntry,

        VIS0,
        SCN0,
        SCN0Camera,
        SCN0Light,
        SCN0Fog,
        SCN0Ambient,
        SCN0LightSet,

        SHP0,
        SHP0VertexSet,
        SHP0Entry,

        SRT0,
        SRT0Entry,
        SRT0Texture,

        PAT0,
        PAT0Entry,
        PAT0Texture,
        PAT0TextureEntry,

        //Audio
        RSAR,
        RSTM,
        RWSD,
        RBNK,
        RSEQ,
        RSARFile,
        RSARSound,
        RSARGroup,
        RSARType,
        RSARBank,
        RWSDDataEntry,
        RSARFileAudioEntry,

        //Groups
        BRESGroup,
        RSARFolder,
        RSARFileSoundGroup,
        RWSDDataGroup,
        RSEQGroup,
        RBNKGroup,

        //Moveset
        MDef,
        NoEdit,
        NoEditFolder,
        NoEditEntry,
        NoEditItem,
        MDefAction,
        MDefActionGroup,
        MDefSubActionGroup,
        MDefMdlVisRef,
        MDefMdlVisSwitch,
        MDefMdlVisGroup,
        MDefActionList,
        MDefSubroutineList,
        MDefActionOverrideList,
        MDefHurtboxList,
        MDefRefList,
        Event,
        Parameter,

        //Nintendo Disc Image
        DiscImage,
        DiscImagePartition,
        DiscImageEntry,

        //J3D
        BMD,
        BMDGroup,
        BTI,

        //Subspace Emmisary
        ADPM,
        BLOCEntry,
        GDOR,
        GEG1,
        ENEMY,
        GMOV,
        GSND,
        GMOT,
        ADSJ,
        GBLK,
        GMPS,
        BGMG,
        GDBF,
        GWAT,
        GCAM,
        GITM,
        GIB2,

        FMDL,

        Havok,
        HavokGroup,

        //BrawlEx
        COSC,
        CSSC,
        CSSCEntry,
        FCFG,
        RSTC,
        RSTCGroup,
        SLTC,

        // Brawl Mods
        MASQ,
        MASQEntry,
        CMM,

        // Items
        ItemFreqNode,
        ItemFreqTableNode,
        ItemFreqTableGroupNode,
        ItemFreqEntryNode,

        // Project +
        ASLS,
        STEX,
        TLST,

        // Folder
        Folder,

        // SSEEX
        SELB,
        SELCTeam
    }
}
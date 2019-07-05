namespace BrawlLib.SSBB.ResourceNodes
{
    //Lower byte is resource type (used for icon index)
    //Upper byte is entry type/flags
    public enum ResourceType : int
    {
        //Base types
        Unknown = 0x0000,
        Container = 0x0101,

        //Archives
        ARC = 0x0202,
        ARCEntry = 0x0300,
        U8 = 0x0423,
        U8Folder = 0x0501,
        BRES = 0x0603,
        BRESEntry = 0x0700,
        BFRES = 0x6800,
        BFRESEntry = 0x6900,
        BFRESGroup = 0x6A01,
        MRG = 0x0801,
        BLOC = 0x5C25,
        Redirect = 0x5E30,
        RARC = 0x6600,
        RARCEntry = 0x6700,
        RARCFolder = 0x6701,
        SARC = 0x6B00,
        SARCEntry = 0x6C00,

        //Effects
        EFLS = 0x0913,
        EFLSEntry = 0x0A00,
        REFF = 0x0B15,
        REFFAnimationList = 0x0C00,
        REFFEntry = 0x0D24,
        REFT = 0x0E1C,
        REFTImage = 0x0F1E,

        //Modules
        REL = 0x1031,
        RELImport = 0x1100,
        RELSection = 0x1200,
        DOL = 0x5A00,
        RELMethod = 0x6300,
        RELExternalMethod = 0x6400,
        RELInheritance = 0x6500,

        //Misc
        CollisionDef = 0x1314,
        MSBin = 0x1404,
        STPM = 0x1520,
        STDT = 0x5B26,
        SCLA = 0x6D34,
        SndBgmTitleDataEntry = 0x6E00,
        SndBgmTitleDataFolder = 0x6E01,
        ClassicStageTbl = 0x6F01,

        //Stage Tables
        TBCL = 0x9000,
        TBGC = 0x9100,
        TBGD = 0x9200,
        TBGM = 0x9300,
        TBLV = 0x9400,
        TBRM = 0x9500,
        TBST = 0x9600,

        //AI
        AI = 0x1616,
        CE = 0x1719,
        CEEntry = 0x1800,
        CEEvent = 0x1900,
        CEString = 0x1A00,
        AIPD = 0x1B17,
        ATKD = 0x1C18,

        //Textures
        TPL = 0x1D21,
        TPLTexture = 0x1E1E,
        TPLPalette = 0x1F22,

        //NW4R
        TEX0 = 0x2005,
        SharedTEX0 = 0x2033,
        PLT0 = 0x2106,

        MDL0 = 0x2207,
        MDL0Group = 0x2301,
        MDL0Bone = 0x2400,
        MDL0Object = 0x2500,
        MDL0Shader = 0x2600,
        TEVStage = 0x2700,
        MDL0Material = 0x2800,

        CHR0 = 0x2908,
        CHR0Entry = 0x2A00,

        CLR0 = 0x2B09,
        CLR0Material = 0x2C00,
        CLR0MaterialEntry = 0x2D00,

        VIS0 = 0x2E0A,
        SCN0 = 0x381F,
        SCN0Camera = 0x8000,
        SCN0Light = 0x8100,
        SCN0Fog = 0x8200,
        SCN0Ambient = 0x8300,
        SCN0LightSet = 0x8400,

        SHP0 = 0x2F0B,
        SHP0VertexSet = 0x5900,
        SHP0Entry = 0x3000,

        SRT0 = 0x310C,
        SRT0Entry = 0x3200,
        SRT0Texture = 0x3300,

        PAT0 = 0x341D,
        PAT0Entry = 0x3500,
        PAT0Texture = 0x3600,
        PAT0TextureEntry = 0x3700,

        //Audio
        RSAR = 0x390D,
        RSTM = 0x3A0E,
        RWSD = 0x3B00,
        RBNK = 0x3C00,
        RSEQ = 0x3D00,
        RSARFile = 0x3E0F,
        RSARSound = 0x3F00,
        RSARGroup = 0x4010,
        RSARType = 0x4111,
        RSARBank = 0x4212,
        RWSDDataEntry = 0x4300,
        RSARFileAudioEntry = 0x4400,

        //Groups
        BRESGroup = 0x4501,
        RSARFolder = 0x4601,
        RSARFileSoundGroup = 0x4701,
        RWSDDataGroup = 0x4801,
        RSEQGroup = 0x4901,
        RBNKGroup = 0x4A01,

        //Moveset
        MDef = 0x4B1A,
        NoEditFolder = 0x4C01,
        NoEditEntry = 0x4C00,
        MDefActionGroup = 0x4D01,
        MDefSubActionGroup = 0x4E01,
        MDefMdlVisRef = 0x4F01,
        MDefMdlVisSwitch = 0x5001,
        MDefMdlVisGroup = 0x5101,
        MDefActionList = 0x5201,
        MDefSubroutineList = 0x5301,
        MDefActionOverrideList = 0x5401,
        MDefHurtboxList = 0x5501,
        MDefRefList = 0x5601,
        Event = 0x571B,
        Parameter = 0x5800,

        //Nintendo Disc Image
        DiscImage = 0x5B00,
        DiscImagePartition = 0x5C01,
        DiscImageEntry = 0x6000,

        //J3D
        BMD = 0x6100,
        BMDGroup = 0x6101,
        BTI = 0x6200,

        //Subspace Emmisary
        GDOR = 0x5D27,
        GEG1 = 0x5D28,
        ENEMY = 0x5D29,
        GMOV = 0x5D2A,
        GSND = 0x5D2B,
        GMOT = 0x5D2C,
        ADSJ = 0x5D2D,
        GBLK = 0x5D2E,
        GMPS = 0x5D2F,
        BGMG = 0x5F32,
        GDBF = 0x5D35,
        GWAT = 0x5D36,
        GCAM = 0x5D37,
        GITM = 0x5D38,
        GIB2 = 0x5D39,

        FMDL = 0x6D00,

        Havok = 0x8500,
        HavokGroup = 0x8501,

        //BrawlEx
        COSC = 0x8600,
        CSSC = 0x8700,
        CSSCEntry = 0x8D00,
        FCFG = 0x8800,
        RSTC = 0x8900,
        RSTCGroup = 0x8901,
        SLTC = 0x8A00,

        // Project M
        MASQ = 0x8B00,
        MASQEntry = 0x8C00,

        // Items
        ItemFreqNode = 0x8B01,
        ItemFreqTableNode = 0x8C01,
        ItemFreqTableGroupNode = 0x8D01,
        ItemFreqEntryNode = 0x8D3A,
    }
}
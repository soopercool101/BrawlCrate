namespace BrawlLib.Wii.Textures
{
    public enum WiiPixelFormat : uint
    {
        I4 = 0,
        I8 = 1,
        IA4 = 2,
        IA8 = 3,
        RGB565 = 4,
        RGB5A3 = 5,
        RGBA8 = 6,
        CI4 = 8,
        CI8 = 9,

        //CI14X2 = 10,
        CMPR = 14

        //A8 = 0x21,
        //Z8 = 0x11,
        //Z16 = 0x13,
        //Z24X8 = 0x16,
    }

    public enum WiiPixelFormatv2 : uint
    {
        GX_TF_I4 = 0x0,
        GX_TF_I8 = 0x1,
        GX_TF_IA4 = 0x2,
        GX_TF_IA8 = 0x3,
        GX_TF_RGB565 = 0x4,
        GX_TF_RGB5A3 = 0x5,
        GX_TF_RGBA8 = 0x6,
        GX_TF_CMPR = 0xE,

        GX_CTF_R4 = 0x20,
        GX_CTF_RA4 = 0x22,
        GX_CTF_RA8 = 0x23,
        GX_CTF_YUVA8 = 0x26,
        GX_CTF_A8 = 0x27,
        GX_CTF_R8 = 0x28,
        GX_CTF_G8 = 0x29,
        GX_CTF_B8 = 0x2A,
        GX_CTF_RG8 = 0x2B,
        GX_CTF_GB8 = 0x2C,

        GX_TF_Z8 = 0x11,
        GX_TF_Z16 = 0x13,
        GX_TF_Z24X8 = 0x16,

        GX_CTF_Z4 = 0x30,
        GX_CTF_Z8M = 0x39,
        GX_CTF_Z8L = 0x3A,
        GX_CTF_Z16L = 0x3C
    }

    public enum WiiPaletteFormat : uint
    {
        IA8 = 0,
        RGB565 = 1,
        RGB5A3 = 2
    }
}
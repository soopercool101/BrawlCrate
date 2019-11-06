namespace BrawlLib.Wii.Models
{
    public enum WiiVertexComponentType : byte
    {
        UInt8 = 0,
        Int8 = 1,
        UInt16 = 2,
        Int16 = 3,
        Float = 4
    }

    public enum WiiColorComponentType : byte
    {
        RGB565 = 0,
        RGB8 = 1,
        RGBX8 = 2,
        RGBA4 = 3,
        RGBA6 = 4,
        RGBA8 = 5
    }

    public enum VertexFormats
    {
        Position,
        Normal,
        Diffuse,
        None
    }

    public enum WiiBeginMode : byte
    {
        PosMtx = 0x20,
        NorMtx = 0x28,
        TexMtx = 0x30,
        LightMtx = 0x38,
        Quads = 0x80,
        TriangleList = 0x90,
        TriangleStrip = 0x98,
        TriangleFan = 0xA0,
        Lines = 0xA8,
        LineStrip = 0xB0,
        Points = 0xB8
    }
}
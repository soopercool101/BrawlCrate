using System;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Textures;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct TPLHeader
    {
        public const int Size = 0xC;
        public const uint Tag = 0x30AF2000;

        public uint _tag;
        public buint _numEntries;
        public buint _dataOffset;

        public TPLTableOffset* Offsets { get { return (TPLTableOffset*)(Address + _dataOffset); } }

        public TPLTextureHeader* GetTextureEntry(int index) { if (Offsets[index]._textureOffset > 0) return (TPLTextureHeader*)(Address + Offsets[index]._textureOffset); return null; }
        public TPLPaletteHeader* GetPaletteEntry(int index) { if (Offsets[index]._paletteOffset > 0) return (TPLPaletteHeader*)(Address + Offsets[index]._paletteOffset); return null; }
        
        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct TPLTableOffset
    {
        public const int Size = 8;

        public buint _textureOffset;
        public buint _paletteOffset;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct TPLTextureHeader
    {
        public const int Size = 0x24;

        public bushort _height;
        public bushort _width;
        public buint _format;
        public buint _data;
        public buint _wrapS;

        public buint _wrapT;
        public buint _minFilter;
        public buint _magFilter;
        public bfloat _LODBias;

        public byte _edgeLODEnable;
        public byte _minLOD;
        public byte _maxLOD;
        public byte _pad;

        public WiiPixelFormat PixelFormat { get { return (WiiPixelFormat)(uint)_format; } set { _format = (uint)value; } }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct TPLPaletteHeader
    {
        public const int Size = 0xC;

        public bushort _numEntries;
        public byte _unpacked;
        public byte _pad;
        public buint _format;
        public buint _data;
        
        public WiiPaletteFormat PaletteFormat { get { return (WiiPaletteFormat)(uint)_format; } set { _format = (uint)value; } }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
    }
}
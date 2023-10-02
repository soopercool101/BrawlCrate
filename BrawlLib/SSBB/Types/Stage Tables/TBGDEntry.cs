using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBGDEntry // TBGD
    {
        public const int Size = 0x14;

        public byte _villagerId;                // 0x00
        public byte _unk0x01;
        public byte _unk0x02;
        public byte _unk0x03;
        public bfloat _animationPlaybackSpeed;  // 0x04
        public bool8 _playAnim1;                // 0x08
        public bool8 _playAnim2;                // 0x09
        public bool8 _playAnim3;                // 0x0A
        public bool8 _playAnim4;                // 0x0B
        public bool8 _playAnim5;                // 0x0C
        public bool8 _playAnim6;                // 0x0D
        public bool8 _playAnim7;                // 0x0E
        public bool8 _playAnim8;                // 0x0F
        public bool8 _playAnim9;                // 0x10
        public bool8 _playAnim10;               // 0x11
        public bool8 _playAnim11;               // 0x12
        public bool8 _playAnim12;               // 0x13
    }
}
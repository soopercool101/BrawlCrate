using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Stage_Tables
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBGDEntry // TBGD
    {
        public const int Size = 0x14;

        public byte _unk0x00;   // ID?
        public byte _unk0x01;   // Padding?
        public byte _unk0x02;   // Padding?
        public byte _unk0x03;   // Padding?
        public bfloat _animationPlaybackSpeed;
        public bool8 _playAnim1;
        public bool8 _playAnim2;
        public bool8 _playAnim3;
        public bool8 _playAnim4;
        public bool8 _playAnim5;
        public bool8 _playAnim6;
        public bool8 _playAnim7;
        public bool8 _playAnim8;
        public bool8 _playAnim9;
        public bool8 _playAnim10;
        public bool8 _playAnim11;
        public bool8 _playAnim12;
    }
}
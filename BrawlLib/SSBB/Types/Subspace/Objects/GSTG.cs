using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GSTGEntry
    {
        public const int Size = 0x1C;

        public bfloat _posX;
        public bfloat _posY;
        public bfloat _posZ;
        public bfloat _rotX;
        public bfloat _rotY;
        public bfloat _rotZ;
        public byte _modelDataIndex;
        public byte _collisionDataIndex;
        public byte _unknown0x1A;
        public byte _unknown0x1B;
    }
}
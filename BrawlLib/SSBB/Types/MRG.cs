using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MRGHeader
    {
        public const int Size = 0x20;

        public buint _numFiles;
        public fixed int padding[7];

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

        public MRGFileHeader* First => (MRGFileHeader*) ((byte*) Address + Size);

        public MRGHeader(uint numFiles)
        {
            _numFiles = numFiles;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MRGFileHeader
    {
        public const int Size = 0x20;

        public bint _fileOffset;
        public bint _fileSize;
        public fixed int padding[6];

        public MRGFileHeader(int size, int offset)
        {
            _fileOffset = offset;
            _fileSize = size;
        }

        private MRGFileHeader* Address
        {
            get
            {
                fixed (MRGFileHeader* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public VoidPtr Data => (int) _fileOffset;
        public int Length => _fileSize;
        public MRGFileHeader* Next => (MRGFileHeader*) ((byte*) Address + Size);
    }
}
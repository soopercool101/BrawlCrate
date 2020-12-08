using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    //Format    Keys    Total    Size

    //F3F        100        200        1208
    //F1F        200        200        800
    //F6B        100        200        616
    //F4B       100        200        416
    //F1B        200        200        208

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct I12Header
    {
        public const int Size = 8;

        public bushort _numFrames;
        public bushort _pad;
        public bfloat _frameScale;

        public I12Header(int entries, float frameScale)
        {
            _numFrames = (ushort) entries;
            _pad = 0;
            _frameScale = frameScale;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public I12Entry* Data => (I12Entry*) (Address + Size);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct I12Entry
    {
        public const int Size = 12;

        public bfloat _index;
        public bfloat _value;
        public bfloat _tangent;

        public I12Entry(float index, float value, float tangent)
        {
            _index = index;
            _value = value;
            _tangent = tangent;
        }
    }

    //FB6Header* header;
    //FB6Entry* pFloor, pCeil;

    //pFloor = pCeil + 1;

    //float scale = header->_scale;
    //float base = header->_base;

    //float valFloor = scale * pFloor->_step + base;
    //float valCeil = scale * pCeil->_step + base;

    //float expFloor = pFloor->_exp / 256.0f;
    //float expCeil = pCeil->_exp / 256.0f;

    //float frameOffset = index - pData->_index;
    //float frameTotal = pCeil->_index - pData->_index;

    //float valDiff = valFloor - valCeil;

    //float frameScale = 1.0f / frameTotal;

    ////Equals framescale
    ////float f9 = -(frameTotal * (frameScale * frameScale) - (frameScale + frameScale));
    ////float f3 = frameOffset * f9;

    //float f3 = frameOffset * frameScale;

    //float f8 = f3 - 1.0;

    //float newScale = frameOffset * f8;
    //float newStep = (f8 * expFloor) + (f3 * expCeil);
    //float newBase = (f3 * f3) * (2.0 * f3 - 3.0) * valDiff + valFloor;

    //return newScale * newStep + newBase;

    //FB6 uses precalculated Hermite splines

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct I6Header
    {
        public const int Size = 16;

        public bushort _numFrames;
        public bushort _unk1;
        public bfloat _frameScale; // = 1 / num frames. Percent each animation frame takes up.
        public bfloat _step;
        public bfloat _base;

        public I6Header(int frames, float frameScale, float step, float floor)
        {
            _numFrames = (ushort) frames;
            _unk1 = 0;
            _frameScale = frameScale;
            _step = step;
            _base = floor;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public I6Entry* Data => (I6Entry*) (Address + Size);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct I6Entry
    {
        public const int Size = 6;

        public bushort _data;
        public bushort _step;
        public bshort _exp;

        public I6Entry(int index, int step, float tangent)
        {
            _data = (ushort) (index << 5);
            _step = (ushort) step;

            tangent *= 256.0f;
            if (tangent < 0.0f)
            {
                tangent -= 0.5f;
            }
            else
            {
                tangent += 0.5f;
            }

            _exp = (short) ((int) tangent).Clamp(-32768, 32767);
        }

        public int FrameIndex
        {
            get => _data >> 5;
            set => _data = (ushort) (value << 5);
        }

        public int Step
        {
            get => _step;
            set => _step = (ushort) value;
        }

        public float Tangent
        {
            get => _exp / 256.0f;
            set => _exp = (short) ((int) (value * 256.0f + 0.5f)).Clamp(-32768, 32767);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct I4Header
    {
        public const int Size = 16;

        public bushort _entries;
        public bushort _unk;
        public bfloat _frameScale;
        public bfloat _step;
        public bfloat _base;

        public I4Header(int entries, float frameScale, float step, float floor)
        {
            _entries = (ushort) entries;
            _unk = 0;
            _frameScale = frameScale;
            _step = step;
            _base = floor;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public I4Entry* Data => (I4Entry*) (Address + Size);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct I4Entry
    {
        public const int Size = 4;

        public buint _data;

        public I4Entry(int index, int step, float tangent)
        {
            tangent *= 32.0f;
            if (tangent < 0)
            {
                tangent -= 0.5f;
            }
            else
            {
                tangent += 0.5f;
            }

            _data = (uint) ((index << 24) | ((step & 0xFFF) << 12) | (((int) tangent).Clamp(-2048, 2047) & 0xFFF));
        }

        public int FrameIndex
        {
            get => (int) _data._data & 0xFF;
            set => _data._data = (_data._data & 0xFFFFFF00) | ((uint) value & 0xFF);
        }

        public int Step
        {
            get => ((int) _data >> 12) & 0xFFF;
            set => _data = (_data & 0xFF000FFF) | (((uint) value & 0xFFF) << 12);
        }

        public float Tangent
        {
            get => (((int) _data << 20) >> 20) / 32.0f;
            set => _data = (_data & 0xFFFFF000) | (uint) ((int) (value * 32.0f) & 0xFFF);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct L1Header
    {
        public const int Size = 8;

        public bfloat _step;
        public bfloat _base;

        public L1Header(float step, float floor)
        {
            _step = step;
            _base = floor;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public byte* Data => (byte*) Address + Size;
    }
}
using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct ADPCMState
    {
        public byte* _srcPtr;
        public int _sampleIndex;
        public short _cps, _cyn1, _cyn2, _ps, _yn1, _yn2, _lps, _lyn1, _lyn2;
        public short[] _coefs;

        public ADPCMState(byte* srcPtr, short yn1, short yn2, short[] coefs)
        {
            _srcPtr = srcPtr;
            _sampleIndex = 0;
            _cps = _ps = _lps = 0;
            _cyn1 = _lyn1 = _yn1 = yn1;
            _cyn2 = _lyn2 = _yn2 = yn2;
            _coefs = coefs;
        }

        public ADPCMState(byte* srcPtr, short ps, short yn1, short yn2, short[] coefs)
        {
            _srcPtr = srcPtr;
            _sampleIndex = 0;
            _cps = _ps = ps;
            _lps = ps;
            _cyn1 = _yn1 = yn1;
            _cyn2 = _yn2 = yn2;
            _lyn1 = yn1;
            _lyn2 = yn2;
            _coefs = coefs;
        }

        public ADPCMState(byte* srcPtr, short ps, short yn1, short yn2, short lps, short lyn1, short lyn2,
                          short[] coefs)
        {
            _srcPtr = srcPtr;
            _sampleIndex = 0;
            _cps = _ps = ps;
            _lps = lps;
            _cyn1 = _yn1 = yn1;
            _cyn2 = _yn2 = yn2;
            _lyn1 = lyn1;
            _lyn2 = lyn2;
            _coefs = coefs;
        }

        public void InitBlock()
        {
            _cps = _ps;
            _cyn1 = _yn1;
            _cyn2 = _yn2;
        }

        public void InitLoop()
        {
            _cps = _lps;
            _cyn1 = _lyn1;
            _cyn2 = _lyn2;
        }

        public short ReadSample()
        {
            int outSample, scale, cIndex;

            if (_sampleIndex % 14 == 0)
            {
                _cps = *_srcPtr++;
            }

            if ((_sampleIndex++ & 1) == 0)
            {
                outSample = *_srcPtr >> 4;
            }
            else
            {
                outSample = *_srcPtr++ & 0x0F;
            }

            if (outSample >= 8)
            {
                outSample -= 16;
            }

            scale = 1 << (_cps & 0x0F);
            cIndex = (_cps >> 4) << 1;

            outSample = (0x400 + ((scale * outSample) << 11) + _coefs[cIndex.Clamp(0, 15)] * _cyn1 +
                         _coefs[(cIndex + 1).Clamp(0, 15)] * _cyn2) >> 11;

            _cyn2 = _cyn1;
            return _cyn1 = (short) outSample.Clamp(-32768, 32767);
        }
    }
}
using System;
using System.Runtime.InteropServices;
using DS = BrawlLib.Platform.Win32.DirectSound;

namespace BrawlLib.Internal.Audio
{
    internal unsafe class wAudioBuffer : AudioBuffer
    {
        private readonly wAudioProvider _parent;
        private DS.IDirectSoundBuffer8 _dsb8;

        internal override int PlayCursor
        {
            get
            {
                uint pos;
                _dsb8.GetCurrentPosition(&pos, null);
                return (int) pos;
            }
            set => _dsb8.SetCurrentPosition((uint) value);
        }

        public override int Volume
        {
            get
            {
                _dsb8.GetVolume(out int vol);
                return vol;
            }
            set => _dsb8.SetVolume(value);
        }

        public override int Pan
        {
            get
            {
                _dsb8.GetPan(out int pan);
                return pan;
            }
            set => _dsb8.SetPan(value);
        }

        //internal wAudioBuffer(wAudioProvider parent, DS.IDirectSoundBuffer8 buffer) { _dsb8 = buffer; }
        internal wAudioBuffer(wAudioProvider parent, ref DS.DSBufferDesc desc)
        {
            _parent = parent;

            if (desc.dwBufferBytes == 0)
            {
                return;
            }

            _parent._ds8.CreateSoundBuffer(ref desc, out _dsb8, IntPtr.Zero);

            _format = desc.lpwfxFormat->wFormatTag;
            _frequency = (int) desc.lpwfxFormat->nSamplesPerSec;
            _channels = desc.lpwfxFormat->nChannels;
            _bitsPerSample = desc.lpwfxFormat->wBitsPerSample;
            _dataLength = (int) desc.dwBufferBytes;
            _blockAlign = _bitsPerSample * _channels / 8;
            _sampleLength = _dataLength / _blockAlign;
        }

        public override void Dispose()
        {
            if (_dsb8 != null)
            {
                Marshal.FinalReleaseComObject(_dsb8);
                _dsb8 = null;
            }

            base.Dispose();
        }

        public override BufferData Lock(int offset, int length)
        {
            BufferData data = new BufferData();

            offset = offset.Align(_blockAlign);
            length = length.Align(_blockAlign);

            data._dataOffset = offset;
            data._dataLength = length;
            data._sampleOffset = offset / _blockAlign;
            data._sampleLength = length / _blockAlign;

            if (length != 0)
            {
                _dsb8.Lock((uint) offset, (uint) length, out IntPtr addr1, out uint len1, out IntPtr addr2,
                    out uint len2, 0);

                data._part1Address = addr1;
                data._part1Length = (int) len1;
                data._part1Samples = (int) len1 / _blockAlign;

                data._part2Address = addr2;
                data._part2Length = (int) len2;
                data._part2Samples = (int) len2 / _blockAlign;
            }

            return data;
        }

        public override void Unlock(BufferData data)
        {
            _dsb8.Unlock(data._part1Address, (uint) data._part1Length, data._part2Address, (uint) data._part2Length);
        }

        public override void Play()
        {
            try
            {
                _dsb8.Play(0, 0, DS.DSBufferPlayFlags.Looping);
            }
            catch
            {
                // ignored
            }
        }

        public override void Stop()
        {
            _dsb8.Stop();
        }
    }
}
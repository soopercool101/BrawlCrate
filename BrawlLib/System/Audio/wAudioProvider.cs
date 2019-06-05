using System.Runtime.InteropServices;
using System.Windows.Forms;
using DS = System.Win32.DirectSound;

namespace System.Audio
{
    internal unsafe class wAudioProvider : AudioProvider
    {
        internal DS.IDirectSound8 _ds8;

        internal wAudioProvider(AudioDevice device)
        {
            _device = device ?? wAudioDevice.DefaultPlaybackDevice;

            var guid = ((wAudioDevice) _device)._guid;
            DS.DirectSoundCreate8(&guid, out _ds8, IntPtr.Zero);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_ds8 != null)
            {
                Marshal.FinalReleaseComObject(_ds8);
                _ds8 = null;
            }
        }

        public override void Attach(Control owner)
        {
            _ds8.SetCooperativeLevel(owner.Handle, DS.DSCooperativeLevel.Normal);
        }

        public override AudioBuffer CreateBuffer(IAudioStream target)
        {
            var size = AudioBuffer.DefaultBufferSpan * target.Frequency * target.Channels * target.BitsPerSample / 8;

            var fmt = new WaveFormatEx(target.Format, target.Channels, target.Frequency, target.BitsPerSample);

            var flags = DS.DSBufferCapsFlags.CtrlVolume | DS.DSBufferCapsFlags.LocDefer |
                        DS.DSBufferCapsFlags.GlobalFocus | DS.DSBufferCapsFlags.GetCurrentPosition2;
            var desc = new DS.DSBufferDesc((uint) size, flags, &fmt, Guid.Empty);

            return new wAudioBuffer(this, ref desc) {_source = target, _owner = this};
        }

        public override string ToString()
        {
            return "DirectSound 8";
        }
    }
}
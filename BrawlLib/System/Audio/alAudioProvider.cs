using System.Windows.Forms;
using System.Runtime.InteropServices;
using OpenTK.Audio;

namespace System.Audio
{
    unsafe class alAudioProvider : AudioProvider
    {
        private AudioContext context;

        internal alAudioProvider()
        {
            context = new AudioContext();
        }
        public override void Dispose()
        {
            base.Dispose();
            if (context != null) context.Dispose();
            context = null;
        }

        public override void Attach(Control owner) { }

        public override AudioBuffer CreateBuffer(IAudioStream target)
        {
            int size = AudioBuffer.DefaultBufferSpan * target.Frequency * target.Channels * target.BitsPerSample / 8;

            WaveFormatEx fmt = new WaveFormatEx(target.Format, target.Channels, target.Frequency, target.BitsPerSample);

            return new alAudioBuffer(this, fmt, target.Samples) { _source = target, _owner = this };
        }

        public override string ToString() {
            return "OpenAL wrapper";
        }
    }
}

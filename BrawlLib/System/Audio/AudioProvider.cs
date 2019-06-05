using System.Collections.Generic;
using System.Windows.Forms;

namespace System.Audio
{
    public abstract class AudioProvider : IDisposable
    {
        [Flags]
        public enum AudioProviderType
        {
            None = 0,
            DirectSound = 1,
            OpenAL = 2,
            All = ~0
        }

        public static AudioProviderType AvailableTypes = AudioProviderType.All;

        internal List<AudioBuffer> _buffers = new List<AudioBuffer>();
        internal AudioDevice _device;
        public AudioDevice Device => _device;
        public List<AudioBuffer> Buffers => _buffers;

        public virtual void Dispose()
        {
            foreach (var buffer in _buffers) buffer.Dispose();

            _buffers.Clear();
            GC.SuppressFinalize(this);
        }

        public static AudioProvider Create(AudioDevice device)
        {
            if (AvailableTypes.HasFlag(AudioProviderType.DirectSound))
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        if (IntPtr.Size <= 4) return new wAudioProvider(device);

                        break;
                }

            if (device == null && AvailableTypes.HasFlag(AudioProviderType.OpenAL))
                try
                {
                    return new alAudioProvider();
                }
                catch (TypeInitializationException)
                {
                }

            return null;
        }

        ~AudioProvider()
        {
            Dispose();
        }

        public abstract void Attach(Control owner);

        public abstract AudioBuffer CreateBuffer(IAudioStream target);
    }
}
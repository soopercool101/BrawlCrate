using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrawlLib.Internal.Audio
{
    public abstract class AudioProvider : IDisposable
    {
        internal AudioDevice _device;
        public AudioDevice Device => _device;

        internal List<AudioBuffer> _buffers = new List<AudioBuffer>();
        public List<AudioBuffer> Buffers => _buffers;

        [Flags]
        public enum AudioProviderType
        {
            None = 0,
            DirectSound = 1,
            OpenAL = 2,
            All = ~0
        };

        public static AudioProviderType AvailableTypes = AudioProviderType.All;

        public static AudioProvider Create(AudioDevice device)
        {
            if (AvailableTypes.HasFlag(AudioProviderType.DirectSound))
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        if (IntPtr.Size <= 4)
                        {
                            return new wAudioProvider(device);
                        }

                        break;
                }
            }

            if (device == null && AvailableTypes.HasFlag(AudioProviderType.OpenAL))
            {
                try
                {
                    return new alAudioProvider();
                }
                catch (TypeInitializationException)
                {
                }
            }

            return null;
        }

        ~AudioProvider()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            foreach (AudioBuffer buffer in _buffers)
            {
                buffer.Dispose();
            }

            _buffers.Clear();
            GC.SuppressFinalize(this);
        }

        public abstract void Attach(Control owner);

        public abstract AudioBuffer CreateBuffer(IAudioStream target);
    }
}
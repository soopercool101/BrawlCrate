using BrawlLib.Platform;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal.Audio
{
    internal unsafe class wAudioDevice : AudioDevice
    {
        internal Guid _guid;

        private wAudioDevice()
        {
        }

        private wAudioDevice(Guid guid, string desc, string driver)
        {
            _guid = guid;
            _description = desc;
            _driver = driver;
        }

        internal new static AudioDevice[] PlaybackDevices
        {
            get
            {
                List<AudioDevice> list = new List<AudioDevice>();
                GCHandle handle = GCHandle.Alloc(list);
                try
                {
                    Win32.DirectSound.DirectSoundEnumerate(EnumCallback, (IntPtr) handle);
                }
                finally
                {
                    handle.Free();
                }

                return list.ToArray();
            }
        }

        internal new static AudioDevice DefaultPlaybackDevice
        {
            get
            {
                Guid g1 = Win32.DirectSound.DefaultPlaybackGuid;
                Win32.DirectSound.GetDeviceID(ref g1, out Guid g2);
                wAudioDevice dev = new wAudioDevice {_guid = g2};

                GCHandle handle = GCHandle.Alloc(dev);
                try
                {
                    Win32.DirectSound.DirectSoundEnumerate(EnumCallback, (IntPtr) handle);
                }
                finally
                {
                    handle.Free();
                }

                return dev;
            }
        }

        public new static AudioDevice DefaultVoicePlaybackDevice
        {
            get
            {
                Guid g1 = Win32.DirectSound.DefaultVoicePlaybackGuid;
                Win32.DirectSound.GetDeviceID(ref g1, out Guid g2);
                wAudioDevice dev = new wAudioDevice {_guid = g2};

                GCHandle handle = GCHandle.Alloc(dev);
                try
                {
                    Win32.DirectSound.DirectSoundEnumerate(EnumCallback, (IntPtr) handle);
                }
                finally
                {
                    handle.Free();
                }

                return dev;
            }
        }

        private static bool EnumCallback(Guid* guid, sbyte* desc, sbyte* module, IntPtr context)
        {
            if (guid == null)
            {
                return true;
            }

            object ctx = ((GCHandle) context).Target;
            if (ctx is List<AudioDevice>)
            {
                ((List<AudioDevice>) ctx).Add(new wAudioDevice(*guid, new string(desc), new string(module)));
                return true;
            }

            if (ctx is wAudioDevice)
            {
                wAudioDevice dev = ctx as wAudioDevice;
                if (*guid == dev._guid)
                {
                    dev._description = new string(desc);
                    dev._driver = new string(module);
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
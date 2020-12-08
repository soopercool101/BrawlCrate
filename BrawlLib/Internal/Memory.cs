using BrawlLib.Platform;
using System;
using System.IO;

namespace BrawlLib.Internal
{
    public static unsafe class Memory
    {
        public static void Move(VoidPtr dst, VoidPtr src, uint size)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    Win32.MoveMemory(dst, src, size);
                    break;
                case PlatformID.MacOSX:
                    OSX.memmove(dst, src, size);
                    break;
                case PlatformID.Unix:
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                    {
                        goto case PlatformID.MacOSX;
                    }
                    else
                    {
                        Linux.memmove(dst, src, size);
                    }

                    break;
            }
        }

        internal static void Fill(VoidPtr dest, uint length, byte value)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                {
                    Win32.FillMemory(dest, length, value);
                    break;
                }

                case PlatformID.MacOSX:
                {
                    OSX.memset(dest, value, length);
                    break;
                }

                case PlatformID.Unix:
                {
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                    {
                        goto case PlatformID.MacOSX;
                    }

                    Linux.memset(dest, value, length);

                    break;
                }
            }
        }
    }
}
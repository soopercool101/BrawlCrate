using BrawlLib.Imaging;
using BrawlLib.Wii.Textures;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace BrawlLib.Internal.Drawing.Imaging
{
    [Flags]
    public enum ColorPaletteFlags : int
    {
        None = 0,
        HasAlpha = 1,
        Greyscale = 2,
        Halftone = 4
    }

    public static class ColorPaletteExtension
    {
        public static ColorPalette CreatePalette(ColorPaletteFlags flags, int entries)
        {
            ColorPalette pal = (ColorPalette) typeof(ColorPalette).InvokeMember("",
                BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Instance, null, null,
                new object[] {entries});
            pal.SetFlags(flags);
            return pal;
        }

        public static void SetFlags(this ColorPalette pal, ColorPaletteFlags flags)
        {
            typeof(ColorPalette).GetField("flags", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(pal, (int) flags);
        }

        public static void Clamp(this ColorPalette pal, WiiPaletteFormat format)
        {
            switch (format)
            {
                case WiiPaletteFormat.IA8:
                {
                    for (int i = 0; i < pal.Entries.Length; i++)
                    {
                        pal.Entries[i] = (Color) (IA8Pixel) pal.Entries[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB565:
                {
                    for (int i = 0; i < pal.Entries.Length; i++)
                    {
                        pal.Entries[i] = (Color) (wRGB565Pixel) pal.Entries[i];
                    }

                    break;
                }

                case WiiPaletteFormat.RGB5A3:
                {
                    for (int i = 0; i < pal.Entries.Length; i++)
                    {
                        pal.Entries[i] = (Color) (wRGB5A3Pixel) pal.Entries[i];
                    }

                    break;
                }
            }
        }

        public static int FindMatch(this ColorPalette pal, ARGBPixel pixel)
        {
            int bestDist = int.MaxValue, bestIndex = 0;
            for (int i = 0, c = pal.Entries.Length; i < c; i++)
            {
                int dist = pixel.DistanceTo(pal.Entries[i]);
                if (dist < bestDist)
                {
                    bestIndex = i;
                    if (dist == 0)
                    {
                        break;
                    }

                    bestDist = dist;
                }
            }

            return bestIndex;
        }
    }
}
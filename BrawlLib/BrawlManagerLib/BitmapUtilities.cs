using BrawlLib.Internal.Drawing;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BrawlLib.BrawlManagerLib
{
    public static class BitmapUtilities
    {
        /// <summary>
        /// Swaps the alpha and value channels of a monochrome image. Transparent areas become black, and black areas become transparent.
        /// </summary>
        public static Bitmap AlphaSwap(Bitmap source)
        {
            Color c;
            if (IsSolidColor(source, out c) && c.A == 0)
            {
                // fully transparent image
                return source;
            }

            Bitmap ret = new Bitmap(source.Width, source.Height);
            for (int x = 0; x < ret.Width; x++)
            {
                for (int y = 0; y < ret.Height; y++)
                {
                    Color fromColor = source.GetPixel(x, y);
                    int toColor = fromColor.A == 0 ? -0x01000000 : fromColor.A * 0x10101 + fromColor.R * 0x1000000;
                    ret.SetPixel(x, y, Color.FromArgb(toColor));
                }
            }

            return ret;
        }

        public static Bitmap ApplyMask(Bitmap source, Bitmap mask)
        {
            source = Resize(source, mask.Size);
            Bitmap ret = new Bitmap(source.Width, source.Height);
            for (int x = 0; x < ret.Width; x++)
            {
                for (int y = 0; y < ret.Height; y++)
                {
                    Color c = source.GetPixel(x, y);
                    ret.SetPixel(x, y, Color.FromArgb(
                        mask.GetPixel(x, y).R,
                        c.R, c.G, c.B));
                }
            }

            return ret;
        }

        /// <summary>
        /// Combines two images. The size of the second (foreground) image will be used for the final image.
        /// </summary>
        public static Bitmap Combine(Bitmap bg, Image fg)
        {
            int w = fg.Width, h = fg.Height;
            Bitmap both = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(both);
            g.DrawImage(Resize(bg, both.Size), 0, 0);
            g.DrawImage(fg, 0, 0);
            return both;
        }

        /// <summary>
        /// Inverts the colors in a bitmap.
        /// </summary>
        public static Bitmap Invert(Bitmap source)
        {
            Bitmap ret = new Bitmap(source.Width, source.Height);
            for (int x = 0; x < ret.Width; x++)
            {
                for (int y = 0; y < ret.Height; y++)
                {
                    Color c = source.GetPixel(x, y);
                    ret.SetPixel(x, y, Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B));
                }
            }

            return ret;
        }

        /// <summary>
        /// Makes a scaled version of an image using BrawlLib's texture converter.
        /// </summary>
        public static Bitmap Resize(Bitmap orig, Size resizeTo)
        {
            return orig.Resize(resizeTo.Width, resizeTo.Height);
        }

        public static Bitmap Border(Bitmap orig, Color color, int penwidth = 1)
        {
            Bitmap b = new Bitmap(orig.Width, orig.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(orig, 0, 0);
            g.DrawRectangle(new Pen(color, penwidth),
                penwidth - 1, penwidth - 1,
                orig.Width - penwidth, orig.Height - penwidth);
            return b;
        }

        /// <summary>
        /// Checks if the given bitmap is a single color. If the result is true, its color will be stored in the second parameter.
        /// </summary>
        public static bool IsSolidColor(Bitmap bmp, out Color c)
        {
            c = bmp.GetPixel(0, 0);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (bmp.GetPixel(x, y) != c)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if an image has any transparency.
        /// </summary>
        public static bool HasAlpha(Bitmap bmp)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (bmp.GetPixel(x, y).A < 255)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if an image has varying RGB values among pixels that are partially or fully opaque.
        /// </summary>
        public static bool HasNonAlpha(Bitmap bmp)
        {
            int? found = null;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color color = bmp.GetPixel(x, y);
                    if (color.A == 0)
                    {
                        continue;
                    }

                    int rgb = color.ToArgb() & 0xFFFFFF;
                    if (found == null)
                    {
                        found = rgb;
                    }
                    else if (rgb != found)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Counts the number of colors in a bitmap, up through the given maximum. If the maximum is reached, the method will return early.
        /// </summary>
        public static int CountColors(Bitmap bmp, int max)
        {
            int count = 0;
            HashSet<Color> colors = new HashSet<Color>();
            for (int y = 0; y < bmp.Height && count < max; y++)
            {
                for (int x = 0; x < bmp.Width && count < max; x++)
                {
                    Color color = bmp.GetPixel(x, y);
                    if (!colors.Contains(color))
                    {
                        colors.Add(color);
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Creates a new image, 2 pixels wider and taller than the original.
        /// Each pixel's alpha value is the largest alpha value among adjacent pixels on the original image.
        /// The color of each pixel will be the color given in the second argument.
        /// </summary>
        public static Bitmap Blur(Bitmap orig, Color blurColor)
        {
            Bitmap b = new Bitmap(orig.Width + 2, orig.Height + 2);
            for (int y = -1; y < orig.Height + 1; y++)
            {
                for (int x = -1; x < orig.Width + 1; x++)
                {
                    byte[] vals =
                    {
                        TryGetAlpha(orig, x - 1, y - 1) ?? 0,
                        TryGetAlpha(orig, x - 1, y) ?? 0,
                        TryGetAlpha(orig, x - 1, y + 1) ?? 0,
                        TryGetAlpha(orig, x, y - 1) ?? 0,
                        TryGetAlpha(orig, x, y) ?? 0,
                        TryGetAlpha(orig, x, y + 1) ?? 0,
                        TryGetAlpha(orig, x + 1, y - 1) ?? 0,
                        TryGetAlpha(orig, x + 1, y) ?? 0,
                        TryGetAlpha(orig, x + 1, y + 1) ?? 0
                    };
                    byte alpha = vals.Max();
                    b.SetPixel(x + 1, y + 1, Color.FromArgb(alpha, blurColor));
                }
            }

            return b;
        }

        /// <summary>
        /// Used for Blur.
        /// </summary>
        private static byte? TryGetAlpha(Bitmap b, int x, int y)
        {
            if (x < 0 || x >= b.Width || y < 0 || y >= b.Height)
            {
                return null;
            }

            return b.GetPixel(x, y).A;
        }

        /// <summary>
        /// Creates a new bitmap with the original bitmap centered on top of the result of the Blur function.
        /// </summary>
        public static Bitmap BlurCombine(Bitmap fg, Color blurColor)
        {
            Bitmap both = Blur(fg, blurColor);
            Graphics g = Graphics.FromImage(both);
            g.DrawImage(fg, 1, 1);
            return both;
        }

        public static bool HasSolidCorners(Bitmap newBitmap)
        {
            int w = newBitmap.Width;
            int h = newBitmap.Height;
            return newBitmap.GetPixel(0, 0).A == 255
                   && newBitmap.GetPixel(w - 1, 0).A == 255
                   && newBitmap.GetPixel(0, h - 1).A == 255
                   && newBitmap.GetPixel(w - 1, h - 1).A == 255;
        }
    }
}
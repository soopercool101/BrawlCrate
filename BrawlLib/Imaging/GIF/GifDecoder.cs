#region .NET Disclaimer/Info

//===============================================================================
//
// gOODiDEA, uland.com
//===============================================================================
//
// $Header :		$  
// $Author :		$
// $Date   :		$
// $Revision:		$
// $History:		$  
//  
//===============================================================================

#endregion

#region Java

/**
 * Class GifDecoder - Decodes a GIF file into one or more frames.
 * <br><pre>
 * Example:
 *    GifDecoder d = new GifDecoder();
 *    d.read("sample.gif");
 *    int n = d.getFrameCount();
 *    for (int i = 0; i < n; i++) {
 *       BufferedImage frame = d.getFrame(i);  // frame i
 *       int t = d.getDelay(i);  // display duration of frame in milliseconds
 *       // do something with frame
 *    }
 * </pre>
 * No copyright asserted on the source code of this class.  May be used for
 * any purpose, however, refer to the Unisys LZW patent for any additional
 * restrictions.  Please forward any corrections to kweiner@fmsware.com.
 *
 * @author Kevin Weiner, FM Software; LZW decoder adapted from John Cristy's ImageMagick.
 * @version 1.03 November 2003
 *
 */

#endregion

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Gif.Components
{
    public class GifDecoder
    {
        /**
		 * File read status: No errors.
		 */
        public static readonly int STATUS_OK = 0;

        /**
		 * File read status: Error decoding file (may be partially decoded)
		 */
        public static readonly int STATUS_FORMAT_ERROR = 1;

        /**
		 * File read status: Unable to open source.
		 */
        public static readonly int STATUS_OPEN_ERROR = 2;

        protected static readonly int MaxStackSize = 4096;
        protected int[] act; // active color table
        protected int bgColor; // background color

        protected int bgIndex; // background color index
        protected Bitmap bitmap;

        protected byte[] block = new byte[256]; // current data block
        protected int blockSize; // block size
        protected int delay; // delay in milliseconds

        // last graphic control extension info
        protected int dispose;
        protected int frameCount, currentFrame;

        protected ArrayList frames; // frames read from current file

        protected int[] gct; // global color table
        protected bool gctFlag; // global color table used
        protected int gctSize; // size of global color table
        protected int height; // full image height
        protected Image image; // current frame

        protected Stream inStream;
        protected bool interlace; // interlace flag

        protected int ix, iy, iw, ih; // current image rectangle

        protected int lastBgColor; // previous bg color

        // 0=no action; 1=leave in place; 2=restore to bg; 3=restore to prev
        protected int lastDispose;
        protected Image lastImage; // previous frame
        protected Rectangle lastRect; // last image rect
        protected int[] lct; // local color table

        protected bool lctFlag; // local color table flag
        protected int lctSize; // local color table size
        protected int loopCount = 1; // iterations; 0 = repeat forever

        private Form owner;
        protected int pixelAspect; // pixel aspect ratio
        protected byte[] pixels;

        protected byte[] pixelStack;
        // max decoder pixel stack size

        // LZW decoder working arrays
        protected short[] prefix;
        protected int status;
        protected byte[] suffix;
        protected int transIndex; // transparent color index
        protected bool transparency; // use transparent color

        protected int width; // full image width

        /**
		 * Gets display duration for specified frame.
		 *
		 * @param n int index of frame
		 * @return delay in milliseconds
		 */
        public int GetDelay(int n)
        {
            //
            delay = -1;
            if (n >= 0 && n < frameCount) delay = ((GifFrame) frames[n]).delay;
            return delay;
        }

        /**
		 * Gets the number of frames read from file.
		 * @return frame count
		 */
        public int GetFrameCount()
        {
            return frameCount;
        }

        /**
		 * Gets the first (or only) image read.
		 *
		 * @return BufferedImage containing first frame, or null if none.
		 */
        public Image GetImage()
        {
            return GetFrame(0);
        }

        /**
		 * Gets the "Netscape" iteration count, if any.
		 * A count of 0 means repeat indefinitiely.
		 *
		 * @return iteration count if one was specified, else 1.
		 */
        public int GetLoopCount()
        {
            return loopCount;
        }

        /**
		 * Creates new frame image from current data (and previous
		 * frames as specified by their disposition codes).
		 */
        private int[] GetPixels(Bitmap bitmap)
        {
            var pixels = new int[3 * image.Width * image.Height];
            var count = 0;
            for (var th = 0; th < image.Height; th++)
            for (var tw = 0; tw < image.Width; tw++)
            {
                var color = bitmap.GetPixel(tw, th);
                pixels[count] = color.R;
                count++;
                pixels[count] = color.G;
                count++;
                pixels[count] = color.B;
                count++;
            }

            return pixels;
        }

        private void SetPixels(int[] pixels)
        {
            var count = 0;
            for (var th = 0; th < image.Height; th++)
            for (var tw = 0; tw < image.Width; tw++)
            {
                var color = Color.FromArgb(pixels[count++]);
                bitmap.SetPixel(tw, th, color);
            }
        }

        protected void SetPixels()
        {
            // expose destination image's pixels as int array
            //		int[] dest =
            //			(( int ) image.getRaster().getDataBuffer()).getData();
            var dest = GetPixels(bitmap);

            // fill in starting image contents based on last image's dispose code
            if (lastDispose > 0)
            {
                if (lastDispose == 3)
                {
                    // use image before last
                    var n = currentFrame - 2;
                    if (n > 0)
                        lastImage = GetFrame(n - 1);
                    else
                        lastImage = null;
                }

                if (lastImage != null)
                {
                    //				int[] prev =
                    //					((DataBufferInt) lastImage.getRaster().getDataBuffer()).getData();
                    var prev = GetPixels(new Bitmap(lastImage));
                    Array.Copy(prev, 0, dest, 0, width * height);
                    // copy pixels

                    if (lastDispose == 2)
                    {
                        // fill last image rect area with background color
                        var g = Graphics.FromImage(image);
                        var c = Color.Empty;
                        if (transparency)
                            c = Color.FromArgb(0, 0, 0, 0); // assume background is transparent
                        else
                            c = Color.FromArgb(lastBgColor);
                        //						c = new Color(lastBgColor); // use given background color
                        Brush brush = new SolidBrush(c);
                        g.FillRectangle(brush, lastRect);
                        brush.Dispose();
                        g.Dispose();
                    }
                }
            }

            // copy each source line to the appropriate place in the destination
            var pass = 1;
            var inc = 8;
            var iline = 0;
            for (var i = 0; i < ih; i++)
            {
                var line = i;
                if (interlace)
                {
                    if (iline >= ih)
                    {
                        pass++;
                        switch (pass)
                        {
                            case 2:
                                iline = 4;
                                break;
                            case 3:
                                iline = 2;
                                inc = 4;
                                break;
                            case 4:
                                iline = 1;
                                inc = 2;
                                break;
                        }
                    }

                    line = iline;
                    iline += inc;
                }

                line += iy;
                if (line < height)
                {
                    var k = line * width;
                    var dx = k + ix; // start of line in dest
                    var dlim = dx + iw; // end of dest line
                    if (k + width < dlim) dlim = k + width; // past dest edge
                    var sx = i * iw; // start of line in source
                    while (dx < dlim)
                    {
                        // map color and insert in destination
                        var index = pixels[sx++] & 0xff;
                        var c = act[index];
                        if (c != 0) dest[dx] = c;
                        dx++;
                    }
                }
            }

            SetPixels(dest);
        }

        /**
		 * Gets the image contents of frame n.
		 *
		 * @return BufferedImage representation of frame, or null if n is invalid.
		 */
        public Image GetFrame(int n)
        {
            Image im = null;
            if (n >= 0 && n < frameCount) im = ((GifFrame) frames[n]).image;
            return im;
        }

        /**
		 * Gets image size.
		 *
		 * @return GIF image dimensions
		 */
        public Size GetFrameSize()
        {
            return new Size(width, height);
        }

        /**
		 * Reads GIF image from stream
		 *
		 * @param BufferedInputStream containing GIF file.
		 * @return read status code (0 = no errors)
		 */
        public int Read(Stream inStream)
        {
            Init();
            if (inStream != null)
            {
                this.inStream = inStream;
                ReadHeader();
                if (!Error())
                {
                    ReadContents();
                    if (currentFrame < 0 || frameCount < 0) status = STATUS_FORMAT_ERROR;
                }

                inStream.Close();
            }
            else
            {
                status = STATUS_OPEN_ERROR;
            }

            return status;
        }

        /**
		 * Reads GIF file from specified file/URL source  
		 * (URL assumed if name contains ":/" or "file:")
		 *
		 * @param name String containing source
		 * @return read status code (0 = no errors)
		 */
        public int Read(string name, Form o)
        {
            owner = o;
            status = STATUS_OK;
            try
            {
                name = name.Trim().ToLower();
                status = Read(new FileInfo(name).OpenRead());
            }
            catch (IOException)
            {
                status = STATUS_OPEN_ERROR;
            }

            return status;
        }

        protected void SkipImageData()
        {
            var data_size = Read();
            var count = 0;
            do
            {
                count = ReadBlock();
            } while (count > 0);
        }

        /**
		 * Decodes LZW image data into pixel array.
		 * Adapted from John Cristy's ImageMagick.
		 */
        protected void DecodeImageData()
        {
            var NullCode = -1;
            var npix = iw * ih;
            int available,
                clear,
                code_mask,
                code_size,
                end_of_information,
                in_code,
                old_code,
                bits,
                code,
                count,
                i,
                datum,
                data_size,
                first,
                top,
                bi,
                pi;

            if (pixels == null || pixels.Length < npix) pixels = new byte[npix]; // allocate new pixel array
            if (prefix == null) prefix = new short[MaxStackSize];

            if (suffix == null) suffix = new byte[MaxStackSize];

            if (pixelStack == null) pixelStack = new byte[MaxStackSize + 1];

            //  Initialize GIF data stream decoder.

            data_size = Read();
            clear = 1 << data_size;
            end_of_information = clear + 1;
            available = clear + 2;
            old_code = NullCode;
            code_size = data_size + 1;
            code_mask = (1 << code_size) - 1;
            for (code = 0; code < clear; code++)
            {
                prefix[code] = 0;
                suffix[code] = (byte) code;
            }

            //  Decode GIF pixel stream.

            datum = bits = count = first = top = pi = bi = 0;

            for (i = 0; i < npix;)
            {
                if (top == 0)
                {
                    if (bits < code_size)
                    {
                        //  Load bytes until there are enough bits for a code.
                        if (count == 0)
                        {
                            // Read a new data block.
                            count = ReadBlock();
                            if (count <= 0) break;

                            bi = 0;
                        }

                        datum += (block[bi] & 0xff) << bits;
                        bits += 8;
                        bi++;
                        count--;
                        continue;
                    }

                    //  Get the next code.

                    code = datum & code_mask;
                    datum >>= code_size;
                    bits -= code_size;

                    //  Interpret the code

                    if (code > available || code == end_of_information) break;

                    if (code == clear)
                    {
                        //  Reset decoder.
                        code_size = data_size + 1;
                        code_mask = (1 << code_size) - 1;
                        available = clear + 2;
                        old_code = NullCode;
                        continue;
                    }

                    if (old_code == NullCode)
                    {
                        pixelStack[top++] = suffix[code];
                        old_code = code;
                        first = code;
                        continue;
                    }

                    in_code = code;
                    if (code == available)
                    {
                        pixelStack[top++] = (byte) first;
                        code = old_code;
                    }

                    while (code > clear)
                    {
                        pixelStack[top++] = suffix[code];
                        code = prefix[code];
                    }

                    first = suffix[code] & 0xff;

                    //  Add a new string to the string table,

                    if (available >= MaxStackSize) break;

                    pixelStack[top++] = (byte) first;
                    prefix[available] = (short) old_code;
                    suffix[available] = (byte) first;
                    available++;
                    if ((available & code_mask) == 0
                        && available < MaxStackSize)
                    {
                        code_size++;
                        code_mask += available;
                    }

                    old_code = in_code;
                }

                //  Pop a pixel off the pixel stack.

                top--;
                pixels[pi++] = pixelStack[top];
                i++;
            }

            for (i = pi; i < npix; i++) pixels[i] = 0; // clear missing pixels
        }

        /**
		 * Returns true if an error was encountered during reading/decoding
		 */
        protected bool Error()
        {
            return status != STATUS_OK;
        }

        /**
		 * Initializes or re-initializes reader
		 */
        protected void Init()
        {
            status = STATUS_OK;
            frameCount = 0;
            currentFrame = 0;
            frames = new ArrayList();
            gct = null;
            lct = null;
        }

        /**
		 * Reads a single byte from the input stream.
		 */
        protected int Read()
        {
            var curByte = 0;
            try
            {
                curByte = inStream.ReadByte();
            }
            catch (IOException)
            {
                status = STATUS_FORMAT_ERROR;
            }

            return curByte;
        }

        /**
		 * Reads next variable length block from input.
		 *
		 * @return number of bytes stored in "buffer"
		 */
        protected int ReadBlock()
        {
            blockSize = Read();
            var n = 0;
            if (blockSize > 0)
            {
                try
                {
                    var count = 0;
                    while (n < blockSize)
                    {
                        count = inStream.Read(block, n, blockSize - n);
                        if (count == -1) break;

                        n += count;
                    }
                }
                catch (IOException)
                {
                }

                if (n < blockSize) status = STATUS_FORMAT_ERROR;
            }

            return n;
        }

        /**
		 * Reads color table as 256 RGB integer values
		 *
		 * @param ncolors int number of colors to read
		 * @return int array containing 256 colors (packed ARGB with full alpha)
		 */
        protected int[] ReadColorTable(int ncolors)
        {
            var nbytes = 3 * ncolors;
            int[] tab = null;
            var c = new byte[nbytes];
            var n = 0;
            try
            {
                n = inStream.Read(c, 0, c.Length);
            }
            catch (IOException)
            {
            }

            if (n < nbytes)
            {
                status = STATUS_FORMAT_ERROR;
            }
            else
            {
                tab = new int[256]; // max size to avoid bounds checks
                var i = 0;
                var j = 0;
                while (i < ncolors)
                {
                    var r = (uint) (c[j++] & 0xff);
                    var g = (uint) (c[j++] & 0xff);
                    var b = (uint) (c[j++] & 0xff);
                    tab[i++] = (int) (0xff000000 | (r << 16) | (g << 8) | b);
                }
            }

            return tab;
        }

        /**
		 * Main file parser.  Reads GIF content blocks.
		 */
        protected void ReadContents()
        {
            var offset = inStream.Position;

            //Scan through file and get the frame count
            frameCount = 0;

            // read GIF file content blocks
            var done = false;
            while (!(done || Error()))
            {
                var code = Read();
                switch (code)
                {
                    case 0x2C: // image separator
                        SkipImage();
                        frameCount++;
                        break;
                    case 0x21:
                        code = Read();
                        switch (code)
                        {
                            case 0xf9: // graphics control extension
                                ReadGraphicControlExt();
                                break;
                            case 0xff: // application extension
                                ReadBlock();
                                var app = "";
                                for (var i = 0; i < 11; i++) app += (char) block[i];
                                if (app.Equals("NETSCAPE2.0"))
                                    ReadNetscapeExt();
                                else
                                    Skip(); // don't care

                                break;
                            default:
                                Skip();
                                break;
                        }

                        break;
                    case 0x3b:
                        done = true;
                        break;
                    case 0x00:
                        break;
                    default:
                        status = STATUS_FORMAT_ERROR;
                        break;
                }
            }

            inStream.Position = offset;

            using (var p = new ProgressWindow(owner, "Decoding Image...", "Reading file, please wait...", false))
            {
                p.Begin(0, frameCount, 0);
                done = false;
                while (!(done || Error()))
                {
                    var code = Read();
                    switch (code)
                    {
                        case 0x2C: // image separator
                            ReadImage();
                            p.Update(currentFrame);
                            break;

                        case 0x21: // extension
                            code = Read();
                            switch (code)
                            {
                                case 0xf9: // graphics control extension
                                    ReadGraphicControlExt();
                                    break;

                                case 0xff: // application extension
                                    ReadBlock();
                                    var app = "";
                                    for (var i = 0; i < 11; i++) app += (char) block[i];
                                    if (app.Equals("NETSCAPE2.0"))
                                        ReadNetscapeExt();
                                    else
                                        Skip(); // don't care

                                    break;

                                default: // uninteresting extension
                                    Skip();
                                    break;
                            }

                            break;

                        case 0x3b: // terminator
                            done = true;
                            break;

                        case 0x00: // bad byte, but keep going and see what happens
                            break;

                        default:
                            status = STATUS_FORMAT_ERROR;
                            break;
                    }
                }

                p.Finish();
            }
        }

        /**
		 * Reads Graphics Control Extension values
		 */
        protected void ReadGraphicControlExt()
        {
            Read(); // block size
            var packed = Read(); // packed fields
            dispose = (packed & 0x1c) >> 2; // disposal method
            if (dispose == 0) dispose = 1; // elect to keep old image if discretionary
            transparency = (packed & 1) != 0;
            delay = ReadShort() * 10; // delay in milliseconds
            transIndex = Read(); // transparent color index
            Read(); // block terminator
        }

        /**
		 * Reads GIF file header information.
		 */
        protected void ReadHeader()
        {
            var id = "";
            for (var i = 0; i < 6; i++) id += (char) Read();
            if (!id.StartsWith("GIF"))
            {
                status = STATUS_FORMAT_ERROR;
                return;
            }

            ReadLSD();
            if (gctFlag && !Error())
            {
                gct = ReadColorTable(gctSize);
                bgColor = gct[bgIndex];
            }
        }

        protected void SkipImage()
        {
            inStream.Seek(8, SeekOrigin.Current);
            var packed = Read();
            lctFlag = (packed & 0x80) != 0;
            lctSize = 2 << (packed & 7);
            if (lctFlag) inStream.Seek(lctSize * 3, SeekOrigin.Current);

            DecodeImageData();
            Skip();
        }

        /**
		 * Reads next frame image
		 */
        protected void ReadImage()
        {
            ix = ReadShort(); // (sub)image position & size
            iy = ReadShort();
            iw = ReadShort();
            ih = ReadShort();

            var packed = Read();
            lctFlag = (packed & 0x80) != 0; // 1 - local color table flag
            interlace = (packed & 0x40) != 0; // 2 - interlace flag
            // 3 - sort flag
            // 4-5 - reserved
            lctSize = 2 << (packed & 7); // 6-8 - local color table size

            if (lctFlag)
            {
                lct = ReadColorTable(lctSize); // read table
                act = lct; // make local table active
            }
            else
            {
                act = gct; // make global table active
                if (bgIndex == transIndex) bgColor = 0;
            }

            var save = 0;
            if (transparency)
            {
                save = act[transIndex];
                act[transIndex] = 0; // set transparent color if specified
            }

            if (act == null) status = STATUS_FORMAT_ERROR; // no color table defined

            if (Error()) return;

            DecodeImageData(); // decode pixel data
            Skip();

            if (Error()) return;

            currentFrame++;

            // create new image to receive frame data
            //		image =
            //			new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB_PRE);

            bitmap = new Bitmap(width, height);
            image = bitmap;
            SetPixels(); // transfer pixel data to image

            frames.Add(new GifFrame(bitmap, delay)); // add image to frame list

            if (transparency) act[transIndex] = save;
            ResetFrame();
        }

        /**
		 * Reads Logical Screen Descriptor
		 */
        protected void ReadLSD()
        {
            // logical screen size
            width = ReadShort();
            height = ReadShort();

            // packed fields
            var packed = Read();
            gctFlag = (packed & 0x80) != 0; // 1   : global color table flag
            // 2-4 : color resolution
            // 5   : gct sort flag
            gctSize = 2 << (packed & 7); // 6-8 : gct size

            bgIndex = Read(); // background color index
            pixelAspect = Read(); // pixel aspect ratio
        }

        /**
		 * Reads Netscape extenstion to obtain iteration count
		 */
        protected void ReadNetscapeExt()
        {
            do
            {
                ReadBlock();
                if (block[0] == 1)
                {
                    // loop count sub-block
                    var b1 = block[1] & 0xff;
                    var b2 = block[2] & 0xff;
                    loopCount = (b2 << 8) | b1;
                }
            } while (blockSize > 0 && !Error());
        }

        /**
		 * Reads next 16-bit value, LSB first
		 */
        protected int ReadShort()
        {
            // read 16-bit value, LSB first
            return Read() | (Read() << 8);
        }

        /**
		 * Resets frame state for reading next image.
		 */
        protected void ResetFrame()
        {
            lastDispose = dispose;
            lastRect = new Rectangle(ix, iy, iw, ih);
            lastImage = image;
            lastBgColor = bgColor;
            //		int dispose = 0;
            //transparency = false;
            //delay = 0;
            lct = null;
        }

        /**
		 * Skips variable length blocks up to and including
		 * next zero length block.
		 */
        protected void Skip()
        {
            do
            {
                ReadBlock();
            } while (blockSize > 0 && !Error());
        }

        public class GifFrame
        {
            public int delay;
            public Image image;

            public GifFrame(Image im, int del)
            {
                image = im;
                delay = del;
            }
        }
    }
}
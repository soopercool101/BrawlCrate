using BrawlLib.Internal;
using BrawlLib.Internal.Drawing.Imaging;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Wii.Textures;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

/* This formula is based on the Median Cut formula used in the GIMP library.
 * Special thanks goes to Spencer Kimball, Peter Mattis, and Adam Moss.
 * http://www.gimp.org/
 */

namespace BrawlLib.Imaging
{
    internal unsafe class MedianCut : IDisposable
    {
        private Bitmap _srcBmp;
        private BitmapData _srcData;

        private readonly int _width, _height, _size;

        private ARGBPixel* _srcPixels;

        private ColorBox* _boxes;
        private int _boxCount;

        private ColorEntry** _groupTable;

        private readonly IDHandler _idFunc;
        private readonly IDConverter _idConv;

        private readonly PixelFormat _outFormat;

        #region Handlers/Converters

        private delegate void IDHandler(ARGBPixel inPixel, ushort* id);

        private static void IA8Handler(ARGBPixel inPixel, ushort* id)
        {
            *(IA8Pixel*) id = inPixel;
        }

        private static void RGB565Handler(ARGBPixel inPixel, ushort* id)
        {
            *(wRGB565Pixel*) id = (wRGB565Pixel) inPixel;
        }

        private static void RGB5A3Handler(ARGBPixel inPixel, ushort* id)
        {
            *(wRGB5A3Pixel*) id = (wRGB5A3Pixel) inPixel;
        }

        private delegate void IDConverter(ushort* id, out ARGBPixel outPixel);

        private static void IA8Converter(ushort* id, out ARGBPixel outPixel)
        {
            outPixel = *(IA8Pixel*) id;
        }

        private static void RGB565Converter(ushort* id, out ARGBPixel outPixel)
        {
            outPixel = (ARGBPixel) (*(wRGB565Pixel*) id);
        }

        private static void RGB5A3Converter(ushort* id, out ARGBPixel outPixel)
        {
            outPixel = (ARGBPixel) (*(wRGB5A3Pixel*) id);
        }

        #endregion

        private MedianCut(Bitmap bmp, WiiPixelFormat texFormat, WiiPaletteFormat palFormat)
        {
            //Set output format
            if (texFormat == WiiPixelFormat.CI4)
            {
                _outFormat = PixelFormat.Format4bppIndexed;
            }
            else if (texFormat == WiiPixelFormat.CI8)
            {
                _outFormat = PixelFormat.Format8bppIndexed;
            }
            else
            {
                throw new ArgumentException("Invalid pixel format.");
            }

            //Set conversion functions
            if (palFormat == WiiPaletteFormat.IA8)
            {
                _idFunc = IA8Handler;
                _idConv = IA8Converter;
            }
            else if (palFormat == WiiPaletteFormat.RGB565)
            {
                _idFunc = RGB565Handler;
                _idConv = RGB565Converter;
            }
            else
            {
                _idFunc = RGB5A3Handler;
                _idConv = RGB5A3Converter;
            }

            //Lock/set source data
            _srcBmp = bmp;
            _width = bmp.Width;
            _height = bmp.Height;
            _size = _width * _height;
            _srcData = bmp.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            _srcPixels = (ARGBPixel*) _srcData.Scan0;

            //Create buffers
            _boxes = (ColorBox*) Marshal.AllocHGlobal(256 * sizeof(ColorBox));
            _groupTable = (ColorEntry**) Marshal.AllocHGlobal(65536 * sizeof(void*));
        }

        ~MedianCut()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_boxes != null)
            {
                Marshal.FreeHGlobal((IntPtr) _boxes);
                _boxes = null;
            }

            if (_groupTable != null)
            {
                Marshal.FreeHGlobal((IntPtr) _groupTable);
                _groupTable = null;
            }

            if (_srcBmp != null)
            {
                _srcBmp.UnlockBits(_srcData);
                _srcBmp = null;
                _srcData = null;
                _srcPixels = null;
            }

            GC.SuppressFinalize(this);
        }

        private bool SelectColors(int targetColors)
        {
            ColorBox* splitBox, boxPtr = _boxes;
            ARGBPixel* sPtr = _srcPixels;
            ColorEntry** gPtr = _groupTable;
            ColorEntry* entry;
            ushort id;

            //Create initial box
            boxPtr->Initialize();
            _boxCount = 1;

            //Iterate through colors using ID generator
            for (int i = 0; i < _size; i++)
            {
                _idFunc(*sPtr++, &id);
                gPtr = _groupTable + id;
                if ((entry = *gPtr) == null)
                {
                    *gPtr = entry = ColorEntry.Create();
                    _idConv(&id, out entry->_color);
                    entry->_weight = 1;
                    boxPtr->_rootEntry->InsertPrev(entry);
                }
                else
                {
                    entry->_weight++;
                }
            }

            //If no quantization is necessary, then leave.
            if (boxPtr->_colors <= targetColors)
            {
                return false;
            }

            //Update initial box
            boxPtr->Update(targetColors - 1);

            //Split until we reach desired colors
            while (_boxCount < targetColors)
            {
                //Find split candidate
                splitBox = ColorBox.FindSplit(_boxes, _boxCount, targetColors, out int splitAxis);

                //Create new box
                (++boxPtr)->Initialize();

                //Move colors from one box to another using split axis
                splitBox->Split(boxPtr, splitAxis);

                //Update boxes
                _boxCount++;
                splitBox->Update(targetColors - _boxCount);
                boxPtr->Update(targetColors - _boxCount);
            }

            return true;
        }

        private void SortBoxes()
        {
            //Sort boxes by luminance
            void** tableData = stackalloc void*[_boxCount];
            ColorBox** colorTable = (ColorBox**) tableData;
            ColorBox* boxPtr = _boxes;
            int index;
            ushort id;
            float luminance;
            for (int count = 0; count < _boxCount; count++)
            {
                luminance = boxPtr->_luminance = boxPtr->_color.Luminance();

                for (index = 0; index < count && luminance > colorTable[index]->_luminance; index++)
                {
                    ;
                }

                //Slide entries right
                for (int y = count; y > index;)
                {
                    colorTable[y--] = colorTable[y];
                }

                colorTable[index] = boxPtr++;
            }

            //Set indices and clamp colors
            for (int i = 0; i < _boxCount; i++)
            {
                colorTable[i]->_index = i;
                _idFunc(colorTable[i]->_color, &id);
                _idConv(&id, out colorTable[i]->_color);
            }
        }

        private void ClearBoxes()
        {
            //Clean up
            ColorBox* boxPtr = _boxes;
            for (int i = 0; i < _boxCount; i++, boxPtr++)
            {
                boxPtr->Destroy();
            }
        }

        private void WritePalette(ColorPalette pal)
        {
            ColorBox* pBox = _boxes;
            for (int i = 0; i < _boxCount; i++, pBox++)
            {
                pal.Entries[pBox->_index] = (Color) pBox->_color;
            }
        }

        private void WriteIndices(BitmapData destData)
        {
            ARGBPixel* sPtr = _srcPixels;
            ushort id;
            byte* dPtr = (byte*) destData.Scan0;
            int step, val;

            if (destData.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                step = destData.Stride - _width;
                for (int y = 0; y < _height; y++, dPtr += step)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        _idFunc(*sPtr++, &id);
                        *dPtr++ = (byte) _groupTable[id]->_box->_index;
                    }
                }
            }
            else
            {
                step = destData.Stride - (_width + 1) / 2;
                for (int y = 0; y < _height; y++, dPtr += step)
                {
                    for (int x = _width; x > 0; x -= 2)
                    {
                        _idFunc(*sPtr++, &id);
                        val = _groupTable[id]->_box->_index << 4;

                        if (x > 1)
                        {
                            _idFunc(*sPtr++, &id);
                            val |= _groupTable[id]->_box->_index & 0xF;
                        }

                        *dPtr++ = (byte) val;
                    }
                }
            }
        }

        private void SpreadColors(int total)
        {
            ColorBox* pBox = _boxes;
            ColorEntry* entry = pBox->_rootEntry->_next;

            int index, count = (int) pBox->_colors;

            //Set initial box
            pBox->_color = entry->_color;
            entry = entry->_next;
            pBox++;

            //Set remaining boxes
            for (index = 1; index < count; index++, pBox++, entry = entry->_next)
            {
                entry->_box = pBox;
                pBox->_color = entry->_color;
                pBox->_rootEntry = null;
            }

            //Not needed because the remaining palette colors will be empty anyways.
            //Set total boxes
            //for (; index < total; index++, pBox++)
            //{
            //    pBox->_color = new ARGBPixel(255, 0, 0, 0);
            //    pBox->_rootEntry = null;
            //}

            //_boxCount = total;
            _boxCount = count;
        }

        //private void AlignColors(int total)
        //{
        //    for (int i = _boxCount; i < total; i++)
        //    {
        //        _boxes[i]._color = new ARGBPixel(255, 0, 0, 0);
        //        _boxes[i]._rootEntry = null;
        //    }

        //    _boxCount = total;
        //}

        private Bitmap Quantize(int targetColors, IProgressTracker progress)
        {
            //Clear group table
            Memory.Fill(_groupTable, (uint) (65536 * sizeof(void*)), 0);

            if (!SelectColors(targetColors))
            {
                SpreadColors(targetColors);
            }

            SortBoxes();

            //Write bitmap
            Bitmap bmp = new Bitmap(_width, _height, _outFormat);
            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.None, targetColors);
            WritePalette(pal);
            bmp.Palette = pal;

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, _outFormat);

            WriteIndices(data);

            bmp.UnlockBits(data);

            ClearBoxes();

            return bmp;
        }

        public static Bitmap Quantize(Bitmap bmp, int colors, WiiPixelFormat texFormat, WiiPaletteFormat palFormat,
                                      IProgressTracker progress)
        {
            using (MedianCut mc = new MedianCut(bmp, texFormat, palFormat))
            {
                return mc.Quantize(colors, progress);
            }
        }

        private const int R_SCALE = 13;
        private const int G_SCALE = 24;
        private const int B_SCALE = 26;
        private const int A_SCALE = 28;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ColorEntry
        {
            public ARGBPixel _color;
            public uint _weight;
            public ColorEntry* _prev, _next;
            public ColorBox* _box;

            public static ColorEntry* Create()
            {
                return (ColorEntry*) Marshal.AllocHGlobal(sizeof(ColorEntry));
            }

            public static void Destroy(ColorEntry* e)
            {
                Marshal.FreeHGlobal((IntPtr) e);
            }

            public void InsertNext(ColorEntry* entry)
            {
                entry->_prev = _next->_prev;
                entry->_next = _next;
                entry->_box = _box;
                _box->_colors++;

                _next->_prev = entry;
                _next = entry;
            }

            public void InsertPrev(ColorEntry* entry)
            {
                entry->_prev = _prev;
                entry->_next = _prev->_next;
                entry->_box = _box;
                _box->_colors++;

                _prev->_next = entry;
                _prev = entry;
            }

            public void Remove()
            {
                _prev->_next = _next;
                _next->_prev = _prev;
                _box->_colors--;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ColorBox
        {
            private uint _min, _max;
            private readonly uint _halfError;
            private uint _volume;
            private ulong bError;
            private ulong gError;
            private ulong rError;
            private ulong aError;

            public ARGBPixel _color;
            public float _luminance;

            public ColorEntry* _rootEntry;
            public uint _colors, _weight;
            public int _index;

            private ColorBox* Address
            {
                get
                {
                    fixed (ColorBox* p = &this)
                    {
                        return p;
                    }
                }
            }

            public void Initialize()
            {
                _rootEntry = ColorEntry.Create();
                _rootEntry->_prev = _rootEntry->_next = _rootEntry;
                _rootEntry->_box = Address;
                _colors = 0;
            }

            public void Destroy()
            {
                ColorEntry* current, next;

                if (_rootEntry != null)
                {
                    current = _rootEntry;
                    do
                    {
                        next = current->_next;
                        ColorEntry.Destroy(current);
                        current = next;
                    } while (current != _rootEntry);

                    _rootEntry = null;
                }
            }

            public void Split(ColorBox* newBox, int axis)
            {
                ColorBox* box = _rootEntry->_box;
                ColorEntry* current, next;
                //Get limit from halfError
                int limit = ((byte*) &box->_halfError)[axis];

                for (current = _rootEntry->_next; current != _rootEntry; current = next)
                {
                    next = current->_next;
                    if (((byte*) &current->_color)[axis] > limit)
                    {
                        //Remove from current box
                        current->Remove();
                        //Add to end of new box
                        newBox->_rootEntry->InsertPrev(current);
                    }
                }
            }

            public void Update(int remaining)
            {
                ColorBox* box = _rootEntry->_box;
                ColorEntry* current;
                byte* tPtr, sColor;
                ulong* colBuffer = &box->bError;
                byte* pMin = (byte*) &box->_min;
                byte* pMax = (byte*) &box->_max;
                uint weight;
                int diff;
                byte val;
                int* size = stackalloc int[4];

                //Reset bounds and weight
                _min = 0xFFFFFFFF;
                _max = 0;
                _weight = 0;

                //Zero errors (for use as color accumulator)
                aError = rError = gError = bError = 0;
                //Memory.Fill(&box->error, 32, 0);

                //Get min/max bounds for all contained color entries and calculate color
                for (current = _rootEntry->_next; current != _rootEntry; current = current->_next)
                {
                    _weight += weight = current->_weight;
                    tPtr = (byte*) &current->_color;

                    //Update bounds for and accumulate each element
                    for (int i = 0; i < 4; i++)
                    {
                        val = *tPtr++;
                        pMin[i] = Math.Min(val, pMin[i]);
                        pMax[i] = Math.Max(val, pMax[i]);
                        colBuffer[i] += val * weight;
                    }
                }

                //Set calculated color
                sColor = (byte*) &box->_color;
                for (int i = 0; i < 4; i++)
                {
                    sColor[i] = (byte) (colBuffer[i] / _weight);
                }

                //Calculate volume
                _volume = 1;
                for (int i = 0; i < 4; i++)
                {
                    diff = pMax[i] - pMin[i] + 1;
                    _volume *= (uint) diff;
                    size[i] = (byte) diff;
                }

                if (_volume == 0)
                {
                    _volume = 0xFFFFFFFF;
                }

                //Calculate error
                aError = rError = gError = bError = 0;
                //Memory.Fill(&box->error, 32, 0);
                for (current = _rootEntry->_next; current != _rootEntry; current = current->_next)
                {
                    weight = current->_weight;
                    tPtr = (byte*) &current->_color;
                    for (int i = 0; i < 4; i++)
                    {
                        diff = tPtr[i] - sColor[i];
                        colBuffer[i] += weight * (uint) (diff * diff);
                    }
                }

                //Get half-error
                tPtr = (byte*) &box->_halfError;
                for (int i = 0; i < 4; i++)
                {
                    tPtr[i] = (byte) (pMin[i] + size[i] / 2);
                }

                if (_volume > 1)
                {
                    int axis1 = -1, axis2 = -1;
                    int len1 = 0, len2 = 0;
                    int ratio;

                    for (int i = 0; i < 4; i++)
                    {
                        if (size[i] > len1)
                        {
                            len2 = len1;
                            axis2 = axis1;
                            len1 = size[i];
                            axis1 = i;
                        }
                        else if (size[i] > len2)
                        {
                            len2 = size[i];
                            axis2 = i;
                        }
                    }

                    if (len2 == 0)
                    {
                        len2 = 1;
                    }

                    ratio = (len1 + len2 / 2) / len2;

                    if (ratio > remaining + 1)
                    {
                        ratio = remaining + 1;
                    }

                    if (ratio > 2 && axis1 >= 0)
                    {
                        diff = pMin[axis1] + (pMax[axis1] - pMin[axis1]) + ratio / 2;
                        if (diff < pMax[axis1])
                        {
                            tPtr[axis1] = (byte) diff;
                        }
                    }
                }

                //If half-error touches ceiling, set to floor
                for (int i = 0; i < 4; i++)
                {
                    if (tPtr[i] == pMax[i])
                    {
                        tPtr[i] = pMin[i];
                    }
                }
            }

            public static ColorBox* FindSplit(ColorBox* boxes, int boxCount, int maxColors, out int axis)
            {
                ColorBox* outBox = null;
                double lBias = 1.0, maxC = 0.0;
                //double val;
                double rpe, gpe, bpe, ape;
                //int index = -1;

                byte* pMin, pMax;
                ulong* pErr;

                axis = -1;

                if (maxColors <= 16 && boxCount <= 2)
                {
                    lBias = (3.0 - boxCount) / (2.0 / 2.66);
                }

                for (int i = 0; i < boxCount; i++, boxes++)
                {
                    if (boxes->_volume <= 1)
                    {
                        continue;
                    }

                    pMin = (byte*) &boxes->_min;
                    pMax = (byte*) &boxes->_max;
                    pErr = &boxes->bError;

                    rpe = boxes->rError * R_SCALE * R_SCALE;
                    gpe = boxes->gError * G_SCALE * G_SCALE;
                    bpe = boxes->bError * B_SCALE * B_SCALE;
                    ape = boxes->aError * A_SCALE * A_SCALE;

                    //for (int x = 0; x < 4; x++)
                    //{
                    //    if (((val = pErr[i]) > maxC) && (pMin[x] < pMax[x]))
                    //    {
                    //        //index = i;
                    //        outBox = boxes;
                    //        maxC = val;
                    //        axis = x;
                    //    }
                    //}

                    if (lBias * rpe > maxC && pMin[2] < pMax[2])
                    {
                        outBox = boxes;
                        maxC = lBias * rpe;
                        axis = 2;
                    }

                    if (gpe > maxC && pMin[1] < pMax[1])
                    {
                        outBox = boxes;
                        maxC = gpe;
                        axis = 1;
                    }

                    if (bpe > maxC && pMin[0] < pMax[0])
                    {
                        outBox = boxes;
                        maxC = bpe;
                        axis = 0;
                    }

                    if (ape > maxC && pMin[3] < pMax[3])
                    {
                        outBox = boxes;
                        maxC = ape;
                        axis = 3;
                    }
                }

                return outBox;
            }
        }
    }
}
using BrawlLib.Internal.Windows.Forms;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Audio
{
    public unsafe class AudioConverter
    {
        public static void CalcCoefs(short* source, int samples, short* dest, IProgressTracker progress)
        {
            //short[,] coefs = new short[8,2];

            //double d10 = 10.0;
            //int channels = 2;
            //int num2 = 2;
            //int chunkSamples = 14;
            //int var30 = 0;
            short* sPtr = source;
            int numBlocks = (samples + 13) / 14;

            int nBits = 3;
            //while ((1 << ++nBits) < 8) ;

            double** bufferArray = stackalloc double*[8];
            for (int z = 0; z < 8; z++)
            {
                bufferArray[z] = (double*) Marshal.AllocHGlobal(3 * 8);
            }

            double* buffer2 = stackalloc double[3];
            short* sampleBuffer = (short*) Marshal.AllocHGlobal(0x7000);
            short* chunkBuffer = stackalloc short[28];
            //double** chunkBuffer = (double**)chunkBufferData;
            for (int z = 0; z < 28; z++)
            {
                chunkBuffer[z] = 0;
            }

            double* sChunkBuffer = stackalloc double[3];
            double* omgBuffer = stackalloc double[3];

            double** pChannels = stackalloc double*[3];
            for (int z = 0; z <= 2; z++)
            {
                pChannels[z] = (double*) Marshal.AllocHGlobal(3 * 8);
            }

            int* anotherBuffer = stackalloc int[3];

            double** multiBuffer = (double**) Marshal.AllocHGlobal(numBlocks * 4 * 2);
            int unused = 0;
            int multiIndex = 0;

            int blockSamples;
            int temp;

            float initValue = 0;
            int lastUpdate = 0;
            if (progress != null)
            {
                initValue = progress.CurrentValue;
            }

            for (int x = samples; x > 0;)
            {
                if (x > 0x3800)
                {
                    blockSamples = 0x3800;
                    x -= 0x3800;
                }
                else
                {
                    blockSamples = x;
                    for (int z = 0; z < 14 && z + blockSamples < 0x3800; z++)
                    {
                        sampleBuffer[blockSamples + z] = 0;
                    }

                    x = 0;
                }

                short* tPtr = sampleBuffer;
                for (int z = 0; z < blockSamples; z++)
                {
                    *tPtr++ = *sPtr++;
                }

                for (int i = 0; i < blockSamples;)
                {
                    for (int z = 0; z < 14; z++)
                    {
                        chunkBuffer[z] = chunkBuffer[z + 14];
                    }

                    for (int z = 0; z < 14; z++)
                    {
                        chunkBuffer[z + 14] = sampleBuffer[i++];
                    }

                    Something1(&chunkBuffer[14], sChunkBuffer);
                    if (Math.Abs(sChunkBuffer[0]) > 10.0)
                    {
                        Something2(&chunkBuffer[14], pChannels);
                        if (!Something3(pChannels, anotherBuffer, &temp))
                        {
                            Something4(pChannels, anotherBuffer, sChunkBuffer);
                            sChunkBuffer[0] = 1.0;
                            if (Something5(sChunkBuffer, omgBuffer) == 0)
                            {
                                multiBuffer[multiIndex] = (double*) Marshal.AllocHGlobal(3 * 8);
                                multiBuffer[multiIndex][0] = 1.0;
                                for (int z = 1; z <= 2; z++)
                                {
                                    if (omgBuffer[z] >= 1.0)
                                    {
                                        omgBuffer[z] = 0.9999999999;
                                    }

                                    if (omgBuffer[z] <= -1.0)
                                    {
                                        omgBuffer[z] = -0.9999999999;
                                    }
                                }

                                Something6(omgBuffer, multiBuffer[multiIndex]);
                                multiIndex++;
                            }
                        }
                    }

                    unused++;
                }

                if (progress != null)
                {
                    lastUpdate += blockSamples;
                    if (lastUpdate % 0x3800 == 0)
                    {
                        progress.Update(progress.CurrentValue + 0x3800);
                    }
                }
            }

            progress?.Update(initValue + samples);

            sChunkBuffer[0] = 1.0;
            for (int y = 1; y <= 2; y++)
            {
                sChunkBuffer[y] = 0.0;
            }

            for (int z = 0; z < multiIndex; z++)
            {
                Something7(multiBuffer[z], bufferArray[0]);
                for (int y = 1; y <= 2; y++)
                {
                    sChunkBuffer[y] = sChunkBuffer[y] + bufferArray[0][y];
                }
            }

            for (int y = 1; y <= 2; y++)
            {
                sChunkBuffer[y] /= multiIndex;
            }

            double tmp;
            Something8(sChunkBuffer, omgBuffer, bufferArray[0], &tmp);
            for (int y = 1; y <= 2; y++)
            {
                if (omgBuffer[y] >= 1.0)
                {
                    omgBuffer[y] = 0.9999999999;
                }

                if (omgBuffer[y] <= -1.0)
                {
                    omgBuffer[y] = -0.9999999999;
                }
            }

            Something6(omgBuffer, bufferArray[0]);

            for (int w = 0; w < nBits;)
            {
                //int mask = 1 << w;
                for (int z = 0; z <= 2; z++)
                {
                    buffer2[z] = 0.0;
                }

                buffer2[1] = -1.0;
                Something9(bufferArray, buffer2, 1 << w++, 0.01);
                //w++;
                Something10(bufferArray, 1 << w, multiBuffer, multiIndex, 0.0);
            }

            //Write output
            for (int z = 0; z < 8; z++)
            {
                for (int y = 0; y < 2; y++)
                {
                    double d = -bufferArray[z][y + 1] * 2048.0;
                    if (d > 0.0)
                    {
                        dest[(z << 1) + y] = d > 32767.0 ? (short) 32767 : (short) (d + 0.5);
                    }
                    else
                    {
                        dest[(z << 1) + y] = d < -32768.0 ? (short) -32768 : (short) (d - 0.5);
                    }
                }
            }

            //Free memory
            for (int i = 0; i < multiIndex; i++)
            {
                Marshal.FreeHGlobal((IntPtr) multiBuffer[i]);
            }

            Marshal.FreeHGlobal((IntPtr) multiBuffer);

            for (int i = 0; i <= 2; i++)
            {
                Marshal.FreeHGlobal((IntPtr) pChannels[i]);
            }

            for (int i = 0; i < 8; i++)
            {
                Marshal.FreeHGlobal((IntPtr) bufferArray[i]);
            }

            Marshal.FreeHGlobal((IntPtr) sampleBuffer);

            //return coefs;
        }

        public static void EncodeBlock(short* source, int samples, byte* dest, short* coefs)
        {
            for (int i = 0; i < samples; i += 14, source += 14, dest += 8)
            {
                EncodeChunk(source, Math.Min(samples - i, 14), dest, coefs);
            }
        }

        //Make sure source includes the yn values (16 samples total)
        private static void EncodeChunk(short* source, int samples, byte* dest, short* coefs)
        {
            //int* sampleBuffer = stackalloc int[14];
            int* buffer1 = stackalloc int[128];
            int* buffer2 = stackalloc int[112];

            double bestDistance = double.MaxValue, distAccum;
            int bestIndex = 0;
            int bestScale = 0;

            int distance, index, scale;

            int* p1, p2, t1, t2;
            short* sPtr;
            int v1, v2, v3;

            //Iterate through each coef set, finding the set with the smallest error
            p1 = buffer1;
            p2 = buffer2;
            for (int i = 0; i < 8; i++, p1 += 16, p2 += 14, coefs += 2)
            {
                //Set yn values
                t1 = p1;
                *t1++ = source[0];
                *t1++ = source[1];

                //Round and clamp samples for this coef set
                distance = 0;
                sPtr = source;
                for (int y = 0; y < samples; y++)
                {
                    //Multiply previous samples by coefs
                    *t1++ = v1 = (*sPtr++ * coefs[1] + *sPtr++ * coefs[0]) / 2048;
                    //Subtract from current sample
                    v2 = *sPtr-- - v1;
                    //Clamp
                    v3 = v2 >= 32767 ? 32767 : v2 <= -32768 ? -32768 : v2;
                    //Compare distance
                    if (Math.Abs(v3) > Math.Abs(distance))
                    {
                        distance = v3;
                    }
                }

                //Set initial scale
                for (scale = 0; scale <= 12 && (distance > 7 || distance < -8); scale++, distance /= 2)
                {
                    ;
                }

                scale = scale <= 1 ? -1 : scale - 2;

                do
                {
                    scale++;
                    distAccum = 0;
                    index = 0;

                    t1 = p1;
                    t2 = p2;
                    sPtr = source + 2;
                    for (int y = 0; y < samples; y++)
                    {
                        //Multiply previous 
                        v1 = *t1++ * coefs[1] + *t1++ * coefs[0];
                        //Evaluate from real sample
                        v2 = ((*sPtr << 11) - v1) / 2048;
                        //Round to nearest sample
                        v3 = v2 > 0
                            ? (int) ((double) v2 / (1 << scale) + 0.4999999f)
                            : (int) ((double) v2 / (1 << scale) - 0.4999999f);

                        //Clamp sample and set index
                        if (v3 < -8)
                        {
                            if (index < (v3 = -8 - v3))
                            {
                                index = v3;
                            }

                            v3 = -8;
                        }
                        else if (v3 > 7)
                        {
                            if (index < (v3 -= 7))
                            {
                                index = v3;
                            }

                            v3 = 7;
                        }

                        //Store result
                        *t2++ = v3;

                        //Round and expand
                        v1 = (v1 + ((v3 * (1 << scale)) << 11) + 1024) >> 11;
                        //Clamp and store
                        *t1-- = v2 = v1 >= 32767 ? 32767 : v1 <= -32768 ? -32768 : v1;
                        //Accumulate distance
                        v3 = *sPtr++ - v2;
                        distAccum += (double) v3 * v3;
                    }

                    for (int x = index + 8; x > 256; x >>= 1)
                    {
                        if (++scale >= 12)
                        {
                            scale = 11;
                        }
                    }
                } while (scale < 12 && index > 1);

                if (distAccum < bestDistance)
                {
                    bestDistance = distAccum;
                    bestIndex = i;
                    bestScale = scale;
                }
            }

            p1 = buffer1 + (bestIndex << 4) + 2;
            p2 = buffer2 + bestIndex * 14;

            //Set resulting yn values
            //*yn++ = (short)*p1++;
            //*yn++ = (short)*p1++;

            //Write converted samples
            sPtr = source + 2;
            for (int i = 0; i < samples; i++)
            {
                *sPtr++ = (short) *p1++;
            }

            //Write ps
            *dest++ = (byte) ((bestIndex << 4) | (bestScale & 0xF));

            //Zero remaining samples
            for (int i = samples; i < 14; i++)
            {
                p2[i] = 0;
            }

            //Write output samples
            for (int y = 0; y++ < 7;)
            {
                *dest++ = (byte) ((*p2++ << 4) | (*p2++ & 0xF));
            }
        }

        private static void Something10(double** bufferArray, int mask, double** multiBuffer, int multiIndex,
                                        double val)
        {
            double** bufferList = stackalloc double*[mask];

            int* buffer1 = stackalloc int[mask];
            double* buffer2 = stackalloc double[3];

            int index;
            double value, tempVal = 0;

            for (int i = 0; i < mask; i++)
            {
                bufferList[i] = (double*) Marshal.AllocHGlobal(8 * 3);
            }

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < mask; y++)
                {
                    buffer1[y] = 0;
                    for (int i = 0; i <= 2; i++)
                    {
                        bufferList[y][i] = 0.0;
                    }
                }

                for (int z = 0; z < multiIndex; z++)
                {
                    index = 0;
                    value = 1.0e30;
                    for (int i = 0; i < mask; i++)
                    {
                        tempVal = Something11(bufferArray[i], multiBuffer[z]);
                        if (tempVal < value)
                        {
                            value = tempVal;
                            index = i;
                        }
                    }

                    buffer1[index]++;
                    Something7(multiBuffer[z], buffer2);
                    for (int i = 0; i <= 2; i++)
                    {
                        bufferList[index][i] += buffer2[i];
                    }
                }

                for (int i = 0; i < mask; i++)
                {
                    if (buffer1[i] > 0)
                    {
                        for (int y = 0; y <= 2; y++)
                        {
                            bufferList[i][y] /= buffer1[i];
                        }
                    }
                }

                for (int i = 0; i < mask; i++)
                {
                    Something8(bufferList[i], buffer2, bufferArray[i], &tempVal);
                    for (int y = 1; y <= 2; y++)
                    {
                        if (buffer2[y] >= 1.0)
                        {
                            buffer2[y] = 0.9999999999;
                        }

                        if (buffer2[y] <= -1.0)
                        {
                            buffer2[y] = -0.9999999999;
                        }
                    }

                    Something6(buffer2, bufferArray[i]);
                }
            }

            for (int i = 0; i < mask; i++)
            {
                Marshal.FreeHGlobal((IntPtr) bufferList[i]);
            }
        }

        private static double Something11(double* source1, double* source2)
        {
            double* b = stackalloc double[3];
            Something12(source2, b);
            double val1 = source1[0] * source1[0] + source1[1] * source1[1] + source1[2] * source1[2];
            double val2 = source1[0] * source1[1] + source1[1] * source1[2];
            double val3 = source1[0] * source1[2];
            return b[0] * val1 + 2.0 * b[1] * val2 + 2.0 * b[2] * val3;
        }

        private static void Something12(double* source, double* dest)
        {
            double v2 = -source[1], v3 = -source[2];
            double val = (v3 * v2 + v2) / (1.0 - v3 * v3);

            dest[0] = 1.0;
            dest[1] = val * dest[0];
            dest[2] = v2 * dest[1] + v3 * dest[0];
        }

        private static void Something9(double** bufferArray, double* buffer2, int mask, double value)
        {
            for (int i = 0; i < mask; i++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    bufferArray[mask + i][y] = value * buffer2[y] + bufferArray[i][y];
                }
            }
        }

        private static int Something8(double* src, double* omgBuffer, double* dst, double* outVar)
        {
            int count = 0;
            double val = src[0];

            dst[0] = 1.0;
            for (int i = 1; i <= 2; i++)
            {
                double v2 = 0.0;
                for (int y = 1; y < i; y++)
                {
                    v2 += dst[y] * src[i - y];
                }

                if (val > 0.0)
                {
                    dst[i] = -(v2 + src[i]) / val;
                }
                else
                {
                    dst[i] = 0.0;
                }

                omgBuffer[i] = dst[i];

                if (Math.Abs(omgBuffer[i]) > 1.0)
                {
                    count++;
                }

                for (int y = 1; y < i; y++)
                {
                    dst[y] += dst[i] * dst[i - y];
                }

                val *= 1.0 - dst[i] * dst[i];
            }

            *outVar = val;
            return count;
        }

        private static void Something7(double* src, double* dst)
        {
            //double v1, v2, v3;
            double* buffer = stackalloc double[9];
            //DVector3* p = (DVector3*)buffer;

            //p[2] = new DVector3(1.0, -src[1], -src[2]);
            //for (int i = 2; i > 0; i--)
            //{
            //    //v3 = 1.0 - (v2 = (v1 = p[i][i]) * v1);
            //    v1 = p[i][i];
            //    v2 = v1 * v1;
            //    v3 = 1.0 - v2;
            //    for (int y = 1; y <= i; y++)
            //        p[i - 1][y] = (v2 + v1) / v3;
            //}

            //dst[0] = 1.0;
            //for (int i = 1; i <= 2; i++)
            //{
            //    dst[i] = 0.0;
            //    for (int y = 1; y <= i; y++)
            //        dst[i] += p[i][y] * dst[i - y];
            //}

            buffer[2 * 3] = 1.0;
            for (int i = 1; i <= 2; i++)
            {
                buffer[2 * 3 + i] = -src[i];
            }

            for (int i = 2; i > 0; i--)
            {
                double val = 1.0 - buffer[i * 3 + i] * buffer[i * 3 + i];
                for (int y = 1; y <= i; y++)
                {
                    buffer[(i - 1) * 3 + y] = (buffer[i * 3 + i] * buffer[i * 3 + y] + buffer[i * 3 + y]) / val;
                }
            }

            dst[0] = 1.0;
            for (int i = 1; i <= 2; i++)
            {
                dst[i] = 0.0;
                for (int y = 1; y <= i; y++)
                {
                    dst[i] += buffer[i * 3 + y] * dst[i - y];
                }
            }
        }


        private static void Something1(short* source, double* dest)
        {
            for (int i = 0; i <= 2; i++)
            {
                dest[i] = 0.0f;
                for (int x = 0; x < 14; x++)
                {
                    dest[i] -= source[x - i] * source[x];
                }
            }
        }

        private static void Something2(short* source, double** outList)
        {
            for (int x = 1; x <= 2; x++)
            {
                for (int y = 1; y <= 2; y++)
                {
                    outList[x][y] = 0.0;
                    for (int z = 0; z < 14; z++)
                    {
                        outList[x][y] += source[z - x] * source[z - y];
                    }
                }
            }
        }

        private static bool Something3(double** outList, int* dest, int* unk)
        {
            double* buffer = stackalloc double[3];
            double val, tmp, min, max;

            *unk = 1;

            //Get greatest distance from zero
            for (int x = 1; x <= 2; x++)
            {
                val = 0.0;
                for (int i = 1; i <= 2; i++)
                {
                    tmp = Math.Abs(outList[x][i]);
                    if (tmp > val)
                    {
                        val = tmp;
                    }
                }

                if (val == 0.0)
                {
                    return true;
                }

                buffer[x] = 1.0 / val;
            }

            int maxIndex = 0;
            for (int i = 1; i <= 2; i++)
            {
                for (int x = 1; x < i; x++)
                {
                    tmp = outList[x][i];
                    for (int y = 1; y < x; y++)
                    {
                        tmp -= outList[x][y] * outList[y][i];
                    }

                    outList[x][i] = tmp;
                }

                val = 0.0;
                for (int x = i; x <= 2; x++)
                {
                    tmp = outList[x][i];
                    for (int y = 1; y < i; y++)
                    {
                        tmp -= outList[x][y] * outList[y][i];
                    }

                    outList[x][i] = tmp;
                    tmp = Math.Abs(tmp) * buffer[x];
                    if (tmp >= val)
                    {
                        val = tmp;
                        maxIndex = x;
                    }
                }

                if (maxIndex != i)
                {
                    for (int y = 1; y <= 2; y++)
                    {
                        tmp = outList[maxIndex][y];
                        outList[maxIndex][y] = outList[i][y];
                        outList[i][y] = tmp;
                    }

                    *unk = -*unk;
                    buffer[maxIndex] = buffer[i];
                }

                dest[i] = maxIndex;

                if (outList[i][i] == 0.0)
                {
                    return true;
                }

                if (i != 2)
                {
                    tmp = 1.0 / outList[i][i];
                    for (int x = i + 1; x <= 2; x++)
                    {
                        outList[x][i] *= tmp;
                    }
                }
            }
            //no need for buffer anymore

            //Get range
            min = 1.0e10;
            max = 0.0;
            for (int i = 1; i <= 2; i++)
            {
                tmp = Math.Abs(outList[i][i]);
                if (tmp < min)
                {
                    min = tmp;
                }

                if (tmp > max)
                {
                    max = tmp;
                }
            }

            if (min / max < 1.0e-10)
            {
                return true;
            }

            return false;
        }

        private static void Something4(double** outList, int* dest, double* block)
        {
            int index;
            double tmp;

            for (int i = 1, x = 0; i <= 2; i++)
            {
                index = dest[i];
                tmp = block[index];
                block[index] = block[i];
                if (x != 0)
                {
                    for (int y = x; y <= i - 1; y++)
                    {
                        tmp -= block[y] * outList[i][y];
                    }
                }
                else if (tmp != 0.0)
                {
                    x = i;
                }

                block[i] = tmp;
            }

            for (int i = 2; i > 0; i--)
            {
                tmp = block[i];
                for (int y = i + 1; y <= 2; y++)
                {
                    tmp -= block[y] * outList[i][y];
                }

                block[i] = tmp / outList[i][i];
            }
        }

        private static int Something5(double* block, double* buffer)
        {
            //int count = 0;
            //double* dBuffer = stackalloc double[3];

            //buffer[2] = block[2];

            //for (int i = 1; i > 0; i--)
            //{
            //    for (int x = 0; x <= i; x++)
            //    {
            //        double dTemp = 1.0 - (buffer[i + 1] * buffer[i + 1]);
            //        if (dTemp == 0.0)
            //            return 1;

            //        dBuffer[x] = (block[x] - (block[i + 1 - x] * buffer[i + 1])) / dTemp;
            //    }

            //    for (int x = 0; x <= i; x++)
            //        block[x] = dBuffer[x];

            //    buffer[i] = dBuffer[i];
            //    if (Math.Abs(buffer[i]) > 1.0)
            //        count++;
            //}
            //return count;

            //Collapsed:
            //double val = buffer[2] = block[2];

            //double tmp = 1.0 - (buffer[2] * buffer[2]);
            //if (tmp == 0.0)
            //    return 1;

            //dBuffer[0] = (block[0] - (block[2] * buffer[2])) / tmp;
            //dBuffer[1] = (block[1] - (block[1] * buffer[2])) / tmp;

            //block[0] = dBuffer[0];
            //block[1] = dBuffer[1];

            //buffer[1] = dBuffer[1];
            //if (Math.Abs(buffer[1]) > 1.0)
            //    return 1;
            //return 0;

            double v0, v1, v2 = block[2];
            double tmp;

            if ((tmp = 1.0 - v2 * v2) == 0.0)
            {
                return 1;
            }

            v0 = (block[0] - v2 * v2) / tmp;
            v1 = (block[1] - block[1] * v2) / tmp;

            block[0] = v0;
            block[1] = v1;

            buffer[1] = v1;
            buffer[2] = v2;

            return Math.Abs(v1) > 1.0 ? 1 : 0;
        }

        private static void Something6(double* omgBuffer, double* buffer)
        {
            //buffer[0] = 1.0;

            //for (int i = 1; i <= 2; i++)
            //{
            //    buffer[i] = omgBuffer[i];
            //    for (int x = 1; x < i; x++)
            //        buffer[x] = (buffer[i] * buffer[i - x]) + buffer[x];
            //}

            //Collapsed:
            //buffer[1] = omgBuffer[1];
            //buffer[2] = omgBuffer[2];
            //buffer[1] = (buffer[2] * buffer[1]) + buffer[1];

            double d1 = omgBuffer[1], d2 = omgBuffer[2];

            buffer[0] = 1.0;
            buffer[1] = d2 * d1 + d1;
            buffer[2] = d2;
        }
    }
}
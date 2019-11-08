using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BrawlLib.Wii.Compression
{
    public unsafe class RunLength
    {
        //Credit goes to Chadderz for this compressor included in CTools.
        //http://wiki.tockdom.com/wiki/User:Chadderz
        private struct Contraction
        {
            public int Location;
            public int Size;
            public int Offset;

            public Contraction(int loc, int sz, int off)
            {
                Location = loc;
                Size = sz;
                Offset = off;
            }
        }

        private static readonly int _lookBackCache = 63;
        private const int _threadChunk = 0x10000;
        private List<Contraction>[] _contractions;
        private int _sourceLen;
        private byte* _pSrc;

        private void FindContractions(int chunk)
        {
            int from, to, run, bestRun, bestOffset;
            Contraction contraction;

            _contractions[chunk] = new List<Contraction>();

            from = chunk * _threadChunk;
            to = Math.Min(from + _threadChunk, _sourceLen);

            for (int i = from; i < to;)
            {
                bestRun = 0;
                bestOffset = 0;

                for (int j = i - 1; j > 0 && j >= i - _lookBackCache; j--)
                {
                    run = 0;

                    while (i + run < _sourceLen && run < 0x111 && _pSrc[j + run] == _pSrc[i + run])
                    {
                        run++;
                    }

                    if (run > bestRun)
                    {
                        bestRun = run;
                        bestOffset = i - j - 1;

                        if (run == 0x111)
                        {
                            break;
                        }
                    }
                }

                if (bestRun >= 3)
                {
                    contraction = new Contraction(i, bestRun, bestOffset);
                    _contractions[chunk].Add(contraction);
                    i += bestRun;
                }
                else
                {
                    i++;
                }
            }
        }

        private void WriteWord(Stream outStream, uint value)
        {
            outStream.WriteByte((byte) ((value >> 24) & 0xFF));
            outStream.WriteByte((byte) ((value >> 16) & 0xFF));
            outStream.WriteByte((byte) ((value >> 08) & 0xFF));
            outStream.WriteByte((byte) ((value >> 00) & 0xFF));
        }

        /// <param name="type">0 is YAZ0, 1 is YAY0, anything else is CompressionHeader</param>
        public int Compress(VoidPtr srcAddr, int srcLen, Stream outStream, IProgressTracker progress, int type)
        {
            _pSrc = (byte*) srcAddr;
            _sourceLen = srcLen;

            int chunkCount = (int) Math.Ceiling((double) srcLen / _threadChunk);

            progress?.Begin(0, srcLen, 0);

            _contractions = new List<Contraction>[chunkCount];

            bool YAY0Comp = type == 1;

            if (type == 0)
            {
                YAZ0 header = new YAZ0
                {
                    _tag = YAZ0.Tag,
                    _unCompDataLen = (uint) _sourceLen
                };
                outStream.Write(&header, YAZ0.Size);
            }
            else if (type == 1)
            {
                //Don't write YAY0 header yet.
                //Collect all compression data first
            }
            else
            {
                CompressionHeader header = new CompressionHeader
                {
                    Algorithm = CompressionType.RunLength,
                    ExpandedSize = (uint) _sourceLen
                };
                outStream.Write(&header, 4 + (header.LargeSize ? 4 : 0));
            }

            ParallelLoopResult result = Parallel.For(0, chunkCount, FindContractions);

            while (!result.IsCompleted)
            {
                Thread.Sleep(100);
            }

            List<Contraction> fullContractions;
            int codeBits, current;
            byte codeByte;
            //byte[] temp;
            int lastUpdate = srcLen;

            fullContractions = new List<Contraction>();
            for (int i = 0; i < _contractions.Length; i++)
            {
                fullContractions.AddRange(_contractions[i]);
                _contractions[i].Clear();
                _contractions[i] = null;
            }

            _contractions = null;

            //temp = new byte[3 * 8];
            codeBits = 0;
            codeByte = 0;
            current = 0;

            List<byte> tempCounts = new List<byte>();
            List<byte> tempData = new List<byte>();
            List<byte> codes = new List<byte>();
            List<List<byte>> counts = new List<List<byte>>();
            List<List<byte>> data = new List<List<byte>>();

            for (int i = 0; i < srcLen;)
            {
                if (codeBits == 8)
                {
                    codes.Add(codeByte);
                    counts.Add(tempCounts);
                    data.Add(tempData);

                    tempCounts = new List<byte>();
                    tempData = new List<byte>();

                    codeBits = 0;
                    codeByte = 0;
                }

                if (current < fullContractions.Count && fullContractions[current].Location == i)
                {
                    if (fullContractions[current].Size >= 0x12)
                    {
                        byte
                            b1 = (byte) (fullContractions[current].Offset >> 8),
                            b2 = (byte) (fullContractions[current].Offset & 0xFF);

                        if (YAY0Comp)
                        {
                            tempCounts.Add(b1);
                            tempCounts.Add(b2);
                        }
                        else
                        {
                            tempData.Add(b1);
                            tempData.Add(b2);
                        }

                        tempData.Add((byte) (fullContractions[current].Size - 0x12));
                    }
                    else
                    {
                        byte
                            b1 = (byte) ((fullContractions[current].Offset >> 8) |
                                         ((fullContractions[current].Size - 2) << 4)),
                            b2 = (byte) (fullContractions[current].Offset & 0xFF);

                        if (YAY0Comp)
                        {
                            tempCounts.Add(b1);
                            tempCounts.Add(b2);
                        }
                        else
                        {
                            tempData.Add(b1);
                            tempData.Add(b2);
                        }
                    }

                    i += fullContractions[current++].Size;

                    while (current < fullContractions.Count && fullContractions[current].Location < i)
                    {
                        current++;
                    }
                }
                else
                {
                    codeByte |= (byte) (1 << (7 - codeBits));
                    tempData.Add(_pSrc[i++]);
                }

                codeBits++;

                if (progress != null)
                {
                    if (i % 0x4000 == 0)
                    {
                        progress.Update(i);
                    }
                }
            }

            codes.Add(codeByte);
            counts.Add(tempCounts);
            data.Add(tempData);

            if (YAY0Comp)
            {
                //Write header
                YAY0 header = new YAY0
                {
                    _tag = YAY0.Tag,
                    _unCompDataLen = (uint) _sourceLen
                };
                uint offset = 0x10 + (uint) codes.Count;
                header._countOffset = offset;
                foreach (List<byte> list in counts)
                {
                    offset += (uint) list.Count;
                }

                header._dataOffset = offset;
                outStream.Write(&header, YAY0.Size);

                //Write codes
                foreach (byte c in codes)
                {
                    outStream.WriteByte(c);
                }

                //Write counts
                foreach (List<byte> list in counts)
                {
                    outStream.Write(list.ToArray(), 0, list.Count);
                }

                //Write data
                foreach (List<byte> list in data)
                {
                    outStream.Write(list.ToArray(), 0, list.Count);
                }
            }
            else
            {
                for (int i = 0; i < codes.Count; i++)
                {
                    //Write code
                    outStream.WriteByte(codes[i]);
                    //Write data
                    outStream.Write(data[i].ToArray(), 0, data[i].Count);
                }
            }

            outStream.Flush();

            progress?.Finish();

            return (int) outStream.Length;
        }

        public static int CompactYAZ0(VoidPtr srcAddr, int srcLen, Stream outStream, ResourceNode r)
        {
            using (ProgressWindow prog = new ProgressWindow(r.RootNode._mainForm, "RunLength - YAZ0",
                $"Compressing {r.Name}, please wait...", false))
            {
                return new RunLength().Compress(srcAddr, srcLen, outStream, prog, 0);
            }
        }

        public static int CompactYAY0(VoidPtr srcAddr, int srcLen, Stream outStream, ResourceNode r)
        {
            using (ProgressWindow prog = new ProgressWindow(r.RootNode._mainForm, "RunLength - YAY0",
                $"Compressing {r.Name}, please wait...", false))
            {
                return new RunLength().Compress(srcAddr, srcLen, outStream, prog, 1);
            }
        }

        public static int Compact(VoidPtr srcAddr, int srcLen, Stream outStream, ResourceNode r)
        {
            using (ProgressWindow prog = new ProgressWindow(r.RootNode._mainForm, "RunLength",
                $"Compressing {r.Name}, please wait...", false))
            {
                return new RunLength().Compress(srcAddr, srcLen, outStream, prog, 2);
            }
        }

        public static void ExpandYAZ0(YAZ0* header, VoidPtr dstAddress, int dstLen)
        {
            Expand(header->Data, dstAddress, dstLen);
        }

        public static void ExpandYAY0(YAY0* header, VoidPtr dstAddress, int dstLen)
        {
            byte*
                codes = (byte*) header + 0x10,
                counts = (byte*) header + header->_countOffset,
                srcPtr = (byte*) header + header->_dataOffset;

            Expand(ref srcPtr, ref codes, ref counts, (byte*) dstAddress, dstLen);
        }

        public static void Expand(CompressionHeader* header, VoidPtr dstAddress, int dstLen)
        {
            Expand(header->Data, dstAddress, dstLen);
        }

        /// <summary>
        /// Expands data at an address using default RunLength compression.
        /// Do not use this for YAY0 data, as it uses codes and counts at their own addresses.
        /// </summary>
        public static void Expand(VoidPtr srcAddress, VoidPtr dstAddress, int dstLen)
        {
            //Use this for reference in the function
            byte* srcData = (byte*) srcAddress;
            Expand(ref srcData, ref srcData, ref srcData, (byte*) dstAddress, dstLen);
        }

        public static void Expand(ref byte* srcPtr, ref byte* codes, ref byte* counts, byte* dstPtr, int dstLen)
        {
            for (byte* ceiling = dstPtr + dstLen; dstPtr < ceiling;)
            {
                for (byte control = *codes++, bit = 8; bit-- != 0 && dstPtr != ceiling;)
                {
                    if ((control & (1 << bit)) != 0)
                    {
                        *dstPtr++ = *srcPtr++;
                    }
                    else
                    {
                        for (int b1 = *counts++,
                            b2 = *counts++,
                            offset = (((b1 & 0xF) << 8) | b2) + 2,
                            temp = (b1 >> 4) & 0xF,
                            num = temp == 0 ? *srcPtr++ + 0x12 : temp + 2;
                            num-- > 0 && dstPtr != ceiling;
                            *dstPtr++ = dstPtr[-offset])
                        {
                            ;
                        }
                    }
                }
            }
        }
    }
}
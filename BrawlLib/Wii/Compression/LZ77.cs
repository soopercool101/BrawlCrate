using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Compression
{
    public unsafe class LZ77 : IDisposable
    {
        public const int WindowMask = 0xFFF;
        public const int WindowLength = 4096; //12 bits - 1, 1 - 4096
        public int PatternLength = 18;        //4 bits + 3, 3 - 18
        public const int MinMatch = 3;
        private VoidPtr _dataAddr;
        private readonly ushort* _Next;
        private readonly ushort* _First;
        private readonly ushort* _Last;
        private int _wIndex;
        private int _wLength;

        private LZ77()
        {
            _dataAddr = Marshal.AllocHGlobal((0x1000 + 0x10000 + 0x10000) * 2);

            _Next = (ushort*) _dataAddr;
            _First = _Next + WindowLength;
            _Last = _First + 0x10000;
        }

        ~LZ77()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_dataAddr)
            {
                Marshal.FreeHGlobal(_dataAddr);
                _dataAddr = 0;
            }

            GC.SuppressFinalize(this);
        }

        public int Compress(VoidPtr srcAddr, int srcLen, Stream outStream, IProgressTracker progress, bool extFmt)
        {
            int dstLen = 4, bitCount;
            byte control;

            byte* sPtr = (byte*) srcAddr;
            int matchLength, matchOffset = 0;
            PatternLength = extFmt ? 0xFFFF + 0xFF + 0xF + 3 : 0xF + 3;

            //Initialize
            Memory.Fill(_First, 0x40000, 0xFF);
            _wIndex = _wLength = 0;

            //Write header
            CompressionHeader header = new CompressionHeader
            {
                Algorithm = CompressionType.LZ77,
                ExpandedSize = (uint) srcLen,
                IsExtendedLZ77 = extFmt
            };
            outStream.Write(&header, 4 + (header.LargeSize ? 4 : 0));

            List<byte> blockBuffer;
            int lastUpdate = srcLen;
            int remaining = srcLen;

            progress?.Begin(0, remaining, 0);

            while (remaining > 0)
            {
                blockBuffer = new List<byte> {0};
                for (bitCount = 0, control = 0; bitCount < 8 && remaining > 0; bitCount++)
                {
                    control <<= 1;
                    if ((matchLength = FindPattern(sPtr, remaining, ref matchOffset)) != 0)
                    {
                        int length;
                        if (extFmt)
                        {
                            if (matchLength >= 0xFF + 0xF + 3)
                            {
                                length = matchLength - 0xFF - 0xF - 3;
                                blockBuffer.Add((byte) (0x10 | (length >> 12)));
                                blockBuffer.Add((byte) (length >> 4));
                            }
                            else if (matchLength >= 0xF + 2)
                            {
                                length = matchLength - 0xF - 2;
                                blockBuffer.Add((byte) (length >> 4));
                            }
                            else
                            {
                                length = matchLength - 1;
                            }
                        }
                        else
                        {
                            length = matchLength - 3;
                        }

                        control |= 1;
                        blockBuffer.Add((byte) ((length << 4) | ((matchOffset - 1) >> 8)));
                        blockBuffer.Add((byte) (matchOffset - 1));
                    }
                    else
                    {
                        matchLength = 1;
                        blockBuffer.Add(*sPtr);
                    }

                    Consume(sPtr, matchLength, remaining);
                    sPtr += matchLength;
                    remaining -= matchLength;
                }

                //Left-align bits
                control <<= 8 - bitCount;

                //Write buffer
                blockBuffer[0] = control;
                outStream.Write(blockBuffer.ToArray(), 0, blockBuffer.Count);
                dstLen += blockBuffer.Count;

                if (progress != null)
                {
                    if (lastUpdate - remaining > 0x4000)
                    {
                        lastUpdate = remaining;
                        progress.Update(srcLen - remaining);
                    }
                }
            }

            outStream.Flush();

            progress?.Finish();

            return dstLen;
        }

        private ushort MakeHash(byte* ptr)
        {
            return (ushort) ((ptr[0] << 6) ^ (ptr[1] << 3) ^ ptr[2]);
        }

        private int FindPattern(byte* sPtr, int length, ref int matchOffset)
        {
            if (length < MinMatch)
            {
                return 0;
            }

            length = Math.Min(length, PatternLength);

            byte* mPtr;
            int bestLen = MinMatch - 1, bestOffset = 0, index;
            for (int offset = _First[MakeHash(sPtr)]; offset != 0xFFFF; offset = _Next[offset])
            {
                if (offset < _wIndex)
                {
                    mPtr = sPtr - _wIndex + offset;
                }
                else
                {
                    mPtr = sPtr - _wLength - _wIndex + offset;
                }

                if (sPtr - mPtr < 2)
                {
                    break;
                }

                for (index = bestLen + 1; --index >= 0 && mPtr[index] == sPtr[index];)
                {
                    ;
                }

                if (index >= 0)
                {
                    continue;
                }

                for (index = bestLen; ++index < length && mPtr[index] == sPtr[index];)
                {
                    ;
                }

                bestOffset = (int) (sPtr - mPtr);
                if ((bestLen = index) == length)
                {
                    break;
                }
            }

            if (bestLen < MinMatch)
            {
                return 0;
            }

            matchOffset = bestOffset;
            return bestLen;
        }

        private void Consume(byte* ptr, int length, int remaining)
        {
            int last, inOffset, inVal, outVal;
            for (int i = Math.Min(length, remaining - 2); i-- > 0;)
            {
                if (_wLength == WindowLength)
                {
                    //Remove node
                    outVal = MakeHash(ptr - WindowLength);
                    if ((_First[outVal] = _Next[_First[outVal]]) == 0xFFFF)
                    {
                        _Last[outVal] = 0xFFFF;
                    }

                    inOffset = _wIndex++;
                    _wIndex &= WindowMask;
                }
                else
                {
                    inOffset = _wLength++;
                }

                inVal = MakeHash(ptr++);
                if ((last = _Last[inVal]) == 0xFFFF)
                {
                    _First[inVal] = (ushort) inOffset;
                }
                else
                {
                    _Next[last] = (ushort) inOffset;
                }

                _Last[inVal] = (ushort) inOffset;
                _Next[inOffset] = 0xFFFF;
            }
        }

        public static int Compact(VoidPtr srcAddr, int srcLen, Stream outStream, ResourceNode r, bool extendedFormat)
        {
            using (LZ77 lz = new LZ77())
            {
                using (ProgressWindow prog = new ProgressWindow(r.RootNode._mainForm,
                    (extendedFormat ? "Extended " : "") + "LZ77",
                    $"Compressing {r.Name}, please wait...", false))
                {
                    return lz.Compress(srcAddr, srcLen, outStream, prog, extendedFormat);
                }
            }
        }

        public static void Expand(CompressionHeader* header, VoidPtr dstAddress, int dstLen)
        {
            Expand(header->Data, dstAddress, dstLen, header->IsExtendedLZ77);
        }

        public static void Expand(VoidPtr data, VoidPtr dstAddress, int dstLen, bool extFmt)
        {
            for (byte* srcPtr = (byte*) data, dstPtr = (byte*) dstAddress, ceiling = dstPtr + dstLen; dstPtr < ceiling;)
            {
                for (byte control = *srcPtr++, bit = 8; bit-- != 0 && dstPtr != ceiling;)
                {
                    if ((control & (1 << bit)) == 0)
                    {
                        *dstPtr++ = *srcPtr++;
                    }
                    else
                    {
                        int temp = *srcPtr >> 4,
                            num = !extFmt
                                ? temp + 3
                                : temp == 1
                                    ? (((*srcPtr++ & 0x0F) << 12) | (*srcPtr++ << 4) | (*srcPtr >> 4)) + 0xFF + 0xF + 3
                                    : temp == 0
                                        ? (((*srcPtr++ & 0x0F) << 4) | (*srcPtr >> 4)) + 0xF + 2
                                        : temp + 1,
                            offset = (((*srcPtr++ & 0xF) << 8) | *srcPtr++) + 2;
                        while (dstPtr != ceiling && num-- > 0)
                        {
                            *dstPtr++ = dstPtr[-offset];
                        }
                    }
                }
            }
        }
    }
}
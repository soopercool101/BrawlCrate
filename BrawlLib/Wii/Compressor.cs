using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Compression;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Wii
{
    [Serializable]
    public class InvalidCompressionException : Exception
    {
        public InvalidCompressionException()
        {
        }

        public InvalidCompressionException(string message) : base(message)
        {
        }

        public InvalidCompressionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidCompressionException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }

    public static unsafe class Compressor
    {
        private const int CompressBufferLen = 0x60;

        internal static CompressionType[] _supportedCompressionTypes =
        {
            CompressionType.None,
            CompressionType.LZ77,
            CompressionType.ExtendedLZ77,
            CompressionType.RunLength,
            CompressionType.RunLengthYAZ0,
            CompressionType.RunLengthYAY0
        };

        public static bool Supports(CompressionType type)
        {
            return type != CompressionType.None && _supportedCompressionTypes.Contains(type);
        }

        private const string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static CompressionType GetAlgorithm(DataSource source)
        {
            return GetAlgorithm(source.Address, source.Length);
        }

        public static CompressionType GetAlgorithm(VoidPtr addr, int length)
        {
            if (length < 4)
            {
                return CompressionType.None;
            }
            BinTag compTag = *(BinTag*) addr;
            if (compTag == YAZ0.Tag)
            {
                return CompressionType.RunLengthYAZ0;
            }

            if (compTag == YAY0.Tag)
            {
                return CompressionType.RunLengthYAY0;
            }

            CompressionHeader* cmpr = (CompressionHeader*) addr;

            if (cmpr->ExpandedSize < length)
            {
                return CompressionType.None;
            }

            if (!cmpr->HasLegitCompression())
            {
                return CompressionType.None;
            }

            //Check to make sure we're not reading a tag
            if (IsTag((byte*) addr))
            {
                return CompressionType.None;
            }

            return cmpr->Algorithm;
        }

        public static uint GetExpandedLength(VoidPtr addr, int length)
        {
            BinTag compTag = *(BinTag*) addr;
            if (compTag == YAZ0.Tag)
            {
                return ((YAZ0*) addr)->_unCompDataLen;
            }

            if (compTag == YAY0.Tag)
            {
                return ((YAY0*) addr)->_unCompDataLen;
            }

            return ((CompressionHeader*) addr)->ExpandedSize;
        }

        private static bool IsTag(byte* c)
        {
            char[] chars = _characters.ToCharArray();
            byte[] tag = {c[0], c[1], c[2], c[3]};
            if (Array.IndexOf(chars, (char) tag[0]) >= 0 &&
                Array.IndexOf(chars, (char) tag[1]) >= 0 &&
                Array.IndexOf(chars, (char) tag[2]) >= 0 &&
                Array.IndexOf(chars, (char) tag[3]) >= 0)
            {
                return true;
            }

            return false;
        }

        private static bool Test(CompressionType alg, VoidPtr src)
        {
            byte* buffer = stackalloc byte[CompressBufferLen];
            if (alg == CompressionType.RunLengthYAZ0)
            {
                Expand((YAZ0*) src, buffer, CompressBufferLen);
            }
            else if (alg == CompressionType.RunLengthYAY0)
            {
                Expand((YAY0*) src, buffer, CompressBufferLen);
            }
            else
            {
                Expand((CompressionHeader*) src, buffer, CompressBufferLen);
            }

            if (NodeFactory.GetRaw(buffer, CompressBufferLen) != null)
            {
                return true;
            }

            return false;
        }

        public static FileMap TryExpand(ref DataSource source, bool doTest = true)
        {
            FileMap decompressedMap = null;
            CompressionType algorithm = GetAlgorithm(source);
            if (Supports(source.Compression = algorithm))
            {
                try
                {
                    if (doTest && !Test(algorithm, source.Address))
                    {
                        return null;
                    }

                    uint len = 0;
                    if (algorithm == CompressionType.RunLengthYAZ0)
                    {
                        len = *(buint*) (source.Address + 4);
                        decompressedMap = FileMap.FromTempFile((int) len);
                        Expand((YAZ0*) source.Address, decompressedMap.Address, decompressedMap.Length);
                    }
                    else if (algorithm == CompressionType.RunLengthYAY0)
                    {
                        len = *(buint*) (source.Address + 4);
                        decompressedMap = FileMap.FromTempFile((int) len);
                        Expand((YAY0*) source.Address, decompressedMap.Address, decompressedMap.Length);
                    }
                    else
                    {
                        CompressionHeader* hdr = (CompressionHeader*) source.Address;
                        len = hdr->ExpandedSize;

                        decompressedMap = FileMap.FromTempFile((int) len);
                        Expand(hdr, decompressedMap.Address, decompressedMap.Length);
                    }
                }
                catch (InvalidCompressionException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            return decompressedMap;
        }

        public static void Expand(CompressionHeader* header, VoidPtr dstAddr, int dstLen)
        {
            switch (header->Algorithm)
            {
                case CompressionType.LZ77:
                case CompressionType.ExtendedLZ77:
                {
                    LZ77.Expand(header, dstAddr, dstLen);
                    break;
                }

                case CompressionType.RunLength:
                {
                    RunLength.Expand(header, dstAddr, dstLen);
                    break;
                }

                //case CompressionType.Huffman: { Huffman.Expand(header, dstAddr, dstLen); break; }
                //case CompressionType.Differential: { Differential.Expand(header, dstAddr, dstLen); break; }
                default:
                    throw new InvalidCompressionException("Unknown compression type.");
            }
        }

        public static void Expand(YAZ0* header, VoidPtr dstAddr, int dstLen)
        {
            RunLength.ExpandYAZ0(header, dstAddr, dstLen);
        }

        public static void Expand(YAY0* header, VoidPtr dstAddr, int dstLen)
        {
            RunLength.ExpandYAY0(header, dstAddr, dstLen);
        }

        public static void Compact(CompressionType type, VoidPtr srcAddr, int srcLen, Stream outStream,
                                   ResourceNode r)
        {
            switch (type)
            {
                case CompressionType.LZ77:
                {
                    LZ77.Compact(srcAddr, srcLen, outStream, r, false);
                    break;
                }

                case CompressionType.ExtendedLZ77:
                {
                    LZ77.Compact(srcAddr, srcLen, outStream, r, true);
                    break;
                }

                case CompressionType.RunLength:
                {
                    RunLength.Compact(srcAddr, srcLen, outStream, r);
                    break;
                }

                case CompressionType.RunLengthYAZ0:
                {
                    RunLength.CompactYAZ0(srcAddr, srcLen, outStream, r);
                    break;
                }

                case CompressionType.RunLengthYAY0:
                {
                    RunLength.CompactYAY0(srcAddr, srcLen, outStream, r);
                    break;
                }
            }
        }
    }
}
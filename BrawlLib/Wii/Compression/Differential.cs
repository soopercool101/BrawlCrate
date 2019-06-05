using System;
using System.IO;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Wii.Compression
{
    public unsafe class Differential
    {
        private Differential()
        {
        }

        public int Compress(VoidPtr srcAddr, int srcLen, Stream outStream, IProgressTracker progress)
        {
            return 0;
        }

        public static int Compact(VoidPtr srcAddr, int srcLen, Stream outStream, ResourceNode r, bool extendedFormat)
        {
            using (var prog = new ProgressWindow(r.RootNode._mainForm, "Differential",
                string.Format("Compressing {0}, please wait...", r.Name), false))
            {
                return new Differential().Compress(srcAddr, srcLen, outStream, prog);
            }
        }

        public static void Expand(CompressionHeader* header, VoidPtr dstAddress, int dstLen)
        {
            uint total = 0;
            var ceil = dstAddress + dstLen;

            if (header->Parameter != 1)
            {
                var pSrc = (byte*) header->Data;
                var pDst = (byte*) dstAddress;
                do
                {
                    total += *pSrc++;
                    *pDst++ = (byte) total;
                } while (pSrc < (byte*) ceil);
            }
            else
            {
                var pSrc = (bushort*) header->Data;
                var pDst = (bushort*) dstAddress;
                do
                {
                    total += *pSrc++;
                    *pDst++ = (ushort) total;
                } while ((byte*) pSrc < (byte*) ceil);
            }
        }
    }
}
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;

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
            using (ProgressWindow prog = new ProgressWindow(r.RootNode._mainForm, "Differential",
                $"Compressing {r.Name}, please wait...", false))
            {
                return new Differential().Compress(srcAddr, srcLen, outStream, prog);
            }
        }

        public static void Expand(CompressionHeader* header, VoidPtr dstAddress, int dstLen)
        {
            uint total = 0;
            VoidPtr ceil = dstAddress + dstLen;

            if (header->Parameter != 1)
            {
                byte* pSrc = (byte*) header->Data;
                byte* pDst = (byte*) dstAddress;
                do
                {
                    total += *pSrc++;
                    *pDst++ = (byte) total;
                } while (pSrc < (byte*) ceil);
            }
            else
            {
                bushort* pSrc = (bushort*) header->Data;
                bushort* pDst = (bushort*) dstAddress;
                do
                {
                    total += *pSrc++;
                    *pDst++ = (ushort) total;
                } while ((byte*) pSrc < (byte*) ceil);
            }
        }
    }
}
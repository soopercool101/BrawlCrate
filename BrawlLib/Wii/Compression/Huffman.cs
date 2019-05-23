using System;
using System.IO;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Wii.Compression
{
    public unsafe class Huffman
    {
        private Huffman()
        {

        }

        public int Compress(VoidPtr srcAddr, int srcLen, Stream outStream, IProgressTracker progress)
        {
            return 0;
        }
        
        public static int Compact(VoidPtr srcAddr, int srcLen, Stream outStream, ResourceNode r)
        {
            using (ProgressWindow prog = new ProgressWindow(r.RootNode._mainForm, "Huffman", String.Format("Compressing {0}, please wait...", r.Name), false))
                return new Huffman().Compress(srcAddr, srcLen, outStream, prog);
        }
        public static void Expand(CompressionHeader* header, VoidPtr dstAddress, int dstLen)
        {
            
        }
    }
}

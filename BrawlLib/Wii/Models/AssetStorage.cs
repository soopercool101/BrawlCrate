using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;

namespace BrawlLib.Wii.Models
{
    public unsafe class AssetStorage : IDisposable
    {
        public UnsafeBuffer[][] Assets = new UnsafeBuffer[4][];

        public AssetStorage(ModelLinker linker)
        {
            int index;

            //Vertices
            if (linker.Vertices != null)
            {
                Assets[0] = new UnsafeBuffer[linker.Vertices->_numEntries];
                index = 0;
                foreach (ResourcePair p in *linker.Vertices)
                {
                    Assets[0][index++] = VertexCodec.Decode((MDL0VertexData*) p.Data);
                }
            }

            //Normals
            if (linker.Normals != null)
            {
                Assets[1] = new UnsafeBuffer[linker.Normals->_numEntries];
                index = 0;
                foreach (ResourcePair p in *linker.Normals)
                {
                    Assets[1][index++] = VertexCodec.Decode((MDL0NormalData*) p.Data);
                }
            }

            //Colors
            if (linker.Colors != null)
            {
                Assets[2] = new UnsafeBuffer[linker.Colors->_numEntries];
                index = 0;
                foreach (ResourcePair p in *linker.Colors)
                {
                    Assets[2][index++] = ColorCodec.Decode((MDL0ColorData*) p.Data);
                }
            }

            //UVs
            if (linker.UVs != null)
            {
                Assets[3] = new UnsafeBuffer[linker.UVs->_numEntries];
                index = 0;
                foreach (ResourcePair p in *linker.UVs)
                {
                    Assets[3][index++] = VertexCodec.Decode((MDL0UVData*) p.Data);
                }
            }
        }

        ~AssetStorage()
        {
            Dispose();
        }

        public void Dispose()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Assets[i] != null)
                {
                    foreach (UnsafeBuffer b in Assets[i])
                    {
                        b?.Dispose();
                    }

                    Assets[i] = null;
                }
            }
        }
    }
}
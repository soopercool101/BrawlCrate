using BrawlLib.Internal;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.Modeling.Triangle_Converter
{
    public class TriangleConverter
    {
        public bool _useStrips;
        public uint _cacheSize;
        public uint _minStripLen;
        public bool _backwardSearch;
        public bool _pushCacheHits;

        public TriangleConverter(bool useStrips, uint cacheSize, uint minStripLen, bool pushCacheHits)
        {
            _useStrips = useStrips;
            _cacheSize = cacheSize;
            _minStripLen = minStripLen;
            _backwardSearch = false;
            _pushCacheHits = pushCacheHits;
        }

        public List<PrimitiveGroup> GroupPrimitives(Facepoint[] points, out int pointCount, out int faceCount)
        {
            pointCount = 0;
            faceCount = 0;
            List<PrimitiveClass> fpPrimitives = new List<PrimitiveClass>();
            if (_useStrips)
            {
                //Remap points so there is only one id per individual point
                Remapper remapData = new Remapper();
                remapData.Remap(points, null);

                //Set up tristripper with remapped point ids
                TriStripper stripper = new TriStripper(
                    remapData._remapTable.Select(x => (uint) x).ToArray(),
                    points.Select(x => x.NodeID).ToArray(),
                    remapData._impTable);
                stripper.SetCacheSize(_cacheSize);
                stripper.SetMinStripSize(_minStripLen);
                stripper.SetPushCacheHits(_pushCacheHits);

                //Backward search doesn't work,
                //but generally won't improve the optimization with the cache on anyway
                //stripper.SetBackwardSearch(_backwardSearch);

                //Create strips using ids
                List<Primitive> primArray = stripper.Strip();

                //Recollect facepoints with new indices and get point/face count
                for (int i = 0; i < primArray.Count; i++)
                {
                    Primitive p = primArray[i];
                    if (p.Type == PrimType.TriangleList)
                    {
                        int count = p.Indices.Count / 3;
                        faceCount += count;
                        for (int r = 0; r < count; r++)
                        {
                            fpPrimitives.Add(new PointTriangle(
                                points[remapData._impTable[p.Indices[r * 3 + 0]]],
                                points[remapData._impTable[p.Indices[r * 3 + 1]]],
                                points[remapData._impTable[p.Indices[r * 3 + 2]]]));
                        }
                    }
                    else
                    {
                        faceCount += p.Indices.Count - 2;
                        fpPrimitives.Add(new PointTriangleStrip
                            {_points = p.Indices.Select(x => points[remapData._impTable[x]]).ToList()});
                    }

                    pointCount += p.Indices.Count;
                }
            }
            else
            {
                faceCount = (pointCount = points.Length) / 3;
                for (int r = 0; r < faceCount; r++)
                {
                    fpPrimitives.Add(new PointTriangle(
                        points[r * 3 + 0],
                        points[r * 3 + 1],
                        points[r * 3 + 2]));
                }
            }

            //Group primitives by each point's influence id
            fpPrimitives.Sort(PrimitiveClass.Compare);
            List<PrimitiveGroup> groups = new List<PrimitiveGroup>();
            foreach (PrimitiveClass p in fpPrimitives)
            {
                bool added = false;
                foreach (PrimitiveGroup g in groups)
                {
                    if (added = g.TryAdd(p))
                    {
                        break;
                    }
                }

                if (!added)
                {
                    PrimitiveGroup g = new PrimitiveGroup();
                    g.TryAdd(p);
                    groups.Add(g);
                }
            }

            return groups;
        }
    }
}
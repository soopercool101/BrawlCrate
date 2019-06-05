using System.Collections;
using System.Collections.Generic;

namespace System
{
    public class Remapper
    {
        internal int[] _impTable;
        internal int[] _remapTable;
        internal object _source;

        public int[] ImplementationTable => _impTable;
        public int[] RemapTable => _remapTable;
        public int ImplementationLength => _impTable.Length;

        public void Remap<T>(IList<T> source, Comparison<T> comp)
        {
            _source = source;
            var count = source.Count;
            int tmp;
            var cache = new Hashtable();

            _remapTable = new int[count];
            _impTable = new int[count];

            //Build remap table by assigning first appearance
            var impIndex = 0;
            for (var i = 0; i < count; i++)
            {
                var t = source[i];
                if (cache.ContainsKey(t))
                {
                    _remapTable[i] = (int) cache[t];
                }
                else
                {
                    _impTable[impIndex] = i;
                    cache[t] = _remapTable[i] = impIndex++;
                }
            }

            var impCount = impIndex;

            if (comp != null)
            {
                //Create new remap table, which is a sorted index list into the imp table
                var sorted = new int[impCount];
                impIndex = 0;
                for (var i = 0; i < impCount; i++)
                {
                    //Get implementation index/object
                    var ind = _impTable[i];
                    var t = source[ind];

                    sorted[impIndex] = i; //Set last, just in case we don't find a match

                    //Iterate entries in sorted list, comparing them
                    for (var y = 0; y < impIndex; y++)
                    {
                        tmp = sorted[y]; //Pull old value, will use it later
                        if (comp(t, source[_impTable[tmp]]) < 0)
                        {
                            sorted[y] = i;

                            //Rotate right
                            for (var z = y; z++ < impIndex;)
                            {
                                ind = sorted[z];
                                sorted[z] = tmp;
                                tmp = ind;
                            }

                            break;
                        }
                    }

                    impIndex++;
                }

                //Swap sorted list, creating a new remap table in the process
                for (var i = 0; i < impCount; i++)
                {
                    tmp = sorted[i]; //Get index
                    sorted[i] = _impTable[tmp]; //Set sorted entry to imp index
                    _impTable[tmp] = i; //Set imp entry to remap index
                }

                //Re-index remap
                for (var i = 0; i < count; i++) _remapTable[i] = _impTable[_remapTable[i]];

                //Swap tables
                _impTable = sorted;
            }
            else
            {
                Array.Resize(ref _impTable, impCount);
            }
        }
    }
}
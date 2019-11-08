using System;
using System.Collections;
using System.Collections.Generic;

namespace BrawlLib.Internal
{
    public class Remapper
    {
        internal object _source;
        internal int[] _impTable;
        internal int[] _remapTable;

        public int[] ImplementationTable => _impTable;
        public int[] RemapTable => _remapTable;
        public int ImplementationLength => _impTable.Length;

        public void Remap<T>(IList<T> source, Comparison<T> comp)
        {
            _source = source;
            int count = source.Count;
            int tmp;
            Hashtable cache = new Hashtable();

            _remapTable = new int[count];
            _impTable = new int[count];

            //Build remap table by assigning first appearance
            int impIndex = 0;
            for (int i = 0; i < count; i++)
            {
                T t = source[i];
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

            int impCount = impIndex;

            if (comp != null)
            {
                //Create new remap table, which is a sorted index list into the imp table
                int[] sorted = new int[impCount];
                impIndex = 0;
                for (int i = 0; i < impCount; i++)
                {
                    //Get implementation index/object
                    int ind = _impTable[i];
                    T t = source[ind];

                    sorted[impIndex] = i; //Set last, just in case we don't find a match

                    //Iterate entries in sorted list, comparing them
                    for (int y = 0; y < impIndex; y++)
                    {
                        tmp = sorted[y]; //Pull old value, will use it later
                        if (comp(t, source[_impTable[tmp]]) < 0)
                        {
                            sorted[y] = i;

                            //Rotate right
                            for (int z = y; z++ < impIndex;)
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
                for (int i = 0; i < impCount; i++)
                {
                    tmp = sorted[i];            //Get index
                    sorted[i] = _impTable[tmp]; //Set sorted entry to imp index
                    _impTable[tmp] = i;         //Set imp entry to remap index
                }

                //Re-index remap
                for (int i = 0; i < count; i++)
                {
                    _remapTable[i] = _impTable[_remapTable[i]];
                }

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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BrawlLib.Internal
{
    public static class ListExtension
    {
        public static int[] FindAllOccurences(this IList a, object o)
        {
            List<int> l = new List<int>();
            int i = 0;
            foreach (object x in a)
            {
                if (x == o)
                {
                    l.Add(i);
                }

                i++;
            }

            return l.ToArray();
        }

        //public static List<BrawlLib.Wii.Audio.RSARConverter.TEntry> ShiftFirst(this IList list, int index)
        //{
        //    List<BrawlLib.Wii.Audio.RSARConverter.TEntry> newList = new List<BrawlLib.Wii.Audio.RSARConverter.TEntry>();
        //    for (int i = index; i < list.Count + index; i++)
        //    {
        //        int x = i;
        //        if (i >= list.Count)
        //            x = i - list.Count;
        //        newList.Add((BrawlLib.Wii.Audio.RSARConverter.TEntry)list[x]);
        //    }
        //    return newList;
        //}
        public static int IndexOf(this byte[] searchList, byte[] pattern)
        {
            Encoding encoding = Encoding.GetEncoding(1252);
            string s1 = encoding.GetString(searchList, 0, searchList.Length);
            string s2 = encoding.GetString(pattern, 0, pattern.Length);
            int result = s1.IndexOf(s2, StringComparison.Ordinal);
            return result;
        }
    }
}
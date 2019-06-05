using System.Collections.Generic;

namespace System
{
    public static class ArrayExtension
    {
        public static int[] FindAllOccurences(this Array a, object o)
        {
            var l = new List<int>();
            var i = 0;
            foreach (var x in a)
            {
                if (x == o) l.Add(i);

                i++;
            }

            return l.ToArray();
        }

        public static int[] Append(this Array a, int[] array)
        {
            var values = new List<int>();
            foreach (int i in a) values.Add(i);

            foreach (var i in array) values.Add(i);

            return values.ToArray();
        }

        public static int IndexOf(this Array a, object value)
        {
            return Array.IndexOf(a, value);
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static T[] Append<T>(this T[] data, T[] appended)
        {
            var final = new T[data.Length + appended.Length];
            data.CopyTo(final, 0);
            appended.CopyTo(final, data.Length);
            return final;
        }
    }
}
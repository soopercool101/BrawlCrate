using System.Collections.Generic;

namespace System
{
    public static class Utils
    {
        public static void CopyList<T>(ref List<T> _original, ref List<T> _toOverwrite, bool clearToOverwrite = false) where T : ICloneable
        {
            if (clearToOverwrite)
                _toOverwrite.Clear();

            foreach (T l in _original)
            {
                _toOverwrite.Add((T)l.Clone());
            }
        }
        public static List<T> CopyList2<T>(ref List<T> _original) where T : ICloneable
        {
            List<T> newValues = new List<T>();

            foreach (T l in _original)
            {
                newValues.Add((T)l.Clone());
            }

            return newValues;
        }
    }
}

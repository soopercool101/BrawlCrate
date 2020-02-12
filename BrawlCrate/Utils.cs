using OpenTK.Graphics;
using System.Drawing;

namespace System
{
    public static class Utils
    {
		//public static Rectangle Add(this Rectangle rect, int x, int y, int w, int h)
		//{
		//	return new Rectangle(rect.X + x, rect.Y + y, rect.Width + w, rect.Height + h);
		//}
		/// <summary> Returns a point, but with added X and Y values. </summary>
		/// <param name="point"> The main point to use from. </param>
		/// <param name="x"> Added X, as an offset. </param>
		/// <param name="y"> Added Y, as an offset. </param>
		/// <returns> A new point with added values from the original Point. </returns>
		public static Point Add(this Point point, int x, int y)
		{
			return new Point(point.X + x, point.Y + y);
		}
		public static Windows.Forms.VisualStyles.CheckBoxState TranslateCheckState(Windows.Forms.CheckState State)
		{
			switch (State)
			{
				case Windows.Forms.CheckState.Checked: return Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
				case Windows.Forms.CheckState.Indeterminate: return Windows.Forms.VisualStyles.CheckBoxState.MixedNormal;
				default:
				case Windows.Forms.CheckState.Unchecked: return Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
			}
		}
		public static Color4 convertTo4(this Color toReplace)
		{
			float R = toReplace.R / 255.0f;
			float G = toReplace.G / 255.0f;
			float B = toReplace.B / 255.0f;
			float A = toReplace.A / 255.0f;

			return new Color4(R, G, B, A);
		}

        //public static void CopyList<T>(ref List<T> _original, ref List<T> _toOverwrite, bool clearToOverwrite = false) where T : ICloneable
        //{
        //    if (clearToOverwrite)
        //        _toOverwrite.Clear();

        //    foreach (T l in _original)
        //    {
        //        _toOverwrite.Add((T)l.Clone());
        //    }
        //}
        //public static List<T> CopyList2<T>(ref List<T> _original) where T : ICloneable
        //{
        //    List<T> newValues = new List<T>();

        //    foreach (T l in _original)
        //    {
        //        newValues.Add((T)l.Clone());
        //    }

        //    return newValues;
        //}
    }
}

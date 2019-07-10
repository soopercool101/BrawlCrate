using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BrawlCrate.StageManager
{
    public class NameCreatorSettings
    {
        public Font Font;
        public int VerticalOffset;
        public bool Corner;

        public override string ToString()
        {
            return string.Format("{0}pt {1} {2} ({3})", Font.SizeInPoints, Font.Name, Font.Style,
                Corner ? "Corner" : VerticalOffset.ToString());
        }
    }
}
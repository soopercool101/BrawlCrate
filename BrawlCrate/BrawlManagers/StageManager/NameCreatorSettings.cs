using System.Drawing;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public class NameCreatorSettings
    {
        public Font Font;
        public int VerticalOffset;
        public bool Corner;

        public override string ToString()
        {
            return
                $"{Font.SizeInPoints}pt {Font.Name} {Font.Style} ({(Corner ? "Corner" : VerticalOffset.ToString())})";
        }
    }
}
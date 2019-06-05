namespace BrawlLib.SSBB
{
    public interface IBoolArraySource
    {
        bool Constant { get; set; }
        bool Enabled { get; set; }
        int EntryCount { get; set; }
        void SetEntry(int index, bool value);
        bool GetEntry(int index);
        void MakeAnimated();
        void MakeConstant(bool value);
    }
}
namespace BrawlLib.Imaging
{
    public interface IColorSource
    {
        bool HasPrimary(int id);
        ARGBPixel GetPrimaryColor(int id);
        void SetPrimaryColor(int id, ARGBPixel color);
        string PrimaryColorName(int id);

        int TypeCount { get; }
        int ColorCount(int id);
        ARGBPixel GetColor(int index, int id);
        void SetColor(int index, int id, ARGBPixel color);

        bool GetColorConstant(int id);
        void SetColorConstant(int id, bool constant);
    }
}
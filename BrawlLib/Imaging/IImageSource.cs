using System.Audio;
using System.Drawing;

namespace BrawlLib.Imaging
{
    public interface IImageSource
    {
        int ImageCount { get; }
        Bitmap GetImage(int index);
    }

    public interface IVideo : IImageSource
    {
        uint NumFrames { get; }
        float FrameRate { get; }
        int GetImageIndexAtFrame(int frame);

        IAudioStream Audio { get; }
        uint Frequency { get; }
    }
}
